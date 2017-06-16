<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="NewEcoAccount.aspx.vb" Inherits="NewEcoAccount" %>

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
	        var HrefAddress = "support/NewEcoAccountSupport.htm";
	        $('.support').colorbox({ innerWidth: 730, innerHeight: 850, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
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
	            msg.innerHTML = "<font color=red>Please select Group</font>";
	        }
	    }
	    function chkAccount() {
	        img = document.getElementById("img_eco_account");
	        img.style.visibility = 'visible';
	        img.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var msg = document.getElementById("msg_eco_account");
	        createXMLHttpRequest(); //建立XMLHttpRequest物件
	        var EcoAccount = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("ecoAccount_txt").ClientID %>').value;
	        if (EcoAccount != '') {
	            var regex1 = /^\w+$/;
	            if (regex1.test(EcoAccount) == true) {
	                var regex2 = /^\w{15}$/;
	                if (regex2.test(EcoAccount) == true) {
	                    var url = "Verification.aspx?Account=" + EcoAccount + "&type=NewEcoAccount&field=account";
	                    xmlHttp.onreadystatechange = callback; //資料回傳之後，使用callback這個函數處理後續動作 
	                    xmlHttp.open("POST", url, true); //將輸入的帳號傳至後端作驗證 
	                    xmlHttp.send(null);
	                } else {
	                    img.src = "img/error.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "<font color=red>Account does not match the number of words, character length limit 15.</font>";
	                }
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>Account can only contain numbers and English.</font>";
	            }
	        } else {
	            img.src = "img/error.png";
	            img.style.visibility = 'visible';
	            msg.innerHTML = "<font color=red>Account can not be empty.</font>";
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
	            var regex1 = /^\w+$/;
	            if (regex1.test(password) == true) {
	                var regex2 = /^.{6}$/;
	                if (regex2.test(password) == true) {
	                    img.src = "img/success.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "";
	                } else {
	                    img.src = "img/error.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "<font color=red>Password does not match the number of words, character length limit 6.</font>";
	                }
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>Password can only contain numbers and English.</font>";
	            }
	        } else {
	            img.src = "img/error.png";
	            img.style.visibility = 'visible';
	            msg.innerHTML = "<font color=red>Password can not be empty.</font>";
	        }
	    }
	    function chkDrawnr() {
	        img = document.getElementById("img_drawnr");
	        img.style.visibility = 'visible';
	        img.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var msg = document.getElementById("msg_drawnr");
	        var Drawnr = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("drawnr_txt").ClientID %>').value;
	        if (Drawnr != '') {
	            var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$/;
	            if (regex.test(Drawnr) == true) {
	                img.src = "img/success.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "";
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>Drawing number containing illegal characters.</font>";
	            }
	        } else {
	            img.style.visibility = 'hidden';
	            msg.innerHTML = "";
	        }  
	    }
	    function chkPosition() {
	        img = document.getElementById("img_position");
	        img.style.visibility = 'visible';
	        img.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var msg = document.getElementById("msg_position");
	        var Position = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("position_txt").ClientID %>').value;
	        if (Position != '') {
	            var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$/;
	            if (regex.test(Position) == true) {
	                img.src = "img/success.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "";
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>Location contains illegal characters.</font>";
	            }
	        } else {
	            img.style.visibility = 'hidden';
	            msg.innerHTML = "";
	        } 
	    }
	    function chkIP() {
	        img = document.getElementById("img_ip");
	        img.style.visibility = 'visible';
	        img.src = "img/Processing.jpg";
	        var msg = document.getElementById("msg_ip");
	        var ip = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("IP_txt").ClientID %>').value;
	        if (ip != '') {
	            var regex = /^([0-9]+[^@#$%^&=-_ ])*$/;
	            if (regex.test(ip) == true) {
	                    img.src = "img/success.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "";
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>IP contains illegal characters.</font>";
	            }
	        } else {
	            img.style.visibility = 'hidden';
	            msg.innerHTML = "";
	        }
        }
	    function chkNum() {
	        img = document.getElementById("img_num");
	        img.style.visibility = 'visible';
	        img.src = "img/Processing.jpg";
	        var msg = document.getElementById("msg_num");
	        var mobile = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("num_txt").ClientID %>').value;
	        var count = mobile.split(",").length;
	        if (mobile != '') {
	            var regex = /^([^@#$%^&=-_ ]+[0-9])*$/;
	            if (regex.test(mobile) == true) {
	                if (count <= 10) {
	                    img.src = "img/success.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "";
	                } else {
	                    img.src = "img/error.png";
	                    img.style.visibility = 'visible';
	                    msg.innerHTML = "<font color=red>Phone number is not more than 10 group.</font>";
	                }
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>Telephone number contains illegal characters.</font>";
	            }
	        } else {
	            img.style.visibility = 'hidden';
	            msg.innerHTML = "";
	        }
	    }
	    function chkEmail() {
	        img8 = document.getElementById("img_mail");
	        img8.style.visibility = 'visible';
	        img8.src = "img/Processing.jpg";
	        var msg8 = document.getElementById("msg_mail");
	        var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("mail_txt").ClientID%>').value;
	        var count = email.split(";").length;
	        if (email != '') {
	            var regex = /^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$/;
	            if (regex.test(email) == true) {
	                if (count <= 10) {
	                    img8.src = "img/success.png";
	                    img8.style.visibility = 'visible';
	                    msg8.innerHTML = "";
	                } else {
	                    img8.src = "img/error.png";
	                    img8.style.visibility = 'visible';
	                    msg8.innerHTML = "<font color=red>Email is not more than 10 group.</font>";
	                }
	            } else {
	                img8.src = "img/error.png";
	                img8.style.visibility = 'visible';
	                msg8.innerHTML = "<font color=red>Email formatting errors.</font>";
	            }
	        } else {
	            img8.style.visibility = 'hidden';
	            msg8.innerHTML = "";
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
                        msg.innerHTML = "<font color=red>Email is not more than 10 group.</font>";
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Email formatting errors.</font>";
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
                        msg.innerHTML = "<font color=red>Email is not more than 10 group.</font>";
                    }
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>Email formatting errors.</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
	    function chkAll() {
	        chkAccount();
	        chkPassword();
	        chkDrop();
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
	        var img = document.getElementById("img_eco_account");
	        var fontColor;
	        var msg = document.getElementById("msg_eco_account");

	        if (isValid == "true" || isValid == "True") {
	            fontColor = "green";
	            if (field == "account") {
	                img.src = "img/success.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = ""; //顯示是否有重複的帳號
	            } 	            
	        }
	        else {
	            fontColor = "red";
	            if (field == "account") {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=" + fontColor + ">" + message + " </font>"; //顯示是否有重複的帳號
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
                    <asp:Label ID="Label5" runat="server" Text="Add ECO5 Account" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                <td align="right" class="style3" width="30">
                                    <asp:Label ID="account" runat="server" Text="*Group"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:DropDownList ID="Group_DropDownList" runat="server" Width="150px">
                                    </asp:DropDownList>
                                    <img id="img_account" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_account" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Group_DropDownList"
                                    ErrorMessage="Please choose..." InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3" width="30">
                                    <asp:Label ID="eco_account" runat="server" Text="*ECO5 Account"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="ecoAccount_txt" runat="server" Width="230px" onblur="chkAccount()"></asp:TextBox>
                                    <img id="img_eco_account" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_eco_account" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="ecoAccount_empty" runat="server" ErrorMessage="ECO5 Account can not be empty."
                                    ControlToValidate="ecoAccount_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter the 15 English characters or numbers."
                                    ValidationExpression="^\w{15}$" ControlToValidate="ecoAccount_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3" width="30">
                                    <asp:Label ID="password" runat="server" Text="*ECO5 password"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="password_txt" runat="server" Width="230px" onblur="chkPassword()"></asp:TextBox>
                                    <img id="img_password" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_password" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RequiredFieldValidator ID="password_empty" runat="server" ErrorMessage="ECO5 Password  can not be empty."
                                    ControlToValidate="password_txt" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Password validation error."
                                    ValidationExpression="^\w{6}$" ControlToValidate="password_txt" Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="Label3" runat="server" Text="ECO5 Model"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:DropDownList ID="EcoType_DropDownList" runat="server">
                                        <asp:ListItem>ECO5</asp:ListItem>
                                        <asp:ListItem>ECO5_Lite</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="ctrlnr" runat="server" Text="ECO5 Number"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:DropDownList ID="ctrlnr_DropDownList" runat="server">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="drawnr" runat="server" Text="Drawing No."></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="drawnr_txt" runat="server" Width="230px" onblur="chkDrawnr()"></asp:TextBox>
                                    <img id="img_drawnr" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_drawnr" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Drawing number can only contain English characters or numbers."
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$" ControlToValidate="drawnr_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="position" runat="server" Text="*Installation location"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="position_txt" runat="server" Width="230px" onblur="chkPosition()"></asp:TextBox>
                                    <img id="img_position" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_position" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Installation location can only contain English characters or numbers."
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$" ControlToValidate="position_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="ip" runat="server" Text="*IP Location"></asp:Label>
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="IP_txt" runat="server" Width="230px" onblur="chkIP()"></asp:TextBox>
                                    <img id="img_ip" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_ip" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage=""
                                    ValidationExpression="^([0-9]+[^@#$%^&=-_ ])*$" ControlToValidate="IP_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    <asp:Label ID="DiffTime_Lab" runat="server" Text="Time difference"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="DiffTime_txt" runat="server" Enabled="True" Width="40px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                    <asp:CheckBox ID="sendnum_enabled" runat="server" Text="Event sending by Phone" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 12px;">Telephone number
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="num_txt" runat="server" Width="230px" Height="50px" onblur="chkNum()" TextMode="MultiLine"></asp:TextBox>
                                    <img id="img_num" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_num" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage=""
                                    ValidationExpression="^([0-9]+[^@#$%^&+=_ ])*$" ControlToValidate="num_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                    <asp:CheckBox ID="sendmail_enabled" runat="server" Text="Event sending by Phone" />
                                </td>

                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 12px;">Email
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="mail_txt" runat="server" Width="230px" Height="50px" onblur="chkEmail()" TextMode="MultiLine"></asp:TextBox>
                                    <img id="img_mail" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_mail" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage=""
                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="mail_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                    <asp:CheckBox ID="sendday_enabled" runat="server" Text="Send Daily Report" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 12px;">Email
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="daymail_txt" runat="server" Width="230px" Height="50px" onblur="chkDayEmail()" TextMode="MultiLine"></asp:TextBox>
                                    <img id="img_daymail" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_daymail" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage=""
                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="daymail_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <%--<tr>
                                <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                    <asp:CheckBox ID="sendweek_enabled" runat="server" Text="寄送週報表" />
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td align="right" style="padding-right: 12px;">電子信箱
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="weekmail_txt" runat="server" Width="230px" Height="50px" onblur="chkWeekEmail()" TextMode="MultiLine"></asp:TextBox>
                                    <img id="img_weekmail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                    <div id="msg_weekmail" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage=""
                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="weekmail_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>--%>
                            <tr>
                                <td align="left" colspan="2" class="style3" style="padding-left: 8px;">
                                    <asp:CheckBox ID="sendmonth_enabled" runat="server" Text="Send Monthly Report" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 12px;">Email
                                </td>
                                <td class="style4" align="left">
                                    <asp:TextBox ID="monthmail_txt" runat="server" Width="230px" Height="50px" onblur="chkMonthEmail()" TextMode="MultiLine"></asp:TextBox>
                                    <img id="img_monthmail" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                    <div id="msg_monthmail" style="width: auto; font-size: 12px">
                                    </div>
                                </td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage=""
                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="monthmail_txt"
                                    Display="None"></asp:RegularExpressionValidator>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="padding-right: 50px; padding-top: 10px;">
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

