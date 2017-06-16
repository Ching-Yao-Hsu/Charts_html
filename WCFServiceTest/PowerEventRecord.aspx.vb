Imports System.Data.OleDb

Partial Class EventRecord
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, 0, Now).ToString("yyyy/MM/dd")
                Date_txt2.Text = DateAdd(DateInterval.Day, 0, Now).ToString("yyyy/MM/dd")

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
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    If Session("Account") = "admin" Then    '系統管理者
                        Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                        Using cmd As New OleDbCommand(strSQL, conn)
                            Using dr As OleDbDataReader = cmd.ExecuteReader
                                If Session("language") = "en" Then
                                    Group_DropDownList.Items.Add("Please choose...")
                                Else
                                    Group_DropDownList.Items.Add("請選擇")
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
                    End If
                End Using
            End If
        End If
    End Sub
End Class
