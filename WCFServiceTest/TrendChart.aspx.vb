Imports System.Data.OleDb
Imports System.Data

Partial Class SumTrendChart
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
        SqlDataSource4.SelectParameters.Clear()

        Dim group As String = Request.QueryString("group")
        Dim node As String = Request.QueryString("node")
        Dim begin_time As String = Request.QueryString("begin_time")
        Dim end_time As String = Request.QueryString("end_time")
        Dim interval As String = Request.QueryString("interval")
        Dim item As String = Request.QueryString("item")
        Dim value As String = Request.QueryString("value")

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("MasterConn").ToString()

        Dim title() As String = node.Split(",")
        Dim check_count_c, check_count_m, check_count_p As Integer
        Dim ctrlnr((title.Length / 5) - 1) As String
        Dim meterid((title.Length / 5) - 1) As String
        Dim position((title.Length / 5) - 1) As String

        For i = 0 To title.Length - 1
            If title(i).Split(":").GetValue(0) = "ECO5編號" Or title(i).Split(":").GetValue(0) = "ECO5 Number" Then
                ctrlnr(check_count_c) = title(i).Split(":").GetValue(1)
                check_count_c += 1
            ElseIf title(i).Split(":").GetValue(0) = "電表編號" Or title(i).Split(":").GetValue(0) = "Meter Number" Then
                meterid(check_count_m) = title(i).Split(":").GetValue(1)
                check_count_m += 1
            ElseIf title(i).Split(":").GetValue(0) = "安裝位置" Or title(i).Split(":").GetValue(0) = "Installation location" Then
                position(check_count_p) = title(i).Split(":").GetValue(1)
                check_count_p += 1
            End If
        Next

        Dim values() As String = value.Split(",")
        Dim sqlstr(check_count_m - 1) As String
        Dim sql As String = ""
        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql &= "SELECT convert(datetime,(RecDate + ' ' + RecTime)) as RecDate," & value & " into #T" & i + 1 & " FROM ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND (RecDate + ' ' + RecTime between '" & begin_time & "' and '" & end_time & "');"
            Else
                sql &= "SELECT convert(datetime,#T1.RecDate) as RecDate,"
                For j = 0 To values.Length - 1
                    For k = 0 To check_count_m - 1
                        sql &= "#T" & k + 1 & "." & values(j) & "+"
                    Next
                    sql = sql.Substring(0, sql.Length - 1)
                    If j <> values.Length Then
                        sql &= " AS " & values(j) & ","
                    End If
                Next
                sql = sql.Substring(0, sql.Length - 1)

                sql &= " FROM "
                For k = 0 To check_count_m - 1
                    sql &= "#T" & k + 1 & ","
                Next
                sql = sql.Substring(0, sql.Length - 1)

                sql &= " WHERE "
                If check_count_m > 1 Then
                    For k = 0 To check_count_m - 2
                        sql &= "substring(Replace(Convert(nvarchar(16),#T" & k + 1 & ".RecDate,120),'-','/'),1,16) = substring(Replace(Convert(nvarchar(16),#T" & k + 2 & ".RecDate,120),'-','/'),1,16) and "
                    Next
                End If

                Select Case interval
                    Case "1"
                        If check_count_m > 1 Then
                            sql = sql.Substring(0, sql.Length - 4) & " order by RecDate;"
                        Else
                            sql = sql.Substring(0, sql.Length - 6) & " order by RecDate;"
                        End If
                    Case "5"
                        sql &= " (substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),16,1) Like '%5%' or substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),16,1) Like '%0%') order by RecDate;"
                    Case "30"
                        sql &= " (substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%00%' or substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%30%') order by RecDate;"
                    Case "60"
                        sql &= " substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%00%' order by RecDate;"
                End Select

                sql &= "Drop table "
                For k = 0 To check_count_m - 1
                    sql &= "#T" & k + 1 & ","
                Next
                sql = sql.Substring(0, sql.Length - 1)
            End If
        Next

        Try
            Dim positions As String = ""
            For i = 0 To check_count_m - 1
                positions &= position(i) & ","
            Next

            Dim strTitle As String = ""
            If Session("language") = "en" Then
                strTitle = "Electricity Trend Chart"
            Else
                strTitle = "電力趨勢圖"
            End If

            positions = positions.Substring(0, positions.Length - 1)
            If item = "電壓" Or item = "Voltage" Then
                SqlDataSource1.ConnectionString = strcon
                SqlDataSource1.ProviderName = "System.Data.OleDb"
                SqlDataSource1.SelectCommand = sql
                Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)   '資料筆數
                If dv.Count > 0 Then
                    Chart_V.Title.Text = strTitle & "(" & positions & ") "

                    'For i = 0 To check_count_m - 1
                    '    Chart_V.Series(i).Title = position(i)
                    'Next
                    Chart_V.Visible = True
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    img_btn.Visible = True
                Else
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
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
                    Chart_I.Title.Text = strTitle & "(" & positions & ")"
                    'For i = 0 To check_count_m - 1
                    '    Chart_I.Series(i).Title = position(i)
                    'Next
                    Chart_V.Visible = False
                    Chart_I.Visible = True
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    img_btn.Visible = True
                Else
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
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
                    Chart_W.Title.Text = strTitle & "(" & positions & ") "
                    'For i = 0 To check_count_m - 1
                    '    Chart_W.Series(i).Title = position(i)
                    'Next
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = True
                    Chart_Mode.Visible = False
                    img_btn.Visible = True
                Else
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    img_btn.Visible = False
                    Dim msg As String = "查無資料"
                    If Session("language").ToString = "en" Then
                        msg = " No Data. "
                    End If
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                End If
            ElseIf item = "需量" Or item = "Demand" Then
                SqlDataSource4.ConnectionString = strcon
                SqlDataSource4.ProviderName = "System.Data.OleDb"
                SqlDataSource4.SelectCommand = sql
                Dim dv As DataView = SqlDataSource4.Select(New DataSourceSelectArguments)
                If dv.Count > 0 Then
                    Chart_Mode.Title.Text = strTitle
                    'msg.Visible = False
                    Chart_Mode.Visible = True
                    Chart_W.Visible = False
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    img_btn.Visible = True
                Else
                    Chart_Mode.Visible = False
                    Chart_W.Visible = False
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    img_btn.Visible = False
                    Dim msg As String = "查無資料"
                    If Session("language").ToString = "en" Then
                        msg = " No Data. "
                    End If
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                End If
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
