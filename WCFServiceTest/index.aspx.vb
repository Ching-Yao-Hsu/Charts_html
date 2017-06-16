Imports System.Data.OleDb
Imports System.Data

Partial Class _Default
    'Inherits TreeSetup
    Inherits ObjectBuilding



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                If Request.QueryString.Count > 0 Then
                    Session("language") = Request.QueryString(0).ToString
                End If


                Dim strPerNum As String = System.Web.Configuration.WebConfigurationManager.AppSettings("bPerNum")
                If strPerNum = "1" Then
                    '//群組下拉是選單
                    If Session("PerNum") Is Nothing Then
                        If Request("PerNum") Is Nothing Then
                            Session("PerNum") = 1
                        Else
                            Session("PerNum") = Request("PerNum")
                        End If
                    Else
                        If Not (Request("PerNum") Is Nothing) Then
                            If Request("PerNum") <> Session("PerNum") Then
                                Session("PerNum") = Request("PerNum")
                                Session("Group01") = Nothing
                            End If
                        End If
                    End If
                End If

                Dim iSelIndex As Integer = 0
                BuildingDropDownList(Group_DropDownList, iSelIndex)
                'Group_DropDownList.SelectedIndex = Group_DropDownList.Items.IndexOf(Group_DropDownList.Items(iSelIndex))
                'Group_DropDownList.Items(iSelIndex).Selected = True
                If Session("Account") = "admin" Then    '系統管理者
                    If Not (Session("Group01") Is Nothing) Then
                        BuildingTree(Meter_TreeView, Session("Group01"), CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)
                    End If
                Else
                    Account_DDList_SelectedIndexChanged(Nothing, Nothing)

                    Dim cmdIndex(3) As Boolean
                    cmdIndex(0) = CtrlNr_CB.Checked
                    cmdIndex(1) = MeterId_CB.Checked
                    cmdIndex(2) = True
                    cmdIndex(3) = LineNum_CB.Checked
                    Position_CB.Checked = True
                    For Each node As TreeNode In Meter_TreeView.Nodes
                        Call ModifyNode(node, 1, cmdIndex, )
                    Next
                End If
            End If

            If Me.Meter_TreeView.SelectedNode IsNot Nothing Then
                Meter_TreeView_SelectedNodeChanged(Nothing, Nothing)
            End If
        End If

    End Sub

    Protected Sub Account_DDList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Group_DropDownList.SelectedIndexChanged
        Dim account As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        Session("Group01") = account

        BuildingTree(Meter_TreeView, account, CtrlNr_CB.Checked, MeterId_CB.Checked, LineNum_CB.Checked, Position_CB.Checked, 1)

    End Sub

    Protected Sub Meter_TreeView_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Meter_TreeView.SelectedNodeChanged
        node_temp.Text = Meter_TreeView.SelectedNode.Text
        If Meter_TreeView.SelectedNode.Text <> "尚未定義電表" Or Meter_TreeView.SelectedNode.Text <> "The meter has not been defined." Then
            Dim node_value As String = Meter_TreeView.SelectedNode.ToolTip
            If node_value <> "" Then
                Dim value() As String = node_value.Split(vbCrLf)
                Dim account As String = value(0).Split(":").GetValue(1)
                Dim eco_account As String = value(1).Split(":").GetValue(1)
                Dim ctrl As String = value(2).Split(":").GetValue(1)
                Dim meter As String = value(3).Split(":").GetValue(1)
                show(account, eco_account, ctrl, meter)

                Session("MEco_Account") = eco_account
                Session("Ctrl") = ctrl
                Session("meter") = meter
            End If
        End If
    End Sub

    Protected Sub show(ByVal account As String, ByVal eco_account As String, ByVal ctrl As String, ByVal meter As String)
        id.Text = ctrl & "-" & meter
        'Dim sSelGroup As String = Find_AdAccount(Group_DropDownList.SelectedValue)
        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & sSelGroup & ""
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & account & ""

        Dim sql As String = " select top 1 PR.* from PowerRecord PR with (Nolock) " & _
                            " Left join ECOSMART.dbo.ControllerSetup CS  with (Nolock) On Account='" & account & "' and CS.CTrlNr=PR.CtrlNr " & _
                            " where PR.CtrlNr = " & ctrl & " and PR.MeterID = " & meter & _
                            " and (PR.recdate + PR.RecTime) >= ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + CONVERT(NVARCHAR, dateAdd(n,ISNULL((CS.DiffTime *60 -6) ,0),getdate()), 108)) " & _
                            " order by PR.Recdate desc, PR.RecTime desc"
        Using conn As New OleDbConnection(strcon)
            Using cmd As New OleDbCommand(sql, conn)
                If conn.State = 0 Then conn.Open()
                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        RecDate.Text = dr("RecDate").ToString
                        I1.Text = dr("I1").ToString
                        I2.Text = dr("I2").ToString
                        I3.Text = dr("I3").ToString
                        Iavg.Text = dr("Iavg").ToString
                        V1.Text = dr("V1").ToString
                        V2.Text = dr("V2").ToString
                        V3.Text = dr("V3").ToString
                        Vavg.Text = dr("Vavg").ToString
                        W.Text = dr("W").ToString
                        V_ar.Text = dr("V_ar").ToString
                        VA.Text = dr("VA").ToString
                        PF.Text = dr("PF").ToString
                        KWH.Text = dr("KWh").ToString
                    Else
                        RecDate.Text = "--"
                        V1.Text = ""
                        V2.Text = ""
                        V3.Text = ""
                        Vavg.Text = ""
                        I1.Text = ""
                        I2.Text = ""
                        I3.Text = ""
                        Iavg.Text = ""
                        W.Text = ""
                        V_ar.Text = ""
                        VA.Text = ""
                        PF.Text = ""
                        KWH.Text = ""
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        If CtrlNr_CB.Checked = False And MeterId_CB.Checked = False And Position_CB.Checked = False And LineNum_CB.Checked = False Then
            Account_DDList_SelectedIndexChanged(Nothing, Nothing)
        Else
            Dim cmdIndex(3) As Boolean
            cmdIndex(0) = CtrlNr_CB.Checked
            cmdIndex(1) = MeterId_CB.Checked
            cmdIndex(2) = Position_CB.Checked
            cmdIndex(3) = LineNum_CB.Checked

            For Each node As TreeNode In Meter_TreeView.Nodes
                Call ModifyNode(node, 1, cmdIndex, )
            Next
        End If
    End Sub
End Class
