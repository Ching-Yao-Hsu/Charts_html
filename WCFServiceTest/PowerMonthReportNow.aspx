<%@ Page Title="" Language="VB" MasterPageFile="~/Power.master" AutoEventWireup="false" CodeFile="PowerMonthReportNow.aspx.vb" Inherits="PowerMonthReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="TBCheckBoxList" Namespace="TBCheckBoxList.WebControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/print.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ShowLoadingMessage() {
            document.getElementById("LoadingMessage").style.display = "block";
        }
        function custom_script(sender) {
            sure = alertify.alert(sender);
            window.location = "home.aspx"
        }
        $(document).ready(function () {
            $("#<%=submit_btn.ClientID %>").click(function () {
                if ($("#<%= Date_Info.ClientID%>").val() != "") {
                    //讀取進度條
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
                    alertify.alert("請選擇月份");
                    return false;
                }
            });
            //月曆
            $("#<%= Date_Info.ClientID%>").datepicker({
                dateFormat: 'yy/mm', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
        });
    </script>
    <style type="text/css">
        .ui-datepicker-trigger
        {
            cursor: pointer;
            width: 20px;
            height: 20px;
        }

        .ui-datepicker-trigger
        {
            cursor: pointer;
            width: 20px;
            height: 20px;
        }

        .menu a
        {
            text-decoration: none;
            color: #333;
        }

            .menu a:hover
            {
                text-decoration: underline;
            }

        .style2
        {
            width: 100px;
            padding-left:50px;
        }

        .style3
        {
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="LoadingMessage" align="center" style="font-size: 14px; width: 950px; height: 690px; background: url('img/blank1.png'); position: absolute; display: none; z-index: 9999">網頁載入中，請稍候...</div>
    <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
        TargetControlID="panContent"
        CollapseControlID="panTitle"
        ExpandControlID="panTitle"
        TextLabelID="content"
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
                <asp:Label ID="content" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="panContent" runat="server" Width="800px" Style="padding-left: 30px;">
        <table width="800px">
            <tr>
                <td>
                    <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff; width:300px;">
                        <table>
                            <tr>
                                <td>
                                    <img src="img/date2.png" />月份
                                    <asp:TextBox ID="Date_Info" runat="server" Width="50px"></asp:TextBox>
                                </td>
                                <td style="padding-left:50px;">
                                    <asp:Button ID="submit_btn" runat="server" Text="確認" /></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            
            <tr>
                <td colspan="2" align="left">
                    <asp:Panel ID="Panel_Export" runat="server" Style="padding-left: 20px;" Visible="false">
                        <asp:Label ID="export" runat="server" Text="輸出"></asp:Label>
                        <asp:ImageButton ID="Print" runat="server" ImageUrl="~/img/print-icon.png"
                            OnClientClick="printScreen(Print_Table)" OnClick="print_Click" ToolTip="列印" />
                        <asp:ImageButton ID="Excel" runat="server" ImageUrl="img/excel-icon.png" ToolTip="匯出Excel" />
                        <asp:ImageButton ID="PDF" runat="server" ImageUrl="img/PDF-icon.png" ToolTip="匯出PDF" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div align="center" style="background-color: white;width:900px;">
        <asp:Label ID="msg" runat="server" Text="查無資料" Visible="false"></asp:Label>
    </div>
    <asp:Panel ID="Panel_Report" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Width="920px">
        <div id="Print_Table" style="padding-left: 5px;">
            <table width="900px">
                <tr>
                    <td colspan="8">
                        <table width="900px" border="0">
                            <tr>
                                <td colspan="8" align="center">
                                    <asp:Label ID="Label2" runat="server" Text="月報表" Font-Bold="true"
                                        Font-Size="22px"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" width="440px">
                        <table width="440px">
                            <tr>
                                <td colspan="4">
                                    <table width="440px">
                                        <tr>
                                            <td colspan="4" align="left" style="padding-left:50px;">
                                                <asp:Label ID="Date_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left" style="padding-left:50px;">
                                                <asp:Label ID="Num_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left" style="padding-left:50px;">
                                                <asp:Label ID="Position_txt" runat="server" Text="Label" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" width="440px">
                        <table width="440px" border="1" rules="rows">
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="W" runat="server" Text="功率" Font-Bold="true" ForeColor="White"
                                        Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">平均值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgW" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">最大值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxW" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="V" runat="server" Text="電壓" Font-Bold="true" ForeColor="White"
                                        Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">平均值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgV" runat="server" Font-Bold="true" Text="--"></asp:Label>V</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">最大值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxV" runat="server" Font-Bold="true" Text="--"></asp:Label>V</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" bgcolor="#333333">
                                    <asp:Label ID="A" runat="server" Text="電流" Font-Bold="true" ForeColor="White"
                                        Font-Size="14px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" bgcolor="white">
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">平均值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="avgI" runat="server" Font-Bold="true" Text="--"></asp:Label>A</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">最大值</td>
                                            <td colspan="3" align="right" style="padding-right: 20px;">
                                                <asp:Label ID="maxI" runat="server" Font-Bold="true" Text="--"></asp:Label>A</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                            </tr>
                        </table>
                    </td>
                    
                    <td colspan="4">
                        <table width="440px">
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" border="1" rules="rows">
                                        <tr>
                                            <td colspan="4" align="center" bgcolor="#333333">
                                                <asp:Label ID="Label4" runat="server" Text="最大需量" ForeColor="White"
                                                    Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Demand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="HalfDemand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">週六半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="SatHalfDemand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">離峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="OffDemand" runat="server" Font-Bold="true" Text="--"></asp:Label>kw</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="440px" border="1" rules="rows">
                                        <tr>
                                            <td colspan="4" align="center" bgcolor="#333333">
                                                <asp:Label ID="Label1" runat="server" Text="用電量" ForeColor="White"
                                                    Font-Bold="true" Font-Size="14px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Rush" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Half" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">週六半尖峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="SatHalf" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1" align="left" style="padding-left: 20px;">離峰</td>
                                                        <td colspan="3" align="right" style="padding-right: 20px;">
                                                            <asp:Label ID="Off" runat="server" Font-Bold="true" Text="--"></asp:Label>KWH</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="440px" bgcolor="white">
                                                    <tr>
                                                        <td colspan="1" align="left" bgcolor="white" style="padding-left: 20px;">總計</td>
                                                        <td colspan="3" align="right" bgcolor="white" style="padding-right: 20px;">
                                                            <asp:Label ID="Sum" runat="server" Text="--" ForeColor="Red" Font-Bold="true" Font-Size="16px"></asp:Label>KWH</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp; </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table width="900px" border="1" rules="all">
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="Label6" runat="server" Text="功率(kw)"></asp:Label></td>
                                <td colspan="5" align="center">
                                    <asp:Label ID="Label7" runat="server" Text="用電量(KWH)"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="GridView1" runat="server" BackColor="White"
                                        BorderColor="Black" BorderWidth="1px" CellPadding="4"
                                        ForeColor="Black" GridLines="Horizontal"
                                        Width="340px" BorderStyle="None">
                                        <RowStyle HorizontalAlign="Center" Wrap="false" />
                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right"
                                            VerticalAlign="Middle" />
                                        <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" Font-Size="14px"
                                            VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" Width="112.5px" />
                                        <AlternatingRowStyle BackColor="#fafafa" />
                                    </asp:GridView>
                                </td>
                                <td colspan="5">
                                    <asp:GridView ID="GridView2" runat="server" BackColor="White"
                                        BorderColor="Black" BorderWidth="1px" CellPadding="4"
                                        ForeColor="Black" GridLines="Horizontal"
                                        Width="560px" BorderStyle="None">
                                        <RowStyle HorizontalAlign="Center" Wrap="false" />
                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" Font-Size="14px"
                                            VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" Width="112.5px" />
                                        <AlternatingRowStyle BackColor="#fafafa" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </asp:Panel>
    <div id="displayBox" style="display: none;">
        <img src="img/loading-bar.gif" /><br />
        <h1>Please wait...</h1>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
</asp:Content>

