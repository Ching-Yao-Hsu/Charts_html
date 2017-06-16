<%@ Page Title="" Language="VB" MasterPageFile="~/index_en.master" AutoEventWireup="false" CodeFile="PowerFeeCalculate.aspx.vb" Inherits="PowerFeeCalculate" %>

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
            var HrefAddress = "support/PowerFeeCalculateSupport.htm";
            $('.support').colorbox({ innerWidth: 750, innerHeight: 540, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            $("input[type=radio]").on("click", function () {
                if ($("input[type=radio]:checked").val() == "41") {
                    $("#<%= txtSelf.ClientID%>").removeAttr('disabled');
                } else {
                    $("#<%= txtSelf.ClientID%>").attr('disabled', 'disabled');
                    $("#<%= txtSelf.ClientID%>").val("");
                }
            });

            $('.imageButtonFinderClass2').click(function () {
                if ($('select[id$=Group_DropDownList] option:selected').val() != "Please choose...") {
                    if ($("#<%= Date_txt2.ClientID%>").val() != "") {
                        var i = 0
                        var sTxtValue = ""
                        //var re = /^[0-9]+$/;  //數字
                        var re = /^[0-9]+([.]{1}[0-9]{1,2})?$/;      //小數點

                        if ($("#<%= txtR.ClientID%>").val() != "") {

                            if (!re.test($("#<%= txtR.ClientID%>").val())) {
                                alert("Contract capacity (peak), the field must be numeric!");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtR.ClientID%>").val();
                            }
                        }
                        sTxtValue += ',';
                        if ($("#<%= txtH.ClientID%>").val() != "") {
                            if (!re.test($("#<%= txtH.ClientID%>").val())) {
                                alert("Contract capacity (half peak), the field must be numeric!");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtH.ClientID%>").val();
                            }
                        }
                        sTxtValue += ',';
                        if ($("#<%= txtS.ClientID%>").val() != "") {
                            if (!re.test($("#<%= txtS.ClientID%>").val())) {
                                alert("Contract capacity (Saturday half peak), the field must be numeric!");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtS.ClientID%>").val();
                            }
                        }
                        sTxtValue += ',';
                        if ($("#<%= txtO.ClientID%>").val() != "") {
                            if (!re.test($("#<%= txtO.ClientID%>").val())) {
                                alert("Contract capacity (off-peak), the field must be numeric!");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtO.ClientID%>").val();
                            }
                        }

                        var sRadiovalue = '';
                        sRadiovalue = $('input[name=RadSelect]:checked').val();
                        if (i == 0 && sRadiovalue != "41") {
                            alertify.alert("Please enter a contract capacity, at least one value!");
                            return false;
                        } else {

                            if (sRadiovalue != '') {

                                if (sRadiovalue == "41") {
                                    if (!re.test($("#<%= txtSelf.ClientID%>").val())) {
                                        alert("Electricity calculation (custom), the field must be numeric!");
                                        return false;
                                    }
                                }

                                var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
                                var group = e.options[e.selectedIndex].value;
                                var datetime = $("#<%= Date_txt2.ClientID%>").val();
                                var sSelf = $("#<%= txtSelf.ClientID%>").val();
                                var HrefAddress = "PowerFeeCalculateReport_en.aspx?group=" + group + "&datetime=" + datetime + "&stxtValue=" + sTxtValue + "&sRadioValue=" + sRadiovalue + "&sSelf=" + sSelf;
                                //alert(HrefAddress);
                                $('.imageButtonFinderClass2').colorbox({ innerWidth: 1030, innerHeight: 580, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                            } else {
                                alertify.alert("Please select query items");
                                return false;
                            }
                        }
                    } else {
                        alertify.alert("Please select month");
                        return false;
                    }
                } else {
                    alertify.alert("Please select Group!");
                    return false;
                }
            });

            //月曆
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
                <td align="left" colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Electricity spreadsheet" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                    <asp:dropdownlist id="Group_DropDownList" runat="server"></asp:dropdownlist>
                                </td>
                                <td align="center" rowspan="3" style="padding-left: 20px;">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:imagebutton id="ViewDetails_btn" runat="server" imageurl="~/img/btn_form_01en.png"
                                            onmouseover="this.src='img/btn_form_02en.png'" onmouseout="this.src='img/btn_form_01en.png'"
                                            cssclass="imageButtonFinderClass2" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                            <td colspan="2">
                                    <table>
                                        <tr>
                                            <td rowspan="2">Contract Capacity</td>
                                            <td align="center" nowrap="nowrap" width="100px">often</td>
                                            <td align="center" nowrap="nowrap" width="150px">Half the peak (non-summer months)</td>
                                            <td align="center" nowrap="nowrap" width="100px">Saturday half peak</td>
                                            <td align="center" nowrap="nowrap" width="100px">Off-peak</td>
                                        </tr>
                                        <tr>
                                            <td align="center" nowrap="nowrap"><asp:textbox id="txtR" runat="server" width="70px" backcolor="White" forecolor="Black"></asp:textbox></td>
                                            <td align="center" nowrap="nowrap"><asp:textbox id="txtH" runat="server" width="70px" backcolor="White" forecolor="Black"></asp:textbox></td>
                                            <td align="center" nowrap="nowrap"><asp:textbox id="txtS" runat="server" width="70px" backcolor="White" forecolor="Black"></asp:textbox></td>
                                            <td align="center" nowrap="nowrap"><asp:textbox id="txtO" runat="server" width="70px" backcolor="White" forecolor="Black"></asp:textbox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>Electricity calculation</td>
                            </tr>
                            <tr>
                            <td colspan="3">
                                　　<input type="radio" name="RadSelect" value="11" checked="checked"/> Table lamp - non-operating
                                　<input type="radio" name="RadSelect" value="12" /> Table Lamp - Business
                                　<input type="radio" name="RadSelect" value="13" /> Table lamp - time <br />
                                　　<input type="radio" name="RadSelect" value="21" /> Low pressure - non-time
                                　<input type="radio" name="RadSelect" value="22" /> Low pressure - time <br />
                                　　<input type="radio" name="RadSelect" value="31" /> High Pressure - Two-stage
                                　<input type="radio" name="RadSelect" value="32" /> High pressure - three-stage <br />
                                　　<input type="radio" name="RadSelect" value="41" /> Custom <asp:textbox id="txtSelf" runat="server" width="70px" backcolor="White" forecolor="Black" Text="" ></asp:textbox><br />
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
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>

        <table>  <tr> <td> 
            


                      </td> </tr></table>
    </div>
</asp:Content>

