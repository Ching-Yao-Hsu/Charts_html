<%@ Page Title="" Language="VB" MasterPageFile="~/Power.master" AutoEventWireup="false" CodeFile="PowerChartNow.aspx.vb" Inherits="PowerChart" Buffer="True" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="JQChart.Web" Namespace="JQChart.Web.UI.WebControls" TagPrefix="jqChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">
        .ui-datepicker-trigger{cursor:pointer; width:20px;height:20px;}
        .menu a{ text-decoration:none;color:#333;}
        .menu a:hover{ text-decoration:underline;}
    </style>
    <script type="text/javascript">
        function ShowLoadingMessage() {
            document.getElementById("LoadingMessage").style.display = "block";
        }
        function custom_script(sender) {
            sure = alertify.alert(sender);
            window.location = "home.aspx"
        }
        $(document).ready(function() {
            $("#<%=submit_btn.ClientID %>").click(function() {
                //判斷日期
                var begin_time = $("#<%= Date_txt1.ClientID %>").val() + " " + $('select[id$=begin_hh] :selected').val() + ":00:00";
                var end_time = $("#<%= Date_txt2.ClientID %>").val() + " " + $('select[id$=end_hh] :selected').val() + ":59:59";
                
                if ($("#<%= Date_txt1.ClientID%>").val() != "" && $("#<%= Date_txt2.ClientID%>").val() != "") {
                    if (begin_time <= end_time) {
                        Date.prototype.dateDiff = function (interval, objDate) {
                            var dtEnd = new Date(objDate);
                            if (isNaN(dtEnd)) return undefined;
                            switch (interval) {
                                case "s": return parseInt((dtEnd - this) / 1000);  //秒
                                case "n": return parseInt((dtEnd - this) / 60000);  //分
                                case "h": return parseInt((dtEnd - this) / 3600000);  //時
                                case "d": return parseInt((dtEnd - this) / 86400000);  //天
                                case "w": return parseInt((dtEnd - this) / (86400000 * 7));  //週
                                case "m": return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - this.getFullYear()) * 12) - (this.getMonth() + 1);  //月份
                                case "y": return dtEnd.getFullYear() - this.getFullYear();  //天
                            }
                        }
                        var sDT = new Date(begin_time);  //必要項。sDT- 這是在計算中想要使用的第一個日期/時間值。
                        var eDT = new Date(end_time);  //必要項。eDT- 這是在計算中想要使用的第二個日期/時間值。
                        var sum = sDT.dateDiff("d", eDT);
                        //判斷30天以內
                        if (sum <= 31) {
                            var value = '';
                            var s = document.getElementById("<%=Item_DDList.ClientID%>");
                            var item = s.options[s.selectedIndex].value;
                            $("#<%=Item_CBList.ClientID%> input[type=checkbox]:checked").each(function () {
                                var currentValue = $(this).val();
                                if (currentValue != '')
                                    value += currentValue + ",";
                            });
                            if (item == "需量") {
                                $("#<%=Demand_CBList.ClientID%> input[type=checkbox]:checked").each(function () {
                                    var currentValue2 = $(this).next('label').text();
                                    if (currentValue2 == "尖峰") {
                                        value += "DeMand,";
                                    } else if (currentValue2 == "半尖峰") {
                                        value += "DeMandHalf,";
                                    } else if (currentValue2 == "週六半尖峰") {
                                        value += "DeMandSatHalf,";
                                    } else if (currentValue2 == "離峰") {
                                        value += "DeMandOff,";
                                    }
                                });
                                $("#<%=W_CBList.ClientID%> input[type=checkbox]:checked").each(function () {
                                    var currentValue3 = $(this).next('label').text();
                                    if (currentValue3 != '')
                                        value += "W,";
                                });
                            }
                            if (value.length > 0) {
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
                            }
                            else {
                                alertify.alert("您尚未勾選查詢項目");
                                return false;
                            }
                        } else {
                            alertify.alert("請搜尋一個月內資料");
                            return false;
                        }
                    } else {
                        alertify.alert("開始日期不可大於结束日期");
                        return false;
                    }
                } else {
                    alertify.alert("請選擇日期");
                    return false;
                }     
            });

            //月曆
            $("#<%= Date_txt1.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });
            $("#<%= Date_txt2.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd', showOn: 'button',
                buttonImageOnly: true, buttonImage: 'img/calendar_Black2.png', changeYear: true, changeMonth: true
            });

            //轉圖檔
            $("#<%=ConvertPng_btn.ClientID %>").click(function () {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="LoadingMessage" align="center" style=" font-size:14px; width:1000px; height:690px; background:url('img/blank1.png'); position:absolute; display:none; z-index:9999">網頁載入中，請稍候...</div>
    <div style=" padding-left:20px;">
      
        <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" 
            TargetControlID="panContent"
            CollapseControlID="panTitle"
            ExpandControlID="panTitle"
            TextLabelID="Label1"
            ExpandedText="（隱藏內容...）"
            CollapsedText="（顯示內容...）"
            ImageControlID="Image1"
            ExpandedImage="~/img/top_arrow.png"
            CollapsedImage="~/img/down_arrow.png"
            ExpandDirection="Vertical"
            SuppressPostBack="False" Collapsed="True" CollapsedSize="0" >
        </ajaxToolkit:CollapsiblePanelExtender>
        
        <asp:Panel ID="Panel_Chart" runat="server" Width="880px">
            <asp:Panel ID="panTitle" runat="server" Width="300px"  Height="30px">
                <div style="padding: 5px; cursor: hand;">
                    <div style="float: left;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/down_arrow.png" Width="18px" />
                    </div>
                    <div style="float: left; margin-left: 10px; font-weight: bolder">
                        查詢
                    </div>
                    <div style="float: right; color: brown">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="panContent" runat="server" style="padding-left:0px;" Width="850px">
                <div style="border-radius: 15px 15px 15px 15px;background-color:#e8f0ff;width:850px;">
                    <table>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <img src="img/date2.png" />時間區間
			                                <asp:TextBox ID="Date_txt1" runat="server" Width="70px"></asp:TextBox>
                                            <asp:DropDownList ID="begin_hh" runat="server"></asp:DropDownList>時～
                                    
                                            <asp:TextBox ID="Date_txt2" runat="server" Width="70px"></asp:TextBox>
                                            <asp:DropDownList ID="end_hh" runat="server"></asp:DropDownList>時
                                        </td>
                                        <td style="padding-left: 20px;" valign="bottom">間隔
			                                <asp:DropDownList ID="interval_DDList" runat="server">
                                                <asp:ListItem Value="1" Selected="True">1分鐘</asp:ListItem>
                                                <asp:ListItem Value="5">5分鐘</asp:ListItem>
                                                <asp:ListItem Value="30">30分鐘</asp:ListItem>
                                                <asp:ListItem Value="60">1小時</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td rowspan="2" valign="middle" style="padding-left: 30px;">
                                <table height="100%">
                                    <tr>
                                        <td align="right" valign="middle">
                                            <asp:Button ID="submit_btn" runat="server" Text="確認" class="btn" />
                                        </td>
                                        <td valign="middle">
                                            <asp:Button ID="ConvertPng_btn" runat="server" Text="輸出圖檔" OnClientClick="return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td valign="top" width="160px" rowspan="2">
                                                    <img src="img/compare.png" />查詢項目
                                                    <asp:DropDownList ID="Item_DDList" runat="server" AutoPostBack="True">
                                                        <asp:ListItem>電壓</asp:ListItem>
                                                        <asp:ListItem>電流</asp:ListItem>
                                                        <asp:ListItem Selected="True">功率</asp:ListItem>
                                                        <asp:ListItem>需量</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="350px" nowrap="nowrap">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Mode" runat="server" Text="預測模式" Visible="false">
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="Item_CBList" runat="server" RepeatDirection="Horizontal">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td rowspan="2">
                                                        <asp:CheckBoxList ID="W_CBList" runat="server" RepeatDirection="Horizontal" Visible="false">
                                                        </asp:CheckBoxList>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td nowrap="nowrap">
                                                                <asp:Label ID="Demand" runat="server" Text="最大需量" Visible="false">
                                                                </asp:Label>
                                                            </td>
                                                            <td nowrap="nowrap">
                                                                <asp:CheckBoxList ID="Demand_CBList" runat="server" RepeatDirection="Horizontal" Visible="false">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="Item_DDList" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <br />  
                 
            <div id="content" align="left" style="padding-left:0px;">
                <div align="center" style="background-color: white;">
                    <asp:Label ID="msg" runat="server" Text="查無資料" Visible="false"></asp:Label>
                </div>
                <jqChart:Chart ID="Chart_V" Width="880px" Height="520px" runat="server" DataSourceID="SqlDataSource1" Visible="false" MouseInteractionMode="Zooming">
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

                <jqChart:Chart ID="Chart_I" Width="880px" Height="520px" runat="server" DataSourceID="SqlDataSource2" Visible="false" MouseInteractionMode="Zooming">
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

                <jqChart:Chart ID="Chart_W" Width="880px" Height="520px" runat="server" DataSourceID="SqlDataSource3" Visible="false" MouseInteractionMode="Zooming">
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

                <jqChart:Chart ID="Chart_Mode" Width="880px" Height="520px" runat="server" DataSourceID="SqlDataSource4" Visible="false" MouseInteractionMode="Zooming">
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
            </div>
            <a id="download_img" hidden="hidden"></a>
            <div id="displayBox" style="display:none;">
                <img src="img/loading-bar.gif" /><br />
                <h1>Please wait...</h1>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

