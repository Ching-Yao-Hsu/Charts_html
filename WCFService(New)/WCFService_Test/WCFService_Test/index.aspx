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
    <link href="css/animate.css" rel="stylesheet" />
    <link href="css/org.css" rel="stylesheet" />
    <style>
        #myModal .modal-header {
            border-bottom: dotted 1px;
        }

        #myModal .modal-footer {
            border: 0;
        }
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script type="text/JavaScript" src="js/org.js"></script>
    <script type="text/JavaScript" src="js/org_recursion.js"></script>
    <style>
        #PowerValue1, #PowerValue2, #PowerValue3 {
            display: inline;
        }        
    </style>
    <script>
        
        

        $(document).ready(function () {  
            var count = 1;
            $("#AnimatedValue2").hide();
            $("#AnimatedValue3").hide();
            setInterval(function () {
                console.log(count);
                switch (count) {
                    case 0:
                        $("#AnimatedValue3").hide();  
                        $("#AnimatedValue1").show().addClass("rotateIn");
                        count++;
                        break;
                    case 1:
                        $("#AnimatedValue1").hide();
                        $("#AnimatedValue2").show().addClass("rotateIn");
                        count++;
                        break;
                    case 2:
                        $("#AnimatedValue2").hide();
                        $("#AnimatedValue3").show().addClass("rotateIn");
                        count = 0;
                        break;
                }
                setTimeout(function () {                    
                    $("#AnimatedValue1").removeClass("rotateIn");
                    $("#AnimatedValue2").removeClass("rotateIn");
                    $("#AnimatedValue3").removeClass("rotateIn");                  
                }, 5000);                   
            }, 6000);

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
                                        $("#myModalLabe2").text(e[0]["InstallPosition"]);
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
                    <div class="headerValue animated" id="AnimatedValue1">
                        <div id="PowerValue1">123</div>
                    </div>
                    <div class="headerValue animated" id="AnimatedValue2">
                        <div id="PowerValue2">456</div>
                    </div>
                    <div class="headerValue animated" id="AnimatedValue3">
                        <div id="PowerValue3">789</div>
                    </div>
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
                                <strong>電表編號</strong></td>
                            <td>
                                <input type="checkbox" />
                                <strong>ECO-5編號</strong></td>
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
        <div class="modal-dialog" role="document" style="width: 800px;">
            <div class="modal-content" style="height: 600px;">
                <div class="modal-header" style="height: 22%;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <table>
                        <tr>
                            <td>
                                <h4 class="modal-title"><strong>電表編號 :&nbsp;</strong><span id="myModalLabel"></span></h4>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h4 class="modal-title"><strong>安裝位置 :&nbsp;</strong><span id="myModalLabe2"></span></h4>
                            </td>
                        </tr>
                    </table>


                </div>
                <div class="modal-body" style="height: 38%; text-align: center;">
                    <table style="width: 100%; height: 100%;">
                        <tr style="height: 25%;">
                            <td colspan="11"></td>
                        </tr>
                        <tr style="height: 25%;">
                            <td colspan="1"></td>
                            <td colspan="1"><span>總電量 :</span></td>
                            <td colspan="2" class="Number"><span id="PowerTotal">321321313321</span></td>
                            <td colspan="1" style="width: 5px;"></td>
                            <td colspan="1" class="Unit"><span>KWH</span></td>
                            <td colspan="1"></td>
                            <td colspan="1"><span>電壓 :</span></td>
                            <td colspan="2" class="Number"><span id="Voltage">565465464654654</span></td>
                            <td colspan="1" style="width: 5px;"></td>
                            <td colspan="1" class="Unit"><span>V</span></td>
                            <td colspan="1"></td>
                        </tr>
                        <tr style="height: 25%;">
                            <td colspan="1"></td>
                            <td colspan="1"><span>功率 :</span></td>
                            <td colspan="2" class="Number"><span id="Power">6546546564</span></td>
                            <td colspan="1" style="width: 5px;"></td>
                            <td colspan="1" class="Unit"><span>KW</span></td>
                            <td colspan="1"></td>
                            <td colspan="1"><span>電流 :</span></td>
                            <td colspan="2" class="Number"><span id="Current">65464654654</span></td>
                            <td colspan="1" style="width: 5px;"></td>
                            <td colspan="1" class="Unit"><span>A</span></td>
                            <td colspan="1"></td>
                        </tr>
                        <tr style="height: 25%;">
                            <td colspan="11"></td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer" style="height: 40%;">

                    <table>
                        <tr>
                            <td class="modal_option"><a href="javascript:;">
                                <div class="Modal_btn_diary"></div>
                            </a></td>
                            <td></td>
                            <td class="modal_option"><a href="javascript:;">
                                <div class="Modal_btn_month"></div>
                            </a></td>
                            <td></td>
                            <td class="modal_option"><a href="javascript:;">
                                <div class="Modal_btn_chart"></div>
                            </a></td>
                            <td></td>
                            <td class="modal_option"><a href="javascript:;">
                                <div class="Modal_btn_record"></div>
                            </a></td>
                        </tr>
                        <tr>
                            <td><strong>日報表</strong></td>
                            <td><strong></strong></td>
                            <td><strong>月報表</strong></td>
                            <td><strong></strong></td>
                            <td><strong>趨勢圖</strong></td>
                            <td><strong></strong></td>
                            <td><strong>數值紀錄</strong></td>
                        </tr>
                    </table>





                    <%--<button type="button" class="btn btn-primary">月報表</button>
                    <button type="button" class="btn btn-primary">日報表</button>
                    <button type="button" class="btn btn-primary">查詢</button>
                    <button type="button" class="btn btn-primary">曲線圖</button>--%>
                </div>
            </div>
        </div>
    </div>


    



</body>
</html>
