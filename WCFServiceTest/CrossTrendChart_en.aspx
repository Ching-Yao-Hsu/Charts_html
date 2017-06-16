<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CrossTrendChart.aspx.vb" Inherits="CrossTrendChart" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="JQChart.Web" Namespace="JQChart.Web.UI.WebControls" TagPrefix="jqChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                var fileName = 'Multi-Meter Trend FIG match Chart_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
                var dataURL1 = $('#<%= Chart_V.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL2 = $('#<%= Chart_I.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL3 = $('#<%= Chart_W.ClientID %>').jqChart('todataurl', 'image/png');
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
                }
            });
            $('#<%= Chart_I.ClientID %>').bind('tooltipFormat', function (e, data) {
                if (data[0] != null && data[1] != null && data[2] == null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1;
                    return tooltip;
                } else if (data[0] != null && data[1] != null && data[2] != null && data[3] == null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip2 = '<span style="color:' + data[2].series.fillStyle + '">' + data[2].series.title + ': </span>' + '<b>' + data[2].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1 + tooltip2;
                    return tooltip;
                } else if (data[0] != null && data[1] != null && data[2] != null && data[3] != null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip2 = '<span style="color:' + data[2].series.fillStyle + '">' + data[2].series.title + ': </span>' + '<b>' + data[2].y + '</b></br>'
                    var tooltip3 = '<span style="color:' + data[3].series.fillStyle + '">' + data[3].series.title + ': </span>' + '<b>' + data[3].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1 + tooltip2 + tooltip3;
                    return tooltip;
                } else {
                    var date = '<b>' + data.chart.stringFormat(data.x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data.series.fillStyle + '">' + data.series.title + ': </span>' + '<b>' + data.y + '</b></br>'
                    var tooltip = date + tooltip0;
                    return tooltip;
                }
            });

            $('#<%= Chart_V.ClientID %>').bind('tooltipFormat', function (e, data) {
                if (data[0] != null && data[1] != null && data[2] == null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1;
                    return tooltip;
                } else if (data[0] != null && data[1] != null && data[2] != null && data[3] == null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip2 = '<span style="color:' + data[2].series.fillStyle + '">' + data[2].series.title + ': </span>' + '<b>' + data[2].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1 + tooltip2;
                    return tooltip;
                } else if (data[0] != null && data[1] != null && data[2] != null && data[3] != null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip2 = '<span style="color:' + data[2].series.fillStyle + '">' + data[2].series.title + ': </span>' + '<b>' + data[2].y + '</b></br>'
                    var tooltip3 = '<span style="color:' + data[3].series.fillStyle + '">' + data[3].series.title + ': </span>' + '<b>' + data[3].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1 + tooltip2 + tooltip3;
                    return tooltip;
                } else {
                    var date = '<b>' + data.chart.stringFormat(data.x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data.series.fillStyle + '">' + data.series.title + ': </span>' + '<b>' + data.y + '</b></br>'
                    var tooltip = date + tooltip0;
                    return tooltip;
                }
            });

            $('#<%= Chart_W.ClientID %>').bind('tooltipFormat', function (e, data) {
                if (data[0] != null && data[1] != null && data[2] == null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1;
                    return tooltip;
                } else if (data[0] != null && data[1] != null && data[2] != null && data[3] == null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip2 = '<span style="color:' + data[2].series.fillStyle + '">' + data[2].series.title + ': </span>' + '<b>' + data[2].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1 + tooltip2;
                    return tooltip;
                } else if (data[0] != null && data[1] != null && data[2] != null && data[3] != null) {
                    var date = '<b>' + data[0].chart.stringFormat(data[0].x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip2 = '<span style="color:' + data[2].series.fillStyle + '">' + data[2].series.title + ': </span>' + '<b>' + data[2].y + '</b></br>'
                    var tooltip3 = '<span style="color:' + data[3].series.fillStyle + '">' + data[3].series.title + ': </span>' + '<b>' + data[3].y + '</b></br>'
                    var tooltip = date + tooltip0 + tooltip1 + tooltip2 + tooltip3;
                    return tooltip;
                } else {
                    var date = '<b>' + data.chart.stringFormat(data.x, "yyyy/mm/dd HH:MM:ss") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data.series.fillStyle + '">' + data.series.title + ': </span>' + '<b>' + data.y + '</b></br>'
                    var tooltip = date + tooltip0;
                    return tooltip;
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel_Chart" runat="server">
            <div id="chart" align="center">
                <jqchart:chart id="Chart_V" width="880px" height="500px" runat="server" Visible="false" mouseinteractionmode="Zooming" DataSourceID="SqlDataSource1">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Title Text="Multi-Meter Trend Chart"></Title>
                    <Legend><Title Text="Meter Comparison"></Title></Legend>
                    <Animation Enabled="True" Duration="00:00:01" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="14px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="True">
                            <Title Text="Voltage(V)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value3">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value4">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqchart:chart>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>

                <jqchart:chart id="Chart_I" width="880px" height="500px" runat="server" Visible="false" mouseinteractionmode="Zooming" DataSourceID="SqlDataSource2">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Title Text="Multi-Meter Trend Chart"></Title>
                    <Legend><Title Text="Meter Comparison"></Title></Legend>
                    <Animation Enabled="True" Duration="00:00:01" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="14px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="True">
                            <Title Text="Current(A)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value3">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value4">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqchart:chart>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>

                <jqchart:chart id="Chart_W" width="880px" height="500px" runat="server" Visible="false" mouseinteractionmode="Zooming" DataSourceID="SqlDataSource3">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Title Text="Multi-Meter Trend Chart"></Title>
                    <Legend><Title Text="Meter Comparison"></Title></Legend>
                    <Animation Enabled="True" Duration="00:00:01" />
                    <Toolbar Visibility="Auto" PanningTooltipText="Drag chart" ZoomingTooltipText="Watch range chart"
                        ResetZoomTooltipText="Restore the full chart" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True">
                            <Title Text="Time" Font="14px sans-serif"></Title>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="True">
                            <Title Text="Power(KW)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value3" >
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="value4">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqchart:chart>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server"></asp:SqlDataSource>

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
