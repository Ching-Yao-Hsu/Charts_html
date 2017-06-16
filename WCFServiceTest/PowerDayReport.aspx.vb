Imports System.Data.OleDb
Imports System.Data

Partial Class PowerDayReport
    'Inherits TreeViewCheck
    Inherits ObjectBuilding

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                '初始化日期區間
                Date_txt.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")

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

    Protected Sub Account_DropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)

    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
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
