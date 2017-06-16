Imports System.Data
Imports System.Data.OleDb

Partial Class EcoInfo
    Inherits AccountAdmin

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    If Session("Account") = "admin" Then    '管理者
                        Dim strSQL As String = "select * from AdminSetup where Enabled = 1 AND CreateDB = 1 and Rank <> 0"
                        Using cmd As New OleDbCommand(strSQL, conn)
                            Using dr As OleDbDataReader = cmd.ExecuteReader
                                If Session("language") = "en" Then
                                    Group_DropDownList.Items.Add(New ListItem("Please choose...", 0))
                                Else
                                    Group_DropDownList.Items.Add(New ListItem("請選擇", 0))
                                End If
                                While (dr.Read() = True)
                                    If dr("Account") <> "admin" Then
                                        Dim item As String = dr("ECO_Group").ToString
                                        Group_DropDownList.Items.Add(item)
                                    End If
                                End While
                            End Using
                        End Using
                        Group_DropDownList.Items(0).Selected = True
                    Else
                        If Session("Rank") = 2 Then
                            Group_DropDownList.Enabled = False
                            Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                            Using cmd As New OleDbCommand(strSQL, conn)
                                Using dr As OleDbDataReader = cmd.ExecuteReader
                                    If Session("language") = "en" Then
                                        Group_DropDownList.Items.Add("Please choose...")
                                    Else
                                        Group_DropDownList.Items.Add("請選擇")
                                    End If
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
                        '先判斷此帳號始啟用，否則不顯示任何訊息
                        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
                        Dim sql = "SELECT * FROM ControllerSetup WHERE Account = '" & account & "' ORDER BY CtrlNr"
                        Using cmd As New OleDbCommand(sql, conn)
                            Using dr As OleDbDataReader = cmd.ExecuteReader
                                While (dr.Read() = True)
                                    Dim NodeName As String = ""
                                    Dim tooltip As String = ""
                                    If Session("language") = "en" Then
                                        NodeName = "ECO5_Number:" & dr("CtrlNr").ToString & " Location:" & dr("InstallPosition").ToString
                                        tooltip = "Account:" & dr("Account").ToString & vbCrLf & "ECO5_Account:" & dr("ECO_Account").ToString
                                    Else
                                        NodeName = "ECO5編號:" & dr("CtrlNr").ToString & " 位置:" & dr("InstallPosition").ToString
                                        tooltip = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString
                                    End If
                                    Dim NewNode As New TreeNode
                                    NewNode.Value = NodeName
                                    NewNode.ToolTip = tooltip
                                    Ctrl_TreeView.Nodes.Add(NewNode)
                                End While
                            End Using
                        End Using
                        delete_btn.Visible = True
                    End If
                End Using
            End If
        End If
    End Sub

    Protected Sub Account_DDList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        If Ctrl_TreeView.Nodes.Count <> 0 Then
            Ctrl_TreeView.Nodes.Clear()
            eco_enabled.Checked = False
            eco5_id.Text = ""
            eco5_account.Text = ""
            eco5_password.Text = ""
            eco5_type.Text = ""
            drawnr.Text = ""
            InstallPosition.Text = ""
            IP_txt.Text = ""
            DiffTime_txt.Text = "0"
            sendnum_enabled.Checked = False
            sendmail_enabled.Checked = False
            num_txt.Text = ""
            mail_txt.Text = ""
            sendday_enabled.Checked = False
            'If dr("WeekMailSnetEnabled") = True Then
            '    sendweek_enabled.Checked = True
            'Else
            '    sendweek_enabled.Checked = False
            'End If
            sendmonth_enabled.Checked = False
            daymail_txt.Text = ""
            'weekmail_txt.Text = dr("WeekMail").ToString
            monthmail_txt.Text = ""
            delete_btn.Visible = False
        End If

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Dim sql As String = "SELECT * FROM ControllerSetup WHERE Account = '" & account & "' ORDER BY CtrlNr"
        'If Session("Rank") = 2 Then
        '    sql = "SELECT * FROM ControllerSetup WHERE Account = '" & Account_admin(Group_DropDownList.SelectedValue) & "' ORDER BY CtrlNr"
        'Else
        '    sql = "SELECT * FROM ControllerSetup WHERE Account = '" & Session("Account_admin") & "' ORDER BY CtrlNr"
        'End If

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    While (dr.Read() = True)
                        Dim NodeName As String = ""
                        Dim tooltip As String = ""

                        If Session("language") = "en" Then
                            NodeName = "ECO5_Number:" & dr("CtrlNr").ToString & " Location:" & dr("InstallPosition").ToString
                            tooltip = "Account:" & dr("Account").ToString & vbCrLf & "ECO5_Account:" & dr("ECO_Account").ToString
                        Else
                            NodeName = "ECO5編號:" & dr("CtrlNr").ToString & " 位置:" & dr("InstallPosition").ToString
                            tooltip = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString
                        End If

                        Dim NewNode As New TreeNode
                        NewNode.Value = NodeName
                        NewNode.ToolTip = tooltip
                        Ctrl_TreeView.Nodes.Add(NewNode)
                        'delete_btn.Visible = True
                    End While
                End Using
            End Using
        End Using
    End Sub

    Protected Sub Ctrl_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Ctrl_TreeView.SelectedNodeChanged
        Dim node_value As String = Ctrl_TreeView.SelectedNode.Value
        Dim value() As String = node_value.Split(" ")
        Dim ctrlnr As String = value(0).Split(":").GetValue(1)
        Dim node_name As String = Ctrl_TreeView.SelectedNode.ToolTip
        Dim value2() As String = node_name.Split(vbCrLf)
        Dim eco_account As String = value2(1).Split(":").GetValue(1)

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Dim sql As String = "select * from ControllerSetup where Account = '" & account & "' and ECO_Account = '" & eco_account & "' and CtrlNr = " & ctrlnr & ""
        'If Session("Rank") = 2 Then
        '    sql = "select * from ControllerSetup where Account = '" & Account_admin(Group_DropDownList.SelectedValue) & "' and ECO_Account = '" & eco_account & "' and CtrlNr = " & ctrlnr & ""
        'Else
        '    sql = "select * from ControllerSetup where Account = '" & Session("Account_admin") & "' and ECO_Account = '" & eco_account & "' and CtrlNr = " & ctrlnr & ""
        'End If
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        If dr("Enabled") = True Then
                            eco_enabled.Checked = True
                        Else
                            eco_enabled.Checked = False
                        End If
                        eco5_id.Text = dr("CtrlNr").ToString
                        eco5_account.Text = dr("ECO_Account").ToString
                        eco5_password.Text = dr("ECO_Password").ToString
                        If dr("ECO_Type").ToString <> "" Then
                            eco5_type.Text = dr("ECO_Type").ToString
                        End If
                        drawnr.Text = dr("DrawNr").ToString
                        InstallPosition.Text = dr("InstallPosition").ToString
                        IP_txt.Text = dr("IP_Address").ToString
                        DiffTime_txt.Text = dr("DiffTime").ToString
                        If dr("SMSSentEnabled") = True Then
                            sendnum_enabled.Checked = True
                        Else
                            sendnum_enabled.Checked = False
                        End If
                        If dr("EmailSentEnabled") = True Then
                            sendmail_enabled.Checked = True
                        Else
                            sendmail_enabled.Checked = False
                        End If
                        num_txt.Text = dr("Num").ToString
                        mail_txt.Text = dr("Mail").ToString
                        If dr("DayMailSentEnabled") = True Then
                            sendday_enabled.Checked = True
                        Else
                            sendday_enabled.Checked = False
                        End If
                        'If dr("WeekMailSnetEnabled") = True Then
                        '    sendweek_enabled.Checked = True
                        'Else
                        '    sendweek_enabled.Checked = False
                        'End If
                        If dr("MonthMailSentEnabled") = True Then
                            sendmonth_enabled.Checked = True
                        Else
                            sendmonth_enabled.Checked = False
                        End If
                        daymail_txt.Text = dr("DayMail").ToString
                        'weekmail_txt.Text = dr("WeekMail").ToString
                        monthmail_txt.Text = dr("MonthMail").ToString
                    Else
                        eco_enabled.Checked = False
                        eco5_id.Text = ""
                        eco5_account.Text = ""
                        eco5_password.Text = ""
                        eco5_type.Text = ""
                        drawnr.Text = ""
                        InstallPosition.Text = ""
                        IP_txt.Text = ""
                        DiffTime_txt.Text = "0"
                        sendnum_enabled.Checked = False
                        sendmail_enabled.Checked = False
                        num_txt.Text = ""
                        mail_txt.Text = ""
                        sendday_enabled.Checked = False
                        'If dr("WeekMailSnetEnabled") = True Then
                        '    sendweek_enabled.Checked = True
                        'Else
                        '    sendweek_enabled.Checked = False
                        'End If
                        sendmonth_enabled.Checked = False
                        daymail_txt.Text = ""
                        'weekmail_txt.Text = dr("WeekMail").ToString
                        monthmail_txt.Text = ""
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        Dim num_count As Integer = num_txt.Text.Split(",").Count
        Dim mail_count As Integer = mail_txt.Text.Split(";").Count
        Dim daymail_count As Integer = daymail_txt.Text.Split(";").Count
        'Dim weekmail_count As Integer = weekmail_txt.Text.Split(";").Count
        Dim monthmail_count As Integer = monthmail_txt.Text.Split(";").Count

        If num_count > 10 Then
            Dim msg As String = "號碼組數不可大於10組"
            If Session("language") = "en" Then
                msg = "The number of groups Can not be greater than 10 groups."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        ElseIf mail_count > 10 Then
            Dim msg As String = "電子郵件組數不可大於10組"
            If Session("language") = "en" Then
                msg = "E-mail can not be greater than 10 groups."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        ElseIf daymail_count > 10 Then
            Dim msg As String = "日報表電子郵件組數不可大於10組"
            If Session("language") = "en" Then
                msg = "[Daily Report] E-mail can not be greater than 10 groups."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            'ElseIf weekmail_count > 10 Then
            '    Dim msg As String = "週報表電子郵件組數不可大於10組"
            '    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            '    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        ElseIf monthmail_count > 10 Then
            Dim msg As String = "月報表電子郵件組數不可大於10組"
            If Session("language") = "en" Then
                msg = "[Month Report] E-mail can not be greater than 10 groups."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        Else
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Dim sql As String = "update ControllerSetup SET ECO_Password = ?, DrawNr = ?,InstallPosition = ?,IP_Address = ?,DiffTime = ?,Enabled = ?,SMSSentEnabled = ?,EmailSentEnabled = ?,Num = ?,Mail = ?,DayMailSentEnabled = ?,MonthMailSentEnabled = ?,DayMail = ?,MonthMail = ? WHERE ECO_Account = ? and CtrlNr = ? "
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?ECO_Password", eco5_password.Text)
                    cmd.Parameters.AddWithValue("?DrawNr", drawnr.Text)
                    cmd.Parameters.AddWithValue("?InstallPosition", InstallPosition.Text)
                    cmd.Parameters.AddWithValue("?IP_Address", IP_txt.Text)
                    cmd.Parameters.AddWithValue("?DiffTime", DiffTime_txt.Text)
                    If eco_enabled.Checked = True Then
                        cmd.Parameters.AddWithValue("?Enabled", True)
                    Else
                        cmd.Parameters.AddWithValue("?Enabled", False)
                    End If

                    If sendnum_enabled.Checked = True Then
                        cmd.Parameters.AddWithValue("?SMSSentEnabled", True)
                    Else
                        cmd.Parameters.AddWithValue("?SMSSentEnabled", False)
                    End If
                    If sendmail_enabled.Checked = True Then
                        cmd.Parameters.AddWithValue("?EmailSentEnabled", True)
                    Else
                        cmd.Parameters.AddWithValue("?EmailSentEnabled", False)
                    End If
                    cmd.Parameters.AddWithValue("?Num", num_txt.Text)
                    cmd.Parameters.AddWithValue("?Mail", mail_txt.Text)

                    If sendday_enabled.Checked = True Then
                        cmd.Parameters.AddWithValue("?DayMailSentEnabled", True)
                    Else
                        cmd.Parameters.AddWithValue("?DayMailSentEnabled", False)
                    End If

                    'If sendweek_enabled.Checked = True Then
                    '    cmd.Parameters.AddWithValue("?WeekMailSnetEnabled", True)
                    'Else
                    '    cmd.Parameters.AddWithValue("?WeekMailSnetEnabled", False)
                    'End If

                    If sendmonth_enabled.Checked = True Then
                        cmd.Parameters.AddWithValue("?MonthMailSentEnabled", True)
                    Else
                        cmd.Parameters.AddWithValue("?MonthMailSentEnabled", False)
                    End If
                    cmd.Parameters.AddWithValue("?DayMail", daymail_txt.Text)
                    'cmd.Parameters.AddWithValue("?WeekMail", weekmail_txt.Text)
                    cmd.Parameters.AddWithValue("?MonthMail", monthmail_txt.Text)

                    cmd.Parameters.AddWithValue("?Account", eco5_account.Text)
                    cmd.Parameters.AddWithValue("?CtrlNr", eco5_id.Text)
                    cmd.ExecuteNonQuery()
                    Account_DDList_SelectedIndexChanged(Nothing, Nothing)
                End Using
            End Using
        End If
        cancel_btn_Click(Nothing, Nothing)
    End Sub

    Protected Sub delete_btn_Click(sender As Object, e As EventArgs) Handles delete_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim sql As String = "Delete from ControllerSetup WHERE ECO_Account = ?"
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?ECO_Account", eco5_account.Text)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        Ctrl_TreeView.Nodes.Remove(Ctrl_TreeView.SelectedNode)
        UpdatePanel1.Update()

        'Dim msg As String = "删除成功"
        'Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
        'Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    End Sub

    Protected Sub edit_btn_Click(sender As Object, e As EventArgs) Handles edit_btn.Click
        drawnr.Enabled = True
        InstallPosition.Enabled = True
        IP_txt.Enabled = True
        DiffTime_txt.Enabled = True
        eco_enabled.Enabled = True
        eco5_password.Enabled = True
        sendnum_enabled.Enabled = True
        sendmail_enabled.Enabled = True
        num_txt.Enabled = True
        mail_txt.Enabled = True
        sendday_enabled.Enabled = True
        'sendweek_enabled.Enabled = True
        sendmonth_enabled.Enabled = True
        daymail_txt.Enabled = True
        'weekmail_txt.Enabled = True
        monthmail_txt.Enabled = True
        submit_btn.Enabled = True
        cancel_btn.Enabled = True
        edit_btn.Enabled = False
        delete_btn.Visible = True
    End Sub

    Protected Sub cancel_btn_Click(sender As Object, e As EventArgs) Handles cancel_btn.Click
        drawnr.Enabled = False
        InstallPosition.Enabled = False
        IP_txt.Enabled = False
        DiffTime_txt.Enabled = False
        eco_enabled.Enabled = False
        eco5_password.Enabled = False
        sendnum_enabled.Enabled = False
        sendmail_enabled.Enabled = False
        num_txt.Enabled = False
        mail_txt.Enabled = False
        sendday_enabled.Enabled = False
        'sendweek_enabled.Enabled = False
        sendmonth_enabled.Enabled = False
        daymail_txt.Enabled = False
        'weekmail_txt.Enabled = False
        monthmail_txt.Enabled = False
        submit_btn.Enabled = False
        cancel_btn.Enabled = False
        edit_btn.Enabled = True
        delete_btn.Visible = False
    End Sub
End Class
