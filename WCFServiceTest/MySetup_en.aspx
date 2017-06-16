<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="MySetup.aspx.vb" Inherits="MySetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style1
        {
            width: 150px;
            height: 35px;
            padding-left: 30px;
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
            var HrefAddress = "support/MySetupSupport.htm";
            $('.support').colorbox({ innerWidth: 720, innerHeight: 345, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
        });

        function chkName() {
            img1 = document.getElementById("img1");
            img1.style.visibility = 'visible';
            img1.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div1 = document.getElementById("msg1");
            var name = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Name").ClientID %>').value;
            if (name != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
                if (regex.test(name) == true) {
                    img1.src = "img/success.png";
                    img1.style.visibility = 'visible';
                    div1.innerHTML = "";
                } else {
                    img1.src = "img/error.png";
                    img1.style.visibility = 'visible';
                    div1.innerHTML = "<font color=red>Name can only contain Chinese, English and numbers!</font>";
                }
            } else {
                img1.style.visibility = 'hidden';
                div1.innerHTML = "";
            }
        }

        function chkCompany() {
            img2 = document.getElementById("img2");
            img2.style.visibility = 'visible';
            img2.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div2 = document.getElementById("msg2");
            var company = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Company").ClientID %>').value;
            if (company != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;  //判斷英文數字中文
                if (regex.test(company) == true) {
                    img2.src = "img/success.png";
                    img2.style.visibility = 'visible';
                    div2.innerHTML = "";
                } else {
                    img2.src = "img/error.png";
                    img2.style.visibility = 'visible';
                    div2.innerHTML = "<font color=red>Company Name can only contain Chinese, English and numbers!</font>";
                }
            } else {
                img2.src = "img/error.png";
                img2.style.visibility = 'visible';
                div2.innerHTML = "<font color=red>Company name can not be empty!</font>";
            }
        }

        function chkAddress() {
            img3 = document.getElementById("img3");
            img3.style.visibility = 'visible';
            img3.src = "img/Processing.jpg";
            var div3 = document.getElementById("msg3");
            var address = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Address").ClientID %>').value;
            if (address != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
                if (regex.test(address) == true) {
                    img3.src = "img/success.png";
                    img3.style.visibility = 'visible';
                    div3.innerHTML = "";
                } else {
                    img3.src = "img/error.png";
                    img3.style.visibility = 'visible';
                    div3.innerHTML = "<font color=red>Address can only contain Chinese, English and numbers!</font>";
                }
            } else {
                img3.style.visibility = 'hidden';
                div3.innerHTML = "";
            }
        }
        function chkTel() {
            img4 = document.getElementById("img4");
            img4.style.visibility = 'visible';
            img4.src = "img/Processing.jpg";
            var div4 = document.getElementById("msg4");
            var tel = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Tel").ClientID %>').value;
            if (tel != '') {
                var regex = /^[0-9]*$/;
                if (regex.test(tel) == true) {
                    img4.src = "img/success.png";
                    img4.style.visibility = 'visible';
                    div4.innerHTML = "";
                } else {
                    img4.src = "img/error.png";
                    img4.style.visibility = 'visible';
                    div4.innerHTML = "<font color=red>Telephone number can only contain numbers!</font>";
                }
            } else {
                img4.style.visibility = 'hidden';
                div4.innerHTML = "";
            }
        }
        function chkMobile() {
            img5 = document.getElementById("img5");
            img5.style.visibility = 'visible';
            img5.src = "img/Processing.jpg";
            var div5 = document.getElementById("msg5");
            var mobile = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Mobile").ClientID %>').value;
            if (mobile != '') {
                var regex = /^[0-9]*$/;
                if (regex.test(mobile) == true) {
                    img5.src = "img/success.png";
                    img5.style.visibility = 'visible';
                    div5.innerHTML = "";
                } else {
                    img5.src = "img/error.png";
                    img5.style.visibility = 'visible';
                    div5.innerHTML = "<font color=red>Cell phone number can only contain numbers!</font>";
                }
            } else {
                img5.style.visibility = 'hidden';
                div5.innerHTML = "";
            }
        }
        function chkAll() {
            chkName();
            chkCompany();
            chkAddress();
            chkTel();
            chkMobile();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="Personal information" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff; width: 650px;">
                        <div align="right" style="padding-right: 5px; padding-top: 5px;">
                        </div>
                        <table>
                            <tr>
                                <td class="style1" align="right">Name
                                </td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="Name" runat="server" onblur="chkName()"></asp:TextBox>
                                    <img id="img1" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg1">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Name can only contain Chinese, English and numbers!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="Name" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">Company Name
                                </td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="Company" runat="server" onblur="chkCompany()"></asp:TextBox>
                                    <img id="img2" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg2">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="company_empty" runat="server" ErrorMessage="Company name can not be empty"
                                    ControlToValidate="Company" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Company Name can only contain Chinese, English and numbers!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="Company" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">address
                                </td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="Address" runat="server" Width="220px" onblur="chkAddress()"></asp:TextBox>
                                    <img id="img3" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg3">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Address can only contain Chinese, English and numbers!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="Address" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">Telephone number
                                </td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="Tel" runat="server" onblur="chkTel()"></asp:TextBox>
                                    <img id="img4" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg4">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Telephone number can only contain numbers!"
                                    ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="Tel" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style1" align="right">Cell phone number
                                </td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="Mobile" runat="server" onblur="chkMobile()"></asp:TextBox>
                                    <img id="img5" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div style="width: auto; font-size: 12px" id="msg5">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Cell phone number can only contain numbers!"
                                    ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="Mobile" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="submit_btn" runat="server" Text="Confirm" />
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

