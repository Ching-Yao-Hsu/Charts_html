Imports Microsoft.VisualBasic
Imports System.Data

Public Class TreeViewCheck
    Inherits AccountAdmin

    'Public WithEvents cn As New SqlClient.SqlConnection
    'Public ConnectionString As String   '連線字串
    'Public CheckDB As Boolean           '檢查DB是否連線成功  True , False
    ''Public I1(), I2(), I3(), Iavg(), V1(), V2(), V3(), Vavg(), V_ar(), VA(), W(), PF(), KWh(), Mode1(), Mode2(), Mode3(), Mode4() As Double
    'Public LineNumber(), MeterLoop() As String
    ''Public SFD As New SaveFileDialog

    ''修改物件的Text屬性為唯讀(為防止使用enabled屬性所造成閱讀障礙，可使用唯讀屬性執行)
    ''Sub ModifyStateChange(ByVal Con As Object, ByVal State As Boolean)
    ''    For Each Txt As Object In Con.Controls
    ''        If TypeOf (Txt) Is TextBox Or TypeOf (Txt) Is MaskedTextBox Then Txt.ReadOnly = State
    ''    Next
    ''End Sub

#Region "樹狀結構圖有關的函數與方法"
    '找出指定編號的節點
    Function FindLastNode(ByVal n As TreeNode, ByVal LineNum As String) As TreeNode
        If n.Value = LineNum Then Return n
        For Each aNode As TreeNode In n.ChildNodes
            n = FindLastNode(aNode, LineNum)
            If n IsNot Nothing Then Return n
        Next
    End Function

    '找出最類似的節點
    Function FindSimilarNode(ByVal n As TreeNode, ByVal LineNum As String, ByVal Level As Integer) As TreeNode
        If Microsoft.VisualBasic.Left(n.Value, LineNum.Length) = LineNum Then Return n
        For Each aNode As TreeNode In n.ChildNodes
            n = FindSimilarNode(aNode, LineNum, Level)
            If n IsNot Nothing Then
                If aNode.Depth = Level Then
                    n = aNode
                End If
                Return n
            End If
        Next
    End Function

    '在指定的TreeView物件建立節點
    'Function DisplayTreeView(ByVal Target As TreeView, ByVal NodeName As String, ByVal LineNum As String, ByVal TagData As String, ByVal ToolTipText As String, ByVal MeterType As Integer) As Boolean
    Function DisplayTreeView(ByVal Target As TreeView, ByVal dr As DataRow) As Boolean

        '(1)Account , (2)ECO_Account, (3)CtrlNr, (4)MeterID, (5)InstallPosition, 
        '(6)LineNum, (7)Enabled, (8)MeterType, (9)RecDate, (10)Iavg, (11)Vavg, (12)W , (13)UpLoadStatus

        Dim sLineNum As String = ""          '//未啟用 會是Null值
        If Not IsDBNull(dr("LineNum")) Then sLineNum = dr("LineNum")

        Dim sMeterType As String = ""          '//未啟用 會是Null值
        If Not IsDBNull(dr("MeterType")) Then sMeterType = dr("MeterType")

        Dim sUpLoadStatus As Integer = -1    '//未啟用 會是空白
        If dr("UpLoadStatus").ToString <> "" Then sUpLoadStatus = dr("UpLoadStatus")

        Dim X() As String = Split(sLineNum, "-")
        Dim Level As Integer = X.Length     '節點的階層數
        Dim Node As New TreeNode

        Dim TagData As String = dr("CtrlNr").ToString & "," & dr("MeterID").ToString & "," & dr("InstallPosition").ToString & "," & sLineNum & "," & sUpLoadStatus.ToString
        Dim ToolTipText As String = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString & vbCrLf & "ECO5編號:" & dr("CtrlNr").ToString & vbCrLf & "電表編號:" & dr("MeterID").ToString & vbCrLf & "安裝位置:" & dr("InstallPosition").ToString

        '是否為無節點之TreeView物件
        If Target.Nodes.Count = 0 Then
            If Level = 1 Then
                Node.Value = sLineNum
                Node.Target = TagData
                Node.ToolTip = ToolTipText
                Node.ShowCheckBox = True
                Call ModifyNode(Node, 2, , sMeterType)
                Target.Nodes.Add(Node)
            Else
                Call ModifyNode(Node, 2, , 8)
                Node.ShowCheckBox = True
                Target.Nodes.Add(Node)
                'Node = Target.Nodes(0)
                Dim SpaceNode As TreeNode = Nothing
                For c = 1 To Level - 2
                    If SpaceNode Is Nothing Then
                        SpaceNode = New TreeNode
                        Call ModifyNode(SpaceNode, 2, , 8)
                        Node.ChildNodes.Add(SpaceNode)
                        Node = Node.ChildNodes(0)
                    Else
                        Dim SpaceNode2 As New TreeNode
                        Call ModifyNode(SpaceNode2, 2, , 8)
                        SpaceNode.ChildNodes.Add(SpaceNode2)
                        SpaceNode = SpaceNode.ChildNodes(0)
                    End If
                Next
                Dim NewNode As New TreeNode
                NewNode.Value = sLineNum
                NewNode.Target = TagData
                NewNode.ToolTip = ToolTipText
                NewNode.ShowCheckBox = True
                Call ModifyNode(NewNode, 2, , sMeterType)
                If SpaceNode IsNot Nothing Then
                    SpaceNode.ChildNodes.Add(NewNode)
                Else
                    Node.ChildNodes.Add(NewNode)
                End If
            End If
        Else
            ''//如果未定義電表, 就不檢查是否重複
            'If sUpLoadStatus <> -1 Then
            '找出是否重複
            For Each NodeTempA As TreeNode In Target.Nodes
                Node = FindLastNode(NodeTempA, sLineNum)
                If Node IsNot Nothing Then Exit For
            Next
            'End If
            If Node IsNot Nothing Then      '重複
                Return False
            Else                            '沒有重複，找出最後一個節點
                Dim LineNumTemp As String = X(0)
                Level -= 1                  '計算為上一層節點的階層數
                Dim f As Integer = 0
                Dim LastNodeTemp As New TreeNode
                Dim NewNode As New TreeNode
                Dim d As Integer = 0
                While d <= Level - 1      'Level為目標節點的陣列大小，目標節點上一層陣列大小為Level-1
                    For Each NodeTempB As TreeNode In Target.Nodes
                        Node = FindLastNode(NodeTempB, LineNumTemp)
                        If Node IsNot Nothing Then Exit For
                    Next
                    If Node IsNot Nothing Then
                        LastNodeTemp = Node
                        f += 1
                    End If
                    d += 1
                    LineNumTemp += "-" & X(d)
                End While
                If LastNodeTemp.Target = "" Then
                    '找出最類似的節點
                    d = 0
                    While d < Level
                        d += 1
                        LineNumTemp = Microsoft.VisualBasic.Left(LineNumTemp, LineNumTemp.Length - 3)
                        For Each NodeTempA As TreeNode In Target.Nodes
                            Node = FindSimilarNode(NodeTempA, LineNumTemp, Level - d)
                            If Node IsNot Nothing Then
                                If Level - d = 0 Then Node = NodeTempA
                                Exit While
                            End If
                        Next
                    End While
                    '找出類似節點之後的處理
                    If Node IsNot Nothing Then  '有找到類似節點
                        d = Node.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = d To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                Node.ChildNodes.Add(SpaceNode)
                                Node = Node.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = sLineNum
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = True
                        Call ModifyNode(NewNode, 2, , sMeterType)
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            Node.ChildNodes.Add(NewNode)
                        End If
                    Else    '沒找到類似節點
                        If Level > 0 Then
                            Call ModifyNode(NewNode, 2, , 8)
                            Target.Nodes.Add(NewNode)
                            Dim SpaceNode As TreeNode = Nothing
                            For g = 1 To Level - 1
                                If SpaceNode Is Nothing Then
                                    SpaceNode = New TreeNode
                                    Call ModifyNode(SpaceNode, 2, , 8)
                                    NewNode.ChildNodes.Add(SpaceNode)
                                    NewNode = NewNode.ChildNodes(0)
                                Else
                                    Dim SpaceNode2 As New TreeNode
                                    Call ModifyNode(SpaceNode2, 2, , 8)
                                    SpaceNode.ChildNodes.Add(SpaceNode2)
                                    SpaceNode = SpaceNode.ChildNodes(0)
                                End If
                            Next
                            Dim NewNode2 As New TreeNode
                            NewNode2.Value = sLineNum
                            NewNode2.Target = TagData
                            NewNode2.ToolTip = ToolTipText
                            NewNode2.ShowCheckBox = True
                            Call ModifyNode(NewNode2, 2, , sMeterType)
                            If SpaceNode IsNot Nothing Then
                                SpaceNode.ChildNodes.Add(NewNode2)
                            Else
                                NewNode.ChildNodes.Add(NewNode2)
                            End If
                        Else
                            NewNode.Value = sLineNum
                            NewNode.Target = TagData
                            NewNode.ToolTip = ToolTipText
                            NewNode.ShowCheckBox = True
                            Call ModifyNode(NewNode, 2, , sMeterType)
                            Target.Nodes.Add(NewNode)
                        End If
                    End If
                Else
                    '判斷該節點之後是否仍有子節點存在
                    If LastNodeTemp.ChildNodes.Count = 0 Then
                        f = LastNodeTemp.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = f To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                LastNodeTemp.ChildNodes.Add(SpaceNode)
                                LastNodeTemp = LastNodeTemp.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = sLineNum
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = True
                        Call ModifyNode(NewNode, 2, , sMeterType)
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            LastNodeTemp.ChildNodes.Add(NewNode)
                        End If

                    Else    '仍然有子節點存在
                        d = 0
                        While d < Level
                            d += 1
                            LineNumTemp = Microsoft.VisualBasic.Left(LineNumTemp, LineNumTemp.Length - 3)
                            Node = FindSimilarNode(LastNodeTemp, LineNumTemp, Level - d)
                            If Node IsNot Nothing Then
                                Exit While
                            End If
                        End While
                        If Node Is Nothing Then
                            Node = LastNodeTemp
                        End If

                        d = Node.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = d To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                Node.ChildNodes.Add(SpaceNode)
                                Node = Node.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = sLineNum
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = True
                        Call ModifyNode(NewNode, 2, , sMeterType)
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            Node.ChildNodes.Add(NewNode)
                        End If
                    End If
                End If
            End If
        End If
        Return True
    End Function

    Function DisplayTreeViewW(ByVal Target As TreeView, ByVal dr As DataRow, ByVal bShowCheck As Boolean) As Boolean

        '// (1) '00' as IDLine , (2)ID,         (3)IP,        (4)Port  (5)MeterID, 
        '// (6) [圖面編號],      (7)[店鋪編號], (8)[店鋪名稱]

        Dim X() As String = Split(dr("IDLine"), "-")
        Dim Level As Integer = X.Length     '節點的階層數
        Dim Node As New TreeNode


        Dim TagData As String = dr("ID").ToString & "," & dr("圖面編號").ToString.Trim & "," & dr("店鋪編號").ToString.Trim & "," & dr("店鋪名稱").ToString.Trim & "," & dr("MeterID").ToString & "," & dr("IDLine").ToString & ", 1"
        Dim ToolTipText As String = "ID:" & dr("ID").ToString & vbCrLf & "圖面編號:" & dr("圖面編號").ToString.Trim & vbCrLf & "店鋪編號:" & dr("店鋪編號").ToString.Trim & vbCrLf & "店鋪名稱:" & dr("店鋪名稱").ToString.Trim & vbCrLf & dr("nValue").ToString.Trim

        '是否為無節點之TreeView物件
        If Target.Nodes.Count = 0 Then
            If Level = 1 Then
                Node.Value = dr("IDLine")
                Node.Target = TagData
                Node.ToolTip = ToolTipText
                Node.ShowCheckBox = bShowCheck
                Call ModifyNode(Node, 2, , 7)    '//給?圖
                Target.Nodes.Add(Node)
            Else
                Call ModifyNode(Node, 2, , 8)
                Node.ShowCheckBox = bShowCheck
                Target.Nodes.Add(Node)
                'Node = Target.Nodes(0)
                Dim SpaceNode As TreeNode = Nothing
                For c = 1 To Level - 2
                    If SpaceNode Is Nothing Then
                        SpaceNode = New TreeNode
                        Call ModifyNode(SpaceNode, 2, , 8)
                        Node.ChildNodes.Add(SpaceNode)
                        Node = Node.ChildNodes(0)
                    Else
                        Dim SpaceNode2 As New TreeNode
                        Call ModifyNode(SpaceNode2, 2, , 8)
                        SpaceNode.ChildNodes.Add(SpaceNode2)
                        SpaceNode = SpaceNode.ChildNodes(0)
                    End If
                Next
                Dim NewNode As New TreeNode
                NewNode.Value = dr("IDLine")
                NewNode.Target = TagData
                NewNode.ToolTip = ToolTipText
                NewNode.ShowCheckBox = bShowCheck
                Call ModifyNode(NewNode, 2, , 7)    '//給?圖
                If SpaceNode IsNot Nothing Then
                    SpaceNode.ChildNodes.Add(NewNode)
                Else
                    Node.ChildNodes.Add(NewNode)
                End If
            End If
        Else
            '找出是否重複
            For Each NodeTempA As TreeNode In Target.Nodes
                Node = FindLastNode(NodeTempA, dr("IDLine"))
                If Node IsNot Nothing Then Exit For
            Next
            If Node IsNot Nothing Then      '重複
                Return False
            Else                            '沒有重複，找出最後一個節點
                Dim LineNumTemp As String = X(0)
                Level -= 1                  '計算為上一層節點的階層數
                Dim f As Integer = 0
                Dim LastNodeTemp As New TreeNode
                Dim NewNode As New TreeNode
                Dim d As Integer = 0
                While d <= Level - 1      'Level為目標節點的陣列大小，目標節點上一層陣列大小為Level-1
                    For Each NodeTempB As TreeNode In Target.Nodes
                        Node = FindLastNode(NodeTempB, LineNumTemp)
                        If Node IsNot Nothing Then Exit For
                    Next
                    If Node IsNot Nothing Then
                        LastNodeTemp = Node
                        f += 1
                    End If
                    d += 1
                    LineNumTemp += "-" & X(d)
                End While
                If LastNodeTemp.Target = "" Then
                    '找出最類似的節點
                    d = 0
                    While d < Level
                        d += 1
                        LineNumTemp = Microsoft.VisualBasic.Left(LineNumTemp, LineNumTemp.Length - 4)    '//
                        For Each NodeTempA As TreeNode In Target.Nodes
                            Node = FindSimilarNode(NodeTempA, LineNumTemp, Level - d)
                            If Node IsNot Nothing Then
                                If Level - d = 0 Then Node = NodeTempA
                                Exit While
                            End If
                        Next
                    End While
                    '找出類似節點之後的處理
                    If Node IsNot Nothing Then  '有找到類似節點
                        d = Node.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = d To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                Node.ChildNodes.Add(SpaceNode)
                                Node = Node.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = dr("IDLine")
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = bShowCheck
                        Call ModifyNode(NewNode, 2, , 7)    '//給?圖
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            Node.ChildNodes.Add(NewNode)
                        End If
                    Else    '沒找到類似節點
                        If Level > 0 Then
                            Call ModifyNode(NewNode, 2, , 8)
                            Target.Nodes.Add(NewNode)
                            Dim SpaceNode As TreeNode = Nothing
                            For g = 1 To Level - 1
                                If SpaceNode Is Nothing Then
                                    SpaceNode = New TreeNode
                                    Call ModifyNode(SpaceNode, 2, , 8)
                                    NewNode.ChildNodes.Add(SpaceNode)
                                    NewNode = NewNode.ChildNodes(0)
                                Else
                                    Dim SpaceNode2 As New TreeNode
                                    Call ModifyNode(SpaceNode2, 2, , 8)
                                    SpaceNode.ChildNodes.Add(SpaceNode2)
                                    SpaceNode = SpaceNode.ChildNodes(0)
                                End If
                            Next
                            Dim NewNode2 As New TreeNode
                            NewNode2.Value = dr("IDLine")
                            NewNode2.Target = TagData
                            NewNode2.ToolTip = ToolTipText
                            NewNode2.ShowCheckBox = True
                            Call ModifyNode(NewNode2, 2, , 7)    '//給?圖
                            If SpaceNode IsNot Nothing Then
                                SpaceNode.ChildNodes.Add(NewNode2)
                            Else
                                NewNode.ChildNodes.Add(NewNode2)
                            End If
                        Else
                            NewNode.Value = dr("IDLine")
                            NewNode.Target = TagData
                            NewNode.ToolTip = ToolTipText
                            NewNode.ShowCheckBox = bShowCheck
                            Call ModifyNode(NewNode, 2, , 7)    '//給?圖
                            Target.Nodes.Add(NewNode)
                        End If
                    End If
                Else
                    '判斷該節點之後是否仍有子節點存在
                    If LastNodeTemp.ChildNodes.Count = 0 Then
                        f = LastNodeTemp.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = f To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                LastNodeTemp.ChildNodes.Add(SpaceNode)
                                LastNodeTemp = LastNodeTemp.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = dr("IDLine")
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = bShowCheck
                        Call ModifyNode(NewNode, 2, , 7)    '//給?圖
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            LastNodeTemp.ChildNodes.Add(NewNode)
                        End If

                    Else    '仍然有子節點存在
                        d = 0
                        While d < Level
                            d += 1
                            LineNumTemp = Microsoft.VisualBasic.Left(LineNumTemp, LineNumTemp.Length - 4)    '//
                            Node = FindSimilarNode(LastNodeTemp, LineNumTemp, Level - d)
                            If Node IsNot Nothing Then
                                Exit While
                            End If
                        End While
                        If Node Is Nothing Then
                            Node = LastNodeTemp
                        End If

                        d = Node.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = d To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                Node.ChildNodes.Add(SpaceNode)
                                Node = Node.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = dr("IDLine")
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = bShowCheck
                        Call ModifyNode(NewNode, 2, , 7)    '//給?圖
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            Node.ChildNodes.Add(NewNode)
                        End If
                    End If
                End If
            End If
        End If
        Return True
    End Function

    '在指定的TreeView物件建立節點
    Function DisplayTreeView(ByVal Target As TreeView, ByVal NodeName As String, ByVal LineNum As String, ByVal TagData As String, ByVal ToolTipText As String, ByVal MeterType As Integer) As Boolean

        Dim X() As String = Split(LineNum, "-")
        Dim Level As Integer = X.Length     '節點的階層數
        Dim Node As New TreeNode

        'Dim TagData As String = dr("CtrlNr").ToString & "," & dr("MeterID").ToString & "," & dr("InstallPosition").ToString & "," & dr("LineNum").ToString & "," & dr("UpLoadStatus").ToString
        'Dim ToolTipText As String = "帳號:" & dr("Account").ToString & vbCrLf & "ECO5帳號:" & dr("ECO_Account").ToString & vbCrLf & "ECO5編號:" & dr("CtrlNr").ToString & vbCrLf & "電表編號:" & dr("MeterID").ToString & vbCrLf & "安裝位置:" & dr("InstallPosition").ToString

        '是否為無節點之TreeView物件
        If Target.Nodes.Count = 0 Then
            If Level = 1 Then
                Node.Value = LineNum
                Node.Target = TagData
                Node.ToolTip = ToolTipText
                Node.ShowCheckBox = True
                Call ModifyNode(Node, 2, , MeterType)
                Target.Nodes.Add(Node)
            Else
                Call ModifyNode(Node, 2, , 8)
                Node.ShowCheckBox = True
                Target.Nodes.Add(Node)
                'Node = Target.Nodes(0)
                Dim SpaceNode As TreeNode = Nothing
                For c = 1 To Level - 2
                    If SpaceNode Is Nothing Then
                        SpaceNode = New TreeNode
                        Call ModifyNode(SpaceNode, 2, , 8)
                        Node.ChildNodes.Add(SpaceNode)
                        Node = Node.ChildNodes(0)
                    Else
                        Dim SpaceNode2 As New TreeNode
                        Call ModifyNode(SpaceNode2, 2, , 8)
                        SpaceNode.ChildNodes.Add(SpaceNode2)
                        SpaceNode = SpaceNode.ChildNodes(0)
                    End If
                Next
                Dim NewNode As New TreeNode
                NewNode.Value = LineNum
                NewNode.Target = TagData
                NewNode.ToolTip = ToolTipText
                NewNode.ShowCheckBox = True
                Call ModifyNode(NewNode, 2, , MeterType)
                If SpaceNode IsNot Nothing Then
                    SpaceNode.ChildNodes.Add(NewNode)
                Else
                    Node.ChildNodes.Add(NewNode)
                End If
            End If
        Else
            '找出是否重複
            For Each NodeTempA As TreeNode In Target.Nodes
                Node = FindLastNode(NodeTempA, LineNum)
                If Node IsNot Nothing Then Exit For
            Next
            If Node IsNot Nothing Then      '重複
                Return False
            Else                            '沒有重複，找出最後一個節點
                Dim LineNumTemp As String = X(0)
                Level -= 1                  '計算為上一層節點的階層數
                Dim f As Integer = 0
                Dim LastNodeTemp As New TreeNode
                Dim NewNode As New TreeNode
                Dim d As Integer = 0
                While d <= Level - 1      'Level為目標節點的陣列大小，目標節點上一層陣列大小為Level-1
                    For Each NodeTempB As TreeNode In Target.Nodes
                        Node = FindLastNode(NodeTempB, LineNumTemp)
                        If Node IsNot Nothing Then Exit For
                    Next
                    If Node IsNot Nothing Then
                        LastNodeTemp = Node
                        f += 1
                    End If
                    d += 1
                    LineNumTemp += "-" & X(d)
                End While
                If LastNodeTemp.Target = "" Then
                    '找出最類似的節點
                    d = 0
                    While d < Level
                        d += 1
                        LineNumTemp = Microsoft.VisualBasic.Left(LineNumTemp, LineNumTemp.Length - 3)
                        For Each NodeTempA As TreeNode In Target.Nodes
                            Node = FindSimilarNode(NodeTempA, LineNumTemp, Level - d)
                            If Node IsNot Nothing Then
                                If Level - d = 0 Then Node = NodeTempA
                                Exit While
                            End If
                        Next
                    End While
                    '找出類似節點之後的處理
                    If Node IsNot Nothing Then  '有找到類似節點
                        d = Node.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = d To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                Node.ChildNodes.Add(SpaceNode)
                                Node = Node.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = LineNum
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = True
                        Call ModifyNode(NewNode, 2, , MeterType)
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            Node.ChildNodes.Add(NewNode)
                        End If
                    Else    '沒找到類似節點
                        If Level > 0 Then
                            Call ModifyNode(NewNode, 2, , 8)
                            Target.Nodes.Add(NewNode)
                            Dim SpaceNode As TreeNode = Nothing
                            For g = 1 To Level - 1
                                If SpaceNode Is Nothing Then
                                    SpaceNode = New TreeNode
                                    Call ModifyNode(SpaceNode, 2, , 8)
                                    NewNode.ChildNodes.Add(SpaceNode)
                                    NewNode = NewNode.ChildNodes(0)
                                Else
                                    Dim SpaceNode2 As New TreeNode
                                    Call ModifyNode(SpaceNode2, 2, , 8)
                                    SpaceNode.ChildNodes.Add(SpaceNode2)
                                    SpaceNode = SpaceNode.ChildNodes(0)
                                End If
                            Next
                            Dim NewNode2 As New TreeNode
                            NewNode2.Value = LineNum
                            NewNode2.Target = TagData
                            NewNode2.ToolTip = ToolTipText
                            NewNode2.ShowCheckBox = True
                            Call ModifyNode(NewNode2, 2, , MeterType)
                            If SpaceNode IsNot Nothing Then
                                SpaceNode.ChildNodes.Add(NewNode2)
                            Else
                                NewNode.ChildNodes.Add(NewNode2)
                            End If
                        Else
                            NewNode.Value = LineNum
                            NewNode.Target = TagData
                            NewNode.ToolTip = ToolTipText
                            NewNode.ShowCheckBox = True
                            Call ModifyNode(NewNode, 2, , MeterType)
                            Target.Nodes.Add(NewNode)
                        End If
                    End If
                Else
                    '判斷該節點之後是否仍有子節點存在
                    If LastNodeTemp.ChildNodes.Count = 0 Then
                        f = LastNodeTemp.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = f To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                LastNodeTemp.ChildNodes.Add(SpaceNode)
                                LastNodeTemp = LastNodeTemp.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = LineNum
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = True
                        Call ModifyNode(NewNode, 2, , MeterType)
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            LastNodeTemp.ChildNodes.Add(NewNode)
                        End If

                    Else    '仍然有子節點存在
                        d = 0
                        While d < Level
                            d += 1
                            LineNumTemp = Microsoft.VisualBasic.Left(LineNumTemp, LineNumTemp.Length - 3)
                            Node = FindSimilarNode(LastNodeTemp, LineNumTemp, Level - d)
                            If Node IsNot Nothing Then
                                Exit While
                            End If
                        End While
                        If Node Is Nothing Then
                            Node = LastNodeTemp
                        End If

                        d = Node.Depth + 1
                        Dim SpaceNode As TreeNode = Nothing
                        For g = d To Level - 1
                            If SpaceNode Is Nothing Then
                                SpaceNode = New TreeNode
                                Call ModifyNode(SpaceNode, 2, , 8)
                                Node.ChildNodes.Add(SpaceNode)
                                Node = Node.ChildNodes(0)
                            Else
                                Dim SpaceNode2 As New TreeNode
                                Call ModifyNode(SpaceNode2, 2, , 8)
                                SpaceNode.ChildNodes.Add(SpaceNode2)
                                SpaceNode = SpaceNode.ChildNodes(0)
                            End If
                        Next
                        NewNode.Value = LineNum
                        NewNode.Target = TagData
                        NewNode.ToolTip = ToolTipText
                        NewNode.ShowCheckBox = True
                        Call ModifyNode(NewNode, 2, , MeterType)
                        If SpaceNode IsNot Nothing Then
                            SpaceNode.ChildNodes.Add(NewNode)
                        Else
                            Node.ChildNodes.Add(NewNode)
                        End If
                    End If
                End If
            End If
        End If
        Return True
    End Function

    '修改節點內容，Meter_cmd=1 改變Text內容  cmd=2 顯示哪個圖形
    Sub ModifyNode(ByVal n As TreeNode, ByVal cmd As Integer, Optional ByVal cmdIndex As Boolean() = Nothing, Optional ByVal cmdContent As String = Nothing)

        Dim TagData() As String
        If n.Target <> "" Then
            TagData = Split(n.Target, ",")

            If TagData(TagData.Length - 1) = -1 Then
                '//0 ECO5編號
                TagData(0) = "ECO5編號:" & TagData(0)
                '//1 電表編號
                TagData(1) = "電表編號:" & TagData(1)
                '//2 安裝位置
                TagData(2) = "安裝位置:" & TagData(2)
            End If
        End If

        Select Case cmd
            Case 1  '改變指定Node之下，每個Node的Text內容
                Dim NodeText As String = ""
                If n.Target <> "" Then
                    'Dim TagData() As String = Split(n.Target, ",")
                    For i = 0 To cmdIndex.Length - 1
                        If cmdIndex(i) = True Then
                            NodeText = NodeText & "-" & TagData(i)
                        End If
                    Next
                    If NodeText <> "" Then NodeText = Mid(NodeText, 2)
                    n.Text = NodeText
                End If
                For Each anode As TreeNode In n.ChildNodes
                    Call ModifyNode(anode, cmd, cmdIndex, )
                Next
            Case 2  '改變指定的Node圖示
                'Dim ImageIndex As Integer = CInt(TagData(5)) '圖示顯示的索引，TagData(5):電表型式
                '7~12：連線正常  13~18：連線異常  19~24：警報發生

                Dim sPhoto As Boolean = True  '//有分離線或上線
                If n.Target <> "" Then
                    If TagData(TagData.Length - 1) = -1 Then sPhoto = False
                Else
                    sPhoto = False
                End If

                If sPhoto Then     '//Target有值  表接收時間與系統時間相差值
                    Select Case cmdContent
                        Case 1
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/AP40/AP40-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/AP40/AP40-On.jpg"
                            End If
                        Case 2
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/DM2436/DM2436-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/AP40/DM2436-On.jpg"
                            End If

                        Case 3
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/CT-1700/CT-1700-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/CT-1700/CT-1700-On.jpg"
                            End If

                        Case 4
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/CT713P/CT713P-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/CT713P/CT713P-On.jpg"
                            End If

                        Case 5
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/SPM-8/SPM-8-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/SPM-8/SPM-8-On.jpg"
                            End If

                        Case 6
                            cmdContent = "~/img/MeterType/PM710/PM710.gif"

                        Case 7
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/other/other1-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/other/other1-On.jpg"
                            End If

                        Case Else
                            'cmdContent = "~/img/MeterType/other/other2-Leave.jpg"
                            If TagData(TagData.Length - 1) > 6 Then
                                cmdContent = "~/img/MeterType/other/other2-Leave.jpg"
                            Else
                                cmdContent = "~/img/MeterType/other/other2-On.jpg"
                            End If
                    End Select

                Else
                    Select Case cmdContent
                        Case 1
                            cmdContent = "~/img/MeterType/AP40/AP40.jpg"
                        Case 2
                            cmdContent = "~/img/MeterType/DM2436/DM2436.jpg"
                        Case 3
                            cmdContent = "~/img/MeterType/CT-1700/CT-1700.jpg"
                        Case 4
                            cmdContent = "~/img/MeterType/CT713P/CT713P.png"
                        Case 5
                            cmdContent = "~/img/MeterType/SPM-8/SPM-8.jpg"
                        Case 6
                            cmdContent = "~/img/MeterType/PM710/PM710.gif"
                        Case 7
                            cmdContent = "~/img/MeterType/other/other1.jpg"
                        Case Else
                            cmdContent = "~/img/MeterType/other/other2.jpg"
                    End Select

                End If

                n.ImageUrl = cmdContent
        End Select
    End Sub
#End Region
End Class
