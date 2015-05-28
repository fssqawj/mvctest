var sid="1";
var aid="0";
var alid="0";
var oid="0";
var cid="1";
var atime="3012/12/12 12:00:00";
$(document).ready(function(){
	$("#android").attr("src","image/Android.jpg");
	$("#ios").attr("src","image/iOS.jpg");
	$("#wp").attr("src","image/WP.jpg");
	$("#lgi").click(function(){
		if($("#un").val()!=""&&$("#pw").val()!=""){
			$("#lgi").text("登录中…");
			$.post("/Home/login",{name:$("#un").val(),passwd:$("#pw").val()},function(data){
				if(data!="false"){window.location.href="user.html#act";}
				else{$("#dlcw").after("<p style=\"color:red\">用户名或密码错误！</p>");}
			});
		}
		else{$("#dlcw").after("<p style=\"color:red\">用户名和密码不能为空！</p>");}
	});
	$("#arf").click(function(){
		var myDate=new Date();
		var Month=myDate.getMonth()+1;
		$.get("/Activity/get_actbytime",{"schoolid":sid,"actid":"0","acttime":myDate.getFullYear()+"/"+Month+"/"+myDate.getDate()+" "+myDate.getHours()+":"+myDate.getMinutes()+":"+myDate.getSeconds()},function(data){
			location.hash="#a_top";
			for(var i=0;i<10;i++){$("#a_"+i).hide();}
			var obj=eval("("+data+")");
			$("#anp").text("已到最后一页");
			$("#anp").attr("disabled",true);
			for(var i=0;i<10;i++){
				var act=obj.activity_all_info[i];
				if(i==9){
					alid=act.ActivityID;
					atime=act.ActivityTime;
					$("#anp").text("下一页");
					$("#anp").removeAttr("disabled");
				}
				$("#a_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/"+act.PhotoDir+"\" width=\"100%\">");
				$("#a_b"+i).text(act.ActivityName);
				$("#a_c"+i).text(act.Address);
				$("#a_d"+i).text(act.ActivityTime);
				$("#i"+i).attr("aid",act.ActivityID);
				$("#a_"+i).fadeIn(i*1000);
			}
		});
	});
	$("#anp").click(function(){
		$.get("/Activity/get_actbytime",{"schoolid":sid,"actid":alid,"acttime":atime},function(data){
			location.hash="#a_top";
			for(var i=0;i<10;i++){$("#a_"+i).hide();}
			var obj=eval("("+data+")");
			$("#anp").text("已到最后一页");
			$("#anp").attr("disabled",true);
			for(var i=0;i<10;i++){
				var act=obj.activity_all_info[i];
				if(i==9){
					alid=act.ActivityID;
					atime=act.ActivityTime;
					$("#anp").text("下一页");
					$("#anp").removeAttr("disabled");
				}
				$("#a_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/"+act.PhotoDir+"\" width=\"100%\">");
				$("#a_b"+i).text(act.ActivityName);
				$("#a_c"+i).text(act.Address);
				$("#a_d"+i).text(act.ActivityTime);
				$("#i"+i).attr("aid",act.ActivityID);
				$("#a_"+i).fadeIn(i*1000);
			}
		});
	});
	$("#ts").click(function(){$("#tips").popup("open");});
	$("#gz").click(function(){$("#gzp").popup("open");});
	$("#bm").click(function(){$("#bmp").popup("open");});
	$(".a2dt").click(function(){aid=$(this).attr("aid");});
	$(".o2dt").click(function(){oid=$(this).attr("oid");});
	$(".school").click(function(){sid=$(this).attr("id");});
	$(".oc").click(function(){cid=$(this).attr("ocid");});
	$("#org").on("swipeleft",function(){$.mobile.changePage("#act",{transition:"slide"});});
	$("#act").on("swiperight",function(){$.mobile.changePage("#org",{transition:"slide",reverse:true});});
});
$(document).on("pagebeforeshow","#act",function(){
	var myDate=new Date();
	var Month=myDate.getMonth()+1;
	$.get("/Activity/get_actbytime",{"schoolid":sid,"actid":"0","acttime":myDate.getFullYear()+"/"+Month+"/"+myDate.getDate()+" "+myDate.getHours()+":"+myDate.getMinutes()+":"+myDate.getSeconds()},function(data){
		for(var i=0;i<10;i++){$("#a_"+i).hide();}
		var obj=eval("("+data+")");
		$("#anp").text("已到最后一页");
		$("#anp").attr("disabled",true);
		for(var i=0;i<10;i++){
			var act=obj.activity_all_info[i];
			if(i==9){
				alid=act.ActivityID;
				atime=act.ActivityTime;
				$("#anp").text("下一页");
				$("#anp").removeAttr("disabled");
			}
			$("#a_a"+i).html("<img src=\"http://www.sheyou.me/admin/images/"+act.PhotoDir+"\" width=\"100%\">");
			$("#a_b"+i).text(act.ActivityName);
			$("#a_c"+i).text(act.Address);
			$("#a_d"+i).text(act.ActivityTime);
			$("#i"+i).attr("aid",act.ActivityID);
			$("#a_"+i).fadeIn(i*1000);
		}
	});
});
$(document).on("pagebeforeshow","#org_class",function(){
	$.get("/Organization/get_org_byclass",{"classid":cid,"schoolid":sid},function(data){
		var obj=eval("("+data+")");
		for(var i=0;i<20;i++){$("#o_"+i).hide();}
		for(var i=0;i<20;i++){
			var org=obj.org_byclass[i];
			$("#o_a"+i).attr("src","http://www.sheyou.me/admin/images/"+org.Logo);
			$("#o_b"+i).text(org.OrganizationName);
			$("#o_c"+i).text(org.Countnumber);
			$("#j"+i).attr("oid",org.OrganizationID);
			$("#o_"+i).fadeIn(i*500);
		}
	});
});
$(document).on("pagebeforeshow","#org_search",function(){
	$.get("/Organization/get_org_byname",{"name":$("#o_name").val()},function(data){
		var obj=eval("("+data+")");
		for(var i=0;i<20;i++){$("#os_"+i).hide();}
		for(var i=0;i<20;i++){
			var org=obj.org_byclass[i];
			$("#os_a"+i).attr("src","http://www.sheyou.me/admin/images/"+org.Logo);
			$("#os_b"+i).text(org.OrganizationName);
			$("#os_c"+i).text(org.Countnumber);
			$("#k"+i).attr("oid",org.OrganizationID);
			$("#os_"+i).fadeIn(i*500);
		}
	});
});
$(document).on("pagebeforeshow","#act_detail",function(){
	$.get("/Activity/get_activityx_info",{"actid":aid},function(data){
		var obj=eval("("+data+")");
		var detail=obj.activityx_info[0];
		$("#ad_a").text(detail.ActivityName);
		$("#ad_b").html("<img src=\"http://www.sheyou.me/admin/images/"+detail.PhotoDir+"\" width=\"100%\">");
		$("#ad_c").text(detail.ActivirtyContent);
	});
});
$(document).on("pagebeforeshow","#org_detail",function(){
	$.get("/Organization/get_orgx_info",{"orgid":oid},function(data){
		var obj=eval("("+data+")");
		var od=obj.orgx_info[0];
		$("#od_a").text(od.OrganizationName);
		$("#od_b").html("<img src=\"http://www.sheyou.me/admin/images/"+od.Logo+"\" width=\"100%\">");
		$("#od_c").text(od.Introduction);
	});
});