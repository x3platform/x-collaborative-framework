x.register('main.connect.list');

main.connect.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    //var whereClauseValue = ' 1 = 1 ';

    //if($('#searchText').val() !== '')
    //{
    //  whereClauseValue += ' AND ( T.Code LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.Title LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.Content LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.AccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE ##%' + $('#searchText').val() + '%## OR AccountLoginName LIKE ##%' + $('#searchText').val() + '%## ) OR T.AssignToAccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE ##%' + $('#searchText').val() + '%## OR AccountLoginName LIKE ##%' + $('#searchText').val() + '%## ) ) ';
    //}

    //// 移除 1 = 1
    //if(whereClauseValue.indexOf(' 1 = 1  AND ') > -1)
    //{
    //  whereClauseValue = whereClauseValue.replace(' 1 = 1  AND ', '');
    //}

    //main.connect.list.paging.whereClause = whereClauseValue;

    main.connect.list.paging.query.scence = 'Query';
    // main.connect.list.paging.query.where.SearchText = $('#searchText').val().trim();

    main.connect.list.paging.query.orders = ' T.ModifiedDate DESC ';

    main.connect.list.getPaging(1);
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
    outString += '<th >名称</th>';
    outString += '<th style="width:60px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
    outString += '<th style="width:60px" title="编辑" ><i class="fa fa-edit"></i></th>';
    // outString += '<th style="width:100px">更新时间</th>';
    // outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
    outString += '<th class="table-freeze-head-padding" ></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col />';
    outString += '<col style="width:60px" />';
    outString += '<col style="width:60px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><strong>' + node.name + '</strong> <span class="label label-warning">' + node.appKey + '</span></td>';
      outString += '<td>' +  x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td><a href="/connect/overview/' + node.id + '" title="编辑" ><i class="fa fa-edit"></i></a></td>';
      // outString += '<td><a href="javascript:main.connect.list.confirmDelete(\'' + node.id + '\');">删除</a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr><td colspan="4" >&nbsp;</td></tr>';

      counter++;
    }

    outString += '</tbody>';
    outString += '</table>';
    outString += '</div>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(currentPage)*/
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.connect.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/connect.queryMyList.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.connect.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.connect.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.connect.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(id)*/
  /*
  * 删除对象
  */
  confirmDelete: function(id)
  {
    if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
    {
      x.net.xhr('/api/connect.delete.aspx?id=' + id, {
        callback: function(response)
        {
          main.connect.list.getPaging(main.connect.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    main.connect.list.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#btnFilter').on('click', function()
    {
      main.connect.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.connect.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.connect.list.filter();
}
