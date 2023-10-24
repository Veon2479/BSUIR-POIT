const fs = require('fs');
const mx = require('async-mutex');

const indexPath = "./data/index.txt";

let notesInfo = new Map;
let infoMutex = new mx.Mutex;

let notesBlocks = new Set;
let blocksMutex = new mx.Mutex;


async function tryCreateNote(noteName, noteContent) {
    let id = Date.now();
    let path = `./data/${id}.txt`;
    let preview = getPreview(noteContent);
    fs.writeFileSync(path, noteContent, {encoding: "utf16le",});
    let item = new Map([
        ["id", `${id}`],
        ["name", noteName],
        ["path", path],
        ["preview", preview],
    ]);
    await infoMutex.runExclusive(() => {
       notesInfo.set(`${id}`, item);
        //note id(timestamp),note name,filename, preview (until first \n or first 50 symbols
        fs.appendFileSync(indexPath, `${id},${noteName},${path},${preview}\n`, {encoding: "utf16le"});
    });
}

async function updateNote(noteInfo, newName, newContent) {
    let id = noteInfo["id"];

    let flag = true;
    while (flag) {
        await blocksMutex.runExclusive(() => {
            if (!notesBlocks.has(id)) {
                notesBlocks.add(id);
                flag = false;
            }
        });
    }

    fs.writeFileSync(noteInfo["path"], newContent, {encoding: "utf-8", });

    let promise = infoMutex.runExclusive(() => {
        noteInfo["name"] = newName;
        noteInfo["preview"] = getPreview(newContent);
    });

    await blocksMutex.runExclusive(() => {
        notesBlocks.delete(id);
    });

    await promise;
}

async function deleteNote(noteInfo) {
    let id = noteInfo.get("id");

    await infoMutex.runExclusive(async () => {
        notesInfo.delete(id);
        fs.unlinkSync(indexPath);
        let f = fs.openSync(indexPath, "a+");
        fs.closeSync(f);
        notesInfo.forEach(function (item) {
            fs.appendFileSync(indexPath, `${item.get("id")},${item.get("name")},${item.get("path")},${item.get("preview")}\n`, {encoding: "utf16le"});
        });
    });

    let flag = true;
    while (flag) {
        await blocksMutex.runExclusive(() => {
            if (!notesBlocks.has(id)) {
                notesBlocks.add(id);
                flag = false;
            }
        });
    }

    fs.unlinkSync(noteInfo.get("path"));

    await blocksMutex.runExclusive(() => {
        notesBlocks.delete(id);
    });
}

function getNotesInfo() {
    // let result = new Map();
    //
    // notesInfo.forEach(info => {
    //     result.set(info["id"], new Map([
    //         ["name", info["name"]],
    //         ["preview", info["preview"]],
    //     ]));
    // });
    //
    // return result;
    return notesInfo;
}

async function getNoteContent(noteInfo) {
    let id = noteInfo.get("id");
    let flag = true;
    while (flag)
    {
        await blocksMutex.runExclusive(() => {
            if (!notesBlocks.has(id))
            {
                notesBlocks.add(id);
                flag = false;
            }
        });
    }

    let content = fs.readFileSync(noteInfo.get("path"), {encoding: 'utf-8'});

    await blocksMutex.runExclusive(() => {
        notesBlocks.delete(id);
    });

    return content;
}
async function loadNotesInfo() {

    await infoMutex.runExclusive(() => {
        let f = fs.openSync(indexPath, "a+");
        //this file stores infos in separate lines about files
        //note id(timestamp),note name,filename, preview (until first \n or first 50 symbols
        const data = fs.readFileSync(f, {encoding: 'utf16le'});
        fs.closeSync(f);

        const infos = data.split("\n");
        infos.forEach(info => {
            if (info.length > 0) {
                const fields = info.split(',');
                notesInfo.set(fields[0], new Map([
                    ["id", fields[0]],
                    ["name", fields[1]],
                    ["path", fields[2]],
                    ["preview", fields[3]],
                ]));
            }
        });
    });
}

function getPreview(content)
{
    // const content = String(`${data_content}`);
    let result = String("");
    let flag = true;
    let i = 0;

    while (flag)
    {
        if ( i < content.length &&
            (content.charAt[i] !== '\n' || content.charAt[i] !== '\t' || content.charAt[i] !== ' ' ))
        {
            result += content[i];
        }
        i++;
        if (i >= content.length)
            flag = false;
        if (result.length >= 50)
            flag = false;
    }
    if (result === "")
        result = " ";
    return result;
}

(async() => {
    console.log("Loading notes data..");
    await loadNotesInfo();
    console.log("Notes are loaded");
})();
// (async() => {
//     await tryCreateNote("New Test Note", "Hello there!");
//     console.log("note created")
// })();
module.exports.tryCreateNote = tryCreateNote;
module.exports.updateNote = updateNote;
module.exports.deleteNote = deleteNote;
module.exports.getNotesInfo = getNotesInfo;
module.exports.getNoteContent = getNoteContent;
