Imports System.Data.OleDb
Imports System.Data

Partial Class LineChart
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        Dim account As String = Request.QueryString("account")
        Dim ctrlnr As String = Request.QueryString("ctrlnr")
        Dim meterid As String = Request.QueryString("meterid")
        Dim position As String = Request.QueryString("position")
        Dim datetime1() As String = Request.QueryString("datetime1").Split(",")
        Dim datetime2() As String = Request.QueryString("datetime2").Split(",")
        Dim interval As String = Request.QueryString("interval")
        Dim item As String = Request.QueryString("item")
        Dim value As String = Request.QueryString("value")

        Dim strcon As String
        If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(account) & ""
        Else
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
        End If

        Dim start_time1 As String = datetime1.GetValue(0)
        Dim end_time1 As String = datetime1.GetValue(1)
        Dim start_time2 As String = datetime2.GetValue(0)
        Dim end_time2 As String = datetime2.GetValue(1)

        Dim sql1 As String = "select SUBSTRING(RecDate,12,5) as RecDate1," & value & " as Value1 from PowerRecord "
        '時間間隔
        If interval = "1" Then
            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' order by RecDate "
        ElseIf interval = "5" Then
            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
        ElseIf interval = "30" Then
            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
        ElseIf interval = "60" Then
            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' and substring(recdate,15,2) Like '%00%' order by RecDate "
        End If

        Dim sql2 As String = "select SUBSTRING(RecDate,12,5) as RecDate2," & value & " as Value12 from PowerRecord "
        '時間間隔
        If interval = "1" Then
            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' order by RecDate "
        ElseIf interval = "5" Then
            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
        ElseIf interval = "30" Then
            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
        ElseIf interval = "60" Then
            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' and substring(recdate,15,2) Like '%00%' order by RecDate "
        End If

        Dim da1 As OleDbDataAdapter = New OleDbDataAdapter(sql1, strcon)
        Dim dt1 As DataTable = New DataTable
        da1.Fill(dt1)
        'Dim key1 As DataColumn() = New DataColumn(0) {}
        'key1(0) = dt1.Columns("時段")
        'dt1.PrimaryKey = key1

        Dim da2 As OleDbDataAdapter = New OleDbDataAdapter(sql2, strcon)
        Dim dt2 As DataTable = New DataTable
        da2.Fill(dt2)
        'Dim key2 As DataColumn() = New DataColumn(0) {}
        'key2(0) = dt2.Columns("時段")
        'dt2.PrimaryKey = key2

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

            If item = "功率" Then
                Chart_W.Series(0).Title = start_time1.Split(" ").GetValue(0).Substring(5, 5)
                Chart_W.Series(1).Title = start_time2.Split(" ").GetValue(0).Substring(5, 5)
                Chart_W.Visible = True
                Chart_V.Visible = False
                Chart_I.Visible = False
                Chart_W.DataSource = dt
                Chart_W.DataBind()
            ElseIf item = "電壓" Then
                Chart_V.Series(0).Title = start_time1.Split(" ").GetValue(0).Substring(5, 5)
                Chart_V.Series(1).Title = start_time2.Split(" ").GetValue(0).Substring(5, 5)
                Chart_V.Visible = True
                Chart_W.Visible = False
                Chart_I.Visible = False
                Chart_V.DataSource = dt
                Chart_V.DataBind()
            ElseIf item = "電流" Then
                Chart_I.Series(0).Title = start_time1.Split(" ").GetValue(0).Substring(5, 5)
                Chart_I.Series(1).Title = start_time2.Split(" ").GetValue(0).Substring(5, 5)
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
