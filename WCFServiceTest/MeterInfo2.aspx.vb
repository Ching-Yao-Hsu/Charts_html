Imports System.Data
Imports System.Data.OleDb

Partial Class MeterInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Rank") <> 2 Then
            Response.Redirect("home.aspx")
        Else
            If Not IsPostBack Then
                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
                Dim conn As OleDbConnection = New OleDbConnection(strcon)
                conn.Open()
                Dim strSQL As String = "select Account from AdminSetup where Enabled = 1"
                Dim cmd As OleDbCommand = New OleDbCommand(strSQL, conn)
                Dim dr As OleDbDataReader = cmd.ExecuteReader()
                Account_DDList.Items.Add("請選擇")
                While (dr.Read() = True)
                    If dr("Account") <> "admin" Then
                        Dim item As String = dr("Account")
                        Account_DDList.Items.Add(item)
                    End If
                End While
                Account_DDList.Items(0).Selected = True

                cmd.Dispose()
                dr.Close()
                conn.Close()
            End If
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()
        SqlDataSource1.SelectParameters.Clear()

        If Account_DDList.SelectedValue = "請選擇" Then
            SqlDataSource1.SelectCommand = "SELECT ECO_Account,CtrlNr,MeterID,DrawNr,InstallPosition,Enabled FROM MeterSetup"
        ElseIf Account_DDList.SelectedValue <> "請選擇" And EcoAccount_DDList.SelectedValue = "請選擇" Then
            SqlDataSource1.SelectCommand = "SELECT MeterSetup.ECO_Account,MeterSetup.CtrlNr,MeterSetup.MeterID,MeterSetup.DrawNr,MeterSetup.InstallPosition,MeterSetup.Enabled FROM MeterSetup,ControllerSetup " & _
                                           "WHERE ControllerSetup.Account= ? and ControllerSetup.ECO_Account = MeterSetup.ECO_Account"
            SqlDataSource1.SelectParameters.Add("ControllerSetup.Account", TypeCode.String, Account_DDList.SelectedValue)
        Else
            SqlDataSource1.SelectCommand = "SELECT ECO_Account,CtrlNr,MeterID,DrawNr,InstallPosition,Enabled FROM MeterSetup WHERE ECO_Account = ?"
            SqlDataSource1.SelectParameters.Add("ECO_Account", TypeCode.String, EcoAccount_DDList.SelectedValue)
        End If
    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        SqlQuery()
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate Then
                Dim Image As Image = TryCast(e.Row.FindControl("enabled"), Image)
                Dim drv As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                Dim type As Integer = Convert.ToInt32(drv("Enabled").ToString())
                If type = 1 Then
                    Image.ImageUrl = "img\on.png"
                Else
                    Image.ImageUrl = "img\off.png"
                End If
            End If
        End If
    End Sub

    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        SqlDataSource1.UpdateParameters.Clear()
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()

        Dim eco_account, ctrlnr, meterid, drawnr, position, enabled As String
        eco_account = GridView1.DataKeys(e.RowIndex).Values(0)
        ctrlnr = GridView1.DataKeys(e.RowIndex).Values(1)
        meterid = GridView1.DataKeys(e.RowIndex).Values(2)
        drawnr = e.NewValues("DrawNr")
        position = e.NewValues("InstallPosition")
        enabled = e.NewValues("Enabled")

        SqlDataSource1.UpdateCommand = "UPDATE MeterSetup SET DrawNr = ?, InstallPosition = ?, Enabled = ? WHERE ECO_Account = ? AND CtrlNr = ? AND MeterID = ? "
        SqlDataSource1.UpdateParameters.Add("DrawNr", TypeCode.String, drawnr)
        SqlDataSource1.UpdateParameters.Add("InstallPosition", TypeCode.String, position)
        SqlDataSource1.UpdateParameters.Add("Enabled", TypeCode.String, enabled)
        SqlDataSource1.UpdateParameters.Add("ECO_Account", TypeCode.String, eco_account)
        SqlDataSource1.UpdateParameters.Add("CtrlNr", TypeCode.String, ctrlnr)
        SqlDataSource1.UpdateParameters.Add("MeterID", TypeCode.String, meterid)

        SqlDataSource1.Update()
        SqlDataSource1.Dispose()

        Dim msg As String = "更新成功！"
        Dim script As String = "<script>alert('" & msg & "');</script>"
        Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)

    End Sub

    'Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

    '    Dim eco_account As String = GridView1.DataKeys(e.RowIndex)("ECO_Account").ToString
    '    Dim ctrlnr As String = GridView1.DataKeys(e.RowIndex)("CtrlNr").ToString
    '    Dim meterid As String = GridView1.DataKeys(e.RowIndex)("MeterID").ToString

    '    SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
    '    SqlDataSource1.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ProviderName()
    '    SqlDataSource1.DeleteCommand = "Delete from MeterSetup WHERE ECO_Account = '" & eco_account & "' AND CtrlNr = '" & ctrlnr & "' AND MeterID = '" & meterid & "'"
    '    SqlDataSource1.Delete()
    '    SqlDataSource1.DataBind()
    '    SqlDataSource1.Dispose()
    '    Dim msg As String = "删除成功！"
    '    Dim script As String = "<script>alert('" & msg & "');</script>"
    '    Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", script)
    'End Sub

    Protected Sub Account_DDList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Account_DDList.SelectedIndexChanged
        If EcoAccount_DDList.Items.Count <> 0 Then
            EcoAccount_DDList.Items.Clear()
        End If

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString()
        Dim conn As OleDbConnection = New OleDbConnection(strcon)
        conn.Open()
        Dim strSQL As String = "select ECO_Account from ControllerSetup where Account = ? and Enabled = 1"
        Dim cmd As OleDbCommand = New OleDbCommand(strSQL, conn)
        cmd.Parameters.AddWithValue("?Account", Account_DDList.SelectedValue)
        Dim dr As OleDbDataReader = cmd.ExecuteReader()
        EcoAccount_DDList.Items.Add("請選擇")
        While (dr.Read() = True)
            Dim item As String = dr("ECO_Account")
            EcoAccount_DDList.Items.Add(item)
        End While
        EcoAccount_DDList.Items(0).Selected = True

        cmd.Dispose()
        dr.Close()
        conn.Close()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        SqlQuery()
    End Sub

    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        GridView1.EditIndex = e.NewEditIndex
        SqlQuery()
    End Sub

    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        'ValidationSummary1.Visible = False
        GridView1.EditIndex = -1
        SqlQuery()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        SqlQuery()
    End Sub
End Class
