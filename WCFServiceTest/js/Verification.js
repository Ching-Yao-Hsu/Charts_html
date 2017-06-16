var xmlHttp;
//程式由此執行
function chkAccount() {
    img_Prc = document.getElementById("img_Prc");
    img_Prc.style.visibility = 'visible';
    img_Prc.src = "img/Processing.jpg"; //顯示處理中的圖片
    var div = document.getElementById("msg");
    createXMLHttpRequest(); //建立XMLHttpRequest物件
    var account = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("Account").ClientID %>').value;
    if (account != '') {
        var regex1 = /^\w+$/;
        if (regex1.test(account) == true) {
            var regex2 = /^\w{5,15}$/;
            if (regex2.test(account) == true) {
                var regex3 = /^[a-zA-Z]/;
                if (regex3.test(account) == true) {
                    var url = "Verification.aspx?account=" + account + "&type=NewUser&field=account";
                    xmlHttp.onreadystatechange = callback; //資料回傳之後，使用callback這個函數處理後續動作 
                    xmlHttp.open("POST", url, true); //將輸入的帳號傳至後端作驗證 
                    xmlHttp.send(null);
                } else {
                    img_Prc.src = "img/error.png";
                    img_Prc.style.visibility = 'visible';
                    div.innerHTML = "<font color=red>帳號需以英文字母作為開頭！</font>";
                }
            } else {
                img_Prc.src = "img/error.png";
                img_Prc.style.visibility = 'visible';
                div.innerHTML = "<font color=red>帳號字數不符，長度限制 5 ~ 15 字元！</font>";
            }
        } else {
            img_Prc.src = "img/error.png";
            img_Prc.style.visibility = 'visible';
            div.innerHTML = "<font color=red>帳號只能含有數字與英文！</font>";
        }
    } else {
        img_Prc.src = "img/error.png";
        img_Prc.style.visibility = 'visible';
        div.innerHTML = "<font color=red>帳號不可為空！</font>";
    }
}
function chkPassword() {
    img_Prc2 = document.getElementById("img_Prc2");
    img_Prc2.style.visibility = 'visible';
    img_Prc2.src = "img/Processing.jpg"; //顯示處理中的圖片
    var div2 = document.getElementById("msg2");
    var password = document.getElementById('<%= Page.FindControl("Password").ClientID %>').value;
    if (password != '') {
        var regex1 = /^\w+$/;
        if (regex1.test(password) == true) {
            var regex2 = /^.{6,15}$/;
            if (regex2.test(password) == true) {
                img_Prc2.src = "img/success.png";
                img_Prc2.style.visibility = 'visible';
                div2.innerHTML = "";
            } else {
                img_Prc2.src = "img/error.png";
                img_Prc2.style.visibility = 'visible';
                div2.innerHTML = "<font color=red>密碼字數不符，長度限制 6 ~ 15 字元！</font>";
            }
        } else {
            img_Prc2.src = "img/error.png";
            img_Prc2.style.visibility = 'visible';
            div2.innerHTML = "<font color=red>密碼只能含有數字與英文！</font>";
        }
    } else {
        img_Prc2.style.visibility = 'hidden';
        div2.innerHTML = "";
    }
}
function ReChkPassword() {
    img_Prc3 = document.getElementById("img_Prc3");
    img_Prc3.style.visibility = 'visible';
    img_Prc3.src = "img/Processing.jpg"; //顯示處理中的圖片
    var div3 = document.getElementById("msg3");
    var password = document.getElementById('<%= Page.FindControl("Password").ClientID %>').value;
    var Rpassword = document.getElementById('<%= Page.FindControl("Reconfirm").ClientID %>').value;
    if (Rpassword != '') {
        if (Rpassword == password) {
            img_Prc3.src = "img/success.png";
            img_Prc3.style.visibility = 'visible';
            div3.innerHTML = "";
        } else {
            img_Prc3.src = "img/error.png";
            img_Prc3.style.visibility = 'visible';
            div3.innerHTML = "<font color=red>確認密碼錯誤！</font>";
        }
    } else {
        img_Prc3.style.visibility = 'hidden';
        div3.innerHTML = "";
    }
}
function chkCompany() {
    img_Prc4 = document.getElementById("img_Prc4");
    img_Prc4.style.visibility = 'visible';
    img_Prc4.src = "img/Processing.jpg"; //顯示處理中的圖片
    var div4 = document.getElementById("msg4");
    var company = document.getElementById('<%= Page.FindControl("Company").ClientID %>').value;
    if (company != '') {
        var regex = /^(\w|[\u4E00-\u9FA5])*$/;  //判斷英文數字中文
        if (regex.test(company) == true) {
            img_Prc4.src = "img/success.png";
            img_Prc4.style.visibility = 'visible';
            div4.innerHTML = "";
        } else {
            img_Prc4.src = "img/error.png";
            img_Prc4.style.visibility = 'visible';
            div4.innerHTML = "<font color=red>公司名稱只能含中文英文數字！</font>";
        }
    } else {
        img_Prc4.src = "img/error.png";
        img_Prc4.style.visibility = 'visible';
        div4.innerHTML = "<font color=red>公司名稱不可為空！</font>";
    }
}
function chkEmail() {
    img_Prc5 = document.getElementById("img_Prc5");
    img_Prc5.style.visibility = 'visible';
    img_Prc5.src = "img/Processing.jpg"; //顯示處理中的圖片
    var div5 = document.getElementById("msg5");
    createXMLHttpRequest(); //建立XMLHttpRequest物件
    var email = document.getElementById('<%= Page.FindControl("E_Mail").ClientID %>').value;
    if (email != '') {
        var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (regex.test(email) == true) {
            var url = "Verification.aspx?email=" + email + "&type=NewUser&field=email";
            xmlHttp.onreadystatechange = callback; //資料回傳之後，使用callback這個函數處理後續動作 
            xmlHttp.open("POST", url, true); //將輸入的密碼傳至後端作驗證 
            xmlHttp.send(null);
        } else {
            img_Prc5.src = "img/error.png";
            img_Prc5.style.visibility = 'visible';
            div5.innerHTML = "<font color=red>信箱格式錯誤！</font>";
        }
    } else {
        img_Prc5.src = "img/error.png";
        img_Prc5.style.visibility = 'visible';
        div5.innerHTML = "<font color=red>信箱不可為空！</font>";
    }

}
function chkName() {
    img_Prc6 = document.getElementById("img_Prc6");
    img_Prc6.style.visibility = 'visible';
    img_Prc6.src = "img/Processing.jpg"; //顯示處理中的圖片
    var div6 = document.getElementById("msg6");
    var name = document.getElementById('<%= Page.FindControl("Name").ClientID %>').value;
    if (name != '') {
        var regex = /^(\w|[\u4E00-\u9FA5])*$/;
        if (regex.test(name) == true) {
            img_Prc6.src = "img/success.png";
            img_Prc6.style.visibility = 'visible';
            div6.innerHTML = "";
        } else {
            img_Prc6.src = "img/error.png";
            img_Prc6.style.visibility = 'visible';
            div6.innerHTML = "<font color=red>姓名只能含中文英文數字！</font>";
        }
    } else {
        img_Prc6.style.visibility = 'hidden';
        div6.innerHTML = "";
    }
}
function chkAddress() {
    img_Prc7 = document.getElementById("img_Prc7");
    img_Prc7.style.visibility = 'visible';
    img_Prc7.src = "img/Processing.jpg";
    var div7 = document.getElementById("msg7");
    var address = document.getElementById('<%= Page.FindControl("Address").ClientID %>').value;
    if (address != '') {
        var regex = /^(\w|[\u4E00-\u9FA5])*$/;
        if (regex.test(address) == true) {
            img_Prc7.src = "img/success.png";
            img_Prc7.style.visibility = 'visible';
            div7.innerHTML = "";
        } else {
            img_Prc7.src = "img/error.png";
            img_Prc7.style.visibility = 'visible';
            div7.innerHTML = "<font color=red>地址只能含中文英文數字！</font>";
        }
    } else {
        img_Prc7.style.visibility = 'hidden';
        div7.innerHTML = "";
    }
}
function chkTel() {
    img_Prc8 = document.getElementById("img_Prc8");
    img_Prc8.style.visibility = 'visible';
    img_Prc8.src = "img/Processing.jpg";
    var div8 = document.getElementById("msg8");
    var tel = document.getElementById('<%= Page.FindControl("Tel").ClientID %>').value;
    if (tel != '') {
        var regex = /^[0-9]*$/;
        if (regex.test(tel) == true) {
            img_Prc8.src = "img/success.png";
            img_Prc8.style.visibility = 'visible';
            div8.innerHTML = "";
        } else {
            img_Prc8.src = "img/error.png";
            img_Prc8.style.visibility = 'visible';
            div8.innerHTML = "<font color=red>電話只能含數字！</font>";
        }
    } else {
        img_Prc8.style.visibility = 'hidden';
        div8.innerHTML = "";
    }
}
function chkMobile() {
    img_Prc9 = document.getElementById("img_Prc9");
    img_Prc9.style.visibility = 'visible';
    img_Prc9.src = "img/Processing.jpg";
    var div9 = document.getElementById("msg9");
    var mobile = document.getElementById('<%= Page.FindControl("Mobile").ClientID %>').value;
    if (mobile != '') {
        var regex = /^[0-9]*$/;
        if (regex.test(mobile) == true) {
            img_Prc9.src = "img/success.png";
            img_Prc9.style.visibility = 'visible';
            div9.innerHTML = "";
        } else {
            img_Prc9.src = "img/error.png";
            img_Prc9.style.visibility = 'visible';
            div9.innerHTML = "<font color=red>手機只能含數字！</font>";
        }
    } else {
        img_Prc9.style.visibility = 'hidden';
        div9.innerHTML = "";
    }
}
function chkAll() {
    chkPassword();
    //chkAddress();
    chkCompany();
    chkEmail();
    //chkMobile();
    //chkName();
    //chkTel();
}
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
    var img_Prc5 = document.getElementById("img_Prc5");
    var fontColor;
    var div5 = document.getElementById("msg5");

    if (isValid == "true" || isValid == "True") {
        fontColor = "green";
        if (field == "email") {
            img_Prc5.src = "img/success.png";
            img_Prc5.style.visibility = 'visible';
            div5.innerHTML = "";
        }
    }
    else {
        fontColor = "red";
        if (field == "email") {
            img_Prc5.src = "img/error.png";
            img_Prc5.style.visibility = 'visible';
            div5.innerHTML = "<font color=" + fontColor + ">" + message + " </font>";
        }
    }
}