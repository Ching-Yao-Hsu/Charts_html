<%@ Page Title="Member setting" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="UserInfo.aspx.vb" Inherits="UserInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/UserInfoSupport.htm";
            $('.support').colorbox({ innerWidth: 740, innerHeight: 775, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            $('.imageButtonFinderClass').click(function() {
                var $tr = $(this).parent().parent();
                var account = $tr.find("td").eq(0).text();
                var HrefAddress = "UserTable.aspx?account=" + account;
                $('.imageButtonFinderClass').colorbox({ innerWidth: 430, innerHeight: 535, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress, onClosed: reloadPage });
            });

            $('.imageButtonFinderClass1').click(function () {
                var $tr = $(this).parent().parent();
                var account = $tr.find("td").eq(0).text();
                var HrefAddress = "UserCom_en.aspx?account=" + account;
                $('.imageButtonFinderClass1').colorbox({ innerWidth: 470, innerHeight: 525, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress, onClosed: reloadPage });
            });
        });
        function reloadPage() {
            //colorbox關閉，刷新母網頁
            parent.location.reload();
        }
    </script>
    <style type="text/css">
        .gridviewstyle1
        {
             border-radius: 10px 0px 0px 0px;
        }
        .gridviewstyle2
        {
             border-radius: 0px 10px 0px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="Member setting" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="left" nowrap="nowrap" style="width:300px;">Member Account
                    <asp:TextBox ID="Account" runat="server"></asp:TextBox>
                    <asp:Button ID="submit_btn" runat="server" Text="Search" />
                </td>
                <td align="left">
                    <div style="position: absolute;">
                        <img src="img/email.png" width="22px" border="0" />
                    </div>
                    <div style="padding-left: 25px; padding-top: 0px;">
                        <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Welcome.aspx" Target="_blank"
                        Font-Size="12px" Style="text-decoration: none;">resend verification letter</asp:HyperLink>--%>
                        <asp:LinkButton ID="LinkButton1" runat="server" Style="text-decoration: none;"
                            OnClientClick="window.open('Welcome.aspx','','menubar=no,status=no,scrollbars=yes,Resizable=yes,width=885px,height=550px').focus();">resend verification letter</asp:LinkButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="White" Width="700px"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1"
                        ForeColor="Black" GridLines="None" AutoGenerateColumns="False" CssClass="normal tablesorter"
                        DataKeyNames="Account">
                        <Columns>
                            <asp:BoundField DataField="Account" HeaderText="Account" ReadOnly="True" HeaderStyle-CssClass="gridviewstyle1" />
                            <asp:BoundField DataField="ECO_Group" HeaderText="Group" />
                            <asp:BoundField DataField="Company" HeaderText="Company" />
                            <asp:BoundField DataField="Tel" HeaderText="Telephone" />
                            <asp:BoundField DataField="E_Mail" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Competence">
                                <ItemTemplate>
                                    <asp:Label ID="Rank" runat="server" Text='<%# Bind("Rank") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Enabled<br>status">
                                <ItemTemplate>
                                    <asp:Image ID="enabled" runat="server" Style="background-color: transparent;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="EnabledTime" HeaderText="Enable time" DataFormatString="{0:yyyy/MM/dd HH:mm}" />--%>
                            <asp:TemplateField HeaderText="Seach<br>Competence">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ViewDetails1_btn" runat="server" CausesValidation="false" ImageUrl="img/manage.png"
                                        Width="22px" CssClass="imageButtonFinderClass1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Management" HeaderStyle-CssClass="gridviewstyle2">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ViewDetails_btn" runat="server" CausesValidation="false" ImageUrl="img/manage.png"
                                        Width="22px" CssClass="imageButtonFinderClass" />
                                    <%--<asp:ImageButton ID="delete_dtn" runat="server" CausesValidation="false"
                                    ImageUrl="assets/icons/actions_small/Trash.png" CssClass="tooltip table_icon"
                                    CommandName="Delete" OnClientClick="return confirm('Confirm that you want to delete? Including its controller and meter data be deleted.');" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle HorizontalAlign="Center" Wrap="false" />
                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                        <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle BackColor="#2851A4" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
                            HorizontalAlign="Center" Wrap="false" />
                        <AlternatingRowStyle BackColor="#e8f0ff" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    </div>
</asp:Content>

