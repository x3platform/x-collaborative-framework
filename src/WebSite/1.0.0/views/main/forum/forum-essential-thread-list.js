x.register('main.forum.thread.list');

main.forum.thread.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter(flag)*/
  /*
  * 查询
  */
  filter: function(flag)
  {
    /*
    var whereClauseValue = ' T.Status = 1 ';

    var title = $('#title').val();
    var content = $('#content').val();
    var accountName = $('#accountName').val();

    if(main.forum.thread.list.searchNodeName != "")
    {
      var searchNodeName = main.forum.thread.list.searchNodeName;
      if(searchNodeName.substring(searchNodeName.length - 1) == '-')
      {
        whereClauseValue += ' AND T.[CategoryIndex] = ##' + x.string.rtrim(searchNodeName, '-') + '##';
      }
      else
      {
        whereClauseValue += ' AND T.[CategoryIndex] LIKE ##' + x.toSafeLike(searchNodeName) + '-%##';
      }
    }

    if(flag == 1)
    {
      if(title.trim() !== '')
      {
        whereClauseValue += ' AND T.Title LIKE ##%' + x.toSafeLike(title) + '%## ';
      }

      if(content.trim() !== '')
      {
        whereClauseValue += ' AND T.Content LIKE ##%' + x.toSafeLike(content) + '%## ';
      }

      if(accountName.trim() !== '')
      {
        whereClauseValue += ' AND (T.AccountName LIKE ##%' + x.toSafeLike(accountName) + '%## OR T.AccountName = ####)';
      }
    }

    main.forum.thread.list.paging.whereClause = whereClauseValue;

    main.forum.thread.list.paging.orderBy = ' IsTop DESC, ModifiedDate DESC';
    */
    main.forum.thread.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:getObjectsView(list, maxCount)*/
  /*
  * 创建对象列表的视图
  */
  getObjectsView: function(list, maxCount)
  {
    var outString = '';

    var counter = 0;

    var classNameValue = '';

    outString += '<div class="table-freeze-head">';
    outString += '<table class="table" >';
    outString += '<thead>';
    outString += '<tr>';
    outString += '<th >标题</th>';
    outString += '<th style="width:100px" >作者</th>';
    // outString += '<th style="width:100px" >发布日期</th>';
    outString += '<th style="width:80px">回复\浏览</th>';
    outString += '<th style="width:100px">更新时间</th>';
    if($('#isAdminToken').val().toLowerCase() == 'true')
    {
      outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
    }
    outString += '<th class="table-freeze-head-padding" ></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col />';
    outString += '<col style="width:100px" />';
    // outString += '<col style="width:100px" />';
    outString += '<col style="width:80px" />';
    outString += '<col style="width:100px" />';
    if($('#isAdminToken').val().toLowerCase() == 'true')
    {
      outString += '<col style="width:30px" />';
    } 
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      var displayName = node.accountName;

      if(node.anonymous == 1)
      {
        displayName = "匿名";
      }

      outString += '<tr>';
      outString += '<td>';
      // outString += main.forum.util.getType(node.isTop, node.sign);
      outString += '<a href="forum-thread-view.aspx?id=' + node.id + '" target="_blank" >' + node.title + '</a> ';
      outString += '<span class="label label-default">' + node.categoryIndex + '</span> ';
      outString += main.forum.util.isNew(node.modifiedDate, $("#nowDate").val());
      outString += main.forum.util.isHot(node.click, node.commentCount)
      outString += main.forum.util.isEssential(node.isEssential);
      outString += '</td>';
      outString += '<td>' + displayName + '</td>';
      // outString += '<td>' + node.createdDateView + '</td>';
      outString += '<td><strong>' + node.commentCount + '</strong> / ' + node.click + '</td>';
      
      if(parseInt(node.commentCount, 10) === 0)
      {
        // outString += '<td></td>';
        outString += '<td>' + node.createdDateView + '</td>';
      }
      else
      {
        // outString += '<td>' + node.latestCommentAccountName + '</td>';
        outString += '<td>' + node.modifiedDateView + '</td>';
      }
      if($('#isAdminToken').val().toLowerCase() == 'true')
      {
        outString += '<td><a href="javascript:main.forum.thread.list.confirmDelete(\'' + node.id + '\',\'' + node.accountId + '\',\'' + node.sign + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      }
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr>';
      outString += '<td colspan="7" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
      outString += '</tr>';

      counter++;
    }

    outString += '</tbody>';
    outString += '</table>';
    outString += '</div>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(value)*/
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.forum.thread.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/forum.thread.query.aspx', outString, {
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.forum.thread.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.forum.thread.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.forum.thread.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(id, accountId, sign)*/
  /*
  * 删除帖子或回帖
  */
  confirmDelete: function(id, accountId, sign)
  {
    if(confirm("确定永久删除该帖子及相关信息吗？"))
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';
      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[delete]]></action>';
      outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
      outString += '<id><![CDATA[' + id + ']]></id>';
      outString += '<accountId><![CDATA[' + accountId + ']]></accountId>';
      outString += '<sign><![CDATA[' + sign + ']]></sign>';
      outString += '</ajaxStorage>';
      var options = {
        resultType: 'json',
        xml: outString
      };
      $.post(main.forum.thread.list.url, options, this.confirmDelete_callback);
    }
  },

  confirmDelete_callback: function(response)
  {
    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
      case 1:
        alert(result.value);
        main.forum.thread.list.getPaging(1);
        break;
      default:
        alert(result.value);
        break;
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    // main.forum.util.getTreeView(0, 0);

    // $('.table-sidebar-tree-view').css('overflow', 'auto');

    main.forum.thread.list.filter();

    // 事件绑定
    $('#btnFilter').on('click', function()
    {
      main.forum.thread.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.forum.thread.list.load);

var xTreeExtend = {
  selected: function(id, parentId, name)
  {
    main.forum.thread.list.searchNodeName = typeof (id) == 'undefined' ? '' : id.replace(/\$/g, '-');

    main.forum.thread.list.filter(0);
  }
};

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.forum.util.getTreeView(0, 0);
  main.forum.thread.list.getPaging(1);
};