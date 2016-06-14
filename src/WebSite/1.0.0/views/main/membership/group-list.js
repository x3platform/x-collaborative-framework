x.register('main.membership.group.list');

main.membership.group.list = {

  url: "/services/X3Platform/Membership/Ajax.GroupWrapper.aspx",

  paging: x.page.newPagingHelper(),

  maskWrapper: x.ui.mask.newMaskWrapper('main.membership.group.list.maskWrapper'),

  /*#region 函数:filter()*/
  /*
  * 查询
  */
  filter: function()
  {
    var whereClauseValue = '';

    var key = $('#searchText').val();

    if(key.trim() !== '')
    {
      whereClauseValue = ' T.Name LIKE ##%' + key + '%## ';
    }

    main.membership.group.list.paging.whereClause = whereClauseValue;

    main.membership.group.list.paging.orderBy = ' OrderId ';

    main.membership.group.list.getPaging(1);
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
    outString += '<th >名称(全局名称)</th>';
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
      outString += '<td><a href="javascript:main.membership.group.list.openDialog(\'' + node.id + '\');" >' + node.globalName + '(' + node.name + ')</a></td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.modifiedDateView + '</td>';
      if(node.locking === '1')
      {
        outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
      }
      else
      {
        outString += '<td><a href="javascript:main.membership.group.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      }
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr><td colspan="4" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td></tr>';

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
    outString += '<td class="table-body-text" >编码</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + ((typeof (param.id) === 'undefined' || param.id === '0') ? '' : param.id) + '" />';
    outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" />';
    outString += '<input id="originalGlobalName" name="originalGlobalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" />';
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

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">名称</span></td>';
    outString += '<td class="table-body-input" style="width:160px" >';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【名称】必须填写。" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">所属类别</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="groupTreeNodeId" name="groupTreeNodeId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.groupTreeNodeId) === 'undefined' ? '' : param.groupTreeNodeId) + '" />';
    outString += '<input id="groupTreeNodeName" name="groupTreeNodeName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【所属类别】必须填写。" value="' + (typeof (param.groupTreeNodeName) === 'undefined' ? '' : param.groupTreeNodeName) + '" class="form-control" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getGroupTreeWizardSingleton(\'groupTreeNodeName\', \'groupTreeNodeId\', \'40000000-0000-0000-0000-000000000000\', \'常用群组类别\');" >编辑</a> ';
    outString += '</td>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">全局名称</span></td>';
    outString += '<td class="table-body-input" style="width:160px" >';
    outString += '<input id="globalName" name="globalName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【全局名称】必须填写。" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px;" >拼音</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="pinyin" name="pinyin" type="text" x-dom-data-type="value" value="' + (typeof (param.pinyin) === 'undefined' ? '' : param.pinyin) + '" class="form-control" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >排序</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + (typeof (param.orderId) === 'undefined' ? '' : param.orderId) + '" class="form-control x-ajax-checkbox" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >启用企业邮箱</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="enableExchangeEmail" name="enableExchangeEmail" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.enableExchangeEmail) === 'undefined' ? '' : param.enableExchangeEmail) + '" class="checkbox-normal x-ajax-checkbox" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >启用</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.status) === 'undefined' ? '' : param.status) + '" class="checkbox-normal x-ajax-checkbox" />';
    outString += '</td>';
    outString += '<td class="table-body-text" >防止意外删除</td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="locking" name="locking" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.locking) === 'undefined' ? '' : param.locking) + '" class="checkbox-normal x-ajax-checkbox" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >更新时间</td>';
    outString += '<td class="table-body-input" >';
    outString += x.date.newTime(param.modifiedDate).toString('yyyy-MM-dd HH:mm:ss');
    outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.modifiedDateTimestampView, '') + '" />';
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
    outString += '<textarea id="memberView" name="memberView" class="textarea-normal" style="width:460px; height:80px;" >' + (typeof (param.memberView) == 'undefined' ? '' : param.memberView) + '</textarea>';
    outString += '<input id="memberText" name="memberText" type="hidden" x-dom-data-type="value" value="' + (typeof (param.memberText) == 'undefined' ? '' : param.memberText) + '" />';
    outString += '<div class="text-right" style="width:460px;" >';
    outString += '<a href="javascript:x.ui.wizards.getContactsWizardSingleton(\'memberView\', \'memberText\', \'account\');" >编辑</a> ';
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';


    outString += '</table>';

    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-3" id="tab-3">角色</a></h2>';

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
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.membership.group.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/membership.group.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.membership.group.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="javascript:main.membership.group.list.openDialog();" class="btn btn-default" >新增</a>'
               + '</div>'
               + '<span>群组管理</span>'
               + '<div class="clearfix" ></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.group.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        var footerHtml = paging.tryParseMenu('javascript:main.membership.group.list.getPaging({0});');

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
      url = '/api/membership.group.create.aspx';

      var treeNode = main.membership.group.list.tree.getSelectedNode();

      outString += '<treeViewId><![CDATA[' + $('#treeViewId').val() + ']]></treeViewId>';

      if(treeNode !== null)
      {
        outString += '<groupTreeNodeId><![CDATA[' + treeNode.id + ']]></groupTreeNodeId>';
      }

      isNewObject = true;
    }
    else
    {
      url = '/api/membership.group.findOne.aspx';

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
                       + '<a href="javascript:main.membership.group.list.save();" class="btn btn-default" >保存</a> '
                       + '<a href="javascript:main.membership.group.list.getPaging(' + main.membership.group.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                       + '</div>'
                       + '<span>群组管理</span>'
                       + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.group.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        // var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

        // $('#window-main-table-footer')[0].innerHTML = footerHtml;

        main.membership.group.list.setObjectView(param);

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

    if(param.enableExchangeEmail === '1')
    {
      $('#enableExchangeEmail')[0].checked = 'checked';
    }

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
    if(x.dom.data.check()) { return false; }

    if($('#name').val() === '')
    {
      $('#name')[0].focus();
      alert('必须填写[名称]信息');
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
    if(main.membership.group.list.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml' });
      outString += '</request>';

      x.net.xhr('/api/membership.group.save.aspx', outString, {
        waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
        callback: function(response)
        {
          main.membership.group.list.getPaging(main.membership.group.list.paging.currentPage);
        }
      });
    }
    /*
    // 1.数据检测

    if(!main.membership.group.list.checkObject())
    {
        return;
    }

    // 2.发送数据

    var outString = '<?xml version="1.0" encoding="utf-8"?>';

    outString += '<request>';
    outString += '<action><![CDATA[save]]></action>';
    outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
    outString += '<code><![CDATA[' + $("#code").val() + ']]></code>';
    outString += '<name><![CDATA[' + $("#name").val() + ']]></name>';
    outString += '<globalName><![CDATA[' + $("#globalName").val() + ']]></globalName>';
    outString += '<pinyin><![CDATA[' + $("#pinyin").val() + ']]></pinyin>';
    outString += '<groupTreeNodeId><![CDATA[' + $("#groupTreeNodeId").val() + ']]></groupTreeNodeId>';
    outString += '<enableExchangeEmail><![CDATA[' + ($("#enableExchangeEmail")[0].checked ? '1' : '0') + ']]></enableExchangeEmail>';
    outString += '<lock><![CDATA[' + ($("#lock")[0].checked ? '1' : '0') + ']]></lock>';
    outString += '<status><![CDATA[' + ($("#status")[0].checked ? '1' : '0') + ']]></status>';
    outString += '<orderId><![CDATA[' + $("#orderId").val() + ']]></orderId>';
    outString += '<remark><![CDATA[' + $("#remark").val() + ']]></remark>';
    outString += '<memberText><![CDATA[' + $("#memberText").val() + ']]></memberText>';
    outString += '<originalName><![CDATA[' + $("#originalName").val() + ']]></originalName>';
    outString += '<originalGlobalName><![CDATA[' + $("#originalGlobalName").val() + ']]></originalGlobalName>';
    outString += '</request>';

    var options = {
        resultType: 'json',
        xml: outString
    };

    $.post(main.membership.group.list.url,
           options,
           main.membership.group.list.save_callback);
    */
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(ids)*/
  /*
  * 删除对象
  */
  confirmDelete: function(id)
  {
    if(confirm('确定删除?'))
    {
      x.net.xhr('/api/membership.group.delete.aspx?id=' + id, {
        waitingMessage: i18n.strings.msg_net_waiting_delete_tip_text,
        callback: function(response)
        {
          main.membership.group.list.getPaging(main.membership.group.list.paging.currentPage);
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
    var treeViewUrl = 'javascript:main.membership.group.list.setTreeViewNode(\'{treeNodeId}\')';

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

    var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.group.list.tree' });

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

    // tree.add("0", "-1", $('#treeViewName').val(), 'javascript:main.membership.group.list.setTreeViewNode(\'' + $('#treeViewRootTreeNodeId').val() + '\')', $('#treeViewName').val(), '', '/resources/images/tree/tree_icon.gif');

    tree.load('/api/membership.groupTree.getDynamicTreeView.aspx', false, outString);

    main.membership.group.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    // var whereClauseValue = ' T.GroupTreeNodeId = ##' + value + '## ';

    main.membership.group.list.paging.query.where.GroupTreeNodeId = value;

    main.membership.group.list.paging.query.orders = ' OrderId ';

    main.membership.group.list.getPaging(1);
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

    main.membership.group.list.getTreeView(treeViewId);

    main.membership.group.list.setTreeViewNode(treeViewId);

    // $('#treeViewContainer').css('height', '291px');
    // $('#treeViewContainer').css('width', '196px');
    // $('#treeViewContainer').css('overflow', 'auto');

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.membership.group.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.membership.group.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.membership.group.list.load);