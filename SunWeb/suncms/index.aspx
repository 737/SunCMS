
<%@ Register Assembly="sun.ui" Namespace="sun.ui" TagPrefix="sun" %>
<%@ Register Assembly="sun.ui" Namespace="sun.ui" TagPrefix="sun" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>SunCMS Administrator Panel</title>
    <link href="style/default/suncms-base.css" rel="stylesheet" />
    <link href="style/default/suncms-style.css" rel="stylesheet" />
    <script type="text/javascript" src="style/javascript/lib/jquery-last.min.js"></script>
    <script type="text/javascript" src="style/javascript/lib/jquery-form.js"></script>
    <script type="text/javascript" src="style/javascript/lib/underscore-last.min.js"></script>
    <script type="text/javascript" src="style/javascript/base.js"></script>
    <script type="text/javascript" src="style/javascript/main.js"></script>
    <script type="text/javascript" language="javascript">
        var system = system || {};

        system.base = function () {

        }();
    </script>
</head>

<body>
    <%--<iframe id="hideIframe" name="hideIframe" frameborder="0" style="display:none;" src="../system/base.aspx"></iframe>--%>
    <%--<form name="form1" id="form1" method="post" target="hideIframe" action="page/system/base.aspx" >--%>
    <form name="myForm" id="myForm" method="post" action="">
        <table id="main_wrap">
            <tbody>
                <!--header frame-->
                <tr>
                    <td id="headerFrame" colspan="2">header is here.
                    </td>
                </tr>
                <!--/header frame-->
                <tr>
                    <td id="leftFrame">
                        <div id="menu_wrap" class="cf">
                            <ul id="menu_sections">
                                <li id="menuTag_core" class="cur">ºËÐÄ</li>
                                <li id="menuTag_module">Ä£¿é</li>
                                <li id="menuTag_html">Éú³É</li>
                                <li id="menuTag_collect ">²É¼¯</li>
                                <li id="menuTag_member">ÓÃ»§</li>
                                <li id="menuTag_templet">Ä£°å</li>
                                <li id="menuTag_system">ÏµÍ³</li>
                            </ul>
                            <div id="menu_detail_wrap">
                                <div id="menu_detail_index" class="menu-detail-section">
                                </div>
                            </div>
                        </div>
                        <%--<div id="menu_scroll">

                    </div>--%>
                    </td>
                    <!-- page -->

                    <td id="mainFrame">

                        <div class="page-system">
                            <!-- navigation -->
                            <div class="page-box">
                                <div class="title">
                                    SunCMSÏµÍ³ÅäÖÃ²ÎÊý£º
                                </div>
                                <ul class="menu">
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(1)">Õ¾µãÉèÖÃ</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(2)">ºËÐÄÉèÖÃ</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(3)">¸½¼þÉèÖÃ</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(4)">»áÔ±ÉèÖÃ</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(5)">»¥¶¯ÉèÖÃ</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(6)">ÐÔÄÜÑ¡Ïî</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(7)">ÆäËüÑ¡Ïî</a></li>
                                    <li><span>&iota;</span><a href="javascript:sun.page.system.base.showConfig(8)">Ä£¿éÉèÖÃ</a></li>
                                    <li><a href="javascript:sun.page.system.base.showConfig(27)">Ìí¼ÓÐÂ±äÁ¿</a></li>
                                </ul>
                            </div>
                            <!-- /navigation -->
                            <br />
                            <br />
                            <div class="page-box">
                                <div class="title-menu">
                                    --
                                </div>
                                <dl id="system_list" class="list">
                                    <dt class="label">
                                        <span class="name">²ÎÊýËµÃ÷</span>
                                        <span class="variable">±äÁ¿Ãû</span>
                                        <span class="value">²ÎÊýÖµ</span>
                                    </dt>
                                    <dd>
                                        <span class="name">²ÎÊýËµÃ÷</span>
                                        <span class="variable">±äÁ¿Ãû</span>
                                        <span class="value">²ÎÊýÖµ</span>
                                    </dd>
                                    <dd>
                                        <span class="name">²ÎÊýËµÃ÷</span>
                                        <span class="variable">±äÁ¿Ãû</span>
                                        <span class="value">²ÎÊýÖµ</span>
                                    </dd>
                                    <dd>
                                        <span class="name">²ÎÊýËµÃ÷</span>
                                        <span class="variable">±äÁ¿Ãû</span>
                                        <span class="value">²ÎÊýÖµ</span>
                                    </dd>
                                    <dd>
                                        <span class="name">²ÎÊýËµÃ÷</span>
                                        <span class="variable">±äÁ¿Ãû</span>
                                        <span class="value">²ÎÊýÖµ</span>
                                    </dd>
                                    <dd>
                                        <span class="name">²ÎÊýËµÃ÷</span>
                                        <span class="variable">±äÁ¿Ãû</span>
                                        <span class="value">²ÎÊýÖµ</span>
                                    </dd>
                                </dl>
                                <input type="text" name="edit___cfg_ddimg_width" id="edit___cfg_ddimg_width" value="240" style="width: 30%" />
                                <div class="submit">
                                    <%--<input type="image" src="style/default/images/button_ok.gif" />--%>
                                    <img src="style/default/images/button_ok.gif" onclick="sun.page.system.base.ok()" />
                                    <input type="image" src="style/default/images/button_back.gif" onclick="document.forms[0].reset();" />
                                </div>
                            </div>
                            <%--<script type="text/javascript" language="javascript">
                                $(document).ready(function () {
                                    sun.page.system.base.init("#system_list")
                                })

                            </script>--%>
                        </div>

                    </td>

                    <!-- /page -->
                </tr>
                <tr>
                    <td id="footerFrame" colspan="2">333333333</td>
                </tr>
            </tbody>
        </table>
    </form>
</body>

</html>

