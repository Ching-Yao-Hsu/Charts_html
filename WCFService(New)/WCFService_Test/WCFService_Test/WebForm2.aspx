<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WCFService_Test.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<<<<<<< HEAD
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
=======
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        
        var pageIndex = 1;
        var pageCount;

        //**************************************************
        $(window).scroll(function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                GetRecords();   // 計算視窗高度（Scroll Bar）被拉到下面就會啟動！展示更多記錄。
            }
        });
        //**************************************************

        function GetRecords() {
            pageIndex++;
            console.log(pageIndex);
            if (pageIndex == 2 || pageIndex <= pageCount) {
                $("#loader").show();
                $.ajax({
                    type: "POST",
                    url: "WebForm2.aspx/GetCustomers",     // *** 記得修改檔名 ***
                    data: '{pageIndex: ' + pageIndex + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert(response.d);
                    }
                });
            }
        }
        function OnSuccess(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            pageCount = parseInt(xml.find("DT_PageCount").eq(0).find("Field_PageCount").text());
            var customers = xml.find("AdminCom");    // Web Service傳回的XML文件，DataSet裡面的DataTable名稱

            customers.each(function () {
                var customer = $(this);
                var table = $("#dvCustomers table").eq(0).clone(true);

                // Repeater裡面每一筆記錄的「欄位」，都會有一個<span class="...">
                $(".name", table).html(customer.find("Account").text());
                $(".city", table).html(customer.find("Com").text());
                $(".postal", table).html(customer.find("Enabled").text());
                $(".country", table).html(customer.find("EnabledTime").text());
                $(".phone", table).html(customer.find("Account").text());
                $(".fax", table).html(customer.find("Account").text());

                $("#dvCustomers").append(table).append("<br />XXXXXXXXXXXX<br /><br />");
                // 畫出下一個表格<table>
            });
            $("#loader").hide();
        }
    </script>
</head>
<body style="font-family: Arial; font-size: 10pt">    
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <div id="dvCustomers">
                        <!-- ************ -->

                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <table id="MIS2000Lab" border="1" style="width: 280px; height: 100px; border: dashed 2px #04AFEF; background-color: #B0E2F5">
                                    <tr>
                                        <td>流水號：<b><%# Container.ItemIndex + 1 %></b> --
                                <b><u><span class="name"><%# Eval("Account") %></span></u></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>城市: </b><span class="city"><%# Eval("Account") %></span><br />
                                            <b>郵遞區號: </b><span class="postal"><%# Eval("Com") %></span><br />
                                            <b>國別: </b><span class="country"><%# Eval("Enabled")%></span><br />
                                            <b>電話: </b><span class="phone"><%# Eval("EnabledTime")%></span><br />
                                            <b>傳真: </b><span class="fax"><%# Eval("Account")%></span><br />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                        ******************************************************************<br />
                        
                        ******************************************************************<br />
                    </div>
                </td>
                <td valign="bottom">
                    <img id="loader" alt="" src="gif/loading.gif" style="display: none" />
                    <!-- 動畫小圖示，載入資料時會出現 -->
                </td>
            </tr>
        </table>
>>>>>>> d75c8ff7a50c840fa434e03448f6a5e980b733d7
    </form>
</body>
</html>
