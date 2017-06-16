<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="PowerFeeCalculate.aspx.vb" Inherits="PowerFeeCalculate" %>

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
                if ($('select[id$=Group_DropDownList] option:selected').val() != "請選擇") {
                    if ($("#<%= Date_txt2.ClientID%>").val() != "") {
                        var i = 0
                        var sTxtValue = ""
                        //var re = /^[0-9]+$/;  //數字
                        var re = /^[0-9]+([.]{1}[0-9]{1,2})?$/;      //小數點

                        if ($("#<%= txtR.ClientID%>").val() != "") {

                            if (!re.test($("#<%= txtR.ClientID%>").val())) {
                                alert("契約容量(尖峰)，欄位必須為數值！");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtR.ClientID%>").val();
                            }
                        }
                        sTxtValue += ',';
                        if ($("#<%= txtH.ClientID%>").val() != "") {
                            if (!re.test($("#<%= txtH.ClientID%>").val())) {
                                alert("契約容量(半尖峰)，欄位必須為數值！");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtH.ClientID%>").val();
                            }
                        }
                        sTxtValue += ',';
                        if ($("#<%= txtS.ClientID%>").val() != "") {
                            if (!re.test($("#<%= txtS.ClientID%>").val())) {
                                alert("契約容量(周六半尖峰)，欄位必須為數值！");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtS.ClientID%>").val();
                            }
                        }
                        sTxtValue += ',';
                        if ($("#<%= txtO.ClientID%>").val() != "") {
                            if (!re.test($("#<%= txtO.ClientID%>").val())) {
                                alert("契約容量(離峰)，欄位必須為數值！");
                                return false;
                            } else {
                                i = i + 1;
                                sTxtValue += $("#<%= txtO.ClientID%>").val();
                            }
                        }

                        var sRadiovalue = '';
                        sRadiovalue = $('input[name=RadSelect]:checked').val();
                        if (i == 0 && sRadiovalue != "41") {
                            alertify.alert("請輸入契約容量,至少一值!");
                            return false;
                        } else {

                            if (sRadiovalue != '') {

                                if (sRadiovalue == "41") {
                                    if (!re.test($("#<%= txtSelf.ClientID%>").val())) {
                                        alert("電費計算(自訂)，欄位必須為數值！");
                                        return false;
                                    }
                                }

                                var e = document.getElementById("<%=Group_DropDownList.ClientID%>");
                                var group = e.options[e.selectedIndex].value;
                                var datetime = $("#<%= Date_txt2.ClientID%>").val();
                                var sSelf = $("#<%= txtSelf.ClientID%>").val();
                                var HrefAddress = "PowerFeeCalculateReport.aspx?group=" + group + "&datetime=" + datetime + "&stxtValue=" + sTxtValue + "&sRadioValue=" + sRadiovalue + "&sSelf=" + sSelf;
                                //alert(HrefAddress);
                                $('.imageButtonFinderClass2').colorbox({ innerWidth: 1030, innerHeight: 580, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                            } else {
                                alertify.alert("請選擇查詢項目");
                                return false;
                            }
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
                    <asp:Label ID="Label1" runat="server" Text="電費試算表" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
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
                                    <asp:dropdownlist id="Group_DropDownList" runat="server"></asp:dropdownlist>
                                </td>
                                <td align="center" rowspan="3" style="padding-left: 20px;">
                                    <div style="height: 50px; padding-top: 5px;">
                                        <asp:imagebutton id="ViewDetails_btn" runat="server" imageurl="~/img/btn_form_01.png"
                                            onmouseover="this.src='img/btn_form_02.png'" onmouseout="this.src='img/btn_form_01.png'"
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
                                            <td rowspan="2">契約容量</td>
                                            <td align="center" nowrap="nowrap" width="100px">經常</td>
                                            <td align="center" nowrap="nowrap" width="150px">半尖峰(非夏月)</td>
                                            <td align="center" nowrap="nowrap" width="100px">周六半尖峰</td>
                                            <td align="center" nowrap="nowrap" width="100px">離峰</td>
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
                                <td>電費計算方式</td>
                            </tr>
                            <tr>
                            <td colspan="3">
                                　　<input type="radio" name="RadSelect" value="11" checked="checked"/> 表燈-非營業
                                　<input type="radio" name="RadSelect" value="12" /> 表燈-營業
                                　<input type="radio" name="RadSelect" value="13" /> 表燈-時間 <br />
                                　　<input type="radio" name="RadSelect" value="21" /> 低壓-非時間
                                　<input type="radio" name="RadSelect" value="22" /> 低壓-時間 <br />
                                　　<input type="radio" name="RadSelect" value="31" /> 高壓-二段式
                                　<input type="radio" name="RadSelect" value="32" /> 高壓-三段式 <br />
                                　　<input type="radio" name="RadSelect" value="41" /> 自訂 <asp:textbox id="txtSelf" runat="server" width="70px" backcolor="White" forecolor="Black" Text="" ></asp:textbox><br />
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

