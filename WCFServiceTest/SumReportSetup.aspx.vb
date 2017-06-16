Imports System.Data.OleDb

Partial Class SumReportSetup
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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
                        Dim strSQL As String = "SELECT * FROM AdminSetup WHERE Enabled = 1 and CreateDB = 1 and Rank <> 0"
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
                            '    If Group_DropDownList.Items(i).Text = Session("Account") Then
                            '        Group_DropDownList.Items(i).Selected = True
                            '        Exit For
                            '    End If
                            'Next
                        Else
                            Group_DropDownList.Enabled = False
                            Group_DropDownList.Items.Add(Session("Group"))
                        End If
                        Account_DDList_SelectedIndexChanged(Nothing, Nothing)
                    End If
                End Using
            End If
        End If
    End Sub

    Protected Sub Account_DDList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Dim sql As String = "SELECT * FROM AdminSetup WHERE Account = '" & account & "' "
        'If Session("Rank") = 2 Then
        '    sql = "SELECT * FROM AdminSetup WHERE Account = '" & Group_DropDownList.SelectedValue & "' "
        'Else
        '    sql = "SELECT * FROM AdminSetup WHERE Account = '" & Session("Account_admin") & "' "
        'End If

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        If dr("SumDayMailSent") = True Then
                            sumday_sent.Checked = True
                        Else
                            sumday_sent.Checked = False
                        End If
                        If dr("SumMonthMailSent") = True Then
                            summonth_sent.Checked = True
                        Else
                            summonth_sent.Checked = False
                        End If
                        daymail_txt.Text = dr("SumDayMail").ToString
                        monthmail_txt.Text = dr("SumMonthMail").ToString
                    Else
                        sumday_sent.Checked = False
                        summonth_sent.Checked = False
                        daymail_txt.Text = ""
                        monthmail_txt.Text = ""
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "UPDATE AdminSetup SET SumDayMailSent = ?,SumMonthMailSent = ?,SumDayMail = ?,SumMonthMail = ? WHERE Account = ?"
            Using cmd As New OleDbCommand(sql, conn)
                If sumday_sent.Checked = True Then
                    cmd.Parameters.AddWithValue("?SumDayMailSent", True)
                Else
                    cmd.Parameters.AddWithValue("?SumDayMailSent", False)
                End If
                If summonth_sent.Checked = True Then
                    cmd.Parameters.AddWithValue("?SumMonthMailSent", True)
                Else
                    cmd.Parameters.AddWithValue("?SumMonthMailSent", False)
                End If
                cmd.Parameters.AddWithValue("?SumDayMail", daymail_txt.Text)
                cmd.Parameters.AddWithValue("?SumMonthMail", monthmail_txt.Text)
                cmd.Parameters.AddWithValue("?Account", account)
                cmd.ExecuteNonQuery()

                Dim msg As String = "更新成功"
                If Session("language") = "en" Then
                    msg = "Update successful"
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
            End Using
        End Using
        cancel_btn_Click(Nothing, Nothing)
    End Sub

    Protected Sub edit_btn_Click(sender As Object, e As EventArgs) Handles edit_btn.Click
        'Group_DropDownList.Enabled = True
        sumday_sent.Enabled = True
        summonth_sent.Enabled = True
        daymail_txt.Enabled = True
        monthmail_txt.Enabled = True
        submit_btn.Visible = True
        cancel_btn.Visible = True
        edit_btn.Visible = False
    End Sub

    Protected Sub cancel_btn_Click(sender As Object, e As EventArgs) Handles cancel_btn.Click
        'Group_DropDownList.Enabled = False
        sumday_sent.Enabled = False
        summonth_sent.Enabled = False
        daymail_txt.Enabled = False
        monthmail_txt.Enabled = False
        submit_btn.Visible = False
        cancel_btn.Visible = False
        edit_btn.Visible = True
    End Sub
End Class
