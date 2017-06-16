Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.IO
Imports System.Net

Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions


Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not Page.IsPostBack Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    Dim strSQL As String = "SELECT DISTINCT ECO_Group FROM AdminSetup WHERE Enabled = 1"
                    Using cmd As New OleDbCommand(strSQL, conn)
                        Using dr As OleDbDataReader = cmd.ExecuteReader

                            If Session("language") = "en" Then
                                Group_DropDownList.Items.Add("Please choose...")
                            Else
                                Group_DropDownList.Items.Add("請選擇")
                            End If

                            While (dr.Read() = True)
                                Dim item As String = dr("ECO_Group").ToString
                                Group_DropDownList.Items.Add(item)
                            End While
                            Group_DropDownList.Items(0).Selected = True
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
            Dim strSQL1 As String = "select * from AdminSetup where Account=? or E_Mail=?"
            Using cmd1 As New OleDbCommand(strSQL1, conn)
                cmd1.Parameters.AddWithValue("?Account", account_txt.Text)
                cmd1.Parameters.AddWithValue("?E_Mail", email_txt.Text)
                Using dr1 As OleDbDataReader = cmd1.ExecuteReader
                    If Not dr1.Read() Then
                        Try
                            '加入新使用者
                            'Dim val As String = "'" & account_txt.Text & "','" & password_txt.Text & "','" & rank_rabtn.SelectedValue & "','" & name_txt.Text & "','" & company_txt.Text & "','" & address_txt.Text & "','" & tel_txt.Text & "','" & mobile_txt.Text & "','" & email_txt.Text & "'"
                            Dim strSQL2 As String = "insert into AdminSetup(Account,Password,ECO_Group,Name,Company,Address,Tel,Mobile,E_Mail,Rank,GUID,Enabled,EnabledTime,CreateDB,SumDayMailSent,SumMonthMailSent"
                            If Session("PerNum") <> 0 Then
                                strSQL2 = strSQL2 & ",PerNum"
                            End If
                            strSQL2 = strSQL2 & ") values (?,?,?,?,?,?,?,?,?,?,newid(),0,getdate(),?,?,?"
                            If Session("PerNum") <> 0 Then
                                strSQL2 = strSQL2 & ",?"
                            End If
                            strSQL2 = strSQL2 & ")"
                            strSQL2 = strSQL2 & "; Insert into AdminCom Select distinct Account, '" & account_txt.Text & "' as Com,0 as Enabled, getdate() From AdminCom Where Account <> 'admin' "
                            strSQL2 = strSQL2 & "; Insert into AdminCom Select '" & account_txt.Text & "' as Account, Com , 0 as Enabled , getdate() as EnabledTime From AdminCom with (Nolock) Where account='admin' and (com<>'admin' and com<>'" & account_txt.Text & "')  "  '以Admin的權限來復製
                            strSQL2 = strSQL2 & "; Insert into AdminCom Select '" & account_txt.Text & "' as Account, '" & account_txt.Text & "' as Com,1 as Enabled, getdate() "
                            strSQL2 = strSQL2 & "; Insert into AdminCom Select 'admin' as Account, '" & account_txt.Text & "' as Com,1 as Enabled, getdate()  "

                            Using cmd2 As New OleDbCommand(strSQL2, conn)
                                cmd2.Parameters.AddWithValue("?Account", account_txt.Text)
                                cmd2.Parameters.AddWithValue("?Password", password_txt.Text)

                                If Group_DropDownList.SelectedValue = "請選擇" Or Group_DropDownList.SelectedValue = "Please choose..." Then
                                    cmd2.Parameters.AddWithValue("?ECO_Group", group_txt.Text)
                                Else
                                    cmd2.Parameters.AddWithValue("?ECO_Group", Group_DropDownList.SelectedValue)
                                End If
                                cmd2.Parameters.AddWithValue("?Name", name_txt.Text)
                                cmd2.Parameters.AddWithValue("?Company", company_txt.Text)
                                cmd2.Parameters.AddWithValue("?Address", address_txt.Text)
                                cmd2.Parameters.AddWithValue("?Tel", tel_txt.Text)
                                cmd2.Parameters.AddWithValue("?Mobile", mobile_txt.Text)
                                cmd2.Parameters.AddWithValue("?E_Mail", email_txt.Text)
                                cmd2.Parameters.AddWithValue("?Rank", rank_RBList.SelectedValue.ToString)
                                If CreateDB_CB.Checked = True Then
                                    cmd2.Parameters.AddWithValue("?CreateDB", True)
                                    CreateDB(account_txt.Text)
                                Else
                                    cmd2.Parameters.AddWithValue("?CreateDB", False)
                                End If
                                cmd2.Parameters.AddWithValue("?SumDayMailSent", False)
                                cmd2.Parameters.AddWithValue("?SumMonthMailSent", False)
                                If Session("PerNum") <> 0 Then
                                    cmd2.Parameters.AddWithValue("?PerNum", Session("PerNum").ToString.Trim)
                                End If
                                cmd2.ExecuteNonQuery()

                                If CreateDB_CB.Checked = True Then
                                    CreateDB(account_txt.Text)
                                End If
                            End Using

                            Using cmd As New OleDbCommand(strSQL1, conn)
                                cmd.Parameters.AddWithValue("?account", account_txt.Text)
                                cmd.Parameters.AddWithValue("?E_Mail", email_txt.Text)
                                Using dr As OleDbDataReader = cmd.ExecuteReader
                                    If dr.Read() Then
                                        '寄送認證信
                                        Try
                                            SendMail(dr("Account"), dr("E_Mail"), dr("GUID").ToString)
                                            account_txt.Text = ""
                                            password_txt.Text = ""
                                            Reconfirm_txt.Text = ""
                                            name_txt.Text = ""
                                            company_txt.Text = ""
                                            address_txt.Text = ""
                                            tel_txt.Text = ""
                                            mobile_txt.Text = ""
                                            email_txt.Text = ""
                                        Catch ex As Exception
                                            Dim msg As String = "無法使用信箱，請檢查信箱是否錯誤"
                                            If Session("language") = "en" Then
                                                msg = "Unable to use the mailbox, check the mailbox for errors"
                                            End If
                                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                        End Try
                                    End If
                                End Using
                            End Using
                        Catch ex As Exception
                            Dim msg As String = "使用者加入失敗"
                            If Session("language") = "en" Then
                                msg = "Users join fails"
                            End If
                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                        End Try
                    Else
                        Dim msg As String = "帳號或電子郵件重複，請重新輸入"
                        If Session("language") = "en" Then
                            msg = "Repeat account or e-mail, please re-enter!"
                        End If
                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub cancel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_btn.Click
        account_txt.Text = ""
        password_txt.Text = ""
        Reconfirm_txt.Text = ""
        name_txt.Text = ""
        company_txt.Text = ""
        address_txt.Text = ""
        tel_txt.Text = ""
        mobile_txt.Text = ""
        email_txt.Text = ""
    End Sub

    '編碼
    Function EnCode(ByVal EnString As String, ByVal sKey As String) As String
        'Dim b As Byte() = Encoding.UTF8.GetBytes(EnString)
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

    Protected Sub SendMail(ByVal account As String, ByVal email As String, ByVal guid As String)
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim sql As String = "SELECT * FROM MailSetup"
        Dim conn As OleDbConnection = New OleDbConnection(strcon)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
        Dim dr As OleDbDataReader = cmd.ExecuteReader()

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
            Dim filepath As String = "/" & path & "/Welcome_en.aspx"

            'mail郵件設置
            Dim mail As MailMessage = New MailMessage()
            Dim mfrom As MailAddress = Nothing
            If dr("MailName") IsNot DBNull.Value And dr("MailName").ToString <> "" Then
                mfrom = New MailAddress(dr("MailAddress").ToString, dr("MailName").ToString) '必須和SmtpServer相同
            Else
                mfrom = New MailAddress(dr("MailAddress").ToString)
            End If

            mail.From = mfrom
            mail.To.Add(email)

            If dr("Bcc") IsNot DBNull.Value And dr("Bcc").ToString <> "" Then
                mail.Bcc.Add(dr("Bcc").ToString)
            End If

            Try
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

                Dim msg As String = "成功加入新會員"
                If Session("language") = "en" Then
                    msg = "Successfully joined the new member"
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            Catch ex As Exception
                Dim msg As String = "郵件設定錯誤，無法寄送驗證信"
                If Session("language") = "en" Then
                    msg = "Mail Settings Error, unable to send the verification letter"
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            End Try
        Else
            Dim msg As String = "郵件尚未設定，無法寄送驗證信"
            If Session("language") = "en" Then
                msg = "Mail is not set, unable to send the verification letter"
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        End If
    End Sub

    Protected Sub CreateDB(ByVal account As String)
        Try
            '建立資料庫(account)
            Using conn_CreateDB As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("CreateDBConnectionString").ToString())
                If conn_CreateDB.State = 0 Then conn_CreateDB.Open()
                'Dim sql_CreateDB As String = "Create Database ECO_" & account & ""
                Dim sql_CreateDB As String = " Usp_CreateDB " & account
                Using cmd As New OleDbCommand(sql_CreateDB, conn_CreateDB)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            Dim sr As StreamReader = New StreamReader(Server.MapPath("./SQL/") + "CreateProcedure_ECO.sql")
            Dim strRead As String = sr.ReadToEnd
            Dim stmts = strRead.Split("$")
            Dim sYear = Year(Now).ToString.Trim
            Using conn_table As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & account & "_" & sYear)
                If conn_table.State = 0 Then conn_table.Open()
                For i As Integer = 0 To UBound(stmts)
                    Using cmd As New OleDbCommand(stmts(i), conn_table)
                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using

        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
