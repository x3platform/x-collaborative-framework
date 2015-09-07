x.register('main.applications.application.setting.list');

main.applications.application.setting.list = {

  paging: x.page.newPagingHelper(),

  /*#region 函数:filter()*/
  /*
  * 过滤
  */
  filter: function()
  {
    main.applications.application.setting.list.paging.query.scence = 'Query';
    main.applications.application.setting.list.paging.query.where.ApplicationId = $('#applicationId').val();
    main.applications.application.setting.list.paging.query.where.SearchText = $('#searchText').val().trim();
    main.applications.application.setting.list.paging.orderBy = ' OrderId, Value ';

    main.applications.application.setting.list.getPaging(1);
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

    outString += '<div class="table-freeze-head">';
    outString += '<table class="table" >';
    outString += '<thead>';
    outString += '<tr>';
    outString += '<th style="width:80px">参数代码</th>';
    outString += '<th >参数的文本</th>';
    outString += '<th style="width:120px">参数的值</th>';
    outString += '<th style="width:60px">状态</th>';
    outString += '<th style="width:100px">修改日期</th>';
    outString += '<th style="width:50px">编辑</th>';
    outString += '<th style="width:50px">删除</th>';
    outString += '<th class="table-freeze-head-padding" ><a href="javascript:window$refresh$callback();"><small><span class="glyphicon glyphicon-refresh"></span></small></a></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col style="width:80px" />';
    outString += '<col />';
    outString += '<col style="width:120px" />';
    outString += '<col style="width:60px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:50px" />';
    outString += '<col style="width:50px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td>' + node.code + '</td>';
      outString += '<td><a href="javascript:main.applications.application.setting.list.openDialog(\'' + node.id + '\');">' + node.text + '</a></td>';
      outString += '<td>' + node.value + '</td>';
      outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      outString += '<td><a href="javascript:main.applications.application.setting.list.openDialog(\'' + node.id + '\');">编辑</a></td>';
      outString += '<td><a href="javascript:main.applications.application.setting.list.confirmDelete(\'' + node.id + '\');">删除</a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr><td colspan="8" >&nbsp;</td></tr>';

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
    var outString = '';

    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.id, '') + '" />';
    outString += '<input id="updateDate" name="updateDate" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.updateDateTimestampView, '') + '" />';

    outString += '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >所属应用<span></td>';
    outString += '<td class="table-body-input form-inline">';
    outString += '<div class="input-group">';
    outString += '<input id="applicationDisplayName" name="applicationDisplayName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【所属应用名称】。" class="form-control" style="width:381px;" value="' + (typeof (param.applicationDisplayName) === 'undefined' ? '' : param.applicationDisplayName) + '" readonly="readonly" /> ';
    outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.applicationId) === 'undefined' ? '' : param.applicationId) + '" /> ';
    outString += '<a href="javascript:x.ui.wizards.getApplicationWizard({\'targetValueName\':\'applicationId\',\'targetViewName\':\'applicationName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >所属参数分组<span></td>';
    outString += '<td class="table-body-input form-inline">';
    outString += '<div class="input-group">';
    outString += '<input id="applicationSettingGroupName" name="applicationSettingGroupName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【所属参数分组】。" class="form-control" style="width:381px;" value="' + (typeof (param.applicationSettingGroupName) === 'undefined' ? '' : (param.applicationSettingGroupId === '00000000-0000-0000-0000-000000000000' ? param.applicationDisplayName : param.applicationSettingGroupName)) + '" readonly="readonly" /> ';
    outString += '<input id="applicationSettingGroupId" name="applicationSettingGroupId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.applicationSettingGroupId) === 'undefined' ? '' : param.applicationSettingGroupId) + '" /> ';
    outString += '<a href="javascript:x.ui.wizards.getApplicationSettingGroupWizard({applicationId:$(\'#applicationId\').val(),applicationDisplayName:$(\'#applicationDisplayName\').val(),\'targetValueName\':\'applicationSettingGroupId\',\'targetViewName\':\'applicationSettingGroupName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >参数代码<span></td>';
    if(typeof (param.code) === 'undefined' || param.code === '')
    {
      outString += '<td class="table-body-input">';
      outString += '<span class="gray-text" >自动生成</span>';
      outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
      outString += '</td>';
    }
    else
    {
      outString += '<td class="table-body-input"><input id="code" name="code" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【分组代码】。" class="form-control" style="width:420px;" value="' + (typeof (param.code) === 'undefined' ? '' : param.code) + '" readonly="readonly" /></td>';
    }
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >参数文本<span></td>';
    outString += '<td class="table-body-input"><input id="text" name="text" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【参数文本】。" class="form-control" style="width:420px;" value="' + (typeof (param.text) === 'undefined' ? '' : param.text) + '" /></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >参数值<span></td>';
    outString += '<td class="table-body-input"><input id="value" name="value" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【参数值】。" class="form-control" style="width:420px;" value="' + (typeof (param.value) === 'undefined' ? '' : param.value) + '" /></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >排序</td>';
    outString += '<td class="table-body-input"><input id="orderId" name="orderId" type="text" x-dom-data-type="value" class="form-control" style="width:420px;" value="' + (typeof (param.orderId) === 'undefined' ? '' : param.orderId) + '" /></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >备注</td>';
    outString += '<td class="table-body-input"><textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="textarea-normal" style="width:420px;height:50px;" >' + (typeof (param.remark) === 'undefined' ? '' : param.remark) + '</textarea></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >启用</td>';
    outString += '<td class="table-body-input"><input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.status, '') + '" /></td>';
    outString += '</tr>';

    outString += '</table>';

    return outString;
  },
  /*#endregion*/

  /*#region 函数:getPaging(currentPage)*/
  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.applications.application.setting.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/application.setting.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.applications.application.setting.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
               + '<a href="javascript:main.applications.application.setting.list.openDialog();" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
               + '</div>'
               + '<span>应用参数设置</span>'
               + '<div class="clearfix" ></div>';

        $('#window-main-table-header').html(headerHtml);

        var containerHtml = main.applications.application.setting.list.getObjectsView(list, paging.pagingize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.applications.application.setting.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        main.applications.application.setting.list.resize();
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
    var id = (typeof (value) === 'undefined' || value === 'new' || value === '0') ? '' : value;

    var url = '';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';

    if(id === '')
    {
      url = '/api/application.setting.create.aspx';

      outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';
    }
    else
    {
      url = '/api/application.setting.findOne.aspx';

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
               + '<a href="javascript:main.applications.application.setting.list.save();" class="btn btn-default"><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
               + '<a href="javascript:main.applications.application.setting.list.getPaging(' + main.applications.application.setting.list.paging.currentPage + ');" class="btn btn-default"><i class="fa fa-ban" title="关闭"></i> 关闭</a>'
               + '</div>'
               + '<span>应用参数设置</span>'
               + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.applications.application.setting.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        // $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" style="height:18px" />';

        // main.applications.application.setting.list.setObjectView(param);

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
  //setObjectView: function(param)
  //{
  //    x.util.readonly('id');
  //    x.util.readonly('#applicationName');
  //    x.util.readonly('#applicationSettingGroupName');
  //    x.util.readonly('#code');
  //},
  /*#endregion*/

  /*#region 函数:checkObject()*/
  /**
   * 检测对象的必填数据
   */
  checkObject: function()
  {
    if(x.dom.data.check())
    {
      return false;
    }

    return true;
  },
  /*#endregion*/

  /*#region 函数:save()*/
  save: function()
  {
    if(main.applications.application.setting.list.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml' });
      outString += '</request>';

      x.net.xhr('/api/application.setting.save.aspx', outString, {
        popResultValue: 1,
        waitingMessage: i18n.net.waiting.saveTipText,
        callback: function(response)
        {
          main.applications.application.setting.list.getPaging(main.applications.application.setting.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(id)*/
  /**
   * 删除对象
   */
  confirmDelete: function(id)
  {
    if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
    {
      x.net.xhr('/api/application.setting.delete.aspx?id=' + id, {
        waitingMessage: i18n.net.waiting.deleteTipText,
        callback: function(response)
        {
          main.applications.application.setting.list.getPaging(main.applications.application.setting.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getTreeView(value)*/
  /*
  * 获取树形菜单
  */
  getTreeView: function(value)
  {
    var treeViewId = value;
    var treeViewName = $('#searchApplicationName').val();
    var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
    var treeViewUrl = 'javascript:main.applications.application.setting.list.setTreeViewNode(\'{treeNodeId}\')';

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

    var tree = x.ui.pkg.tree.newTreeView('main.applications.application.setting.list.tree');

    tree.setAjaxMode(true);

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', '[ApplicationId]' + treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/application.settingGroup.getDynamicTreeView.aspx', false, outString);

    main.applications.application.setting.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    main.applications.application.setting.list.paging.query.scence = 'QueryByApplicationSettingGroupId';

    if(value.indexOf('[ApplicationId]') == 0)
    {
      main.applications.application.setting.list.paging.query.where.ApplicationId = value.replace('[ApplicationId]', '');
      main.applications.application.setting.list.paging.query.where.ApplicationSettingGroupId = '00000000-0000-0000-0000-000000000000';
    }
    else
    {
      main.applications.application.setting.list.paging.query.where.ApplicationSettingGroupId = value;
    }

    main.applications.application.setting.list.paging.query.orders = ' OrderId, Value ';

    main.applications.application.setting.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:getApplicationWizard(type)*/
  /*
  * 打开应用查询窗口
  */
  getApplicationWizard: function(type)
  {
    // -------------------------------------------------------
    // 关键代码 开始
    // -------------------------------------------------------

    if(type == 'search')
    {
      main.applications.applicationwizard.localStorage = '{"text":"' + $('#searchApplicationName').val() + '","value":"' + $('#searchApplicationId').val() + '"}';

      // 保存回调函数
      main.applications.applicationwizard.save_callback = function(response)
      {
        var resultView = '';
        var resultValue = '';

        var node = x.toJSON(response);

        resultView += node.text + ';';
        resultValue += node.value + ';';

        if(resultValue.substr(resultValue.length - 1, 1) == ';')
        {
          resultView = resultView.substr(0, resultView.length - 1)
          resultValue = resultValue.substr(0, resultValue.length - 1);
        }

        $('#searchApplicationName').val(resultView);
        $('#searchApplicationId').val(resultValue);

        main.applications.applicationwizard.localStorage = response;

        // 回调后 重新加载树
        var treeViewRootTreeNodeId = $('#searchApplicationId').val();

        main.applications.application.setting.list.getTreeView(treeViewRootTreeNodeId);

        main.applications.application.setting.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);
      }
    }
    else
    {
      main.applications.applicationwizard.localStorage = '{"text":"' + $('#applicationName').val() + '","value":"' + $('#applicationId').val() + '"}';

      // 保存回调函数
      main.applications.applicationwizard.save_callback = function(response)
      {
        var resultView = '';
        var resultValue = '';

        var node = x.toJSON(response);

        resultView += node.text + ';';
        resultValue += node.value + ';';

        if(resultValue.substr(resultValue.length - 1, 1) == ';')
        {
          resultView = resultView.substr(0, resultView.length - 1)
          resultValue = resultValue.substr(0, resultValue.length - 1);
        }

        $('#applicationName').val(resultView);
        $('#applicationId').val(resultValue);

        $('#applicationSettingGroupName').val(resultView);
        $('#applicationSettingGroupId').val('00000000-0000-0000-0000-000000000000');

        main.applications.applicationwizard.localStorage = response;
      }
    };

    // 取消回调函数
    // 注:执行完保存回调函数后, 默认执行取消回调函数.
    main.applications.applicationwizard.cancel_callback = function(response)
    {
      if(main.applications.applicationwizard.maskWrapper !== '')
      {
        main.applications.applicationwizard.maskWrapper.close();
      }
    };

    // -------------------------------------------------------
    // 关键代码 结束
    // -------------------------------------------------------

    // 非模态窗口, 需要设置.
    main.applications.applicationwizard.maskWrapper = main.applications.application.setting.list.maskWrapper;

    // 加载地址簿信息
    main.applications.applicationwizard.load();

    main.applications.applicationwizard.maskWrapper.resize();
  },
  /*#endregion*/

  /*#region 函数:getApplicationSettingGroupWizard()*/
  /*
  * 打开应用参数分组查询窗口
  */
  getApplicationSettingGroupWizard: function()
  {
    // -------------------------------------------------------
    // 关键代码 开始
    // -------------------------------------------------------

    main.applications.applicationsettinggroupwizard.setTreeView($('#applicationId').val(), $('#applicationName').val());

    main.applications.applicationsettinggroupwizard.localStorage = '{"text":"' + $('#applicationSettingGroupName').val() + '","value":"' + $('#applicationSettingGroupId').val() + '"}';

    // 保存回调函数
    main.applications.applicationsettinggroupwizard.save_callback = function(response)
    {
      var resultView = '';
      var resultValue = '';

      var node = x.toJSON(response);

      if(response === "{}")
      {
        $('#applicationSettingGroupName').val('');
        $('#applicationSettingGroupId').val('');
        return;
      }

      resultView += node.text + ';';
      resultValue += node.value + ';';

      if(resultValue.substr(resultValue.length - 1, 1) === ';')
      {
        resultView = resultView.substr(0, resultView.length - 1);
        resultValue = resultValue.substr(0, resultValue.length - 1);
      }

      $('#applicationSettingGroupName').val(resultView);
      $('#applicationSettingGroupId').val(resultValue);

      main.applications.applicationsettinggroupwizard.localStorage = response;
    };

    // 取消回调函数
    // 注:执行完保存回调函数后, 默认执行取消回调函数.
    main.applications.applicationsettinggroupwizard.cancel_callback = function(response)
    {
      if(main.applications.applicationsettinggroupwizard.maskWrapper !== '')
      {
        main.applications.applicationsettinggroupwizard.maskWrapper.close();
      }
    };

    // -------------------------------------------------------
    // 关键代码 结束
    // -------------------------------------------------------

    // 非模态窗口, 需要设置.
    main.applications.applicationsettinggroupwizard.maskWrapper = main.applications.application.setting.list.maskWrapper;

    // 加载地址簿信息
    main.applications.applicationsettinggroupwizard.load();
  },
  /*#endregion*/

  /*#region 函数:resize()*/
  /*
  * 页面大小调整
  */
  resize: function()
  {
    var height = x.page.getViewHeight();

    var freezeHeight = 0;

    $('.x-freeze-height').each(function(index, node)
    {
      freezeHeight += $(node).outerHeight();
    });

    var freezeTableHeadHeight = $('#window-main-table-body .table-freeze-head').outerHeight();
    var freezeTableSidebarSearchHeight = $('#window-main-table-body .table-sidebar-search').outerHeight();

    $('#treeViewContainer').css({
      'height': (height - freezeHeight - freezeTableSidebarSearchHeight) + 'px',
      'overflow': 'auto'
    });

    $('#window-main-table-body .table-freeze-body').css(
    {
      'height': (height - freezeHeight - freezeTableHeadHeight) + 'px',
      'overflow-y': 'scroll'
    });

    $('.table-freeze-head-padding').css({ width: x.page.getScrollBarWidth().vertical, display: (x.page.getScrollBarWidth().vertical == 0 ? 'none' : '') });
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /**
   * 页面加载事件
   */
  load: function()
  {
    // 调整页面结构尺寸
    main.applications.application.setting.list.resize();

    var treeViewRootTreeNodeId = $('#searchApplicationId').val();

    main.applications.application.setting.list.getTreeView(treeViewRootTreeNodeId);

    main.applications.application.setting.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#btnApplicationWizard').bind('click', function()
    {
      x.ui.wizards.getApplicationWizard({
        targetValueName: 'searchApplicationId',
        targetViewName: 'searchApplicationName',
        save_callback: function(response)
        {
          var resultView = '';
          var resultValue = '';

          var node = x.toJSON(response);

          resultView += node.text + ';';
          resultValue += node.value + ';';

          if(resultValue.substr(resultValue.length - 1, 1) == ';')
          {
            resultView = resultView.substr(0, resultView.length - 1)
            resultValue = resultValue.substr(0, resultValue.length - 1);
          }

          $('#searchApplicationName').val(resultView);
          $('#searchApplicationId').val(resultValue);

          // 回调后 重新加载树
          var treeViewRootTreeNodeId = $('#searchApplicationId').val();

          main.applications.application.setting.list.getTreeView(treeViewRootTreeNodeId);

          main.applications.application.setting.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);
        }
      });
    });

    $('#searchText').bind('keyup', function()
    {
      main.applications.application.setting.list.filter();
    });

    $('#btnFilter').bind('click', function()
    {
      main.applications.application.setting.list.filter();
    });
  }
  /*#endregion*/
};

$(document).ready(main.applications.application.setting.list.load);
// 重新调整页面大小
$(window).resize(main.applications.application.setting.list.resize);
// 重新调整页面大小
$(document.body).resize(main.applications.application.setting.list.resize);
