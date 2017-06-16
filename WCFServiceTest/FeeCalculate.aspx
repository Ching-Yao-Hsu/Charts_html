<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FeeCalculate.aspx.vb" Inherits="FeeCalculate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <style type="text/css">
        .td_style
        {
            font-weight:700;
        }
        /*td
        {
            height: 20px;
        }*/
    </style>
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
                <table width="800px" border="0">
<%--                    <tr>
                        <td colspan="6" align="center">
                            <asp:Label ID="Label3" runat="server" Text="台灣國際造船股份有限公司 - 基隆廠" Font-Bold="true" Font-Size="22px"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="center" colspan="7">
                            <asp:Label ID="Label2" runat="server" Text="電費試算表" Font-Bold="true" Font-Size="22px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table border="0" style="font-size:12pt;">
                                <tr>
                                    <%--<td align="right" width="100px" style="font-weight: 800">編號：</td>--%>
                                    <td align="left" colspan="3" width="400px" style="padding-left:30px;">
                                        <asp:Label ID="id" runat="server" Text="Label"></asp:Label></td>
                                    <%--<td align="right" width="100px" style="font-weight: 800">計費區間：</td>--%>
                                    <td align="left" colspan="4" width="400px" style="padding-left:30px;">
                                        <asp:Label ID="date_txt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <%--<td align="right" style="font-weight: 800">位置：</td>--%>
                                    <td align="left" colspan="3" style="padding-left:30px;">
                                        <asp:Label ID="position_txt" runat="server" Text="Label"></asp:Label></td>
                                    <%--<td align="right" style="font-weight: 800">電價類型：</td>--%>
                                    <td align="left" colspan="4" style="padding-left:30px;">
                                        <asp:Label ID="feetype_txt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="400px" border="1" rules="all" style="font-size:12pt;">
                                <tr>
                                    <td align="center" style="font-weight: 700">種類</td>
                                    <td align="center" style="font-weight: 700">契約容量</td>
                                    <td align="center" style="font-weight: 700">最大需量(kw)</td>
                                </tr>
                                <tr>
                                    <td align="center">尖峰</td>
                                    <td align="center">
                                        <asp:Label ID="RsuhCapTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="RushOverTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="HalfHour_txt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="HalfCapTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="HalfOverTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">週六半尖峰</td>
                                    <td align="center">
                                        <asp:Label ID="SatHalfCapTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="SatHalfOverTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">離峰</td>
                                    <td align="center">
                                        <asp:Label ID="OffCapTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="OffOverTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <table width="400px" border="0">
                                            <tr>
                                                <td align="right" colspan="2" style="font-weight: 700;font-size: 12pt;">基本電費：</td>
                                                <td align="right" style="font-weight: 700;font-size: large;">
                                                    <asp:Label ID="BasicFeeTxt" runat="server" Text="Label" Font-Size="16pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; 元
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2" style="font-weight: 700;font-size: 12pt;">超約附加費：</td>
                                                <td align="right" style="font-weight: 700;font-size: large;">
                                                    <asp:Label ID="OverFeeTxt" runat="server" Text="Label" Font-Size="16pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; 元
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    
                                </tr>
                                
                            </table>
                        </td>
                        <td colspan="4">
                            <table width="400px" border="1" rules="all" style="font-size:12pt;">
                                <tr>
                                    <td align="center" style="font-weight: 700">種類</td>
                                    <td align="center" style="font-weight: 700">用電量(KWH)</td>
                                    <td align="center" style="font-weight: 700">單價($)</td>
                                    <td align="center" style="font-weight: 700">總計($)</td>
                                </tr>
                                <tr>
                                    <td align="center">尖峰</td>
                                    <td align="center">
                                        <asp:Label ID="RushHourTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="RushPriceTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="RushSumTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">半尖峰</td>
                                    <td align="center">
                                        <asp:Label ID="HalfHourTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="HalfPriceTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="HalfSumTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">週六半尖峰</td>
                                    <td align="center">
                                        <asp:Label ID="SatHalfHourTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="SatHalfPriceTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="SatHalfSumTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">離峰</td>
                                    <td align="center">
                                        <asp:Label ID="OffHourTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="OffPriceTxt" runat="server" Text="Label"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="OffSumTxt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table width="400px">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td align="right" colspan="2" rowspan="2" style="font-weight: 700;font-size: 12pt;">總流動電費：
                                                </td>
                                                <td align="right" rowspan="2" style="font-weight: 700;font-size: large;">
                                                    <asp:Label ID="FlowFeeTxt" runat="server" Text="Label" Font-Size="16pt" Font-Bold="True"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; 元
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" width="300px" style="font-size:14pt;font-weight:700;">
                            平均每度用電：
                        </td>
                        <td align="right" style="font-weight: 700;font-size: large;">
                            <asp:Label ID="AvgFeeTxt" runat="server" Text="Label" Font-Bold="True" Font-Size="18pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; 元</td>
                        <td align="right" colspan="3" width="250px" style="font-size:14pt;font-weight:700;">
                            總計：
                        </td>
                        <td align="right" style="font-weight: 700;font-size: large;">
                            <asp:Label ID="TotalFeeTxt" runat="server" Text="Label" Font-Bold="True" Font-Size="18pt" ForeColor="#FF3300"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; 元</td>
                    </tr>
                </table>
            </div>
            <br />
        </asp:Panel>
    </form>
</body>
</html>
