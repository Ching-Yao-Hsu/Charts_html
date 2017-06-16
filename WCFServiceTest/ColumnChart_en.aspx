<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ColumnChart.aspx.vb" Inherits="ColumnChart" %>

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
                //var config = {
                //    fileName: '效益分析圖_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png',
                //    type: 'image/png' // 'image/png' or 'image/jpeg'
                //};
                var fileName = 'Benefit analysis chart_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
                var dataURL1 = $('#<%= PowerColumnChart_V.ClientID%>').jqChart('todataurl', 'image/png');
                var dataURL2 = $('#<%= PowerColumnChart_I.ClientID%>').jqChart('todataurl', 'image/png');
                var dataURL3 = $('#<%= PowerColumnChart_W.ClientID%>').jqChart('todataurl', 'image/png');
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
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel_Chart" runat="server">
            <div id="chart" align="center">
                <table>
                    <tr>
                        <td>
                            <jqChart:Chart ID="PowerColumnChart_V" Width="700px" Height="400px" runat="server" Visible="false">
                                <Background FillStyleType="LinearGradient" X1="0">
                                    <ColorStops>
                                        <jqChart:ColorStop Color="#d2e6c9" />
                                        <jqChart:ColorStop Color="white" Offset="1" />
                                    </ColorStops>
                                </Background>
                                <Title Text="Benefit analysis chart"></Title>
                                <Animation Enabled="True" Duration="00:00:01" />
                                <Axes>
                                    <jqChart:LinearAxis Location="Left" Name="Y1">
                                        <Title Text="Voltage(V)" Font="14px sans-serif"></Title>
                                    </jqChart:LinearAxis>
                                </Axes>
                                <Series>
                                    <jqChart:ColumnSeries XValuesField="location" YValuesField="Average" Title="Average">
                                    </jqChart:ColumnSeries>
                                    <jqChart:ColumnSeries XValuesField="location" YValuesField="Maximum" Title="Maximum">
                                    </jqChart:ColumnSeries>
                                </Series>
                            </jqChart:Chart>
                            <jqChart:Chart ID="PowerColumnChart_I" Width="700px" Height="400px" runat="server" Visible="false">
                                <Background FillStyleType="LinearGradient" X1="0">
                                    <ColorStops>
                                        <jqChart:ColorStop Color="#d2e6c9" />
                                        <jqChart:ColorStop Color="white" Offset="1" />
                                    </ColorStops>
                                </Background>
                                <Title Text="Benefit analysis chart"></Title>
                                <Animation Enabled="True" Duration="00:00:01" />
                                <Axes>
                                    <jqChart:LinearAxis Location="Left" Name="Y1">
                                        <Title Text="Current(A)" Font="14px sans-serif"></Title>
                                    </jqChart:LinearAxis>
                                </Axes>
                                <Series>
                                    <jqChart:ColumnSeries XValuesField="location" YValuesField="Average" Title="Average">
                                    </jqChart:ColumnSeries>
                                    <jqChart:ColumnSeries XValuesField="location" YValuesField="Maximum" Title="Maximum">
                                    </jqChart:ColumnSeries>
                                </Series>
                            </jqChart:Chart>
                            <jqChart:Chart ID="PowerColumnChart_W" Width="700px" Height="400px" runat="server" Visible="false">
                                <Background FillStyleType="LinearGradient" X1="0">
                                    <ColorStops>
                                        <jqChart:ColorStop Color="#d2e6c9" />
                                        <jqChart:ColorStop Color="white" Offset="1" />
                                    </ColorStops>
                                </Background>
                                <Title Text="Benefit analysis chart"></Title>
                                <Animation Enabled="True" Duration="00:00:01" />
                                <Axes>
                                    <jqChart:LinearAxis Location="Left" Name="Y1">
                                        <Title Text="Power(KW)" Font="14px sans-serif"></Title>
                                    </jqChart:LinearAxis>
                                </Axes>
                                <Series>
                                    <jqChart:ColumnSeries XValuesField="location" YValuesField="Average" Title="Average">
                                    </jqChart:ColumnSeries>
                                    <jqChart:ColumnSeries XValuesField="location" YValuesField="Maximum" Title="Maximum">
                                    </jqChart:ColumnSeries>
                                </Series>
                            </jqChart:Chart>
                        </td>
                    </tr>
                </table>
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
