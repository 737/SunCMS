<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SunCMS Administrator Panel</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="style/default/suncms-base.css" rel="stylesheet" />
    <link href="style/default/suncms-style.css" rel="stylesheet" />
    <script type="text/javascript" src="javascript/lib/jquery-last.min.js"></script>
    <script type="text/javascript" src="javascript/lib/jquery-form.js"></script>
    <script type="text/javascript" src="javascript/lib/underscore-last.min.js"></script>
    <script type="text/javascript" src="javascript/lib/knockout-last.js"></script>
    <script type="text/javascript" src="javascript/lib/knockout.mapping-latest.debug.js"></script>

    <script type="text/javascript" src="include/ueditor/ueditor.all.js"></script>
    <script type="text/javascript" src="include/ueditor/ueditor.config.js"></script>

    <script charset="utf-8" src="include/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" src="include/kindeditor/lang/zh_CN.js"></script>


    <script charset="utf-8" src="include/ckeditor/ckeditor.js"></script>
    <script src="include/ckeditor/adapters/jquery.js"></script>


    <link href="include/calendar/calendar-green.css" rel="stylesheet" />
    <script type="text/javascript" src="include/calendar/calendar.js"></script>

    <link rel="stylesheet" media="screen" type="text/css" href="include/colorpicker/css/colorpicker.css" />
    
    <script type="text/javascript" src="include/colorpicker/js/colorpicker.js"></script>

    <script type="text/javascript" src="javascript/base.js"></script>
    <script type="text/javascript" src="javascript/main.js"></script>
</head>

<body class='cf'>
    <table id="main_wrap">
        <tbody>
            <!--header frame-->
            <tr>
                <td id="headerFrame" colspan="2">
                    <div class="head-box cf">
                        <div class="head-right">
                            <ul class="head-link cf">
                                <li class="welcome">您好：admin ，欢迎使用SunCMS！</li>
                                <li><a href="javascript:sun.header.selectCoreMenu()">主菜单</a></li>
                                <li><a href="javascript:void(0)">内容发布</a></li>
                                <li><a href="javascript:void(0)">内容维护</a></li>
                                <li><a href="javascript:sun.header.openHome()">系统主页</a></li>
                                <li><a href="../member" target="_blank">会员中心</a></li>
                                <li><a href="../" target="_blank">网站主页</a></li>
                                <li><a href="exit.aspx" target="_top">注销</a></li>
                            </ul>
                            <div id="head_shortcut" class="cf">
                                <p>快捷方式</p>
                                <ul style="display: none">
                                    <li><a href="javascript:void('global/base.aspx')" title="文档列表">文档列表</a></li>
                                    <li><a href="javascript:void('global/base.aspx')" title="文档列表">文档列表</a></li>
                                    <li><a href="javascript:void('global/base.aspx')" title="文档列表">文档列表</a></li>
                                    <li><a href="javascript:void('global/base.aspx')" title="文档列表">文档列表</a></li>
                                </ul>
                            </div>
                        </div>
                        <img src="style/default/images/logo_top.png" alt="SunCMS" />
                    </div>
                    <div class="head-ass cf">
                        <div class="menuact">
                            <a href="javascript:void(0)" title="" id="toggleMenu">隐藏菜单</a>
                            <a href="javascript:void(0)" title="" id="allMenu">系统地图</a>
                        </div>
                    </div>
                </td>
            </tr>
            <!--/header frame-->
            <tr>
                <!-- menu -->
                <td id="leftFrame">
                    <div id="menu_wrap" class="cf">
                        <ul id="menu_sections" data-bind='foreach: $root.menu'>
                            <li class='' data-bind='text: module, click: $root.onSelectSection, event: { mouseover: $root.onMouseOver, mouseout: $root.onMouseOut }'></li>
                        </ul>
                        <div id="menu_detail_wrap">
                            <!--menu item-->
                            <div id="menu_detail_index" class="menu-detail-section" data-bind='foreach: $root.children'>                                
                                <dl class="menu-detail-subItem" >
                                    <dt data-bind='click: $root.onToggle'><b data-bind='text: title'></b></dt>
                                    <dd>
                                        <ul class="menu-detail-siteMenu" data-bind='foreach: list'>
                                            <li>
                                                <div class="menu-detail-siteMenu-icon" data-bind='if: !!icoType()'>
                                                    <a data-bind='attr: {title: icoTitle}, click: function(vmodel, jqevt) {  $root.onOpenMain(vmodel, jqevt, true) }' target="_self">
                                                        <img data-bind='attr: {src: $root.getImgUrl(icoType), alt: icoTitle}'>
                                                    </a>
                                                </div>
                                                <div class="menu-detail-siteMenu-content"><a target="_self" data-bind='text: name, click: $root.onOpenMain'></a></div>
                                            </li>
                                        </ul>
                                    </dd>
                                </dl>
                            </div>
                            <!--/menu item-->
                        </div>
                    </div>
                    <!-- <div id="menu_scroll"></div> -->
                </td>
                <!-- /menu -->
                <!-- page -->
                <td id="mainFrame"></td>
                <!-- /page -->
            </tr>
            <tr>
                <td style="background-color: #F7F7F7;">&nbsp;</td>
                <td id="footerFrame">
                    <div id="footer_task" style="display: none;">
                        Powered by <a href="http://www.suncms.cn" target="_blank" title="SunCMS">SunCMS</a>
                        <span>
                            <img src="style/default/images/task_active.gif" alt="" /></span>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="pageMask" style="display: none;"></div>
    <div id="pageMask_allMenu_box" style="display: none;">
        under construction<br />
        <center><a href="http://wwww.suncms.cn" target="_blank">SunCMS</a></center>
    </div>

    <!-- 加载页面 -->
    <div id='global_loadding' style='dispaly: none'>
        <img src='style/default/images/loading_03.gif' />
        <span></span>
    </div>
    <!-- /加载页面 -->
</body>

</html>

