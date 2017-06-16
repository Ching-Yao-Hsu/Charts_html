<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="PowerSummaryReport.aspx.vb" Inherits="SummaryReport" %>

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
        function ShowLoadingMessage() {
            document.getElementById("LoadingMessage").style.display = "block";
        }
        $(document).ready(function () {
            var HrefAddress = "support/SummaryReportSupport.htm";
            $('.support').colorbox({ innerWidth: 750, innerHeight: 540, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            $('.imageButtonFinderClass1').click(function () {
                if ($('select[id$=Group_DropDownList1] option:selected').val() != "Please choose...") {
                    if ($("#<%= Date_txt1.ClientID%>").val() != "") {
                        var value = '';
                        $("#<%=item_CheckBoxList1.ClientID%> input[type=checkbox]:checked").each(function () {
                            var currentValue = $(this).next('label').text();
                            if (currentValue != '') {
                                if (currentValue == "Current") {
                                    value += "[Current],";
                                } else if (currentValue == "Voltage") {
                                    value += "Voltage,";
                                } else if (currentValue == "Power") {
                                    value += "Power,";
                                } else if (currentValue == "Electricity consumption") {
                                    value += "Electricity_consumption,";
                                } else if (currentValue == "ElectricityValue") {
                                    value += "ElectricityValue,";
                                }
                            }
                        });
                        if (value != '') {
                            var e = document.getElementById("<%=Group_DropDownList1.ClientID%>");
                            var group = e.options[e.selectedIndex].value;
                            var datetime = $("#<%= Date_txt1.ClientID%>").val();
                            var HrefAddress = "SummaryDayReport_en.aspx?group=" + group + "&datetime=" + datetime + "&value=" + value;
                            var value = value.substring(0, value.length - 1);
                            $('.imageButtonFinderClass1').colorbox({ innerWidth: 900, innerHeight: 580, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                        } else {
                            alertify.alert("Please select query items.");
                            return false;
                        }
                    } else {
                        alertify.alert("Please select a Date.");
                        return false;
                    }
                } else {
                    alertify.alert("Please select a Group.");
                    return false;
                }
            });
            
            $('.imageButtonFinderClass2').click(function () {
                if ($('select[id$=Group_DropDownList2] option:selected').val() != "Please choose...") {
                    if ($("#<%= Date_txt2.ClientID%>").val() != "") {
                        var value = '';
                        $("#<%=item_CheckBoxList2.ClientID%> input[type=checkbox]:checked").each(function () {
                            var currentValue = $(this).next('label').text();
                            if (currentValue != '') {
                                if (currentValue == "Current") {
                                    value += "[Current],";
                                } else if (currentValue == "Voltage") {
                                    value += "Voltage,";
                                } else if (currentValue == "Power") {
                                    value += "Power,";
                                } else if (currentValue == "Max Demand") {
                                    value += "Max_Demand,";
                                } else if (currentValue == "Electricity consumption") {
                                    value += "Electricity_consumption,";
                                } else if (currentValue == "ElectricityValue") {
                                    value += "ElectricityValue,";
                                }
                            }
                        });
                        if (value != '') {
                            var e = document.getElementById("<%=Group_DropDownList2.ClientID%>");
                            var group = e.options[e.selectedIndex].value;
                            var datetime = $("#<%= Date_txt2.ClientID%>").val();
                            var HrefAddress = "SummaryMonthReport_en.aspx?group=" + group + "&datetime=" + datetime + "&value=" + value;
                            var value = value.substring(0, value.length - 1);
                            $('.imageButtonFinderClass2').colorbox({ innerWidth: 1080, innerHeight: 580, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                        } else {
                            alertify.alert("Please select query items.");
                            return false;
                        }
                    } else {
                        alertify.alert("Please select a Month.");
                        return false;
                    }
                } else {
                    alertify.alert("Please select a Group");
                    return false;
                }
            });

            //月曆
            $("#<%= Date_txt1.ClientID%>").datepicker({
                dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            $("#<%= Date_txt2.ClientID%>").datepicker({
                dateFormat: 'yy/mm', showOn: 'button',
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
                    <asp:Label ID="Label2" runat="server" Text="Summary Daily report" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right:10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff; width:795px;">
                        <table>
                            <tr>
                                <td align="left">
                                    <img src="img/date2.png" />Date
			                        <asp:TextBox ID="Date_txt1" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                </td>
                                <td align="left" nowrap="nowrap" style="padding-left:20px;">
                                    <img src="img/icon-account.png" />Group
                                    <asp:DropDownList ID="Group_DropDownList1" runat="server"></asp:DropDownList>
                                </td>
                                <td align="center" rowspan="2" style="padding-left:20px;">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:ImageButton ID="ViewDetails_btn" runat="server" ImageUrl="~/img/btn_all_01en.png"
                                            onmouseover="this.src='img/btn_all_02en.png'" onmouseout="this.src='img/btn_all_01en.png'"
                                            CssClass="imageButtonFinderClass1" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="img/compare.png" />Inquiry Item
                                            </td>
                                            <td align="left" nowrap="nowrap">
                                                <asp:CheckBoxList ID="item_CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="I" Selected="True">Current</asp:ListItem>
                                                    <asp:ListItem Value="V" Selected="True">Voltage</asp:ListItem>
                                                    <asp:ListItem Value="W" Selected="True">Power</asp:ListItem>
                                                    <asp:ListItem Value="E" Selected="True">Electricity consumption</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Summary Monthly report" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff; width:795px;">
                        <table>
                            <tr>
                                <td align="left">
                                    <img src="img/date2.png" />Month
			                        <asp:textbox id="Date_txt2" runat="server" width="70px" backcolor="White" forecolor="Black"></asp:textbox>
                                </td>
                                <td align="left" nowrap="nowrap" style="padding-left: 20px;">
                                    <img src="img/icon-account.png" />Group
                                    <asp:dropdownlist id="Group_DropDownList2" runat="server"></asp:dropdownlist>
                                </td>
                                <td align="center" rowspan="2" style="padding-left: 20px;">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:imagebutton id="ViewDetails_btn2" runat="server" imageurl="~/img/btn_all_01en.png"
                                            onmouseover="this.src='img/btn_all_02en.png'" onmouseout="this.src='img/btn_all_01en.png'"
                                            cssclass="imageButtonFinderClass2" />
                                    </div>
                                </td>
                            </tr>
                            <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="img/compare.png" />Inquiry Item
                                            </td>
                                            <td align="left" nowrap="nowrap">
                                                <asp:CheckBoxList ID="item_CheckBoxList2" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="I" Selected="True">Current</asp:ListItem>
                                                    <asp:ListItem Value="V" Selected="True">Voltage</asp:ListItem>
                                                    <asp:ListItem Value="W" Selected="True">Power</asp:ListItem>
                                                    <asp:ListItem Value="D" Selected="True">Max Demand</asp:ListItem>
                                                    <asp:ListItem Value="E" Selected="True">Electricity consumption</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>

