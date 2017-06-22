﻿Imports System.Data.OleDb

Partial Class home
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Page.Header.Title = "電力分析系統 - 緯創"
    End Sub

    Protected Sub login_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles login.Click
        Try
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Dim strSQL As String = "select * from AdminSetup with (Nolock) where Account=? and Password=?"
                Using cmd As New OleDbCommand(strSQL, conn)
                    cmd.Parameters.AddWithValue("?Account", Account_txt.Text)
                    cmd.Parameters.AddWithValue("?Password", Password_txt.Text)
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        If Not dr.Read() Then
                            Account_txt.Text = ""
                            Password_txt.Text = ""

                            Dim msg As String = "帳密錯誤或無此帳密"
                            If Session("language").ToString = "en" Then
                                msg = "Error! This account is not found or password is wrong."
                            End If

                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)

                            Session.Abandon()
                            Session.Clear()
                        Else
                            If dr("Enabled") = True Then
                                If dr("Rank") = 0 Then  '停用
                                    Dim msg As String = "該帳號已停用，請洽管理員"
                                    If Session("language").ToString = "en" Then
                                        msg = "This account has been disabled, please contact Administrator."
                                    End If
                                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                Else '一般使用者
                                    Session("Account") = Account_txt.Text
                                    Session("Rank") = dr("Rank")
                                    Session("Group") = dr("ECO_Group")
                                    'Session("language") = "tw"
                                    Dim sql As String = "select ad.Account from AdminSetup as ad,ControllerSetup as CS where ad.ECO_Group = '" & Session("Group") & "' and ad.Account=cs.Account"
                                    Using cmd2 As New OleDbCommand(sql, conn)
                                        Using dr2 As OleDbDataReader = cmd2.ExecuteReader
                                            If dr2.Read() Then
                                                Session("Account_admin") = dr2("Account").ToString
                                            End If
                                        End Using
                                    End Using
                                    Dim strGoASPX As String = System.Web.Configuration.WebConfigurationManager.AppSettings("bSpecial")
                                    If Session("language") = "en" Then
                                        Response.Redirect(strGoASPX.Replace(".", "_en.").ToString)
                                    Else
                                        Response.Redirect(strGoASPX)
                                    End If
                                End If
                            Else
                                Dim msg As String = "該帳號尚未啟用，請洽管理員"
                                If Session("language").ToString = "en" Then
                                    msg = "This account is not enabled, please contact Administrator."
                                End If
                                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
