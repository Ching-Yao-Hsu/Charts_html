Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.IO
Imports System.Net.Mail
Imports System.Net

Partial Class AlterProcedure
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("account") IsNot Nothing Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    Dim sql As String = "select * from AdminSetup where Account = ?"
                    Using cmd As New OleDbCommand(sql, conn)
                        '解密
                        Dim account As String = DeCode(Request("account"), "ecosmart")
                        cmd.Parameters.AddWithValue("?account", account)
                        Using dr As OleDbDataReader = cmd.ExecuteReader
                            If dr.Read() Then
                                '判斷啟用狀態
                                If dr("Enabled") = 0 Then
                                    Dim EnabledTime As Date = dr("EnabledTime")
                                    Dim ts As TimeSpan = DateTime.Now - EnabledTime
                                    '判斷GUID是否過期
                                    If ts.TotalHours <= 24 Then
                                        Dim guid As String = Request("active")
                                        '判斷GUID是否相符
                                        If dr("GUID").ToString = guid Then
                                            Dim sql2 As String = "UPDATE AdminSetup SET Enabled = 1 ,EnabledTime = getdate() ,GUID = newid() WHERE Account = ?"
                                            Dim cmd2 As OleDbCommand = New OleDbCommand(sql2, conn)
                                            cmd2.Parameters.AddWithValue("?account", account)
                                            cmd2.ExecuteNonQuery()

                                            '建立資料庫
                                            If dr("CreateDB") = True Then
                                                'CreateDB(account)       '--1041125 mark (全移至新增會員-NewUser)
                                            End If

                                            Dim msg As String = "帳號啟用成功"
                                            If Session("language") = "en" Then
                                                msg = "Account Enable Success"
                                            End If
                                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                                        Else
                                            Dim msg As String = "驗證碼已變更，請重新發送驗證信"
                                            If Session("language") = "en" Then
                                                msg = "Verification code has been changed, please re-send the verification letter"
                                            End If
                                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                        End If
                                    Else
                                        Dim msg As String = "驗證碼已過期，請重新發送驗證信"
                                        If Session("language") = "en" Then
                                            msg = "Code expired, please re-send the verification letter"
                                        End If
                                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                    End If
                                Else
                                    Dim msg As String = "此帳號已啟用"
                                    If Session("language") = "en" Then
                                        msg = "This account has been enabled"
                                    End If
                                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                End If
                            Else
                                Response.Redirect("index.aspx")
                            End If
                        End Using
                    End Using
                End Using
            End If
        End If
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim strSQL As String = "select * from AdminSetup where Account = '" & Account.Text & "'"
            Using objcmd1 As New OleDbCommand(strSQL, conn)
                If Account.Text <> "" Then
                    Using dr As OleDbDataReader = objcmd1.ExecuteReader
                        If dr.Read() Then
                            If dr("Enabled") = 0 Then
                                Dim sql As String = "UPDATE AdminSetup SET GUID = newid(), EnabledTime = getdate() WHERE Account = '" & Account.Text & "'"
                                Using cmd As New OleDbCommand(sql, conn)
                                    cmd.ExecuteNonQuery()
                                End Using

                                Using objcmd2 As New OleDbCommand(strSQL, conn)
                                    Using dr2 As OleDbDataReader = objcmd2.ExecuteReader
                                        If dr2.Read() Then
                                            Try
                                                SendMail(Account.Text, dr2("E_Mail"), dr2("GUID").ToString)
                                            Catch ex As Exception
                                                ex.ToString()
                                            End Try
                                        End If
                                    End Using
                                End Using
                            Else
                                Dim msg As String = "此帳號已啟用"
                                If Session("language") = "en" Then
                                    msg = "This account has been enabled"
                                End If
                                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                            End If
                        Else
                            Dim msg As String = "無此帳號"
                            If Session("language") = "en" Then
                                msg = "No such account"
                            End If
                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                        End If
                    End Using
                Else
                    Dim msg As String = "請輸入帳號"
                    If Session("language") = "en" Then
                        msg = "Please enter your account"
                    End If
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                End If
            End Using
        End Using
    End Sub

    '解碼
    Function DeCode(ByVal DeString As String, ByVal sKey As String) As String
        Dim des As New DESCryptoServiceProvider()

        Dim inputByteArray As Byte() = New Byte(DeString.Length / 2 - 1) {}
        For x As Integer = 0 To DeString.Length / 2 - 1
            Dim i As Integer = (Convert.ToInt32(DeString.Substring(x * 2, 2), 16))
            inputByteArray(x) = CByte(i)
        Next

        des.Key = ASCIIEncoding.ASCII.GetBytes(sKey)
        '建立加密对象的密钥和偏移量，此值重要，不能修改
        des.IV = ASCIIEncoding.ASCII.GetBytes(sKey)
        Dim ms As New MemoryStream()
        Dim cs As New CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write)

        cs.Write(inputByteArray, 0, inputByteArray.Length)
        cs.FlushFinalBlock()

        '建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象
        Dim ret As New StringBuilder()

        Return System.Text.Encoding.[Default].GetString(ms.ToArray())
    End Function

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
                        'mailserver.Port = 25 ' 這行是郵箱送信的port, 但我將它關了, 沒什麼太大的用處
                        mailserver.Timeout = 50000 ' 送信若超過xx秒會停止送信, 以免佔用服務太多時間

                        '查IPv4的第一筆IP位址就可避開IPv6
                        Dim ip As String = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(Function(o) o.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork).ToString()
                        Dim url As String = Request.Url.AbsoluteUri.Split("/").GetValue(2)
                        Dim filepath As String = Request.FilePath

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
                            mail.Dispose()

                            Dim msg As String = "驗證信寄送成功"
                            If Session("language") = "en" Then
                                msg = "Verification letter sent successfully"
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
                        Dim msg As String = "郵件尚未設定"
                        If Session("language") = "en" Then
                            msg = "Mail has not been set"
                        End If
                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub CreateDB(ByVal account As String)
        Try
            '建立資料庫(account)
            Using conn_CreateDB As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("CreateDBConnectionString").ToString())
                If conn_CreateDB.State = 0 Then conn_CreateDB.Open()
                Dim sql_CreateDB As String = "Create Database ECO_" & account & ""
                Using cmd As New OleDbCommand(sql_CreateDB, conn_CreateDB)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            Using conn_table As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & account & "")
                If conn_table.State = 0 Then conn_table.Open()
                '建立資料表 PowerRecord
                Dim sql_PowerRecord As String = "CREATE TABLE PowerRecord(CtrlNr int NOT NULL,MeterID int NOT NULL,RecDate varchar(20) NOT NULL,I1 float,I2 float,I3 float,Iavg float," & _
                "V1 float,V2 float,V3 float,Vavg float,W float,V_ar float,VA float,PF float,KWh int,Mode1 float,Mode2 float,Mode3 float,Mode4 float," & _
                "DeMand int,DeMandHalf int,DeMandSatHalf int,DeMandOff int,RushHour int,HalfHour int,SatHalfHour int,OffHour int,State varchar(4)," & _
                "CONSTRAINT PK_PowerRecord PRIMARY KEY (CtrlNr, MeterID, RecDate))"
                Using cmd As New OleDbCommand(sql_PowerRecord, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '建立資料表 PowerRecordCollection
                Dim sql_PowerRecordCollection As String = "CREATE TABLE PowerRecordCollection(CtrlNr int NOT NULL,MeterID int NOT NULL,RecDate varchar(20) NOT NULL,Iavg float,Imax float," & _
                "Vavg float,Vmax float,Wavg float,Wmax float,DeMand int,DeMandHalf int,DeMandSatHalf int,DeMandOff int,RushHour int,HalfHour int,SatHalfHour int,OffHour int," & _
                "RushHourMax int,HalfHourMax int,SatHalfHourMax int,OffHourMax int,KWh int," & _
                "MonthMark char(2),WeekMark char(10),ImaxRD varchar(20),VmaxRD varchar(20),WmaxRD varchar(20)" & _
                "CONSTRAINT PK_PowerRecordCollection PRIMARY KEY (CtrlNr, MeterID, RecDate))"
                Using cmd As New OleDbCommand(sql_PowerRecordCollection, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_AlterPowerRecordCollection
                Dim sql_usp_RecordCollection As New StringBuilder
                sql_usp_RecordCollection.AppendLine("CREATE PROCEDURE [dbo].[Usp_AlterPowerRecordCollection]")
                sql_usp_RecordCollection.AppendLine("@CtrNr int,@MeterID int,@RecDate nVarchar(10),	@IAvg Float,@IMax Float,@VAvg Float,@VMax Float,@WAvg Float,@WMax Float,")
                sql_usp_RecordCollection.AppendLine("@DeMand int,@DeMandHalf int,@DeMandSatHalf int,@DeMandOff int,@RushHour int,@HalfHour int,@SatHalfHour int,@OffHour int,")
                sql_usp_RecordCollection.AppendLine("@RushHourMax int,@HalfHourMax int,@SatHalfHourMax int,@OffHourMax int,@KWh	int,")
                sql_usp_RecordCollection.AppendLine("@MonthMark nVarchar(2),@WeekMark nVarchar(10),@ImaxRD nVarchar(20),@VmaxRD nVarchar(20),@WmaxRD nVarchar(20)")
                sql_usp_RecordCollection.AppendLine("AS")
                sql_usp_RecordCollection.AppendLine("BEGIN")
                sql_usp_RecordCollection.AppendLine("declare @iCount int")
                sql_usp_RecordCollection.AppendLine("set @iCount = (Select count(*) From PowerRecordCollection Where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate)")
                sql_usp_RecordCollection.AppendLine("If @iCount < 1")
                sql_usp_RecordCollection.AppendLine("Begin")
                sql_usp_RecordCollection.AppendLine("Insert into PowerRecordCollection")
                sql_usp_RecordCollection.AppendLine("Select @CtrNr, @MeterID, @RecDate, @IAvg, @IMax, @VAvg, @VMax, @WAvg, @WMax,")
                sql_usp_RecordCollection.AppendLine("@DeMand, @DeMandHalf, @DeMandSatHalf, @DeMandOff, @RushHour, @HalfHour, @SatHalfHour, @OffHour,")
                sql_usp_RecordCollection.AppendLine("@RushHourMax, @HalfHourMax, @SatHalfHourMax, @OffHourMax, @KWh, @MonthMark, @WeekMark, @ImaxRD, @VmaxRD, @WmaxRD")
                sql_usp_RecordCollection.AppendLine("End")
                sql_usp_RecordCollection.AppendLine("Else")
                sql_usp_RecordCollection.AppendLine("Begin")
                sql_usp_RecordCollection.AppendLine("Update PowerRecordCollection")
                sql_usp_RecordCollection.AppendLine("set IAvg=@IAvg, IMax=@IMax, VAvg=@VAvg, VMax=@VMax, WAvg=@WAvg, WMax=@WMax,")
                sql_usp_RecordCollection.AppendLine("DeMand=@DeMand, DeMandHalf=@DeMandHalf, DeMandSatHalf=@DeMandSatHalf, DeMandOff=@DeMandOff,")
                sql_usp_RecordCollection.AppendLine("RushHour=@RushHour, HalfHour=@HalfHour, SatHalfHour=@SatHalfHour, OffHour=@OffHour,")
                sql_usp_RecordCollection.AppendLine("RushHourMax=@RushHourMax,HalfHourMax=@HalfHourMax,SatHalfHourMax=@SatHalfHourMax,OffHourMax=@OffHourMax,KWh=@KWh,")
                sql_usp_RecordCollection.AppendLine("MonthMark=@MonthMark, WeekMark=@WeekMark, ImaxRD=@ImaxRD, VmaxRD=@VmaxRD, WmaxRD=@WmaxRD")
                sql_usp_RecordCollection.AppendLine("Where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate")
                sql_usp_RecordCollection.AppendLine("End")
                sql_usp_RecordCollection.AppendLine("Return")
                sql_usp_RecordCollection.AppendLine("END")
                Using cmd As New OleDbCommand(sql_usp_RecordCollection.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_Day_Record
                Dim sql_Usp_Day_Record As New StringBuilder
                sql_Usp_Day_Record.AppendLine("CREATE PROCEDURE [dbo].[Usp_Day_Record]")
                sql_Usp_Day_Record.AppendLine("@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10)")
                sql_Usp_Day_Record.AppendLine("AS")
                sql_Usp_Day_Record.AppendLine("BEGIN")
                sql_Usp_Day_Record.AppendLine("SET NOCOUNT ON;")
                sql_Usp_Day_Record.AppendLine("declare @iavgW Float;declare @imaxW Float;declare @dmaxWRD nvarchar(20);")
                sql_Usp_Day_Record.AppendLine("declare @imaxV Float;declare @iavgV Float;declare @dmaxVRD nvarchar(20);")
                sql_Usp_Day_Record.AppendLine("declare @imaxI Float;declare @iavgI Float;declare @dmaxIRD nvarchar(20);")
                sql_Usp_Day_Record.AppendLine("declare @iDeMand int;declare @iDeMandHalf int;declare @iDeMandSatHalf int;declare @iDeMandOff int;")
                sql_Usp_Day_Record.AppendLine("declare @iRush int;declare @iHalf int;declare @iSatHalf int;declare @iOff int;")
                sql_Usp_Day_Record.AppendLine("declare @iMaxRush int;declare @iMaxHalf int;declare @iMaxSatHalf int;declare @iMaxOff int;declare @iKWh int;")
                sql_Usp_Day_Record.AppendLine("declare @iCount int;declare @Last_Day nvarchar(20);declare @MonthMark nvarchar(2);declare @WeekMark nvarchar(10)")
                sql_Usp_Day_Record.AppendLine("set @MonthMark = Right('00' + rtrim(Month(cast(@sRecDate as date))),2)")
                sql_Usp_Day_Record.AppendLine("set @WeekMark = rtrim(replace(cast(cast(DATEADD(week, DATEDIFF(week, '', @sRecDate), '') as date) as char),'-','/'))")
                sql_Usp_Day_Record.AppendLine("set @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate)")
                sql_Usp_Day_Record.AppendLine("If @ICOUNT  > 0")
                sql_Usp_Day_Record.AppendLine("Begin")
                sql_Usp_Day_Record.AppendLine("SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax")
                sql_Usp_Day_Record.AppendLine("into #T4 FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate")
                sql_Usp_Day_Record.AppendLine("Select @iavgI = Round(#T4.Iavg,2), @imaxI = Round(#T4.Imax,2), @iavgV = Round(#T4.Vavg,2), @imaxV = Round(#T4.Vmax,2), @iavgW = Round(#T4.Wavg,2), @imaxW = Round(#T4.Wmax,2) FROM #T4")
                sql_Usp_Day_Record.AppendLine("Select @dmaxIRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate AND PowerRecord.Iavg=#T4.Imax")
                sql_Usp_Day_Record.AppendLine("Select @dmaxVRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate AND PowerRecord.Vavg=#T4.Vmax")
                sql_Usp_Day_Record.AppendLine("Select @dmaxWRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate AND PowerRecord.W=#T4.Wmax")
                sql_Usp_Day_Record.AppendLine("Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff) From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate")
                sql_Usp_Day_Record.AppendLine("Select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));set @Last_Day = Replace(@Last_Day,'-','/')")
                sql_Usp_Day_Record.AppendLine("Select Top 1 SUBSTRING(RecDate,1,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour into #T1 From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day Order By RecDate DESC")
                sql_Usp_Day_Record.AppendLine("Select Top 1 SUBSTRING(RecDate,1,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour into #T2 From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate Order By RecDate DESC")
                sql_Usp_Day_Record.AppendLine("SELECT (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT RushHour FROM #T1) ELSE (SELECT TOP 1 RushHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS RushHour,")
                sql_Usp_Day_Record.AppendLine("(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT HalfHour FROM #T1) ELSE (SELECT TOP 1 HalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS HalfHour,")
                sql_Usp_Day_Record.AppendLine("(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT SatHalfHour FROM #T1) ELSE (SELECT TOP 1 SatHalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS SatHalfHour,")
                sql_Usp_Day_Record.AppendLine("(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT OffHour FROM #T1) ELSE (SELECT TOP 1 OffHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS OffHour INTO #T3")
                sql_Usp_Day_Record.AppendLine("Select Top 1")
                sql_Usp_Day_Record.AppendLine("@iRush = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.RushHour ELSE #T2.RushHour - #T3.RushHour END),")
                sql_Usp_Day_Record.AppendLine("@iHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.HalfHour ELSE #T2.HalfHour - #T3.HalfHour END),")
                sql_Usp_Day_Record.AppendLine("@iSatHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.SatHalfHour ELSE #T2.SatHalfHour - #T3.SatHalfHour END),")
                sql_Usp_Day_Record.AppendLine("@iOff = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.OffHour ELSE #T2.OffHour - #T3.OffHour END)")
                sql_Usp_Day_Record.AppendLine("From #T2,#T3")
                sql_Usp_Day_Record.AppendLine("Select TOP 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour")
                sql_Usp_Day_Record.AppendLine("From PowerRecord")
                sql_Usp_Day_Record.AppendLine("Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And SUBSTRING(RecDate,1,10)=@sRecDate")
                sql_Usp_Day_Record.AppendLine("Order By RecDate DESC")
                sql_Usp_Day_Record.AppendLine("Select Top 1 @iKWh = KWh From PowerRecord")
                sql_Usp_Day_Record.AppendLine("Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And SUBSTRING(RecDate,1,10)=@sRecDate")
                sql_Usp_Day_Record.AppendLine("Order By RecDate DESC")
                sql_Usp_Day_Record.AppendLine("Drop Table #T2,#T3")
                sql_Usp_Day_Record.AppendLine("Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff, @iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD")
                sql_Usp_Day_Record.AppendLine("END")
                sql_Usp_Day_Record.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_Day_Record.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_BatchRun
                Dim sql_Usp_BatchRun As New StringBuilder
                sql_Usp_BatchRun.AppendLine("CREATE PROCEDURE [dbo].[Usp_BatchRun]")
                sql_Usp_BatchRun.AppendLine("AS")
                sql_Usp_BatchRun.AppendLine("BEGIN")
                sql_Usp_BatchRun.AppendLine("SET NOCOUNT ON;")
                sql_Usp_BatchRun.AppendLine("declare @CtrlNr int;declare @MeterID int;declare @RecDate nVarchar(10);declare @RecDateS nVarchar(10);declare @RecDateE nVarchar(10);")
                sql_Usp_BatchRun.AppendLine("declare @iCount int;declare @Err int;set @RecDateS = '';set @RecDateE = '';")
                sql_Usp_BatchRun.AppendLine("DECLARE Cursor_tmp CURSOR FOR (SELECT DISTINCT CtrLNr , MeterID, rtrim(CAST(cast(RecDate as date) AS CHAR)) as RecDate FROM PowerRecord)")
                sql_Usp_BatchRun.AppendLine("OPEN Cursor_tmp")
                sql_Usp_BatchRun.AppendLine("FETCH NEXT FROM Cursor_tmp INTO @CtrlNr,@MeterID,@RecDate")
                sql_Usp_BatchRun.AppendLine("WHILE @@FETCH_STATUS = 0")
                sql_Usp_BatchRun.AppendLine("BEGIN")
                sql_Usp_BatchRun.AppendLine("if @RecDateS > @RecDate or @RecDateS ='' Begin set @RecDateS = @RecDate end")
                sql_Usp_BatchRun.AppendLine("else if @RecDateE < @RecDate Begin set @RecDateE = @RecDate end ")
                sql_Usp_BatchRun.AppendLine("set @RecDate = Replace(@RecDate,'-','/')")
                sql_Usp_BatchRun.AppendLine("exec Usp_Day_Record @CtrlNr,@MeterID,@RecDate")
                sql_Usp_BatchRun.AppendLine("FETCH NEXT FROM Cursor_tmp INTO @CtrlNr,@MeterID,@RecDate")
                sql_Usp_BatchRun.AppendLine("END")
                sql_Usp_BatchRun.AppendLine("CLOSE Cursor_tmp")
                sql_Usp_BatchRun.AppendLine("DEALLOCATE Cursor_tmp")
                sql_Usp_BatchRun.AppendLine("Select * From PowerRecordCollection Where RecDate Between @RecDateS and @RecDateE Order by CtrlNr, MeterID, RecDate Desc")
                sql_Usp_BatchRun.AppendLine("print rtrim(cast(@RecDateS as char)) + '至' + rtrim(cast(@RecDatee as char)) + '，共執行' + rtrim(cast(@@RowCount as char)) + '筆'")
                sql_Usp_BatchRun.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_BatchRun.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_Day_PowerRecord
                Dim sql_Usp_Day_PowerRecord As New StringBuilder
                sql_Usp_Day_PowerRecord.AppendLine("CREATE PROCEDURE [dbo].[Usp_Day_PowerRecord]")
                sql_Usp_Day_PowerRecord.AppendLine("@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10),")
                sql_Usp_Day_PowerRecord.AppendLine("@IAvg Float OUTPUT,@IMax Float OUTPUT,@VAvg Float OUTPUT,@VMax Float OUTPUT,@WAvg Float OUTPUT,@WMax Float OUTPUT,")
                sql_Usp_Day_PowerRecord.AppendLine("@DeMand int OUTPUT,@DeMandHalf int OUTPUT,@DeMandSatHalf int OUTPUT,@DeMandOff int OUTPUT,")
                sql_Usp_Day_PowerRecord.AppendLine("@RushHour int OUTPUT,@HalfHour int OUTPUT,@SatHalfHour int OUTPUT,@OffHour int OUTPUT,")
                sql_Usp_Day_PowerRecord.AppendLine("@ImaxRD nVarchar(20) OUTPUT,@VmaxRD nVarchar(20) OUTPUT,@WmaxRD nVarchar(20) OUTPUT")
                sql_Usp_Day_PowerRecord.AppendLine("AS")
                sql_Usp_Day_PowerRecord.AppendLine("BEGIN")
                sql_Usp_Day_PowerRecord.AppendLine("SET NOCOUNT ON;")
                sql_Usp_Day_PowerRecord.AppendLine("declare @iavgW Float;declare @imaxW Float;declare @dmaxWRD nvarchar(20);")
                sql_Usp_Day_PowerRecord.AppendLine("declare @imaxV Float;declare @iavgV Float;declare @dmaxVRD nvarchar(20);")
                sql_Usp_Day_PowerRecord.AppendLine("declare @imaxI Float;declare @iavgI Float;declare @dmaxIRD nvarchar(20);")
                sql_Usp_Day_PowerRecord.AppendLine("declare @iDeMand int;declare @iDeMandHalf int;declare @iDeMandSatHalf int;declare @iDeMandOff int;")
                sql_Usp_Day_PowerRecord.AppendLine("declare @iRush int;declare @iHalf int;declare @iSatHalf int;declare @iOff int;")
                sql_Usp_Day_PowerRecord.AppendLine("declare @iCount int;declare @Last_Day nvarchar(20);declare @MonthMark nvarchar(2);declare @WeekMark nvarchar(10)")
                sql_Usp_Day_PowerRecord.AppendLine("set @MonthMark = Right('00' + rtrim(Month(cast(@sRecDate as date))),2)")
                sql_Usp_Day_PowerRecord.AppendLine("set @WeekMark = rtrim(replace(cast(cast(DATEADD(week, DATEDIFF(week, '', @sRecDate), '') as date) as char),'-','/'))")
                sql_Usp_Day_PowerRecord.AppendLine("set @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate)")
                sql_Usp_Day_PowerRecord.AppendLine("If @ICOUNT  > 0")
                sql_Usp_Day_PowerRecord.AppendLine("Begin")
                sql_Usp_Day_PowerRecord.AppendLine("SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax")
                sql_Usp_Day_PowerRecord.AppendLine("into #T4 FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate")
                sql_Usp_Day_PowerRecord.AppendLine("Select @iavgI = Round(#T4.Iavg,2), @imaxI = Round(#T4.Imax,2), @iavgV = Round(#T4.Vavg,2), @imaxV = Round(#T4.Vmax,2), @iavgW = Round(#T4.Wavg,2), @imaxW = Round(#T4.Wmax,2) FROM #T4")
                sql_Usp_Day_PowerRecord.AppendLine("Select @dmaxIRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate AND PowerRecord.Iavg=#T4.Imax")
                sql_Usp_Day_PowerRecord.AppendLine("Select @dmaxVRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate AND PowerRecord.Vavg=#T4.Vmax")
                sql_Usp_Day_PowerRecord.AppendLine("Select @dmaxWRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate AND PowerRecord.W=#T4.Wmax")
                sql_Usp_Day_PowerRecord.AppendLine("Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff) From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate")
                sql_Usp_Day_PowerRecord.AppendLine("Select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));set @Last_Day = Replace(@Last_Day,'-','/')")
                sql_Usp_Day_PowerRecord.AppendLine("Select Top 1 SUBSTRING(RecDate,1,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour into #T1 From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day Order By RecDate DESC")
                sql_Usp_Day_PowerRecord.AppendLine("Select Top 1 SUBSTRING(RecDate,1,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour into #T2 From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate Order By RecDate DESC")
                sql_Usp_Day_PowerRecord.AppendLine("SELECT (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT RushHour FROM #T1) ELSE (SELECT TOP 1 RushHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS RushHour,")
                sql_Usp_Day_PowerRecord.AppendLine("(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT HalfHour FROM #T1) ELSE (SELECT TOP 1 HalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS HalfHour,")
                sql_Usp_Day_PowerRecord.AppendLine("(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT SatHalfHour FROM #T1) ELSE (SELECT TOP 1 SatHalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS SatHalfHour,")
                sql_Usp_Day_PowerRecord.AppendLine("(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT OffHour FROM #T1) ELSE (SELECT TOP 1 OffHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS OffHour INTO #T3")
                sql_Usp_Day_PowerRecord.AppendLine("Select Top 1")
                sql_Usp_Day_PowerRecord.AppendLine("@iRush = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.RushHour ELSE #T2.RushHour - #T3.RushHour END),")
                sql_Usp_Day_PowerRecord.AppendLine("@iHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.HalfHour ELSE #T2.HalfHour - #T3.HalfHour END),")
                sql_Usp_Day_PowerRecord.AppendLine("@iSatHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.SatHalfHour ELSE #T2.SatHalfHour - #T3.SatHalfHour END),")
                sql_Usp_Day_PowerRecord.AppendLine("@iOff = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.OffHour ELSE #T2.OffHour - #T3.OffHour END)")
                sql_Usp_Day_PowerRecord.AppendLine("From #T2,#T3")
                sql_Usp_Day_PowerRecord.AppendLine("Drop Table #T2,#T3")
                sql_Usp_Day_PowerRecord.AppendLine("Select @IAvg = @iavgI,@IMax = @imaxI,@VAvg = @iavgV,@VMax = @imaxV,@WAvg = @iavgW,@WMax = @imaxW,")
                sql_Usp_Day_PowerRecord.AppendLine("@RushHour = @iRush,@HalfHour = @iHalf,@SatHalfHour = @iSatHalf,@OffHour = @iOff,@ImaxRD = @dmaxIRD,@VmaxRD = @dmaxVRD,@WmaxRD = @dmaxWRD")
                sql_Usp_Day_PowerRecord.AppendLine("END")
                sql_Usp_Day_PowerRecord.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_Day_PowerRecord.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_Day_PowerRecord_Detail
                Dim sql_Usp_Day_PowerRecord_Detail As New StringBuilder
                sql_Usp_Day_PowerRecord_Detail.AppendLine("CREATE PROCEDURE [dbo].[Usp_Day_PowerRecord_Detail]")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10),@sInterval int")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("AS")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("BEGIN")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("SET NOCOUNT ON;")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("If @sInterval = 1")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Begin")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("SELECT substring(RecDate,11,9) as 時間,Iavg as 電流,Vavg as 電壓,W as 實功,V_ar as 虛功,VA as 視在,PF as 功因,KWh as 用電度數 ")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate order by RecDate")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("END")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Else if @sInterval = 5")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Begin")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("SELECT substring(RecDate,11,9) as 時間,Iavg as 電流,Vavg as 電壓,W as 實功,V_ar as 虛功,VA as 視在,PF as 功因,KWh as 用電度數 ")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("END")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Else if @sInterval = 30")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Begin")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("SELECT substring(RecDate,11,9) as 時間,Iavg as 電流,Vavg as 電壓,W as 實功,V_ar as 虛功,VA as 視在,PF as 功因,KWh as 用電度數 ")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("END")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Else if @sInterval = 60")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("Begin")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("SELECT substring(RecDate,11,9) as 時間,Iavg as 電流,Vavg as 電壓,W as 實功,V_ar as 虛功,VA as 視在,PF as 功因,KWh as 用電度數 ")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate and substring(recdate,15,2) Like '%00%' order by RecDate")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("END")
                sql_Usp_Day_PowerRecord_Detail.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_Day_PowerRecord_Detail.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_Month_PowerRecord
                Dim sql_Usp_Month_PowerRecord As New StringBuilder
                sql_Usp_Month_PowerRecord.AppendLine("CREATE PROCEDURE [dbo].[Usp_Month_PowerRecord]")
                sql_Usp_Month_PowerRecord.AppendLine("@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10),")
                sql_Usp_Month_PowerRecord.AppendLine("@IAvg Float OUTPUT,@IMax Float OUTPUT,@VAvg Float OUTPUT,@VMax Float OUTPUT,@WAvg Float OUTPUT,@WMax Float OUTPUT,")
                sql_Usp_Month_PowerRecord.AppendLine("@DeMand int OUTPUT,@DeMandHalf int OUTPUT,@DeMandSatHalf int OUTPUT,@DeMandOff int OUTPUT,")
                sql_Usp_Month_PowerRecord.AppendLine("@RushHour int OUTPUT,@HalfHour int OUTPUT,@SatHalfHour int OUTPUT,@OffHour int OUTPUT,")
                sql_Usp_Month_PowerRecord.AppendLine("@ImaxRD nVarchar(20) OUTPUT,@VmaxRD nVarchar(20) OUTPUT,@WmaxRD nVarchar(20) OUTPUT")
                sql_Usp_Month_PowerRecord.AppendLine("AS")
                sql_Usp_Month_PowerRecord.AppendLine("BEGIN")
                sql_Usp_Month_PowerRecord.AppendLine("SET NOCOUNT ON;")
                sql_Usp_Month_PowerRecord.AppendLine("declare @iavgW Float;declare @imaxW Float;declare @dmaxWRD nvarchar(20);")
                sql_Usp_Month_PowerRecord.AppendLine("declare @imaxV Float;declare @iavgV Float;declare @dmaxVRD nvarchar(20);")
                sql_Usp_Month_PowerRecord.AppendLine("declare @imaxI Float;declare @iavgI Float;declare @dmaxIRD nvarchar(20);")
                sql_Usp_Month_PowerRecord.AppendLine("declare @iDeMand int;declare @iDeMandHalf int;declare @iDeMandSatHalf int;declare @iDeMandOff int;")
                sql_Usp_Month_PowerRecord.AppendLine("declare @iRush int;declare @iHalf int;declare @iSatHalf int;declare @iOff int;declare @iSum int")
                sql_Usp_Month_PowerRecord.AppendLine("declare @iCount int;declare @Last_Day nvarchar(20);")
                sql_Usp_Month_PowerRecord.AppendLine("Begin")
                sql_Usp_Month_PowerRecord.AppendLine("SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax")
                sql_Usp_Month_PowerRecord.AppendLine("into #T6 FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,7)=@sRecDate")
                sql_Usp_Month_PowerRecord.AppendLine("Select @iavgI = Round(#T6.Iavg,2), @imaxI = Round(#T6.Imax,2), @iavgV = Round(#T6.Vavg,2), @imaxV = Round(#T6.Vmax,2), @iavgW = Round(#T6.Wavg,2), @imaxW = Round(#T6.Wmax,2) FROM #T6")
                sql_Usp_Month_PowerRecord.AppendLine("Select @dmaxIRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T6 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,7)=@sRecDate AND PowerRecord.Iavg=#T6.Imax")
                sql_Usp_Month_PowerRecord.AppendLine("Select @dmaxVRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T6 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,7)=@sRecDate AND PowerRecord.Vavg=#T6.Vmax")
                sql_Usp_Month_PowerRecord.AppendLine("Select @dmaxWRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T6 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,7)=@sRecDate AND PowerRecord.W=#T6.Wmax")
                sql_Usp_Month_PowerRecord.AppendLine("Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff) From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And SUBSTRING(RecDate,1,7)=@sRecDate")
                sql_Usp_Month_PowerRecord.AppendLine("SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER")
                sql_Usp_Month_PowerRecord.AppendLine("(Partition By SUBSTRING(RecDate,1,10) order by recdate desc) as Sort")
                sql_Usp_Month_PowerRecord.AppendLine("into #T1 FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID AND SUBSTRING(RecDate,1,7) = @sRecDate;")
                sql_Usp_Month_PowerRecord.AppendLine("SELECT RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER")
                sql_Usp_Month_PowerRecord.AppendLine("(Partition By SUBSTRING(RecDate,1,10) order by recdate) as Sort")
                sql_Usp_Month_PowerRecord.AppendLine("into #T3 FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID AND SUBSTRING(RecDate,1,7) = @sRecDate;")
                sql_Usp_Month_PowerRecord.AppendLine("SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour into #T2 FROM #T1 WHERE #T1.sort = 1 order by RecDate;")
                sql_Usp_Month_PowerRecord.AppendLine("SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour into #T4 FROM #T3 WHERE #T3.sort = 1 order by RecDate;")
                sql_Usp_Month_PowerRecord.AppendLine("SELECT SUBSTRING(#T1.RecDate,1,10) AS RecDate,")
                sql_Usp_Month_PowerRecord.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.RushHour,0)")
                sql_Usp_Month_PowerRecord.AppendLine("ELSE (isnull(#T1.RushHour,0) - (CASE WHEN EXISTS (SELECT #T2.RushHour FROM #T2) THEN isnull(#T2.RushHour,#T4.RushHour) ELSE (#T4.RushHour) END)) END) AS 尖峰,")
                sql_Usp_Month_PowerRecord.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.HalfHour,0)")
                sql_Usp_Month_PowerRecord.AppendLine("ELSE (isnull(#T1.HalfHour,0) - (CASE WHEN EXISTS (SELECT HalfHour FROM #T2) THEN isnull(#T2.HalfHour,#T4.HalfHour) ELSE (#T4.HalfHour) END)) END) AS 半尖峰,")
                sql_Usp_Month_PowerRecord.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.SatHalfHour,0)")
                sql_Usp_Month_PowerRecord.AppendLine("ELSE (isnull(#T1.SatHalfHour,0) - (CASE WHEN EXISTS (SELECT #T2.SatHalfHour FROM #T2) THEN isnull(#T2.SatHalfHour,#T4.SatHalfHour) ELSE (#T4.SatHalfHour) END)) END) AS 週六半尖峰,")
                sql_Usp_Month_PowerRecord.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.OffHour,0)")
                sql_Usp_Month_PowerRecord.AppendLine("ELSE (isnull(#T1.OffHour,0) - (CASE WHEN EXISTS (SELECT #T2.OffHour FROM #T2) THEN isnull(#T2.OffHour,#T4.OffHour) ELSE (#T4.OffHour) END)) END) AS 離峰 into #T5")
                sql_Usp_Month_PowerRecord.AppendLine("FROM #T1 left JOIN #T2 ON convert(char(10),dateadd(day,-1,#T1.RecDate),111) = #T2.RecDate,#T4")
                sql_Usp_Month_PowerRecord.AppendLine("WHERE #T1.Sort = 1 and #T1.RecDate = #T4.RecDate ORDER BY #T1.RecDate;")
                sql_Usp_Month_PowerRecord.AppendLine("Select @iRush=SUM(尖峰),@iHalf=SUM(半尖峰),@iSatHalf=SUM(週六半尖峰),@iOff=SUM(離峰),@iSum=SUM(尖峰+半尖峰+週六半尖峰+離峰) from #T5;")
                sql_Usp_Month_PowerRecord.AppendLine("drop table #T1,#T2,#T3,#T4,#T5;")
                sql_Usp_Month_PowerRecord.AppendLine("Select @IAvg = @iavgI,@IMax = @imaxI,@VAvg = @iavgV,@VMax = @imaxV,@WAvg = @iavgW,@WMax = @imaxW,")
                sql_Usp_Month_PowerRecord.AppendLine("@DeMand = @iDeMand,@DeMandHalf = @iDeMandHalf,@DeMandSatHalf = @iDeMandSatHalf,@DeMandOff = @iDeMandOff,")
                sql_Usp_Month_PowerRecord.AppendLine("@RushHour = @iRush,@HalfHour = @iHalf,@SatHalfHour = @iSatHalf,@OffHour = @iOff,")
                sql_Usp_Month_PowerRecord.AppendLine("@ImaxRD = @dmaxIRD,@VmaxRD = @dmaxVRD,@WmaxRD = @dmaxWRD")
                sql_Usp_Month_PowerRecord.AppendLine("END")
                sql_Usp_Month_PowerRecord.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_Month_PowerRecord.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_Month_PowerRecord_W_Detail
                Dim sql_Usp_Month_PowerRecord_W_Detail As New StringBuilder
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("CREATE PROCEDURE [dbo].[Usp_Month_PowerRecord_W_Detail]")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10)")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("AS")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("BEGIN")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("SET NOCOUNT ON;")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("SELECT SUBSTRING(RecDate,6,5) AS 日期,Round(AVG(W),2) AS 功率平均值,Round(MAX(W),2) AS 功率最大值")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND SUBSTRING(RecDate,1,7)=@sRecDate")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("GROUP BY SUBSTRING(RecDate,6,5) ORDER BY 日期")
                sql_Usp_Month_PowerRecord_W_Detail.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_Month_PowerRecord_W_Detail.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using

                '預存程序 Usp_Month_PowerRecord_E_Detail
                Dim sql_Usp_Month_PowerRecord_E_Detail As New StringBuilder
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("CREATE PROCEDURE [dbo].[Usp_Month_PowerRecord_E_Detail]")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("AS")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("BEGIN")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("SET NOCOUNT ON;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("(Partition By SUBSTRING(RecDate,1,10) order by recdate desc) as Sort")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("into #T1 FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID AND SUBSTRING(RecDate,1,7) = @sRecDate;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("SELECT RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("(Partition By SUBSTRING(RecDate,1,10) order by recdate) as Sort")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("into #T3 FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID AND SUBSTRING(RecDate,1,7) = @sRecDate;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("into #T2 FROM #T1 WHERE #T1.sort = 1 order by RecDate;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("into #T4 FROM #T3 WHERE #T3.sort = 1 order by RecDate;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("SELECT SUBSTRING(#T1.RecDate,1,10) AS RecDate,")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.RushHour,0)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("ELSE (isnull(#T1.RushHour,0) - (CASE WHEN EXISTS (SELECT #T2.RushHour FROM #T2)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("THEN isnull(#T2.RushHour,#T4.RushHour) ELSE (#T4.RushHour) END)) END) AS 尖峰,")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.HalfHour,0)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("ELSE (isnull(#T1.HalfHour,0) - (CASE WHEN EXISTS (SELECT HalfHour FROM #T2)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("THEN isnull(#T2.HalfHour,#T4.HalfHour) ELSE (#T4.HalfHour) END)) END) AS 半尖峰,")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.SatHalfHour,0)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("ELSE (isnull(#T1.SatHalfHour,0) - (CASE WHEN EXISTS (SELECT #T2.SatHalfHour FROM #T2)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("THEN isnull(#T2.SatHalfHour,#T4.SatHalfHour) ELSE (#T4.SatHalfHour) END)) END) AS 週六半尖峰,")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.OffHour,0)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("ELSE (isnull(#T1.OffHour,0) - (CASE WHEN EXISTS (SELECT #T2.OffHour FROM #T2)")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("THEN isnull(#T2.OffHour,#T4.OffHour) ELSE (#T4.OffHour) END)) END) AS 離峰 into #T5")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("FROM #T1 left JOIN #T2 ON convert(char(10),dateadd(day,-1,#T1.RecDate),111) = #T2.RecDate,#T4")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("WHERE #T1.Sort = 1 and #T1.RecDate = #T4.RecDate ORDER BY #T1.RecDate;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("Select 尖峰,半尖峰,週六半尖峰,離峰,尖峰+半尖峰+週六半尖峰+離峰 AS 總計 from #T5;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("drop table #T1,#T2,#T3,#T4,#T5;")
                sql_Usp_Month_PowerRecord_E_Detail.AppendLine("END")
                Using cmd As New OleDbCommand(sql_Usp_Month_PowerRecord_E_Detail.ToString, conn_table)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
