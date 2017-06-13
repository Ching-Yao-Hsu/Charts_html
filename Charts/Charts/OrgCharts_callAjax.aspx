<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgCharts_callAjax.aspx.cs" Inherits="Charts.OrgCharts_callAjax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/org.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script type="text/JavaScript"  src="js/org.js"></script>
    <script type="text/JavaScript">
        $(document).ready(function () {
            function OrgCharts_recursion(e) {
                var table = [];
                var strArray = [];
                var bool;
                var count = 0;
                for (i = e.length - 1; i >= 0; i--) {
                    bool = true;
                    strArray[i] = e[i]['id'].split('-');
                    strArray[i].pop();
                    for (j = i - 1; j >= 0; j--) {
                        bool = (strArray[i].join('-') !== e[j]['id']) ? false : true;
                        if (bool)
                            break;
                    }

                    if (!bool) {
                        if (table.length === 0) {
                            table.push({ id: strArray[i].join('-') });
                            count++;
                        } else {
                            for (k = 0; k < count; k++) {
                                if (strArray[i].join('-') !== table[k] && strArray[i].join('-') != '') {
                                    table.push({ id: strArray[i].join('-') });
                                }
                            }
                        }
                    }
                }

                var aaa = '00-00';
                var bbb = aaa.split('-');
                console.log(bbb);
            }

            $.ajax({
                url: "OrgCharts_responseAjax.aspx",
                dataType: "json",
                type: "POST",                
                success: function (e) {

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

                    //var table = [];  
                    //var strArray = [];
                    //var bool;
                    //var count = 0;
                    //for (i = e.length - 1; i >= 0; i--) {                        
                    //    bool = true;
                    //    strArray[i] = e[i]['id'].split('-');
                    //    strArray[i].pop();
                    //    for (j = i - 1; j >= 0; j--) {
                    //        bool = (strArray[i].join('-') !== e[j]['id']) ? false : true;
                    //        if (bool)
                    //            break;
                    //    }                        

                    //    if (!bool) {
                    //        if (table.length === 0) {
                    //            table.push({ id: strArray[i].join('-') });
                    //            count++;
                    //        } else {
                    //            for (k = 0; k < count; k++) {
                    //                if (strArray[i].join('-') !== table[k] && strArray[i].join('-') != '') {
                    //                    table.push({ id: strArray[i].join('-') });
                    //                }
                    //            }
                    //        }                                                     
                    //    }                       
                    //}
                    
                    //for (i = 0; i < table.length ; i++) {
                    //    e.push(table[i]);
                    //}
                    //console.log(e);
                    //console.log(table);
                    OrgCharts_recursion(e);
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
