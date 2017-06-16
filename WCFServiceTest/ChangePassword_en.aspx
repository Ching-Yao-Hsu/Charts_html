<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />

     <style type="text/css">
        .style1
        {
            width: 170px;
            height: 35px;
            padding-left:10px;
        }
        .style2
        {
            width: 250px;
            height: 40px;
        }
        .support
        {
            outline: none;
            border: none;
        }
    </style>
    
    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/ChangePassword.htm";
            $('.support').colorbox({ innerWidth: 715, innerHeight: 260, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
        });

        var xmlHttp;
        function chkOldPassword() {
            img1 = document.getElementById("img1");
            img1.style.visibility = 'visible';
            img1.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div1 = document.getElementById("msg1");
            var password = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("OldPassword").ClientID %>').value;
            if (password != '') {
                var regex1 = /^\w+$/;
                if (regex1.test(password) == true) {
                    var regex2 = /^.{6,15}$/;
                    if (regex2.test(password) == true) {
                        img1.src = "img/success.png";
                        img1.style.visibility = 'visible';
                        div1.innerHTML = "";
                    } else {
                        img1.src = "img/error.png";
                        img1.style.visibility = 'visible';
                        div1.innerHTML = "<font color=red>Password does not match the number of words, from 6 to 15 characters in length limit!</font>";
                    }
                } else {
                    img1.src = "img/error.png";
                    img1.style.visibility = 'visible';
                    div1.innerHTML = "<font color=red>The password can only contain numbers and English!</font>";
                }
            } else {
                img1.src = "img/error.png";
                img1.style.visibility = 'visible';
                div1.innerHTML = "<font color=red>Please enter your old password!</font>";
            }
        }
        
        function chkNewPassword() {
            img2 = document.getElementById("img2");
            img2.style.visibility = 'visible';
            img2.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div2 = document.getElementById("msg2");
            var password = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("NewPassword").ClientID %>').value;
            if (password != '') {
                var regex1 = /^\w+$/;
                if (regex1.test(password) == true) {
                    var regex2 = /^.{6,15}$/;
                    if (regex2.test(password) == true) {
                        img2.src = "img/success.png";
                        img2.style.visibility = 'visible';
                        div2.innerHTML = "";
                    } else {
                        img2.src = "img/error.png";
                        img2.style.visibility = 'visible';
                        div2.innerHTML = "<font color=red>Password does not match the number of words, from 6 to 15 characters in length limit!</font>";
                    }
                } else {
                    img2.src = "img/error.png";
                    img2.style.visibility = 'visible';
                    div2.innerHTML = "<font color=red>The password can only contain numbers and English!</font>";
                }
            } else {
                img2.src = "img/error.png";
                img2.style.visibility = 'visible';
                div2.innerHTML = "<font color=red>Please enter your new password!</font>";
            }
        }
        
        function ReChkPassword() {
            img3 = document.getElementById("img3");
            img3.style.visibility = 'visible';
            img3.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div3 = document.getElementById("msg3");
            var password = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("NewPassword").ClientID %>').value;
            var Rpassword = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Reconfirm").ClientID %>').value;
            if (Rpassword != '') {
                var regex1 = /^\w+$/;
                if (regex1.test(Rpassword) == true) {
                    if (Rpassword == password) {
                        img3.src = "img/success.png";
                        img3.style.visibility = 'visible';
                        div3.innerHTML = "";
                    } else {
                        img3.src = "img/error.png";
                        img3.style.visibility = 'visible';
                        div3.innerHTML = "<font color=red>Confirm password wrong!</font>";
                    }
                } else {
                    img3.src = "img/error.png";
                    img3.style.visibility = 'visible';
                    div3.innerHTML = "<font color=red>The password can only contain numbers and English!</font>";
                }
                
            } else {
                img3.src = "img/error.png";
                img3.style.visibility = 'visible';
                div3.innerHTML = "<font color=red>Please enter your new password!</font>";
            }
        }
        function chkAll() {
            chkNewPassword();
            chkOldPassword();
            ReChkPassword();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="Change your password" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                <td class="style1" align="right">Old password
                                </td>
                                <td class="style2">
                                    <asp:TextBox ID="OldPassword" runat="server" TextMode="Password" Width="180px" onblur="chkOldPassword()"></asp:TextBox>
                                    <img id="img1" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg1">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="OldPassword_empty" runat="server" ErrorMessage="The old password is not empty"
                                    ControlToValidate="OldPassword" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Password format error"
                                    ValidationExpression="^\w{6,15}$" ControlToValidate="OldPassword" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">New password
                                </td>
                                <td class="style2">
                                    <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" Width="180px" onblur="chkNewPassword()"></asp:TextBox>
                                    <img id="img2" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg2">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="The new password is not empty"
                                    ControlToValidate="NewPassword" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Password format error"
                                    ValidationExpression="^\w{6,15}$" ControlToValidate="NewPassword" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">Confirm the new password
                                </td>
                                <td class="style2">
                                    <asp:TextBox ID="Reconfirm" runat="server" TextMode="Password" Width="180px" onblur="ReChkPassword()"></asp:TextBox>
                                    <img id="img3" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg3">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="The new password is not empty"
                                    ControlToValidate="Reconfirm" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords are different" ControlToCompare="NewPassword"
                                    ControlToValidate="Reconfirm" Display="None"></asp:CompareValidator>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="submit_btn" runat="server" Text="Confirm" OnClientClick="chkAll()" />
                                    <asp:Button ID="cancel_btn" runat="server" Text="Cancel" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </td>
            </tr>
        </table>
        
	</div>
</asp:Content>

