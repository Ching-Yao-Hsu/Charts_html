<%@ Page Title="電表設定" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="MeterInfo2.aspx.vb" Inherits="MeterInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>  
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />
    
    <%--select Gridview row--%>
    <script type="text/javascript">
        $(document).ready(function() {
            $('.imageButtonFinderClass').click(function() {
                var $tr = $(this).parent().parent();
                var eco_account = $tr.find("td").eq(0).text();
                var ctrlnr = $tr.find("td").eq(1).text();
                var meterid = $tr.find("td").eq(2).text();
                var HrefAddress = "MeterTable.aspx?eco_account=" + eco_account + "&ctrlnr=" + ctrlnr + "&meterid=" + meterid;
                $('.imageButtonFinderClass').colorbox({ innerWidth: 500, innerHeight: 420, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress, onClosed: reloadPage });
            });
        });
        function reloadPage() {
            //colorbox關閉，刷新母網頁
            location.reload(true);
        }
    </script>
    
    <style type="text/css">
        .style1
        {
            width: 440px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="Account_DDList" runat="server" Width="150px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:DropDownList ID="EcoAccount_DDList" runat="server" Width="160px">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Account_DDList" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td class="style1">
                <asp:Button ID="submit_btn" runat="server" Text="查詢" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1"
                            ForeColor="Black" GridLines="Horizontal" Width="750px" AutoGenerateColumns="False"
                            CssClass="normal tablesorter" DataKeyNames="ECO_Account,CtrlNr,MeterID" AllowPaging="True">
                            <Columns>
                                <asp:BoundField DataField="ECO_Account" HeaderText="ECO帳號" ReadOnly="True" />
                                <asp:BoundField DataField="CtrlNr" HeaderText="控制器編號" ReadOnly="True" />
                                <asp:BoundField DataField="MeterID" HeaderText="電表編號" ReadOnly="True" />
                                <asp:TemplateField HeaderText="圖面編號">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("DrawNr") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="drawtxt" runat="server" Text='<%# Bind("DrawNr") %>'></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="圖面編號只能含中文英文數字"
                                            ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="drawtxt" Display="None"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ControlStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="安裝位置">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("InstallPosition") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="positiontxt" runat="server" Text='<%# Bind("InstallPosition") %>'></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="位置只能含中文英文數字"
                                            ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="positiontxt"
                                            Display="None"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="啟用狀態">
                                    <EditItemTemplate>
                                        <asp:RadioButtonList ID="enabled_rabtn" runat="server" Width="160px" RepeatDirection="Horizontal"
                                            SelectedValue='<%# Bind("Enabled") %>'>
                                            <asp:ListItem Value="1">啟用</asp:ListItem>
                                            <asp:ListItem Value="0">停用</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Image ID="enabled" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="管理">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ViewDetails_btn" runat="server" CausesValidation="false" CommandName="Edit"
                                            Width="22px" ImageUrl="img/manage.png" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                            Text="更新"></asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="取消"></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle HorizontalAlign="Center" Wrap="false" />
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                            <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
                                HorizontalAlign="Center" Wrap="false" />
                            <AlternatingRowStyle BackColor="#fafafa" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div align="left" style="padding-left: 20px;">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    </div>
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
</asp:Content>

