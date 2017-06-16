<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserTable.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="css/home.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />
    <script src="js/alertify.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 80px;
            height: 35px;
            padding-right:10px;
        }
        .style2
        {
            width: 270px;
            height: 35px;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#<%=cancel_btn.ClientID%>").click(function () {
                parent.$.fn.colorbox.close();
            });
        });

        function custom_script(sender) {
            alertify.alert(sender, function (e) {
                if (e) {
                    parent.$.fn.colorbox.close();
                } else {
                    // user clicked "cancel"
                }
            });
        }
        var xmlHttp;
        function chkPassword() {
            img = document.getElementById("img_password");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_password");
            var password = document.getElementById('<%= Page.FindControl("Password").ClientID %>').value;
            if (password != '') {
                var regex1 = /^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$/;
                if (regex1.test(password) == true) {
                    var regex2 = /^.{6,15}$/;
                    if (regex2.test(password) == true) {
                        img.src = "img/success.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "";
                    } else {
                        img.src = "img/error.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "<font color=red>密碼字數不符，長度限制 6 ~ 15 字元</font>";
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>密碼不允許特殊符號、數字、英文字母以外的字元</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>密碼不可為空</font>";
            }    
        }
        function chkGroupList() {
            img = document.getElementById("img_grouplist");
            img2 = document.getElementById("img_group");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_grouplist");
            var group = document.getElementById('<%= Page.FindControl("group_txt").ClientID%>');
            var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
            if (e.options[e.selectedIndex].value != "請選擇") {
                img.style.visibility = 'hidden';
                img2.style.visibility = 'hidden';
                msg.innerHTML = "";
                group.innerText = "";
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>請選擇群組</font>";
            }
        }
        function chkGroup() {
            img = document.getElementById("img_group");
            img2 = document.getElementById("img_grouplist");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_group");
            var msg2 = document.getElementById("msg_grouplist");
            var group = document.getElementById('<%= Page.FindControl("group_txt").ClientID%>').value;
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
                            msg.innerHTML = "<font color=red>群組重覆</font>";
                            break;
                        }
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>群組含不合法字元</font>";
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
            var company = document.getElementById('<%= Page.FindControl("Company").ClientID %>').value;
            if (company != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;  //判斷英文數字中文
                if (regex.test(company) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>公司名稱只能含中文英文數字</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>公司名稱不可為空</font>";
            }
        }
        function chkEmail() {
            img = document.getElementById("img_email");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_email");
            var email = document.getElementById('<%= Page.FindControl("E_Mail").ClientID %>').value;
            if (email != '') {
                var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                if (regex.test(email) == true) {
                      img.src = "img/success.png";
                      img.style.visibility = 'visible';
                      msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>電子郵件格式錯誤</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                msg.innerHTML = "<font color=red>電子郵件不可為空</font>";
            }
        }
        function chkName() {
            img = document.getElementById("img_name");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_name");
            var name = document.getElementById('<%= Page.FindControl("Name").ClientID %>').value;
            if (name != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
                if (regex.test(name) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>姓名只能含中文英文數字</font>";
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
            var address = document.getElementById('<%= Page.FindControl("Address").ClientID %>').value;
            if (address != '') {
                var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+='" ])*$/;
                if (regex.test(address) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>地址只能含中文英文數字</font>";
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
            var tel = document.getElementById('<%= Page.FindControl("Tel").ClientID %>').value;
            if (tel != '') {
                var regex = /^([0-9]+[^@#$%^&+=_ ])*$/;
                if (regex.test(tel) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>通訊電話只能含數字</font>";
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
            var mobile = document.getElementById('<%= Page.FindControl("Mobile").ClientID %>').value;
            if (mobile != '') {
                var regex = /^([0-9]+[^@#$%^&+=_ ])*$/;
                if (regex.test(mobile) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>行動電話只能含數字</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
        function chkAll() {
            chkPassword();
            //chkAddress();
            chkGroup();
            chkCompany();
            chkEmail();
            //chkMobile();
            //chkName();
            //chkTel();
        }
        var showalert = true;
        function attention() {
            if (showalert) {
                alertify.alert("變更電子郵件需重新驗證，請至變更後電子郵件重新啟用帳號");
                showalert = false;
            }
            
        }
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
            var img = document.getElementById("img");
            var fontColor;
            var msg = document.getElementById("msg5");

            if (isValid == "true" || isValid == "True") {
                fontColor = "green";
                if (field == "email") {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                }
            }
            else {
                fontColor = "red";
                if (field == "email") {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=" + fontColor + ">" + message + " </font>";
                }
            }
        }
    </script>
     
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding-top: 5px; padding-left: 5px;">
            <asp:Panel ID="Panel1" runat="server" Width="390px">
                <fieldset>
                    <legend>帳號資訊</legend>
                    <table>
                        <tr>
                            <td class="style1" align="right">會員帳號
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Account" runat="server" ReadOnly="true" Width="230px" Enabled="False"></asp:TextBox>
                                <img id="img1" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" align="right">*會員密碼
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Password" runat="server" Width="230px" onblur="chkPassword()"></asp:TextBox>
                                <img id="img_password" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                <div id="msg_password" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="密碼驗證錯誤"
                                ValidationExpression="^\w{6,15}$" ControlToValidate="Password" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">*群組
                            </td>
                            <td align="left" style="width:110px;padding-left:0px;">
                                <asp:DropDownList ID="Group_DropDownList" runat="server" onblur="chkGroupList()" Height="22px">
                                </asp:DropDownList>
                                <img id="img_grouplist" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                <div id="msg_grouplist" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <td align="left" style="padding-right: 0px;">
                                <asp:Label ID="Label1" runat="server" Text="新群組" Font-Size="11pt"></asp:Label>
                                <asp:TextBox ID="group_txt" runat="server" Width="70px" onblur="chkGroup()"></asp:TextBox>
                                <img id="img_group" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                <div id="msg_group" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" align="right">姓名
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Name" runat="server" Width="230px" onblur="chkName()"></asp:TextBox>
                                <img id="img_name" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_name" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="姓名只能含中文英文數字！"
                                ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="Name" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">*公司名稱
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Company" runat="server" Width="230px" onblur="chkCompany()"></asp:TextBox>
                                <img id="img_company" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_company" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RequiredFieldValidator ID="company_empty" runat="server" ErrorMessage="公司名稱不可為空"
                                ControlToValidate="Company" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="公司名稱只能含中文英文數字！"
                                ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="Company" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">地址
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Address" runat="server" Width="230px" onblur="chkAddress()"></asp:TextBox>
                                <img id="img_address" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_address" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="地址只能含中文英文數字！"
                                ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+=' ])*$" ControlToValidate="Address" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">通訊電話
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Tel" runat="server" Width="230px" onblur="chkTel()"></asp:TextBox>
                                <img id="img_tel" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_tel" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="通訊電話只能含數字！"
                                ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="Tel" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">行動電話
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="Mobile" runat="server" Width="230px" onblur="chkMobile()"></asp:TextBox>
                                <img id="img_mobile" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_mobile" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="行動電話只能含數字！"
                                ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="Mobile" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">*電子郵件
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="E_Mail" runat="server" Width="230px" onblur="chkEmail()" onfocus="attention()"></asp:TextBox>
                                <img id="img_email" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_email" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RequiredFieldValidator ID="email_empty" runat="server" ErrorMessage="電子郵件不可為空"
                                ControlToValidate="E_Mail" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="電子郵件驗證錯誤"
                                ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="E_Mail"
                                Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">*權限
                            </td>
                            <td colspan="2" class="style2" style="padding-left: 10px;">
                                <asp:RadioButtonList ID="Rank_RBList" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1">一般</asp:ListItem>
                                    <asp:ListItem Value="2">總管理</asp:ListItem>
                                    <asp:ListItem Value="0">停用</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left:110px;">
                                <asp:Label ID="DBtxt" runat="server" Text="資料庫已建立" Visible="False" Font-Bold="True" ForeColor="#000099"></asp:Label>
                                <asp:CheckBox ID="CreateDB_CB" runat="server" Text="建立資料庫" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div align="center" style="padding-top:10px;">
                    <asp:Button ID="update_btn" runat="server" Text="確認" OnClientClick="chkAll()" />
                    <asp:Button ID="cancel_btn" runat="server" Text="取消" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
