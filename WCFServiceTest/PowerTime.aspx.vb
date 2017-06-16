Imports System.Data.OleDb
Imports System.Data

Partial Class PowerTime
    'Inherits TreeViewCheck
    Inherits ObjectBuilding

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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

                If Session("language") = "en" Then
                    Item_RBList.Items.Add(New ListItem("Actual power output", "W"))
                    Item_RBList.Items.Add(New ListItem("Virtual power", "V_ar"))
                    Item_RBList.Items.Add(New ListItem("Apparent power", "VA"))
                Else
                    Item_RBList.Items.Add(New ListItem("實功", "W"))
                    Item_RBList.Items.Add(New ListItem("虛功", "V_ar"))
                    Item_RBList.Items.Add(New ListItem("視在", "VA"))
                End If
                Item_RBList.Items(0).Selected = True

                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, -2, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")

                '建置開始时间dropdownlist ....  建置結束时间dropdownlist
                For i = 0 To 23
                    begin_hh1.Items.Add(Right("0" & i.ToString, 2))
                    begin_hh2.Items.Add(Right("0" & i.ToString, 2))
                    end_hh1.Items.Add(Right("0" & i.ToString, 2))
                    end_hh2.Items.Add(Right("0" & i.ToString, 2))
                Next
                '預設
                begin_hh1.Items(0).Selected = True
                begin_hh2.Items(0).Selected = True
                end_hh1.Items(23).Selected = True
                end_hh2.Items(23).Selected = True
            End If
        End If
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)

    End Sub

    Protected Sub Item_DDList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Item_DDList.SelectedIndexChanged
        Item_RBList.Items.Clear()
        If Item_DDList.SelectedValue = "電壓" Or Item_DDList.SelectedValue = "Voltage" Then

            If Session("language") = "en" Then
                Item_RBList.Items.Add(New ListItem("R Phase", "V1"))
                Item_RBList.Items.Add(New ListItem("S Phase", "V2"))
                Item_RBList.Items.Add(New ListItem("T Phase", "V3"))
                Item_RBList.Items.Add(New ListItem("Average voltage", "Vavg"))
            Else
                Item_RBList.Items.Add(New ListItem("R相", "V1"))
                Item_RBList.Items.Add(New ListItem("S相", "V2"))
                Item_RBList.Items.Add(New ListItem("T相", "V3"))
                Item_RBList.Items.Add(New ListItem("平均電壓", "Vavg"))
            End If

            For i = 0 To 3
                Item_RBList.Items(i).Selected = True
            Next
        ElseIf Item_DDList.SelectedValue = "電流" Or Item_DDList.SelectedValue = "Current" Then
            If Session("language") = "en" Then
                Item_RBList.Items.Add(New ListItem("R Phase", "I1"))
                Item_RBList.Items.Add(New ListItem("S Phase", "I2"))
                Item_RBList.Items.Add(New ListItem("T Phase", "I3"))
                Item_RBList.Items.Add(New ListItem("Average current", "Iavg"))
            Else
                Item_RBList.Items.Add(New ListItem("R相", "I1"))
                Item_RBList.Items.Add(New ListItem("S相", "I2"))
                Item_RBList.Items.Add(New ListItem("T相", "I3"))
                Item_RBList.Items.Add(New ListItem("平均電流", "Iavg"))
            End If
            For i = 0 To 3
                Item_RBList.Items(i).Selected = True
            Next
        ElseIf Item_DDList.SelectedValue = "功率" Or Item_DDList.SelectedValue = "Power" Then
            If Session("language") = "en" Then
                Item_RBList.Items.Add(New ListItem("Actual power output", "W"))
                Item_RBList.Items.Add(New ListItem("Virtual power", "V_ar"))
                Item_RBList.Items.Add(New ListItem("Apparent power", "VA"))
            Else
                Item_RBList.Items.Add(New ListItem("實功", "W"))
                Item_RBList.Items.Add(New ListItem("虛功", "V_ar"))
                Item_RBList.Items.Add(New ListItem("視在", "VA"))
            End If
            For i = 0 To 2
                Item_RBList.Items(i).Selected = True
            Next
        End If
    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Or Meter_TreeView.SelectedNode.Text <> "The meter has not been defined." Then
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
    '        Session("SelectedNode") = Meter_TreeView.SelectedNode.Text
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
    '        Dim start_time1 As String = Date_txt1.Text & " " & begin_hh1.SelectedValue & ":00:00"
    '        Dim end_time1 As String = Date_txt1.Text & " " & end_hh1.SelectedValue & ":59:59"
    '        Dim start_time2 As String = Date_txt2.Text & " " & begin_hh2.SelectedValue & ":00:00"
    '        Dim end_time2 As String = Date_txt2.Text & " " & end_hh2.SelectedValue & ":59:59"

    '        Dim sql1 As String = "select SUBSTRING(RecDate,12,5) as RecDate1," & Item_RBList.SelectedValue & " as Value1 from PowerRecord "
    '        時間間隔()
    '        If interval_DDList.SelectedValue = "1" Then
    '            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "5" Then
    '            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "30" Then
    '            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "60" Then
    '            sql1 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time1 & "' and '" & end_time1 & "' and substring(recdate,15,2) Like '%00%' order by RecDate "
    '        End If

    '        Dim sql2 As String = "select SUBSTRING(RecDate,12,5) as RecDate2," & Item_RBList.SelectedValue & " as Value12 from PowerRecord "
    '        時間間隔()
    '        If interval_DDList.SelectedValue = "1" Then
    '            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "5" Then
    '            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "30" Then
    '            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "60" Then
    '            sql2 &= " WHERE CtrlNr = " & ctrlnr & " AND MeterID = " & meterid & " AND RecDate between '" & start_time2 & "' and '" & end_time2 & "' and substring(recdate,15,2) Like '%00%' order by RecDate "
    '        End If

    '        Dim da1 As OleDbDataAdapter = New OleDbDataAdapter(sql1, strcon)
    '        Dim dt1 As DataTable = New DataTable
    '        da1.Fill(dt1)
    '        Dim key1 As DataColumn() = New DataColumn(0) {}
    '        key1(0) = dt1.Columns("時段")
    '        dt1.PrimaryKey = key1

    '        Dim da2 As OleDbDataAdapter = New OleDbDataAdapter(sql2, strcon)
    '        Dim dt2 As DataTable = New DataTable
    '        da2.Fill(dt2)
    '        Dim key2 As DataColumn() = New DataColumn(0) {}
    '        key2(0) = dt2.Columns("時段")
    '        dt2.PrimaryKey = key2

    '        If dt1.Rows.Count > 0 And dt2.Rows.Count > 0 Then
    '            Dim dt As DataTable = New DataTable
    '            dt.Columns.Add("RecDate1", GetType(Date))
    '            dt.Columns.Add("Value11", GetType(Double))
    '            dt.Columns.Add("RecDate2", GetType(Date))
    '            dt.Columns.Add("Value12", GetType(Double))

    '            If dt1.Rows.Count < dt2.Rows.Count Then
    '                For i = 0 To dt1.Rows.Count - 1
    '                    Dim recdate1 As Date = dt1.Rows(i).Item(0)
    '                    Dim w1 As Double = dt1.Rows(i).Item(1)
    '                    For j = 0 To dt2.Rows.Count - 1
    '                        Dim recdate2 As Date = dt2.Rows(j).Item(0)
    '                        Dim w2 As Double = dt2.Rows(j).Item(1)
    '                        If recdate1 = recdate2 Then
    '                            dt.Rows.Add(recdate1, w1, recdate2, w2)
    '                            Exit For
    '                        End If
    '                    Next
    '                Next
    '            Else
    '                For i = 0 To dt2.Rows.Count - 1
    '                    Dim recdate2 As Date = dt2.Rows(i).Item(0)
    '                    Dim w2 As Double = dt2.Rows(i).Item(1)
    '                    For j = 0 To dt1.Rows.Count - 1
    '                        Dim recdate1 As Date = dt1.Rows(j).Item(0)
    '                        Dim w1 As Double = dt1.Rows(j).Item(1)
    '                        If recdate1 = recdate2 Then
    '                            dt.Rows.Add(recdate1, w1, recdate2, w2)
    '                        End If
    '                    Next

    '                Next
    '            End If

    '            If Item_DDList.SelectedValue = "功率" Then
    '                Chart_W.Series(0).Title = Date_txt1.Text.Substring(5, 5)
    '                Chart_W.Series(1).Title = Date_txt2.Text.Substring(5, 5)
    '                Chart_W.Visible = True
    '                Chart_V.Visible = False
    '                Chart_I.Visible = False
    '                Chart_W.DataSource = dt
    '                Chart_W.DataBind()
    '            ElseIf Item_DDList.SelectedValue = "電壓" Then
    '                Chart_V.Series(0).Title = Date_txt1.Text.Substring(5, 5)
    '                Chart_V.Series(1).Title = Date_txt2.Text.Substring(5, 5)
    '                Chart_V.Visible = True
    '                Chart_W.Visible = False
    '                Chart_I.Visible = False
    '                Chart_V.DataSource = dt
    '                Chart_V.DataBind()
    '            ElseIf Item_DDList.SelectedValue = "電流" Then
    '                Chart_I.Series(0).Title = Date_txt1.Text.Substring(5, 5)
    '                Chart_I.Series(1).Title = Date_txt2.Text.Substring(5, 5)
    '                Chart_I.Visible = True
    '                Chart_W.Visible = False
    '                Chart_V.Visible = False
    '                Chart_I.DataSource = dt
    '                Chart_I.DataBind()
    '            End If
    '            img_btn.Visible = True
    '            Panel_Chart.Visible = True
    '        Else
    '            Dim msg As String = "查無資料"
    '            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '            Chart_W.Visible = False
    '            Chart_V.Visible = False
    '            Chart_I.Visible = False
    '            img_btn.Visible = False
    '            Panel_Chart.Visible = False
    '        End If
    '    End If
    'End Sub
End Class
