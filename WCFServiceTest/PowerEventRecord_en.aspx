<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="PowerEventRecord.aspx.vb" Inherits="EventRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .menu a{ text-decoration:none;color:#333;}
        .menu a:hover{ text-decoration:underline;}
        .style1
        {
            width: 200px;
            padding-left:100px;
        }
        .td_width
        {
            height: 65px;
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
    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/PowerEventRecordSupport.htm";
            $('.support').colorbox({ innerWidth: 725, innerHeight: 430, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            $('.imageButtonFinderClass').click(function () {
                var begin_time = $("#<%= Date_txt1.ClientID %>").val() + " " + $('select[id$=begin_hh] :selected').text() + ":00:00";
                var end_time = $("#<%= Date_txt2.ClientID %>").val() + " " + $('select[id$=end_hh] :selected').text() + ":59:59";
                //判斷日期
                if ($("#<%= Date_txt1.ClientID%>").val() != "" && $("#<%= Date_txt2.ClientID%>").val() != "") {
                    if (begin_time < end_time) {
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
                        //判斷一個月以內
                        if (sum <= 31) {
                            var e1 = document.getElementById("<%=Group_DropDownList.ClientID%>");
                            var group = e1.options[e1.selectedIndex].value;
                            var e2 = document.getElementById("<%=Event_DropDownList.ClientID%>");
                            var type = e2.options[e2.selectedIndex].value;
                            var datetime = begin_time + "," + end_time;
                            var HrefAddress = "EventRecord_en.aspx?group=" + group + "&type=" + type + "&datetime=" + datetime;
                            $('.imageButtonFinderClass').colorbox({ innerWidth: 1200, innerHeight: 690, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                        } else {
                            alertify.alert("Please search for information within one month.");
                            return false;
                        }
                    } else {
                        alertify.alert("Start date can not be greater than the end date.");
                        return false;   //取消postback
                    }
                } else {
                    alertify.alert("Please select a date.");
                    return false;
                }
            });
            $("#<%= submit_btn.ClientID%>").click(function () {
                if ($('select[id$=Group_DropDownList] option:selected').text() == "Please choose...") {
                    alertify.alert("Please select Group!");
                    return false;
                } else {
                    if ($('select[id$=Event_DropDownList] option:selected').text() == "Please choose...") {
                        alertify.alert("Please select event name");
                        return false;
                    }
                }
            });

            //月曆
            $("#<%= Date_txt1.ClientID %>").datepicker({
                dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            $("#<%= Date_txt2.ClientID %>").datepicker({
                dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="Event Record" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right:10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff; width:600px;">
                        <table>
                            <tr>
                                <td align="left">
                                    <img src="img/icon-account.png" />Group
                                    <asp:DropDownList ID="Group_DropDownList" runat="server"></asp:DropDownList>
                                </td>
                                <td align="left" height="30px">
                                    <img src="img/event_icon.png" />Event Name
                                    <asp:DropDownList ID="Event_DropDownList" runat="server">
                                        <asp:ListItem Value="99">Please choose...</asp:ListItem>
                                        <asp:ListItem Value="0">All_Select</asp:ListItem>
                                        <asp:ListItem Value="80">Fire_host_exception</asp:ListItem>
                                        <asp:ListItem Value="81">Fire_alarm</asp:ListItem>
                                        <asp:ListItem Value="1">ECO5_Disconnected</asp:ListItem>
                                        <asp:ListItem Value="3">Ultra-about</asp:ListItem>
                                        <asp:ListItem Value="4">Abnormal_current</asp:ListItem>
                                        <asp:ListItem Value="5">Abnormal_voltage</asp:ListItem>
                                        <asp:ListItem Value="6">Exceed_the_specified_electricity</asp:ListItem>
                                        <asp:ListItem Value="7">Meter_communication_error</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" height="30px">
                                    <img src="img/date2.png" />Time Segments
			                        <asp:TextBox ID="Date_txt1" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    <asp:DropDownList ID="begin_hh" runat="server"></asp:DropDownList>:00
                                    ～
                                    <asp:TextBox ID="Date_txt2" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    <asp:DropDownList ID="end_hh" runat="server"></asp:DropDownList>:00
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="padding-top:10px;">
                                    <asp:Button ID="submit_btn" runat="server" Text="Confirm" CssClass="imageButtonFinderClass" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

