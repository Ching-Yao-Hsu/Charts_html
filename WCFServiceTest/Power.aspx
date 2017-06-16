<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="Power.aspx.vb" Inherits="Power" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <fieldset>
                    <legend>控制器、電表</legend>
                    <table border="0" width="100%">
                    <tr>
                        <td>會員帳號：<asp:DropDownList ID="Account_DropDownList" runat="server"  Enabled="false"></asp:DropDownList></td>
                        <td>ECO5帳號：<asp:DropDownList ID="EcoAccount_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                        <td>ECO5編號：<asp:TextBox ID="CtrlNr" runat="server" Width="10px" Enabled="false"></asp:TextBox></td>
                        <td>電表編號：<asp:DropDownList ID="Meter_DropDownList" runat="server"></asp:DropDownList></td>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ecoAccount_DropDownList"
                            ErrorMessage="請選擇帳號" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                    </tr>
                    
                    </table>
                    </fieldset>
                    
                    <fieldset>
                    <legend>日期</legend>
                    <table border="0" width="100%">
                    <tr>
                        <td><div style="width: 105%">
                            起：<asp:TextBox ID="Date_txt1" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="CalendarImageButton1" runat="server" Height="20px"
                                    ImageUrl="~/img/calendar_Black2.png" style="background-color: transparent;cursor:hand;" />        
                                <asp:DropDownList ID="begin_hh" runat="server"></asp:DropDownList>時
                                <asp:DropDownList ID="begin_mm" runat="server"></asp:DropDownList>分～
                                
                            迄：<asp:TextBox ID="Date_txt2" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="CalendarImageButton2" runat="server" Height="20px" 
                                    ImageUrl="~/img/calendar_Black2.png" style="background-color: transparent;cursor:hand;" />
                                <asp:DropDownList ID="end_hh" runat="server"></asp:DropDownList>時
                                <asp:DropDownList ID="end_mm" runat="server"></asp:DropDownList>分
                            </div>    
                            <ajaxtoolkit:calendarextender ID="AjaxCalendarExtender1" runat="server"
                                TargetControlID="Date_txt1"
                                CssClass="cssCalendar"
                                Format="yyyy/MM/dd"
                                PopupButtonID="CalendarImageButton1" />
                            <ajaxtoolkit:calendarextender ID="AjaxCalendarextender2" runat="server"
                                TargetControlID="Date_txt2"
                                CssClass="cssCalendar"
                                Format="yyyy/MM/dd"
                                PopupButtonID="CalendarImageButton2" />       
                        </td>
                    </tr>
                    </table>
                    </fieldset>
                    
                    <fieldset>
                    <legend>查詢項目</legend>
                    <table align="left" border="0" width="100%">
                        <tr>
                            <td>
                                <asp:CheckBox ID="i_CheckBox" runat="server" Text="電流" onClick="I_checkAll(this);"/>
                            </td> 
                            <td>
                                <asp:CheckBoxList ID="I_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="350px">
                                    <asp:ListItem Value="I1">R相</asp:ListItem>
                                    <asp:ListItem Value="I2">S相</asp:ListItem>
                                    <asp:ListItem Value="I3">T相</asp:ListItem>
                                    <asp:ListItem Value="Iavg">平均電流</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="V_CheckBox" runat="server" Text="電壓" onClick="V_checkAll(this);"/>
                            </td> 
                            <td>
                                <asp:CheckBoxList ID="V_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="350px">
                                    <asp:ListItem Value="I1">R相</asp:ListItem>
                                    <asp:ListItem Value="I2">S相</asp:ListItem>
                                    <asp:ListItem Value="I3">T相</asp:ListItem>
                                    <asp:ListItem Value="Iavg">平均電壓</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="W_CheckBox" runat="server" Text="功率" onClick="W_checkAll(this);"/>
                            </td> 
                            <td>
                                <asp:CheckBoxList ID="W_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="330px">
                                    <asp:ListItem Value="W">實功</asp:ListItem>
                                    <asp:ListItem Value="V_ar">虛功</asp:ListItem>
                                    <asp:ListItem Value="VA">視在</asp:ListItem>
                                    <asp:ListItem Value="PF">功因</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Mode_CheckBox" runat="server" Text="預測模式" onClick="Mode_checkAll(this);"/>
                            </td> 
                            <td>
                                <asp:CheckBoxList ID="Mode_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="330px">
                                    <asp:ListItem Value="Mode1">模式1</asp:ListItem>
                                    <asp:ListItem Value="Mode2">模式2</asp:ListItem>
                                    <asp:ListItem Value="Mode3">模式3</asp:ListItem>
                                    <asp:ListItem Value="Mode4">模式4</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="DeMand_CheckBox" runat="server" Text="需量" onClick="DeMand_checkAll(this);"/>
                            </td> 
                            <td>
                                <asp:CheckBoxList ID="DeMand_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="380px">
                                    <asp:ListItem Value="DeMand">尖峰</asp:ListItem>
                                    <asp:ListItem Value="DemandHalf">半尖峰</asp:ListItem>
                                    <asp:ListItem Value="DemandSatHalf">週六半尖峰</asp:ListItem>
                                    <asp:ListItem Value="DemandOff">離峰</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="E_CheckBox" runat="server" Text="電量" onClick="E_checkAll(this);"/>
                            </td> 
                            <td>
                                <asp:CheckBoxList ID="E_CheckBoxList" runat="server" RepeatDirection="Horizontal" Width="380px">
                                    <asp:ListItem Value="RushHour">尖峰</asp:ListItem>
                                    <asp:ListItem Value="HalfHour">半尖峰</asp:ListItem>
                                    <asp:ListItem Value="SatHalfHour">週六半尖峰</asp:ListItem>
                                    <asp:ListItem Value="OffHour">離峰</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td height="35px">           
                                <asp:CheckBoxList ID="KWh_CheckBoxList" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="KWh">用電度數</asp:ListItem></asp:CheckBoxList>    
                            </td>

                        </tr>  
                    </table> 
                    </fieldset>
                </ContentTemplate>
         </asp:UpdatePanel>
     <div style="width:100%;" align="center">
        <asp:Button ID="submit_btn" runat="server" Text="查詢" Width="70px" Height="30px" />
    </div>
</asp:Content>

