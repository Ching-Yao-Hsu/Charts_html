Imports System.Data.OleDb
Imports System.Data

Partial Class EcoTable
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Dim ecoaccount As String = Request.QueryString("eco_account")
            Dim sql As String = "SELECT * FROM ControllerSetup WHERE ECO_Account =?"
            Dim conn As OleDbConnection = New OleDbConnection(strcon)
            Try
                conn.Open()
                Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?ECO_Account", ecoaccount)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()

                '顯示控制器資訊
                If dr.Read() Then
                    Account.Text = dr("Account")
                    ECO_Account.Text = dr("ECO_Account")
                    ECO_Password.Text = dr("ECO_Password")
                    ECO_Type.Text = dr("ECO_Type")
                    CtrlNr.Text = dr("CtrlNr")

                    If dr("DrawNr") IsNot DBNull.Value Then
                        DrawNr.Text = dr("DrawNr")
                    End If

                    If dr("InstallPosition") IsNot DBNull.Value Then
                        InstallPosition.Text = dr("InstallPosition")
                    End If

                    If dr("Enabled") = 1 Then
                        enabled_RadioButtonList.SelectedValue = 1
                    ElseIf dr("Enabled") = 0 Then
                        enabled_RadioButtonList.SelectedValue = 0
                    End If

                    'old_EcoAccount.Text = dr("ECO_Account")
                End If
                cmd.Dispose()
                dr.Close()
                conn.Close()
            Catch ex As Exception
                conn.Close()
            End Try
        End If
    End Sub

    Protected Sub update_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles update_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim conn As OleDbConnection = New OleDbConnection(strcon)
        conn.Open()

        'If old_EcoAccount.Text = ECO_Account.Text Then   '帳號不變
        Dim sql As String = "UPDATE ControllerSetup SET ECO_Password = ?, DrawNr = ?, InstallPosition = ?, Enabled = ? WHERE ECO_Account = ? "

        Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
        cmd.Parameters.AddWithValue("?ECO_Password", ECO_Password.Text)
        'cmd.Parameters.AddWithValue("?CtrlNr", ctrlnr_DropDownList.SelectedValue)
        cmd.Parameters.AddWithValue("?DrawNr", DrawNr.Text)
        cmd.Parameters.AddWithValue("?InstallPosition", InstallPosition.Text)
        cmd.Parameters.AddWithValue("?Enabled", enabled_RadioButtonList.SelectedValue)
        cmd.Parameters.AddWithValue("?ECO_Account", ECO_Account.Text)
        cmd.ExecuteReader()
        cmd.Dispose()

        Dim msg As String = "更新成功！"
        If Session("language") = "en" Then
            msg = "Update successful."
        End If

        Dim script As String = "<script>alert('" & msg & "');</script>"
        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        'Else    '帳號更改
        'Dim sql2 As String = "SELECT * FROM ControllerSetup WHERE ECO_Account=?"
        'Dim cmd2 As OleDbCommand = New OleDbCommand(sql2, conn)
        'cmd2.Parameters.AddWithValue("?ECO_Account", ECO_Account.Text)
        'Dim dr As OleDbDataReader = cmd2.ExecuteReader()

        'If dr.Read() Then
        '    Dim msg As String = "帳號重複！"
        '    Dim script As String = "<script>alert('" & msg & "');</script>"
        '    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
        'Else
        '    Dim sql As String = "UPDATE ControllerSetup SET ECO_Password = ?," & _
        '                        "DrawNr = ?, InstallPosition = ?, Enabled = ? WHERE ECO_Account = ? "

        '    Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
        '    'cmd.Parameters.AddWithValue("?ECO_Account", ECO_Account.Text)
        '    cmd.Parameters.AddWithValue("?ECO_Password", ECO_Password.Text)
        '    'cmd.Parameters.AddWithValue("?CtrlNr", ctrlnr_DropDownList.SelectedValue)
        '    cmd.Parameters.AddWithValue("?DrawNr", DrawNr.Text)
        '    cmd.Parameters.AddWithValue("?InstallPosition", InstallPosition.Text)
        '    cmd.Parameters.AddWithValue("?Enabled", enabled_RadioButtonList.SelectedValue)
        '    cmd.Parameters.AddWithValue("?ECO_Account", old_EcoAccount.Text)
        '    cmd.ExecuteNonQuery()
        '    cmd.Dispose()

        '    Dim msg As String = "更新成功！"
        '    Dim script As String = "<script>alert('" & msg & "');</script>"
        '    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        'End If
        'End If
        conn.Close()
    End Sub

End Class
