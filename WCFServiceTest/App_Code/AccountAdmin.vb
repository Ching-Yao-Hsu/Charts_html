Imports Microsoft.VisualBasic
Imports System.Data.OleDb

Public Class AccountAdmin
    Inherits System.Web.UI.Page

    '回傳帳號中資料庫建立者
    Protected Function Account_admin(ByVal account As String) As String
        Using conn As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString())
            If conn.State = 0 Then conn.Open()
            Dim sql1 As String = "select * from AdminSetup where Account='" & account & "'"
            Using cmd1 As New OleDbCommand(sql1, conn)
                Using dr1 As OleDbDataReader = cmd1.ExecuteReader
                    If dr1.Read() Then
                        Dim sql2 As String = "select ad.Account from AdminSetup as ad,ControllerSetup as CS where ad.ECO_Group = '" & dr1("ECO_Group").ToString & "' and ad.Account=cs.Account"
                        Using cmd2 As New OleDbCommand(sql2, conn)
                            Using dr2 As OleDbDataReader = cmd2.ExecuteReader
                                If dr2.Read() Then
                                    Return dr2("Account").ToString
                                Else
                                    Return ""
                                End If
                            End Using
                        End Using
                    Else
                        Return ""
                    End If
                End Using
            End Using
        End Using
    End Function

    '回傳群組中資料庫建立者
    Protected Function Find_AdAccount(ByVal group As String) As String
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Dim strSQL As String = "select * from AdminSetup where ECO_Group = '" & group & "' and Enabled = 1 and CreateDB = 1 and Rank <> 0"
            Using cmd As New OleDbCommand(strSQL, conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        Return dr("Account").ToString
                    Else
                        Return ""
                    End If
                End Using
            End Using
        End Using
    End Function

    '回傳時間間隔
    Protected Function TimeInterval() As String
        Dim timeS As String
        Dim datetime As String = Now.ToString("yyyy/MM/dd HH:mm:ss")
        '前兩分鐘
        timeS = DateAdd("n", -5, datetime).ToString("yyyy/MM/dd HH:mm:ss")
        Dim time_range As String = "Recdate between '" & timeS & "' and '" & datetime & "' "
        Return time_range
    End Function

    '回傳日期格式(YYYY/MM/DD)
    Protected Function CYMD(ByVal sDate As String) As String
        Dim strDate As String = ""
        Dim aYMD() As String = Split(sDate, "/")
        If aYMD.Length = 2 Then
            strDate = aYMD(0) & "/" & Right("0" & aYMD(1), 2)
        ElseIf aYMD.Length = 3 Then
            strDate = aYMD(0) & "/" & Right("0" & aYMD(1), 2) & "/" & Right(("0" & aYMD(2)), 2)
        End If
        Return strDate
    End Function
End Class
