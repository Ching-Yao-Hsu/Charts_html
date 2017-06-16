// JavaScript Document

$(function () {
    var FilePath = location.pathname;
    var NameArray = new Array();
    var NameArray = FilePath.split("/");
    var i, j;
    switch (NameArray[2]) {
        case "index.aspx": i = 0; break;
        case "PowerChart.aspx":          i = 1; j = 0; break;
        case "PowerRecord.aspx":         i = 1; j = 1; break;
        case "PowerDayReport.aspx":      i = 1; j = 2; break;
        case "PowerMonthReport.aspx":    i = 1; j = 3; break;
        case "PowerSummaryReport.aspx":  i = 1; j = 4; break;
        case "PowerFeeCalculate.aspx":   i = 1; j = 5; break;
        case "PowerCrossChart.aspx":     i = 2; j = 6; break;
        case "PowerProportionS.aspx":    i = 2; j = 7; break;
        case "PowerProportionM.aspx":    i = 2; j = 8; break;
        case "PowerCross.aspx":          i = 2; j = 9; break;
        case "PowerTime.aspx":           i = 2; j = 10; break;
        case "PowerColumn.aspx":         i = 2; j = 11; break;
        case "Account.aspx":             i = 3; j = 12; break;
        case "ChangePassword.aspx":      i = 3; j = 13; break;
        case "MySetup.aspx":             i = 3; j = 14; break;
        case "NewUser.aspx":             i = 4; j = 15; break;
        case "UserInfo.aspx":            i = 4; j = 16; break;
        case "NewEcoAccount.aspx":       i = 4; j = 17; break;
        case "EcoInfo.aspx":             i = 4; j = 18; break;
        case "MeterInfo.aspx":           i = 4; j = 19; break;
        case "SumReportSetup.aspx":      i = 4; j = 20; break;
        case "MailSetup.aspx":           i = 4; j = 21; break;
        case "PowerEventRecord.aspx":    i = 4; j = 22; break;
    }
    //目前所在母選項頁面
    $('.dropdown>li>a').eq(i).css('background-image', 'url(img/menu_ul_a.jpg)').css('color', '#ffffff');
	//$('.dropdown>li>a').eq(i).css('background-color', '#315e5b').css('color','#ffffff');
	
    //目前所在的子選項頁面
    if (i != 0) {
        $('.dropdown>li>ul>li>a').eq(j).css('background-image', 'url(img/menu_ul_a.jpg)').css('color', '#ffffff');
    }
	
	// 幫 .dropdown li 加上 hover 事件
	$('.dropdown li').hover(function(){
		// 先找到 li 中的子選單
		var _this = $(this),
			_subnav = _this.children('ul');
			
		// 變更目前母選項的背景顏色
		// 同時顯示子選單(如果有的話)
		_subnav.css('display', 'block');
	} , function(){
		// 變更目前母選項的背景顏色
		// 同時隱藏子選單(如果有的話)
		// 也可以把整句拆成上面的寫法
		$(this).css('background-color', '').css('color','').children('ul').css('display', 'none');

	});
 
	// 取消超連結的虛線框
	$('a').focus(function(){
		this.blur();
	});
});