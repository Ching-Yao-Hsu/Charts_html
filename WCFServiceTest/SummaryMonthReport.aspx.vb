Imports System.Data.OleDb
Imports System.Data
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class SummaryMonthReport
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
        'Dim account As String = ""
        'If Session("Rank") = 2 Then
        '    account = Account_admin(Request.QueryString("account"))
        'Else
        '    account = Session("Account_admin")
        'End If
        Dim group As String = Request.QueryString("group")
        Dim datetime As String = Request.QueryString("datetime")
        Dim sYear As String = datetime.Substring(0, 4).ToString.Trim
        Dim value As String = Request.QueryString("value")




        '//Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("MasterConn").ToString()
        Dim sDBName As String = ""
        Dim sql As String = ""
        If group = "全部" Or group = "ALL" Then

            sql = "select * from ECOSMART.dbo.AdminSetup with (Nolock) where Enabled = 1 and CreateDB = 1 and Rank <> 0 and Account<>'admin' "

            Dim strPerNum As String = System.Web.Configuration.WebConfigurationManager.AppSettings("bPerNum")
            If strPerNum = "1" Then
                sql = sql & " and PerNum='" & Session("PerNum") & "' "
            End If

            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Using cmd As New OleDbCommand(sql, conn)
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        While (dr.Read() = True)
                            sDBName += dr("Account").ToString & ","
                        End While
                    End Using
                End Using
            End Using
            sDBName = Left(sDBName, Len(sDBName) - 1)
        Else
            sDBName = Find_AdAccount(group)
        End If

        Dim aDB() As String = sDBName.Split(",")
        Dim sColumn As String = ""
        Dim item() As String = value.Split(",")
        '//欄位
        For i = 0 To item.Length - 1
            Select Case item(i)
                Case "電流", "[Current]"
                    If item(i) = "[Current]" Then
                        sColumn &= ",Round(AVG(PRC.Iavg),1) AS Average1, Round(MAX(PRC.Imax),1) AS Maximum1"  '//電流
                    Else
                        sColumn &= ",Round(AVG(PRC.Iavg),1) AS 平均值1, Round(MAX(PRC.Imax),1) AS 最大值1"    '//電流
                    End If
                Case "電壓", "Voltage"
                    If item(i) = "Voltage" Then
                        sColumn &= ",Round(AVG(PRC.Vavg),0) AS Average2, Round(MAX(PRC.Vmax),0) AS Maximum2"    '//電壓
                    Else
                        sColumn &= ",Round(AVG(PRC.Vavg),0) AS 平均值2, Round(MAX(PRC.Vmax),0) AS 最大值2"    '//電壓
                    End If
                Case "功率", "Power"
                    If item(i) = "Power" Then
                        sColumn &= ",Round(AVG(PRC.Wavg),1) AS Average3, Round(MAX(PRC.Wmax),1) AS Maximum3"    '//功率
                    Else
                        sColumn &= ",Round(AVG(PRC.Wavg),1) AS 平均值3, Round(MAX(PRC.Wmax),1) AS 最大值3"    '//功率
                    End If
                Case "最大需量", "Max_Demand"
                    If item(i) = "Max_Demand" Then
                        sColumn &= ",isnull(MAX(PRC.Demand),0) AS Peak_Time_Demand, isnull(MAX(PRC.DemandHalf),0) AS Half_Peak_Time_Demand " & _
                                   ",isnull(MAX(PRC.DemandSatHalf),0) AS Saturday_Half_Peak_Time_Demand, isnull(MAX(PRC.DemandOff),0) AS Off_Peak_Time_Demand "
                    Else
                        sColumn &= ",isnull(MAX(PRC.Demand),0) AS 尖峰需量, isnull(MAX(PRC.DemandHalf),0) AS 半尖峰需量 " & _
                                   ",isnull(MAX(PRC.DemandSatHalf),0) AS 週六半尖峰需量, isnull(MAX(PRC.DemandOff),0) AS 離峰需量 "
                    End If
                Case "用電量", "Electricity_consumption"
                    If item(i) = "Electricity_consumption" Then
                        sColumn &= ",MAX(PRC.RushHourMax) AS Peak_Time_Electricity, MAX(PRC.HalfHourMax) AS Half_Peak_Time_Electricity" & _
                                   ",MAX(PRC.SatHalfHourMax) AS Saturday_Half_Peak_Time_Electricity, MAX(PRC.OffHourMax) AS Off_Peak_Time_Electricity" & _
                                   ",MAX(PRC.RushHourMax)+MAX(PRC.HalfHourMax)+MAX(PRC.SatHalfHourMax)+MAX(PRC.OffHourMax) AS Total "
                    Else
                        sColumn &= ",MAX(PRC.RushHourMax) AS 尖峰電量, MAX(PRC.HalfHourMax) AS 半尖峰電量" & _
                                   ",MAX(PRC.SatHalfHourMax) AS 週六半尖峰電量, MAX(PRC.OffHourMax) AS 離峰電量" & _
                                   ",MAX(PRC.RushHourMax)+MAX(PRC.HalfHourMax)+MAX(PRC.SatHalfHourMax)+MAX(PRC.OffHourMax) AS 總計 "
                    End If
                Case "電表值", "ElectricityValue"
                    If item(i) = "ElectricityValue" Then
                        sColumn &= ",Min(IsNull(PRC.StarKWh,0)) AS StarKWh,Max(IsNull(PRC.KWh,0)) AS KWh "
                    Else
                        sColumn &= ",Min(IsNull(PRC.StarKWh,0)) AS 電表值_起,Max(IsNull(PRC.KWh,0)) AS 電表值_迄  "
                    End If
            End Select
        Next

        sql = ""
        For DD As Integer = 0 To aDB.Length - 1
            If DD > 0 Then  sql += " Union "

            If group = "全部" Or group = "ALL" Then
                If group = "ALL" Then
                    sql += "SELECT CS.Account as Account, RTrim(PRC.CtrlNr) + '-' + RTrim(PRC.MeterID) AS Number, MS.InstallPosition AS switchboard "
                Else
                    sql += "SELECT CS.Account as 帳號, RTrim(PRC.CtrlNr) + '-' + RTrim(PRC.MeterID) AS 編號, MS.InstallPosition AS 配電盤 "
                End If
            Else
                If Session("language").ToString = "en" Then
                    sql += "SELECT  RTrim(PRC.CtrlNr) + '-' + RTrim(PRC.MeterID) AS Number, MS.InstallPosition AS switchboard "
                Else
                    sql += "SELECT  RTrim(PRC.CtrlNr) + '-' + RTrim(PRC.MeterID) AS 編號, MS.InstallPosition AS 配電盤 "
                End If
            End If
            sql += sColumn & _
                  " FROM ECO_" & aDB(DD).ToString & "_PowerRecordCollection AS PRC, ECOSMART.dbo.ControllerSetup AS CS, ECOSMART.dbo.MeterSetup AS MS " & _
                  " WHERE PRC.CtrlNr=MS.CtrlNr and PRC.MeterID=MS.MeterID and CS.ECO_Account=MS.ECO_Account and CS.Account='~~' and SUBSTRING(PRC.RecDate,1,7)='" & datetime & "' " & _
                  " GROUP BY CS.Account,SubString(PRC.RecDate,1,7), PRC.CtrlNr, PRC.MeterID, MS.InstallPosition "

            sql = Replace(sql, "~~", aDB(DD).ToString)
        Next

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            'Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv.Count > 0 Then
                If Session("language").ToString = "en" Then
                    RecDate.Text = "Month：" & datetime
                Else
                    RecDate.Text = "月份：" & datetime
                End If
                Panel_Report.Visible = True
                Panel_Export.Visible = True
                GridView1.Attributes("border") = "1"
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
            'Catch ex As Exception
            '    ex.ToString()
            'End Try
        End Using
    End Sub

    Protected Sub GridView1_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            '此方法輸出Excel會友空白行，只是移除表頭內容，<tr>還存在
            '將原有的表頭移除()
            'Dim oldCell As TableCellCollection = e.Row.Cells
            'oldCell.Clear()

            Dim ColumnCount As Integer = 0
            '多重表頭的第一列
            Dim gvRow As GridViewRow = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            '第一欄
            Dim tc As TableCell = New TableCell()
            If Session("language").ToString = "en" Then
                tc.Text = "Meter information"
            Else
                tc.Text = "電表資訊"
            End If
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Middle
            tc.BackColor = System.Drawing.Color.White
            tc.ForeColor = System.Drawing.Color.Black
            tc.RowSpan = 1 '所跨的row數

            Dim group As String = Request.QueryString("group")
            If group = "全部" Or group = "ALL" Then
                tc.ColumnSpan = 3 '所跨的column數
            Else
                tc.ColumnSpan = 2 '所跨的column數
            End If
            gvRow.Cells.Add(tc) '新增
            ColumnCount += 2

            Dim count As Integer = 0
            Dim value As String = Request.QueryString("value")
            Dim item() As String = value.Split(",")

            Dim strVag As String = "", strMax As String = ""
            If Session("language").ToString = "en" Then
                strVag = "Average" : strMax = "Maximum"
            Else
                strVag = "平均值" : strMax = "最大值"
            End If

            For i = 0 To item.Length - 1
                Select Case item(i)
                    Case "電流", "[Current]"
                        tc = New TableCell()
                        If item(i) = "[Current]" Then
                            tc.Text = "Current(A)"
                        Else
                            tc.Text = "電流(A)"
                        End If
                        tc.HorizontalAlign = HorizontalAlign.Center
                        tc.VerticalAlign = VerticalAlign.Middle
                        tc.BackColor = System.Drawing.Color.White
                        tc.ForeColor = System.Drawing.Color.Black
                        tc.RowSpan = 1
                        tc.ColumnSpan = 2
                        gvRow.Cells.Add(tc) '新增
                        If count = 0 Then
                            e.Row.Cells(count + 2).Text = strVag
                            e.Row.Cells(count + 3).Text = strMax
                        End If
                        count += 2
                        ColumnCount += 2
                    Case "電壓", "Voltage"
                        tc = New TableCell()
                        If item(i) = "Voltage" Then
                            tc.Text = "Voltage(V)"
                        Else
                            tc.Text = "電壓(V)"
                        End If
                        tc.HorizontalAlign = HorizontalAlign.Center
                        tc.VerticalAlign = VerticalAlign.Middle
                        tc.BackColor = System.Drawing.Color.White
                        tc.ForeColor = System.Drawing.Color.Black
                        tc.RowSpan = 1
                        tc.ColumnSpan = 2
                        gvRow.Cells.Add(tc) '新增

                        If count = 0 Then
                            e.Row.Cells(count + 2).Text = strVag
                            e.Row.Cells(count + 3).Text = strMax
                        ElseIf count = 2 Then
                            e.Row.Cells(count + 2).Text = strVag
                            e.Row.Cells(count + 3).Text = strMax
                        End If
                        count += 2
                        ColumnCount += 2

                    Case "功率", "Power"
                        tc = New TableCell()
                        If item(i) = "Power" Then
                            tc.Text = "Power(kw)"
                        Else
                            tc.Text = "功率(kw)"
                        End If
                        tc.HorizontalAlign = HorizontalAlign.Center
                        tc.VerticalAlign = VerticalAlign.Middle
                        tc.BackColor = System.Drawing.Color.White
                        tc.ForeColor = System.Drawing.Color.Black
                        tc.RowSpan = 1
                        tc.ColumnSpan = 2
                        gvRow.Cells.Add(tc) '新增
                        If count = 0 Then
                            e.Row.Cells(count + 2).Text = strVag
                            e.Row.Cells(count + 3).Text = strMax
                        ElseIf count = 2 Then
                            e.Row.Cells(count + 2).Text = strVag
                            e.Row.Cells(count + 3).Text = strMax
                        ElseIf count = 4 Then
                            e.Row.Cells(count + 2).Text = strVag
                            e.Row.Cells(count + 3).Text = strMax
                        End If
                        count += 2
                        ColumnCount += 2
                    Case "最大需量", "Max_Demand"
                        tc = New TableCell()
                        If item(i) = "Max_Demand" Then
                            tc.Text = "Max Demand(kw)"
                        Else
                            tc.Text = "最大需量(kw)"
                        End If
                        tc.HorizontalAlign = HorizontalAlign.Center
                        tc.VerticalAlign = VerticalAlign.Middle
                        tc.BackColor = System.Drawing.Color.White
                        tc.ForeColor = System.Drawing.Color.Black
                        tc.RowSpan = 1
                        tc.ColumnSpan = 4
                        gvRow.Cells.Add(tc) '新增

                        If count = 0 Or count = 2 Or count = 4 Or count = 6 Then
                            If item(i) = "Max_Demand" Then
                                e.Row.Cells(count + 2).Text = "Peak_Time"
                                e.Row.Cells(count + 3).Text = "Half_Peak_Time"
                                e.Row.Cells(count + 4).Text = "Saturday_Half_Peak_Time"
                                e.Row.Cells(count + 5).Text = "Off_Peak_Time"
                            Else
                                e.Row.Cells(count + 2).Text = "尖峰"
                                e.Row.Cells(count + 3).Text = "半尖峰"
                                e.Row.Cells(count + 4).Text = "週六半尖峰"
                                e.Row.Cells(count + 5).Text = "離峰"
                            End If
                        End If
                        count += 4
                        ColumnCount += 4
                    Case "用電量", "Electricity_consumption"
                        tc = New TableCell()
                        If item(i) = "Electricity_consumption" Then
                            tc.Text = "Electricity consumption(KWH)"
                        Else
                            tc.Text = "用電量(KWH)"
                        End If
                        tc.HorizontalAlign = HorizontalAlign.Center
                        tc.VerticalAlign = VerticalAlign.Middle
                        tc.BackColor = System.Drawing.Color.White
                        tc.ForeColor = System.Drawing.Color.Black
                        tc.RowSpan = 1
                        tc.ColumnSpan = 5
                        gvRow.Cells.Add(tc) '新增

                        If count = 0 Or count = 2 Or count = 4 Or count = 6 Or count = 8 Or count = 10 Then
                            If item(i) = "Electricity_consumption" Then
                                e.Row.Cells(count + 2).Text = "Peak_Time"
                                e.Row.Cells(count + 3).Text = "Half_Peak_Time"
                                e.Row.Cells(count + 4).Text = "Saturday_Half_Peak_Time"
                                e.Row.Cells(count + 5).Text = "Off_Peak_Time"
                            Else
                                e.Row.Cells(count + 2).Text = "尖峰"
                                e.Row.Cells(count + 3).Text = "半尖峰"
                                e.Row.Cells(count + 4).Text = "週六半尖峰"
                                e.Row.Cells(count + 5).Text = "離峰"
                            End If
                        End If
                        ColumnCount += 5
                    Case "電表值", "ElectricityValue"
                        tc = New TableCell()
                        If item(i) = "ElectricityValue" Then
                            tc.Text = "ElectricityValue(KWH)"
                        Else
                            tc.Text = "電表值(KWH)"
                        End If
                        tc.HorizontalAlign = HorizontalAlign.Center
                        tc.VerticalAlign = VerticalAlign.Middle
                        tc.BackColor = System.Drawing.Color.White
                        tc.ForeColor = System.Drawing.Color.Black
                        tc.RowSpan = 1
                        tc.ColumnSpan = 2
                        gvRow.Cells.Add(tc) '新增

                        If count = 0 Or count = 2 Or count = 4 Or count = 6 Or count = 8 Or count = 10 Then
                            If item(i) = "ElectricityValue" Then
                                e.Row.Cells(count + 2).Text = "StarKWh"
                                e.Row.Cells(count + 3).Text = "KWh"
                            Else
                                e.Row.Cells(count + 2).Text = "電表值_起"
                                e.Row.Cells(count + 3).Text = "電表值_迄"
                            End If
                        End If
                        ColumnCount += 2
                End Select
            Next
            Session("ColumnCount") = ColumnCount

            '把這一列加到最上面
            GridView1.Controls(0).Controls.AddAt(0, gvRow)
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("class", "text") '指定欄位為文字樣式
        End If
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Print.Click
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(sender As Object, e As ImageClickEventArgs) Handles Excel.Click
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        Dim sFilename As String
        If Session("language").ToString = "en" Then
            sFilename = "Summary Monthly report_" & Request.QueryString("datetime")
        Else
            sFilename = "總月報表_" & Request.QueryString("datetime")
        End If
        'sFilename = Server.UrlPathEncode("總月報表_" & Request.QueryString("datetime"))
        sFilename = Server.UrlPathEncode(sFilename)


        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "utf-8"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.ContentType = "application/ms-excel"
        Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>")

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write(stringWrite.ToString)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles PDF.Click
        Dim account As String = ""
        If Session("Rank") = 2 Then
            account = Account_admin(Request.QueryString("account"))
        Else
            account = Session("Account_admin")
        End If
        Dim datetime As String = Request.QueryString("datetime")
        Dim value As String = Request.QueryString("value")

        Dim sFilename As String
        Dim sTitle As String = ""
        If Session("language").ToString = "en" Then
            sFilename = Server.UrlPathEncode("Summary Monthly report_" & datetime)
            sTitle = "Summary Monthly report"
        Else
            sFilename = Server.UrlPathEncode("總月報表_" & datetime)
            sTitle = "總月報表"
        End If
        'Dim sFilename As String = Server.UrlPathEncode("總月報表_" & datetime)

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.AddHeader("content-disposition", "attachment;filename=" + sFilename + ".pdf")  '檔名
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim stringWrite As System.IO.StringWriter = New StringWriter
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Dim reader As StringReader = New StringReader(stringWrite.ToString())
        Dim doc As Document = New Document(PageSize.A4.Rotate(), 5, 5, 10, 10)    '設定PageSize, Margin, left, right, top, bottom
        PdfWriter.GetInstance(doc, Response.OutputStream)
        '處理中文
        Dim bfCh2 As BaseFont = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\MINGLIU.TTC,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
        Dim fontchinese As Font = New Font(bfCh2)
        doc.Open()

        '要定義 PDF Table 的大小（Column 數量）
        Dim CountColumns As Integer = Session("ColumnCount")   '自動生成
        Dim CountRow As Integer = GridView1.Rows.Count
        Dim pdf As PdfPTable = New PdfPTable(CountColumns)

        pdf.HeaderRows = 1
        pdf.WidthPercentage = 100  '表格寬度百分比
        pdf.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '字型
        Dim title_font As Font = New Font(bfCh2, 14, Font.BOLD)
        Dim unit_font As Font = New Font(bfCh2, 10)
        Dim sum_font As Font = New Font(bfCh2, 14, Font.BOLD, BaseColor.RED)
        Dim head_font As Font = New Font(bfCh2, 10, Font.BOLD)
        Dim head_font2 As Font = New Font(bfCh2, 10)

        '表頭說明文字
        'Dim Header1 As PdfPCell = New PdfPCell(New Phrase("台灣國際造船股份有限公司 - 基隆廠", title_font))
        'Header1.Colspan = CountColumns
        'Header1.Border = 0
        'Header1.HorizontalAlignment = 1
        'pdf.AddCell(Header1)

        Dim Header2 As PdfPCell = New PdfPCell(New Phrase(sTitle, title_font))
        Header2.Colspan = CountColumns
        Header2.Border = 0
        Header2.HorizontalAlignment = 1
        pdf.AddCell(Header2)

        '日期
        If Session("language").ToString = "en" Then
            sTitle = "Date：" & datetime
        Else
            sTitle = "日期：" & datetime
        End If
        Dim cell_date As PdfPCell = New PdfPCell(New Phrase(sTitle, unit_font))
        cell_date.Colspan = CountColumns
        cell_date.Border = 0
        cell_date.PaddingBottom = 5.0F
        pdf.AddCell(cell_date)

        '表格相對寬度
        Dim item() As String = value.Split(",")
        Dim pdf2 As PdfPTable = New PdfPTable(CountColumns)
        If CountColumns = 17 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1, 1, 1.1, 1.1, 1.5, 1.1, 1.1, 1.1, 1.5, 1.1, 1.1})
        ElseIf CountColumns = 15 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1.1, 1.1, 1.5, 1.1, 1.1, 1.1, 1.5, 1.1, 1.1})
        ElseIf CountColumns = 13 Then
            If Array.IndexOf(item, "最大需量") = -1 Or Array.IndexOf(item, "Max_Demand") = -1 Then
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1, 1, 1.1, 1.1, 1.5, 1.1, 1.1})
            Else
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1.1, 1.1, 1.5, 1.1, 1.1, 1.1, 1.5, 1.1, 1.1})
            End If
        ElseIf CountColumns = 12 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1, 1, 1.1, 1.5, 1.1, 1.1})
        ElseIf CountColumns = 11 Then
            If Array.IndexOf(item, "最大需量") = -1 Or Array.IndexOf(item, "Max_Demand") = -1 Then
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1.1, 1.1, 1.5, 1.1, 1.1})
            Else
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1.1, 1.1, 1.5, 1.1, 1.1, 1.1, 1.5, 1.1, 1.1})
            End If
        ElseIf CountColumns = 10 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1.1, 1.5, 1.1, 1.1})
        ElseIf CountColumns = 9 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1.1, 1.1, 1.5, 1.1, 1.1})
        ElseIf CountColumns = 8 Then
            If Array.IndexOf(item, "最大需量") = -1 Or Array.IndexOf(item, "Max_Demand") = -1 Then
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1, 1})
            Else
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1, 1.1, 1.1, 1.5, 1.1})
            End If
        ElseIf CountColumns = 7 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1.1, 1.1, 1.5, 1.1, 1.1})
        ElseIf CountColumns = 6 Then
            If Array.IndexOf(item, "最大需量") = -1 Or Array.IndexOf(item, "Max_Demand") = -1 Then
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1, 1, 1})
            Else
                pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1.1, 1.1, 1.5, 1.1})
            End If
        ElseIf CountColumns = 4 Then
            pdf2 = New PdfPTable(New Single() {0.6, 2.6, 1, 1})
        End If

        pdf2.HeaderRows = 2
        pdf2.WidthPercentage = 100  '表格寬度百分比
        pdf2.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '--- 表格標題

        If Session("language").ToString = "en" Then
            sTitle = "Meter information"
        Else
            sTitle = "電表資訊"
        End If
        Dim title As PdfPCell = New PdfPCell(New Phrase(sTitle, head_font2))
        title.Colspan = 2
        title.HorizontalAlignment = 1
        title.PaddingBottom = 5.0F
        pdf2.AddCell(title)

        For k = 0 To item.Length - 1
            Select Case item(k)
                Case "電流", "[Current]"
                    If item(k) = "[Current]" Then
                        sTitle = "Current(A)"
                    Else
                        sTitle = "電流(A)"
                    End If
                    title = New PdfPCell(New Phrase(sTitle, head_font2))
                    title.Colspan = 2
                    title.HorizontalAlignment = 1
                    title.PaddingBottom = 5.0F
                    pdf2.AddCell(title)
                Case "電壓", "Voltage"
                    If item(k) = "Voltage" Then
                        sTitle = "Voltage(V)"
                    Else
                        sTitle = "電壓(V)"
                    End If
                    title = New PdfPCell(New Phrase(sTitle, head_font2))
                    title.Colspan = 2
                    title.HorizontalAlignment = 1
                    title.PaddingBottom = 5.0F
                    pdf2.AddCell(title)
                Case "功率", "Power"
                    If item(k) = "Power" Then
                        sTitle = "Power(kw)"
                    Else
                        sTitle = "功率(kw)"
                    End If
                    title = New PdfPCell(New Phrase(sTitle, head_font2))
                    title.Colspan = 2
                    title.HorizontalAlignment = 1
                    title.PaddingBottom = 5.0F
                    pdf2.AddCell(title)
                Case "最大需量", "Max_Demand"
                    If item(k) = "Max_Demand" Then
                        sTitle = "Max_Demand(kw)"
                    Else
                        sTitle = "最大需量(kw)"
                    End If
                    title = New PdfPCell(New Phrase(sTitle, head_font2))
                    title.Colspan = 4
                    title.HorizontalAlignment = 1
                    title.PaddingBottom = 5.0F
                    pdf2.AddCell(title)
                Case "用電量", "Electricity_consumption"
                    If item(k) = "Electricity_consumption" Then
                        sTitle = "Electricity_consumption(KWH)"
                    Else
                        sTitle = "用電量(KWH)"
                    End If
                    title = New PdfPCell(New Phrase(sTitle, head_font2))
                    title.Colspan = 5
                    title.HorizontalAlignment = 1
                    title.PaddingBottom = 5.0F
                    pdf2.AddCell(title)
                Case "電表值", "ElectricityValue"
                    If item(k) = "ElectricityValue" Then
                        sTitle = "ElectricityValue(KWH)"
                    Else
                        sTitle = "電表值(KWH)"
                    End If
                    title = New PdfPCell(New Phrase(sTitle, head_font2))
                    title.Colspan = 2
                    title.HorizontalAlignment = 1
                    title.PaddingBottom = 5.0F
                    pdf2.AddCell(title)
            End Select
        Next

        '如果要自訂標題，就不要用迴圈跑
        Dim head As PdfPCell
        Dim i, j As Integer
        For i = 0 To CountColumns - 1
            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, head_font))
            head.HorizontalAlignment = 1
            head.VerticalAlignment = 2
            head.PaddingBottom = 5.0F
            'head.NoWrap = True         '//表頭不會自動折行
            pdf2.AddCell(head)
        Next
        '--- 表格內文
        '利用迴圈規則性的填入內文
        Dim table As PdfPCell
        For i = 0 To CountRow - 1
            For j = 0 To CountColumns - 1
                table = New PdfPCell(New Phrase(GridView1.Rows(i).Cells(j).Text, unit_font))
                table.HorizontalAlignment = 1
                table.VerticalAlignment = 2
                table.PaddingBottom = 5.0F
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
