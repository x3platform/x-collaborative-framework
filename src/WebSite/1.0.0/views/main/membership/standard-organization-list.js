x.register('main.membership.standard.organization.list');

main.membership.standard.organization.list = {

  paging: x.page.newPagingHelper(50),

  maskWrapper: x.ui.mask.newMaskWrapper('main.membership.standard.organization.list.maskWrapper'),

  /*#region 函数:filter()*/
  /*
  * 过滤
  */
  filter: function()
  {
    var key = $('#searchText').val();

    if(key.trim() !== '')
    {
      main.membership.standard.organization.list.paging.query.where.Name = key;
    }

    main.membership.standard.organization.list.paging.query.orderBy = ' OrderId ';

    main.membership.standard.organization.list.getPaging(1);
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
      outString += '<td><a href="javascript:main.membership.standard.organization.list.openDialog(\'' + node.id + '\');" >' + node.name + '(' + node.globalName + ')</a></td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      if(node.locking === '1')
      {
        outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
      }
      else
      {
        outString += '<td><a href="javascript:main.membership.standard.organization.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
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

    // return x.template({ fileName: '#template-list', data: { list: list } });
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
    outString += '<li><a href="#tab-2">所属组织</a></li>';
    outString += '</ul>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="tab-1" name="tab-1" >基本信息</a></h2>';
    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" >编码</td>';
    outString += '<td class="table-body-input" style="width:160px" >';

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
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">上级标准组织</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) === 'undefined' ? '' : param.parentId) + '" />';
    outString += '<input id="parentName" name="parentName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【上级标准组织】必须填写。" value="' + (typeof (param.parentName) === 'undefined' ? '' : param.parentName) + '" class="form-control" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getContactWizard({targetViewName:\'parentName\',targetValueName:\'parentId\',contactTypeText:\'standard-organization\'});" >编辑</a> ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" ><span class="required-text">名称</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" />';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【名称】必须填写。" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text bold-text" ><span class="required-text">全局名称</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="originalGlobalName" name="originalGlobalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" />';
    outString += '<input id="globalName" name="globalName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【全局名称】必须填写。" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text bold-text" >排序</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + (typeof (param.orderId) === 'undefined' ? '' : param.orderId) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >启用</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.status) === 'undefined' ? '' : param.status) + '" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >防止意外删除</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="locking" name="locking" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.locking) === 'undefined' ? '' : param.locking) + '"  />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >备注</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="form-control x-ajax-textarea" style="width:460px; height:40px;" >' + (typeof (param.remark) === 'undefined' ? '' : param.remark) + '</textarea>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >修改时间</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += (typeof (param.updateDateTimestampView) === 'undefined' ? '' : param.updateDateTimestampView);
    outString += '<input id="updateDate" name="updateDate" type="hidden" x-dom-data-type="value" value="' + (typeof (param.updateDateTimestampView) === 'undefined' ? '' : param.updateDateTimestampView) + '" />';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';

    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">所属组织</a></h2>';

    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >所属成员</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="accountView" name="accountView" class="form-control x-ajax-textarea" style="width:460px; height:80px;" >' + (typeof (param.accountView) === 'undefined' ? '' : param.accountView) + '</textarea>';
    outString += '<input id="accountValue" name="accountValue" type="hidden" value="' + (typeof (param.accountValue) === 'undefined' ? '' : param.accountValue) + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >所属组织</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="organizationView" name="organizationView" class="form-control x-ajax-textarea" style="width:460px; height:80px;" >' + (typeof (param.organizationView) === 'undefined' ? '' : param.organizationView) + '</textarea>';
    outString += '<input id="organizationValue" name="organizationValue" type="hidden" value="' + (typeof (param.organizationValue) === 'undefined' ? '' : param.organizationValue) + '" />';
    // outString += '<div class="text-right" style="width:460px;" >';
    // outString += '<a href="javascript:x.ui.wizards.getContactsWizard({targetViewName:\'organizationView\',targetValueName:\'organizationValue\',contactTypeText:\'organization\'});" >编辑</a> ';
    // outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '</table>';
    outString += '</div>';

    outString += '</div>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(value)*/
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.membership.standard.organization.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/membership.standardOrganization.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.membership.standard.organization.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.membership.standard.organization.list.openDialog();" class="btn btn-default" >新增</a>'
                       + '</div>'
                       + '<span>标准组织管理</span>'
                       + '<div class="clearfix" ></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.standard.organization.list.getObjectsView(list, paging.pagingize);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        var footerHtml = paging.tryParseMenu('javascript:main.membership.standard.organization.list.getPaging({0});');

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
    if(id === '')
    {
      url = '/api/membership.standardOrganization.create.aspx';

      var treeNode = main.membership.standard.organization.list.tree.getSelectedNode();

      if(treeNode !== null)
      {
        outString += '<parentId><![CDATA[' + treeNode.id + ']]></parentId>';
      }
    }
    else
    {
      url = '/api/membership.standardOrganization.findOne.aspx';

      outString += '<id><![CDATA[' + id + ']]></id>';
    }
    outString += '</request>';

    x.net.xhr(url, outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.membership.standard.organization.list.save();" class="btn btn-default" >保存</a> '
                       + '<a href="javascript:main.membership.standard.organization.list.getPaging(' + main.membership.standard.organization.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                       + '</div>'
                       + '<span>标准组织信息</span>'
                       + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.standard.organization.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        // var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

        // $('#window-main-table-footer')[0].innerHTML = footerHtml;

        main.membership.standard.organization.list.setObjectView(param);

        x.dom.features.bind();

        x.ui.pkg.tabs.newTabs();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:setObjectView(param)*/
  /*
  * 设置对象的视图
  */
  setObjectView: function(param)
  {
    // x.util.readonly('id');
    // x.util.readonly('code');

    if(param.lock === '1')
    {
      $('#lock')[0].checked = 'checked';
    }

    if(param.status === '1')
    {
      $('#status')[0].checked = 'checked';
    }
  },
  /*#endregion*/

  /*#region 函数:checkObject()*/
  /*
  * 检测对象的必填数据
  */
  checkObject: function()
  {
    if(x.dom.data.check())
    {
      return false;
    }

    // id === '' 为新建的对象
    if($('#id').val() !== '' && $('#parentId').val() === $('#id').val())
    {
      alert('上级角色不能为自己本身。');
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
    if(!main.membership.standard.organization.list.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml' });
      outString += '</request>';

      x.net.xhr('/api/membership.standardOrganization.save.aspx', outString, {
        waitingMessage: i18n.net.waiting.saveTipText,
        callback: function(response)
        {
          main.membership.standard.organization.list.getPaging(main.membership.standard.organization.list.paging.currentPage);
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
      x.net.xhr('/api/membership.standardOrganization.delete.aspx?id=' + id, {
        waitingMessage: i18n.net.waiting.deleteTipText,
        callback: function(response)
        {
          main.membership.standard.organization.list.getPaging(main.membership.standard.organization.list.paging.currentPage);
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
    var treeViewUrl = 'javascript:main.membership.standard.organization.list.setTreeViewNode(\'{treeNodeId}\')';

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

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.standard.organization.list.tree' });

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

    // tree.add("0", "-1", $('#treeViewName').val(), 'javascript:main.membership.standard.organization.list.setTreeViewNode(\'' + $('#treeViewRootTreeNodeId').val() + '\')', $('#treeViewName').val(), '', '/resources/images/tree/tree_icon.gif');

    tree.load('/api/membership.standardOrganization.getDynamicTreeView.aspx', false, outString);

    main.membership.standard.organization.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value, text)*/
  setTreeViewNode: function(value, text)
  {
    main.membership.standard.organization.list.paging.query.where.ParentId = value;

    main.membership.standard.organization.list.paging.query.orders = ' OrderId ';

    main.membership.standard.organization.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:getContactsWindow(type)*/
  /*
  * 打开联系人窗口
  */
  getContactsWindow: function(type)
  {
    type = (typeof (type) === 'undefined') ? 'default' : type;

    var storage = '';
    var viewArray = [];
    var valueArray = [];

    if(type === 'parent')
    {
      main.contacts.home.contactType = 16;

      main.contacts.home.multiSelection = 0;

      viewArray = $('#parentName').val().split(';');
      valueArray = $('#parentId').val().split(',');

      storage += '{"list":[';

      for(var i = 0;i < valueArray.length;i++)
      {
        if(valueArray[i] !== '')
        {
          storage += '{"text":"' + viewArray[i] + '","value":"organization#' + valueArray[i] + '#' + viewArray[i] + '"},';
        }
      }

      if(storage.substr(storage.length - 1, 1) === ',')
      {
        storage = storage.substr(0, storage.length - 1);
      }

      storage += ']}';

      main.contacts.home.localStorage = storage;

      main.contacts.home.save_callback = function(response)
      {
        var resultView = '';
        var resultText = '';

        var list = x.toJSON(response).list;

        x.each(list, function(index, node)
        {
          resultView += node.text + ';';
          resultText += node.value.substring(node.value.indexOf('#') + 1, node.value.lastIndexOf('#')) + ',';
        });

        if(resultText.substr(resultText.length - 1, 1) === ',')
        {
          resultView = resultView.substr(0, resultView.length - 1);
          resultText = resultText.substr(0, resultText.length - 1);
        }

        $('#parentName').val(resultView);
        $('#parentId').val(resultText);

        main.contacts.home.localStorage = response;
      };
    }

    main.contacts.home.cancel_callback = function(response)
    {
      main.membership.standard.organization.list.maskWrapper.close();
    };

    //
    // 关键代码 结束
    //

    // 非模态窗口, 需要设置.
    main.contacts.home.maskWrapper = main.membership.standard.organization.list.maskWrapper;

    // 加载地址簿信息
    main.contacts.home.load();
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    var treeViewId = $('#treeViewId').val();

    main.membership.standard.organization.list.getTreeView();

    main.membership.standard.organization.list.setTreeViewNode(treeViewId);

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.membership.standard.organization.list.filter();
    });

    $('.table-sidebar-search-button').bind('click', function()
    {
      main.membership.standard.organization.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.membership.standard.organization.list.load);
