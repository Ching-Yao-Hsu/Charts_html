Imports System.Data.OleDb
Imports System.Data

Partial Class TreeSetting
    'Inherits TreeSetup
    Inherits ObjectBuilding

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then

                'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                'Using conn As New OleDbConnection(strcon)
                '    If conn.State = 0 Then conn.Open()
                '    If Session("Account") = "admin" Then    '管理者
                '        Dim sql = "select * from AdminSetup where Enabled = 1 AND CreateDB = 1 and Rank <> 0"
                '        Using cmd As New OleDbCommand(sql, conn)
                '            Using dr As OleDbDataReader = cmd.ExecuteReader
                '                Group_DropDownList.Items.Add(New ListItem("請選擇", 0))
                '                While (dr.Read() = True)
                '                    If dr("Account") <> "admin" Then
                '                        Dim item As String = dr("ECO_Group").ToString
                '                        Group_DropDownList.Items.Add(item)
                '                    End If
                '                End While
                '                Group_DropDownList.Items(0).Selected = True
                '            End Using
                '        End Using
                '    Else
                '        If Session("Rank") = 2 Then
                '            Group_DropDownList.Enabled = False
                '            Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                '            Using cmd As New OleDbCommand(strSQL, conn)
                '                Using dr As OleDbDataReader = cmd.ExecuteReader
                '                    Group_DropDownList.Items.Add(New ListItem("請選擇", 0))
                '                    While (dr.Read() = True)
                '                        If dr("Account") <> "admin" And dr("ECO_Group").ToString = Session("Group").ToString Then
                '                            Dim item As String = dr("ECO_Group").ToString
                '                            Group_DropDownList.Items.Add(item)
                '                        End If
                '                    End While
                '                    Group_DropDownList.Items(1).Selected = True
                '                End Using
                '            End Using
                '            'For i = 0 To Group_DropDownList.Items.Count - 1
                '            '    If Group_DropDownList.Items(i).Text = Session("Account_admin") Then
                '            '        Group_DropDownList.Items(i).Selected = True
                '            '        Exit For
                '            '    End If
                '            'Next
                '        Else
                '            Group_DropDownList.Enabled = False
                '            Group_DropDownList.Items.Add(Session("Group"))
                '        End If


                '        '先判斷此帳號始啟用，否則不顯示任何訊息
                '        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
                '        Dim sql = "select CS.Account,MS.CtrlNr,MS.MeterID,MS.ECO_Account,MS.InstallPosition from ControllerSetup as CS,MeterSetup as MS " & _
                '        "where (MS.LineNum is NULL or MS.LineNum = '') and CS.Account = '" & account & "' and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
                '        Using cmd As New OleDbCommand(sql, conn)
                '            Using dr As OleDbDataReader = cmd.ExecuteReader
                '                While (dr.Read() = True)
                '                    Dim NodeName As String = "ECO5編號:" & dr("CtrlNr").ToString & " " & "電表編號:" & dr("MeterID").ToString & " " & "安裝位置:" & dr("InstallPosition").ToString
                '                    Dim tooltip As String = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString
                '                    Dim NewNode As New TreeNode
                '                    NewNode.Text = NodeName
                '                    NewNode.ToolTip = tooltip
                '                    Meter_TreeView.Nodes.Add(NewNode)
                '                End While
                '            End Using
                '        End Using
                '        draw_btn_Click(Nothing, Nothing)
                '    End If
                'End Using



                '//群組下拉是選單
                Dim iSelIndex As Integer = 0
                BuildingDropDownList(Group_DropDownList, iSelIndex)
                If Session("Account") = "admin" Then    '系統管理者
                    If Not (Session("Group01") Is Nothing) Then
                        BuildingTree(Meter_TreeView, Session("Group01"), True, True, False, True, 0)
                        BuildingTree(NewMeter_TreeView, Session("Group01"), CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
                    End If
                Else
                    Account_DDList_SelectedIndexChanged(Nothing, Nothing)

                    '先判斷此帳號始啟用，否則不顯示任何訊息
                    Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                    Using conn As New OleDbConnection(strcon)
                        If conn.State = 0 Then conn.Open()
                        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
                        Dim sql = "select CS.Account,MS.CtrlNr,MS.MeterID,MS.ECO_Account,MS.InstallPosition from ControllerSetup as CS,MeterSetup as MS " & _
                        "where (MS.LineNum is NULL or MS.LineNum = '') and CS.Account = '" & account & "' and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
                        Using cmd As New OleDbCommand(sql, conn)
                            Using dr As OleDbDataReader = cmd.ExecuteReader
                                While (dr.Read() = True)
                                    Dim NodeName As String = ""
                                    Dim tooltip As String = ""

                                    If Session("language") = "en" Then
                                        NodeName = "ECO5_Number:" & dr("CtrlNr").ToString & " " & "Meter_Number:" & dr("MeterID").ToString & " " & "Installation_location:" & dr("InstallPosition").ToString
                                        tooltip = "Account:" & dr("Account").ToString & vbCrLf & "ECO5_Account:" & dr("ECO_Account").ToString
                                    Else
                                        NodeName = "ECO5編號:" & dr("CtrlNr").ToString & " " & "電表編號:" & dr("MeterID").ToString & " " & "安裝位置:" & dr("InstallPosition").ToString
                                        tooltip = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString
                                    End If

                                    Dim NewNode As New TreeNode
                                    NewNode.Text = NodeName
                                    NewNode.ToolTip = tooltip
                                    Meter_TreeView.Nodes.Add(NewNode)
                                End While
                            End Using
                        End Using
                        draw_btn_Click(Nothing, Nothing)
                    End Using
                End If



            End If
        End If
    End Sub

    Protected Sub Account_DDList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        If Meter_TreeView.Nodes.Count <> 0 Or NewMeter_TreeView.Nodes.Count <> 0 Then
            Meter_TreeView.Nodes.Clear()
            eco5_position.Text = ""
            eco5_enabled.Text = ""
            meter_enabled.Checked = False
            eco5_id.Text = ""
            eco5_account.Text = ""
            meter_id.Text = ""
            MeterType_DDList.SelectedValue = 7
            drawnr.Text = ""
            InstallPosition.Text = ""
            P_LineNum.Text = ""
        End If

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView, Session("Group01"), True, True, False, True, 0)
        BuildingTree(NewMeter_TreeView, Session("Group01"), CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)

        'Dim sql As String = "select CS.Account,MS.CtrlNr,MS.MeterID,MS.ECO_Account,MS.InstallPosition from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & account & "' and CS.ECO_Account = MS.ECO_Account and (LineNum = '' or LineNum is NULL) order by CtrlNr"
        ''If Session("Rank") = 2 Then
        ''    sql = "select CS.Account,MS.CtrlNr,MS.MeterID,MS.ECO_Account,MS.InstallPosition from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Account_admin(Group_DropDownList.SelectedValue) & "' and CS.ECO_Account = MS.ECO_Account and (LineNum = '' or LineNum is NULL) order by CtrlNr"
        ''Else
        ''    sql = "select CS.Account,MS.CtrlNr,MS.MeterID,MS.ECO_Account,MS.InstallPosition from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Session("Account_admin") & "' and CS.ECO_Account = MS.ECO_Account and (LineNum = '' or LineNum is NULL) order by CtrlNr"
        ''End If
        'Using conn As New OleDbConnection(strcon)
        '    If conn.State = 0 Then conn.Open()
        '    Using cmd As New OleDbCommand(sql, conn)
        '        Using dr As OleDbDataReader = cmd.ExecuteReader
        '            While (dr.Read() = True)
        '                Dim NodeName As String = "ECO5編號:" & dr("CtrlNr").ToString & " " & "電表編號:" & dr("MeterID").ToString & " " & "安裝位置:" & dr("InstallPosition").ToString
        '                Dim tooltip As String = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString
        '                Dim NewNode As New TreeNode
        '                NewNode.Value = NodeName
        '                NewNode.ToolTip = tooltip
        '                Meter_TreeView.Nodes.Add(NewNode)
        '            End While
        '        End Using
        '    End Using
        'End Using
        'draw_btn_Click(Nothing, Nothing)
    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        Show_TreeDate(Meter_TreeView.SelectedNode, 0)
    End Sub

    Protected Sub NewMeter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles NewMeter_TreeView.SelectedNodeChanged
        Show_TreeDate(NewMeter_TreeView.SelectedNode, 1)
    End Sub

    Private Sub Show_TreeDate(ByVal tTreeNode As TreeNode, ByVal iTreeKind As Integer)
        Dim node_value As String = tTreeNode.ToolTip
        If node_value <> "尚未定義電表" Then
            Dim value() As String = node_value.Split(vbCrLf)
            Dim account As String = value(0).Split(":").GetValue(1)
            Dim eco_account As String = value(1).Split(":").GetValue(1)
            Dim ctrlnr As String = value(2).Split(":").GetValue(1)
            Dim meterid As String = value(3).Split(":").GetValue(1)

            Dim strSQL As String = " exec [ECOSMART].[dbo].[ReadMeterTree] " & iTreeKind & ",'" & account & "', " & ctrlnr & "," & meterid

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

                            meter_enabled.Checked = dr("Enabled")

                            eco5_account.Text = dr("ECO_Account").ToString
                            meter_id.Text = dr("MeterID").ToString
                            If dr("MeterType").ToString <> "" Then
                                MeterType_DDList.SelectedValue = dr("MeterType").ToString
                            End If
                            drawnr.Text = dr("DrawNr").ToString
                            InstallPosition.Text = dr("InstallPosition").ToString
                            If iTreeKind = 0 Then
                                P_LineNum.Text = ""
                            Else
                                P_LineNum.Text = dr("LineNum").ToString
                            End If
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    Protected Sub edit_btn_Click(sender As Object, e As EventArgs) Handles edit_btn.Click
        edit_btn.Enabled = False
        InstallPosition.Enabled = True
        drawnr.Enabled = True
        P_LineNum.Enabled = True
        meter_enabled.Enabled = True

        MeterType_DDList.Enabled = True
        submit_btn.Enabled = True
        cancel_btn.Enabled = True
    End Sub

    Protected Sub cancel_btn_Click(sender As Object, e As EventArgs) Handles cancel_btn.Click
        edit_btn.Enabled = True
        InstallPosition.Enabled = False
        drawnr.Enabled = False
        P_LineNum.Enabled = False
        meter_enabled.Enabled = False

        MeterType_DDList.Enabled = False
        submit_btn.Enabled = False
        cancel_btn.Enabled = False
    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sqlstr As String = "select LineNum from MeterSetup where ECO_Account = ? and CtrlNr = ? and LineNum = ?"
            Using cmd2 As New OleDbCommand(sqlstr, conn)
                cmd2.Parameters.AddWithValue("?ECO_Account", eco5_account.Text)
                cmd2.Parameters.AddWithValue("?CtrlNr", eco5_id.Text)
                cmd2.Parameters.AddWithValue("?LineNum", P_LineNum.Text)
                Using dr As OleDbDataReader = cmd2.ExecuteReader
                    If dr.Read() And P_LineNum.Text <> "" Then
                        Dim sqlstr2 As String = "select LineNum from MeterSetup where ECO_Account = ? and CtrlNr = ? and MeterID = ? and LineNum = ?"
                        If dr("LineNum").ToString = P_LineNum.Text Then
                            Dim sql As String = "update MeterSetup SET MeterType = ?,DrawNr = ?,InstallPosition = ?,Enabled = ? WHERE ECO_Account = ? and CtrlNr = ? and MeterID = ?"
                            Using cmd As New OleDbCommand(sql, conn)
                                cmd.Parameters.AddWithValue("?MeterType", MeterType_DDList.SelectedValue)
                                cmd.Parameters.AddWithValue("?DrawNr", drawnr.Text)
                                cmd.Parameters.AddWithValue("?InstallPosition", InstallPosition.Text)
                                If meter_enabled.Checked = True Then
                                    cmd.Parameters.AddWithValue("?Enabled", True)
                                Else
                                    cmd.Parameters.AddWithValue("?Enabled", False)
                                End If
                                cmd.Parameters.AddWithValue("?ECO_Account", eco5_account.Text)
                                cmd.Parameters.AddWithValue("?CtrlNr", eco5_id.Text)
                                cmd.Parameters.AddWithValue("?MeterID", meter_id.Text)
                                cmd.ExecuteNonQuery()
                            End Using
                        Else
                            'msg.Text = "單線圖編號重複"
                            'Dim msg As String = "單線圖編號重複，請重新輸入！"
                            'Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            'Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                        End If
                    Else
                        '新編號
                        Dim sql As String = "update MeterSetup SET LineNum = ?,MeterType = ?,DrawNr = ?,InstallPosition = ?,Enabled = ? WHERE ECO_Account = ? and CtrlNr = ? and MeterID = ?"
                        Using cmd As New OleDbCommand(sql, conn)
                            cmd.Parameters.AddWithValue("?LineNum", P_LineNum.Text)
                            cmd.Parameters.AddWithValue("?MeterType", MeterType_DDList.SelectedValue)
                            cmd.Parameters.AddWithValue("?DrawNr", drawnr.Text)
                            cmd.Parameters.AddWithValue("?InstallPosition", InstallPosition.Text)
                            If meter_enabled.Checked = True Then
                                cmd.Parameters.AddWithValue("?Enabled", True)
                            Else
                                cmd.Parameters.AddWithValue("?Enabled", False)
                            End If
                            cmd.Parameters.AddWithValue("?ECO_Account", eco5_account.Text)
                            cmd.Parameters.AddWithValue("?CtrlNr", eco5_id.Text)
                            cmd.Parameters.AddWithValue("?MeterID", meter_id.Text)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                    edit_btn.Enabled = True
                    InstallPosition.Enabled = False
                    drawnr.Enabled = False
                    P_LineNum.Enabled = False
                    meter_enabled.Enabled = False
                    MeterType_DDList.Enabled = False
                    submit_btn.Enabled = False
                    cancel_btn.Enabled = False

                    Account_DDList_SelectedIndexChanged(Nothing, Nothing)
                    draw_btn_Click(Nothing, Nothing)
                End Using
            End Using
        End Using
    End Sub

    Protected Sub draw_btn_Click(sender As Object, e As EventArgs) Handles draw_btn.Click
        Account_DDList_SelectedIndexChanged(Nothing, Nothing)

        'NewMeter_TreeView.Nodes.Clear()
        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        'Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        'Dim sql As String = "select CS.Account as Account,MS.* from ControllerSetup as CS,MeterSetup as MS where (MS.LineNum is not NULL and MS.LineNum <> '') and CS.Account = '" & account & "' and CS.ECO_Account = MS.ECO_Account order by LineNum"
        ''If Session("Rank") = 2 Then
        ''    sql = "select CS.Account as Account,MS.* from ControllerSetup as CS,MeterSetup as MS where (MS.LineNum is not NULL and MS.LineNum <> '') and CS.Account = '" & Account_admin(Group_DropDownList.SelectedValue) & "' and CS.ECO_Account = MS.ECO_Account order by LineNum"
        ''Else
        ''    sql = "select CS.Account as Account,MS.* from ControllerSetup as CS,MeterSetup as MS where (MS.LineNum is not NULL and MS.LineNum <> '') and CS.Account = '" & Session("Account_admin") & "' and CS.ECO_Account = MS.ECO_Account order by LineNum"
        ''End If

        'Using conn As New OleDbConnection(strcon)
        '    If conn.State = 0 Then conn.Open()
        '    Using cmd As New OleDbCommand(sql, conn)
        '        Using dr As OleDbDataReader = cmd.ExecuteReader
        '            Try
        '                While (dr.Read() = True)
        '                    Dim DataTemp As String = dr("CtrlNr").ToString & "," & dr("MeterID").ToString & "," & dr("InstallPosition").ToString & "," & dr("LineNum").ToString
        '                    Dim tooltip As String = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString & vbCrLf & "ECO5編號:" & dr("CtrlNr").ToString & vbCrLf & "電表編號:" & dr("MeterID").ToString & vbCrLf & "安裝位置:" & dr("InstallPosition").ToString
        '                    Dim Ret As Boolean = DisplayTreeView(NewMeter_TreeView, dr("MeterID").ToString, dr("LineNum").ToString, DataTemp, tooltip, dr("MeterType").ToString)
        '                End While
        '                Dim cmdIndex(3) As Boolean
        '                cmdIndex(0) = CtrlNr_CB.Checked
        '                cmdIndex(1) = MeterId_CB.Checked
        '                cmdIndex(2) = True
        '                cmdIndex(3) = True
        '                Position_CB.Checked = True
        '                LineNum_CB.Checked = True
        '                For Each node As TreeNode In NewMeter_TreeView.Nodes
        '                    Call ModifyNode(node, 1, cmdIndex, )
        '                Next
        '                NewMeter_TreeView.ExpandAll()
        '            Catch ex As Exception
        '                NewMeter_TreeView.ExpandAll()
        '                'msg.Text = ex.ToString
        '            End Try
        '        End Using
        '    End Using
        'End Using
    End Sub

    Protected Sub show_btn_Click(sender As Object, e As EventArgs) Handles show_btn.Click
        If CtrlNr_CB.Checked = False And MeterId_CB.Checked = False And Position_CB.Checked = False And LineNum_CB.Checked = False Then
            '//draw_btn_Click(Nothing, Nothing)
            Account_DDList_SelectedIndexChanged(Nothing, Nothing)
        Else
            Dim cmdIndex(3) As Boolean
            cmdIndex(0) = CtrlNr_CB.Checked
            cmdIndex(1) = MeterId_CB.Checked
            cmdIndex(2) = Position_CB.Checked
            cmdIndex(3) = LineNum_CB.Checked

            For Each node As TreeNode In NewMeter_TreeView.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
    End Sub
End Class
