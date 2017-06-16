Imports System.Data.OleDb

Partial Class MySetup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Account") IsNot Nothing Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    Dim strSQL As String = "select * from AdminSetup where Account = ?"
                    Using cmd As New OleDbCommand(strSQL, conn)
                        cmd.Parameters.AddWithValue("?Account", Session("Account").ToString)
                        Using dr As OleDbDataReader = cmd.ExecuteReader
                            If dr.Read() Then
                                Name.Text = dr("Name").ToString
                                Company.Text = dr("Company").ToString
                                Address.Text = dr("Address").ToString
                                Tel.Text = dr("Tel").ToString
                                Mobile.Text = dr("Mobile").ToString
                            End If
                        End Using
                    End Using
                End Using
            End If
        End If
    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim strSQL As String = "UPDATE AdminSetup SET Name = ?, Company = ?, Address =?, Tel = ?, Mobile = ? WHERE Account = ?"
            Using cmd As New OleDbCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("?Name", Name.Text)
                cmd.Parameters.AddWithValue("?Company", Company.Text)
                cmd.Parameters.AddWithValue("?Address", Address.Text)
                cmd.Parameters.AddWithValue("?Tel", Tel.Text)
                cmd.Parameters.AddWithValue("?Mobile", Mobile.Text)
                cmd.Parameters.AddWithValue("?Account", Session("Account").ToString)
                cmd.ExecuteReader()
            End Using
        End Using

        Dim msg As String = "個人資訊變更成功！"
        If Session("language") = "en" Then
            msg = "Personal information changed successfully!"
        End If
        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    End Sub

    Protected Sub cancel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "SELECT * FROM AdminSetup WHERE Account= ? "
            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?Account", Session("Account").ToString)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        Name.Text = dr("Name")
                        Company.Text = dr("Company")
                        Address.Text = dr("Address")
                        Tel.Text = dr("Tel")
                        Mobile.Text = dr("Mobile")
                    End If
                End Using
            End Using
        End Using
    End Sub
End Class
