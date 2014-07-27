Validator = {
    unValid: /[\x22-\x27\x2A\x2F\x3C\x3E\x3F\x40\x5B-\x5E\x60\x7B\x7D\u201D\u201C\u203B\u2192\u25CB\u25CE\u25C6\u25C7\u25A0\u2584\u2586\u2605\u2606\u3010\u3011\uFFE5]/g,
    isValid: function($) {
        return !this.unValid.test($)
    },
    Valid: "this.isValid(value)",
    Require: /\S+/,
    Email: /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
    Phone: /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/,
    Mobile: /^((\(\d{2,3}\))|(\d{3}\-))?1(3|5|8)\d{9}$/,
    Url: /^http(s){0,1}:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/,
    IdCard: "this.IsIdCard(value)",
    Currency: /^\d+(\.\d+)?$/,
    Number: /^\d+$/,
    Zip: /^[1-9]\d{5}$/,
    QQ: /^[1-9]\d{4,11}$/,
    isIM: function($) {
        return this.QQ.test($) || this.Email.test($)
    },
    Positive: /^\d+(\.\d+)?/,
    Integer: /^[-\+]?\d+$/,
    Double: /^[-\+]?\d+(\.\d+)?$/,
    English: /^[A-Za-z]+$/,
    Chinese: /^[\u0391-\uFFE5]+$/,
    Username: /^[a-z]\w{3,}$/i,
    UnSafe: /^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/,
    IsSafe: function($) {
        return !this.UnSafe.test($)
    },
    SafeString: "this.IsSafe(value)",
    IM: "this.isIM(value)",
    Filter: "this.DoFilter(value, getAttribute('accept'))",
    Limit: "this.limit(value.length,getAttribute('min'), getAttribute('max'))",
    LimitB: "this.limit(this.LenB(value), getAttribute('min'), getAttribute('max'))",
    Date: "this.IsDate(value, getAttribute('min'), getAttribute('format'))",
    Repeat: "value == document.getElementsByName(getAttribute('to'))[0].value",
    Range: "getAttribute('min') < (value|0) && (value|0) < getAttribute('max')",
    Compare: "this.compare(value,getAttribute('operator'),getAttribute('to'))",
    CompareObj: "this.compare(value,getAttribute('operator'),obj[getAttribute('to')].value)",
    Custom: "this.Exec(value, getAttribute('regexp'))",
    Group: "this.MustChecked(getAttribute('name'), getAttribute('min'), getAttribute('max'))",
    ErrorItem: [document.forms[0]],
    ErrorMessage: ["\u4ee5\u4e0b\u539f\u56e0\u5bfc\u81f4\u63d0\u4ea4\u5931\u8d25\uff1a\t\t\t\t"],
    Validate: function(theForm, mode) {
        var obj = theForm || event.srcElement,
            count = obj.elements.length;
        this.ErrorMessage.length = 1;
        this.ErrorItem.length = 1;
        this.ErrorItem[0] = obj;
        for (var i = 0; i < count; i++) with(obj.elements[i]) {
                var _dataType = getAttribute("dataType");
                if (typeof(_dataType) == "object") continue;
                this.ClearState(obj.elements[i]);
                if (getAttribute("require") == "false" && value == "") continue;
                _dataType = _dataType.split(" ");
                var fflag = true;
                for (k = 0, kl = _dataType.length; k < kl; k++) {
                    if (typeof(this[_dataType[k]]) == "undefined") continue;
                    if (!fflag) break;
                    switch (_dataType[k]) {
                        case "IdCard":
                        case "Date":
                        case "Repeat":
                        case "Range":
                        case "Compare":
                        case "CompareObj":
                        case "Custom":
                        case "Group":
                        case "Limit":
                        case "LimitB":
                        case "SafeString":
                        case "Filter":
                        case "IM":
                        case "Valid":
                            if (!eval(this[_dataType[k]])) {
                                this.AddError(i, getAttribute("msg"));
                                fflag = false
                            }
                            break;
                        default:
                            if (!this[_dataType[k]].test(value)) {
                                this.AddError(i, getAttribute("msg"));
                                fflag = false
                            }
                            break
                    }
                }
        }
        if (this.ErrorMessage.length > 1) {
            mode = mode || 1;
            var errCount = this.ErrorItem.length;
            switch (mode) {
                case 2:
                    for (i = 1; i < errCount; i++) this.ErrorItem[i].style.color = "red";
                case 1:
                    if (window.showMessage) showMessage(this.ErrorMessage.join("<br />"), null, function() {
                            try {
                                Validator.ErrorItem[1].focus()
                            } catch ($) {}
                        });
                    else {
                        alert(this.ErrorMessage.join("\n"));
                        if (this.ErrorItem[1].style.display != "none" && this.ErrorItem[1].type != "hidden") this.ErrorItem[1].focus()
                    }
                    break;
                case 3:
                    for (i = 1; i < errCount; i++) {
                        try {
                            var span = document.createElement("SPAN");
                            span.id = "__ErrorMessagePanel" + i;
                            span.style.color = "red";
                            if (this.ErrorItem[i].nextSibling) this.ErrorItem[i].parentNode.insertBefore(span, this.ErrorItem[i].nextSibling);
                            else this.ErrorItem[i].parentNode.appendChild(span);
                            span.innerHTML = this.ErrorMessage[i].replace(/\d+:/, "");
                            this.ErrorItem[i].className = "err"
                        } catch (e) {
                            alert(e.description)
                        }
                    }
                    this.ErrorItem[1].focus();
                    break;
                default:
                    alert(this.ErrorMessage.join("\n"));
                    break
            }
            return false
        }
        return true
    },
    limit: function(A, $, _) {
        $ = $ || 0;
        _ = _ || Number.MAX_VALUE;
        return $ <= A && A <= _
    },
    LenB: function($) {
        return $.replace(/[^\x00-\xff]/g, "**").length
    },
    ClearState: function($) {
        with($) {
            if (style.color == "red") style.color = "";
            if (className == "err") className = "";
            var lastNode = nextSibling;
            if (lastNode && /^__ErrorMessagePanel/.test(lastNode.id)) parentNode.removeChild(lastNode)
        }
    },
    AddError: function($, _) {
        this.ErrorItem[this.ErrorItem.length] = this.ErrorItem[0].elements[$];
        this.ErrorMessage[this.ErrorMessage.length] = this.ErrorMessage.length + ":" + _
    },
    Exec: function($, _) {
        return new RegExp(_, "g").test($)
    },
    compare: function($, A, _) {
        if (this.Double.test($) && this.Double.test(_)) {
            $ = parseInt($, 10);
            _ = parseInt(_, 10)
        }
        switch (A) {
            case "NotEqual":
                return ($ != _);
            case "GreaterThan":
                return ($ > _);
            case "GreaterThanEqual":
                return ($ >= _);
            case "LessThan":
                return ($ < _);
            case "LessThanEqual":
                return ($ <= _);
            default:
                return ($ == _)
        }
    },
    MustChecked: function(B, _, A) {
        var C = document.getElementsByName(B),
            $ = 0;
        _ = _ || 1;
        A = A || C.length;
        for (var D = C.length - 1; D >= 0; D--) if (C[D].checked) $++;
        return _ <= $ && $ <= A
    },
    DoFilter: function($, _) {
        return new RegExp("^.+.(?=EXT)(EXT)$".replace(/EXT/g, _.split(/\s*,\s*/).join("|")), "gi").test($)
    },
    IsIdCard: function(C) {
        C = C.toUpperCase();
        var _ = {
            11: "\u5317\u4eac",
            12: "\u5929\u6d25",
            13: "\u6cb3\u5317",
            14: "\u5c71\u897f",
            15: "\u5185\u8499\u53e4",
            21: "\u8fbd\u5b81",
            22: "\u5409\u6797",
            23: "\u9ed1\u9f99\u6c5f",
            31: "\u4e0a\u6d77",
            32: "\u6c5f\u82cf",
            33: "\u6d59\u6c5f",
            34: "\u5b89\u5fbd",
            35: "\u798f\u5efa",
            36: "\u6c5f\u897f",
            37: "\u5c71\u4e1c",
            41: "\u6cb3\u5357",
            42: "\u6e56\u5317",
            43: "\u6e56\u5357",
            44: "\u5e7f\u4e1c",
            45: "\u5e7f\u897f",
            46: "\u6d77\u5357",
            50: "\u91cd\u5e86",
            51: "\u56db\u5ddd",
            52: "\u8d35\u5dde",
            53: "\u4e91\u5357",
            54: "\u897f\u85cf",
            61: "\u9655\u897f",
            62: "\u7518\u8083",
            63: "\u9752\u6d77",
            64: "\u5b81\u590f",
            65: "\u65b0\u7586",
            71: "\u53f0\u6e7e",
            81: "\u9999\u6e2f",
            82: "\u6fb3\u95e8",
            91: "\u56fd\u5916"
        }, C, D, $, A, B, E = new Array();
        E = C.split("");
        if (_[parseInt(C.substr(0, 2))] == null) return false;
        switch (C.length) {
            case 15:
                if ((parseInt(C.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(C.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(C.substr(6, 2)) + 1900) % 4 == 0)) ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/;
                else ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; if (ereg.test(C)) return true;
                else return false;
                break;
            case 18:
                if (parseInt(C.substr(6, 4)) % 4 == 0 || (parseInt(C.substr(6, 4)) % 100 == 0 && parseInt(C.substr(6, 4)) % 4 == 0)) ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/;
                else ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; if (ereg.test(C)) {
                    A = (parseInt(E[0]) + parseInt(E[10])) * 7 + (parseInt(E[1]) + parseInt(E[11])) * 9 + (parseInt(E[2]) + parseInt(E[12])) * 10 + (parseInt(E[3]) + parseInt(E[13])) * 5 + (parseInt(E[4]) + parseInt(E[14])) * 8 + (parseInt(E[5]) + parseInt(E[15])) * 4 + (parseInt(E[6]) + parseInt(E[16])) * 2 + parseInt(E[7]) * 1 + parseInt(E[8]) * 6 + parseInt(E[9]) * 3;
                    D = A % 11;
                    B = "F";
                    $ = "10X98765432";
                    B = $.substr(D, 1);
                    if (B == E[17]) return true;
                    else return false
                } else return false;
                break;
            default:
                return false;
                break
        }
    },
    IsDate: function(_, $) {
        $ = $ || "ymd";
        var E, C, B, D;
        switch ($) {
            case "ymd":
                E = _.match(new RegExp("^((\\d{4})|(\\d{2}))([-./])(\\d{1,2})\\4(\\d{1,2})$"));
                if (E == null) return false;
                D = E[6];
                B = E[5] * 1;
                C = (E[2].length == 4) ? E[2] : A(parseInt(E[3], 10));
                break;
            case "dmy":
                E = _.match(new RegExp("^(\\d{1,2})([-./])(\\d{1,2})\\2((\\d{4})|(\\d{2}))$"));
                if (E == null) return false;
                D = E[1];
                B = E[3] * 1;
                C = (E[5].length == 4) ? E[5] : A(parseInt(E[6], 10));
                break;
            default:
                break
        }
        if (!parseInt(B)) return false;
        B = B == 0 ? 12 : B;
        var F = new Date(C, B - 1, D);
        return (typeof(F) == "object" && C == F.getFullYear() && B == (F.getMonth() + 1) && D == F.getDate());

        function A($) {
            return (($ < 30 ? "20" : "19") + $) | 0
        }
    }
}