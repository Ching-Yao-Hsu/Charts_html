<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WCFService_Test.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="rowid" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="rowid" HeaderText="rowid" InsertVisible="False" ReadOnly="True" SortExpression="rowid" />
                    <asp:BoundField DataField="customertypeid" HeaderText="customertypeid" SortExpression="customertypeid" />
                    <asp:BoundField DataField="isenabled" HeaderText="isenabled" SortExpression="isenabled" />
                    <asp:BoundField DataField="mno" HeaderText="mno" SortExpression="mno" />
                    <asp:BoundField DataField="msname" HeaderText="msname" SortExpression="msname" />
                    <asp:BoundField DataField="mname" HeaderText="mname" SortExpression="mname" />
                    <asp:BoundField DataField="mtel" HeaderText="mtel" SortExpression="mtel" />
                    <asp:BoundField DataField="mfax" HeaderText="mfax" SortExpression="mfax" />
                    <asp:BoundField DataField="maddr" HeaderText="maddr" SortExpression="maddr" />
                    <asp:BoundField DataField="remark" HeaderText="remark" SortExpression="remark" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:weberpConnectionString %>" SelectCommand="SELECT * FROM [bas_customer]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
