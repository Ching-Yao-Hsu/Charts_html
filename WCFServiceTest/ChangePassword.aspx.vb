Imports System.Data.OleDb

Partial Class ChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "select * from AdminSetup where Account = ?"
            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?Account", Session("Account").ToString)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        '判斷舊密碼
                        If OldPassword.Text = dr("Password").ToString Then
                            If NewPassword.Text = Reconfirm.Text Then
                                Dim sql2 As String = "UPDATE AdminSetup SET Password = ? WHERE  Account = ?"
                                Using cmd2 As New OleDbCommand(sql2, conn)
                                    cmd2.Parameters.AddWithValue("?Password", NewPassword.Text)
                                    cmd2.Parameters.AddWithValue("?Account", Session("Account").ToString)
                                    cmd2.ExecuteReader()
                                End Using

                                Dim msg As String = "密碼變更成功"
                                If Session("language") = "en" Then
                                    msg = "Password changed successfully"
                                End If
                                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                            Else
                                Dim msg As String = "密碼不相符"
                                If Session("language") = "en" Then
                                    msg = "Passwords do not match"
                                End If
                                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                            End If
                        Else
                            Dim msg As String = "舊密碼錯誤"
                            If Session("language") = "en" Then
                                msg = "Old password is incorrect"
                            End If
                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                        End If
                    End If
                End Using
            End Using
        End Using
    End Sub
End Class
