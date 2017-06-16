Imports System.Data.OleDb
Imports System.Data

Partial Class PowerRecordBatch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then
                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, -1, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Month, 0, Now).ToString("yyyy/MM")

                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    If Session("Account") = "admin" Then    '系統管理者
                        Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
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
                            Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                            Using cmd As New OleDbCommand(strSQL, conn)
                                Using dr As OleDbDataReader = cmd.ExecuteReader
                                    Account_DropDownList.Items.Add("請選擇")
                                    While (dr.Read() = True)
                                        If dr("Account") <> "admin" And dr("ECO_Group") = Session("Group") Then
                                            Dim item As String = dr("Account")
                                            Account_DropDownList.Items.Add(item)
                                        End If
                                    End While
                                End Using
                            End Using
                            For i = 0 To Account_DropDownList.Items.Count - 1
                                If Account_DropDownList.Items(i).Text = Session("Account_admin") Then
                                    Account_DropDownList.Items(i).Selected = True
                                    Exit For
                                End If
                            Next
                        Else
                            Account_DropDownList.Enabled = False
                            Account_DropDownList.Items.Add(Session("Account"))
                        End If
                        'Account_DropDownList_SelectedIndexChanged(Nothing, Nothing)
                    End If
                End Using
            End If
        End If
    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Account_DropDownList.SelectedValue & ""
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            '預存程序
            If Date_CB.Checked = True Then
                Using cmd_sp As New OleDbCommand("Usp_Day_Record", conn)
                    cmd_sp.CommandType = CommandType.StoredProcedure
                    Dim CtrlNrParam As New OleDbParameter("@CtrlNr", CtrlNr_DropDownList.SelectedValue.Split(",").GetValue(1))
                    Dim MeterIDParam As New OleDbParameter("@MeterID", MeterID_DropDownList.SelectedValue)
                    Dim RecDateParam As New OleDbParameter("@RecDate", Date_txt1.Text)
                    cmd_sp.Parameters.Add(CtrlNrParam)
                    cmd_sp.Parameters.Add(MeterIDParam)
                    cmd_sp.Parameters.Add(RecDateParam)
                    cmd_sp.ExecuteNonQuery()

                    Dim msg As String = "完成"
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                End Using
            ElseIf Month_CB.Checked = True Then
                '月份天數
                Dim count As Integer = CInt(DateTime.DaysInMonth(CInt(Date_txt2.Text.Split("/").GetValue(0)), CInt(Date_txt2.Text.Split("/").GetValue(1))))
                Dim dateinfo(count - 1) As String
                For i = 0 To count - 1
                    If i < 9 Then
                        dateinfo(i) = Date_txt2.Text & "/0" & i + 1
                    Else
                        dateinfo(i) = Date_txt2.Text & "/" & i + 1
                    End If
                Next

                If MeterID_DropDownList.SelectedValue = 0 Then
                    For i = 1 To MeterID_DropDownList.Items.Count - 1
                        For j = 0 To dateinfo.Length - 1
                            Using cmd_sp As New OleDbCommand("Usp_Day_Record", conn)
                                cmd_sp.CommandType = CommandType.StoredProcedure
                                Dim CtrlNrParam As New OleDbParameter("@CtrlNr", CtrlNr_DropDownList.SelectedValue.Split(",").GetValue(1))
                                Dim MeterIDParam As New OleDbParameter("@MeterID", i)
                                Dim RecDateParam As New OleDbParameter("@RecDate", dateinfo(j))
                                cmd_sp.Parameters.Add(CtrlNrParam)
                                cmd_sp.Parameters.Add(MeterIDParam)
                                cmd_sp.Parameters.Add(RecDateParam)
                                cmd_sp.ExecuteNonQuery()
                            End Using
                        Next
                    Next
                    Dim msg As String = "完成"
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                Else
                    For j = 0 To dateinfo.Length - 1
                        Using cmd_sp As New OleDbCommand("Usp_Day_Record", conn)
                            cmd_sp.CommandType = CommandType.StoredProcedure
                            Dim CtrlNrParam As New OleDbParameter("@CtrlNr", CtrlNr_DropDownList.SelectedValue.Split(",").GetValue(1))
                            Dim MeterIDParam As New OleDbParameter("@MeterID", MeterID_DropDownList.SelectedValue)
                            Dim RecDateParam As New OleDbParameter("@RecDate", dateinfo(j))
                            cmd_sp.Parameters.Add(CtrlNrParam)
                            cmd_sp.Parameters.Add(MeterIDParam)
                            cmd_sp.Parameters.Add(RecDateParam)
                            cmd_sp.ExecuteNonQuery()
                        End Using
                    Next
                    Dim msg As String = "完成"
                    Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
                End If
            End If
        End Using
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Account_DropDownList.SelectedIndexChanged
        CtrlNr_DropDownList.Items.Clear()
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "select * from ControllerSetup where Account = '" & Account_DropDownList.SelectedValue & "' and Enabled = 1"
            Using cmd As New OleDbCommand(Sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    CtrlNr_DropDownList.Items.Add(New ListItem("請選擇", 0))
                    While (dr.Read() = True)
                        Dim item As String = dr("ECO_Account").ToString & "," & dr("CtrlNr").ToString
                        CtrlNr_DropDownList.Items.Add(item)
                    End While
                End Using
            End Using
        End Using
    End Sub

    Protected Sub CtrlNr_DropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CtrlNr_DropDownList.SelectedIndexChanged
        MeterID_DropDownList.Items.Clear()
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim sql As String = "select * from MeterSetup where ECO_Account = '" & CtrlNr_DropDownList.SelectedValue.Split(",").GetValue(0) & "' and Enabled = 1"
            Using cmd As New OleDbCommand(sql, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    MeterID_DropDownList.Items.Add(New ListItem("請選擇", 0))
                    While (dr.Read() = True)
                        Dim item As String = dr("MeterID").ToString
                        MeterID_DropDownList.Items.Add(item)
                    End While
                End Using
            End Using
        End Using
    End Sub
End Class
