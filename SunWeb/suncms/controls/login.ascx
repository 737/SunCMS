<script type="text/javascript">
    function EvaluateIsValid(e) {
        return true;
    }

</script>
<h1>登录</h1>
<ul>
    <li>
        <label>用户名</label>
        <sun:SunTextBox ID="txtUser" runat="server" Text="" TextStyle="Required"></sun:SunTextBox>
    </li>
    <li>
        <label>密码</label>
        <sun:SunTextBox ID="txtPwd" runat="server" TextMode="Password" TextStyle="Required"></sun:SunTextBox>
    </li>
    <li>
        <label>验证码</label>
        <sun:SunTextBox ID="txtValidateCode" runat="server" TextStyle="Required"></sun:SunTextBox>
        <sun:AuthCode ID="authCode" runat="server" ValidateControlId="txtValidateCode" />        
        <span>
            <asp:CustomValidator ID="loginValidator"  runat="server" ></asp:CustomValidator>
        </span>
    </li>
</ul>
<sun:SunButton ID="btnLogin" runat="server" Text="Login" />
