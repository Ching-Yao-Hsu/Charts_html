<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProportionChartS.aspx.vb" Inherits="ProportionChartS" %>
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
                var fileName = '電量比重圖_' + today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate() + '.png';
                var dataURL = $('#<%= PieChart.ClientID%>').jqChart('todataurl', 'image/png');
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
                <jqChart:Chart ID="PieChart" Width="600px" Height="350px" runat="server" Visible="false">
                    <Background FillStyleType="LinearGradient" X1="0">
                        <ColorStops>
                            <jqChart:ColorStop Color="#d2e6c9" />
                            <jqChart:ColorStop Color="white" Offset="1" />
                        </ColorStops>
                    </Background>
                    <Border StrokeStyle="#6ba851" />
                    <Legend><Title Text="比對項目"></Title></Legend>
                    <Animation Enabled="True" Duration="00:00:01" />
                    <Series>
                        <jqChart:PieSeries DataLabelsField="Items" DataValuesField="Values"
                            ExplodedRadius="10"
                            LabelsPosition="Outside"
                            LabelsAlign="Circle"
                            LabelsExtend="20"
                            LeaderLineWidth="1"
                            LeaderLineStrokeStyle="Black">
                            <ExplodedSlices>
                                <jqChart:DoubleValue Value="5" />
                            </ExplodedSlices>
                            <Labels Visible="true" Font="15px sans-serif" StringFormat="%.1f%%" ValueType="Percentage">
                                <FillStyle Color="Black"></FillStyle>
                            </Labels>
                        </jqChart:PieSeries>
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
        </asp:Panel>
    </form>
</body>
</html>
