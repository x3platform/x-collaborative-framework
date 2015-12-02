x.register('main.membership.role.list');

main.membership.role.list = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    var searchText = $('#searchText').val().trim();

    if(searchText !== '')
    {
      // whereClauseValue = ' T.Name LIKE ##%' + x.toSafeLike(key) + '%## OR T.GlobalName LIKE ##%' + x.toSafeLike(key) + '%## ';
      main.membership.role.list.paging.query.scence = 'Query';
      main.membership.role.list.paging.query.where.SearchText = x.toSafeLike(searchText);
      main.membership.role.list.paging.query.orders = ' T.Name ';
    }

    main.membership.role.list.getPaging(1);
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
    outString += '<th style="width:160px" >全局名称</th>';
    outString += '<th style="width:40px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
    outString += '<th style="width:100px" >更新日期</th>';
    outString += '<th style="width:100px" >数据验证</th>';
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
    outString += '<col style="width:160px" />';
    outString += '<col style="width:40px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="javascript:main.membership.role.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
      outString += '<td>' + node.globalName + '</td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';
      outString += '<td><a href="/apps/paging/membership/role-validator.aspx?roleId=' + node.id + '" target="_blank" >数据验证</a></td>';
      if(node.locking === '1')
      {
        outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
      }
      else
      {
        outString += '<td><a href="javascript:main.membership.role.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      }
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr><td colspan="6" >&nbsp;</td></tr>';

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
    outString += '<li><a href="#tab-2">所属成员</a></li>';
    outString += '<li><a href="#tab-3">备注</a></li>';
    outString += '</ul>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="tab-1" name="tab-1" >基本信息</a></h2>';
    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" >编码</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + ((typeof (param.id) === 'undefined' || param.id === '0') ? '' : param.id) + '" />';
    outString += '<input id="type" name="type" type="hidden" x-dom-data-type="value" value="' + (typeof (param.type) === 'undefined' ? '' : param.type) + '" />';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) === 'undefined' ? '' : param.parentId) + '" />';
    if(typeof (param.code) === 'undefined' || param.code === '')
    {
      outString += '<span class="gray-text">自动编号</span>';
      outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
    }
    else
    {
      outString += '<input id="code" name="code" type="text" x-dom-data-type="value" value="' + (typeof (param.code) === 'undefined' ? '' : param.code) + '" class="form-control" style="width:120px;" />';
    }
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" >上级角色</td>';
    outString += '<td class="table-body-input" style="width:160px" >';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) === 'undefined' ? '' : param.parentId) + '" />';
    outString += '<input id="parentName" name="parentName" type="text" x-dom-data-type="value" value="' + (typeof (param.parentName) === 'undefined' ? '' : param.parentName) + '" class="form-control" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getSingleObjectWizardSingleton(\'parentName\',\'parentId\',\'role\');" >编辑</a> ';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">所属标准角色</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="standardRoleId" name="standardRoleId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.standardRoleId) === 'undefined' ? '' : param.standardRoleId) + '" />';
    outString += '<input id="standardRoleName" name="standardRoleName" type="text" x-dom-data-type="value" value="' + (typeof (param.standardRoleName) === 'undefined' ? '' : param.standardRoleName) + '" class="form-control" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getContactWizard({targetViewName:\'standardRoleName\',targetValueName:\'standardRoleId\',contactTypeText:\'standard-role\'});" >编辑</a>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text">名称</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" />';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" dataVerifyWarning="【名称】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">所属组织</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="organizationId" name="organizationId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.organizationId) === 'undefined' ? '' : param.organizationId) + '" />';
    outString += '<input id="organizationGlobalName" name="organizationGlobalName" type="text" x-dom-data-type="value" value="' + (typeof (param.organizationGlobalName) === 'undefined' ? '' : param.organizationGlobalName) + '" dataVerifyWarning="【所属组织】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getContactWizard({targetViewName:\'organizationGlobalName\',targetValueName:\'organizationId\',contactTypeText:\'organization\'});" >编辑</a> ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text">全局名称</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="originalGlobalName" name="originalGlobalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" />';
    outString += '<input id="globalName" name="globalName" type="text" x-dom-data-type="value" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" dataVerifyWarning="【全局名称】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >所属通用角色</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="generalRoleId" name="generalRoleId" type="hidden" value="' + (typeof (param.generalRoleId) === 'undefined' ? '' : param.generalRoleId) + '" />';
    outString += '<input id="generalRoleName" name="generalRoleName" type="text" value="' + (typeof (param.generalRoleName) === 'undefined' ? '' : param.generalRoleName) + '" class="form-control" readonly="readonly" style="width:120px;" /> ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >拼音</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="pinyin" name="pinyin" type="text" x-dom-data-type="value" value="' + (typeof (param.pinyin) === 'undefined' ? '' : param.pinyin) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >作用范围</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="effectScope" name="effectScope" type="text" x-dom-data-type="value" value="' + (typeof (param.effectScope) === 'undefined' ? '' : param.effectScope) + '" x-dom-feature="combobox" selectedText="' + (typeof (param.effectScopeView) === 'undefined' ? '' : param.effectScopeView) + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_权限作用范围## ) " class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >排序标识</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.orderId, '') + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >启用企业邮箱</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="enableExchangeEmail" name="enableExchangeEmail" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.enableExchangeEmail, '0') + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >启用</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.status, '0') + '" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >防止意外删除</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="lock" name="lock" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.locking, '0') + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >更新时间</td>';
    outString += '<td class="table-body-input" >';
    outString += x.date.newTime(param.updateDate).toString('yyyy-MM-dd HH:mm:ss');
    outString += '<input id="updateDate" name="updateDate" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.updateDateTimestampView, '') + '" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >创建时间</td>';
    outString += '<td class="table-body-input" >';
    outString += x.date.newTime(param.createDate).toString('yyyy-MM-dd HH:mm:ss');
    outString += '<input id="createDate" name="createDate" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.createDateTimestampView, '') + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '</table>';

    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">所属成员</a></h2>';

    outString += '<table class="table-style" style="width:100%">';
    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >所属成员</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="memberText" name="memberText" type="text" x-dom-data-type="value" x-dom-feature="contacts" contactTypeText="account" contactMultiSelection="1" value="' + x.isUndefined(param.memberText, '') + '" selectedText="' + x.isUndefined(param.memberView, '') + '" class="textarea-normal" style="width:460px; height:80px;" />';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-3" id="tab-3">备注</a></h2>';

    outString += '<table class="table-style" style="width:100%">';
    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text bold-text" style="width:120px" >备注</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="form-control" style="width:460px; height:80px;" >' + (typeof (param.remark) === 'undefined' ? '' : param.remark) + '</textarea>';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '</div>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(currentPage)*/
  /**
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.membership.role.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/membership.role.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.membership.role.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="/membership/role/report" target="_blank" class="btn btn-default" >报表</a> '
               + '<a href="javascript:main.membership.role.list.openDialog();" class="btn btn-default" >新增</a>'
               + '</div>'
               + '<span>角色管理</span>'
               + '<div class="clearfix" ></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.role.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        var footerHtml = paging.tryParseMenu('javascript:main.membership.role.list.getPaging({0});');

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

    if(id === '')
    {
      url = '/api/membership.role.create.aspx';

      var treeNode = main.membership.role.list.tree.getSelectedNode();

      if(treeNode !== null)
      {
        outString += '<organizationId><![CDATA[' + treeNode.id + ']]></organizationId>';
      }

      isNewObject = true;
    }
    else
    {
      url = '/api/membership.role.findOne.aspx';

      outString += '<id><![CDATA[' + id + ']]></id>';
    }

    outString += '</request>';

    var options = {
      resultType: 'json',
      xml: outString
    };

    x.net.xhr(url, outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="javascript:main.membership.role.list.resetParent();" class="btn btn-default" >重置上级角色</a> '
               + '<a href="javascript:main.membership.role.list.save();" class="btn btn-default" >保存</a> '
               + '<a href="javascript:main.membership.role.list.getPaging(' + main.membership.role.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
               + '</div>'
               + '<span>角色信息</span>'
               + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.role.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        $('#window-main-table-footer')[0].innerHTML = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

        main.membership.role.list.setObjectView(param);

        x.dom.features.bind();

        x.ui.pkg.tabs.newTabs();
      }
    });
  },
  /*#endregion*/

  /*
  * 设置对象的视图
  */
  setObjectView: function(param)
  {
    // x.util.readonly('id');

    // x.util.readonly('code');

    if(param.enableExchangeEmail === '1')
    {
      $('#enableExchangeEmail')[0].checked = true;
    }

    if(param.lock === '1')
    {
      $('#locking')[0].checked = true;
    }

    if(param.status === '1')
    {
      $('#status')[0].checked = true;
    }
  },

  /*#region 函数:checkObject()*/
  /*
  * 检测对象的必填数据
  */
  checkObject: function()
  {
    if(x.dom.data.check()) { return false; }

    // id === '' 为新建的对象
    if($('#id').val() !== '' && $('#parentId').val() === $('#id').val())
    {
      x.msg('上级角色不能为自己本身。');
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
    if(main.membership.role.list.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
      outString += '</request>';

      x.net.xhr('/api/membership.role.save.aspx', outString, {
        waitingMessage: i18n.net.waiting.saveTipText,
        callback: function(response)
        {
          main.membership.role.list.getPaging(main.membership.role.list.paging.currentPage);
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
    if(confirm('确定删除?'))
    {
      x.net.xhr('/api/membership.role.delete.aspx?id=' + id, {
        waitingMessage: i18n.net.waiting.deleteTipText,
        callback: function(response)
        {
          main.membership.role.list.getPaging(main.membership.role.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:resetParent()*/
  resetParent: function()
  {
    $('#parentId').val('');
    $('#parentName').val('');
  },
  /*#endregion*/

  /*#region 函数:getTreeView(organizationId)*/
  /*
  * 获取树形菜单
  */
  getTreeView: function(organizationId)
  {
    var treeViewType = 'organization';
    var treeViewId = '10000000-0000-0000-0000-000000000000';
    var treeViewName = '组织架构';
    var treeViewRootTreeNodeId = organizationId; // 默认值:'00000000-0000-0000-0000-000000000001'
    var treeViewUrl = 'javascript:main.membership.role.list.setTreeViewNode(\'{treeNodeId}\')';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getDynamicTreeView]]></action>';
    outString += '<treeViewType><![CDATA[' + treeViewType + ']]></treeViewType>';
    outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
    outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
    outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
    outString += '<tree><![CDATA[{tree}]]></tree>';
    outString += '<parentId><![CDATA[{parentId}]]></parentId>';
    outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
    outString += '</request>';

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.role.list.tree' });

    tree.setAjaxMode(true);

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/membership.contacts.getDynamicTreeView.aspx', false, outString);

    main.membership.role.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    main.membership.role.list.paging.query.scence = 'QueryByOrganizationUnitId';

    main.membership.role.list.paging.query.where.OrganizationUnitId = value;
    main.membership.role.list.paging.query.orders = ' OrderId ';

    main.membership.role.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /**
   * 页面加载事件
   */
  load: function()
  {
    // 正常加载
    var organizationId = '00000000-0000-0000-0000-000000000001';

    main.membership.role.list.getTreeView(organizationId);

    main.membership.role.list.setTreeViewNode(organizationId);

    // -------------------------------------------------------
    // 过滤事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.membership.role.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.membership.role.list.filter();
    });
  }
  /*#endregion*/
}

$(document).ready(main.membership.role.list.load);