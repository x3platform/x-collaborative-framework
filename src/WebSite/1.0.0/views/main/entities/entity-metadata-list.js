x.register('main.entities.entity.metadata.list');

main.entities.entity.metadata.list = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
        //var whereClauseValue = ' Status != 2';

        //var searchText = $('#searchText').val();

        //if(searchText !== '')
        //{
        //  whereClauseValue += '  AND CategoryIndex LIKE ##%' + searchText + '%##  ';
        //}

        main.entities.entity.metadata.list.paging.query.scence = 'Query';
        main.entities.entity.metadata.list.paging.query.where.SearchText = $('#searchText').val();
        main.entities.entity.metadata.list.paging.query.orders = ' OrderId, ModifiedDate DESC';
        main.entities.entity.metadata.list.getPaging(1);
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
        outString += '<th >字段名称</th>';
        outString += '<th style="width:80px;" >字段类型</th>';
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
        outString += '<col />';
        outString += '<col style="width:80px" />';
        outString += '<col style="width:30px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:30px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td><a href="/entities/entity-metadata/form?id=' + node.id + '" target="_blank">' + node.fieldName + '</a></td>';
            outString += '<td>' + node.fieldType + '</td>';
            outString += '<td class="text-center" >' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            if(node.locking === '1')
            {
                outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
            }
            else
            {
                outString += '<td><a href="javascript:main.entities.entity.metadata.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
            }
            outString += '</tr>';

            counter++;
        });

        // 补全
        while(counter < maxCount)
        {
            outString += '<tr><td colspan="5" >&nbsp;</td></tr>';

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
        var paging = main.entities.entity.metadata.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/kernel.entities.metadata.query.aspx', outString, {
            popCorrectValue: 0,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.entities.entity.metadata.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerHtml = main.entities.entity.metadata.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.entities.entity.metadata.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                masterpage.resize();
            }
        });
    },
    /*#endregion*/

    /*#region 函数:confirmDelete(ids)*/
    confirmDelete: function(ids)
    {
        if(confirm('确定删除?'))
        {
            x.net.xhr('/api/kernel.entities.metadata.delete.aspx?ids=' + ids, {
                popCorrectValue: 1,
                callback: function(response)
                {
                    main.entities.entity.metadata.list.getPaging(main.entities.entity.metadata.list.paging.currentPage);
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
        var treeViewId = '00000000-0000-0000-0000-000000000001';
        var treeViewName = '实体架构';
        var treeViewRootTreeNodeId = 0; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.entities.entity.metadata.list.setTreeViewNode(\'{treeNodeId}\')';

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

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.entities.entity.metadata.list.tree', ajaxMode: true });

        tree.add({
            id: "0",
            parentId: "-1",
            name: treeViewName,
            url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
            title: treeViewName,
            target: '',
            icon: '/resources/images/tree/tree_icon.gif'
        });

        //var tree = x.ui.tree.newTreeView('main.entities.entity.metadata.list.tree');

        //tree.setAjaxMode(true);

        //tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', ''), treeViewName, '', '/resources/images/tree/tree_icon.gif');

        tree.load('/api/kernel.entities.schema.getDynamicTreeView.aspx', false, outString);

        main.entities.entity.metadata.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        // var whereClause = ' T.EntitySchemaId =  ##' + value + '## ';

        main.entities.entity.metadata.list.paging.query.where.EntitySchemaId = value;

        main.entities.entity.metadata.list.paging.query.orders = ' OrderId ';

        main.entities.entity.metadata.list.getPaging(1);

        // 设置新增按钮 
        var toolbar = '';

        if(value !== '')
        {
            toolbar = '<a href="/entities/entity-metadata/form?entitySchemaId=' + value + '" target="_blank" class="btn btn-default">新增</a>';
        }

        $('#toolbar').html(toolbar);
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /*
    * 页面加载事件
    */
    load: function()
    {
        main.entities.entity.metadata.list.getTreeView();

        main.entities.entity.metadata.list.setTreeViewNode('');

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#searchText').on('keyup', function()
        {
            main.entities.entity.metadata.list.filter();
        });

        $('#btnFilter').on('click', function()
        {
            main.entities.entity.metadata.list.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.entities.entity.metadata.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.entities.entity.metadata.list.getPaging(1);
}
