<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="SumReportSetup.aspx.vb" Inherits="SumReportSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            width: 140px;
            height: 35px;
            padding-right:10px;
        }
        .style4
        {
            width: 308px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/SumReportSetupSupport.htm";
            $('.support').colorbox({ innerWidth: 715, innerHeight: 370, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
        });
        var xmlHttp;
        //程式由此執行
        function chkDrop() {
            img = document.getElementById("img_account");
            img.style.visibility = 'visible';
            var msg = document.getElementById("msg_account");
            var value = document.getElementById("<%=Group_DropDownList.ClientID%>");
	        var ovalue = value.options[value.selectedIndex].value;

	        if (ovalue != 0) {
	            img.src = "img/success.png";
	            img.style.visibility = 'visible';
	            msg.innerHTML = "";
	        } else {
	            img.src = "img/error.png";
	            img.style.visibility = 'visible';
	            msg.innerHTML = "<font color=red>請選擇群組</font>";
	        }
        }
        function chkDayEmail() {
            img = document.getElementById("img_daymail");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_daymail");
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("daymail_txt").ClientID%>').value;
	        var count = email.split(";").length;
	        if (email != '') {
	            var regex = /^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$/;
	            if (regex.test(email) == true) {
	                if (count <= 10) {
	                    img.src = "img/success.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "";
	                } else {
	                    img.src = "img/error.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "<font color=red>電子郵件不可超過10組</font>";
	                }
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>電子郵件格式錯誤</font>";
	            }
	        } else {
	            img.style.visibility = 'hidden';
	            msg.innerHTML = "";
	        }
        }

        function chkMonthEmail() {
            img = document.getElementById("img_monthmail");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_monthmail");
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("monthmail_txt").ClientID%>').value;
            var count = email.split(";").length;
            if (email != '') {
                var regex = /^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$/;
                if (regex.test(email) == true) {
                    if (count <= 10) {
                        img.src = "img/success.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "";
                    } else {
                        img.src = "img/error.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "<font color=red>電子郵件不可超過10組</font>";
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>電子郵件格式錯誤</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
        function chkAll() {
            chkDrop();
            chkDayEmail();
            chkMonthEmail();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label5" runat="server" Text="總表設定" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff; width: 650px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right" class="style3" width="30">
                                                        <img src="img/icon-account.png" />群組
                                                    </td>
                                                    <td class="style4" align="left">
                                                        <asp:DropDownList ID="Group_DropDownList" runat="server" Width="150px" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <img id="img_account" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                        <div id="msg_account" style="width: auto; font-size: 12px">
                                                        </div>
                                                    </td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Group_DropDownList"
                                                        ErrorMessage="請選擇群組" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                                        <asp:CheckBox ID="sumday_sent" runat="server" Text="寄送總日報表" Enabled="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="padding-right: 12px;">電子信箱
                                                    </td>
                                                    <td class="style4" align="left">
                                                        <asp:TextBox ID="daymail_txt" runat="server" Width="230px" Height="50px" onblur="chkDayEmail()" TextMode="MultiLine" Enabled="False"></asp:TextBox>
                                                        <img id="img_daymail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                        <div id="msg_daymail" style="width: auto; font-size: 12px">
                                                        </div>
                                                    </td>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage=""
                                                        ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="daymail_txt"
                                                        Display="None"></asp:RegularExpressionValidator>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                                        <asp:CheckBox ID="summonth_sent" runat="server" Text="寄送總月報表" Enabled="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="padding-right: 12px;">電子信箱
                                                    </td>
                                                    <td class="style4" align="left">
                                                        <asp:TextBox ID="monthmail_txt" runat="server" Width="230px" Height="50px" onblur="chkMonthEmail()" TextMode="MultiLine" Enabled="False"></asp:TextBox>
                                                        <img id="img_monthmail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                        <div id="msg_monthmail" style="width: auto; font-size: 12px">
                                                        </div>
                                                    </td>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage=""
                                                        ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="monthmail_txt"
                                                        Display="None"></asp:RegularExpressionValidator>
                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="padding-top: 10px;">
                                    <asp:Button ID="edit_btn" runat="server" Text="修改" CausesValidation="False" />
                                    <asp:Button ID="submit_btn" runat="server" Text="確定" OnClientClick="chkAll()" Visible="False" />
                                    &nbsp;&nbsp;&nbsp;
	                                <asp:Button ID="cancel_btn" runat="server" Text="取消" CausesValidation="False" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

