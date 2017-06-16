Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports System.Data

Partial Class EventRecord
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
        Dim type As String = Request.QueryString("type")
        Dim start_time As String = Request.QueryString("datetime").Split(",").GetValue(0)
        Dim end_time As String = Request.QueryString("datetime").Split(",").GetValue(1)


        Dim strIndex, strAccount, strNumber, strTime, strLocation, strError, strMsg, strMail, strTel, strEmail As String
        Dim strTitle As String
        If Session("language") = "en" Then
            strIndex = "Index" : strAccount = "Account" : strNumber = "Number" : strTime = "Time" : strLocation = "Location" : strError = "Error"
            strMsg = "Message_sending" : strMail = "Mail_Sending" : strTel = "Telephone_number" : strEmail = "EMail"
            strTitle = "Date：" & start_time & "~" & end_time
        Else
            strIndex = "索引" : strAccount = "帳號" : strNumber = "編號" : strTime = "時間" : strLocation = "位置" : strError = "錯誤內容"
            strMsg = "簡訊發送" : strMail = "信件發送" : strTel = "電話號碼" : strEmail = "電子信箱"
            strTitle = "日期：" & start_time & "~" & end_time
        End If
        Date_txt.Text = strTitle
        'Date_txt.Text = "日期：" & start_time & "~" & end_time

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString
        Dim sql As String = ""
        If type <> 0 Then
            sql = "SELECT distinct ER.EventIndex AS " & strIndex & ",ER.ECO_Account AS " & strAccount & ",ER.MeterID AS " & strNumber & ",ER.RecDate AS " & strTime & ",ER.InstallPosition AS " & strLocation & ",ER.ErrContent AS " & strError & "," & _
                  "ER.SMSSent AS " & strMsg & ",ER.EmailSent AS " & strMail & ",ER.Num AS " & strTel & ",ER.Mail AS " & strEmail & " " & _
                  "FROM ControllerSetup AS CS,MeterSetup AS MS,EventRecord AS ER " & _
                  "WHERE CS.Account = '" & Find_AdAccount(group) & "' AND CS.ECO_Account = MS.ECO_Account AND MS.ECO_Account = ER.ECO_Account AND ER.ErrType = " & type & " AND " & _
                  "ER.recdate BETWEEN '" & start_time & "' AND '" & end_time & "' ORDER BY EventIndex DESC"
        Else
            sql = "SELECT distinct ER.EventIndex AS " & strIndex & ",ER.ECO_Account AS " & strAccount & ",ER.MeterID AS " & strNumber & ",ER.RecDate AS " & strTime & ",ER.InstallPosition AS " & strLocation & ",ER.ErrContent AS " & strError & "," & _
                  "ER.SMSSent AS " & strMsg & ",ER.EmailSent AS " & strMail & ",ER.Num AS " & strTel & ",ER.Mail AS " & strEmail & " " & _
                  "FROM ControllerSetup AS CS,MeterSetup AS MS,EventRecord AS ER " & _
                  "WHERE CS.Account = '" & Find_AdAccount(group) & "' AND CS.ECO_Account = MS.ECO_Account AND MS.ECO_Account = ER.ECO_Account AND " & _
                  "ER.recdate BETWEEN '" & start_time & "' AND '" & end_time & "' ORDER BY EventIndex DESC"
        End If

        Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
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
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("事件紀錄表_" & Now())

        'Using DataGrid1 As New DataGrid
        '    DataGrid1.DataSource = SqlDataSource1
        '    DataGrid1.DataBind()

        '    If DataGrid1.Items.Count = 0 Then
        '        DataGrid1.Visible = False
        '        Dim strScript As String
        '        strScript = "<script>" & ControlChars.CrLf & "alert('查無資料');" & ControlChars.CrLf & "</script>"
        '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ClientScript", strScript)
        '    Else
        '        Dim stringWrite As StringWriter = New System.IO.StringWriter
        '        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)

        '        DataGrid1.RenderControl(htmlWrite)
        '        Response.Clear()
        '        Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8")
        '        Response.ContentType = "application/ms-excel"
        '        Response.Charset = "big5"
        '        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        '        'Response.Write("<meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"">")
        '        Response.Write(style)
        '        Response.Write("<center><font size='5'><B>事件紀錄表</B></font></center>")
        '        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        '        Response.End()
        '    End If
        'End Using

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Record.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.End()
    End Sub

    Protected Sub PDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PDF.Click
        '檔案名稱
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("事件紀錄表_" & Now())

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.AddHeader("content-disposition", "attachment;filename=" + sFilename + ".pdf")  '檔名
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim stringWrite As System.IO.StringWriter = New StringWriter
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Dim reader As StringReader = New StringReader(stringWrite.ToString())
        '文件格式為橫式A4,若建構式不加引數,預設為直式A4
        Dim doc As Document = New Document(PageSize.A4.Rotate(), 10, 10, 20, 20)
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
        Dim unit_font As Font = New Font(bfCh2, 12)
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

        Dim Header As PdfPCell = New PdfPCell(New Phrase("事件紀錄表", title_font))
        Header.Border = 0
        Header.Colspan = CountColumns
        Header.HorizontalAlignment = 1
        pdf.AddCell(Header)

        Dim datetime As PdfPCell = New PdfPCell(New Phrase(Date_txt.Text & "", unit_font))
        datetime.Colspan = CountColumns
        datetime.Border = 0
        datetime.PaddingLeft = 30.0F
        datetime.PaddingBottom = 5.0F
        pdf.AddCell(datetime)

        'pdf.TotalWidth = GridView1.HeaderRow.Width.Value
        'pdf.LockedWidth = False
        Dim pdf2 As PdfPTable = New PdfPTable(New Single() {0.5, 1.2, 0.4, 1.0, 1.7, 1.2, 0.7, 0.7, 0.8, 2.0})

        pdf2.HeaderRows = 1
        pdf2.WidthPercentage = 100  '表格寬度百分比
        pdf2.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '--- 表格標題
        '如果要自訂標題，就不要用迴圈跑
        Dim head As PdfPCell
        Dim i, j As Integer
        For i = 0 To CountColumns - 1
            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, head_font))
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
                If GridView1.Rows(i).Cells(j).Text <> "" Then
                    table = New PdfPCell(New Phrase(GridView1.Rows(i).Cells(j).Text.Replace("&nbsp;", ""), unit_font))
                Else
                    table = New PdfPCell(New Phrase("", unit_font))
                End If
                'table.Width = GridView1.Rows(i).Cells(j).Text.Length
                table.HorizontalAlignment = 1
                table.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                table.PaddingBottom = 5.0F
                'If i Mod 2 Then
                '    table.BackgroundColor = New BaseColor(255, 255, 204)
                'End If
                'table.NoWrap = True
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

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Attributes.Add("class", "text")
            e.Row.Cells(2).Attributes.Add("class", "text")
            e.Row.Cells(3).Attributes.Add("class", "text") '指定欄位為文字樣式
            e.Row.Cells(8).Attributes.Add("class", "text")

            If e.Row.Cells(6).Text = "True" Then
                e.Row.Cells(6).Text = "Yes"
            Else
                e.Row.Cells(6).Text = "No"
            End If

            If e.Row.Cells(7).Text = "True" Then
                e.Row.Cells(7).Text = "Yes"
            Else
                e.Row.Cells(7).Text = "No"
            End If
        End If
    End Sub

End Class
