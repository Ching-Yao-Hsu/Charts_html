Imports System.Data.OleDb
Imports System.Data

Partial Class ProportionChartM
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
        Dim node1 As String = Request.QueryString("node1")
        Dim node2 As String = Request.QueryString("node2")
        Dim begin_time As String = Request.QueryString("begin_time")
        Dim end_time As String = Request.QueryString("end_time")
        Dim value As String = Request.QueryString("value")

        Dim title1() As String = node1.Split(",")
        Dim check_count_c, check_count_m As Integer
        Dim ctrlnr((title1.Length / 4) - 1) As String
        Dim meterid((title1.Length / 4) - 1) As String

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("MasterConn").ToString()

        For i = 0 To title1.Length - 1
            If title1(i).Split(":").GetValue(0) = "ECO5編號" Then
                ctrlnr(check_count_c) = title1(i).Split(":").GetValue(1)
                check_count_c += 1
            ElseIf title1(i).Split(":").GetValue(0) = "電表編號" Then
                meterid(check_count_m) = title1(i).Split(":").GetValue(1)
                check_count_m += 1
            End If
        Next

        Dim str01, str02, str03, str04, str05 As String
        If Session("language") = "en" Then
            str01 = "Peak Time Electricity"
            str02 = "Half Peak Time Electricity"
            str03 = "Saturday Half Peak Time Electricity"
            str04 = "Off Peak Time Electricity"
            str05 = "Kilowatt"
        Else
            str01 = "尖峰電量"
            str02 = "半尖峰電量"
            str03 = "週六半尖峰電量"
            str04 = "離峰電量"
            str05 = "用電度數"
        End If

        Dim select_txt As String = ""
        If value = "RushHour" Then
            select_txt = str01
            value = "RushHourMax"
        ElseIf value = "HalfHour" Then
            select_txt = str02
            value = "HalfHourMax"
        ElseIf value = "SatHalfHour" Then
            value = "SatHalfHourMax"
            select_txt = str03
        ElseIf value = "OffHour" Then
            select_txt = str04
            value = "OffHourMax"
        ElseIf value = "KWh" Then
            select_txt = str05
            value = "KWh"
        End If

        Dim datetime As String = Nothing
        If begin_time.Substring(0, 7) = end_time.Substring(0, 7) Then
            datetime = " PRC.RecDate  = '" & end_time & "'"
        End If

        '組合SQL字串
        Dim sqlstr(check_count_m - 1) As String
        Dim sqlstr2(check_count_m - 1) As String
        Dim sqlstr3(check_count_m - 1) As String
        Dim sql As String = ""
        Dim sql2 As String = ""
        Dim sql3 As String = ""
        For i = 0 To check_count_m - 1
            '同月
            If begin_time.Substring(0, 7) = end_time.Substring(0, 7) Then
                sqlstr(i) = "SELECT MS.InstallPosition AS SelectItem," & value & " AS Value FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup AS CS,ECOSMART.dbo.MeterSetup AS MS " & _
                "WHERE PRC.CtrlNr = " & ctrlnr(i) & " AND PRC.MeterID = " & meterid(i) & " AND CS.ECO_Account = MS.ECO_Account " & _
                "AND PRC.CtrlNr = MS.CtrlNr AND PRC.MeterID = MS.MeterID AND CS.Account='" & Find_AdAccount(group) & "' AND " & datetime & " "

                If i <> check_count_m - 1 Then
                    sql &= sqlstr(i) & " UNION "
                Else
                    sql &= sqlstr(i) & ";"
                End If
            Else
                '跨月
                Dim t1 As String = DateAdd("d", -1, begin_time).ToString("yyyy/MM/dd")
                Dim t2 As String = DateAdd("d", -1, DateAdd("m", 1, begin_time.Substring(0, 7) & "/01")).ToString("yyyy/MM/dd")

                sqlstr(i) = "SELECT MeterSetup.InstallPosition AS SelectItem, " & value & " AS Value into #T" & (i * 3) + 1 & " " & _
                "FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
                "WHERE PRC.CtrlNr = " & ctrlnr(i) & " AND PRC.MeterID = " & meterid(i) & " AND PRC.CtrlNr = MeterSetup.CtrlNr AND " & _
                "PRC.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & Find_AdAccount(group) & "' AND " & _
                "ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND PRC.RecDate = '" & t1 & "';" & _
                "SELECT MeterSetup.InstallPosition AS SelectItem, " & value & " AS Value into #T" & (i * 3) + 2 & " " & _
                "FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
                "WHERE PRC.CtrlNr = " & ctrlnr(i) & " AND PRC.MeterID = " & meterid(i) & " AND PRC.CtrlNr = MeterSetup.CtrlNr AND " & _
                "PRC.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & Find_AdAccount(group) & "' AND " & _
                "ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND PRC.RecDate = '" & t2 & "';" & _
                "SELECT MeterSetup.InstallPosition AS SelectItem, " & value & " AS Value into #T" & (i * 3) + 3 & " " & _
                "FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
                "WHERE PRC.CtrlNr = " & ctrlnr(i) & " AND PRC.MeterID = " & meterid(i) & " AND PRC.CtrlNr = MeterSetup.CtrlNr AND " & _
                "PRC.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & Find_AdAccount(group) & "' AND " & _
                "ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND PRC.RecDate = '" & end_time & "';"

                sqlstr2(i) = "SELECT #T" & (i * 3) + 1 & ".SelectItem,(#T" & (i * 3) + 2 & ".Value - #T" & (i * 3) + 1 & ".Value + #T" & (i * 3) + 3 & ".Value) AS Value " & _
                "FROM #T" & (i * 3) + 1 & ",#T" & (i * 3) + 2 & ",#T" & (i * 3) + 3 & ""

                sqlstr3(i) = "DROP Table #T" & (i * 3) + 1 & ",#T" & (i * 3) + 2 & ",#T" & (i * 3) + 3 & ";"

                If i <> check_count_m - 1 Then
                    sql &= sqlstr(i)
                    sql2 &= sqlstr2(i) & " UNION "
                    sql3 &= sqlstr3(i)
                Else
                    sql &= sqlstr(i)
                    sql2 &= sqlstr2(i) & ";"
                    sql3 &= sqlstr3(i)
                    sql = sql & sql2 & sql3
                End If
            End If
        Next

        Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            Dim dt As DataTable = dv.ToTable()

            'Dim da As OleDbDataAdapter = New OleDbDataAdapter(sql, strcon)
            'Dim dt As DataTable = New DataTable
            'da.Fill(dt)

            If dt.Rows.Count > 0 Then
                '有選擇總表
                If node2 <> "" Then
                    analysis_panel.Visible = True
                    '取得總表ECO5、電表編號
                    Dim title_s() As String = node2.Split(",")
                    Dim check_count_cs, check_count_ms As Integer
                    Dim ctrlnr_s((title_s.Length / 4) - 1) As String
                    Dim meterid_s((title_s.Length / 4) - 1) As String

                    For i = 0 To title_s.Length - 1
                        If title_s(i).Split(":").GetValue(0) = "ECO5編號" Then
                            ctrlnr_s(check_count_cs) = title_s(i).Split(":").GetValue(1)
                            check_count_cs += 1
                        ElseIf title_s(i).Split(":").GetValue(0) = "電表編號" Then
                            meterid_s(check_count_ms) = title_s(i).Split(":").GetValue(1)
                            check_count_ms += 1
                        End If
                    Next

                    '組合總表SQL字串
                    Dim sqlstr_S(check_count_ms - 1) As String
                    Dim sqlstr_S2(check_count_ms - 1) As String
                    Dim sqlstr_S3(check_count_ms - 1) As String
                    Dim sql_S As String = ""
                    Dim sql_S2 As String = ""
                    Dim sql_S3 As String = ""

                    For i = 0 To check_count_ms - 1
                        If begin_time.Substring(0, 7) = end_time.Substring(0, 7) Then
                            sqlstr_S(i) = "SELECT MS.InstallPosition AS SelectItem, " & value & " AS Value FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup AS CS,ECOSMART.dbo.MeterSetup AS MS " & _
                            "WHERE PRC.CtrlNr = " & ctrlnr_s(i) & " AND PRC.MeterID = " & meterid_s(i) & " AND CS.ECO_Account = MS.ECO_Account AND " & _
                            "PRC.CtrlNr = MS.CtrlNr AND PRC.MeterID = MS.MeterID AND CS.Account='" & Find_AdAccount(group) & "' AND " & datetime & " "
                        Else
                            Dim t1 As String = DateAdd("d", -1, begin_time).ToString("yyyy/MM/dd")
                            Dim t2 As String = DateAdd("d", -1, DateAdd("m", 1, begin_time.Substring(0, 7) & "/01")).ToString("yyyy/MM/dd")

                            sqlstr_S(i) = "SELECT MeterSetup.InstallPosition AS SelectItem, " & value & " AS Value into #T" & (i * 3) + 1 & " " & _
                            "FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
                            "WHERE PRC.CtrlNr = " & ctrlnr_s(i) & " AND PRC.MeterID = " & meterid_s(i) & " AND PRC.CtrlNr = MeterSetup.CtrlNr AND " & _
                            "PRC.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & Find_AdAccount(group) & "' AND " & _
                            "ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND PRC.RecDate = '" & t1 & "';" & _
                            "SELECT MeterSetup.InstallPosition AS SelectItem, " & value & " AS Value into #T" & (i * 3) + 2 & " " & _
                            "FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
                            "WHERE PRC.CtrlNr = " & ctrlnr_s(i) & " AND PRC.MeterID = " & meterid_s(i) & " AND PRC.CtrlNr = MeterSetup.CtrlNr AND " & _
                            "PRC.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & Find_AdAccount(group) & "' AND " & _
                            "ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND PRC.RecDate = '" & t2 & "';" & _
                            "SELECT MeterSetup.InstallPosition AS SelectItem, " & value & " AS Value into #T" & (i * 3) + 3 & " " & _
                            "FROM  ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
                            "WHERE PRC.CtrlNr = " & ctrlnr_s(i) & " AND PRC.MeterID = " & meterid_s(i) & " AND PRC.CtrlNr = MeterSetup.CtrlNr AND " & _
                            "PRC.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & Find_AdAccount(group) & "' AND " & _
                            "ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND PRC.RecDate = '" & end_time & "';"

                            sqlstr_S2(i) = "SELECT #T" & (i * 3) + 1 & ".SelectItem,(#T" & (i * 3) + 2 & ".Value - #T" & (i * 3) + 1 & ".Value + #T" & (i * 3) + 3 & ".Value) AS Value " & _
                            "FROM #T" & (i * 3) + 1 & ",#T" & (i * 3) + 2 & ",#T" & (i * 3) + 3 & ""

                            sqlstr_S3(i) = "DROP Table #T" & (i * 3) + 1 & ",#T" & (i * 3) + 2 & ",#T" & (i * 3) + 3 & ";"
                        End If

                        If i <> check_count_ms - 1 Then
                            sql_S &= sqlstr_S(i)
                            sql_S2 &= sqlstr_S2(i) & " UNION "
                            sql_S3 &= sqlstr_S3(i)
                        Else
                            sql_S &= sqlstr_S(i)
                            sql_S2 &= sqlstr_S2(i) & ";"
                            sql_S3 &= sqlstr_S3(i)
                        End If
                    Next
                    sql_S = sql_S & sql_S2 & sql_S3

                    SqlDataSource2.ConnectionString = strcon
                    SqlDataSource2.ProviderName = "System.Data.OleDb"
                    SqlDataSource2.SelectCommand = sql_S
                    Dim dv_S As DataView = SqlDataSource2.Select(New DataSourceSelectArguments)
                    Dim dt_S As DataTable = dv_S.ToTable()

                    'Dim da_S As OleDbDataAdapter = New OleDbDataAdapter(sql_S, strcon)
                    'Dim dt_S As DataTable = New DataTable
                    'da_S.Fill(dt_S)

                    If dt_S.Rows.Count > 0 Then
                        Dim sum As Integer
                        For i = 0 To dt_S.Rows.Count - 1
                            sum = sum + dt_S.Rows(i).Item(1)
                        Next

                        Dim total As Integer
                        For i = 0 To dt.Rows.Count - 1
                            total = total + dt.Rows(i).Item(1)
                        Next

                        '取得差額
                        Dim dif As Integer = sum - total
                        Dim diff As Double = total / sum
                        Dim dif_P As String = String.Format("{0:P}", diff)
                        item.Text = select_txt

                        If Session("language") = "en" Then
                            efficiency.Text = "Use efficiency：" & dif_P
                            diftxt.Text = "Loss ratio：" & String.Format("{0:P}", 1 - diff)
                            diftxt_E.Text = "Power loss：" & dif & " KWH"
                        Else
                            efficiency.Text = "使用效率：" & dif_P
                            diftxt.Text = "損耗率：" & String.Format("{0:P}", 1 - diff)
                            diftxt_E.Text = "損耗電量：" & dif & " KWH"
                        End If
                        efficiency.Visible = True
                        diftxt.Visible = True
                        diftxt_E.Visible = True

                        Dim dt2 As DataTable = New DataTable
                        dt2.Columns.Add("SelectItem")
                        dt2.Columns.Add("Value")
                        For i = 0 To dt.Rows.Count - 1
                            dt2.Rows.Add(dt.Rows(i).Item(0), dt.Rows(i).Item(1))
                        Next
                        If Session("language") = "en" Then
                            dt2.Rows.Add("loss", dif)
                        Else
                            dt2.Rows.Add("損耗", dif)
                        End If

                        Chart_ES.DataSource = dt2
                        Chart_ES.DataBind()
                        If Session("language") = "en" Then
                            Chart_ES.Title.Text = "Multi-Meter Proportional Chart(" & begin_time & "~" & end_time & ")"
                        Else
                            Chart_ES.Title.Text = "多電表比重圖(" & begin_time & "~" & end_time & ")"
                        End If
                        Chart_ES.Visible = True
                        img_btn.Visible = True
                        Panel_Chart.Visible = True
                    Else
                        Chart_ES.Visible = False
                        img_btn.Visible = False
                        analysis_panel.Visible = False
                        efficiency.Visible = False
                        diftxt.Visible = False
                        diftxt_E.Visible = False
                        Panel_Chart.Visible = False

                        Dim msg As String = "查無資料"
                        If Session("language").ToString = "en" Then
                            msg = " No Data. "
                        End If
                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
                    End If
                Else
                    Chart_ES.DataSource = dt
                    Chart_ES.DataBind()
                    If Session("language") = "en" Then
                        Chart_ES.Title.Text = "Multi-Meter Proportional Chart(" & begin_time & "~" & end_time & ")"
                    Else
                        Chart_ES.Title.Text = "多電表比重圖(" & begin_time & "~" & end_time & ")"
                    End If
                    Chart_ES.Visible = True
                    img_btn.Visible = True
                    analysis_panel.Visible = False
                    efficiency.Visible = False
                    diftxt.Visible = False
                    diftxt_E.Visible = False
                    Panel_Chart.Visible = True
                End If
            Else
                Chart_ES.Visible = False
                img_btn.Visible = False
                analysis_panel.Visible = False
                efficiency.Visible = False
                diftxt.Visible = False
                diftxt_E.Visible = False
                Panel_Chart.Visible = False

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
