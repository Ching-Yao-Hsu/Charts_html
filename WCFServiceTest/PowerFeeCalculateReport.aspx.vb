Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class PowerFeeCalculateReport
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
        Dim group As String = Request.QueryString("group")
        Dim datetime As String = Request.QueryString("datetime")
        Dim stxtValue As String = Request.QueryString("stxtValue")
        Dim sRadiovalue As String = Request.QueryString("sRadiovalue")
        Dim sSelf As String = Request.QueryString("sSelf")

        Dim aDate() As String = Split(datetime, "/")
        Dim atxtValue() As String = Split(stxtValue, ",")   '//契約容量

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConn").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        Dim strSQL As String = ""
        If Session("language").ToString = "en" Then
            strSQL = "SELECT RTrim(PRC.CtrlNr) + '-' + RTrim(PRC.MeterID) AS Number, MS.InstallPosition AS switchboard," & _
                                " isnull(MAX(PRC.Demand),0) AS Peak_Time_Demand,isnull(MAX(PRC.DemandHalf),0) AS Half_Peak_Time_Demand," & _
                                " isnull(MAX(PRC.DemandSatHalf),0) AS Saturday_Half_Peak_Time_Demand,isnull(MAX(PRC.DemandOff),0) AS Off_Peak_Time_Demand," & _
                                " MAX(PRC.RushHourMax) AS Peak_Time_Electricity, MAX(PRC.HalfHourMax) AS Half_Peak_Time_Electricity," & _
                                " MAX(PRC.SatHalfHourMax) AS Saturday_Half_Peak_Time_Electricity, MAX(PRC.OffHourMax) AS Off_Peak_Time_Electricity," & _
                                " MAX(PRC.RushHourMax)+MAX(PRC.HalfHourMax)+MAX(PRC.SatHalfHourMax)+MAX(PRC.OffHourMax) AS Total, 0 as Electricity_statistics" & _
                                " FROM PowerRecordCollection AS PRC, ECOSMART.dbo.ControllerSetup AS CS, ECOSMART.dbo.MeterSetup AS MS " & _
                                " WHERE PRC.CtrlNr=MS.CtrlNr and PRC.MeterID=MS.MeterID and CS.ECO_Account=MS.ECO_Account and CS.Account='" & Find_AdAccount(group) & "' and SUBSTRING(PRC.RecDate,1,7)='" & datetime & "' " & _
                                " GROUP BY SubString(PRC.RecDate,1,7), PRC.CtrlNr, PRC.MeterID, MS.InstallPosition;"
        Else
            strSQL = "SELECT RTrim(PRC.CtrlNr) + '-' + RTrim(PRC.MeterID) AS 編號, MS.InstallPosition AS 配電盤," & _
                                " isnull(MAX(PRC.Demand),0) AS 尖峰需量,isnull(MAX(PRC.DemandHalf),0) AS 半尖峰需量," & _
                                " isnull(MAX(PRC.DemandSatHalf),0) AS 週六半尖峰需量,isnull(MAX(PRC.DemandOff),0) AS 離峰需量," & _
                                " MAX(PRC.RushHourMax) AS 尖峰電量, MAX(PRC.HalfHourMax) AS 半尖峰電量," & _
                                " MAX(PRC.SatHalfHourMax) AS 週六半尖峰電量, MAX(PRC.OffHourMax) AS 離峰電量," & _
                                " MAX(PRC.RushHourMax)+MAX(PRC.HalfHourMax)+MAX(PRC.SatHalfHourMax)+MAX(PRC.OffHourMax) AS 總計, 0 as 電費總計" & _
                                " FROM PowerRecordCollection AS PRC, ECOSMART.dbo.ControllerSetup AS CS, ECOSMART.dbo.MeterSetup AS MS " & _
                                " WHERE PRC.CtrlNr=MS.CtrlNr and PRC.MeterID=MS.MeterID and CS.ECO_Account=MS.ECO_Account and CS.Account='" & Find_AdAccount(group) & "' and SUBSTRING(PRC.RecDate,1,7)='" & datetime & "' " & _
                                " GROUP BY SubString(PRC.RecDate,1,7), PRC.CtrlNr, PRC.MeterID, MS.InstallPosition;"
        End If

        '//0 編號, 1 配電盤, 2 尖峰需量, 3 半尖峰需量, 4  週六半尖峰需量, 5 離峰需量, 6 尖峰電量, 7 半尖峰電量, 8 週六半尖峰電量,  9 離峰電量, 10 總計, 11 電費總計
        Dim adPower As SqlDataAdapter = New SqlDataAdapter(strSQL, strcon)
        Dim dsPowers As DataSet = New DataSet()
        adPower.Fill(dsPowers, "Power01")
        Dim pRow As DataRow
        For Each pRow In dsPowers.Tables("Power01").Rows
            Select Case sRadiovalue
                Case "11"   '//表燈-非營業
                    '//Dim a As Single = ElectricBill.表燈非營業(Numer電量尖峰.Value + Numer電量半尖峰.Value + Numer電量週六.Value + Numer電量離峰.Value, ListBox1.SelectedIndex + 1(月份), Nothing)
                    'Dim sElectCost As String = ElectricBillCal.ElectricBill.表燈非營業(pRow.Item("尖峰電量").ToString, pRow.Item("半尖峰電量").ToString _
                    '                                                              , pRow.Item("週六半尖峰電量").ToString, pRow.Item("離峰電量").ToString _
                    '                                                              , Val(aDate(1)), Nothing)
                    Dim sElectCost As String = ElectricBillCal.ElectricBill.表燈非營業(pRow.Item(6).ToString, pRow.Item(7).ToString _
                                                              , pRow.Item(8).ToString, pRow.Item(9).ToString _
                                                              , Val(aDate(1)), Nothing)
                    'pRow.Item("電費總計") = Val(sElectCost)
                    pRow.Item(11) = Val(sElectCost)

                Case "12"   '//表燈-營業
                    '//a = ElectricBill.表燈營業用(Numer電量尖峰.Value + Numer電量半尖峰.Value + Numer電量週六.Value + Numer電量離峰.Value, ElectricBill.Month.Jan(月份), Nothing)
                    'Dim sElectCost As String = ElectricBillCal.ElectricBill.表燈營業用(pRow.Item("尖峰電量").ToString, pRow.Item("半尖峰電量").ToString _
                    '                                                              , pRow.Item("週六半尖峰電量").ToString, pRow.Item("離峰電量").ToString _
                    '                                                              , Val(aDate(1)), Nothing)
                    Dim sElectCost As String = ElectricBillCal.ElectricBill.表燈營業用(pRow.Item(6).ToString, pRow.Item(7).ToString _
                                                              , pRow.Item(8).ToString, pRow.Item(9).ToString _
                                                              , Val(aDate(1)), Nothing)
                    'pRow.Item("電費總計") = Val(sElectCost)
                    pRow.Item(11) = Val(sElectCost)

                Case "13"   '//表燈-時間
                    '//Dim b As ElectricBill.Bill = ElectricBill.表燈時間電價(Numer契約經常.Value, Numer契約半尖峰.Value, Numer契約週六.Value, _
                    '//                                   Numer契約離峰.Value, Numer需量經常.Value, Numer需量半尖峰.Value, Numer需量週六.Value, _
                    '//                                   Numer需量離峰.Value, Numer電量尖峰.Value, Numer電量半尖峰.Value, Numer電量週六.Value, Numer電量離峰.Value, _
                    '//                                   ListBox1.SelectedIndex + 1(月份), Nothing, False)
                    '//Label表燈時間基本.Text = b.Basic  :  Label表燈時間超約.Text = b.OverDemand  :  Label表燈時間流動.Text = b.Used  :  Label表燈時間.Text = b.Total
                    'Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.表燈時間電價(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                    '                                   , pRow.Item("尖峰需量").ToString, pRow.Item("半尖峰需量").ToString _
                    '                                   , pRow.Item("週六半尖峰需量").ToString, pRow.Item("離峰需量").ToString _
                    '                                   , pRow.Item("尖峰電量").ToString, pRow.Item("半尖峰電量").ToString _
                    '                                   , pRow.Item("週六半尖峰電量").ToString, pRow.Item("離峰電量").ToString _
                    '                                   , Val(aDate(1)), Nothing, False)
                    Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.表燈時間電價(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                                   , pRow.Item(2).ToString, pRow.Item(3).ToString _
                                   , pRow.Item(4).ToString, pRow.Item(5).ToString _
                                   , pRow.Item(6).ToString, pRow.Item(7).ToString _
                                   , pRow.Item(8).ToString, pRow.Item(9).ToString _
                                   , Val(aDate(1)), Nothing, False)
                    'pRow.Item("電費總計") = Val(sElectCost.Total)
                    pRow.Item(11) = Val(sElectCost.Total)

                Case "21"   '//低壓-非時間
                    '//b = ElectricBill.低壓非時間電價(Numer契約經常.Value, Numer契約半尖峰.Value, Numer需量經常.Value, Numer需量半尖峰.Value, _
                    '//           Numer電量尖峰.Value + Numer電量半尖峰.Value + Numer電量週六.Value + Numer電量離峰.Value, _
                    '//           ListBox1.SelectedIndex + 1, Nothing)
                    '// Label低壓非時間基本.Text = b.Basic : Label低壓非時間超約.Text = b.OverDemand : Label低壓非時間流動.Text = b.Used : Label低壓非時間.Text = b.Total
                    'Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.低壓非時間電價(Val(atxtValue(0)), Val(atxtValue(1)) _
                    '               , pRow.Item("尖峰需量").ToString, pRow.Item("半尖峰需量").ToString _
                    '               , Val(pRow.Item("尖峰電量").ToString) + Val(pRow.Item("半尖峰電量").ToString) + Val(pRow.Item("週六半尖峰電量").ToString) + Val(pRow.Item("離峰電量").ToString) _
                    '               , Val(aDate(1)), Nothing)
                    Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.低壓非時間電價(Val(atxtValue(0)), Val(atxtValue(1)) _
                                   , pRow.Item(2).ToString, pRow.Item(3).ToString _
                                   , Val(pRow.Item(6).ToString) + Val(pRow.Item(7).ToString) + Val(pRow.Item(8).ToString) + Val(pRow.Item(9).ToString) _
                                   , Val(aDate(1)), Nothing)
                    'pRow.Item("電費總計") = Val(sElectCost.Total)
                    pRow.Item(11) = Val(sElectCost.Total)

                Case "22"   '//低壓-時間
                    '//b = ElectricBill.低壓電力時間電價(Numer契約經常.Value, Numer契約半尖峰.Value, Numer契約週六.Value, _
                    '//                 Numer契約離峰.Value, Numer需量經常.Value, Numer需量半尖峰.Value, Numer需量週六.Value, _
                    '//                Numer需量離峰.Value, Numer電量尖峰.Value, Numer電量半尖峰.Value, Numer電量週六.Value, Numer電量離峰.Value, _
                    '//                ListBox1.SelectedIndex + 1, Nothing)
                    'Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.低壓電力時間電價(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                    '               , pRow.Item("尖峰需量").ToString, pRow.Item("半尖峰需量").ToString _
                    '               , pRow.Item("週六半尖峰需量").ToString, pRow.Item("離峰需量").ToString _
                    '               , pRow.Item("尖峰電量").ToString, pRow.Item("半尖峰電量").ToString _
                    '               , pRow.Item("週六半尖峰電量").ToString, pRow.Item("離峰電量").ToString _
                    '               , Val(aDate(1)), Nothing)
                    Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.低壓電力時間電價(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                                   , pRow.Item(2).ToString, pRow.Item(3).ToString _
                                   , pRow.Item(4).ToString, pRow.Item(5).ToString _
                                   , pRow.Item(6).ToString, pRow.Item(7).ToString _
                                   , pRow.Item(8).ToString, pRow.Item(9).ToString _
                                   , Val(aDate(1)), Nothing)
                    'pRow.Item("電費總計") = Val(sElectCost.Total)
                    pRow.Item(11) = Val(sElectCost.Total)

                Case "31"   '//高壓-二段式
                    '//b = ElectricBill.高壓電力時間電價二段式(Numer契約經常.Value, Numer契約半尖峰.Value, Numer契約週六.Value, _
                    '//        Numer契約離峰.Value, Numer需量經常.Value, Numer需量半尖峰.Value, Numer需量週六.Value, _
                    '//        Numer需量離峰.Value, Numer電量尖峰.Value, Numer電量半尖峰.Value, Numer電量週六.Value, Numer電量離峰.Value, _
                    '//        ListBox1.SelectedIndex + 1, Nothing)
                    'Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.高壓電力時間電價二段式(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                    '                 , pRow.Item("尖峰需量").ToString, pRow.Item("半尖峰需量").ToString _
                    '                 , pRow.Item("週六半尖峰需量").ToString, pRow.Item("離峰需量").ToString _
                    '                 , pRow.Item("尖峰電量").ToString, pRow.Item("半尖峰電量").ToString _
                    '                 , pRow.Item("週六半尖峰電量").ToString, pRow.Item("離峰電量").ToString _
                    '                 , Val(aDate(1)), Nothing
                    Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.高壓電力時間電價二段式(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                                     , pRow.Item(2).ToString, pRow.Item(3).ToString _
                                     , pRow.Item(4).ToString, pRow.Item(5).ToString _
                                     , pRow.Item(6).ToString, pRow.Item(7).ToString _
                                     , pRow.Item(8).ToString, pRow.Item(9).ToString _
                                     , Val(aDate(1)), Nothing)
                    'pRow.Item("電費總計") = Val(sElectCost.Total)
                    pRow.Item(11) = Val(sElectCost.Total)

                Case "32"   '//高壓-三段式
                    '//b = ElectricBill.高壓電力時間電價三段式(Numer契約經常.Value, Numer契約半尖峰.Value, Numer契約週六.Value, _
                    '//                 Numer契約離峰.Value, Numer需量經常.Value, Numer需量半尖峰.Value, Numer需量週六.Value, _
                    '//                Numer需量離峰.Value, Numer電量尖峰.Value, Numer電量半尖峰.Value, Numer電量週六.Value, Numer電量離峰.Value, _
                    '//                ListBox1.SelectedIndex + 1, Nothing)
                    'Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.高壓電力時間電價三段式(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                    '              , pRow.Item("尖峰需量").ToString, pRow.Item("半尖峰需量").ToString _
                    '              , pRow.Item("週六半尖峰需量").ToString, pRow.Item("離峰需量").ToString _
                    '              , pRow.Item("尖峰電量").ToString, pRow.Item("半尖峰電量").ToString _
                    '              , pRow.Item("週六半尖峰電量").ToString, pRow.Item("離峰電量").ToString _
                    '              , Val(aDate(1)), Nothing)
                    Dim sElectCost As ElectricBillCal.ElectricBill.Bill = ElectricBillCal.ElectricBill.高壓電力時間電價三段式(Val(atxtValue(0)), Val(atxtValue(1)), Val(atxtValue(2)), Val(atxtValue(3)) _
                                  , pRow.Item(2).ToString, pRow.Item(3).ToString _
                                  , pRow.Item(4).ToString, pRow.Item(5).ToString _
                                  , pRow.Item(6).ToString, pRow.Item(7).ToString _
                                  , pRow.Item(8).ToString, pRow.Item(9).ToString _
                                  , Val(aDate(1)), Nothing)
                    'pRow.Item("電費總計") = Val(sElectCost.Total)
                    pRow.Item(11) = Val(sElectCost.Total)

                Case "41"   '//自訂
                    '用電量來計算
                    'Dim sElectCost As String = (pRow.Item("尖峰電量") * Val(sSelf) + pRow.Item("半尖峰電量") * Val(sSelf) + pRow.Item("週六半尖峰電量") * Val(sSelf) + pRow.Item("離峰電量") * Val(sSelf)).ToString
                    Dim sElectCost As String = (pRow.Item(6) * Val(sSelf) + pRow.Item(7) * Val(sSelf) + pRow.Item(8) * Val(sSelf) + pRow.Item(9) * Val(sSelf)).ToString
                    'pRow.Item("電費總計") = Val(sElectCost)
                    pRow.Item(11) = Val(sElectCost)
            End Select

        Next

        If dsPowers.Tables("Power01").Rows.Count > 0 Then
            If Session("language").ToString = "en" Then
                RecDate.Text = "Month：" & datetime
            Else
                RecDate.Text = "月份：" & datetime
            End If

            GridView1.DataSource = dsPowers
            GridView1.DataBind()
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
    End Sub

    Protected Sub GridView1_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            '此方法輸出Excel會友空白行，只是移除表頭內容，<tr>還存在
            '將原有的表頭移除()
            'Dim oldCell As TableCellCollection = e.Row.Cells
            'oldCell.Clear()

            Dim ColumnCount As Integer = 0
            '多重表頭的第一列
            Dim gvRow As GridViewRow = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)

            Dim str01 As String = "", str02 As String = "", str03 As String = "", str04 As String = "", str05 As String = ""

            If Session("language").ToString = "en" Then
                str01 = "Peak Time"
                str02 = "Half Peak Time"
                str03 = "Saturday Half Peak Time"
                str04 = "Off Peak Time"
                str05 = "Total"
            Else
                str01 = "尖峰"
                str02 = "半尖峰"
                str03 = "週六半尖峰"
                str04 = "離峰"
                str05 = "總計"
            End If

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
            tc.ColumnSpan = 2 '所跨的column數
            gvRow.Cells.Add(tc) '新增
            ColumnCount += 2

            Dim count As Integer = 1

            tc = New TableCell()
            If Session("language").ToString = "en" Then
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
            e.Row.Cells(count + 1).Text = str01
            e.Row.Cells(count + 2).Text = str02
            e.Row.Cells(count + 3).Text = str03
            e.Row.Cells(count + 4).Text = str04
            count += 4
            ColumnCount += 4

            tc = New TableCell()
            If Session("language").ToString = "en" Then
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
            e.Row.Cells(count + 1).Text = str01
            e.Row.Cells(count + 2).Text = str02
            e.Row.Cells(count + 3).Text = str03
            e.Row.Cells(count + 4).Text = str04
            e.Row.Cells(count + 5).Text = str05
            count += 5
            ColumnCount += 5

            tc = New TableCell()
            'tc = e.Row.Cells(count + 1)
            tc.Text = ""
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Middle
            tc.BackColor = System.Drawing.Color.White
            tc.ForeColor = System.Drawing.Color.Black
            tc.RowSpan = 1
            tc.ColumnSpan = 1
            gvRow.Cells.Add(tc) '新增


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
            sFilename = "Electricity spreadsheet_" & Request.QueryString("datetime")
        Else
            sFilename = "電費試算表_" & Request.QueryString("datetime")
        End If
        sFilename = Server.UrlPathEncode(sFilename)
        'sFilename = Server.UrlPathEncode("電費試算表_" & Request.QueryString("datetime"))

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
            sFilename = Server.UrlPathEncode("Electricity spreadsheet_" & datetime)
            sTitle = "Electricity spreadsheet"
        Else
            sFilename = Server.UrlPathEncode("電費試算表_" & datetime)
            sTitle = "電費試算表"
        End If
        'Dim sFilename As String = Server.UrlPathEncode("電費試算表_" & datetime)

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
        Dim CountColumns As Integer = 12  '自動生成
        Dim CountRow As Integer = GridView1.Rows.Count
        Dim pdf As PdfPTable = New PdfPTable(2)

        pdf.HeaderRows = 1
        pdf.WidthPercentage = 90  '表格寬度百分比
        pdf.HorizontalAlignment = 1 '整個表格在文件的位置0=left、1=center、2=right

        '字型
        Dim title_font As Font = New Font(bfCh2, 14, Font.BOLD)
        Dim unit_font As Font = New Font(bfCh2, 10)
        Dim sum_font As Font = New Font(bfCh2, 14, Font.BOLD, BaseColor.RED)
        Dim head_font As Font = New Font(bfCh2, 10, Font.BOLD)
        Dim head_font2 As Font = New Font(bfCh2, 10)

        '表頭說明文字
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
        Dim pdf2 As PdfPTable = New PdfPTable(CountColumns)
        pdf2 = New PdfPTable(New Single() {0.6, 1.5, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.8, 0.8})
        pdf2.HeaderRows = 2
        pdf2.WidthPercentage = 90  '表格寬度百分比
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

        If Session("language").ToString = "en" Then
            sTitle = "Max Demand(kw)"
        Else
            sTitle = "最大需量(kw)"
        End If
        title = New PdfPCell(New Phrase(sTitle, head_font2))
        title.Colspan = 4
        title.HorizontalAlignment = 1
        title.PaddingBottom = 5.0F
        pdf2.AddCell(title)

        If Session("language").ToString = "en" Then
            sTitle = "Electricity consumption(KWH)"
        Else
            sTitle = "用電量(KWH)"
        End If
        title = New PdfPCell(New Phrase(sTitle, head_font2))
        title.Colspan = 5
        title.HorizontalAlignment = 1
        title.PaddingBottom = 5.0F
        pdf2.AddCell(title)

        title = New PdfPCell(New Phrase("", head_font2))
        title.Colspan = 1
        title.Rowspan = 1
        title.HorizontalAlignment = 1
        title.PaddingBottom = 5.0F
        pdf2.AddCell(title)

        '如果要自訂標題，就不要用迴圈跑
        Dim head As PdfPCell
        Dim i, j As Integer
        For i = 0 To CountColumns - 1
            head = New PdfPCell(New Phrase(GridView1.HeaderRow.Cells(i).Text, head_font))
            head.HorizontalAlignment = 1
            head.VerticalAlignment = 2
            head.PaddingBottom = 5.0F
            'head.NoWrap = True             '//表頭不會自動折行
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
