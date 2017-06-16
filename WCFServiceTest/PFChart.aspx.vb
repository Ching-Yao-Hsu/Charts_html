Imports System.Data.OleDb
Imports System.Data
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing
Imports System.IO

Partial Class PFChart
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.SelectParameters.Clear()

        Dim group As String = Request.QueryString("group")
        Dim ctrlnr As String = Request.QueryString("ctrlnr")
        Dim meterid As String = Request.QueryString("meterid")
        Dim position As String = Request.QueryString("position")
        Dim start_time As String = Request.QueryString("datetime").Split(",").GetValue(0)
        Dim end_time As String = Request.QueryString("datetime").Split(",").GetValue(1)
        Dim interval As String = Request.QueryString("interval")
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        Dim sql As String = "select convert(datetime,RecDate) as RecDate,CASE WHEN (PF<0) THEN (PF+2) ELSE PF END AS PF from PowerRecord"
        '時間間隔
        If interval = "1" Then
            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
        ElseIf interval = "5" Then
            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
        ElseIf interval = "30" Then
            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
        ElseIf interval = "60" Then
            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
        End If

        Try
            SqlDataSource1.ConnectionString = strcon
            SqlDataSource1.ProviderName = "System.Data.OleDb"
            SqlDataSource1.SelectCommand = sql
            SqlDataSource1.SelectParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
            SqlDataSource1.SelectParameters.Add("MeterID", TypeCode.String, meterid)
            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, start_time)
            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, end_time)
            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)   '資料筆數
            If dv.Count > 0 Then
                'Chart_PF.Title.Text = "功因趨勢圖(" & ctrlnr & "-" & meterid & " " & position & ")"
                Chart_PF.Visible = True

                '背景
                Chart_PF.BackColor = Drawing.Color.Azure
                Chart_PF.BackGradientStyle = GradientStyle.TopBottom
                Chart_PF.BorderlineWidth = 1
                Chart_PF.BorderlineColor = Color.Green
                Chart_PF.BorderlineDashStyle = ChartDashStyle.Solid
                Chart_PF.BorderSkin.SkinStyle = BorderSkinStyle.Emboss
                Chart_PF.ChartAreas(0).BackColor = Drawing.Color.Azure
                Chart_PF.ChartAreas(0).BackGradientStyle = GradientStyle.TopBottom

                '標題
                Dim title As New Title()
                title.Text = "功因趨勢圖(" & ctrlnr & "-" & meterid & " " & position & ")"
                title.Alignment = ContentAlignment.MiddleCenter
                title.Font = New System.Drawing.Font("Trebuchet MS", 16.0F, FontStyle.Regular)
                Chart_PF.Titles.Add(title)

                'X軸(時間)
                Chart_PF.ChartAreas(0).AxisX.MajorGrid.Enabled = False
                'Chart_PF.ChartAreas(0).AxisX.Interval = 4
                'Chart_PF.ChartAreas(0).AxisX.IntervalType = DataVisualization.Charting.DateTimeIntervalType.Hours
                'Chart_PF.Series(0).XValueType = DataVisualization.Charting.ChartValueType.Time
                Chart_PF.ChartAreas(0).AxisX.LabelStyle.Format = "{M/d HH:mm:ss}"
                Chart_PF.ChartAreas(0).AxisX.Title = "時間"

                'Y軸(功因)
                Chart_PF.ChartAreas(0).AxisY.MajorTickMark.Enabled = False
                'Chart_PF.ChartAreas(0).AxisY.LabelStyle.ForeColor = System.Drawing.Color.Transparent
                Chart_PF.ChartAreas(0).AxisY.Maximum = 2
                Chart_PF.ChartAreas(0).AxisY.Minimum = 0
                Chart_PF.ChartAreas(0).AxisY.Interval = 0.5
                Chart_PF.ChartAreas(0).AxisY.CustomLabels.Add(0, 0.5, "0 ~ 0.5")
                Chart_PF.ChartAreas(0).AxisY.CustomLabels.Add(0.5, 1, "0.5 ~ 1")
                Chart_PF.ChartAreas(0).AxisY.CustomLabels.Add(1, 1.5, "1 ~ -0.5")
                Chart_PF.ChartAreas(0).AxisY.CustomLabels.Add(1.5, 2, "-0.5 ~ 0")

                img_btn.Visible = True
            Else
                Dim msg As String = "查無資料"
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                Chart_PF.Visible = False
                img_btn.Visible = False
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub

    Protected Sub img_btn_Click(sender As Object, e As ImageClickEventArgs) Handles img_btn.Click
        Dim ms As New MemoryStream
        Chart_PF.SaveImage(ms, ChartImageFormat.Png)
        Dim array() As Byte = ms.ToArray

        Response.Clear()
        Response.Expires = 0
        Response.AppendHeader("content-disposition", "attachment;filename=功因趨勢圖_" + Now().ToString("yyyy/MM/dd HH:mm:ss") + ".png")
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8")
        Response.ContentType = "application/octet-stream"
        Response.OutputStream.Write(array, 0, array.Length)
        Response.End()
    End Sub
End Class
