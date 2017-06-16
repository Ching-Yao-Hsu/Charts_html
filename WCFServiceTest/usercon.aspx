<%@ Page Language="C#" AutoEventWireup="true" CodeFile="usercon.aspx.cs" Inherits="usercon" %>

<%@ Register Src="~/WebUserControl.ascx" TagPrefix="uc1" TagName="WebUserControl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/JavaScript" src="/js/jquery-1.8.2.min.js"></script>
    <script type="text/JavaScript">
        console.log($('li').width());
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:WebUserControl runat="server" ID="WebUserControl" />
    </div>
    </form>
</body>
</html>
