Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class WaterMonthReport
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            Dim sRecDate As String
            If Not IsPostBack Then
                sRecDate = Request.QueryString("sRecdate")
            Else
                sRecDate = CYMD(txtRecDate.Text)
            End If
            Label1.Text = "日期：" & sRecDate
            txtRecDate.Text = sRecDate
            Label1.Visible = False
            SqlQuery(sRecDate)
        End If
    End Sub

    Protected Sub SqlQuery(ByVal sRecDate As String)
        SqlDataSource1.SelectParameters.Clear()

        Label1.Text = "月份：" & sRecdate

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=Water"

        Try
            Dim strSQL As String = "  Select A.ID as 編號,  A.圖面編號,  A.店鋪編號, A.店鋪名稱 " & _
                                   " , isnull((Select Round(Sum(B.Amount),4) From Record B With (NoLock) " & _
                                   " Where B.ID=A.ID and SubString(B.RecDate,1,7) = '" & sRecDate & "'), 0) AS 水量 " & _
                                   " FROM [Water].[dbo].[Setup] A with (Nolock)  Order by A.ID "

            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = strSQL
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv Is Nothing Then
                Panel_Report.Visible = True
                Panel_Export.Visible = True
                Dim msg As String = "查無資料"
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "訊息:用水量表" & msg, True)
            Else
                If dv.Count > 0 Then
                    Panel_Report.Visible = True
                    Panel_Export.Visible = True
                Else
                    Panel_Report.Visible = True
                    Panel_Export.Visible = True
                    Dim msg As String = "查無資料"
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "訊息:用水量表" & msg, True)
                End If

            End If
        Catch ex As Exception
            ex.ToString()
            Panel_Export.Visible = False
            Panel_Report.Visible = False
        End Try
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        SqlQuery(txtRecDate.Text)
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(sender As Object, e As ImageClickEventArgs) Handles Excel.Click
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("總月報表_" & Now())

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"

        Label1.Visible = True : Label5.Visible = False : txtRecDate.Visible = False : ButtonSM.Visible = False
        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Label1.Visible = False : Label5.Visible = True : txtRecDate.Visible = True : ButtonSM.Visible = True
        Response.End()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles pdf.Click
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("總月報表_" & Now())

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
        Dim CountColumns As Integer = 6   '自動生成
        Dim CountRow As Integer = 2
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
        Dim Header2 As PdfPCell = New PdfPCell(New Phrase("總日報表", title_font))
        Header2.Colspan = CountColumns
        Header2.Border = 0
        Header2.HorizontalAlignment = 1
        pdf.AddCell(Header2)

        '空白行
        Dim space1 As PdfPCell = New PdfPCell(New Phrase(" "))
        space1.Colspan = CountColumns
        space1.Border = 0
        pdf.AddCell(space1)


        Dim datetxt As PdfPCell = New PdfPCell(New Phrase(Label1.Text.ToString, unit_font))
        datetxt.Colspan = CountColumns
        datetxt.Border = 0
        datetxt.HorizontalAlignment = 0
        pdf.AddCell(datetxt)

        '資訊
        'Dim pdf_Info As PdfPTable = New PdfPTable(4)

        If GridView1.Rows.Count <> 0 Then
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
                head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text.Replace("&nbsp;", ""), head_font))
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

            table = New PdfPCell(New Phrase(Label3.Text.ToString, unit_font))
            table.Colspan = 3
            table.Border = 0
            table.HorizontalAlignment = 0
            pdf2.AddCell(table)

            table = New PdfPCell(New Phrase(Label4.Text.ToString, unit_font))
            table.Colspan = 3
            table.Border = 0
            table.HorizontalAlignment = 0
            pdf2.AddCell(table)


            doc.Add(pdf)
            doc.Add(pdf2)
        Else
            doc.Add(pdf)
        End If
        doc.Close()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub

    Protected Sub ButtonSM_Click(sender As Object, e As EventArgs) Handles ButtonSM.Click
        SqlQuery(txtRecDate.Text)
    End Sub
End Class
