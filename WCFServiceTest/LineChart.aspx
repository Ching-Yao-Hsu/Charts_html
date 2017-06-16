<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LineChart.aspx.vb" Inherits="LineChart" %>
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
                var config = {
                    fileName: 'LineChart_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png',
                    type: 'image/png' // 'image/png' or 'image/jpeg'
                };
                var dataURL1 = $('#<%= Chart_V.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL2 = $('#<%= Chart_I.ClientID %>').jqChart('todataurl', 'image/png');
                var dataURL3 = $('#<%= Chart_W.ClientID %>').jqChart('todataurl', 'image/png');
                if (dataURL1 != null) {
                    $('#<%= Chart_V.ClientID%>').jqChart('exportToImage', config);
                } else if (dataURL2 != null) {
                    $('#<%= Chart_I.ClientID%>').jqChart('exportToImage', config);
                } else if (dataURL3 != null) {
                    $('#<%= Chart_W.ClientID%>').jqChart('exportToImage', config);
                }
            });
            $('#<%= Chart_W.ClientID %>').bind('tooltipFormat', function (e, data) {
                if (data[0] != null && data[1] != null && data[2] == null) {
                    var date0 = '<b>' + data[0].chart.stringFormat(data[0].x, "HH:MM") + '</b></br>';
                    var date1 = '<b>' + data[1].chart.stringFormat(data[1].x, "HH:MM") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip = date0 + tooltip0 + date1 + tooltip1;
                    return tooltip;
                } else {
                    var date = '<b>' + data.chart.stringFormat(data.x, "HH:MM") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data.series.fillStyle + '">' + data.series.title + ': </span>' + '<b>' + data.y + '</b></br>'
                    var tooltip = date + tooltip0;
                    return tooltip;
                }
            });
            $('#<%= Chart_V.ClientID%>').bind('tooltipFormat', function (e, data) {
                if (data[0] != null && data[1] != null && data[2] == null) {
                    var date0 = '<b>' + data[0].chart.stringFormat(data[0].x, "HH:MM") + '</b></br>';
                    var date1 = '<b>' + data[1].chart.stringFormat(data[1].x, "HH:MM") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip = date0 + tooltip0 + date1 + tooltip1;
                    return tooltip;
                } else {
                    var date = '<b>' + data.chart.stringFormat(data.x, "HH:MM") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data.series.fillStyle + '">' + data.series.title + ': </span>' + '<b>' + data.y + '</b></br>'
                    var tooltip = date + tooltip0;
                    return tooltip;
                }
            });
            $('#<%= Chart_I.ClientID %>').bind('tooltipFormat', function (e, data) {
                if (data[0] != null && data[1] != null && data[2] == null) {
                    var date0 = '<b>' + data[0].chart.stringFormat(data[0].x, "HH:MM") + '</b></br>';
                    var date1 = '<b>' + data[1].chart.stringFormat(data[1].x, "HH:MM") + '</b></br>';
                    var tooltip0 = '<span style="color:' + data[0].series.fillStyle + '">' + data[0].series.title + ': </span>' + '<b>' + data[0].y + '</b></br>'
                    var tooltip1 = '<span style="color:' + data[1].series.fillStyle + '">' + data[1].series.title + ': </span>' + '<b>' + data[1].y + '</b></br>'
                    var tooltip = date0 + tooltip0 + date1 + tooltip1;
                    return tooltip;
                } else {
                    var date = '<b>' + data.chart.stringFormat(data.x, "HH:MM") + '</b></br>';
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
                <jqChart:Chart ID="Chart_W" Width="880px" Height="480px" runat="server" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Title Text="時段效益分析圖"></Title>
                    <Legend><Title Text="比對時段"></Title></Legend>
                    <Animation Enabled="True" Duration="00:00:01" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Top" ZoomEnabled="True" Name="X1">
                            <Title Text="時段一" Font="14px sans-serif"></Title>
                            <Labels StringFormat="HH:mm">
                            </Labels>
                        </jqChart:DateTimeAxis>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True" Name="X2">
                            <Title Text="時段二" Font="14px sans-serif"></Title>
                            <Labels StringFormat="HH:mm">
                            </Labels>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="電量(KW)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries AxisX="X1" XValuesField="RecDate1" YValuesField="Value11" Title="時段1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries AxisX="X2" XValuesField="RecDate2" YValuesField="Value12" Title="時段2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <jqChart:Chart ID="Chart_V" Width="880px" Height="480px" runat="server" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Title Text="時段效益分析圖"></Title>
                    <Legend><Title Text="比對時段"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Top" ZoomEnabled="True" Name="X1" Interval="1" IntervalType="Hours">
                            <Title Text="時段一" Font="14px sans-serif"></Title>
                            <Labels StringFormat="HH:mm">
                            </Labels>
                        </jqChart:DateTimeAxis>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True" Name="X2" Interval="1" IntervalType="Hours">
                            <Title Text="時段二" Font="14px sans-serif"></Title>
                            <Labels StringFormat="HH:mm">
                            </Labels>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="電壓(V)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries AxisX="X1" XValuesField="RecDate1" YValuesField="Value11" Title="時段1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries AxisX="X2" XValuesField="RecDate2" YValuesField="Value12" Title="時段2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <jqChart:Chart ID="Chart_I" Width="880px" Height="480px" runat="server" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Title Text="時段效益分析圖"></Title>
                    <Legend><Title Text="比對時段"></Title></Legend>
                    <Animation Enabled="True" />
                    <Toolbar Visibility="Auto" PanningTooltipText="拖曳圖表" ZoomingTooltipText="觀看範圍圖表"
                        ResetZoomTooltipText="恢復完整圖表" />
                    <Tooltips TooltipsType="Shared" />
                    <Crosshairs Enabled="True">
                        <HorizontalLine Visible="False" />
                        <VerticalLine StrokeStyle="#cc0a0c" />
                    </Crosshairs>
                    <Axes>
                        <jqChart:DateTimeAxis Location="Top" ZoomEnabled="True" Name="X1">
                            <Title Text="時段一" Font="14px sans-serif"></Title>
                            <Labels StringFormat="HH:mm">
                            </Labels>
                        </jqChart:DateTimeAxis>
                        <jqChart:DateTimeAxis Location="Bottom" ZoomEnabled="True" Name="X2">
                            <Title Text="時段二" Font="14px sans-serif"></Title>
                            <Labels StringFormat="HH:mm">
                            </Labels>
                        </jqChart:DateTimeAxis>
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="true">
                            <Title Text="電流(A)" Font="16px sans-serif"></Title>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries AxisX="X1" XValuesField="RecDate1" YValuesField="Value11" Title="時段1">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                        <jqChart:LineSeries AxisX="X2" XValuesField="RecDate2" YValuesField="Value12" Title="時段2">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>
                <div style="height: 50px;">
                    <asp:ImageButton ID="img_btn" runat="server" ImageUrl="~/img/btn_export_01.png"
                        onmouseover="this.src='img/btn_export_02.png'" onmouseout="this.src='img/btn_export_01.png'"
                        OnClientClick="return false;" Visible="False" />
                </div>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
