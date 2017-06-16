<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="EcoInfo.aspx.vb" Inherits="EcoInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    
    <%--select Gridview row--%>
    <script type="text/javascript">
        
        $(document).ready(function () {
            var HrefAddress = "support/EcoInfoSupport.htm";
            $('.support').colorbox({ innerWidth: 715, innerHeight: 635, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            $('.imageButtonFinderClass').click(function() {
                var $tr = $(this).parent().parent();
                var eco_account = $tr.find("td").eq(1).text();
                var enabled = $tr.find("td").eq(6).text();
                var HrefAddress = "EcoTable.aspx?eco_account=" + eco_account;
                $('.imageButtonFinderClass').colorbox({ innerWidth: 500, innerHeight: 480, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress, onClosed: reloadPage });
            });
        });
        function reloadPage() {
            //colorbox關閉，刷新母網頁
            parent.location.reload();
        }
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
        function chkPassword() {
            img = document.getElementById("img_password");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_password");
            //createXMLHttpRequest(); //建立XMLHttpRequest物件
            var password = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("eco5_password").ClientID %>').value;
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
	                    msg.innerHTML = "<font color=red>密碼字數不符，長度限制 6 字元</font>";
	                }
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>密碼只能含有數字與英文</font>";
	            }
	        } else {
	            img.src = "img/error.png";
	            img.style.visibility = 'visible';
	            msg.innerHTML = "<font color=red>密碼不可為空</font>";
	        }
        }
        function chkPosition() {
            img = document.getElementById("img_position");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_position");
            var Position = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("InstallPosition").ClientID %>').value;
            if (Position != '') {
                var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$/;
                if (regex.test(Position) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>位置含不合法字元</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
        function chkDrawnr() {
            img = document.getElementById("img_drawnr");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg_drawnr");
            var Drawnr = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("drawnr").ClientID %>').value;
            if (Drawnr != '') {
                var regex = /^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$/;
                if (regex.test(Drawnr) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>圖面編號含不合法字元</font>";
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
	                msg.innerHTML = "<font color=red>IP含不合法字元</font>";
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
	                    msg.innerHTML = "<font color=red>電話號碼不可超過10組</font>";
	                }
	            } else {
	                img.src = "img/error.png";
	                img.style.visibility = 'visible';
	                msg.innerHTML = "<font color=red>電話號碼含不合法字元</font>";
	            }
	        } else {
	            img.style.visibility = 'hidden';
	            msg.innerHTML = "";
	        }
        }
        function chkEmail() {
            img = document.getElementById("img_mail");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_mail");
            var email = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("mail_txt").ClientID%>').value;
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
    <style type="text/css">
        .title_div
        {
            line-height: 25px;
            width: 120px;
            text-align: center;
        }

        .title_td
        {
            border: 1px solid #a09ac1;
            padding: 10px;
        }

        .td_width
        {
            height: 35px;
            width: 80px;
        }

        .auto-style1
        {
            width: 75px;
            height: 29px;
        }

        .auto-style2
        {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="ECO5帳號設定" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td nowrap="nowrap" colspan="2" align="left">
                                        <img src="img/icon-account.png" />群組
                                        <asp:DropDownList ID="Group_DropDownList" runat="server" Width="150px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <img id="img_account" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                        <div id="msg_account" style="width: auto; font-size: 12px;padding-left:92px;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <div style="border-radius: 15px 15px 15px 15px; background-color: #eaf8c7; height:620px">
                                            <table>
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        <div class="title_div">
                                                            <asp:Label ID="Label3" runat="server" Text="ECO5資訊" Font-Bold="True" Font-Names="微軟正黑體" Font-Size="Medium"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:Button ID="edit_btn" runat="server" Text="修改" OnClientClick="chkDrop()" />
                                                        <asp:Button ID="submit_btn" runat="server" Text="確認" Enabled="False" />
                                                        <asp:Button ID="cancel_btn" runat="server" Text="取消" Enabled="False" CausesValidation="False" />
                                                        <asp:Button ID="delete_btn" runat="server" Text="刪除" Visible="False" OnClientClick="return confirm('確認要刪除嗎？包括其電表資料一併刪除。');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="td_width" align="right">編號</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="eco5_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                </td>
                                                                <td class="td_width" align="right">型號</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="eco5_type" runat="server" Enabled="False" Width="80px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" class="td_width">帳號</td>
                                                                <td align="left" width="70px">
                                                                    <asp:TextBox ID="eco5_account" runat="server" BackColor="White" Width="120px" ForeColor="#000099" Enabled="False"></asp:TextBox></td>
                                                                <td align="right" class="td_width">密碼</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="eco5_password" runat="server" Enabled="False" Width="50px" BackColor="White" ForeColor="Black" onblur="chkPassword()"></asp:TextBox>
                                                                    <img id="img_password" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_password" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RequiredFieldValidator ID="password_empty" runat="server" ErrorMessage="ECO5密碼不可為空"
                                                                    ControlToValidate="eco5_password" Display="None"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="密碼驗證錯誤"
                                                                    ValidationExpression="^\w{6}$" ControlToValidate="eco5_password" Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right">圖面編號</td>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="drawnr" runat="server" Width="150px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkDrawnr()"></asp:TextBox>
                                                                    <img id="img_drawnr" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_drawnr" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$" ControlToValidate="drawnr"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right" nowrap="nowrap">*安裝位置</td>
                                                                <td colspan="3" class="auto-style2" align="left">
                                                                    <asp:TextBox ID="InstallPosition" runat="server" Width="150px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkPosition()"></asp:TextBox>
                                                                    <img id="img_position" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_position" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$" ControlToValidate="InstallPosition"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right">*IP位置</td>
                                                                <td colspan="3" class="auto-style2" align="left">
                                                                    <asp:TextBox ID="IP_txt" runat="server" Width="150px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkIP()"></asp:TextBox>
                                                                    <img id="img_ip" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_ip" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^([0-9]+[^@#$%^&=-_ ])*$" ControlToValidate="IP_txt"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" style="padding-left: 10px;">
                                                                    <asp:CheckBox ID="eco_enabled" runat="server" Enabled="False" Text="啟用ECO5" />
                                                                </td>
                                                                <td class="td_width" align="right">時差</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="DiffTime_txt" runat="server" Enabled="False" Width="40px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" align="left" style="padding-left: 10px;">
                                                                    <asp:CheckBox ID="sendnum_enabled" runat="server" Enabled="False" Text="事件電話發送" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right">電話號碼</td>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="num_txt" runat="server" Width="250px" Height="50px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkNum()" TextMode="MultiLine"></asp:TextBox>
                                                                    <img id="img_num" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_num" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="num_txt"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" align="left" style="padding-left: 10px;">
                                                                    <asp:CheckBox ID="sendmail_enabled" runat="server" Enabled="False" Text="事件信件發送" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right">電子郵件</td>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="mail_txt" runat="server" Width="250px" Height="50px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkEmail()" TextMode="MultiLine"></asp:TextBox>
                                                                    <img id="img_mail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_mail" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="mail_txt"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" align="left" style="padding-left: 10px;">
                                                                    <asp:CheckBox ID="sendday_enabled" runat="server" Enabled="False" Text="寄送日報表" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right">電子信箱
                                                                </td>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="daymail_txt" runat="server" Width="250px" Height="50px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkDayEmail()" TextMode="MultiLine"></asp:TextBox>
                                                                    <img id="img_daymail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_daymail" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="daymail_txt"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <%--<tr>
                                                                <td colspan="4" align="left" style="padding-left: 10px;">
                                                                    <asp:CheckBox ID="sendweek_enabled" runat="server" Enabled="False" Text="寄送週報表" />
                                                                </td>
                                                            </tr>--%>
                                                            <%--<tr>
                                                                <td class="td_width" align="right">電子信箱
                                                                </td>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="weekmail_txt" runat="server" Width="250px" Height="50px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkWeekEmail()" TextMode="MultiLine"></asp:TextBox>
                                                                    <img id="img_weekmail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_weekmail" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="weekmail_txt"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>--%>
                                                            <tr>
                                                                <td colspan="4" align="left" style="padding-left: 10px;">
                                                                    <asp:CheckBox ID="sendmonth_enabled" runat="server" Enabled="False" Text="寄送月報表" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_width" align="right">電子信箱
                                                                </td>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="monthmail_txt" runat="server" Width="250px" Height="50px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkMonthEmail()" TextMode="MultiLine"></asp:TextBox>
                                                                    <img id="img_monthmail" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                    <div id="msg_monthmail" style="width: auto; font-size: 12px">
                                                                    </div>
                                                                </td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage=""
                                                                    ValidationExpression="^(?!.*[^\x21-\x7e])(?!.*[\[+|\]+$|[\[\]\x22\x27]).*$" ControlToValidate="monthmail_txt"
                                                                    Display="None"></asp:RegularExpressionValidator>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" align="left" style="padding-left: 10px;">　</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td align="left" valign="top">
                                        <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff;">
                                            <table>
                                                <tr>
                                                    <td valign="middle">
                                                        <div class="title_div">
                                                            <asp:Label ID="Label1" runat="server" Text="ECO5列表" Font-Bold="True" ForeColor="#CC0000" Font-Size="Medium" Font-Names="微軟正黑體"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel1" runat="server" Height="585px" Width="350px" Direction="LeftToRight" ScrollBars="Auto">
                                                            <asp:TreeView ID="Ctrl_TreeView" runat="server" ShowLines="True" ForeColor="Black" Font-Names="微軟正黑體">
                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                            </asp:TreeView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Group_DropDownList" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </asp:Content>

