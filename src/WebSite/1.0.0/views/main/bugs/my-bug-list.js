x.register('main.bugs.my.list');

main.bugs.my.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    //var whereClause = ' Status IN (' + $('#status').val() + ') ';

    //if($('#searchText').val() !== '')
    //{
    //  whereClause += ' AND ( Code LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR Title LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## ) ';
    //}

    //main.bugs.my.list.paging.whereClause = whereClause;

    main.bugs.my.list.paging.query.scence = 'QueryMyList';
    main.bugs.my.list.paging.query.where.SearchText = $('#searchText').val().trim();
    main.bugs.my.list.paging.query.where.Status = $('#status').val();

    main.bugs.my.list.getPaging(1);
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
    outString += '<th >标题</td>';
    outString += '<th style="width:100px">提交人</th>';
    outString += '<th style="width:80px" >状态</th>';
    outString += '<th style="width:100px" >更新日期</th>';
    outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
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
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td>';
      outString += '<a href="/bugs/detail/' + node.id + '" target="_blank" >' + node.title + '</a> ';
      if(node.priority > 0)
      {
        outString += main.bugs.util.setColorPriorityView(node.priority) + ' ';
      }
      outString += '<span class="label label-default" >' + node.categoryIndex + '</span>';
      outString += '</td>';
      outString += '<td>' + node.accountName + '</td>';
      outString += '<td>' + main.bugs.util.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';
      outString += '<td><a href="javascript:main.bugs.my.list.confirmDelete(\'' + node.id + '\',\'' + node.title + '\',\'' + main.bugs.my.list.paging.currentPage + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr><td colspan="5" >&nbsp;</td></tr>';

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
    var paging = main.bugs.my.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';
    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/bug.queryMyList.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: '正在查询数据，请稍后......',
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.bugs.my.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.bugs.my.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.bugs.my.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        // 调整页面结构尺寸
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
    if(confirm('确定删除?'))
    {
      x.net.xhr('/api/bug.delete.aspx?id=' + id, {
        callback: function(response)
        {
          main.bugs.my.list.getPaging(main.bugs.my.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getTab(tabName)*/
  /*
  * 获取页签信息
  */
  getTab: function(tabName)
  {
    var tabIndex = 0;

    switch(tabName)
    {
      case '0':
        //【待解决】页签                  
        tabIndex = 0;
        $('#status').val('0,1,3');
        break;

      case '1':
        //【已解决】页签                     
        tabIndex = 1;
        $('#status').val('2');
        break;

      case '2':
        //【无法修复】页签                         
        tabIndex = 2;
        $('#status').val('4');
        break;

      case '3':
        //【已关闭】页签                          
        tabIndex = 3;
        $('#status').val('5');
        break;

      case '4':
        //【被打回】页签
        tabIndex = 4;
        $('#status').val('6');
        break;
    }

    $('.x-ui-pkg-tabs-menu li').removeClass('active');
    $('.x-ui-pkg-tabs-container').css('display', 'none');

    // 设置当前页签信息
    $($('.x-ui-pkg-tabs-menu li')[tabIndex]).addClass('active');
    $($('.x-ui-pkg-tabs-container')[tabIndex]).css('display', '');
    // $('#containerId').val('tab-' + (tabIndex + 1) + '-container');

    main.bugs.my.list.filter();
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    main.bugs.my.list.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $("#btnFilter").on('click', function()
    {
      main.bugs.my.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.bugs.my.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.bugs.my.list.filter();
}
