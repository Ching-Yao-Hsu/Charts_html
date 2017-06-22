﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WaterSMonthReport.aspx.vb" Inherits="WaterSMonthReport" %>

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
        
        $(document).ready(function () {
            $("#<%= txtMonth.ClientID%>").datepicker({
                dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });

            $("#<%=ButtonSM.ClientID%>").click(function () {
                var sShopNo = $("#<%= LabShopNo.ClientID%>").val();
                var sDrawNo = $("#<%= LabDrawNo.ClientID%>").val();
                var sShopName = $("#<%= LabShopName.ClientID%>").val();
                var sMonthS = $("#<%= txtMonth.ClientID%>").val();
                if (sMonthS != '') {
                    var HrefAddress = "WaterSMonthReport.aspx?sShopNo=" + sShopNo + "&sDrawNo=" + sDrawNo + "&sShopName=" + sShopName + "&sMonthS=" + sMonthS;
                    alert(HrefAddress);
                    $("#<%=ButtonSM.ClientID%>").colorbox({ innerWidth: 950, innerHeight: 520, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                    return true;
                } else {
                    alertify.alert("月報表的月份不可空白!");
                    return false;
                }
                });
        });

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
                <table style="width:900px">
                   <tr>
                    <td  style="text-align:center;">
                        <asp:Label ID="Label2" runat="server" Text="月報表" Font-Bold="true" Font-Size="22px"></asp:Label>
                    </td>
                   </tr>
                                        <tr>
                                           <td>
                                                <asp:Label ID="Label1" runat="server" Text="月份:" Font-Bold="true" Font-Size="14px"></asp:Label>
                                                <asp:Label ID="Label5" runat="server" Text="月份:" Font-Bold="true" Font-Size="14px"></asp:Label>
                                                <asp:TextBox ID="txtMonth" runat="server" Width="50px"></asp:TextBox>
                                               　<asp:Button ID="ButtonSM" runat="server" Text="確認" />
                                           </td>
                                        </tr>
                    <tr>
                        <td>&nbsp; </td>
                    </tr>
                   <tr>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td>店鋪名稱：<asp:Label ID="LabShopName" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                <td>店鋪編號：<asp:Label ID="LabShopNo" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>圖面編號：<asp:Label ID="LabDrawNo" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                <td>總用水量：<asp:Label ID="LabSumAmount" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                   </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" BackColor="White"
                                BorderColor="Black" BorderWidth="1px" CellPadding="3"
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
                   <tr>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td><asp:Label ID="Label3" runat="server" Text="審核：" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                <td></td>
                                <td><asp:Label ID="Label4" runat="server" Text="製單：" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                   </tr>
                </table>
            </div>
            <br />
        </asp:Panel>
    </form>
</body>
</html>