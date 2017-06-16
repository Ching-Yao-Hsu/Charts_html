Imports System.Data.OleDb
Imports System.Data

Partial Class PowerLine
    Inherits TreeSetup

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
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    If Session("Account") = "admin" Then    '系統管理者
                        Dim strSQL As String = "select Account from AdminSetup where Enabled = 1"
                        Using cmd As New OleDbCommand(strSQL, conn)
                            Using dr As OleDbDataReader = cmd.ExecuteReader
                                Account_DropDownList.Items.Add("請選擇")
                                While (dr.Read() = True)
                                    If dr("Account") <> "admin" Then
                                        Dim item As String = dr("Account").ToString
                                        Account_DropDownList.Items.Add(item)
                                    End If
                                End While
                                Account_DropDownList.Items(0).Selected = True
                            End Using
                        End Using
                    Else
                        If Session("Rank") = 2 Then
                            Dim strSQL As String = "select Account from AdminSetup where Enabled = 1"
                            Using cmd As New OleDbCommand(strSQL, conn)
                                Using dr As OleDbDataReader = cmd.ExecuteReader
                                    Account_DropDownList.Items.Add("請選擇")
                                    While (dr.Read() = True)
                                        If dr("Account") <> "admin" Then
                                            Dim item As String = dr("Account")
                                            Account_DropDownList.Items.Add(item)
                                        End If
                                    End While
                                End Using
                            End Using
                            For i = 0 To Account_DropDownList.Items.Count - 1
                                If Account_DropDownList.Items(i).Text = Session("Account") Then
                                    Account_DropDownList.Items(i).Selected = True
                                    Exit For
                                End If
                            Next
                        Else
                            Account_DropDownList.Enabled = False
                            Account_DropDownList.Items.Add(Session("Account"))
                        End If
                        Account_DropDownList_SelectedIndexChanged(Nothing, Nothing)
                    End If
                End Using

                If Me.Meter_TreeView.SelectedNode IsNot Nothing Then
                    Meter_TreeView_SelectedNodeChanged(Nothing, Nothing)
                End If

                Item_RBList.Items.Add(New ListItem("實功", "W"))
                Item_RBList.Items.Add(New ListItem("虛功", "V_ar"))
                Item_RBList.Items.Add(New ListItem("視在", "VA"))
                Item_RBList.Items(0).Selected = True

                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, -2, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")

                '建置開始时间dropdownlist
                For i = 0 To 9
                    begin_hh1.Items.Add("0" & i & "")
                    begin_hh2.Items.Add("0" & i & "")
                Next
                For i = 10 To 23
                    begin_hh1.Items.Add("" & i & "")
                    begin_hh2.Items.Add("" & i & "")
                Next
                '預設
                begin_hh1.Items(0).Selected = True
                begin_hh2.Items(0).Selected = True

                '建置結束时间dropdownlist
                For i = 0 To 9
                    end_hh1.Items.Add("0" & i & "")
                    end_hh2.Items.Add("0" & i & "")
                Next
                For i = 10 To 23
                    end_hh1.Items.Add("" & i & "")
                    end_hh2.Items.Add("" & i & "")
                Next
                '預設
                end_hh1.Items(23).Selected = True
                end_hh2.Items(23).Selected = True
            End If
        End If
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Account_DropDownList.SelectedIndexChanged
        Meter_TreeView.Nodes.Clear()
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim sql As String = ""
        If Session("Rank") = 2 Then
            sql = "select CS.Account as Account,MS.* from ControllerSetup as CS,MeterSetup as MS where (MS.LineNum is not NULL and MS.LineNum <> '') and CS.Account = '" & Account_admin(Account_DropDownList.SelectedValue) & "' and CS.ECO_Account = MS.ECO_Account order by LineNum"
        Else
            sql = "select CS.Account as Account,MS.* from ControllerSetup as CS,MeterSetup as MS where (MS.LineNum is not NULL and MS.LineNum <> '') and CS.Account = '" & Session("Account_admin") & "' and CS.ECO_Account = MS.ECO_Account order by LineNum"
        End If

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                While (dr.Read() = True)
                    Dim DataTemp As String = dr("CtrlNr").ToString & "," & dr("MeterID").ToString & "," & dr("InstallPosition").ToString & "," & dr("LineNum").ToString
                    Dim tooltip As String = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString & vbCrLf & "ECO5編號:" & dr("CtrlNr").ToString & vbCrLf & "電表編號:" & dr("MeterID").ToString
                    Dim Ret As Boolean = DisplayTreeView(Meter_TreeView, dr("MeterID").ToString, dr("LineNum").ToString, DataTemp, tooltip, dr("MeterType").ToString)
                End While
            End Using
        End Using

        Meter_TreeView.ExpandAll()

        If Meter_TreeView.Nodes.Count = 0 Then
            Dim tmpNote As New TreeNode
            tmpNote.Text = "尚未定義電表"
            Meter_TreeView.Nodes.Add(tmpNote)
        Else
            Dim cmdIndex(3) As Boolean
            cmdIndex(0) = CtrlNr_CB.Checked
            cmdIndex(1) = MeterId_CB.Checked
            cmdIndex(2) = True
            cmdIndex(3) = LineNum_CB.Checked
            Position_CB.Checked = True
            For Each node As TreeNode In Meter_TreeView.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
    End Sub

    Protected Sub Item_DDList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Item_DDList.SelectedIndexChanged
        Item_RBList.Items.Clear()
        If Item_DDList.SelectedValue = "電壓" Then
            Item_RBList.Items.Add(New ListItem("R相", "V1"))
            Item_RBList.Items.Add(New ListItem("S相", "V2"))
            Item_RBList.Items.Add(New ListItem("T相", "V3"))
            Item_RBList.Items.Add(New ListItem("平均電壓", "Vavg"))
            For i = 0 To 3
                Item_RBList.Items(i).Selected = True
            Next
        ElseIf Item_DDList.SelectedValue = "電流" Then
            Item_RBList.Items.Add(New ListItem("R相", "I1"))
            Item_RBList.Items.Add(New ListItem("S相", "I2"))
            Item_RBList.Items.Add(New ListItem("T相", "I3"))
            Item_RBList.Items.Add(New ListItem("平均電流", "Iavg"))
            For i = 0 To 3
                Item_RBList.Items(i).Selected = True
            Next
        ElseIf Item_DDList.SelectedValue = "功率" Then
            Item_RBList.Items.Add(New ListItem("實功", "W"))
            Item_RBList.Items.Add(New ListItem("虛功", "V_ar"))
            Item_RBList.Items.Add(New ListItem("視在", "VA"))
            For i = 0 To 2
                Item_RBList.Items(i).Selected = True
            Next
        End If
    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Then
            'Session("SelectedNode") = Meter_TreeView.SelectedNode.Text
            Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
            Dim value() As String = node_value.Split(vbCrLf)
            Dim ctrlnr As String = value(2).Split(":").GetValue(1)
            Dim meterid As String = value(3).Split(":").GetValue(1)
            Dim strcon, strSQL As String
            'If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(Account_DropDownList.SelectedValue) & ""
            strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Find_AdAccount(Account_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
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
                                eco5_enabled.Text = "-ECO5已啟用-"
                                eco5_enabled.ForeColor = Drawing.Color.DarkGreen
                            Else
                                eco5_enabled.Text = "-ECO5未啟用-"
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
                                        MeterType.Text = "其它"
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
    '            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(Account_DropDownList.SelectedValue) & ""
    '            strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Account_admin(Account_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
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
