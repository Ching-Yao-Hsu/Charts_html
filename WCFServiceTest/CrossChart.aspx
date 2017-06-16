<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CrossChart.aspx.vb" Inherits="SumCrossChart" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="JQChart.Web" Namespace="JQChart.Web.UI.WebControls" TagPrefix="jqChart" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="css/menu.css" rel="stylesheet" type="text/css" />
    <link href="css/home.css" rel="stylesheet" type="text/css" />

    <link href="css/jquery.jqChart.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.jqRangeSlider.css" rel="stylesheet" type="text/css" />
    <link href="css/themes/smoothness/jquery-ui-1.8.21.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.jqRangeSlider.min.js" type="text/javascript"></script>
    <script src="js/jquery.jqChart.min.js" type="text/javascript"></script>
    <script src="js/jquery.mousewheel.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/alertify.min.js"></script>
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />
    <title></title>
    <script type="text/javascript">
        function custom_script(sender) {
            alertify.alert(sender, function (e) {
                if (e) {
                    parent.$.fn.colorbox.close();
                } else {
                    // user clicked "cancel"
                }
            });
        }

        $(document).ready(function () {
            //轉圖檔
            $("#<%=img_btn.ClientID%>").click(function () {
                var today = new Date();
                var fileName = '電能比對圖_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
                var dataURL = $('#<%= PowerCrossChart.ClientID%>').jqChart('todataurl', 'image/png');
                var downloadLink = document.getElementById("download_img");
                downloadLink.href = dataURL;
                downloadLink.download = fileName;
                downloadLink.click();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel_Chart" runat="server">
            <div id="chart" align="center">
                <jqChart:Chart ID="PowerCrossChart" Width="850px" Height="450px" runat="server" Visible="false">
                    <%--<Title Text="電能比對圖"></Title>--%>
                    <Border StrokeStyle="#6ba851" />
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Legend><Title Text="比對項目"></Title></Legend>
                    <Animation Enabled="True" Duration="00:00:01" />
                    <%--<Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                            ResetZoomTooltipText="恢復完整圖表" />--%>
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:LinearAxis Location="Left" Name="Y1">
                            <Title Text="用電量(KWH)" Font="14px sans-serif"></Title>
                        </jqChart:LinearAxis>
                        <jqChart:LinearAxis Location="Right" Name="Y2" StrokeStyle="#999999"
                            MajorGridLines-StrokeStyle="#999999"
                            MajorTickMarks-StrokeStyle="#999999">
                            <Title Text="功率(KW)" Font="14px sans-serif"></Title>
                            <MajorGridLines Visible="False"></MajorGridLines>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:ColumnSeries XValuesField="時間" YValuesField="尖峰" Title="尖峰" AxisY="Y1">
                        </jqChart:ColumnSeries>
                        <jqChart:ColumnSeries XValuesField="時間" YValuesField="半尖峰" Title="半尖峰" AxisY="Y1">
                        </jqChart:ColumnSeries>
                        <jqChart:ColumnSeries XValuesField="時間" YValuesField="週六半尖峰" Title="週六半尖峰" AxisY="Y1">
                        </jqChart:ColumnSeries>
                        <jqChart:ColumnSeries XValuesField="時間" YValuesField="離峰" Title="離峰" AxisY="Y1">
                        </jqChart:ColumnSeries>
                        <jqChart:LineSeries XValuesField="時間" YValuesField="功率最大值" Title="功率最大值" AxisY="Y2" StrokeStyle="#CC6600">
                            <FillStyle Color="#CC6600"></FillStyle>
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="時間" YValuesField="功率平均值" Title="功率平均值" AxisY="Y2" StrokeStyle="#00CC00">
                            <FillStyle Color="#00CC00"></FillStyle>
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <div style="height: 50px;">
                    <asp:ImageButton ID="img_btn" runat="server" ImageUrl="~/img/btn_export_01.png"
                        onmouseover="this.src='img/btn_export_02.png'" onmouseout="this.src='img/btn_export_01.png'"
                        OnClientClick="return false;" Visible="False" />
                </div>
            </div>
        <a id="download_img" hidden="hidden"></a>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
        </asp:Panel>
    </form>
</body>
</html>
