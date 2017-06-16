<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="PowerSummaryReport.aspx.vb" Inherits="SummaryReport" %>

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
                if ($('select[id$=Group_DropDownList1] option:selected').val() != "請選擇") {
                    if ($("#<%= Date_txt1.ClientID%>").val() != "") {
                        var value = '';
                        $("#<%=item_CheckBoxList1.ClientID%> input[type=checkbox]:checked").each(function () {
                            var currentValue = $(this).next('label').text();
                            if (currentValue != '') {
                                if (currentValue == "電流") {
                                    value += "電流,";
                                } else if (currentValue == "電壓") {
                                    value += "電壓,";
                                } else if (currentValue == "功率") {
                                    value += "功率,";
                                } else if (currentValue == "用電量") {
                                    value += "用電量,";
                                } else if (currentValue == "電表值") {
                                    value += "電表值,";
                                }
                            }
                        });
                        if (value != '') {
                            var e = document.getElementById("<%=Group_DropDownList1.ClientID%>");
                            var group = e.options[e.selectedIndex].value;
                            var datetime = $("#<%= Date_txt1.ClientID%>").val();
                            var HrefAddress = "SummaryDayReport.aspx?group=" + group + "&datetime=" + datetime + "&value=" + value;
                            var value = value.substring(0, value.length - 1);
                            $('.imageButtonFinderClass1').colorbox({ innerWidth: 900, innerHeight: 580, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                        } else {
                            alertify.alert("請選擇查詢項目");
                            return false;
                        }
                    } else {
                        alertify.alert("請選擇日期");
                        return false;
                    }
                } else {
                    alertify.alert("請選擇群組");
                    return false;
                }
            });
            
            $('.imageButtonFinderClass2').click(function () {
                if ($('select[id$=Group_DropDownList2] option:selected').val() != "請選擇") {
                    if ($("#<%= Date_txt2.ClientID%>").val() != "") {
                        var value = '';
                        $("#<%=item_CheckBoxList2.ClientID%> input[type=checkbox]:checked").each(function () {
                            var currentValue = $(this).next('label').text();
                            if (currentValue != '') {
                                if (currentValue == "電流") {
                                    value += "電流,";
                                } else if (currentValue == "電壓") {
                                    value += "電壓,";
                                } else if (currentValue == "功率") {
                                    value += "功率,";
                                } else if (currentValue == "最大需量") {
                                    value += "最大需量,";
                                } else if (currentValue == "用電量") {
                                    value += "用電量,";
                                } else if (currentValue == "電表值") {
                                    value += "電表值,";
                                }
                            }
                        });
                        if (value != '') {
                            var e = document.getElementById("<%=Group_DropDownList2.ClientID%>");
                            var group = e.options[e.selectedIndex].value;
                            var datetime = $("#<%= Date_txt2.ClientID%>").val();
                            var HrefAddress = "SummaryMonthReport.aspx?group=" + group + "&datetime=" + datetime + "&value=" + value;
                            var value = value.substring(0, value.length - 1);
                            $('.imageButtonFinderClass2').colorbox({ innerWidth: 1080, innerHeight: 580, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                        } else {
                            alertify.alert("請選擇查詢項目");
                            return false;
                        }
                    } else {
                        alertify.alert("請選擇月份");
                        return false;
                    }
                } else {
                    alertify.alert("請選擇群組");
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
                    <asp:Label ID="Label2" runat="server" Text="總日報表" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                    <img src="img/date2.png" />日期
			                        <asp:TextBox ID="Date_txt1" runat="server" Width="70px" BackColor="White" ForeColor="Black"></asp:TextBox>
                                </td>
                                <td align="left" nowrap="nowrap" style="padding-left:20px;">
                                    <img src="img/icon-account.png" />群組
                                    <asp:DropDownList ID="Group_DropDownList1" runat="server"></asp:DropDownList>
                                </td>
                                <td align="center" rowspan="2" style="padding-left:20px;">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:ImageButton ID="ViewDetails_btn" runat="server" ImageUrl="~/img/btn_all_01.png"
                                            onmouseover="this.src='img/btn_all_02.png'" onmouseout="this.src='img/btn_all_01.png'"
                                            CssClass="imageButtonFinderClass1" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="img/compare.png" />查詢項目
                                            </td>
                                            <td align="left" nowrap="nowrap">
                                                <asp:CheckBoxList ID="item_CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="I" Selected="True">電流</asp:ListItem>
                                                    <asp:ListItem Value="V" Selected="True">電壓</asp:ListItem>
                                                    <asp:ListItem Value="W" Selected="True">功率</asp:ListItem>
                                                    <asp:ListItem Value="E" Selected="True">用電量</asp:ListItem>
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
                    <asp:Label ID="Label1" runat="server" Text="總月報表" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff; width:795px;">
                        <table>
                            <tr>
                                <td align="left">
                                    <img src="img/date2.png" />月份
			                        <asp:textbox id="Date_txt2" runat="server" width="70px" backcolor="White" forecolor="Black"></asp:textbox>
                                </td>
                                <td align="left" nowrap="nowrap" style="padding-left: 20px;">
                                    <img src="img/icon-account.png" />群組
                                    <asp:dropdownlist id="Group_DropDownList2" runat="server"></asp:dropdownlist>
                                </td>
                                <td align="center" rowspan="2" style="padding-left: 20px;">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:imagebutton id="ViewDetails_btn2" runat="server" imageurl="~/img/btn_all_01.png"
                                            onmouseover="this.src='img/btn_all_02.png'" onmouseout="this.src='img/btn_all_01.png'"
                                            cssclass="imageButtonFinderClass2" />
                                    </div>
                                </td>
                            </tr>
                            <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="img/compare.png" />查詢項目
                                            </td>
                                            <td align="left" nowrap="nowrap">
                                                <asp:CheckBoxList ID="item_CheckBoxList2" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="I" Selected="True">電流</asp:ListItem>
                                                    <asp:ListItem Value="V" Selected="True">電壓</asp:ListItem>
                                                    <asp:ListItem Value="W" Selected="True">功率</asp:ListItem>
                                                    <asp:ListItem Value="D" Selected="True">最大需量</asp:ListItem>
                                                    <asp:ListItem Value="E" Selected="True">用電量</asp:ListItem>
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

