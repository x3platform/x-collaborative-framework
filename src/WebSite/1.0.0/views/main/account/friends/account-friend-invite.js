(function(x, window)
{
    var main = {

        paging: x.page.newPagingHelper(100),

        /*#region 函数:filter()*/
        /*
        * 查询
        */
        filter: function()
        {
            main.paging.query.where.SearchText = $('#searchText').val();

            main.paging.query.orders = 'OrderId, ModifiedDate';

            main.findFriends(1);
        },
        /*#endregion*/

        /*#region 函数:getObjectsView(list, maxCount)*/
        /*
        * 创建对象列表的视图
        */
        getObjectsView: function(list, maxCount)
        {
            var outString = '';

            // 处理数据
            x.each(list, function(index, node)
            {
                outString += '<div class="clearfix">';
                outString += '<div class="pull-right" ><a href="/account/friends/detail/' + node.id + '" >详情</a></div>';
                outString += '<div class="pull-left" >' + node.displayName + '</div>';
                outString += '</div>';
            });

            return outString;
        },
        /*#endregion*/

        /*#region 函数:getPages(currentPage)*/
        /*
        * 分页
        */
        findFriends: function(currentPage)
        {
            var paging = main.paging;

            paging.currentPage = currentPage;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += paging.toXml();
            outString += '</request>';

            // membership.account.query
            // maimai.friends.query
            x.net.xhr('/api/membership.account.findFriends.aspx', outString, {
                waitingType: 'mini',
                waitingMessage: '正在查询数据，请稍后......',
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var paging = main.paging;

                    var list = result.data;

                    paging.load(result.paging);

                    var containerHtml = main.getObjectsView(list, paging.pageSize);

                    $('#window-main-table-container').html(containerHtml);
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
            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            $('#btnFilter').on('click', function()
            {
                main.filter();
            });
        }
        /*#endregion*/
    }


    // 注册到 Window 对象
    window.main = main;

    x.dom.ready(main.load);

}(x, document));