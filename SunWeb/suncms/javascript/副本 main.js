
var sun = sun || {};

//headerFrame
sun.header = function () {
    var me = {};

    function toggleAllMenu() {
        var $allMenu = $('#toggleMenu'),
            $leftFrame = $('#leftFrame');

        $allMenu.toggle(function () {
            $leftFrame.hide();
            $allMenu.text('显示菜单');
        },
        function () {
            $leftFrame.show();
            $allMenu.text('隐藏菜单');
        })
    }

    function loadAllMenu() {
        var $triger = $('#allMenu'),
            $pageMask = $('#pageMask'),
            $allMenu = $("#pageMask_allMenu_box");

        $triger.click(function () {
            $pageMask.show();
            $allMenu.show();
        })

        $pageMask.click(function () {
            $pageMask.hide();
            $allMenu.hide();
        })

        $allMenu.mouseup(function () {
            $pageMask.click();
        })
    }

    return {
        init: function () {
            //toggle the main menu
            toggleAllMenu();

            loadAllMenu();
        },
        openHome: function () {
            this.selectCoreMenu();
            sun.main.loadPage('main.home', '系统首页');
        },
        selectCoreMenu: function () {
            sun.left.selectMenu();
        }
    }
}();

//mainFrame
sun.main = function () {
    var me = {},
        $mainFrame;

    function loadSubmit() {
        $mainFrame.on('submit', 'form:first', function () {
            var $btns = $('#action .button'),
                $loading = $('#action .loading'),
                $message = $('#action .message > div'),
                speed = 'normal',
                _me = this;

            var options = {
                data: { "SunActionType": "update" },
                async: true,
                type: "POST",               //GET,POST
                url: _me.action,
                //target: '#mainFrame',     //a jQuery object, or a DOM element  e.g "#mainFrame"
                //dataType: null,           //xml,json,script,null    default：null（callback responseText valuse）
                beforeSubmit: function (arr, $form, options) {
                    $btns.hide();
                    $loading.fadeIn(speed);
                    $message.hide();
                },
                error: function () {
                    debugger;
                    //$update.append('error')
                },
                success: function (responseText, statusText) {
                    $loading.hide();
                    $btns.fadeIn(speed);
                    $message.fadeIn(speed);
                    $message.on('click', '.close', function () {
                        $message.fadeOut(speed)
                    })
                }
            };

            $(_me).ajaxSubmit(options);
            return false;
        });

        $mainFrame.on('click', 'form input[name="reset"]', function () {
            $message = $('#action .message > div').hide()
        })
    }

    function firePageInit(nameSpace) {
        var flag = false;

        if (nameSpace.length === 2) {
            _flag = _.has(sun.pagelet, nameSpace[0]);
            if (_flag) {
                _flag = _.has(sun.pagelet[nameSpace[0]], nameSpace[1]);
                if (_flag) {
                    _flag = _.has(sun.pagelet[nameSpace[0]][nameSpace[1]], 'init');
                    if (_flag) {
                        sun.pagelet[nameSpace[0]][nameSpace[1]].init();
                    }
                }
            }
        }
    }

    return {
        init: function () {
            //setting value
            me = this;
            $mainFrame = $('#mainFrame');

            //function
            loadSubmit();

            //default show the home pagelet
            me.loadPage('main.test', '首页');
        },
        loadPage: function (sPageUrl, sPageName, oParameter) {
            var _data = sun.ajax.getPage(sPageUrl, oParameter),
                reg = /\w*[\.]\w*/i;

            $mainFrame.html(_data);

            //fire pagelet init
            if (reg.test(sPageUrl)) {
                var nameSpace = sPageUrl.split('.');

                firePageInit(nameSpace);
            }
        }
    }
}();

//leftFrame
sun.left = function () {
    var me = {},
        menuCache = sun.config.adminMenuCollection,
        $menuSections = null;

    function getMenu(sKey) {
        var template = '',
            $menuIdex = $('#menu_detail_index'),
            _url = 'http://' + window.location.host + '/api/SystemMenu/' + sKey,
            josn = {};

        if (!!menuCache[sKey]) {
            $('#menu_detail_index').html(menuCache[sKey]);
            return;
        }

        json = sun.ajax.getJSON(_url);

        if (json.Message === false) {
            return;
        }

        //为了防止数据返回的不正确导致的重复请求而缓存
        if (json.Data.length < 1) {
            //cache the content
            menuCache[sKey] = [];
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
                    icoPath = 'style/default/images/icons_' + icoType + '.png';
                    icoContent = '<div class="menu-detail-siteMenu-icon"><a href=\'javascript:void("' + icoUrl + '","' + icoTitle + '")\' title="' + icoTitle + '" target="_self"><img src="' + icoPath + '" alt="' + icoTitle + '" /></a></div>';
                }

                itemContent = itemContent + '<li>' + icoContent + '<div class="menu-detail-siteMenu-content"><a href=\'javascript:void("' + menuUrl + '","' + menuName + '")\' target="_self">' + menuName + '</a></div></li>';
            });

            template = template + '<dl class="menu-detail-subItem"><dt><b>' + itemName + '</b></dt><dd><ul class="menu-detail-siteMenu">' + itemContent + '</ul></dd></dl>';

            //cache the content
            menuCache[sKey] = template;
        })

        $('#menu_detail_index').html(template);
    }

    return {
        init: function () {
            $menuSections = $('#menu_sections > li'),
                selectedSection = $menuSections[0],
                subItem = $('#menu_detail_index');

            me = this;

            $menuSections.click(function (oEvent) {
                var me = $(this),
                    key = _.last(this.id.split('_'));

                $menuSections.removeClass('cur');
                selectedSection = me.addClass('cur')[0];

                //fill the menu content by key
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

                //loadPage to mainFrame
                sun.main.loadPage(url, name);
            })

            //set default menu
            getMenu('core');
        },
        selectMenu: function (sKey) {
            if (sKey) {
                getMenu(sKey)
            } else {
                $menuSections[0].click();
            }
        }
    }
}();

//footerFrame
sun.footer = function () {
    var me = {};
}();

sun.pagelet = sun.pagelet || { core: {}, global: {}, html: {}, member: {}, module: {}, system: {}, templet: {} };

sun.pagelet.system.information = function () {

    return {
        init: function () {
            var aList = $('.box .list').children("dd:odd");

            aList.addClass("even");
        },
        showConfig: function (sKey, sName) {
            var $list = null;

            if (!sKey) {
                return;
            }

            $list = $('#' + sKey);

            if ($list.length > 0) {
                $('.box > dl').hide();
                $list.show();
                $('.box > .title-menu').html('-&nbsp;' + sName + '&nbsp;-');
            }
        }
    }
}();