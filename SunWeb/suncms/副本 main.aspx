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
    <script type="text/javascript" src="javascript/base.js"></script>
    <script type="text/javascript" src="javascript/main.js"></script>
</head>

<body>
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
                <td id="leftFrame">
                    <div id="menu_wrap" class="cf">
                        <ul id="menu_sections">
                            <li id="menuTag_core" class="cur">核心</li>
                            <li id="menuTag_module">模块</li>
                            <li id="menuTag_html">生成</li>
                            <li id="menuTag_collect ">采集</li>
                            <li id="menuTag_member">用户</li>
                            <li id="menuTag_templet">模板</li>
                            <li id="menuTag_system">系统</li>
                        </ul>
                        <div id="menu_detail_wrap">
                            <div id="menu_detail_index" class="menu-detail-section">
                            </div>
                        </div>
                    </div>
                    <%--<div id="menu_scroll"></div>--%>
                </td>
                <!-- page -->
                <td id="mainFrame">
                </td>
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
</body>

</html>

