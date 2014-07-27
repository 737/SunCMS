
<%@ Register Assembly="Sun.UI" Namespace="Sun.UI" TagPrefix="sun" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <sun:SunLiteral ID="ltTitle" Text="$login" runat="server"></sun:SunLiteral>
    </title>    
    <link href="style/suncms-base.css" rel="stylesheet" />
    <link href="style/suncms-style.css" rel="stylesheet" />
    <script type="text/javascript">
        if (top.location != self.location) {
            top.location.href = 'login.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%--<sun:Login ID="login" CssClass="mc" runat="server" ></sun:Login>--%>
    </form>
</body>
</html>
