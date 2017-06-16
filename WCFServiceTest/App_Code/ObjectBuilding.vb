Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb


Public Class ObjectBuilding
    Inherits MeterTreeBuilding


    Public Sub BuildingDropDownList(ByRef DDL As DropDownList, ByRef iSelIndex As Integer, Optional sProgramType As String = "M")

        Dim iReadIndex As Integer = 0

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        ' Dim strSQL As String = "select * from AdminSetup with (Nolock) where Enabled = 1 and CreateDB = 1 and Rank <> 0"
        Dim strSQL As String = " Select * from  [dbo].[AdminSetup] where Account in (Select Com From AdminCom Where Account='" & Session("Account") & "' and Enabled=1) and Enabled = 1 and CreateDB = 1 and Rank <> 0 and account<>'admin'"

        Dim strPerNum As String = System.Web.Configuration.WebConfigurationManager.AppSettings("bPerNum")
        If strPerNum = "1" Then
            strSQL = strSQL & " and PerNum='" & Session("PerNum") & "' "
        End If

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()

            Using cmd As New OleDbCommand(strSQL, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If Session("language") = "en" Then   '//英文版
                        DDL.Items.Add("Please choose...")
                        If System.Web.Configuration.WebConfigurationManager.AppSettings("bOpenGroup") = "1" And sProgramType = "R" Then   '//報表（可列印全部）
                            DDL.Items.Add("ALL")
                        End If
                    Else '//中文版
                        DDL.Items.Add("請選擇")
                        If System.Web.Configuration.WebConfigurationManager.AppSettings("bOpenGroup") = "1" And sProgramType = "R" Then   '//報表（可列印全部）
                            DDL.Items.Add("全部")
                        End If
                    End If

                    Dim sGroup As String = ""
                    While (dr.Read() = True)
                        Dim item As String = dr("ECO_Group").ToString
                        Dim itemV As String = dr("Account").ToString
                        DDL.Items.Add(item)
                        iReadIndex = iReadIndex + 1
                        If Not (Session("Group01") Is Nothing) Then
                            If Session("Group01") = itemV Then
                                iSelIndex = iReadIndex
                            End If
                        Else
                            sGroup = itemV
                        End If
                    End While

                    If (iReadIndex = 1) And (Session("Group01") Is Nothing) Then
                        iSelIndex = 1
                        Session("Group01") = sGroup
                    End If

                    'While (dr.Read() = True)
                    '    If Session("Account") = "admin" Then    '系統管理者
                    '        If dr("Account") <> "admin" Then
                    '            Dim item As String = dr("ECO_Group").ToString
                    '            Dim itemV As String = dr("Account").ToString
                    '            DDL.Items.Add(item)
                    '            iReadIndex = iReadIndex + 1
                    '            If Not (Session("Group01") Is Nothing) Then
                    '                If Session("Group01") = itemV Then
                    '                    iSelIndex = iReadIndex
                    '                End If
                    '            End If
                    '        End If
                    '    Else
                    '        DDL.Enabled = False
                    '        If Session("Rank") = 2 Then
                    '            'While (dr.Read() = True)
                    '            If dr("Account") <> "admin" And dr("ECO_Group").ToString = Session("Group").ToString Then
                    '                Dim item As String = dr("ECO_Group").ToString
                    '                DDL.Items.Add(item)
                    '            End If
                    '            'End While
                    '        Else
                    '            DDL.Items.Add(Session("Group"))
                    '        End If
                    '        iSelIndex = 1
                    '    End If
                    'End While

                End Using
            End Using
            DDL.SelectedIndex = DDL.Items.IndexOf(DDL.Items(iSelIndex))
        End Using

    End Sub

End Class
