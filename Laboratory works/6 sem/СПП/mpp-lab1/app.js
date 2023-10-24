"use strict";

const express = require('express');
const bodyParser = require('body-parser');
const db = require('./src/database.js');

const app = express();


app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: true
}));

app.use(express.static('public'));
app.set('view engine', 'ejs');

app.post('/editNote/:id', function (req, res) {
    const id = req.params.id;
    const name = req.body.noteName;
    const content = req.body.noteContent;
    // const data =
    return res.redirect('/redactor/:' + req.params.id);
});

app.post('/duplicateNote/:id', async function (req, res) {
    let data = db.getNotesInfo();
    let info = data.get(req.params.id);
    let content = await db.getNoteContent(info);
    await db.tryCreateNote(info.get("name"), content);
    return res.redirect('/');
});
app.post('/deleteNote/:id', async function (req, res) {
    const data = db.getNotesInfo();
    const info = data.get(`${req.params.id}`);
    console.log("data is: " + Array.from(data.entries()));
    console.log("id is :" + req.params.id);
    console.log("noteinfo is: " + info);

    if (info !== 'undefined')
    {
        await db.deleteNote(info);
        console.log("deleted " + req.params.id);
    }
    else
        console.log("undefined while deleting!");

    return res.redirect('/');
});

app.post('/newNote', async function (req, res) {
    const name = req.body.noteName;
    await db.tryCreateNote(name, "");
    return res.redirect('/');
});

app.use('/redactor/:id', function (request, response) {
    const data = db.getNotesInfo();
    const item = data.get(request.params.id);
    const content = db.getNoteContent(request.params.id);
    response.render('redactor', {
        title: "Edit note",
        id: request.params.id,
        nameNote: item.get("name"),
        content: content,
    });
});

app.use('/', function (request, response) {
    let params = {};
    params.title = "My notes";
    const data = db.getNotesInfo();
    // console.log(data);
    params.count = data.size;

    params.notes = Array.from(Array.from(data.values()), i => {
        return JSON.stringify(Object.fromEntries(i))
    });



    response.render('notes', params);
});


app.listen(3000);