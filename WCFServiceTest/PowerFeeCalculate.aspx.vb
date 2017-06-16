Imports System.Data.OleDb

Partial Class PowerFeeCalculate
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
                Date_txt2.Text = Now.ToString("yyyy/MM")
                txtSelf.Enabled = False
                '//群組下拉是選單
                Dim iSelIndex As Integer = 0
                BuildingDropDownList(Group_DropDownList, iSelIndex)
                Group_DropDownList.Items(iSelIndex).Selected = True
            End If
        End If
    End Sub

    '//Protected Sub ViewDetails_btn_Click(sender As Object, e As ImageClickEventArgs) Handles ViewDetails_btn.Click


    '//表燈非營業(Numer電量尖峰.Value + Numer電量半尖峰.Value + Numer電量週六.Value + Numer電量離峰.Value, 月份, Nothing)
    '//Dim oBill As ElectricBillCal.ElectricBill


    'Dim a As Single = oBill.表燈非營業(Numer電量尖峰.Value + Numer電量半尖峰.Value + Numer電量週六.Value + Numer電量離峰.Value,  ListBox1.SelectedIndex + 1, Nothing)

    '//表燈營業用(Numer電量尖峰.Value + Numer電量半尖峰.Value + Numer電量週六.Value + Numer電量離峰.Value, ElectricBill.Month.Jan, Nothing)


    '//表燈時間電價(Numer契約經常.Value, Numer契約半尖峰.Value, Numer契約週六.Value, _
    '//                                                Numer契約離峰.Value, Numer需量經常.Value, Numer需量半尖峰.Value, Numer需量週六.Value, _
    '//                                                Numer需量離峰.Value, Numer電量尖峰.Value, Numer電量半尖峰.Value, Numer電量週六.Value, Numer電量離峰.Value, _
    '//                                                ListBox1.SelectedIndex + 1, Nothing, False)

    '//End Sub
End Class
