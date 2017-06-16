<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgCharts_callAjax.aspx.cs" Inherits="OrgCharts_callAjax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/org.css" rel="stylesheet" />
    <style>
       
    </style>
    <script type="text/JavaScript" src="/js/jquery-1.8.2.min.js"></script>
    <script type="text/JavaScript" src="/js/org.js"></script>    
    <script type="text/JavaScript">

        $(document).ready(function () {           
            $.ajax({
                url: "OrgCharts_responseAjax.aspx",
                dataType: "json",
                type: "POST",
                success: function (e) {                                        
                    $('#tree').EzOrgChart(SortAndCreateNodeIntoTable(e));
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
        <div id="tree">

        </div>
    </form>
</body>
</html>
