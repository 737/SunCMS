function preLoad() {
	if (!this.support.loading) {
		alert("You need the Flash Player 9.028 or above to use SWFUpload.");
		return false;
	}
}
function loadFailed() {
	alert("Something went wrong while loading SWFUpload. If this were a real application we'd clean up and then give you an alternative");
}
function fileQueueError(file, errorCode, message) {
	try {
		var imageName = "error.gif";
		var errorName = "";
		if (errorCode === SWFUpload.errorCode_QUEUE_LIMIT_EXCEEDED) {
			errorName = "You have attempted to queue too many files.";
		}

		if (errorName !== "") {
			alert(errorName);
			return;
		}

		switch (errorCode) {
		case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
			imageName = "zerobyte.gif";
			break;
		case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
			imageName = "toobig.gif";
			break;
		case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
		case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
		default:
		    alert("Error:" + message); //Ì«¶à
			break;
		}

		//addImage("-1");

	} catch (ex) {
		this.debug(ex);
	}
}

function fileDialogComplete(numFilesSelected, numFilesQueued) {
	try {
		if (numFilesQueued > 0) {
			this.startUpload();
		}
	} catch (ex) {
		this.debug(ex);
	}
}

function uploadProgress(file, bytesLoaded) {

	try {
		var percent = Math.ceil((bytesLoaded / file.size) * 100);

		var progress = new FileProgress(file,  this.customSettings.upload_target);
		progress.setProgress(percent);
		if (percent === 100) {
			progress.setStatus("Save...");
			progress.toggleCancel(false, this);
		} else {
			progress.setStatus("Uploading...");
			progress.toggleCancel(true, this);
		}
	} catch (ex) {
		this.debug(ex);
	}
}
 

function uploadSuccess(file, serverData) {
	try {
	    if (this.customSettings.upload_callback && serverData != "-1") this.customSettings.upload_callback(serverData);
		var progress = new FileProgress(file,  this.customSettings.upload_target);
		progress.setStatus("Files Uploaded.");
		progress.toggleCancel(false);
		progress.setStatus("");

	} catch (ex) {
		this.debug(ex);
	}
}

function uploadComplete(file) {
	try {
		/*  I want the next upload to continue automatically so I'll call startUpload here */
		if (this.getStats().files_queued > 0) {
			this.startUpload();
      } else {          
			var progress = new FileProgress(file,  this.customSettings.upload_target);
			progress.setComplete();
			progress.setStatus("All Files received.");
			progress.toggleCancel(false);
			progress.setStatus("");
			if (this.customSettings.complete_callback) this.customSettings.complete_callback();
		}
	} catch (ex) {
		this.debug(ex);
	}
}

function uploadError(file, errorCode, message) {
	var imageName =  "error.gif";
	var progress;
	try {
		switch (errorCode) {
		case SWFUpload.UPLOAD_ERROR.FILE_CANCELLED:
			try {
				progress = new FileProgress(file,  this.customSettings.upload_target);
				progress.setCancelled();
				progress.setStatus("Cancelled");
				progress.toggleCancel(false);
				progress.setStatus("");
			}
			catch (ex1) {
				this.debug(ex1);
			}
			break;
		case SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED:
			try {
				progress = new FileProgress(file,  this.customSettings.upload_target);
				progress.setCancelled();
				progress.setStatus("Stopped");
				progress.toggleCancel(false);
				progress.setStatus("");
			}
			catch (ex2) {
				this.debug(ex2);
			}
		case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:
			imageName = "uploadlimit.gif";
			break;
		default:
		    alert("Error:" + message);
			break;
		}

	} catch (ex3) {
		this.debug(ex3);
	}
}

function fadeIn(element, opacity) {
    
	var reduceOpacityBy = 5;
	var rate = 30;	// 15 fps


	if (opacity < 100) {
		opacity += reduceOpacityBy;
		if (opacity > 100) {
			opacity = 100;
		}

		try {
		    if (element.filters) {
		        try {
		            element.filters.item("DXImageTransform.Microsoft.Alpha").opacity = opacity;
		        } catch (e) {
		            // If it is not set initially, the browser will throw an error.  This will set it if it is not set yet.
		            element.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + opacity + ')';
		        }
		    } else {
		        element.style.opacity = opacity / 100;
		    }
		} catch (e) {
	      	return;
		}
	}

	if (opacity < 100) {
		setTimeout(function () {
			fadeIn(element, opacity);
		}, rate);
	}
}


function setStyle(R, Q) {
    var S = document.styleSheets;
  
    if (!S || S.length <= 0) {
        var P = document.createElement("STYLE");
        P.type = "text/css";
        var T = document.getElementsByTagName("HEAD")[0];
        T.appendChild(P)
    }
  
    S = document.styleSheets;
    S = S[S.length - 1];
  
    var ie;
    if (/msie (\d+\.\d)/i.test(navigator.userAgent)) {
        ie = parseFloat(RegExp.$1)
    }
    if (ie) {      
        S.addRule(R, Q)       
    } else {
        S.insertRule(R + " { " + Q + " }", S.cssRules.length)
    }   
}


/* ******************************************
 *	FileProgress Object
 *	Control object for displaying file info
 * ****************************************** */

function FileProgress(file, targetID) {
	this.fileProgressID = "divFileProgress";
	this.file = file;
	this.targetID = targetID;
	document.getElementById(targetID).style.display = '';  
	this.fileProgressWrapper = document.getElementById(this.fileProgressID);
	if (!this.fileProgressWrapper) {
	    setStyle(".progressWrapper", "width: 100%;overflow: hidden;");	  
	    setStyle(".progressContainer", "margin: 5px;padding: 4px;overflow: hidden;");
	    setStyle("a.progressCancel", "font-size: 0;display: block;height: 14px;width: 14px;background-image: url(../images/cancelbutton.gif);background-repeat: no-repeat;background-position: -14px 0px;	float: right;");
	    setStyle("a.progressCancel:hover", "background-position: 0px 0px;");	   
	    setStyle(".progressName", "font-size: 8pt;font-weight: 700;color: #555;width: 323px;height: 14px;text-align: left;	white-space: nowrap;overflow: hidden;");	  
        setStyle(".progressBarInProgress", "font-size: 0;width: 0%;height: 2px;background-color: blue;margin-top: 2px;");
        setStyle(".progressBarComplete", "width: 100%;background-color: green;visibility: hidden;font-size: 0;width: 0%;height: 2px;background-color: blue;margin-top: 2px;");
        setStyle(".progressBarError", "width: 100%;background-color: red;visibility: hidden;font-size: 0;width: 0%;height: 2px;background-color: blue;margin-top: 2px;");
	    setStyle(".progressBarStatus", "margin-top: 2px;width: 337px;font-size: 7pt;font-family: Arial;text-align: left;white-space: nowrap;");
	    setStyle(".red", "border: solid 1px #B50000;background-color: #FFEBEB;");	
        setStyle(".swfupload", "vertical-align:top;");		    
	   
		this.fileProgressWrapper = document.createElement("div");
		this.fileProgressWrapper.className = "progressWrapper";
		this.fileProgressWrapper.id = this.fileProgressID;

		this.fileProgressElement = document.createElement("div");
		this.fileProgressElement.className = "progressContainer";

		var progressCancel = document.createElement("a");
		progressCancel.className = "progressCancel";
		progressCancel.href = "#";
		progressCancel.style.visibility = "hidden";
		progressCancel.appendChild(document.createTextNode(" "));	

		var progressBar = document.createElement("div");
		progressBar.className = "progressBarInProgress";

		var progressStatus = document.createElement("div");
		progressStatus.className = "progressBarStatus";
		progressStatus.innerHTML = "&nbsp;";

		this.fileProgressElement.appendChild(progressCancel);		
		this.fileProgressElement.appendChild(progressStatus);
		this.fileProgressElement.appendChild(progressBar);

		this.fileProgressWrapper.appendChild(this.fileProgressElement);

		document.getElementById(targetID).appendChild(this.fileProgressWrapper);		
		fadeIn(this.fileProgressWrapper, 0);

	} else {
		this.fileProgressElement = this.fileProgressWrapper.firstChild;		
	}

	this.height = this.fileProgressWrapper.offsetHeight;

}
FileProgress.prototype.setProgress = function (percentage) {
    this.fileProgressElement.className = "progressContainer";	
	this.fileProgressElement.childNodes[2].className = "progressBarInProgress";
	this.fileProgressElement.childNodes[2].style.width = percentage + "%";
};
FileProgress.prototype.setComplete = function () {
    this.fileProgressElement.className = "progressContainer";
	this.fileProgressElement.childNodes[2].className = "progressBarComplete";
	this.fileProgressElement.childNodes[2].style.width = "";

};
FileProgress.prototype.setError = function () {
	this.fileProgressElement.className = "progressContainer red";
	this.fileProgressElement.childNodes[2].className = "progressBarError";
	this.fileProgressElement.childNodes[2].style.width = "";

};
FileProgress.prototype.setCancelled = function () {
	this.fileProgressElement.className = "progressContainer";
	this.fileProgressElement.childNodes[2].className = "progressBarError";
	this.fileProgressElement.childNodes[2].style.width = "";

};
FileProgress.prototype.setStatus = function (status) {
    if (status) status = this.file.name+":"+ status;
    this.fileProgressElement.childNodes[1].innerHTML = status;
    var e =  document.getElementById(this.targetID);
    if (status)
       _popWin(e);
    else {
       e.style.display = "none";
    }
};

FileProgress.prototype.toggleCancel = function (show, swfuploadInstance) {
	this.fileProgressElement.childNodes[0].style.visibility = show ? "visible" : "hidden";
	if (swfuploadInstance) {
		var fileID = this.fileProgressID;
		this.fileProgressElement.childNodes[0].onclick = function () {
			swfuploadInstance.cancelUpload(fileID);
			return false;
		};
	}
};


function _getViewportHeight() {
    if (window.innerHeight != window.undefined) return window.innerHeight;
    if (document.compatMode == 'CSS1Compat') return document.documentElement.clientHeight;
    if (document.body) return document.body.clientHeight;
    return window.undefined;
};
function _getViewportWidth() {
    if (window.innerWidth != window.undefined) return window.innerWidth;
    if (document.compatMode == 'CSS1Compat') return document.documentElement.clientWidth;
    if (document.body) return document.body.clientWidth;
    return window.undefined;
};

function _popWin(e) {
    var fullHeight = _getViewportHeight();
    var fullWidth = _getViewportWidth();

    e.style.display = "";
    e.style.position = "absolute";
    e.style.zIndex = "99";   

    var height = e.offsetHeight;
    var width = e.offsetWidth;

    var theBody = document.documentElement;

    var scTop = parseInt(theBody.scrollTop, 10);
    var scLeft = parseInt(theBody.scrollLeft, 10);

    if (scTop == 0)
        scTop = document.body.scrollTop;
    if (scLeft == 0)
        scLeft = document.body.scrollLeft;

    e.style.top = (scTop + ((fullHeight - height) / 2)) + "px";
    e.style.left = (scLeft + ((fullWidth - width) / 2)) + "px";
}
