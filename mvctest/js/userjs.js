var sid = "0"
var aid = "0";
var alid = "0";
var oid = "0";
var cid = "1";
var atime = "3012/12/12 12:00:00";

$(document).ready(function(){
	$(".school").click(function(){
		sid = $(this).attr("id");
	});
	$("#getrc").click(function(){
		var tel = $("#name").val();
		if ($("#name").val().match(/^((13|15|17|18)+\d{9})$/)){ 
			$("#sreg").text("立即注册");
			$("#sreg").removeAttr("disabled");
			$.post("/Home/regcode", {name:$("#name").val()}, function(data){
				if(data=="reg already"){
					$("#ww").text("该号码已被注册！");
					$("#wrong").popup("open");
				}
			});
			var count = 60;
			var countdown = setInterval(CountDown, 1000);
			function CountDown(){
				$("#getrc").attr("disabled", true);
				$("#getrc").text("请等待" + count + "秒后重试");
				if (count == 0){
					$("#getrc").removeAttr("disabled");
					$("#getrc").text("获取短信验证码");
					clearInterval(countdown);
				}
				count--;
			}
		}
		else{
			$("#ww").text("手机号错误！");
			$("#wrong").popup("open");
		}
	});
	$("#sreg").click(function(){
		if($("#rinfcode").val()=="" || $("#passwd").val()=="" || $("#passwd2").val()=="" || $("#nickname").val()==""){
			$("#ww").text("请将信息填写完整！");
			$("#wrong").popup("open");
		}
		else if($("#rinfcode").val().length!=4){
			$("#ww").text("验证码错误！");
			$("#wrong").popup("open");
		}
		else if($("#passwd").val().length<6){
			$("#ww").text("密码必须大于或等于6位！");
			$("#wrong").popup("open");
		}
		else if($("#passwd").val()==$("#passwd2").val()){
			$("#sreg").text("请稍候…");
			$.post("/Home/regx", {"rinfcode":$("#rinfcode").val(), "passwd":$("#passwd").val(), "nickname":$("#nickname").val(), "schid":sid, "sex":$("#slider2").val()}, function(data){
				if(data=="wrong code"){
					$("#ww").text("验证码错误！");
					$("#wrong").popup("open");
				}
				else{
					$.mobile.changePage("#act", {transition: "flow"});
				}
			});
		}
		else{
			$("#ww").text("两次输入的密码不同！");
			$("#wrong").popup("open");
		}
	});
	$(".logout").click(function(){
		$.get("/Home/logout",function(data){
			if(data=="logout sucess"){
				window.location.href="index.html";
			}
		});
	});
	$("#arf").click(function(){
		var myDate = new Date();
		var Month = myDate.getMonth()+1;
		$.get("/Activity/get_actbytime",{"schoolid":sid, "actid":"0", "acttime":myDate.getFullYear()+"/"+Month+"/"+myDate.getDate()+" "+myDate.getHours()+":"+myDate.getMinutes()+":"+myDate.getSeconds()},function(data){
			location.hash = "#a_top";
			for (var i=0;i<10;i++) {
				$("#a_"+i).hide();
			}
			var obj = eval ("(" + data + ")");
			$("#anp").text("已到最后一页");
			$("#anp").attr("disabled", true);
			for (var i=0;i<10;i++) {
				var act = obj.activity_all_info[i];
				if(i==9) {
					alid = act.ActivityID;
					atime = act.ActivityTime;
					$("#anp").text("下一页");
					$("#anp").removeAttr("disabled");
				}
				$("#a_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/" + act.PhotoDir + "\" width=\"100%\">");
				$("#a_b"+i).text(act.ActivityName);
				$("#a_c"+i).text(act.Address);
				$("#a_d"+i).text(act.ActivityTime);
				$("#i"+i).attr("aid", act.ActivityID);
				$("#a_"+i).fadeIn(i*1000);
			}
		});
	});
	$("#anp").click(function(){
		$.get("/Activity/get_actbytime",{"schoolid":sid, "actid":alid, "acttime":atime},function(data){
			location.hash = "#a_top";
			for (var i=0;i<10;i++) {
				$("#a_"+i).hide();
			}
			var obj = eval ("(" + data + ")");
			$("#anp").text("已到最后一页");
			$("#anp").attr("disabled", true);
			for (var i=0;i<10;i++) {
				var act = obj.activity_all_info[i];
				if(i==9) {
					alid = act.ActivityID;
					atime = act.ActivityTime;
					$("#anp").text("下一页");
					$("#anp").removeAttr("disabled");
				}
				$("#a_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/" + act.PhotoDir + "\" width=\"100%\">");
				$("#a_b"+i).text(act.ActivityName);
				$("#a_c"+i).text(act.Address);
				$("#a_d"+i).text(act.ActivityTime);
				$("#i"+i).attr("aid", act.ActivityID);
				$("#a_"+i).fadeIn(i*1000);
			}
		});
	});
	$("#gz").click(function(){
		$.post("/Organization/add_gz", {org_id:oid}, function(data){
			if(data=="true"){
				$("#gzp").popup("open");
			}
		});
	});
	$("#bm").click(function(){
		$.post("/Activity/add_cj_activity", {activity_id:aid}, function(data){
			if(data=="true"){
				$("#bmp").popup("open");
			}
		});
	});
	$(".a2dt").click(function(){
		aid = $(this).attr("aid");
	});
	$(".o2dt").click(function(){
		oid = $(this).attr("oid");
	});
	$(".school").click(function(){
		sid = $(this).attr("id");
	});
	$(".oc").click(function(){
		cid = $(this).attr("ocid");
	});
	$("#manage").on("swipeleft", function(){
		$.mobile.changePage("#act", {transition: "slide"});
	})
	$("#act").on("swipeleft", function(){
		$.mobile.changePage("#org", {transition: "slide"});
	}).on("swiperight", function(){
		$.mobile.changePage("#manage", {transition: "slide", reverse:true});
	});
	$("#org").on("swiperight", function(){
		$.mobile.changePage("#act", {transition: "slide", reverse:true});
	});
	$("#myact").on("swipeleft", function(){
		$.mobile.changePage("#myorg", {transition: "slide"});
	});
	$("#myorg").on("swiperight", function(){
		$.mobile.changePage("#myact", {transition: "slide", reverse:true});
	});
});

$(document).on("pagebeforeshow","#act",function(){
	var countdown = setInterval(CountDown, 5000);
	$("#fc").text("左右滑动有惊喜~");
	$.get("/Manage/get_my_acount",function(data){
		var obj = eval ("(" + data + ")");
		var acount = obj.my_acount[0];
		sid = acount.SchoolID;
		var myDate = new Date();
		var Month = myDate.getMonth()+1;
		$.get("/Activity/get_actbytime",{"schoolid":sid, "actid":"0", "acttime":myDate.getFullYear()+"/"+Month+"/"+myDate.getDate()+" "+myDate.getHours()+":"+myDate.getMinutes()+":"+myDate.getSeconds()},function(data){
			var obj = eval ("(" + data + ")");
			$("#anp").text("已到最后一页");
			$("#anp").attr("disabled", true);
			for (var i=0;i<10;i++) {
				$("#a_"+i).hide();
			}
			for (var i=0;i<10;i++) {
				var act = obj.activity_all_info[i];
				if(i==9) {
					alid = act.ActivityID;
					atime = act.ActivityTime;
					$("#anp").text("下一页");
					$("#anp").removeAttr("disabled");
				}
				$("#a_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/" + act.PhotoDir + "\" width=\"100%\">");
				$("#a_b"+i).text(act.ActivityName);
				$("#a_c"+i).text(act.Address);
				$("#a_d"+i).text(act.ActivityTime);
				$("#i"+i).attr("aid", act.ActivityID);
				$("#a_"+i).fadeIn(i*1000);
			}
		});
	});
	function CountDown(){
		$("#fc").fadeOut(1000,function(){
			$("#fc").text("社友——校园活动应有尽有~");
			$("#fc").fadeIn(1000);
		});
		clearInterval(countdown);
	}
});

$(document).on("pagebeforeshow","#org_class",function(){
	$.get("/Organization/get_org_byclass",{"classid":cid, "schoolid":sid},function(data){
		var obj = eval ("(" + data + ")");
		for (var i=0;i<20;i++) {
			$("#o_"+i).hide();
		}
		for (var i=0;i<20;i++) {
			var org = obj.org_byclass[i];
			$("#o_a"+i).attr("src","http://www.sheyou.me/admin/images/" + org.Logo);
			$("#o_b"+i).text(org.OrganizationName);
			$("#o_c"+i).text(org.Countnumber);
			$("#j"+i).attr("oid", org.OrganizationID);
			$("#o_"+i).fadeIn(i*500);
		}
	});
});

$(document).on("pagebeforeshow","#org_search",function(){
	$.get("/Organization/get_org_byname",{"name":$("#o_name").val()},function(data){
		var obj = eval ("(" + data + ")");
		for (var i=0;i<20;i++) {
			$("#os_"+i).hide();
		}
		for (var i=0;i<20;i++) {
			var org = obj.org_byclass[i];
			$("#os_a"+i).attr("src","http://www.sheyou.me/admin/images/" + org.Logo);
			$("#os_b"+i).text(org.OrganizationName);
			$("#os_c"+i).text(org.Countnumber);
			$("#k"+i).attr("oid", org.OrganizationID);
			$("#os_"+i).fadeIn(i*500);
		}
	});
});

$(document).on("pagebeforeshow","#act_detail",function(){
	$.get("/Activity/get_activityx_info",{"actid": aid},function(data){
		var obj = eval ("(" + data + ")");
		var detail = obj.activityx_info[0];
		$("#ad_a").text(detail.ActivityName);
		$("#ad_b").html("<img src=\"http://www.sheyou.me/admin/images/" + detail.PhotoDir + "\" width=\"100%\">");
		$("#ad_c").text(detail.ActivirtyContent);
		if(detail.cjState==0){
			$("#bm").text("报名");
			$("#bm").removeAttr("disabled");
		}
		else{
			$("#bm").text("已报名");
			$("#bm").attr("disabled", true);
		}
	});
});

$(document).on("pagebeforeshow","#org_detail",function(){
	$.get("/Organization/get_orgx_info",{"orgid": oid},function(data){
		var obj = eval ("(" + data + ")");
		var od = obj.orgx_info[0];
		$("#od_a").text(od.OrganizationName);
		$("#od_b").html("<img src=\"http://www.sheyou.me/admin/images/" + od.Logo + "\" width=\"100%\">");
		$("#od_c").text(od.Introduction);
		if(od.gzstate==0){
			$("#gz").text("关注");
			$("#gz").removeAttr("disabled");
		}
		else{
			$("#gz").text("已关注");
			$("#gz").attr("disabled", true);
		}
	});
});

$(document).on("pagebeforeshow","#myact",function(){
	$.get("/Manage/get_my_activity",function(data){
		for (var i=0;i<10;i++) {
			$("#ma_"+i).hide();
		}
		var obj = eval ("(" + data + ")");
		for (var i=0;i<10;i++) {
			var act = obj.my_activity[i];
			$("#ma_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/" + act.PhotoDir + "\" width=\"100%\">");
			$("#ma_b"+i).text(act.ActivityName);
			$("#ma_c"+i).text(act.OrganizationName);
			$("#ma_d"+i).text(act.ActivityTime);
			$("#mi"+i).attr("aid", act.ActivityID);
			$("#ma_"+i).fadeIn(i*1000);
		}
	});
});

$(document).on("pagebeforeshow","#myorg",function(){
	$.get("/Manage/get_my_org",function(data){
		var obj = eval ("(" + data + ")");
		for (var i=0;i<10;i++) {
			$("#mo_"+i).hide();
		}
		for (var i=0;i<10;i++) {
			var org = obj.my_org[i];
			$("#mo_a"+i).attr("src","http://www.sheyou.me/admin/images/" + org.Logo);
			$("#mo_b"+i).text(org.OrganizationName);
			$("#mo_c"+i).text(org.Countnumber);
			$("#mj"+i).attr("oid", org.OrganizationID);
			$("#mo_"+i).fadeIn(i*500);
		}
	});
});

$(document).on("swiperight","#manage",function(){
	$("#quick_manage2").panel("open");
});
$(document).on("taphold","#act",function(){
	$("#quick_act2").panel("open");
});
$(document).on("swipeleft","#org",function(){
	$("#quick_org2").panel("open");
});