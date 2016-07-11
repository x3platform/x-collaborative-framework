(function(x, window)
{
    var main = {

        paging: x.page.newPagingHelper(100),

        maskWrapper: x.ui.mask.newMaskWrapper('main.maskWrapper'),

        /*
        * 过滤
        */
        filter: function()
        {
            var whereClauseValue = '';

            var key = $('#searchText').val();

            if (key.trim() != '')
            {
                whereClauseValue = ' T.Name LIKE ##%' + key + '%## ';
            }

            main.paging.whereClause = whereClauseValue;

            main.membership.group.tree.list.paging.orderBy = ' OrderId ';

            main.getPaging(1);
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
                node.modifiedDateView = x.date.newTime(node.modifiedDateView).toString('yyyy-MM-dd');
            });

            x.template.config('escape', false);

            return x.template({ fileName: '#template-list', data: { list: list } });

            var counter = 0;

            var classNameValue = '';

            var outString = '';

            outString += '<div class="table-freeze-head">';
            outString += '<table class="table" >';
            outString += '<thead>';
            outString += '<tr>';
            outString += '<th >名称</th>';
            outString += '<th style="width:80px">状态</th>';
            outString += '<th style="width:100px">更新日期</th>';
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
            outString += '<col style="width:100px" />';
            outString += '<col style="width:30px" />';
            // outString += '<col style="width:30px" />';
            outString += '</colgroup>';
            outString += '<tbody>';

            x.each(list, function(index, node)
            {
                classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

                classNameValue = classNameValue + ((counter + 1) == maxCount ? '-transparent' : '');

                outString += '<tr class="' + classNameValue + '">';
                outString += '<td><a href="javascript:main.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
                outString += '<td>' + (node.status == '1' ? '<span class="green-text">启用</span>' : '<span class="red-text">禁用</span>') + '</td>';
                outString += '<td>' + x.date.newTime(node.modifiedDateView).toString('yyyy-MM-dd') + '</td>';
                outString += '<td><a href="javascript:main.confirmDelete(\'' + node.id + '\');">删除</a></td>';
                outString += '</tr>';

                counter++;
            });

            // 补全

            while (counter < maxCount)
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
            outString += '</div>';

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
            var paging = main.paging;

            paging.currentPage = currentPage;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += '<action><![CDATA[getPaging]]></action>';
            outString += paging.toXml();
            outString += '</request>';

            x.net.xhr('/api/membership.catalogItem.query.aspx', outString, {
                waitingType: 'mini',
                waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var paging = main.paging;

                    var list = result.data;

                    var counter = 0;

                    paging.load(result.paging);

                    var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                                   + '<a href="javascript:main.openDialog();" class="btn btn-default" >新增</a>'
                                   + '</div>'
                                   + '<span>' + $('#treeViewName').val() + '类别设置</span>'
                                   + '<div class="clearfix" ></div>';

                    $('#window-main-table-header')[0].innerHTML = headerHtml;

                    var containerHtml = main.getObjectsView(list, paging.pagingize);

                    $('#window-main-table-container')[0].innerHTML = containerHtml;

                    var footerHtml = paging.tryParseMenu('javascript:main.getPaging({0});');

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

            if (id === '')
            {
                url = '/api/membership.catalogItem.create.aspx';

                var treeNode = main.tree.getSelectedNode();

                outString += '<treeViewId><![CDATA[' + $('#treeViewId').val() + ']]></treeViewId>';

                if (treeNode != null)
                {
                    outString += '<parentId><![CDATA[' + treeNode.id + ']]></parentId>';
                }
            }
            else
            {
                url = '/api/membership.catalogItem.findOne.aspx';

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
                                   + '<a href="javascript:main.save();" class="btn btn-default" >保存</a> '
                                   + '<a href="javascript:main.getPaging(' + main.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                                   + '</div>'
                                   + '<span>' + $('#treeViewName').val() + '类别设置</span>'
                                   + '<div class="clear"></div>';

                    $('#window-main-table-header')[0].innerHTML = headerHtml;

                    var containerHtml = main.getObjectView(param);

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
            if (x.dom.data.check()) { return false; }

            return true;
        },
        /*#endregion*/

        /*#region 函数:save()*/
        /*
        * 保存对象
        */
        save: function()
        {
            if (main.checkObject())
            {
                var outString = '<?xml version="1.0" encoding="utf-8"?>';

                outString += '<request>';
                outString += x.dom.data.serialize({ storageType: 'xml' });
                outString += '</request>';

                x.net.xhr('/api/membership.catalogItem.save.aspx', outString, {
                    waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
                    callback: function(response)
                    {
                        main.getPaging(main.paging.currentPage);
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
            if (confirm(i18n.msg.ARE_YOU_SURE_YOU_WANT_TO_DELETE))
            {
                x.net.xhr('/api/membership.catalogItem.delete.aspx?id=' + id, {
                    waitingMessage: i18n.strings.msg_net_waiting_delete_tip_text,
                    callback: function(response)
                    {
                        main.getPaging(main.paging.currentPage);
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
            var treeViewUrl = 'javascript:main.setTreeViewNode(\'{treeNodeId}\')';

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

            var tree = x.ui.pkg.tree.newTreeView({ name: 'main.tree' });

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

            tree.load('/api/membership.catalog.getDynamicTreeView.aspx', false, outString);

            main.tree = tree;

            $('#treeViewContainer')[0].innerHTML = tree;
        },
        /*#endregion*/

        /*#region 函数:setTreeViewNode(value)*/
        setTreeViewNode: function(value)
        {
            main.paging.query.where.ParentId = value;
            main.getPaging(1);
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
            main.resize();

            // 正常加载
            var treeViewId = x.net.request.find('treeViewId');

            main.getTreeView(treeViewId);

            main.setTreeViewNode(treeViewId);

            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            $('#searchText').bind('keyup', function()
            {
                main.filter();
            });

            $('#btnFilter').bind('click', function()
            {
                main.filter();
            });
        }
        /*#endregion*/
    }

    // 注册到 Window 对象
    window.main = main;

    x.dom.ready(main.load);

}(x, window));