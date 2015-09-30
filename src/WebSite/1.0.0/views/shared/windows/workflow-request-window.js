x.register('x.ui.windows');

x.ui.windows.newWorkflowRequestWindow = function(name, options)
{
  return x.ext(x.ui.windows.newWindow(name, options), {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:bindOptions(options)*/
    bindOptions: function(options)
    {
      this.workflowTransactUrl = options.workflowTransactUrl;
      this.workflowViewUrl = options.workflowViewUrl;

      this.actorId = options.actorId;
      this.type = options.type;
      this.entityClassName = options.entityClassName;

      // 实体类的标识后缀，适用于自定义库等一个应用对用多套数据库的场景
      this.entityIdSuffix = (typeof (options.entityIdSuffix) === 'undefined') ? '' : options.entityIdSuffix;

      this.searchText = options.searchText;
      this.btnFilter = options.btnFilter;

      this.tableHeader = options.tableHeader;
      this.tableContainer = options.tableContainer;
      this.tableFooter = options.tableFooter;
    },
    /*#endregion*/

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
      this.paging.query.scence = 'QueryByWorkflowRequest';

      this.paging.query.where.SearchText = this.searchText ? this.searchText.val() : '';
      this.paging.query.where.WorkflowRequestType = this.type;
      this.paging.query.where.WorkflowEntityClassName = this.entityClassName;
      this.paging.query.where.WorkflowEntityIdSuffix = this.entityIdSuffix;

      // this.paging.whereClause = whereClauseValue;

      this.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:getObjectsView(me, list, maxCount)*/
    /*
    * 创建对象列表的视图
    */
    getObjectsView: function(me, list, maxCount)
    {
      var outString = '';

      var counter = 0;

      var classNameValue = '';

      outString += '<div class="table-freeze-head">';
      outString += '<table class="table" >';
      outString += '<thead>';
      outString += '<tr>';
      outString += '<th >名称</th>';
      outString += '<th style="width:100px" >发起人</th>';
      outString += '<th style="width:160px" >创建时间</th>';
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
      outString += '<col style="width:160px" />';
      outString += '</colgroup>';
      outString += '<tbody>';

      x.each(list, function(index, node)
      {
        outString += '<tr>';
        if(me.type == 'pending')
        {
          outString += '<td><a href="' + me.workflowTransactUrl + '?workflowInstanceId=' + node.workflowInstanceId + '&nodeId=' + node.workflowNodeId + '&historyNodeId=' + node.id + '" target="_blank">' + node.workflowInstanceTitle + '</a> ' + (node.receivedTimeView == '2000-01-01' ? '' : ('<span class="label label-default" >接收时间:' + x.date.create(node.receivedTime).toString() + '</span>')) + '</td>';
        }
        else
        {
          outString += '<td><a href="' + me.workflowViewUrl + '?workflowInstanceId=' + node.workflowInstanceId + '&nodeId=' + node.workflowNodeId + '&historyNodeId=' + node.id + '" target="_blank">' + node.workflowInstanceTitle + '</a> ' + (node.finishedTimeView == '2000-01-01' ? '' : ('<span class="label label-default" >完成时间:' + x.date.create(node.finishedTime).toString() + '</span>')) + '</td>';
        }
        outString += '<td>' + node.workflowInstanceStartActorName + '</td>';
        outString += '<td>' + x.date.create(node.createdTime).toString() + '</td>';
        outString += '</tr>';

        counter++;
      });

      // 补全

      while(counter < maxCount)
      {
        outString += '<tr><td colspan="3" >&nbsp;</td></tr>';

        counter++;
      }

      outString += '</tbody>';
      outString += '</table>';
      outString += '</div>';

      return outString;
    },
    /*#endregion*/

    /*#region 函数:getPaging(currentPage)*/
    getPaging: function(currentPage)
    {
      var paging = this.paging;

      paging.currentPage = currentPage;

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += paging.toXml();
      outString += '</request>';

      var me = this;

      x.net.xhr('/api/workflow.historyNode.query.aspx', outString, {
        waitingType: 'mini',
        waitingMessage: i18n.net.waiting.queryTipText,
        callback: function(response)
        {
          var result = x.toJSON(response);

          var paging = me.paging;

          var list = result.data;

          paging.load(result.paging);

          var containerHtml = me.getObjectsView(me, list, paging.pageSize);

          me.tableContainer.html(containerHtml);

          var footerHtml = paging.tryParseMenu('javascript:' + me.name + '.getPaging({0})');

          me.tableFooter.html(footerHtml);

          // 调整页面结构尺寸
          me.resize();
        }
      });
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

      $('#window-main-table-body .table-freeze-body').css(
      {
        'height': (height - freezeHeight - freezeTableHeadHeight) + 'px',
        'overflow-y': 'scroll'
      });

      $('.table-freeze-head-padding').css({ width: x.page.getScrollBarWidth().vertical, display: (x.page.getScrollBarWidth().vertical == 0 ? 'none' : '') });
    },
    /*#endregion*/

    /*#region 函数:create()*/
    /**
     * 页面加载事件
     */
    create: function()
    {
      this.filter();

      // -------------------------------------------------------
      // 绑定事件
      // -------------------------------------------------------

      var me = this;

      this.btnFilter.on('click', function()
      {
        me.filter();
      });

      // 调整页面结构尺寸
      me.resize();
      // 重新调整页面大小
      $(window).resize(me.resize);
      // 重新调整页面大小
      $(document.body).resize(me.resize);
    }
    /*#endregion*/
  });
};

x.ui.windows.getWorkflowRequestWindow = function(name, options)
{
  var name = x.getFriendlyName(location.pathname + '-window-' + name);

  // 绑定到Window对象
  var internalWindow = x.ui.windows.newWorkflowRequestWindow(name, options);

  // 加载界面、数据、事件
  internalWindow.load(options);

  // 绑定到Window对象
  window[name] = internalWindow;

  // [默认]私有回调函数, 供子窗口回调
  window.window$refresh$callback = function()
  {
    window[name].filter();
  };

  return internalWindow;
};