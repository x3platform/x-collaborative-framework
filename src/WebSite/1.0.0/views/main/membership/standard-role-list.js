x.register('main.membership.standard.role.list');

main.membership.standard.role.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 过滤
  */
  filter: function()
  {
    var searchText = $('#searchText').val().trim();

    if(searchText != '')
    {
      main.membership.standard.role.list.paging.where.Name = searchText;
    }

    main.membership.standard.role.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:getObjectsView(list, maxCount)*/
  /*
  * 创建对象列表的视图
  */
  getObjectsView: function(list, maxCount)
  {
    var counter = 0;

    var outString = '';

    outString += '<div class="table-freeze-head">';
    outString += '<table class="table" >';
    outString += '<thead>';
    outString += '<tr>';
    outString += '<th >名称</th>';
    outString += '<th style="width:50px" >状态</th>';
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
    outString += '<col style="width:40px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="javascript:main.membership.standard.role.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
      outString += '<td class="text-center" >' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';

      if(node.locking === '1')
      {
        outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
      }
      else
      {
        outString += '<td><a href="javascript:main.membership.standard.role.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      }

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

  /*#region 函数:getObjectView(param)*/
  /*
  * 创建单个对象的视图
  */
  getObjectView: function(param)
  {
    var outString = '<div class="x-ui-pkg-tabs-wrapper">';

    outString += '<div class="x-ui-pkg-tabs-menu-wrapper" >';
    outString += '<ul class="x-ui-pkg-tabs-menu nav nav-tabs" >';
    outString += '<li><a href="#tab-1">基本信息</a></li>';
    outString += '<li><a href="#tab-2">所属角色</a></li>';
    // outString += '<li><a href="#tab-3">所属权限</a></li>';
    outString += '</ul>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="tab-1" name="tab-1" >基本信息</a></h2>';
    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" >编码</td>';
    outString += '<td class="table-body-input" style="width:160px" >';

    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + ((typeof (param.id) == 'undefined' || param.id == '0') ? '' : param.id) + '" />';
    if(typeof (param.code) == 'undefined' || param.code == '')
    {
      outString += '<span class="gray-text">自动编号</span>';
      outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
    }
    else
    {
      outString += '<input id="code" name="code" type="text" x-dom-data-type="value" value="' + (typeof (param.code) == 'undefined' ? '' : param.code) + '" class="form-control" style="width:120px;" />';
    }
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">上级标准角色</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) == 'undefined' ? '' : param.parentId) + '" />';
    outString += '<input id="parentName" name="parentName" type="text" x-dom-data-type="value" value="' + (typeof (param.parentName) == 'undefined' ? '' : param.parentName) + '" dataVerifyWarning="【上级标准角色】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getContactWizard({targetViewName:\'parentName\', targetValueName:\'parentId\', contactTypeText:\'standard-role\'});" >编辑</a> ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text">名称</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" value="' + (typeof (param.name) == 'undefined' ? '' : param.name) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" ><span class="required-text">所属标准组织</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="standardOrganizationUnitId" name="standardOrganizationUnitId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.standardOrganizationUnitId) == 'undefined' ? '' : param.standardOrganizationUnitId) + '" />';
    outString += '<input id="standardOrganizationName" name="standardOrganizationName" type="text" x-dom-data-type="value" value="' + (typeof (param.standardOrganizationName) == 'undefined' ? '' : param.standardOrganizationName) + '" dataVerifyWarning="【所属标准组织】必须填写。"  class="form-control custom-forms-data-required" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getContactWizard({targetViewName:\'standardOrganizationName\', targetValueName:\'standardOrganizationUnitId\', contactTypeText:\'standard-organization\'});" >编辑</a> ';
    outString += '</td>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">类型</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="type" name="type" type="text" x-dom-data-type="value" value="' + (typeof (param.type) == 'undefined' ? '' : param.type) + '" x-dom-feature="combobox" selectedText="' + (typeof (param.typeView) == 'undefined' ? '' : param.typeView) + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_标准角色管理_标准角色类别## ) " class="form-control x-ajax-text" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">等级</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="priority" name="priority" type="text" x-dom-data-type="value" value="' + (typeof (param.priority) == 'undefined' ? '' : param.priority) + '" x-dom-feature="combobox" selectedText="' + (typeof (param.priorityView) == 'undefined' ? '' : param.priorityView) + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_标准角色管理_标准角色权重## ) " class="form-control x-ajax-text" style="width:120px;" />';
    outString += '</td>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >排序</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + (typeof (param.orderId) == 'undefined' ? '' : param.orderId) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >关键角色</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="isKey" name="isKey" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.status) == 'undefined' ? '' : param.status) + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >启用</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.status) == 'undefined' ? '' : param.status) + '" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >防止意外删除</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="locking" name="locking" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.locking) == 'undefined' ? '' : param.locking) + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >备注</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="form-control" style="width:460px; height:40px;" >' + (typeof (param.remark) == 'undefined' ? '' : param.remark) + '</textarea>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >修改时间</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += (typeof (param.modifiedDateTimestampView) == 'undefined' ? '' : param.modifiedDateTimestampView);
    outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" x-dom-data-type="value" value="' + (typeof (param.modifiedDateTimestampView) == 'undefined' ? '' : param.modifiedDateTimestampView) + '" />';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';

    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">所属角色</a></h2>';

    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >所属成员</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="accountView" name="accountView" class="form-control" style="width:460px; height:80px;" >' + (typeof (param.accountView) == 'undefined' ? '' : param.accountView) + '</textarea>';
    outString += '<input id="accountValue" name="accountValue" type="hidden" x-dom-data-type="value" value="' + (typeof (param.accountValue) == 'undefined' ? '' : param.accountValue) + '" />';
    // outString += '<div class="text-right">';
    // outString += '<a href="javascript:main.membership.standard.role.list.getContactsWindow(\'account\');" >编辑</a> ';
    // outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >所属角色</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="roleView" name="roleView" class="form-control" style="width:460px; height:80px;" >' + (typeof (param.organizationView) == 'undefined' ? '' : param.organizationView) + '</textarea>';
    outString += '<input id="roleValue" name="roleValue" type="hidden" x-dom-data-type="value" value="' + (typeof (param.organizationValue) == 'undefined' ? '' : param.organizationValue) + '" />';
    // outString += '<div class="text-right">';
    // outString += '<a href="javascript:main.membership.standard.role.list.getContactsWindow(\'role\');" >编辑</a> ';
    // outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '</table>';
    outString += '</div>';

    outString += '</div>';

    // -------------------------------------------------------
    // 隐藏值设置
    // -------------------------------------------------------

    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + ((typeof (param.id) == 'undefined' || param.id == '0') ? '' : param.id) + '" />';
    outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.name) == 'undefined' ? '' : param.name) + '" />';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) == 'undefined' ? '' : param.parentId) + '" />';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(currentPage)*/
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.membership.standard.role.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/membership.standardRole.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.membership.standard.role.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="/apps/paging/membership/standard-role-report.aspx" target="_blank" class="btn btn-default" >报表</a> '
               + '<a href="javascript:main.membership.standard.role.list.openDialog();" class="btn btn-default" >新增</a>'
               + '</div>'
               + '<span>标准角色管理</span>'
               + '<div class="clearfix" ></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.standard.role.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        var footerHtml = paging.tryParseMenu('javascript:main.membership.standard.role.list.getPaging({0});');

        $('#window-main-table-footer')[0].innerHTML = footerHtml;

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:openDialog(value)*/
  /*
  * 查看单个记录的信息
  */
  openDialog: function(value)
  {
    var id = x.isUndefined(value, '');

    var url = '';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';

    var isNewObject = false;

    if(id == '')
    {
      url = '/api/membership.standardRole.create.aspx';

      var treeNode = main.membership.standard.role.list.tree.getSelectedNode();

      if(treeNode != null) { outString += '<standardOrganizationUnitId><![CDATA[' + treeNode.id + ']]></standardOrganizationUnitId>'; }

      isNewObject = true;
    }
    else
    {
      url = '/api/membership.standardRole.findOne.aspx';

      outString += '<id><![CDATA[' + id + ']]></id>';
    }

    outString += '</request>';

    x.net.xhr(url, outString, {
      waitingType: 'mini',
      waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="javascript:main.membership.standard.role.list.save();" class="btn btn-default" >保存</a> '
               + '<a href="javascript:main.membership.standard.role.list.getPaging(' + main.membership.standard.role.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
               + '</div>'
               + '<span>标准角色信息</span>'
               + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.standard.role.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        x.dom.features.bind();

        x.ui.pkg.tabs.newTabs();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:checkObject()*/
  /*
  * 检测对象的必填数据
  */
  checkObject: function()
  {
    if(x.dom.data.check()) { return false; }

    // id == '' 为新建的对象
    if($('#id').val() != '' && $('#parentId').val() == $('#id').val())
    {
      alert('上级标准角色不能为自己本身。');
      return false;
    }

    return true;
  },
  /*#endregion*/

  /*#region 函数:save()*/
  /*
  * 保存对象
  */
  save: function()
  {
    if(main.membership.standard.role.list.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml' });
      outString += '</request>';

      x.net.xhr('/api/membership.standardRole.save.aspx', outString, {
        waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
        callback: function(response)
        {
          main.membership.standard.role.list.getPaging(main.membership.standard.role.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(id)*/
  /*
  * 删除对象
  */
  confirmDelete: function(id)
  {
    if(confirm(i18n.msg.ARE_YOU_SURE_YOU_WANT_TO_DELETE))
    {
      x.net.xhr('/api/membership.standardRole.delete.aspx?id=' + id, {
        waitingMessage: i18n.strings.msg_net_waiting_delete_tip_text,
        callback: function(response)
        {
          main.membership.standard.role.list.getPaging(main.membership.standard.role.list.paging.currentPage);
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
    var treeViewId = $('#treeViewId').val();
    var treeViewName = $('#treeViewName').val();
    var treeViewRootTreeNodeId = $('#treeViewRootTreeNodeId').val();
    var treeViewUrl = 'javascript:main.membership.standard.role.list.setTreeViewNode(\'{treeNodeId}\')';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getDynamicTreeView]]></action>';
    outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
    outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
    outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
    outString += '<tree><![CDATA[{tree}]]></tree>';
    outString += '<parentId><![CDATA[{parentId}]]></parentId>';
    outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
    outString += '</request>';

    tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.standard.role.list.tree' });

    tree.setAjaxMode(true);

    // tree.add("0", "-1", $('#treeViewName').val(), 'javascript:main.membership.standard.role.list.setTreeViewNode(\'' + $('#treeViewRootTreeNodeId').val() + '\')', $('#treeViewName').val(), '', '/resources/images/tree/tree_icon.gif');

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/membership.standardOrganization.getDynamicTreeView.aspx', false, outString);

    main.membership.standard.role.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    main.membership.standard.role.list.paging.query.where.StandardOrganizationUnitId = value;
    main.membership.standard.role.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    // 正常加载
    var treeViewId = $('#treeViewId').val();

    main.membership.standard.role.list.getTreeView();

    main.membership.standard.role.list.setTreeViewNode(treeViewId);

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.membership.standard.role.list.filter();
    });

    $('.table-sidebar-search-button').bind('click', function()
    {
      main.membership.standard.role.list.filter();
    });
  }
  /*#endregion*/
}

$(document).ready(main.membership.standard.role.list.load);
