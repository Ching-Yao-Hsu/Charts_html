<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="NewUser.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style3
        {
            width: 150px;
            height: 35px;
            padding-right:20px;
        }
        .style4
        {
            width: 308px;
        }
        .support
        {
            outline: none;
            border: none;
        }
    </style>
    
    <script type="text/javascript">

        $(document).ready(function () {
            var HrefAddress = "support/NewUserSupport.htm";
            $('.support').colorbox({ innerWidth: 730, innerHeight: 625, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
        });

        var xmlHttp;
        //程式由此執行
        function chkAccount() {
            img = document.getElementById("img_account");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_account");
            createXMLHttpRequest(); //建立XMLHttpRequest物件
            var account = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("account_txt").ClientID %>').value;
            if (account != '') {
                var regex1 = /^\w+([-+.]\w+)*$/;
                if (regex1.test(account) == true) {
                    var regex2 = /^(?=.{3,15}$)/;
                    if (regex2.test(account) == true) {
                        //var regex3 = /^[a-zA-Z]/;
                        //if (regex3.test(account) == true) {
                        var url = "Verification.aspx?account=" + account + "&type=NewUser&field=account";
                        xmlHttp.onreadystatechange = callback; //資料回傳之後，使用callback這個函數處理後續動作 
                        xmlHttp.open("POST", url, true); //將輸入的帳號傳至後端作驗證 
                        xmlHttp.send(null);
                        //}else{
                        //img_Prc.src = "img/error.png";
                        //img_Prc.style.visibility = 'visible';
                        //div.innerHTML = "<font color=red>帳號需以英文字母作為開頭</font>";
                        //}
                    } else {
                        img.src = "img/error.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "<font color=red>Account does not match the words, 3 to 15 characters in length restrictions</font>";
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Account characters does not allow special symbols, numbers, letters outside</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>Account can not be empty</font>";
            }
        }
        function chkPassword() {
            img = document.getElementById("img_password");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_password");
            //createXMLHttpRequest(); //建立XMLHttpRequest物件
            var password = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("password_txt").ClientID %>').value;
            if (password != '') {
                var regex1 = /^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$/;
                if (regex1.test(password) == true) {
                    var regex2 = /^.{5,15}$/;
                    if (regex2.test(password) == true) {
                        img.src = "img/success.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "";
                    } else {
                        img.src = "img/error.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "<font color=red>Password does not match the words, restricted to 15 characters in length</font>";
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Character password does not allow special symbols, numbers, letters outside</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>Password is not empty</font>";
            }
        }
        function ReChkPassword() {
            img = document.getElementById("img_Reconfirm");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_Reconfirm");
            //createXMLHttpRequest(); //建立XMLHttpRequest物件
            var password = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("password_txt").ClientID %>').value;
        var Rpassword = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Reconfirm_txt").ClientID %>').value;
            if (Rpassword != '') {
                if (Rpassword == password) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Confirm password error</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>Confirm password is not empty</font>";
            }
        }
        function chkGroupList() {
            img = document.getElementById("img_grouplist");
            img2 = document.getElementById("img_group");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_grouplist");
            var group = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("group_txt").ClientID%>');
            var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
            if (e.options[e.selectedIndex].value != "Please choose...") {
                img.style.visibility = 'hidden';
                img2.style.visibility = 'hidden';
                msg.innerHTML = "";
                group.innerText = "";
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>Please select Group</font>";
            }
        }
        function chkGroup() {
            img = document.getElementById("img_group");
            img2 = document.getElementById("img_grouplist");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_group");
            var msg2 = document.getElementById("msg_grouplist");
            var group = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("group_txt").ClientID%>').value;
            if (group != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
                if (regex.test(group) == true) {
                    var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
                    for (var i = 0; i < e.length; i++) {
                        if (e.options[i].value != group) {
                            img.src = "img/success.png";
                            img.style.visibility = 'visible';
                            img2.style.visibility = 'hidden';
                            msg.innerHTML = "";
                            msg2.innerHTML = "";
                        } else {
                            img.src = "img/error.png";
                            img.style.visibility = 'visible';
                            msg.innerHTML = "<font color=red>Repeat Groups</font>";
                            break;
                        }
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Groups containing illegal characters</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
        function chkCompany() {
            img = document.getElementById("img_company");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_company");
            var company = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("company_txt").ClientID %>').value;
            if (company != '') {
                var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+='" ])*$/;  //判斷英文數字中文
                if (regex.test(company) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Company name contains illegal characters</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>Company name can not be empty</font>";
            }
        }

        function chkEmail() {
            img = document.getElementById("img_email");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_email");
            createXMLHttpRequest(); //建立XMLHttpRequest物件
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("email_txt").ClientID %>').value;
            if (email != '') {
                var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                if (regex.test(email) == true) {
                    var url = "Verification.aspx?email=" + email + "&type=NewUser&field=email";
                    xmlHttp.onreadystatechange = callback; //資料回傳之後，使用callback這個函數處理後續動作 
                    xmlHttp.open("POST", url, true); //將輸入的密碼傳至後端作驗證 
                    xmlHttp.send(null);
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Email format error</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>Email can not be empty</font>";
            }
        }
        function chkName() {
            img = document.getElementById("img_name");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_name");
            var name = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("name_txt").ClientID %>').value;
            if (name != '') {
                var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+=_'" ])*$/;
                if (regex.test(name) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Name contains illegal characters</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }

        function chkAddress() {
            img = document.getElementById("img_address");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_address");
            var address = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("address_txt").ClientID %>').value;
            if (address != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
                if (regex.test(address) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Address contains illegal characters</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }

        function chkTel() {
            img = document.getElementById("img_tel");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_tel");
            var tel = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("tel_txt").ClientID %>').value;
            if (tel != '') {
                var regex = /^([0-9]+[^@#$%^&+=_ ])*$/;
                if (regex.test(tel) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Telephone number containing illegal characters</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }

        function chkMobile() {
            img = document.getElementById("img_mobile");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_mobile");
            var mobile = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("mobile_txt").ClientID %>').value;
            if (mobile != '') {
                var regex = /^([0-9]+[^@#$%^&+=_ ])*$/;
                if (regex.test(mobile) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Cell phone number containing illegal characters</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
        function chkAll() {
            chkAccount();
            chkPassword();
            ReChkPassword();
            chkGroup();
            chkGroupList()
            chkCompany();
            chkEmail();
            chkAddress();
            chkMobile();
            chkName();
            chkTel();
        }
        //xmlHttp.open(a,b,c) 
        //第一個參數是 HTTP request 的方法，也就是從 GET、POST、HEAD 中選一個使用,都要大寫 
        //第二個參數是要呼叫的url,不過只能呼叫同一個網域的網頁 
        //第三個參數決定此 request 是否不同步進行 
        //如果設定為true則即使後端尚未傳回資料也會繼續執行後面的程式 
        //如果設定為false則必須等後端傳回資料才會繼續執行後面的程式 

        //此函式在建立XMLHttpRequest物件 
        function createXMLHttpRequest() {
            if (window.ActiveXObject) {//IE 
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            else if (window.XMLHttpRequest) {//other browser 
                xmlHttp = new XMLHttpRequest();
            }
        }
        function callback() {
            if (xmlHttp.readyState == 4) {
                if (xmlHttp.status == 200) {
                    var xmldoc = xmlHttp.responseXML; //接收後端程式傳回來的xml 
                    var mes = xmldoc.getElementsByTagName("message")[0].firstChild.data; //將Tag 為message的值抓出來
                    var val = xmldoc.getElementsByTagName("passed")[0].firstChild.data; //將Tag 為passed的值抓出來
                    var field = xmldoc.getElementsByTagName("field")[0].firstChild.data; //將Tag 為field的值抓出來
                    setMessage(mes, val, field);
                }
            }
        }
        //xmlHttp.readyState 所有可能的值： 0 (還沒開始), 1 (讀取中), 2 (已讀取), 3 (資訊交換中), 4 (一切完成) 
        //xmlHttp.status = 200 (一切正常), status = 404 (查無此頁), status =500 (內部錯誤)

        function setMessage(message, isValid, field) {
            var img_account = document.getElementById("img_account");
            var img_group = document.getElementById("img_group");
            var img_email = document.getElementById("img_email");
            var msg_account = document.getElementById("msg_account");
            var msg_group = document.getElementById("msg_group");
            var msg_email = document.getElementById("msg_email");
            var fontColor;

            if (isValid == "true" || isValid == "True") {
                fontColor = "green";
                if (field == "account") {
                    img_account.src = "img/success.png";
                    img_account.style.visibility = 'visible';
                    msg_account.innerHTML = ""; //顯示是否有重複的帳號
                } else if (field == "group") {
                    img_group.src = "img/success.png";
                    img_group.style.visibility = 'visible';
                    msg_group.innerHTML = "";
                } else if (field == "email") {
                    img_email.src = "img/success.png";
                    img_email.style.visibility = 'visible';
                    msg_email.innerHTML = "";
                }
            }
            else {
                fontColor = "red";
                if (field == "account") {
                    img_account.src = "img/error.png";
                    img_account.style.visibility = 'visible';
                    msg_account.innerHTML = "<font color=" + fontColor + ">" + message + " </font>";
                } else if (field == "group") {
                    img_group.src = "img/error.png";
                    img_group.style.visibility = 'visible';
                    msg_group.innerHTML = "<font color=" + fontColor + ">" + message + " </font>";
                } else if (field == "email") {
                    img_email.src = "img/error.png";
                    img_email.style.visibility = 'visible';
                    msg_email.innerHTML = "<font color=" + fontColor + ">" + message + " </font>";
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="New Member" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff; width: 600px;">
                        <table style="width:600px">
                            <tr>
                                <td rowspan="12" valign="top" style="padding-top: 0px; padding-left: 5px" width="70px"></td>
                                <td align="right" class="style3">
                                    <asp:Label ID="account" runat="server" Text="*Member Account"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4" style="word-break: keep-all; overflow: hidden;">
                                    <asp:TextBox ID="account_txt" runat="server" Width="230px" onblur="chkAccount()"></asp:TextBox>
                                    <img id="img_account" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_account" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="account_empty" runat="server" ErrorMessage="Member Account can not be empty"
                                    ControlToValidate="account_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Account must be in English letters as the beginning, can only contain numbers and English, 3 to 15 characters in length restrictions"
                                    ValidationExpression="^.*(?=.*\w+([-+.]\w+)*)(?=.{2,14}).*$" ControlToValidate="account_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3" width="30">
                                    <asp:Label ID="password" runat="server" Text="*Member Password"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="password_txt" runat="server" TextMode="Password" Width="230px" onblur="chkPassword()"></asp:TextBox>
                                    <img id="img_password" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_password" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="password_empty" runat="server" ErrorMessage="Password is not empty"
                                    ControlToValidate="password_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Password validation error"
                                    ValidationExpression="^\w{5,15}$" ControlToValidate="password_txt" Display="None"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage="Password validation error"
                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="password_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="Reconfirm" runat="server" Text="*Password Confirmation"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="Reconfirm_txt" runat="server" TextMode="Password" Width="230px"
                                        onblur="ReChkPassword()"></asp:TextBox>
                                    <img id="img_Reconfirm" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_Reconfirm" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="Reconfirm_empty" runat="server" ErrorMessage="Passwords are different"
                                    ControlToValidate="Reconfirm_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords are different" ControlToCompare="password_txt"
                                    ControlToValidate="Reconfirm_txt" Display="None"></asp:CompareValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="group" runat="server" Text="*Groups"></asp:Label>
                                </td>
                                <td align="left" style="width:110px">
                                    <asp:DropDownList ID="Group_DropDownList" runat="server" onblur="chkGroupList()" Height="22px">
                                    </asp:DropDownList>
                                    <img id="img_grouplist" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_grouplist" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <td align="left" style="padding-right:0px;">
                                    <asp:Label ID="Label1" runat="server" Text="New Groups"></asp:Label>
                                    <asp:TextBox ID="group_txt" runat="server" Width="70px" onblur="chkGroup()"></asp:TextBox>
                                    <img id="img_group" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_group" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage="Groups can only contain Chinese, English and numbers!!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="group_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="name" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="name_txt" runat="server" Width="230px" onblur="chkName()"></asp:TextBox>
                                    <img id="img_name" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_name" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Name can only contain Chinese, English and numbers!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="name_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="company" runat="server" Text="*Company name"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="company_txt" runat="server" Width="230px" onblur="chkCompany()"></asp:TextBox>
                                    <img id="img_company" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_company" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="company_empty" runat="server" ErrorMessage="Company name can not be empty"
                                    ControlToValidate="company_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="Company Name can only contain Chinese, English and numbers!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="company_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="address" runat="server" Text="Address"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="address_txt" runat="server" Width="230px" onblur="chkAddress()"></asp:TextBox>
                                    <img id="img_address" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_address" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Address can only contain Chinese, English and numbers!"
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="address_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style3" align="right">
                                    <asp:Label ID="tel" runat="server" Text="Telephone number"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="tel_txt" runat="server" Width="230px" onblur="chkTel()"></asp:TextBox>
                                    <img id="img_tel" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_tel" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Telephone number can only contain numbers"
                                    ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="tel_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style3" align="right">
                                    <asp:Label ID="mobile" runat="server" Text="Cell phone number"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="mobile_txt" runat="server" Width="230px" onblur="chkMobile()"></asp:TextBox>
                                    <img id="img_mobile" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_mobile" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="Cell phone number can only contain numbers"
                                    ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="mobile_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="style3" align="right">
                                    <asp:Label ID="email" runat="server" Text="*E-Mail"></asp:Label>
                                </td>
                                <td colspan="2" align="left" class="style4">
                                    <asp:TextBox ID="email_txt" runat="server" Width="230px" onblur="chkEmail()"></asp:TextBox>
                                    <img id="img_email" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_email" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="email_empty" runat="server" ErrorMessage="ail can not be empty"
                                    ControlToValidate="email_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Email validation error"
                                    ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="email_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="rank" runat="server" Text="*Competence"></asp:Label>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:RadioButtonList ID="rank_RBList" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Selected="True">General</asp:ListItem>
                                        <asp:ListItem Value="2">General Management</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                
                                <asp:RequiredFieldValidator ID="rank_empty" runat="server" ErrorMessage="Competence can not be empty"
                                    ControlToValidate="rank_RBList" Display="None"></asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="left" colspan="2" style="padding-left:5px;">
                                    <asp:CheckBox ID="CreateDB_CB" runat="server" Text="Creating a database" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="padding-top:10px;">
                                    <asp:Button ID="submit_btn" runat="server" Text="Confirm" OnClientClick="chkAll()" />
                                    &nbsp;&nbsp;&nbsp;
	                                <asp:Button ID="cancel_btn" runat="server" Text="Cancel" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
	</div>
</asp:Content>

