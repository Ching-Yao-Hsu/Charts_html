<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WeekReport.aspx.vb" Inherits="WeekReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="js/print.js" type="text/javascript"></script>
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />
    <script src="js/alertify.min.js" type="text/javascript"></script>
    <link href="css/home.css" rel="stylesheet" type="text/css" />
    <style  type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .menu a{ text-decoration:none;color:#333;}
        .menu a:hover{ text-decoration:underline;}
        .style2
        {
            width: 60px;
        }
        .style3
        {
            width: 200px;
        }
    </style>
    <script type="text/javascript">
        function custom_script(sender) {
            alertify.alert(sender, function (e) {
                if (e) {
                    parent.$.fn.colorbox.close();
                } else {
                    // user clicked "cancel"
                }
            });
        }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel_Export" runat="server" Style="padding-left: 20px;" Visible="false">
            <asp:Label ID="export" runat="server" Text="輸出"></asp:Label>
            <asp:ImageButton ID="Print" runat="server" ImageUrl="~/img/print-icon.png"
                OnClientClick="printScreen(Print_Table)" OnClick="print_Click" ToolTip="列印" />
            <asp:ImageButton ID="Excel" runat="server" ImageUrl="img/excel-icon.png" ToolTip="匯出Excel" />
            <asp:ImageButton ID="PDF" runat="server" ImageUrl="img/PDF-icon.png" ToolTip="匯出PDF" />
        </asp:Panel>
        <asp:Panel ID="Panel_Report" runat="server">
        <div id="Print_Table" class="inner" style="padding-left: 0px;">
            <table width="900px">
                <tr>
                    <td colspan="8">
                        <table width="900px" border="0">
                            <%--<tr>
                                <td colspan="8" align="center">
                                    <asp:Label ID="Label3" runat="server" Text="台灣國際造船股份有限公司 - 基隆廠" Font-Bold="true" Font-Size="22px"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="8" align="center">
                                    <asp:Label ID="Label2" runat="server" Text="週報表" Font-Bold="true" Font-Size="22px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" width="440px">
                        <table width="440px">
                            <tr>
                                <td colspan="4">
                                    <table width="440px">
                                        <tr>
                                            <td colspan="4" align="left" style="padding-left:30px;">
                                                <asp:Label ID="Date_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left" style="padding-left:30px;">
                                                <asp:Label ID="Num_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left" style="padding-left:30px;">
                                                <asp:Label ID="Position_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" width="440px">
                        <table width="440px" border="1" rules="rows">
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="W" runat="server" Text="功率" Font-Bold="true" ForeColor="White"
                                        Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">平均值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgW" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">最大值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxW" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="V" runat="server" Text="電壓" Font-Bold="true" ForeColor="White"
                                        Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">平均值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgV" runat="server" Font-Bold="true" Text="--"></asp:Label>V</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">最大值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxV" runat="server" Font-Bold="true" Text="--"></asp:Label>V</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="A" runat="server" Text="電流" Font-Bold="true" ForeColor="White"
                                        Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">平均值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgI" runat="server" Font-Bold="true" Text="--"></asp:Label>A</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">最大值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxI" runat="server" Font-Bold="true" Text="--"></asp:Label>A</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                            </tr>
                        </table>
                    </td>
                    
                    <td colspan="4">
                        <table width="440px">
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" border="1" rules="rows">
                                        <tr>
                                            <td colspan="4" align="center" bgcolor="#333333">
                                                <asp:Label ID="Label4" runat="server" Text="最大需量" ForeColor="White"
                                                    Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Demand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="HalfDemand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">週六半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="SatHalfDemand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">離峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="OffDemand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" border="1" rules="rows">
                                        <tr>
                                            <td colspan="4" align="center" bgcolor="#333333">
                                                <asp:Label ID="Label1" runat="server" Text="用電量" ForeColor="White"
                                                    Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Rush" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Half" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">週六半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="SatHalf" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">離峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Off" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">總計</td>
                                                        <td colspan="3" align="right" bgcolor="white" style="padding-right: 20px;">
                                                            <asp:Label ID="Sum" runat="server" Text="--" ForeColor="Red" Font-Bold="true" Font-Size="16px"></asp:Label>KWH</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp; </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table width="900px" border="1" rules="all">
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="Label6" runat="server" Text="功率(kw)"></asp:Label></td>
                                <td colspan="5" align="center">
                                    <asp:Label ID="Label7" runat="server" Text="用電量(KWH)"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="GridView1" runat="server" BackColor="White"
                                        BorderColor="Black" BorderWidth="1px" CellPadding="4"
                                        DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Horizontal"
                                        Width="340px" BorderStyle="None" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="日期" HeaderText="日期" />
                                            <asp:BoundField DataField="Wavg" HeaderText="平均值" />
                                            <asp:BoundField DataField="Wmax" HeaderText="最大值" />
                                        </Columns>
                                        <RowStyle HorizontalAlign="Center" Wrap="false" />
                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right"
                                            VerticalAlign="Middle" />
                                        <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" Font-Size="14px"
                                            VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" Width="112.5px" />
                                        <AlternatingRowStyle BackColor="#fafafa" />
                                    </asp:GridView>
                                </td>
                                <td colspan="5">
                                    <asp:GridView ID="GridView2" runat="server" BackColor="White"
                                        BorderColor="Black" BorderWidth="1px" CellPadding="4"
                                        DataSourceID="SqlDataSource2" ForeColor="Black" GridLines="Horizontal"
                                        Width="560px" BorderStyle="None" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="RushHour" HeaderText="尖峰" />
                                            <asp:BoundField DataField="HalfHour" HeaderText="半尖峰" />
                                            <asp:BoundField DataField="SatHalfHour" HeaderText="週六半尖峰" />
                                            <asp:BoundField DataField="OffHour" HeaderText="離峰" />
                                            <asp:TemplateField HeaderText="總計"></asp:TemplateField>
                                        </Columns>
                                        <RowStyle HorizontalAlign="Center" Wrap="false" />
                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" Font-Size="14px"
                                            VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" Width="112.5px" />
                                        <AlternatingRowStyle BackColor="#fafafa" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
        <br />
    </asp:Panel>
    </form>
</body>
</html>
