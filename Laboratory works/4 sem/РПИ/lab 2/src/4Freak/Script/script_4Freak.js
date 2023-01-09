const inputForm_RegName = /^[A-Za-zА-Яа-я-]{1,40}$/;
const inputForm_RegEmail = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
const inputForm_RegMessage = /^(\w|\s|\,|\.|\?|[А-Яа-я]){0,200}$/;

let inputForm_Inputs = document.querySelectorAll('input[data-InputFormRule]');
let inputForm_Textareas = document.querySelectorAll('textarea[data-InputFormRule]');
let inputForm_BtnSubmit = document.querySelector('.inputForm_BtnSubmit');
let inputForm_ContactContentBlocker = document.querySelector('.ContactContentBlocker');

inputForm_BtnSubmit.setAttribute('disabled', true);
inputForm_BtnSubmit.addEventListener('click', inputForm_SendMessage)


for (let inputForm_Input of inputForm_Inputs){
	inputForm_Input.addEventListener('input', InputForm_CheckAllInput);
}
for (let inputForm_Textarea of inputForm_Textareas){
	inputForm_Textarea.addEventListener('input', InputForm_CheckAllInput)
}

function InputForm_CheckAllInput()
{
	console.log('change');
	let inputForm_IsCorrect = true;
	for(let inputForm_Input of inputForm_Inputs){
		if (inputForm_IsCorrect === true)
			inputForm_IsCorrect = InputForm_CheckContent(inputForm_Input)
	}
	for(let inputForm_Textarea of inputForm_Textareas){
		if (inputForm_IsCorrect === true)
			inputForm_IsCorrect = InputForm_CheckContent(inputForm_Textarea)
	}

	if (inputForm_IsCorrect === true){
		inputForm_BtnSubmit.removeAttribute('disabled');
	}else{
		inputForm_BtnSubmit.setAttribute('disabled', true);
	}
}


function InputForm_CheckContent (InputForm_Obg)
{
	let inputForm_IsCorrect = false;
	let inputForm_Rule = InputForm_Obg.dataset.inputformrule;
	let inputForm_InputValue = InputForm_Obg.value;

	switch(inputForm_Rule){
		case "name":
			inputForm_IsCorrect = inputForm_RegName.test(inputForm_InputValue);
			break;
		case "email":
			inputForm_IsCorrect = inputForm_RegEmail.test(inputForm_InputValue);
			break;	
		case "message":
			inputForm_IsCorrect = inputForm_RegMessage.test(inputForm_InputValue);
			break;
	}
	if (inputForm_InputValue === ""){
		inputForm_IsCorrect = false;
	}
	
	return inputForm_IsCorrect;
}

function inputForm_SendMessage()
{
	function inputFrom_RemoveStyle(){
		inputForm_ContactContentBlocker.classList.remove("_active");
		document.body.style.overflow = "";
	}
	inputForm_ContactContentBlocker.classList.add("_active");
	document.body.style.overflow = "hidden";
	console.log("Send message");
	window.setTimeout(inputFrom_RemoveStyle, 5000);
}