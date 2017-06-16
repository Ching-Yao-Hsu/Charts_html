Imports System.Data
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.Net

Partial Class PowerDayReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then
                Date_Info.Text = DateAdd(DateInterval.Day, 0, Now).ToString("yyyy/MM/dd")
            End If
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()
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
        Date_txt.Text = "日期：" & Date_Info.Text
        Num_txt.Text = "編號：" & Session("ctrlnr") & "-" & Session("meterid")
        Position_txt.Text = "位置：" & Session("position")
        Try
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                '預存程序
                'Dim sql_sp As String = "Exec [ECO_" & account(i) & "].[dbo].[Usp_Day_Record] " & ctrlnr & "," & meterid & "," & datetime & ""
                Using cmd_sp As New OleDbCommand("Usp_Day_PowerRecord", conn)
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
                    Rush.Text = CInt(paramRet_rushhour.Value).ToString("#,0") & "　"
                    Half.Text = CInt(paramRet_halfhour.Value).ToString("#,0") & "　"
                    SatHalf.Text = CInt(paramRet_sathalfhour.Value).ToString("#,0") & "　"
                    Off.Text = CInt(paramRet_offhour.Value).ToString("#,0") & "　"
                    Sum.Text = (CInt(paramRet_rushhour.Value) + CInt(paramRet_halfhour.Value) + CInt(paramRet_sathalfhour.Value) + CInt(paramRet_offhour.Value)).ToString("#,0") & "　"
                    maxI.ToolTip = paramRet_imaxrd.Value
                    maxV.ToolTip = paramRet_vmaxrd.Value
                    maxW.ToolTip = paramRet_wmaxrd.Value
                End Using

                Using cmd_sp As New OleDbCommand("Usp_Day_PowerRecord_Detail", conn)
                    cmd_sp.CommandType = CommandType.StoredProcedure
                    Dim CtrlNrParam As New OleDbParameter("@sCtrlNr", Session("ctrlnr"))
                    Dim MeterIDParam As New OleDbParameter("@sMeterID", Session("meterid"))
                    Dim RecDateParam As New OleDbParameter("@sRecDate", Date_Info.Text)
                    Dim IntervalParam As New OleDbParameter("@sInterval", interval_DDList.SelectedValue)
                    cmd_sp.Parameters.Add(CtrlNrParam)
                    cmd_sp.Parameters.Add(MeterIDParam)
                    cmd_sp.Parameters.Add(RecDateParam)
                    cmd_sp.Parameters.Add(IntervalParam)
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

                ''電流/電壓/功率-平均值/最大值
                'Dim ImaxRD, VmaxRD, WmaxRD As String
                'Dim sql_data As String = "SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax," & _
                '"AVG(W) AS Wavg,MAX(W) AS Wmax into #T1 FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,10) = '" & Date_Info.Text & "';" & _
                '"SELECT RecDate AS 最大電流時間 into #T2 FROM PowerRecord,#T1 " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,10) = '" & Date_Info.Text & "' AND  PowerRecord.Iavg = #T1.Imax;" & _
                '"SELECT RecDate AS 最大電壓時間 into #T3 FROM PowerRecord,#T1 " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,10) = '" & Date_Info.Text & "' AND  PowerRecord.Vavg = #T1.Vmax;" & _
                '"SELECT RecDate AS 最大功率時間 into #T4 FROM PowerRecord,#T1 " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,10) = '" & Date_Info.Text & "' AND  PowerRecord.W = #T1.Wmax;" & _
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
                '            ImaxRD = dr_data("最大電流時間").ToString
                '            VmaxRD = dr_data("最大電壓時間").ToString
                '            WmaxRD = dr_data("最大功率時間").ToString
                '        Else
                '            ImaxRD = ""
                '            VmaxRD = ""
                '            WmaxRD = ""
                '        End If
                '    End Using
                'End Using

                '最大需量
                'Dim demand, demandhalf, demandsathalf, demandoff As String
                'Dim sql_demand As String = "SELECT MAX(DeMand) AS 尖峰,MAX(DeMandHalf) AS 半尖峰,MAX(DeMandSatHalf) AS 週六半尖峰,MAX(DeMandOff) AS 離峰 FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " AND substring(RecDate,1,10) = '" & Date_Info.Text & "'"
                'Using cmd_Demand As New OleDbCommand(sql_demand, conn)
                '    Using dr_KWH As OleDbDataReader = cmd_Demand.ExecuteReader()
                '        If dr_KWH.Read() Then
                '            demand = CInt(dr_KWH("尖峰")).ToString("#,0")
                '            demandhalf = CInt(dr_KWH("半尖峰")).ToString("#,0")
                '            demandsathalf = CInt(dr_KWH("週六半尖峰")).ToString("#,0")
                '            demandoff = CInt(dr_KWH("離峰")).ToString("#,0")
                '        Else
                '            demand = 0
                '            demandhalf = 0
                '            demandsathalf = 0
                '            demandoff = 0
                '        End If
                '    End Using
                'End Using

                ''今日用電量
                'Dim sql_KWH As String = "SELECT TOP 1 SUBSTRING(RecDate,1,10) AS 日期," & _
                '"RushHour,HalfHour,SatHalfHour,OffHour INTO #T1 FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,10) = '" & DateAdd(DateInterval.Day, -1, CDate(Date_Info.Text)).ToString("yyyy/MM/dd") & "' ORDER BY RecDate DESC;" & _
                '"SELECT TOP 1 SUBSTRING(RecDate,1,10) AS 日期," & _
                '"RushHour,HalfHour,SatHalfHour,OffHour INTO #T2 FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,10) = '" & Date_Info.Text & "' ORDER BY RecDate DESC;" & _
                '"SELECT " & _
                '"(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT RushHour FROM #T1) " & _
                '"ELSE (SELECT TOP 1 RushHour FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,10) = '" & Date_Info.Text & "') END) AS RushHour," & _
                '"(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT HalfHour FROM #T1) " & _
                '"ELSE (SELECT TOP 1 HalfHour FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,10) = '" & Date_Info.Text & "') END) AS HalfHour," & _
                '"(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT SatHalfHour FROM #T1) " & _
                '"ELSE (SELECT TOP 1 SatHalfHour FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,10) = '" & Date_Info.Text & "') END) AS SatHalfHour," & _
                '"(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT OffHour FROM #T1) " & _
                '"ELSE (SELECT TOP 1 OffHour FROM PowerRecord WHERE CtrlNr = " & Session("ctrlnr") & " AND MeterID = " & Session("meterid") & " and SUBSTRING(RecDate,1,10) = '" & Date_Info.Text & "') END) AS OffHour INTO #T3;" & _
                '"SELECT TOP 1 " & _
                '"(CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.RushHour ELSE #T2.RushHour - #T3.RushHour END) AS 尖峰," & _
                '"(CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.HalfHour ELSE #T2.HalfHour - #T3.HalfHour END) AS 半尖峰," & _
                '"(CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.SatHalfHour ELSE #T2.SatHalfHour - #T3.SatHalfHour END) AS 週六半尖峰," & _
                '"(CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.OffHour ELSE #T2.OffHour - #T3.OffHour END) AS 離峰 " & _
                '"FROM #T3,#T2;DROP TABLE #T1,#T2;"

                'Using cmd As New OleDbCommand(sql_KWH, conn)
                '    Using dr As OleDbDataReader = cmd.ExecuteReader()
                '        If dr.Read() Then
                '            Rush.Text = CInt(dr("尖峰")).ToString("#,0") & "　"
                '            Half.Text = CInt(dr("半尖峰")).ToString("#,0") & "　"
                '            SatHalf.Text = CInt(dr("週六半尖峰")).ToString("#,0") & "　"
                '            Off.Text = CInt(dr("離峰")).ToString("#,0") & "　"
                '            Sum.Text = CInt((dr("尖峰") + dr("半尖峰") + dr("週六半尖峰") + dr("離峰"))).ToString("#,0") & "　"
                '        End If
                '    End Using
                'End Using
                
                'SqlDataSource1.ConnectionString = strcon
                'SqlDataSource1.ProviderName = "System.Data.OleDb"
                'SqlDataSource1.SelectCommand = "SELECT substring(RecDate,11,9) as 時間,Iavg as 電流,Vavg as 電壓,W as 實功,V_ar as 虛功,VA as 視在,PF as 功因,KWh as 用電度數 FROM PowerRecord " & _
                '"WHERE CtrlNr = " & Session("ctrlnr") & " and MeterID = " & Session("meterid") & " and substring(RecDate,1,10) = '" & Date_Info.Text & "' order by 時間 "
                'Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
                'If dv.Count > 0 Then
                '    'Dim sql As String = "SELECT * FROM PowerRecordCollection " & _
                '    '"WHERE CtrlNr = " & Session("ctrlnr") & " and MeterID = " & Session("meterid") & " and substring(RecDate,1,10) = '" & Date_Info.Text & "' "
                '    'Using cmd As New OleDbCommand(sql, conn)
                '    '    Using dr As OleDbDataReader = cmd.ExecuteReader()
                '    '        If Not dr.Read() Then
                '    '            Try
                '    '                Dim val As String = Session("ctrlnr") & "," & Session("meterid") & ",'" & Date_Info.Text & "'," & _
                '    '                "" & CDbl(avgI.Text.Trim) & "," & CDbl(maxI.Text.Trim) & "," & CDbl(avgV.Text.Trim) & "," & CDbl(maxV.Text.Trim) & "," & CDbl(avgW.Text.Trim) & "," & CDbl(maxW.Text.Trim) & "," & _
                '    '                "" & CInt(demand) & "," & CInt(demandhalf) & "," & CInt(demandsathalf) & "," & CInt(demandoff) & "," & _
                '    '                "" & CInt(Rush.Text.Trim) & "," & CInt(Half.Text.Trim) & "," & CInt(SatHalf.Text.Trim) & "," & CInt(Off.Text.Trim) & "," & _
                '    '                "'" & Now.ToString("MM") & "','" & DateAdd(DateInterval.Day, -(Weekday(Date_Info.Text)) + 2, CDate(Date_Info.Text)).ToString("yyyy/MM/dd") & "'," & _
                '    '                "'" & ImaxRD & "','" & VmaxRD & "','" & WmaxRD & "'"
                '    '                Dim sql_collection As String = "insert into PowerRecordCollection" & _
                '    '                "(CtrlNr,MeterID,RecDate,Iavg,Imax,Vavg,Vmax,Wavg,Wmax,Demand,DemandHalf,DemandSatHalf,DemandOff,RushHour,HalfHour,SatHalfHour,OffHour,MonthMark,WeekMark,ImaxRD,VmaxRD,WmaxRD) " & _
                '    '                "values(" & val & ")"
                '    '                Using cmd_collection As New OleDbCommand(sql_collection, conn)
                '    '                    cmd_collection.ExecuteNonQuery()
                '    '                End Using
                '    '            Catch ex As Exception
                '    '                Dim val As String = "0,0,'" & Now.ToString("yyyy/MM/dd HH:mm:ss") & "',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'錯誤'"
                '    '                Dim strSQL As String = "insert into PowerRecord(CtrlNr,MeterID,RecDate,I1,I2,I3,Iavg,V1,V2,V3,Vavg,W,V_ar,VA,PF,KWh,Mode1,Mode2,Mode3,Mode4,DeMand,DemandHalf,DemandSatHalf,DemandOff,RushHour,HalfHour,SatHalfHour,OffHour,state) values (" & val & ")"
                '    '                Using conn3 As New OleDbConnection(strcon)
                '    '                    If conn3.State = 0 Then conn3.Open()
                '    '                    Using cmd3 As New OleDbCommand(strSQL, conn3)
                '    '                        cmd3.ExecuteNonQuery()
                '    '                    End Using
                '    '                End Using
                '    '            End Try
                '    '        End If
                '    '    End Using
                '    'End Using

                '    Panel_Report.Visible = True
                '    msg.Visible = False
                '    Panel_Export.Visible = True
                'Else
                '    Panel_Report.Visible = False
                '    msg.Visible = True
                '    Panel_Export.Visible = False
                'End If
            End Using
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            For i = 0 To 7
                e.Row.Cells(i).Width = 113
            Next
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = CDate(e.Row.Cells(0).Text).ToString("HH:mm:ss")
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
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("日報表_" & Now().Date)

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"
        GridView1.DataBind()

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)

        Dim CountColumn As Integer = GridView1.HeaderRow.Cells.Count
        Dim CountRow As Integer = GridView1.Rows.Count

        Panel_Report.RenderControl(htmlWrite)

        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.End()
        GridView1.DataBind()
    End Sub

    Protected Sub PDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PDF.Click
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("日報表_" & Now())

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
        Dim Header As PdfPCell = New PdfPCell(New Phrase("日報表", title_font))
        Header.Colspan = CountColumns
        Header.Border = 0
        Header.HorizontalAlignment = 1
        pdf.AddCell(Header)

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
        datetxt.PaddingLeft = 20.0F
        pdf_Info.AddCell(datetxt)

        Dim ctrlnr As PdfPCell = New PdfPCell(New Phrase(Num_txt.Text & "", unit_font))
        ctrlnr.Colspan = 4
        ctrlnr.Border = 0
        ctrlnr.PaddingLeft = 20.0F
        pdf_Info.AddCell(ctrlnr)

        Dim positiontxt As PdfPCell = New PdfPCell(New Phrase(Position_txt.Text & "", unit_font))
        positiontxt.PaddingBottom = 5.0F
        positiontxt.Colspan = 4
        positiontxt.Border = 0
        positiontxt.PaddingLeft = 20.0F
        pdf_Info.AddCell(positiontxt)

        Dim left1 As PdfPCell = New PdfPCell(pdf_Info)
        left1.Colspan = 4
        left1.Rowspan = 3
        left1.Border = 0
        pdf.AddCell(left1)

        '功率
        Dim pdf_W As PdfPTable = New PdfPTable(4)

        Dim Wtxt As PdfPCell = New PdfPCell(New Phrase("功率", unit_font))
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

        '用電量
        Dim pdf_KWH As PdfPTable = New PdfPTable(4)

        Dim KWHtxt As PdfPCell = New PdfPCell(New Phrase("用電量", unit_font))
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

        '電壓
        Dim pdf_V As PdfPTable = New PdfPTable(4)

        Dim Vtxt As PdfPCell = New PdfPCell(New Phrase("電壓", unit_font))
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

        '電流
        Dim pdf_I As PdfPTable = New PdfPTable(4)

        Dim Itxt As PdfPCell = New PdfPCell(New Phrase("電流", unit_font))
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
        Dim CountColumns2 As Integer = GridView1.HeaderRow.Cells.Count  '自動生成
        Dim CountRow2 As Integer = GridView1.Rows.Count
        Dim pdf2 As PdfPTable = New PdfPTable(CountColumns2)

        pdf2.HeaderRows = 1
        pdf2.WidthPercentage = 100  '表格寬度百分比
        pdf2.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '--- 表格標題
        '如果要自訂標題，就不要用迴圈跑
        Dim head As PdfPCell  'GridView1標題
        For i = 0 To GridView1.HeaderRow.Cells.Count - 1
            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, unit_font))
            head.HorizontalAlignment = 1
            head.PaddingBottom = 5.0F
            'head.BackgroundColor = New BaseColor(255, 255, 204)
            pdf2.AddCell(head)
        Next

        '--- 表格內文
        '利用迴圈規則性的填入內文
        Dim table As PdfPCell  'GridView1內容
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
        Next

        doc.Add(pdf)
        doc.Add(pdf2)
        doc.Close()
    End Sub
End Class
