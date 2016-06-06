x.register('main.applications.home');

main.applications.home = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /**
     * 查询
     */
    filter: function()
    {
        main.applications.home.paging.query.scence = 'Query';
        main.applications.home.paging.query.where.SearchText = $('#searchText').val().trim();
        main.applications.home.paging.query.orders = ' OrderId ';
        main.applications.home.getPaging(1);
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
        outString += '<th style="width:80px">应用代码</th>';
        outString += '<th >应用名称(应用显示名称)</th>';
        outString += '<th style="width:40px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
        outString += '<th style="width:100px">修改日期</th>';
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
        outString += '<col />';
        outString += '<col style="width:40px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:30px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td>' + node.code + '</td>';
            outString += '<td><a href="/applications/application/form?id=' + node.id + '" target="_blank" >' + node.applicationName + '(' + node.applicationDisplayName + ')</a></td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            if(node.locking == 1)
            {
                outString += '<td><a href="javascript:main.applications.home.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
            }
            else
            {
                outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
            }
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            outString += '<tr>';
            outString += '<td colspan="8" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
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
        var paging = main.applications.home.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/application.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.applications.home.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerHtml = main.applications.home.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.applications.home.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                main.applications.home.resize();
            }
        });
    },
    /*#endregion*/

    /*#region 函数:confirmDelete(ids)*/
    /*
    * 删除对象
    */
    confirmDelete: function(ids)
    {
        if(confirm('确定删除?'))
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += '<action><![CDATA[delete]]></action>';
            outString += '<ids><![CDATA[' + ids + ']]></ids>';
            outString += '</request>';

            var options = {
                resultType: 'json',
                xml: outString
            };

            $.post(main.applications.home.url, options, main.applications.home.confirmDelete_callback);
        }
    },

    confirmDelete_callback: function(response)
    {

        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
            case 0:
                alert(result.value);
                main.applications.home.getPaging(1);
                break;

            case 1:
            case -1:
                alert(result.value);
                break;

            default:
                break;
        }
    },
    /*#endregion*/

    /*#region 函数:getTreeView(value)*/
    /*
    * 获取树形菜单
    */
    getTreeView: function(value)
    {
        var treeViewId = '00000000-0000-0000-0000-000000000001';
        var treeViewName = '应用管理';
        var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.applications.home.setTreeViewNode(\'{treeNodeId}\')';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[getDynamicTreeView]]></action>';
        outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
        outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
        outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
        outString += '<tree><![CDATA[{tree}]]></tree>';
        outString += '<parentId><![CDATA[{parentId}]]></parentId>';
        outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
        outString += '</ajaxStorage>';

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.applications.home.tree' });

        tree.setAjaxMode(true);

        // tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId), treeViewName, '', '/resources/images/tree/tree_icon.gif');

        tree.add({
            id: "0",
            parentId: "-1",
            name: treeViewName,
            url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
            title: treeViewName,
            target: '',
            icon: '/resources/images/tree/tree_icon.gif'
        });

        tree.load('/api/application.getDynamicTreeView.aspx', false, outString);

        main.applications.home.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        main.applications.home.paging.query.scence = 'QueryByParentId';
        main.applications.home.paging.query.where.ParentId = value;
        main.applications.home.paging.query.orders = ' OrderId ';

        main.applications.home.getPaging(1);
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
        main.applications.home.resize();

        var treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';

        main.applications.home.getTreeView(treeViewRootTreeNodeId);

        main.applications.home.setTreeViewNode(treeViewRootTreeNodeId);

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#searchText').bind('keyup', function()
        {
            main.applications.home.filter();
        });

        $('#btnFilter').bind('click', function()
        {
            main.applications.home.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.applications.home.load);
// 重新调整页面大小
$(window).resize(main.applications.home.resize);
// 重新调整页面大小
$(document.body).resize(main.applications.home.resize);

/**
 * [默认]私有回调函数, 供子窗口回调
 */
function window$refresh$callback()
{
    main.applications.home.getPaging(1);
}