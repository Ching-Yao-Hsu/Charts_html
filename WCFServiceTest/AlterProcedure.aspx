<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AlterProcedure.aspx.vb" Inherits="AlterProcedure" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>電力分析系統</title>
    <link href="css/menu_slide.css" rel="stylesheet" type="text/css" />
    <link href="css/layout.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/alertify.min.js"></script>
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />

    <script type='text/javascript'>
	    function custom_script(sender) {
	        sure = alert(sender);
	        window.location = "index.aspx"
	    }
	</script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="all">
            <div id="header">
                <%--<div style="float: right; margin-top: 100px; margin-right: 0px;">
                    <asp:Button ID="logout_btn" runat="server" CausesValidation="False" Text="登出" />
                </div>--%>
            </div>

            <div id="content" align="center">
                <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" EnableScriptGlobalization="true"
                    EnableScriptLocalization="true" />
                <br />
                <br />
                <br />
                <div align="center">
                    <asp:Button ID="btnRun" runat="server" Text="執行" Height="35px" Width="100px" />
                </div>
                <br /><br /><br /><br /><br /><br />
            </div>
            <div id="FOOTER">
<%--                <p>
                    艾可智能科技有限公司 ECO SMART ENERGY TECH CO.,LTD All Rights Reserved.
                    <br />
                    32068 桃園縣中壢市中華路一段805巷22弄7號 TEL:03-4625590 FAX:03-4630690 &copy; 2013 版權所有
                    <br />
                </p>--%>
            </div>
        </div> 
    </form>
</body>
</html>
