
var sc = sc || {};

//headerFrame
sc.header = function () {
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

                sc.left.getMenu(authID);
            });
        }
    }
}();

//mainFrame
sc.main = function () {
    var me = {},
        $domMain,
        tabs,
        labelCollection = [];

    return {
        init: function () {
            me = this;

            $domMain = $('#' + sc.config.framesID.main);
            tabs = $domMain.tabs();

            //default show the home page
            me.addPage('home.aspx', '首页');
        },
        addPage: function (sTabContentPath, sTabLabel) {
            var $labels = $domMain.find('ul'),
                labelLength = $labels.find('li').length,
                 labelTemplate = "<li><a href='#{href}'>#{label}</a><span class='ui-icon ui-icon-close'>Remove Tab</span></li>",
                 tabID = "tabs-" + (labelLength + 1),
                 li = $(labelTemplate.replace(/#\{href\}/g, "#" + tabID).replace(/#\{label\}/g, sTabLabel)),
                 liIndex = _.indexOf(labelCollection, sTabLabel);

            if (liIndex > 0) {
                tabs.tabs("option", "active", liIndex);
            } else {
                labelCollection.push(sTabLabel);

                $.get(sTabContentPath, function (data) {
                    //如果是‘首页’则删除 cose icon                    
                    if (labelCollection.length === 1) {
                        labelTemplate = "<li><a href='#{href}'>#{label}</a></li>";
                        li = $(labelTemplate.replace(/#\{href\}/g, "#" + tabID).replace(/#\{label\}/g, sTabLabel));
                    } else {
                        // close icon: removing the tab on click
                        li.find('span').click(function () {
                            var me = $(this),
                                label = me.siblings().text(),
                                panelId = me.closest("li").remove().attr("aria-controls");

                            $("#" + panelId).remove();
                            tabs.tabs("refresh");

                            labelCollection = _.without(labelCollection, label);
                        })
                    }

                    $labels.append(li[0]);

                    $('#mainFrame_body').append("<div id='" + tabID + "'><p>" + data + "</p></div>");
                    //tabs.append("<div id='" + tabID + "'><p>" + data + "</p></div>");
                    tabs.tabs("refresh");
                }, 'html').done(function () {
                    tabs.tabs("option", "active", labelLength);
                });
            }
        }
    }
}();

//leftFrame
sun.left = function () {
    var me = {};

    return {
        init: function () {
            var indexID = 0; //home page

            me = this;
            me.getMenu(indexID);
        },
        getMenu: function (nID) {
            var parame = { auth: nID };

            $.get('frames/left.aspx', parame, function (data) {
                $('#' + sc.config.framesID.left).html(data);
            }, 'html').done(function () {
                var $dd = $('#left_tree dd');

                $dd.on('click', 'a', function (oEvent) {
                    var _this = $(this),
                        reg = /([^"]+[^"])/ig,
                        href = _this.attr('href').match(reg),
                        name = href[1],
                        content = href[2];

                    //addpage to mainFrame
                    sc.main.addPage(content, name);
                })
            })
        }

    }
}();

//footerFrame
sc.footer = function () {
    var me = {};


}();