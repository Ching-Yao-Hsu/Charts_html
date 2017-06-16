<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="PowerProportionM.aspx.vb" Inherits="PowerProportionM" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="JQChart.Web" Namespace="JQChart.Web.UI.WebControls" TagPrefix="jqChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function custom_script(sender) {
            sure = alertify.alert(sender);
            window.location = "home.aspx"
        }
        $(document).ready(function () {
            var HrefAddress = "support/ProportionM.htm";
            $('.support').colorbox({ innerWidth: 660, innerHeight: 600, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
            //月曆
            $("#<%= Date_Info_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            $("#<%= Date_Info_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            
            $('.imageButtonFinderClass').click(function () {
                //判斷日期
                var begin_time = $("#<%= Date_Info_S.ClientID%>").val();
                var end_time = $("#<%= Date_Info_E.ClientID%>").val();
                if ($("#<%= Date_Info_S.ClientID%>").val() != "" && $("#<%= Date_Info_E.ClientID%>").val() != "") {
                    if (begin_time <= end_time) {
                        Date.prototype.dateDiff = function (interval, objDate) {
                            var dtEnd = new Date(objDate);
                            if (isNaN(dtEnd)) return undefined;
                            switch (interval) {
                                case "s": return parseInt((dtEnd - this) / 1000);  //秒
                                case "n": return parseInt((dtEnd - this) / 60000);  //分
                                case "h": return parseInt((dtEnd - this) / 3600000);  //時
                                case "d": return parseInt((dtEnd - this) / 86400000);  //天
                                case "w": return parseInt((dtEnd - this) / (86400000 * 7));  //週
                                case "m": return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - this.getFullYear()) * 12) - (this.getMonth() + 1);  //月份
                                case "y": return dtEnd.getFullYear() - this.getFullYear();  //天
                            }
                        }
                        var sDT = new Date(begin_time);  //必要項。sDT- 這是在計算中想要使用的第一個日期/時間值。
                        var eDT = new Date(end_time);  //必要項。eDT- 這是在計算中想要使用的第二個日期/時間值。
                        var sum = sDT.dateDiff("d", eDT);
                        //判斷30天以內
                        if (sum <= 31) {
                            var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
                            var group = e.options[e.selectedIndex].value;
                            var value = $("input[name*='E_RBList']:checked").val();
                            var j = 0;
                            node1 = new Array();
                            node2 = new Array();
                            var tree1 = document.getElementById("<%=Meter_TreeView.ClientID%>").getElementsByTagName("INPUT");
                            for (var i = 0; i < tree1.length; i++) {
                                if (tree1[i].type == "checkbox" && tree1[i].checked) {
                                    var s1 = tree1[i].nextSibling.firstChild.nodeValue;
                                    if (s1 != '') {
                                        node1[j] = tree1[i].title.split(/\r?\n/);
                                        j += 1;
                                    }
                                }
                            }
                            if (j != 0) {
                                var k = 0;
                                var tree2 = document.getElementById("<%=Meter_TreeView_Sum.ClientID%>").getElementsByTagName("INPUT");
                                for (var i = 0; i < tree2.length; i++) {
                                    if (tree2[i].type == "checkbox" && tree2[i].checked) {
                                        var s2 = tree2[i].nextSibling.firstChild.nodeValue;
                                        if (s2 != '') {
                                            node2[k] = tree2[i].title.split(/\r?\n/);
                                            k += 1;
                                        }
                                    }
                                }
                                if (k != 0) {
                                    var HrefAddress = "ProportionChartM.aspx?group=" + group + "&node1=" + node1 + "&node2=" + node2 + "&begin_time=" + begin_time + "&end_time=" + end_time + "&value=" + value;
                                    $('.imageButtonFinderClass').colorbox({ innerWidth: 790, innerHeight: 410, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                                } else {
                                    var HrefAddress = "ProportionChartM.aspx?group=" + group + "&node1=" + node1 + "&begin_time=" + begin_time + "&end_time=" + end_time + "&value=" + value;
                                    $('.imageButtonFinderClass').colorbox({ innerWidth: 600, innerHeight: 410, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                                }
                                
                            } else {
                                alertify.alert("請勾選比對電表");
                                return false;
                            }
                        } else {
                            alertify.alert("請搜尋一個月內資料");
                            return false;
                        }
                    } else {
                        alertify.alert("開始日期不可大於结束日期");
                        return false;
                    }
                } else {
                    alertify.alert("請選擇日期");
                    return false;
                }
            });
            
            $("#<%= submit_btn.ClientID%>").click(function () {
                if ($('select[id$=Group_DropDownList] :selected').val() == "請選擇") {
                    alertify.alert("請選擇群組");
                    return false;
                }
            });
        });
        function custom_script(sender) {
            sure = alertify.alert(sender);
            window.location = "home.aspx"
        }
    </script>
    <style type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .support
        {
            outline: none;
            border: none;
        }
        .td_width
        {
            height: 40px;
            width: 80px;
        }
        .title_div
        {
            line-height: 25px;
            width: 100px;
            text-align: center;
            padding-top:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left" style="padding-left: 5px;">
                    <asp:Label ID="Label2" runat="server" Text="多電表比重圖" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <div align="center" style="border-radius: 15px 15px 15px 15px; background-color: #eaf8c7; width:780px;">
                        <table width="750px">
                            <tr>
                                <td align="left" colspan="2" style="padding-left: 5px;">
                                    <img src="img/date2.png" />日期區間
                                    <asp:TextBox ID="Date_Info_S" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    ～
                                    <asp:TextBox ID="Date_Info_E" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="img/compare.png" />比對項目</td>
                                            <td style="padding-top:8px;">
                                                <asp:RadioButtonList ID="E_RBList" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="RushHour">尖峰電量</asp:ListItem>
                                                    <asp:ListItem Value="HalfHour">半尖峰電量</asp:ListItem>
                                                    <asp:ListItem Value="SatHalfHour">週六半尖峰電量</asp:ListItem>
                                                    <asp:ListItem Value="OffHour">離峰電量</asp:ListItem>
                                                    <asp:ListItem Value="KWh" Selected="True">用電度數</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
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
                                            <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff;">
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <table>
                                                                <tr>
                                                                    <td nowrap="nowrap">
                                                                        <img src="img/icon-account.png" />群組
                                                                    </td>
                                                                    <td align="left" style="padding-top:8px;padding-right:75px;">
                                                                        <asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                                    </td>
                                                                    <td align="right" nowrap="nowrap">
                                                                        <img src="img/compare.png" />顯示名稱</td>
                                                                    <td align="left" valign="middle" nowrap="nowrap">
                                                                        <asp:CheckBox ID="CtrlNr_CB" runat="server" Text="ECO5編號" /><br />
                                                                        <asp:CheckBox ID="Position_CB" runat="server" Text="安裝位置" />
                                                                    </td>
                                                                    <td align="left" valign="middle" nowrap="nowrap">
                                                                        <asp:CheckBox ID="MeterId_CB" runat="server" Text="電表編號" /><br />
                                                                        <asp:CheckBox ID="LineNum_CB" runat="server" Text="單線圖編號" />
                                                                    </td>
                                                                    <td align="left" style="padding-right:0px; width:170px;">
                                                                        <asp:Button ID="submit_btn" runat="server" Text="查詢" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="6">
                                                                        <img src="img/Dash_05.png" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" colspan="3">
                                                                        <asp:Label ID="Label3" runat="server" Text="總表" Font-Bold="True" Font-Size="Medium" Font-Names="微軟正黑體"></asp:Label>
                                                                        <div id="div1" style="height: 360px; width: 365px; padding-top:5px; overflow: auto; background-color: #e8f0ff; border-radius: 15px 15px 15px 15px;">
                                                                            <asp:TreeView ID="Meter_TreeView_Sum" runat="server" Style="margin-right: 3px" Font-Names="微軟正黑體" ForeColor="Black" ShowLines="True">
                                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                                            </asp:TreeView>
                                                                        </div>
                                                                    </td>
                                                                    <td align="left" valign="top" colspan="3">
                                                                        <asp:Label ID="Label4" runat="server" Text="比對電表" Font-Bold="True" Font-Size="Medium" Font-Names="微軟正黑體"></asp:Label>
                                                                        <div id="div2" style="height: 360px; width: 365px; padding-top:5px; overflow: auto; background-color: #e8f0ff; border-radius: 15px 15px 15px 15px;">
                                                                            <asp:TreeView ID="Meter_TreeView" runat="server" Style="margin-right: 3px" Font-Names="微軟正黑體" ForeColor="Black" ShowLines="True">
                                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                                            </asp:TreeView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div>
                                                <table>
                                                    <tr>
                                                        <td width="365px">
                                                            <table>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <div class="title_div">
                                                                            <asp:Label ID="Label5" runat="server" Text="ECO5資訊" Font-Bold="True" ForeColor="#CC0000" Font-Size="14pt" Font-Names="微軟正黑體"></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">ECO5</td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="eco5_position" runat="server" Enabled="False" Width="130px" Font-Bold="True" ForeColor="Blue" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="eco5_enabled" runat="server" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">ECO5編號</td>
                                                                    <td width="20px" align="left">
                                                                        <asp:TextBox ID="eco5_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">ECO5帳號</td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="eco5_account" runat="server" BackColor="White" Width="115px" ForeColor="#000099" Enabled="False"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="365px">
                                                            <table>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <div class="title_div">
                                                                            <asp:Label ID="Label1" runat="server" Text="電表資訊" Font-Bold="True" ForeColor="#CC0000" Font-Size="14pt" Font-Names="微軟正黑體"></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">電表編號</td>
                                                                    <td>
                                                                        <asp:TextBox ID="meter_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td class="td_width" align="right">電表型式</td>
                                                                    <td align="left" width="70px">
                                                                        <asp:TextBox ID="MeterType" runat="server" Enabled="False" Width="60px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">安裝位置</td>
                                                                    <td colspan="3" class="auto-style2">
                                                                        <asp:TextBox ID="InstallPosition" runat="server" Width="130px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkPosition()"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                                <td class="td_width" align="right">圖面編號</td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="drawnr" runat="server" Width="130px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkDrawnr()"></asp:TextBox>
                                                                                </td>
                                                                            </tr>--%>
                                                                <tr>
                                                                    <td class="td_width" align="right">啟用狀態</td>
                                                                    <td>
                                                                        <asp:Image ID="enabled_img" runat="server" />
                                                                    </td>
                                                                    <td class="td_width" align="right">上傳狀態</td>
                                                                    <td>
                                                                        <asp:Image ID="upload_img" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Group_DropDownList" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="Meter_TreeView" EventName="SelectedNodeChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:ImageButton ID="ViewDetails_btn" runat="server" ImageUrl="~/img/btn_proportion_01.png"
                                            onmouseover="this.src='img/btn_proportion_02.png'" onmouseout="this.src='img/btn_proportion_01.png'"
                                            CssClass="imageButtonFinderClass" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>