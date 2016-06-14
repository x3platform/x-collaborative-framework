x.register('main.sys.authorities');

main.sys.authorities = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /**
   * 过滤
   */
  filter: function()
  {
    main.sys.authorities.paging.query.scence = 'Query';
    main.sys.authorities.paging.query.where.Name = $('#searchText').val().trim();

    main.sys.authorities.paging.query.orders = ' OrderId, UpdateDate DESC';

    main.sys.authorities.getPaging(1);
  },
  /*#endregion*/

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
    outString += '<th >名称</th>';
    outString += '<th style="width:400px">值</th>';
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
    outString += '<col style="width:400px" />';
    outString += '<col style="width:30px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr title="' + node.description + '" >';
      outString += '<td><a href="javascript:main.sys.authorities.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
      outString += '<td>' + node.id + '</td>';
      outString += '<td><a href="javascript:main.sys.authorities.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
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

  /*
   * 创建单个对象的视图
   */
  getObjectView: function(param)
  {
    var outString = '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal">';
    outString += '<td class="table-body-text" style="width:120px" >键</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + param.id + '" />';
    outString += '<input id="key" name="key" type="text" x-dom-data-type="value" value="' + param.id + '" x-dom-data-required="1" x-dom-data-required-warning="必须填写【键】。" readonly="readonly" class="form-control" style="width:400px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal">';
    outString += '<td class="table-body-text" >名称</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="name" name="name" type="text" x-dom-data-type="value" value="' + param.name + '" x-dom-data-required="1" x-dom-data-required-warning="必须填写【名称】。" class="form-control" style="width:400px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal">';
    outString += '<td class="table-body-text" >描述</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="description" name="description" type="text" x-dom-data-type="value" class="form-control" style="width:400px; height:80px;" >' + param.description + '</textarea>';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal">';
    outString += '<td class="table-body-text" >标签</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="tags" name="tags" type="text" x-dom-data-type="value" value="' + param.tags + '" class="form-control" style="width:400px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal">';
    outString += '<td class="table-body-text" >排序</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + param.orderId + '" class="form-control" style="width:400px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '</table>';

    return outString;
  },

  /*#region 函数:getPaging(currentPage)*/
  /*
   * 分页
   */
  getPaging: function(currentPage)
  {
    var paging = main.sys.authorities.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getPaging]]></action>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/kernel.security.authority.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.sys.authorities.paging;

        var list = result.data;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
            + '<a href="javascript:main.sys.authorities.openDialog();" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
            + '</div>'
            + '<span>权限参数设置</span>'
            + '<div class="clearfix" ></div>';

        $('#window-main-table-header').html(headerHtml);

        var containerHtml = main.sys.authorities.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.sys.authorities.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);


        // 设置显示/隐藏
        $('.table-row-filter').css({ display: '' });
        $('#window-main-table-footer').css({ display: '' });

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
      url = '/api/kernel.security.authority.create.aspx';

      isNewObject = true;
    }
    else
    {
      url = '/api/kernel.security.authority.findOne.aspx';

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
                       + '<a href="javascript:main.sys.authorities.save();" class="btn btn-default" ><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
                       + '<a href="javascript:main.sys.authorities.getPaging(' + main.sys.authorities.paging.currentPage + ');" class="btn btn-default" ><i class="fa fa-ban" title="取消"></i> 取消</a>'
                       + '</div>'
                       + '<span>权限参数设置</span>'
                       + '<div class="clearfix" ></div>';

        $('#window-main-table-header').html(headerHtml);

        var containerHtml = main.sys.authorities.getObjectView(param);

        $('#window-main-table-container').html(containerHtml);

        $('#window-main-table-footer').hide();

        // 设置显示/隐藏
        $('.table-row-filter').css({ display: 'none' });
        $('#window-main-table-footer').css({ display: 'none' });

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:save()*/
  /*
   * 保存对象
   */
  save: function()
  {
    if(!x.dom.data.check())
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
      outString += '</request>';

      x.net.xhr('/api/kernel.security.authority.save.aspx', outString, {
        callback: function(response)
        {
          main.sys.authorities.getPaging(main.sys.authorities.paging.currentPage);
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
    if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
    {
      x.net.xhr('/api/kernel.security.authority.delete.aspx?id=' + id, {
        callback: function(response)
        {
          main.sys.authorities.getPaging(main.sys.authorities.paging.currentPage);
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
   * 页面加载事件
   */
  load: function()
  {
    main.sys.authorities.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').on('keyup', function()
    {
      main.sys.authorities.filter();
    });

    $('#btnFilter').on('click', function()
    {
      main.sys.authorities.filter();
    });
  }
  /*#endregion*/
}

$(document).ready(main.sys.authorities.load);
