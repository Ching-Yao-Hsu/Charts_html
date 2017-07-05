<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WCFService_Test.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/html_reset.css" rel="stylesheet" />
    <link href="css/WCF_index.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous" />
    <link href="css/org.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script type="text/JavaScript" src="js/org.js"></script>
    <script type="text/JavaScript" src="js/org_recursion.js"></script>

    <script>


        $(document).ready(function () {
            var node;
            var _ECO_Group_account;
            $.ajax({
                url: "server.aspx",
                dataType: "json",
                type: "POST",
                success: function (e) {                    
                    for (i = 0; i < e.length; i++) {
                        $("#ECO_Group").append(
                            $("<option/>")
                                .text(e[i]["ECO_Group"])
                                .attr("value", e[i]["Account"])
                        );
                    }
                },
                error: function () {
                    alert("error");
                }
            });
            
            $("#ECO_Group").change(function () {
                _ECO_Group_account = this.value;
                if (_ECO_Group_account != "") {
                    $.ajax({
                        url: "server.aspx",
                        dataType: "json",
                        data: {
                            ECO_Group_account: _ECO_Group_account
                        },
                        type: "POST",
                        success: function (e) {
                            $('#tree').empty();
                            var rec = new Ezrecursion();
                            var table = rec.init_recursion(e);
                            rec.recursion(e)(table);
                            function SortByName(a, b) {
                                var aName = a.id.toLowerCase();
                                var bName = b.id.toLowerCase();
                                return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
                            }
                            e.sort(SortByName);
                            $('#tree').EzOrgChart(e);

                            $("#tree li a").click(function () {
                                $.ajax({
                                    url: "server.aspx",
                                    dataType: "json",
                                    data: {
                                        node: $(this).data("node"),
                                        ECO_Group_account: _ECO_Group_account
                                    },
                                    type: "POST",
                                    success: function (e) {                                        
                                        $("#myModalLabel").text(e[0]["ECO_AccountAndMeterId"]);
                                    },
                                    error: function () {
                                        alert("error");
                                    }
                                })
                            });
                        },
                        error: function () {
                            alert("error");
                        }
                    })
                } else {
                    $('#tree').empty();
                }

            });
        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="WCF_index">
            <div class="header">
                <div class="title">
                    <div id="PowerValue">11111564656789</div>                    
                </div>
                <nav class="menu">
                    <table>
                        <tr>
                            <td></td>
                            <td class="option">及時電力數值</td>
                            <td></td>
                            <td class="option">電力數值分析</td>
                            <td></td>
                            <td class="option">電力比對分析</td>
                            <td></td>
                            <td class="option">個人資料設定</td>
                            <td></td>
                            <td class="option">管理者專區</td>
                            <td></td>
                        </tr>
                    </table>
                </nav>
            </div>

            <div class="content">
                <div class="top">
                    <select name="" id="ECO_Group">
                        <option value="">請選擇</option>
                    </select>
                </div>
                <div class="middle">

                    <table>
                        <tr></tr>
                        <tr>
                            <td>
                                <input type="checkbox" />
                                <strong>單線圖編號</strong></td>
                            <td>
                                <input type="checkbox" />
                                <strong>安裝位置</strong></td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" />
                                <strong>安裝位置</strong></td>
                            <td>
                                <input type="checkbox" />
                                <strong>安裝位置</strong></td>
                        </tr>
                    </table>
                    <input type="button" name="name" value="查詢" id="button_search" />

                    <%--<a href="javascript:;" id="button_search">查詢</a>--%>
                </div>

                <div id="treeview_block">
                    <div id="tree"></div>
                </div>

                <div class="botttom"></div>
            </div>

            <div class="footer"></div>
        </div>
    </form>
    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="height: 600px;">
                <div class="modal-header" style="height: 8%;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">電表編號 : <span id="myModalLabel"></span></h4>
                </div>
                <div class="modal-body" style="height: 82%;">
                    ...
                </div>
                <div class="modal-footer" style="height: 10%;">


                    <button type="button" class="btn btn-primary">月報表</button>
                    <button type="button" class="btn btn-primary">日報表</button>
                    <button type="button" class="btn btn-primary">查詢</button>
                    <button type="button" class="btn btn-primary">曲線圖</button>


                </div>
            </div>
        </div>
    </div>
</body>
</html>
