<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerDayReport.aspx.cs" Inherits="WCFService_Test.PowerDayReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />    
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous" />
    <link href="css/org.css" rel="stylesheet" />
    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        .ui-datepicker-trigger {
            cursor: pointer;
            width: 20px;
            height: 20px;
        }

        .td_width {
            height: 65px;
            width: 80px;
        }

        .title_div {
            line-height: 25px;
            width: 100px;
            text-align: center;
            padding-top: 10px;
        }
    </style>
    
    <script>

        function toObject(arr) {
            var rv = {};
            for (var i = 0; i < arr.length; ++i)
                if (arr[i] !== undefined) rv[i] = arr[i];
            return rv;
        }

        $(document).ready(function () {
            var _nodeId = "00";
            var bool = false;
            var _ECO_Group = "eco";

            $.ajax({
                url: "server.aspx",
                dataType: "json",
                data: {
                    nodeId: _nodeId,
                    ECO_Group: _ECO_Group
                },
                type: "POST",
                success: function (e) {
                    $("#ECO_Group").empty();
                    for (i = 0; i < e[1]["DropDownList"].length; i++) {                        
                        $("#ECO_Group").append(
                            $("<option/>")
                                .text(e[1]["DropDownList"][i]["ECO_Group"])
                                .attr("value", e[1]["DropDownList"][i]["Account"])
                        );
                    }
                },
                error: function () {
                    alert("error");
                }
            })






            //$("#ECO_Group").change(function () {
            //    console.log(this.value);
            //    $.ajax({
            //        url: "server.aspx",
            //        dataType: "json",
            //        data: {
            //            nodeId: _nodeId
            //        },
            //        type: "POST",
            //        success: function (e) {
            //            $("#ECO_Group").empty();
            //            for (i = 0; i < e[1]["DropDownList"].length; i++) {
            //                $("#ECO_Group").append(
            //                    $("<option/>")
            //                        .text(e[1]["DropDownList"][i]["ECO_Group"])
            //                        .attr("value", e[1]["DropDownList"][i]["Account"])
            //                );
            //            }

            //            var rec = new Ezrecursion();
            //            var table = rec.init_recursion(e[2]["TreeView"]);
            //            rec.recursion(e[2]["TreeView"])(table);
            //            function SortByName(a, b) {
            //                var aName = a.id.toLowerCase();
            //                var bName = b.id.toLowerCase();
            //                return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
            //            }
            //            e[2]["TreeView"].sort(SortByName);
            //            $('#tree').EzOrgChart(e[2]["TreeView"]);

            //        },
            //        error: function () {
            //            alert("error");
            //        }
            //    })
            //});


            //$("#tree a").click(function () {
            //    _nodeId = $(this).data("node");
            //    $("#myModalLabel").text(e[0]["Modal"][0]["ECO_AccountAndMeterId"]);
            //});


            

            //$.ajax({
            //    url: "server.aspx",
            //    dataType: "json",
            //    data: {
            //        nodeId: _nodeId
            //    },
            //    type: "POST",
            //    success: function (e) {
            //        for (i = 0; i < e[1]["DropDownList"].length; i++) {
            //            $("#ECO_Group").append(
            //                $("<option/>")
            //                    .text(e[1]["DropDownList"][i]["ECO_Group"])
            //                    .attr("value", e[1]["DropDownList"][i]["Account"])
            //            );
            //        }                                        
            //        var rec = new Ezrecursion();
            //        var table = rec.init_recursion(e[2]["TreeView"]);
            //        rec.recursion(e[2]["TreeView"])(table);

            //        function SortByName(a, b) {
            //            var aName = a.id.toLowerCase();
            //            var bName = b.id.toLowerCase();
            //            return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
            //        }

            //        e[2]["TreeView"].sort(SortByName);

            //        $('#tree').EzOrgChart(e[2]["TreeView"]);
                    
            //    },
            //    error: function () {
            //        alert("error");
            //    }
            //})
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="content" align="center">
            <table>
                <tr>
                    <td align="left" colspan="1">
                        <asp:Label ID="Label2" runat="server" Text="日報表" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="1">
                        <div style="border-radius: 15px; background-color: #eaf8c7; width: 795px;">
                            <table style="width: 100%;">
                                <tr style="height: 2px;">
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="1">&nbsp;</td>
                                    <td align="left" colspan="1">
                                        <img src="img/date2.png" />日期
                                    <asp:TextBox ID="Date_txt" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    </td>
                                    <td align="left" colspan="1">時間間隔
			                        <asp:DropDownList ID="interval_DDList" runat="server">
                                        <asp:ListItem Value="1" Selected="True">1分鐘</asp:ListItem>
                                        <asp:ListItem Value="5">5分鐘</asp:ListItem>
                                        <asp:ListItem Value="30">30分鐘</asp:ListItem>
                                        <asp:ListItem Value="60">1小時</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                                    <td colspan="1">&nbsp;</td>
                                </tr>
                                <tr style="height: 2px;">
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="1" style="width: 5%;"></td>
                                    <td colspan="2">
                                        <div style="background-color: #e8f0ff; border-radius: 15px;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td align="left" colspan="4" nowrap="nowrap">
                                                                    <img src="img/icon-account.png" />群組
                                                                    <%--<asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>--%>

                                                                    <select id="ECO_Group">
                                                                        <option value="">請選擇</option>
                                                                    </select>
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" nowrap="nowrap">
                                                                    <img src="img/compare.png" />顯示名稱</td>
                                                                <td align="left" valign="middle" nowrap="nowrap">
                                                                    <asp:CheckBox ID="CtrlNr_CB" runat="server" Text="ECO5編號" /><br />
                                                                    <asp:CheckBox ID="Position_CB" runat="server" Text="安裝位置" />
                                                                </td>
                                                                <td align="left" valign="middle" nowrap="nowrap">
                                                                    <asp:CheckBox ID="MeterId_CB" runat="server" Text="電表編號" /><br />
                                                                    <asp:CheckBox ID="LineNum_CB" runat="server" Text="單線圖編號" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Button ID="submit_btn" runat="server" Text="查詢" OnClick="submit_btn_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                
                                                <tr style="border-top:dotted 1px;">
                                                    <td align="left" valign="top">
                                                        <div style="height: 415px; width: 100%; overflow: auto; background-color: #e8f0ff; border-radius: 0px 0px 15px 15px;">                                                              
                                                            <div id="tree"></div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td colspan="1" style="width: 5%;"></td>
                                </tr>

                                <tr>
                                    <td align="center" colspan="4">
                                        <div style="height: 50px; padding-top: 5px;">
                                            <asp:ImageButton ID="ViewDetails_btn" runat="server" ImageUrl="~/img/btn_dailyreport_01.png"
                                                onmouseover="this.src='img/btn_dailyreport_02.png'" onmouseout="this.src='img/btn_dailyreport_01.png'"
                                                CssClass="imageButtonFinderClass" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>


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



    </form>
</body>
<asp:sqldatasource runat="server" id="DataSource_ECO_Group" connectionstring="<%$ ConnectionStrings:ECOSMARTConnectionString %>" selectcommand="SELECT [ECO_Group], [Account] FROM [AdminSetup]"></asp:sqldatasource>
</html>
