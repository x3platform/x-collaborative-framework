x.register('main.applications.application.method.list');

main.applications.application.method.list = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /*
    * 过滤
    */
    filter: function()
    {
        main.applications.application.method.list.paging.query.scence = 'Query';
        main.applications.application.method.list.paging.query.where.SearchText = $('#searchText').val().trim();
        main.applications.application.method.list.paging.query.orders = ' OrderId, Code ';

        main.applications.application.method.list.getPaging(1);
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
        outString += '<th style="width:160px">方法代码</th>';
        outString += '<th >名称</th>';
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
        outString += '<col style="width:160px" />';
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
            outString += '<td><a href="javascript:main.applications.application.method.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            outString += '<td><a href="javascript:main.applications.application.method.list.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');" title="删除"><i class="fa fa-trash" ></i></a></td>';
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

    /*#region 函数:getObjectView(param)*/
    /*
    * 创建单个对象的视图
    */
    getObjectView: function(param)
    {
        var outString = '';

        outString += '<div class="x-ui-pkg-tabs-wrapper">';
        outString += '<div class="x-ui-pkg-tabs-menu-wrapper" >';
        outString += '<ul class="x-ui-pkg-tabs-menu nav nav-tabs" >';
        outString += '<li><a href="#tab-1">基本信息</a></li>';
        outString += '<li><a href="#tab-2">详细文档</a></li>';
        outString += '<li><a href="#tab-3">备注</a></li>';
        outString += '</ul>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden" ><a id="tab-1" name="tab-1" >基本信息</a></h2>';

        outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.id, '') + '" />';
        outString += '<input id="type" name="type" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.type, 'generic') + '" />';
        outString += '<input id="version" name="version" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.version, '1') + '" />';
        outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.modifiedDateTimestampView, '') + '" />';
        outString += '<input id="originalCode" name="originalCode" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.code, '') + '" />';
        outString += '<input id="originalName" name="version" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.name, '') + '" />';

        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >代码<span></td>';
        outString += '<td class="table-body-input" style="width:200px" ><input id="code" name="code" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" style="width:160px;" x-dom-data-required-warning="必须填写【代码】。" value="' + x.isUndefined(param.code, '') + '" /></td>';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >所属应用名称<span></td>';
        outString += '<td class="table-body-input form-inline">';
        outString += '<div class="input-group">';
        outString += '<input id="applicationName" name="applicationName" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" style="width:120px;" x-dom-data-required-warning="必须填写【所属应用名称】。" value="' + x.isUndefined(param.applicationDisplayName, '') + '" /> ';
        outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.applicationId, '') + '" /> ';
        outString += '<a href="javascript:x.ui.wizards.getApplicationWizard({\'targetValueName\':\'applicationId\',\'targetViewName\':\'applicationName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >名称<span></td>';
        outString += '<td class="table-body-input" colspan="3" ><input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【名称】。" class="form-control" style="width:460px;" value="' + x.isUndefined(param.name, '') + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >显示名称<span></td>';
        outString += '<td class="table-body-input" colspan="3" ><input id="displayName" name="displayName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【显示名称】。" class="form-control" style="width:460px;" value="' + x.isUndefined(param.displayName, '') + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >选项<span></td>';
        outString += '<td class="table-body-input" colspan="3" ><textarea id="options" name="options" rows="4" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【选项】。" class="form-control" style="width:460px;" />' + x.isUndefined(param.options, '') + '</textarea></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >描述</td>';
        outString += '<td class="table-body-input" colspan="3" ><textarea id="description" name="description" rows="3" x-dom-data-type="value" class="form-control" style="width:460px;" />' + x.isUndefined(param.description, '') + '</textarea></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >排序</td>';
        outString += '<td class="table-body-input"><input id="orderId" name="orderId" type="text" x-dom-data-type="value" class="form-control" style="width:120px;" value="' + x.isUndefined(param.orderId, '') + '" /></td>';
        outString += '<td class="table-body-text" >作用范围</td>';
        outString += '<td class="table-body-input"><input id="effectScope" name="effectScope" type="text" x-dom-data-type="value" class="form-control" style="width:120px;" value="' + x.isUndefined(param.effectScope, '1') + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input" colspan="3" ><input id="status" name="status" type="checkbox" x-dom-feature="checkbox" x-dom-data-type="value" value="' + x.isUndefined(param.status, '0') + '" /></td>';
        outString += '</tr>';

        outString += '</table>';

        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">详细文档</a></h2>';
        outString += '<div style="margin:20px;width:600px;" >';
        outString += '<textarea id="detail" name="detail" type="text" x-dom-data-type="value" class="textarea-normal" style="display:none;" >' + x.isUndefined(param.detail, '') + '</textarea>';
        outString += '<div id="detail-editor" style="position:absolute; border:1px solid #ddd; border-radius:4px; width:815px; height:400px; font-size:14px;"></div>';
        outString += '</div>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-3" id="tab-3">备注</a></h2>';

        outString += '<table class="table-style" style="width:100%">';
        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" >备注</td>';
        outString += '<td class="table-body-input" ><textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="textarea-normal" style="width:460px;height:80px;" >' + x.isUndefined(param.remark, '') + '</textarea></td>';
        outString += '</tr>';
        outString += '</table>';
        outString += '</div>';

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
        var paging = main.applications.application.method.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/application.method.query.aspx', outString, {
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.applications.application.method.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.applications.application.method.list.openDialog();" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
                       + '</div>'
                       + '<span>应用方法管理</span>'
                       + '<div class="clearfix" ></div>';

                $('#window-main-table-header').html(headerHtml);

                var containerHtml = main.applications.application.method.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.applications.application.method.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                main.applications.application.method.list.resize();
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
        var id = (typeof (value) === 'undefined' || value === 'new' || value === '0') ? '' : value;

        var url = '';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';

        if(id === '')
        {
            url = '/api/application.method.create.aspx';

            outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';
        }
        else
        {
            url = '/api/application.method.findOne.aspx';

            outString += '<id><![CDATA[' + id + ']]></id>';
        }

        outString += '</request>';

        x.net.xhr(url, outString, {
            callback: function(response)
            {
                var param = x.toJSON(response).data;

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.applications.application.method.list.save();" class="btn btn-default"><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
                       + '<a href="javascript:main.applications.application.method.list.getPaging(' + main.applications.application.method.list.paging.currentPage + ');" class="btn btn-default"><i class="fa fa-ban" title="关闭"></i> 关闭</a>'
                       + '</div>'
                       + '<span>应用方法设置</span>'
                       + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.applications.application.method.list.getObjectView(param);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" style="height:18px" />';

                x.dom.features.bind();

                x.ui.pkg.tabs.newTabs();

                // -------------------------------------------------------
                // 初始化代码编辑器
                // -------------------------------------------------------

                $('#detail-editor').width($('#window-main-table-container').width() - 40);
                $('#detail-editor').height($('#window-main-table-container').height() - 80);

                var textarea = $('textarea[id="detail"]');

                var editor = ace.edit("detail-editor");
                // 设置主题 
                editor.setTheme("ace/theme/github");
                // 设置程序语言模式
                editor.getSession().setMode("ace/mode/html");
                // 设置制表符的大小 
                editor.getSession().setTabSize(2);
                // 设置打印边距是否可见
                editor.setShowPrintMargin(false);
                // 设置内容
                editor.getSession().setValue(html_beautify(textarea.val(), { indent_size: 2 }));
                // 设置事件
                editor.getSession().on('change', function()
                {
                    textarea.val(editor.getSession().getValue());
                });
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
        // 1.数据检测
        if(!main.applications.application.method.list.checkObject())
        {
            return;
        }

        // 2.发送数据
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false })
        outString += '</request>';

        x.net.xhr('/api/application.method.save.aspx', outString, {
            popResultValue: 1,
            callback: function(response)
            {
                main.applications.application.method.list.getPaging(main.applications.application.method.list.paging.currentPage);
            }
        });
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
            x.net.xhr('/api/application.method.delete.aspx?id=' + id, {
                waitingMessage: i18n.net.waiting.deleteTipText,
                callback: function(response)
                {
                    main.applications.application.method.list.getPaging(main.applications.application.method.list.paging.currentPage);
                }
            });
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
        var treeViewName = '应用方法管理';
        var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.applications.application.method.list.setTreeViewNode(\'{treeNodeId}\')';

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

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.applications.application.method.list.tree' });

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

        main.applications.application.method.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        main.applications.application.method.list.paging.query.where.ApplicationId = value;

        main.applications.application.method.list.paging.query.orders = ' OrderId ';

        main.applications.application.method.list.getPaging(1);
    },
    /*#endregion*/

    /*
    * 打开应用查询窗口
    */
    getApplicationWizard: function(type)
    {
        type = (typeof (type) === 'undefined') ? 'default' : type;

        //
        // 关键代码 开始
        //

        if(type === 'search')
        {
            main.applications.applicationwizard.localStorage = '{"text":"' + $('#searchApplicationName').val() + '","value":"' + $('#searchApplicationId').val() + '"}';

            // 保存回调函数
            main.applications.applicationwizard.save_callback = function(response)
            {
                var resultView = '';
                var resultValue = '';

                var node = x.toJSON(response);

                resultView += node.text + ';';
                resultValue += node.value + ';';

                if(resultValue.substr(resultValue.length - 1, 1) === ';')
                {
                    resultView = resultView.substr(0, resultView.length - 1);
                    resultValue = resultValue.substr(0, resultValue.length - 1);
                }

                $('#searchApplicationName').val(resultView);
                $('#searchApplicationId').val(resultValue);

                main.applications.applicationwizard.localStorage = response;

                // 回调后 重新加载树
                var treeViewRootTreeNodeId = $('#searchApplicationId').val();

                main.applications.application.method.list.getTreeView(treeViewRootTreeNodeId);

                main.applications.application.method.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);
            };
        }
        else
        {
            main.applications.applicationwizard.localStorage = '{"text":"' + $('#applicationName').val() + '","value":"' + $('#applicationId').val() + '"}';

            // 保存回调函数
            main.applications.applicationwizard.save_callback = function(response)
            {
                var resultView = '';
                var resultValue = '';

                var node = x.toJSON(response);

                resultView += node.text + ';';
                resultValue += node.value + ';';

                if(resultValue.substr(resultValue.length - 1, 1) === ';')
                {
                    resultView = resultView.substr(0, resultView.length - 1);
                    resultValue = resultValue.substr(0, resultValue.length - 1);
                }

                $('#applicationName').val(resultView);
                $('#applicationId').val(resultValue);

                $('#applicationSettingGroupName').val(resultView);
                $('#applicationSettingGroupId').val('00000000-0000-0000-0000-000000000000');

                main.applications.applicationwizard.localStorage = response;
            };
        }

        // 取消回调函数
        // 注:执行完保存回调函数后, 默认执行取消回调函数.
        main.applications.applicationwizard.cancel_callback = function(response)
        {
            if(main.applications.applicationwizard.maskWrapper !== '')
            {
                main.applications.applicationwizard.maskWrapper.close();
            }
        };

        //
        // 关键代码 结束
        //

        // 非模态窗口, 需要设置.
        main.applications.applicationwizard.maskWrapper = main.applications.application.method.list.maskWrapper;

        // 加载地址簿信息
        main.applications.applicationwizard.load();

        main.applications.applicationwizard.maskWrapper.resize();
    },

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
    /*
    * 页面加载事件
    */
    load: function()
    {
        // 调整页面结构尺寸
        main.applications.application.method.list.resize();

        var treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';

        main.applications.application.method.list.getTreeView(treeViewRootTreeNodeId);

        main.applications.application.method.list.setTreeViewNode(treeViewRootTreeNodeId);

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnApplicationWizard').bind('click', function()
        {
            main.applications.application.method.list.getApplicationWizard('search');
        });

        $('#searchText').bind('keyup', function()
        {
            main.applications.application.method.list.filter();
        });

        $('#btnFilter').bind('click', function()
        {
            main.applications.application.method.list.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.applications.application.method.list.load);
// 重新调整页面大小
$(window).resize(main.applications.application.method.list.resize);
// 重新调整页面大小
$(document.body).resize(main.applications.application.method.list.resize);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.applications.application.method.list.getPaging(1);
}