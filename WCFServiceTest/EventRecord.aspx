<%@ Page Language="VB" AutoEventWireup="true" EnableEventValidation="false" CodeFile="EventRecord.aspx.vb" Inherits="EventRecord" %>

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
    <style type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .menu a{ text-decoration:none;color:#333;}
        .menu a:hover{ text-decoration:underline;}
        .style1
        {
            width: 200px;
            padding-left:30px;
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
        <asp:Panel ID="Panel_Record" runat="server" Visible="false">
            <div id="Print_Table" class="inner" align="center" style="padding-left: 0px;">
                <table width="800px">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" Text="事件紀錄表" Font-Bold="true" Font-Size="22px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style1" nowrap="nowrap">
                            <asp:Label ID="Date_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" BackColor="White"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Horizontal"
                                Width="800px">
                                <RowStyle HorizontalAlign="Center" Wrap="false" />
                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                                <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" />
                                <AlternatingRowStyle BackColor="#fafafa" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
