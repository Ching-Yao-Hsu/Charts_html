<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WCFService_Test.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .loaingpage {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .8) url('http://i.stack.imgur.com/FhHRx.gif') 50% 50% no-repeat;
        }
        
        body.loading {
            overflow: hidden;
        }
        
        body.loading .loaingpage {
            display: block;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>

    <script>

        
    </script>

    <script>
        $(document).ready(function () {
            $.ajax({
                url: "WebForm3.aspx",
                dataType: "json",
                type: "POST",
                success: function (e) {
                    console.log(e);
                },
                error: function () {
                    alert("error");
                }
            });
        });



        $body = $("body");

        $(document).on({
            ajaxStart: function () {
                $body.addClass("loading");
            },
            ajaxStop: function () {
                $body.removeClass("loading");
            }
        });

        







    </script>



</head>
<body>
    <form id="form1" runat="server">
        


        <div></div>










    </form>
     <div class="loaingpage">
        <!-- Place at bottom of page -->
     </div>
</body>   
</html>
