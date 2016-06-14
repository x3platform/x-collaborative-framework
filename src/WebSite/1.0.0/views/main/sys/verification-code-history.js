x.register('main.sys.verification.code.history');

main.sys.verification.code.history = {

  paging: x.page.newPagingHelper(50),

  /*#region 函数:filter()*/
  /**
   * 过滤
   */
  filter: function()
  {
    main.sys.verification.code.history.paging.query.scence = 'Query';
    main.sys.verification.code.history.paging.query.where.ObjectValue = $('#searchText').val().trim();

    main.sys.verification.code.history.getPaging(1);
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
    outString += '<th style="width:300px" >唯一标识</th>';
    outString += '<th style="width:120px" >对象类型</th>';
    outString += '<th >对象值</th>';
    outString += '<th style="width:160px" >验证码</th>';
    outString += '<th style="width:100px" >验证类型</th>';
    outString += '<th style="width:200px" >创建时间</th>';
    outString += '<th class="table-freeze-head-padding" ></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col style="width:300px" />';
    outString += '<col style="width:120px" />';
    outString += '<col />';
    outString += '<col style="width:160px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:200px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      outString += '<tr>';
      outString += '<td>' + node.id + '</td>';
      outString += '<td>' + node.objectType + '</td>';
      outString += '<td>' + node.objectValue + '</td>';
      outString += '<td>' + node.code + '</td>';
      outString += '<td>' + node.validationType + '</td>';
      outString += '<td>' + node.createdDateTimestampView + '</td>';
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

  /*#region 函数:getPaging(currentPage)*/
  /*
   * 分页
   */
  getPaging: function(currentPage)
  {
    var paging = main.sys.verification.code.history.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getPaging]]></action>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/kernel.security.verificationCode.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.sys.verification.code.history.paging;

        var list = result.data;

        paging.load(result.paging);

        var containerHtml = main.sys.verification.code.history.getObjectsView(list, paging.pageSize);

        $('#window-main-table-container').html(containerHtml);

        var footerHtml = paging.tryParseMenu('javascript:main.sys.verification.code.history.getPaging({0});');

        $('#window-main-table-footer').html(footerHtml);


        // 设置显示/隐藏
        $('.table-row-filter').css({ display: '' });
        $('#window-main-table-footer').css({ display: '' });

        masterpage.resize();
      }
    });
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
   * 页面加载事件
   */
  load: function()
  {
    main.sys.verification.code.history.filter();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    $('#searchText').on('keyup', function()
    {
      main.sys.verification.code.history.filter();
    });

    $('#btnFilter').on('click', function()
    {
      main.sys.verification.code.history.filter();
    });
  }
  /*#endregion*/
}

$(document).ready(main.sys.verification.code.history.load);
