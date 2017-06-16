Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class PowerMonthReport
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then
                '初始化日期區間
                Date_Info.Text = Now.ToString("yyyy/MM")
            End If
            SqlQuery()
        End If

    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()
        SqlDataSource2.SelectParameters.Clear()

        Dim strcon As String = Nothing
        If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("AccountPower") & ""
        Else
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account") & ""
        End If

        Dim ctrlnr As String = Request.QueryString("ctrlnr")
        Dim meterid As String = Request.QueryString("meterid")
        If ctrlnr IsNot Nothing And meterid IsNot Nothing Then
            Session("ctrlnr") = Request.QueryString("ctrlnr")
            Session("meterid") = Request.QueryString("meterid")
            Session("position") = Request.QueryString("position")
        End If
        Date_txt.Text = "月份：" & Date_Info.Text
        Num_txt.Text = "編號：" & Session("ctrlnr") & "-" & Session("meterid")
        Position_txt.Text = "位置：" & Session("position")
        Dim datetime As String = Date_Info.Text
        Try
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Using cmd_sp As New OleDbCommand("Usp_Month_PowerRecord", conn)
                    cmd_sp.CommandType = CommandType.StoredProcedure

                    'SP的參數
                    Dim CtrlNrParam As New OleDbParameter("@CtrlNr", Session("ctrlnr"))
                    Dim MeterIDParam As New OleDbParameter("@MeterID", Session("meterid"))
                    Dim RecDateParam As New OleDbParameter("@RecDate", Date_Info.Text)
                    cmd_sp.Parameters.Add(CtrlNrParam)
                    cmd_sp.Parameters.Add(MeterIDParam)
                    cmd_sp.Parameters.Add(RecDateParam)

                    '設定SP的回傳參數和大小
                    Dim paramRet_iavg As OleDbParameter = cmd_sp.Parameters.Add("@IAvg", OleDbType.Single, 5)
                    paramRet_iavg.Direction = ParameterDirection.Output
                    Dim paramRet_imax As OleDbParameter = cmd_sp.Parameters.Add("@IMax", OleDbType.Single, 5)
                    paramRet_imax.Direction = ParameterDirection.Output
                    Dim paramRet_vavg As OleDbParameter = cmd_sp.Parameters.Add("@VAvg", OleDbType.Single, 5)
                    paramRet_vavg.Direction = ParameterDirection.Output
                    Dim paramRet_vmax As OleDbParameter = cmd_sp.Parameters.Add("@VMax", OleDbType.Single, 5)
                    paramRet_vmax.Direction = ParameterDirection.Output
                    Dim paramRet_wavg As OleDbParameter = cmd_sp.Parameters.Add("@WAvg", OleDbType.Single, 5)
                    paramRet_wavg.Direction = ParameterDirection.Output
                    Dim paramRet_wmax As OleDbParameter = cmd_sp.Parameters.Add("@WMax", OleDbType.Single, 5)
                    paramRet_wmax.Direction = ParameterDirection.Output
                    Dim paramRet_demand As OleDbParameter = cmd_sp.Parameters.Add("@Demand", OleDbType.Integer, 10)
                    paramRet_demand.Direction = ParameterDirection.Output
                    Dim paramRet_demandhalf As OleDbParameter = cmd_sp.Parameters.Add("@DemandHalf", OleDbType.Integer, 10)
                    paramRet_demandhalf.Direction = ParameterDirection.Output
                    Dim paramRet_demandsathalf As OleDbParameter = cmd_sp.Parameters.Add("@DemandSatHalf", OleDbType.Integer, 10)
                    paramRet_demandsathalf.Direction = ParameterDirection.Output
                    Dim paramRet_demandoff As OleDbParameter = cmd_sp.Parameters.Add("@DemandOff", OleDbType.Integer, 10)
                    paramRet_demandoff.Direction = ParameterDirection.Output
                    Dim paramRet_rushhour As OleDbParameter = cmd_sp.Parameters.Add("@RushHour", OleDbType.Integer, 10)
                    paramRet_rushhour.Direction = ParameterDirection.Output
                    Dim paramRet_halfhour As OleDbParameter = cmd_sp.Parameters.Add("@HalfHour", OleDbType.Integer, 10)
                    paramRet_halfhour.Direction = ParameterDirection.Output
                    Dim paramRet_sathalfhour As OleDbParameter = cmd_sp.Parameters.Add("@SatHalfHour", OleDbType.Integer, 10)
                    paramRet_sathalfhour.Direction = ParameterDirection.Output
                    Dim paramRet_offhour As OleDbParameter = cmd_sp.Parameters.Add("@OffHour", OleDbType.Integer, 10)
                    paramRet_offhour.Direction = ParameterDirection.Output
                    Dim paramRet_imaxrd As OleDbParameter = cmd_sp.Parameters.Add("@ImaxRD", OleDbType.VarChar, 20)
                    paramRet_imaxrd.Direction = ParameterDirection.Output
                    Dim paramRet_vmaxrd As OleDbParameter = cmd_sp.Parameters.Add("@VmaxRD", OleDbType.VarChar, 20)
                    paramRet_vmaxrd.Direction = ParameterDirection.Output
                    Dim paramRet_wmaxrd As OleDbParameter = cmd_sp.Parameters.Add("@WmaxRD", OleDbType.VarChar, 20)
                    paramRet_wmaxrd.Direction = ParameterDirection.Output
                    cmd_sp.ExecuteNonQuery()

                    '取得回傳的參數
                    avgI.Text = String.Format("{0:N}", paramRet_iavg.Value) & "　"
                    maxI.Text = String.Format("{0:N}", paramRet_imax.Value) & "　"
                    avgV.Text = String.Format("{0:N}", paramRet_vavg.Value) & "　"
                    maxV.Text = String.Format("{0:N}", paramRet_vmax.Value) & "　"
                    avgW.Text = String.Format("{0:N}", paramRet_wavg.Value) & "　"
                    maxW.Text = String.Format("{0:N}", paramRet_wmax.Value) & "　"
                    Demand.Text = CInt(paramRet_demand.Value).ToString("#,0") & "　"
                    HalfDemand.Text = CInt(paramRet_demandhalf.Value).ToString("#,0") & "　"
                    SatHalfDemand.Text = CInt(paramRet_demandsathalf.Value).ToString("#,0") & "　"
                    OffDemand.Text = CInt(paramRet_demandoff.Value).ToString("#,0") & "　"
                    Rush.Text = CInt(paramRet_rushhour.Value).ToString("#,0") & "　"
                    Half.Text = CInt(paramRet_halfhour.Value).ToString("#,0") & "　"
                    SatHalf.Text = CInt(paramRet_sathalfhour.Value).ToString("#,0") & "　"
                    Off.Text = CInt(paramRet_offhour.Value).ToString("#,0") & "　"
                    Sum.Text = (CInt(paramRet_rushhour.Value) + CInt(paramRet_halfhour.Value) + CInt(paramRet_sathalfhour.Value) + CInt(paramRet_offhour.Value)).ToString("#,0") & "　"
                    maxI.ToolTip = paramRet_imaxrd.Value
                    maxV.ToolTip = paramRet_vmaxrd.Value
                    maxW.ToolTip = paramRet_wmaxrd.Value
                End Using

                '電流/電壓/功率-平均值/最大值
                'Dim sql_W As String = "SELECT Round(AVG(Iavg),2) AS 電流平均值,MAX(Imax) AS 電流最大值,Round(AVG(Vavg),2) AS 電壓平均值,MAX(Vmax) AS 電壓最大值," & _
                '"Round(AVG(Wavg),2) AS 功率平均值,MAX(Wmax) AS 功率最大值,MAX(ImaxRD) AS 電流最大時間,MAX(VmaxRD) AS 電壓最大時間,MAX(WmaxRD) AS 功率最大時間 FROM PowerRecordCollection " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & datetime & "'"
                'Using cmd As New OleDbCommand(sql_W, conn)
                '    Using dr As OleDbDataReader = cmd.ExecuteReader()
                '        If dr.Read() Then
                '            avgI.Text = String.Format("{0:N}", dr("電流平均值")) & "　"
                '            maxI.Text = String.Format("{0:N}", dr("電流最大值")) & "　"
                '            maxV.Text = String.Format("{0:N}", dr("電壓最大值")) & "　"
                '            avgV.Text = String.Format("{0:N}", dr("電壓平均值")) & "　"
                '            avgW.Text = String.Format("{0:N}", dr("功率平均值")) & "　"
                '            maxW.Text = String.Format("{0:N}", dr("功率最大值")) & "　"
                '            'maxI.ToolTip = dr("電流最大時間").ToString
                '            'maxV.ToolTip = dr("電壓最大時間").ToString
                '            'maxW.ToolTip = dr("功率最大時間").ToString
                '        End If
                '    End Using
                'End Using
                'Dim ImaxRD, VmaxRD, WmaxRD As String
                'Dim sql_data As String = "SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax," & _
                '"AVG(W) AS Wavg,MAX(W) AS Wmax into #T1 FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & Date_Info.Text & "';" & _
                '"SELECT RecDate AS 最大電流時間 into #T2 FROM PowerRecord,#T1 " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & Date_Info.Text & "' AND  PowerRecord.Iavg = #T1.Imax;" & _
                '"SELECT RecDate AS 最大電壓時間 into #T3 FROM PowerRecord,#T1 " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & Date_Info.Text & "' AND  PowerRecord.Vavg = #T1.Vmax;" & _
                '"SELECT RecDate AS 最大功率時間 into #T4 FROM PowerRecord,#T1 " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & Date_Info.Text & "' AND  PowerRecord.W = #T1.Wmax;" & _
                '"SELECT TOP 1 Round(#T1.Iavg,2) AS 電流平均值,Round(#T1.Imax,2) AS 電流最大值," & _
                '"Round(#T1.Vavg,2) AS 電壓平均值,Round(#T1.Vmax,2) AS 電壓最大值," & _
                '"Round(#T1.Wavg,2) AS 功率平均值,Round(#T1.Wmax,2) AS 功率最大值," & _
                '"#T2.*,#T3.*,#T4.* FROM #T1,#T2,#T3,#T4 " & _
                '"ORDER BY 最大電流時間 DESC,最大電壓時間 DESC,最大功率時間 DESC;drop table #T1,#T2,#T3,#T4"

                'Using cmd_data As New OleDbCommand(sql_data, conn)
                '    Using dr_data As OleDbDataReader = cmd_data.ExecuteReader
                '        If dr_data.Read() Then
                '            avgI.Text = String.Format("{0:N}", dr_data("電流平均值")) & "　"
                '            maxI.Text = String.Format("{0:N}", dr_data("電流最大值")) & "　"
                '            maxV.Text = String.Format("{0:N}", dr_data("電壓最大值")) & "　"
                '            avgV.Text = String.Format("{0:N}", dr_data("電壓平均值")) & "　"
                '            avgW.Text = String.Format("{0:N}", dr_data("功率平均值")) & "　"
                '            maxW.Text = String.Format("{0:N}", dr_data("功率最大值")) & "　"
                '            maxI.ToolTip = dr_data("最大電流時間").ToString
                '            maxV.ToolTip = dr_data("最大電壓時間").ToString
                '            maxW.ToolTip = dr_data("最大功率時間").ToString
                '            'ImaxRD = dr_data("最大電流時間").ToString
                '            'VmaxRD = dr_data("最大電壓時間").ToString
                '            'WmaxRD = dr_data("最大功率時間").ToString
                '        End If
                '    End Using
                'End Using

                '最大需量
                'Dim sql_de As String = "SELECT MAX(Demand) AS Demand,MAX(DemandHalf) AS DemandHalf,MAX(DemandSatHalf) AS DemandSatHalf,MAX(DemandOff) AS DemandOff FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND SUBSTRING(Replace(Convert(nvarchar(10),RecDate,120),'-','/'),1,7) = '" & datetime & "'"
                'Using cmd As New OleDbCommand(sql_de, conn)
                '    Using dr As OleDbDataReader = cmd.ExecuteReader()
                '        If dr.Read() Then
                '            Demand.Text = CInt(dr("Demand")).ToString("#,0") & "　"
                '            HalfDemand.Text = CInt(dr("DemandHalf")).ToString("#,0") & "　"
                '            SatHalfDemand.Text = CInt(dr("DemandSatHalf")).ToString("#,0") & "　"
                '            OffDemand.Text = CInt(dr("DemandOff")).ToString("#,0") & "　"
                '        End If
                '    End Using
                'End Using

                '用電量
                'Dim sql_KWH As String = "SELECT isnull(SUM(RushHour),0) AS 尖峰,isnull(SUM(HalfHour),0) AS 半尖峰,isnull(SUM(SatHalfHour),0) AS 週六半尖峰,isnull(SUM(OffHour),0) AS 離峰 FROM PowerRecordCollection " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & datetime & "'"
                'Using cmd As New OleDbCommand(sql_KWH, conn)
                '    Using dr As OleDbDataReader = cmd.ExecuteReader()
                '        If dr.Read() Then
                '            Rush.Text = dr("尖峰") & "　"
                '            Half.Text = dr("半尖峰") & "　"
                '            SatHalf.Text = dr("週六半尖峰") & "　"
                '            Off.Text = dr("離峰") & "　"
                '            Sum.Text = (dr("尖峰") + dr("半尖峰") + dr("週六半尖峰") + dr("離峰")) & "　"
                '        End If
                '    End Using
                'End Using
                'Dim sql As String = "SELECT SUBSTRING(RecDate,1,10) AS RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER " & _
                '"(Partition By SUBSTRING(RecDate,1,10) order by recdate desc) as Sort into #T3 " & _
                '"FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,7) = '" & datetime & "';" & _
                '"SELECT SUBSTRING(RecDate,1,10) AS RecDate,RushHour, HalfHour, SatHalfHour, OffHour into #T2 FROM #T3 Record WHERE Record.sort = 1;" & _
                '"SELECT RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER " & _
                '"(Partition By SUBSTRING(RecDate,1,10) order by recdate desc) as Sort into #T1 " & _
                '"FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,7) = '" & datetime & "';" & _
                '"SELECT " & _
                '"(CASE WHEN SUBSTRING(SUBSTRING(#T1.RecDate,1,10),9,10)='01' THEN isnull(#T1.RushHour,0) ELSE (isnull(#T1.RushHour,0) - isnull(#T2.RushHour,0)) END) AS 尖峰," & _
                '"(CASE WHEN SUBSTRING(SUBSTRING(#T1.RecDate,1,10),9,10)='01' THEN isnull(#T1.HalfHour,0) ELSE (isnull(#T1.HalfHour,0) - isnull(#T2.HalfHour,0)) END) AS 半尖峰, " & _
                '"(CASE WHEN SUBSTRING(SUBSTRING(#T1.RecDate,1,10),9,10)='01' THEN isnull(#T1.SatHalfHour,0) ELSE (isnull(#T1.SatHalfHour,0) - isnull(#T2.SatHalfHour,0)) END) AS 週六半尖峰," & _
                '"(CASE WHEN SUBSTRING(SUBSTRING(#T1.RecDate,1,10),9,10)='01' THEN isnull(#T1.OffHour,0) ELSE (isnull(#T1.OffHour,0) - isnull(#T2.OffHour,0)) END) AS 離峰 into #T4 " & _
                '"FROM #T1 left JOIN #T2 ON convert(char(10),dateadd(day,-1,#T1.RecDate),111) = #T2.RecDate WHERE #T1.Sort = 1 ORDER BY #T1.RecDate;" & _
                '"SELECT isnull(SUM(#T4.尖峰),0) AS 尖峰,isnull(SUM(#T4.半尖峰),0) AS 半尖峰,isnull(SUM(#T4.週六半尖峰),0) AS 週六半尖峰,isnull(SUM(#T4.離峰),0) AS 離峰 FROM #T4;" & _
                '"drop table #T1;drop table #T2;drop table #T3;drop table #T4;"

                'Using cmd As New OleDbCommand(sql, conn)
                '    Using dr As OleDbDataReader = cmd.ExecuteReader()
                '        If dr.Read() Then
                '            Rush.Text = dr("尖峰") & "　"
                '            Half.Text = dr("半尖峰") & "　"
                '            SatHalf.Text = dr("週六半尖峰") & "　"
                '            Off.Text = dr("離峰") & "　"
                '            Sum.Text = (dr("尖峰") + dr("半尖峰") + dr("週六半尖峰") + dr("離峰")) & "　"
                '        End If
                '    End Using
                'End Using
                Using cmd_sp As New OleDbCommand("Usp_Month_PowerRecord_W_Detail", conn)
                    cmd_sp.CommandType = CommandType.StoredProcedure
                    Dim CtrlNrParam As New OleDbParameter("@CtrlNr", Session("ctrlnr"))
                    Dim MeterIDParam As New OleDbParameter("@MeterID", Session("meterid"))
                    Dim RecDateParam As New OleDbParameter("@RecDate", Date_Info.Text)
                    cmd_sp.Parameters.Add(CtrlNrParam)
                    cmd_sp.Parameters.Add(MeterIDParam)
                    cmd_sp.Parameters.Add(RecDateParam)
                    Dim da As New OleDbDataAdapter
                    da.SelectCommand = cmd_sp
                    Dim dt As New DataTable
                    da.Fill(dt)
                    GridView1.DataSource = dt
                    GridView1.DataBind()

                    If dt.Rows.Count > 0 Then
                        Panel_Report.Visible = True
                        msg.Visible = False
                        Panel_Export.Visible = True
                    Else
                        Panel_Report.Visible = False
                        msg.Visible = True
                        Panel_Export.Visible = False
                    End If
                End Using

                Using cmd_sp As New OleDbCommand("Usp_Month_PowerRecord_E_Detail", conn)
                    cmd_sp.CommandType = CommandType.StoredProcedure
                    Dim CtrlNrParam As New OleDbParameter("@CtrlNr", Session("ctrlnr"))
                    Dim MeterIDParam As New OleDbParameter("@MeterID", Session("meterid"))
                    Dim RecDateParam As New OleDbParameter("@RecDate", Date_Info.Text)
                    cmd_sp.Parameters.Add(CtrlNrParam)
                    cmd_sp.Parameters.Add(MeterIDParam)
                    cmd_sp.Parameters.Add(RecDateParam)
                    Dim da As New OleDbDataAdapter
                    da.SelectCommand = cmd_sp
                    Dim dt As New DataTable
                    da.Fill(dt)
                    GridView2.DataSource = dt
                    GridView2.DataBind()

                    If dt.Rows.Count > 0 Then
                        Panel_Report.Visible = True
                        msg.Visible = False
                        Panel_Export.Visible = True
                    Else
                        Panel_Report.Visible = False
                        msg.Visible = True
                        Panel_Export.Visible = False
                    End If
                End Using
            End Using

            'SqlDataSource1.ConnectionString = strcon
            'SqlDataSource1.ProviderName = "System.Data.OleDb"
            ''SqlDataSource1.SelectCommand = "SELECT SUBSTRING(RecDate,6,10) AS 日期,Round(AVG(Wavg),2) AS 平均值,Round(MAX(Wmax),2) AS 最大值 FROM PowerRecordCollection " & _
            ''"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & datetime & "' GROUP BY SUBSTRING(RecDate,6,10) order by 日期"
            'SqlDataSource1.SelectCommand = "SELECT SUBSTRING(RecDate,6,5) AS 日期,Round(AVG(W),2) AS 功率平均值,Round(MAX(W),2) AS 功率最大值 FROM PowerRecord " & _
            '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND SUBSTRING(RecDate,1,7) = '" & datetime & "' " & _
            '"GROUP BY SUBSTRING(RecDate,6,5) ORDER BY 日期"

            'SqlDataSource2.ConnectionString = strcon
            'SqlDataSource2.ProviderName = "System.Data.OleDb"
            ''SqlDataSource2.SelectCommand = "SELECT RushHour AS 尖峰,HalfHour AS 半尖峰,SatHalfHour AS 週六半尖峰,OffHour AS 離峰 FROM PowerRecordCollection " & _
            ''"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,7) = '" & datetime & "' order by Recdate"
            'SqlDataSource2.SelectCommand = "SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER  " & _
            '"(Partition By SUBSTRING(RecDate,1,10) order by recdate desc) as Sort  " & _
            '"into #T1 FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND SUBSTRING(RecDate,1,7) = '" & datetime & "'; " & _
            '"SELECT RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER  " & _
            '"(Partition By SUBSTRING(RecDate,1,10) order by recdate) as Sort  " & _
            '"into #T3 FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND SUBSTRING(RecDate,1,7) = '" & datetime & "'; " & _
            '"SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour  " & _
            '"into #T2 FROM #T1 WHERE #T1.sort = 1 order by RecDate; " & _
            '"SELECT SUBSTRING(RecDate,1,10) AS RecDate, RushHour, HalfHour, SatHalfHour, OffHour  " & _
            '"into #T4 FROM #T3 WHERE #T3.sort = 1 order by RecDate; " & _
            '"SELECT SUBSTRING(#T1.RecDate,1,10) AS RecDate, " & _
            '"(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.RushHour,0)  " & _
            '"ELSE (isnull(#T1.RushHour,0) - (CASE WHEN EXISTS (SELECT #T2.RushHour FROM #T2)  " & _
            '"THEN isnull(#T2.RushHour,#T4.RushHour) ELSE (#T4.RushHour) END)) END) AS 尖峰, " & _
            '"(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.HalfHour,0)  " & _
            '"ELSE (isnull(#T1.HalfHour,0) - (CASE WHEN EXISTS (SELECT HalfHour FROM #T2)  " & _
            '"THEN isnull(#T2.HalfHour,#T4.HalfHour) ELSE (#T4.HalfHour) END)) END) AS 半尖峰, " & _
            '"(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.SatHalfHour,0)  " & _
            '"ELSE (isnull(#T1.SatHalfHour,0) - (CASE WHEN EXISTS (SELECT #T2.SatHalfHour FROM #T2)  " & _
            '"THEN isnull(#T2.SatHalfHour,#T4.SatHalfHour) ELSE (#T4.SatHalfHour) END)) END) AS 週六半尖峰, " & _
            '"(CASE WHEN SUBSTRING(#T1.RecDate,9,10)='01' THEN isnull(#T1.OffHour,0)  " & _
            '"ELSE (isnull(#T1.OffHour,0) - (CASE WHEN EXISTS (SELECT #T2.OffHour FROM #T2)  " & _
            '"THEN isnull(#T2.OffHour,#T4.OffHour) ELSE (#T4.OffHour) END)) END) AS 離峰 into #T5  " & _
            '"FROM #T1 left JOIN #T2 ON convert(char(10),dateadd(day,-1,#T1.RecDate),111) = #T2.RecDate,#T4 " & _
            '"WHERE #T1.Sort = 1 and #T1.RecDate = #T4.RecDate ORDER BY #T1.RecDate; " & _
            '"select *,(尖峰+半尖峰+週六半尖峰+離峰) AS 總計 from #T5; " & _
            '"drop table #T1,#T2,#T3,#T4, #T5;"

            'Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            'If dv.Count > 0 Then
            '    'Dim sql As String = "SELECT SUBSTRING(RecDate,6,5) AS 日期,Round(AVG(Iavg),2) AS 電流平均值,Round(MAX(Iavg),2) AS 電流最大值," & _
            '    '"Round(AVG(Vavg),2) AS 電壓平均值,Round(MAX(Vavg),2) AS 電壓最大值 FROM PowerRecord " & _
            '    '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND SUBSTRING(RecDate,1,7) = '" & datetime & "' " & _
            '    '"GROUP BY SUBSTRING(RecDate,6,5) ORDER BY 日期"
            '    'Dim dt As New DataTable
            '    'Dim da As New OleDbDataAdapter(sql, strcon)
            '    'da.Fill(dt)

            '    'Dim dt1 As New DataTable
            '    'dt1 = DirectCast(SqlDataSource1.Select(DataSourceSelectArguments.Empty), DataView).ToTable()
            '    'avgI.Text = String.Format("{0:N}", dt.Compute("avg(電流平均值)", "")) & "　"
            '    'maxI.Text = String.Format("{0:N}", dt.Compute("max(電流最大值)", "")) & "　"
            '    'avgV.Text = String.Format("{0:N}", dt.Compute("avg(電壓平均值)", "")) & "　"
            '    'maxV.Text = String.Format("{0:N}", dt.Compute("max(電壓最大值)", "")) & "　"
            '    'avgW.Text = String.Format("{0:N}", dt1.Compute("avg(功率平均值)", "")) & "　"
            '    'maxW.Text = String.Format("{0:N}", dt1.Compute("max(功率最大值)", "")) & "　"

            '    'Dim dt2 As New DataTable
            '    'dt2 = DirectCast(SqlDataSource2.Select(DataSourceSelectArguments.Empty), DataView).ToTable()
            '    'Rush.Text = CInt(dt2.Compute("sum(尖峰)", "")).ToString("#,0") & "　"
            '    'Half.Text = CInt(dt2.Compute("sum(半尖峰)", "")).ToString("#,0") & "　"
            '    'SatHalf.Text = CInt(dt2.Compute("sum(週六半尖峰)", "")).ToString("#,0") & "　"
            '    'Off.Text = CInt(dt2.Compute("sum(離峰)", "")).ToString("#,0") & "　"
            '    'Sum.Text = CInt(dt2.Compute("sum(總計)", "")).ToString("#,0") & "　"
            '    Panel_Report.Visible = True
            '    msg.Visible = False
            '    Panel_Export.Visible = True
            'Else
            '    Panel_Report.Visible = False
            '    msg.Visible = True
            '    Panel_Export.Visible = False
            'End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        e.Row.Cells(0).Attributes.Add("class", "text")
    End Sub

    Protected Sub GridView2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(4).Text = CInt(e.Row.Cells(0).Text) + CInt(e.Row.Cells(1).Text) + CInt(e.Row.Cells(2).Text) + CInt(e.Row.Cells(3).Text)
        End If
    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        Panel_Report.Visible = True
        SqlQuery()
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        SqlQuery()
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub

    Protected Sub Excel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Excel.Click
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("月報表_" & Now.Year & "_" & Now.Month)

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"
        GridView1.DataBind()

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.Flush()
        Response.End()
        'GridView1.DataBind()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles PDF.Click
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("月報表_" & Now.Year & "_" & Now.Month)

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.AddHeader("content-disposition", "attachment;filename=" + sFilename + ".pdf")  '檔名
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim stringWrite As System.IO.StringWriter = New StringWriter
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Dim reader As StringReader = New StringReader(stringWrite.ToString())
        Dim doc As Document = New Document(PageSize.A4)
        PdfWriter.GetInstance(doc, Response.OutputStream)
        '處理中文
        Dim bfCh2 As BaseFont = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\MINGLIU.TTC,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
        Dim fontchinese As Font = New Font(bfCh2)
        doc.Open()

        '要定義 PDF Table 的大小（Column 數量）
        Dim CountColumns As Integer = 8   '自動生成
        Dim CountRow As Integer = 4
        Dim pdf As PdfPTable = New PdfPTable(CountColumns)

        pdf.HeaderRows = 1
        pdf.WidthPercentage = 100  '表格寬度百分比
        pdf.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '字型
        Dim title_font As Font = New Font(bfCh2, 14, Font.BOLD)
        Dim detail_font As Font = New Font(bfCh2, 16)
        Dim unit_font As Font = New Font(bfCh2, 12)
        Dim sum_font As Font = New Font(bfCh2, 14, Font.BOLD, BaseColor.RED)
        Dim head_font As Font = New Font(bfCh2, 12, Font.BOLD)

        '表頭說明文字
        'Dim Header As PdfPCell = New PdfPCell(New Phrase("台灣國際造船股份有限公司 - 基隆廠", title_font))
        'Header.Colspan = CountColumns
        'Header.Border = 0
        'Header.HorizontalAlignment = 1
        'pdf.AddCell(Header)

        Dim Header2 As PdfPCell = New PdfPCell(New Phrase("月報表", title_font))
        Header2.Colspan = CountColumns
        Header2.Border = 0
        Header2.HorizontalAlignment = 1
        pdf.AddCell(Header2)

        '空白行
        Dim space1 As PdfPCell = New PdfPCell(New Phrase(" "))
        space1.Colspan = CountColumns
        space1.Border = 0
        pdf.AddCell(space1)

        '資訊
        Dim pdf_Info As PdfPTable = New PdfPTable(4)

        Dim datetxt As PdfPCell = New PdfPCell(New Phrase(Date_txt.Text & "", unit_font))
        datetxt.Colspan = 4
        datetxt.Border = 0
        datetxt.PaddingLeft = 30.0F
        pdf_Info.AddCell(datetxt)

        Dim ctrlnr As PdfPCell = New PdfPCell(New Phrase(Num_txt.Text & "", unit_font))
        ctrlnr.Colspan = 4
        ctrlnr.Border = 0
        ctrlnr.PaddingLeft = 30.0F
        pdf_Info.AddCell(ctrlnr)

        Dim positiontxt As PdfPCell = New PdfPCell(New Phrase(Position_txt.Text & "", unit_font))
        positiontxt.PaddingBottom = 5.0F
        positiontxt.Colspan = 4
        positiontxt.Border = 0
        positiontxt.PaddingLeft = 30.0F
        pdf_Info.AddCell(positiontxt)

        Dim left1 As PdfPCell = New PdfPCell(pdf_Info)
        left1.Colspan = 4
        left1.Rowspan = 3
        left1.Border = 0
        pdf.AddCell(left1)

        '最大需量
        Dim pdf_Demand As PdfPTable = New PdfPTable(4)

        Dim Demandtitle As PdfPCell = New PdfPCell(New Phrase("最大需量", head_font))
        Demandtitle.PaddingBottom = 5.0F
        Demandtitle.Colspan = 4
        Demandtitle.HorizontalAlignment = 1
        pdf_Demand.AddCell(Demandtitle)

        Dim Demandtxt As PdfPCell = New PdfPCell(New Phrase("尖峰", unit_font))
        Demandtxt.Colspan = 1
        Demandtxt.HorizontalAlignment = 0
        Demandtxt.PaddingBottom = 5.0F
        Demandtxt.PaddingLeft = 20.0F
        Demandtxt.Border = 0
        pdf_Demand.AddCell(Demandtxt)
        Dim Demandmax As PdfPCell = New PdfPCell(New Phrase("" & Demand.Text & "kw ", title_font))
        Demandmax.Colspan = 3
        Demandmax.HorizontalAlignment = 2
        Demandmax.PaddingBottom = 5.0F
        Demandmax.PaddingRight = 20.0F
        Demandmax.Border = 0
        pdf_Demand.AddCell(Demandmax)

        Dim HalfDemandtxt As PdfPCell = New PdfPCell(New Phrase("半尖峰", unit_font))
        HalfDemandtxt.Colspan = 1
        HalfDemandtxt.HorizontalAlignment = 0
        HalfDemandtxt.PaddingBottom = 5.0F
        HalfDemandtxt.PaddingLeft = 20.0F
        HalfDemandtxt.Border = 0
        pdf_Demand.AddCell(HalfDemandtxt)
        Dim HalfDemandmax As PdfPCell = New PdfPCell(New Phrase("" & HalfDemand.Text & "kw ", title_font))
        HalfDemandmax.Colspan = 3
        HalfDemandmax.HorizontalAlignment = 2
        HalfDemandmax.PaddingBottom = 5.0F
        HalfDemandmax.PaddingRight = 20.0F
        HalfDemandmax.Border = 0
        pdf_Demand.AddCell(HalfDemandmax)

        Dim SatHalfDemandtxt As PdfPCell = New PdfPCell(New Phrase("週六半尖峰", unit_font))
        SatHalfDemandtxt.Colspan = 1
        SatHalfDemandtxt.HorizontalAlignment = 0
        SatHalfDemandtxt.PaddingBottom = 5.0F
        SatHalfDemandtxt.PaddingLeft = 20.0F
        SatHalfDemandtxt.Border = 0
        SatHalfDemandtxt.NoWrap = True
        pdf_Demand.AddCell(SatHalfDemandtxt)
        Dim SatHalfDemandmax As PdfPCell = New PdfPCell(New Phrase("" & SatHalfDemand.Text & "kw ", title_font))
        SatHalfDemandmax.Colspan = 3
        SatHalfDemandmax.HorizontalAlignment = 2
        SatHalfDemandmax.PaddingBottom = 5.0F
        SatHalfDemandmax.PaddingRight = 20.0F
        SatHalfDemandmax.Border = 0
        pdf_Demand.AddCell(SatHalfDemandmax)

        Dim OffDemandtxt As PdfPCell = New PdfPCell(New Phrase("離峰", unit_font))
        OffDemandtxt.Colspan = 1
        OffDemandtxt.HorizontalAlignment = 0
        'OffDemandtxt.PaddingBottom = 2.0F
        OffDemandtxt.PaddingLeft = 20.0F
        OffDemandtxt.Border = 0
        pdf_Demand.AddCell(OffDemandtxt)
        Dim OffDemandmax As PdfPCell = New PdfPCell(New Phrase("" & OffDemand.Text & "kw ", title_font))
        OffDemandmax.Colspan = 3
        OffDemandmax.HorizontalAlignment = 2
        'OffDemandmax.PaddingBottom = 2.0F
        OffDemandmax.PaddingRight = 20.0F
        OffDemandmax.Border = 0
        pdf_Demand.AddCell(OffDemandmax)

        Dim left5 As PdfPCell = New PdfPCell(pdf_Demand)
        left5.Colspan = 4
        left5.Rowspan = 6
        pdf.AddCell(left5)

        '功率
        Dim pdf_W As PdfPTable = New PdfPTable(4)

        Dim Wtxt As PdfPCell = New PdfPCell(New Phrase("功率", head_font))
        Wtxt.Colspan = 4
        Wtxt.PaddingBottom = 5.0F
        Wtxt.HorizontalAlignment = 1
        pdf_W.AddCell(Wtxt)

        Dim AWtxt As PdfPCell = New PdfPCell(New Phrase("平均值", unit_font))
        AWtxt.Colspan = 1
        AWtxt.HorizontalAlignment = 0
        AWtxt.PaddingBottom = 5.0F
        AWtxt.PaddingLeft = 20.0F
        AWtxt.Border = 0
        pdf_W.AddCell(AWtxt)
        Dim AW As PdfPCell = New PdfPCell(New Phrase("" & avgW.Text & "kw ", title_font))
        AW.Colspan = 3
        AW.HorizontalAlignment = 2
        AW.PaddingBottom = 5.0F
        AW.PaddingRight = 20.0F
        AW.Border = 0
        pdf_W.AddCell(AW)

        Dim MWtxt As PdfPCell = New PdfPCell(New Phrase("最大值", unit_font))
        MWtxt.Colspan = 1
        MWtxt.HorizontalAlignment = 0
        MWtxt.PaddingBottom = 5
        MWtxt.PaddingLeft = 20.0F
        MWtxt.Border = 0
        pdf_W.AddCell(MWtxt)
        Dim MW As PdfPCell = New PdfPCell(New Phrase("" & maxW.Text & "kw ", title_font))
        MW.Colspan = 3
        MW.HorizontalAlignment = 2
        MW.PaddingBottom = 5.0F
        MW.PaddingRight = 20.0F
        MW.Border = 0
        pdf_W.AddCell(MW)

        Dim right1 As PdfPCell = New PdfPCell(pdf_W)
        right1.Colspan = 4
        right1.Rowspan = 3
        pdf.AddCell(right1)

        '電壓
        Dim pdf_V As PdfPTable = New PdfPTable(4)

        Dim Vtxt As PdfPCell = New PdfPCell(New Phrase("電壓", head_font))
        Vtxt.PaddingBottom = 5.0F
        Vtxt.Colspan = 4
        Vtxt.HorizontalAlignment = 1
        pdf_V.AddCell(Vtxt)

        Dim AVtxt As PdfPCell = New PdfPCell(New Phrase("平均值", unit_font))
        AVtxt.Colspan = 1
        AVtxt.HorizontalAlignment = 0
        AVtxt.PaddingBottom = 5.0F
        AVtxt.PaddingLeft = 20.0F
        AVtxt.Border = 0
        pdf_V.AddCell(AVtxt)
        Dim av As PdfPCell = New PdfPCell(New Phrase("" & avgV.Text & "V ", title_font))
        av.Colspan = 3
        av.HorizontalAlignment = 2
        av.PaddingBottom = 5.0F
        av.PaddingRight = 20.0F
        av.Border = 0
        pdf_V.AddCell(av)

        Dim MVtxt As PdfPCell = New PdfPCell(New Phrase("最大值", unit_font))
        MVtxt.Colspan = 1
        MVtxt.HorizontalAlignment = 0
        MVtxt.PaddingBottom = 5.0F
        MVtxt.PaddingLeft = 20.0F
        MVtxt.Border = 0
        pdf_V.AddCell(MVtxt)
        Dim mv As PdfPCell = New PdfPCell(New Phrase("" & maxV.Text & "V ", title_font))
        mv.Colspan = 3
        mv.HorizontalAlignment = 2
        mv.PaddingBottom = 5.0F
        mv.PaddingRight = 20.0F
        mv.Border = 0
        pdf_V.AddCell(mv)

        Dim left3 As PdfPCell = New PdfPCell(pdf_V)
        left3.Colspan = 4
        left3.Rowspan = 3
        pdf.AddCell(left3)

        '用電量
        Dim pdf_KWH As PdfPTable = New PdfPTable(4)

        Dim KWHtxt As PdfPCell = New PdfPCell(New Phrase("用電量", head_font))
        KWHtxt.PaddingBottom = 5.0F
        KWHtxt.Colspan = 4
        KWHtxt.HorizontalAlignment = 1
        pdf_KWH.AddCell(KWHtxt)

        Dim rushtxt As PdfPCell = New PdfPCell(New Phrase("尖峰", unit_font))
        rushtxt.Colspan = 1
        rushtxt.HorizontalAlignment = 0
        rushtxt.PaddingBottom = 5.0F
        rushtxt.PaddingLeft = 20.0F
        rushtxt.Border = 0
        pdf_KWH.AddCell(rushtxt)
        Dim rushmax As PdfPCell = New PdfPCell(New Phrase("" & Rush.Text & "KWH ", title_font))
        rushmax.Colspan = 3
        rushmax.HorizontalAlignment = 2
        rushmax.PaddingBottom = 5.0F
        rushmax.PaddingRight = 20.0F
        rushmax.Border = 0
        pdf_KWH.AddCell(rushmax)

        Dim halftxt As PdfPCell = New PdfPCell(New Phrase("半尖峰", unit_font))
        halftxt.Colspan = 1
        halftxt.HorizontalAlignment = 0
        halftxt.PaddingBottom = 5.0F
        halftxt.PaddingLeft = 20.0F
        halftxt.Border = 0
        pdf_KWH.AddCell(halftxt)
        Dim halfmax As PdfPCell = New PdfPCell(New Phrase("" & Half.Text & "KWH ", title_font))
        halfmax.Colspan = 3
        halfmax.HorizontalAlignment = 2
        halfmax.PaddingBottom = 5.0F
        halfmax.PaddingRight = 20.0F
        halfmax.Border = 0
        pdf_KWH.AddCell(halfmax)

        Dim sathalftxt As PdfPCell = New PdfPCell(New Phrase("週六半尖峰", unit_font))
        sathalftxt.Colspan = 1
        sathalftxt.HorizontalAlignment = 0
        sathalftxt.PaddingBottom = 5.0F
        sathalftxt.PaddingLeft = 20.0F
        sathalftxt.NoWrap = True
        sathalftxt.Border = 0
        pdf_KWH.AddCell(sathalftxt)
        Dim sathalfmax As PdfPCell = New PdfPCell(New Phrase("" & SatHalf.Text & "KWH ", title_font))
        sathalfmax.Colspan = 3
        sathalfmax.HorizontalAlignment = 2
        sathalfmax.PaddingBottom = 5.0F
        sathalfmax.PaddingRight = 20.0F
        sathalfmax.Border = 0
        pdf_KWH.AddCell(sathalfmax)

        Dim offtxt As PdfPCell = New PdfPCell(New Phrase("離峰", unit_font))
        offtxt.Colspan = 1
        offtxt.HorizontalAlignment = 0
        offtxt.PaddingBottom = 5.0F
        offtxt.PaddingLeft = 20.0F
        offtxt.Border = 0
        pdf_KWH.AddCell(offtxt)
        Dim offmax As PdfPCell = New PdfPCell(New Phrase("" & Off.Text & "KWH ", title_font))
        offmax.Colspan = 3
        offmax.HorizontalAlignment = 2
        offmax.PaddingBottom = 5.0F
        offmax.PaddingRight = 20.0F
        offmax.Border = 0
        pdf_KWH.AddCell(offmax)

        Dim pdf_sum As PdfPTable = New PdfPTable(4)

        Dim sumtxt As PdfPCell = New PdfPCell(New Phrase("總計", unit_font))
        sumtxt.Colspan = 1
        sumtxt.HorizontalAlignment = 0
        sumtxt.PaddingBottom = 5.0F
        sumtxt.PaddingLeft = 20.0F
        sumtxt.Border = 0
        pdf_sum.AddCell(sumtxt)
        Dim summax As PdfPCell = New PdfPCell(New Phrase("" & Sum.Text & "KWH ", sum_font))
        summax.Colspan = 3
        summax.HorizontalAlignment = 2
        summax.PaddingBottom = 5.0F
        summax.PaddingRight = 20.0F
        summax.Border = 0
        pdf_sum.AddCell(summax)

        Dim left_sum As PdfPCell = New PdfPCell(pdf_sum)
        left_sum.Colspan = 4
        left_sum.Border = 1
        pdf_KWH.AddCell(left_sum)

        Dim left2 As PdfPCell = New PdfPCell(pdf_KWH)
        left2.Colspan = 4
        left2.Rowspan = 6
        pdf.AddCell(left2)

        '電流
        Dim pdf_I As PdfPTable = New PdfPTable(4)

        Dim Itxt As PdfPCell = New PdfPCell(New Phrase("電流", head_font))
        Itxt.PaddingBottom = 5.0F
        Itxt.Colspan = 4
        Itxt.HorizontalAlignment = 1
        pdf_I.AddCell(Itxt)

        Dim AItxt As PdfPCell = New PdfPCell(New Phrase("平均值", unit_font))
        AItxt.Colspan = 1
        AItxt.HorizontalAlignment = 0
        AItxt.PaddingBottom = 5.0F
        AItxt.PaddingLeft = 20.0F
        AItxt.Border = 0
        pdf_I.AddCell(AItxt)
        Dim ai As PdfPCell = New PdfPCell(New Phrase("" & avgI.Text & "A ", title_font))
        ai.Colspan = 3
        ai.HorizontalAlignment = 2
        ai.PaddingBottom = 5.0F
        ai.PaddingRight = 20.0F
        ai.Border = 0
        pdf_I.AddCell(ai)

        Dim MItxt As PdfPCell = New PdfPCell(New Phrase("最大值", unit_font))
        MItxt.Colspan = 1
        MItxt.HorizontalAlignment = 0
        MItxt.PaddingBottom = 5.0F
        MItxt.PaddingLeft = 20.0F
        MItxt.Border = 0
        pdf_I.AddCell(MItxt)
        Dim mi As PdfPCell = New PdfPCell(New Phrase("" & maxI.Text & "A ", title_font))
        mi.Colspan = 3
        mi.HorizontalAlignment = 2
        mi.PaddingBottom = 5.0F
        mi.PaddingRight = 20.0F
        mi.Border = 0
        pdf_I.AddCell(mi)

        Dim left4 As PdfPCell = New PdfPCell(pdf_I)
        left4.Colspan = 4
        left4.Rowspan = 3
        pdf.AddCell(left4)

        '空白行
        Dim space2 As PdfPCell = New PdfPCell(New Phrase(" "))
        space2.Colspan = CountColumns
        space2.Border = 0
        pdf.AddCell(space2)

        '要定義 PDF Table 的大小（Column 數量）
        Dim CountColumns2 As Integer = 8  '自動生成
        Dim CountRow2 As Integer = GridView1.Rows.Count
        Dim pdf2 As PdfPTable = New PdfPTable(CountColumns2)

        pdf2.HeaderRows = 1
        pdf2.WidthPercentage = 100  '表格寬度百分比
        pdf2.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '--- 表格標題
        Dim title As PdfPCell = New PdfPCell(New Phrase("功率(kw)", unit_font))
        title.HorizontalAlignment = 1
        title.Colspan = 3
        title.PaddingBottom = 5.0F
        pdf2.AddCell(title)

        Dim title2 As PdfPCell = New PdfPCell(New Phrase("用電量(KWH)", unit_font))
        title2.HorizontalAlignment = 1
        title2.Colspan = 5
        title2.PaddingBottom = 5.0F
        pdf2.AddCell(title2)

        '如果要自訂標題，就不要用迴圈跑
        Dim head As PdfPCell  'GridView1標題
        For i = 0 To GridView1.HeaderRow.Cells.Count - 1
            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, head_font))
            head.HorizontalAlignment = 1
            head.PaddingBottom = 5.0F
            'head.BackgroundColor = New BaseColor(255, 255, 204)
            pdf2.AddCell(head)
        Next
        Dim head2 As PdfPCell  'GridView2標題
        For i = 0 To GridView2.HeaderRow.Cells.Count - 1
            head2 = New PdfPCell(New Phrase(GridView2.HeaderRow.Cells(i).Text, head_font))
            head2.HorizontalAlignment = 1
            head2.PaddingBottom = 5.0F
            'head2.BackgroundColor = New BaseColor(255, 255, 204)
            pdf2.AddCell(head2)
        Next
        '--- 表格內文
        '利用迴圈規則性的填入內文
        Dim table As PdfPCell  'GridView1內容
        Dim table2 As PdfPCell  'GridView2內容
        For i = 0 To CountRow2 - 1
            For j = 0 To GridView1.HeaderRow.Cells.Count - 1
                table = New PdfPCell(New Phrase(GridView1.Rows(i).Cells(j).Text, fontchinese))
                table.HorizontalAlignment = 1
                table.PaddingBottom = 5.0F
                'If i Mod 2 Then
                '    table.BackgroundColor = New BaseColor(255, 255, 204)
                'End If
                pdf2.AddCell(table)
            Next
            For j = 0 To GridView2.HeaderRow.Cells.Count - 1
                table2 = New PdfPCell(New Phrase(GridView2.Rows(i).Cells(j).Text, fontchinese))
                table2.HorizontalAlignment = 1
                table2.PaddingBottom = 5.0F
                'If i Mod 2 Then
                '    table2.BackgroundColor = New BaseColor(255, 255, 204)
                'End If
                pdf2.AddCell(table2)
            Next
        Next

        doc.Add(pdf)
        doc.Add(pdf2)
        doc.Close()
    End Sub
End Class
