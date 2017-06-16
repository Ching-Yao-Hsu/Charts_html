<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="PowerRecord.aspx.vb" Inherits="PowerRecord" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="TBCheckBoxList" Namespace="TBCheckBoxList.WebControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/print.js" type="text/javascript"></script>
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
        .auto-style1 {
            width: 708px;
        }
        .auto-style2 {
            width: 12px;
        }
        .auto-style3 {
            width: 80px;
        }
    </style>
    <script type="text/javascript">
        function ShowLoadingMessage() {
            document.getElementById("LoadingMessage").style.display = "block";
        }
        $(document).ready(function () {
            var HrefAddress = "support/PowerRecordSupport.htm";
            $('.support').colorbox({ innerWidth: 660, innerHeight: 540, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

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
                        //判斷30天以內
                        if (sum <= 31) {
                            var value = '';
                            $("#<%=I_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "R Phase") {
                                        value += "I1 AS " + currentValue.replace(" ", "_") + "_Current,";
                                    } else if (currentValue == "S Phase") {
                                        value += "I2 AS " + currentValue.replace(" ", "_") + "_Current,";
                                    } else if (currentValue == "T Phase") {
                                        value += "I3 AS " + currentValue.replace(" ", "_") + "_Current,";
                                    } else if (currentValue == "Average") {
                                        value += "Iavg AS " + currentValue.replace(" ", "_") + "_Current,";
                                    }
                                }
                            });
                            $("#<%=V_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "R Phase") {
                                        value += "V1 AS " + currentValue.replace(" ", "_") + "_Voltage,";
                                    } else if (currentValue == "S Phase") {
                                        value += "V2 AS " + currentValue.replace(" ", "_") + "_Voltage,";
                                    } else if (currentValue == "T Phase") {
                                        value += "V3 AS " + currentValue.replace(" ", "_") + "_Voltage,";
                                    } else if (currentValue == "Average") {
                                        value += "Vavg AS " + currentValue.replace(" ", "_") + "_Voltage,";
                                    }
                                }
                            });
                            $("#<%=W_CheckBoxList.ClientID%> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "Actual power output") {
                                        //value += "W AS " + currentValue.replace(" ", "_") + ",";
                                        value += "W AS Actual_power_output,";
                                    } else if (currentValue == "Virtual power") {
                                        //value += "V_ar AS " + currentValue.replace(" ", "_") + ",";
                                        value += "V_ar AS Virtual_power,";
                                    } else if (currentValue == "Apparent power") {
                                        //value += "VA AS " + currentValue.replace(" ", "_") + ",";
                                        value += "VA AS Apparent_power,";
                                    } else if (currentValue == "Power Factor (PF)") {
                                        //value += "PF AS " + currentValue.replace(" ", "_") + ",";
                                        value += "PF AS Power_Factor,";
                                    }
                                }
                            });
                            $("#<%=Mode_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "Mode 1") {
                                        value += "Mode1 AS " + currentValue.replace(" ", "") + ",";
                                    } else if (currentValue == "Mode 2") {
                                        value += "Mode2 AS " + currentValue.replace(" ", "") + ",";
                                    } else if (currentValue == "Mode 3") {
                                        value += "Mode3 AS " + currentValue.replace(" ", "") + ",";
                                    } else if (currentValue == "Mode 4") {
                                        value += "Mode4 AS " + currentValue.replace(" ", "") + ",";
                                    }
                                }
                            });
                            $("#<%=DeMand_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "Peak Time") {
                                        //value += "Demand AS " + currentValue.replace(" ", "_") + "_Demand,";
                                        value += "Demand AS Peak_Time_Demand,";
                                    } else if (currentValue == "Half Peak Time") {
                                        //value += "DemandHalf AS " + currentValue.replace(" ", "_") + "_Demand,";
                                        value += "DemandHalf AS Half_Peak_Time_Demand,";
                                    } else if (currentValue == "Saturday Half Peak Time") {
                                        //value += "DemandSatHalf AS " + currentValue.replace(" ", "_") + "_Demand,";
                                        value += "DemandSatHalf AS Saturday_Half_Peak_Time_Demand,";
                                    } else if (currentValue == "Off Peak Time") {
                                        //value += "DemandOff AS " + currentValue.replace(" ", "_") + "_Demand,";
                                        value += "DemandOff AS Off_Peak_Time_Demand,";
                                    } 
                                }
                            });
                            $("#<%=E_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "Peak Time") {
                                        //value += "RushHour AS " + currentValue.replace(" ", "_") + "_Electricity,";
                                        value += "RushHour AS Peak_Time_Electricity,";
                                    } else if (currentValue == "Half Peak Time") {
                                        //value += "HalfHour AS " + currentValue.replace(" ", "_") + "_Electricity,";
                                        value += "HalfHour AS Half_Peak_Time_Electricity,";
                                    } else if (currentValue == "Saturday Half Peak Time") {
                                        //value += "SatHalfHour AS " + currentValue.replace(" ", "_") + "_Electricity,";
                                        value += "SatHalfHour AS Saturday_Half_Peak_Time_Electricity,";
                                    } else if (currentValue == "Off Peak Time") {
                                        //value += "OffHour AS " + currentValue.replace(" ", "_") + "_Electricity,";
                                        value += "OffHour AS Off_Peak_Time_Electricity,";
                                    } 
                                }
                            });
                            $("#<%=KWh_CheckBoxList.ClientID%> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '')
                                    value += "KWh AS " + currentValue + ",";
                            });
                            var j = 0;
                            node = new Array();
                            var tree = document.getElementById("<%=Meter_TreeView.ClientID%>").getElementsByTagName("INPUT");
                            for (var i = 0; i < tree.length; i++) {
                                if (tree[i].type == "checkbox" && tree[i].checked) {
                                    var s = tree[i].nextSibling.firstChild.nodeValue;
                                    if (s != '') {
                                        node[j] = tree[i].title.split(/\r?\n/);
                                        j += 1;
                                    }
                                }
                            }
                            if (value.length > 0) {
                                if (j > 0) {
                                    if (j <= 10) {
                                        var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
                                        var group = e.options[e.selectedIndex].value;
                                        var datetime = begin_time + "," + end_time;
                                        var interval = $('select[id$=interval_DDList] :selected').val();
                                        var eco_account = $("#<%= eco5_account.ClientID%>").val();
                                        value = value.substring(0, value.length - 1);
                                        var HrefAddress = "Record_en.aspx?group=" + group + "&node=" + node + "&datetime=" + datetime + "&interval=" + interval + "&value=" + value;
                                        $('.imageButtonFinderClass').colorbox({ innerWidth: 910, innerHeight: 570, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                                    } else {
                                        alertify.alert("Check the meter  this can't be more than ten.");
                                        return false;
                                    }
                                } else {
                                    alertify.alert("Please check the meter!");
                                    return false;
                                }
                            } else {
                                alertify.alert("You have not checked query items.");
                                return false;
                            }
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
                if ($('select[id$=Group_DropDownList] option:selected').val() == "Please choose...") {
                    alertify.alert("Please select Group!");
                    return false;
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

        //CheckBoxList全選及全取消
        function I_checkAll(obj) {
            var I_CheckBoxList = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("I_CheckBoxList").ClientID %>');
         if (obj.checked) {
             for (i = 0; i < I_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < I_CheckBoxList.rows[i].cells.length; j++) {
                     if (I_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         I_CheckBoxList.rows[i].cells[j].childNodes[0].checked = true;
                     }
                 }
             }
         }
         else {
             for (i = 0; i < I_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < I_CheckBoxList.rows[i].cells.length; j++) {
                     if (I_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         I_CheckBoxList.rows[i].cells[j].childNodes[0].checked = false;
                     }
                 }
             }
         }
     }
     function V_checkAll(obj) {
         var V_CheckBoxList = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("V_CheckBoxList").ClientID %>');
         if (obj.checked) {
             for (i = 0; i < V_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < V_CheckBoxList.rows[i].cells.length; j++) {
                     if (V_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         V_CheckBoxList.rows[i].cells[j].childNodes[0].checked = true;
                     }
                 }
             }
         }
         else {
             for (i = 0; i < V_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < V_CheckBoxList.rows[i].cells.length; j++) {
                     if (V_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         V_CheckBoxList.rows[i].cells[j].childNodes[0].checked = false;
                     }
                 }
             }
         }
     }
     function W_checkAll(obj) {
         var W_CheckBoxList = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("W_CheckBoxList").ClientID %>');
         if (obj.checked) {
             for (i = 0; i < W_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < W_CheckBoxList.rows[i].cells.length; j++) {
                     if (W_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         W_CheckBoxList.rows[i].cells[j].childNodes[0].checked = true;
                     }
                 }
             }
         }
         else {
             for (i = 0; i < W_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < W_CheckBoxList.rows[i].cells.length; j++) {
                     if (W_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         W_CheckBoxList.rows[i].cells[j].childNodes[0].checked = false;
                     }
                 }
             }
         }
     }
     function Mode_checkAll(obj) {
         var Mode_CheckBoxList = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Mode_CheckBoxList").ClientID %>');

         if (obj.checked) {
             for (i = 0; i < Mode_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < Mode_CheckBoxList.rows[i].cells.length; j++) {
                     if (Mode_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         Mode_CheckBoxList.rows[i].cells[j].childNodes[0].checked = true;
                     }
                 }
             }
         }
         else {
             for (i = 0; i < Mode_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < Mode_CheckBoxList.rows[i].cells.length; j++) {
                     if (Mode_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         Mode_CheckBoxList.rows[i].cells[j].childNodes[0].checked = false;
                     }
                 }
             }
         }
     }
     function DeMand_checkAll(obj) {
         var DeMand_CheckBoxList = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("DeMand_CheckBoxList").ClientID %>');
         if (obj.checked) {
             for (i = 0; i < DeMand_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < DeMand_CheckBoxList.rows[i].cells.length; j++) {
                     if (DeMand_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         DeMand_CheckBoxList.rows[i].cells[j].childNodes[0].checked = true;
                     }
                 }
             }
         }
         else {
             for (i = 0; i < DeMand_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < DeMand_CheckBoxList.rows[i].cells.length; j++) {
                     if (DeMand_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         DeMand_CheckBoxList.rows[i].cells[j].childNodes[0].checked = false;
                     }
                 }
             }
         }
     }
     function E_checkAll(obj) {
         var E_CheckBoxList = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("E_CheckBoxList").ClientID %>');
         if (obj.checked) {
             for (i = 0; i < E_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < E_CheckBoxList.rows[i].cells.length; j++) {
                     if (E_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         E_CheckBoxList.rows[i].cells[j].childNodes[0].checked = true;
                     }
                 }
             }
         }
         else {
             for (i = 0; i < E_CheckBoxList.rows.length; i++) {
                 for (j = 0; j < E_CheckBoxList.rows[i].cells.length; j++) {
                     if (E_CheckBoxList.rows[i].cells[j].childNodes[0]) {
                         E_CheckBoxList.rows[i].cells[j].childNodes[0].checked = false;
                     }
                 }
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
                    <asp:Label ID="Label2" runat="server" Text="Electricity Numerical record" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right:10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#eaf8c7;">
                        <table>
                            <tr>
                               <td  nowrap="nowrap" colspan="6">
                                  <table width="100%">
                                    <tr>
                                        <td>
                                            <img src="img/date2.png" />Time Segments
			                                <asp:TextBox ID="Date_txt1" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                            <asp:DropDownList ID="begin_hh" runat="server"></asp:DropDownList>:00
                                            ～
                                            <asp:TextBox ID="Date_txt2" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                            <asp:DropDownList ID="end_hh" runat="server"></asp:DropDownList>:00
                                        </td>
                                        <td>Time interval
			                                <asp:DropDownList ID="interval_DDList" runat="server">
                                                <asp:ListItem Value="1">1 minute</asp:ListItem>
                                                <asp:ListItem Value="5">5 minute</asp:ListItem>
                                                <asp:ListItem Value="30">30 minute</asp:ListItem>
                                                <asp:ListItem Value="60">1 Hour</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                               </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap">
                                    <img src="img/compare.png" />Inquiry Item</td>
                                <td nowrap="nowrap">
                                    <asp:CheckBox ID="V_CheckBox" runat="server" Text="Voltage" onClick="V_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td colspan="4">
                                    <asp:CheckBoxList ID="V_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Value="V1">R Phase</asp:ListItem>
                                        <asp:ListItem Value="V2">S Phase</asp:ListItem>
                                        <asp:ListItem Value="V3">T Phase</asp:ListItem>
                                        <asp:ListItem Value="Vavg">Average</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td nowrap="nowrap">
                                    <asp:CheckBox ID="i_CheckBox" runat="server" Text="Current" onClick="I_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td colspan="4">
                                    <asp:CheckBoxList ID="I_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Value="I1">R Phase</asp:ListItem>
                                        <asp:ListItem Value="I2">S Phase</asp:ListItem>
                                        <asp:ListItem Value="I3">T Phase</asp:ListItem>
                                        <asp:ListItem Value="Iavg">Average</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="W_CheckBox" runat="server" Text="Power" onClick="W_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td colspan="4">
                                    <cc1:TBCheckBoxList ID="W_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Value="W">Actual power output</asp:ListItem>
                                        <asp:ListItem Value="V_ar">Virtual power</asp:ListItem>
                                        <asp:ListItem Value="VA">Apparent power</asp:ListItem>
                                        <asp:ListItem Value="PF">Power Factor (PF)</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                            </tr>

                            
                            <tr>
                                <td nowrap="nowrap"> </td>
                                <td>
                                    <asp:CheckBox ID="Mode_CheckBox" runat="server" Text="Prediction" onClick="Mode_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td colspan="4">
                                    <asp:CheckBoxList ID="Mode_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Value="Mode1">Mode 1</asp:ListItem>
                                        <asp:ListItem Value="Mode2">Mode 2</asp:ListItem>
                                        <asp:ListItem Value="Mode3">Mode 3</asp:ListItem>
                                        <asp:ListItem Value="Mode4">Mode 4</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" width="50px">　　　　</td>
                                <td>
                                    <asp:CheckBox ID="DeMand_CheckBox" runat="server" Text="Demand" onClick="DeMand_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td colspan="4">
                                    <asp:CheckBoxList ID="DeMand_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Value="DeMand">Peak Time</asp:ListItem>
                                        <asp:ListItem Value="DemandHalf">Half Peak Time</asp:ListItem>
                                        <asp:ListItem Value="DemandSatHalf">Saturday Half Peak Time</asp:ListItem>
                                        <asp:ListItem Value="DemandOff">Off Peak Time</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="E_CheckBox" runat="server" Text="Electricity" onClick="E_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td colspan="4">
                                    <asp:CheckBoxList ID="E_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Value="RushHour">Peak Time</asp:ListItem>
                                        <asp:ListItem Value="HalfHour">Half Peak Time</asp:ListItem>
                                        <asp:ListItem Value="SatHalfHour">Saturday Half Peak Time</asp:ListItem>
                                        <asp:ListItem Value="OffHour">Off Peak Time</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="5">
                                    <asp:CheckBoxList ID="KWh_CheckBoxList" runat="server" Font-Bold="True">
                                    <asp:ListItem Value="KWh">Kilowatt</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <script type="text/javascript">
                                        var xPos1, yPos1;
                                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                                        prm.add_beginRequest(BeginRequestHandler);
                                        prm.add_endRequest(EndRequestHandler);
                                        function BeginRequestHandler(sender, args) {
                                            xPos1 = $get('div1').scrollLeft; //gvDiv請更換成適合的ID名稱
                                            yPos1 = $get('div1').scrollTop;
                                        }
                                        function EndRequestHandler(sender, args) {
                                            $get('div1').scrollLeft = xPos1;
                                            $get('div1').scrollTop = yPos1;
                                        }
                                    </script>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff;">
                                                <table>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <table width="380px">
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td align="left" colspan="4" nowrap="nowrap">
                                                                                    <img src="img/icon-account.png" />Group
                                                                                    <asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <img src="img/compare.png" />Name</td>
                                                                                <td align="left" valign="middle">
                                                                                    <asp:CheckBox ID="CtrlNr_CB" runat="server" Text="ECO5 Number" /><br />
                                                                                    <asp:CheckBox ID="MeterId_CB" runat="server" Text="Meter Number" /><br />
                                                                                    <asp:CheckBox ID="Position_CB" runat="server" Text="Installation location" /><br />
                                                                                    <asp:CheckBox ID="LineNum_CB" runat="server" Text="One-Line Diagram Number" />
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:Button ID="submit_btn" runat="server" Text="Search" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <img src="img/Dash_01.png" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <div id="div1" style="height: 415px; width: 370px; overflow: auto; background-color: #e8f0ff; border-radius: 15px 15px 15px 15px;">
                                                                            <asp:TreeView ID="Meter_TreeView" runat="server" Style="margin-right: 3px" Font-Names="微軟正黑體" ForeColor="Black" ShowLines="True" CssClass="ParentNodeStyle">
                                                                                <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                                            </asp:TreeView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <img src="img/Dash_04.png" />
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <table width="380px">
                                                                <tr>
                                                                    <td>
                                                                        <div class="title_div">
                                                                            <asp:Label ID="Label1"  Width="190px" runat="server" Text="ECO5 information" Font-Bold="True" ForeColor="#CC0000" Font-Size="14pt" Font-Names="微軟正黑體"></asp:Label>
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
                                                                    <td class="td_width" align="right">ECO5 Number</td>
                                                                    <td width="20px" align="left">
                                                                        <asp:TextBox ID="eco5_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td class="td_width" align="right">ECO5 Account</td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="eco5_account" runat="server" BackColor="White" Width="120px" ForeColor="#000099" Enabled="False"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="4">
                                                                        <img src="img/Dash_01.png" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div class="title_div">
                                                                            <asp:Label ID="Label3" Width="190px" runat="server" Text="Meter information" Font-Bold="True" ForeColor="#CC0000" Font-Size="14pt" Font-Names="微軟正黑體"></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">Meter Number</td>
                                                                    <td>
                                                                        <asp:TextBox ID="meter_id" runat="server" Enabled="False" Width="20px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                    <td class="td_width" align="right">Meter Type</td>
                                                                    <td align="left" width="70px">
                                                                        <asp:TextBox ID="MeterType" runat="server" Enabled="False" Width="60px" ForeColor="#000099" BackColor="White"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">Installation location</td>
                                                                    <td colspan="3" class="auto-style2">
                                                                        <asp:TextBox ID="InstallPosition" runat="server" Width="130px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkPosition()"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">One-Line Diagram Number</td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="drawnr" runat="server" Width="130px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkDrawnr()"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_width" align="right">Enabled status</td>
                                                                    <td>
                                                                        <asp:Image ID="enabled_img" runat="server" />
                                                                    </td>
                                                                    <td class="td_width" align="right">Uploading status</td>
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
                                <td align="center" colspan="5">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:ImageButton ID="ViewDetails_btn" runat="server" ImageUrl="~/img/btn_powervalue_01en.png"
                                            onmouseover="this.src='img/btn_powervalue_02en.png'" onmouseout="this.src='img/btn_powervalue_01en.png'"
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

