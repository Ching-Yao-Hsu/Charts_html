<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrendChart.aspx.vb" Inherits="SumTrendChart" %>
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
    <link rel="stylesheet" href="css/jquery-ui.css" />
    <script type="text/javascript" src="js/jquery-ui.js"></script>
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
                var fileName = '電力趨勢圖_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
                var dataURL1 = $('#<%= Chart_V.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL2 = $('#<%= Chart_I.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL3 = $('#<%= Chart_W.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL4 = $('#<%= Chart_Mode.ClientID %>').jqChart('todataurl', 'image/png');
                if (dataURL1 != null) {
                    var downloadLink = document.getElementById("download_img");
                    downloadLink.href = dataURL1;
                    downloadLink.download = fileName;
                    downloadLink.click();
                } else if (dataURL2 != null) {
                    var downloadLink = document.getElementById("download_img");
                    downloadLink.href = dataURL2;
                    downloadLink.download = fileName;
                    downloadLink.click();
                } else if (dataURL3 != null) {
                    var downloadLink = document.getElementById("download_img");
                    downloadLink.href = dataURL3;
                    downloadLink.download = fileName;
                    downloadLink.click();
                } else if (dataURL4 != null) {
                    var downloadLink = document.getElementById("download_img");
                    downloadLink.href = dataURL4;
                    downloadLink.download = fileName;
                    downloadLink.click();
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel_Chart" runat="server">
            <div align="center">
                <jqChart:Chart ID="Chart_V" Width="850px" Height="460px" runat="server" DataSourceID="SqlDataSource1" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Legend><Title Text="電壓"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="時間" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="True">
                            <Title Text="電壓(V)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V1" Title="R相">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V2" Title="S相">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V3" Title="T相">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Vavg" Title="平均電壓">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>

                <jqChart:Chart ID="Chart_I" Width="850px" Height="460px" runat="server" DataSourceID="SqlDataSource2" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Legend><Title Text="電流"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="時間" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="電流(A)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="I1" Title="R相">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="I2" Title="S相">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="I3" Title="T相">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Iavg" Title="平均電流">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>

                <jqChart:Chart ID="Chart_W" Width="850px" Height="460px" runat="server" DataSourceID="SqlDataSource3" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Legend><Title Text="功率"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="時間" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="功率(KW)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="W" Title="實功">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V_ar" Title="虛功">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="VA" Title="視在">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server"></asp:SqlDataSource>

                <jqChart:Chart ID="Chart_Mode" Width="850px" Height="460px" runat="server" DataSourceID="SqlDataSource4" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Legend><Title Text="預測模式"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="時間" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="功率(KW)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode1" Title="模式1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode2" Title="模式2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode3" Title="模式3">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode4" Title="模式4">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMand" Title="尖峰">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMandHalf" Title="半尖峰">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMandSatHalf" Title="週六半尖峰">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMandOff" Title="離峰">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="W" Title="實功">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <asp:SqlDataSource ID="SqlDataSource4" runat="server"></asp:SqlDataSource>
                <div style="height: 50px;">
                    <asp:ImageButton ID="img_btn" runat="server" ImageUrl="~/img/btn_export_01.png"
                        onmouseover="this.src='img/btn_export_02.png'" onmouseout="this.src='img/btn_export_01.png'"
                        OnClientClick="return false;" Visible="False" />
                </div>
                <a id="download_img" hidden="hidden"></a>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
