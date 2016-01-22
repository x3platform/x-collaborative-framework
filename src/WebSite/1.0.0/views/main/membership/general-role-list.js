x.register('main.membership.general.role.list');

main.membership.general.role.list = {

    url: "/services/X3Platform/Membership/Ajax.GeneralRoleWrapper.aspx",

    paging: x.page.newPagingHelper(),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {

        var whereClauseValue = '';

        var key = $('#searchText').val();

        if(key.trim() !== '')
        {
            whereClauseValue = ' T.Name LIKE ##%' + key + '%## ';
        }

        main.membership.general.role.list.paging.whereClause = whereClauseValue;

        main.membership.general.role.list.paging.orderBy = ' OrderId ';

        main.membership.general.role.list.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:getObjectsView(list, maxCount)*/
    /*
    * 创建对象列表的视图
    */
    getObjectsView: function(list, maxCount)
    {
        var counter = 0;

        var classNameValue = '';

        var outString = '';

        outString += '<table class="table-style" style="width:100%">';
        outString += '<tbody>';
        outString += '<tr class="table-row-title">';
        outString += '<td >名称</td>';
        outString += '<td style="width:60px" >状态</td>';
        outString += '<td style="width:80px" >更新日期</td>';
        outString += '<td class="end" style="width:40px" >删除</td>';
        outString += '</tr>';

        x.each(list, function(index, node)
        {
            classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

            classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

            outString += '<tr class="' + classNameValue + '">';
            outString += '<td><a href="javascript:main.membership.general.role.list.openDialog(\'' + node.id + '\');" >' + node.name + '</a></td>';
            outString += '<td>' + x.customForm.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            if(node.locking === '1')
            {
                outString += '<td><span class="gray-text">删除</span></td>';
            }
            else
            {
                outString += '<td><a href="javascript:main.membership.general.role.list.confirmDelete(\'' + node.id + '\');">删除</a></td>';
            }
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            var classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

            classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

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
        var outString = '<div class="x-ui-pkg-tabs-wrapper">';

        outString += '<div class="x-ui-pkg-tabs-menu-wrapper" >';
        outString += '<ul class="x-ui-pkg-tabs-menu nav nav-tabs" >';
        outString += '<li><a href="#tab-1">基本信息</a></li>';
        outString += '<li><a href="#tab-2">授权关系</a></li>';
        outString += '<li><a href="#tab-3">备注</a></li>';
        outString += '</ul>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="tab-1" name="tab-1" >基本信息</a></h2>';
        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" >编码</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + ((typeof (param.id) === 'undefined' || param.id === '0') ? '' : param.id) + '" />';
        outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" />';
        outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) === 'undefined' ? '' : param.parentId) + '" />';
        if(typeof (param.code) === 'undefined' || param.code === '')
        {
            outString += '<span class="gray-text">自动编号</span>';
            outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
        }
        else
        {
            outString += '<input id="code" name="code" type="text" x-dom-data-type="value" value="' + (typeof (param.code) === 'undefined' ? '' : param.code) + '" class="form-control" style="width:120px;" />';
        }
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">名称</span></td>';
        outString += '<td class="table-body-input" style="width:160px" >';
        outString += '<input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【名称】必须填写。" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">所属类别</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="groupTreeNodeId" name="groupTreeNodeId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.groupTreeNodeId) === 'undefined' ? '' : param.groupTreeNodeId) + '" />';
        outString += '<input id="groupTreeNodeName" name="groupTreeNodeName" type="text" x-dom-data-type="value" value="' + (typeof (param.groupTreeNodeName) === 'undefined' ? '' : param.groupTreeNodeName) + '" dataVerifyWarning="【所属类别】必须填写。" class="form-control" style="width:120px;" /> ';
        outString += '<a href="javascript:x.ui.wizards.getGroupTreeWizardSingleton(\'groupTreeNodeName\', \'groupTreeNodeId\', \'50000000-0000-0000-0000-000000000000\', \'通用角色类别\');" >编辑</a> ';
        outString += '</td>';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" ><span class="required-text">全局名称</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="originalGlobalName" name="originalGlobalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" />';
        outString += '<input id="globalName" name="globalName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【全局名称】必须填写。" value="' + (typeof (param.globalName) === 'undefined' ? '' : param.globalName) + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px;" >拼音</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="pinyin" name="pinyin" type="text" x-dom-data-type="value" value="' + (typeof (param.pinyin) === 'undefined' ? '' : param.pinyin) + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" >排序</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" value="' + (typeof (param.orderId) === 'undefined' ? '' : param.orderId) + '" class="form-control x-ajax-checkbox" style="width:120px;" />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px" >启用企业邮箱</td>';
        outString += '<td class="table-body-input" >';
        outString += '<div class="checkbox-wrapper" ><input id="enableExchangeEmail" name="enableExchangeEmail" type="checkbox" x-dom-data-type="value" value="' + (typeof (param.enableExchangeEmail) === 'undefined' ? '' : param.enableExchangeEmail) + '" class="checkbox-normal x-ajax-checkbox" /><div>';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="status" name="status" type="checkbox" x-dom-data-type="value" value="' + (typeof (param.status) === 'undefined' ? '' : param.status) + '" class="form-control x-ajax-checkbox" />';
        outString += '</td>';
        outString += '<td class="table-body-text" >防止意外删除</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="locking" name="locking" type="checkbox" x-dom-data-type="value" value="' + (typeof (param.locking) === 'undefined' ? '' : param.locking) + '" class="form-control x-ajax-checkbox" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >修改时间</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += (typeof (param.modifiedDateTimestampView) === 'undefined' ? '' : param.modifiedDateTimestampView);
        outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" x-dom-data-type="value" value="' + (typeof (param.modifiedDateTimestampView) === 'undefined' ? '' : param.modifiedDateTimestampView) + '" />';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">授权关系</a></h2>';

        //        outString += '<table class="table-style" style="width:100%">';

        //        outString += '<tr class="table-row-normal">';
        //        outString += '<td class="table-body-text" style="width:120px" >成员</td>';
        //        outString += '<td class="table-body-input">';
        //        outString += '<textarea id="accountView" name="accountView" class="form-control" style="width:100%;height:40px;" >' + (typeof (param.accountView) === 'undefined' ? '' : param.accountView) + '</textarea>';
        //        outString += '<input id="accountValue" name="accountValue" type="hidden" value="' + (typeof (param.accountValue) === 'undefined' ? '' : param.accountValue) + '" />';
        //        outString += '<div class="text-right">';
        //        outString += '<a href="javascript:main.membership.general.role.list.getContactsWindow(\'account\');" >编辑</a> ';
        //        outString += '</div>';
        //        outString += '</td>';
        //        outString += '</tr>';

        //        outString += '<tr class="table-row-normal-transparent">';
        //        outString += '<td class="table-body-text" style="width:120px" >所属组织</td>';
        //        outString += '<td class="table-body-input">';
        //        outString += '<textarea id="organizationView" name="organizationView" class="form-control" style="width:100%;height:40px;" >' + (typeof (param.organizationView) === 'undefined' ? '' : param.organizationView) + '</textarea>';
        //        outString += '<input id="organizationValue" name="organizationValue" type="hidden" value="' + (typeof (param.organizationValue) === 'undefined' ? '' : param.organizationValue) + '" />';
        //        outString += '<div class="text-right">';
        //        outString += '<a href="javascript:main.membership.general.role.list.getContactsWindow(\'organization\');" >编辑</a> ';
        //        outString += '</div>';
        //        outString += '</td>';
        //        outString += '</tr>';

        //        outString += '</table>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-3" id="tab-3">备注</a></h2>';

        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" >备注</td>';
        outString += '<td class="table-body-input">';
        outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="form-control" style="width:460px; height:80px;" >' + (typeof (param.remark) === 'undefined' ? '' : param.remark) + '</textarea>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';
        outString += '</div>';

        outString += '</div>';

        // -------------------------------------------------------
        // 隐藏值设置
        // -------------------------------------------------------

        outString += '<input id="id" name="id" type="hidden" value="' + (typeof (param.id) === 'undefined' ? '' : param.id) + '" />';
        outString += '<input id="originalName" name="originalName" type="hidden" value="' + (typeof (param.name) === 'undefined' ? '' : param.name) + '" />';
        outString += '<input id="parentId" name="parentId" type="hidden" value="' + (typeof (param.parentId) === 'undefined' ? '' : param.parentId) + '" />';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:getPaging(currentPage)*/
    /*
    * 分页
    */
    getPaging: function(currentPage)
    {
        var paging = main.membership.general.role.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';
        
        x.net.xhr('/api/membership.generalRole.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.net.waiting.queryTipText,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.membership.general.role.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                               + '<a href="javascript:main.membership.general.role.list.openDialog();" class="btn btn-default" >新增</a>'
                               + '</div>'
                               + '<span>通用角色管理</span>'
                               + '<div class="clearfix" ></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.membership.general.role.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                var footerHtml = paging.tryParseMenu('javascript:main.membership.general.role.list.getPaging({0});');

                $('#window-main-table-footer')[0].innerHTML = footerHtml;
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
        var id = x.isUndefined(value, '');

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';

        var isNewObject = false;

        if(id === '')
        {
            url = '/api/membership.generalRole.create.aspx';

            var treeNode = main.membership.general.role.list.tree.getSelectedNode();

            outString += '<treeViewId><![CDATA[' + $('#treeViewId').val() + ']]></treeViewId>';

            if(treeNode !== null)
            {
                outString += '<groupTreeNodeId><![CDATA[' + treeNode.id + ']]></groupTreeNodeId>';
            }
        }
        else
        {
            url = '/api/membership.generalRole.findOne.aspx';

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
                               + '<a href="javascript:main.membership.general.role.list.save();" class="btn btn-default" >保存</a> '
                               + '<a href="javascript:main.membership.general.role.list.getPaging(' + main.membership.general.role.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                               + '</div>'
                               + '<span>通用角色</span>'
                               + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.membership.general.role.list.getObjectView(param);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

                $('#window-main-table-footer')[0].innerHTML = footerHtml;

                main.membership.general.role.list.setObjectView(param);

                x.ui.pkg.tabs.newTabs();
            }
        });
    },
    /*#endregion*/

    /*#region 函数:setObjectView(param)*/
    /*
    * 设置对象的视图
    */
    setObjectView: function(param)
    {
        // x.util.readonly('id');

        // x.util.readonly('code');

        if(param.enableExchangeEmail === '1')
        {
            $('#enableExchangeEmail')[0].checked = 'checked';
        }

        if(param.status === '1')
        {
            $('#status')[0].checked = 'checked';
        }

        if(param.lock === '1')
        {
            $('#lock')[0].checked = true;
        }

        if(param.status === '1')
        {
            $('#status')[0].checked = true;
        }
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
    /*
    * 保存对象
    */
    save: function()
    {
        if(main.membership.general.role.list.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' });
            outString += '</request>';

            x.net.xhr('/api/membership.generalRole.save.aspx', outString, {
                waitingMessage: i18n.net.waiting.saveTipText,
                callback: function(response)
                {
                    main.membership.general.role.list.getPaging(main.membership.general.role.list.paging.currentPage);
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
            x.net.xhr('/api/membership.generalRole.delete.aspx?id=' + id, {
                waitingMessage: i18n.net.waiting.deleteTipText,
                callback: function(response)
                {
                    main.membership.general.role.list.getPaging(main.membership.general.role.list.paging.currentPage);
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
        var treeViewId = $('#treeViewId').val();
        var treeViewName = $('#treeViewName').val();
        var treeViewRootTreeNodeId = $('#treeViewRootTreeNodeId').val();
        var treeViewUrl = 'javascript:main.membership.general.role.list.setTreeViewNode(\'{treeNodeId}\')';

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

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.general.role.list.tree' });

        tree.setAjaxMode(true);

        // tree.add("0", "-1", $('#treeViewName').val(), 'javascript:main.membership.general.role.list.setTreeViewNode(\'' + $('#treeViewRootTreeNodeId').val() + '\')', $('#treeViewName').val(), '', '/resources/images/tree/tree_icon.gif');

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

        main.membership.general.role.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        main.membership.general.role.list.paging.query.where.GroupTreeNodeId = value;

        main.membership.general.role.list.paging.query.orders = ' OrderId ';

        main.membership.general.role.list.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:resize()*/
    /*
    * 页面大小调整
    */
    resize: function()
    {
        // $('#treeViewContainer').css('height', '291px');
        // $('#treeViewContainer').css('width', '196px');
        // $('#treeViewContainer').css('overflow', 'auto');

        var height = x.page.getViewHeight() - 39;

        $('#treeViewContainer').css({
            'height': (height - 115) + 'px',
            'width': '196px',
            'overflow': 'auto'
        });
        //window-main-table-body
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
        main.membership.general.role.list.resize();

        // 正常加载
        main.membership.general.role.list.getTreeView();

        main.membership.general.role.list.setTreeViewNode($('#treeViewId').val());

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#searchText').bind('keyup', function()
        {
            main.membership.general.role.list.filter();
        });

        $('#btnFilter').bind('click', function()
        {
            main.membership.general.role.list.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.membership.general.role.list.load);
// 重新调整页面大小
$(window).resize(main.membership.general.role.list.resize);
// 重新调整页面大小
$(document.body).resize(main.membership.general.role.list.resize);
