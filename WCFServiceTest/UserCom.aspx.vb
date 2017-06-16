Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.IO
Imports System.Net.Mail
Imports System.Net

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim Cblindex As Integer = 0
            Account.Text = Request.QueryString("account")
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()

                '//顯示登入者的讀取DB資料，至於權限部分傳入Account的資料顯示
                Dim sql As String = " Select  A.Com as Com , A.Enabled, ECO_Group from [dbo].[AdminCom] A with (Nolock)  " & _
                                    " Left Join AdminSetup B with (Nolock)  On B.Account =A.com " & _
                                    " Where A.Account= ?  and Com in (select Com from [dbo].[AdminCom] where account='" & Session("Account").ToString & "' and enabled=1) "
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?Account", Account.Text)

                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        While (dr.Read() = True)
                            Cblindex += 1
                            Dim item As String = dr("ECO_Group").ToString
                            Dim itemV As String = dr("Com").ToString
                            Com_CheckBoxList.Items.Add(New ListItem(item, itemV))

                            If dr("Enabled") Then
                                Com_CheckBoxList.Items.FindByValue(itemV).Selected = True
                            End If
                        End While
                    End Using
                End Using
            End Using
        End If
    End Sub

    Protected Sub update_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles update_btn.Click

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "SELECT * FROM AdminCom A with (Nolock) WHERE A.Account= ? and A.com in (Select Com From AdminCom C with (nolock) where C.account='" & Session("Account").ToString & "' and enabled=1) "
            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?Account", Account.Text)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    While (dr.Read() = True)
                        Dim sql2 As String = "UPDATE AdminCom SET Enabled = ? WHERE Account = ? and Com=?"
                        Using cmd2 As New OleDbCommand(sql2, conn)
                            'cmd2.Parameters.AddWithValue("?Enabled", Account.Text)
                            'cmd2.Parameters.AddWithValue("?Account", Account.Text)
                            'cmd2.Parameters.AddWithValue("?Com", Password.Text)
                            'cmd2.Parameters.AddWithValue("?Account", Account.Text)


                            cmd2.Parameters.AddWithValue("?Enabled", Com_CheckBoxList.Items.FindByValue(dr("Com").ToString).Selected)
                            cmd2.Parameters.AddWithValue("?Account", dr("Account").ToString)
                            cmd2.Parameters.AddWithValue("?Com", dr("Com").ToString)
                            cmd2.ExecuteReader()
                        End Using
                    End While

                    Dim msg As String = "更新成功"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                End Using
            End Using
        End Using
    End Sub

End Class
