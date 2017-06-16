Imports System.Data.OleDb
Imports System.Data

Partial Class PowerChart
    'Inherits TreeViewCheck
    Inherits ObjectBuilding

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                If Session("language") = "en" Then
                    Item_CBList.Items.Add(New ListItem("Actual power output", "W"))
                    Item_CBList.Items.Add(New ListItem("Virtual power", "V_ar"))
                    Item_CBList.Items.Add(New ListItem("Apparent power", "VA"))
                Else
                    Item_CBList.Items.Add(New ListItem("實功", "W"))
                    Item_CBList.Items.Add(New ListItem("虛功", "V_ar"))
                    Item_CBList.Items.Add(New ListItem("視在", "VA"))
                End If

                Item_CBList.Items(0).Selected = True

                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")

                '建置開始时间dropdownlist
                For i = 0 To 23
                    begin_hh.Items.Add(Right("0" & i.ToString, 2))
                    end_hh.Items.Add(Right("0" & i.ToString, 2))
                Next
                begin_hh.Items(0).Selected = True '預設
                end_hh.Items(23).Selected = True '預設

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

    Protected Sub Item_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Item_DropDownList.SelectedIndexChanged
        Item_CBList.Items.Clear()
        Demand_CBList.Items.Clear()
        W_CBList.Items.Clear()
        'Meter_TreeView.SelectedNode.Selected = False
        If Item_DropDownList.SelectedValue = "電壓" Or Item_DropDownList.SelectedValue = "Voltage" Then
            Demand_CBList.Visible = False
            Mode.Visible = False
            Demand.Visible = False
            W_CBList.Visible = False

            If Session("language") = "en" Then
                Item_CBList.Items.Add(New ListItem("R Phase", "V1"))
                Item_CBList.Items.Add(New ListItem("S Phase", "V2"))
                Item_CBList.Items.Add(New ListItem("T Phase", "V3"))
                Item_CBList.Items.Add(New ListItem("Average voltage", "Vavg"))
            Else
                Item_CBList.Items.Add(New ListItem("R相", "V1"))
                Item_CBList.Items.Add(New ListItem("S相", "V2"))
                Item_CBList.Items.Add(New ListItem("T相", "V3"))
                Item_CBList.Items.Add(New ListItem("平均電壓", "Vavg"))
            End If

            For i = 0 To 3
                Item_CBList.Items(i).Selected = True
            Next
        ElseIf Item_DropDownList.SelectedValue = "電流" Or Item_DropDownList.SelectedValue = "Current" Then
            Demand_CBList.Visible = False
            Mode.Visible = False
            Demand.Visible = False
            W_CBList.Visible = False

            If Session("language") = "en" Then
                Item_CBList.Items.Add(New ListItem("R Phase", "I1"))
                Item_CBList.Items.Add(New ListItem("S Phase", "I2"))
                Item_CBList.Items.Add(New ListItem("T Phase", "I3"))
                Item_CBList.Items.Add(New ListItem("Average current", "Iavg"))
            Else
                Item_CBList.Items.Add(New ListItem("R相", "I1"))
                Item_CBList.Items.Add(New ListItem("S相", "I2"))
                Item_CBList.Items.Add(New ListItem("T相", "I3"))
                Item_CBList.Items.Add(New ListItem("平均電流", "Iavg"))
            End If

            For i = 0 To 3
                Item_CBList.Items(i).Selected = True
            Next
        ElseIf Item_DropDownList.SelectedValue = "功率" Or Item_DropDownList.SelectedValue = "Power" Then
            Demand_CBList.Visible = False
            Mode.Visible = False
            Demand.Visible = False
            W_CBList.Visible = False
            If Session("language") = "en" Then
                Item_CBList.Items.Add(New ListItem("Actual power output", "W"))
                Item_CBList.Items.Add(New ListItem("Virtual power", "V_ar"))
                Item_CBList.Items.Add(New ListItem("Apparent power", "VA"))
            Else
                Item_CBList.Items.Add(New ListItem("實功", "W"))
                Item_CBList.Items.Add(New ListItem("虛功", "V_ar"))
                Item_CBList.Items.Add(New ListItem("視在", "VA"))
            End If
            'For i = 0 To 2
            '    Item_CBList.Items(i).Selected = True
            'Next
            Item_CBList.Items(0).Selected = True
        ElseIf Item_DropDownList.SelectedValue = "需量" Or Item_DropDownList.SelectedValue = "Demand" Then
            Demand_CBList.Visible = True
            Mode.Visible = True
            Demand.Visible = True
            W_CBList.Visible = True
            If Session("language") = "en" Then
                Item_CBList.Items.Add(New ListItem("Mode 1", "Mode1"))
                Item_CBList.Items.Add(New ListItem("Mode 2", "Mode2"))
                Item_CBList.Items.Add(New ListItem("Mode 3", "Mode3"))
                Item_CBList.Items.Add(New ListItem("Mode 4", "Mode4"))
                Demand_CBList.Items.Add(New ListItem("Peak Time", "DeMand"))
                Demand_CBList.Items.Add(New ListItem("Half Peak Time", "DeMandHalf"))
                Demand_CBList.Items.Add(New ListItem("Saturday Half Peak Time", "DeMandSatHalf"))
                Demand_CBList.Items.Add(New ListItem("Off Peak Time", "DeMandOff"))
                W_CBList.Items.Add(New ListItem("Actual power output", "W"))
            Else
                Item_CBList.Items.Add(New ListItem("模式1", "Mode1"))
                Item_CBList.Items.Add(New ListItem("模式2", "Mode2"))
                Item_CBList.Items.Add(New ListItem("模式3", "Mode3"))
                Item_CBList.Items.Add(New ListItem("模式4", "Mode4"))
                Demand_CBList.Items.Add(New ListItem("尖峰", "DeMand"))
                Demand_CBList.Items.Add(New ListItem("半尖峰", "DeMandHalf"))
                Demand_CBList.Items.Add(New ListItem("週六半尖峰", "DeMandSatHalf"))
                Demand_CBList.Items.Add(New ListItem("離峰", "DeMandOff"))
                W_CBList.Items.Add(New ListItem("實功", "W"))
            End If
            'For i = 0 To 4
            '    Item_CBList.Items(i).Selected = True
            'Next
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
            Dim start_time As String = Date_txt1.Text & " " & begin_hh.SelectedValue & ":00:00"
            Dim end_time As String = Date_txt2.Text & " " & end_hh.SelectedValue & ":59:59"

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

    'Protected Sub SqlQuery()
    '    SqlDataSource1.SelectParameters.Clear()
    '    SqlDataSource2.SelectParameters.Clear()
    '    SqlDataSource3.SelectParameters.Clear()
    '    SqlDataSource4.SelectParameters.Clear()

    '    Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
    '    Dim value() As String = node_value.Split(vbCrLf)
    '    Dim account As String = value(0).Split(":").GetValue(1)
    '    Dim eco_account As String = value(1).Split(":").GetValue(1)
    '    Dim ctrlnr As String = value(2).Split(":").GetValue(1)
    '    Dim meterid As String = value(3).Split(":").GetValue(1)
    '    Dim start_time As String = Date_txt1.Text & " " & begin_hh.SelectedValue & ":00:00"
    '    Dim end_time As String = Date_txt2.Text & " " & end_hh.SelectedValue & ":59:59"

    '    Dim strcon, strSQL As String
    '    If Session("Account") = "admin" Then
    '        strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_admin(Group_DropDownList.SelectedValue) & ""
    '        strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Account_admin(Group_DropDownList.SelectedValue) & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
    '    Else
    '        strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
    '        strSQL = "select CS.InstallPosition AS ECO_Position,CS.Enabled as ECO_Enabled,MS.* from ControllerSetup as CS,MeterSetup as MS where CS.Account = '" & Session("Account_admin") & "' and MS.CtrlNr = " & ctrlnr & " and MS.MeterID = " & meterid & " and CS.ECO_Account = MS.ECO_Account order by CtrlNr"
    '    End If

    '    '判斷勾選
    '    Dim item_V() As String = {"V1", "V2", "V3", "Vavg"}
    '    Dim item_I() As String = {"I1", "I2", "I3", "Iavg"}
    '    Dim item_W() As String = {"W", "V_ar", "VA"}
    '    Dim item_Mode() As String = {"Mode1", "Mode2", "Mode3", "Mode4", "W"}
    '    Dim SelectItem As String = Nothing

    '    If Item_DropDownList.SelectedValue = "電壓" Then
    '        For i = 0 To 3
    '            If Item_CBList.Items(i).Selected = True Then
    '                SelectItem &= item_V(i) & ","
    '            End If
    '        Next
    '        Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
    '        '時間間隔
    '        If interval_DDList.SelectedValue = "1" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "5" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "30" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "60" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
    '        End If

    '        Try
    '            SqlDataSource1.ConnectionString = strcon
    '            SqlDataSource1.ProviderName = "System.Data.OleDb"
    '            SqlDataSource1.SelectCommand = sql
    '            SqlDataSource1.SelectParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
    '            SqlDataSource1.SelectParameters.Add("MeterID", TypeCode.String, meterid)
    '            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, start_time)
    '            SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, end_time)
    '            Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)   '資料筆數
    '            If dv.Count > 0 Then
    '                Chart_V.Title.Text = "電力趨勢圖(" & eco5_id.Text & "-" & meter_id.Text & " " & InstallPosition.Text & ")"
    '                'msg.Visible = False
    '                Chart_V.Visible = True
    '                Chart_I.Visible = False
    '                Chart_W.Visible = False
    '                Chart_Mode.Visible = False
    '                ConvertPng_btn.Visible = True
    '            Else
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '                Chart_V.Visible = False
    '                Chart_I.Visible = False
    '                Chart_W.Visible = False
    '                Chart_Mode.Visible = False
    '                ConvertPng_btn.Visible = False
    '            End If
    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try

    '    ElseIf Item_DropDownList.SelectedValue = "電流" Then
    '        For i = 0 To 3
    '            If Item_CBList.Items(i).Selected = True Then
    '                SelectItem &= item_I(i) & ","
    '            End If
    '        Next

    '        Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
    '        '時間間隔ˇ
    '        If interval_DDList.SelectedValue = "1" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "5" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "30" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "60" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
    '        End If

    '        Try
    '            SqlDataSource2.ConnectionString = strcon
    '            SqlDataSource2.ProviderName = "System.Data.OleDb"
    '            SqlDataSource2.SelectCommand = sql
    '            SqlDataSource2.SelectParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
    '            SqlDataSource2.SelectParameters.Add("MeterID", TypeCode.String, meterid)
    '            SqlDataSource2.SelectParameters.Add("RecDate", TypeCode.String, start_time)
    '            SqlDataSource2.SelectParameters.Add("RecDate", TypeCode.String, end_time)
    '            Dim dv As DataView = SqlDataSource2.Select(New DataSourceSelectArguments)
    '            If dv.Count > 0 Then
    '                Chart_I.Title.Text = "電力趨勢圖(" & eco5_id.Text & "-" & meter_id.Text & " " & InstallPosition.Text & ")"
    '                'msg.Visible = False
    '                Chart_I.Visible = True
    '                Chart_V.Visible = False
    '                Chart_W.Visible = False
    '                Chart_Mode.Visible = False
    '                ConvertPng_btn.Visible = True
    '            Else
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '                Chart_I.Visible = False
    '                Chart_V.Visible = False
    '                Chart_W.Visible = False
    '                Chart_Mode.Visible = False
    '                ConvertPng_btn.Visible = False
    '            End If
    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try

    '    ElseIf Item_DropDownList.SelectedValue = "功率" Then
    '        For i = 0 To 2
    '            If Item_CBList.Items(i).Selected = True Then
    '                SelectItem &= item_W(i) & ","
    '            End If
    '        Next

    '        Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
    '        '時間間隔
    '        If interval_DDList.SelectedValue = "1" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "5" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "30" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "60" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
    '        End If

    '        Try
    '            SqlDataSource3.ConnectionString = strcon
    '            SqlDataSource3.ProviderName = "System.Data.OleDb"
    '            SqlDataSource3.SelectCommand = sql
    '            SqlDataSource3.SelectParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
    '            SqlDataSource3.SelectParameters.Add("MeterID", TypeCode.String, meterid)
    '            SqlDataSource3.SelectParameters.Add("RecDate", TypeCode.String, start_time)
    '            SqlDataSource3.SelectParameters.Add("RecDate", TypeCode.String, end_time)
    '            Dim dv As DataView = SqlDataSource3.Select(New DataSourceSelectArguments)
    '            If dv.Count > 0 Then
    '                Chart_W.Title.Text = "電力趨勢圖(" & eco5_id.Text & "-" & meter_id.Text & " " & InstallPosition.Text & ")"
    '                'msg.Visible = False
    '                Chart_W.Visible = True
    '                Chart_I.Visible = False
    '                Chart_V.Visible = False
    '                Chart_Mode.Visible = False
    '                ConvertPng_btn.Visible = True
    '            Else
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '                Chart_W.Visible = False
    '                Chart_I.Visible = False
    '                Chart_V.Visible = False
    '                Chart_Mode.Visible = False
    '                ConvertPng_btn.Visible = False
    '            End If
    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try

    '    ElseIf Item_DropDownList.SelectedValue = "預測模式" Then
    '        For i = 0 To 4
    '            If Item_CBList.Items(i).Selected = True Then
    '                SelectItem &= item_Mode(i) & ","
    '            End If
    '        Next

    '        Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
    '        '時間間隔
    '        If interval_DDList.SelectedValue = "1" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "5" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "30" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
    '        ElseIf interval_DDList.SelectedValue = "60" Then
    '            sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
    '        End If

    '        Try
    '            SqlDataSource4.ConnectionString = strcon
    '            SqlDataSource4.ProviderName = "System.Data.OleDb"
    '            SqlDataSource4.SelectCommand = sql
    '            SqlDataSource4.SelectParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
    '            SqlDataSource4.SelectParameters.Add("MeterID", TypeCode.String, meterid)
    '            SqlDataSource4.SelectParameters.Add("RecDate", TypeCode.String, start_time)
    '            SqlDataSource4.SelectParameters.Add("RecDate", TypeCode.String, end_time)
    '            Dim dv As DataView = SqlDataSource4.Select(New DataSourceSelectArguments)
    '            If dv.Count > 0 Then
    '                Chart_Mode.Title.Text = "電力趨勢圖(" & eco5_id.Text & "-" & meter_id.Text & " " & InstallPosition.Text & ")"
    '                'msg.Visible = False
    '                Chart_Mode.Visible = True
    '                Chart_W.Visible = False
    '                Chart_I.Visible = False
    '                Chart_V.Visible = False
    '                ConvertPng_btn.Visible = True
    '            Else
    '                Dim msg As String = "查無資料"
    '                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '                Chart_Mode.Visible = False
    '                Chart_W.Visible = False
    '                Chart_I.Visible = False
    '                Chart_V.Visible = False
    '                ConvertPng_btn.Visible = False
    '            End If
    '        Catch ex As Exception
    '            ex.ToString()
    '        End Try
    '    End If
    'End Sub

    'Protected Sub ViewDetails_btn_Click(sender As Object, e As ImageClickEventArgs) Handles ViewDetails_btn.Click
    '    If Meter_TreeView.SelectedNode Is Nothing Then
    '        Dim msg As String = "請選擇電表"
    '        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
    '        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    '    Else
    '        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Then
    '            SqlQuery()
    '        End If
    '    End If
    'End Sub
End Class
