x.register('main.applications.application.setting.group.list');

main.applications.application.setting.group.list = {

    paging: x.page.newPagingHelper(50),

    /*
    * 过滤
    */
    filter: function()
    {
        var whereClauseValue = '';

        var key = $('#searchText').val();

        if(key.trim() != '')
        {
            whereClauseValue = ' ( T.Code LIKE ##%' + key + '%## OR T.Name LIKE ##%' + key + '%## ) ';
        }

        main.applications.application.setting.group.list.whereClause = whereClauseValue;

        main.applications.application.setting.group.list.paging.whereClause = whereClauseValue;

        main.applications.application.setting.group.list.getPaging(1);
    },

    /**
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
        outString += '<th style="width:80px">分组代码</th>';
        outString += '<th >分组名称</th>';
        outString += '<th style="width:50px" >状态</th>';
        outString += '<th style="width:100px">修改日期</th>';
        outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></th>';
        outString += '<th class="table-freeze-head-padding" ><a href="javascript:window$refresh$callback();"><small><span class="glyphicon glyphicon-refresh"></span></small></a></th>';
        outString += '</tr>';
        outString += '</thead>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="table-freeze-body">';
        outString += '<table class="table table-striped">';
        outString += '<colgroup>';
        outString += '<col style="width:80px" />';
        outString += '<col />';
        outString += '<col style="width:60px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:30px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td>' + node.code + '</td>';
            outString += '<td><a href="javascript:main.applications.application.setting.group.list.openDialog(\'' + node.id + '\');">' + node.name + '</a></td>';
            outString += '<td class="text-center" >' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + x.date.newTime(node.modifiedDateView).toString('yyyy-MM-dd') + '</td>';
            outString += '<td><a href="javascript:main.applications.application.setting.group.list.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');" title="删除"><i class="fa fa-trash" ></i></a></td>';
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            outString += '<tr><td colspan="6" >&nbsp;</td></tr>';

            counter++;
        }

        outString += '</tbody>';
        outString += '</table>';
        outString += '</div>';

        return outString;
    },

    /*
     * 创建单个对象的视图
     */
    getObjectView: function(param)
    {
        var outString = '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px" ><span class="required-text" >所属应用名称<span></td>';
        outString += '<td class="table-body-input form-inline">';
        outString += '<div class="input-group">';
        outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.applicationId) == 'undefined' ? '' : param.applicationId) + '" /> ';
        outString += '<input id="applicationName" name="applicationName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【所属应用名称】必须填写。" class="form-control" style="width:381px;" value="' + (typeof (param.applicationDisplayName) == 'undefined' ? '' : param.applicationDisplayName) + '" /> ';
        outString += '<a href="javascript:x.ui.wizards.getApplicationWizard({\'targetValueName\':\'applicationId\',\'targetViewName\':\'applicationName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >所属父级分组<span></td>';
        outString += '<td class="table-body-input form-inline">';
        outString += '<div class="input-group">';
        outString += '<input id="parentId" name="parentId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.parentId) == 'undefined' ? '' : param.parentId) + '" /> ';
        outString += '<input id="parentName" name="parentName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【所属父级分组】必须填写。" class="form-control" style="width:381px;" value="' + (typeof (param.parentName) == 'undefined' ? '' : param.parentName) + '" /> ';
        outString += '<a href="javascript:x.ui.wizards.getApplicationSettingGroupWizard({applicationId:\'' + param.applicationId + '\',applicationDisplayName:\'' + param.applicationDisplayName + '\',\'targetValueName\':\'parentId\',\'targetViewName\':\'parentName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >分组代码<span></td>';
        if(typeof (param.code) == 'undefined' || param.code == '')
        {
            outString += '<td class="table-body-input">';
            outString += '<span class="gray-text" >自动生成</span>';
            outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
            outString += '</td>';
        }
        else
        {
            outString += '<td class="table-body-input"><input id="code" name="code" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【分组代码】。" class="form-control" style="width:420px;" value="' + (typeof (param.code) == 'undefined' ? '' : param.code) + '" /></td>';
        }
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >分组名称<span></td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="name" name="name" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【分组名称】。" class="form-control" style="width:420px;" value="' + (typeof (param.name) == 'undefined' ? '' : param.name) + '" />';
        outString += '<input id="originalName" name="originalName" type="hidden" x-dom-data-type="value" value="' + (typeof (param.name) == 'undefined' ? '' : param.name) + '" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text" >分组显示名称<span></td>';
        outString += '<td class="table-body-input"><input id="displayName" name="displayName" type="text" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="必须填写【分组显示名称】。" class="form-control" style="width:420px;" value="' + (typeof (param.displayName) == 'undefined' ? '' : param.displayName) + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >排序</td>';
        outString += '<td class="table-body-input"><input id="orderId" name="orderId" type="text" x-dom-data-type="value" class="form-control" style="width:420px;" value="' + (typeof (param.orderId) == 'undefined' ? '' : param.orderId) + '" /></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >备注</td>';
        outString += '<td class="table-body-input"><textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="textarea-normal" style="width:420px;height:50px;" >' + (typeof (param.remark) == 'undefined' ? '' : param.remark) + '</textarea></td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input"><input id="status" name="status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + (typeof (param.status) == 'undefined' ? '' : param.status) + '" /></td>';
        outString += '</tr>';

        outString += '</table>';

        outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + (typeof (param.id) == 'undefined' ? '' : param.id) + '" />';
        outString += '<input id="contentType" name="contentType" type="hidden" x-dom-data-type="value" value="' + (typeof (param.contentType) == 'undefined' ? '0' : param.contentType) + '" />';
        outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" x-dom-data-type="value" value="' + (typeof (param.modifiedDateTimestampView) == 'undefined' ? '' : param.modifiedDateTimestampView) + '" />';

        return outString;
    },

    /*
    * 分页
    */
    getPaging: function(currentPage)
    {
        var paging = main.applications.application.setting.group.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/application.settingGroup.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.applications.application.setting.group.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                               + '<a href="javascript:main.applications.application.setting.group.list.openDialog();" class="btn btn-default" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
                               + '</div>'
                               + '<span>应用参数分组</span>'
                               + '<div class="clearfix" ></div>';

                $('#window-main-table-header').html(headerHtml);

                var containerHtml = main.applications.application.setting.group.list.getObjectsView(list, paging.pagingize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.applications.application.setting.group.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                masterpage.resize();
            }
        });
    },

    /*#region 函数:openDialog(value)*/
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
            url = '/api/application.settingGroup.create.aspx';

            outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';

            // var treeNode = main.membership.organization.list.tree.getSelectedNode();

            // if(treeNode != null)
            // {
            //    outString += '<parentId><![CDATA[' + treeNode.id + ']]></parentId>';
            // }

            isNewObject = true;
        }
        else
        {
            url = '/api/application.settingGroup.findOne.aspx';

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
                               + '<a href="javascript:main.applications.application.setting.group.list.save();" class="btn btn-default"><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
                               + '<a href="javascript:main.applications.application.setting.group.list.getPaging(' + main.applications.application.setting.group.list.paging.currentPage + ');" class="btn btn-default"><i class="fa fa-ban" title="关闭"></i> 关闭</a>'
                               + '</div>'
                               + '<span>应用参数分组</span>'
                               + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.applications.application.setting.group.list.getObjectView(param);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                x.dom.features.bind();

                x.ui.pkg.tabs.newTabs();
            }
        });
    },

    /*
    * 检测对象的必填数据
    */
    checkObject: function()
    {
        if(x.dom.data.check()) { return false; }

        if($('#parentId').val() == $('#id').val())
        {
            alert('【父级分组】不能是自己本身。');

            return false;
        }

        return true;
    },

    save: function()
    {
        // 1.数据检测
        if(main.applications.application.setting.group.list.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' });
            outString += '</request>';

            x.net.xhr('/api/application.settingGroup.save.aspx', outString, {
                waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
                callback: function(response)
                {
                    var treeViewRootTreeNodeId = $('#searchApplicationId').val();

                    main.applications.application.setting.group.list.getTreeView(treeViewRootTreeNodeId);

                    main.applications.application.setting.group.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);

                    main.applications.application.setting.group.list.getPaging(main.applications.application.setting.group.list.paging.currentPage);
                }
            });
        }

        //// 2.发送数据
        //var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        //outString += '<ajaxStorage>';
        //outString += '<action><![CDATA[save]]></action>';
        //outString += '<id><![CDATA[' + $('#id').val() + ']]></id>';
        //outString += '<applicationId><![CDATA[' + $('#applicationId').val() + ']]></applicationId>';
        //outString += '<parentId><![CDATA[' + $('#parentId').val() + ']]></parentId>';
        //outString += '<code><![CDATA[' + $('#code').val() + ']]></code>';
        //outString += '<name><![CDATA[' + $('#name').val() + ']]></name>';
        //outString += '<displayName><![CDATA[' + $('#displayName').val() + ']]></displayName>';
        //outString += '<contentType><![CDATA[' + $('#contentType').val() + ']]></contentType>';
        //outString += '<orderId><![CDATA[' + $('#orderId').val() + ']]></orderId>';
        //outString += '<status><![CDATA[' + ($('#status')[0].checked ? '1' : '0') + ']]></status>';
        //outString += '<remark><![CDATA[' + $('#remark').val() + ']]></remark>';
        //outString += '<modifiedDate><![CDATA[' + $('#modifiedDate').val() + ']]></modifiedDate>';
        //outString += '<originalName><![CDATA[' + $('#originalName').val() + ']]></originalName>';
        //outString += '</ajaxStorage>';

        //var options = {
        //    resultType: 'json',
        //    xml: outString
        //};

        //$.post(main.applications.application.setting.group.list.url,
        //        options,
        //        main.applications.application.setting.group.list.save_callback);
    },

    /*#region 函数:confirmDelete(id)*/
    /*
    * 删除对象
    */
    confirmDelete: function(id)
    {
        if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
        {
            x.net.xhr('/api/application.settingGroup.delete.aspx?id=' + id, {
                waitingMessage: i18n.strings.msg_net_waiting_delete_tip_text,
                callback: function(response)
                {
                    main.applications.application.setting.group.list.getPaging(1);
                }
            });
        }
    },
    /*#endregion*/

    /*
    * 获取树形菜单
    */
    getTreeView: function(value)
    {
        var treeViewId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewName = $('#searchApplicationName').val();
        var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.applications.application.setting.group.list.setTreeViewNode(\'{treeNodeId}\')';

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

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.applications.application.setting.group.list.tree' });

        tree.setAjaxMode(true);

        // tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', '[ApplicationId]' + treeViewRootTreeNodeId), treeViewName, '', '/resources/images/tree/tree_icon.gif');

        tree.add({
            id: "0",
            parentId: "-1",
            name: treeViewName,
            url: treeViewUrl.replace('{treeNodeId}', '[ApplicationId]' + treeViewRootTreeNodeId),
            title: treeViewName,
            target: '',
            icon: '/resources/images/tree/tree_icon.gif'
        });

        tree.load('/api/application.settingGroup.getDynamicTreeView.aspx', false, outString);

        main.applications.application.setting.group.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },

    setTreeViewNode: function(value)
    {
        // var whereClause = ' T.ParentId =  ##' + value + '## ';

        // if(value.indexOf('[ApplicationId]') == 0)
        // {
        //    whereClause = ' T.ApplicationId = ##' + value.replace('[ApplicationId]', '') + '## AND T.ParentId = ##00000000-0000-0000-0000-000000000000## ';
        // }

        // main.applications.application.setting.group.list.paging.whereClause = whereClause;

        main.applications.application.setting.group.list.paging.query.where.ParentId = value;

        if(value.indexOf('[ApplicationId]') == 0)
        {
            main.applications.application.setting.group.list.paging.query.where.ApplicationId = value.replace('[ApplicationId]', '');
            main.applications.application.setting.group.list.paging.query.where.ParentId = '00000000-0000-0000-0000-000000000000';
        }

        main.applications.application.setting.group.list.getPaging(1);
    },

    /*
    * 打开应用查询窗口
    */
    getApplicationWizard: function(type)
    {
        var storage = '';
        var viewArray = '';
        var valueArray = '';

        type = (typeof (type) == 'undefined') ? 'default' : type;

        //
        // 关键代码 开始
        //

        if(type == 'search')
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

                if(resultValue.substr(resultValue.length - 1, 1) == ';')
                {
                    resultView = resultView.substr(0, resultView.length - 1)
                    resultValue = resultValue.substr(0, resultValue.length - 1);
                }

                $('#searchApplicationName').val(resultView);
                $('#searchApplicationId').val(resultValue);

                main.applications.applicationwizard.localStorage = response;

                // 回调后 重新加载树
                var treeViewRootTreeNodeId = $('#searchApplicationId').val();

                main.applications.application.setting.group.list.getTreeView(treeViewRootTreeNodeId);

                main.applications.application.setting.group.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);
            }
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

                if(resultValue.substr(resultValue.length - 1, 1) == ';')
                {
                    resultView = resultView.substr(0, resultView.length - 1)
                    resultValue = resultValue.substr(0, resultValue.length - 1);
                }

                $('#applicationName').val(resultView);
                $('#applicationId').val(resultValue);

                $('#parentName').val(resultView);
                $('#parentId').val('00000000-0000-0000-0000-000000000000');

                main.applications.applicationwizard.localStorage = response;
            }
        }

        // 取消回调函数
        // 注:执行完保存回调函数后, 默认执行取消回调函数.
        main.applications.applicationwizard.cancel_callback = function(response)
        {
            if(main.applications.applicationwizard.maskWrapper != '')
            {
                main.applications.applicationwizard.maskWrapper.close();
            }
        }

        //
        // 关键代码 结束
        //

        // 非模态窗口, 需要设置.
        main.applications.applicationwizard.maskWrapper = main.applications.application.setting.group.list.maskWrapper;

        // 加载地址簿信息
        main.applications.applicationwizard.load();
    },

    /*
    * 打开应用参数分组查询窗口
    */
    getApplicationSettingGroupWizard: function()
    {
        var storage = '';
        var viewArray = '';
        var valueArray = '';

        //
        // 关键代码 开始
        //

        main.applications.applicationsettinggroupwizard.setTreeView($('#applicationId').val(), $('#applicationName').val());

        main.applications.applicationsettinggroupwizard.localStorage = '{"text":"' + $('#applicationSettingGroupName').val() + '","value":"' + $('#applicationSettingGroupId').val() + '"}';

        // 保存回调函数
        main.applications.applicationsettinggroupwizard.save_callback = function(response)
        {
            var resultView = '';
            var resultValue = '';

            var node = x.toJSON(response);

            if(response == "{}")
            {
                $('#parentName').val('');
                $('#parentId').val('');
                return;
            }

            resultView += node.text + ';';
            resultValue += node.value + ';';

            if(resultValue.substr(resultValue.length - 1, 1) == ';')
            {
                resultView = resultView.substr(0, resultView.length - 1)
                resultValue = resultValue.substr(0, resultValue.length - 1);
            }

            $('#parentName').val(resultView);
            $('#parentId').val(resultValue);

            main.applications.applicationsettinggroupwizard.localStorage = response;
        }

        // 取消回调函数
        // 注:执行完保存回调函数后, 默认执行取消回调函数.
        main.applications.applicationsettinggroupwizard.cancel_callback = function(response)
        {
            if(main.applications.applicationsettinggroupwizard.maskWrapper != '')
            {
                main.applications.applicationsettinggroupwizard.maskWrapper.close();
            }
        }

        //
        // 关键代码 结束
        //

        // 非模态窗口, 需要设置.
        main.applications.applicationsettinggroupwizard.maskWrapper = main.applications.application.setting.group.list.maskWrapper;

        // 加载地址簿信息
        main.applications.applicationsettinggroupwizard.load();
    },

    /**
    * 页面加载事件
    */
    load: function()
    {
        var treeViewRootTreeNodeId = $('#searchApplicationId').val();

        main.applications.application.setting.group.list.getTreeView(treeViewRootTreeNodeId);

        main.applications.application.setting.group.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);

        // -------------------------------------------------------
        // 过滤事件
        // -------------------------------------------------------

        $('#btnApplicationWizard').on('click', function()
        {
            x.ui.wizards.getApplicationWizard({
                targetValueName: 'searchApplicationId',
                targetViewName: 'searchApplicationName',
                save_callback: function(response)
                {
                    var resultView = '';
                    var resultValue = '';

                    var node = x.toJSON(response);

                    resultView += node.text + ';';
                    resultValue += node.value + ';';

                    if(resultValue.substr(resultValue.length - 1, 1) == ';')
                    {
                        resultView = resultView.substr(0, resultView.length - 1)
                        resultValue = resultValue.substr(0, resultValue.length - 1);
                    }

                    $('#searchApplicationName').val(resultView);
                    $('#searchApplicationId').val(resultValue);

                    // 回调后 重新加载树
                    var treeViewRootTreeNodeId = $('#searchApplicationId').val();

                    main.applications.application.setting.group.list.getTreeView(treeViewRootTreeNodeId);

                    main.applications.application.setting.group.list.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);
                }
            });
        });

        $('#searchText').on('keyup', function()
        {
            main.applications.application.setting.group.list.filter();
        });

        $('#searchButton').on('click', function()
        {
            main.applications.application.setting.group.list.filter();
        });
    }
    /*#endregion*/
}

$(document).ready(main.applications.application.setting.group.list.load);