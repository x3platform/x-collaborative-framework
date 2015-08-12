x.register('main.forum.category.list');

main.forum.category.list = {

  pages: x.page.newPagesHelper(),

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

    main.forum.category.list.pages.whereClause = whereClauseValue;

    main.forum.category.list.pages.orderBy = ' T.OrderId, T.UpdateDate DESC';

    main.forum.category.list.getPages(1);
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

    outString += '<table class="table-style" style="width:100%">';
    outString += '<tbody>';
    outString += '<tr class="table-row-title">';
    outString += '<td >名称</td>';
    outString += '<td style="width:80px" >创建人</td>';
    outString += '<td style="width:30px" >状态</td>';
    outString += '<td style="width:80px" >更新时间</td>';
    outString += '<td class="end" style="width:40px" >删除</td>';
    outString += '</tr>';

    list.each(function(node, index)
    {
      classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '" >';
      outString += '<td><a href="/apps/pages/forum/forum-category-form.aspx?id=' + node.id + '" >' + node.categoryIndex + '</a></td>';
      outString += '<td>' + node.accountName + '</td>';
      outString += '<td>' + x.customForm.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      outString += '<td><a href="javascript:main.forum.category.list.confirmDelete(\'' + node.id + '\');">删除</a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      var classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '">';
      outString += '<td colspan="5" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
      outString += '</tr>';

      counter++;
    }

    outString += '</tbody>';
    outString += '</table>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPages(currentPage)*/
  /*
  * 分页
  */
  getPages: function(currentPage)
  {
    var pages = main.forum.category.list.pages;

    pages.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<ajaxStorage>';
    outString += pages.toXml();
    outString += '</ajaxStorage>';

    x.net.xhr('/api/forum.category.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var pages = main.forum.category.list.pages;

        var list = result.ajaxStorage;

        pages.load(result.pages);

        var containerHtml = main.forum.category.list.getObjectsView(list, pages.pageSize);

        $('#windowMainTableContainer').html(containerHtml);

        var footerHtml = pages.tryParseMenu('javascript:main.forum.category.list.getPages({0});');

        $('#windowMainTableFooter').html(footerHtml);
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
          main.forum.category.list.getPages(main.forum.category.list.pages.currentPage);
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
    var treeViewRootTreeNodeId = '选择类别'; // 默认值:'00000000-0000-0000-0000-000000000001'
    var treeViewUrl = 'javascript:main.forum.category.list.setTreeViewNode(\'{treeNodeId}\')';

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

    var tree = x.tree.newTreeView('main.forum.category.list.tree');

    tree.setAjaxMode(true);

    tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', ''), treeViewName, '', '/resources/images/tree/tree_icon.gif');

    tree.load('/api/forum.category.getDynamicTreeView.aspx', false, outString);

    main.forum.category.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    var whereClause = ' T.CategoryIndex LIKE ##' + value.replace('选择类别_', '').replace(/_/g, '\\') + '%## ';

    main.forum.category.list.pages.whereClause = whereClause;
    main.forum.category.list.getPages(1);
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