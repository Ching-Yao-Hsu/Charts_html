<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WCFService_Test.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <br />
    </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CtrlNr,MeterID,RecDate,RecTime" DataSourceID="SqlDataSource1" style="z-index: 1; left: 10px; top: 62px; position: absolute; height: 139px; width: 238px">
            <Columns>
                <asp:BoundField DataField="CtrlNr" HeaderText="CtrlNr" ReadOnly="True" SortExpression="CtrlNr" />
                <asp:BoundField DataField="MeterID" HeaderText="MeterID" ReadOnly="True" SortExpression="MeterID" />
                <asp:BoundField DataField="RecDate" HeaderText="RecDate" ReadOnly="True" SortExpression="RecDate" />
                <asp:BoundField DataField="RecTime" HeaderText="RecTime" ReadOnly="True" SortExpression="RecTime" />
                <asp:BoundField DataField="I1" HeaderText="I1" SortExpression="I1" />
                <asp:BoundField DataField="I2" HeaderText="I2" SortExpression="I2" />
                <asp:BoundField DataField="I3" HeaderText="I3" SortExpression="I3" />
                <asp:BoundField DataField="Iavg" HeaderText="Iavg" SortExpression="Iavg" />
                <asp:BoundField DataField="V1" HeaderText="V1" SortExpression="V1" />
                <asp:BoundField DataField="V2" HeaderText="V2" SortExpression="V2" />
                <asp:BoundField DataField="V3" HeaderText="V3" SortExpression="V3" />
                <asp:BoundField DataField="Vavg" HeaderText="Vavg" SortExpression="Vavg" />
                <asp:BoundField DataField="W" HeaderText="W" SortExpression="W" />
                <asp:BoundField DataField="V_ar" HeaderText="V_ar" SortExpression="V_ar" />
                <asp:BoundField DataField="VA" HeaderText="VA" SortExpression="VA" />
                <asp:BoundField DataField="PF" HeaderText="PF" SortExpression="PF" />
                <asp:BoundField DataField="KWh" HeaderText="KWh" SortExpression="KWh" />
                <asp:BoundField DataField="Mode1" HeaderText="Mode1" SortExpression="Mode1" />
                <asp:BoundField DataField="Mode2" HeaderText="Mode2" SortExpression="Mode2" />
                <asp:BoundField DataField="Mode3" HeaderText="Mode3" SortExpression="Mode3" />
                <asp:BoundField DataField="Mode4" HeaderText="Mode4" SortExpression="Mode4" />
                <asp:BoundField DataField="DeMand" HeaderText="DeMand" SortExpression="DeMand" />
                <asp:BoundField DataField="DeMandHalf" HeaderText="DeMandHalf" SortExpression="DeMandHalf" />
                <asp:BoundField DataField="DeMandSatHalf" HeaderText="DeMandSatHalf" SortExpression="DeMandSatHalf" />
                <asp:BoundField DataField="DeMandOff" HeaderText="DeMandOff" SortExpression="DeMandOff" />
                <asp:BoundField DataField="RushHour" HeaderText="RushHour" SortExpression="RushHour" />
                <asp:BoundField DataField="HalfHour" HeaderText="HalfHour" SortExpression="HalfHour" />
                <asp:BoundField DataField="SatHalfHour" HeaderText="SatHalfHour" SortExpression="SatHalfHour" />
                <asp:BoundField DataField="OffHour" HeaderText="OffHour" SortExpression="OffHour" />
                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ECO_twenergyConnectionString %>" SelectCommand="SELECT * FROM [PowerRecord]"></asp:SqlDataSource>
    </form>
</body>
</html>
