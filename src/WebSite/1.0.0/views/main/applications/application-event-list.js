x.register('main.applications.application.event.list');

main.applications.application.event.list = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /**
     * 过滤
     */
    filter: function()
    {
        main.applications.application.event.list.paging.query.where.Description = $('#searchText').val().trim();

        main.applications.application.event.list.getPaging(1);
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

        var classNameValue = '';

        outString += '<div class="table-freeze-head">';
        outString += '<table class="table" >';
        outString += '<thead>';
        outString += '<tr>';
        outString += '<th >事件</td>';
        outString += '<th style="width:200px" >时间</th>';
        outString += '<th class="table-freeze-head-padding" ></th>';
        outString += '</tr>';
        outString += '</thead>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="table-freeze-body">';
        outString += '<table class="table table-striped">';
        outString += '<colgroup>';
        outString += '<col />';
        outString += '<col style="width:200px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr">';
            outString += '<td>' + node.description + ' ' + node.tags + ' <span class="label label-default" >' + node.timeSpan + ' 秒</span></td>';
            outString += '<td>' + x.date.newTime(node.date).toString('yyyy-MM-dd HH:mm:ss.fff') + '</td>';
            outString += '</tr>';

            counter++;
        });

        // 补全

        while (counter < maxCount)
        {
            outString += '<tr ><td colspan="2" >&nbsp;</td></tr>';

            counter++;
        }

        outString += '</tbody>';
        outString += '</table>';
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
        var paging = main.applications.application.event.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/application.event.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.applications.application.event.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerHtml = main.applications.application.event.list.getObjectsView(list, paging.pagingize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.applications.application.event.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                masterpage.resize();
            }
        });
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /**
     * 页面加载事件
     */
    load: function()
    {
        main.applications.application.event.list.filter();

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnFilter').bind('click', function()
        {
            main.applications.application.event.list.filter();
        });
    }
    /*#endregion*/
}

$(document).ready(main.applications.application.event.list.load);