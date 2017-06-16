Imports System.Data.OleDb

Partial Class ValiAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = ""
            Dim uFlag As Boolean = False
            Dim message As String = ""
            Response.ContentType = "text/xml;charset=UTF-8"
            Response.AddHeader("Cache-Control", "no-cache")
            Response.Write("<response>")

            If Request("field") = "account" Then
                If Request("type") = "NewUser" Then
                    sql = "select * from AdminSetup where Account = '" & Request("account") & "' "
                ElseIf Request("type") = "NewEcoAccount" Then
                    sql = "select * from ControllerSetup where ECO_Account = '" & Request("account") & "' "
                End If
            ElseIf Request("field") = "email" Then
                sql = "select * from AdminSetup where E_Mail = '" & Request("email") & "' "
            End If

            Using cmd As New OleDbCommand(sql, conn)
                If Request("field") = "account" Then
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        If Not dr.Read() Then
                            message = "帳號可使用"
                            uFlag = True
                        Else
                            message = "帳號重覆"
                        End If
                    End Using
                ElseIf Request("field") = "email" Then
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        If Not dr.Read() Then
                            message = "電子郵件可使用"
                            uFlag = True
                        Else
                            message = "電子郵件重覆"
                        End If
                    End Using
                End If

                Response.Write("<passed>" + uFlag.ToString() + "</passed>")
                Response.Write("<message>" + message + "</message>")
                Response.Write("<field>" + Request("field") + "</field>")
                Response.Write("</response>")
                Response.End()
            End Using
        End Using
    End Sub
End Class
