Imports System.Data.OleDb
Imports System.Data

Partial Class CrossTrendChart
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Dim msg As String = "連線逾時，請重新登入(Connection timed out, please sign in again.)"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()
        SqlDataSource2.SelectParameters.Clear()
        SqlDataSource3.SelectParameters.Clear()

        Dim group As String = Request.QueryString("group")
        Dim node As String = Request.QueryString("node")
        Dim begin_time As String = Request.QueryString("begin_time")
        Dim end_time As String = Request.QueryString("end_time")
        Dim interval As String = Request.QueryString("interval")
        Dim item As String = Request.QueryString("item")
        Dim value As String = Request.QueryString("value")

        'Dim sYearString As String = Left(Request.QueryString("begin_time"), 4)     '紀錄有沒有跨年  --0表沒有　　1表有
        'If Left(Request.QueryString("begin_time"), 4) <> Left(Request.QueryString("end_time"), 4) Then
        ' sYearString = "," & Left(Request.QueryString("end_time"), 4)
        'End If

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString()
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("MasterConn").ToString()

        Dim title() As String = node.Split(",")
        Dim check_count_c, check_count_m, check_count_p As Integer
        Dim ctrlnr((title.Length / 5) - 1) As String
        Dim meterid((title.Length / 5) - 1) As String
        Dim position((title.Length / 5) - 1) As String

        For i = 0 To title.Length - 1
            If title(i).Split(":").GetValue(0) = "ECO5編號" Then
                ctrlnr(check_count_c) = title(i).Split(":").GetValue(1)
                check_count_c += 1
            ElseIf title(i).Split(":").GetValue(0) = "電表編號" Then
                meterid(check_count_m) = title(i).Split(":").GetValue(1)
                check_count_m += 1
            ElseIf title(i).Split(":").GetValue(0) = "安裝位置" Then
                position(check_count_p) = title(i).Split(":").GetValue(1)
                check_count_p += 1
            End If
        Next

        Dim sqlstr(check_count_m - 1) As String
        Dim sql As String = ""

        'Dim aYear() As String = Split(sYearString, ",")

        For i = 0 To check_count_m
            If i <> check_count_m Then
                Select Case interval
                    Case "1"
                        'For Y = 0 To UBound(aYear)
                        'If sql <> "" Then sql &= " Union "
                        'sql &= " select convert(datetime,RecDate) as RecDate," & value & " AS value" & i + 1 + Y & " into #T" & i + 1 + Y & _
                        '       " from ECO_" & Find_AdAccount(group) & "_" & aYear(Y) & ".dbo.PowerRecord " & _
                        '       " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & begin_time & "' and '" & end_time & "' order by RecDate;"

                        sql &= " select convert(datetime,(RecDate + ' ' + RecTime)) as RecDate," & value & " AS value" & i + 1 & " into #T" & i + 1 & _
                               " from ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                               " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND  (RecDate + ' ' + RecTime between '" & begin_time & "' and '" & end_time & "') order by RecDate, RecTime;"
                        'Next
                        'sql &= ";"
                    Case "5"
                        'For Y = 0 To UBound(aYear)
                        'If sql <> "" Then sql &= " Union "
                        'sql &= " select convert(datetime,RecDate) as RecDate," & value & " AS value" & i + 1 + Y & " into #T" & i + 1 + Y & _
                        '       " from ECO_" & Find_AdAccount(group) & "_" & aYear(Y) & ".dbo.PowerRecord " & _
                        '       " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & begin_time & "' and '" & end_time & "' order by RecDate;"
                        sql &= " select convert(datetime,(RecDate + ' ' + RecTime)) as RecDate," & value & " AS value" & i + 1 & " into #T" & i + 1 & _
                               " from ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                               " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND  (RecDate + ' ' + RecTime between '" & begin_time & "' and '" & end_time & "') order by RecDate, RecTime;"
                        'Next
                        'sql &= ";"
                    Case "30"
                        'For Y = 0 To UBound(aYear)
                        'If sql <> "" Then sql &= " Union "
                        'sql &= " select convert(datetime,RecDate) as RecDate," & value & " AS value" & i + 1 & " into #T" & i + 1 & _
                        '       " from ECO_" & Find_AdAccount(group) & "_" & aYear(Y) & ".dbo.PowerRecord " & _
                        '       " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & begin_time & "' and '" & end_time & "' order by RecDate;"
                        sql &= " select convert(datetime,(RecDate + ' ' + RecTime)) as RecDate," & value & " AS value" & i + 1 & " into #T" & i + 1 & _
                               " from ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                               " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND (RecDate + ' ' + RecTime between '" & begin_time & "' and '" & end_time & "') order by RecDate, RecTime;"
                        'Next
                        'sql &= ";"
                    Case "60"
                        'For Y = 0 To UBound(aYear)
                        'If sql <> "" Then sql &= " Union "
                        'sql &= " select convert(datetime,RecDate) as RecDate," & value & " AS value" & i + 1 & " into #T" & i + 1 & _
                        '       " from ECO_" & Find_AdAccount(group) & "_" & aYear(Y) & ".dbo.PowerRecord " & _
                        '       " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & begin_time & "' and '" & end_time & "' order by RecDate;"
                        sql &= " select convert(datetime,(RecDate + ' ' + RecTime)) as RecDate," & value & " AS value" & i + 1 & " into #T" & i + 1 & _
                               " from ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                               " where CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND  (RecDate + ' ' + RecTime between '" & begin_time & "' and '" & end_time & "') order by RecDate, RecTime;"
                        'Next
                        'sql &= ";"
                End Select
            Else
                Dim value_s As String = ""
                Dim table_s As String = ""
                Dim condition As String = ""
                Dim release As String = ""
                For j = 1 To check_count_m
                    value_s &= "value" & j & ","
                    table_s &= "#T" & j & ","
                    release &= "drop table #T" & j & ";"
                Next
                value_s = value_s.Substring(0, value_s.Length - 1)
                table_s = table_s.Substring(0, table_s.Length - 1)
                release = release.Substring(0, release.Length - 1)

                For k = 1 To check_count_m - 1
                    condition &= "Replace(Convert(nvarchar(16),#T" & k & ".RecDate,120),'-','/') = Replace(Convert(nvarchar(16),#T" & k + 1 & ".RecDate,120),'-','/') and "
                Next
                condition = condition.Substring(0, condition.Length - 4)
                Select Case interval
                    Case "1"
                        sql &= "select convert(datetime,#T1.RecDate) as RecDate," & value_s & " from " & table_s & " where " & condition & " " & release
                    Case "5"
                        sql &= "select convert(datetime,#T1.RecDate) as RecDate," & value_s & " from " & table_s & " where " & condition & " and (substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),16,1) Like '%5%' or substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),16,1) Like '%0%') order by RecDate;" & release
                    Case "30"
                        sql &= "select convert(datetime,#T1.RecDate) as RecDate," & value_s & " from " & table_s & " where " & condition & " and (substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%00%' or substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%30%') order by RecDate;" & release
                    Case "60"
                        sql &= "select convert(datetime,#T1.RecDate) as RecDate," & value_s & " from " & table_s & " where " & condition & " and substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%00%' order by RecDate;" & release
                End Select
            End If
        Next

        Try
            Dim begin_day As String = begin_time.Split(" ").GetValue(0)
            Dim end_day As String = end_time.Split(" ").GetValue(0)
            Dim time1 As String = begin_time.Split(" ").GetValue(1)
            Dim time2 As String = end_time.Split(" ").GetValue(1)
            Dim begin_hour As String = time1.Split(":").GetValue(0) & ":00"
            Dim end_hour As String = time2.Split(":").GetValue(0) & ":00"

            If item = "電壓" Or item = "Voltage" Then
                SqlDataSource1.ConnectionString = strcon
                SqlDataSource1.ProviderName = "System.Data.OleDb"
                SqlDataSource1.SelectCommand = sql
                Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)   '資料筆數
                If dv.Count > 0 Then
                    If Session("language").ToString = "en" Then
                        Chart_V.Title.Text = "Multi-Meter Trend Chart(" & begin_day & " " & begin_hour & "~" & end_day & " " & end_hour & ")"
                    Else
                        Chart_V.Title.Text = "多電表趨勢圖(" & begin_day & " " & begin_hour & "~" & end_day & " " & end_hour & ")"
                    End If
                    For i = 0 To check_count_m - 1
                        Chart_V.Series(i).Title = position(i)
                    Next
                    Chart_V.Visible = True
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    img_btn.Visible = True
                Else
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    img_btn.Visible = False
                    Dim msg As String = "查無資料"
                    If Session("language").ToString = "en" Then
                        msg = " No Data. "
                    End If
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                End If
            ElseIf item = "電流" Or item = "Current" Then
                SqlDataSource2.ConnectionString = strcon
                SqlDataSource2.ProviderName = "System.Data.OleDb"
                SqlDataSource2.SelectCommand = sql
                Dim dv As DataView = SqlDataSource2.Select(New DataSourceSelectArguments)   '資料筆數
                If dv.Count > 0 Then
                    If Session("language").ToString = "en" Then
                        Chart_I.Title.Text = "Multi-Meter Trend Chart(" & begin_day & " " & begin_hour & "~" & end_day & " " & end_hour & ")"
                    Else
                        Chart_I.Title.Text = "多電表趨勢圖(" & begin_day & " " & begin_hour & "~" & end_day & " " & end_hour & ")"
                    End If
                    For i = 0 To check_count_m - 1
                        Chart_I.Series(i).Title = position(i)
                    Next
                    Chart_V.Visible = False
                    Chart_I.Visible = True
                    Chart_W.Visible = False
                    img_btn.Visible = True
                Else
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    img_btn.Visible = False
                    Dim msg As String = "查無資料"
                    If Session("language").ToString = "en" Then
                        msg = " No Data. "
                    End If
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                End If
            ElseIf item = "功率" Or item = "Power" Then
                SqlDataSource3.ConnectionString = strcon
                SqlDataSource3.ProviderName = "System.Data.OleDb"
                SqlDataSource3.SelectCommand = sql
                Dim dv As DataView = SqlDataSource3.Select(New DataSourceSelectArguments)   '資料筆數
                If dv.Count > 0 Then
                    If Session("language").ToString = "en" Then
                        Chart_W.Title.Text = "Multi-Meter Trend Chart(" & begin_day & " " & begin_hour & "~" & end_day & " " & end_hour & ")"
                    Else
                        Chart_W.Title.Text = "多電表趨勢圖(" & begin_day & " " & begin_hour & "~" & end_day & " " & end_hour & ")"
                    End If
                    For i = 0 To check_count_m - 1
                        Chart_W.Series(i).Title = position(i)
                    Next
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = True
                    img_btn.Visible = True
                Else
                    Dim msg As String = "查無資料"
                    If Session("language").ToString = "en" Then
                        msg = " No Data. "
                    End If
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    img_btn.Visible = False
                End If
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
