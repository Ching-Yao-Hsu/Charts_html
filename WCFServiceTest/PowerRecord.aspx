<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="PowerRecord.aspx.vb" Inherits="PowerRecord" MaintainScrollPositionOnPostback="true" %>
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
                                    if (currentValue == "R相") {
                                        value += "I1 AS " + currentValue + "電流,";
                                    } else if (currentValue == "S相") {
                                        value += "I2 AS " + currentValue + "電流,";
                                    } else if (currentValue == "T相") {
                                        value += "I3 AS " + currentValue + "電流,";
                                    } else if (currentValue == "平均") {
                                        value += "Iavg AS " + currentValue + "電流,";
                                    }
                                }
                            });
                            $("#<%=V_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "R相") {
                                        value += "V1 AS " + currentValue + "電壓,";
                                    } else if (currentValue == "S相") {
                                        value += "V2 AS " + currentValue + "電壓,";
                                    } else if (currentValue == "T相") {
                                        value += "V3 AS " + currentValue + "電壓,";
                                    } else if (currentValue == "平均") {
                                        value += "Vavg AS " + currentValue + "電壓,";
                                    }
                                }
                            });
                            $("#<%=W_CheckBoxList.ClientID%> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "實功") {
                                        value += "W AS " + currentValue + ",";
                                    } else if (currentValue == "虛功") {
                                        value += "V_ar AS " + currentValue + ",";
                                    } else if (currentValue == "視在") {
                                        value += "VA AS " + currentValue + ",";
                                    } else if (currentValue == "功因") {
                                        value += "PF AS " + currentValue + ",";
                                    }
                                }
                            });
                            $("#<%=Mode_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "模式1") {
                                        value += "Mode1 AS " + currentValue + ",";
                                    } else if (currentValue == "模式2") {
                                        value += "Mode2 AS " + currentValue + ",";
                                    } else if (currentValue == "模式3") {
                                        value += "Mode3 AS " + currentValue + ",";
                                    } else if (currentValue == "模式4") {
                                        value += "Mode4 AS " + currentValue + ",";
                                    }
                                }
                            });
                            $("#<%=DeMand_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "尖峰") {
                                        value += "Demand AS " + currentValue + "需量,";
                                    } else if (currentValue == "半尖峰") {
                                        value += "DemandHalf AS " + currentValue + "需量,";
                                    } else if (currentValue == "週六半尖峰") {
                                        value += "DemandSatHalf AS " + currentValue + "需量,";
                                    } else if (currentValue == "離峰") {
                                        value += "DemandOff AS " + currentValue + "需量,";
                                    } 
                                }
                            });
                            $("#<%=E_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).next('label').text();
                                if (currentValue != '') {
                                    if (currentValue == "尖峰") {
                                        value += "RushHour AS " + currentValue + "電量,";
                                    } else if (currentValue == "半尖峰") {
                                        value += "HalfHour AS " + currentValue + "電量,";
                                    } else if (currentValue == "週六半尖峰") {
                                        value += "SatHalfHour AS " + currentValue + "電量,";
                                    } else if (currentValue == "離峰") {
                                        value += "OffHour AS " + currentValue + "電量,";
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
                                        var HrefAddress = "Record.aspx?group=" + group + "&node=" + node + "&datetime=" + datetime + "&interval=" + interval + "&value=" + value;
                                        $('.imageButtonFinderClass').colorbox({ innerWidth: 910, innerHeight: 570, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                                    } else {
                                        alertify.alert("不可勾選超過十個電表");
                                        return false;
                                    }
                                } else {
                                    alertify.alert("請勾選電表");
                                    return false;
                                }
                            } else {
                                alertify.alert("您尚未勾選查詢項目");
                                return false;
                            }
                        } else {
                            alertify.alert("請搜尋30天內資料");
                            return false;
                        }
                    } else {
                        alertify.alert("開始日期不可大於结束日期");
                        return false;   //取消postback
                    }
                } else {
                    alertify.alert("請選擇日期");
                    return false;
                }
            });
            $("#<%= submit_btn.ClientID%>").click(function () {
                if ($('select[id$=Group_DropDownList] option:selected').val() == "請選擇") {
                    alertify.alert("請選擇群組");
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
                    <asp:Label ID="Label2" runat="server" Text="電力數值紀錄表" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                <td colspan="3">
                                    <img src="img/date2.png" />時間區間
			                        <asp:TextBox ID="Date_txt1" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    <asp:DropDownList ID="begin_hh" runat="server"></asp:DropDownList>時
                                    ～
                                    <asp:TextBox ID="Date_txt2" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                    <asp:DropDownList ID="end_hh" runat="server"></asp:DropDownList>時
                                </td>
                                <td colspan="2" nowrap="nowrap">時間間隔
			                        <asp:DropDownList ID="interval_DDList" runat="server">
                                        <asp:ListItem Value="1">1分鐘</asp:ListItem>
                                        <asp:ListItem Value="5">5分鐘</asp:ListItem>
                                        <asp:ListItem Value="30">30分鐘</asp:ListItem>
                                        <asp:ListItem Value="60">1小時</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap">
                                    <img src="img/compare.png" />查詢項目</td>
                                <td nowrap="nowrap">
                                    <asp:CheckBox ID="V_CheckBox" runat="server" Text="電壓" onClick="V_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="V_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Value="V1">R相</asp:ListItem>
                                        <asp:ListItem Value="V2">S相</asp:ListItem>
                                        <asp:ListItem Value="V3">T相</asp:ListItem>
                                        <asp:ListItem Value="Vavg">平均</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="Mode_CheckBox" runat="server" Text="預測" onClick="Mode_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="Mode_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Value="Mode1">模式1</asp:ListItem>
                                        <asp:ListItem Value="Mode2">模式2</asp:ListItem>
                                        <asp:ListItem Value="Mode3">模式3</asp:ListItem>
                                        <asp:ListItem Value="Mode4">模式4</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td nowrap="nowrap">
                                    <asp:CheckBox ID="i_CheckBox" runat="server" Text="電流" onClick="I_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="I_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Value="I1">R相</asp:ListItem>
                                        <asp:ListItem Value="I2">S相</asp:ListItem>
                                        <asp:ListItem Value="I3">T相</asp:ListItem>
                                        <asp:ListItem Value="Iavg">平均</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="DeMand_CheckBox" runat="server" Text="需量" onClick="DeMand_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="DeMand_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="310px">
                                        <asp:ListItem Value="DeMand">尖峰</asp:ListItem>
                                        <asp:ListItem Value="DemandHalf">半尖峰</asp:ListItem>
                                        <asp:ListItem Value="DemandSatHalf">週六半尖峰</asp:ListItem>
                                        <asp:ListItem Value="DemandOff">離峰</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="W_CheckBox" runat="server" Text="功率" onClick="W_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="W_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="289px">
                                        <asp:ListItem Value="W">實功</asp:ListItem>
                                        <asp:ListItem Value="V_ar">虛功</asp:ListItem>
                                        <asp:ListItem Value="VA">視在</asp:ListItem>
                                        <asp:ListItem Value="PF">功因</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="E_CheckBox" runat="server" Text="電量" onClick="E_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="E_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="310px">
                                        <asp:ListItem Value="RushHour">尖峰</asp:ListItem>
                                        <asp:ListItem Value="HalfHour">半尖峰</asp:ListItem>
                                        <asp:ListItem Value="SatHalfHour">週六半尖峰</asp:ListItem>
                                        <asp:ListItem Value="OffHour">離峰</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4" align="left">
                                    <asp:CheckBoxList ID="KWh_CheckBoxList" runat="server" Font-Bold="True">
                                        <asp:ListItem Value="KWh">用電度數</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
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
                                                                                    <img src="img/icon-account.png" />群組
                                                                                    <asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <img src="img/compare.png" />顯示名稱</td>
                                                                                <td align="left" valign="middle">
                                                                                    <asp:CheckBox ID="CtrlNr_CB" runat="server" Text="ECO5編號" /><br />
                                                                                    <asp:CheckBox ID="Position_CB" runat="server" Text="安裝位置" />
                                                                                </td>
                                                                                <td align="left" valign="middle">
                                                                                    <asp:CheckBox ID="MeterId_CB" runat="server" Text="電表編號" /><br />
                                                                                    <asp:CheckBox ID="LineNum_CB" runat="server" Text="單線圖編號" />
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:Button ID="submit_btn" runat="server" Text="查詢" />
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
                                                                            <asp:Label ID="Label1" runat="server" Text="ECO5資訊" Font-Bold="True" ForeColor="#CC0000" Font-Size="14pt" Font-Names="微軟正黑體"></asp:Label>
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
                                                                    <td class="td_width" align="right">ECO5帳號</td>
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
                                                                            <asp:Label ID="Label3" runat="server" Text="電表資訊" Font-Bold="True" ForeColor="#CC0000" Font-Size="14pt" Font-Names="微軟正黑體"></asp:Label>
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
                                                                <tr>
                                                                    <td class="td_width" align="right">圖面編號</td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="drawnr" runat="server" Width="130px" Enabled="False" BackColor="White" ForeColor="Black" onblur="chkDrawnr()"></asp:TextBox>
                                                                    </td>
                                                                </tr>
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
                                <td align="center" colspan="5">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:ImageButton ID="ViewDetails_btn" runat="server" ImageUrl="~/img/btn_powervalue_01.png"
                                            onmouseover="this.src='img/btn_powervalue_02.png'" onmouseout="this.src='img/btn_powervalue_01.png'"
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

