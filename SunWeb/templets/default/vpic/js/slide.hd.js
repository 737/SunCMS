var Drag={
	obj: null,
	leftTime: null,
	rightTime: null,
	init: function (o,minX,maxX,btnRight,btnLeft) {
		o.onmousedown=Drag.start;
		o.hmode=true;
		if(o.hmode&&isNaN(parseInt(o.style.left))) { o.style.left="0px"; }
		if(!o.hmode&&isNaN(parseInt(o.style.right))) { o.style.right="0px"; }
		o.minX=typeof minX!='undefined'?minX:null;
		o.maxX=typeof maxX!='undefined'?maxX:null;
		o.onDragStart=new Function();
		o.onDragEnd=new Function();
		o.onDrag=new Function();
		btnLeft.onmousedown=Drag.startLeft;
		btnRight.onmousedown=Drag.startRight;
		btnLeft.onmouseup=Drag.stopLeft;
		btnRight.onmouseup=Drag.stopRight;
	},
	start: function (e) {
		var o=Drag.obj=this;
		e=Drag.fixE(e);
		var x=parseInt(o.hmode?o.style.left:o.style.right);
		o.onDragStart(x);
		o.lastMouseX=e.clientX;
		if(o.hmode) {
			if(o.minX!=null) { o.minMouseX=e.clientX-x+o.minX; }
			if(o.maxX!=null) { o.maxMouseX=o.minMouseX+o.maxX-o.minX; }
		} else {
			if(o.minX!=null) { o.maxMouseX= -o.minX+e.clientX+x; }
			if(o.maxX!=null) { o.minMouseX= -o.maxX+e.clientX+x; }
		}
		document.onmousemove=Drag.drag;
		document.onmouseup=Drag.end;
		return false;
	},
	drag: function (e) {
		e=Drag.fixE(e);
		var o=Drag.obj;
		var ex=e.clientX;
		var x=parseInt(o.hmode?o.style.left:o.style.right);
		var nx;
		if(o.minX!=null) { ex=o.hmode?Math.max(ex,o.minMouseX):Math.min(ex,o.maxMouseX); }
		if(o.maxX!=null) { ex=o.hmode?Math.min(ex,o.maxMouseX):Math.max(ex,o.minMouseX); }
		nx=x+((ex-o.lastMouseX)*(o.hmode?1:-1));
		$("scrollcontent").style[o.hmode?"left":"right"]=(-nx*barUnitWidth)+"px";
		Drag.obj.style[o.hmode?"left":"right"]=nx+"px";
		Drag.obj.lastMouseX=ex;
		Drag.obj.onDrag(nx);
		return false;
	},
	startLeft: function () {
		Drag.leftTime=setInterval("Drag.scrollLeft()",1);
	},
	scrollLeft: function () {
		var c=$("scrollcontent");
		var o = $("scrollbar");
        var ol = parseInt(o.style.left.replace("px",""));
        if ((ol < o.maxX) && (ol >= 0)) {
            o.style.left = (ol + 1) + "px";
			c.style.left=(-(ol+1)*barUnitWidth)+"px";
		} else {
			Drag.stopLeft();
		}
	},
	stopLeft: function () {
		clearInterval(Drag.leftTime);
	},
	startRight: function () {
		Drag.rightTime=setInterval("Drag.scrollRight()",1);
	},
	scrollRight: function () {
		var c=$("scrollcontent");
		var o = $("scrollbar");
		var ol = parseInt(o.style.left.replace("px", ""));
		if ((ol <= o.maxX) && (ol > 0)) {
		    o.style.left = (ol - 1) + "px";
		    c.style.left = (-(ol - 1) * barUnitWidth) + "px";
		} else {
			Drag.stopRight();
		}
	},
	stopRight: function () {
		clearInterval(Drag.rightTime);
	},
	end: function () {
		document.onmousemove=null;
		document.onmouseup=null;
		Drag.obj.onDragEnd(parseInt(Drag.obj.style[Drag.obj.hmode?"left":"right"]));
		Drag.obj=null;
	},
	fixE: function (e) {
		if(typeof e=='undefined') { e=window.event; }
		if(typeof e.layerX=='undefined') { e.layerX=e.offsetX; }
		return e;
	}
};


function $(el) {
    if (!el) {
        return null;
    }
    else if (typeof el == 'string') {
        return document.getElementById(el);
    }
    else if (typeof el == 'object') {
        return el;
    }
}

function getpos(element) {
    if (arguments.length != 1 || element == null) {
        return null;
    }
    var elmt = element;
    var offsetTop = elmt.offsetTop;
    var offsetLeft = elmt.offsetLeft;
    var offsetWidth = elmt.offsetWidth;
    var offsetHeight = elmt.offsetHeight;
    while (elmt = elmt.offsetParent) {
        if (elmt.style.position == 'absolute' || (elmt.style.overflow != 'visible' && elmt.style.overflow != '')) {
            break;
        }
        offsetTop += elmt.offsetTop;
        offsetLeft += elmt.offsetLeft;
    }
    return {
        top: offsetTop,
        left: offsetLeft,
        right: offsetWidth + offsetLeft,
        bottom: offsetHeight + offsetTop
    };
}
function imageonmousemove(evnt) {
    var photopos = getpos($("bigImage"));
    if (evnt) {
        nx = (parseInt(evnt.clientX) - photopos.left) / $("bigImage").width;
        if (nx > 0.5 && picIndex < liarr.length - 1) {
            if ($("bigLink").style.cursor.toString().indexOf("left") != -1 || !$("bigLink").style.cursor) {
                $("bigLink").style.cursor = "url(http://localhost/templets/default/vpic/css/right.cur),auto";
            }
            $("bigImage").title = "点击浏览下一张>>";
            $("bigLink").onclick = function () {
                showPic(picIndex + 1);
            }
        }
        if (nx <= 0.5 && picIndex > 0) {
            if ($("bigLink").style.cursor.toString().indexOf("right") != -1 || !$("bigLink").style.cursor) {
                $("bigLink").style.cursor = "url(http://localhost/templets/default/vpic/css/left.cur),auto";
            }
            $("bigImage").title = "<<点击浏览上一张";
            $("bigLink").onclick = function () {
                showPic(picIndex - 1);
            }
        }
    }
}
document.onkeydown = pageEvent;
function pageEvent(evt) {
    evt = evt || window.event;
    var key = evt.which || evt.keyCode;
    if (key == 37 && picIndex > 0) showPic(picIndex - 1);
    if (key == 39 && picIndex < liarr.length - 1) showPic(picIndex + 1);
};
function showPic(i) {
    if (i < 0 || i > liarr.length - 1) return;
    picIndex = i;
    var pic = liarr[picIndex].getElementsByTagName("img")[0];
    $("bigImage").src = pic.src.replace("v_","");
    $("desDiv").innerHTML = pic.alt;
    var c = $("scrollcontent");
    var o = $("scrollbar");

    var l = 0 - (parseInt(c.style.left.replace("px", "")) | 0);

   
    if (l + 470 <= 118 * i) {
        o.style.left = (118 * (i + 1) - 470) / barUnitWidth + "px";
        c.style.left = -118 * (i + 1) + 470 + "px";
    } else if (l >= 118 * i) {
        o.style.left = (118 * i) / barUnitWidth + "px";
        c.style.left = -118 * i + "px";
    }

    for (var n = 0; n < liarr.length; n++) {
        liarr[n].getElementsByTagName("img")[0].style.borderColor = (n == picIndex ? "#a46b00" : "#666666");
    }
}


var barUnitWidth = 0;
var picIndex = 0;
var scrollcontent = $('scrollcontent');
if (scrollcontent) {
    var liarr = scrollcontent.getElementsByTagName("li");
    barUnitWidth = (liarr.length * 118 - 470) / 298;

    if (liarr.length * 118 > 472) {
        Drag.init($('scrollbar'), 0, 298, $('scrollleft'), $('scrollright'));
    } else {
        $("scrollbar").style.display = "none";
    }
}