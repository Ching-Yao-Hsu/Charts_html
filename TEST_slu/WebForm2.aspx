<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Test.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/org.css" rel="stylesheet" />

    <style>        
        
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <%--<script src="orgDataTable.js"></script>--%>
    <script type="text/JavaScript"  src="js/org.js"></script>
    <script type="text/JavaScript">
        $(document).ready(function () {
            $.ajax({
                url: "http://localhost:52248/WebForm1.aspx",
                dataType: "json",
                type: "POST",
                async: true,
                success: function (e) {                    
                    $('#tree').EzOrgChart(e);
                },
                error: function () {
                    alert("error");
                }
            })
        });
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div id="tree"></div>
    </form>
</body>
</html>
