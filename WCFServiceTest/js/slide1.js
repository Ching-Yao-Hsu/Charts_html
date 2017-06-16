$(function(){
	//將子選單藏起來
	$('.dropdown li ul').hide();
	//從.dropdown找出它的孩子
	$('.dropdown > li').hover(function(){
		var $this = $(this),
			$menu = $this.find('ul');
		
		$menu.css({
			left:0,
			top:$this.height()
		}).stop(false, true).slideDown(300);
	},function(){
		$(this).find('ul').stop(false, true).slideUp();
	});
//===========================================================================
// 幫 #hmenu li a 加上 hover 事件
$("#hmenu li a").hover(function(){
// 滑鼠移進選項時..
// 把背景圖片的位置往上移動
var _this = $(this),
_height = _this.height() * -1;
_this.stop().animate({
backgroundPosition: "(0px " + _height + "px)"
}, 200);
}, function(){
// 滑鼠移出選項時..
// 把背景圖片的位置移回原位
$(this).stop().animate({
backgroundPosition: "(0px 0px)"
}, 200);
});
});

$(function(){
	$("#img1").click(function(){
		var _w = $(document).width();
		var _h = $(document).height(), 
			$win = $(window), 
			_winHeight = $win.height(), 
			_winWidth = $win.width();

		$("<div class='showDiv' />").css({
			position: "absolute",
			width: _w,
			height: _h,
			left: 0,
			top: 0,
			opacity: 0.6,
			display: "none",
			backgroundColor: "black"
		}).appendTo("body").fadeIn(400)
			.click(function(){
				$(".showDiv, .bImg").fadeOut(400, function(){
					$(this).remove();	
				});
			});
		var _bImg = $('<img src="images/eco5-display-large.jpg" class="bImg" />').hide()
			.css({
				left: (_winWidth - 600) / 2  + $win.scrollLeft(),
				top: (_winHeight - 431) / 2 + $win.scrollTop()
			}).click(function(){
				$(".showDiv, .bImg").fadeOut(400, function(){
					$(this).remove();	
				});
			}).appendTo("body");

		_bImg.show().animate({
			width: 600,
			height:431
		}, 400);
	});
});

$(function(){
	$("#img2").click(function(){
		var _w = $(document).width();
		var _h = $(document).height(), 
			$win = $(window), 
			_winHeight = $win.height(), 
			_winWidth = $win.width();

		$("<div class='showDiv2' />").css({
			position: "absolute",
			width: _w,
			height: _h,
			left: 0,
			top: 0,
			opacity: 0.6,
			display: "none",
			backgroundColor: "black"
		}).appendTo("body").fadeIn(400)
			.click(function(){
				$(".showDiv, .bImg").fadeOut(400, function(){
					$(this).remove();	
				});
			});
		var _bImg2 = $('<img src="../images/eco5-display-en-large.jpg" class="bImg2" />').hide()
			.css({
				left: (_winWidth - 600) / 2  + $win.scrollLeft(),
				top: (_winHeight - 431) / 2 + $win.scrollTop()
			}).click(function(){
				$(".showDiv2, .bImg2").fadeOut(400, function(){
					$(this).remove();	
				});
			}).appendTo("body");

		_bImg2.show().animate({
			width: 600,
			height:431
		}, 400);
	});
});

$(function(){
	$("#img3").click(function(){
		var _w = $(document).width();
		var _h = $(document).height(), 
			$win = $(window), 
			_winHeight = $win.height(), 
			_winWidth = $win.width();

		$("<div class='showDiv3' />").css({
			position: "absolute",
			width: _w,
			height: _h,
			left: 0,
			top: 0,
			opacity: 0.6,
			display: "none",
			backgroundColor: "black"
		}).appendTo("body").fadeIn(400)
			.click(function(){
				$(".showDiv, .bImg").fadeOut(400, function(){
					$(this).remove();	
				});
			});
		var _bImg3 = $('<img src="../images/eco5-display-large-s.jpg" class="bImg3" />').hide()
			.css({
				left: (_winWidth - 600) / 2  + $win.scrollLeft(),
				top: (_winHeight - 431) / 2 + $win.scrollTop()
			}).click(function(){
				$(".showDiv3, .bImg3").fadeOut(400, function(){
					$(this).remove();	
				});
			}).appendTo("body");

		_bImg3.show().animate({
			width: 600,
			height:431
		}, 400);
	});
});

$(function(){
	$("img.showIMG").click(function(){
		var _w = $(document).width();
		var _h = $(document).height(), 
			$win = $(window), 
			_winHeight = $win.height(), 
			_winWidth = $win.width();

		$("<div class='showDiv3' />").css({
			position: "absolute",
			width: _w,
			height: _h,
			left: 0,
			top: 0,
			opacity: 0.6,
			display: "none",
			backgroundColor: "black"
		}).appendTo("body").fadeIn(400)
			.click(function(){
				$(".showDiv, .bImg").fadeOut(400, function(){
					$(this).remove();	
				});
			});
		var _bImg3 = $('<img src="../images/eco5-display-large-s.jpg" class="bImg3" />').hide()
			.css({
				left: (_winWidth - 600) / 2  + $win.scrollLeft(),
				top: (_winHeight - 431) / 2 + $win.scrollTop()
			}).click(function(){
				$(".showDiv3, .bImg3").fadeOut(400, function(){
					$(this).remove();	
				});
			}).appendTo("body");

		_bImg3.show().animate({
			width: 600,
			height:431
		}, 400);
	});
});

$(function(){
	$("#GOTOP").click(function(){
		$("html,body").animate({scrollTop:0},400);
		return false;
	});
})
$(function(){
		// 先取得 #abgne-block , 必要參
		var $block = $('#abgne-block'); 
		// 幫 #abgne-block .title ul li 加上 hover() 事件
		var $li = $('.title ul li', $block).hover(function(){
			// 當滑鼠移上時加上 .over 樣式
			$(this).addClass('over').siblings('.over').removeClass('over');
		}, function(){
			// 當滑鼠移出時移除 .over 樣式
			$(this).removeClass('over');
		}).click(function(){
			// 當滑鼠點擊時, 顯示相對應的 div.info
			// 並加上 .on 樣式
			var $this = $(this);
			$this.add($('.bd div.info', $block).eq($this.index())).addClass('on').siblings('.on').removeClass('on');
		});
		
	});

$(function(){
	$("img.yearImg").next().hide().eq(0).show();

	$("img.yearImg").click(function(){
		if(!$(this).next().is(":visible")){
			$("img.yearImg").next().slideUp();
			$(this).next().slideToggle();
		}
	});
});

$(function(){
	
	$("div.photoBox img").click( function(){
		
		var NN = $(this).index();
		
	$("div.showphoto img").eq(NN).fadeIn(600).siblings().fadeOut(600);
	
	});

	$("div.photoBox img").fadeTo(200, 0.5);

	$("div.photoBox img").hover(function(){
		$(this).fadeTo(200, 1);	
	}, function(){
		$(this).fadeTo(200, 0.5);
	});

});