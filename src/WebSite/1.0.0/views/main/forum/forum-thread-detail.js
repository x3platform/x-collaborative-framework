x.register('main.forum.thread.detail');

main.forum.thread.detail = {

  threadurl: '/services/Elane/X/Forum/Ajax.ForumThreadWrapper.aspx',

  commentUrl: '/services/Elane/X/Forum/Ajax.ForumCommentWrapper.aspx',

  optionUrl: '/services/Elane/X/Forum/Ajax.ForumThreadOptionWrapper.aspx',

  maskWrapper: x.ui.mask.newMaskWrapper('main.forum.thread.detail.maskWrapper'),

  showColor: ['#56B9F9', '#8BBA00', '#FDC12E', '#E92725', '#C9198D', '#FF8E46', '#008E8E', '#8E468E', '#588526', '#B3AA00', '#008ED6', '#9D080D', '#A186BE', '#9966CC', '#666600', '#003366', '#FF9933', '#99CC00', '#99CCCC', '#0099CC', '#006699', '#336633', '#CC9999', '#B3AA00', '#CCCCFF', '#CC6699', '#CC3399', '#CCCC33', '#339933', '#CC0033'],

  paging: x.page.newPagingHelper(20),

  isShowFloor: true,

  isOnlyShow: false,

  onlyAccountId: '',

  /*#region 函数:getPaging(currentPage, flag)*/
  /**
  * 分页
  */
  getPaging: function(currentPage, flag)
  {
    var paging = main.forum.thread.detail.paging;

    paging.currentPage = currentPage;

    paging.pageSize = $("#pageSize").val();

    var threadId = $("#threadId").val().trim();

    if(threadId != '')
    {
      paging.query.scence = 'Query';
      paging.query.where.ThreadId = threadId;

      // 只显示某个人
      if(main.forum.thread.detail.isOnlyShow && main.forum.thread.detail.onlyAccountId != "")
      {
        paging.query.where.AccountId = main.forum.thread.detail.onlyAccountId;
        paging.query.where.Anonymous = 0;
      }

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += paging.toXml();
      outString += '</request>';

      x.net.xhr('/api/forum.comment.query.aspx', outString, {
        waitingType: 'mini',
        waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
        callback: function(response)
        {
          var result = x.toJSON(response);

          var list = result.data;

          var paging = main.forum.thread.detail.paging;

          paging.load(result.paging);

          $('#window-main-table-container').html(main.forum.thread.detail.getObjectsView(list));

          if(main.forum.thread.detail.paging.currentPage == 1)
          {
            // 加载投票信息
            main.forum.thread.detail.vote.loadVote();

            // 加载主贴附件
            if($('#' + $('#threadId').val()).size() > 0)
            {
              x.app.files.findAll({
                entityId: $('#threadId').val(),
                entityClassName: 'X3Platform.Plugins.Forum.Model.ForumThreadInfo, X3Platform.Plugins.Forum',
                readonly: 1,
                targetViewName: $('#threadId').val()
              });

              // x.attachment.findAllOnlyReadCompact($('#threadId').val(), 'X3Platform.Plugins.Forum.Model.ForumThreadInfo, X3Platform.Plugins.Forum', $('#threadId').val());
            }
          }

          // 页码信息
          var footerHtml = paging.tryParseMenu('javascript:main.forum.thread.detail.getPaging({0},1);');

          // var begin = footerHtml.indexOf('<a');
          // var end = footerHtml.lastIndexOf('a>');
          // $(".forum-thread-view-enterprise-paging").html(footerHtml.substring(begin, end + 2));
          $(".forum-thread-view-enterprise-paging").html(footerHtml);

          // 加载更多回帖信息
          main.forum.thread.detail.getMoreCommentView();

          // 加载回帖附件
          $(".forum-thread-view-bottom-content-other-attachment").each(function()
          {
            var entityId = $(this).attr("id");
            if(entityId != "")
            {
              // 回调后执行的方法
              x.attachment.findOnlyReadCompact(entityId, 'X3Platform.Plugins.Forum.Model.ForumCommentInfo, X3Platform.Plugins.Forum', entityId);
            }
          });

          // 显示到指定楼层(论坛电梯)
          if(main.forum.thread.detail.isShowFloor)
          {
            var showFloor = $("#showFloor").val().trim();
            if(showFloor != "")
            {
              var selector = "#windowMainTableContainer DIV[showFloor = '" + showFloor + "']";
              main.forum.thread.detail.pagePosition($(selector).offset().top);
            }
            main.forum.thread.detail.isShowFloor = false;
          }

          // 设置flash显示模式
          $("embed").attr("wmode", "transparent");
        }
      });
    }

    // 页面滚动
    switch(flag)
    {
      case 1:
        main.forum.thread.detail.pagePosition("min");
        break;
      case 2:
        main.forum.thread.detail.pagePosition("max");
        break;
    }
  },

  /*
  * 创建对象列表的视图
  */
  getObjectsView: function(list)
  {
    var outString = '';

    x.each(list, function(index, node)
    {
      // 用户名连接
      var person = '';
      var displayName = '匿名';
      if(node.anonymous == 1)
      {
        person = "匿名"; // 隐藏数据
      }
      else
      {
        person = '<a href="forum-member-view.aspx?id=' + node.accountId + '" target="_blank" >' + node.accountName + '</a>';
        displayName = node.accountName;
      }

      // 得到帖子时间
      var time = main.forum.util.getTimeHelper(node.createdDate);

      // 得到楼层说明
      var showFloor = main.forum.thread.detail.showFloor(node.rowIndex);

      // 回帖中回复某人信息            
      var replyCommentHtml = '';
      if(node.replyCommentId != "")
      {
        var replyCommentTime = main.forum.util.getTimeHelper(node.replyCommentCreateDate);
        replyCommentHtml += '<table class="forum-thread-view-bottom-content-reply">';
        replyCommentHtml += '<tr><td class="forum-thread-view-bottom-content-reply-left">';
        replyCommentHtml += '<div style="width: 28px; height: 27px; background-image: url(\'/resources/images/forum/icon_quote_s.gif\')"></div>';
        replyCommentHtml += '</td><td class="forum-thread-view-bottom-content-reply-middle">';
        replyCommentHtml += '<span style="font-size: 12px;">' + main.forum.thread.detail.showFloor(node.replyCommentRowIndex) + '<span style="color: #C2D5E3;"> | </span>' + node.replyCommentName + '<span style="color: #C2D5E3;"> | </span>' + ' 发表于 ' + replyCommentTime + '</span>';
        replyCommentHtml += '<div class="forum-thread-view-bottom-reply-content">' + node.replyCommentContent + '</div>';
        replyCommentHtml += '</td><td class="forum-thread-view-bottom-content-reply-right">';
        replyCommentHtml += '<div style="width: 28px; height: 27px; background-image: url(\'/resources/images/forum/icon_quote_e.gif\')"></div>';
        replyCommentHtml += '</td></tr></table>';
      }

      // 回复功能
      var repley = '';
      if(node.rowIndex == 1)
      {
        // repley = '<a href="javascript:main.forum.thread.detail.useBack(\'\',\'' + showFloor + '\',\'' + displayName + '\',\'' + time + '\')"><img src="/resources/images/forum/back.png" alt="" class="forum-thread-enterprise-view-img" />回复</a>';
        repley = '<a href="javascript:main.forum.thread.detail.useBack(\'\',\'' + showFloor + '\',\'' + displayName + '\',\'' + time + '\')"><i class="fa fa-reply"></i> 回复</a>';
      }
      else
      {
        // repley = '<a href="javascript:main.forum.thread.detail.useBack(\'' + node.id + '\',\'' + showFloor + '\',\'' + displayName + '\',\'' + time + '\')"><img src="/resources/images/forum/back.png" alt="" class="forum-thread-enterprise-view-img" />回复</a>';
        repley = '<a href="javascript:main.forum.thread.detail.useBack(\'' + node.id + '\',\'' + showFloor + '\',\'' + displayName + '\',\'' + time + '\')"><i class="fa fa-reply"></i> 回复</a>';
      }

      // 编辑功能
      var edit = ''
      var isEdit = false;
      if($("#isAdminToken").val() == "True" || $("#categoryAdministrator").val() == "True")
      {
        isEdit = true;
      }
      else
      {
        if($("#userId").val() == node.accountId && node.anonymous == 0)
        {
          var dayCountStr = $("#dayCountStr").val();
          switch(dayCountStr)
          {
            case "on":
              isEdit = true;
              break;
            default:
              var dayCount = parseInt(dayCountStr, 10);
              if(!isNaN(dayCount) && dayCount != 0)
              {
                var strDate = node.createdDate.split(',');
                var createdDate = new Date(strDate[0], parseInt(strDate[1], 10) - 1, strDate[2], strDate[3], strDate[4], strDate[5]);
                // 得到当前时间
                var nowDate = new Date(Date.parse($("#nowDate").val().replace(/-/g, "/")));
                var day = (nowDate.getTime() - createdDate.getTime()) / (24 * 60 * 60 * 1000)
                if(day <= dayCount)
                {
                  isEdit = true;
                }
              }
              break;
          }
        }
      }

      if(isEdit)
      {
        if(node.rowIndex == 1)
        {
          // edit = '<a href="forum-thread-form.aspx?id=' + node.id + '"><img src="/resources/images/forum/edit.png" alt="" class="forum-thread-enterprise-view-img" />编辑</a>';
          edit = '<a href="forum/form?id=' + node.id + '" ><i class="fa fa-pencil"></i> 编辑</a>';
        }
        else
        {
          // edit = '<a href="javascript:main.forum.thread.detail.updateContent(\'' + node.id + '\')"><img src="/resources/images/forum/edit.png" alt="" class="forum-thread-enterprise-view-img" />编辑</a>';
          edit = '<a href="javascript:main.forum.thread.detail.updateContent(\'' + node.id + '\')"><i class="fa fa-pencil"></i> 编辑</a>';
        }
      }

      // 删除功能
      var remove = "";
      if($("#isAdminToken").val() == "True")
      {
        if(node.rowIndex == 1)
        {
          // <i class="fa fa-times"></i>
          // remove = '<a href="javascript:main.forum.thread.detail.confirmDelete(\'\')"><img src="/resources/images/forum/delete.png" alt="" class="forum-thread-enterprise-view-img" />删除</a>';
          remove = '<a href="javascript:main.forum.thread.detail.confirmDelete(\'\')"><i class="fa fa-times"></i> 删除</a>';
        }
        else
        {
          // remove = '<a href="javascript:main.forum.thread.detail.confirmDelete(\'' + node.id + '\')"><img src="/resources/images/forum/delete.png" alt="" class="forum-thread-enterprise-view-img" />删除</a>';
          remove = '<a href="javascript:main.forum.thread.detail.confirmDelete(\'' + node.id + '\')"><i class="fa fa-times"></i> 删除</a>';
        }
      }

      /*********************** 拼装帖子信息 **************************/
      outString += '<tr>';
      outString += '<td class="forum-thread-detail-layout-left">';
      outString += '<div class="forum-thread-detail-layout-header">';
      outString += '<span class="forum-thread-view-enterprise-floor">';
      outString += '<a href="javascript:main.forum.thread.detail.copyFloor(\'' + node.id + '\')" title="点击我复制楼层链接">' + showFloor + '</a>';
      outString += '</span><span class="forum-thread-view-enterprise-line"> | </span>';
      outString += '<span class="forum-thread-view-enterprise-person">' + person + '</span>';
      outString += '</div>';
      outString += '</td>';
      outString += '<td class="forum-thread-detail-layout-right">';
      outString += '<div class="forum-thread-detail-layout-header" showFloor="' + node.id + '">';
      outString += '发表于 ' + time;
      if(node.anonymous == 0)
      {
        outString += '<span class="forum-thread-view-enterprise-line"> | </span>';
        outString += ' <a href="javascript:main.forum.thread.detail.onlyAuthor(\'' + node.accountId + '\')">';
        if(main.forum.thread.detail.isOnlyShow)
        {
          outString += '显示全部楼层';
        }
        else
        {
          outString += '只查看该作者';
        }
        outString += '</a>';
      }
      outString += '</div>';
      outString += '</td>';
      outString += '</tr>';

      outString += '<tr>';
      outString += '<td class="forum-thread-detail-layout-left" rowspan="2">';
      // 判断是否是匿名用户
      if(node.anonymous == 0)
      {
        outString += '<div class="forum-thread-view-enterprise-iconPath"><img src="' + node.memberIconPath + '" alt="" /></div>';
        outString += '<table class="forum-thread-view-enterprise-info"><tr>';
        outString += '<td style="width: 30px;">来自</td><td>' + node.memberOrganizationPath + '</td>';
        outString += '</tr><tr>';
        outString += '<td style="width: 30px;">岗位</td><td>' + node.memberHeadship + '</td>';
        outString += '</tr><tr>';
        outString += '<td style="width: 30px;">积分</td><td>' + node.memberScore + '</td>';
        outString += '</tr><tr>';
        outString += '<td style="width: 30px;">帖子</td><td>' + node.memberThreadCount + '</td>';
        outString += '</tr></table>';
      }
      else
      {
        outString += '<div class="forum-thread-view-enterprise-iconPath"><img src="/uploads/avatar/anonymous_120x120.png" alt=""/></div>';
      }
      outString += '</td>';
      outString += '<td class="forum-thread-detail-layout-right">';
      outString += '<div class="forum-thread-detail-content-body">' + replyCommentHtml + node.content + '</div>';
      // 帖子投票
      if($("#sign").val() == "vote" && node.rowIndex == 1 && node.Id == node.ThreadId)
      {
        outString += '<div class="forum-thread-view-bottom-content-vote"></div>';
      }
      //  发帖心情
      if(node.heart != "")
      {
        outString += '<div class="forum-thread-view-bottom-content-other"><img src="' + node.heart + '" alt="" style="width: 50px; height: 50px;" /></div>';
      }

      // 回帖修改信息
      if(node.updateInfo != "" && node.anonymous == 0)
      {
        outString += '<div class="forum-thread-view-bottom-content-update">' + node.updateHistoryLog + '</div>';
      }
      outString += '</td>';
      outString += '</tr>';

      outString += '<tr>';
      outString += '<td class="forum-thread-detail-layout-right" style="border-top:none;vertical-align: bottom;" >';
      // 附件
      if(node.attachmentFileCount == 1)
      {
        outString += '<div class="forum-thread-view-bottom-content-other">';
        outString += '<div class="forum-thread-view-bottom-content-other-title">附件</div>';
        if(node.rowIndex == 1)
        {
          outString += '<div id="' + node.id + '" class="forum-thread-view-bottom-content-other-content"></div>';
        }
        else
        {
          outString += '<div id="' + node.id + '" class="forum-thread-view-bottom-content-other-content forum-thread-view-bottom-content-other-attachment"></div>';
        }
        outString += '</div>';
      }
      // 是否有更多回帖
      if(node.commentNum > 1)
      {
        outString += '<input class="enterprise-more" type="hidden" value="' + node.id + '" />';
        outString += '<input class="enterprise-more" type="hidden" value="' + node.accountId + '" />';
        outString += '<input class="enterprise-more" type="hidden" value="' + node.anonymous + '" />';
      }
      // 个人签名
      if(node.anonymous == 0)
      {
        if(node.memberSignature == "")
        {
          outString += '<div class="forum-thread-view-enterprise-sign"><div class="forum-thread-view-enterprise-sign-title"></div><div class="forum-thread-view-enterprise-sign-content">这家伙很懒，什么都没留下...</div></div>';
        }
        else
        {
          outString += '<div class="forum-thread-view-enterprise-sign"><div class="forum-thread-view-enterprise-sign-title"></div><div class="forum-thread-view-enterprise-sign-content">' + node.memberSignature + '</div></div>';
        }
      }
      outString += '</td>';
      outString += '</tr>';

      // 数据操作
      outString += '<tr>';
      outString += '<td class="forum-thread-detail-layout-left"></td>';
      outString += '<td class="forum-thread-detail-layout-right">';
      outString += '<div class="forum-thread-detail-layout-footer" >' + repley + '<span style="margin-left:10px">' + edit + '</span><span style="margin-left:10px">' + remove + '</span>' + '</div>';
      outString += '</td>';
      outString += '</tr>';

      outString += '<tr><td class="forum-thread-detail-separator-left"></td><td class="forum-thread-detail-separator-right"></td></tr>';
    });

    return '<table class="" style="width:100%" >' + outString + '</table>';
  },
  /*#endregion*/

  /*#region 函数:getMoreCommentView()*/
  /*
  * 加载更多回帖信息
  */
  getMoreCommentView: function()
  {
    $(".forum-thread-detail-layout-right").each(function()
    {
      if($(this).children(".enterprise-more").length > 0)
      {
        var $temp = $(this);
        var whereClause = '[ThreadId] = ##' + $("#threadId").val() + '## @@ [Id] != ##' + $(this).children(".enterprise-more:first").val() + '## AND [Id] != [ThreadId] AND [AccountId] = ##' + $(this).children(".enterprise-more:eq(1)").val() + '## AND [Anonymous] = ' + $(this).children(".enterprise-more:last").val();

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';
        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[getMoreComment]]></action>';
        outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
        outString += '<whereClause><![CDATA[' + whereClause + ']]></whereClause>';
        outString += '</ajaxStorage>';

        var options = {
          resultType: 'json',
          xml: outString
        };

        $.ajax({
          type: "POST",
          async: false,
          url: main.forum.thread.detail.commentUrl,
          data: options,
          success: function(response)
          {
            x.net.fetchException(response);

            var result = x.toJSON(response);

            var list = result.ajaxStorage;

            var outString = '';
            outString += '<div class="forum-thread-view-bottom-content-other">';
            outString += '<div class="forum-thread-view-bottom-content-other-title">本用户其他回帖楼层</div>';
            outString += '<div class="forum-thread-view-bottom-content-other-content">';
            list.each(function(node, list)
            {
              var floorUrl = window.location.href;
              if(floorUrl.indexOf("&") > 0)
              {
                floorUrl = floorUrl.substring(0, floorUrl.indexOf("&"))
              }
              floorUrl += "&showFloor=" + node.id;
              outString += '<span class="forum-thread-view-enterprise-floor"><a title="点击我查看该楼层回帖" href="' + floorUrl + '">' + main.forum.thread.detail.showFloor(node.rowIndex) + '</a></span>';
            });
            outString += '</div>';
            outString += '</div>';

            $($temp).append(outString);
          }
        });
      }
    });
  },
  /*#endregion*/

  /*#region 函数:onlyAuthor(accountId)*/
  /*
  * 只查看作者
  */
  onlyAuthor: function(accountId)
  {
    if(main.forum.thread.detail.isOnlyShow)
    {
      main.forum.thread.detail.isOnlyShow = false;
      main.forum.thread.detail.onlyAccountId = "";
    }
    else
    {
      main.forum.thread.detail.isOnlyShow = true;
      main.forum.thread.detail.onlyAccountId = accountId;
    }

    main.forum.thread.detail.getPaging(1, 1);
  },
  /*#endregion*/

  /*#region 函数:copyFloor(showFloor)*/
  /*
  * 复制楼层地址
  */
  copyFloor: function(showFloor)
  {
    var floorUrl = window.location.href;
    if(floorUrl.indexOf("&") > 0)
    {
      floorUrl = floorUrl.substring(0, floorUrl.indexOf("&"))
    }
    floorUrl += "&showFloor=" + showFloor;

    if(window.clipboardData)
    {
      window.clipboardData.setData("Text", floorUrl);
      alert("已将楼层链接复制到剪贴板");
    }
    else if(window.netscape)
    {
      window.prompt("请按Ctrl+C复制到剪贴板", floorUrl);
    }
  },
  /*#endregion*/

  /*#region 函数:showFloor()*/
  /*
  * 展示楼层说明
  */
  showFloor: function(rowIndex)
  {
    var outString = '';

    switch(parseInt(rowIndex, 10))
    {
      case 1:
        outString = '楼主';
        break;
      case 2:
        outString = '沙发';
        break;
      case 3:
        outString = '板凳';
        break;
      case 4:
        outString = '地板';
        break;
      default:
        outString = rowIndex + '楼';
        break;
    }

    return outString;
  },
  /*#endregion*/

  /*#region 函数:pagePosition()*/
  /*
  * 快速页面定位
  */
  pagePosition: function(tag)
  {
    var value = 0;
    var objStr = "body";
    switch(tag)
    {
      case "min":
        if(x.browser.ie6)
        {
          objStr = ".web-body";
        }
        value = 0;
        break;
      case "max":
        if(x.browser.ie6)
        {
          objStr = ".web-body";
          value = document.getElementById("windowContainer").scrollHeight;
        }
        else
        {
          value = document.body.scrollHeight;
        }
        break;
      default:
        if(x.browser.ie6)
        {
          objStr = ".web-body";
        }
        if(!isNaN(parseInt(tag, 10)))
        {
          value = parseInt(tag, 10);
        }
        break;
    }

    $(objStr).animate({ scrollTop: value }, 100);
  },
  /*#endregion*/

  /*#region 函数:useBack(id, showFloor, name, time)*/
  /*
  * 回复
  */
  useBack: function(id, showFloor, name, time)
  {
    var outString = '您正在回复<span style="color: #FF6600; font-weight: bold; padding-left: 10px;">' + showFloor + '</span>&nbsp;&nbsp;<span class="person">' + name + '</span> <span class="text">发表于&nbsp;' + time + '</span>&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:main.forum.thread.detail.cancelUseBack()">取消回复</a>';

    $("#useInfoTip").html(outString);
    $("#useShow").val(id);
    $("#useInfoTip").show();

    main.forum.thread.detail.pagePosition("max");
  },

  cancelUseBack: function()
  {
    $("#useInfoTip").hide();
    $("#useInfoTip").html("");
    $("#useShow").val("");
  },
  /*#endregion*/

  /*#region 函数:save()*/
  /*
  * 发表回帖
  */
  save: function()
  {
    // var editor = FCKeditorAPI.GetInstance('content');
    // var content = editor.GetXHTML(true);

    var editor = CKEDITOR.instances['content']
    var content = editor.getData();

    var error = "";
    if(content == "")
    {
      alert("回复内容不能为空！");
      return;
    }

    var anonymous = 0;
    if($("#anonymous").attr("checked") == true)
    {
      anonymous = 1;
    }

    var outString = '<?xml version="1.0" encoding="utf-8"?>';
    outString += '<request>';
    outString += '<action><![CDATA[save]]></action>';
    outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
    outString += '<id><![CDATA[' + $("#commentId").val() + ']]></id>';
    outString += '<accountId><![CDATA[' + $("#userId").val() + ']]></accountId>';
    outString += '<accountName><![CDATA[' + $("#userDisplayName").val() + ']]></accountName>';
    outString += '<threadId><![CDATA[' + $("#threadId").val() + ']]></threadId>';
    outString += '<categoryId><![CDATA[' + $("#categoryId").val() + ']]></categoryId>';
    outString += '<replyCommentId><![CDATA[' + $("#useShow").val() + ']]></replyCommentId>';
    outString += '<content><![CDATA[' + content + ']]></content>';
    outString += '<heart><![CDATA[' + $("#heart").val() + ']]></heart>';
    outString += '<anonymous><![CDATA[' + anonymous + ']]></anonymous>';
    outString += '</request>';

    x.net.xhr('/api/forum.comment.save.aspx', outString, {
      waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
      callback: function(response)
      {
        var result = x.toJSON(response).message;

        var paging = main.forum.thread.detail.paging;
        if(paging.currentPage == paging.lastPage)
        {
          main.forum.thread.detail.getPaging(paging.currentPage, 2);
        }

        // 获取最新回复信息
        main.forum.thread.detail.cancelUseBack();
        // 取消匿名选择
        $("#anonymous").attr("checked", false);
        $(".heart").removeClass("slHeart");
        $("#heart").val("");
        $("#commentId").val(result.commentId);

        // 附件src更新
        // var temp = $("#uploadFileWizard").attr("src").split("entityId=");
        // var left = temp[0];
        // var right = temp[1].substring(36, temp[1].length);
        // var url = left + "entityId=" + $("#commentId").val() + right;
        // $("#uploadFileWizard").attr("src", url);

        // 清空附件和隐藏扩展功能
        // $("#windowAttachmentContainer").html("");
        // $(".toolContent").slideUp(100);

        // 清空文本编辑器

        // var editor = FCKeditorAPI.GetInstance('content');
        // editor.SetHTML("", false);
        var editor = CKEDITOR.instances['content'];
        editor.setData('');

        alert(result.value);
      }
    });
  },
  /*#endregion*/

  /*#region 函数:slHeart(obj)*/
  // 选择心情添加样式
  slHeart: function(obj)
  {
    if($(obj).attr("class") == "heart slHeart")
    {
      $(".heart").removeClass("slHeart");
      $("#heart").val("");
    }
    else
    {
      $(obj).addClass("slHeart")
      $(".heart").not($(obj)).removeClass("slHeart");
      $("#heart").val($(obj).attr("src"));
    }
  },
  /*#endregion*/

  /*#region 函数:updateContent(id)*/
  /*
  * 修改回帖信息
  */
  updateContent: function(id)
  {
    // 查询内容
    var outString = '<?xml version="1.0" encoding="utf-8"?>';
    outString += '<ajaxStorage>';
    outString += '<action><![CDATA[findOne]]></action>';
    outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
    outString += '<id><![CDATA[' + id + ']]></id>';
    outString += '</ajaxStorage>';

    var options = {
      resultType: 'json',
      xml: outString
    };

    $.post(
                main.forum.thread.detail.commentUrl,
                options,
                main.forum.thread.detail.updateContent_callback);

  },

  updateContent_callback: function(response)
  {
    // 抛异常
    x.net.fetchException(response);

    var result = x.toJSON(response);
    var info = result.ajaxStorage;

    var outString = '';
    outString += '<div id="windowCategoryWizardWrapper" class="winodw-wizard-wrapper" style="width:736px; height:auto;">';

    outString += '<div class="winodw-wizard-toolbar">';
    outString += '<div class="winodw-wizard-toolbar-close">';
    outString += '<a href="javascript:main.forum.thread.detail.maskWrapper.close();" class="button-text" >关闭</a>';
    outString += '</div>';
    outString += '<div class="float-left">';
    outString += '<div class="winodw-wizard-toolbar-item"><span>编辑回帖内容</span></div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';

    outString += '<table class="table-style" style="width:100%;">';

    outString += '<tr>';
    outString += '<td class="table-body-input" >';
    outString += '<textarea name="updateContent" id="updateContent" style="display:none;">' + info.content + '</textarea>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr>';
    outString += '<td class="table-body-input">';
    outString += '<div style="margin:0 0 4px 2px;">';
    outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;">';
    outString += '<a id="save" class="button-text" href="javascript:main.forum.thread.detail.saveContent(\'' + info.id + '\');">确定</a>';
    outString += '</div>';
    outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;">';
    outString += '<a id="cancel" class="button-text" href="javascript:main.forum.thread.detail.maskWrapper.close();">取消</a>';
    outString += '</div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '</table>';
    outString += '<input id="tempAccountId" name="tempAccountId" type="hidden" value="' + info.accountId + '" />';
    outString += '<input id="tempAccountName" name="tempAccountName" type="hidden" value="' + info.accountName + '" />';

    outString += '<div class="clear" ></div>';
    outString += '</div>';

    // 加载遮罩和页面内容
    x.mask.getWindow(outString, main.forum.thread.detail.maskWrapper);

    // 创建文本编辑器
    var editor = new FCKeditor('updateContent');
    editor.Width = 720;
    editor.Height = 400;
    editor.BasePath = '/resources/editor/fckeditor/';
    editor.ToolbarSet = 'ForumComment_Toolbar';
    editor.ReplaceTextarea();
  },
  /*#endregion*/

  /*#region 函数:saveContent(id)*/
  /*
  * 保存修改回帖信息
  */
  saveContent: function(id)
  {
    var editor = FCKeditorAPI.GetInstance('updateContent');
    var content = editor.GetXHTML(true);

    var error = "";
    if(content == "")
    {
      alert("内容不能为空！");
      return;
    }

    var time = new Date();
    var updateInfo = "该文章已在" + time.getFullYear() + '-' + (Number(time.getMonth()) + 1) + '-' + time.getDate() + ' ' + time.toLocaleTimeString() + "被" + $("#userDisplayName").val() + "编辑过";

    var outString = '<?xml version="1.0" encoding="utf-8"?>';
    outString += '<ajaxStorage>';
    outString += '<action><![CDATA[save]]></action>';
    outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
    outString += '<id><![CDATA[' + id + ']]></id>';
    outString += '<accountId><![CDATA[' + $("#tempAccountId").val() + ']]></accountId>';
    outString += '<accountName><![CDATA[' + $("#tempAccountName").val() + ']]></accountName>';
    outString += '<content><![CDATA[' + content + ']]></content>';
    outString += '<updateInfo><![CDATA[' + updateInfo + ']]></updateInfo>';
    outString += '</ajaxStorage>';

    var options = {
      resultType: 'json',
      xml: outString
    };

    $.post(
                main.forum.thread.detail.commentUrl,
                options,
                main.forum.thread.detail.saveContent_callback);
  },

  saveContent_callback: function(response)
  {
    x.net.fetchException(response);

    var result = x.toJSON(response).message;
    switch(Number(result.returnCode))
    {
      case 0:
        // 获取最新回复信息
        main.forum.thread.detail.getPaging(main.forum.thread.detail.paging.currentPage, 0);
        // 清空文本编辑器
        var editor = FCKeditorAPI.GetInstance('updateContent');
        editor.SetHTML("", false);
        // 关闭窗口
        main.forum.thread.detail.maskWrapper.close();
        alert(result.value);
        break;
      default:
        alert(result.value);
        break;
    }
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(id)*/
  /*
  * 删除帖子或回帖
  */
  confirmDelete: function(id)
  {
    var info = "确定永久删除该回帖及相关信息吗？";

    var url = main.forum.thread.detail.commentUrl
    if(id == "")
    {
      info = "确定永久删除该帖子及相关信息吗？";
      id = $("#threadId").val();
      url = main.forum.thread.detail.threadurl;
    }

    if(confirm(info))
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';
      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[delete]]></action>';
      outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
      outString += '<id><![CDATA[' + id + ']]></id>';
      outString += '<threadId><![CDATA[' + $("#threadId").val() + ']]></threadId>';
      outString += '<accountId><![CDATA[' + $("#accountId").val() + ']]></accountId>';
      outString += '<sign><![CDATA[' + $("#sign").val() + ']]></sign>';
      outString += '</ajaxStorage>';
      var options = {
        resultType: 'json',
        xml: outString
      };
      $.post(url, options, this.confirmDelete_callback);
    }
  },

  confirmDelete_callback: function(response)
  {
    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
        // 获取最新回复信息 
        main.forum.thread.detail.getPaging(main.forum.thread.detail.paging.currentPage, 0);
        alert('删除回帖信息成功！');
        break;
      case 1:
        window.opener.window$refresh$callback();
        alert('删除帖子信息成功！');
        x.page.close();
        break;
      default:
        alert(result.value);
        break;
    }
  },
  /*#endregion*/

  /*#region 函数:setEssential(id)*/
  /*
  * 设置精华帖
  */
  setEssential: function(id)
  {
    var isEssential = $("#isEssential").val();

    switch(isEssential)
    {
      case "0":
        isEssential = 1;
        break;
      case "1":
        isEssential = 0;
        break;
      default:
        isEssential = 0;
        break;
    }

    var outString = '<?xml version="1.0" encoding="utf-8"?>';

    outString += '<request>';
    outString += '<action><![CDATA[setEssential]]></action>';
    outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
    outString += '<id><![CDATA[' + id + ']]></id>';
    outString += '<isEssential><![CDATA[' + isEssential + ']]></isEssential>';
    outString += '</request>';

    var options = {
      resultType: 'json',
      xml: outString
    };

    $.post(
                main.forum.thread.detail.threadurl,
                options,
                main.forum.thread.detail.setEssential_callback);
  },

  setEssential_callback: function(response)
  {
    var result = x.toJSON(response).message;

    var num = Number(result.returnCode);

    switch(num)
    {
      case 0:
        alert("取消精华成功！");
        break;
      case 1:
        alert("设置精华成功！");
        break;
      default:
        alert(result.value);
        break;
    }

    $("#isEssential").val(num);
    main.forum.thread.detail.showIsEssentialTip();
  },
  /*#endregion*/

  /*#region 函数:setTop(id)*/
  /*
  * 设置置顶帖
  */
  setTop: function(id)
  {
    var isTop = $("#isTop").val();
    switch(isTop)
    {
      case "0":
        isTop = 1;
        break;
      case "1":
        isTop = 0;
        break;
      default:
        isTop = 0;
        break;
    }

    var outString = '<?xml version="1.0" encoding="utf-8"?>';
    outString += '<ajaxStorage>';
    outString += '<action><![CDATA[setTop]]></action>';
    outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
    outString += '<id><![CDATA[' + id + ']]></id>';
    outString += '<isTop><![CDATA[' + isTop + ']]></isTop>';
    outString += '</ajaxStorage>';

    var options = {
      resultType: 'json',
      xml: outString
    };

    $.post(
                main.forum.thread.detail.threadurl,
                options,
                main.forum.thread.detail.setTop_callback);
  },

  setTop_callback: function(response)
  {
    var result = x.toJSON(response).message;

    var num = Number(result.returnCode);

    switch(num)
    {
      case 0:
        alert("取消置顶成功！");
        break;
      case 1:
        alert("设置置顶成功！");
        break;
      default:
        alert(result.value);
        break;
    }

    $("#isTop").val(num);
    main.forum.thread.detail.showIsTopTip();
  },
  /*#endregion*/

  /*#region 函数:showIsEssentialTip(),showIsTopTip()*/
  showIsEssentialTip: function()
  {
    if($("#isEssential").val() == "0")
    {
      $("#isEssentialTip").html("设置精华");
    }
    else
    {
      $("#isEssentialTip").html("取消精华");
    }
  },

  showIsTopTip: function()
  {
    if($("#isTop").val() == "0")
    {
      $("#isTopTip").html("设置置顶");
    }
    else
    {
      $("#isTopTip").html("取消置顶");
    }
  },
  /*#endregion*/

  /*#region 函数:resizeWindow()*/
  resizeWindow: function()
  {
    var width = document.body.offsetWidth - 280;
    if(width >= 700)
    {
      $(".forum-thread-view-bottom-show").css("width", width);
      $(".forum-thread-view-bottom-content-info img").css("max-width", "100%");
    }
  },
  /*#endregion*/

  vote:
    {
      /*#region 函数:loadVote()*/
      loadVote: function()
      {
        if($(".forum-thread-view-bottom-content-vote").length > 0)
        {
          $(".forum-thread-view-bottom-content-vote").hide();

          var outString = '<?xml version="1.0" encoding="utf-8" ?>';
          outString += '<ajaxStorage>';
          outString += '<action><![CDATA[getVote]]></action>';
          outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
          outString += '<threadId><![CDATA[' + $("#threadId").val() + ']]></threadId>';
          outString += '</ajaxStorage>';

          var options = {
            resultType: 'json',
            xml: outString
          };

          $.post(
              main.forum.thread.detail.optionUrl,
              options,
              function(response)
              {
                // 捕获处理异常
                x.net.fetchException(response);
                var voteInfo = x.toJSON(response).ajaxStorage1;
                var optionList = x.toJSON(response).ajaxStorage2;
                var result = x.toJSON(response).message;

                var outString = '';
                outString += '<div class="content-vote-head">';
                if(voteInfo.maximum == 1)
                {
                  outString += '<strong>单选投票</strong> ';
                }
                else
                {
                  outString += '<strong>多选投票</strong>（最多可选 ' + voteInfo.maximum + ' 项）';
                }
                outString += '共有 ' + voteInfo.joinCount + ' 人参与投票 ';
                if(voteInfo.isPublic == 1 || main.forum.thread.detail.vote.isResultScope())
                {
                  outString += '<a href="javascript:main.forum.thread.detail.vote.showJoin();">查看投票参与人</a> ';
                }
                // 投票结束时间
                var strTime = voteInfo.endDate.split(',');
                var isTime = false;
                if(!(parseInt(strTime[0], 10) == 1 || parseInt(strTime[0], 10) == 1900 || parseInt(strTime[0], 10) == 2000))
                {
                  outString += '投票截止日期：' + strTime[0] + '年' + strTime[1] + '月' + strTime[2] + '日 ';
                  isTime = true;
                }
                outString += '</div>';

                /*
                * 投票选项信息
                */
                var colorIndex = 0;
                optionList.each(function(node, index)
                {
                  var value = node.voteCount > 0 ? (Math.round(parseInt(node.selectCount, 10) / parseInt(node.voteCount, 10) * 10000) / 100) + '%' : '0.0%';

                  outString += '<div class="content-vote-option">';
                  outString += '<div class="content-vote-option-title">';
                  // 是否参与投票
                  if(result.isJoin == 0)
                  {
                    // 单选或者多选
                    if(voteInfo.maximum == 1)
                    {
                      outString += '<input type="radio" name="voteOptioin" value="' + node.id + '" /> ';
                    }
                    else
                    {
                      outString += '<input type="checkbox" name="isResult" value="' + node.id + '" /> '
                    }
                  }
                  outString += node.index + '. ' + node.content;
                  outString += '</div>';

                  // 是否可以查看结果
                  if(voteInfo.isResult == 1 || main.forum.thread.detail.vote.isResultScope())
                  {
                    outString += '<div class="content-vote-option-view">';
                    outString += '<div class="content-vote-option-view-area">';
                    outString += '<div style="width: ' + value + '; height: 100%; background-color: ' + main.forum.thread.detail.showColor[colorIndex] + ';"></div>';
                    outString += '</div>';
                    outString += '<div class="content-vote-option-view-info" style="color: #000000;">' + node.selectCount + ' (' + value + ') </div>';
                    outString += '</div>';
                  }
                  outString += '</div>';

                  colorIndex++;
                  if(colorIndex >= main.forum.thread.detail.showColor.length)
                  {
                    colorIndex = 0;
                  }
                });

                /*
                * 投票信息底栏
                */
                var endDate = new Date(strTime[0], parseInt(strTime[1], 10) - 1, strTime[2], strTime[3], strTime[4], strTime[5]);
                var nowDate = new Date(Date.parse($("#nowDate").val().replace(/-/g, "/")));
                if(isTime && nowDate >= endDate)
                {
                  outString += '<div class="content-vote-foot">该投票已过期，不能投票！</div>';
                }
                else
                {
                  if(result.isJoin == 0)
                  {
                    outString += '<div class="content-vote-foot">';
                    outString += '<div class="button-2font-wrapper">';
                    outString += '<a class="button-text" href="javascript:main.forum.thread.detail.vote.saveVote();">提交</a>';
                    outString += '</div>'
                    outString += '</div>';

                    // 多选添加最多选项限制
                    $(".content-vote-option-title input:checkbox").live("click", function()
                    {
                      if($(".content-vote-option-title input:checked").length >= voteInfo.maximum)
                      {
                        $(".content-vote-option-title input:checkbox").each(function()
                        {
                          if($(this).attr("checked") == false)
                          {
                            $(this).attr("disabled", true);
                          }
                        });
                      }
                      else
                      {
                        $(".content-vote-option-title input:disabled").each(function()
                        {
                          $(this).attr("disabled", false);
                        });
                      }
                    });
                  }
                  else
                  {
                    outString += '<div class="content-vote-foot">您已经投过票，谢谢您的参与！</div>';
                  }
                }

                $(".forum-thread-view-bottom-content-vote").html(outString);
                $(".forum-thread-view-bottom-content-vote").show();
              });
        }
      },
      /*#endregion*/

      /*#region 函数:isResultScope()*/
      /*
      * 是否查看结果权限
      */
      isResultScope: function()
      {
        return $("#isAdminToken").val() == "True" || $("#categoryAdministrator").val() == "True" || $("#accountId").val() == $("#userId").val();
      },
      /*#endregion*/

      /*#region 函数:saveVote()*/
      saveVote: function()
      {
        var optionIds = '';
        $(".content-vote-option .content-vote-option-title input:checked").each(function()
        {
          optionIds += $(this).val() + ',';
        });
        if(optionIds == '')
        {
          alert("您目前未选中任何选项，请选择后提交！");
          return;
        }
        optionIds = optionIds.substring(0, optionIds.length - 1);

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';
        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[saveVote]]></action>';
        outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
        outString += '<threadId><![CDATA[' + $("#threadId").val() + ']]></threadId>';
        outString += '<optionIds><![CDATA[' + optionIds + ']]></optionIds>';
        outString += '</ajaxStorage>';

        var options = {
          resultType: 'json',
          xml: outString
        };

        $.post(
                main.forum.thread.detail.optionUrl,
                options,
                main.forum.thread.detail.vote.saveVote_callback);
      },

      saveVote_callback: function(response)
      {
        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
          case 0:
            alert(result.value);
            main.forum.thread.detail.vote.loadVote();
            break;
          default:
            alert(result.value);
            break;
        }
      },
      /*#endregion*/

      /*#region 函数:showJoin()*/
      showJoin: function()
      {
        var outString = '';
        outString += '<div id="windowJoinWizardWrapper" class="winodw-wizard-wrapper" style="width:736px; height:400px;">';

        outString += '<div class="winodw-wizard-toolbar">';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a href="javascript:main.forum.thread.detail.maskWrapper.close();" class="button-text" >关闭</a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:100px;"><span>查看投票参与人</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%;">';
        outString += '<tr class="table-row-normal">';
        outString += '<td class="table-body-text" style="width:17%;">投票选项</td>';
        outString += '<td class="table-body-input" style="width:83%;" >';
        outString += '<input id="optionId" name="optionId" type="hidden" feature="combobox" topoffset="-1" url="' + main.forum.thread.detail.optionUrl + '" ajaxmethod="getCombobox" comboboxwhereclause="' + $("#threadId").val() + '#' + $("#applicationTag").val() + '" callback="javascript:main.forum.thread.detail.vote.loadJoin();" class="input-normal x-ajax-text" style="width: 500px;" />';
        outString += '</td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td colspan="2">';
        outString += '<div class="content-vote-join">';

        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear" ></div>';
        outString += '</div>';

        // 加载遮罩和页面内容
        x.mask.getWindow(outString, main.forum.thread.detail.maskWrapper);

        // 加载表单元素特性
        x.form.features.bind();
      },

      loadJoin: function()
      {
        if($("#optionId").val() != "")
        {
          var outString = '<?xml version="1.0" encoding="utf-8" ?>';
          outString += '<ajaxStorage>';
          outString += '<action><![CDATA[getJoin]]></action>';
          outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
          outString += '<threadId><![CDATA[' + $("#threadId").val() + ']]></threadId>';
          outString += '<optionId><![CDATA[' + $("#optionId").val() + ']]></optionId>';
          outString += '</ajaxStorage>';

          var options = {
            resultType: 'json',
            xml: outString
          };

          $.post(
              main.forum.thread.detail.optionUrl,
              options,
              function(response)
              {
                // 捕获处理异常
                x.net.fetchException(response);
                var joinInfo = x.toJSON(response).ajaxStorage;

                var outString = '';
                outString += '<table class="table-style" style="width:100%;">';

                var count = 0;
                joinInfo.each(function(node, index)
                {
                  if(count == 0)
                  {
                    outString += '<tr>';
                  }
                  outString += '<td><a href="/forum/member/profile/' + node.accountId + '" target="_blank" >' + node.accountName + '</a></td>';

                  count++;
                  if(count >= 8)
                  {
                    outString += '</tr>';
                    count = 0;
                  }
                });

                if(count < 8)
                {
                  for(var i = count;i < 8;i++)
                  {
                    outString += '<td></td>';
                  }
                  outString += '</tr>';
                }

                outString += '</table>';

                $(".content-vote-join").html(outString);
              });
        }
      }
      /*#endregion*/
    },

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    var currentPage = $("#currentPage").val().trim();

    if(isNaN(parseInt(currentPage, 10)))
    {
      main.forum.thread.detail.getPaging(1, 1);
    }
    else
    {
      main.forum.thread.detail.getPaging(parseInt(currentPage, 10), 1);
    }

    // 显示置顶和精华
    main.forum.thread.detail.showIsEssentialTip();
    main.forum.thread.detail.showIsTopTip();

    CKEDITOR.replace('content', { height: 150 });

    // 创建fck插件
    // var editor = new FCKeditor('content');
    // editor.Width = 720;
    // editor.Height = 150;
    // editor.BasePath = '/resources/editor/fckeditor/';
    // editor.ToolbarSet = 'ForumComment_Toolbar';
    // editor.ReplaceTextarea();

    // 绑定心情单击事件
    $(".heart").bind("click", function()
    {
      main.forum.thread.detail.slHeart($(this));
    });

    // 隐藏展开心情附件功能
    $(".showToolContent").bind("click", function()
    {
      if($(this).attr("title") == "展开")
      {
        $(this).attr("src", "/resources/images/forum/toolbar.collapse.gif");
        $(this).attr("title", "折叠");
        $(this).html("折叠");
        $(".toolContent").slideDown(100);
      }
      else
      {
        $(this).attr("src", "/resources/images/forum/toolbar.expand.gif");
        $(this).attr("title", "展开");
        $(this).html("展开");
        $(".toolContent").slideUp(100);
      }
    });

    $(window).resize(function()
    {
      main.forum.thread.detail.resizeWindow();
    });

    // 让窗口最大化，以免部分用户浏览窗口过小产生滚动条
    var width = screen.availWidth - (document.layers ? 10 : -8);
    var height = screen.availHeight - (document.layers ? 20 : -8);

    window.moveTo(0, 0);
    window.resizeTo(width, height);
    masterpage.load();
    main.forum.thread.detail.resizeWindow();
  }
  /*#endregion*/
};

$(document).ready(main.forum.thread.detail.load);