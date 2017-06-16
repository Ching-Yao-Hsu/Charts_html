Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb


Public Class MeterTreeBuilding
    Inherits TreeViewCheck

    Function BuildingTree(ByVal tTreeView As TreeView, ByVal account As String, ByVal CtrlNr_CB As Boolean, ByVal MeterId_CB As Boolean, ByVal LineNum_CB As Boolean, ByVal Position_CB As Boolean, ByVal nEnable As Integer) As Boolean
        tTreeView.Nodes.Clear()

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim sql As String = " exec [ECOSMART].[dbo].[ReadMeterTree] " & nEnable.ToString.Trim & ",'" & account & "', '', '' "

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                'Dim schemaTable As DataTable = dr.GetSchemaTable()

                Dim dt As DataTable = New DataTable()
                dt.Load(dr)

                For Each row In dt.Rows
                    Dim Ret As Boolean = DisplayTreeView(tTreeView, row)
                Next
            End Using
        End Using

        tTreeView.ExpandAll()

        If tTreeView.Nodes.Count = 0 Then
            Dim tmpNote As New TreeNode
            If Session("language") = "en" Then tmpNote.Text = "The meter has not been defined."
            If Session("language") = "tw" Or Session("language") = "" Then tmpNote.Text = "尚未定義電表"
            tTreeView.Nodes.Add(tmpNote)
        Else
            Dim cmdIndex(3) As Boolean
            cmdIndex(0) = CtrlNr_CB
            cmdIndex(1) = MeterId_CB
            cmdIndex(2) = True
            cmdIndex(3) = LineNum_CB
            Position_CB = True
            For Each node As TreeNode In tTreeView.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
        Return True
    End Function


    Function WBuildingTree(ByVal tTreeView As TreeView, ByRef bID As Boolean, ByRef bDrawNo As Boolean, ByRef bShopNo As Boolean, ByRef bShopName As Boolean, ByVal bShowCheckBox As Boolean) As Boolean
        tTreeView.Nodes.Clear()


        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=Water "
        'Dim sql As String = " Select '00' as IDLine , ID, IP, Port ,MeterID, 圖面編號, 店鋪編號, 店鋪名稱   from [dbo].[Setup] " & _
        '                    " Where ID=1 " & _
        '                    " Union " & _
        '                    " Select '00-' + Right('00' + ltrim(CAST(ID AS nVarChar(3) )),3)  as IDLine , ID, IP, Port, MeterID, 圖面編號, 店鋪編號, 店鋪名稱   from [dbo].[Setup] " & _
        '                    " Where ID > 1 " & _
        '                    " order by 1 "

        '//連同最近的水表值一起讀取
        Dim sql As String = " Select '00' as IDLine , M.ID, IP, Port ,MeterID, 圖面編號, 店鋪編號, 店鋪名稱 " & _
                            " ,isNull((Select rtrim(A.Recdate) + '**' + rtrim(CAST(isnull(A.水表值,'') as nvarchar(50))) From dbo.Record A WITH (NOLOCK) " & _
                            " Where A.ID=M.ID and A.RecDate = (select Max(B.Recdate) From dbo.Record B WITH (NOLOCK) Where B.ID=A.ID )),'**') as nValue " & _
                            " from [dbo].[Setup] as M " & _
                            " Where M.ID=1  Union  " & _
                            " Select '00-' + Right('00' + ltrim(CAST(ID AS nVarChar(3) )),3)  as IDLine , M.ID, IP, Port, MeterID, 圖面編號, 店鋪編號, 店鋪名稱 " & _
                            "  , isNull((Select rtrim(A.Recdate) + '**' +rtrim(CAST(isnull(A.水表值,'') as nvarchar(50))) From dbo.Record A WITH (NOLOCK) " & _
                            " Where A.ID=M.ID and A.RecDate = (select Max(B.Recdate) From dbo.Record B WITH (NOLOCK) Where B.ID=A.ID )),'**') as nValue " & _
                            " from [Water].[dbo].[Setup] as M  Where M.ID > 1  order by 1 "

        Using conn As New OleDbConnection(strcon)
            If conn.State = 0 Then conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                'Dim schemaTable As DataTable = dr.GetSchemaTable()

                Dim dt As DataTable = New DataTable()
                dt.Load(dr)

                For Each row In dt.Rows
                    Dim Ret As Boolean = DisplayTreeViewW(tTreeView, row, bShowCheckBox)
                Next
            End Using
        End Using

        tTreeView.ExpandAll()

        If tTreeView.Nodes.Count = 0 Then
            Dim tmpNote As New TreeNode
            tmpNote.Text = "尚未定義電表"
            tTreeView.Nodes.Add(tmpNote)
        Else
            Dim cmdIndex(3) As Boolean
            cmdIndex(0) = bID
            cmdIndex(1) = bDrawNo
            cmdIndex(2) = bShopNo
            cmdIndex(3) = True
            bShopName = True
            For Each node As TreeNode In tTreeView.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
        Return True
    End Function


End Class
