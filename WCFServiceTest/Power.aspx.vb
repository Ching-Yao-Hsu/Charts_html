Imports System.Data.OleDb

Partial Class Power
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
            Dim conn As OleDbConnection = New OleDbConnection(strcon)
            conn.Open()
            If Session("Account") Is Nothing Then
                Response.Redirect("home.aspx")
            ElseIf Session("Account") = "admin" Then    '管理者
                Dim sql = "select Account from AdminSetup"
                Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                Account_DropDownList.Items.Add("請選擇")
                While (dr.Read() = True)
                    Dim item As String = dr("Account")
                    Account_DropDownList.Items.Add(item)
                End While
                Account_DropDownList.Items(0).Selected = True
                cmd.Dispose()
                dr.Close()
                Account_DropDownList.Enabled = True
                Account_DropDownList.AutoPostBack = True
            Else
                Account_DropDownList.Items.Add(Session("Account"))
                Dim strSQL As String = "select ECO_Account from ControllerSetup where Account = '" & Session("Account") & "' and Enabled = 1"
                Dim cmd As OleDbCommand = New OleDbCommand(strSQL, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                EcoAccount_DropDownList.Items.Add("請選擇")
                While (dr.Read() = True)
                    Dim item As String = dr("ECO_Account")
                    EcoAccount_DropDownList.Items.Add(item)
                End While
                EcoAccount_DropDownList.Items(0).Selected = True

                cmd.Dispose()
                dr.Close()
                conn.Close()
            End If
            conn.Close()

            '初始化日期區間
            If Month(Now) < 10 And Day(Now) < 10 Then
                Date_txt1.Text = Year(Now) & "/0" & Month(Now) & "/0" & Day(Now)
            ElseIf Month(Now) < 10 And Day(Now) > 9 Then
                Date_txt1.Text = Year(Now) & "/0" & Month(Now) & "/" & Day(Now)
            ElseIf Month(Now) > 9 And Day(Now) < 10 Then
                Date_txt1.Text = Year(Now) & "/" & Month(Now) & "/0" & Day(Now)
            Else
                Date_txt1.Text = Year(Now) & "/" & Month(Now) & "/" & Day(Now)
            End If

            If Month(Now) < 10 And Day(Now) < 10 Then
                Date_txt2.Text = Year(Now) & "/0" & Month(Now) & "/0" & Day(Now)
            ElseIf Month(Now) < 10 And Day(Now) > 9 Then
                Date_txt2.Text = Year(Now) & "/0" & Month(Now) & "/" & Day(Now)
            ElseIf Month(Now) > 9 And Day(Now) < 10 Then
                Date_txt2.Text = Year(Now) & "/" & Month(Now) & "/0" & Day(Now)
            Else
                Date_txt2.Text = Year(Now) & "/" & Month(Now) & "/" & Day(Now)
            End If

            '建置開始时間dropdownlist
            For i = 0 To 9
                begin_hh.Items.Add("0" & i & "")
            Next
            For i = 10 To 23
                begin_hh.Items.Add("" & i & "")
            Next
            For i = 0 To 9
                begin_mm.Items.Add("0" & i & "")
            Next
            For i = 10 To 59
                begin_mm.Items.Add("" & i & "")
            Next
            '預設今天
            begin_hh.Items(0).Selected = True
            begin_mm.Items(0).Selected = True


            '建置結束时間dropdownlist
            For i = 0 To 9
                end_hh.Items.Add("0" & i & "")
            Next
            For i = 10 To 23
                end_hh.Items.Add("" & i & "")
            Next

            For i = 0 To 9
                end_mm.Items.Add("0" & i & "")
            Next
            For i = 10 To 59
                end_mm.Items.Add("" & i & "")
            Next

            '預設今天
            For i = 0 To end_hh.Items.Count - 1
                If end_hh.Items(i).Text = Hour(Now) Then
                    end_hh.Items(i).Selected = True
                End If
            Next
            For i = 0 To end_mm.Items.Count - 1
                If end_mm.Items(i).Text = Minute(Now) Then
                    end_mm.Items(i).Selected = True
                End If
            Next
        End If
    End Sub

    Protected Sub EcoAccount_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EcoAccount_DropDownList.SelectedIndexChanged
        If Meter_DropDownList.Items.Count <> 0 Then
            Meter_DropDownList.Items.Clear()
        End If

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim conn As OleDbConnection = New OleDbConnection(strcon)
        conn.Open()
        Dim strSQL As String = "select CtrlNr,MeterID from MeterSetup where ECO_Account = '" & EcoAccount_DropDownList.SelectedValue & "' and Enabled = 1"
        Dim cmd As OleDbCommand = New OleDbCommand(strSQL, conn)
        Dim dr As OleDbDataReader = cmd.ExecuteReader()
        Meter_DropDownList.Items.Add("請選擇")
        While (dr.Read() = True)
            CtrlNr.Text = dr("CtrlNr")
            'CtrlNr.Enabled = True
            Dim item As String = dr("MeterID")
            Meter_DropDownList.Items.Add(item)
        End While
        Meter_DropDownList.Items(0).Selected = True

        cmd.Dispose()
        dr.Close()
        conn.Close()
    End Sub

    Protected Sub Account_DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Account_DropDownList.SelectedIndexChanged
        If EcoAccount_DropDownList.Items.Count <> 0 Then
            EcoAccount_DropDownList.Items.Clear()
        End If
        If Meter_DropDownList.Items.Count <> 0 Then
            Meter_DropDownList.Items.Clear()
        End If

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim conn As OleDbConnection = New OleDbConnection(strcon)
        conn.Open()

        If Account_DropDownList.SelectedValue = "admin" Then
            CtrlNr.Text = ""
            Dim strSQL As String = "select ECO_Account from ControllerSetup"
            Dim cmd As OleDbCommand = New OleDbCommand(strSQL, conn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader()
            EcoAccount_DropDownList.Items.Add("請選擇")
            While (dr.Read() = True)
                Dim item As String = dr("ECO_Account")
                EcoAccount_DropDownList.Items.Add(item)
            End While
            EcoAccount_DropDownList.Items(0).Selected = True

            cmd.Dispose()
            dr.Close()

        ElseIf Account_DropDownList.SelectedValue <> "請選擇" Then
            CtrlNr.Text = ""
            Dim strSQL As String = "select ECO_Account from ControllerSetup where Account = '" & Account_DropDownList.SelectedValue & "' and Enabled = 1"
            Dim cmd As OleDbCommand = New OleDbCommand(strSQL, conn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader()
            EcoAccount_DropDownList.Items.Add("請選擇")
            While (dr.Read() = True)
                Dim item As String = dr("ECO_Account")
                EcoAccount_DropDownList.Items.Add(item)
            End While
            EcoAccount_DropDownList.Items(0).Selected = True

            cmd.Dispose()
            dr.Close()
        Else
            CtrlNr.Text = ""
        End If
        conn.Close()
    End Sub

    'Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click

    '    If Session("Account") Is Nothing Then
    '        Response.Redirect("home.aspx")
    '    ElseIf Session("Account") = "admin" Then
    '        Dim account As String = Account_DropDownList.SelectedValue

    '        Dim strcon As String = "Provider=SQLOLEDB;Data Source=ECOSMART-HP;Persist Security Info=False;Password=1234;User ID=test_man;Initial Catalog=" & account & ""
    '        Dim conn As OleDbConnection = New OleDbConnection(strcon)
    '        conn.Open()
    '        Dim sql = "select * from AdminSetup"
    '    Else

    '    End If
    'End Sub
End Class
