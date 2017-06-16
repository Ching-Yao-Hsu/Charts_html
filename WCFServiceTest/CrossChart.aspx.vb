Imports System.Data.OleDb
Imports System.Data

Partial Class SumCrossChart
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
        Dim datetime As String = Request.QueryString("datetime")

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group)
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

        '用電量
        Dim sql_E As String = ""
        Dim str00, str01, str02, str03, str04, str05, str06 As String
        If Session("language").ToString = "en" Then
            str00 = "Time" : str01 = "Peak_Time" : str02 = "Half_Peak_Time" : str03 = "Saturday_Half_Peak_Time" : str04 = "Off_Peak_Time" : str05 = "Max_Power" : str06 = "Average_Power"
        Else
            str00 = "時間" : str01 = "尖峰" : str02 = "半尖峰" : str03 = "週六半尖峰" : str04 = "離峰" : str05 = "功率最大值" : str06 = "功率平均值"
        End If

        'Dim value_E() As String = {"尖峰", "半尖峰", "週六半尖峰", "離峰"}
        Dim value_E() As String = {str01, str02, str03, str04}


        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql_E &= " SELECT RecDate,RushHour AS " & str01 & ",HalfHour AS " & str02 & ",SatHalfHour AS " & str03 & ",OffHour AS " & str04 & " into #T" & i + 1 & " " & _
                         " FROM ECO_" & Find_AdAccount(group) & "_PowerRecordCollection " & _
                         " WHERE CtrlNr= " & ctrlnr(i) & " and MeterID= " & meterid(i) & " and substring(RecDate,1,7) = '" & datetime & "' order by RecDate;"
            Else
                sql_E &= "SELECT SUBSTRING(#T1.RecDate,6,5) AS " & str00 & ","
                For j = 0 To value_E.Length - 1
                    For k = 0 To check_count_m - 1
                        sql_E &= "#T" & k + 1 & "." & value_E(j) & "+"
                    Next
                    sql_E = sql_E.Substring(0, sql_E.Length - 1)
                    If j <> value_E.Length Then
                        sql_E &= " AS " & value_E(j) & ","
                    End If
                Next
                sql_E = sql_E.Substring(0, sql_E.Length - 1)

                sql_E &= " FROM "
                For k = 0 To check_count_m - 1
                    sql_E &= "#T" & k + 1 & ","
                Next
                sql_E = sql_E.Substring(0, sql_E.Length - 1)

                If check_count_m > 1 Then
                    sql_E &= " WHERE "
                    For k = 0 To check_count_m - 2
                        sql_E &= "#T" & k + 1 & ".RecDate = #T" & k + 2 & ".RecDate and "
                    Next
                    sql_E = sql_E.Substring(0, sql_E.Length - 4)
                End If

                sql_E &= ";Drop table "
                For k = 0 To check_count_m - 1
                    sql_E &= "#T" & k + 1 & ","
                Next
                sql_E = sql_E.Substring(0, sql_E.Length - 1)
            End If
        Next

        'Dim value_W() As String = {"功率平均值", "功率最大值"}
        Dim value_W() As String = {str06, str05}
        Dim sql_W As String = ""
        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql_W &= " SELECT SUBSTRING(RecDate,6,5) AS RecDate,Round(AVG(Wavg),2) AS " & str06 & ",Round(MAX(Wmax),2) AS " & str05 & " into #T" & i + 1 & " " & _
                         " FROM ECO_" & Find_AdAccount(group) & "_PowerRecordCollection " & _
                         " WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND substring(RecDate,1,7) = '" & datetime & "' GROUP BY RecDate ORDER BY RecDate;"
            Else
                sql_W &= " SELECT #T1.RecDate AS " & str00 & ","
                For j = 0 To value_W.Length - 1
                    For k = 0 To check_count_m - 1
                        sql_W &= "#T" & k + 1 & "." & value_W(j) & "+"
                    Next
                    sql_W = sql_W.Substring(0, sql_W.Length - 1)
                    If j <> value_W.Length Then
                        sql_W &= " AS " & value_W(j) & ","
                    End If
                Next
                sql_W = sql_W.Substring(0, sql_W.Length - 1)

                sql_W &= " FROM "
                For k = 0 To check_count_m - 1
                    sql_W &= "#T" & k + 1 & ","
                Next
                sql_W = sql_W.Substring(0, sql_W.Length - 1)

                If check_count_m > 1 Then
                    sql_W &= " WHERE "
                    For k = 0 To check_count_m - 2
                        sql_W &= "#T" & k + 1 & ".RecDate = #T" & k + 2 & ".RecDate and "
                    Next
                    sql_W = sql_W.Substring(0, sql_W.Length - 4)
                End If

                sql_W &= ";Drop table "
                For k = 0 To check_count_m - 1
                    sql_W &= "#T" & k + 1 & ","
                Next
                sql_W = sql_W.Substring(0, sql_W.Length - 1)
            End If
        Next

        Try
            Dim positions As String = ""
            For i = 0 To check_count_m - 1
                positions &= position(i) & ","
            Next
            positions = positions.Substring(0, positions.Length - 1)
            PowerCrossChart.Title.Text = "Energy Comparison Chart(" & positions & ")"

            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql_E
            Dim dv_E As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            Dim dt_E As DataTable = dv_E.ToTable()
            Dim key_E As DataColumn() = New DataColumn(0) {}
            'key_E(0) = dt_E.Columns("時間")
            key_E(0) = dt_E.Columns(str00)
            dt_E.PrimaryKey = key_E

            SqlDataSource2.ConnectionString = strcon
            SqlDataSource2.ProviderName = "System.Data.OleDb"
            SqlDataSource2.SelectCommand = sql_W
            Dim dv_W As DataView = SqlDataSource2.Select(New DataSourceSelectArguments)
            Dim dt_W As DataTable = dv_W.ToTable()
            Dim key_W As DataColumn() = New DataColumn(0) {}
            'key_W(0) = dt_W.Columns("時間")
            key_W(0) = dt_W.Columns(str00)
            dt_W.PrimaryKey = key_W

            If dt_E.Rows.Count > 0 And dt_W.Rows.Count > 0 Then
                dt_E.Merge(dt_W, False, MissingSchemaAction.Add)
                PowerCrossChart.DataSource = dt_E
                PowerCrossChart.DataBind()
                PowerCrossChart.Visible = True
                Select Case dt_E.Rows.Count
                    Case Is < 7
                        PowerCrossChart.Width = 750
                    Case Is < 12
                        PowerCrossChart.Width = 850
                    Case Is < 17
                        PowerCrossChart.Width = 950
                    Case Is < 22
                        PowerCrossChart.Width = 1050
                    Case Is < 27
                        PowerCrossChart.Width = 1150
                    Case Is < 32
                        PowerCrossChart.Width = 1250
                End Select
                img_btn.Visible = True
            Else
                PowerCrossChart.Visible = False
                img_btn.Visible = False
                Dim msg As String = "查無資料"
                If Session("language").ToString = "en" Then
                    msg = " No Data. "
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
