
Partial Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetNoStore()
        Session.Abandon()
        Session.Clear()
        Session.Contents.RemoveAll()

        Response.Redirect("home.aspx")
    End Sub
End Class
