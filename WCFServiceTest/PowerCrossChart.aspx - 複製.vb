Imports System.Data
Imports System.Data.OleDb

Partial Class PowerCrossChart
    Inherits TreeViewCheck

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                Item_RBList.Items.Add(New ListItem("實功", "W"))
                Item_RBList.Items.Add(New ListItem("虛功", "V_ar"))
                Item_RBList.Items.Add(New ListItem("視在", "VA"))
                Item_RBList.Items(0).Selected = True

                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")

                '建置開始时间dropdownlist
                For i = 0 To 9
                    begin_hh.Items.Add("0" & i & "")
                Next
                For i = 10 To 23
                    begin_hh.Items.Add("" & i & "")
                Next
                begin_hh.Items(0).Selected = True '預設

                '建置結束时间dropdownlist
                For i = 0 To 9
                    end_hh.Items.Add("0" & i & "")
                Next
                For i = 10 To 23
                    end_hh.Items.Add("" & i & "")
                Next
                end_hh.Items(23).Selected = True '預設

                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Dim iSelIndex As Integer = 0
                Dim iReadIndex As Integer = 0
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    If Session("Account") = "admin" Then    '系統管理者
                        Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                        Using cmd As New OleDbCommand(strSQL, conn)
                            Using dr As OleDbDataReader = cmd.ExecuteReader
                                Group_DropDownList.Items.Add("請選擇")
                                While (dr.Read() = True)
                                    If dr("Account") <> "admin" Then
                                        Dim item As String = dr("ECO_Group").ToString
                                        Group_DropDownList.Items.Add(item)

                                        iReadIndex = iReadIndex + 1
                                        If Not (Session("Group01") Is Nothing) Then
                                            If Session("Group01") = item Then
                                                iSelIndex = iReadIndex
                                            End If
                                        End If
                                    End If
                                End While
                                If Session("Group01") Is Nothing Then
                                    Group_DropDownList.Items(0).Selected = True
                                Else
                                    Group_DropDownList.Items(iSelIndex).Selected = True
                                End If
                            End Using
                        End Using
                    Else
                        If Session("Rank") = 2 Then
                            Group_DropDownList.Enabled = False
                            Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                            Using cmd As New OleDbCommand(strSQL, conn)
                                Using dr As OleDbDataReader = cmd.ExecuteReader
                                    Group_DropDownList.Items.Add("請選擇")
                                    While (dr.Read() = True)
                                        If dr("Account") <> "admin" And dr("ECO_Group").ToString = Session("Group").ToString Then
                                            Dim item As String = dr("ECO_Group").ToString
                                            Group_DropDownList.Items.Add(item)
                                        End If
                                    End While
                                    Group_DropDownList.Items(1).Selected = True
                                End Using
                            End Using
                            'For i = 0 To Group_DropDownList.Items.Count - 1
                            '    If Group_DropDownList.Items(i).Text = Session("Account_admin") Then
                            '        Group_DropDownList.Items(i).Selected = True
                            '        Exit For
                            '    End If
                            'Next
                        Else
                            Group_DropDownList.Enabled = False
                            Group_DropDownList.Items.Add(Session("Group"))
                        End If
                        Account_DropDownList_SelectedIndexChanged(Nothing, Nothing)
                    End If
                End Using

                If Me.Meter_TreeView.SelectedNode IsNot Nothing Then
                    Meter_TreeView_SelectedNodeChanged(Nothing, Nothing)
                End If
            End If
        End If
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Meter_TreeView.Nodes.Clear()
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim sql As String = " exec [ECOSMART].[dbo].[ReadMeterTree] '" & account & "' "

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                'Dim schemaTable As DataTable = dr.GetSchemaTable()

                Dim dt As DataTable = New DataTable()
                dt.Load(dr)

                For Each row In dt.Rows
                    Dim Ret As Boolean = DisplayTreeView(Meter_TreeView, row)
                Next
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

    Protected Sub Item_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Item_DropDownList.SelectedIndexChanged
        Item_RBList.Items.Clear()

        If Item_DropDownList.SelectedValue = "電壓" Then
            Item_RBList.Items.Add(New ListItem("R相", "V1"))
            Item_RBList.Items.Add(New ListItem("S相", "V2"))
            Item_RBList.Items.Add(New ListItem("T相", "V3"))
            Item_RBList.Items.Add(New ListItem("平均電壓", "Vavg"))
            For i = 0 To 3
                Item_RBList.Items(i).Selected = True
            Next
        ElseIf Item_DropDownList.SelectedValue = "電流" Then
            Item_RBList.Items.Add(New ListItem("R相", "I1"))
            Item_RBList.Items.Add(New ListItem("S相", "I2"))
            Item_RBList.Items.Add(New ListItem("T相", "I3"))
            Item_RBList.Items.Add(New ListItem("平均電流", "Iavg"))
            For i = 0 To 3
                Item_RBList.Items(i).Selected = True
            Next
        ElseIf Item_DropDownList.SelectedValue = "功率" Then
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
            Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
            Dim value() As String = node_value.Split(vbCrLf)
            Dim account As String = value(0).Split(":").GetValue(1)
            Dim eco_account As String = value(1).Split(":").GetValue(1)
            Dim ctrlnr As String = value(2).Split(":").GetValue(1)
            Dim meterid As String = value(3).Split(":").GetValue(1)
            Dim start_time As String = Date_txt1.Text & " " & begin_hh.SelectedValue & ":00:00"
            Dim end_time As String = Date_txt2.Text & " " & end_hh.SelectedValue & ":59:59"

            Dim strcon, strSQL As String
            'If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(Group_DropDownList.SelectedValue) & ""
            strSQL = " Select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* " & _
                     " ,DateDiff(n, recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus " & _
                     " From ControllerSetup as CS,MeterSetup as MS " & _
                     " Where CS.Account = '" & Find_AdAccount(Group_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account " & _
                     " Order by CtrlNr"
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
End Class
