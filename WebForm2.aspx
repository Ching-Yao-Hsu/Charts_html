<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Test.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $.ajax({
                url: "WebForm1.aspx",
                dataType: "json",
                success: function (e) {
                    $(e).each(function () {
                        //var c = "<div>" + this.s_name + "</div>"
                        //c += "<div>" + this.s_phone + "</div>"
                        //c += "<div>" + this.s_mail + "</div>"
                        //c += "<div>" + this.s_addr + "</div>"
                        //$(".container").append(c);                        
                        console.log(e.name);
                    });
                },
                error: function () {
                    alert("error");
                }
            })
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container"></div>
    </form>
</body>
</html>
