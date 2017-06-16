<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PowerFeeCalculateReport.aspx.vb" Inherits="PowerFeeCalculateReport" %>

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
    <asp:Panel ID="Panel_Export" runat="server" Style="padding-left: 20px;">
            <asp:Label ID="export" runat="server" Text="Export"></asp:Label>
            <asp:ImageButton ID="Print" runat="server" ImageUrl="~/img/print-icon.png"
                OnClientClick="printScreen(Print_Table)" OnClick="print_Click" ToolTip="Print" />
            <asp:ImageButton ID="Excel" runat="server" ImageUrl="img/excel-icon.png" ToolTip="Export Excel" />
            <asp:ImageButton ID="PDF" runat="server" ImageUrl="img/PDF-icon.png" ToolTip="Export PDF" />
        </asp:Panel>
        <div align="center">
            <asp:Panel ID="Panel_Report" runat="server">
                <div id="Print_Table" class="inner" style="padding-left: 0px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label2" runat="server" Text="Electricity spreadsheet" Font-Bold="true" Font-Size="22px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="RecDate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView1" runat="server" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="2"
                                    ForeColor="Black" Showheader="true">
                                    <RowStyle HorizontalAlign="Center" Wrap="false" />
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White"
                                        VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#fafafa" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
