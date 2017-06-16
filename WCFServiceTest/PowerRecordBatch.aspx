<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="PowerRecordBatch.aspx.vb" Inherits="PowerRecordBatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .menu a{ text-decoration:none;color:#333;}
        .menu a:hover{ text-decoration:underline;}
        .style1
        {
            width: 200px;
            padding-left:100px;
        }
        .td_width
        {
            height: 65px;
            width: 80px;
        }
        .title_div
        {
            line-height: 25px;
            width: 100px;
            text-align: center;
            padding-top:10px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            //月曆
            $("#<%= Date_txt1.ClientID %>").datepicker({
                dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            $("#<%= Date_txt2.ClientID %>").datepicker({
                dateFormat: 'yy/mm', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
        });
    </script>
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="電力紀錄批次" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right:10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#eaf8c7; width:600px;">
                        <table>
                            <tr>
                                <td align="left">
                                    會員帳號
                                    <asp:DropDownList ID="Account_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    ECO5編號
                                    <asp:DropDownList ID="CtrlNr_DropDownList" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" height="30px">
                                    電表編號
                                    <asp:DropDownList ID="MeterID_DropDownList" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" height="30px">
                                    <asp:CheckBox ID="Date_CB" runat="server" Text="日期" />
			                        <asp:TextBox ID="Date_txt1" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="Month_CB" runat="server" Text="月份" />
                                    <asp:TextBox ID="Date_txt2" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="padding-top:10px;">
                                    <asp:Button ID="submit_btn" runat="server" Text="確定" CssClass="imageButtonFinderClass" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

