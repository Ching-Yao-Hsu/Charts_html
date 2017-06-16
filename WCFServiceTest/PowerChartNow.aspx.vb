Imports System.Data
Imports System.Data.OleDb

Partial Class PowerChart
    Inherits AccountAdmin

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Session.Abandon()
            Session.Clear()
            Dim msg As String = "連線逾時，請重新登入"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            If Not IsPostBack Then
                Item_CBList.Items.Add(New ListItem("實功", "W"))
                Item_CBList.Items.Add(New ListItem("虛功", "V_ar"))
                Item_CBList.Items.Add(New ListItem("視在", "VA"))
                'For i = 0 To 2
                '    Item_CBList.Items(i).Selected = True
                'Next
                Item_CBList.Items(0).Selected = True

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
            End If

            SqlDataSource3.SelectParameters.Clear()
            Dim account As String = Find_AdAccount(Request.QueryString("group"))
            Dim ctrlnr As String = Request.QueryString("ctrlnr")
            Dim meterid As String = Request.QueryString("meterid")

            Dim strcon As String = Nothing
            If Session("Rank") = 2 Then
                '切換表頭
                If Session("AccountPower") Is Nothing Then
                    Session("AccountPower") = account
                End If
                '換帳號
                If Session("AccountPower") <> account Then
                    If account IsNot Nothing Then
                        Session("AccountPower") = account
                    End If
                End If
                strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("AccountPower") & ""
            Else
                strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account_admin") & ""
            End If

            If ctrlnr IsNot Nothing And meterid IsNot Nothing Then
                Session("ctrlnr") = Request.QueryString("ctrlnr")
                Session("meterid") = Request.QueryString("meterid")
            End If

            Using conn As New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ECOConnectionString").ToString())
                If conn.State = 0 Then conn.Open()
                Dim sql_C As String = "select * from ControllerSetup where Account = ? and CtrlNr = ?"
                Using cmd As New OleDbCommand(sql_C, conn)
                    If Session("Rank") = 2 Then
                        cmd.Parameters.AddWithValue("?Account", Session("AccountPower"))
                    Else
                        cmd.Parameters.AddWithValue("?Account", Session("Account"))
                    End If
                    cmd.Parameters.AddWithValue("?CtrlNr", Session("ctrlnr"))
                    Using dr As OleDbDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            Session("ECO_Account") = dr("ECO_Account")
                            Dim sql_P As String = "select * from MeterSetup where ECO_Account = ? and CtrlNr = ? and MeterID = ?"
                            Using cmd_P As New OleDbCommand(sql_P, conn)
                                cmd_P.Parameters.AddWithValue("?ECO_Account", Session("ECO_Account"))
                                cmd_P.Parameters.AddWithValue("?CtrlNr", Session("ctrlnr"))
                                cmd_P.Parameters.AddWithValue("?MeterID", Session("meterid"))
                                Using dr_P As OleDbDataReader = cmd_P.ExecuteReader
                                    If dr_P.Read() Then
                                        Session("position") = dr_P("InstallPosition")
                                    End If
                                    Dim start_time As String = Date_txt1.Text & " 00:00:00"
                                    Dim end_time As String = Date_txt2.Text & " 23:59:59"
                                    Try
                                        SqlDataSource3.ConnectionString = strcon
                                        SqlDataSource3.ProviderName = "System.Data.OleDb"
                                        SqlDataSource3.SelectCommand = "select convert(datetime,RecDate) as RecDate,W from PowerRecord " & _
                                        "where CtrlNr= ? and MeterID= ? and RecDate between ? and ? order by RecDate "
                                        SqlDataSource3.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
                                        SqlDataSource3.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
                                        SqlDataSource3.SelectParameters.Add("RecDate", TypeCode.String, start_time)
                                        SqlDataSource3.SelectParameters.Add("RecDate", TypeCode.String, end_time)
                                        Dim dv As DataView = SqlDataSource3.Select(New DataSourceSelectArguments)
                                        If dv.Count > 0 Then
                                            Chart_W.Title.Text = "電力趨勢圖(" & Session("ctrlnr") & "-" & Session("meterid") & " " & Session("position") & ")"
                                            msg.Visible = False
                                            Chart_V.Visible = False
                                            Chart_I.Visible = False
                                            Chart_W.Visible = True
                                            Chart_Mode.Visible = False
                                            ConvertPng_btn.Visible = True
                                        Else
                                            msg.Visible = True
                                            Chart_V.Visible = False
                                            Chart_I.Visible = False
                                            Chart_W.Visible = False
                                            Chart_Mode.Visible = False
                                            ConvertPng_btn.Visible = False
                                        End If
                                    Catch ex As Exception
                                        ex.ToString()
                                    End Try
                                End Using
                            End Using
                        Else
                            msg.Visible = True
                            Chart_V.Visible = False
                            Chart_I.Visible = False
                            Chart_W.Visible = False
                            Chart_Mode.Visible = False
                            ConvertPng_btn.Visible = False
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    Protected Sub Item_DDList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Item_DDList.SelectedIndexChanged
        Item_CBList.Items.Clear()
        Demand_CBList.Items.Clear()
        W_CBList.Items.Clear()
        If Item_DDList.SelectedValue = "電壓" Then
            Demand_CBList.Visible = False
            Mode.Visible = False
            Demand.Visible = False
            W_CBList.Visible = False
            Item_CBList.Items.Add(New ListItem("R相", "V1"))
            Item_CBList.Items.Add(New ListItem("S相", "V2"))
            Item_CBList.Items.Add(New ListItem("T相", "V3"))
            Item_CBList.Items.Add(New ListItem("平均電壓", "Vavg"))
            For i = 0 To 3
                Item_CBList.Items(i).Selected = True
            Next
        ElseIf Item_DDList.SelectedValue = "電流" Then
            Demand_CBList.Visible = False
            Mode.Visible = False
            Demand.Visible = False
            W_CBList.Visible = False
            Item_CBList.Items.Add(New ListItem("R相", "I1"))
            Item_CBList.Items.Add(New ListItem("S相", "I2"))
            Item_CBList.Items.Add(New ListItem("T相", "I3"))
            Item_CBList.Items.Add(New ListItem("平均電流", "Iavg"))
            For i = 0 To 3
                Item_CBList.Items(i).Selected = True
            Next
        ElseIf Item_DDList.SelectedValue = "功率" Then
            Demand_CBList.Visible = False
            Mode.Visible = False
            Demand.Visible = False
            W_CBList.Visible = False
            Item_CBList.Items.Add(New ListItem("實功", "W"))
            Item_CBList.Items.Add(New ListItem("虛功", "V_ar"))
            Item_CBList.Items.Add(New ListItem("視在", "VA"))
            'For i = 0 To 2
            '    Item_CBList.Items(i).Selected = True
            'Next
            Item_CBList.Items(0).Selected = True
        ElseIf Item_DDList.SelectedValue = "需量" Then
            Demand_CBList.Visible = True
            Mode.Visible = True
            Demand.Visible = True
            W_CBList.Visible = True
            Item_CBList.Items.Add(New ListItem("模式1", "Mode1"))
            Item_CBList.Items.Add(New ListItem("模式2", "Mode2"))
            Item_CBList.Items.Add(New ListItem("模式3", "Mode3"))
            Item_CBList.Items.Add(New ListItem("模式4", "Mode4"))
            Demand_CBList.Items.Add(New ListItem("尖峰", "DeMand"))
            Demand_CBList.Items.Add(New ListItem("半尖峰", "DeMandHalf"))
            Demand_CBList.Items.Add(New ListItem("週六半尖峰", "DeMandSatHalf"))
            Demand_CBList.Items.Add(New ListItem("離峰", "DeMandOff"))
            W_CBList.Items.Add(New ListItem("實功", "W"))
            'For i = 0 To 4
            '    Item_CBList.Items(i).Selected = True
            'Next
        End If

    End Sub

    Protected Sub submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_btn.Click
        SqlDataSource1.SelectParameters.Clear()
        SqlDataSource2.SelectParameters.Clear()
        SqlDataSource3.SelectParameters.Clear()
        SqlDataSource4.SelectParameters.Clear()
        SqlDataSource1.Dispose()
        SqlDataSource2.Dispose()
        SqlDataSource3.Dispose()
        SqlDataSource4.Dispose()

        Dim strcon As String = Nothing
        If Session("Rank") = 2 Then
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("AccountPower") & ""
        Else
            strcon = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Session("Account") & ""
        End If

        'Dim ctrlnr As String = Request.QueryString("ctrlnr")
        'Dim meterid As String = Request.QueryString("meterid")
        Dim start_time As String = Date_txt1.Text & " " & begin_hh.SelectedValue & ":00:00"
        Dim end_time As String = Date_txt2.Text & " " & end_hh.SelectedValue & ":59:59"
        '判斷勾選
        Dim item_V() As String = {"V1", "V2", "V3", "Vavg"}
        Dim item_I() As String = {"I1", "I2", "I3", "Iavg"}
        Dim item_W() As String = {"W", "V_ar", "VA"}
        Dim item_Mode() As String = {"Mode1", "Mode2", "Mode3", "Mode4"}
        Dim item_DeMand() As String = {"DeMand", "DeMandHalf", "DeMandSatHalf", "DeMandOff"}
        Dim SelectItem As String = Nothing

        If Item_DDList.SelectedValue = "電壓" Then
            For i = 0 To 3
                If Item_CBList.Items(i).Selected = True Then
                    SelectItem &= item_V(i) & ","
                End If
            Next
            Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
            '時間間隔
            If interval_DDList.SelectedValue = "1" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
            ElseIf interval_DDList.SelectedValue = "5" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "30" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "60" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
            End If

            Try
                SqlDataSource1.ConnectionString = strcon
                SqlDataSource1.ProviderName = "System.Data.OleDb"
                SqlDataSource1.SelectCommand = sql
                SqlDataSource1.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
                SqlDataSource1.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
                SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, start_time)
                SqlDataSource1.SelectParameters.Add("RecDate", TypeCode.String, end_time)
                Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)   '資料筆數
                If dv.Count > 0 Then
                    Chart_V.Title.Text = "電力趨勢圖(" & Session("ctrlnr") & "-" & Session("meterid") & " " & Session("position") & ")"
                    msg.Visible = False
                    Chart_V.Visible = True
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    ConvertPng_btn.Visible = True
                Else
                    msg.Visible = True
                    Chart_V.Visible = False
                    Chart_I.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    ConvertPng_btn.Visible = False
                End If
            Catch ex As Exception
                ex.ToString()
            End Try

        ElseIf Item_DDList.SelectedValue = "電流" Then
            For i = 0 To 3
                If Item_CBList.Items(i).Selected = True Then
                    SelectItem &= item_I(i) & ","
                End If
            Next

            Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
            '時間間隔ˇ
            If interval_DDList.SelectedValue = "1" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
            ElseIf interval_DDList.SelectedValue = "5" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "30" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "60" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
            End If

            Try
                SqlDataSource2.ConnectionString = strcon
                SqlDataSource2.ProviderName = "System.Data.OleDb"
                SqlDataSource2.SelectCommand = sql
                SqlDataSource2.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
                SqlDataSource2.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
                SqlDataSource2.SelectParameters.Add("RecDate", TypeCode.String, start_time)
                SqlDataSource2.SelectParameters.Add("RecDate", TypeCode.String, end_time)
                Dim dv As DataView = SqlDataSource2.Select(New DataSourceSelectArguments)
                If dv.Count > 0 Then
                    Chart_I.Title.Text = "電力趨勢圖(" & Session("ctrlnr") & "-" & Session("meterid") & " " & Session("position") & ")"
                    msg.Visible = False
                    Chart_I.Visible = True
                    Chart_V.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    ConvertPng_btn.Visible = True
                Else
                    msg.Visible = True
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    Chart_W.Visible = False
                    Chart_Mode.Visible = False
                    ConvertPng_btn.Visible = False
                End If
            Catch ex As Exception
                ex.ToString()
            End Try

        ElseIf Item_DDList.SelectedValue = "功率" Then
            For i = 0 To 2
                If Item_CBList.Items(i).Selected = True Then
                    SelectItem &= item_W(i) & ","
                End If
            Next

            Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
            '時間間隔
            If interval_DDList.SelectedValue = "1" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
            ElseIf interval_DDList.SelectedValue = "5" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "30" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "60" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
            End If

            Try
                SqlDataSource3.ConnectionString = strcon
                SqlDataSource3.ProviderName = "System.Data.OleDb"
                SqlDataSource3.SelectCommand = sql
                SqlDataSource3.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
                SqlDataSource3.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
                SqlDataSource3.SelectParameters.Add("RecDate", TypeCode.String, start_time)
                SqlDataSource3.SelectParameters.Add("RecDate", TypeCode.String, end_time)
                Dim dv As DataView = SqlDataSource3.Select(New DataSourceSelectArguments)
                If dv.Count > 0 Then
                    Chart_W.Title.Text = "電力趨勢圖(" & Session("ctrlnr") & "-" & Session("meterid") & " " & Session("position") & ")"
                    msg.Visible = False
                    Chart_W.Visible = True
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    Chart_Mode.Visible = False
                    ConvertPng_btn.Visible = True
                Else
                    msg.Visible = True
                    Chart_W.Visible = False
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    Chart_Mode.Visible = False
                    ConvertPng_btn.Visible = False
                End If
            Catch ex As Exception
                ex.ToString()
            End Try

        ElseIf Item_DDList.SelectedValue = "需量" Then
            For i = 0 To 3
                If Item_CBList.Items(i).Selected = True Then
                    SelectItem &= item_Mode(i) & ","
                End If
            Next
            For i = 0 To 3
                If Demand_CBList.Items(i).Selected = True Then
                    SelectItem &= item_DeMand(i) & ","
                End If
            Next
            If W_CBList.Items(0).Selected = True Then
                SelectItem &= item_W(0) & ","
            End If

            Dim sql As String = "select convert(datetime,RecDate) as RecDate," & SelectItem.Substring(0, SelectItem.Length - 1) & " from PowerRecord "
            '時間間隔
            If interval_DDList.SelectedValue = "1" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? order by RecDate "
            ElseIf interval_DDList.SelectedValue = "5" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,16,1) Like '%5%' or substring(recdate,16,1) Like '%0%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "30" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and (substring(recdate,15,2) Like '%00%' or substring(recdate,15,2) Like '%30%') order by RecDate "
            ElseIf interval_DDList.SelectedValue = "60" Then
                sql &= " WHERE CtrlNr = ? AND MeterID = ? AND RecDate between ? and ? and substring(recdate,15,2) Like '%00%' order by RecDate "
            End If

            Try
                SqlDataSource4.ConnectionString = strcon
                SqlDataSource4.ProviderName = "System.Data.OleDb"
                SqlDataSource4.SelectCommand = sql
                SqlDataSource4.SelectParameters.Add("CtrlNr", TypeCode.String, Session("ctrlnr"))
                SqlDataSource4.SelectParameters.Add("MeterID", TypeCode.String, Session("meterid"))
                SqlDataSource4.SelectParameters.Add("RecDate", TypeCode.String, start_time)
                SqlDataSource4.SelectParameters.Add("RecDate", TypeCode.String, end_time)
                Dim dv As DataView = SqlDataSource4.Select(New DataSourceSelectArguments)
                If dv.Count > 0 Then
                    Chart_Mode.Title.Text = "電力趨勢圖(" & Session("ctrlnr") & "-" & Session("meterid") & " " & Session("position") & ")"
                    msg.Visible = False
                    Chart_Mode.Visible = True
                    Chart_W.Visible = False
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    ConvertPng_btn.Visible = True
                Else
                    msg.Visible = True
                    Chart_Mode.Visible = False
                    Chart_W.Visible = False
                    Chart_I.Visible = False
                    Chart_V.Visible = False
                    ConvertPng_btn.Visible = False
                End If
            Catch ex As Exception
                ex.ToString()
            End Try
        End If
    End Sub
End Class
