x.register('main.membership.organization.list');

main.membership.organization.list = {

  paging: x.page.newPagingHelper(50),

  // 组织结构根节点
  organizationRootId: '00000000-0000-0000-0000-000000000001',

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    var whereClauseValue = '';

    var key = $('#searchText').val().trim();

    if(key !== '')
    {
      whereClauseValue = ' T.Name LIKE ##%' + x.toSafeLike(key) + '%## OR T.GlobalName LIKE ##%' + x.toSafeLike(key) + '%## ';
    }

    main.membership.organization.list.paging.whereClause = whereClauseValue;

    main.membership.organization.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:getObjectsView(list, maxCount)*/
  /*
  * 创建对象列表的视图
  */
  getObjectsView: function(list, maxCount)
  {
    var counter = 0;

    var classNameValue = '';

    var outString = '';

    outString += '<div class="table-freeze-head">';
    outString += '<table class="table" >';
    outString += '<thead>';
    outString += '<tr>';
    outString += '<th style="width:180px" >名称</th>';
    outString += '<th >全局名称</th>';
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
    outString += '<col style="width:180px" />';
    outString += '<col />';
    outString += '<col style="width:40px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td><a href="javascript:main.membership.organization.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
      outString += '<td>' + node.globalName + '</td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      outString += '<td><a href="/apps/paging/membership/organization-validator.aspx?organizationId=' + node.id + '" target="_blank" >数据验证</a></td>';
      if(node.locking === '1')
      {
        outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
      }
      else
      {
        outString += '<td><a href="javascript:main.membership.organization.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
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
    outString += '<li><a href="#tab-2">所属角色</a></li>';
    outString += '<li><a href="#tab-3">备注</a></li>';
    outString += '</ul>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="tab-1" name="tab-1" >基本信息</a></h2>';
    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent" ' + ((param.id === '00000000-0000-0000-0000-000000000001') ? 'style="display:none;"' : '') + ' >';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">上级组织</span></td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.parentId, '') + '" />';
    outString += '<input id="parentName" name="parentName" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.parentGlobalName, '') + '" class="form-control" style="width:120px;" /> ';
    outString += '<a href="javascript:main.membership.organization.list.setParentNodeWizard(\'' + param.parentId + '\');" >编辑</a> ';

    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" >编号</td>';
    outString += '<td class="table-body-input" style="width:160px" >';
    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.id, '') + '" />';
    outString += '<input id="level" name="level" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.level, '') + '" />';
    outString += '<input id="fullPath" name="fullPath" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.fullPath, '') + '" />';
    outString += '<input id="distinguishedName" name="distinguishedName" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.distinguishedName, '') + '" />';
    if(typeof (param.code) === 'undefined' || param.code === '')
    {
      outString += '<span class="gray-text">自动编号</span>';
      outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
    }
    else
    {
      outString += '<input id="code" name="code" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.code, '') + '" class="form-control" style="width:120px;" />';
    }
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >全称</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="fullName" name="fullName" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.fullName, '') + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">名称</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.name, '') + '" />';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【名称】必须填写。" value="' + x.isUndefined(param.name, '') + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >所属标准组织</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="standardOrganizationId" name="standardOrganizationId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.standardOrganizationId, '') + '" selectedText="' + x.isUndefined(param.standardOrganizationName, '') + '" x-dom-feature="contacts" contactTypeText="standard-organization" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text">全局名称</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="originalGlobalName" name="originalGlobalName" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.globalName, '') + '" />';
    outString += '<input id="globalName" name="globalName" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.globalName, '') + '" x-dom-data-required="1" x-dom-data-required-warning="【全局名称】必须填写。" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >拼音</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="pinyin" name="pinyin" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.pinyin, '') + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text">组织类型</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="type" name="type" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.type, '') + '" x-dom-feature="combobox" selectedText="' + x.isUndefined(param.typeView, '') + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_组织管理_组织类别## ) " class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >作用范围</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="effectScope" name="effectScope" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.effectScope, '') + '" x-dom-feature="combobox" selectedText="' + (typeof (param.effectScopeView) === 'undefined' ? '' : param.effectScopeView) + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_权限作用范围## ) " class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >排序标识</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.orderId, '') + '" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >启用企业邮箱</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="enableExchangeEmail" name="enableExchangeEmail" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.enableExchangeEmail, '0') + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >启用</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.status, '0') + '" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >防止意外删除</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="lock" name="lock" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.lock, '0') + '" />';
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
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">角色</a></h2>';

    outString += '<table class="table-style" style="width:100%">';
    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >角色</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="roleText" name="roleText" x-dom-data-type="value" class="textarea-normal" style="width:460px; height:80px;" />' + x.isUndefined(param.roleView, '') + '</textarea>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >成员</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="roleMemberView" name="roleMemberView" class="textarea-normal" style="width:460px;height:80px;" >' + x.isUndefined(param.roleMemberView, '') + '</textarea>';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-3" id="tab-3">备注</a></h2>';

    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" >备注</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="textarea-normal x-ajax-textarea" style="width:460px; height:80px;" >' + x.isUndefined(param.remark, '') + '</textarea>';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

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
    var paging = main.membership.organization.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/membership.organization.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.membership.organization.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="javascript:main.membership.organization.list.openDialog();" class="btn btn-default" >新增</a>'
               + '</div>'
               + '<span>组织管理</span>'
               + '<div class="clearfix" ></div>';

        $('#window-main-table-header').html(headerHtml);

        var containerHtml = main.membership.organization.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.membership.organization.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

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
      url = '/api/membership.organization.create.aspx';

      var treeNode = main.membership.organization.list.tree.getSelectedNode();

      if(treeNode != null)
      {
        outString += '<parentId><![CDATA[' + treeNode.id + ']]></parentId>';
      }

      isNewObject = true;
    }
    else
    {
      url = '/api/membership.organization.findOne.aspx';

      outString += '<id><![CDATA[' + id + ']]></id>';
    }

    outString += '</request>';

    x.net.xhr(url, outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        var headerHtml = '';

        if(isNewObject)
        {
          headerHtml = '<div id="toolbar" class="table-toolbar" >'
             + '<a href="javascript:main.membership.organization.list.save();" class="btn btn-default" >保存</a> '
             + '<a href="javascript:main.membership.organization.list.getPaging(' + main.membership.organization.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
             + '</div>'
             + '<span>组织信息</span>'
             + '<div class="clear"></div>';
        }
        else
        {
          headerHtml = '<div id="toolbar" class="table-toolbar" >'
             + '<a href="javascript:main.membership.organization.list.createChildNode(\'' + param.id + '\',\'' + param.name + '\');" class="btn btn-default" >创建子级组织</a> '
             + '<a href="javascript:main.membership.organization.list.save();" class="btn btn-default" >保存</a> '
             + '<a href="javascript:main.membership.organization.list.getPaging(' + main.membership.organization.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
             + '</div>'
             + '组织信息'
             + '<div class="clear"></div>';
        }

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.organization.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        // $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" alt="" style="height:18px" />';

        // main.membership.organization.list.setObjectView(param);

        x.dom.features.bind();

        x.ui.pkg.tabs.newTabs();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:createChildNode(parentId, parentName)*/
  /*
  * 创建子组织
  */
  createChildNode: function(parentId, parentName)
  {
    x.net.xhr('/api/membership.organization.create.aspx?parentId=' + parentId, {
      waitingType: 'mini',
      waitingMessage: '正在查询数据，请稍后......',
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="javascript:main.membership.organization.list.save();" class="btn btn-default" >保存</a> '
               + '<a href="javascript:main.membership.organization.list.getPaging(' + main.membership.organization.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
               + '</div>'
               + '组织信息'
               + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.organization.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        // $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" alt="" style="height:18px" />';

        // main.membership.organization.list.setObjectView(param);

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

    if($('#parentId').val() === $('#id').val())
    {
      alert('上级组织不能为自己本身。');
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
    if(main.membership.organization.list.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml' });
      outString += '</request>';

      x.net.xhr('/api/membership.organization.save.aspx', outString, {
        waitingMessage: i18n.net.waiting.saveTipText,
        callback: function(response)
        {
          main.membership.organization.list.getPaging(main.membership.organization.list.paging.currentPage);
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
      x.net.xhr('/api/membership.organization.delete.aspx?id=' + id, {
        waitingMessage: i18n.net.waiting.deleteTipText,
        callback: function(response)
        {
          main.membership.organization.list.getPaging(main.membership.organization.list.paging.currentPage);
        }
      });
    }
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
    var treeViewUrl = 'javascript:main.membership.organization.list.setTreeViewNode(\'{treeNodeId}\')';

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

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.organization.list.tree' });

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

    main.membership.organization.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    // var whereClauseValue = ' T.ParentId = ##' + value + '## ';

    // main.membership.organization.list.paging.whereClause = whereClauseValue;
    main.membership.organization.list.paging.query.where.ParentId = value;
    main.membership.organization.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:setParentNodeWizard()*/
  /*
  * 设置父级节点
  */
  setParentNodeWizard: function()
  {
    x.ui.wizards.getWizard('parentNode', {

      bindOptions: function() { },

      save_callback: function()
      {
        x.dom('#parentName').val(x.dom('#parentNode-wizardText').val());
        x.dom('#parentId').val(x.dom('#parentNode-wizardValue').val());

        return 0;
      },

      create: function()
      {
        var outString = '';

        var counter = 0;

        outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:300px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>父级节点设置向导</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<div id="parentNode-wizardTreeViewContainer" class="winodw-wizard-tree-view" style="height:300px;" ></div>';

        outString += '<div class="winodw-wizard-result-container form-inline text-right" >';
        outString += '<label class="winodw-wizard-result-item-text" >名称</label> ';
        outString += '<input id="parentNode-wizardText" name="parentNode-wizardText" type="text" value="' + $('#parentName').val() + '" class="winodw-wizard-result-item-input form-control input-sm" style="width:160px" /> ';
        outString += '<input id="parentNode-wizardValue" name="parentNode-wizardValue" type="hidden" value="' + $('#parentId').val() + '" />';
        outString += '<a href="javascript:' + this.name + '.save();" class="btn btn-default btn-sm" >确定</a>';
        outString += '</div>';

        return outString;
      }
    });

    x.net.xhr('/api/membership.contacts.getTreeView.aspx?organizationId=' + main.membership.organization.list.organizationRootId, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var list = result.data;

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.organization.list.changeParentTree' });

        var rootIndex = 0;

        tree.add({
          id: rootIndex,
          parentId: -1,
          name: '组织结构',
          url: 'javascript:main.membership.organization.list.setParentNode(\'组织结构\',\'00000000-0000-0000-0000-000000000001\');',
          title: '组织结构',
          target: '',
          icon: '/resources/images/tree/tree_icon.gif'
        });

        x.each(list, function(index, node)
        {
          // tree.add(node.id, rootIndex, '<span id="' + node.id + '" >' + node.name + '</span>', 'javascript:main.membership.organization.list.setParentNode(\'' + node.name + '\',\'' + node.id + '\');', node.name);
          tree.add({
            id: node.id,
            parentId: rootIndex,
            name: '<span id="' + node.id + '" >' + node.name + '</span>',
            url: 'javascript:main.membership.organization.list.setParentNode(\'' + node.name + '\',\'' + node.id + '\');',
            title: node.name
          });

          main.membership.organization.list.bindParnetTreeViewChildNodes(tree, node);
        });

        main.membership.organization.list.changeParentTree = tree;

        document.getElementById('parentNode-wizardTreeViewContainer').innerHTML = tree;
      }
    });
  },

  bindParnetTreeViewChildNodes: function(tree, node)
  {
    if(typeof (node.childNodes) != 'undefined')
    {
      x.each(node.childNodes, function(index, childNode)
      {
        // tree.add(childNode.id, node.id, '<span id="' + childNode.id + '" >' + childNode.name + '</span>', 'javascript:main.membership.organization.list.setParentNode(\'' + childNode.name + '\',\'' + childNode.id + '\');', childNode.name);
        tree.add({
          id: childNode.id,
          parentId: node.id,
          name: '<span id="' + childNode.id + '" >' + childNode.name + '</span>',
          url: 'javascript:main.membership.organization.list.setParentNode(\'' + childNode.name + '\',\'' + childNode.id + '\');',
          title: childNode.name
        });

        main.membership.organization.list.bindParnetTreeViewChildNodes(tree, childNode);
      });
    }
  },
  /*#endregion*/

  /*#region 函数:setParentNode(name, value)*/
  setParentNode: function(name, value)
  {
    if($('#id').val() === value)
    {
      alert('当前节点为【' + name + '】，父级节点不能为自己本身。');
      return;
    }

    x.dom('#parentNode-wizardText').val(name);
    x.dom('#parentNode-wizardValue').val(value);
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    // 正常加载
    var organizationId = '00000000-0000-0000-0000-000000000001';

    main.membership.organization.list.getTreeView(organizationId);

    main.membership.organization.list.setTreeViewNode(organizationId);

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.membership.organization.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.membership.organization.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.membership.organization.list.load);