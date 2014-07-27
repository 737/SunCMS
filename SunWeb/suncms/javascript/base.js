//global setting 
// ueditor
window.UEDITOR_HOME_URL = "/suncms/ueditor/";


var sun = sun || {};

sun.url = function () {
    var model = {
        prod: {
            core: {
                main: "definition/main.json",
                cutform: "definition/core/cutform/index.json",
                archive: '/sun/api/pagelet/archive/retrieve.ashx',
                archive_update: '/sun/api/pagelet/archive/update.ashx',
                archive_delete: '/sun/api/pagelet/archive/remove.ashx?id={0}',  // 0 -> id
                archive_add: '/sun/api/pagelet/archive/create.ashx',
                channel: '/sun/api/pagelet/channel/retrieve.ashx',
                channel_edit: '/sun/api/pagelet/channel/update.ashx',
                channel_add: '/sun/api/pagelet/channel/create.ashx',
                channel_delete: '/sun/api/pagelet/channel/remove.ashx?id={0}',  // 0 -> id
            },
            module: {
                friendlink: "/sun/api/pagelet/friendlink/retrieve.ashx",
                friendlinkEdit: "/sun/api/pagelet/friendlink/update.ashx",  // post
                friendlinkAdd: "/sun/api/pagelet/friendlink/create.ashx",  // post
                friendlinkDelete: "/sun/api/pagelet/friendlink/remove.ashx",
                friendLinkGroup: "/sun/api/pagelet/friendlinkgroup/retrieve.ashx",
                friendLinkGroupEdit: "/sun/api/pagelet/friendlinkgroup/update.ashx",  // post
                friendLinkGroupAdd: "/sun/api/pagelet/friendlinkgroup/create.ashx",  // post
                friendLinkGroupDelete: "/sun/api/pagelet/friendlinkgroup/remove.ashx?id={0}",  // 0 -> id
                advertisement: "/sun/api/pagelet/advertisement/retrieve.ashx",
                advertisementEdit: "/sun/api/pagelet/advertisement/update.ashx", //POST
                advertisementAdd: "/sun/api/pagelet/advertisement/create.ashx", //POST
                advertisementDelete: "/sun/api/pagelet/advertisement/remove.ashx?id={0}",  // 0 -> id
                advertisementgroup: "/sun/api/pagelet/advertisementgroup/retrieve.ashx",
                advertisementgroupEdit: "/sun/api/pagelet/advertisementgroup/update.ashx", //POST
                advertisementgroupAdd: "/sun/api/pagelet/advertisementgroup/create.ashx", //POST
                advertisementgroupDelete: "/sun/api/pagelet/advertisementgroup/remove.ashx?id={0}",  // 0 -> id
            },
            html: {},
            collection: {},
            member: {},
            templet: {},
            system: {
                base: "definition/system/base.json"
            }
        },
        dev: {
            core: {
                main: "definition/main.json",
                cutform: "definition/core/cutform/index.json",
                archive: 'definition/core_archive.json',
                channel: 'definition/core_channel.json'
            },
            module: {
                friendlink: "definition/module/friendlink/index.json",
                friendLinkGroup: "definition/module/friendlinkgroup/index.json",
                advertisement: "definition/module/advertisement/index.json",
                advertisementgroup: "definition/module/advertisementgroup/index.json"
            },
            html: {},
            collection: {},
            member: {},
            templet: {},
            system: {
                base: "definition/system/base.json"
            }
        }
    };

    var _mode = model['prod'];   // dev prod

    return {
        core: {
            getMainURL: function () {
                return _mode.core.main;
            },
            getCutformUrl: function() {
                return _mode.core.cutform
            },
            getTestURL: function () { },
            getArchiveURL: function() {
                return _mode.core.archive
            },
            getArchive_edit: function() {
                return _mode.core.archive_update
            },
            getArchive_delete: function(nId) {
                return sun.util.stringFormat(_mode.core.archive_delete, nId)
            },
            getArchive_add: function() {
                return _mode.core.archive_add
            },
            getChannel: function(){
                return _mode.core.channel
            },
            getChannel_edit: function(){
                return _mode.core.channel_edit
            },
            getChannel_add: function(){
                return _mode.core.channel_add
            },
            getChannel_delete: function(nId){
                return sun.util.stringFormat(_mode.core.channel_delete, nId)
            }
        },
        module: {
            getFriendlink: function () {
                return _mode.module.friendlink
            },
            getFriendlinkEdit: function () {
                return _mode.module.friendlinkEdit
            },
            getFriendlinkAdd: function () {
                return _mode.module.friendlinkAdd
            },
            getFriendlinkDelete: function() {
                return _mode.module.friendlinkDelete
            },
            getFriendLinkGroup: function () {
                return _mode.module.friendLinkGroup
            },
            getFriendLinkGroupEdit: function () {
                return _mode.module.friendLinkGroupEdit
            },
            getFriendLinkGroupAdd: function () {
                return _mode.module.friendLinkGroupAdd
            },
            getFriendLinkGroupDelete: function (nId) {
                return sun.util.stringFormat(_mode.module.friendLinkGroupDelete, nId)
            },
            getAdvertisement: function () {
                return _mode.module.advertisement
            },
            getAdvertisementEdit: function () {
                return _mode.module.advertisementEdit
            },
            getAdvertisementAdd: function () {
                return _mode.module.advertisementAdd
            },
            getAdvertisementDelete: function(nId) {
                return sun.util.stringFormat(_mode.module.advertisementDelete, nId)
            },
            getAdvertisementGroup: function () {
                return _mode.module.advertisementgroup
            },
            getAdvertisementGroupEdit: function () {
                return _mode.module.advertisementgroupEdit
            },
            getAdvertisementGroupAdd: function () {
                return _mode.module.advertisementgroupAdd
            },
            getAdvertisementGroupDelete: function (nId) {
                return sun.util.stringFormat(_mode.module.advertisementgroupDelete, nId)
            }
        },
        system: {
            getBaseUrl: function() {
                return _mode.system.base;
            }
        },
        util: {
            getMD5URL: function () { }
        }
    }

}();

sun.ajax = function () {
    var mime = {
        html: 'html',
        js: 'script',
        json: 'json',
        xml: 'xml',
        txt: 'text'
    }

    base = function (sUrl, sType, sDataType, oData, fnCallBack, isShlowLoading) {
        var _data = null;
        
        if (sType === 'post') {
            //下面转来转去的，是为了消除 oData 中的function,而后台对post的请为 对证筛选，所以ajax的传入参数必须为对象而不是json对象
            oData = JSON.stringify(oData);
            oData = JSON.parse(oData);
        }

        $.ajax({
            async: true,
            type: sType,
            url: sUrl,
            data: oData,
            dataType: sDataType,
            beforeSend: function (XMLHttpRequest) {
                if (!!isShlowLoading) {
                    $('#global_loadding').show();
                }

            },
            success: function (data, textStatus) {
                if (!!isShlowLoading) {
                    $('#global_loadding').hide();
                }

                _data = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (!!isShlowLoading) {
                    $('#global_loadding').hide();
                }

                debugger;
                _data = errorThrown;
            }
        }).done(function (data, textStatus, _self) {
            if (_.isFunction(fnCallBack)) {
                fnCallBack(data, textStatus);
            }
        });;

        return _data;
    };


    return {
        getPage: function (sPageUrl, oData, fnCallBack) {
            return base(sPageUrl, 'get', mime.html, oData, fnCallBack, false);
        },
        getJSON: function (sPageUrl, oData, fnCallBack) {
            if ((_.isFunction(oData)) && (!fnCallBack)) {
                fnCallBack = oData;
                oData = null;
            }

            return base(sPageUrl, 'get', mime.json, oData, fnCallBack, true);
        },
        post: function (sPageUrl, oData, fnCallBack) {
            if ((_.isFunction(oData)) && (!fnCallBack)) {
                fnCallBack = oData;
                oData = null;
            }

            return base(sPageUrl, 'post', mime.json, oData, fnCallBack, true);
        }
    }
}();

sun.config = function () {
    return {
        pagePath: 'pagelet/',
        adminMenuCollection: {} //e.g ==> { url : html }
    }
}();


sun.util = sun.util || {};

sun.util = function () {

    return {
        checkJSON: function (jsonData) {
            var flag = false;

            if (jsonData.State === false) {
                console.warn(jsonData.Message);
                flag = true;
            };

            return flag;
        },
        clone: function (val) {
            var newObj = null,
                oldObj = null;

            if (!!val) {
                oldObj = JSON.stringify(val);
                newObj = JSON.parse(oldObj);
            }

            return newObj;
        },
        jqClone: function (val) {
            var newObj = jQuery.extend({}, val);

            return newObj;
        },
        stringFormat: function (txt) {
            var arg = arguments,
                matchResult,
                matLength,
                str = txt,
                reg = /\{\d+?\}/gmi,
                i;

            matchResult = str.match(reg);
            if (matchResult) {
                matLength = matchResult.length;
                if (arg.length >= matLength) {
                    for (i = 0; i < matLength; i++) {
                        str = str.replace(matchResult[i], arg[i + 1]);
                    }
                }
            }

            return str;
        },
        parseEnToCn: function (arguments) {
            var _temp = arguments;

            return !!_temp === true ? '是' : '否';
        },
        // eg. format = 'yyyy-MM-dd hh:mm:ss'
        getCurrentTime: function(format) {
            var _this = new Date();
            var o = {
                "M+": _this.getMonth() + 1, //month
                "d+": _this.getDate(), //day
                "h+": _this.getHours(), //hour
                "m+": _this.getMinutes(), //minute
                "s+": _this.getSeconds(), //second
                "q+": Math.floor((_this.getMonth() + 3) / 3), //quarter
                "S": _this.getMilliseconds() //millisecond
            }

            if(!format) {
                format = "yyyy-MM-dd hh:mm:ss";
            }

            if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (_this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(format))
                    format = format.replace(RegExp.$1,
                        RegExp.$1.length == 1 ? o[k] :
                        ("00" + o[k]).substr(("" + o[k]).length));
            return format;
        }
    }
}();