<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WCFService_Test.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
<<<<<<< HEAD
        /* Start by setting display:none to make this hidden.
   Then we position it in relation to the viewport window
   with position:fixed. Width, height, top and left speak
   for themselves. Background we set to 80% white with
   our animation centered, and no-repeating */
        .modal {
=======
        .loaingpage {
>>>>>>> d75c8ff7a50c840fa434e03448f6a5e980b733d7
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
<<<<<<< HEAD
            background: rgba( 255, 255, 255, .8 ) 50% 50% no-repeat;
            background: url(gif/loading.gif);
            background-size:cover;
        }

        /* When the body has the loading class, we turn
   the scrollbar off with overflow:hidden */
        body.loading {
            overflow: hidden;
        }

            /* Anytime the body has the loading class, our
   modal element will be visible */
            body.loading .modal {
                display: block;
            }
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script>
       


        $(document).ready(function () {
            var $body = $("body");
            
            $("#abc").click(function () {
                


                $.ajax({
                    url: "WebForm2.aspx",
                    dataType: "json",
                    type: "POST",
                    success: function (e) {
                        console.log(e);
                        console.log(e[0]["CtrlNr"]);
                        console.log(e[0]["MeterID"]);
                        console.log(e[0]["RecDate"]);
                        console.log(e[0]["RecTime"]);

                        //for (i = 0; i < e.length; i++) {
                        //    $("#go").append(
                        //            $("<tr/>").append(
                        //                $("<td/>")
                        //                    .attr("style", "background-color: white")
                        //                    .text(e[i]["CtrlNr"])
                        //            ).append(
                        //                $("<td/>")
                        //                    .attr("style", "background-color: white")
                        //                    .text(e[i]["MeterID"])
                        //            ).append(
                        //            $("<td/>")
                        //                    .attr("style", "background-color: white")
                        //                    .text(e[i]["RecDate"])
                        //            ).append(
                        //                $("<td/>")
                        //                    .attr("style", "background-color: white")
                        //                    .text(e[i]["RecTime"])
                        //            )
                        //        )
                        //}
                        
                        
                        
                        //CtrlNr
                        //MeterID
                        //RecDate
                        //RecTime
                    },
                    beforeSend: function () {
                        $body.addClass("loading");
                    },
                    complete: function () {
                        $body.removeClass("loading");
                    },
                    error: function () {
                        alert("error");
                    }
                });

                

            });
           
        });


        //$body = $("body");

        //$(document).on({
        //    ajaxStart: function () {  },
        //    ajaxStop: function () {  }
        //});
=======
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

        






>>>>>>> d75c8ff7a50c840fa434e03448f6a5e980b733d7

    </script>


<<<<<<< HEAD
</head>
<body>
    <form id="form1" runat="server">
        <div style="background-color: turquoise; width: 800px;">
            <table id="go" style="width: 100%;">
                <tr>
                    <td style="background-color: white">CtrlNr</td>
                    <td style="background-color: white">MeterID</td>
                    <td style="background-color: white">RecDate</td>
                    <td style="background-color: white">RecTime</td>
                </tr>
            </table>
        </div>



        <a href="#" id="abc">content</a>
    </form>
    <div class="modal">
        <!-- Place at bottom of page -->
    </div>
</body>
=======

</head>
<body>
    <form id="form1" runat="server">
        


        <div></div>










    </form>
     <div class="loaingpage">
        <!-- Place at bottom of page -->
     </div>
</body>   
>>>>>>> d75c8ff7a50c840fa434e03448f6a5e980b733d7
</html>
