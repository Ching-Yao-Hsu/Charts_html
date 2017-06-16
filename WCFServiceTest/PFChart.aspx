<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PFChart.aspx.vb" Inherits="PFChart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
                var fileName = '功因趨勢圖_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
                var dataURL1 = $('#<%= Chart_PF.ClientID%>').jqChart('todataurl', 'image/png');
                if (dataURL1 != null) {
                    var downloadLink = document.getElementById("download_img");
                    downloadLink.href = dataURL1;
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
                <%--<jqChart:Chart ID="Chart_PF" Width="850px" Height="460px" runat="server" DataSourceID="SqlDataSource1" Visible="false" MouseInteractionMode="Zooming">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border CornerRadius="20" LineWidth="1" StrokeStyle="Green" />
                    <Legend><Title Text="查詢項目"></Title></Legend>
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
                        <jqChart:LinearAxis Location="Left" ZoomEnabled="True" VisibleMaximum="2" VisibleMinimum="0">
                            <CustomTickMarks>
                                <jqChart:DoubleValue Value="0.5" />
                                <jqChart:DoubleValue Value="1" />
                                <jqChart:DoubleValue Value="1.5" />
                                <jqChart:DoubleValue Value="2" />
                            </CustomTickMarks>
                        </jqChart:LinearAxis>
                    </Axes>
                    <Series>
                        <jqChart:LineSeries XValuesField="RecDate" YValuesField="PF" Title="功因">
                            <Markers Visible="False" />
                        </jqChart:LineSeries>
                    </Series>
                </jqChart:Chart>--%>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>

                <asp:Chart ID="Chart_PF" runat="server" Width="850px" Height="460px" DataSourceID="SqlDataSource1">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Line" XValueMember="RecDate" YValueMembers="PF"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

                <div style="height: 50px;">
                    <asp:ImageButton ID="img_btn" runat="server" ImageUrl="~/img/btn_export_01.png"
                        onmouseover="this.src='img/btn_export_02.png'" onmouseout="this.src='img/btn_export_01.png'"
                         Visible="False" />
                </div>
                <a id="download_img" hidden="hidden"></a>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
