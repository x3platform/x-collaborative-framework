x.register('main.entities.entity.schema.list');

main.entities.entity.schema.list = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
        var whereClauseValue = ' 1 = 1 ';

        if($('#searchText').val() !== '')
        {
            whereClauseValue += ' AND ( Code LIKE ##%' + $('#searchText').val() + '%## OR Name LIKE ##%' + $('#searchText').val() + '%## OR EntityClassName LIKE ##%' + $('#searchText').val() + '%## )';
        }

        // 移除 1 = 1
        if(whereClauseValue.indexOf(' 1 = 1  AND ') > -1)
        {
            whereClauseValue = whereClauseValue.replace(' 1 = 1  AND ', '');
        }

        main.entities.entity.schema.list.paging.whereClause = whereClauseValue;

        main.entities.entity.schema.list.paging.orderBy = ' T.Code , T.ModifiedDate DESC ';

        main.entities.entity.schema.list.getPaging(1);
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

        var columnCount = 6;

        outString += '<div class="table-freeze-head">';
        outString += '<table class="table" >';
        outString += '<thead>';
        outString += '<tr>';
        outString += '<th style="width:80px;" >编号</th>';
        outString += '<th style="width:120px;" >名称</th>';
        outString += '<th >实体类名称</th>';
        outString += '<th style="width:30px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
        outString += '<th style="width:100px">更新时间</th>';
        outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
        outString += '<th class="table-freeze-head-padding" ></th>';
        outString += '</tr>';
        outString += '</thead>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="table-freeze-body">';
        outString += '<table class="table table-striped">';
        outString += '<colgroup>';
        outString += '<col style="width:80px" />';
        outString += '<col style="width:120px" />';
        outString += '<col />';
        outString += '<col style="width:30px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:30px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td>' + node.code + '</td>';
            outString += '<td><a href="/entities/entity-schema/form?id=' + node.id + '" target="_blank">' + node.name + '</a></td>';
            outString += '<td>' + node.entityClassName + '</td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            if(node.locking === '1')
            {
                outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
            }
            else
            {
                outString += '<td><a href="javascript:main.entities.entity.schema.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
            }
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            outString += '<tr><td colspan="' + columnCount + '" >&nbsp;</td></tr>';

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
        var paging = main.entities.entity.schema.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/kernel.entities.schema.query.aspx', outString, {
            popCorrectValue: 0,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.entities.entity.schema.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerHtml = main.entities.entity.schema.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.entities.entity.schema.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                masterpage.resize();
            }
        });
    },
    /*#endregion*/

    /*#region 函数:confirmDelete(id)*/
    confirmDelete: function(id)
    {
        if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
        {
            x.net.xhr('/api/kernel.entities.schema.delete.aspx?id=' + id, {
                popCorrectValue: 1,
                callback: function(response)
                {
                    main.entities.entity.schema.list.getPaging(main.entities.entity.schema.list.paging.currentPage);
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
        main.entities.entity.schema.list.filter();

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $("#btnFilter").bind('click', function()
        {
            main.entities.entity.schema.list.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.entities.entity.schema.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.entities.entity.schema.list.getPaging(main.entities.entity.schema.list.paging.currentPage);
}
