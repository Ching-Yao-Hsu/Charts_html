<%@ Page Title="" Language="VB" MasterPageFile="~/indexW.master" AutoEventWireup="false" CodeFile="indexW.aspx.vb" Inherits="_Default" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <link href="css/menu_slide.css" rel="stylesheet" type="text/css" />
    <link href="css/layout.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.jqChart.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.jqRangeSlider.css" rel="stylesheet" type="text/css" />
    <link href="css/themes/smoothness/jquery-ui-1.8.21.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.jqRangeSlider.min.js" type="text/javascript"></script>
    <script src="js/jquery.jqChart.min.js" type="text/javascript"></script>
    <script src="js/jquery.mousewheel.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/slide.js"></script>
    <script type="text/javascript" src="js/jquery.backgroundPosition.js"></script>
    <script type="text/javascript" src="js/alertify.min.js"></script>
    <script src="js/jquery.colorbox.js" type="text/javascript"></script>
    <script src="js/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />
    <link href="css/datepicker.css" rel="stylesheet" type="text/css" />
    <script src="js/datepicker.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/jquery-ui.css" />
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var HrefAddress = "support/indexW.htm";
            $('.support').colorbox({ innerWidth: 740, innerHeight: 775, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            //新增店鋪
            $('.imageButtonFinderClassA').click(function () {
                var $tr = $(this).parent().parent();
                var account = $tr.find("td").eq(0).text();
                var HrefAddress = "WaterSetup.aspx?Ftype=I";
                $('.imageButtonFinderClassA').colorbox({ innerWidth: 430, innerHeight: 400, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress, onClosed: reloadPage });
            });

            //總日報表
            $('.imageButtonFinderClassSumD').click(function () {
                var $tr = $(this).parent().parent();
                var sRecdate = ($tr.find("td").eq(6).text()).substring(0, 10);
                var dt = new Date();
                var sMonth = String(dt.getMonth() + 1);
                var sRecdate
                if (sMonth.length < 2) {
                    sRecdate = dt.getFullYear() + '/' + ('0' + sMonth);
                } else {
                    sRecdate = dt.getFullYear() + '/' + sMonth;
                }
                var sDate = String(dt.getDate());
                if (sDate.length < 2) {
                    sRecdate = sRecdate + '/' + ('0' + sDate);
                } else {
                    sRecdate = sRecdate + '/' + sDate;
                }

                var HrefAddress = "WaterDayReport.aspx?sRecdate=" + sRecdate;
                $('.imageButtonFinderClassSumD').colorbox({ innerWidth: 950, innerHeight: 520, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
            });

            //總月報表
            $('.imageButtonFinderClassSumM').click(function () {
                var $tr = $(this).parent().parent();
                var account = ($tr.find("td").eq(6).text()).substring(0, 7);
                var dt = new Date();
                var sMonth = String(dt.getMonth() + 1);
                var sRecdate
                if (sMonth.length < 2) {
                    sRecdate = dt.getFullYear() + '/' + ('0' + sMonth);
                } else {
                    sRecdate = dt.getFullYear() + '/' + sMonth;
                }
                var HrefAddress = "WaterMonthReport.aspx?sRecdate=" + sRecdate;
                //alert(HrefAddress);
                $('.imageButtonFinderClassSumM').colorbox({ innerWidth: 950, innerHeight: 520, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
            });

            //編輯
            $('.imageButtonFinderClass').click(function () {
                var $tr = $(this).parent().parent();
                var sID = $tr.find("td").eq(0).text();
                var HrefAddress = "WaterSetup.aspx?Ftype=U&sID=" + sID;
                //alert(HrefAddress );
                $('.imageButtonFinderClass').colorbox({ innerWidth: 430, innerHeight: 400, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress, onClosed: reloadPage });
            });


            //月報表
            $('.imageButtonFinderClassM').click(function () {
                var $tr = $(this).parent().parent();
                var sShopNo = $tr.find("td").eq(4).text();
                var sDrawNo = $tr.find("td").eq(3).text();
                var sShopName = $tr.find("td").eq(5).text();
                var sMonthS = ($tr.find("td").eq(6).text()).substring(0,7);
                var HrefAddress = "WaterSMonthReport.aspx?sShopNo=" + sShopNo + "&sDrawNo=" + sDrawNo + "&sShopName=" + sShopName + "&sMonthS=" + sMonthS;
                $('.imageButtonFinderClassM').colorbox({ innerWidth: 950, innerHeight: 520, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
            });

            //紀錄查詢
            $('.imageButtonFinderClassD').click(function () {
                var $tr = $(this).parent().parent();
                var sShopNo = $tr.find("td").eq(4).text();
                var sDrawNo = $tr.find("td").eq(3).text();
                var sShopName = $tr.find("td").eq(5).text();
                var sDateS = ($tr.find("td").eq(6).text()).substring(0, 10);
                var sDateE = ($tr.find("td").eq(6).text()).substring(0, 10);
                var sHHS = '00';
                var sHHE = ($tr.find("td").eq(6).text()).substring(11, 13);
                var HrefAddress = "WaterSDayReport.aspx?sShopNo=" + sShopNo + "&sDrawNo=" + sDrawNo + "&sShopName=" + sShopName + "&sDateS=" + sDateS + "&sDateE=" + sDateE + "&sHHS=" + sHHS + "&sHHE=" + sHHE;
                //alert(HrefAddress);
                $('.imageButtonFinderClassD').colorbox({ innerWidth: 950, innerHeight: 520, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
            });
        });
        function reloadPage() {
            //colorbox關閉，刷新母網頁
            parent.location.reload();
        }

    </script>
    <style type="text/css">
        .div {
            border-radius: 15px 15px 15px 15px;
            background-color: #f7f7f7;
            width: 795px;
        }

        .gridviewstyle1
        {
             border-radius: 10px 0px 0px 0px;
        }
        .gridviewstyle2
        {
             border-radius: 0px 10px 0px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="水表設定" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td align="left"  nowrap="nowrap" style="width:300px;">關鍵字
                    <asp:TextBox ID="txtKeyWork" runat="server"></asp:TextBox>
                    <asp:Button ID="submit_btn" runat="server" Text="查詢" />
                </td>
                <td align="left"  nowrap="nowrap" style="width:300px;">
                    <asp:ImageButton ID="btnAddWaterSetup" runat="server" ImageUrl="~/img/btn_store01.png"
                        onmouseover="this.src='img/btn_store02.png'" onmouseout="this.src='img/btn_store01.png'"
                        CssClass="imageButtonFinderClassA" />
                    <asp:ImageButton ID="btnDayReport" runat="server" ImageUrl="~/img/btn_day01.png"
                        onmouseover="this.src='img/btn_day02.png'" onmouseout="this.src='img/btn_day01.png'"
                        CssClass="imageButtonFinderClassSumD" />
                    <asp:ImageButton ID="btnMonthReport" runat="server" ImageUrl="~/img/btn_month01.png"
                        onmouseover="this.src='img/btn_month02.png'" onmouseout="this.src='img/btn_month01.png'"
                        CssClass="imageButtonFinderClassSumM" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="White" Width="700px"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1"
                        ForeColor="Black" GridLines="None" AutoGenerateColumns="False" CssClass="normal tablesorter"
                        DataKeyNames="ID">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="序號" ReadOnly="True" HeaderStyle-CssClass="gridviewstyle1" />
                            <asp:TemplateField HeaderText="編輯" HeaderStyle-CssClass="gridviewstyle2">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Edit_btn" runat="server" CausesValidation="false" ImageUrl="img/manage.png"
                                        Width="22px" CssClass="imageButtonFinderClass" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MeterID"  HeaderText="水表編號" />
                            <asp:BoundField DataField="圖面編號" HeaderText="圖面編號" />
                            <asp:BoundField DataField="店鋪編號" HeaderText="店鋪編號" />
                            <asp:BoundField DataField="店鋪名稱" HeaderText="店鋪名稱" />
                            <asp:BoundField DataField="RecDate"  HeaderText="接收時間" />
                            
                            <asp:TemplateField HeaderText="水表值">
                                <ItemTemplate>
                                    <asp:Label ID="LebWater" runat="server" Text='<%# Bind("水表值")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="報表" HeaderStyle-CssClass="gridviewstyle2">
                                <ItemTemplate>
                                    <asp:ImageButton ID="MonthReport_btn" runat="server" CausesValidation="false" ImageUrl="img/btn_Monthly-Report-icon.png"
                                        Width="22px" CssClass="imageButtonFinderClassM" />
                                    <asp:ImageButton ID="Sreach_btn" runat="server" CausesValidation="false" ImageUrl="img/btn_records.png"
                                        Width="22px" CssClass="imageButtonFinderClassD" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle HorizontalAlign="Center" Wrap="false" />
                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" VerticalAlign="Middle" />
                        <SelectedRowStyle BackColor="#D2E9FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle BackColor="#2851A4" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
                            HorizontalAlign="Center" Wrap="false" />
                        <AlternatingRowStyle BackColor="#e8f0ff" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    </div>
</asp:Content>

