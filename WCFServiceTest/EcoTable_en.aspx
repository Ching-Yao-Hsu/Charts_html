<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EcoTable.aspx.vb" Inherits="EcoTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /> 
    <meta name="author" content="Flycan.com" />
    <link type="text/css" href="style.css" rel="stylesheet" /> <!-- the layout css file -->
	<link type="text/css" href="css/jquery.cleditor.css" rel="stylesheet" />
    <script type='text/javascript' src='js/jquery-1.8.2.min.js'></script>	<!-- jquery library -->
	<script type='text/javascript' src='js/jquery-ui-1.8.5.custom.min.js'></script> <!-- jquery UI -->
	<script type='text/javascript' src='js/cufon-yui.js'></script> <!-- Cufon font replacement -->
	<script type='text/javascript' src='js/ColaborateLight_400.font.js'></script> <!-- the Colaborate Light font -->
	<script type='text/javascript' src='js/easyTooltip.js'></script> <!-- element tooltips -->
	<script type='text/javascript' src='js/jquery.tablesorter.min.js'></script> <!-- tablesorter -->
	
	<!--[if IE 8]>
		<script type='text/javascript' src='js/excanvas.js'></script>
		<link rel="stylesheet" href="css/IEfix.css" type="text/css" media="screen" />
	<![endif]--> 
 
	<!--[if IE 7]>
		<script type='text/javascript' src='js/excanvas.js'></script>
		<link rel="stylesheet" href="css/IEfix.css" type="text/css" media="screen" />
	<![endif]--> 
	
	<script type='text/javascript' src='js/visualize.jQuery.js'></script> <!-- visualize plugin for graphs / statistics -->
	<script type='text/javascript' src='js/iphone-style-checkboxes.js'></script> <!-- iphone like checkboxes -->
	<script type='text/javascript' src='js/jquery.cleditor.min.js'></script> <!-- wysiwyg editor -->

	<script type='text/javascript' src='js/custom.js'></script> <!-- the "make them work" script -->
	
	<script type='text/javascript'>
	    function custom_script(sender) {
	        sure = alert(sender);
	    }
	</script>
	
	<script type="text/javascript">
	    var xmlHttp;
	    //程式由此執行
	    function chkAccount() {
	        img_Prc = document.getElementById("img_Prc");
	        img_Prc.style.visibility = 'visible';
	        img_Prc.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var div = document.getElementById("msg");
	        createXMLHttpRequest(); //建立XMLHttpRequest物件
	        var EcoAccount = document.getElementById('<%= Page.FindControl("ECO_Account").ClientID %>').value;
	        if (EcoAccount != '') {
	            var regex1 = /^\w+$/;
	            if (regex1.test(EcoAccount) == true) {
	                var regex2 = /^\w{15}$/;
	                if (regex2.test(EcoAccount) == true) {
//	                    var url = "Verification.aspx?Account=" + EcoAccount + "&type=NewEcoAccount&field=account";
//	                    xmlHttp.onreadystatechange = callback; //資料回傳之後，使用callback這個函數處理後續動作 
//	                    xmlHttp.open("POST", url, true); //將輸入的帳號傳至後端作驗證
//	                    xmlHttp.send(null);
	                    img_Prc.src = "img/success.png";
	                    img_Prc.style.visibility = 'visible';
	                    div.innerHTML = "";
	                } else {
	                    img_Prc.src = "img/error.png";
	                    img_Prc.style.visibility = 'visible';
	                    div.innerHTML = "<font color=red>Account does not match the number of words, character length limit 15.</font>";
	                }
	            } else {
	                img_Prc.src = "img/error.png";
	                img_Prc.style.visibility = 'visible';
	                div.innerHTML = "<font color=red>Account can only contain numbers and English.</font>";
	            }
	        } else {
	            img_Prc.src = "img/error.png";
	            img_Prc.style.visibility = 'visible';
	            div.innerHTML = "<font color=red>Account can not be empty.</font>";
	        }

	    }
	    function chkPassword() {
	        img_Prc2 = document.getElementById("img_Prc2");
	        img_Prc2.style.visibility = 'visible';
	        img_Prc2.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var div2 = document.getElementById("msg2");
	        //createXMLHttpRequest(); //建立XMLHttpRequest物件
	        var password = document.getElementById('<%= Page.FindControl("ECO_Password").ClientID %>').value;
	        if (password != '') {
	            var regex1 = /^\w+$/;
	            if (regex1.test(password) == true) {
	                var regex2 = /^.{6}$/;
	                if (regex2.test(password) == true) {
	                    img_Prc2.src = "img/success.png";
	                    img_Prc2.style.visibility = 'visible';
	                    div2.innerHTML = "";
	                } else {
	                    img_Prc2.src = "img/error.png";
	                    img_Prc2.style.visibility = 'visible';
	                    div2.innerHTML = "<font color=red>Password does not match the number of words, character length limit 6.</font>";
	                }
	            } else {
	                img_Prc2.src = "img/error.png";
	                img_Prc2.style.visibility = 'visible';
	                div2.innerHTML = "<font color=red>Password can only contain numbers and English.</font>";
	            }
	        } else {
	            img_Prc2.src = "img/error.png";
	            img_Prc2.style.visibility = 'visible';
	            div2.innerHTML = "<font color=red>Password can not be empty.</font>";
	        }
	    }

	    function chkDrawnr() {
	        img_Prc4 = document.getElementById("img_Prc4");
	        img_Prc4.style.visibility = 'visible';
	        img_Prc4.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var div4 = document.getElementById("msg4");
	        var Drawnr = document.getElementById('<%= Page.FindControl("DrawNr").ClientID %>').value;
	        if (Drawnr != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
	            if (regex.test(Drawnr) == true) {
	                img_Prc4.src = "img/success.png";
	                img_Prc4.style.visibility = 'visible';
	                div4.innerHTML = "";
	            } else {
	                img_Prc4.src = "img/error.png";
	                img_Prc4.style.visibility = 'visible';
	                div4.innerHTML = "<font color=red>Drawing NO. can only contain English characters or  numbers.</font>";
	            }
	        } else {
	            img_Prc4.style.visibility = 'hidden';
	            div4.innerHTML = "";
	        }  
	    }

	    function chkPosition() {
	        img_Prc5 = document.getElementById("img_Prc5");
	        img_Prc5.style.visibility = 'visible';
	        img_Prc5.src = "img/Processing.jpg"; //顯示處理中的圖片
	        var div5 = document.getElementById("msg5");
	        var Position = document.getElementById('<%= Page.FindControl("InstallPosition").ClientID %>').value;
	        if (Position != '') {
                var regex = /^(\w|[\u4E00-\u9FA5])*$/;
	            if (regex.test(Position) == true) {
	                img_Prc5.src = "img/success.png";
	                img_Prc5.style.visibility = 'visible';
	                div5.innerHTML = "";
	            } else {
	                img_Prc5.src = "img/error.png";
	                img_Prc5.style.visibility = 'visible';
	                div5.innerHTML = "<font color=red>Location can only contain English characters or  numbers.</font>";
	            }
	        } else {
	            img_Prc5.style.visibility = 'hidden';
	            div5.innerHTML = "";
	        } 
	    }

	    function chkAll() {
	        chkAccount();
	        chkPassword();
	        chkDrop();
	    }
	    //xmlHttp.open(a,b,c) 
	    //第一個參數是 HTTP request 的方法，也就是從 GET、POST、HEAD 中選一個使用,都要大寫 
	    //第二個參數是要呼叫的url,不過只能呼叫同一個網域的網頁 
	    //第三個參數決定此 request 是否不同步進行 
	    //如果設定為true則即使後端尚未傳回資料也會繼續執行後面的程式 
	    //如果設定為false則必須等後端傳回資料才會繼續執行後面的程式 

	    //此函式在建立XMLHttpRequest物件 
	    function createXMLHttpRequest() {
	        if (window.ActiveXObject) {//IE 
	            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	        }
	        else if (window.XMLHttpRequest) {//other browser 
	            xmlHttp = new XMLHttpRequest();
	        }
	    }

	    function callback() {
	        if (xmlHttp.readyState == 4) {
	            if (xmlHttp.status == 200) {
	                var xmldoc = xmlHttp.responseXML; //接收後端程式傳回來的xml 
	                var mes = xmldoc.getElementsByTagName("message")[0].firstChild.data; //將Tag 為message的值抓出來
	                var val = xmldoc.getElementsByTagName("passed")[0].firstChild.data; //將Tag 為passed的值抓出來
	                var field = xmldoc.getElementsByTagName("field")[0].firstChild.data; //將Tag 為field的值抓出來
	                setMessage(mes, val, field);
	            }
	        }
	    }
	    //xmlHttp.readyState 所有可能的值： 0 (還沒開始), 1 (讀取中), 2 (已讀取), 3 (資訊交換中), 4 (一切完成) 
	    //xmlHttp.status = 200 (一切正常), status = 404 (查無此頁), status =500 (內部錯誤)

	    function setMessage(message, isValid, field) {
	        img_Prc = document.getElementById("img_Prc");
	        var img_Prc2 = document.getElementById("img_Prc2");
	        var img_Prc3 = document.getElementById("img_Prc3");
	        var fontColor;
	        var div = document.getElementById("msg");
	        var div2 = document.getElementById("msg2");
	        var div3 = document.getElementById("msg3");

	        if (isValid == "true" || isValid == "True") {
	            fontColor = "green";
	            if (field == "account") {
	                img_Prc.src = "img/success.png";
	                img_Prc.style.visibility = 'visible';
	                div.innerHTML = "";
	            }
	        }
	        else {
	            fontColor = "red";
	            if (field == "account") {
	                img_Prc.src = "img/error.png";
	                img_Prc.style.visibility = 'visible';
	                div.innerHTML = "<font color=" + fontColor + ">" + message + " </font>"; //顯示是否有重複的帳號
	            }
	        }
	    }
	    var status = true;
	    function show() {
	        if (status) {
	            alert("if Enabled Status was disabled, stop receiving the value and the client does not show.");
	        }
	        status = false;
	    }
      </script>
	
    <style type="text/css">
        .style1
        {
            width: 130px;
            height: 35px;
            padding-left:10px;
        }
        .style2
        {
            width: 250px;
            height: 40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="inner" style=" padding-top:21px; padding-left:25px;">
                <asp:Panel ID="Panel1" runat="server" Width="445px" Height="350px">
                    <fieldset>				
					<legend>ECO5 Account information</legend>
					<table>
					    <tr>
					        <td class="style1">Member Account</td>
					        <td class="style2">
                                <asp:Label ID="Account" runat="server" Text=""></asp:Label></td>
					    </tr>
					    <tr>
					        <td class="style1">ECO5 Account</td>
					        <td class="style2">
                                <asp:Label ID="ECO_Account" runat="server" Text=""></asp:Label><%--<asp:TextBox ID="ECO_Account" runat="server" onblur=chkAccount()></asp:TextBox>--%>
					            </td>
<%--                                <asp:RequiredFieldValidator ID="ecoAccount_empty" runat="server" ErrorMessage="ECO5帳號不可為空" ControlToValidate="ECO_Account" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="請輸入15個英文或數字" 
                                    ValidationExpression="^\w{15}$" ControlToValidate="ECO_Account" Display="None"></asp:RegularExpressionValidator>--%>
					    </tr>
					    <tr>
					        <td class="style1">ECO5 Password</td>
					        <td class="style2"><asp:TextBox ID="ECO_Password" runat="server" onblur="chkPassword();"></asp:TextBox>
					            <img id="img_Prc2" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                <div style="width:auto; font-size:12px" id="msg2"></div></td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Password validation error" 
                                    ValidationExpression="^\w{6}$" ControlToValidate="ECO_Password" Display="None"></asp:RegularExpressionValidator>
					    </tr>
					    <tr>
					        <td class="style1">ECO5 Model</td>
					        <td class="style2">
                                <asp:Label ID="ECO_Type" runat="server" Text=""></asp:Label></td>
					    </tr>
					    <tr>
					        <td class="style1">Controller number</td>
					        <td class="style2">
                                <asp:Label ID="CtrlNr" runat="server" Text=""></asp:Label>
					            <%--<asp:DropDownList ID="ctrlnr_DropDownList" runat="server" Width="70px">
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>5</asp:ListItem>
                                    <asp:ListItem>6</asp:ListItem>
                                    <asp:ListItem>7</asp:ListItem>
                                    <asp:ListItem>8</asp:ListItem>
                                    <asp:ListItem>9</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                </asp:DropDownList>--%></td>
					    </tr>
					    <tr>
					        <td class="style1">Drawing No.</td>
					        <td class="style2"><asp:TextBox ID="DrawNr" runat="server" onblur="chkDrawnr();"></asp:TextBox>
					            <img id="img_Prc4" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                <div style="width:auto; font-size:12px" id="msg4"></div></td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Drawing NO. can only contain English characters or numbers." 
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="DrawNr" Display="None"></asp:RegularExpressionValidator>
					    </tr>
					    <tr>
					        <td class="style1">Installation location</td>
					        <td class="style2"><asp:TextBox ID="InstallPosition" runat="server" onblur="chkPosition();"></asp:TextBox>
					            <img id="img_Prc5" style="VISIBILITY: hidden; CURSOR: wait;" height="20px" width="20px">
                                <div style="width:auto; font-size:12px" id="msg5"></div></td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Location can only contain English characters or numbers." 
                                    ValidationExpression="^(\w|[\u4E00-\u9FA5])*$" ControlToValidate="InstallPosition" Display="None"></asp:RegularExpressionValidator>
					    </tr>
					    <tr>
					        <td class="style1">Enabled status</td>
					        <td class="style2"><asp:RadioButtonList ID="enabled_RadioButtonList" runat="server" 
                                    RepeatDirection="Horizontal" Width="150px" onclick="show();">
                                <asp:ListItem Value="1">Enable</asp:ListItem>
                                <asp:ListItem Value="0">Disable</asp:ListItem>
                            </asp:RadioButtonList></td>
					    </tr>
					</table>
						
					</fieldset>
					
					<div align="center">
					    <asp:Button ID="update_btn" runat="server" Text="Confirm" />
					</div>
        
				</asp:Panel>
        <asp:Label ID="old_EcoAccount" runat="server" Text="" Visible="False"></asp:Label>
        </div>
        
    </form>
</body>
</html>
