Imports System.Data.OleDb
Imports System.Data
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text

Partial Class WeekReport
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then
                SqlQuery()
            End If
        End If
    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()
        SqlDataSource2.SelectParameters.Clear()
        Dim group As String = Request.QueryString("group")
        Dim node As String = Request.QueryString("node")
        Dim start_time As String = Request.QueryString("datetime").Split(",").GetValue(0)
        Dim end_time As String = Request.QueryString("datetime").Split(",").GetValue(1)

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        'If Session("Rank") = 2 Then
        '    strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(account) & ""
        'Else
        '    strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
        'End If

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

        Date_txt.Text = "日期：" & start_time & "~" & end_time
        Num_txt.Text = "編號："
        For i = 0 To check_count_m - 1
            Num_txt.Text &= ctrlnr(i) & "-" & meterid(i) & ","
        Next
        Num_txt.Text = Num_txt.Text.Substring(0, Num_txt.Text.Length - 1)

        Position_txt.Text = "位置："
        For i = 0 To check_count_m - 1
            Position_txt.Text &= position(i) & ","
        Next
        Position_txt.Text = Position_txt.Text.Substring(0, Position_txt.Text.Length - 1)

        Try
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                '電流/電壓/功率-平均值/最大值
                Dim value_data() As String = {"Iavg", "Imax", "Vavg", "Vmax", "Wavg", "Wmax"}
                Dim sql_data As String = ""
                For i = 0 To check_count_m
                    If i <> check_count_m Then
                        sql_data &= "SELECT Round(AVG(Iavg),2) AS Iavg,MAX(Imax) AS Imax,Round(AVG(Vavg),2) AS Vavg,MAX(Vmax) AS Vmax,Round(AVG(Wavg),2) AS Wavg,MAX(Wmax) AS Wmax into #T" & i + 1 & " FROM PowerRecordCollection " & _
                        "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & start_time & "' and '" & end_time & "';"
                    Else
                        sql_data &= "SELECT "
                        For j = 0 To value_data.Length - 1
                            For k = 0 To check_count_m - 1
                                sql_data &= "#T" & k + 1 & "." & value_data(j) & "+"
                            Next
                            sql_data = sql_data.Substring(0, sql_data.Length - 1)
                            If j <> value_data.Length Then
                                sql_data &= " AS " & value_data(j) & ","
                            End If
                        Next
                        sql_data = sql_data.Substring(0, sql_data.Length - 1)

                        sql_data &= " FROM "
                        For k = 0 To check_count_m - 1
                            sql_data &= "#T" & k + 1 & ","
                        Next
                        sql_data = sql_data.Substring(0, sql_data.Length - 1) & ";"

                        sql_data &= "Drop table "
                        For k = 0 To check_count_m - 1
                            sql_data &= "#T" & k + 1 & ","
                        Next
                        sql_data = sql_data.Substring(0, sql_data.Length - 1)
                    End If
                Next
                Using cmd As New OleDbCommand(sql_data, conn)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            avgI.Text = String.Format("{0:N}", dr("Iavg")) & "　"
                            maxI.Text = String.Format("{0:N}", dr("Imax")) & "　"
                            avgV.Text = String.Format("{0:N}", dr("Vavg")) & "　"
                            maxV.Text = String.Format("{0:N}", dr("Vmax")) & "　"
                            avgW.Text = String.Format("{0:N}", dr("Wavg")) & "　"
                            maxW.Text = String.Format("{0:N}", dr("Wmax")) & "　"
                            'maxI.ToolTip = dr("ImaxRD").ToString
                            'maxV.ToolTip = dr("VmaxRD").ToString
                            'maxW.ToolTip = dr("WmaxRD").ToString
                        End If
                    End Using
                End Using

                '最大需量
                Dim value_demand() As String = {"Demand", "DemandHalf", "DemandSatHalf", "DemandOff"}
                Dim sql_demand As String = ""
                For i = 0 To check_count_m
                    If i <> check_count_m Then
                        sql_demand &= "SELECT MAX(Demand) AS Demand,MAX(DemandHalf) AS DemandHalf,MAX(DemandSatHalf) AS DemandSatHalf,MAX(DemandOff) AS DemandOff into #T" & i + 1 & " FROM PowerRecordCollection " & _
                        "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & start_time & "' and '" & end_time & "';"
                    Else
                        sql_demand &= "SELECT "
                        For j = 0 To value_demand.Length - 1
                            For k = 0 To check_count_m - 1
                                sql_demand &= "#T" & k + 1 & "." & value_demand(j) & "+"
                            Next
                            sql_demand = sql_demand.Substring(0, sql_demand.Length - 1)
                            If j <> value_demand.Length Then
                                sql_demand &= " AS " & value_demand(j) & ","
                            End If
                        Next
                        sql_demand = sql_demand.Substring(0, sql_demand.Length - 1)

                        sql_demand &= " FROM "
                        For k = 0 To check_count_m - 1
                            sql_demand &= "#T" & k + 1 & ","
                        Next
                        sql_demand = sql_demand.Substring(0, sql_demand.Length - 1) & ";"

                        sql_demand &= "Drop table "
                        For k = 0 To check_count_m - 1
                            sql_demand &= "#T" & k + 1 & ","
                        Next
                        sql_demand = sql_demand.Substring(0, sql_demand.Length - 1)
                    End If
                Next
                Using cmd As New OleDbCommand(sql_demand, conn)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Demand.Text = CInt(dr("Demand")).ToString("#,0") & "　"
                            HalfDemand.Text = CInt(dr("DemandHalf")).ToString("#,0") & "　"
                            SatHalfDemand.Text = CInt(dr("DemandSatHalf")).ToString("#,0") & "　"
                            OffDemand.Text = CInt(dr("DemandOff")).ToString("#,0") & "　"
                        End If
                    End Using
                End Using

                '用電量
                Dim value_KWH() As String = {"RushHour", "HalfHour", "SatHalfHour", "OffHour", "SumHour"}
                Dim sql_KWH As String = ""
                For i = 0 To check_count_m
                    If i <> check_count_m Then
                        sql_KWH &= "SELECT isnull(SUM(RushHour),0) AS RushHour,isnull(SUM(HalfHour),0) AS HalfHour,isnull(SUM(SatHalfHour),0) AS SatHalfHour,isnull(SUM(OffHour),0) AS OffHour,isnull(SUM(RushHour+HalfHour+SatHalfHour+OffHour),0) AS SumHour into #T" & i + 1 & " FROM PowerRecordCollection " & _
                        "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate between '" & start_time & "' and '" & end_time & "';"
                    Else
                        sql_KWH &= "SELECT "
                        For j = 0 To value_KWH.Length - 1
                            For k = 0 To check_count_m - 1
                                sql_KWH &= "#T" & k + 1 & "." & value_KWH(j) & "+"
                            Next
                            sql_KWH = sql_KWH.Substring(0, sql_KWH.Length - 1)
                            If j <> value_KWH.Length Then
                                sql_KWH &= " AS " & value_KWH(j) & ","
                            End If
                        Next
                        sql_KWH = sql_KWH.Substring(0, sql_KWH.Length - 1)

                        sql_KWH &= " FROM "
                        For k = 0 To check_count_m - 1
                            sql_KWH &= "#T" & k + 1 & ","
                        Next
                        sql_KWH = sql_KWH.Substring(0, sql_KWH.Length - 1) & ";"

                        sql_KWH &= "Drop table "
                        For k = 0 To check_count_m - 1
                            sql_KWH &= "#T" & k + 1 & ","
                        Next
                        sql_KWH = sql_KWH.Substring(0, sql_KWH.Length - 1)
                    End If
                Next
                Using cmd As New OleDbCommand(sql_KWH, conn)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Rush.Text = CInt(dr("RushHour")).ToString("#,0") & "　"
                            Half.Text = CInt(dr("HalfHour")).ToString("#,0") & "　"
                            SatHalf.Text = CInt(dr("SatHalfHour")).ToString("#,0") & "　"
                            Off.Text = CInt(dr("OffHour")).ToString("#,0") & "　"
                            Sum.Text = CInt(dr("SumHour")).ToString("#,0") & "　"
                        End If
                    End Using
                End Using
            End Using

            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            Dim sql1 As String = ""
            Dim value() As String = {"Wavg", "Wmax"}
            For i = 0 To check_count_m
                If i <> check_count_m Then
                    sql1 &= "SELECT SUBSTRING(RecDate,6,10) AS 日期,Round(AVG(Wavg),2) AS Wavg,Round(MAX(Wmax),2) AS Wmax into #T" & i + 1 & " FROM PowerRecordCollection " & _
                    "WHERE CtrlNr= " & ctrlnr(i) & " and MeterID= " & meterid(i) & " and RecDate between '" & start_time & "' and '" & end_time & "' GROUP BY SUBSTRING(RecDate,6,10) order by 日期;"
                Else
                    sql1 &= "SELECT #T1.日期,"
                    For j = 0 To value.Length - 1
                        For k = 0 To check_count_m - 1
                            sql1 &= "#T" & k + 1 & "." & value(j) & "+"
                        Next
                        sql1 = sql1.Substring(0, sql1.Length - 1)
                        If j <> value.Length Then
                            sql1 &= " AS " & value(j) & ","
                        End If
                    Next
                    sql1 = sql1.Substring(0, sql1.Length - 1)

                    sql1 &= " FROM "
                    For k = 0 To check_count_m - 1
                        sql1 &= "#T" & k + 1 & ","
                    Next
                    sql1 = sql1.Substring(0, sql1.Length - 1)

                    If check_count_m > 1 Then
                        sql1 &= " WHERE "
                        For k = 0 To check_count_m - 2
                            sql1 &= "#T" & k + 1 & ".日期 = #T" & k + 2 & ".日期 and "
                        Next
                        sql1 = sql1.Substring(0, sql1.Length - 4)
                    End If

                    sql1 &= ";Drop table "
                    For k = 0 To check_count_m - 1
                        sql1 &= "#T" & k + 1 & ","
                    Next
                    sql1 = sql1.Substring(0, sql1.Length - 1)
                End If
            Next
            SqlDataSource1.SelectCommand = sql1

            SqlDataSource2.ConnectionString = strcon
            SqlDataSource2.ProviderName = "System.Data.OleDb"
            Dim sql2 As String = ""
            Dim value2() As String = {"RushHour", "HalfHour", "SatHalfHour", "OffHour"}
            For i = 0 To check_count_m
                If i <> check_count_m Then
                    sql2 &= "SELECT SUBSTRING(RecDate,6,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour,(RushHour+HalfHour+SatHalfHour+OffHour) AS SumHour into #T" & i + 1 & " FROM PowerRecordCollection " & _
                    "WHERE CtrlNr= " & ctrlnr(i) & " and MeterID= " & meterid(i) & " and RecDate between '" & start_time & "' and '" & end_time & "' order by RecDate;"
                Else
                    sql2 &= "SELECT #T1.日期,"
                    For j = 0 To value2.Length - 1
                        For k = 0 To check_count_m - 1
                            sql2 &= "#T" & k + 1 & "." & value2(j) & "+"
                        Next
                        sql2 = sql2.Substring(0, sql2.Length - 1)
                        If j <> value2.Length Then
                            sql2 &= " AS " & value2(j) & ","
                        End If
                    Next
                    sql2 = sql2.Substring(0, sql2.Length - 1)

                    sql2 &= " FROM "
                    For k = 0 To check_count_m - 1
                        sql2 &= "#T" & k + 1 & ","
                    Next
                    sql2 = sql2.Substring(0, sql2.Length - 1)

                    If check_count_m > 1 Then
                        sql2 &= " WHERE "
                        For k = 0 To check_count_m - 2
                            sql2 &= "#T" & k + 1 & ".日期 = #T" & k + 2 & ".日期 and "
                        Next
                        sql2 = sql2.Substring(0, sql2.Length - 4)
                    End If

                    sql2 &= ";Drop table "
                    For k = 0 To check_count_m - 1
                        sql2 &= "#T" & k + 1 & ","
                    Next
                    sql2 = sql2.Substring(0, sql2.Length - 1)
                End If
            Next
            SqlDataSource2.SelectCommand = sql2

            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv.Count > 0 Then
                Panel_Report.Visible = True
                Panel_Export.Visible = True
            Else
                Panel_Report.Visible = False
                Panel_Export.Visible = False
                Dim msg As String = "查無資料"
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
            End If
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

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Print.Click
        SqlQuery()
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(sender As Object, e As ImageClickEventArgs) Handles Excel.Click
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String = Server.UrlPathEncode("週報表_" & Now())

        Response.ClearHeaders()
        Response.Clear()
        Response.Expires = 0
        Response.Buffer = True
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write(stringWrite.ToString)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles PDF.Click
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("週報表_" & Now.Year & "_" & Now.Month)

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

        Dim Header2 As PdfPCell = New PdfPCell(New Phrase("週報表", title_font))
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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub

End Class
