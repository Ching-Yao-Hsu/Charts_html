﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerDayReport.aspx.cs" Inherits="WCFService_Test.PowerDayReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script>
        $(document).ready(function () {
            $("#myModalLabel").text("ECOSmart");

            $("#container_treeview a").click(function () {
                var _nodeId = $(this).data("node");   
                $.ajax({                    
                    url: "server.aspx",
                    dataType: "json",
                    data: {
                        nodeId: _nodeId
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
        });
    </script>



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
                            <table width="100%">
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
                                                                    <asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
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
                                                <tr>
                                                    <td>
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <div id="container_treeview" style="height: 415px; width: 100%; overflow: auto; background-color: #e8f0ff; border-radius: 0px 0px 15px 15px;">

                                                            <div><a href="#" data-toggle="modal" data-target="#myModal" data-node="00">00</a></div>
                                                            <div><a href="#" data-toggle="modal" data-target="#myModal" data-node="01">01</a></div>
                                                            <div><a href="#" data-toggle="modal" data-target="#myModal" data-node="02">02</a></div>
                                                            <div><a href="#" data-toggle="modal" data-target="#myModal" data-node="03">03</a></div>

                                                            <button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal">
                                                                Launch demo modal
                                                            </button>
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
