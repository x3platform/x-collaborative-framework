(function(x, document)
{
    var main = {

        paging: x.page.newPagingHelper(50),

        /*#region 函数:filter()*/
        /**
         * 查询
         */
        filter: function()
        {
            main.paging.query.scence = 'Query';
            main.paging.query.where.SearchText = $('#searchText').val().trim();

            main.getPaging(1);
        },
        /*#endregion*/

        /*#region 函数:getObjectsView(list, maxCount)*/
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
            outString += '<th >名称</th>';
            outString += '<th style="width:40px">' + i18n.strings.btn_delete + '</th>';
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
            outString += '</colgroup>';
            outString += '<tbody>';

            x.each(list, function(index, node)
            {
                outString += '<tr>';
                outString += '<td><a href="/account/friends/detail/' + node.friendAccountId + '" >' + node.friendDisplayName + '</a> ' + (node.status == 0 ? '<span class="label label-default">等待确认</span>' : '') + '</td>';
                outString += '<td><a href="javascript:main.confirmDelete(\'' + node.friendAccountId + '\');">' + i18n.strings.btn_delete + '</a></td>';
                outString += '</tr>';

                counter++;
            });

            // 补全

            while(counter < maxCount)
            {
                outString += '<tr>';
                outString += '<td colspan="4" >&nbsp;</td>';
                outString += '</tr>';

                counter++;
            }

            outString += '</tbody>';
            outString += '</table>';
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
            var paging = main.paging;

            paging.currentPage = currentPage;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += paging.toXml();
            outString += '</request>';

            x.net.xhr('/api/membership.accountFriend.queryMyList.aspx', outString, {
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

                    var footerHtml = paging.tryParseMenu('javascript:main.getPaging({0});');

                    $('#window-main-table-footer').html(footerHtml);

                    masterpage.resize();
                }
            });
        },
        /*#endregion*/

        /*#region 函数:confirmDelete(friendAccountId)*/
        /**
         * 删除对象
         */
        confirmDelete: function(friendAccountId)
        {
            if(confirm('确定删除?'))
            {
                x.net.xhr('/api/membership.accountFriend.unfriend.aspx?friendAccountId=' + friendAccountId, {
                    callback: function(response)
                    {
                        main.getPaging(main.paging.currentPage);
                    }
                });
            }
        },
        /*#endregion*/

        /*#region 函数:load()*/
        /**
         * 页面加载事件
         */
        load: function()
        {
            main.filter();

            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            $('#searchText').on('keyup', function()
            {
                main.filter();
            });

            $('#btnFilter').on('click', function()
            {
                main.filter();
            });

            $('#btnAccept').on('click', function()
            {
                location.href = "/account/friends/accept";
            });

            $('#btnInvite').on('click', function()
            {
                location.href = "/account/friends/invite";
            });
        }
        /*#endregion*/
    }

    // 注册到 Window 对象
    window.main = main;

    x.dom.ready(main.load);

}(x, document));
