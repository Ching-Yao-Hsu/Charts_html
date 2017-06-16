<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WaterSetup.aspx.vb" Inherits="_Default" %>

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
        //ip與域名驗證函數 
        function checkIP() {
            img = document.getElementById("img_IP");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg";
            var msg = document.getElementById("msg_IP");
            var sIP = document.getElementById('<%= Page.FindControl("txtIP").ClientID %>').value;

            if (/[A-Za-z_-]/.test(sIP)) {
                if (sIP.indexOf(" ") >= 0) {
                    sIP = sIP.replace(/ /g, "");
                    document.getElementById('<%= Page.FindControl("txtIP").ClientID %>').value = sIP;
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                }
                if (sIP.toLowerCase().indexOf("http://") == 0) {
                    sIP = sIP.slice(7);
                    document.getElementById('<%= Page.FindControl("txtIP").ClientID %>').value = sIP;
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                }
                if (!/^([\w-]+\.)+((com)|(net)|(org)|(gov\.cn)|(info)|(cc)|(com\.cn)|(net\.cn)|(org\.cn)|(name)|(biz)|(tv)|(cn)|(mobi)|(name)|(sh)|(ac)|(io)|(tw)|(com\.tw)|(hk)|(com\.hk)|(ws)|(travel)|(us)|(tm)|(la)|(me\.uk)|(org\.uk)|(ltd\.uk)|(plc\.uk)|(in)|(eu)|(it)|(jp))$/.test(sIP)) {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>不是正確的域名</font>";
                }
            } else {
                ipArray = sIP.split(".");
                j = ipArray.length
                if (j != 4) {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>不是正確的IP</font>";
                    return false;
                } else {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                }

                for (var i = 0; i < 4; i++) {
                    if (ipArray[i].length == 0 || ipArray[i] > 255) {
                        img.src = "img/error.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "<font color=red>不是正確的IP</font>";
                    } else {
                        img.src = "img/success.png";
                        img.style.visibility = 'visible';
                        msg.innerHTML = "";
                    }
                }

            }
        }


        function chkAll() {
            chkIP();
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
                    <% If Request("Ftype") = "I" Then
                            Response.Write("<legend>新增店鋪</legend>")
                        Else
                            Response.Write("<legend>編輯店鋪</legend>")
                        End If%>
                    <table>
                        <tr>
                            <td class="style1" align="right">*ID
                            </td>
                            <td colspan="2" class="style2">        
                                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" Width="50px"  Enabled="False"></asp:TextBox>  (系統自動給號)                      
                                <asp:TextBox ID="txtFtype" runat="server" Visible="false" Width="50px" ></asp:TextBox>
                                <img id="img1" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" align="right">IP</td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="txtIP" runat="server" Width="230px" onblur="checkIP()"></asp:TextBox>
                                <img id="img_IP" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="Div_IP" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="IP格式不正確！"
                                ValidationExpression="[0-2]{1}[0-5]{1}[0-5]{1}.[0-2]{1}[0-5]{1}[0-5]{1}.[0-2]{1}[0-5]{1}[0-5]{1}.[0-2]{1}[0-5]{1}[0-5]{1}" ControlToValidate="txtIP" Display="None"></asp:RegularExpressionValidator>
                        </tr>

                        <tr>
                            <td class="style1" align="right">Port</td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="txtPort" runat="server" Width="230px"></asp:TextBox>
                                <img id="img4" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                <div id="Div3" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="只能輸入數字"
                                ValidationExpression="^[0-9]*$" ControlToValidate="txtPort" Display="None"></asp:RegularExpressionValidator>
                        </tr>

                        <tr>
                            <td class="style1" align="right">水表編號</td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="txtMeterID" runat="server" Width="230px"></asp:TextBox>
                                <img id="img2" style="visibility: hidden; cursor: wait;" height="20px" width="20px" />
                                <div id="Div1" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="只能輸入數字"
                                ValidationExpression="^[0-9]*$" ControlToValidate="txtMeterID"  Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">圖面編號
                            </td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="txtPhotoNo" runat="server" Width="230px" ></asp:TextBox>
                                <img id="img_PhotoNo" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_PhotoNo" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="圖面編號只能含中文英文數字！"
                                ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="txtPhotoNo" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">店鋪編號</td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="txtStoreID" runat="server" Width="230px"></asp:TextBox>
                                <img id="img_StoreID" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_StoreID" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RequiredFieldValidator ID="StoreID_empty" runat="server" ErrorMessage="公司名稱不可為空"
                                ControlToValidate="txtStoreID" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="店鋪編號只能含中文英文數字！"
                                ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="txtStoreID" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                        <tr>
                            <td class="style1" align="right">店鋪名稱</td>
                            <td colspan="2" class="style2" align="center">
                                <asp:TextBox ID="txtStoreName" runat="server" Width="230px"></asp:TextBox>
                                <img id="img_StoreName" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                <div id="msg_StoreName" style="width: auto; font-size: 12px">
                                </div>
                            </td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="店鋪名稱只能含中文英文數字！"
                                ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+=' ])*$" ControlToValidate="txtStoreName" Display="None"></asp:RegularExpressionValidator>
                        </tr>
                    </table>
                </fieldset>
                <div align="center" style="padding-top:10px;">
                    <asp:Button ID="Submit_btn" runat="server" Text="確認" OnClientClick="chkAll()" />
                    <asp:Button ID="cancel_btn" runat="server" Text="取消" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
