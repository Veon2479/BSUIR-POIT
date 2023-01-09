//create text in span tag
function createSpanText(className, text) {
    let spanText = document.createElement("span");
    spanText.id = className;
    if (className !== '') {
        spanText.className = className;
    }
    spanText.appendChild(document.createTextNode(text));
    return spanText;
}

//create input box
function createInputBox(className, type, id, value, required, text) {
    let divBox = document.createElement("div");
    divBox.className = className;

    let inputBox = document.createElement("input");
    inputBox.id = id;
    inputBox.type = type;
    inputBox.value = value;
    inputBox.className = type;
    if (required !== '') {
        inputBox.required = required;
    }

    divBox.appendChild(inputBox);

    if (text !== ''){
        let spanText = createSpanText('', text);
        divBox.appendChild(spanText);
    }

    return divBox;
}

//create input modal
function createWindow() {
    let winDiv = document.createElement("div");
    winDiv.id = "orderModal";
    winDiv.className = "modal";

    let divContent = document.createElement("div");
    divContent.className = "modal-content";

    let spanClose = createSpanText('close', '×');

    let pReg = document.createElement("p");
    pReg.appendChild(document.createTextNode("Registration"));

    let divName = createInputBox('namebox', 'text', 'namebox', '', 'required', "Name");
    let divEmail = createInputBox('emailbox', 'text', 'emailbox', '', 'required', "Email");
    let divPhone = createInputBox('phonebox', 'text', 'phonebox', '', 'required', "Phone");
    let divSend = createInputBox('sendbox', 'submit', 'modal-send', 'send', '', '');

    divContent.append(spanClose, pReg, divName, divEmail, divPhone, divSend);
    winDiv.append(divContent);
    document.body.append(winDiv);
    console.log(winDiv);
    return winDiv;
}

//create success modal
function createSuccess() {

    let winSuc = document.createElement("div");
    winSuc.id = 'orderSuc';
    winSuc.className = 'modal';

    let divContent = document.createElement("div");
    divContent.className = 'modal-content';

    let spanText = createSpanText('', 'Successfully sent');

    divContent.appendChild(spanText);
    winSuc.appendChild(divContent);
    document.body.append(winSuc);
    console.log(winSuc);
    return winSuc;
}

//create done modal
function createDone() {
    let winDone = document.createElement("div");
    winDone.id = 'orderDone';
    winDone.className = 'modal';

    let divContent = document.createElement("div");
    divContent.className = 'modal-content';

    let spanText = createSpanText('', 'The order has already been made');

    divContent.appendChild(spanText);
    winDone.appendChild(divContent);
    document.body.append(winDone);
    console.log(winDone);
    return winDone;
}

//close modal window
function closeWindow(modal){
    modal.style.display = "none";
    document.body.style.overflow = '';
}

//show modal window
function showWindow(win) {
    let ordered = sessionStorage.getItem('ordered');
    if (win === modal && ordered === 'true') {
        showWindow(done);
        let timerID = setTimeout(closeWindow, 5000, done);
    }
    else
        win.style.display = "block";
    document.body.style.overflow = 'hidden';
}

//check entered name
function validateName(name) {
    var reg = /(\s*[а-яА-ЯёЁa-zA-Z])+$/;

    if (reg.test(name) === false) {
        alert('Enter correct name');
        return false;
    }
    else
        return true;
}

//check entered email
function validateEmail(email){
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if(reg.test(email) === false) {
        alert('Enter correct e-mail');
        return false;
    }
    else
        return true;
}

//check entered phone
function validatePhone(phone) {
    var reg = /^(\+)?((\d{2,3}) ?\d|\d)(([ -]?\d)|( ?(\d{2,3}) ?)){5,12}\d$/;

    if(reg.test(phone) === false) {
        alert('Enter correct phone');
        return false;
    }
    else
        return true;
}

//send action
function sendWindow(success) {
    //validation
    let name = document.getElementById("namebox").value;
    let email = document.getElementById("emailbox").value;
    let phone = document.getElementById("phonebox").value;

    if (validateName(name) && validateEmail(email) && validatePhone(phone)) {
        closeWindow(modal);
        showWindow(success);
        sessionStorage.setItem('ordered', true);
        let timerID = setTimeout(closeWindow, 5000, success);
    }
}

//creating all possible modal windows
createWindow();
createSuccess();
createDone();

var modal = document.getElementById("orderModal");
var success = document.getElementById("orderSuc");
var done = document.getElementById("orderDone");
if (!sessionStorage.getItem('ordered'))
    sessionStorage.setItem('ordered', false);

//show input modal window
var btn = document.getElementsByClassName("btnOrder");
for (let i = 0; i < btn.length; i++)
    btn[i].addEventListener("click", () => { showWindow(modal); });

//close input modal window
var span = document.getElementById("close");
console.log(span);
span.addEventListener("click", () => {closeWindow(modal); });


///show success modal window
var send = document.getElementById("modal-send");
send.addEventListener("click", () => {sendWindow(success); });

//close input modal window on area click
window.onclick = function(event) {
    if (event.target === modal) {
       closeWindow(modal);
    }
}

window.onclose = () => {
    sessionStorage.clear();
}