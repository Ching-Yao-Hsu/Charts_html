<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgCharts_callAjax.aspx.cs" Inherits="Charts.OrgCharts_callAjax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/org.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script type="text/JavaScript" src="js/org.js"></script>
    <script type="text/JavaScript">
        $(document).ready(function () {
            function OrgCharts_recursion(e) {
                var table = [];
                var strArray = [];
                var bool;
                var booltable;
                var count = 0;

                for (i = e.length - 1; i >= 0; i--) {
                    bool = false;
                    strArray[i] = e[i]['id'].split('-');
                    strArray[i].pop();
                    for (j = i - 1; j >= 0; j--) {
                        bool = (strArray[i].join('-') !== e[j]['id']) ? true : false;
                        if (!bool)
                            break;
                    }

                    if (bool) {
                        if (table.length === 0) {
                            table.push({ id: strArray[i].join('-') });
                        } else {
                            booltable = true;
                            for (k = 0; k < table.length; k++) {
                                booltable = (strArray[i].join('-') === table[k]['id']) ? false : true;
                            }
                            if (booltable && strArray[i].join('-') != '') {
                                table.push({ id: strArray[i].join('-') });
                            }
                        }
                    }
                }

                for (i = 0; i < table.length; i++) {
                    e.push(table[i]);
                }

                return table;
            }

            function recursion(e) {
                var check = true;
                return function (table) {

                    if (!check) {                        
                        return table;
                    } else {

                        var newtable = [];
                        var strArray = [];
                        var bool;
                        var booltable;

                        for (i = 0; i < table.length; i++) {
                            strArray[i] = table[i]['id'].split('-');
                            strArray[i].pop();

                            for (j = 0; j < e.length; j++) {
                                bool = (strArray[i].join('-') === e[j]['id']) ? false : true;
                                if (!bool) {
                                    break;
                                }
                            }

                            if (bool) {
                                if (newtable.length === 0 && strArray[i].join('-') != '') {
                                    newtable.push({ id: strArray[i].join('-') });                                    
                                } else {
                                    booltable = true;
                                    for (k = 0; k < newtable.length; k++) {
                                        booltable = (strArray[i].join('-') === newtable[k]['id']) ? false : true;
                                    }
                                    if (booltable && strArray[i].join('-') != '') {
                                        newtable.push({ id: strArray[i].join('-') });
                                    }
                                }
                            }
                        }

                        if (newtable.length === 0) {
                            check = false;
                        }
                        else {
                            for (i = 0; i < newtable.length; i++) {                                
                                e.push(newtable[i]);                                
                            }
                        }                        
                        return arguments.callee(newtable);
                    }
                };
            }


            //function recursion(e) {
            //    return function (check, num) {
            //        console.log(num);
            //        if (num <= 0) {
            //            check = true;
            //        }
            //        if (check) {
            //            return 1;
            //        } else
            //            return arguments.callee(check,num - 1);
            //    };
            //}

            //var result = recursion()(false, 6);
            //console.log(result);

            $.ajax({
                url: "OrgCharts_responseAjax.aspx",
                dataType: "json",
                type: "POST",
                success: function (e) {
                    var table = OrgCharts_recursion(e);
                    recursion(e)(table);                    
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
