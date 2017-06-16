Imports System.Data.OleDb
Imports System.Data
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class DayReport
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入(Connection timed out, please sign in again.)"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()
        Dim group As String = Request.QueryString("group")
        Dim node As String = Request.QueryString("node")
        Dim datetime As String = Request.QueryString("datetime")
        Dim interval As String = Request.QueryString("interval")

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


        If Session("language").ToString = "en" Then
            Date_txt.Text = "Date：" & datetime
            Num_txt.Text = "Number："
            Position_txt.Text = "Position："
        Else
            Date_txt.Text = "日期：" & datetime
            Num_txt.Text = "編號："
            Position_txt.Text = "位置："
        End If
        '//編號
        For i = 0 To check_count_m - 1
            Num_txt.Text &= ctrlnr(i) & "-" & meterid(i) & ","
        Next
        Num_txt.Text = Num_txt.Text.Substring(0, Num_txt.Text.Length - 1)

        '//位置
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
                        sql_data &= "SELECT Iavg,Imax,Vavg,Vmax,Wavg,Wmax into #T" & i + 1 & " FROM ECO_" & Find_AdAccount(group) & "_PowerRecordCollection " & _
                        "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate = '" & datetime & "';"
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

                '今日用電量
                Dim value_KWH() As String = {"RushHour", "HalfHour", "SatHalfHour", "OffHour", "Sum"}
                Dim sql_KWH As String = ""
                For i = 0 To check_count_m
                    If i <> check_count_m Then
                        sql_KWH &= "SELECT RushHour,HalfHour,SatHalfHour,OffHour,(RushHour+HalfHour+SatHalfHour+OffHour) AS Sum into #T" & i + 1 & " FROM ECO_" & Find_AdAccount(group) & "_PowerRecordCollection " & _
                        "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate = '" & datetime & "';"
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
                            Sum.Text = CInt(dr("Sum")).ToString("#,0") & "　"
                        End If
                    End Using
                End Using
            End Using

            Dim sql As String = ""
            Dim sTimeField As String = ""
            Dim value() As String
            If Session("language").ToString = "en" Then
                value = {"[Current]", "Voltage", "Actual_power_output", "Virtual_power", "Apparent_power", "Power_Factor", "Kilowatt"}
                sTimeField = "Time"
            Else
                value = {"電流", "電壓", "實功", "虛功", "視在", "功因", "用電度數"}
                sTimeField = "時間"
            End If



            For i = 0 To check_count_m
                If i <> check_count_m Then
                    If Session("language").ToString = "en" Then
                        '// Current 在SQL是保留字, 若不加中括號會錯誤
                        sql &= "SELECT RecTime AS Time,Iavg as [Current],Vavg as Voltage,W as Actual_power_output,V_ar as Virtual_power,VA as Apparent_power,PF as Power_Factor,KWh as Kilowatt into #T" & i + 1
                    Else
                        sql &= "SELECT RecTime AS 時間,Iavg as 電流,Vavg as 電壓,W as 實功,V_ar as 虛功,VA as 視在,PF as 功因,KWh as 用電度數 into #T" & i + 1
                    End If

                    sql &= " FROM ECO_" & Find_AdAccount(group) & "_PowerRecord WHERE CtrlNr= " & ctrlnr(i) & " and MeterID= " & meterid(i) & " and RecDate = '" & datetime & "';"
                Else
                    sql &= "SELECT #T1." & sTimeField & " as " & sTimeField & ","
                    For j = 0 To value.Length - 1
                        For k = 0 To check_count_m - 1
                            sql &= "#T" & k + 1 & "." & value(j) & "+"
                        Next
                        sql = sql.Substring(0, sql.Length - 1)
                        If j <> value.Length Then
                            sql &= " AS " & value(j) & ","
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
                            sql &= "substring(#T" & k + 1 & "." & sTimeField & ",1,5)=substring(#T" & k + 2 & "." & sTimeField & ",1,5) and "
                        Next
                    End If

                    Select Case interval
                        Case "1"
                            If check_count_m > 1 Then
                                sql = sql.Substring(0, sql.Length - 4) & " order by " & sTimeField & ";"
                            Else
                                sql = sql.Substring(0, sql.Length - 6) & " order by " & sTimeField & ";"
                            End If
                        Case "5"
                            sql &= " (substring(#T1." & sTimeField & ",5,1) Like '%5%' or substring(#T1." & sTimeField & ",5,1) Like '%0%') order by " & sTimeField & ";"
                        Case "30"
                            sql &= " (substring(#T1." & sTimeField & ",4,2) Like '%00%' or substring(#T1." & sTimeField & ",4,2) Like '%30%') order by " & sTimeField & ";"
                        Case "60"
                            sql &= " substring(#T1." & sTimeField & ",4,2) Like '%00%' order by " & sTimeField & ";"
                    End Select

                    sql &= "Drop table "
                    For k = 0 To check_count_m - 1
                        sql &= "#T" & k + 1 & ","
                    Next
                    sql = sql.Substring(0, sql.Length - 1)
                End If
            Next

            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv.Count > 0 Then
                Panel_Report.Visible = True
                Panel_Export.Visible = True
            Else
                Panel_Report.Visible = False
                Panel_Export.Visible = False
                Dim msg As String = "查無資料"
                If Session("language").ToString = "en" Then
                    msg = " No Data. "
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
            End If
        Catch ex As Exception
            ex.ToString()
            Panel_Export.Visible = False
            Panel_Report.Visible = False
        End Try
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        SqlQuery()
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(sender As Object, e As ImageClickEventArgs) Handles Excel.Click
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        If Session("language").ToString = "en" Then
            sFilename = "Daily Report_" & Now()
        Else
            sFilename = "日報表_" & Now()
        End If
        'sFilename = Server.UrlPathEncode("日報表_" & Now())
        sFilename = Server.UrlPathEncode(sFilename)

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"
        Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>")

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.End()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles PDF.Click
        Dim sFilename As String
        Dim sTitle As String = ""
        If Session("language").ToString = "en" Then
            sFilename = Server.UrlPathEncode("Daily Report_" & Now())
            sTitle = "Daily Report"
        Else
            sFilename = Server.UrlPathEncode("日報表_" & Now())
            sTitle = "日報表"
        End If


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

        Dim Header2 As PdfPCell = New PdfPCell(New Phrase(sTitle, title_font))
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

        '//功率
        Dim pdf_W As PdfPTable = New PdfPTable(4)

        If Session("language").ToString = "en" Then
            sTitle = "Power"
        Else
            sTitle = "功率"
        End If
        Dim Wtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, head_font))
        Wtxt.Colspan = 4
        Wtxt.PaddingBottom = 5.0F
        Wtxt.HorizontalAlignment = 1
        pdf_W.AddCell(Wtxt)

        '//平均值
        If Session("language").ToString = "en" Then
            sTitle = "Average"
        Else
            sTitle = "平均值"
        End If
        Dim AWtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//最大值
        If Session("language").ToString = "en" Then
            sTitle = "Maximum"
        Else
            sTitle = "最大值"
        End If
        Dim MWtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//用電量
        Dim pdf_KWH As PdfPTable = New PdfPTable(4)

        If Session("language").ToString = "en" Then
            sTitle = "Electricity consumption"
        Else
            sTitle = "用電量"
        End If
        Dim KWHtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, head_font))
        KWHtxt.PaddingBottom = 5.0F
        KWHtxt.Colspan = 4
        KWHtxt.HorizontalAlignment = 1
        pdf_KWH.AddCell(KWHtxt)

        '//尖峰
        If Session("language").ToString = "en" Then
            sTitle = "Peak Time"
        Else
            sTitle = "尖峰"
        End If
        Dim rushtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
        rushtxt.Colspan = 1
        rushtxt.HorizontalAlignment = 0
        rushtxt.PaddingBottom = 5.0F
        rushtxt.PaddingLeft = 20.0F
        rushtxt.NoWrap = True
        rushtxt.Border = 0
        pdf_KWH.AddCell(rushtxt)
        Dim rushmax As PdfPCell = New PdfPCell(New Phrase("" & Rush.Text & "KWH ", title_font))
        rushmax.Colspan = 3
        rushmax.HorizontalAlignment = 2
        rushmax.PaddingBottom = 5.0F
        rushmax.PaddingRight = 20.0F
        rushmax.Border = 0
        pdf_KWH.AddCell(rushmax)

        '//半尖峰
        If Session("language").ToString = "en" Then
            sTitle = "Half Peak Time"
        Else
            sTitle = "半尖峰"
        End If
        Dim halftxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
        halftxt.Colspan = 1
        halftxt.HorizontalAlignment = 0
        halftxt.PaddingBottom = 5.0F
        halftxt.PaddingLeft = 20.0F
        halftxt.NoWrap = True
        halftxt.Border = 0
        pdf_KWH.AddCell(halftxt)
        Dim halfmax As PdfPCell = New PdfPCell(New Phrase("" & Half.Text & "KWH ", title_font))
        halfmax.Colspan = 3
        halfmax.HorizontalAlignment = 2
        halfmax.PaddingBottom = 5.0F
        halfmax.PaddingRight = 20.0F
        halfmax.Border = 0
        pdf_KWH.AddCell(halfmax)

        '//週六半尖峰
        If Session("language").ToString = "en" Then
            sTitle = "Saturday Half Peak Time"
        Else
            sTitle = "週六半尖峰"
        End If
        Dim sathalftxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//離峰
        If Session("language").ToString = "en" Then
            sTitle = "Off Peak Time"
        Else
            sTitle = "離峰"
        End If
        Dim offtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
        offtxt.Colspan = 1
        offtxt.HorizontalAlignment = 0
        offtxt.PaddingBottom = 5.0F
        offtxt.PaddingLeft = 20.0F
        offtxt.NoWrap = True
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

        '//總計
        If Session("language").ToString = "en" Then
            sTitle = "Total"
        Else
            sTitle = "總計"
        End If
        Dim sumtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//電壓
        Dim pdf_V As PdfPTable = New PdfPTable(4)

        If Session("language").ToString = "en" Then
            sTitle = "Voltage"
        Else
            sTitle = "電壓"
        End If
        Dim Vtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, head_font))
        Vtxt.PaddingBottom = 5.0F
        Vtxt.Colspan = 4
        Vtxt.HorizontalAlignment = 1
        pdf_V.AddCell(Vtxt)

        '//平均值
        If Session("language").ToString = "en" Then
            sTitle = "Average"
        Else
            sTitle = "平均值"
        End If
        Dim AVtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//最大值
        If Session("language").ToString = "en" Then
            sTitle = "Maximum"
        Else
            sTitle = "最大值"
        End If
        Dim MVtxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//電流
        Dim pdf_I As PdfPTable = New PdfPTable(4)
        If Session("language").ToString = "en" Then
            sTitle = "Current"
        Else
            sTitle = "電流"
        End If

        Dim Itxt As PdfPCell = New PdfPCell(New Phrase(sTitle, head_font))
        Itxt.PaddingBottom = 5.0F
        Itxt.Colspan = 4
        Itxt.HorizontalAlignment = 1
        pdf_I.AddCell(Itxt)

        '//平均值
        If Session("language").ToString = "en" Then
            sTitle = "Average"
        Else
            sTitle = "平均值"
        End If
        Dim AItxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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

        '//最大值
        If Session("language").ToString = "en" Then
            sTitle = "Maximum"
        Else
            sTitle = "最大值"
        End If
        Dim MItxt As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
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
            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, head_font))
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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub
End Class
