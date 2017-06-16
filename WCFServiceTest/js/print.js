function printScreen(printlist) {
//    var value = printlist.innerHTML;
////    var newstr = document.getElementById(myDiv).innerHTML;
////    alert(value);
//    var oldstr = document.body.innerHTML;
//    document.body.innerHTML = value;
//    window.print();
////    window.close();
//    document.body.innerHTML = oldstr;
//    return false;

    var value = printlist.innerHTML;
    var printPage = window.open("", "Printing...", "");
    printPage.document.open();
    printPage.document.write("<HTML><head></head><BODY onload='window.print();window.close()'>");
    printPage.document.write("<PRE>");
    printPage.document.write(value);
    printPage.document.write("</PRE>");
    printPage.document.close("</BODY></HTML>");
}

function printPowerChart(printlist) {
    var value = printlist.innerHTML;
    var printPage = window.open("", "Printing...", "");
    printPage.document.open();
    printPage.document.write("<HTML><head></head><BODY onload='window.print();window.close()'>");
    printPage.document.write("<PRE>");
    printPage.document.write(value);
    printPage.document.write("</PRE>");
    printPage.document.close("</BODY></HTML>");
    
}