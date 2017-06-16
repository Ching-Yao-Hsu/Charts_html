
<%@ Page Title="" Language="VB" MasterPageFile="~/index.master" AutoEventWireup="false" CodeFile="index.aspx.vb" Inherits="_Default" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="js/slide.js"></script>    
    <script type="text/javascript">
        var recdate = "<%=Application("RecDate")%>";
        var w = parseFloat($("<%=Application("W")%>"));
        var v_ar = parseFloat($("<%=Application("V_ar")%>"));
        var va = parseFloat($("<%=Application("VA")%>"));
        var newDate = recdate.replace(/-/g, '/'); // 變成"2012/01/01 12:30:10";  
        var w_data1 = [];
        var w_data2 = [];
        var w_data3 = [];
        w_data1.push([new Date(newDate), w]);
        w_data2.push([new Date(newDate), v_ar]);
        w_data3.push([new Date(newDate), va]);

        var i1 = parseFloat($("<%=Application("I1")%>"));
        var i2 = parseFloat($("<%=Application("I2")%>"));
        var i3 = parseFloat($("<%=Application("I3")%>"));
        var iavg = parseFloat($("<%=Application("Iavg")%>"));
        var i_data1 = [];
        var i_data2 = [];
        var i_data3 = [];
        var i_data4 = [];
        i_data1.push([new Date(newDate), i1]);
        i_data2.push([new Date(newDate), i2]);
        i_data3.push([new Date(newDate), i3]);
        i_data4.push([new Date(newDate), iavg]);

        var v1 = parseFloat($("<%=Application("V1")%>"));
        var v2 = parseFloat($("<%=Application("V2")%>"));
        var v3 = parseFloat($("<%=Application("V3")%>"));
        var vavg = parseFloat($("<%=Application("Vavg")%>"));
        var v_data1 = [];
        var v_data2 = [];
        var v_data3 = [];
        var v_data4 = [];
        v_data1.push([new Date(newDate), v1]);
        v_data2.push([new Date(newDate), v2]);
        v_data3.push([new Date(newDate), v3]);
        v_data4.push([new Date(newDate), vavg]);

        var background = {
            type: 'linearGradient',
            x0: 0,
            y0: 0,
            x1: 0,
            y1: 1,
            colorStops: [{ offset: 0, color: '#f3f4e2' },
                         { offset: 1, color: 'white' }]
        };

        $(document).ready(function () {
            $('#tree').load('OrgCharts_callAjax.aspx');
            var HrefAddress = "support/IndexSupport.htm";
            $('.support').colorbox({ innerWidth: 725, innerHeight: 540, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });

            $("#<%=ChangeSeries_btn.ClientID%>").click(function () {
                var item = $('#<%=item_RBList.ClientID %> input:checked').val();
                if (item == 'W') {
                    $('#W_jqChart').show();
                    $('#I_jqChart').hide();
                    $('#V_jqChart').hide();
                }else if (item == 'I') {
                    $('#I_jqChart').show();
                    $('#W_jqChart').hide();
                    $('#V_jqChart').hide();
                }
                else if (item == 'V') {
                    $('#V_jqChart').show();
                    $('#I_jqChart').hide();
                    $('#W_jqChart').hide();
                }
            });

            $('#W_jqChart').jqChart({
                title: {
                    text: '即時趨勢圖',
                    font: '15px sans-serif'
                },
                noDataMessage: {
                    text: '無資料',
                    font: '20px sans-serif'
                },
                border: { strokeStyle: '#6ba851' },
                background: background,
                border: {
                    cornerRadius: 20,
                    lineWidth: 2,
                    strokeStyle: '#c5c5c5'
                },
                shadows: {
                    enabled: true
                },
                axes: [
                        {
                            title: '時間',
                            type: 'dateTime',
                            location: 'bottom'
                        },
                        {
                            title: '功率(W)',
                            type: 'linear',
                            location: 'left'
                        }
                ],
                series: [
                            {
                                type: 'line',
                                title: '實功',
                                markers: null,
                                data: w_data1
                            },
                            {
                                type: 'line',
                                title: '虛功',
                                markers: null,
                                data: w_data2
                            },
                            {
                                type: 'line',
                                title: '視在',
                                markers: null,
                                data: w_data3
                            }
                ]
            });
            updateChart_W();

            $('#I_jqChart').jqChart({
                title: {
                    text: '即時趨勢圖',
                    font: '15px sans-serif'
                },
                noDataMessage: {
                    text: '無資料',
                    font: '20px sans-serif'
                },
                border: { strokeStyle: '#6ba851' },
                background: background,
                border: {
                    cornerRadius: 20,
                    lineWidth: 2,
                    strokeStyle: '#c5c5c5'
                },
                shadows: {
                    enabled: true
                },
                axes: [
                        {
                            title: '時間',
                            type: 'dateTime',
                            location: 'bottom'
                        },
                        {
                            title: '電流(A)',
                            type: 'linear',
                            location: 'left'
                        }
                ],
                series: [
                            {
                                type: 'line',
                                title: 'R相',
                                markers: null,
                                data: i_data1
                            },
                            {
                                type: 'line',
                                title: 'S相',
                                markers: null,
                                data: i_data2
                            },
                            {
                                type: 'line',
                                title: 'T相',
                                markers: null,
                                data: i_data3
                            },
                            {
                                type: 'line',
                                title: '平均',
                                markers: null,
                                data: i_data4
                            }
                ]
            });
            updateChart_I()

            $('#V_jqChart').jqChart({
                title: {
                    text: '即時趨勢圖',
                    font: '15px sans-serif'
                },
                noDataMessage: {
                    text: '無資料',
                    font: '20px sans-serif'
                },
                border: { strokeStyle: '#6ba851' },
                background: background,
                border: {
                    cornerRadius: 20,
                    lineWidth: 2,
                    strokeStyle: '#c5c5c5'
                },
                shadows: {
                    enabled: true
                },
                axes: [
                        {
                            title: '時間',
                            type: 'dateTime',
                            location: 'bottom'
                        },
                        {
                            title: '電壓(V)',
                            type: 'linear',
                            location: 'left'
                        }
                ],
                series: [
                            {
                                type: 'line',
                                title: 'R相',
                                markers: null,
                                data: v_data1
                            },
                            {
                                type: 'line',
                                title: 'S相',
                                markers: null,
                                data: v_data2
                            },
                            {
                                type: 'line',
                                title: 'T相',
                                markers: null,
                                data: v_data3
                            },
                            {
                                type: 'line',
                                title: '平均',
                                markers: null,
                                data: v_data4
                            }
                ]
            });
            updateChart_V()
        });

        function updateChart_W() {
            var lbl = $("#<%= node_temp.ClientID%>").text();
            var node = GetSelectedNode();
            if (node != '' && lbl != '') {
                var series = $('#W_jqChart').jqChart('option', 'series'); // get current series
                if (node != lbl) {
                    for (i = 0; i <= 2; i++) {
                        series.splice(0, 1); // remove the last series
                    }
                    $('#W_jqChart').jqChart('update'); // update (redraw) the chart
                }
                var count = $('#W_jqChart').jqChart('option', 'series').length; // get current series
                if (count != 3) {
                    w_data1 = [];
                    w_data2 = [];
                    w_data3 = [];
                    var newSeries1 = {
                        type: 'line',
                        title: '實功',
                        markers: null,
                        data: w_data1
                    };
                    var newSeries2 = {
                        type: 'line',
                        title: '虛功',
                        markers: null,
                        data: w_data2
                    };
                    var newSeries3 = {
                        type: 'line',
                        title: '視在',
                        markers: null,
                        data: w_data3
                    };
                    // get the data from the first series
                    series.push(newSeries1);
                    series.push(newSeries2);
                    series.push(newSeries3);
                    $('#W_jqChart').jqChart('update'); // update (redraw) the chart
                }
            }
            var series_count = $('#W_jqChart').jqChart('option', 'series').length; // get current series
            if (w_data1 != []) {
                if (series_count > 0) {
                    recdate = $("#<%= RecDate.ClientID%>").text();
                    w = parseFloat($("#<%= W.ClientID%>").text());
                    v_ar = parseFloat($("#<%= V_ar.ClientID%>").text());
                    va = parseFloat($("#<%= VA.ClientID%>").text());
                    var newDate = recdate.replace(/-/g, '/'); // 變成"2012/01/01 12:30:10";  
                    //data.splice(0, 1);    // remove the first element
                    w_data1.push([new Date(newDate), w]);  // add a new element
                    w_data2.push([new Date(newDate), v_ar]);
                    w_data3.push([new Date(newDate), va]);
                    $('#W_jqChart').jqChart('update');    // update chart
                    setTimeout("updateChart_W()", 2000);
                }
            } 
        }
        function updateChart_I() {
            var lbl = $("#<%= node_temp.ClientID%>").text();
            var node = GetSelectedNode();
            if (node != '' && lbl != '') {
                var series = $('#I_jqChart').jqChart('option', 'series'); // get current series
                if (node != lbl) {
                    for (i = 0; i <= 3; i++) {
                        series.splice(0, 1); // remove the last series
                    }
                    $('#I_jqChart').jqChart('update'); // update (redraw) the chart
                }
                var count = $('#I_jqChart').jqChart('option', 'series').length; // get current series
                if (count != 4) {
                    i_data1 = [];
                    i_data2 = [];
                    i_data3 = [];
                    i_data4 = [];
                    var newSeries1 = {
                        type: 'line',
                        title: 'R相',
                        markers: null,
                        data: i_data1
                    };
                    var newSeries2 = {
                        type: 'line',
                        title: 'S相',
                        markers: null,
                        data: i_data2
                    };
                    var newSeries3 = {
                        type: 'line',
                        title: 'T相',
                        markers: null,
                        data: i_data3
                    };
                    var newSeries4 = {
                        type: 'line',
                        title: '平均',
                        markers: null,
                        data: i_data4
                    };
                    // get the data from the first series
                    series.push(newSeries1);
                    series.push(newSeries2);
                    series.push(newSeries3);
                    series.push(newSeries4);
                    $('#I_jqChart').jqChart('update'); // update (redraw) the chart
                }
            }
            var series_count = $('#I_jqChart').jqChart('option', 'series').length; // get current series
            if (i_data4 != []) {
                if (series_count > 0) {
                    recdate = $("#<%= RecDate.ClientID%>").text();
                    i1 = parseFloat($("#<%= I1.ClientID%>").text());
                    i2 = parseFloat($("#<%= I2.ClientID%>").text());
                    i3 = parseFloat($("#<%= I3.ClientID%>").text());
                    iavg = parseFloat($("#<%= Iavg.ClientID%>").text());
                    var newDate = recdate.replace(/-/g, '/'); // 變成"2012/01/01 12:30:10";  
                    //data.splice(0, 1);    // remove the first element
                    i_data1.push([new Date(newDate), i1]);
                    i_data2.push([new Date(newDate), i2]);
                    i_data3.push([new Date(newDate), i3]);
                    i_data4.push([new Date(newDate), iavg]);
                    $('#I_jqChart').jqChart('update');    // update chart
                    setTimeout("updateChart_I()", 2000);
                }   
            }
        }
        function updateChart_V() {
            var lbl = $("#<%= node_temp.ClientID%>").text();
            var node = GetSelectedNode();
            if (node != '' && lbl != '') {
                var series = $('#V_jqChart').jqChart('option', 'series'); // get current series
                if (node != lbl) {
                    for (i = 0; i <= 3; i++) {
                        series.splice(0, 1); // remove the last series
                    }
                    $('#V_jqChart').jqChart('update'); // update (redraw) the chart
                }
                var count = $('#V_jqChart').jqChart('option', 'series').length; // get current series
                if (count != 4) {
                    v_data1 = [];
                    v_data2 = [];
                    v_data3 = [];
                    v_data4 = [];
                    var newSeries1 = {
                        type: 'line',
                        title: 'R相',
                        markers: null,
                        data: v_data1
                    };
                    var newSeries2 = {
                        type: 'line',
                        title: 'S相',
                        markers: null,
                        data: v_data2
                    };
                    var newSeries3 = {
                        type: 'line',
                        title: 'T相',
                        markers: null,
                        data: v_data3
                    };
                    var newSeries4 = {
                        type: 'line',
                        title: '平均',
                        markers: null,
                        data: v_data4
                    };
                    // get the data from the first series
                    series.push(newSeries1);
                    series.push(newSeries2);
                    series.push(newSeries3);
                    series.push(newSeries4);
                    $('#V_jqChart').jqChart('update'); // update (redraw) the chart
                }
            }
            var series_count = $('#I_jqChart').jqChart('option', 'series').length; // get current series
            if (v_data4 != []) {
                if (series_count > 0) {
                    recdate = $("#<%= RecDate.ClientID%>").text();
                    v1 = parseFloat($("#<%= V1.ClientID%>").text());
                    v2 = parseFloat($("#<%= V2.ClientID%>").text());
                    v3 = parseFloat($("#<%= V3.ClientID%>").text());
                    vavg = parseFloat($("#<%= Vavg.ClientID%>").text());
                    var newDate = recdate.replace(/-/g, '/'); // 變成"2012/01/01 12:30:10";  
                    //data.splice(0, 1);    // remove the first element
                    v_data1.push([new Date(newDate), v1]);
                    v_data2.push([new Date(newDate), v2]);
                    v_data3.push([new Date(newDate), v3]);
                    v_data4.push([new Date(newDate), vavg]);
                    $('#V_jqChart').jqChart('update');    // update chart
                    setTimeout("updateChart_V()", 2000);
                }
            }
        }
        function GetSelectedNode() {
            var treeViewData = window["<%=Meter_TreeView.ClientID%>" + "_Data"];
            if (treeViewData.selectedNodeID.value != "") {
                var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
                var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                var text = selectedNode.innerHTML;
                return text;
            } else {
                return '';
            }
        }
        function colorbox_Chart() {
            var session = "<%=session("Account")%>";
            if (session == "null") {
                alertify.alert("登入逾時，請重新登入");
                window.location = "home.aspx"
            } else {
                var treeViewData = window["<%=Meter_TreeView.ClientID%>" + "_Data"];
                if (treeViewData.selectedNodeID.value != "") {
                    var e = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Group_DropDownList").ClientID%>');
                    var group = e.options[e.selectedIndex].value;
                    var id = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("id").ClientID%>');
                    var NewArray = new Array();
                    NewArray = id.innerText.split("-");
                    var ctrlnr = NewArray[0];
                    var meterid = NewArray[1];
                    var HrefAddress = "PowerChartNow.aspx?group=" + group + "&ctrlnr=" + ctrlnr + "&meterid=" + meterid;
                    $('.imageButtonFinderClass').colorbox({ innerWidth: 960, innerHeight: 690, iframe: true, slideshow: true, scrolling: true, overlayClose: false, href: HrefAddress });
                } else {
                    alertify.alert("請選擇電表");
                }
            }
        }
    </script>

    <style type="text/css">
        .auto-style3
        {
            height: 30px;
        }
        .support
        {
            outline: none;
            border: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%
    Session("language") = "tw"
    %>
    <div id="content" align="center">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="即時電力數值" Font-Bold="True" Font-Size="16pt" CssClass="fontline"></asp:Label>
                </td>
                <td align="right" style="padding-right:10px;">
                    <asp:ImageButton ID="support" runat="server" ImageUrl="~/img/support.png" onmouseover="this.src='img/support_move.png'" onmouseout="this.src='img/support.png'" CssClass="support" CausesValidation="False" ToolTip="說明" />
                </td>
            </tr>
            <tr>
                <td>
                    <script type="text/javascript">
                        var xPos1, yPos1;
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        prm.add_beginRequest(BeginRequestHandler);
                        prm.add_endRequest(EndRequestHandler);
                        function BeginRequestHandler(sender, args) {
                            xPos1 = $get('div1').scrollLeft; //gvDiv請更換成適合的ID名稱
                            yPos1 = $get('div1').scrollTop;
                        }
                        function EndRequestHandler(sender, args) {
                            $get('div1').scrollLeft = xPos1;
                            $get('div1').scrollTop = yPos1;
                        }
                    </script>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="border-radius: 15px 15px 15px 15px; background-color: #e8f0ff;">
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td align="left" colspan="4" nowrap="nowrap">
                                                        <img src="img/icon-account.png" alt="img/icon-account.png"/>群組
                                                        <asp:DropDownList ID="Group_DropDownList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" nowrap="nowrap">
                                                        <img src="img/compare.png" alt="img/compare.png"/>顯示名稱</td>
                                                    <td align="left" valign="middle" nowrap="nowrap">
                                                        <asp:CheckBox ID="CtrlNr_CB" runat="server" Text="ECO5編號" /><br />
                                                        <asp:CheckBox ID="Position_CB" runat="server" Text="安裝位置" />
                                                    </td>
                                                    <td align="left" valign="middle" nowrap="nowrap">
                                                        <asp:CheckBox ID="MeterId_CB" runat="server" Text="電表編號" /><br />
                                                        <asp:CheckBox ID="LineNum_CB" runat="server" Text="單線圖編號" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="submit_btn" runat="server" Text="查詢" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <img src="img/Dash_01.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <div id="div1" style="height: 490px; width: 370px; overflow: auto; border-radius: 15px 15px 15px 15px;">
                                                <asp:TreeView ID="Meter_TreeView" runat="server" Style="margin-right: 3px" Font-Names="微軟正黑體" ForeColor="Black" ShowLines="True" CssClass="ParentNodeStyle" Visible="False">
                                                    <SelectedNodeStyle BackColor="#6699FF" ForeColor="White" />
                                                </asp:TreeView>
                                                <div id="tree"></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Group_DropDownList" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="3" valign="top">
                                <asp:Panel ID="Panel_Data" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel2" runat="server" Width="425px" Height="276px" BackImageUrl="~/img/power_panel2.jpg">
                                                <div style="height: 60px;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="240px" height="60px" nowrap="nowrap">
                                                                <div style="position: relative; padding-left: 90px; padding-top: 20px; width: 160px;">
                                                                    <asp:Label ID="RecDate" runat="server" Font-Size="16px" Text="--" CssClass="style3"></asp:Label>
                                                                </div>
                                                            </td>
                                                            <td width="55px" height="60px">
                                                                <div style="position: relative; padding-left: 25px; padding-top: 20px; width: 30px;">
                                                                    <asp:Label ID="id" runat="server" Font-Size="16px" Text="--" CssClass="style3"></asp:Label>
                                                                </div>
                                                            </td>
                                                            <td width="115px" height="60px">
                                                                <div style="position: relative; padding-left: 0px; padding-top: 0px; width: 110px;">
                                                                    <asp:ImageButton ID="PowerChart" runat="server" OnClientClick="colorbox_Chart()"
                                                                        CssClass="imageButtonFinderClass" ImageUrl="~/img/pe2.gif" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="50%" align="right" style="padding-right: 40px; padding-top: 50px;">
                                                                <table width="100%" style="height: 150px">
                                                                    <tr>
                                                                        <td align="right" valign="top">
                                                                            <div style="height: 50px; position: relative; padding-top: 0px; padding-right: 5px;">
                                                                                <asp:Label ID="W" runat="server" Text="--" Font-Size="40px"></asp:Label>
                                                                            </div>
                                                                            <div style="height: 20px; position: relative; padding-top: 10px;">
                                                                                <asp:Label ID="PF" runat="server" Text="--" Font-Size="20px"></asp:Label>
                                                                            </div>
                                                                            <div style="height: 25px; position: relative; padding-top: 7px;">
                                                                                <asp:Label ID="V_ar" runat="server" Text="--" Font-Size="20px"></asp:Label>
                                                                            </div>
                                                                            <div style="height: 20px; position: relative; padding-top: 5px;">
                                                                                <asp:Label ID="VA" runat="server" Text="--" Font-Size="20px"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" align="right" style="padding-right: 25px; padding-top: 30px;">
                                                                <table width="100%" style="height: 150px">
                                                                    <tr>
                                                                        <td align="right" valign="top">
                                                                            <div style="height: 50px; position: relative; padding-top: 10px; padding-right: 25px;">
                                                                                <asp:Label ID="KWH" runat="server" Text="--" Font-Size="40px"></asp:Label>
                                                                            </div>
                                                                            <div style="height: 20px; position: relative; padding-top: 18px;">
                                                                                <asp:Label ID="Iavg" runat="server" Text="--" Font-Size="25px"></asp:Label>
                                                                            </div>
                                                                            <div style="height: 20px; position: relative; padding-top: 22px;">
                                                                                <asp:Label ID="Vavg" runat="server" Text="--" Font-Size="25px"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                                                <div style="visibility: hidden;">
                                                    <asp:Label ID="node_temp" runat="server" Text="" Visible="true"></asp:Label>
                                                    <asp:Label ID="I1" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="I2" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="I3" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="V1" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="V2" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="V3" runat="server" Text=""></asp:Label>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:Timer ID="Timer1" runat="server" Interval="2800"></asp:Timer>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" align="center" valign="middle">
                                <div style="position: relative; padding-bottom: 15px;">
                                    <img src="img/power_item.png" alt="img/power_item.png"/>
                                </div>
                            </td>
                            <td nowrap="nowrap">
                                <asp:RadioButtonList ID="item_RBList" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="W" Selected="True">功率</asp:ListItem>
                                    <asp:ListItem Value="I">電流</asp:ListItem>
                                    <asp:ListItem Value="V">電壓</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td align="left">
                                <asp:Button ID="ChangeSeries_btn" runat="server" Text="顯示" OnClientClick="return false;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left: 10px">
                                <div id="W_jqChart" style="width: 410px; height: 250px;">
                                </div>
                                <div id="I_jqChart" style="width: 410px; height: 250px; display: none;">
                                </div>
                                <div id="V_jqChart" style="width: 410px; height: 250px; display: none;">
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

