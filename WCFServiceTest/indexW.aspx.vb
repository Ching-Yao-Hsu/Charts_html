Imports System.Data.OleDb
Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString() & ";Initial Catalog=Water"
                SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()
                SqlDataSource1.SelectCommand = "SELECT * FROM Setup"
            End If
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim iWaterDate As Label = TryCast(e.Row.FindControl("LebWater"), Label)
            Dim drv As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            Dim sWaterDate As Integer = 0
            If Not IsDBNull(drv("水表值")) Then Convert.ToInt32(drv("水表值"))
            If sWaterDate < 0 Then
                iWaterDate.Text = "異常"
            End If
        End If

    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        SqlDataSource1.SelectParameters.Clear()
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString() & ";Initial Catalog=Water"
        SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()

        If txtKeyWork.Text = "" Then
            SqlDataSource1.SelectCommand = "SELECT * FROM Setup"
            GridView1.DataBind()
        Else
            If Regex.IsMatch(txtKeyWork.Text, "^\w+$") Then
                SqlDataSource1.SelectCommand = "SELECT * FROM Setup WHERE 圖面編號 like ? or 店鋪編號 like ? or 店鋪名稱 like ? order by ID"
                SqlDataSource1.SelectParameters.Add("圖面編號", TypeCode.String, "%" & txtKeyWork.Text & "%")
                SqlDataSource1.SelectParameters.Add("店鋪編號", TypeCode.String, "%" & txtKeyWork.Text & "%")
                SqlDataSource1.SelectParameters.Add("店鋪名稱", TypeCode.String, "%" & txtKeyWork.Text & "%")
                GridView1.DataBind()
            Else
                txtKeyWork.Text = ""
                Dim msg As String = "無效字元！"
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            End If
        End If
    End Sub
End Class
