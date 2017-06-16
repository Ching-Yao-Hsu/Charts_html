
Partial Class index
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Response.Redirect("home.aspx")
            'Else
            '    Page.Header.Title = "電力分析系統 - 緯創"
        End If
    End Sub

    Protected Sub logout_btn_Click(sender As Object, e As EventArgs) Handles logout_btn.Click
        Session.Abandon()
        Session.Clear()
        Session.Contents.RemoveAll()

        Response.Redirect("home.aspx")
    End Sub

    Protected Sub GOMain_Click(sender As Object, e As EventArgs) Handles GOMain.Click
        Dim aa As String = "aa"
        Response.Redirect(System.Web.Configuration.WebConfigurationManager.AppSettings("bSpecial"))
    End Sub

End Class

