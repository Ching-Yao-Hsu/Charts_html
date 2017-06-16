<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="Account.aspx.vb" Inherits="Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style1
        {
            width: 80px;
            height: 35px;
            padding-left:30px;
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
        .auto-style1 {
            width: 118px;
            height: 35px;
            padding-left: 30px;
        }
    </style>
    
    <script type="text/javascript">

        $(document).ready(function () {
            var HrefAddress = "support/AccountSupport.htm";
            $('.support').colorbox({ innerWidth: 715, innerHeight: 290, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
        });
        var showalert = true;
        function attention() {
            var v = document.getElementById("<%=E_Mail.ClientID%>");
            v.style.backgroundColor = "#ffd";
            if (showalert) {
                //變更電子郵件位址需重新驗證，驗證信會寄至您填寫的電子郵件位址
                alertify.alert("Change email address to be re-verified verification letter will be sent to the email address you fill!");
                showalert = false;
            }
        }

        function chkEmail() {
            var v = document.getElementById("<%=E_Mail.ClientID%>");
            v.style.backgroundColor = "#eee";
            img = document.getElementById("img");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var div = document.getElementById("msg");
            createXMLHttpRequest(); //建立XMLHttpRequest物件
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("E_Mail").ClientID %>').value;
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
                    //電子郵件格式錯誤
                    div.innerHTML = "<font color=red>Email is malformed</font>";
                }
            } else {
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                //電子郵件不可為空
                div.innerHTML = "<font color=red>Email Address can not be empty</font>";
            }
        }
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
            var img = document.getElementById("img");
            var fontColor;
            var div = document.getElementById("msg");

            if (isValid == "true" || isValid == "True") {
                fontColor = "green";
                img.src = "img/success.png";
                img.style.visibility = 'visible';
                div.innerHTML = ""; //顯示是否有重複的帳號
            }
            else {
                fontColor = "red";
                img.src = "img/error.png";
                img.style.visibility = 'visible';
                div.innerHTML = "<font color=" + fontColor + ">" + message + " </font>";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    Account Information</td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff; width: 650px;">
                        <table>
                            <tr>
                                <td class="auto-style1"><span lang="EN-US" style="font-size:12.0pt;mso-bidi-font-size:
11.0pt;font-family:&quot;Calibri&quot;,&quot;sans-serif&quot;;mso-ascii-theme-font:minor-latin;
mso-fareast-font-family:新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-theme-font:
minor-latin;mso-bidi-font-family:&quot;Times New Roman&quot;;mso-bidi-theme-font:minor-bidi;
mso-ansi-language:EN-US;mso-fareast-language:ZH-TW;mso-bidi-language:AR-SA">Member Account</span></td>
                                <td class="style2" align="left">
                                    <asp:Label ID="AccountL" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Enable Time</td>
                                <td class="style2" align="left">
                                    <asp:Label ID="EnabledTime" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="auto-style1">E-Mail</td>
                                <td class="style2" align="left">
                                    <asp:TextBox ID="E_Mail" runat="server" onfocus="attention()" onblur="chkEmail()"
                                        Width="200px" ForeColor="Black"></asp:TextBox>
                                    <img id="img" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px" />
                                    <div style="width: auto; font-size: 12px" id="msg"></div>
                                </td>
                                <asp:RequiredFieldValidator ID="email_empty" runat="server" ErrorMessage="Email Address can not be empty" ControlToValidate="E_Mail" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Email Address Validation Error"
                                    ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ControlToValidate="E_Mail" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Did not receive verification letter？" ForeColor="#666666"></asp:Label>
                                    <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Welcome.aspx"
                                        Target="_blank" Font-Size="12px" Style="text-decoration: none;">resend verification letter</asp:HyperLink>--%>
                                    <asp:LinkButton ID="LinkButton1" runat="server" Style="text-decoration: none;" Font-Size="12pt"
                                    OnClientClick="window.open('Welcome.aspx','','menubar=no,status=no,scrollbars=yes,Resizable=yes,width=885px,height=550px').focus();">resend verification letter</asp:LinkButton>
                                </td>
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

