x.register('main.customizes.customize.page.list');

main.customizes.customize.page.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /**
   * 查询
   */
  filter: function()
  {
    main.customizes.customize.page.list.paging.query.scence = 'Query';
    main.customizes.customize.page.list.paging.query.where.SearchText = $('#searchText').val().trim();
    main.customizes.customize.page.list.paging.query.orders = ' UpdateDate DESC ';
    main.customizes.customize.page.list.getPaging(1);
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
    outString += '<th >名称</th>';
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
    outString += '<col />';
    outString += '<col style="width:40px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="/customizes/customize-page/form?id=' + node.id + '" target="_blank" >' + node.name + '</a></td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      if(node.locking == 1)
      {
        outString += '<td><a href="javascript:main.customizes.customize.page.list.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      }
      else
      {
        outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
      }
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
    var paging = main.customizes.customize.page.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/web.customizes.customizePage.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.customizes.customize.page.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.customizes.customize.page.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.customizes.customize.page.list.getPaging({0});');

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

      $.post(main.customizes.customize.page.list.url, options, main.customizes.customize.page.list.confirmDelete_callback);
    }
  },

  confirmDelete_callback: function(response)
  {

    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
        alert(result.value);
        main.customizes.customize.page.list.getPaging(1);
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

  /*#region 函数:getTreeView(value)*/
  /*
  * 获取树形菜单
  */
  getTreeView: function(value)
  {
    var treeViewId = '00000000-0000-0000-0000-000000000001';
    var treeViewName = '应用管理';
    var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
    var treeViewUrl = 'javascript:main.customizes.customize.page.list.setTreeViewNode(\'{treeNodeId}\')';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<ajaxStorage>';
    outString += '<action><![CDATA[getDynamicTreeView]]></action>';
    outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
    outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
    outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
    outString += '<tree><![CDATA[{tree}]]></tree>';
    outString += '<parentId><![CDATA[{parentId}]]></parentId>';
    outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
    outString += '</ajaxStorage>';

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.customizes.customize.page.list.tree' });

    tree.setAjaxMode(true);

    // tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId), treeViewName, '', '/resources/images/tree/tree_icon.gif');

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/web.customizes.customizePage.getDynamicTreeView.aspx', false, outString);

    main.customizes.customize.page.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    main.customizes.customize.page.list.paging.query.scence = 'QueryByParentId';
    main.customizes.customize.page.list.paging.query.where.ParentId = value;
    main.customizes.customize.page.list.paging.query.orders = ' OrderId ';

    main.customizes.customize.page.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /**
   * 页面加载事件
   */
  load: function()
  {
    main.customizes.customize.page.list.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.customizes.customize.page.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.customizes.customize.page.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.customizes.customize.page.list.load);

/**
 * [默认]私有回调函数, 供子窗口回调
 */
function window$refresh$callback()
{
  main.customizes.customize.page.list.getPaging(1);
}