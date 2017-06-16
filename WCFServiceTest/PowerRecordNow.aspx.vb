Imports System.Data.OleDb
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Data

Partial Class PowerDetail
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
                Date_txt1.Text = DateAdd(DateInterval.Day, 0, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Day, 0, Now).ToString("yyyy/MM/dd")

                '建置開始时间dropdownlist
                For i = 0 To 9
                    begin_hh.Items.Add("0" & i & "")
                Next
                For i = 10 To 23
                    begin_hh.Items.Add("" & i & "")
                Next
                begin_hh.Items(0).Selected = True '預設

                '建置結束时间dropdownlist
                For i = 0 To 9
                    end_hh.Items.Add("0" & i & "")
                Next
                For i = 10 To 23
                    end_hh.Items.Add("" & i & "")
                Next
                end_hh.Items(23).Selected = True '預設
            End If
            SqlQuery2()
        End If
    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        SqlQuery()
    End Sub
    Protected Sub SqlQuery2()
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
        Dim start_time As String = Date_txt1.Text & " 00:00:00"
        Dim end_time As String = Date_txt1.Text & " 23:59:59"
        Date_txt.Text = "時間：" & start_time & "~" & end_time
        Num_txt.Text = "編號：" & Session("ctrlnr") & "-" & Session("meterid")
        Position_txt.Text = "位置：" & Session("position")

        Dim sql As String = "select RecDate AS 時間,Vavg AS 平均電壓,Iavg AS 平均電流,W AS 實功,V_ar AS 虛功,VA AS 視在,PF AS 功因,KWh AS 用電度數 from PowerRecord " & _
                            "where CtrlNr= ? and MeterID= ? and RecDate between ? and ? order by RecDate "
        Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            SqlDataSource1.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
            SqlDataSource1.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, start_time)
            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, end_time)
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv.Count > 0 Then
                Print_Detail.Visible = True
                msg.Visible = False
                export.Visible = True
                Print.Visible = True
                Excel.Visible = True
                PDF.Visible = True
            Else
                Print_Detail.Visible = False
                msg.Visible = True
                export.Visible = False
                Print.Visible = False
                Excel.Visible = False
                PDF.Visible = False
            End If
        Catch ex As Exception
            ex.ToString()
        End Try

    End Sub
    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()
        Dim strcon As String = Nothing
        If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("AccountPower") & ""
        Else
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account") & ""
        End If

        Dim start_time As String = Date_txt1.Text & " " & begin_hh.SelectedValue & ":00:00"
        Dim end_time As String = Date_txt2.Text & " " & end_hh.SelectedValue & ":59:59"

        Date_txt.Text = "時間：" & start_time & "~" & end_time
        Num_txt.Text = "編號：" & Session("ctrlnr") & "-" & Session("meterid")
        Position_txt.Text = "位置：" & Session("position")

        Dim sql As String = "SELECT RecDate AS 時間"
        Dim QueryPara(7) As String   '儲存選取字串
        Dim arrData() As String   '暫存選取字串
        Dim i As Integer     '迴圈變數

        '---------判斷各checkbox選取狀況 start
        If V_CheckBoxList.SelectedIndex > -1 Then
            arrData = V_CheckBoxList.SelectedValueList.Split(",")
            For i = 0 To arrData.Length - 1
                If arrData(i) = "V1" Then
                    arrData(i) = "V1 AS R相電壓"
                    If arrData.Count > 1 Then
                        QueryPara(1) &= "," & arrData(i)
                    Else
                        QueryPara(1) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "V2" Then
                    arrData(i) = "V2 AS S相電壓"
                    If arrData.Count > 1 Then
                        QueryPara(1) &= "," & arrData(i)
                    Else
                        QueryPara(1) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "V3" Then
                    arrData(i) = "V3 AS T相電壓"
                    If arrData.Count > 1 Then
                        QueryPara(1) &= "," & arrData(i)
                    Else
                        QueryPara(1) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "Vavg" Then
                    arrData(i) = "Vavg AS 平均電壓"
                    If arrData.Count > 1 Then
                        QueryPara(1) &= "," & arrData(i)
                    Else
                        QueryPara(1) &= "," & arrData(i)
                    End If
                End If
            Next
            For i = 0 To arrData.Length - 1
                arrData(i) = ""
            Next
        End If

        If I_CheckBoxList.SelectedIndex > -1 Then
            arrData = I_CheckBoxList.SelectedValueList.Split(",")
            For i = 0 To arrData.Length - 1
                '欄位改中文,因為TBcheckboxlist會以","儲存選取,所以用","分割後逐一判斷加上AS陳述式(gridview為自動產生欄位,一般固定欄位設定BoundField就好)
                If arrData(i) = "I1" Then
                    arrData(i) = "I1 AS R相電流"
                    If arrData.Count > 1 Then
                        QueryPara(0) &= "," & arrData(i)
                    Else
                        QueryPara(0) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "I2" Then
                    arrData(i) = "I2 AS S相電流"
                    If arrData.Count > 1 Then
                        QueryPara(0) &= "," & arrData(i)
                    Else
                        QueryPara(0) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "I3" Then
                    arrData(i) = "I3 AS T相電流"
                    If arrData.Count > 1 Then
                        QueryPara(0) &= "," & arrData(i)
                    Else
                        QueryPara(0) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "Iavg" Then
                    arrData(i) = "Iavg AS 平均電流"
                    If arrData.Count > 1 Then
                        QueryPara(0) &= "," & arrData(i)
                    Else
                        QueryPara(0) &= "," & arrData(i)
                    End If
                End If
            Next
            For i = 0 To arrData.Length - 1
                arrData(i) = ""
            Next
        End If

        If W_CheckBoxList.SelectedIndex > -1 Then
            arrData = W_CheckBoxList.SelectedValueList.Split(",")
            For i = 0 To arrData.Length - 1
                If arrData(i) = "W" Then
                    arrData(i) = "W AS 實功"
                    If arrData.Count > 1 Then
                        QueryPara(2) &= "," & arrData(i)
                    Else
                        QueryPara(2) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "V_ar" Then
                    arrData(i) = "V_ar AS 虛功"
                    If arrData.Count > 1 Then
                        QueryPara(2) &= "," & arrData(i)
                    Else
                        QueryPara(2) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "VA" Then
                    arrData(i) = "VA AS 視在"
                    If arrData.Count > 1 Then
                        QueryPara(2) &= "," & arrData(i)
                    Else
                        QueryPara(2) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "PF" Then
                    arrData(i) = "PF AS 功因"
                    If arrData.Count > 1 Then
                        QueryPara(2) &= "," & arrData(i)
                    Else
                        QueryPara(2) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "KWh" Then
                    arrData(i) = "KWh AS 用電度數"
                    If arrData.Count > 1 Then
                        QueryPara(2) &= "," & arrData(i)
                    Else
                        QueryPara(2) &= "," & arrData(i)
                    End If
                End If
            Next
            For i = 0 To arrData.Length - 1
                arrData(i) = ""
            Next
        End If

        If Mode_CheckBoxList.SelectedIndex > -1 Then
            arrData = Mode_CheckBoxList.SelectedValueList.Split(",")
            For i = 0 To arrData.Length - 1
                If arrData(i) = "Mode1" Then
                    arrData(i) = "Mode1 AS 模式1"
                    If arrData.Count > 1 Then
                        QueryPara(3) &= "," & arrData(i)
                    Else
                        QueryPara(3) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "Mode2" Then
                    arrData(i) = "Mode2 AS 模式2"
                    If arrData.Count > 1 Then
                        QueryPara(3) &= "," & arrData(i)
                    Else
                        QueryPara(3) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "Mode3" Then
                    arrData(i) = "Mode3 AS 模式3"
                    If arrData.Count > 1 Then
                        QueryPara(3) &= "," & arrData(i)
                    Else
                        QueryPara(3) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "Mode4" Then
                    arrData(i) = "Mode4 AS 模式4"
                    If arrData.Count > 1 Then
                        QueryPara(3) &= "," & arrData(i)
                    Else
                        QueryPara(3) &= "," & arrData(i)
                    End If
                End If
            Next
            For i = 0 To arrData.Length - 1
                arrData(i) = ""
            Next
        End If

        If DeMand_CheckBoxList.SelectedIndex > -1 Then
            arrData = DeMand_CheckBoxList.SelectedValueList.Split(",")
            For i = 0 To arrData.Length - 1
                If arrData(i) = "DeMand" Then
                    arrData(i) = "DeMand AS 尖峰需量"
                    If arrData.Count > 1 Then
                        QueryPara(4) &= "," & arrData(i)
                    Else
                        QueryPara(4) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "DemandHalf" Then
                    arrData(i) = "DemandHalf AS 半尖峰需量"
                    If arrData.Count > 1 Then
                        QueryPara(4) &= "," & arrData(i)
                    Else
                        QueryPara(4) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "DemandSatHalf" Then
                    arrData(i) = "DemandSatHalf AS 週六半尖峰需量"
                    If arrData.Count > 1 Then
                        QueryPara(4) &= "," & arrData(i)
                    Else
                        QueryPara(4) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "DemandOff" Then
                    arrData(i) = "DemandOff AS 離峰需量"
                    If arrData.Count > 1 Then
                        QueryPara(4) &= "," & arrData(i)
                    Else
                        QueryPara(4) &= "," & arrData(i)
                    End If
                End If
            Next
            For i = 0 To arrData.Length - 1
                arrData(i) = ""
            Next
        End If

        If E_CheckBoxList.SelectedIndex > -1 Then
            arrData = E_CheckBoxList.SelectedValueList.Split(",")
            For i = 0 To arrData.Length - 1
                If arrData(i) = "RushHour" Then
                    arrData(i) = "RushHour AS 尖峰電量"
                    If arrData.Count > 1 Then
                        QueryPara(5) &= "," & arrData(i)
                    Else
                        QueryPara(5) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "HalfHour" Then
                    arrData(i) = "HalfHour AS 半尖峰電量"
                    If arrData.Count > 1 Then
                        QueryPara(5) &= "," & arrData(i)
                    Else
                        QueryPara(5) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "SatHalfHour" Then
                    arrData(i) = "SatHalfHour AS 週六半尖峰電量"
                    If arrData.Count > 1 Then
                        QueryPara(5) &= "," & arrData(i)
                    Else
                        QueryPara(5) &= "," & arrData(i)
                    End If
                ElseIf arrData(i) = "OffHour" Then
                    arrData(i) = "OffHour AS 離峰電量"
                    If arrData.Count > 1 Then
                        QueryPara(5) &= "," & arrData(i)
                    Else
                        QueryPara(5) &= "," & arrData(i)
                    End If
                End If
            Next
            For i = 0 To arrData.Length - 1
                arrData(i) = ""
            Next
        End If

        '組合各字串
        Dim CountCheck As Integer = 0
        Dim j As Integer
        For j = 0 To 7
            If QueryPara(j) <> "" Then
                'If CountCheck = 0 Then
                sql &= QueryPara(j)
            Else
                'sql &= " , " & QueryPara(j)
                'End If
                'CountCheck = CountCheck + 1
            End If
        Next

        '時間間隔
        If interval_DDList.SelectedValue = "1" Then
            sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
        ElseIf interval_DDList.SelectedValue = "5" Then
            sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
        ElseIf interval_DDList.SelectedValue = "30" Then
            sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
        ElseIf interval_DDList.SelectedValue = "60" Then
            sql &= " FROM PowerRecord WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
        End If

        Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            SqlDataSource1.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
            SqlDataSource1.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, start_time)
            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, end_time)
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
            If dv.Count > 0 Then
                Print_Detail.Visible = True
                msg.Visible = False
                export.Visible = True
                Print.Visible = True
                Excel.Visible = True
                PDF.Visible = True
            Else
                Print_Detail.Visible = False
                msg.Visible = True
                export.Visible = False
                Print.Visible = False
                Excel.Visible = False
                PDF.Visible = False
            End If
        Catch ex As Exception
            ex.ToString()
        End Try

    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        If GridView1.Rows.Count = 0 Then
            msg.Visible = True
            export.Visible = False
            Print.Visible = False
            Excel.Visible = False
            PDF.Visible = False
        Else
            msg.Visible = False
            export.Visible = True
            Print.Visible = True
            Excel.Visible = True
            PDF.Visible = True
        End If
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'If V_CheckBoxList.SelectedIndex > -1 Or I_CheckBoxList.SelectedIndex > -1 Or W_CheckBoxList.SelectedIndex > -1 Or Mode_CheckBoxList.SelectedIndex > -1 Or DeMand_CheckBoxList.SelectedIndex > -1 Or E_CheckBoxList.SelectedIndex > -1 Then
        '    SqlQuery()
        'Else
        '    SqlQuery2()
        'End If
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Excel.Click
        'If V_CheckBoxList.SelectedIndex > -1 Or I_CheckBoxList.SelectedIndex > -1 Or W_CheckBoxList.SelectedIndex > -1 Or Mode_CheckBoxList.SelectedIndex > -1 Or DeMand_CheckBoxList.SelectedIndex > -1 Or E_CheckBoxList.SelectedIndex > -1 Then
        '    SqlQuery()
        'Else
        '    SqlQuery2()
        'End If
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("電力數值紀錄表_" & Now())

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

        ''寫入標題
        'htmlWrite.WriteLine("<center><font size='5'><U><B>電力數值紀錄表</B></U></font></center>")
        'htmlWrite.WriteLine("<font size='3'>日期：" & Now.Date & "</font>")
        'htmlWrite.WriteLine("<br />")
        'htmlWrite.WriteLine("<font size='3'>位置：" & Session("ctrlnr") & "" & Session("meterid") & "</font>")

        'For i = 0 To CountRow - 1
        '    CType(GridView1.Rows(i).FindControl("LinkButton1"), LinkButton).Visible = False
        'Next

        Print_Detail.RenderControl(htmlWrite)

        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.End()
        GridView1.DataBind()
    End Sub

    Protected Sub PDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PDF.Click
        'If V_CheckBoxList.SelectedIndex > -1 Or I_CheckBoxList.SelectedIndex > -1 Or W_CheckBoxList.SelectedIndex > -1 Or Mode_CheckBoxList.SelectedIndex > -1 Or DeMand_CheckBoxList.SelectedIndex > -1 Or E_CheckBoxList.SelectedIndex > -1 Then
        '    SqlQuery()
        'Else
        '    SqlQuery2()
        'End If
        '檔案名稱
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("電力數值紀錄表_" & Now())

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
        GridView1.AllowPaging = False
        GridView1.DataBind()

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

        Dim Header As PdfPCell = New PdfPCell(New Phrase("電力數值紀錄表", title_font))
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
End Class
