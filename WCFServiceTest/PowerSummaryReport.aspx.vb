Imports System.Data.OleDb

Partial Class SummaryReport
    'Inherits System.Web.UI.Page
    Inherits ObjectBuilding


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                '初始化日期區間
                Date_txt1.Text = DateAdd(DateInterval.Day, -1, Now.Date).ToString("yyyy/MM/dd")
                Date_txt2.Text = Now.ToString("yyyy/MM")

                Dim bStarKWh As String = System.Web.Configuration.WebConfigurationManager.AppSettings("bStarKWh")

                '//群組下拉是選單
                Dim iSelIndex As Integer = 0
                BuildingDropDownList(Group_DropDownList1, iSelIndex, "R")
                Group_DropDownList1.Items(iSelIndex).Selected = True
                BuildingDropDownList(Group_DropDownList2, iSelIndex, "R")
                Group_DropDownList2.Items(iSelIndex).Selected = True

                If bStarKWh = 1 Then

                    If Session("language") = "en" Then
                        item_CheckBoxList1.Items.Add(New ListItem("ElectricityValue", "K"))
                        item_CheckBoxList2.Items.Add(New ListItem("ElectricityValue", "K"))
                    Else
                        item_CheckBoxList1.Items.Add(New ListItem("電表值", "K"))
                        item_CheckBoxList2.Items.Add(New ListItem("電表值", "K"))
                    End If
                    item_CheckBoxList1.Items(4).Selected = True
                    item_CheckBoxList2.Items(5).Selected = True
                End If

                'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                'Using conn As New OleDbConnection(strcon)
                '    If conn.State = 0 Then conn.Open()
                '    If Session("Account") = "admin" Then    '系統管理者
                '        Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                '        Using cmd As New OleDbCommand(strSQL, conn)
                '            Using dr As OleDbDataReader = cmd.ExecuteReader
                '                Group_DropDownList1.Items.Add("請選擇")
                '                Group_DropDownList2.Items.Add("請選擇")
                '                While (dr.Read() = True)
                '                    If dr("Account") <> "admin" Then
                '                        Dim item As String = dr("ECO_Group").ToString
                '                        Group_DropDownList1.Items.Add(item)
                '                        Group_DropDownList2.Items.Add(item)
                '                    End If
                '                End While
                '                Group_DropDownList1.Items(0).Selected = True
                '                Group_DropDownList2.Items(0).Selected = True
                '            End Using
                '        End Using
                '    Else
                '        If Session("Rank") = 2 Then
                '            Group_DropDownList1.Enabled = False
                '            Group_DropDownList2.Enabled = False
                '            Dim strSQL As String = "select * from AdminSetup where Enabled = 1 and CreateDB = 1 and Rank <> 0"
                '            Using cmd As New OleDbCommand(strSQL, conn)
                '                Using dr As OleDbDataReader = cmd.ExecuteReader
                '                    Group_DropDownList1.Items.Add("請選擇")
                '                    Group_DropDownList2.Items.Add("請選擇")
                '                    While (dr.Read() = True)
                '                        If dr("Account") <> "admin" And dr("ECO_Group").ToString = Session("Group").ToString Then
                '                            Dim item As String = dr("ECO_Group").ToString
                '                            Group_DropDownList1.Items.Add(item)
                '                            Group_DropDownList2.Items.Add(item)
                '                        End If
                '                    End While
                '                    Group_DropDownList1.Items(1).Selected = True
                '                    Group_DropDownList2.Items(1).Selected = True
                '                End Using
                '            End Using
                '            'For i = 0 To Group_DropDownList1.Items.Count - 1
                '            '    If Group_DropDownList1.Items(i).Text = Session("Account_admin") Then
                '            '        Group_DropDownList1.Items(i).Selected = True
                '            '        Group_DropDownList2.Items(i).Selected = True
                '            '        Exit For
                '            '    End If
                '            'Next
                '        Else
                '            Group_DropDownList1.Enabled = False
                '            Group_DropDownList1.Items.Add(Session("Group"))
                '            Group_DropDownList2.Enabled = False
                '            Group_DropDownList2.Items.Add(Session("Group"))
                '        End If
                '    End If
                'End Using
            End If
            End If
    End Sub
End Class
