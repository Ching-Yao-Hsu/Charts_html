<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserCom.aspx.vb" Inherits="_Default" %>

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
            width: 100px;
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
        function chkAll() {
            //chkTel();
        }
    </script>
     
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding-top: 21px; padding-left: 35px;">
            <asp:Panel ID="Panel1" runat="server" Width="390px">
                <fieldset>
                    <legend>權限設定</legend>
                    <table>
                        <tr>
                            <td class="style1" align="right">會員帳號
                            </td>
                            <td class="style2" align="center">
                                <asp:TextBox ID="Account" runat="server" ReadOnly="true" Width="230px" Enabled="False"></asp:TextBox>
                                <img id="img1" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" align="right">*權限</td>
                            <td class="style2" style="padding-left: 10px;">
                                <asp:CheckBoxList ID="Com_CheckBoxList" runat="server" RepeatColumns="2" DataTextField="ECO_Group" DataValueField="Com" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div align="center">
                    <asp:Button ID="update_btn" runat="server" Text="確認" OnClientClick="chkAll()" />
                    <asp:Button ID="cancel_btn" runat="server" Text="取消" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
