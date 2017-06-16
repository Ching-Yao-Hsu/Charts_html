<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrendChart.aspx.vb" Inherits="SumTrendChart" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="JQChart.Web" Namespace="JQChart.Web.UI.WebControls" TagPrefix="jqChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="css/menu.css" rel="stylesheet" type="text/css" />
    <link href="css/home01.css" rel="stylesheet" type="text/css" />

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
                var fileName = 'Electricity Trend Chart_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
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
                    <Legend><Title Text="Voltage"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="True">
                            <Title Text="Voltage(V)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V1" Title="R Phase">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V2" Title="S Phase">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V3" Title="T Phase">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Vavg" Title="Average voltage">
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
                    <Legend><Title Text="Current"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="Current(A)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="I1" Title="R Phase">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="I2" Title="S Phase">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="I3" Title="T Phase">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Iavg" Title="Average current">
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
                    <Legend><Title Text="Power"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="Power(KW)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="W" Title="Actual power output">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="V_ar" Title="Virtual power">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="VA" Title="Apparent power">
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
                    <Legend><Title Text="Prediction Mode"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="16px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="Power(KW)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode1" Title="Mode 1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode2" Title="Mode 2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode3" Title="Mode 3">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="Mode4" Title="Mode 4">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMand" Title="Peak Time">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMandHalf" Title="Half Peak Time">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMandSatHalf" Title="Saturday Half Peak Time">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="DeMandOff" Title="Off Peak Time">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="W" Title="Actual power output">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <asp:SqlDataSource ID="SqlDataSource4" runat="server"></asp:SqlDataSource>
                <div style="height: 50px;">
                    <asp:ImageButton ID="img_btn" runat="server" ImageUrl="~/img/btn_export_01en.png"
                        onmouseover="this.src='img/btn_export_02en.png'" onmouseout="this.src='img/btn_export_01en.png'"
                        OnClientClick="return false;" Visible="False" />
                </div>
                <a id="download_img" hidden="hidden"></a>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
