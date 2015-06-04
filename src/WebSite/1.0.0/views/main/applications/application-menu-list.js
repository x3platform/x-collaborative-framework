x.register('main.applications.application.menu.list');

main.applications.application.menu.list = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /**
     * 过滤
     */
    filter: function()
    {
        main.applications.application.menu.list.paging.query.scence = 'Query';
        main.applications.application.menu.list.paging.where.SearchText = $('#searchText').val().trim();
        main.applications.application.menu.list.paging.where.orders = ' OrderId ';

        main.applications.application.menu.list.getPaging(1);
    },
    /*#endregion*/

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
        outString += '<th style="width:80px">菜单代码</th>';
        outString += '<th >菜单显示文字</th>';
        outString += '<th style="width:80px">菜单类型</th>';
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
        outString += '<col style="width:80px" />';
        outString += '<col style="width:40px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:30px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td>' + node.code + '</td>';
            outString += '<td><a href="/applications/application-menu/form?id=' + node.id + '" target="_blank" >' + node.name + '</a> <span class="label label-default" >' + node.displayTypeView + '</span></td>';
            outString += '<td>' + node.menuTypeView + '</td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.updateDateView + '</td>';
            outString += '<td><a href="javascript:main.applications.application.menu.list.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');" title="删除"><i class="fa fa-trash" ></i></a></td>';
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

        return outString;
    },
    /*#endregion*/

    /*
    * 创建单个对象的视图
    */
    getObjectView: function(param)
    {
        var outString = '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >所属应用名称<span></td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="applicationId" name="applicationId" type="hidden" value="' + (typeof (param.applicationId) == 'undefined' ? '' : param.applicationId) + '" /> ';
        outString += '<input id="applicationName" name="applicationName" type="text" class="form-control custom-forms-data-required" style="width:420px;" dataVerifyWarning="必须填写【所属应用名称】。" value="' + (typeof (param.applicationDisplayName) == 'undefined' ? '' : param.applicationDisplayName) + '" /> ';
        outString += '<a href="javascript:main.applications.application.menu.list.getApplicationWizard(\'default\');" >编辑</a>';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >所属参数分组<span></td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="applicationSettingGroupId" name="applicationSettingGroupId" type="hidden" value="' + (typeof (param.applicationSettingGroupId) == 'undefined' ? '' : param.applicationSettingGroupId) + '" /> ';
        outString += '<input id="applicationSettingGroupName" name="applicationSettingGroupName" type="text" class="form-control custom-forms-data-required" dataVerifyWarning="必须填写【所属参数分组】。" style="width:420px;" value="' + (typeof (param.applicationSettingGroupName) == 'undefined' ? '' : (param.applicationSettingGroupId == '00000000-0000-0000-0000-000000000000' ? param.applicationDisplayName : param.applicationSettingGroupName)) + '" /> ';
        outString += '<a href="javascript:main.applications.application.menu.list.getApplicationSettingGroupWizard();" >编辑</a>';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >参数代码<span></td>';
        if(typeof (param.code) == 'undefined' || param.code == '')
        {
            outString += '<td class="table-body-input">';
            outString += '<span class="gray-text" >自动生成</span>';
            outString += '<input id="code" name="code" type="hidden" value="" />';
            outString += '</td>';
        }
        else
        {
            outString += '<td class="table-body-input"><input id="code" name="code" type="text" class="form-control custom-forms-data-required" style="width:420px;" dataVerifyWarning="必须填写【分组代码】。" value="' + (typeof (param.code) == 'undefined' ? '' : param.code) + '" /></td>';
        }
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >参数文本<span></td>';
        outString += '<td class="table-body-input"><input id="text" name="text" type="text" class="form-control custom-forms-data-required" style="width:420px;" dataVerifyWarning="必须填写【参数文本】。" value="' + (typeof (param.text) == 'undefined' ? '' : param.text) + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >参数值<span></td>';
        outString += '<td class="table-body-input"><input id="value" name="value" type="text" class="form-control custom-forms-data-required" dataVerifyWarning="必须填写【参数值】。" style="width:420px;" value="' + (typeof (param.value) == 'undefined' ? '' : param.value) + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >排序</td>';
        outString += '<td class="table-body-input"><input id="orderId" name="orderId" type="text" class="form-control" style="width:420px;" value="' + (typeof (param.orderId) == 'undefined' ? '' : param.orderId) + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >备注</td>';
        outString += '<td class="table-body-input"><textarea id="remark" name="remark" type="text" class="textarea-normal" style="width:420px;height:50px;" >' + (typeof (param.remark) == 'undefined' ? '' : param.remark) + '</textarea></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input"><input id="status" name="status" type="checkbox" value="' + (typeof (param.status) == 'undefined' ? '' : param.status) + '" /></td>';
        outString += '</tr>';

        outString += '</table>';

        outString += '<input id="id" name="id" type="hidden" value="' + (typeof (param.id) == 'undefined' ? '' : param.id) + '" />';
        outString += '<input id="updateDate" name="updateDate" type="hidden" value="' + (typeof (param.updateDateTimestampView) == 'undefined' ? '' : param.updateDateTimestampView) + '" />';

        return outString;
    },
    /*#endregion*/

    /*
    * 分页
    */
    getPaging: function(currentPage)
    {
        var paging = main.applications.application.menu.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/application.menu.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.net.waiting.queryTipText,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.applications.application.menu.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerHtml = main.applications.application.menu.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.applications.application.menu.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                main.applications.application.menu.list.resize();
            }
        });
    },

    createNewObject: function()
    {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[createNewObject]]></action>';
        outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';
        outString += '</ajaxStorage>';

        var options = {
            resultType: 'json',
            xml: outString
        };

        $.post(main.applications.application.menu.list.url,
                options,
                main.applications.application.menu.list.openDialog_callback);
    },

    /*
    * 查看单个记录的信息
    */
    openDialog: function(value)
    {
        var id = (typeof (value) == 'undefined' || value == 'new') ? '' : value;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[findOne]]></action>';
        outString += '<id><![CDATA[' + id + ']]></id>';
        outString += '</ajaxStorage>';

        var options = {
            resultType: 'json',
            xml: outString
        };

        $.post(main.applications.application.menu.list.url,
                options,
                main.applications.application.menu.list.openDialog_callback);
    },

    openDialog_callback: function(response)
    {
        x.net.fetchException(response);

        var param = x.toJSON(response).ajaxStorage;

        var headerHtml = '<div>'
                       + '<div class="float-right">'
                       + '<a href="javascript:main.applications.application.menu.list.save();" >保存</a> '
                       + '<a href="javascript:main.applications.application.menu.list.getPaging(' + main.applications.application.menu.list.paging.currentPage + ');" >关闭</a>'
                       + '</div>'
                       + '<div>应用参数设置</div>'
                       + '<div class="clear"></div>'
                       + '</div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.applications.application.menu.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" style="height:18px" />';

        main.applications.application.menu.list.setObjectView(param);

        x.form.features.bind();
    },

    /*
    * 设置对象的视图
    */
    setObjectView: function(param)
    {
        x.util.readonly('id');
        x.util.readonly('#applicationName');
        x.util.readonly('#applicationSettingGroupName');
        x.util.readonly('#code');

        if(param.status == '1')
        {
            $('#status')[0].checked = 'checked';
        }
    },

    /*
    * 检测对象的必填数据
    */
    checkObject: function()
    {

        if(x.customForm.checkDataStorage())
            return false;

        return true;
    },

    save: function()
    {
        // 1.数据检测
        if(!main.applications.application.menu.list.checkObject())
        {
            return;
        }

        // 2.发送数据
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[save]]></action>';
        outString += '<id><![CDATA[' + $('#id').val() + ']]></id>';
        outString += '<applicationId><![CDATA[' + $('#applicationId').val() + ']]></applicationId>';
        outString += '<applicationSettingGroupId><![CDATA[' + $('#applicationSettingGroupId').val() + ']]></applicationSettingGroupId>';
        outString += '<code><![CDATA[' + $('#code').val() + ']]></code>';
        outString += '<text><![CDATA[' + $('#text').val() + ']]></text>';
        outString += '<value><![CDATA[' + $('#value').val() + ']]></value>';
        outString += '<orderId><![CDATA[' + $('#orderId').val() + ']]></orderId>';
        outString += '<status><![CDATA[' + ($('#status')[0].checked ? '1' : '0') + ']]></status>';
        outString += '<remark><![CDATA[' + $('#remark').val() + ']]></remark>';
        outString += '<updateDate><![CDATA[' + $('#updateDate').val() + ']]></updateDate>';
        outString += '</ajaxStorage>';

        var options = {
            resultType: 'json',
            xml: outString
        };

        $.post(main.applications.application.menu.list.url,
                options,
                main.applications.application.menu.list.save_callback);
    },

    save_callback: function(response)
    {
        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
            case 0:
                alert(result.value);
                main.applications.application.menu.list.getPaging(main.applications.application.menu.list.paging.currentPage);
                break;

            case 1:
            case -1:
                alert(result.value);
                break;

            default:
                break;
        }
    },

    confirmDelete: function(value)
    {
        if(confirm('确定删除?'))
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<ajaxStorage>';
            outString += '<action><![CDATA[delete]]></action>';
            outString += '<ids><![CDATA[' + value + ']]></ids>';
            outString += '</ajaxStorage>';

            var options = {
                resultType: 'json',
                xml: outString
            };

            $.post(main.applications.application.menu.list.url, options, main.applications.application.menu.list.confirmDelete_callback);
        }
    },

    confirmDelete_callback: function(response)
    {
        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
            case 0:
                alert(result.value);
                main.applications.application.menu.list.getPaging(1);
                break;

            case 1:
            case -1:
                alert(result.value);
                break;

            default:
                break;
        }
    },

    /*
    * 获取树形菜单
    */
    getTreeView: function(value)
    {
        var treeViewId = value;
        var treeViewName = '菜单管理';
        var treeViewRootTreeNodeId = 'menu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000'; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.applications.application.menu.list.setTreeViewNode(\'{treeNodeId}\')';

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

        tree = x.ui.pkg.tree.newTreeView({ name: 'main.applications.application.menu.list.tree' });

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

        tree.load('/api/application.menu.getDynamicTreeView.aspx', false, outString);

        main.applications.application.menu.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        var keys = value.split('#');

        if(keys[0] == 'menu')
            keys[0] = 'ApplicationMenu';

        keys[0] = keys[0].substr(0, 1).toUpperCase() + keys[0].substr(1, keys[0].length - 1);

        $('#toolbar').html('<a href="/applications/application-menu/form?menuType=' + keys[0] + '&applicationId=' + keys[2] + '&menuId=' + keys[4] + '" target="_blank" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>');

        // var whereClause = ' T.MenuType = ##' + keys[0] + '## AND T.ApplicationId = ##' + keys[2] + '## AND T.ParentId =  ##' + keys[4] + '## ';

        main.applications.application.menu.list.paging.query.where.MenuType = keys[0];
        main.applications.application.menu.list.paging.query.where.ApplicationId = keys[2];
        main.applications.application.menu.list.paging.query.where.ParentId = keys[4];
        main.applications.application.menu.list.paging.query.orders = ' OrderId ';

        main.applications.application.menu.list.getPaging(1);
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

    /*
    * 页面加载事件
    */
    load: function()
    {
        // 调整页面结构尺寸
        main.applications.application.menu.list.resize();

        var treeViewRootTreeNodeId = 'menu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000';

        main.applications.application.menu.list.getTreeView(treeViewRootTreeNodeId);

        main.applications.application.menu.list.setTreeViewNode(treeViewRootTreeNodeId);

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnApplicationWizard').on('click', function()
        {
            main.applications.application.menu.list.getApplicationWizard('search');
        });

        $('#searchText').on('keyup', function()
        {
            main.applications.application.menu.list.filter();
        });

        $('#searchButton').on('click', function()
        {
            main.applications.application.menu.list.filter();
        });
    }
}

$(document).ready(main.applications.application.menu.list.load);
// 重新调整页面大小
$(window).resize(main.applications.application.menu.list.resize);
// 重新调整页面大小
$(document.body).resize(main.applications.application.menu.list.resize);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.applications.application.menu.list.getPaging(1);
}