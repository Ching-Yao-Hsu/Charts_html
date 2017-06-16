Imports System.Data.OleDb
Imports System.Data

Partial Class TimeChart
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
        Dim group As String = Request.QueryString("group")
        Dim node As String = Request.QueryString("node")
        Dim datetime1() As String = Request.QueryString("datetime1").Split(",")
        Dim datetime2() As String = Request.QueryString("datetime2").Split(",")
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

        Dim start_time1 As String = datetime1.GetValue(0)
        Dim end_time1 As String = datetime1.GetValue(1)
        Dim start_time2 As String = datetime2.GetValue(0)
        Dim end_time2 As String = datetime2.GetValue(1)

        'Dim value_data() As String = {"Iavg", "Imax", "Vavg", "Vmax", "Wavg", "Wmax"}
        Dim sql1 As String = ""
        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql1 &= "SELECT (RecDate + ' ' + RecTime) as RecDate," & value & " into #T" & i + 1 & " from ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND (RecDate + ' ' + RecTime between '" & start_time1 & "' and '" & end_time1 & "') order by RecDate, RecTime;"
            Else
                sql1 &= "SELECT SUBSTRING(#T1.RecDate,12,5) AS RecDate1,"

                For k = 0 To check_count_m - 1
                    sql1 &= "#T" & k + 1 & ".W+"
                Next
                sql1 = sql1.Substring(0, sql1.Length - 1) & " AS Value1"

                sql1 &= " FROM "
                For k = 0 To check_count_m - 1
                    sql1 &= "#T" & k + 1 & ","
                Next
                sql1 = sql1.Substring(0, sql1.Length - 1)

                sql1 &= " WHERE "
                If check_count_m > 1 Then
                    For k = 0 To check_count_m - 2
                        sql1 &= "substring(#T" & k + 1 & ".RecDate,1,16)=substring(#T" & k + 2 & ".RecDate,1,16) and "
                    Next
                End If

                Select Case interval
                    Case "1"
                        If check_count_m > 1 Then
                            sql1 = sql1.Substring(0, sql1.Length - 4) & " order by #T1.RecDate;"
                        Else
                            sql1 = sql1.Substring(0, sql1.Length - 6) & " order by #T1.RecDate;"
                        End If
                    Case "5"
                        sql1 &= " (substring(#T1.RecDate,16,1) Like '%5%' or substring(#T1.RecDate,16,1) Like '%0%') order by #T1.RecDate;"
                    Case "30"
                        sql1 &= " (substring(#T1.RecDate,15,2) Like '%00%' or substring(#T1.RecDate,15,2) Like '%30%') order by #T1.RecDate;"
                    Case "60"
                        sql1 &= " substring(#T1.RecDate,15,2) Like '%00%' order by #T1.RecDate;"
                End Select

                sql1 &= "Drop table "
                For k = 0 To check_count_m - 1
                    sql1 &= "#T" & k + 1 & ","
                Next
                sql1 = sql1.Substring(0, sql1.Length - 1)
            End If
        Next

        Dim sql2 As String = ""
        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql2 &= "SELECT (RecDate + ' ' + RecTime) as RecDate," & value & " into #T" & i + 1 & " from ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND (RecDate + ' ' + RecTime between '" & start_time2 & "' and '" & end_time2 & "') order by RecDate, RecTime;"
            Else
                sql2 &= "SELECT SUBSTRING(#T1.RecDate,12,5) AS RecDate2,"

                For k = 0 To check_count_m - 1
                    sql2 &= "#T" & k + 1 & ".W+"
                Next
                sql2 = sql2.Substring(0, sql2.Length - 1) & " AS Value2"

                sql2 &= " FROM "
                For k = 0 To check_count_m - 1
                    sql2 &= "#T" & k + 1 & ","
                Next
                sql2 = sql2.Substring(0, sql2.Length - 1)

                sql2 &= " WHERE "
                If check_count_m > 1 Then
                    For k = 0 To check_count_m - 2
                        sql2 &= "substring(#T" & k + 1 & ".RecDate,1,16)=substring(#T" & k + 2 & ".RecDate,1,16) and "
                    Next
                End If

                Select Case interval
                    Case "1"
                        If check_count_m > 1 Then
                            sql2 = sql2.Substring(0, sql2.Length - 4) & " order by #T1.RecDate;"
                        Else
                            sql2 = sql2.Substring(0, sql2.Length - 6) & " order by #T1.RecDate;"
                        End If
                    Case "5"
                        sql2 &= " (substring(#T1.RecDate,16,1) Like '%5%' or substring(#T1.RecDate,16,1) Like '%0%') order by #T1.RecDate;"
                    Case "30"
                        sql2 &= " (substring(#T1.RecDate,15,2) Like '%00%' or substring(#T1.RecDate,15,2) Like '%30%') order by #T1.RecDate;"
                    Case "60"
                        sql2 &= " substring(#T1.RecDate,15,2) Like '%00%' order by #T1.RecDate;"
                End Select

                sql2 &= "Drop table "
                For k = 0 To check_count_m - 1
                    sql2 &= "#T" & k + 1 & ","
                Next
                sql2 = sql2.Substring(0, sql2.Length - 1)
            End If
        Next

        SqlDataSource1.ConnectionString = strcon
        SqlDataSource1.ProviderName = "System.Data.OleDb"
        SqlDataSource1.SelectCommand = sql1
        Dim dv1 As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
        Dim dt1 As DataTable = dv1.ToTable()

        SqlDataSource2.ConnectionString = strcon
        SqlDataSource2.ProviderName = "System.Data.OleDb"
        SqlDataSource2.SelectCommand = sql2
        Dim dv2 As DataView = SqlDataSource2.Select(New DataSourceSelectArguments)
        Dim dt2 As DataTable = dv2.ToTable()

        If dt1.Rows.Count > 0 And dt2.Rows.Count > 0 Then
            Dim dt As DataTable = New DataTable
            dt.Columns.Add("RecDate1", GetType(Date))
            dt.Columns.Add("Value11", GetType(Double))
            dt.Columns.Add("RecDate2", GetType(Date))
            dt.Columns.Add("Value12", GetType(Double))

            If dt1.Rows.Count < dt2.Rows.Count Then
                For i = 0 To dt1.Rows.Count - 1
                    Dim recdate1 As Date = dt1.Rows(i).Item(0)
                    Dim w1 As Double = dt1.Rows(i).Item(1)
                    For j = 0 To dt2.Rows.Count - 1
                        Dim recdate2 As Date = dt2.Rows(j).Item(0)
                        Dim w2 As Double = dt2.Rows(j).Item(1)
                        If recdate1 = recdate2 Then
                            dt.Rows.Add(recdate1, w1, recdate2, w2)
                            Exit For
                        End If
                    Next
                Next
            Else
                For i = 0 To dt2.Rows.Count - 1
                    Dim recdate2 As Date = dt2.Rows(i).Item(0)
                    Dim w2 As Double = dt2.Rows(i).Item(1)
                    For j = 0 To dt1.Rows.Count - 1
                        Dim recdate1 As Date = dt1.Rows(j).Item(0)
                        Dim w1 As Double = dt1.Rows(j).Item(1)
                        If recdate1 = recdate2 Then
                            dt.Rows.Add(recdate1, w1, recdate2, w2)
                        End If
                    Next

                Next
            End If

            Dim positions As String = ""
            For i = 0 To check_count_m - 1
                positions &= position(i) & ","
            Next
            Dim strTitle As String = ""
            If Session("language") = "en" Then
                strTitle = "Time interval Comparison Chart"
            Else
                strTitle = "時段比對圖"
            End If

            positions = positions.Substring(0, positions.Length - 1)
            If item = "功率" Or item = "Power" Then
                Chart_W.Series(0).Title = start_time1.Split(" ").GetValue(0).Substring(5, 5)
                Chart_W.Series(1).Title = start_time2.Split(" ").GetValue(0).Substring(5, 5)
                Chart_W.Title.Text = strTitle & "(" & positions & ")"
                Chart_W.Visible = True
                Chart_V.Visible = False
                Chart_I.Visible = False
                Chart_W.DataSource = dt
                Chart_W.DataBind()
            ElseIf item = "電壓" Or item = "Voltage" Then
                Chart_V.Series(0).Title = start_time1.Split(" ").GetValue(0).Substring(5, 5)
                Chart_V.Series(1).Title = start_time2.Split(" ").GetValue(0).Substring(5, 5)
                Chart_V.Title.Text = strTitle & "(" & positions & ")"
                Chart_V.Visible = True
                Chart_W.Visible = False
                Chart_I.Visible = False
                Chart_V.DataSource = dt
                Chart_V.DataBind()
            ElseIf item = "電流" Or item = "Current" Then
                Chart_I.Series(0).Title = start_time1.Split(" ").GetValue(0).Substring(5, 5)
                Chart_I.Series(1).Title = start_time2.Split(" ").GetValue(0).Substring(5, 5)
                Chart_I.Title.Text = strTitle & "(" & positions & ")"
                Chart_I.Visible = True
                Chart_W.Visible = False
                Chart_V.Visible = False
                Chart_I.DataSource = dt
                Chart_I.DataBind()
            End If
            img_btn.Visible = True
            Panel_Chart.Visible = True
        Else
            Dim msg As String = "查無資料"
            If Session("language").ToString = "en" Then
                msg = " No Data. "
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
            Chart_W.Visible = False
            Chart_V.Visible = False
            Chart_I.Visible = False
            img_btn.Visible = False
            Panel_Chart.Visible = False
        End If
    End Sub
End Class
