Imports System.Data.OleDb
Imports System.Data

Partial Class PowerProportionS
    'Inherits TreeViewCheck
    Inherits ObjectBuilding

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

                '初始化日期區間
                Date_Info_S.Text = DateAdd(DateInterval.Day, -2, Now).ToString("yyyy/MM/dd")
                'Date_Info_E.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")
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
            'Dim strcon, strSQL As String
            'If Session("Rank") = 2 Then
            '    strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(Group_DropDownList.SelectedValue) & ""
            '    strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Find_AdAccount(Group_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
            'Else
            '    strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
            '    strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Session("Account_admin") & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
            'End If

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

    '        Dim SelectItem As String = Nothing
    '        For i = 0 To 3
    '            If E_CBList_S.Items(i).Selected Then
    '                If i = 0 Then
    '                    SelectItem &= E_CBList_S.Items(0).Value & " AS 尖峰,"
    '                ElseIf i = 1 Then
    '                    SelectItem &= E_CBList_S.Items(1).Value & " AS 半尖峰,"
    '                ElseIf i = 2 Then
    '                    SelectItem &= E_CBList_S.Items(2).Value & " AS 週六半尖峰,"
    '                ElseIf i = 3 Then
    '                    SelectItem &= E_CBList_S.Items(3).Value & " AS 離峰,"
    '                End If
    '            End If
    '        Next

    '        Dim sql As String = "select top 1 " & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND substring(RecDate,1,10) = '" & Date_Info_S.Text & "' order by Recdate DESC"
    '        Try
    '            Dim da As OleDbDataAdapter = New OleDbDataAdapter(sql, strcon)
    '            Dim dt As DataTable = New DataTable
    '            da.Fill(dt)

    '            If dt.Rows.Count <> 0 Then
    '                PieChart.Visible = True
    '                PieChart.Title.Text = "電量比重圖(" & eco5_id.Text & "-" & meter_id.Text & " " & InstallPosition.Text & ")"
    '                Dim dt2 As DataTable = New DataTable
    '                dt2.Columns.Add("Items")
    '                dt2.Columns.Add("Values")
    '                For i = 0 To dt.Columns.Count - 1
    '                    dt2.Rows.Add(dt.Columns(i).ColumnName.ToString, dt.Rows(0).Item(i))
    '                Next

    '                PieChart.DataSource = dt2
    '                img_btn.Visible = True
    '            Else
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '                PieChart.Visible = False
    '                img_btn.Visible = False
    '            End If

    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try
    '    End If
    'End Sub
End Class
