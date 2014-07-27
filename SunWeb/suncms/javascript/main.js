//  全局变量
var goto, tojs,
    vmGlobal;

$(document).ready(function() {
    sun.ajax.getJSON(sun.url.core.getMainURL(), function(json) {
        if (sun.util.checkJSON(json)) {
            return;
        }
        vmGlobal = json;

        sun.header.init();
        sun.left.init();
        sun.main.init();
    })

    //sun.main.init();
    //sun.header.init();

    //shortcut ==> sun.main.loadPage()
    //放在这里可能 IE7下可能失效
    goto = function(sPageUrl, sPageName, oParameter) {
        sun.main.getPage(sPageUrl, sPageName, oParameter)
    };


    tojs = function(argument) {
        return ko.mapping.toJS(argument);
    };

    log = function() {
        console.log(arguments);
    };

    logtojs = function(argument) {
        log(tojs(argument));
    };
});

var sun = sun || {};

//headerFrame
sun.header = function() {
    var me = null;

    function toggleAllMenu() {
        var $allMenu = $('#toggleMenu'),
            $leftFrame = $('#leftFrame');

        $allMenu.toggle(function() {
                $leftFrame.hide();
                $allMenu.text('显示菜单');
            },
            function() {
                $leftFrame.show();
                $allMenu.text('隐藏菜单');
            })
    }

    function loadAllMenu() {
        var $triger = $('#allMenu'),
            $pageMask = $('#pageMask'),
            $allMenu = $("#pageMask_allMenu_box");

        $triger.click(function() {
            $pageMask.show();
            $allMenu.show();
        })

        $pageMask.click(function() {
            $pageMask.hide();
            $allMenu.hide();
        })

        $allMenu.mouseup(function() {
            $pageMask.click();
        })
    }

    return {
        init: function() {
            me = null;
            //toggle the main menu
            toggleAllMenu();

            loadAllMenu();
        },
        openHome: function() {
            this.selectCoreMenu();
            sun.main.loadPage('main.home', '系统首页');
        },
        selectCoreMenu: function() {
            sun.left.selectMenu();
        }
    }
}();

//mainFrame
var firstOpenPage = 'core_channel';
sun.main = function() {
    var me = null,
        $mainFrame;

    function firePage(nameSpace, oParameter) {
        var flag = false,
            _module = null,
            _section = null,
            _length = nameSpace.length;

        if (_.isArray(nameSpace) && (nameSpace.length > 0)) {
            _.map(nameSpace, function(value, index) {
                switch (index) {
                    case 0:
                        if (( !! sun.pagelet) && (_.isObject(sun.pagelet[value]))) {
                            _module = value;
                        }
                        break;
                    case 1:
                        if (( !! _module) && (_.isObject(sun.pagelet[_module][value]))) {
                            _section = value;

                            if (_length === 2) {
                                if (_.has(sun.pagelet[_module][_section], 'index')) {
                                    sun.pagelet[_module][_section].index(oParameter);
                                } else {
                                    sun.pagelet[_module][_section](oParameter);
                                }
                            }
                        }
                        break;
                    case 2:
                        if (( !! _section) && (_.isFunction(sun.pagelet[_module][_section][value]))) {
                            if (_length === 3) {
                                sun.pagelet[_module][_section][value](oParameter);
                            }
                        };
                        break;
                }

            });
        };
    }

    return {
        init: function() {
            var $firstForm;

            //setting value
            me = this;
            $mainFrame = $('#mainFrame');

            //default show the home pagelet
            me.gotoPage(firstOpenPage + '.html');
        },
        gotoPage: function(sPageUrl, oData, fnCallBack) {
            if (sPageUrl.indexOf('http') > -1) {
                window.open(sPageUrl);
                return;
            }

            var _pageUrl,
                reg = /[a-zA-Z]+/gi,
                nameSpace, newNameSpace = [],
                $mainFrame = $('#mainFrame');

            if ((_.isFunction(oData)) && (!fnCallBack)) {
                fnCallBack = oData;
                oData = null;
            }

            if (reg.test(sPageUrl)) {
                nameSpace = sPageUrl.match(reg);
            } else {
                console.error('页面不存在或路径不正确');
                return;
            }

            sPageUrl = 'pagelet/' + sPageUrl;

            sun.ajax.getPage(sPageUrl, null, function(_data, state) {
                $mainFrame.html(_data);

                nameSpace = _.without(nameSpace, 'html');

                // namespace to lowercase
                _.map(nameSpace, function(k, v) {
                    newNameSpace.push(k.toLowerCase());
                })
                firePage(newNameSpace, oData);
            });
        }
    }
}();

//leftFrame
sun.left = function() {

    return {
        init: function() {
            //----------------   section: ko.observableArray(['核心', '模块', '', '', '', '', ''])
            var vmLeftModel = ko.mapping.fromJS(vmGlobal.data),
                vmCurrentMenus = null,
                vmDOMID = document.getElementById('menu_sections'),
                DOM_selectedSection;

            vmLeftModel.onSelectSection = function(vmodel, jqevt) {
                var _menu = ko.mapping.toJS(vmodel),
                    _cur = 'cur';

                if (!jqevt) {
                    vmDOMID.children[0].className = _cur;
                    DOM_selectedSection = vmDOMID.children[0];
                } else {
                    $(jqevt.srcElement.parentElement).children('li').removeClass(_cur);
                    jqevt.srcElement.className = _cur;
                    DOM_selectedSection = jqevt.srcElement;
                }
                if ( !! vmCurrentMenus) {
                    ko.mapping.fromJS(_menu, vmCurrentMenus);
                    return;
                }

                vmCurrentMenus = ko.mapping.fromJS(_menu);
                vmCurrentMenus.getImgUrl = function(icoType) {
                    var _type = icoType();

                    return sun.util.stringFormat('style/default/images/icons_{0}.png', _type)
                };
                vmCurrentMenus.onToggle = function(vmodel, jqevt) {
                    var _dl = $(jqevt.srcElement).parents('dl');

                    _dl.children('dd').slideToggle(100);
                };
                vmCurrentMenus.onOpenMain = function(vmodel, jqevt, isImg) {
                    var _url = null;
                    if ( !! isImg) {
                        _url = vmodel.icoUrl();
                    } else {
                        _url = vmodel.url();
                    }
                    log(_url);
                    sun.main.gotoPage(_url);
                };
                ko.applyBindings(vmCurrentMenus, document.getElementById('menu_detail_index'));
            };
            vmLeftModel.onLiHover = function(vmodel, jqevt) {
                var _li = jqevt.srcElement,
                    _cur = 'cur',
                    _hasCur = $(_li).hasClass(_cur);

                $(_li).addClass(_cur);
                if (!_hasCur) {
                    $(_li).removeClass('cur');
                }
                return;

                $(_li).hover(
                    function() {
                        log('before')
                        if (!_hasCur) {
                            $(_li).removeClass('cur');
                        }

                    },
                    function() {
                        $(_li).addClass(_cur);
                        log('after')
                    }
                )
            };
            vmLeftModel.onMouseOver = function(vmodel, jqevt) {
                jqevt.srcElement.className = 'cur';
            };
            vmLeftModel.onMouseOut = function(vmodel, jqevt) {
                if (jqevt.srcElement != DOM_selectedSection) {
                    jqevt.srcElement.className = '';
                }
            };

            ko.applyBindings(vmLeftModel, vmDOMID);
            //fire event
            vmLeftModel.onSelectSection(vmLeftModel.menu()[0]);
            //----------/vm------------
        }
    }
}();

//footerFrame
sun.footer = function() {
    var me = {};
}();

// 注意 类名 再多也要全部使用小写
sun.pagelet = sun.pagelet || {
    core: {},
    domain: {},
    global: {},
    html: {},
    member: {},
    module: {},
    system: {},
    templet: {}
};

sun.pagelet.util = function() {
    var sun_editor = null;

    return {
        removeTR: function($tr) {
            $tr.fadeOut().queue(function() {
                $tr.remove();
            });
        },
        //include start
        // editor 加载内容
        // fnCallback?
        setEditor: function(domID, fnCallback) {
            if (!domID) {
                domID = 'include_editor';
            }

            // 关闭过滤模式，保留所有标签
            KindEditor.options.filterMode = false;
            KindEditor.ready(function(K) {
                sun_editor = K.create('#' + domID, {
                    width: '99.5%',
                    height: '500px',
                    afterChange: function() {},
                    afterCreate: function() {},
                    //fillDescAfterUploadImage: true
                });

                if (typeof fnCallback === 'function') {
                    fnCallback()
                }
            });

            //editor.render(domID);

            // editor.addListener("afterSetContent ",function(type,data){
            //     if (typeof fnCallback === 'function') {
            //             $('#table_tab_content').ready(function () {
            //                 fnCallback()
            //             });

            //         }
            // })
        },
        getEditor: function(domID) {
            if (!domID) {
                domID = 'include_editor';
            }

            var _text = $('#' + domID);

            if ( !! _text) {
                return sun_editor.html();
            }
            return '';
        },
        setPickCalender: function(domID) {
            if (!domID) {
                domID = 'include_pickDate';
            }

            Calendar.setup({
                inputField: domID,
                ifFormat: "%Y-%m-%d %H:%M:%S",
                showsTime: true,
                timeFormat: "24"
            });
        },
        getPickCalender: function(domID) {
            if (!domID) {
                domID = 'include_pickDate';
            }

            return $('#' + domID).val().trim();
        },
        setColorPicker: function(domID) {
            var jqdom = null;

            if (!domID) {
                domID = 'include_pickerColor';
            }
            jqdom = $('#' + domID);

            jqdom.on('change', function() {
                var _v = this.value;

                if (!_v) {
                    jqdom.parents('td:first').css('backgroundColor', '#fff');
                }
            })

            if (jqdom.length > 0) {
                jqdom.ColorPicker({
                    color: '#' + jqdom.val(),
                    onShow: function(colpkr) {
                        $(colpkr).show();
                        return false;
                    },
                    onBeforeShow: function() {
                        $(this).ColorPickerSetColor(this.value);
                    },
                    onHide: function(colpkr) {
                        $(colpkr).hide();
                        return false;
                    },
                    onChange: function(hsb, hex, rgb) {
                        $(this).css('backgroundColor', '#' + hex);

                        jqdom.val(hex);
                        jqdom.parents('td:first').css('backgroundColor', '#' + hex);
                    }
                }).bind('keyup', function() {
                    var hex = this.value;

                    $(this).ColorPickerSetColor(hex);
                    jqdom.parents('td:first').css('backgroundColor', '#' + hex);
                });
            }
        },
        getColorPicker: function(domID) {
            if (!domID) {
                domID = 'include_pickerColor';
            }

            return $('#' + domID).val().trim();
        }
        //include end
    }
}();

sun.pagelet.core.channel = function() {
    var _addUrl = 'core_channel_add.html',
        _editUrl = 'core_channel_edit.html',
        _moveUrl = 'core_channel_move.html',
        _indexUrl = 'core_channel.html';

    var _sum = 0,
        arr_channel = arr_path = _step = [],
        __flag = false;

    function f_tab(DOMid, elm) {
        var _tabs = $('.sun-edit');

        _tabs.hide();
        $('.sun-edit' + '#' + DOMid).show();

        $(elm).siblings('li').removeClass('cur');
        $(elm).addClass('cur');
    }
    
    function _parseChannel(item, nCurrentChannelID) {
        var _tag = '';
        _sum = _sum || 0;

        if (_sum > 0) {
            var i = _sum;

            _tag = '';
            for (i; i > 0; i--) {
                _tag = '..../' +  _tag;
            }
        }

        if (!__flag) {
             arr_path.push(item.subject)
        }

        item.subject = '/' + _tag + item.subject;

        if (!!nCurrentChannelID) {
            if (item.id != nCurrentChannelID){
                arr_channel.push(item);
            } else {
                __flag = true;

                return arr_channel;
            }
        } else {
            arr_channel.push(item);
        }

        if (item.children.length > 0){
            _sum++;

            for (var i = item.children.length - 1; i > 0; i--) {
                _step.push(_sum);
            };
            
            _.each(item.children, function(v, i){
                _parseChannel(v, nCurrentChannelID)
            })
        } else {
            _sum = _.last(_step);
            _step.pop();
            if (!__flag) {
                arr_path.pop();
            }
        }
    }

    return {
        index: function() {
            var _url = sun.url.core.getChannel();

            function parseSibling(_table, x) {
                var x = x || 1,
                    _siblingTab = [],
                    _firstTab = $(_table).find('table:first'),
                    _otherTab = _firstTab.find('~table');

                if (_firstTab.length > 0) {
                    _siblingTab.push(_firstTab);
                }
                if (_otherTab.length > 0) {
                    _siblingTab = _siblingTab.concat(_otherTab);
                }
                if (_siblingTab.length > 0) {
                    _.each(_siblingTab, function(v, i) {
                        v.css('text-indent', x * 2 + 'em');
                    })
                    x++;
                    _.each(_siblingTab, function(v, i) {
                        parseSibling(v, x)
                    })
                }
            }

            sun.ajax.getJSON(_url, function(json) {
                var vmChannel = ko.mapping.fromJS(json.data),
                    _EXPOSE_FLAG = false;

                vmChannel.onExpose = function(vmodel, jqevt) {
                    var dom = $(jqevt.srcElement),
                        domChild = dom.parents('tr:first').siblings('tr:first');

                    dom.toggleClass('exp');
                    domChild.slideToggle();
                };
                vmChannel.onExposeAll = function(vmodel, jqevt) {
                    var tabList = $('#channel_box > table');

                    if (!!_EXPOSE_FLAG) {
                        $(jqevt.srcElement).text('[展开全部]');
                    } else {
                        $(jqevt.srcElement).text('[全部折叠]');
                    }
                    
                    _.each(tabList, function(v, i){
                        if (!!_EXPOSE_FLAG) {
                            $(v).find('tr:odd').hide();
                            $(v).find('span').removeClass('exp');
                        } else {
                            $(v).find('tr:odd').show();
                            $(v).find('span').addClass('exp');
                        }
                    })

                    _EXPOSE_FLAG = !_EXPOSE_FLAG;
                };
                vmChannel.gotoEditPage = function(vmodel, jqevt) {
                    sun.main.gotoPage(_editUrl,  { channel : vmChannel, currentChannel : vmodel });
                };
                vmChannel.gotoAddPage = function() {
                    sun.main.gotoPage(_addUrl);
                };
                vmChannel.gotoAddPageForChild = function(vmodel, jqevt) {
                    var _data = ko.mapping.toJS(vmodel);

                    sun.main.gotoPage(_addUrl, _data);
                };
                vmChannel.gotoMovePage = function(vmodel, jqevt){
                    sun.main.gotoPage(_moveUrl, { channel : ko.mapping.toJS(vmChannel.channel), currentChannel : ko.mapping.toJS(vmodel)})
                };
                vmChannel.gotoArchive = function(vmodel, jqevt) {
                    sun.main.gotoPage('core_archive.html', ko.mapping.toJS(vmodel));
                };
                vmChannel.parseChild = function(vmChildren) {
                    var children = vmChildren();

                    if (!children) {
                        children = [];
                    };

                    return children;
                };
                vmChannel.onDelete = function(vmodel, jqevt) {
                    var _id = vmodel.channelId();

                    sun.ajax.getJSON(sun.url.core.getChannel_delete(_id), function(odata) {
                        var _tb = $(jqevt.srcElement).parents('table').first();
                        sun.pagelet.util.removeTR(_tb);
                    })
                };
                ko.applyBindings(vmChannel, document.getElementById('vm_core_channel'));

                // 必须在 bind之后执行，否则抓不到 table这个dom
                var tabList = $('#channel_box > table');
                _.each(tabList, function(v, i) {
                    parseSibling(v);
                })
            })
            //end ajax
        },
        // vm => { channel : ko.observableArray, currentChannel : ko.obervable }
        edit: function(vm) {
            var vmodel = vm.currentChannel;

            vmodel.funcTab = f_tab;
            vmodel.parseChannelPath = function() {
                var _list = ko.mapping.toJS(vm.channel.channel),
                    _curChannel = ko.mapping.toJS(vm.currentChannel),
                    t_path = '';

                arr_channel = arr_path = _step = [];
                __flag = false;

                _.find(_list, function(v, i){
                    _sum = 0;
                    _step = [];
                    arr_path = [];

                    _parseChannel(v, _curChannel.id);

                    if (!__flag) {
                        arr_path = [];
                    }
                    if (__flag === true) {
                        return v;
                    }
                })
                
                arr_path.pop();
                if (arr_path.length > 0) {
                    _.each(arr_path, function(v ,i) {
                        t_path = t_path + '/' + v;
                    })
                } else {
                    t_path = '/'
                }

                return t_path;
            };

            vmodel.onGoback = function() {
                sun.main.gotoPage(_indexUrl)
            };
            vmodel.onUpdate = function(_vmodel, jqevt) {
                _vmodel.body(sun.pagelet.util.getEditor());

                var _odata = ko.mapping.toJS(_vmodel);

                sun.ajax.post(sun.url.core.getChannel_edit(), _odata, function(odata) {
                    console.log(odata);
                })
            };
            ko.applyBindings(vmodel, document.getElementById('vm_core_channel_edit'))

            sun.pagelet.util.setEditor('include_editor', function() {
                $('#table_tab_content').hide()
            });
        },
        add: function(_data) {
            var _vm = {
                parentId: ko.observable(-1),
                channelType: ko.observable(1000),
                count: ko.observable(0),
                isHidden: ko.observable(false),
                code: ko.observable(""),
                subject: ko.observable(""),
                contentPath: ko.observable("/news/{yyyyMM}/"),
                index: ko.observable("index.html"),
                templateIndex: ko.observable("index_article.htm"),
                templateList: ko.observable("index_list.htm"),
                templateBody: ko.observable("index_content.htm"),
                bodyRule: ko.observable("{typedir}/list_{tid}_{page}.html"),
                listRule: ko.observable("{typedir}/list_{tid}_{page}.html"),
                seo: ko.observable(""),
                keywords: ko.observable(""),
                description: ko.observable(""),
                body: ko.observable(""),
                sort: ko.observable(30)
            };

            _vm.funcTab = f_tab;
            _vm.parseChannelPath = function() {

            };
            _vm.onGoback = function() {
                sun.main.gotoPage(_indexUrl);
            };
            _vm.onAdd = function(_vmodel, jqevt) {
                _vmodel.body(sun.pagelet.util.getEditor());

                var _odata = ko.mapping.toJS(_vmodel);

                if (!!_data) {
                    _odata.parentId =  _data.id;
                }

                sun.ajax.post(sun.url.core.getChannel_add(), _odata, function(odata) {

                    console.log(odata);
                });
            };

            if (!!_data) {
                this.getChannelPath(_data.id, function(_path) {
                    _vm.parseChannelPath = function() {
                        return _path
                    }

                    ko.applyBindings(_vm, document.getElementById('vm_core_channel_add'))

                    sun.pagelet.util.setEditor('include_editor', function() {
                        $('#table_tab_content').hide()
                    });
                })
            } else {
                _vm.parseChannelPath = function() {
                    return '/'
                }

                ko.applyBindings(_vm, document.getElementById('vm_core_channel_add'))

                sun.pagelet.util.setEditor('include_editor', function() {
                    $('#table_tab_content').hide()
                });
            }
        },
        // odata => { channel :[], currentChannel : {}}
        move: function(odata){
            arr_channel = arr_path = _step = [];
            __flag = false;

            _.each(odata.channel, function(v, i){
                _sum = 0;
                _step = [];
                if (!__flag) {
                    arr_path = [];
                }
                _parseChannel(v, odata.currentChannel.id);
            })

            var _vm = ko.mapping.fromJS({ 'channel' : arr_channel, 'currentChannel' : odata.currentChannel });

            _vm.channel.unshift({ subject: '/顶级栏目', id : -1 });
            _vm.path = function() {
                var t_path = '';

                _.each(arr_path, function(v ,i) {
                    t_path = t_path + '/' + v;
                })

                return t_path;
            };
            _vm.onGoback = function() {
                sun.main.gotoPage(_indexUrl)
            };
            _vm.onUpdate = function(_vmodel, jqevt) {
                var options = { 
                    id: _vmodel.currentChannel.id(), 
                    parentId: $('#dom_channel').val()
                };

                sun.ajax.post(sun.url.core.getChannel_edit(), options, function(odata) {
                    console.log(odata);
                })
            };
            ko.applyBindings(_vm, document.getElementById('vm_core_archive_move'))
        },
        getChannelLevel: function(fnCallback) {
            var _url = sun.url.core.getChannel();

            sun.ajax.getJSON(_url, function(json) {
                arr_channel = arr_path = _step = [];
                __flag = false;

                _.each(json.data.channel, function(v, i){
                    _sum = 0;
                    _step = [];
                    if (!__flag) {
                        arr_path = [];
                    }
                    _parseChannel(v);
                })

                if (_.isFunction(fnCallback)) {
                    fnCallback(arr_channel)
                }
            })
        },
        getChannelPath: function(currentChannelID, fnCallback) {
            var _url = sun.url.core.getChannel();

            sun.ajax.getJSON(_url, function(json) {
                arr_channel = arr_path = _step = [];
                __flag = false;

                _.find(json.data.channel, function(v, i){
                    _sum = 0;
                    _step = [];
                    arr_path = [];

                    _parseChannel(v, currentChannelID);

                    if (!__flag) {
                        arr_path = [];
                    }
                    if (__flag === true) {
                        return v;
                    }
                })

                var t_path = '';

                _.each(arr_path, function(v ,i) {
                    t_path = t_path + '/' + v;
                })
                
                if (_.isFunction(fnCallback)) {
                    fnCallback(t_path)
                }
            })  
        }
    }
}();

//单页
sun.pagelet.core.cutform = function() {
    var _addUrl = 'core_cutform_add.html',
        _editUrl = 'core_cutform_edit.html',
        _indexUrl = 'core_cutform.html';

    function gotoIndex() {
        sun.main.gotoPage(_indexUrl);
    }

    return {
        index: function() {
            sun.ajax.getJSON(sun.url.core.getCutformUrl(), function(odata) {
                var vmodel = ko.mapping.fromJS(odata.data);

                vmodel.gotoEditPage = function(vmodel, jqevt) {
                    if ( !! vmodel) {
                        vmodel.vmAdvertisementGroup = ko.observableArray(json.data.vmAdvertisementGroup);
                    }

                    sun.main.gotoPage(_editUrl, vmodel);
                };
                vmodel.onDelete = function(vmodel, jqevt) {

                };

                ko.applyBindings(vmodel, document.getElementById('vm_cuteform'));
            })
        },
        edit: function(vmodel) {
            var vmodel = {},
                DOM_tab_table_common = $('#table_tab_common'),
                DOM_tab_table_advanced = $('#table_tab_advanced');

            vmodel.onGoback = gotoIndex;


            vmodel.funcTab = function(DOMid) {
                var _DOM_cur = $('#' + DOMid),
                    _DOM_ul = _DOM_cur.parent();

                _DOM_ul.children('li').removeClass('cur');

                _DOM_cur.addClass('cur');

                if (DOMid === 'tab_common') {
                    DOM_tab_table_advanced.hide();
                    DOM_tab_table_common.show();
                } else {
                    DOM_tab_table_common.hide();
                    DOM_tab_table_advanced.show();
                }

            };

            ko.applyBindings(vmodel, document.getElementById('vm_core_cutform_edit'));
        }
    }
}();

sun.pagelet.core.archive = function() {
    var _addUrl = 'core_archive_add.html',
        _editUrl = 'core_archive_edit.html',
        _indexUrl = 'core_archive.html';

    function gotoIndex() {
        sun.main.gotoPage(_indexUrl);
    }

    function _navTab(DOMid) {
        var DOM_tab_table_common = $('#table_tab_common'),
            DOM_tab_table_advanced = $('#table_tab_advanced');

        var _DOM_cur = $('#' + DOMid),
            _DOM_ul = _DOM_cur.parent();

        _DOM_ul.children('li').removeClass('cur');

        _DOM_cur.addClass('cur');

        if (DOMid === 'tab_common') {
            DOM_tab_table_advanced.hide();
            DOM_tab_table_common.show();
        } else {
            DOM_tab_table_common.hide();
            DOM_tab_table_advanced.show();
        }
    }

    return {
        index: function(pOdata) {
            var options = null;

            if (!!pOdata) {  
                options = { groupid : pOdata.id };
            };

            sun.ajax.getJSON(sun.url.core.getArchiveURL(), options, function(odata) {
                var vmodel = ko.mapping.fromJS(odata.data);

                vmodel.gotoEditPage = function(vmodel, jqevt) {
                    sun.main.gotoPage(_editUrl, vmodel);
                };
                vmodel.gotoAddPage = function() {
                    sun.main.gotoPage(_addUrl);
                };
                vmodel.onDelete = function(vmodel, jqevt) {
                    var _id = vmodel.id();

                    sun.ajax.getJSON(sun.url.core.getArchive_delete(_id), function(odata) {
                        var _tr = $(jqevt.srcElement).parents('tr').first();

                        sun.pagelet.util.removeTR(_tr);
                    })
                };

                ko.applyBindings(vmodel, document.getElementById('vm_core_archive'));
            })
        },
        edit: function(vmodel) {
            var DOM_tab_table_common = $('#table_tab_common'),
                DOM_tab_table_advanced = $('#table_tab_advanced');

            vmodel.onGoback = gotoIndex;

            vmodel.funcTab = function(DOMid) {
                _navTab(DOMid)
            };
            vmodel.onUpdate = function(_vmodel, jqevt) {
                _vmodel.body(sun.pagelet.util.getEditor());
                _vmodel.time(sun.pagelet.util.getPickCalender());
                _vmodel.color(sun.pagelet.util.getColorPicker());

                var _odata = ko.mapping.toJS(_vmodel);

                sun.ajax.post(sun.url.core.getArchive_edit(), _odata, function(odata) {
                    console.log(odata);
                })
            };

            sun.pagelet.core.channel.getChannelLevel(function(arrChannel) {
                vmodel.channel = ko.observableArray(arrChannel);
                vmodel.channelSelected = ko.observableArray([ vmodel.groupID() ]);
                vmodel.channelChange = function(DOM_option) {
                    var jqOption = $(DOM_option.selectedOptions);

                    vmodel.groupID(jqOption.val()|0);
                };
                

                ko.applyBindings(vmodel, document.getElementById('vm_core_archive_add'));

                sun.pagelet.util.setPickCalender();
                sun.pagelet.util.setEditor();
                sun.pagelet.util.setColorPicker();
            });
        },
        add: function() {
            var _vm = {
                subject: ko.observable(""),
                shortSubject: ko.observable(""),
                tag: ko.observable(""),
                isRebuild: ko.observable(true),
                authority: ko.observable(1),
                clicked: ko.observable(_.random(1, 300)),
                source: ko.observable(""),
                author: ko.observable("admin"),
                time: ko.observable(sun.util.getCurrentTime()),
                groupID: ko.observable(1),
                groupName: ko.observable(""),
                sort: ko.observable(0),
                color: ko.observable(""),
                keyword: ko.observable(""),
                description: ko.observable(""),
                body: ko.observable("")
            };

            sun.pagelet.core.channel.getChannelLevel(function(arrChannel) {
                _vm.channel = ko.observableArray(arrChannel);
                _vm.channel.unshift({ subject : '请制定一个栏目', id : -1 });
                _vm.channelChange = function(DOM_option) {
                    var jqOption = $(DOM_option.selectedOptions);

                    _vm.groupID(jqOption.val()|0);
                };

                _vm.funcTab = function(DOMid) {
                    _navTab(DOMid)
                };
                _vm.onAdd = function(_vmodel, jqevt) {
                    _vmodel.body(sun.pagelet.util.getEditor());
                    _vmodel.time(sun.pagelet.util.getPickCalender());
                    _vmodel.color(sun.pagelet.util.getColorPicker());

                    var _odata = ko.mapping.toJS(_vmodel);

                    sun.ajax.post(sun.url.core.getArchive_add(), _odata, function(odata) {

                        console.log(odata);
                    });
                };


                _vm.onGoback = gotoIndex;
                ko.applyBindings(_vm, document.getElementById('vm_core_archive_add'));

                sun.pagelet.util.setPickCalender();
                sun.pagelet.util.setEditor();
                sun.pagelet.util.setColorPicker();
            })
        }
    }
}();

sun.pagelet.module.friendlink = function() {
    var __updateModel = null,
        validFlaged = true;

    // 更新数据库
    fnUpdate = function(options) {
        $.post('/sun/api/pagelet/module/friendlink/update.ashx', options, function() {
            //debugger;
        })
    };

    fnDelete = function(data, jqEvent) {
        if ( !! data && !! data.id) {
            $.post('/sun/api/pagelet/module/friendlink/Remove.ashx', {
                id: data.id
            }, function(data) {
                var callData = JSON.parse(data);
                if ( !! callData && !! callData.State) {
                    if ( !! jqEvent && !! jqEvent.target) {
                        $(jqEvent.target).parents('tr:first').hide('slow');
                    };
                } else {
                    alert('删除失败, 错误信息为：' + callData.MessageDetail);
                }
            })
        }
    };

    funcEdit = function(data) {
        if ((!oParameter) && (!oParameter.id)) {
            return false
        };

        sun.ajax.getJSON('/sun/api/pagelet/module/friendlink/retrievebyid.ashx', {
            id: oParameter.id
        }, function(data) {
            var vm = ko.mapping.fromJS(data);
            vm.data.FriendLink.Subject = ko.dependentObservable({
                read: function() {
                    return this.data.FriendLink.Subject()
                },
                write: function(value) {
                    var _input = $('#FriendLink_Subject'),
                        _errorInfo = _input.next();

                    if (_.isEmpty(value)) {
                        _input.addClass('error');
                        _errorInfo.show();
                        validFlaged = false;
                    } else {
                        _input.removeClass('error');
                        _errorInfo.hide();
                        validFlaged = true;
                        this.data.FriendLink.Subject = value;
                    }
                }
            }, vm);

            vm.fnUpdate = function() {
                var data = ko.mapping.toJS(this);
                if ( !! data.data.FriendLink) {
                    fnUpdate(data.data.FriendLink);
                } else {
                    alert('有信息填写不正确，请检查');
                }
            }

            __updateModel = vm;

            ko.applyBindings(vm, document.getElementById('friendlink_edit'));
        });
    };

    return {
        index: function() {
            var _url = sun.url.module.getFriendlink();

            sun.ajax.getJSON(_url, function(json) {
                var vmlFriendlink = ko.mapping.fromJS(json);

                vmlFriendlink.data.friendLinkGroup.unshift({
                    id: ko.observable('-1'),
                    Subject: ko.observable("全部")
                });

                //event
                vmlFriendlink.gotoEditPage = function(vmodel, evt) {
                    if ( !! vmodel) {
                        vmodel.friendLinkGroup = ko.observableArray(json.data.friendLinkGroup);
                    }

                    sun.main.gotoPage('module_friendlink_edit.html', vmodel);
                };

                vmlFriendlink.gotoAddPage = function() {
                    sun.main.gotoPage('module_friendlink_add.html');
                }

                vmlFriendlink.parseToWord = function(argument) {
                    var _temp = argument();

                    return !!_temp === true ? '是' : '否';
                };

                vmlFriendlink.onDelete = function(vmodel, jqevt) {
                    sun.ajax.getJSON(sun.url.module.getFriendlinkDelete(), {
                        id: vmodel.id()
                    }, function(odata) {
                        var _tr = $(jqevt.srcElement).parents('tr').first();

                        sun.pagelet.util.removeTR(_tr);
                    })
                }

                __updateModel = vmlFriendlink;

                ko.applyBindings(vmlFriendlink, document.getElementById('vm_friendlink'));
            });
        },
        search: function(evt) {
            var param = {};

            if ( !! $('#friend_groupID')) {
                param.GroupID = $('#friend_groupID').val();
                param.subject = $('#friend_subject').val();
            }

            $.getJSON('/sun/api/pagelet/module/friendlink/retrieve.ashx', param, function(data) {
                ko.mapping.fromJS(data, {
                    'ignore': ['data.friendLinkGroup']
                }, __updateModel);
            });
        },
        edit: function(vmodel) {
            vmodel.onUpdate = function(vmodel, jqevt) {
                var _data = ko.mapping.toJS(vmodel);

                //_data = _.omit(_data, 'friendLinkGroup');

                log(_data.GroupID);
                return;

                sun.ajax.post(sun.url.module.getFriendlinkEdit(), _data, function(json) {
                    log(json);
                });
            };
            vmodel.onGoback = function() {
                sun.main.gotoPage('module_friendlink_index.html');
            };

            ko.applyBindings(vmodel, document.getElementById('vm_friendlinkEdit'))
        },
        add: function() {
            sun.ajax.getJSON(sun.url.module.getFriendLinkGroup(), function(json) {
                var _vm = {
                    Subject: ko.observable(''),
                    SiteUrl: ko.observable(''),
                    SortNum: ko.observable(0),
                    LogoUrl: ko.observable(''),
                    Description: ko.observable(''),
                    GroupID: ko.observable(-1),
                    IsEnable: ko.observable(true),
                    friendLinkGroup: ko.observableArray(json.data.friendLinkGroup)
                };

                _vm.onSubmit = function(vmodel, jqevt) {
                    sun.ajax.post(sun.url.module.getFriendlinkAdd(), ko.mapping.toJS(vmodel), function(data) {
                        log(data);
                    })
                };
                _vm.onGoback = function() {
                    sun.main.gotoPage('module_friendlink_index.html');
                };

                ko.applyBindings(_vm, document.getElementById('vm_friendlinkAdd'))
            })
        }
    }
}();
sun.pagelet.module.friendlinkgroup = function() {

    return {
        index: function() {
            sun.ajax.getJSON(sun.url.module.getFriendLinkGroup(), function(json) {
                var vmFriendlinkGroup = ko.mapping.fromJS(json.data);

                vmFriendlinkGroup.gotoEditPage = function(vmodel, jqevt) {
                    sun.main.gotoPage('module_friendLinkGroup_edit.html', vmodel);
                };
                vmFriendlinkGroup.gotoAddPage = function(argument) {
                    sun.main.gotoPage('module_friendLinkGroup_add.html');
                };
                vmFriendlinkGroup.onDelete = function(vmodel, jqevt) {
                    var _id = vmodel.id();

                    sun.ajax.getJSON(sun.url.module.getFriendLinkGroupDelete(_id), function(odata) {
                        var _tr = $(jqevt.srcElement).parents('tr').first();

                        sun.pagelet.util.removeTR(_tr);
                    })
                }

                ko.applyBindings(vmFriendlinkGroup, document.getElementById('vm_friendlinkgroup'));
            });
        },
        edit: function(vmodel) {
            vmodel.onUpdate = function(_vmodel, jqevt) {
                var _options = ko.mapping.toJS(_vmodel);

                sun.ajax.post(sun.url.module.getFriendLinkGroupEdit(), _options, function(odata) {
                    log(odata);
                });
            };
            vmodel.onGoback = function() {
                sun.main.gotoPage('module_friendLinkGroup_index.html');
            };

            ko.applyBindings(vmodel, document.getElementById('vm_friendlinkgroupEdit'));
        },
        add: function() {
            var _vm = {
                subject: ko.observable(''),
                description: ko.observable('')
            };

            _vm.onSubmit = function(vmodel, jqevt) {
                sun.ajax.post(sun.url.module.getFriendLinkGroupAdd(), ko.mapping.toJS(vmodel), function(data, textStatus, xhr) {
                    log(data)
                });

            };
            _vm.onGoback = function() {
                sun.main.gotoPage('module_friendLinkGroup_index.html');
            };

            ko.applyBindings(_vm, document.getElementById('vm_friendlinkgroupAdd'))
        }
    }
}();
sun.pagelet.module.advertisement = function() {

    function gotoIndex() {
        sun.main.gotoPage('module_advertisement_index.html');
    }

    return {
        index: function() {
            sun.ajax.getJSON(sun.url.module.getAdvertisement(), function(json) {
                var vmAdvertisement = ko.mapping.fromJS(json.data);

                vmAdvertisement.gotoAddPage = function() {
                    sun.main.gotoPage('module_advertisement_add.html');
                };
                vmAdvertisement.gotoEditPage = function(vmodel, jqevt) {
                    if ( !! vmodel) {
                        vmodel.advertisementGroup = ko.observableArray(json.data.advertisementGroup);
                    }

                    sun.main.gotoPage('module_advertisement_edit.html', vmodel);
                };
                vmAdvertisement.onDelete = function(vmodel, jqevt) {
                    var _id = vmodel.id();

                    sun.ajax.getJSON(sun.url.module.getAdvertisementDelete(_id), function(odata) {
                        var _tr = $(jqevt.srcElement).parents('tr').first();

                        sun.pagelet.util.removeTR(_tr);
                    })
                };


                ko.applyBindings(vmAdvertisement, document.getElementById('vm_advertisement'));
            });
        },
        edit: function(vmodel) {
            vmodel.onGoback = gotoIndex;
            vmodel.onUpdate = function(vmodel, jqevt) {
                sun.ajax.post(sun.url.module.getAdvertisementEdit(), ko.mapping.toJS(vmodel), function(odata) {

                    log(odata)
                })
            };

            ko.applyBindings(vmodel, document.getElementById('vm_advertisementEdit'));
        },
        add: function() {
            sun.ajax.getJSON(sun.url.module.getAdvertisementGroup(), function(json) {
                var _vm = {
                    subject: ko.observable(''),
                    siteUrl: ko.observable(''),
                    detail: ko.observable(''),
                    startTime: ko.observable(''),
                    isEnable: ko.observable(true),
                    groupID: ko.observable(-1),
                    groupName: ko.observable(''),
                    startTime: ko.observable(''),
                    advertisementGroup: ko.observableArray(json.data.advertisementGroup)
                };

                _vm.onGoback = gotoIndex;
                _vm.onAdd = function(vmodel, jqevt) {
                    sun.ajax.post(sun.url.module.getAdvertisementAdd(), ko.mapping.toJS(vmodel), function(odata) {
                        console.log(odata);
                    })
                };
                ko.applyBindings(_vm, document.getElementById('vm_advertisementAdd'));
            })
        }
    }
}();
sun.pagelet.module.advertisementgroup = function() {
    var _addUrl = 'module_advertisementgroup_add.html',
        _editUrl = 'module_advertisementgroup_edit.html',
        _indexUrl = 'module_advertisementgroup_index.html';

    function gotoIndex() {
        sun.main.gotoPage(_indexUrl);
    }

    return {
        index: function() {
            sun.ajax.getJSON(sun.url.module.getAdvertisementGroup(), function(json) {
                var vmAdvertisementGroup = ko.mapping.fromJS(json.data);

                vmAdvertisementGroup.gotoAddPage = function() {
                    sun.main.gotoPage(_addUrl);
                };
                vmAdvertisementGroup.gotoEditPage = function(vmodel, jqevt) {
                    if ( !! vmodel) {
                        vmodel.vmAdvertisementGroup = ko.observableArray(json.data.vmAdvertisementGroup);
                    }

                    sun.main.gotoPage(_editUrl, vmodel);
                };
                vmAdvertisementGroup.onDelete = function(vmodel, jqevt) {
                    var _id = vmodel.id();

                    sun.ajax.getJSON(sun.url.module.getAdvertisementGroupDelete(_id), function(odata) {
                        var _tr = $(jqevt.srcElement).parents('tr').first();

                        sun.pagelet.util.removeTR(_tr);
                    })
                };


                ko.applyBindings(vmAdvertisementGroup, document.getElementById('vm_AdvertisementGroup'));
            });
        },
        edit: function(vmodel) {

            vmodel.onGoback = gotoIndex;
            vmodel.onUpdate = function(_vmodel, jqevt) {
                sun.ajax.post(sun.url.module.getAdvertisementGroupEdit(), ko.mapping.toJS(_vmodel), function(odata) {
                    console.log(odata);
                })
            };

            ko.applyBindings(vmodel, document.getElementById('vm_AdvertisementGroupEdit'));
        },
        add: function() {
            var _vm = {
                subject: ko.observable(''),
                description: ko.observable('')
            };

            _vm.onAdd = function(vmodel, jqevt) {
                sun.ajax.post(sun.url.module.getAdvertisementGroupAdd(), ko.mapping.toJS(vmodel), function(odata) {

                    console.log(odata);
                });
            };

            _vm.onGoback = gotoIndex;
            ko.applyBindings(_vm, document.getElementById('vm_AdvertisementGroupAdd'));
        }
    }
}();
sun.pagelet.system = function() {

    return {
        base: function() {
            sun.ajax.getJSON(sun.url.system.getBaseUrl(), function(odata) {
                var vmodel = ko.mapping.fromJS(odata.data);

                var _nav_dl = $('#vm_base .box > dl');
                vmodel.onNavigation = function(_key, jqevt) {
                    _nav_dl.hide();

                    var one = _nav_dl.filter('#' + _key);
                    if (one.length > 0) {
                        one.show();
                    }
                };

                ko.applyBindings(vmodel, document.getElementById('vm_base'));
            })
        }

    }
}();