x.register('main.customizes.customize.widget.list');

main.customizes.customize.widget.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /**
   * 查询
   */
  filter: function()
  {
    main.customizes.customize.widget.list.paging.query.scence = 'Query';
    main.customizes.customize.widget.list.paging.query.where.SearchText = $('#searchText').val().trim();
    main.customizes.customize.widget.list.paging.query.orders = ' UpdateDate DESC ';
    main.customizes.customize.widget.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:getObjectsView(list, maxCount)*/
  /**
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
    outString += '<th style="width:120px">名称</th>';
    outString += '<th >描述</th>';
    outString += '<th style="width:40px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
    outString += '<th style="width:100px">更新日期</th>';
    outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
    outString += '<th class="table-freeze-head-padding" ></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col style="width:120px" />';
    outString += '<col />';
    outString += '<col style="width:40px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="/customizes/customize-widget/form?id=' + node.id + '" target="_blank" >' + node.name + '</a></td>';
      outString += '<td>' + node.description + '</td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      outString += '<td><a href="javascript:main.customizes.customize.widget.list.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr>';
      outString += '<td colspan="8" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
      outString += '</tr>';

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
    var paging = main.customizes.customize.widget.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/web.customizes.customizeWidget.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.customizes.customize.widget.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.customizes.customize.widget.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.customizes.customize.widget.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(ids)*/
  /*
  * 删除对象
  */
  confirmDelete: function(ids)
  {
    if(confirm('确定删除?'))
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[delete]]></action>';
      outString += '<ids><![CDATA[' + ids + ']]></ids>';
      outString += '</ajaxStorage>';

      var options = {
        resultType: 'json',
        xml: outString
      };

      $.post(main.customizes.customize.widget.list.url, options, main.customizes.customize.widget.list.confirmDelete_callback);
    }
  },

  confirmDelete_callback: function(response)
  {

    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
        alert(result.value);
        main.customizes.customize.widget.list.getPaging(1);
        break;

      case 1:
      case -1:
        alert(result.value);
        break;

      default:
        break;
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /**
   * 页面加载事件
   */
  load: function()
  {
    main.customizes.customize.widget.list.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.customizes.customize.widget.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.customizes.customize.widget.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.customizes.customize.widget.list.load);

/**
 * [默认]私有回调函数, 供子窗口回调
 */
function window$refresh$callback()
{
  main.customizes.customize.widget.list.getPaging(1);
}