Imports System.Data.OleDb
Imports System.Data
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class Chart
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
        Dim start_time As String = Request.QueryString("datetime").Split(",").GetValue(0)
        Dim end_time As String = Request.QueryString("datetime").Split(",").GetValue(1)
        Dim interval As String = Request.QueryString("interval")
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

        If Session("language").ToString = "en" Then
            Date_txt.Text = "Time：" & start_time & "~" & end_time
            Num_txt.Text = "Number："
            Position_txt.Text = "Position："
        Else
            Date_txt.Text = "時間：" & start_time & "~" & end_time
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

            Dim values() As String = value.Split(",")
            Dim item(values.Length - 1) As String
            For n = 0 To values.Length - 1
            item(n) = values(n).Split(" ").GetValue(2)
            Next
            Dim sqlstr(check_count_m - 1) As String
        Dim sql As String = ""
        Dim sTimeField As String = "時間"
        If Session("language").ToString = "en" Then sTimeField = "Time"

        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql &= "SELECT RecDate + ' ' + Rectime as RecDate," & value & " into #T" & i + 1 & " FROM ECO_" & Find_AdAccount(group) & "_PowerRecord " & _
                "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND (RecDate + ' ' + RecTime between '" & start_time & "' and '" & end_time & "');"
            Else
                sql &= "SELECT #T1.RecDate AS " & sTimeField & ","
                For j = 0 To item.Length - 1
                    For k = 0 To check_count_m - 1
                        sql &= "#T" & k + 1 & "." & item(j).Replace(" ", "-") & "+"
                    Next
                    sql = sql.Substring(0, sql.Length - 1)
                    If j <> item.Length Then
                        sql &= " AS " & item(j).Replace(" ", "-") & ","
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
                        sql &= "substring(Replace(Convert(nvarchar(16),#T" & k + 1 & ".RecDate,120),'-','/'),1,16) = substring(Replace(Convert(nvarchar(16),#T" & k + 2 & ".RecDate,120),'-','/'),1,16) and "
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
                        sql &= " (substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),16,1) Like '%5%' or substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),16,1) Like '%0%') order by " & sTimeField & ";"
                    Case "30"
                        sql &= " (substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%00%' or substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%30%') order by " & sTimeField & ";"
                    Case "60"
                        sql &= " substring(Replace(Convert(nvarchar(16),#T1.RecDate,120),'-','/'),15,2) Like '%00%' order by " & sTimeField & ";"
                End Select

                sql &= "Drop table "
                For k = 0 To check_count_m - 1
                    sql &= "#T" & k + 1 & ","
                Next
                sql = sql.Substring(0, sql.Length - 1)
            End If
        Next

        '時間間隔
        'Dim sql As String = "select RecDate AS 紀錄時間," & value

        'If interval = "1" Then
        '    sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
        'ElseIf interval = "5" Then
        '    sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
        'ElseIf interval = "30" Then
        '    sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
        'ElseIf interval = "60" Then
        '    sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
        'End If

        Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            'SqlDataSource1.SelectParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
            'SqlDataSource1.SelectParameters.Add("MeterID", TypeCode.String, meterid)
            'SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, start_time)
            'SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, end_time)
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv.Count > 0 Then
                Panel_Record.Visible = True
                Panel_Export.Visible = True
            Else
                Panel_Record.Visible = False
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
        End Try
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Print.Click
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Excel.Click
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String

        If Session("language").ToString = "en" Then
            sFilename = "Electricity Numerical record_" & Now()
        Else
            sFilename = "電力數值紀錄表_" & Now()
        End If
        'sFilename = Server.UrlPathEncode("電力數值紀錄表_" & Now())
        sFilename = Server.UrlPathEncode(sFilename)

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"
        Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>")

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)

        Panel_Record.RenderControl(htmlWrite)

        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.End()
    End Sub

    Protected Sub PDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PDF.Click
        '檔案名稱
        Dim sFilename As String
        Dim sTitle As String = ""
        If Session("language").ToString = "en" Then
            sFilename = Server.UrlPathEncode(" Electricity Numerical record_" & Now())
            sTitle = " Electricity Numerical record "
        Else
            sFilename = Server.UrlPathEncode(" 電力數值紀錄表_" & Now())
            sTitle = "電力數值紀錄表"
        End If

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.AddHeader("content-disposition", "attachment;filename=" + sFilename + ".pdf")  '檔名
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim stringWrite As System.IO.StringWriter = New StringWriter
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Dim reader As StringReader = New StringReader(stringWrite.ToString())
        '文件格式為橫式A4,若建構式不加引數,預設為直式A4
        Dim doc As Document = New Document(PageSize.A4.Rotate())
        PdfWriter.GetInstance(doc, Response.OutputStream)
        '處理中文
        Dim bfCh2 As BaseFont = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\MINGLIU.TTC,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
        Dim fontchinese As Font = New Font(bfCh2)

        'Dim fontpath As String = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\..\Fonts\dft_yf3.ttc" '設定選用文字樣式的檔案
        'FontFactory.Register(fontpath) '登記文字樣式路徑
        'Dim fontchinese As Font = FontFactory.GetFont("華康雅風體W3(P)", BaseFont.IDENTITY_H, 16.0F)
        doc.Open()

        '字型
        Dim title_font As Font = New Font(bfCh2, 16, Font.BOLD)
        Dim unit_font As Font = New Font(bfCh2, 14)
        Dim head_font As Font = New Font(bfCh2, 12, Font.BOLD)

        ' ----- PDF 顯示的內容 ------
        '要定義 PDF Table 的大小（Column 數量）
        Dim CountColumns As Integer = GridView1.HeaderRow.Cells.Count  '自動生成
        Dim CountRow As Integer = GridView1.Rows.Count
        Dim pdf As PdfPTable = New PdfPTable(CountColumns)

        pdf.HeaderRows = 1
        pdf.WidthPercentage = 100  '表格寬度百分比
        pdf.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '---寫入表頭
        'Dim title As PdfPCell = New PdfPCell(New Phrase("高德电子电能管理系统", title_font))
        'title.Colspan = CountColumns
        'title.Border = 0
        'title.HorizontalAlignment = 1
        'pdf.AddCell(title)

        Dim Header As PdfPCell = New PdfPCell(New Phrase(sTitle, title_font))
        Header.Border = 0
        Header.Colspan = CountColumns
        Header.HorizontalAlignment = 1
        pdf.AddCell(Header)

        Dim right As PdfPCell = New PdfPCell(New Phrase(Date_txt.Text & "", unit_font))
        right.Colspan = CountColumns
        right.Border = 0
        right.PaddingLeft = 50.0F
        pdf.AddCell(right)

        Dim ctrlnr As PdfPCell = New PdfPCell(New Phrase(Num_txt.Text & "", unit_font))
        ctrlnr.Colspan = CountColumns
        ctrlnr.Border = 0
        ctrlnr.PaddingLeft = 50.0F
        pdf.AddCell(ctrlnr)

        Dim nesthousing As PdfPCell = New PdfPCell(New Phrase(Position_txt.Text & "", unit_font))
        nesthousing.PaddingBottom = 10.0F
        nesthousing.Colspan = CountColumns
        nesthousing.Border = 0
        nesthousing.PaddingLeft = 50.0F
        pdf.AddCell(nesthousing)

        'pdf.TotalWidth = GridView1.HeaderRow.Width.Value
        'pdf.LockedWidth = False
        Dim pdf2 As PdfPTable = New PdfPTable(CountColumns + 1)

        pdf2.HeaderRows = 1
        pdf2.WidthPercentage = 100  '表格寬度百分比
        pdf2.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '--- 表格標題
        '如果要自訂標題，就不要用迴圈跑
        Dim head As PdfPCell
        Dim i, j As Integer
        For i = 0 To CountColumns - 1

            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, head_font))
            If i = 0 Then
                head.Colspan = 2
            End If
            head.HorizontalAlignment = 1
            head.PaddingBottom = 5.0F
            'head.BackgroundColor = New BaseColor(255, 255, 204)
            'head.NoWrap = True
            pdf2.AddCell(head)
        Next
        '--- 表格內文
        '利用迴圈規則性的填入內文
        Dim table As PdfPCell
        For i = 0 To CountRow - 1
            For j = 0 To CountColumns - 1
                table = New PdfPCell(New Phrase(GridView1.Rows(i).Cells(j).Text, fontchinese))
                If j = 0 Then
                    table.Colspan = 2
                End If
                'table.Width = GridView1.Rows(i).Cells(j).Text.Length
                table.HorizontalAlignment = 1
                table.PaddingBottom = 5.0F
                'If i Mod 2 Then
                '    table.BackgroundColor = New BaseColor(255, 255, 204)
                'End If
                'table.NoWrap = True
                pdf2.AddCell(table)
            Next
            'pdf.CompleteRow()  ' 強迫跳到下一個 Row
        Next
        doc.Add(pdf)
        doc.Add(pdf2)
        doc.Close()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub
End Class
