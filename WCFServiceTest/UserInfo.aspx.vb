Imports System.Data.OleDb
Imports System.Data

Partial Class UserInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then

                Dim strSQL As String = ""
                SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()
                If Session("Group").ToString = "總管理" Then
                    strSQL = "SELECT * FROM AdminSetup"
                    If Session("PerNum") <> 0 Then
                        strSQL = strSQL & " where PerNum=" & Session("PerNum")
                    End If
                    SqlDataSource1.SelectCommand = strSQL

                Else
                    strSQL = "SELECT * FROM AdminSetup WHERE ECO_Group = '" & Session("Group").ToString & "'"
                    If Session("PerNum") <> 0 Then
                        strSQL = strSQL & " and PerNum=" & Session("PerNum")
                    End If
                    SqlDataSource1.SelectCommand = strSQL
                End If
            End If
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Rank As Label = TryCast(e.Row.FindControl("Rank"), Label)
            Dim drv As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            Dim type As Integer = Convert.ToInt32(drv("Rank").ToString())
            If type = 1 Then
                Rank.Text = "一般"
            ElseIf type = 2 Then
                Rank.Text = "總管理"
            Else
                Rank.Text = "停用"
            End If

            Dim Image As Image = TryCast(e.Row.FindControl("enabled"), Image)
            Dim drv2 As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            Dim type2 As Boolean = drv2("Enabled")
            If drv2("Enabled") = True Then
                Image.ImageUrl = "img\on.png"
            Else
                Image.ImageUrl = "img\off.png"
            End If
        End If

    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        SqlDataSource1.SelectParameters.Clear()
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()

        If Account.Text = "" Then
            SqlDataSource1.SelectCommand = "SELECT * FROM AdminSetup"
            GridView1.DataBind()
        Else
            If Regex.IsMatch(Account.Text, "^\w+$") Then
                SqlDataSource1.SelectCommand = "SELECT * FROM AdminSetup WHERE Account = ?"
                SqlDataSource1.SelectParameters.Add("Account", TypeCode.String, Account.Text)
            Else
                Account.Text = ""
                Dim msg As String = "無效字元！"
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            End If
        End If
    End Sub
End Class
