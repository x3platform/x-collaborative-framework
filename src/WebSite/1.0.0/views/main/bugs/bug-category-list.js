x.register('main.bugs.bug.category.list');

main.bugs.bug.category.list = {

  tree: '',

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    //var whereClauseValue = ' Status!=2 ';

    //var searchText = $('#searchText').val();

    //if($('#searchText').val() != '')
    //{
    //  whereClauseValue += ' AND CategoryIndex LIKE ##%' + $('#searchText').val() + '%## ';
    //}

    //main.bugs.bug.category.list.paging.whereClause = whereClauseValue;

    //main.bugs.bug.category.list.paging.orderBy = ' T.OrderId, T.ModifiedDate DESC';

    main.bugs.bug.category.list.paging.query.scence = 'Query';
    main.bugs.bug.category.list.paging.query.where.CategoryIndex = $('#searchText').val().trim();
    main.bugs.bug.category.list.paging.query.where.Status = '0,1';

    main.bugs.bug.category.list.paging.query.orders = ' OrderId, ModifiedDate DESC';

    main.bugs.bug.category.list.getPaging(1);
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
    outString += '<th>类别索引</th>';
    outString += '<th style="width:100px">创建人</th>';
    outString += '<th style="width:30px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
    outString += '<th style="width:100px">更新时间</th>';
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
    outString += '<col style="width:30px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="/bugs/bug-category/form?id=' + node.id + '" target="_blank">' + node.categoryIndex + '</a></td>';
      outString += '<td>' + node.accountName + '</td>';
      outString += '<td class="text-center" >' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';
      outString += '<td>' + ($('#isAdminToken').val().toLowerCase() === 'true' ? ('<a href="javascript:main.bugs.bug.category.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a>') : '<span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span>') + '</td>';
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
    var paging = main.bugs.bug.category.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getPaging]]></action>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/bug.category.query.aspx', outString, {
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.bugs.bug.category.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.bugs.bug.category.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.bugs.bug.category.list.getPaging({0});');

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
      x.net.xhr('/api/bug.category.delete.aspx?id=' + id, {
        callback: function(response)
        {
          main.bugs.bug.category.list.getPaging(main.bugs.bug.category.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getTreeView()*/
  getTreeView: function()
  {
    var treeViewName = $('#treeViewName').val();
    var treeViewRootTreeNodeId = $('#treeViewRootTreeNodeId').val();
    var treeViewUrl = 'javascript:main.bugs.bug.category.list.setTreeViewNode(\'{treeNodeId}\')';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getDynamicTreeView]]></action>';
    outString += '<treeViewId><![CDATA[]]></treeViewId>';
    outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
    outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
    outString += '<tree><![CDATA[{tree}]]></tree>';
    outString += '<parentId><![CDATA[{parentId}]]></parentId>';
    outString += '<enabledLeafClick><![CDATA[false]]></enabledLeafClick>';
    outString += '<elevatedPrivileges><![CDATA[true]]></elevatedPrivileges>';
    outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
    outString += '<treeName>tree</treeName>';
    outString += '</request>';

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.bugs.bug.category.list.tree', ajaxMode: true });

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/bug.category.getDynamicTreeView.aspx', false, outString);

    main.bugs.bug.category.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    // var whereClauseValue = ' T.CategoryIndex LIKE ##' + value + '%## AND Status IN (0, 1) ';

    // main.bugs.bug.category.list.paging.whereClause = whereClauseValue;
    main.bugs.bug.category.list.paging.query.where.CategoryIndex = value;
    main.bugs.bug.category.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    main.bugs.bug.category.list.getTreeView();

    main.bugs.bug.category.list.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#btnFilter').on('click', function()
    {
      main.bugs.bug.category.list.filter();
    });
  }
  /*#endregion*/
}

$(document).ready(main.bugs.bug.category.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.bugs.bug.category.list.filter();
}
