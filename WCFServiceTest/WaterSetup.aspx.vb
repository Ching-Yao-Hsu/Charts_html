Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.IO
Imports System.Net.Mail
Imports System.Net

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim sFtype As String = Request.QueryString("Ftype")
            txtFtype.Text = sFtype
            If sFtype = "U" Then
                Dim sID As String = Request.QueryString("sID")

                Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=Water"
                Using conn As New OleDbConnection(strcon)
                    If conn.State = 0 Then conn.Open()
                    Dim strSQL As String = "SELECT ID, IP, Port, MeterID, 圖面編號, 店鋪編號, 店鋪名稱, RecDate, 水表值 FROM Setup WHERE ID=" & sID
                    Using cmd As New OleDbCommand(strSQL, conn)
                        Using dr As OleDbDataReader = cmd.ExecuteReader
                            While (dr.Read() = True)
                                txtID.Text = dr("ID").ToString : txtID.ReadOnly = True : txtID.Enabled = False
                                txtIP.Text = dr("IP").ToString : txtIP.ReadOnly = True : txtIP.Enabled = False
                                txtPort.Text = dr("Port").ToString : txtPort.ReadOnly = True : txtPort.Enabled = False
                                txtMeterID.Text = dr("MeterID").ToString : txtMeterID.ReadOnly = True : txtMeterID.Enabled = False
                                txtPhotoNo.Text = dr("圖面編號").ToString : txtPhotoNo.ReadOnly = True : txtPhotoNo.Enabled = False
                                txtStoreID.Text = dr("店鋪編號").ToString : txtStoreID.ReadOnly = True : txtStoreID.Enabled = False
                                txtStoreName.Text = dr("店鋪名稱").ToString
                            End While
                        End Using
                    End Using
                End Using
            End If
        End If
    End Sub

    Protected Sub Submit_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit_btn.Click
        Try
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ToString() & ";Initial Catalog=Water"
            Using conn As New OleDbConnection(strcon)
                If conn.State = 0 Then conn.Open()
                Dim strSQL As String
                Dim msg As String = ""
                If txtFtype.Text = "I" Then
                    strSQL = " Insert Into Setup (IP, Port, MeterID, 圖面編號, 店鋪編號, 店鋪名稱) values (?,?,?,?,?,?)"

                    Using cmd3 As New OleDbCommand(strSQL, conn)
                        cmd3.Parameters.AddWithValue("?IP", txtIP.Text)
                        cmd3.Parameters.AddWithValue("?Port", txtPort.Text)
                        cmd3.Parameters.AddWithValue("?MeterID", txtMeterID.Text)
                        cmd3.Parameters.AddWithValue("?圖面編號", txtPhotoNo.Text)
                        cmd3.Parameters.AddWithValue("?店鋪編號", txtStoreID.Text)
                        cmd3.Parameters.AddWithValue("?店鋪名稱", txtStoreName.Text)

                        cmd3.ExecuteReader()
                        msg = "新增成功!"
                    End Using
                ElseIf txtFtype.Text = "U" Then
                    strSQL = "UPDATE Setup SET 店鋪名稱 = ? WHERE ID = ?"
                    Using cmd3 As New OleDbCommand(strSQL, conn)
                        cmd3.Parameters.AddWithValue("?店鋪名稱", txtStoreName.Text)
                        cmd3.Parameters.AddWithValue("?ID", txtID.Text)

                        cmd3.ExecuteReader()
                        msg = "更新成功!"
                    End Using
                End If
                Page.ClientScript.RegisterStartupScript(Page.GetType, "FailedGetData", "custom_script('" & msg & "')", True)
            End Using

        Catch ex As Exception
            If txtFtype.Text = "I" Then
                MsgBox("新增失敗" & ex.ToString)
            ElseIf txtFtype.Text = "U" Then
                MsgBox("更新失敗" & ex.Message)
            End If
        End Try

    End Sub

End Class
