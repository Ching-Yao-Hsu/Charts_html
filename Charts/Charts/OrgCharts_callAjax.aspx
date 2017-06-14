<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgCharts_callAjax.aspx.cs" Inherits="Charts.OrgCharts_callAjax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/org.css" rel="stylesheet" />
    <script type="text/JavaScript" src="scripts/jquery-3.1.1.min.js"></script>
    <script type="text/JavaScript" src="js/org.js"></script>
    <script type="text/JavaScript" src="js/recursion.js"></script>
    <script type="text/JavaScript">
        $(document).ready(function () {
            
            $.ajax({
                url: "OrgCharts_responseAjax.aspx",
                dataType: "json",
                type: "POST",
                success: function (e) {
                    var circle = new Circle();
                    var table = circle.OrgCharts_recursion(e);
                    circle.recursion(table);
                    $('#tree').EzOrgChart(e);
                    //e.splice(5, 0,
                    //    { id: "02" }
                    //);                    

                    //function SortByName(a, b) {
                    //    var aName = a.id.toLowerCase();
                    //    console.log(aName);
                    //    var bName = b.id.toLowerCase();
                    //    console.log(bName);
                    //    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
                    //}
                    //e.sort(SortByName);
                    
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
