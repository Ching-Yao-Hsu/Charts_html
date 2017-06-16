Imports System.Data.OleDb

Partial Class NewEcoAccount
    Inherits AccountAdmin

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Or Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not Page.IsPostBack Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    If Session("Account") = "admin" Then    '管理者
                        Dim strSQL As String = "SELECT * FROM AdminSetup WHERE Enabled = 1 AND CreateDB = 1 and Rank <> 0"
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
                                Group_DropDownList.Items(0).Selected = True
                            End Using
                        End Using
                    Else
                        If Session("Rank") = 2 Then
                            Group_DropDownList.Enabled = False
                            Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                            Using cmd As New OleDbCommand(strSQL, conn)
                                Using dr As OleDbDataReader = cmd.ExecuteReader
                                    If Session("language") = "en" Then
                                        Group_DropDownList.Items.Add(New ListItem("Please choose...", 0))
                                    Else
                                        Group_DropDownList.Items.Add(New ListItem("請選擇", 0))
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
                    End If
                End Using
            End If
        End If
    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        Dim num_count As Integer = num_txt.Text.Split(",").Count
        Dim mail_count As Integer = mail_txt.Text.Split(";").Count
        If num_count > 10 Then
            Dim msg As String = "號碼組數不可大於10組"
            If Session("language") = "en" Then
                msg = "The number of groups Can not be greater than 10 groups."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        ElseIf mail_count > 10 Then
            '//
            Dim msg As String = "電子郵件組數不可大於10組"
            If Session("language") = "en" Then
                msg = "E-mail can not be greater than 10 groups."
            End If
            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        Else
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Dim sql1 As String = "select * from ControllerSetup where ECO_Account =? "
                Using cmd1 As New OleDbCommand(sql1, conn)
                    cmd1.Parameters.AddWithValue("?ECO_Account", ecoAccount_txt.Text)
                    Using dr1 As OleDbDataReader = cmd1.ExecuteReader
                        If Not dr1.Read() Then
                            Dim sql2 As String = "select CtrlNr from ControllerSetup where Account = ? and CtrlNr = ?"
                            Using cmd2 As New OleDbCommand(sql2, conn)
                                cmd2.Parameters.AddWithValue("?Account", account)
                                cmd2.Parameters.AddWithValue("?CtrlNr", ctrlnr_DropDownList.SelectedValue)
                                Using dr2 As OleDbDataReader = cmd2.ExecuteReader
                                    '判斷重複控制器編號
                                    If Not dr2.Read() Then
                                        '加入新帳號
                                        Dim sql3 As String = "insert into ControllerSetup " & _
                                                             "(Account, ECO_Account, ECO_Password, ECO_Type, CtrlNr, DrawNr, InstallPosition, IP_Address, Enabled, SMSSentEnabled " & _
                                                             " , EmailSentEnabled, Num, Mail, DayMailSentEnabled, MonthMailSentEnabled, DayMail, MonthMail, DiffTime) " & _
                                                             " Values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) "
                                        Using cmd3 As New OleDbCommand(sql3, conn)
                                            cmd3.Parameters.AddWithValue("?Account", account)
                                            cmd3.Parameters.AddWithValue("?ECO_Account", ecoAccount_txt.Text)
                                            cmd3.Parameters.AddWithValue("?ECO_Password", password_txt.Text)
                                            cmd3.Parameters.AddWithValue("?ECO_Type", EcoType_DropDownList.SelectedValue)
                                            cmd3.Parameters.AddWithValue("?CtrlNr", ctrlnr_DropDownList.SelectedValue)
                                            cmd3.Parameters.AddWithValue("?DrawNr", drawnr_txt.Text)
                                            cmd3.Parameters.AddWithValue("?InstallPosition", position_txt.Text)
                                            cmd3.Parameters.AddWithValue("?IP_Address", IP_txt.Text)
                                            cmd3.Parameters.AddWithValue("?Enabled", 1)
                                            If sendnum_enabled.Checked = True Then
                                                cmd3.Parameters.AddWithValue("?SMSSentEnabled", True)
                                            Else
                                                cmd3.Parameters.AddWithValue("?SMSSentEnabled", False)
                                            End If
                                            If sendmail_enabled.Checked = True Then
                                                cmd3.Parameters.AddWithValue("?EmailSentEnabled", True)
                                            Else
                                                cmd3.Parameters.AddWithValue("?EmailSentEnabled", False)
                                            End If
                                            cmd3.Parameters.AddWithValue("?Num", num_txt.Text)
                                            cmd3.Parameters.AddWithValue("?Mail", mail_txt.Text)

                                            If sendday_enabled.Checked = True Then
                                                cmd3.Parameters.AddWithValue("?DayMailSentEnabled", True)
                                            Else
                                                cmd3.Parameters.AddWithValue("?DayMailSentEnabled", False)
                                            End If
                                            If sendmonth_enabled.Checked = True Then
                                                cmd3.Parameters.AddWithValue("?MonthMailSentEnabled", True)
                                            Else
                                                cmd3.Parameters.AddWithValue("?MonthMailSentEnabled", False)
                                            End If
                                            cmd3.Parameters.AddWithValue("?DayMaisl", daymail_txt.Text)
                                            cmd3.Parameters.AddWithValue("?MonthMail", monthmail_txt.Text)
                                            'cmd3.Parameters.AddWithValue("?DiffTime", DiffTime_txt.Text)
                                            cmd3.ExecuteNonQuery()

                                            ecoAccount_txt.Text = ""
                                            password_txt.Text = ""
                                            drawnr_txt.Text = ""
                                            position_txt.Text = ""
                                            IP_txt.Text = ""
                                            num_txt.Text = ""
                                            mail_txt.Text = ""
                                            daymail_txt.Text = ""
                                            'weekmail_txt.Text = ""
                                            monthmail_txt.Text = ""
                                            'DiffTime_txt.Text = "0"

                                            Dim msg As String = "成功加入新帳號"
                                            If Session("language") = "en" Then
                                                msg = "Successfully joined the new account."
                                            End If
                                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                        End Using
                                    Else
                                        Dim msg As String = "控制器編號重複"
                                        If Session("language") = "en" Then
                                            msg = "Repeat controller number."
                                        End If
                                        Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                                        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                                    End If
                                End Using
                            End Using
                        Else
                            Dim msg As String = "帳號重覆"
                            If Session("language") = "en" Then
                                msg = "Repeat Account."
                            End If
                            Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    Protected Sub cancel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_btn.Click
        ecoAccount_txt.Text = ""
        password_txt.Text = ""
        drawnr_txt.Text = ""
        position_txt.Text = ""
        IP_txt.Text = ""
        num_txt.Text = ""
        mail_txt.Text = ""
        daymail_txt.Text = ""
        'weekmail_txt.Text = ""
        monthmail_txt.Text = ""
        'DiffTime_txt.Text = ""
    End Sub
End Class
