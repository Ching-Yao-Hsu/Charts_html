<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="MeterInfo.aspx.vb" Inherits="TreeSetting" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            height: 45px;
            width: 100px;
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

    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/MeterInfoSupport.htm";
            $('.support').colorbox({ innerWidth: 725, innerHeight: 570, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
        });

        function mask() {
            var v = document.getElementById("<%=P_LineNum.ClientID%>");
            if ((v.value.length + 1) % 3 == 0) {
                v.value += "-";
            }
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
        function chkPosition() {
            img = document.getElementById("img1");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg1");
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
            img = document.getElementById("img2");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg2");
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
        function chkLineNum() {
            img = document.getElementById("img3");
            img.style.visibility = 'visible';
            img.src = "img/Processing.jpg"; //顯示處理中的圖片
            var msg = document.getElementById("msg3");
            var LineNum = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("P_LineNum").ClientID %>').value;
            if (LineNum != '') {
                var regex = /^([^@#$%^&=+ ]+[0-9])*$/;
                if (regex.test(LineNum) == true) {
                    img.src = "img/success.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "";
                } else {
                    img.src = "img/error.png";
                    img.style.visibility = 'visible';
                    msg.innerHTML = "<font color=red>單線圖編號含不合法字元</font>";
                }
            } else {
                img.style.visibility = 'hidden';
                msg.innerHTML = "";
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left" style="padding-left:5px;">
                    <asp:label id="Label4" runat="server" text="電表設定" font-bold="True" font-size="16pt" cssclass="fontline"></asp:label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:imagebutton id="support" runat="server" imageurl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" cssclass="support" causesvalidation="False" tooltip="說明" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <script type="text/javascript">
                        var xPos1, yPos1, xPos2, yPos2;
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        prm.add_beginRequest(BeginRequestHandler);
                        prm.add_endRequest(EndRequestHandler);
                        function BeginRequestHandler(sender, args) {
                            xPos1 = $get('div1').scrollLeft; //gvDiv請更換成適合的ID名稱
                            yPos1 = $get('div1').scrollTop;
                            xPos2 = $get('div2').scrollLeft;
                            yPos2 = $get('div2').scrollTop;
                        }
                        function EndRequestHandler(sender, args) {
                            $get('div1').scrollLeft = xPos1;
                            $get('div1').scrollTop = yPos1;
                            $get('div2').scrollLeft = xPos2;
                            $get('div2').scrollTop = yPos2;
                        }
                    </script>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="95%">
                                <tr>
                                    <td colspan="2" style="padding-left: 10px">
                                        <img src="img/icon-account.png" />群組
                                        <asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                        <img id="img_account" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                        <div id="msg_account" style="width: auto; font-size: 12px;padding-left:92px;">
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Group_DropDownList"
                                            ErrorMessage="請選擇群組" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff;">
                                            <table>
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        <div class="title_div">
                                                            <asp:Label ID="Label3" runat="server" Text="電表資訊" Font-Bold="True" Font-Names="微軟正黑體" Font-Size="Medium"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:Button ID="edit_btn" runat="server" Text="修改"  OnClientClick="chkDrop()" />
                                                        <asp:Button ID="submit_btn" runat="server" Text="確認" Enabled="False" />
                                                        <asp:Button ID="cancel_btn" runat="server" Text="取消" Enabled="False" CausesValidation="False" />
                                                        <asp:Button ID="draw_btn" runat="server" Text="載入" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Panel ID="Panel2" runat="server" Direction="LeftToRight" Height="530px">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="td_width" align="right">ECO5</td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="eco5_position" runat="server" Enabled="False" Width="150px" Font-Bold="True" ForeColor="Blue" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td width="50px" align="center">
                                                                        <asp:Label ID="eco5_enabled" runat="server" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">ECO5編號</td>
                                                                    <td>
                                                                        <asp:TextBox ID="eco5_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td align="right">ECO5帳號</td>
                                                                    <td width="70px">
                                                                        <asp:TextBox ID="eco5_account" runat="server" BackColor="White" Width="120px" ForeColor="#000099" Enabled="False"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">電表編號</td>
                                                                    <td>
                                                                        <asp:TextBox ID="meter_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td colspan="2" align="left" style="padding-left: 0px;">
                                                                        <asp:CheckBox ID="meter_enabled" runat="server" Enabled="False" Text="啟用電表" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">電表型式</td>
                                                                    <td width="70px">
                                                                        <asp:DropDownList ID="MeterType_DDList" runat="server" Enabled="False" BackColor="White">
                                                                            <asp:ListItem Value="1">A40</asp:ListItem>
                                                                            <asp:ListItem Value="2">DM2436</asp:ListItem>
                                                                            <asp:ListItem Value="3">CT-1700</asp:ListItem>
                                                                            <asp:ListItem Value="4">CT713P</asp:ListItem>
                                                                            <asp:ListItem Value="5">SPM-8</asp:ListItem>
                                                                            <asp:ListItem Value="6">PM710</asp:ListItem>
                                                                            <asp:ListItem Selected="True" Value="7">其它</asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">*安裝位置</td>
                                                                    <td colspan="3" class="auto-style2">
                                                                        <asp:TextBox ID="InstallPosition" runat="server" Width="150px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkPosition()"></asp:TextBox>
                                                                        <img id="img1" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                        <div style="width: auto; font-size: 12px" id="msg1">
                                                                        </div>
                                                                    </td>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage=""
                                                                        ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$" ControlToValidate="InstallPosition"
                                                                        Display="None"></asp:RegularExpressionValidator>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">圖面編號</td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="drawnr" runat="server" Width="150px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkDrawnr()"></asp:TextBox>
                                                                        <img id="img2" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                        <div style="width: auto; font-size: 12px" id="msg2">
                                                                        </div>
                                                                    </td>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage=""
                                                                        ValidationExpression="^(\w|[\u4E00-\u9FA5]|[^@#$%^&+= ])*$" ControlToValidate="drawnr"
                                                                        Display="None"></asp:RegularExpressionValidator>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">*單線圖編號</td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="P_LineNum" runat="server" Width="150px" Enabled="False" onkeypress="mask()" BackColor="White" ForeColor="Black" onblur="chkLineNum()"></asp:TextBox>
                                                                        <img id="img3" style="visibility: hidden; cursor: wait;" height="20px" width="20px">
                                                                        <div style="width: auto; font-size: 12px" id="msg3">
                                                                        </div>
                                                                    </td>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage=""
                                                                        ValidationExpression="^([^@#$%^&=+ ]+[0-9])*$" ControlToValidate="P_LineNum"
                                                                        Display="None"></asp:RegularExpressionValidator>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="border-radius: 15px 15px 15px 15px; background-color: #eaf8c7;">
                                            <table width="95%">
                                                <tr>
                                                    <td valign="middle">
                                                        <div class="title_div">
                                                            <asp:Label ID="Label1" runat="server" Text="未定義電表" Font-Bold="True" ForeColor="#CC0000" Font-Size="Medium" Font-Names="微軟正黑體"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="div1" style="height: 170px; width:415px; overflow:auto;">
                                                            <asp:TreeView ID="Meter_TreeView" runat="server" ShowLines="True" ForeColor="Black" Font-Names="微軟正黑體">
                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                            </asp:TreeView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="border-radius: 15px 15px 15px 15px; background-color: #ececec;">
                                            <table>
                                                <tr>
                                                    <td valign="middle">
                                                        <div class="title_div">
                                                            <asp:Label ID="Label2" runat="server" Text="已定義電表" Font-Bold="True" ForeColor="#008C2E" Font-Size="Medium" Font-Names="微軟正黑體"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td align="left" valign="middle" nowrap="nowrap">
                                                        <asp:CheckBox ID="CtrlNr_CB" runat="server" Text="ECO5編號" /><br />
                                                        <asp:CheckBox ID="Position_CB" runat="server" Text="安裝位置" />

                                                    </td>
                                                    <td align="left" valign="middle" nowrap="nowrap">
                                                        <asp:CheckBox ID="MeterId_CB" runat="server" Text="電表編號" /><br />
                                                        <asp:CheckBox ID="LineNum_CB" runat="server" Text="單線圖編號" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="show_btn" runat="server" Text="顯示" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" valign="top">
                                                        <div id="div2" style="height: 320px; width:415px; overflow: auto; background-color: #ececec;">
                                                            <asp:TreeView ID="NewMeter_TreeView" runat="server" ShowLines="True" ForeColor="Black" Font-Names="微軟正黑體">
                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                            </asp:TreeView>
                                                        </div>
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

