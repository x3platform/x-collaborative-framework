x.register('main.applications.application.feature.list');

main.applications.application.feature.list = {

  paging: x.page.newPagingHelper(50),

  /*
  * 过滤
  */
  filter: function()
  {
    var whereClauseValue = '';

    var key = $('#searchText').val();

    if(key.trim() != '')
    {
      whereClauseValue = ' ( T.Code LIKE ##%' + key + '%## OR T.Name LIKE ##%' + key + '%## ) ';
    }

    main.applications.application.feature.list.whereClause = whereClauseValue;

    main.applications.application.feature.list.paging.whereClause = whereClauseValue;

    main.applications.application.feature.list.getPaging(1);
  },

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
    outString += '<th style="width:100px" >功能代码</th>';
    outString += '<th >功能名称</th>';
    outString += '<th style="width:100px" >类型</th>';
    outString += '<th style="width:60px" >状态</th>';
    outString += '<th style="width:100px" >修改日期</th>';
    outString += '<th style="width:50px" >编辑</th>';
    outString += '<th style="width:50px">删除</th>';
    outString += '<th class="table-freeze-head-padding" ><a href="javascript:window$refresh$callback();"><small><span class="glyphicon glyphicon-refresh"></span></small></a></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col style="width:100px" />';
    outString += '<col />';
    outString += '<col style="width:100px" />';
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
      outString += '<td><a href="javascript:main.applications.application.feature.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
      outString += '<td>' + (node.type == 'function' ? '功能点' : '动作点') + '</td>';
      outString += '<td>' + (node.status == '1' ? '<span class="green-text">启用</span>' : '<span class="red-text">禁用</span>') + '</td>';
      outString += '<td>' + node.updateDateView + '</td>';
      outString += '<td><a href="javascript:main.applications.application.feature.list.openDialog(\'' + node.id + '\');" >编辑</a></td>';
      outString += '<td><a href="javascript:main.applications.application.feature.list.confirmDelete(\'' + node.id + '\',\'' + node.name + '\');" >删除</a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      outString += '<tr><td colspan="7" >&nbsp;</td></tr>';

      counter++;
    }

    outString += '</tbody>';
    outString += '</table>';
    outString += '</div>';

    return outString;
  },

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
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >所属应用名称<span></td>';
    outString += '<td class="table-body-input form-inline">';
    outString += '<div class="input-group">';
    outString += '<input id="applicationDisplayName" name="applicationDisplayName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【所属应用名称】。" value="' + x.isUndefined(param.applicationDisplayName, '') + '" class="form-control" style="width:381px;" /> ';
    outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.applicationId, '') + '" /> ';
    outString += '<a href="javascript:x.ui.wizards.getApplicationWizard({\'targetValueName\':\'applicationId\',\'targetViewName\':\'applicationDisplayName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >父级功能名称<span></td>';
    outString += '<td class="table-body-input form-inline">';
    outString += '<div class="input-group">';
    outString += '<input id="parentName" name="parentName" type="text" x-dom-data-type="value" class="form-control custom-forms-data-required" style="width:381px;" x-dom-data-required-warning="必须填写【父级功能名称】。" value="' + x.isUndefined(param.parentName, '') + '" /> ';
    outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.parentId, '') + '" /> ';
    outString += '<a href="javascript:x.ui.wizards.getApplicationFeatureWizard({applicationId:$(\'#applicationId\').val(),applicationDisplayName:$(\'#applicationDisplayName\').val(),targetViewName:\'parentName\',targetValueName:\'parentId\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >功能代码<span></td>';
    if(typeof (param.code) == 'undefined' || param.code == '')
    {
      outString += '<td class="table-body-input">';
      outString += '<span class="gray-text" >自动生成</span>';
      outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
      outString += '</td>';
    }
    else
    {
      outString += '<td class="table-body-input"><input id="code" name="code" type="text" x-dom-data-type="value" class="form-control custom-forms-data-required" style="width:420px;" x-dom-data-required-warning="必须填写【功能代码】。" value="' + x.isUndefined(param.code, '') + '" /></td>';
    }
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >功能名称<span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" class="form-control custom-forms-data-required" style="width:420px;" x-dom-data-required-warning="必须填写【功能名称】。" value="' + x.isUndefined(param.name, '') + '" />';
    outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.name, '') + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >功能显示名称<span></td>';
    outString += '<td class="table-body-input"><input id="displayName" name="displayName" type="text" x-dom-data-type="value" class="form-control custom-forms-data-required" style="width:420px;" x-dom-data-required-warning="必须填写【功能显示名称】。" value="' + x.isUndefined(param.displayName, '') + '" /></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text" >功能类型<span></td>';
    outString += '<td class="table-body-input"><input id="type" name="type" type="text" x-dom-data-type="value" class="form-control custom-forms-data-required" style="width:381px;" x-dom-data-required-warning="必须填写【功能类型】。" value="' + x.isUndefined(param.type, '') + '" x-dom-feature="combobox" data="[{text:\'功能\',value:\'function\'},{text:\'动作\',value:\'action\'}]" /></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >排序</td>';
    outString += '<td class="table-body-input"><input id="orderId" name="orderId" type="text" x-dom-data-type="value" class="form-control" style="width:420px;" value="' + x.isUndefined(param.orderId, '') + '" /></td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" >备注</td>';
    outString += '<td class="table-body-input"><textarea id="remark" name="remark" rows="3" x-dom-data-type="value" class="form-control" style="width:420px;" >' + x.isUndefined(param.remark, '') + '</textarea></td>';
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
    var paging = main.applications.application.feature.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/application.feature.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.applications.application.feature.list.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.applications.application.feature.list.openDialog();" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
                       + '</div>'
                       + '<span>应用功能配置</span>'
                       + '<div class="clearfix" ></div>';

        $('#window-main-table-header').html(headerHtml);

        var containerHtml = main.applications.application.feature.list.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.applications.application.feature.list.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);

        main.applications.application.feature.list.resize();
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
      url = '/api/application.feature.create.aspx';

      outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';
    }
    else
    {
      url = '/api/application.feature.findOne.aspx';

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
                       + '<a href="javascript:main.applications.application.feature.list.save();" class="btn btn-default"><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
                       + '<a href="javascript:main.applications.application.feature.list.getPaging(' + main.applications.application.feature.list.paging.currentPage + ');" class="btn btn-default"><i class="fa fa-ban" title="关闭"></i> 关闭</a>'
                       + '</div>'
                       + '<span>应用功能配置</span>'
                       + '<div class="clear"></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.applications.application.feature.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" style="height:18px" />';

        // main.applications.application.feature.list.setObjectView(param);

        x.dom.features.bind();

        x.ui.pkg.tabs.newTabs();
      }
    });
  },

  ///*
  //* 设置对象的视图
  //*/
  //setObjectView: function(param)
  //{
  //    x.util.readonly('id');
  //    x.util.readonly('#applicationName');
  //    x.util.readonly('#parentName');
  //    x.util.readonly('#code');

  //    if(param.status == '1')
  //    {
  //        $('#status')[0].checked = 'checked';
  //    }
  //},

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

    if($('#parentId').val() == $('#id').val())
    {
      alert('【父级功能】不能是自己本身。');

      return false;
    }

    return true;
  },
  /*#endregion*/

  /*#region 函数:save()*/
  save: function()
  {
    // 1.数据检测
    if(main.applications.application.feature.list.checkObject())
    {
      // 2.发送数据
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<action><![CDATA[save]]></action>';
      outString += '<id><![CDATA[' + $('#id').val() + ']]></id>';
      outString += '<applicationId><![CDATA[' + $('#applicationId').val() + ']]></applicationId>';
      outString += '<parentId><![CDATA[' + $('#parentId').val() + ']]></parentId>';
      outString += '<code><![CDATA[' + $('#code').val() + ']]></code>';
      outString += '<name><![CDATA[' + $('#name').val() + ']]></name>';
      outString += '<displayName><![CDATA[' + $('#displayName').val() + ']]></displayName>';
      outString += '<type><![CDATA[' + $('#type').val() + ']]></type>';
      outString += '<orderId><![CDATA[' + $('#orderId').val() + ']]></orderId>';
      outString += '<status><![CDATA[' + ($('#status')[0].checked ? '1' : '0') + ']]></status>';
      outString += '<remark><![CDATA[' + $('#remark').val() + ']]></remark>';
      outString += '<updateDate><![CDATA[' + $('#updateDate').val() + ']]></updateDate>';
      outString += '<originalName><![CDATA[' + $('#originalName').val() + ']]></originalName>';
      outString += '</request>';

      x.net.xhr('/api/application.feature.save.aspx', outString, {
        popResultValue: 1,
        waitingMessage: i18n.net.waiting.saveTipText,
        callback: function(response)
        {
          var treeViewRootTreeNodeId = $('#searchApplicationId').val();

          main.applications.application.feature.list.getTreeView(treeViewRootTreeNodeId);

          main.applications.application.feature.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);

          main.applications.application.feature.list.getPaging(main.applications.application.feature.list.paging.currentPage);
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
      x.net.xhr('/api/application.feature.delete.aspx?id=' + id, {
        waitingMessage: i18n.net.waiting.deleteTipText,
        callback: function(response)
        {
          main.applications.application.feature.list.getPaging(main.applications.application.feature.list.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getTreeView(value)*/
  /**
   * 获取树形菜单
   */
  getTreeView: function(value)
  {
    // var url = '/services/X3Platform/Apps/Ajax.ApplicationFeatureWrapper.aspx';

    var treeViewId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
    var treeViewName = $('#searchApplicationName').val();
    var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
    var treeViewUrl = 'javascript:main.applications.application.feature.list.setTreeViewNode(\'{treeNodeId}\')';

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

    tree = x.ui.pkg.tree.newTreeView({ name: 'main.applications.application.feature.list.tree', ajaxMode: true });

    tree.add({
      id: "0",
      parentId: "-1",
      name: treeViewName,
      url: treeViewUrl.replace('{treeNodeId}', '[ApplicationId]' + treeViewRootTreeNodeId),
      title: treeViewName,
      target: '',
      icon: '/resources/images/tree/tree_icon.gif'
    });

    tree.load('/api/application.feature.getDynamicTreeView.aspx', false, outString);

    main.applications.application.feature.list.tree = tree;

    $('#treeViewContainer')[0].innerHTML = tree;
  },
  /*#endregion*/

  /*#region 函数:setTreeViewNode(value)*/
  setTreeViewNode: function(value)
  {
    // var whereClause = ' T.ParentId =  ##' + value + '## ';

    // if(value.indexOf('[ApplicationId]') == 0)
    //{
    //  whereClause = ' T.ApplicationId = ##' + value.replace('[ApplicationId]', '') + '## AND T.ParentId = ##00000000-0000-0000-0000-000000000000## ';
    //}

    // main.applications.application.feature.list.paging.whereClause = whereClause;

    main.applications.application.feature.list.paging.query.scence = 'QueryByParentId';

    if(value.indexOf('[ApplicationId]') == 0)
    {
      main.applications.application.feature.list.paging.query.where.ApplicationId = value.replace('[ApplicationId]', '');
      main.applications.application.feature.list.paging.query.where.ParentId = '00000000-0000-0000-0000-000000000000';
    }
    else
    {
      main.applications.application.feature.list.paging.query.where.ApplicationId = '';
      main.applications.application.feature.list.paging.query.where.ParentId = value;
    }

    main.applications.application.feature.list.getPaging(1);
  },
  /*#endregion*/

  /*#region 函数:resize()*/
  /**
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
    main.applications.application.feature.list.resize();

    var treeViewRootTreeNodeId = $('#searchApplicationId').val();

    main.applications.application.feature.list.getTreeView(treeViewRootTreeNodeId);

    main.applications.application.feature.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#btnApplicationWizard').on('click', function()
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

          main.applications.application.feature.list.getTreeView(treeViewRootTreeNodeId);

          main.applications.application.feature.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);
        }
      });
    });

    // $('#btnApplicationWizard').on('click', function()
    // {
    //    main.applications.application.feature.list.getApplicationWizard('search');
    // });

    $('#searchText').on('keyup', function()
    {
      main.applications.application.feature.list.filter();
    });

    $('#searchButton').on('click', function()
    {
      main.applications.application.feature.list.filter();
    });
  }
  /*#endregion*/
}

$(document).ready(main.applications.application.feature.list.load);
// 重新调整页面大小
$(window).resize(main.applications.application.feature.list.resize);
// 重新调整页面大小
$(document.body).resize(main.applications.application.feature.list.resize);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
  main.applications.application.menu.list.getPaging(1);
}