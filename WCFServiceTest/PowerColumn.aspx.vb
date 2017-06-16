Imports System.Data.OleDb
Imports System.Data

Partial Class ColumnChart
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
                '初始化日期區間
                Date_Info_S.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM") & "/01"
                Date_Info_E.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")

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
            End If
        End If
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)

    End Sub

    'Protected Sub SqlQuery()
    '    If Meter_TreeView.CheckedNodes.Count < 1 Then
    '        img_btn.Visible = False
    '        Dim msg As String = "請勾選電表"
    '        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '        Chart1.Visible = False
    '    Else
    '        Dim strcon, account As String
    '        If Session("Rank") = 2 Then
    '            account = Account_admin(Group_DropDownList.SelectedValue)
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(Group_DropDownList.SelectedValue) & ""
    '        Else
    '            account = Session("Account_admin")
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
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

    '        Dim datetime As String = "substring(RecDate,1,10) between '" & Date_Info_S.Text & "' and '" & Date_Info_E.Text & "' "
    '        '組合SQL字串
    '        Dim sqlstr(check_count - 1) As String
    '        Dim sql As String = Nothing
    '        For i = 0 To check_count - 1
    '            sqlstr(i) = "SELECT MeterSetup.InstallPosition AS 位置, Round(AVG(" & Item_RBList.SelectedValue & "),2) AS 平均值,Round(MAX(" & Item_RBList.SelectedValue & "),2) AS 最大值 " & _
    '                        "FROM PowerRecord,ECOSMART.dbo.ControllerSetup,ECOSMART.dbo.MeterSetup WHERE PowerRecord.CtrlNr = " & ctrlnr(i) & " AND PowerRecord.MeterID = " & meterid(i) & "" & _
    '                        " AND PowerRecord.CtrlNr = MeterSetup.CtrlNr AND PowerRecord.MeterID = MeterSetup.MeterID AND ControllerSetup.Account='" & account & "' AND ControllerSetup.ECO_Account = MeterSetup.ECO_Account AND " & datetime & " GROUP BY MeterSetup.InstallPosition "
    '            If i <> check_count - 1 Then
    '                sql &= sqlstr(i) & " UNION "
    '            Else
    '                sql &= sqlstr(i)
    '            End If
    '        Next

    '        Try
    '            Dim da As OleDbDataAdapter = New OleDbDataAdapter(sql, strcon)
    '            Dim dt As DataTable = New DataTable
    '            da.Fill(dt)

    '            If dt.Rows.Count > 0 Then
    '                img_btn.Visible = True
    '                Chart1.Visible = True
    '                Chart1.DataSource = dt
    '            Else
    '                img_btn.Visible = False
    '                Chart1.Visible = False
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '            End If
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
        End If
    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Or Meter_TreeView.SelectedNode.Text <> "The meter has not been defined." Then
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
                        End If

                        If dr("UpLoadStatus") < 6 Then    '//相差超過5分鐘 表斷線
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
    '        Panel_Chart.Visible = True
    '    Else
    '        Dim msg As String = "登入逾時，請重新登入"
    '        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
    '    End If
    'End Sub
End Class
