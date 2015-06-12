x.register('main.membership.group.tree.node.list');

main.membership.group.tree.node.list = {

    paging: x.page.newPagingHelper(),

    maskWrapper: x.ui.mask.newMaskWrapper('main.membership.group.tree.node.list.maskWrapper'),

    /*
    * 过滤
    */
    filter: function()
    {
        var whereClauseValue = '';

        var key = $('#searchText').val();

        if(key.trim() != '')
        {
            whereClauseValue = ' T.Name LIKE ##%' + key + '%## ';
        }

        main.membership.group.tree.node.list.paging.whereClause = whereClauseValue;

        main.membership.group.tree.list.paging.orderBy = ' OrderId ';

        main.membership.group.tree.node.list.getPaging(1);
    },

    /*#region 函数:getObjectView(param)*/
    /*
    * 创建对象列表的视图
    */
    getObjectsView: function(list, maxCount)
    {
        // 默认字段处理
        //param = x.ext({
        //    id: '',
        //    parentId: '',
        //    orderId: ''
        //}, param);

        x.each(list, function(index, node)
        {
            node.statusView = x.app.setColorStatusView(node.status);
        });

        x.template.config('escape', false);

        return x.template({ fileName: '#template-list', data: { list: list }});

        var counter = 0;

        var classNameValue = '';

        var outString = '';

        outString += '<table class="table-style" style="width:100%">';
        outString += '<tbody>';
        outString += '<tr class="table-row-title">';
        outString += '<td >名称</td>';
        outString += '<td style="width:100px">状态</td>';
        outString += '<td style="width:80px">更新日期</td>';
        outString += '<td style="width:40px">删除</td>';
        outString += '</tr>';

        x.each(list, function(index, node)
        {
            classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

            classNameValue = classNameValue + ((counter + 1) == maxCount ? '-transparent' : '');

            outString += '<tr class="' + classNameValue + '">';
            outString += '<td><a href="javascript:main.membership.group.tree.node.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
            outString += '<td>' + (node.status == '1' ? '<span class="green-text">启用</span>' : '<span class="red-text">禁用</span>') + '</td>';
            outString += '<td>' + node.updateDateView + '</td>';
            outString += '<td><a href="javascript:main.membership.group.tree.node.list.confirmDelete(\'' + node.id + '\');">删除</a></td>';
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            var classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

            classNameValue = classNameValue + ((counter + 1) == maxCount ? '-transparent' : '');

            outString += '<tr class="' + classNameValue + '">';
            outString += '<td colspan="4" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
            outString += '</tr>';

            counter++;
        }

        outString += '</tbody>';
        outString += '</table>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:getObjectView(param)*/
    /*
    * 创建单个对象的视图
    */
    getObjectView: function(param)
    {
        // 默认字段处理
        param = x.ext({
            id: '',
            parentId: '',
            orderId: ''
        }, param);

        return x.template({ fileName: '#template-form', data: { param: param } });
    },
    /*#endregion*/

    /*
    * 分页
    */
    getPaging: function(currentPage)
    {
        var paging = main.membership.group.tree.node.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<action><![CDATA[getPaging]]></action>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/membership.groupTreeNode.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.net.waiting.queryTipText,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.membership.group.tree.node.list.paging;

                var list = result.data;

                var counter = 0;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                               + '<a href="javascript:main.membership.group.tree.node.list.openDialog();" class="btn btn-default" >新增</a>'
                               + '</div>'
                               + '<span>' + $('#treeViewName').val() + '类别设置</span>'
                               + '<div class="clearfix" ></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.membership.group.tree.node.list.getObjectsView(list, paging.pagingize);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                var footerHtml = paging.tryParseMenu('javascript:main.membership.group.tree.node.list.getPaging({0});');

                $('#window-main-table-footer')[0].innerHTML = footerHtml;
            }
        });
    },
    /*#endregion*/

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
            url = '/api/membership.groupTreeNode.create.aspx';

            var treeNode = main.membership.group.tree.node.list.tree.getSelectedNode();

            outString += '<treeViewId><![CDATA[' + $('#treeViewId').val() + ']]></treeViewId>';

            if(treeNode != null)
            {
                outString += '<parentId><![CDATA[' + treeNode.id + ']]></parentId>';
            }
        }
        else
        {
            url = '/api/membership.groupTreeNode.findOne.aspx';

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
                               + '<a href="javascript:main.membership.group.tree.node.list.save();" class="btn btn-default" >保存</a> '
                               + '<a href="javascript:main.membership.group.tree.node.list.getPaging(' + main.membership.group.tree.node.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                               + '</div>'
                               + '<span>' + $('#treeViewName').val() + '类别设置</span>'
                               + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.membership.group.tree.node.list.getObjectView(param);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                x.dom.features.bind();

                x.ui.pkg.tabs.newTabs();
            }
        });
    },
    /*#endregion*/

    /*#region 函数:checkObject()*/
    /*
    * 检测对象的必填数据
    */
    checkObject: function()
    {
        if(x.dom.data.check()) { return false; }

        return true;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    /*
    * 保存对象
    */
    save: function()
    {
        if(main.membership.group.tree.node.list.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' });
            outString += '</request>';

            x.net.xhr('/api/membership.groupTreeNode.save.aspx', outString, {
                waitingMessage: i18n.net.waiting.saveTipText,
                callback: function(response)
                {
                    main.membership.group.tree.node.list.getPaging(main.membership.group.tree.node.list.paging.currentPage);
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
        if(confirm(i18n.msg.ARE_YOU_SURE_YOU_WANT_TO_DELETE))
        {
            x.net.xhr('/api/membership.groupTreeNode.delete.aspx?id=' + id, {
                waitingMessage: i18n.net.waiting.deleteTipText,
                callback: function(response)
                {
                    main.membership.group.tree.node.list.getPaging(main.membership.group.tree.node.list.paging.currentPage);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:getTreeView(treeViewId)*/
    /*
    * 获取树形菜单
    */
    getTreeView: function(treeViewId)
    {
        // var treeViewType = 'organization';
        // var treeViewId = '10000000-0000-0000-0000-000000000000';
        var treeViewName = $('#treeViewName').val();
        var treeViewRootTreeNodeId = $('#treeViewRootTreeNodeId').val(); // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.membership.group.tree.node.list.setTreeViewNode(\'{treeNodeId}\')';

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

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.group.tree.node.list.tree' });

        tree.setAjaxMode(true);

        tree.add({
            id: "0",
            parentId: "-1",
            name: treeViewName,
            url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
            title: treeViewName,
            target: '',
            icon: '/resources/images/tree/tree_icon.gif'
        });

        tree.load('/api/membership.groupTree.getDynamicTreeView.aspx', false, outString);

        main.membership.group.tree.node.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        main.membership.group.tree.node.list.paging.query.where.ParentId = value;
        main.membership.group.tree.node.list.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:resize()*/
    /*
    * 页面大小调整
    */
    resize: function()
    {
        var height = x.page.getViewHeight() - 39;

        $('#treeViewContainer').css({
            'height': (height - 115) + 'px',
            'width': '196px',
            'overflow': 'auto'
        });

        // window-main-table-body
        $('#window-main-table-body').css({
            'height': (height - 75) + 'px',
            'overflow': 'auto'
        });

        $('#window-main-table-container').css({
            'height': (height - 75) + 'px',
            'overflow': 'auto'
        });

        // 分页组件
        $('#window-main-table-footer').css({
            // 'width': (width - 20) + 'px',
            // 'position': 'absolute',
            // 'bottom': 0,
            'background-color': '#fff'
        });
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /*
    * 页面加载事件
    */
    load: function()
    {
        // 调整页面结构尺寸
        main.membership.group.tree.node.list.resize();

        // 正常加载
        var treeViewId = x.net.request.find('treeViewId');

        main.membership.group.tree.node.list.getTreeView(treeViewId);

        main.membership.group.tree.node.list.setTreeViewNode(treeViewId);

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#searchText').bind('keyup', function()
        {
            main.membership.group.tree.node.list.filter();
        });

        $('#btnFilter').bind('click', function()
        {
            main.membership.group.tree.node.list.filter();
        });
    }
    /*#endregion*/
}

$(document).ready(main.membership.group.tree.node.list.load);
// 重新调整页面大小
$(window).resize(main.membership.group.tree.node.list.resize);
// 重新调整页面大小
$(document.body).resize(main.membership.group.tree.node.list.resize);
