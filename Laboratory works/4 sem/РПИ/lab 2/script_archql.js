function next_random_int(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

var is_capital = true;
function make_str(length) {
    var result           = '';
    var capital_chars = 'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEFFFFFFFFFFFFFFFFFFFFFFFFFGGGGGGGGGGGGGGGGGHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIJJJJKKKKKKKKLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOPPPPPPPPPPPPPPPPQQQQQRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUVVVVVVVVVVVVWWWWWWWWWWWWWWWWWWWWXXXXYYYYYYYYYYYYYYYYYYYYZZ';
    if (is_capital) {
        result += capital_chars.charAt(Math.floor(Math.random() * capital_chars.length));
        is_capital = false;
    }

    var characters       = 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbbbccccccccccccccccccccccccccccccddddddddddddddddddddddddddddddddddddddddddddeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeefffffffffffffffffffffffffggggggggggggggggghhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiijjjjkkkkkkkkllllllllllllllllllllllllllllllllllllllllmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooppppppppppppppppqqqqqrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrsssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssstttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuvvvvvvvvvvvvwwwwwwwwwwwwwwwwwwwwxxxxyyyyyyyyyyyyyyyyyyyyzz';
    var charactersLength = characters.length;
    for ( var i = 0; i < length; i++ ) {
        result += characters.charAt(Math.floor(Math.random() *
            charactersLength));
    }
    
    if (next_random_int(0, 15) < 2) {
        result += '.';
        is_capital = true;
    }
    
    return result;
}

function create_window(index, parent)
{
    let name = parent.getElementsByTagName("h3").item(0).innerText;
    let text = parent.getElementsByTagName("span").item(0).innerText;
    
    // window
    let div_window = document.createElement("div");
    div_window.className = "aq_modal";
    div_window.id = "aq_modal_id_" + index.toString();

    // content
    let div_content = document.createElement("div");
    div_content.className = "aq_modal_content";

    // close baton setup
    let span_close = createSpanText('aq_close', '×');

    // content
    let p_header = document.createElement("h4");
    p_header.textContent = name;
    
    let p_text = document.createElement("div");
    //for (i = 0; i < Math.floor(Math.random() * 5) + 5; i++)
    //    p_text.textContent += text + " ";
    is_capital = true;
    for (i = 0; i < next_random_int(80, 180); i++)
        p_text.textContent += make_str(next_random_int(3, 8)) + " ";

    div_content.append(span_close, p_header, p_text);
    div_window.append(div_content);
    
    document.body.append(div_window);
    
    // -- setup listeners --
    // close on area click
    div_window.onclick = function(event) {
        if (event.target === div_window) {
            closeWindow(div_window);
        }
    }
    // close on cross click
    span_close.addEventListener("click", () => {close_window(div_window); });
    
    return div_window;
}

function show_window(index)
{
    let window = document.getElementById("aq_modal_id_" + index.toString());
    window.style.display = "block";
	
	document.body.style.overflow = 'hidden';
}
function close_window(window)
{
    window.style.display = "none";
	
	document.body.style.overflow = '';
}

console.log("Init!!!");

var btns = document.getElementsByClassName("aq_table_button");
for (let i = 0; i < btns.length; i++) {

    let window = create_window(i, btns[i].parentElement);

    btns[i].addEventListener("click", () => {
        show_window(i);
    });
};

