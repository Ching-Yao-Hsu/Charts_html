$(function(){
	$("a#TOPup").click(function(){
		$("html,body").animate({scrollTop:0},400);
		return false;
	});
	
	var $block = $('#F_BOX01'),
		$block2 = $('#F_BOX02'),
		$block3 = $('#F_BOX03'),
		$block4 = $('#F_BOX04'),
		$link = $block.find('.link li'),
		$link2 = $block2.find('.link li');
	    $link3 = $block3.find('.link li');
	    $link4 = $block4.find('.link li');
	$link.click( function(){
		NN = $(this).index(),
		$imgBox = $block.find('.imgBox img');
		$(this).find("a").addClass("ckick_C").parent().siblings().find("a").removeClass("ckick_C");
		$imgBox.eq(NN).fadeIn(300).siblings().fadeOut(100);
		return false;
	});	
	
	$link2.click( function(){
		MM = $(this).index(),
		$imgBox2 = $block2.find('.imgBox img');
		$(this).find("a").addClass("ckick_C").parent().siblings().find("a").removeClass("ckick_C");
		$imgBox2.eq(MM).fadeIn(300).siblings().fadeOut(100);
		return false;
    });

    $link3.click(function() {
        KK = $(this).index(),
        $imgBox3 = $block3.find('.imgBox img');
        $(this).find("a").addClass("ckick_C").parent().siblings().find("a").removeClass("ckick_C");
        $imgBox3.eq(KK).fadeIn(300).siblings().fadeOut(100);
        return false;
    });

    $link4.click(function() {
        LL = $(this).index(),
        $imgBox4 = $block4.find('.imgBox img');
        $(this).find("a").addClass("ckick_C").parent().siblings().find("a").removeClass("ckick_C");
        $imgBox4.eq(LL).fadeIn(300).siblings().fadeOut(100);
        return false;
    });	
	
	var _tabshow = 0;
	var $Lidefault = $('#MenuList li').eq(_tabshow).find("a").addClass('M_click');
	var _Hrefdefault = $Lidefault.attr('href');
	
	//#tab1 抓矛點&id
	//抓出自己把其他兄弟藏起來
	$(_Hrefdefault).show().siblings().hide();
	//點選li
	$('#MenuList li').click(function(){
		var $this = $(this),
		_href = $this.find('a').attr('href');
					
		$this.find("a").addClass('M_click').parent().siblings().find("a").removeClass('M_click');
		$(_href).fadeIn().siblings().hide();
		return false;
		}).find('a').focus(function(){
			this.blur();
		});
	
});
