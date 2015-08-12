x.register('main.forum.home');

main.forum.home = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    // var whereClauseValue = ' [Status] = 1 ';

    // main.forum.home.paging.whereClause = whereClauseValue;

    // main.forum.home.paging.orderBy = ' IsTop DESC, UpdateDate DESC ';

    main.forum.home.getPaging(1);
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

    outString += '<div class="table-freeze-head">';
    outString += '<table class="table" >';
    outString += '<thead>';
    outString += '<tr>';
    outString += '<th >标题</th>';
    outString += '<th style="width:100px" >作者</th>';
    outString += '<th style="width:80px">回复/浏览</th>';
    outString += '<th style="width:100px">更新时间</th>';
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
    outString += '<col style="width:80px" />';
    outString += '<col style="width:100px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      classNameValue = 'table-row-normal' + ((counter + 1) == maxCount ? '-transparent' : '');

      var displayName = node.accountName;
      if(node.anonymous == 1)
      {
        displayName = "匿名";
      }

      outString += '<tr class="' + classNameValue + '" >';
      outString += '<td>';
      // outString += main.forum.util.getType(node.isTop, node.sign);
      outString += '<a href="/forum/detail/' + node.id + '" target="_blank" >' + node.title + '</a> ';
      outString += '<span class="label label-default">' + node.categoryIndex + '</span> ';
      // outString += main.forum.util.isNew(node.updateDate, $("#nowDate").val());
      outString += main.forum.util.isHot(node.click, node.commentCount)
      outString += main.forum.util.isEssential(node.isEssential);
      outString += '</td>';
      outString += '<td>' + displayName + '</td>';
      outString += '<td><strong>' + node.commentCount + '</strong> / ' + node.click + '</td>';
      if(parseInt(node.commentCount, 10) === 0)
      {
        // outString += '<td></td>';
        outString += '<td>' + node.createDateView + '</td>';
      }
      else
      {
        // outString += '<td>' + node.latestCommentAccountName + '</td>';
        outString += '<td>' + node.updateDateView + '</td>';
      }
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      var classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '">';
      outString += '<td colspan="6" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
      outString += '</tr>';

      counter++;
    }

    outString += '</tbody>';
    outString += '</table>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(currentPage)*/
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.forum.home.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/forum.thread.query.aspx', outString, {
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.forum.home.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.forum.home.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.forum.home.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        window.scrollTo(0, 0);
        // location.hash = 'scrollTop';
        // scroller('windowTopToken', 800);

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    // 根据屏幕获取页面显示记录条数
    // var num = main.forum.util.getPageSizeByScreen(document.body.offsetHeight - 200, 71);

    // main.forum.home.paging.pageSize = 20;

    main.forum.home.filter();
  }
  /*#endregion*/
};

$(document).ready(main.forum.home.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.forum.home.getPaging(1);
};
