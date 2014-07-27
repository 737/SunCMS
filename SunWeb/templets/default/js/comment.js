__load(function(){
    meta.g("vimg").style.display = 'none';
    var auth =meta.g("auth");
    auth.onfocus=function(){ setImgsrc(); }
    auth.onblur= function (){meta.g("vimg").style.display='none';}
});

function _comment() {
    var box = document.getElementById("contentbox");
    var poll = document.getElementById("g");
    if (poll)   poll.value = "3";
    if (box.innerHTML == "") alert("«ÎÃÓ–¥∆¿¬€ƒ⁄»›£°"); else check(document.getElementById("auth").value);
}

function cmted() {
    comment();
    document.getElementById("auth").value = "";
    document.getElementById("contentbox").value = "";
}
function setImgsrc() {
    var img = document.getElementById("vimg");
    img.src = '/ac.ashx?code=' + Math.random();
    img.style.display = "";
}

function vote(i){meta.g("g").value=i}
