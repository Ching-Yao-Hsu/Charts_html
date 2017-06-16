Imports System.Data.OleDb
Imports System.Data

Partial Class ColumnChart
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
        Dim value As String = Request.QueryString("value")
        Dim title() As String = node.Split(",")
        Dim check_count_c, check_count_m As Integer
        Dim ctrlnr((title.Length / 4) - 1) As String
        Dim meterid((title.Length / 4) - 1) As String
        Dim datetime As String = "PRC.RecDate between '" & Request.QueryString("begin_time") & "' and '" & Request.QueryString("end_time") & "' "

        Dim sYearString As String = Left(Request.QueryString("begin_time"), 4)     '紀錄有沒有跨年  --0表沒有　　1表有
        If Left(Request.QueryString("begin_time"), 4) <> Left(Request.QueryString("end_time"), 4) Then
            sYearString = "," & Left(Request.QueryString("end_time"), 4)
        End If

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString()
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("MasterConn").ToString()

        For i = 0 To title.Length - 1
            If title(i).Split(":").GetValue(0) = "ECO5編號" Then
                ctrlnr(check_count_c) = title(i).Split(":").GetValue(1)
                check_count_c += 1
            ElseIf title(i).Split(":").GetValue(0) = "電表編號" Then
                meterid(check_count_m) = title(i).Split(":").GetValue(1)
                check_count_m += 1
            End If
        Next

        '組合SQL字串
        Dim sqlstr(check_count_m - 1) As String
        Dim sql As String = ""
        Dim ValueAvg, ValueMax As String
        Select Case value
            Case "Vavg"
                ValueAvg = "PRC.Vavg"
                ValueMax = "PRC.Vmax"
            Case "Iavg"
                ValueAvg = "PRC.Iavg"
                ValueMax = "PRC.Imax"
            Case "W"
                ValueAvg = "PRC.Wavg"
                ValueMax = "PRC.Wmax"
            Case Else
                ValueAvg = ""
                ValueMax = ""
        End Select

        Dim aYear() As String = Split(sYearString, ",")
        Dim strlocation, strAverage, strMaximum As String
        Dim strTitle As String = ""
        If Session("language") = "en" Then
            strlocation = "location" : strAverage = "Average" : strMaximum = "Maximum"
            strTitle = "Voltage benefit analysis chart"
        Else
            strlocation = "位置" : strAverage = "平均值" : strMaximum = "最大值"
            strTitle = "電壓效益分析圖"
        End If

        For j = 0 To UBound(aYear)
            For i = 0 To check_count_m - 1
                'sqlstr(i) = " SELECT MS.InstallPosition AS 位置, Round(AVG(" & ValueAvg & "),2) AS 平均值,MAX(" & ValueMax & ") AS 最大值 " & _
                '            " FROM ECO_" & Find_AdAccount(group) & "_" & aYear(j) & ".dbo.PowerRecordCollection AS PRC " & _
                '            "     ,ECOSMART.dbo.ControllerSetup AS CS " & _
                '            "     ,ECOSMART.dbo.MeterSetup AS MS " & _
                '            " WHERE PRC.CtrlNr = " & ctrlnr(i) & " AND PRC.MeterID = " & meterid(i) & " AND " & _
                '            " PRC.CtrlNr = MS.CtrlNr AND PRC.MeterID = MS.MeterID AND CS.Account='" & Find_AdAccount(group) & "' AND " & _
                '            " CS.ECO_Account = MS.ECO_Account AND " & datetime & " GROUP BY MS.InstallPosition "


                sqlstr(i) = " SELECT MS.InstallPosition AS " & strlocation & ", Round(AVG(" & ValueAvg & "),2) AS " & strAverage & ",MAX(" & ValueMax & ") AS " & strMaximum & " " & _
                            " FROM ECO_" & Find_AdAccount(group) & "_PowerRecordCollection AS PRC " & _
                            "     ,ECOSMART.dbo.ControllerSetup AS CS " & _
                            "     ,ECOSMART.dbo.MeterSetup AS MS " & _
                            " WHERE PRC.CtrlNr = " & ctrlnr(i) & " AND PRC.MeterID = " & meterid(i) & " AND " & _
                            " PRC.CtrlNr = MS.CtrlNr AND PRC.MeterID = MS.MeterID AND CS.Account='" & Find_AdAccount(group) & "' AND " & _
                            " CS.ECO_Account = MS.ECO_Account AND " & datetime & " GROUP BY MS.InstallPosition "

                If i <> check_count_m - 1 Then
                    sql &= sqlstr(i) & " UNION "
                Else
                    sql &= sqlstr(i)
                End If
            Next
        Next

        Try
            Dim begin_day As String = Request.QueryString("begin_time").Split(" ").GetValue(0)
            Dim end_day As String = Request.QueryString("end_time").Split(" ").GetValue(0)
            'Dim begin_hh As String = Request.QueryString("begin_time").Split(" ").GetValue(1)
            'Dim end_hh As String = Request.QueryString("end_time").Split(" ").GetValue(1)
            'begin_hh = CInt(begin_hh.Split(":").GetValue(0)) & "時"
            'end_hh = CInt(end_hh.Split(":").GetValue(0)) & "時"
            'Dim begin_time As String = begin_day & " " & begin_hh
            'Dim end_time As String = end_day & " " & end_hh

            Dim da As New OleDbDataAdapter(sql, strcon)
            Dim dt As New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                img_btn.Visible = True
                Select Case value
                    Case "Vavg"
                        PowerColumnChart_V.Title.Text = strTitle & "(" & begin_day & "~" & end_day & ")"
                        PowerColumnChart_V.Visible = True
                        PowerColumnChart_V.DataSource = dt
                        PowerColumnChart_V.DataBind()
                    Case "Iavg"
                        PowerColumnChart_I.Title.Text = strTitle & "(" & begin_day & "~" & end_day & ")"
                        PowerColumnChart_I.Visible = True
                        PowerColumnChart_I.DataSource = dt
                        PowerColumnChart_I.DataBind()
                    Case "W"
                        PowerColumnChart_W.Title.Text = strTitle & "(" & begin_day & "~" & end_day & ")"
                        PowerColumnChart_W.Visible = True
                        PowerColumnChart_W.DataSource = dt
                        PowerColumnChart_W.DataBind()
                End Select
            Else
                img_btn.Visible = False
                PowerColumnChart_V.Visible = False
                PowerColumnChart_I.Visible = False
                PowerColumnChart_W.Visible = False
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
