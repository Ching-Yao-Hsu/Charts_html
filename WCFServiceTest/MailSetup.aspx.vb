Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.Net

Partial Class MailSetup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    Dim sql As String = "SELECT * FROM MailSetup"
                    Using cmd As New OleDbCommand(sql, conn)
                        Using dr As OleDbDataReader = cmd.ExecuteReader
                            '顯示郵件資訊
                            If dr.Read() Then
                                SmtpServer.Text = dr("SmtpServer").ToString
                                MailServer_Account.Text = dr("MailServer_Account").ToString
                                MailServer_Password.Text = dr("MailServer_Password").ToString
                                MailAddress.Text = dr("MailAddress").ToString
                                MailName.Text = dr("MailName").ToString
                                Bcc.Text = dr("Bcc").ToString
                            End If
                        End Using
                    End Using
                End Using
            End If
        End If
    End Sub



    'Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        'Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "SELECT * FROM MailSetup"
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    '更新
                    If dr.Read() Then
                        '更改密碼
                        If MailServer_Password.Text <> "" Then
                            Dim sql2 As String = "UPDATE MailSetup SET SmtpServer = ?, MailServer_Account = ?, MailServer_Password = ?, MailAddress = ?, MailName = ?, Bcc = ?"
                            Using cmd2 As New OleDbCommand(sql2, conn)
                                cmd2.Parameters.AddWithValue("?SmtpServer", SmtpServer.Text)
                                cmd2.Parameters.AddWithValue("?MailServer_Account", MailServer_Account.Text)
                                cmd2.Parameters.AddWithValue("?MailServer_Password", MailServer_Password.Text)
                                cmd2.Parameters.AddWithValue("?MailAddress", MailAddress.Text)
                                cmd2.Parameters.AddWithValue("?MailName", MailName.Text)
                                cmd2.Parameters.AddWithValue("?Bcc", Bcc.Text)
                                cmd2.ExecuteReader()
                            End Using

                            Dim msg As String = "更新成功"
                            If Session("language") = "en" Then
                                msg = "Update successful."
                            End If

                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                            cancel_btn_Click(Nothing, Nothing)
                        Else
                            Dim sql2 As String = "UPDATE MailSetup SET SmtpServer = ?, MailServer_Account = ?, MailAddress = ?, MailName = ?, Bcc = ?"
                            Using cmd2 As New OleDbCommand(sql2, conn)
                                cmd2.Parameters.AddWithValue("?SmtpServer", SmtpServer.Text)
                                cmd2.Parameters.AddWithValue("?MailServer_Account", MailServer_Account.Text)
                                cmd2.Parameters.AddWithValue("?MailAddress", MailAddress.Text)
                                cmd2.Parameters.AddWithValue("?MailName", MailName.Text)
                                cmd2.Parameters.AddWithValue("?Bcc", Bcc.Text)
                                cmd2.ExecuteReader()
                            End Using

                            Dim msg As String = "更新成功"
                            If Session("language") = "en" Then
                                msg = "Update successful."
                            End If
                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                            cancel_btn_Click(Nothing, Nothing)
                        End If
                    Else
                        '新增
                        Dim sql2 As String = "insert into MailSetup(SmtpServer,MailServer_Account,MailServer_Password,MailAddress,MailName,Bcc) values (?,?,?,?,?,?)"
                        Using cmd2 As New OleDbCommand(sql2, conn)
                            cmd2.Parameters.AddWithValue("?SmtpServer", SmtpServer.Text)
                            cmd2.Parameters.AddWithValue("?MailServer_Account", MailServer_Account.Text)
                            cmd2.Parameters.AddWithValue("?MailServer_Password", MailServer_Password.Text)
                            cmd2.Parameters.AddWithValue("?MailAddress", MailAddress.Text)
                            cmd2.Parameters.AddWithValue("?MailName", MailName.Text)
                            cmd2.Parameters.AddWithValue("?Bcc", Bcc.Text)
                            cmd2.ExecuteReader()
                        End Using

                        Dim msg As String = "新增成功"
                        If Session("language") = "en" Then
                            msg = "Insert successful."
                        End If
                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                        cancel_btn_Click(Nothing, Nothing)
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub edit_btn_Click(sender As Object, e As EventArgs) Handles edit_btn.Click
        SmtpServer.Enabled = True
        MailServer_Account.Enabled = True
        MailServer_Password.Enabled = True
        MailName.Enabled = True
        MailAddress.Enabled = True
        Bcc.Enabled = True
        submit_btn.Visible = True
        cancel_btn.Visible = True
        edit_btn.Visible = False
    End Sub

    Protected Sub send_btn_Click(sender As Object, e As EventArgs) Handles send_btn.Click
        If test_mail.Text = "" Then
            Dim msg As String = "請輸入您要測試的電子郵件地址"
            If Session("language") = "en" Then
                msg = "Please enter the email address you want to test."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        Else
            Dim mailserver As SmtpClient = New SmtpClient(SmtpServer.Text)
            mailserver.Credentials = New NetworkCredential(MailServer_Account.Text, MailServer_Password.Text)
            mailserver.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

            'mail郵件設置
            Dim mail As MailMessage = New MailMessage()
            Dim mfrom As MailAddress = Nothing
            If MailName.Text IsNot DBNull.Value Then
                mfrom = New MailAddress(MailAddress.Text, MailName.Text) '必須和SmtpServer相同
            Else
                mfrom = New MailAddress(MailAddress.Text)
            End If

            mail.From = mfrom
            mail.To.Add(test_mail.Text)
            'If Bcc.Text IsNot DBNull.Value Then
            '    mail.Bcc.Add(Bcc.Text)
            'End If

            Try


                If Session("language") = "en" Then
                    mail.Subject = "wer Analysis System test signal"
                    mail.Body = "   This is an analysis by the power system to test your mailbox as emails. <br/><br/>"
                    mail.Body &= "<font color=red>Note: This letter is automatically generated and sent by the system, do not reply.</font>" & "<br/><br/>"
                Else
                    mail.Subject = "電力分析系統測試信"
                    mail.Body = "   這是一封由 電力分析系統 用來測試您的信箱所寄出的電子郵件。 <br/><br/>"
                    mail.Body &= "<font color=red>注意：本信件是由系統自動產生與發送，請勿直接回覆</font>" & "<br/><br/>"
                End If

                mail.SubjectEncoding = System.Text.Encoding.GetEncoding("BIG5")
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("BIG5")
                mail.IsBodyHtml = True
                mailserver.Send(mail)

                Dim msg As String = "發送成功"
                If Session("language") = "en" Then
                    msg = "Sent successfully."
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            Catch ex As Exception
                Dim msg As String = "發送失敗"
                If Session("language") = "en" Then
                    msg = "Failed to send."
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            End Try

            SmtpServer.Enabled = False
            MailServer_Account.Enabled = False
            MailServer_Password.Enabled = False
            MailName.Enabled = False
            MailAddress.Enabled = False
            Bcc.Enabled = False
            submit_btn.Visible = False
            cancel_btn.Visible = False
            edit_btn.Visible = True
        End If

    End Sub

    Protected Sub cancel_btn_Click(sender As Object, e As EventArgs) Handles cancel_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "SELECT * FROM MailSetup"
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    '顯示郵件資訊
                    If dr.Read() Then
                        SmtpServer.Text = dr("SmtpServer").ToString
                        MailServer_Account.Text = dr("MailServer_Account").ToString
                        MailServer_Password.Text = dr("MailServer_Password").ToString
                        MailAddress.Text = dr("MailAddress").ToString
                        MailName.Text = dr("MailName").ToString
                        Bcc.Text = dr("Bcc").ToString
                    End If
                End Using
            End Using
        End Using

        SmtpServer.Enabled = False
        MailServer_Account.Enabled = False
        MailServer_Password.Enabled = False
        MailName.Enabled = False
        MailAddress.Enabled = False
        Bcc.Enabled = False
        submit_btn.Visible = False
        cancel_btn.Visible = False
        edit_btn.Visible = True
    End Sub
End Class
