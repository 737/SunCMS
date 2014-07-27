function search(url) {
    var q = document.getElementById('sinput').value;
    if (q == "")
        alert('请输入您要搜索的关键字!');
    else {
        window.location.href = url+"/search/?q=" + q;
    }
}

function setInputValue(input) {
    if (input.value == input.defaultValue) {
        input.value = "";
    }
    input.onblur = function () {
        input.value == "" ? input.value = input.defaultValue : "";
    }
}

function getust(un) {
    if (un == "") {
        window.document.getElementById("login").style.display = "block";
    } else {
        window.document.getElementById("loged").style.display = "block";
        window.document.getElementById("u").innerText = un;
    }
}






