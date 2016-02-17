x.register('main.applications.application.option.list');

main.applications.application.option.list = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /*
    * 过滤
    */
    filter: function()
    {
        var whereClauseValue = '';

        var key = $('#searchText').val().trim();

        if(key !== '')
        {
            whereClauseValue = ' ( T.Name LIKE ##%' + x.toSafeLike(key) + '%## OR T.Value LIKE ##%' + x.toSafeLike(key) + '%## ) ';
        }

        main.applications.application.option.list.paging.whereClause = whereClauseValue;

        main.applications.application.option.list.paging.query.orders = ' Name ';

        main.applications.application.option.list.getPaging(1);
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

        outString += '<div class="table-freeze-head">';
        outString += '<table class="table" >';
        outString += '<thead>';
        outString += '<tr>';
        outString += '<th style="width:160px">名称</th>';
        outString += '<th >值</th>';
        outString += '<th style="width:50px">状态</th>';
        outString += '<th style="width:100px">修改日期</th>';
        outString += '<th style="width:50px">删除</th>';
        outString += '<th class="table-freeze-head-padding" ><a href="javascript:window$refresh$callback();"><small><span class="glyphicon glyphicon-refresh"></span></small></a></th>';
        outString += '</tr>';
        outString += '</thead>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="table-freeze-body">';
        outString += '<table class="table table-striped">';
        outString += '<colgroup>';
        outString += '<col style="width:160px" />';
        outString += '<col />';
        outString += '<col style="width:50px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:50px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td><a href="javascript:main.applications.application.option.list.openDialog(\'' + node.name + '\');">' + node.name + '</a></td>';
            outString += '<td style="word-break:break-all;" >' + node.value + '</td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            outString += '<td><a href="javascript:main.applications.application.option.list.confirmDelete(\'' + node.name + '\');">删除</a></td>';
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            outString += '<tr><td colspan="8" >&nbsp;</td></tr>';

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
        var outString = '';
        // 
        outString += '<table class="table-style" style="width:100%">';
        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >所属应用<span></td>';
        outString += '<td class="table-body-input form-inline">';
        if(param.isInternal == '1')
        {
            outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="" /> ';
            outString += '<span class="red-text" >系统内置选项</span>';
        }
        else
        {
            outString += '<div class="input-group"> ';
            outString += '<input id="applicationName" name="applicationName" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" style="width:381px;" x-dom-data-required-warning="必须填写【所属应用】。" value="' + x.isUndefined(param.applicationDisplayName, '') + '" ' + ((param.applicationDisplayName === '') ? '' : ' readonly="readonly" ') + '/> ';
            outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.applicationId, '') + '" /> ';
            if(param.applicationId === '')
            {
                outString += '<a href="javascript:x.ui.wizards.getApplicationWizard({\'targetValueName\':\'applicationId\',\'targetViewName\':\'applicationName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
            }
            outString += '</div> ';
        }
        outString += '<input id="isInternal" name="isInternal" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.isInternal, '0') + '" /> ';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >名称<span></td>';
        outString += '<td class="table-body-input"><input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" style="width:420px;" x-dom-data-required-warning="必须填写【名称】。" value="' + x.isUndefined(param.name, '') + '" ' + ((param.name === '') ? '' : ' readonly="readonly" ') + '/></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >值<span></td>';
        outString += '<td class="table-body-input"><textarea id="value" name="value" type="text" x-dom-data-type="value" class="textarea-normal" style="width:420px;height:60px;" >' + x.isUndefined(param.value, '') + '</textarea></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" >排序</td>';
        outString += '<td class="table-body-input" ><input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + x.isUndefined(param.orderId, '') + '" class="form-control x-ajax-input" style="width:120px;" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >备注</td>';
        outString += '<td class="table-body-input"><textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="textarea-normal" style="width:420px;height:40px;" >' + x.isUndefined(param.remark, '') + '</textarea></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input" ><input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.status, '1') + '" /></td>';
        outString += '</tr>';

        outString += '</table>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:getPaging(currentPage)*/
    /*
    * 分页
    */
    getPaging: function(currentPage)
    {
        var paging = main.applications.application.option.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/application.option.query.aspx', outString, {
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.applications.application.option.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.applications.application.option.list.openDialog();" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
                       + '</div>'
                       + '<span>应用选项设置</span>'
                       + '<div class="clearfix" ></div>';

                $('#window-main-table-header').html(headerHtml);

                var containerHtml = main.applications.application.option.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.applications.application.option.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                main.applications.application.option.list.resize();
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
        var name = (typeof (value) === 'undefined' || value === 'new' || value === '0') ? '' : value;

        var url = '';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';

        if(name === '')
        {
            url = '/api/application.option.create.aspx';

            // outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';
        }
        else
        {
            url = '/api/application.option.findOne.aspx';

            outString += '<name><![CDATA[' + name + ']]></name>';
        }
        outString += '</request>';

        x.net.xhr(url, outString, {
            callback: function(response)
            {
                var param = x.toJSON(response).data;

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.applications.application.option.list.save();" class="btn btn-default"><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
                       + '<a href="javascript:main.applications.application.option.list.getPaging(' + main.applications.application.option.list.paging.currentPage + ');" class="btn btn-default"><i class="fa fa-ban" title="关闭"></i> 关闭</a>'
                       + '</div>'
                       + '<span>应用选项设置</span>'
                       + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.applications.application.option.list.getObjectView(param);

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
        if(x.dom.data.check())
        {
            return false;
        }

        return true;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
        if(main.applications.application.option.list.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' })
            outString += '</request>';

            x.net.xhr('/api/application.option.save.aspx', outString, {
                waitingMessage: i18n.net.waiting.saveTipText,
                popCorrectValue: 1,
                callback: function(response)
                {
                    main.applications.application.option.list.getPaging(main.applications.application.option.list.paging.currentPage);
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
            x.net.xhr('/api/application.option.delete.aspx?id=' + id, {
                waitingMessage: i18n.net.waiting.deleteTipText,
                callback: function(response)
                {
                    main.applications.application.option.list.getPaging(main.applications.application.option.list.paging.currentPage);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:getTreeView(value)*/
    /*
    * 获取树形菜单
    */
    getTreeView: function()
    {
        var treeViewId = '00000000-0000-0000-0000-000000000001';
        var treeViewName = '应用管理';
        var treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001'; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.applications.application.option.list.setTreeViewNode(\'{treeNodeId}\')';

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

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.applications.application.option.list.tree' });

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

        tree.load('/api/application.getDynamicTreeView.aspx', false, outString);

        main.applications.application.option.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        // var whereClause = (value === '') ? '' : ' T.ApplicationId =  ##' + value + '## ';

        main.applications.application.option.list.paging.query.where.ApplicationId = value;

        main.applications.application.option.list.paging.query.orders = ' Name ';

        main.applications.application.option.list.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:resize()*/
    /*
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
    /*
    * 页面加载事件
    */
    load: function()
    {
        // 调整页面结构尺寸
        main.applications.application.option.list.resize();

        main.applications.application.option.list.getTreeView();

        main.applications.application.option.list.setTreeViewNode('');

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#searchText').bind('keyup', function()
        {
            main.applications.application.option.list.filter();
        });

        $('#btnFilter').bind('click', function()
        {
            main.applications.application.option.list.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.applications.application.option.list.load);
// 重新调整页面大小
$(window).resize(main.applications.application.option.list.resize);
// 重新调整页面大小
$(document.body).resize(main.applications.application.option.list.resize);
