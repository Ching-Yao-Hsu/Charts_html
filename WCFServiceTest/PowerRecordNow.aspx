<%@ Page Title="" Language="VB" MasterPageFile="~/Power.master" AutoEventWireup="false" CodeFile="PowerRecordNow.aspx.vb" Inherits="PowerDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="TBCheckBoxList" Namespace="TBCheckBoxList.WebControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"> 
    <script src="js/print.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .menu a{ text-decoration:none;color:#333;}
        .menu a:hover{ text-decoration:underline;}
    </style>
    <script type="text/javascript">
        function ShowLoadingMessage() {
            document.getElementById("LoadingMessage").style.display = "block";
        }
        function custom_script(sender) {
            sure = alertify.alert(sender);
            window.location = "home.aspx"
        }
        $(document).ready(function() {
            $("#<%=submit_btn.ClientID %>").click(function() {
                var begin_time = $("#<%= Date_txt1.ClientID %>").val() + " " + $('select[id$=begin_hh] :selected').text() + ":00:00";
                var end_time = $("#<%= Date_txt2.ClientID %>").val() + " " + $('select[id$=end_hh] :selected').text() + ":00:00";
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
                        if (sum <= 30) {
                            var value = '';
                            $("#<%=I_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            $("#<%=V_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            $("#<%=W_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            $("#<%=Mode_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            $("#<%=DeMand_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            $("#<%=E_CheckBoxList.ClientID %> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            if (value.length > 0) {
                                $.blockUI({
                                    message: $('#displayBox'),
                                    css: {
                                        border: 'none',
                                        padding: '15px',
                                        backgroundColor: '#000',
                                        opacity: .5,
                                        color: '#fff',
                                        top: ($(window).height() - 100) / 2 + 'px',
                                        left: ($(window).width() - 400) / 2 + 'px',
                                        width: '400px',
                                        '-webkit-border-radius': '10px',
                                        '-moz-border-radius': '10px'
                                    }
                                });
                                setTimeout($.unblockUI, 60000);
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

            //月曆
            $("#<%= Date_txt1.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            $("#<%= Date_txt2.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd', showOn: 'button',
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
    <div id="LoadingMessage" align="center" style="font-size: 14px; width: 1000px; height: 690px; background: url('img/blank1.png'); position: absolute; display: none; z-index: 9999">網頁載入中，請稍候...</div>
    <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
        TargetControlID="panContent"
        CollapseControlID="panTitle"
        ExpandControlID="panTitle"
        TextLabelID="Label1"
        ExpandedText="（隱藏內容...）"
        CollapsedText="（顯示內容...）"
        ImageControlID="Image1"
        ExpandedImage="~/img/top_arrow.png"
        CollapsedImage="~/img/down_arrow.png"
        ExpandDirection="Vertical"
        SuppressPostBack="true" Collapsed="True" CollapsedSize="0">
    </ajaxToolkit:CollapsiblePanelExtender>

    <asp:Panel ID="panTitle" runat="server" Width="300px" Height="30px">
        <div style="padding: 5px; cursor: hand;">
            <div style="float: left;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/down_arrow.png" Width="18px" />
            </div>
            <div style="float: left; margin-left: 10px; font-weight: bolder">
                查詢
            </div>
            <div style="float: right; color: brown">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="panContent" runat="server" Style="padding-left: 10px;">
        <table width="900px">
            <tr>
                <td>
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff;">
                        <table>
                            <tr>
                                <td colspan="3">
                                    <img src="img/date2.png" />時間區間
			                        <asp:TextBox ID="Date_txt1" runat="server" Width="70px"></asp:TextBox>
                                    <asp:DropDownList ID="begin_hh" runat="server"></asp:DropDownList>時～
                                    <asp:TextBox ID="Date_txt2" runat="server" Width="70px"></asp:TextBox>
                                    <asp:DropDownList ID="end_hh" runat="server"></asp:DropDownList>時
                                </td>
                                <td colspan="2">間隔
			                        <asp:DropDownList ID="interval_DDList" runat="server">
                                        <asp:ListItem Value="1">1分鐘</asp:ListItem>
                                        <asp:ListItem Value="5">5分鐘</asp:ListItem>
                                        <asp:ListItem Value="30">30分鐘</asp:ListItem>
                                        <asp:ListItem Value="60">1小時</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="img/compare.png" />查詢項目</td>
                                <td>
                                    <asp:CheckBox ID="V_CheckBox" runat="server" Text="電壓" onClick="V_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="V_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Value="V1">R相</asp:ListItem>
                                        <asp:ListItem Value="V2">S相</asp:ListItem>
                                        <asp:ListItem Value="V3">T相</asp:ListItem>
                                        <asp:ListItem Value="Vavg">平均電壓</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="Mode_CheckBox" runat="server" Text="預測" onClick="Mode_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="Mode_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Value="Mode1">模式1</asp:ListItem>
                                        <asp:ListItem Value="Mode2">模式2</asp:ListItem>
                                        <asp:ListItem Value="Mode3">模式3</asp:ListItem>
                                        <asp:ListItem Value="Mode4">模式4</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="i_CheckBox" runat="server" Text="電流" onClick="I_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="I_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Value="I1">R相</asp:ListItem>
                                        <asp:ListItem Value="I2">S相</asp:ListItem>
                                        <asp:ListItem Value="I3">T相</asp:ListItem>
                                        <asp:ListItem Value="Iavg">平均電流</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="DeMand_CheckBox" runat="server" Text="需量" onClick="DeMand_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="DeMand_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="310px">
                                        <asp:ListItem Value="DeMand">尖峰</asp:ListItem>
                                        <asp:ListItem Value="DemandHalf">半尖峰</asp:ListItem>
                                        <asp:ListItem Value="DemandSatHalf">週六半尖峰</asp:ListItem>
                                        <asp:ListItem Value="DemandOff">離峰</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="W_CheckBox" runat="server" Text="功率" onClick="W_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="W_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="360px">
                                        <asp:ListItem Value="W">實功</asp:ListItem>
                                        <asp:ListItem Value="V_ar">虛功</asp:ListItem>
                                        <asp:ListItem Value="VA">視在</asp:ListItem>
                                        <asp:ListItem Value="PF">功因</asp:ListItem>
                                        <asp:ListItem Value="KWh">用電度數</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="E_CheckBox" runat="server" Text="電量" onClick="E_checkAll(this);" Font-Bold="True" />
                                </td>
                                <td>
                                    <cc1:TBCheckBoxList ID="E_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="310px">
                                        <asp:ListItem Value="RushHour">尖峰</asp:ListItem>
                                        <asp:ListItem Value="HalfHour">半尖峰</asp:ListItem>
                                        <asp:ListItem Value="SatHalfHour">週六半尖峰</asp:ListItem>
                                        <asp:ListItem Value="OffHour">離峰</asp:ListItem>
                                    </cc1:TBCheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center">
                                    <asp:Button ID="submit_btn" runat="server" Text="確認" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            
            <tr>
                <td colspan="5">
                    <asp:Label ID="export" runat="server" Text="輸出"></asp:Label>
                    <asp:ImageButton ID="Print" runat="server" ImageUrl="~/img/print-icon.png"
                        OnClientClick="printScreen(Print_Table)" OnClick="print_Click" ToolTip="列印" />
                    <asp:ImageButton ID="Excel" runat="server" ImageUrl="img/excel-icon.png" ToolTip="匯出Excel" />
                    <asp:ImageButton ID="PDF" runat="server" ImageUrl="img/PDF-icon.png" ToolTip="匯出PDF" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div align="center" style="background-color: white;width:900px;">
        <asp:Label ID="msg" runat="server" Text="查無資料" Visible="false"></asp:Label>
    </div>
    <asp:Panel ID="Print_Detail" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Width="920px">
        <div id="Print_Table" class="inner" style="padding-left: 5px;">
            <table width="900px">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label2" runat="server" Text="電力數值紀錄表" Font-Bold="true" Font-Size="22px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:50px;">
                        <%--<asp:Label ID="Label3" runat="server" Text="日期：" Font-Size="14px"></asp:Label>--%>
                        <asp:Label ID="Date_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:50px;">
                        <%--<asp:Label ID="Label4" runat="server" Text="編號：" Font-Size="14px"></asp:Label>--%>
                        <asp:Label ID="Num_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:50px;">
                        <%--<asp:Label ID="Label5" runat="server" Text="位置：" Font-Size="14px"></asp:Label>--%>
                        <asp:Label ID="Position_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridView1" runat="server" BackColor="White"
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                            DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Horizontal"
                            Width="900px">
                            <RowStyle HorizontalAlign="Center" Wrap="false" />
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                            <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" />
                            <AlternatingRowStyle BackColor="#fafafa" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    <br />
    <div id="displayBox" style="display: none;">
        <img src="img/loading-bar.gif" /><br />
        <h1>Please wait...</h1>
    </div>
</asp:Content>

