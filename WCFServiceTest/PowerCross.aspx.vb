Imports System.Data.OleDb
Imports System.Data

Partial Class PowerCross
    'Inherits TreeViewCheck
    Inherits ObjectBuilding

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Const _ScrollPosX As String = "_ScrollPosX"
        'Const _ScrollPosY As String = "_ScrollPosY"

        '' 頁面 Sumbit 時，記錄 Panel 的水平及垂直捲軸位置
        'Dim sScript As String = Nothing
        'sScript = "window.document.getElementById('" & _ScrollPosX & "').value =" & _
        '        "window.document.getElementById('" & Panel1.ClientID & "').scrollLeft;"
        'sScript = sScript & _
        '        "window.document.getElementById('" & _ScrollPosY & "').value = " & _
        '        "window.document.getElementById('" & Panel1.ClientID & "').scrollTop;"

        'Me.ClientScript.RegisterOnSubmitStatement(Me.GetType, "SavePanelScroll", sScript)

        '' 向 Page 物件註冊隱藏值維護 Panel 捲軸位置
        'Me.ClientScript.RegisterHiddenField(_ScrollPosX, Request(_ScrollPosX))
        'Me.ClientScript.RegisterHiddenField(_ScrollPosY, Request(_ScrollPosY))

        'sScript = "window.document.getElementById('" & Panel1.ClientID & "').scrollLeft = " & Request(_ScrollPosX) & ";" & _
        '       "window.document.getElementById('" & Panel1.ClientID & "').scrollTop = " & Request(_ScrollPosY) & ";"

        'Me.ClientScript.RegisterStartupScript(Me.GetType, "SetPanelScroll", sScript, True)

        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入(Connection timed out, please sign in again.)"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then

                '//群組下拉是選單
                Dim iSelIndex As Integer = 0
                BuildingDropDownList(Group_DropDownList, iSelIndex)
                If Session("Account") = "admin" Then    '系統管理者
                    If Not (Session("Group01") Is Nothing) Then
                        BuildingTree(Meter_TreeView, Session("Group01"), CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
                    End If
                Else
                    Account_DropDownList_SelectedIndexChanged(Nothing, Nothing)
                End If

                If Me.Meter_TreeView.SelectedNode IsNot Nothing Then
                    Meter_TreeView_SelectedNodeChanged(Nothing, Nothing)
                End If

                Date_Info.Text = Now.ToString("yyyy/MM")
            End If
        End If
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)

    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Or Meter_TreeView.SelectedNode.Text <> "The meter has not been defined." Then
            'Panel_Chart.Visible = False
            'Session("SelectedNode") = Meter_TreeView.SelectedNode.Text
            Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
            Dim value() As String = node_value.Split(vbCrLf)
            Dim account As String = value(0).Split(":").GetValue(1)
            Dim eco_account As String = value(1).Split(":").GetValue(1)
            Dim ctrlnr As String = value(2).Split(":").GetValue(1)
            Dim meterid As String = value(3).Split(":").GetValue(1)

            Dim strSQL As String = " exec [ECOSMART].[dbo].[ReadMeterTree] 1,'" & account & "', " & ctrlnr & "," & meterid

            Using conn As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString())
                If conn.State = 0 Then conn.Open()
                Using cmd As New OleDbCommand(strSQL, conn)
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            eco5_position.Text = dr("ECO_Position").ToString
                            If dr("ECO_Enabled") = True Then
                                If Session("language") = "en" Then
                                    eco5_enabled.Text = "-ECO5 Enabled-"
                                Else
                                    eco5_enabled.Text = "-ECO5已啟用-"
                                End If
                                eco5_enabled.ForeColor = Drawing.Color.DarkGreen
                            Else
                                If Session("language") = "en" Then
                                    eco5_enabled.Text = "-ECO5 Not enabled-"
                                Else
                                    eco5_enabled.Text = "-ECO5未啟用-"
                                End If
                                eco5_enabled.ForeColor = Drawing.Color.DarkRed
                            End If
                            eco5_id.Text = dr("CtrlNr").ToString
                            eco5_account.Text = dr("ECO_Account").ToString
                            meter_id.Text = dr("MeterID").ToString
                            If dr("MeterType").ToString <> "" Then
                                Select Case dr("MeterType").ToString
                                    Case 1
                                        MeterType.Text = "A40"
                                    Case 2
                                        MeterType.Text = "DM2436"
                                    Case 3
                                        MeterType.Text = "CT-1700"
                                    Case 4
                                        MeterType.Text = "CT713P"
                                    Case 5
                                        MeterType.Text = "SPM-8"
                                    Case 6
                                        MeterType.Text = "PM710"
                                    Case 7
                                        If Session("language") = "en" Then
                                            MeterType.Text = "Other"
                                        Else
                                            MeterType.Text = "其它"
                                        End If
                                End Select
                            End If
                            drawnr.Text = dr("DrawNr").ToString
                            InstallPosition.Text = dr("InstallPosition").ToString
                            If dr("Enabled") = True Then
                                enabled_img.ImageUrl = "img\on.png"
                            Else
                                enabled_img.ImageUrl = "img\off.png"
                            End If

                            If dr("UpLoadStatus") < 6 Then    '//相差超過5分鐘 表斷線
                                upload_img.ImageUrl = "img/GreenBall.png"
                            Else
                                upload_img.ImageUrl = "img/RedBall.png"
                            End If
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        If CtrlNr_CB.Checked = False And MeterId_CB.Checked = False And Position_CB.Checked = False And LineNum_CB.Checked = False Then
            Account_DropDownList_SelectedIndexChanged(Nothing, Nothing)
        Else
            Dim cmdIndex(3) As Boolean
            cmdIndex(0) = CtrlNr_CB.Checked
            cmdIndex(1) = MeterId_CB.Checked
            cmdIndex(2) = Position_CB.Checked
            cmdIndex(3) = LineNum_CB.Checked

            For Each node As TreeNode In Meter_TreeView.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
    End Sub

    'Protected Sub ViewDetails_btn_Click(sender As Object, e As ImageClickEventArgs) Handles ViewDetails_btn.Click
    '    If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Then
    '        'Panel_Chart.Visible = True
    '        'Session("SelectedNode") = Meter_TreeView.SelectedNode.Text
    '        Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
    '        Dim value() As String = node_value.Split(vbCrLf)
    '        Dim ctrlnr As String = value(2).Split(":").GetValue(1)
    '        Dim meterid As String = value(3).Split(":").GetValue(1)
    '        Dim strcon, strSQL As String
    '        If Session("Rank") = 2 Then
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(Group_DropDownList.SelectedValue) & ""
    '            strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Account_admin(Group_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
    '        Else
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
    '            strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Session("Account_admin") & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
    '        End If

    '        '用電量
    '        Dim sql_E As String = "SELECT SUBSTRING(T1.RecDate,6,5) AS 時間," & _
    '        "(CASE WHEN SUBSTRING(SUBSTRING(T1.RecDate,1,10),9,10)='01' THEN isnull(T1.RushHour,0) ELSE (isnull(T1.RushHour,0) - isnull(T2.RushHour,0)) END) AS 尖峰, " & _
    '        "(CASE WHEN SUBSTRING(SUBSTRING(T1.RecDate,1,10),9,10)='01' THEN isnull(T1.HalfHour,0) ELSE (isnull(T1.HalfHour,0) - isnull(T2.HalfHour,0)) END) AS 半尖峰, " & _
    '        "(CASE WHEN SUBSTRING(SUBSTRING(T1.RecDate,1,10),9,10)='01' THEN isnull(T1.SatHalfHour,0) ELSE (isnull(T1.SatHalfHour,0) - isnull(T2.SatHalfHour,0)) END) AS 週六半尖峰, " & _
    '        "(CASE WHEN SUBSTRING(SUBSTRING(T1.RecDate,1,10),9,10)='01' THEN isnull(T1.OffHour,0) ELSE (isnull(T1.OffHour,0) - isnull(T2.OffHour,0)) END) AS 離峰 " & _
    '        "FROM (SELECT RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER " & _
    '        "(Partition By Replace(Convert(nvarchar(10),RecDate,120),'-','/') order by recdate desc) as Sort " & _
    '        "FROM PowerRecord WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " and SUBSTRING(Replace(Convert(nvarchar(10),RecDate,120),'-','/'),1,7) = '" & Date_Info.Text & "') T1 left JOIN " & _
    '        "(SELECT Replace(Convert(nvarchar(10),RecDate,120),'-','/') AS RecDate,RushHour, HalfHour, SatHalfHour, OffHour FROM " & _
    '        "(SELECT Replace(Convert(nvarchar(10),RecDate,120),'-','/') AS RecDate,RushHour, HalfHour, SatHalfHour, OffHour, ROW_NUMBER() OVER " & _
    '        "(Partition By Replace(Convert(nvarchar(10),RecDate,120),'-','/') order by recdate desc) as Sort " & _
    '        "FROM PowerRecord WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " and SUBSTRING(Replace(Convert(nvarchar(10),RecDate,120),'-','/'),1,7) = '" & Date_Info.Text & "') Record " & _
    '        "WHERE Record.sort = 1 ) AS T2 ON convert(char(10),dateadd(day,-1,T1.RecDate),111) = T2.RecDate " & _
    '        "WHERE t1.Sort = 1 ORDER BY T1.RecDate"

    '        Dim sql_W As String = "SELECT SUBSTRING(RecDate,6,5) AS 時間,Round(MAX(W),2) AS 功率最大值,Round(AVG(W),2) AS 功率平均值 FROM PowerRecord " & _
    '                              "WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND SUBSTRING(Replace(Convert(nvarchar(10),RecDate,120),'-','/'),1,7) = '" & Date_Info.Text & "' GROUP BY SUBSTRING(RecDate,6,5) ORDER BY SUBSTRING(RecDate,6,5)"

    '        Try
    '            Chart1.Title.Text = "電能比對圖(" & eco5_id.Text & "-" & meter_id.Text & " " & InstallPosition.Text & ")"
    '            Dim da_E As OleDbDataAdapter = New OleDbDataAdapter(sql_E, strcon)
    '            Dim dt_E As DataTable = New DataTable
    '            da_E.Fill(dt_E)
    '            Dim key_E As DataColumn() = New DataColumn(0) {}
    '            key_E(0) = dt_E.Columns("時間")
    '            dt_E.PrimaryKey = key_E

    '            Dim da_W As OleDbDataAdapter = New OleDbDataAdapter(sql_W, strcon)
    '            Dim dt_W As DataTable = New DataTable
    '            da_W.Fill(dt_W)
    '            Dim key_W As DataColumn() = New DataColumn(0) {}
    '            key_W(0) = dt_W.Columns("時間")
    '            dt_W.PrimaryKey = key_W

    '            If dt_E.Rows.Count > 0 And dt_W.Rows.Count > 0 Then
    '                dt_E.Merge(dt_W, False, MissingSchemaAction.Add)
    '                Chart1.DataSource = dt_E
    '                Chart1.DataBind()
    '                Chart1.Visible = True
    '                img_btn.Visible = True
    '            Else
    '                Chart1.Visible = False
    '                img_btn.Visible = False
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '            End If
    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try
    '    End If
    'End Sub
End Class
