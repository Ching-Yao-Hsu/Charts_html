﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DayReport.aspx.vb" Inherits="DayReport" %>

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
            <asp:Label ID="export" runat="server" Text="Export"></asp:Label>
            <asp:ImageButton ID="Print" runat="server" ImageUrl="~/img/print-icon.png"
                OnClientClick="printScreen(Print_Table)" OnClick="print_Click" ToolTip="Print" />
            <asp:ImageButton ID="Excel" runat="server" ImageUrl="img/excel-icon.png" ToolTip="Export Excel" />
            <asp:ImageButton ID="PDF" runat="server" ImageUrl="img/PDF-icon.png" ToolTip="Export PDF" />
        </asp:Panel>
        <asp:Panel ID="Panel_Report" runat="server">
            <div id="Print_Table" class="inner" style="padding-left: 0px;">
                <table width="900px">
                   <tr>
                    <td colspan="8">
                        <table width="900px" border="0">
                            <tr>
                                <td colspan="8" align="center">
                                    <asp:Label ID="Label2" runat="server" Text="Daily Report" Font-Bold="true" Font-Size="22px"></asp:Label></td>
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
                                <td colspan="4">
                                    <table width="440px" border="1" rules="rows">
                                        <tr>
                                            <td colspan="4" align="center" bgcolor="#333333">
                                                <asp:Label ID="Label1" runat="server" Text="Electricity consumption" ForeColor="White" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">Peak Time</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Rush" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">Half Peak Time</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Half" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">Saturday Half Peak Time</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="SatHalf" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">Off Peak Time</td>
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
                                                        <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Total</td>
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
                    <td colspan="4" width="440px">
                        <table width="440px" border="1" rules="rows">
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="W" runat="server" Text="Power" Font-Bold="true" ForeColor="White" Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Average</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgW" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Maximum</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxW" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="V" runat="server" Text="Voltage" Font-Bold="true" ForeColor="White" Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Average</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgV" runat="server" Font-Bold="true" Text="--"></asp:Label>V</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Maximum</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxV" runat="server" Font-Bold="true" Text="--"></asp:Label>V</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="A" runat="server" Text="Current" Font-Bold="true" ForeColor="White" Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Average</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgI" runat="server" Font-Bold="true" Text="--"></asp:Label>A</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">Maximum</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxI" runat="server" Font-Bold="true" Text="--"></asp:Label>A</td>
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
                            <asp:GridView ID="GridView1" runat="server" BackColor="White"
                                BorderColor="Black" BorderWidth="1px" CellPadding="4"
                                DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Horizontal"
                                Width="900px" BorderStyle="Solid">
                                <RowStyle HorizontalAlign="Center" Wrap="false" />
                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" Font-Size="14px"
                                    VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" Width="112.5px" />
                                <AlternatingRowStyle BackColor="#fafafa" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
        </asp:Panel>
    </form>
</body>
</html>
