x.register('main.bugs.bug.list');

main.bugs.bug.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    //var whereClauseValue = ' 1 = 1 ';

    //if($('#query-searchText').val() !== '')
    //{
    //  whereClauseValue += ' AND ( T.Code LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.Title LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.Content LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.AccountId IN (SELECT AuthorizationObjectId FROM view_AuthorizationObject_Account WHERE AccountGlobalName LIKE ##%' + $('#searchText').val() + '%## OR AccountLoginName LIKE ##%' + $('#searchText').val() + '%## ) OR T.AssignToAccountId IN (SELECT AuthorizationObjectId FROM view_AuthorizationObject_Account WHERE AccountGlobalName LIKE ##%' + $('#searchText').val() + '%## OR AccountLoginName LIKE ##%' + $('#searchText').val() + '%## ) ) ';
    //}
    //if($('#categoryIndex').val() != '')
    //{
    //  whereClauseValue += ' AND CategoryId IN ( SELECT Id FROM tb_Bugzilla_Category WHERE CategoryIndex LIKE ##' + $('#categoryIndex').val() + '%## ) ';
    //}
    //if($('#searchStatus').val() !== '')
    //{
    //  whereClauseValue += ' AND ( T.Status = ' + $('#searchStatus').val() + ' ) ';
    //}

    //// 移除 1 = 1
    //if(whereClauseValue.indexOf(' 1 = 1  AND ') > -1)
    //{
    //  whereClauseValue = whereClauseValue.replace(' 1 = 1  AND ', '');
    //}

    //main.bugs.bug.list.paging.whereClause = whereClauseValue;

    main.bugs.bug.list.paging.query.scence = 'Query';
    main.bugs.bug.list.paging.query.where.SearchText = $('#query-searchText').val().trim();
    main.bugs.bug.list.paging.query.where.CategoryIndex = $('#query-categoryIndex').val().trim().replace('\\', '\\\\');
    main.bugs.bug.list.paging.query.where.Status = $('#query-status').val();

    main.bugs.bug.list.paging.query.orders = ' ModifiedDate DESC ';

    main.bugs.bug.list.getPaging(1);
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
    outString += '<th >' + i18n.translates['Name'] + '</td>';
    outString += '<th style="width:100px">' + i18n.translates['SubmittedBy'] + '</th>';
    outString += '<th style="width:100px">' + i18n.translates['Bugs']['AssignTo'] + '</th>';
    outString += '<th style="width:80px" >' + i18n.translates['Status'] + '</th>';
    outString += '<th style="width:120px" >' + i18n.translates['ModifiedDate'] + '</th>';
    outString += '<th style="width:30px" title="' + i18n.strings['btn_remove'] + '" ><i class="fa fa-trash" ></i></th>';
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
    outString += '<col style="width:100px" />';
    outString += '<col style="width:80px" />';
    outString += '<col style="width:120px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '" >';
      outString += '<td>';
      outString += '<a href="/bugs/detail/' + node.id + '" target="_blank" >' + node.title + '</a> ';
      if(node.priority > 0)
      {
        outString += main.bugs.util.setColorPriorityView(node.priority) + ' ';
      }
      outString += '<span class="label label-default" >' + node.categoryIndex + '</span>';
      outString += '</td>';
      outString += '<td>' + node.accountName + '</td>';
      outString += '<td>' + (node.assignToAccountName === '' ? '<span class="label label-danger">未指定</span>' : node.assignToAccountName) + '</td>';
      outString += '<td>' + main.bugs.util.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';
      // outString += '<td><a href="/apps/paging/bugzilla/bugzilla-form.aspx?id=' + node.id + '" target="_blank" >编辑</a></td>';
      outString += '<td><a href="javascript:main.bugs.bug.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      var classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '">';
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
    var paging = main.bugs.bug.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/bug.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: '正在查询数据，请稍后......',
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.bugs.bug.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.bugs.bug.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.bugs.bug.list.getPaging({0});');

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
    if(confirm('确定删除?'))
    {
      x.net.xhr('/api/bug.delete.aspx?id=' + id, {
        callback: function(response)
        {
          main.bugs.bug.list.getPaging(main.bugs.bug.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getCategoryWizard()*/
  getCategoryWizard: function()
  {
    x.ui.wizards.getWizard('category', {
      create: function()
      {
        main.bugs.bug.list.categoryWizardName = this.name;

        var outString = '';

        outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:300px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>类别选择</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<div id="' + this.name + '-treeViewContainer" class="winodw-wizard-tree-view" style="width:296px; height:300px;"></div>';

        outString += '<div class="winodw-wizard-result-container form-inline text-right" >';
        outString += '<label class="winodw-wizard-result-item-text"　>所属类别</label> ';
        outString += '<input id="' + this.name + '-wizardResultText" name="wizardResultText" type="text" value="" class="winodw-wizard-result-item-input form-control input-sm" style="width:160px" /> '
        outString += '<input id="' + this.name + '-wizardResultValue" name="wizardResultValue" type="hidden" value="" />';
        outString += '<a href="javascript:' + this.name + '.save();" class="btn btn-default btn-sm" >确定</a>';
        outString += '</div>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      },

      save_callback: function()
      {
        main.bugs.bug.list.setCategoryIndex($('#' + this.name + '-wizardResultValue').val());

        return 0;
      }
    });

    main.bugs.bug.list.getTreeView();
  },
  /*#endregion*/

  /*#region 函数:setCategoryIndex(value)*/
  setCategoryIndex: function(value)
  {
    $('#query-categoryName').val(value);
    $('#query-categoryIndex').val(value);
  },
  /*#endregion*/

  /*#region 函数:getTreeView()*/
  getTreeView: function()
  {
    var treeViewName = '问题类别';
    var treeViewRootTreeNodeId = '1';
    var treeViewUrl = 'javascript:main.bugs.bug.list.setTreeViewNode(\'{treeNodeToken}\',\'{treeNodeId}\')';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
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

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.bugs.bug.list.tree', ajaxMode: true });

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: 'javascript:void(0);',
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/bug.category.getDynamicTreeView.aspx', false, outString);

    main.bugs.bug.list.tree = tree;

    var wizardName = main.bugs.bug.list.categoryWizardName;

    $('#' + wizardName + '-treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value, name)
  {
    var wizardName = main.bugs.bug.list.categoryWizardName;

    $('#' + wizardName + '-wizardResultText').val(x.string.trim(name, '\\'));
    $('#' + wizardName + '-wizardResultValue').val(x.string.trim(name, '\\'));
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    main.bugs.bug.list.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#btnCategoryWizard').on('click', function()
    {
      main.bugs.bug.list.getCategoryWizard()
    });

    $('#btnFilter').on('click', function()
    {
      main.bugs.bug.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.bugs.bug.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.bugs.bug.list.filter();
}
