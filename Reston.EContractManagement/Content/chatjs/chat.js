$(function () {
		$(".chat-window-title").parent().find(".chat-window-content").toggle();
		$(document).on("click", ".chat-window-title", function () {
            $(this).parent().find(".chat-window-content").toggle();
        });
		$(".user-list-item").on("click",function(){
			var username=$(this).find(".content-chat").text();
			var src=$(this).find("img").attr("src");
			
			if($("#"+username).length == 0)
				createPrivateChatWindow(username,src);
		});
		$("#divContainer").on("click",".close",function(){
			$(this).parent().parent().remove();
			$("#divContainer").find(".chat-window").each(function (index, el) {
				$(el).css("right", (index + 1) * 246 + "px");
			});
		});
	  })
	  
	  
	  function createPrivateChatWindow(userName,avatar) {
			var jumChatWindow=$("#divContainer").find(".chat-window").length;
			var windowPosition = (jumChatWindow + 1) * 246;
			var div = '<div id="'+userName+'" skip="0"   class="chat-window" style="right: ' + windowPosition + 'px;">' +
					'<div class="chat-window-title">' +
					   ' <div class="close"></div>' +
						'<div class="text"><img class="window-picture " src="'+avatar+'" >' + userName + '</div>' +
					'</div>' +
					'<div class="chat-window-content">' +
						'<div class="chat-window-inner-content message-board pm-window" style="height: 235px;">' +
							'<div class="messages-wrapper" style="height: 214px;">' +
							
							'</div>' +
							'<div class="chat-window-text-box-wrapper">' +
								'<textarea rows="1" class="chat-window-text-box" style="overflow: hidden; word-wrap: break-word; resize: none; height: 21px;"></textarea>' +
							'</div>' +
					   ' </div>' +
					'</div>' +
				'</div>';
			 $('#divContainer').prepend(div);
		}