
const deleteElement = (guid, action) => {
    try {
        fetch(`${document.location.origin}/cars/edit/delete?g=${guid}&act=${action}`, {
            method: "DELETE"
        }).then((response) => {
            const status = response.status;
            if (status === 200) {
                appentErrorText("������� ������!");
                document.location.reload();
            }
            else appentErrorText("��������� ������ ��� ��������!");
        }).catch(e => { console.error(e); appentErrorText("��������� ������ ��� ��������!"); })
    } catch (e) {
        appentErrorText("��������� ������ ��� ��������!");
    }
}

const changeElement = (guid, action, value, field) => {
    if (value.length <= 3) {
        appentErrorText("�������� �� ����� ���� ����� ���� ��������!");
    }

    try {
        fetch(`${document.location.origin}/cars/edit/value?g=${guid}&act=${action}&v=${value}&fl=${field}`, {
            method: "PUT"
        }).then((response) => {
            const status = response.status;
            if (status === 200) {
                appentErrorText("������� �������!");
                document.location.reload();
            }
            else appentErrorText("��������� ������ ��� ���������!");
        }).catch(e => { console.error(e); appentErrorText("��������� ������ ��� ���������!"); })
    } catch (e) {
        appentErrorText("��������� ������ ��� ���������!");
    }
}

const setStateEditor = (elementIdEnb, elementIdDsb) => {
    const elementEnable = document.getElementById(elementIdEnb),
        elementDisable = document.getElementById(elementIdDsb),
        editButton = document.getElementById(elementIdEnb + "_btn");

    if (elementEnable !== undefined && elementDisable !== undefined && editButton !== undefined) {
        elementEnable.style.display = elementEnable.style.display === "flex" ? "none" : "flex";
        elementDisable.style.display = elementDisable.style.display === "flex" ? "none" : "flex";

        if (elementEnable.style.display === "flex") {
            editButton.style.backgroundColor = "#4e8a00";
            editButton.innerHTML = "����.";
        }
        else {
            editButton.style.backgroundColor = "black";
            editButton.innerHTML = "���.";
        }
    } else return undefined;
}

const errorTextBlock = document.getElementById("error_text");

const appentErrorText = (text) => errorTextBlock.innerText = text;

const setVisibleMessageBlock = () => setInterval(() => {
    if (errorTextBlock.innerText.length >= 1) {

        errorTextBlock.style.display = "flex";
        setTimeout(() => {
            errorTextBlock.innerText = "";
        }, 5000);
    }
}, 500);
setVisibleMessageBlock();

const addValue = (idUppendBlockForm, idUppendBlockInfo, idCurrentInput) => {
    const currentInput = document.getElementById(idCurrentInput);
    const uppendBlockForm = document.getElementById(idUppendBlockForm);
    const uppendInfoBlock = document.getElementById(idUppendBlockInfo);

    if (currentInput && uppendBlockForm && uppendInfoBlock) {
        const newHiddeninput = currentInput.cloneNode(true);
        newHiddeninput.id = "";

        uppendBlockForm.appendChild(newHiddeninput);

        const newInfoItem = document.createElement("b");
        newInfoItem.innerText = currentInput.value;

        uppendInfoBlock.appendChild(newInfoItem);

        currentInput.value = "";
    }
}