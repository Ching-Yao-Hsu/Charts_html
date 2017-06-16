<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebUserControl.ascx.cs" Inherits="WebUserControl" %>



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

   <div id="tree"></div>
    
