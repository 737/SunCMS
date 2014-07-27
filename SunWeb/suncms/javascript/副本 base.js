
var sun = sun || {};

sun.ajax = function () {
    var mime = {
        html: 'html',
        js: 'script',
        json: 'json',
        xml: 'xml',
        txt: 'text'
    }

    return {
        base: function (sUrl, sType, sDataType, oData) {
            var _url = sUrl,
                _type = sType,
                _data = oData,
                _datatype = sDataType;

                console.log(_data);
                
            $.ajax({
                async: false,
                type: _type,
                url: _url,
                data: _data,
                dataType: _datatype,
                success: function (data, textStatus) {
                    _data = data;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //debugger;
                    _data = errorThrown;
                }
            }).done(function () {
                var pageObj = sUrl.split('.')[0].split('/');

                //debugger;
            });;

            return _data;
        },
        getPage: function (sPageUrl, oData) {
            if (!sPageUrl) {
                return 'url error';
            }

            oData = oData || {};
            oData.sun_page = sPageUrl;

            return this.base('mother.aspx', 'get', mime.html, oData);
        },
        getJSON: function (sPageUrl, oData) {
            if (!sPageUrl) {
                return 'url error';
            }

            return this.base(sPageUrl, 'get', mime.json, oData);
        }
    }
}();

sun.config = function () {
    return {
        pagePath:'pagelet/',
        adminMenuCollection: {} //e.g ==> { url : html }
    }
}();

//  全局变量
    var goto;

$(document).ready(function () {
    //_.each(sun.config.framesID, function (sValue, sKey) {
    //    var oFrame;

    //    if (_.has(sc, sKey)) {
    //        oFrame = sc[sKey];
    //        if ((_.isObject(oFrame)) && (_.isFunction(sc[sKey].init))) {
    //            sc[sKey].init()
    //        }
    //    }
    //})

    sun.left.init();
    sun.main.init();
    sun.header.init();

    //shortcut ==> sun.main.loadPage()
    //放在这里可能 IE7下可能失效
    goto = function (sPageUrl, sPageName, oParameter) {
        sun.main.loadPage(sPageUrl, sPageName, oParameter)
    };
});