

var sun = sun || {};

//headerFrame
sun.header = function () {
    var me = {};

    return {
        init: function () {
            var $ul = $('#header_nav'),
                $li = $ul.find('li'),
                nFirstNavigation = 0;

            $li.on('click', 'a', function (oEvent) {
                var _this = $(this),
                    authID = _this.attr('href'),
                    parentDOM = _this.parent(),
                    reg = /\d{1,}/g;

                if (parentDOM.hasClass('cur')) {
                    return;
                }

                _this.parents('ul').find('.cur').removeAttr('class');
                _this.parent().addClass('cur');

                authID = authID.match(reg)[0];

                sun.left.getMenu(authID);
            });
        }
    }
}();

//mainFrame
sun.main = function () {
    var me = {},
        $mainFrame;

    return {
        init: function () {
            me = this;
            $mainFrame = $('#mainFrame');

            //default show the home page
            //me.addPage('home.aspx', '首页');
        },
        addPage: function (sPageUrl, sPageName) {
            var _data = sun.ajax.getPage(sPageUrl);            
            document.forms[0].action = sPageUrl;

            $mainFrame.html(_data)
        }
    }
}();

//leftFrame
sun.left = function () {
    var me = {},
        menuCache = sun.config.adminMenuCollection;

    function getMenu(iID) {
        var template = '',
            $menuIdex = $('#menu_detail_index'),
            _url = window.location.origin + '/api/adminmenus/' + iID,
            josn = {};

        if (!!menuCache[iID]) {
            $('#menu_detail_index').html(menuCache[iID]);
            return;
        }

        json = sun.ajax.getJSON(_url);

        if (json.Message === false) {
            return;
        }

        //为了防止数据返回的不正确导致的重复请求而缓存
        if (json.Data.length < 1) {
            //cache the content
            menuCache[iID] = [];
        }

        _.each(json.Data, function (_item) {
            var itemContent = '',
                itemName = _item.Name;

            _.each(_item.Children, function (_subItem) {
                var menuName = _subItem.Name,
                    menuUrl = _subItem.Url,
                    icoTitle = _subItem.IcoTitle,
                    icoType = _subItem.IcoType,
                    icoUrl = _subItem.IcoUrl,
                    icoContent = '';

                if (icoType) {
                    icoPath = '../style/default/images/icons_' + icoType + '.png';
                    icoContent = '<div class="menu-detail-siteMenu-icon"><a href="' + icoUrl + '" title="' + icoTitle + '" target="_self"><img src="' + icoPath + '" alt="' + icoTitle + '" /></a></div>';
                }

                itemContent = itemContent + '<li>' + icoContent + '<div class="menu-detail-siteMenu-content"><a href=\'javascript:void("' + menuUrl + '","' + menuName + '")\' target="_self">' + menuName + '</a></div></li>';
            });

            template = template + '<dl class="menu-detail-subItem"><dt><b>' + itemName + '</b></dt><dd><ul class="menu-detail-siteMenu">' + itemContent + '</ul></dd></dl>';

            //cache the content
            menuCache[iID] = template;
        })

        $('#menu_detail_index').html(template);
    }

    return {
        init: function () {
            var $menuSections = $('#menu_sections > li'),
                selectedSection = $menuSections[0],
                subItem = $('#menu_detail_index');

            me = this;

            $menuSections.click(function (oEvent) {
                var me = $(this),
                    key = _.last(this.id.split('_'));

                $menuSections.removeClass('cur');
                selectedSection = me.addClass('cur')[0];

                //fill the menu content by id
                getMenu($.trim(key));
            });

            $menuSections.hover(
                function () {
                    $(this).addClass('cur');
                },
                function () {
                    if (this != selectedSection) {
                        $(this).removeClass('cur');
                    }
                }
            );

            subItem.on('click', 'dt', function () {
                var $title = $(this);
                var $subItem = $title.parent();
                var $content = $title.siblings();

                $content.slideToggle('fast');
                $subItem.toggleClass('fold');
            })

            subItem.on('click', 'a', function () {
                var _this = $(this),
                    reg = /([^"]+[^"])/ig,
                    href = _this.attr('href').match(reg),
                    name = href[2],
                    url = href[1];

                //addpage to mainFrame
                sun.main.addPage(url, name);
            })

            //set default menu
            getMenu('core');
        }
    }
}();

//footerFrame
sun.footer = function () {
    var me = {};
}();

sun.page = sun.page || { core: {}, global: {}, html: {}, member: {}, module: {}, system: {}, templet: {} };

sun.page.system.base = function () {

    return {
        init: function (sDomKey) {
            var aList = $(sDomKey).children("dd:odd");

            aList.addClass("even");
        },
        ok: function(e){
            //debugger;
            
            document.forms[0].target = "hideIframe";
            document.forms[0].submit();
            
            debugger;
        },
        reset: function (e) {
            //document.forms[0].target = "hideIframe";
            document.forms[0].reset();
        },
        showConfig: function (ID) {
            console.log(ID);
        },
        formsubmit: function (c) {
            debugger;
            //debugger;
        }
    }
}();