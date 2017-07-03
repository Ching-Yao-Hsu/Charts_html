<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerDayReport.aspx.cs" Inherits="WCFService_Test.PowerDayReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>


    <style type="text/css">
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
                    <td align="left">
                        <asp:Label ID="Label2" runat="server" Text="日報表" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="1">
                        <div style="border-radius: 15px 15px 15px 15px; background-color: #eaf8c7; width: 795px;">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <img src="img/date2.png" />日期
                                    <asp:TextBox ID="Date_txt" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    </td>
                                    <td align="left">時間間隔
			                        <asp:DropDownList ID="interval_DDList" runat="server">
                                        <asp:ListItem Value="1" Selected="True">1分鐘</asp:ListItem>
                                        <asp:ListItem Value="5">5分鐘</asp:ListItem>
                                        <asp:ListItem Value="30">30分鐘</asp:ListItem>
                                        <asp:ListItem Value="60">1小時</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff;">
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
                                                        <div id="div1" style="height: 415px; width: 100%; overflow: auto; background-color: #e8f0ff; border-radius: 15px 15px 15px 15px;">
                                                            <asp:TreeView runat="server" ID="Meter_TreeView" Style="margin-right: 3px" ForeColor="Black" ShowLines="True" CssClass="ParentNodeStyle" Font-Italic="False" ExpandDepth="FullyExpand" NodeWrap="False" Font-Names="微軟正黑體">
                                                                <NodeStyle VerticalPadding="2px" HorizontalPadding="2px" />
                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" Width="100%" />
                                                            </asp:TreeView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="center" colspan="2">
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
    </form>
</body>
<asp:sqldatasource runat="server" ID="DataSource_ECO_Group" ConnectionString="<%$ ConnectionStrings:ECOSMARTConnectionString %>" SelectCommand="SELECT [ECO_Group], [Account] FROM [AdminSetup]"></asp:sqldatasource>
</html>
