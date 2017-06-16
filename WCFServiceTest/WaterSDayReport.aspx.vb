Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class WaterSDayReport
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else

            For i = 0 To 23
                begin_hh.Items.Add(Right("0" & i.ToString, 2))
                end_hh.Items.Add(Right("0" & i.ToString, 2))
            Next
            
            Dim sShopNo As String = Request.QueryString("sShopNo")
            Dim sDrawNo As String = Request.QueryString("sDrawNo")
            Dim sShopName As String = Request.QueryString("sShopName")
            Dim sDateS As String
            Dim sDateE As String
            Dim sHHS As String
            Dim sHHE As String
            If Not IsPostBack Then
                sDateS = Request.QueryString("sDateS")
                sDateE = Request.QueryString("sDateE")
                sHHS = Request.QueryString("sHHS")
                sHHE = Request.QueryString("sHHE")
            Else
                sDateS = CYMD(txtDateS.Text)
                sDateE = CYMD(txtDateE.Text)
                sHHS = begin_hh.Text
                sHHE = end_hh.Text
            End If

            Label1.Text = "查詢區間：" & sDateS & " " & sHHS & "時~" & sDateE & " " & sHHE & "時"
            txtDateS.Text = sDateS
            txtDateE.Text = sDateE
            Label1.Visible = False
            SqlQuery(sDateS, sDateE, sHHS, sHHE, sShopNo, sDrawNo, sShopName)
        End If
    End Sub

    Protected Sub SqlQuery(ByVal sDateS As String, ByVal sDateE As String, ByVal sHHS As String, ByVal sHHE As String, ByVal sShopNo As String, ByVal sDrawNo As String, ByVal sShopName As String)
        SqlDataSource1.SelectParameters.Clear()

        Dim iSumAmount As Integer

        Label1.Text = "查詢區間：" & sDateS & " " & sHHS & "時 ~ " & sDateE & " " & sHHE & "時"

        txtDateS.Text = sDateS
        If sDateE < sDateS Then
            txtDateE.Text = sDateS
            If sHHE < sHHS Then
                begin_hh.Items(CInt(sHHS)).Selected = True '預設
                end_hh.Items(CInt(23)).Selected = True '預設
            Else
                begin_hh.Items(CInt(sHHS)).Selected = True '預設
                end_hh.Items(CInt(sHHE)).Selected = True '預設
            End If
        Else
            txtDateE.Text = sDateE
            begin_hh.Items(CInt(sHHS)).Selected = True '預設
            end_hh.Items(CInt(sHHE)).Selected = True '預設
        End If

        LabShopName.Text = sShopName
        LabShopNo.Text = sShopNo
        LabDrawNo.Text = sDrawNo

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=Water"

        Try

            Dim sql As String = " Select isNull(Sum(Amount),0) as SumAmount From Record Where Substring(Recdate,1,13) Between '" & sDateS & " " & sHHS & "' and  '" & sDateE & " " & sHHE & "'" & _
                                " And ID in (Select ID From Setup with (Nolock) Where 店鋪編號='" & sShopNo & "' and 圖面編號='" & sDrawNo & "' and 店鋪名稱='" & sShopName & "')  "

            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Using cmd As New OleDbCommand(sql, conn)
                    Dim dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        iSumAmount = dr("SumAmount").ToString
                    End If
                End Using
            End Using
            LabSumAmount.Text = iSumAmount

            Dim strSQL As String = " Select RecDate as 日期, Round(水表值,4) , '' as 備註  From Record With (NoLock) " & _
                                   " Where Substring(Recdate,1,13)  Between '" & sDateS & " " & sHHS & "' and  '" & sDateE & " " & sHHE & "'" & _
                                   " And ID in (Select ID From Setup with (Nolock) Where 店鋪編號='" & sShopNo & "' and 圖面編號='" & sDrawNo & "' and 店鋪名稱='" & sShopName & "')  " & _
                                   " Order By RecDate "

            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = strSQL
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            Print.Enabled = True : Excel.Enabled = True : PDF.Enabled = True
            If dv Is Nothing Then
                Panel_Report.Visible = True
                Panel_Export.Visible = True
                Dim msg As String = "查無資料"
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "訊息:用水量表" & msg, False)
                Print.Enabled = False : Excel.Enabled = False : PDF.Enabled = False
            Else
                If dv.Count > 0 Then
                    Panel_Report.Visible = True
                    Panel_Export.Visible = True
                Else
                    Panel_Report.Visible = True
                    Panel_Export.Visible = True
                    Dim msg As String = "查無資料"
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "訊息:用水量表" & msg, False)
                    Print.Enabled = False : Excel.Enabled = False : PDF.Enabled = False
                End If

            End If
        Catch ex As Exception
            ex.ToString()
            Panel_Export.Visible = False
            Panel_Report.Visible = False
        End Try
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        SqlQuery(txtDateS.Text, txtDateE.Text, begin_hh.SelectedValue.ToString, end_hh.SelectedValue.ToString, LabShopNo.Text, LabDrawNo.Text, LabShopName.Text)
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(sender As Object, e As ImageClickEventArgs) Handles Excel.Click
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("紀錄查詢_" & Now())

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"

        Label1.Visible = True : Label5.Visible = False
        txtDateS.Visible = False : txtDateE.Visible = False : begin_hh.Visible = False : end_hh.Visible = False : ButtonSM.Visible = False
        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")

        Label1.Visible = False : Label5.Visible = True
        txtDateS.Visible = True : txtDateE.Visible = True : begin_hh.Visible = True : end_hh.Visible = True : ButtonSM.Visible = True
        Response.End()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles PDF.Click
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("紀錄查詢_" & Now())

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
        Dim CountColumns As Integer = 3   '自動生成
        Dim CountRow As Integer = 1
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
        Dim Header2 As PdfPCell = New PdfPCell(New Phrase("紀錄查詢", title_font))
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


        datetxt = New PdfPCell(New Phrase("店鋪名稱：" & LabShopName.Text.ToString, unit_font))
        datetxt.Colspan = 2
        datetxt.Border = 0
        datetxt.HorizontalAlignment = 0
        pdf.AddCell(datetxt)

        datetxt = New PdfPCell(New Phrase("店鋪編號：" & LabShopNo.Text.ToString, unit_font))
        datetxt.Colspan = 1
        datetxt.Border = 0
        datetxt.HorizontalAlignment = 0
        pdf.AddCell(datetxt)

        datetxt = New PdfPCell(New Phrase("圖面編號：" & LabDrawNo.Text.ToString, unit_font))
        datetxt.Colspan = 2
        datetxt.Border = 0
        datetxt.HorizontalAlignment = 0
        pdf.AddCell(datetxt)

        datetxt = New PdfPCell(New Phrase("總用水量：" & LabSumAmount.Text.ToString, unit_font))
        datetxt.Colspan = 1
        datetxt.Border = 0
        datetxt.HorizontalAlignment = 0
        pdf.AddCell(datetxt)

        '資訊
        'Dim pdf_Info As PdfPTable = New PdfPTable(4)

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
                table = New PdfPCell(New Phrase(GridView1.Rows(i).Cells(j).Text.Replace("&nbsp;", ""), fontchinese))
                table.HorizontalAlignment = 1
                table.PaddingBottom = 5.0F
                'If i Mod 2 Then
                '    table.BackgroundColor = New BaseColor(255, 255, 204)
                'End If
                pdf2.AddCell(table)
            Next
        Next

        table = New PdfPCell(New Phrase(Label3.Text.ToString, unit_font))
        table.Colspan = 2
        table.Border = 0
        table.HorizontalAlignment = 0
        pdf2.AddCell(table)

        table = New PdfPCell(New Phrase(Label4.Text.ToString, unit_font))
        table.Colspan = 1
        table.Border = 0
        table.HorizontalAlignment = 0
        pdf2.AddCell(table)


        doc.Add(pdf)
        doc.Add(pdf2)
        doc.Close()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub

    Protected Sub ButtonSM_Click(sender As Object, e As EventArgs) Handles ButtonSM.Click
        SqlQuery(txtDateS.Text, txtDateE.Text, begin_hh.SelectedValue.ToString, end_hh.SelectedValue.ToString, LabShopNo.Text, LabDrawNo.Text, LabShopName.Text)
    End Sub
End Class
