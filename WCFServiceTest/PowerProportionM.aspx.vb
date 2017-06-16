Imports System.Data.OleDb
Imports System.Data

Partial Class PowerProportionM
    'Inherits TreeViewCheck
    Inherits ObjectBuilding

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
                        BuildingTree(Meter_TreeView_Sum, Session("Group01"), CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
                        BuildingTree(Meter_TreeView, Session("Group01"), CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
                    End If
                Else
                    Account_DropDownList_SelectedIndexChanged(Nothing, Nothing)
                End If

                If Me.Meter_TreeView.SelectedNode IsNot Nothing Then
                    Meter_TreeView_SelectedNodeChanged(Nothing, Nothing)
                End If

                '初始化日期區間
                Date_Info_S.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM") & "/01"
                Date_Info_E.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")
            End If
        End If

    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView_Sum, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
        BuildingTree(Meter_TreeView, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
    End Sub

    'Protected Sub SqlQuery()
    '    If Meter_TreeView.CheckedNodes.Count < 1 Then
    '        Dim msg As String = "請勾選電表"
    '        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '    Else
    '        Dim strcon As String = Nothing
    '        Dim account As String = ""
    '        If Session("Rank") = 2 Then
    '            account = Account_admin(Group_DropDownList.SelectedValue)
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(Group_DropDownList.SelectedValue) & ""
    '        Else
    '            account = Session("Account_admin")
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
    '        End If

    '        '取得比對項目
    '        Dim select_item As String = Nothing
    '        Dim select_txt As String = Nothing

    '        If E_RBList.SelectedValue = "RushHour" Then
    '            select_item = "MAX(" & E_RBList.SelectedValue & ") AS Value"
    '            select_txt = "尖峰電量"
    '        ElseIf E_RBList.SelectedValue = "HalfHour" Then
    '            select_item = "MAX(" & E_RBList.SelectedValue & ") AS Value"
    '            select_txt = "半尖峰電量"
    '        ElseIf E_RBList.SelectedValue = "SatHalfHour" Then
    '            select_item = "MAX(" & E_RBList.SelectedValue & ") AS Value"
    '            select_txt = "週六半尖峰電量"
    '        ElseIf E_RBList.SelectedValue = "OffHour" Then
    '            select_item = "MAX(" & E_RBList.SelectedValue & ") AS Value"
    '            select_txt = "離峰電量"
    '        ElseIf E_RBList.SelectedValue = "KWh" Then
    '            select_item = "MAX(" & E_RBList.SelectedValue & ") AS Value"
    '            select_txt = "用電度數"
    '        End If

    '        '取得ECO5、電表編號
    '        Dim ctrlnr(Meter_TreeView.CheckedNodes.Count - 1) As String
    '        Dim meterid(Meter_TreeView.CheckedNodes.Count - 1) As String
    '        Dim check_count As Integer = 0
    '        For Each node As TreeNode In Meter_TreeView.CheckedNodes
    '            If node.Checked = True Then
    '                Dim node_value As String = node.ToolTip
    '                Dim value() As String = node_value.Split(vbCrLf)
    '                ctrlnr(check_count) = value(2).Split(":").GetValue(1)
    '                meterid(check_count) = value(3).Split(":").GetValue(1)
    '                check_count += 1
    '            End If
    '        Next

    '        Dim datetime As String = Nothing
    '        If Date_Info_S.Text.Substring(0, 7) = Date_Info_E.Text.Substring(0, 7) Then
    '            datetime = "substring(RecDate,1,10) = '" & Date_Info_E.Text & "'"
    '        End If

    '        '組合SQL字串
    '        Dim sqlstr(check_count - 1) As String
    '        Dim sql As String = Nothing
    '        For i = 0 To check_count - 1
    '            '同月
    '            If Date_Info_S.Text.Substring(0, 7) = Date_Info_E.Text.Substring(0, 7) Then
    '                sqlstr(i) = "SELECT MeterSetup.InstallPosition AS SelectItem, " & select_item & " FROM PowerRecord,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup WHERE PowerRecord.CtrlNr = " & ctrlnr(i) & " AND PowerRecord.MeterID = " & meterid(i) & " " & _
    '                            "AND PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & datetime & " GROUP BY MeterSetup.InstallPosition "
    '            Else
    '                '跨月
    '                Dim t1 As String = DateAdd("d", -1, Date_Info_S.Text).ToString("yyyy/MM/dd")
    '                Dim t2 As String = DateAdd("d", -1, DateAdd("m", 1, Date_Info_S.Text.Substring(0, 7) & "/01")).ToString("yyyy/MM/dd")

    '                sqlstr(i) = "SELECT MeterSetup.InstallPosition AS SelectItem, MAX(" & E_RBList.SelectedValue & ") + MAX(T3.Value) AS Value FROM ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup,PowerRecord inner JOIN " & _
    '                            "(SELECT M1.InstallPosition AS SelectItem, MAX(" & E_RBList.SelectedValue & ")- MAX(T2.Value) AS Value FROM ECOSMART.dbo.ControllerSetup AS C1,ECOSMART.dbo.MeterSetup AS M1,PowerRecord AS T1 inner JOIN " & _
    '                            "(SELECT MeterSetup.InstallPosition AS SelectItem, MAX(" & E_RBList.SelectedValue & ") AS Value  FROM PowerRecord,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
    '                            "WHERE PowerRecord.CtrlNr = " & ctrlnr(i) & " AND PowerRecord.MeterID = " & meterid(i) & " AND PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & _
    '                            "substring(RecDate,1,10) = '" & t1 & "' GROUP BY MeterSetup.InstallPosition  ) AS T2 ON SelectItem = T2.SelectItem " & _
    '                            "WHERE T1.CtrlNr = " & ctrlnr(i) & " AND T1.MeterID = " & meterid(i) & " AND T1.CtrlNr = M1.CtrlNr AND T1.MeterID = M1.MeterID AND C1.Account='" & account & "' AND C1.ECO_Account = M1.ECO_Account AND " & _
    '                            "DATEADD(day, -1, convert(datetime,(convert(char(7),dateadd(month,1,RecDate),111)+'/1'))) = '" & t2 & "' GROUP BY M1.InstallPosition  ) AS T3 ON SelectItem = T3.SelectItem " & _
    '                            "WHERE PowerRecord.CtrlNr = " & ctrlnr(i) & " AND PowerRecord.MeterID = " & meterid(i) & " AND PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & _
    '                            "substring(RecDate,1,10) = '" & Date_Info_E.Text & "' GROUP BY MeterSetup.InstallPosition "
    '            End If

    '            If i <> check_count - 1 Then
    '                sql &= sqlstr(i) & " UNION "
    '            Else
    '                sql &= sqlstr(i)
    '            End If
    '        Next

    '        Try
    '            Dim da As System.Data.OleDb.OleDbDataAdapter = New System.Data.OleDb.OleDbDataAdapter(sql, strcon)
    '            Dim dt As DataTable = New DataTable
    '            da.Fill(dt)

    '            If dt.Rows.Count > 0 Then
    '                '有選擇總表
    '                If Meter_TreeView_Sum.CheckedNodes.Count > 0 Then
    '                    analysis_panel.Visible = True
    '                    '取得總表ECO5、電表編號
    '                    Dim ctrlnr_S(Meter_TreeView_Sum.CheckedNodes.Count - 1) As String
    '                    Dim meterid_S(Meter_TreeView_Sum.CheckedNodes.Count - 1) As String
    '                    Dim check_count_S As Integer = 0
    '                    For Each node As TreeNode In Meter_TreeView_Sum.CheckedNodes
    '                        If node.Checked = True Then
    '                            Dim node_value As String = node.ToolTip
    '                            Dim value() As String = node_value.Split(vbCrLf)
    '                            ctrlnr_S(check_count_S) = value(2).Split(":").GetValue(1)
    '                            meterid_S(check_count_S) = value(3).Split(":").GetValue(1)
    '                            check_count_S += 1
    '                        End If
    '                    Next

    '                    '組合總表SQL字串
    '                    Dim sqlstr_S(check_count_S - 1) As String
    '                    Dim sql_S As String = Nothing
    '                    For i = 0 To check_count_S - 1
    '                        If Date_Info_S.Text.Substring(0, 7) = Date_Info_E.Text.Substring(0, 7) Then
    '                            sqlstr_S(i) = "SELECT MeterSetup.InstallPosition AS SelectItem, " & select_item & " FROM PowerRecord,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup WHERE PowerRecord.CtrlNr = " & ctrlnr_S(i) & " AND PowerRecord.MeterID = " & meterid_S(i) & " AND " & _
    '                                          "PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & datetime & " GROUP BY MeterSetup.InstallPosition "
    '                        Else
    '                            Dim t1 As String = DateAdd("d", -1, Date_Info_S.Text).ToString("yyyy/MM/dd")
    '                            Dim t2 As String = DateAdd("d", -1, DateAdd("m", 1, Date_Info_S.Text.Substring(0, 7) & "/01")).ToString("yyyy/MM/dd")

    '                            sqlstr_S(i) = "SELECT MeterSetup.InstallPosition AS SelectItem, MAX(" & E_RBList.SelectedValue & ") + MAX(T3.Value) AS Value FROM ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup,PowerRecord inner JOIN " & _
    '                                        "(SELECT M1.InstallPosition AS SelectItem, MAX(" & E_RBList.SelectedValue & ")- MAX(T2.Value) AS Value FROM ECOSMART.dbo.ControllerSetup AS C1,ECOSMART.dbo.MeterSetup AS M1,PowerRecord AS T1 inner JOIN " & _
    '                                        "(SELECT MeterSetup.InstallPosition AS SelectItem, MAX(" & E_RBList.SelectedValue & ") AS Value  FROM PowerRecord,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup " & _
    '                                        "WHERE PowerRecord.CtrlNr = " & ctrlnr_S(i) & " AND PowerRecord.MeterID = " & meterid_S(i) & " AND PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & _
    '                                        "substring(RecDate,1,10) = '" & t1 & "' GROUP BY MeterSetup.InstallPosition  ) AS T2 ON SelectItem = T2.SelectItem " & _
    '                                        "WHERE T1.CtrlNr = " & ctrlnr_S(i) & " AND T1.MeterID = " & meterid_S(i) & " AND T1.CtrlNr = M1.CtrlNr AND T1.MeterID = M1.MeterID AND C1.Account='" & account & "' AND C1.ECO_Account = M1.ECO_Account AND " & _
    '                                        "DATEADD(day, -1, convert(datetime,(convert(char(7),dateadd(month,1,RecDate),111)+'/1'))) = '" & t2 & "' GROUP BY M1.InstallPosition  ) AS T3 ON SelectItem = T3.SelectItem " & _
    '                                        "WHERE PowerRecord.CtrlNr = " & ctrlnr_S(i) & " AND PowerRecord.MeterID = " & meterid_S(i) & " AND PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & _
    '                                        "substring(RecDate,1,10) = '" & Date_Info_E.Text & "' GROUP BY MeterSetup.InstallPosition "
    '                        End If

    '                        If i <> check_count_S - 1 Then
    '                            sql_S &= sqlstr_S(i) & " UNION "
    '                        Else
    '                            sql_S &= sqlstr_S(i)
    '                        End If
    '                    Next

    '                    Dim da_S As System.Data.OleDb.OleDbDataAdapter = New System.Data.OleDb.OleDbDataAdapter(sql_S, strcon)
    '                    Dim dt_S As DataTable = New DataTable
    '                    da_S.Fill(dt_S)

    '                    Dim sum As Integer
    '                    For i = 0 To check_count_S - 1
    '                        sum = sum + dt_S.Rows(i).Item(1)
    '                    Next

    '                    Dim total As Integer
    '                    For i = 0 To check_count - 1
    '                        total = total + dt.Rows(i).Item(1)
    '                    Next

    '                    '取得差額
    '                    Dim dif As Integer = sum - total
    '                    Dim diff As Double = total / sum
    '                    Dim dif_P As String = String.Format("{0:P}", diff)
    '                    efficiency.Text = select_txt & "使用效率：" & dif_P
    '                    diftxt.Text = "損耗率：" & String.Format("{0:P}", 1 - diff)
    '                    diftxt_E.Text = "損耗電量：" & dif & " KWH"
    '                    efficiency.Visible = True
    '                    diftxt.Visible = True
    '                    diftxt_E.Visible = True

    '                    Dim dt2 As DataTable = New DataTable
    '                    dt2.Columns.Add("SelectItem")
    '                    dt2.Columns.Add("Value")
    '                    For i = 0 To check_count - 1
    '                        dt2.Rows.Add(dt.Rows(i).Item(0), dt.Rows(i).Item(1))
    '                    Next
    '                    dt2.Rows.Add("損耗", dif)

    '                    Chart_ES.DataSource = dt2
    '                    Chart_ES.Visible = True
    '                    img_btn.Visible = True
    '                    Panel_Chart.Visible = True
    '                Else
    '                    Chart_ES.DataSource = dt
    '                    Chart_ES.Visible = True
    '                    img_btn.Visible = True
    '                    analysis_panel.Visible = False
    '                    efficiency.Visible = False
    '                    diftxt.Visible = False
    '                    diftxt_E.Visible = False
    '                    Panel_Chart.Visible = True
    '                End If
    '            Else
    '                Chart_ES.Visible = False
    '                img_btn.Visible = False
    '                analysis_panel.Visible = False
    '                efficiency.Visible = False
    '                diftxt.Visible = False
    '                diftxt_E.Visible = False
    '                Panel_Chart.Visible = False

    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '            End If

    '            'PieChart_M.DataSource = dt
    '            'PieChart_M.Series("Series1").XValueMember = "電表編號"
    '            'PieChart_M.Series("Series1").YValueMembers = values
    '            'PieChart_M.Series("Series1").LegendText = "#VALX" 'X軸
    '            'PieChart_M.Series("Series1").Label = "#PERCENT{P1}\n#VALY" '百分比+原始值
    '            'PieChart_M.Series("Series1")("PieLabelStyle") = "Outside"
    '            'PieChart_M.ChartAreas(0).Area3DStyle.Enable3D = True
    '            'PieChart_M.Series("Series1")("3DLabelLineSize") = "100"
    '            'PieChart_M.DataBind()

    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try
    '    End If
    'End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
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

            For Each node As TreeNode In Meter_TreeView_Sum.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Or Meter_TreeView.SelectedNode.Text <> "The meter has not been defined." Then
            If Me.Meter_TreeView_Sum.SelectedNode IsNot Nothing Then
                Meter_TreeView_Sum.SelectedNode.Selected = False
            End If
            Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
            Dim value() As String = node_value.Split(vbCrLf)
            Dim account As String = value(0).Split(":").GetValue(1)
            Dim eco_account As String = value(1).Split(":").GetValue(1)
            Dim ctrlnr As String = value(2).Split(":").GetValue(1)
            Dim meterid As String = value(3).Split(":").GetValue(1)
            'Dim strcon, strSQL As String
            ''If Session("Rank") = 2 Then
            'strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(Group_DropDownList.SelectedValue) & ""
            'strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Find_AdAccount(Group_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
            ''Else
            ''    strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
            ''    strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Session("Account_admin") & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
            ''End If

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
                            'drawnr.Text = dr("DrawNr").ToString
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

    Protected Sub Meter_TreeView_Sum_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView_Sum.SelectedNodeChanged
        If Meter_TreeView_Sum.SelectedNode.Text <> "尚未定義電表" Or Meter_TreeView_Sum.SelectedNode.Text <> "The meter has not been defined." Then
            If Me.Meter_TreeView.SelectedNode IsNot Nothing Then
                Meter_TreeView.SelectedNode.Selected = False
            End If
            Dim node_value As String = Meter_TreeView_Sum.SelectedNode.ToolTip
            Dim value() As String = node_value.Split(vbCrLf)
            Dim ctrlnr As String = value(2).Split(":").GetValue(1)
            Dim meterid As String = value(3).Split(":").GetValue(1)
            Dim strcon, strSQL As String
            'If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(Group_DropDownList.SelectedValue) & ""
            strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Find_AdAccount(Group_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
            'Else
            '    strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
            '    strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Session("Account_admin") & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
            'End If

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
                            'drawnr.Text = dr("DrawNr").ToString
                            InstallPosition.Text = dr("InstallPosition").ToString
                            If dr("Enabled") = True Then
                                enabled_img.ImageUrl = "img\on.png"
                            Else
                                enabled_img.ImageUrl = "img\off.png"
                            End If
                        End If
                    End Using
                End Using
            End Using

            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Dim strSQL2 As String = "select * from PowerRecord where CtrlNr = " & ctrlnr & " and MeterID = " & meterid & " and " & TimeInterval() & " "
                Using cmd2 As New OleDbCommand(strSQL2, conn)
                    Using dr2 As OleDbDataReader = cmd2.ExecuteReader
                        If dr2.Read() Then
                            upload_img.ImageUrl = "img/GreenBall.png"
                        Else
                            upload_img.ImageUrl = "img/RedBall.png"
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    'Protected Sub ViewDetails_btn_Click(sender As Object, e As ImageClickEventArgs) Handles ViewDetails_btn.Click
    '    If Session("Account") IsNot Nothing Then
    '        SqlQuery()
    '    Else
    '        Dim msg As String = "登入逾時，請重新登入"
    '        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
    '    End If
    'End Sub
End Class
