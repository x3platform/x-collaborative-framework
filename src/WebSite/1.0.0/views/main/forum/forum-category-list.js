x.register('main.forum.category.list');

main.forum.category.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    var whereClauseValue = ' Status IN (0, 1)';

    var searchText = $('#searchText').val();

    if(searchText != '')
    {
      whereClauseValue += ' AND CategoryIndex LIKE ##%' + x.toSafeLike(searchText) + '%##  ';
    }

    main.forum.category.list.paging.whereClause = whereClauseValue;

    main.forum.category.list.paging.orderBy = ' T.OrderId, T.ModifiedDate DESC';

    main.forum.category.list.getPaging(1);
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
    outString += '<th style="width:100px" >创建人</th>';
    outString += '<th style="width:40px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
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
    outString += '<col style="width:40px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="/forum/forum-category/form?id=' + node.id + '" target="_blank" >' + node.categoryIndex + '</a></td>';
      outString += '<td>' + node.accountName + '</td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';
      outString += '<td><a href="javascript:main.forum.category.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr>';
      outString += '<td colspan="5" >&nbsp;</td>';
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
    var paging = main.forum.category.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/forum.category.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.forum.category.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.forum.category.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.forum.category.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(id)*/
  confirmDelete: function(id)
  {
    if(confirm('您确定删除?'))
    {
      x.net.xhr('/api/forum.category.delete.aspx?id=' + id, {
        waitingMessage: i18n.net.waiting.deleteTipText,
        callback: function(response)
        {
          main.forum.category.list.getPaging(main.forum.category.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getTreeView()*/
  /*
  * 获取树形菜单
  */
  getTreeView: function()
  {
    var treeViewId = '选择类别';
    var treeViewName = '选择类别';
    var treeViewRootTreeNodeId = '选择类别'; 
    var treeViewUrl = 'javascript:main.forum.category.list.setTreeViewNode(\'{treeNodeId}\')';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
    outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
    outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
    outString += '<tree><![CDATA[{tree}]]></tree>';
    outString += '<parentId><![CDATA[{parentId}]]></parentId>';
    outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
    outString += '</request>';

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.forum.category.list.tree', ajaxMode: true })

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/forum.category.getDynamicTreeView.aspx', false, outString);

    main.forum.category.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    var whereClause = ' T.CategoryIndex LIKE ##' + value.replace('选择类别_', '').replace(/_/g, '\\') + '%## ';

    main.forum.category.list.paging.whereClause = whereClause;
    main.forum.category.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    main.forum.category.list.filter();

    main.forum.category.list.getTreeView();

    $('#treeViewContainer').css('height', '291px');
    $('#treeViewContainer').css('overflow', 'auto');

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.forum.category.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.forum.category.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.forum.category.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.forum.category.list.filter();
};