Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography
Imports System.IO

Partial Class Account
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
                                AccountL.Text = dr("Account").ToString
                                EnabledTime.Text = dr("EnabledTime").ToString
                                E_Mail.Text = dr("E_Mail").ToString
                                'If dr("DayReportSend") = 1 Then
                                '    DR_RBList.SelectedValue = 1
                                'Else
                                '    DR_RBList.SelectedValue = 0
                                'End If
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
            Dim sql As String = "SELECT * FROM AdminSetup WHERE Account=?"
            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?Account", Session("Account").ToString)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        Dim old_email As String = dr("E_Mail").ToString
                        '變更電子郵件
                        If old_email <> E_Mail.Text Then
                            Dim sql2 As String = "SELECT * FROM AdminSetup WHERE E_Mail = ? "
                            Using cmd2 As New OleDbCommand(sql2, conn)
                                cmd2.Parameters.AddWithValue("?E_Mail", E_Mail.Text)
                                Using dr2 As OleDbDataReader = cmd2.ExecuteReader
                                    '判斷Email是否重複
                                    If dr2.Read() Then
                                        Dim msg As String = ""
                                        If Session("language") = "en" Then
                                            msg = "Email repeat"
                                        Else
                                            msg = "電子郵件重複"
                                        End If

                                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                    Else
                                        Dim sql3 As String = "UPDATE AdminSetup SET E_Mail = ?, GUID=newid(), Enabled= 0, EnabledTime = getdate() WHERE Account = ?"
                                        Using cmd3 As New OleDbCommand(sql3, conn)
                                            cmd3.Parameters.AddWithValue("?E_Mail", E_Mail.Text)
                                            cmd3.Parameters.AddWithValue("?Account", Session("Account").ToString)
                                            cmd3.ExecuteReader()
                                        End Using

                                        Using cmd4 As New OleDbCommand(sql, conn)
                                            cmd4.Parameters.AddWithValue("?Account", Session("Account").ToString)
                                            Using dr4 As OleDbDataReader = cmd4.ExecuteReader
                                                If dr4.Read() Then
                                                    Try
                                                        SendMail(dr4("Account").ToString, E_Mail.Text, dr4("GUID").ToString)
                                                    Catch ex As Exception
                                                        ex.ToString()
                                                    End Try
                                                End If
                                            End Using
                                        End Using
                                    End If
                                    'Else
                                    '    Dim sql2 As String = "UPDATE AdminSetup SET DayReportSend = ? WHERE Account = ?"
                                    '    Dim cmd2 As OleDbCommand = New OleDbCommand(sql2, conn)
                                    '    cmd2.Parameters.AddWithValue("?DayReportSend", DR_RBList.SelectedValue)
                                    '    cmd2.Parameters.AddWithValue("?Account", Session("Account").ToString)

                                    '    cmd2.ExecuteReader()
                                    '    cmd2.Dispose()

                                    '    Dim msg As String = "更新成功！"
                                    '    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                    '    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                End Using
                            End Using
                        End If
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub SendMail(ByVal account As String, ByVal email As String, ByVal guid As String)
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "SELECT * FROM MailSetup"
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        '發送郵件的郵件服務器
                        Dim mailserver As SmtpClient = New SmtpClient(dr("SmtpServer").ToString)
                        mailserver.Credentials = New NetworkCredential(dr("MailServer_Account").ToString, dr("MailServer_Password").ToString)
                        mailserver.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
                        'mailserver.Host = "127.0.0.1" ' 基本都是以hinet為主, 除非你家的isp不走hinet系統

                        '查IPv4的第一筆IP位址就可避開IPv6
                        Dim ip As String = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(Function(o) o.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork).ToString()
                        Dim url As String = Request.Url.AbsoluteUri.Split("/").GetValue(2)
                        Dim path As String = Request.FilePath.Split("/").GetValue(1)
                        Dim filepath As String = "/" & path & "/Welcome.aspx"

                        'mail郵件設置
                        Dim mail As MailMessage = New MailMessage()
                        Dim mfrom As MailAddress = Nothing
                        If dr("MailName") IsNot DBNull.Value Then
                            mfrom = New MailAddress(dr("MailAddress").ToString, dr("MailName").ToString) '必須和SmtpClient相同
                        Else
                            mfrom = New MailAddress(dr("MailAddress").ToString)
                        End If

                        mail.From = mfrom
                        mail.To.Add(email)

                        If dr("Bcc") IsNot DBNull.Value And dr("Bcc").ToString <> "" Then
                            mail.Bcc.Add(dr("Bcc").ToString)
                        End If

                        If Session("language") = "en" Then
                            mail.Subject = "Power Analysis System account verification letter"
                            mail.Body = "Dear " & account & ",<br/><br/>"
                            mail.Body &= "Please click on the link below to activate your account.<br/><br/>"
                            mail.Body &= "http://" & url & "" & filepath & "?account=" + EnCode(account, "ecosmart") & "&active=" & guid & "<br/><br/>" '加密
                            mail.Body &= "Please activate your account within 24 hours (before " & DateTime.Now.AddDays(1) & "), otherwise invalid codes.<br/><br/>"
                            mail.Body &= "Resend verification letter：http://" & url & "" & filepath & "<br/><br/>"
                            mail.Body &= "<font color=red>Note: This letter is automatically generated and sent by the system, do not reply.</font>" & "<br/><br/>"
                        Else
                            mail.Subject = "電力分析系統帳號驗證信"
                            mail.Body = "親愛的用戶 " & account & "，您好：<br/><br/>"
                            mail.Body &= "請點選以下連結啟用您的帳號。<br/><br/>"
                            mail.Body &= "http://" & url & "" & filepath & "?account=" + EnCode(account, "ecosmart") & "&active=" & guid & "<br/><br/>" '加密
                            mail.Body &= "請在24小時內(" & DateTime.Now.AddDays(1) & "之前)啟用帳號，否則驗證碼失效。<br/><br/>"
                            mail.Body &= "重新寄送驗證信：http://" & url & "" & filepath & "<br/><br/>"
                            mail.Body &= "<font color=red>注意：本信件是由系統自動產生與發送，請勿直接回覆。</font>" & "<br/><br/>"
                        End If

                        mail.SubjectEncoding = System.Text.Encoding.GetEncoding("BIG5")
                        mail.BodyEncoding = System.Text.Encoding.GetEncoding("BIG5")
                        mail.IsBodyHtml = True
                        mailserver.Send(mail)

                        'Dim msg As String = "變更成功，請至更改後電子郵件重新啟用帳號"
                        Dim msg As String = ""
                        If Session("language") = "en" Then
                            msg = "Change successful, Please re-enable to change the e-mail account!"
                        Else
                            msg = "變更成功，請至更改後電子郵件重新啟用帳號"
                        End If
                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                    Else
                        'Dim msg As String = "郵件尚未設定，無法寄送驗證信"
                        Dim msg As String = ""
                        If Session("language") = "en" Then
                            msg = "Mail is not set, you can not send verification letter."
                        Else
                            msg = "郵件尚未設定，無法寄送驗證信"
                        End If
                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                    End If
                End Using
            End Using
        End Using
    End Sub

    '編碼
    Function EnCode(ByVal EnString As String, ByVal sKey As String) As String
        Dim DES As DESCryptoServiceProvider = New DESCryptoServiceProvider()
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey)
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey)
        Dim inputByteArray As Byte() = Encoding.[Default].GetBytes(EnString)
        Dim ms As New MemoryStream()
        Dim cs As New CryptoStream(ms, DES.CreateEncryptor(), CryptoStreamMode.Write)
        cs.Write(inputByteArray, 0, inputByteArray.Length)
        cs.FlushFinalBlock()
        Dim ret As New StringBuilder()
        For Each b As Byte In ms.ToArray()
            ret.AppendFormat("{0:X2}", b)
        Next
        ret.ToString()
        Return ret.ToString()
    End Function

    Protected Sub cancel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "SELECT * FROM AdminSetup WHERE Account=?"
            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?Account", Session("Account").ToString)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        E_Mail.Text = dr("E_Mail").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub
End Class
