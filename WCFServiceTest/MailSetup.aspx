<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="MailSetup.aspx.vb" Inherits="MailSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/MailSetupSupport.htm";
            $('.support').colorbox({ innerWidth: 680, innerHeight: 460, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            //alert('<%= send_btn.ClientID%>');

            $("#<%= send_btn.ClientID%>").click(function () {
                if ($("#<%= test_mail.ClientID%>").val() == "") {
                    alertify.alert("請輸入您要測試的電子郵件地址");
                    return false;
                }
            });
        });
        function chkSmtp() {
            img_Prc = document.getElementById("img_Prc");
            img_Prc.style.visibility = 'visible';
            img_Prc.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div = document.getElementById("msg");
            var smtp = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("SmtpServer").ClientID %>').value;
            if (smtp != '') {
                var regex = /^\w+([-.]\w+)*\.\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                if (regex.test(smtp) == true) {
                    img_Prc.src = "img/success.png";
                    img_Prc.style.visibility = 'visible';
                    div.innerHTML = "";
                } else {
                    img_Prc.src = "img/error.png";
                    img_Prc.style.visibility = 'visible';
                    div.innerHTML = "<font color=red>SmtpServer格式錯誤</font>";
                }
            } else {
                img_Prc.src = "img/error.png";
                img_Prc.style.visibility = 'visible';
                div.innerHTML = "<font color=red>SmtpServer不可為空</font>";
            }
        }
        function chkAccount() {
            img_Prc2 = document.getElementById("img_Prc2");
            img_Prc2.style.visibility = 'visible';
            img_Prc2.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div2 = document.getElementById("msg2");
            var MailAccount = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("MailServer_Account").ClientID %>').value;
            if (MailAccount != '') {
                var regex1 = /^\w+$/;
                if (regex1.test(MailAccount) == true) {
                         var regex3 = /^[a-zA-Z0-9]/;
                         if (regex3.test(MailAccount) == true) {
                            img_Prc2.src = "img/success.png";
                            img_Prc2.style.visibility = 'visible';
                            div2.innerHTML = "";
                         } else {
                             img_Prc2.src = "img/error.png";
                             img_Prc2.style.visibility = 'visible';
                             div2.innerHTML = "<font color=red>寄件者認證帳號需以英文字母或數字作為開頭</font>";
                         }
                } else {
                    img_Prc2.src = "img/error.png";
                    img_Prc2.style.visibility = 'visible';
                    div2.innerHTML = "<font color=red>寄件者認證帳號只能含有數字與英文</font>";
                }
            } else {
                img_Prc2.style.visibility = 'hidden';
                div2.innerHTML = "";
            }
        }
        function chkPassword() {
            img_Prc3 = document.getElementById("img_Prc3");
            img_Prc3.style.visibility = 'visible';
            img_Prc3.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div3 = document.getElementById("msg3");
            var MailPassword = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("MailServer_Password").ClientID %>').value;
            if (MailPassword != '') {
                var regex1 = /^(?!.*[^\x21-\x7e]).*$/;
                if (regex1.test(MailPassword) == true) {
                        img_Prc3.src = "img/success.png";
                        img_Prc3.style.visibility = 'visible';
                        div3.innerHTML = "";
                } else {
                    img_Prc3.src = "img/error.png";
                    img_Prc3.style.visibility = 'visible';
                    div3.innerHTML = "<font color=red>密碼不允許特殊符號、數字、英文字母以外的字元</font>";
                }
            } else {
                img_Prc3.style.visibility = 'hidden';
                div3.innerHTML = "";
            }
        }
        function chkEmail() {
            img_Prc4 = document.getElementById("img_Prc4");
            img_Prc4.style.visibility = 'visible';
            img_Prc4.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div4 = document.getElementById("msg4");
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("MailAddress").ClientID %>').value;
            if (email != '') {
                var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                if (regex.test(email) == true) {
                    img_Prc4.src = "img/success.png";
                    img_Prc4.style.visibility = 'visible';
                    div4.innerHTML = "";
                } else {
                    img_Prc4.src = "img/error.png";
                    img_Prc4.style.visibility = 'visible';
                    div4.innerHTML = "<font color=red>電子郵件格式錯誤</font>";
                }
            } else {
                img_Prc4.style.visibility = 'hidden';
                div4.innerHTML = "";
            }
        }
        function chkName() {
            img_Prc5 = document.getElementById("img_Prc5");
            img_Prc5.style.visibility = 'visible';
            img_Prc5.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div5 = document.getElementById("msg5");
            var name = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("MailName").ClientID %>').value;
            if (name != '') {
                var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+='" ])*$/;
                if (regex.test(name) == true) {
                    img_Prc5.src = "img/success.png";
                    img_Prc5.style.visibility = 'visible';
                    div5.innerHTML = "";
                } else {
                    img_Prc5.src = "img/error.png";
                    img_Prc5.style.visibility = 'visible';
                    div5.innerHTML = "<font color=red>寄件者名稱不允許特殊符號、數字、英文字母以外的字元</font>";
                }
            } else {
                img_Prc5.style.visibility = 'hidden';
                div5.innerHTML = "";
            }
        }

        function chkBcc() {
            img_Prc6 = document.getElementById("img_Prc6");
            img_Prc6.style.visibility = 'visible';
            img_Prc6.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div6 = document.getElementById("msg6");
            var bcc = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Bcc").ClientID %>').value;
            if (bcc != '') {
                var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                if (regex.test(bcc) == true) {
                    img_Prc6.src = "img/success.png";
                    img_Prc6.style.visibility = 'visible';
                    div6.innerHTML = "";
                } else {
                    img_Prc6.src = "img/error.png";
                    img_Prc6.style.visibility = 'visible';
                    div6.innerHTML = "<font color=red>密件副本格式錯誤</font>";
                }
            } else {
                img_Prc6.style.visibility = 'hidden';
                div6.innerHTML = "";
            }
        }

        function chkAll() {
            chkAccount();
            chkPassword();
            chkSmtp();
            chkEmail();
        }

        function chkTestEmail() {
            img_Prc7 = document.getElementById("img_Prc7");
            img_Prc7.style.visibility = 'visible';
            img_Prc7.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div7 = document.getElementById("msg7");
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("test_mail").ClientID %>').value;
            if (email != '') {
                var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                if (regex.test(email) == true) {
                    img_Prc7.src = "img/success.png";
                    img_Prc7.style.visibility = 'visible';
                    div7.innerHTML = "";
                } else {
                    img_Prc7.src = "img/error.png";
                    img_Prc7.style.visibility = 'visible';
                    div7.innerHTML = "<font color=red>電子郵件格式錯誤</font>";
                }
            } else {
                img_Prc7.style.visibility = 'hidden';
                div7.innerHTML = "";
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 120px;
            height: 35px;
            padding-left:20px;
        }
        .style2
        {
            width: 250px;
            height: 40px;
            padding-left:20px;
        }
        .style3
        {
            width: 170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="郵件設定" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                <td class="style1" align="right">*SmtpServer</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="SmtpServer" runat="server" class="style3" onblur="chkSmtp()" Enabled="False"></asp:TextBox>
                                    <img id="img_Prc" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg"></div>
                                </td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="SmtpServer不可為空" ControlToValidate="SmtpServer" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="SmtpServer格式錯誤"
                                    ValidationExpression="^\w+([-.]\w+)*\.\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="SmtpServer" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">*寄件者認證帳號</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="MailServer_Account" runat="server" class="style3" onblur="chkAccount()" Enabled="False"></asp:TextBox>
                                    <img id="img_Prc2" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg2"></div>
                                </td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="寄件者認證帳號不可為空" ControlToValidate="MailServer_Account" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="寄件者認證帳號格式錯誤"
                                    ValidationExpression="^[a-zA-Z0-9]\w{4,14}$" ControlToValidate="MailServer_Account" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">*寄件者認證密碼</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="MailServer_Password" runat="server" class="style3" onblur="chkPassword()" Enabled="False"></asp:TextBox>
                                    <img id="img_Prc3" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg3"></div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="寄件者認證密碼格式錯誤"
                                    ValidationExpression="^(?!.*[^\x21-\x7e]).*$" ControlToValidate="MailServer_Password" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">*寄件者電子郵件</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="MailAddress" runat="server" class="style3" onblur="chkEmail()" Enabled="False"></asp:TextBox>
                                    <img id="img_Prc4" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg4"></div>
                                </td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="寄件者電子郵件不可為空" ControlToValidate="MailAddress" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="寄件者電子郵件格式錯誤"
                                    ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="MailAddress" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">寄件者名稱</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="MailName" runat="server" class="style3" onblur="chkName()" Enabled="False"></asp:TextBox>
                                    <img id="img_Prc5" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg5"></div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="寄件者名稱只能含中文英文數字"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="MailName" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">密件副本</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="Bcc" runat="server" class="style3" onblur="chkBcc()" Enabled="False"></asp:TextBox>
                                    <img id="img_Prc6" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg6"></div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="密件副本格式錯誤"
                                    ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="Bcc" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="edit_btn" runat="server" Text="修改" CausesValidation="False" />
                                    <asp:Button ID="submit_btn" runat="server" Text="確認" Visible="False" CausesValidation="False" OnClientClick="chkAll()" />
                                    <asp:Button ID="cancel_btn" runat="server" Text="取消" Visible="False" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        測試電子郵件&nbsp;&nbsp;
                        <asp:TextBox ID="test_mail" runat="server" class="style3" onblur="chkTestEmail()"></asp:TextBox>
                        <img id="img_Prc7" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                        <div style="width: auto; font-size: 12px" id="msg7"></div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="電子郵件格式錯誤"
                            ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="test_mail" Display="None"></asp:RegularExpressionValidator>
                        <br />
                        <asp:Button ID="send_btn" runat="server" CausesValidation="False" Text="發送測試" />
                    </div>
                </td>
            </tr>
        </table>
    </div>	       
</asp:Content>

