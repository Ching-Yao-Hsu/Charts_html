Imports System.Data.OleDb
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class FeeCalculate
    Inherits AccountAdmin

    'Dim Cal1 As New PowerFeeCalculate.FeeCalculate
    Dim rushcap, halfcap, sathalfcap, offcap As Integer '契約容量

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        SqlQuery()
    End Sub

    Protected Sub SqlQuery()
        Dim group As String = Request.QueryString("group")
        Dim eco_account As String = Request.QueryString("eco_account")
        Dim ctrlnr As String = Request.QueryString("ctrlnr")
        Dim meterid As String = Request.QueryString("meterid")
        Dim start_time As String = Request.QueryString("datetime").Split(",").GetValue(0)
        Dim end_time As String = Request.QueryString("datetime").Split(",").GetValue(1)
        Dim feetype As String = Request.QueryString("feetype")
        Dim position As String = Request.QueryString("position")

        Dim SupplyType As Integer '供電方式
        Select Case feetype
            Case 1, 3, 5
                SupplyType = 0 '高壓
            Case Else
                SupplyType = 1 '特高壓
        End Select

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        Dim sql As String = "SELECT SUM(RushHour) AS RushHour,SUM(HalfHour) AS HalfHour,SUM(SatHalfHour) AS SatHalfHour,SUM(OffHour) AS OffHour into #T1 " & _
        "FROM PowerRecordCollection " & _
        "WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate BETWEEN '" & start_time & "' AND '" & end_time & "';" & _
        "SELECT MAX(DeMand) AS DeMand,MAX(DeMandHalf) AS DeMandHalf,MAX(DeMandSatHalf) AS DeMandSatHalf,MAX(DeMandOff) AS DeMandOff into #T2 " & _
        "FROM PowerRecordCollection " & _
        "WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate BETWEEN '" & start_time & "' AND '" & end_time & "';" & _
        "SELECT #T1.*,#T2.* FROM #T1,#T2;DROP TABLE #T1,#T2"

        Dim Month As Integer = start_time.Split("/").GetValue(1)
        Dim RushHour, HalfHour, SatHalfHour, OffHour As Integer '用電量
        Dim Demand, DemandHalf, DemandSatHalf, DemandOff As Integer '需量最大值

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    Dim i As Integer = 0
                    While dr.Read() = True
                        If i = 0 Then
                            RushHour = dr("RushHour")
                            HalfHour = dr("HalfHour")
                            SatHalfHour = dr("SatHalfHour")
                            OffHour = dr("OffHour")
                            Demand = dr("Demand")
                            DemandHalf = dr("DemandHalf")
                            DemandSatHalf = dr("DemandSatHalf")
                            DemandOff = dr("DemandOff")
                            i += 1
                        End If
                    End While
                    'If dr.Read() Then

                    'End If
                End Using
            End Using

            Dim strcon2 As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Dim sql2 As String = "Select * From FeeSet Where ID=" & feetype
            Using conn2 As New OleDbConnection(strcon2)
                If conn2.State = 0 Then conn2.Open()
                Using cmd As New OleDbCommand(sql2, conn2)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        While dr.Read() = True
                            Select Case feetype
                                Case 1 To 2 '兩段式
                                    PowerFeeCalculate.FeeCalculate.Two.Sum_Basic_Demand(SupplyType) = dr("S1")
                                    PowerFeeCalculate.FeeCalculate.Two.Sum_Basic_SatHalfDemand(SupplyType) = dr("S3")
                                    PowerFeeCalculate.FeeCalculate.Two.Sum_Basic_OffDemand(SupplyType) = dr("S4")
                                    PowerFeeCalculate.FeeCalculate.Two.Sum_Flow_Top(SupplyType) = dr("S5")
                                    PowerFeeCalculate.FeeCalculate.Two.Sum_Flow_SatHalf(SupplyType) = dr("S7")
                                    PowerFeeCalculate.FeeCalculate.Two.Sum_Flow_Off(SupplyType) = dr("S8")

                                    PowerFeeCalculate.FeeCalculate.Two.NoSum_Basic_Demand(SupplyType) = dr("NS1")
                                    PowerFeeCalculate.FeeCalculate.Two.NoSum_Basic_SatHalfDemand(SupplyType) = dr("NS3")
                                    PowerFeeCalculate.FeeCalculate.Two.NoSum_Basic_OffDemand(SupplyType) = dr("NS4")
                                    PowerFeeCalculate.FeeCalculate.Two.NoSum_Flow_Top(SupplyType) = dr("NS5")
                                    PowerFeeCalculate.FeeCalculate.Two.NoSum_Flow_SatHalf(SupplyType) = dr("NS7")
                                    PowerFeeCalculate.FeeCalculate.Two.NoSum_Flow_Off(SupplyType) = dr("NS8")
                                Case 3 To 4 '三段式(固定)
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_Demand(SupplyType) = dr("S1")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_HalfDemand(SupplyType) = dr("S2")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_SatHalfDemand(SupplyType) = dr("S3")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_OffDemand(SupplyType) = dr("S4")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Top(SupplyType) = dr("S5")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Half(SupplyType) = dr("S6")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_SatHalf(SupplyType) = dr("S7")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Off(SupplyType) = dr("S8")

                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_Demand(SupplyType) = dr("NS1")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_HalfDemand(SupplyType) = dr("NS2")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_SatHalfDemand(SupplyType) = dr("NS3")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_OffDemand(SupplyType) = dr("NS4")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_Half(SupplyType) = dr("NS6")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_SatHalf(SupplyType) = dr("NS7")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_Off(SupplyType) = dr("NS8")
                                Case 5 To 6 '三段式(可變動)
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_Demand(SupplyType) = dr("S1")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_HalfDemand(SupplyType) = dr("S2")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_SatHalfDemand(SupplyType) = dr("S3")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Basic_OffDemand(SupplyType) = dr("S4")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Top_IfNoFix(SupplyType) = dr("S5")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Half(SupplyType) = dr("S6")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_SatHalf(SupplyType) = dr("S7")
                                    PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Off(SupplyType) = dr("S8")

                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_Demand(SupplyType) = dr("NS1")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_HalfDemand(SupplyType) = dr("NS2")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_SatHalfDemand(SupplyType) = dr("NS3")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Basic_OffDemand(SupplyType) = dr("NS4")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_Half(SupplyType) = dr("NS6")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_SatHalf(SupplyType) = dr("NS7")
                                    PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_Off(SupplyType) = dr("NS8")
                            End Select
                        End While
                    End Using
                End Using

                sql2 = "Select * from MeterSetup where ECO_Account = '" & eco_account & "' AND CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & ""
                Using cmd As New OleDbCommand(sql2, conn2)
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            rushcap = dr("RushCap")
                            offcap = dr("OffCap")
                            halfcap = dr("HalfCap")
                            sathalfcap = dr("SatHalfCap")
                        End If
                    End Using
                End Using
            End Using

            '計算電費
            Dim BasicFee As Double
            Dim FlowFee As Double
            Dim OverFee As Double
            Dim Totalfee As Double
            Dim RushPrice As Double
            Dim HalfPrice As Double
            Dim SatHalfPrice As Double
            Dim OffPrice As Double

            Select Case feetype
                Case 1 To 2 '兩段式
                    Dim sum As Boolean = False
                    Dim TempBasicFee As Double
                    Dim TempFlowFee As Double
                    Dim TempOverFee As Double
                    If Month >= 6 And Month <= 9 Then sum = True
                    Totalfee += PowerFeeCalculate.FeeCalculate.Two.Caculate(SupplyType, sum, RushHour, SatHalfHour, OffHour, Demand, DemandSatHalf, DemandOff, rushcap, halfcap, sathalfcap, offcap, TempBasicFee, TempFlowFee, TempOverFee)
                    BasicFee += TempBasicFee
                    FlowFee += TempFlowFee
                    OverFee += TempOverFee
                    'Next

                    If sum = True Then
                        RushPrice = PowerFeeCalculate.FeeCalculate.Two.Sum_Flow_Top(SupplyType)
                        HalfPrice = 0
                        SatHalfPrice = PowerFeeCalculate.FeeCalculate.Two.Sum_Flow_SatHalf(SupplyType)
                        OffPrice = PowerFeeCalculate.FeeCalculate.Two.Sum_Flow_Off(SupplyType)
                    Else
                        RushPrice = PowerFeeCalculate.FeeCalculate.Two.NoSum_Flow_Top(SupplyType)
                        HalfPrice = 0
                        SatHalfPrice = PowerFeeCalculate.FeeCalculate.Two.NoSum_Flow_SatHalf(SupplyType)
                        OffPrice = PowerFeeCalculate.FeeCalculate.Two.NoSum_Flow_Off(SupplyType)
                    End If

                Case 3 To 6
                    Dim Iffix As Boolean = True
                    If feetype = 5 Or feetype = 6 Then Iffix = False
                    'For i = 0 To UBound(Month)
                    Dim sum As Boolean = False
                    Dim TempBasicFee As Double
                    Dim TempFlowFee As Double
                    Dim TempOverFee As Double

                    If Month >= 6 And Month <= 9 Then sum = True
                    Totalfee += PowerFeeCalculate.FeeCalculate.Three.Caculate(SupplyType, sum, Iffix, RushHour, HalfHour, SatHalfHour, OffHour, Demand, DemandHalf, DemandSatHalf, DemandOff, rushcap, halfcap, sathalfcap, offcap, TempBasicFee, TempFlowFee, TempOverFee)
                    BasicFee += TempBasicFee
                    FlowFee += TempFlowFee
                    OverFee += TempOverFee
                    'Next
                    If sum = True Then
                        If Iffix Then
                            RushPrice = PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Top(SupplyType)
                        Else
                            RushPrice = PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Top_IfNoFix(SupplyType)
                        End If
                        HalfPrice = PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Half(SupplyType)
                        SatHalfPrice = PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_SatHalf(SupplyType)
                        OffPrice = PowerFeeCalculate.FeeCalculate.Three.Sum_Flow_Off(SupplyType)
                    Else
                        RushPrice = 0
                        HalfPrice = PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_Half(SupplyType)
                        SatHalfPrice = PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_SatHalf(SupplyType)
                        OffPrice = PowerFeeCalculate.FeeCalculate.Three.NoSum_Flow_Off(SupplyType)
                    End If
            End Select

            Select Case feetype
                Case 1
                    feetype_txt.Text = "電價類型：二段式-高壓"
                    HalfHour_txt.Text = "非夏月"
                Case 2
                    feetype_txt.Text = "電價類型：二段式-特高壓"
                    HalfHour_txt.Text = "非夏月"
                Case 3
                    feetype_txt.Text = "電價類型：三段式(尖峰時間固定)-高壓"
                    HalfHour_txt.Text = "半尖峰"
                Case 4
                    feetype_txt.Text = "電價類型：三段式(尖峰時間固定)-特高壓"
                    HalfHour_txt.Text = "半尖峰"
                Case 5
                    feetype_txt.Text = "電價類型：三段式(尖峰時間可變動)-高壓"
                    HalfHour_txt.Text = "半尖峰"
                Case 6
                    feetype_txt.Text = "電價類型：三段式(尖峰時間可變動)-特高壓"
                    HalfHour_txt.Text = "半尖峰"
            End Select

            id.Text = "編號：" & ctrlnr & "-" & meterid
            position_txt.Text = "位置：" & position
            date_txt.Text = "計費區間：" & start_time & "~" & end_time

            RsuhCapTxt.Text = rushcap
            HalfCapTxt.Text = halfcap
            SatHalfCapTxt.Text = sathalfcap
            OffCapTxt.Text = offcap

            RushOverTxt.Text = Demand
            HalfOverTxt.Text = DemandHalf
            SatHalfOverTxt.Text = DemandSatHalf
            OffOverTxt.Text = DemandOff

            RushHourTxt.Text = RushHour
            HalfHourTxt.Text = If(feetype = 1 Or feetype = 2, "--", HalfHour.ToString)
            SatHalfHourTxt.Text = SatHalfHour
            OffHourTxt.Text = OffHour

            RushPriceTxt.Text = RushPrice
            HalfPriceTxt.Text = If(feetype = 1 Or feetype = 2, "--", HalfPrice.ToString)
            SatHalfPriceTxt.Text = SatHalfPrice
            OffPriceTxt.Text = OffPrice

            RushSumTxt.Text = RushHour * RushPrice
            HalfSumTxt.Text = If(feetype = 1 Or feetype = 2, 0, HalfHour * HalfPrice)
            SatHalfSumTxt.Text = SatHalfHour * SatHalfPrice
            OffSumTxt.Text = OffHour * OffPrice

            BasicFeeTxt.Text = BasicFee.ToString("#,0")
            OverFeeTxt.Text = OverFee.ToString("#,0")
            FlowFeeTxt.Text = FlowFee.ToString("#,0")
            TotalFeeTxt.Text = Totalfee.ToString("#,0")

            AvgFeeTxt.Text = If((CInt(RushHour) + CInt(If(feetype = 1 Or feetype = 2, 0, HalfHour)) + CInt(SatHalfHour) + CInt(OffHour)) = 0, "--", FormatNumber(Math.Round(Totalfee / (CInt(RushHour) + CInt(If(feetype = 1 Or feetype = 2, 0, HalfHour)) + CInt(SatHalfHour) + CInt(OffHour)), 2)))
            Panel_Export.Visible = True
        End Using
    End Sub

    Protected Sub print_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        SqlQuery()
        Page.ClientScript.RegisterClientScriptInclude("myPrint", "print.js")
        'Response.Write("<script>window.opener=null;window.close();</script>")
    End Sub

    Protected Sub Excel_Click(sender As Object, e As ImageClickEventArgs) Handles Excel.Click
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> " '文字樣式字串
        id.Attributes.Add("class", "text")
        '檔案名稱需經 UrlEncode 編碼，解決中文檔名的問題   
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("電費試算表_" & Now())

        Response.Clear()
        Response.AppendHeader("content-disposition", "attachment;filename=" + sFilename + ".xls")
        Response.Charset = "big5"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/ms-excel"

        Dim stringWrite As StringWriter = New System.IO.StringWriter
        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Panel_Report.RenderControl(htmlWrite)
        Response.Write(style)
        Response.Write("<p align=middle>" & stringWrite.ToString & "</p>")
        Response.End()
    End Sub

    Protected Sub PDF_Click(sender As Object, e As ImageClickEventArgs) Handles PDF.Click
        Dim sFilename As String
        sFilename = Server.UrlPathEncode("電費試算表_" & Now())

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.AddHeader("content-disposition", "attachment;filename=" + sFilename + ".pdf")  '檔名
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim stringWrite As System.IO.StringWriter = New StringWriter
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        Dim reader As StringReader = New StringReader(stringWrite.ToString())
        Dim doc As Document = New Document(PageSize.A4.Rotate())
        PdfWriter.GetInstance(doc, Response.OutputStream)
        '處理中文
        Dim bfCh2 As BaseFont = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\MINGLIU.TTC,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
        Dim fontchinese As Font = New Font(bfCh2)
        doc.Open()

        '要定義 PDF Table 的大小（Column 數量）
        Dim pdf As PdfPTable = New PdfPTable(New Single() {1, 1, 1, 0.7, 0.8, 0.6, 0.9})

        pdf.HeaderRows = 1
        pdf.WidthPercentage = 100  '表格寬度百分比
        pdf.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '字型
        Dim title_font As Font = New Font(bfCh2, 14, Font.BOLD)
        Dim detail_font As Font = New Font(bfCh2, 16)
        Dim unit_font As Font = New Font(bfCh2, 12)
        Dim sum_font As Font = New Font(bfCh2, 16, Font.BOLD)
        Dim sum_font2 As Font = New Font(bfCh2, 16, Font.BOLD, BaseColor.RED)
        Dim head_font As Font = New Font(bfCh2, 12, Font.BOLD)

        '表頭說明文字
        'Dim Header As PdfPCell = New PdfPCell(New Phrase("台灣國際造船股份有限公司 - 基隆廠", title_font))
        'Header.Colspan = CountColumns
        'Header.Border = 0
        'Header.HorizontalAlignment = 1
        'pdf.AddCell(Header)

        Dim Header2 As PdfPCell = New PdfPCell(New Phrase("電費試算表", title_font))
        Header2.Colspan = 7
        Header2.PaddingBottom = 10.0F
        Header2.Border = 0
        Header2.HorizontalAlignment = 1
        pdf.AddCell(Header2)

        '空白行
        'Dim space1 As PdfPCell = New PdfPCell(New Phrase(" "))
        'space1.Colspan = 7
        'space1.Border = 0
        'pdf.AddCell(space1)

        '資訊
        Dim pdf_Info As PdfPTable = New PdfPTable(2)

        Dim idtxt As PdfPCell = New PdfPCell(New Phrase(id.Text & "", unit_font))
        idtxt.Colspan = 1
        idtxt.Border = 0
        idtxt.PaddingLeft = 20.0F
        pdf_Info.AddCell(idtxt)

        Dim datetxt As PdfPCell = New PdfPCell(New Phrase(date_txt.Text & "", unit_font))
        datetxt.Colspan = 1
        datetxt.Border = 0
        datetxt.PaddingLeft = 20.0F
        pdf_Info.AddCell(datetxt)

        Dim positiontxt As PdfPCell = New PdfPCell(New Phrase(position_txt.Text & "", unit_font))
        positiontxt.PaddingBottom = 10.0F
        positiontxt.Colspan = 1
        positiontxt.Border = 0
        positiontxt.PaddingLeft = 20.0F
        pdf_Info.AddCell(positiontxt)

        Dim feetypetxt As PdfPCell = New PdfPCell(New Phrase(feetype_txt.Text & "", unit_font))
        feetypetxt.PaddingBottom = 10.0F
        feetypetxt.Colspan = 1
        feetypetxt.Border = 0
        feetypetxt.PaddingLeft = 20.0F
        pdf_Info.AddCell(feetypetxt)

        Dim info As PdfPCell = New PdfPCell(pdf_Info)
        info.Colspan = 7
        info.Border = 0
        pdf.AddCell(info)


        Dim pdf_table As PdfPTable = New PdfPTable(New Single() {1, 1, 1, 0.7, 0.8, 0.6, 0.9})

        Dim kind1 As PdfPCell = New PdfPCell(New Phrase("種類", head_font))
        kind1.Colspan = 1
        kind1.HorizontalAlignment = Element.ALIGN_CENTER
        kind1.PaddingBottom = 5.0F
        pdf_table.AddCell(kind1)

        Dim cap As PdfPCell = New PdfPCell(New Phrase("契約容量", head_font))
        cap.Colspan = 1
        cap.HorizontalAlignment = Element.ALIGN_CENTER
        cap.PaddingBottom = 5.0F
        'cap.Border = 0
        pdf_table.AddCell(cap)

        Dim mand As PdfPCell = New PdfPCell(New Phrase("最大需量(kw)", head_font))
        mand.Colspan = 1
        mand.HorizontalAlignment = Element.ALIGN_CENTER
        mand.PaddingBottom = 5.0F
        'mand.Border = 0
        pdf_table.AddCell(mand)

        Dim kind2 As PdfPCell = New PdfPCell(New Phrase("種類", head_font))
        kind2.Colspan = 1
        kind2.HorizontalAlignment = Element.ALIGN_CENTER
        kind2.PaddingBottom = 5.0F
        'kind2.Border = 0
        pdf_table.AddCell(kind2)

        Dim kwh As PdfPCell = New PdfPCell(New Phrase("用電量(KWH)", head_font))
        kwh.Colspan = 1
        kwh.HorizontalAlignment = Element.ALIGN_CENTER
        kwh.PaddingBottom = 5.0F
        'kwh.Border = 0
        pdf_table.AddCell(kwh)

        Dim price As PdfPCell = New PdfPCell(New Phrase("單價($)", head_font))
        price.Colspan = 1
        price.HorizontalAlignment = Element.ALIGN_CENTER
        price.PaddingBottom = 5.0F
        'price.Border = 0
        pdf_table.AddCell(price)

        Dim sum As PdfPCell = New PdfPCell(New Phrase("總計($)", head_font))
        sum.Colspan = 1
        sum.HorizontalAlignment = Element.ALIGN_CENTER
        sum.PaddingBottom = 5.0F
        'sum.Border = 0
        pdf_table.AddCell(sum)

        '--
        Dim rushtxt As PdfPCell = New PdfPCell(New Phrase("尖峰", unit_font))
        rushtxt.Colspan = 1
        rushtxt.HorizontalAlignment = Element.ALIGN_CENTER
        rushtxt.PaddingBottom = 5.0F
        pdf_table.AddCell(rushtxt)

        Dim RsuhCap As PdfPCell = New PdfPCell(New Phrase("" & RsuhCapTxt.Text & "", unit_font))
        RsuhCap.Colspan = 1
        RsuhCap.HorizontalAlignment = Element.ALIGN_CENTER
        RsuhCap.PaddingBottom = 5.0F
        pdf_table.AddCell(RsuhCap)

        Dim RushOver As PdfPCell = New PdfPCell(New Phrase("" & RushOverTxt.Text & "", unit_font))
        RushOver.Colspan = 1
        RushOver.HorizontalAlignment = Element.ALIGN_CENTER
        RushOver.PaddingBottom = 5.0F
        pdf_table.AddCell(RushOver)

        rushtxt.Colspan = 1
        rushtxt.HorizontalAlignment = Element.ALIGN_CENTER
        rushtxt.PaddingBottom = 5.0F
        pdf_table.AddCell(rushtxt)

        Dim RushHour As PdfPCell = New PdfPCell(New Phrase("" & RushHourTxt.Text & "", unit_font))
        RushHour.Colspan = 1
        RushHour.HorizontalAlignment = Element.ALIGN_CENTER
        RushHour.PaddingBottom = 5.0F
        pdf_table.AddCell(RushHour)

        Dim RushPrice As PdfPCell = New PdfPCell(New Phrase("" & RushPriceTxt.Text & "", unit_font))
        RushPrice.Colspan = 1
        RushPrice.HorizontalAlignment = Element.ALIGN_CENTER
        RushPrice.PaddingBottom = 5.0F
        pdf_table.AddCell(RushPrice)

        Dim RushSum As PdfPCell = New PdfPCell(New Phrase("" & RushSumTxt.Text & "", unit_font))
        RushSum.Colspan = 1
        RushSum.HorizontalAlignment = Element.ALIGN_CENTER
        RushSum.PaddingBottom = 5.0F
        pdf_table.AddCell(RushSum)

        '--
        Dim halftxt As PdfPCell = New PdfPCell(New Phrase("" & HalfHour_txt.Text & "", unit_font))
        halftxt.Colspan = 1
        halftxt.HorizontalAlignment = Element.ALIGN_CENTER
        halftxt.PaddingBottom = 5.0F
        pdf_table.AddCell(halftxt)

        Dim HalfCap As PdfPCell = New PdfPCell(New Phrase("" & HalfCapTxt.Text & "", unit_font))
        HalfCap.Colspan = 1
        HalfCap.HorizontalAlignment = Element.ALIGN_CENTER
        HalfCap.PaddingBottom = 5.0F
        pdf_table.AddCell(HalfCap)

        Dim HalfOver As PdfPCell = New PdfPCell(New Phrase("" & HalfOverTxt.Text & "", unit_font))
        HalfOver.Colspan = 1
        HalfOver.HorizontalAlignment = Element.ALIGN_CENTER
        HalfOver.PaddingBottom = 5.0F
        pdf_table.AddCell(HalfOver)

        halftxt = New PdfPCell(New Phrase("半尖峰", unit_font))
        halftxt.Colspan = 1
        halftxt.HorizontalAlignment = Element.ALIGN_CENTER
        halftxt.PaddingBottom = 5.0F
        pdf_table.AddCell(halftxt)

        Dim HalfHour As PdfPCell = New PdfPCell(New Phrase("" & HalfHourTxt.Text & "", unit_font))
        HalfHour.Colspan = 1
        HalfHour.HorizontalAlignment = Element.ALIGN_CENTER
        HalfHour.PaddingBottom = 5.0F
        pdf_table.AddCell(HalfHour)

        Dim HalfPrice As PdfPCell = New PdfPCell(New Phrase("" & HalfPriceTxt.Text & "", unit_font))
        HalfPrice.Colspan = 1
        HalfPrice.HorizontalAlignment = Element.ALIGN_CENTER
        HalfPrice.PaddingBottom = 5.0F
        pdf_table.AddCell(HalfPrice)

        Dim HalfSum As PdfPCell = New PdfPCell(New Phrase("" & HalfSumTxt.Text & "", unit_font))
        HalfSum.Colspan = 1
        HalfSum.HorizontalAlignment = Element.ALIGN_CENTER
        HalfSum.PaddingBottom = 5.0F
        pdf_table.AddCell(HalfSum)

        '--
        Dim sathalftxt As PdfPCell = New PdfPCell(New Phrase("週六半尖峰", unit_font))
        sathalftxt.Colspan = 1
        sathalftxt.HorizontalAlignment = Element.ALIGN_CENTER
        sathalftxt.PaddingBottom = 5.0F
        pdf_table.AddCell(sathalftxt)

        Dim SatHalfCap As PdfPCell = New PdfPCell(New Phrase("" & SatHalfCapTxt.Text & "", unit_font))
        SatHalfCap.Colspan = 1
        SatHalfCap.HorizontalAlignment = Element.ALIGN_CENTER
        SatHalfCap.PaddingBottom = 5.0F
        pdf_table.AddCell(SatHalfCap)

        Dim SatHalfOver As PdfPCell = New PdfPCell(New Phrase("" & SatHalfOverTxt.Text & "", unit_font))
        SatHalfOver.Colspan = 1
        SatHalfOver.HorizontalAlignment = Element.ALIGN_CENTER
        SatHalfOver.PaddingBottom = 5.0F
        pdf_table.AddCell(SatHalfOver)

        sathalftxt.Colspan = 1
        sathalftxt.HorizontalAlignment = Element.ALIGN_CENTER
        sathalftxt.PaddingBottom = 5.0F
        pdf_table.AddCell(sathalftxt)

        Dim SatHalfHour As PdfPCell = New PdfPCell(New Phrase("" & SatHalfHourTxt.Text & "", unit_font))
        SatHalfHour.Colspan = 1
        SatHalfHour.HorizontalAlignment = Element.ALIGN_CENTER
        SatHalfHour.PaddingBottom = 5.0F
        pdf_table.AddCell(SatHalfHour)

        Dim SatHalfPrice As PdfPCell = New PdfPCell(New Phrase("" & SatHalfPriceTxt.Text & "", unit_font))
        SatHalfPrice.Colspan = 1
        SatHalfPrice.HorizontalAlignment = Element.ALIGN_CENTER
        SatHalfPrice.PaddingBottom = 5.0F
        pdf_table.AddCell(SatHalfPrice)

        Dim SatHalfSum As PdfPCell = New PdfPCell(New Phrase("" & SatHalfSumTxt.Text & "", unit_font))
        SatHalfSum.Colspan = 1
        SatHalfSum.HorizontalAlignment = Element.ALIGN_CENTER
        SatHalfSum.PaddingBottom = 5.0F
        pdf_table.AddCell(SatHalfSum)

        '--
        Dim offtxt As PdfPCell = New PdfPCell(New Phrase("離峰", unit_font))
        offtxt.Colspan = 1
        offtxt.HorizontalAlignment = Element.ALIGN_CENTER
        offtxt.PaddingBottom = 5.0F
        pdf_table.AddCell(offtxt)

        Dim OffCap As PdfPCell = New PdfPCell(New Phrase("" & OffCapTxt.Text & "", unit_font))
        OffCap.Colspan = 1
        OffCap.HorizontalAlignment = Element.ALIGN_CENTER
        OffCap.PaddingBottom = 5.0F
        pdf_table.AddCell(OffCap)

        Dim OffOver As PdfPCell = New PdfPCell(New Phrase("" & OffOverTxt.Text & "", unit_font))
        OffOver.Colspan = 1
        OffOver.HorizontalAlignment = Element.ALIGN_CENTER
        OffOver.PaddingBottom = 5.0F
        pdf_table.AddCell(OffOver)

        offtxt.Colspan = 1
        offtxt.HorizontalAlignment = Element.ALIGN_CENTER
        offtxt.PaddingBottom = 5.0F
        pdf_table.AddCell(offtxt)

        Dim OffHour As PdfPCell = New PdfPCell(New Phrase("" & OffHourTxt.Text & "", unit_font))
        OffHour.Colspan = 1
        OffHour.HorizontalAlignment = Element.ALIGN_CENTER
        OffHour.PaddingBottom = 5.0F
        pdf_table.AddCell(OffHour)

        Dim OffPrice As PdfPCell = New PdfPCell(New Phrase("" & OffPriceTxt.Text & "", unit_font))
        OffPrice.Colspan = 1
        OffPrice.HorizontalAlignment = Element.ALIGN_CENTER
        OffPrice.PaddingBottom = 5.0F
        pdf_table.AddCell(OffPrice)

        Dim OffSum As PdfPCell = New PdfPCell(New Phrase("" & OffSumTxt.Text & "", unit_font))
        OffSum.Colspan = 1
        OffSum.HorizontalAlignment = Element.ALIGN_CENTER
        OffSum.PaddingBottom = 5.0F
        pdf_table.AddCell(OffSum)

        '--
        Dim pdf_fee1 As PdfPTable = New PdfPTable(3)
        Dim BasicFee As PdfPCell = New PdfPCell(New Phrase("基本電費：", title_font))
        BasicFee.Colspan = 2
        BasicFee.HorizontalAlignment = Element.ALIGN_RIGHT
        BasicFee.PaddingBottom = 5.0F
        BasicFee.Border = 0
        pdf_fee1.AddCell(BasicFee)
        BasicFee = New PdfPCell(New Phrase("" & BasicFeeTxt.Text & "  元", title_font))
        BasicFee.Colspan = 1
        BasicFee.HorizontalAlignment = Element.ALIGN_RIGHT
        BasicFee.PaddingBottom = 5.0F
        BasicFee.PaddingRight = 10.0F
        BasicFee.Border = 0
        pdf_fee1.AddCell(BasicFee)

        Dim OverFee As PdfPCell = New PdfPCell(New Phrase("超約附加費：", title_font))
        OverFee.Colspan = 2
        OverFee.HorizontalAlignment = Element.ALIGN_RIGHT
        OverFee.PaddingBottom = 5.0F
        OverFee.Border = 0
        pdf_fee1.AddCell(OverFee)
        OverFee = New PdfPCell(New Phrase("" & OverFeeTxt.Text & "  元", title_font))
        OverFee.Colspan = 1
        OverFee.HorizontalAlignment = Element.ALIGN_RIGHT
        OverFee.PaddingBottom = 5.0F
        OverFee.PaddingRight = 10.0F
        OverFee.Border = 0
        pdf_fee1.AddCell(OverFee)

        Dim content1 As PdfPCell = New PdfPCell(pdf_fee1)
        content1.Colspan = 3
        pdf_table.AddCell(content1)

        Dim pdf_fee2 As PdfPTable = New PdfPTable(4)
        Dim FlowFee As PdfPCell = New PdfPCell(New Phrase("總流動電費：", title_font))
        FlowFee.Colspan = 3
        FlowFee.HorizontalAlignment = Element.ALIGN_RIGHT
        FlowFee.VerticalAlignment = 1
        FlowFee.PaddingTop = 10.0F
        FlowFee.PaddingBottom = 5.0F
        FlowFee.Border = 0
        pdf_fee2.AddCell(FlowFee)
        FlowFee = New PdfPCell(New Phrase("" & FlowFeeTxt.Text & "  元", title_font))
        FlowFee.Colspan = 1
        FlowFee.HorizontalAlignment = Element.ALIGN_RIGHT
        FlowFee.VerticalAlignment = 1
        FlowFee.PaddingTop = 10.0F
        FlowFee.PaddingBottom = 5.0F
        FlowFee.PaddingRight = 10.0F
        FlowFee.Border = 0
        pdf_fee2.AddCell(FlowFee)

        Dim content2 As PdfPCell = New PdfPCell(pdf_fee2)
        content2.Colspan = 4
        pdf_table.AddCell(content2)

        Dim all As PdfPCell = New PdfPCell(pdf_table)
        all.Colspan = 7
        pdf.AddCell(all)

        Dim AvgFee As PdfPCell = New PdfPCell(New Phrase("平均每度用電：", sum_font))
        AvgFee.HorizontalAlignment = Element.ALIGN_RIGHT
        AvgFee.Colspan = 2
        AvgFee.PaddingBottom = 5.0F
        AvgFee.Border = 0
        pdf.AddCell(AvgFee)
        AvgFee = New PdfPCell(New Phrase("" & AvgFeeTxt.Text & "  元", sum_font))
        AvgFee.HorizontalAlignment = Element.ALIGN_RIGHT
        AvgFee.Colspan = 1
        AvgFee.PaddingBottom = 5.0F
        AvgFee.PaddingRight = 10.0F
        AvgFee.Border = 0
        pdf.AddCell(AvgFee)

        Dim TotalFee As PdfPCell = New PdfPCell(New Phrase("總計：", sum_font))
        TotalFee.HorizontalAlignment = Element.ALIGN_RIGHT
        TotalFee.Colspan = 3
        TotalFee.PaddingBottom = 5.0F
        TotalFee.Border = 0
        pdf.AddCell(TotalFee)
        TotalFee = New PdfPCell(New Phrase("" & TotalFeeTxt.Text & "  元", sum_font2))
        TotalFee.HorizontalAlignment = Element.ALIGN_RIGHT
        TotalFee.Colspan = 1
        TotalFee.PaddingBottom = 5.0F
        TotalFee.PaddingRight = 10.0F
        TotalFee.Border = 0
        pdf.AddCell(TotalFee)

        doc.Add(pdf)
        doc.Close()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中
    End Sub
End Class
