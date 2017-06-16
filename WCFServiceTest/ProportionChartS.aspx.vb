Imports System.Data
Imports System.Data.OleDb

Partial Class ProportionChartS
    Inherits AccountAdmin

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Account") Is Nothing Then
            Dim msg As String = "連線逾時，請重新登入(Connection timed out, please sign in again.)"
            Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
        Else
            SqlQuery()
        End If
    End Sub

    Protected Sub SqlQuery()
        Dim group As String = Request.QueryString("group")
        Dim node As String = Request.QueryString("node")
        Dim datetime As String = Request.QueryString("datetime")
        Dim value As String = Request.QueryString("value")

        'Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=ECO_" & Find_AdAccount(group) & ""
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("MasterConn").ToString()

        Dim title() As String = node.Split(",")
        Dim check_count_c, check_count_m, check_count_p As Integer
        Dim ctrlnr((title.Length / 5) - 1) As String
        Dim meterid((title.Length / 5) - 1) As String
        Dim position((title.Length / 5) - 1) As String

        For i = 0 To title.Length - 1
            If title(i).Split(":").GetValue(0) = "ECO5編號" Then
                ctrlnr(check_count_c) = title(i).Split(":").GetValue(1)
                check_count_c += 1
            ElseIf title(i).Split(":").GetValue(0) = "電表編號" Then
                meterid(check_count_m) = title(i).Split(":").GetValue(1)
                check_count_m += 1
            ElseIf title(i).Split(":").GetValue(0) = "安裝位置" Then
                position(check_count_p) = title(i).Split(":").GetValue(1)
                check_count_p += 1
            End If
        Next


        Dim values() As String = value.Split(",")
        Dim item(values.Length - 1) As String
        For n = 0 To values.Length - 1
            item(n) = values(n).Split(" ").GetValue(2)
        Next
        Dim sql As String = ""
        For i = 0 To check_count_m
            If i <> check_count_m Then
                sql &= "SELECT TOP 1 " & value & " into #T" & i + 1 & " FROM ECO_" & Find_AdAccount(group) & "_PowerRecordCollection " & _
                "WHERE CtrlNr = " & ctrlnr(i) & " AND MeterID = " & meterid(i) & " AND RecDate = '" & datetime & "' order by RecDate desc;"
            Else
                sql &= "SELECT "
                For j = 0 To item.Length - 1
                    For k = 0 To check_count_m - 1
                        sql &= "#T" & k + 1 & "." & item(j) & "+"
                    Next
                    sql = sql.Substring(0, sql.Length - 1)
                    If j <> item.Length Then
                        sql &= " AS " & item(j) & ","
                    End If
                Next
                sql = sql.Substring(0, sql.Length - 1)

                sql &= " FROM "
                For k = 0 To check_count_m - 1
                    sql &= "#T" & k + 1 & ","
                Next
                sql = sql.Substring(0, sql.Length - 1)

                'If check_count_m > 1 Then
                '    sql &= " WHERE "
                '    For k = 0 To check_count_m - 2
                '        sql &= "#T" & k + 1 & ".RecDate = #T" & k + 2 & ".RecDate and "
                '    Next
                '    sql = sql.Substring(0, sql.Length - 4)
                'End If

                sql &= ";Drop table "
                For k = 0 To check_count_m - 1
                    sql &= "#T" & k + 1 & ","
                Next
                sql = sql.Substring(0, sql.Length - 1)
            End If
        Next

        SqlDataSource1.ConnectionString = strcon
        SqlDataSource1.ProviderName = "System.Data.OleDb"
        SqlDataSource1.SelectCommand = sql
        Dim dv As DataView = SqlDataSource1.Select(New DataSourceSelectArguments)
        Dim dt As DataTable = dv.ToTable()

        Try
            If dt.Rows.Count > 0 Then
                Dim positions As String = ""
                For i = 0 To check_count_m - 1
                    positions &= position(i) & ","
                Next
                positions = positions.Substring(0, positions.Length - 1)
                If Session("language").ToString = "en" Then
                    PieChart.Title.Text = "Electricity Proportional Chart(" & positions & ")"
                Else
                    PieChart.Title.Text = "電量比重圖(" & positions & ")"
                End If
                PieChart.Visible = True
                Dim dt2 As DataTable = New DataTable
                dt2.Columns.Add("Items")
                dt2.Columns.Add("Values")
                For i = 0 To dt.Columns.Count - 1
                    dt2.Rows.Add(dt.Columns(i).ColumnName.ToString, dt.Rows(0).Item(i))
                Next

                PieChart.DataSource = dt2
                PieChart.DataBind()
                PieChart.Visible = True
                img_btn.Visible = True
            Else
                PieChart.Visible = False
                img_btn.Visible = False
                Dim msg As String = "查無資料"
                If Session("language").ToString = "en" Then
                    msg = " No Data. "
                End If
                Dim script As String = "<script>alertify.alert('" & msg & "');</script>"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class
