x.register('x.ui.windows');

x.ui.windows.newApplicationSettingListWindow = function(name, options)
{
    return x.ext(x.ui.windows.newWindow(name, options), {

        paging: x.page.newPagingHelper(50),

        maskWrapper: x.ui.mask.newMaskWrapper('this.maskWrapper'),

        /*#region 函数:bindOptions(options)*/
        bindOptions: function(options)
        {
            this.applicationId = options.applicationId;
            this.applicationName = options.applicationName;

            this.treeViewContainer = options.treeViewContainer;
            this.searchText = options.searchText;
            this.btnFilter = options.btnFilter;

            this.tableHeader = options.tableHeader;
            this.tableContainer = options.tableContainer;
            this.tableFooter = options.tableFooter;
        },
        /*#endregion*/

        /*#region 函数:filter()*/
        /*
        * 查询
        */
        filter: function()
        {
            var whereClauseValue = ' ApplicationId = ##' + this.applicationId + '## ';

            var key = this.searchText ? this.searchText.val().trim() : '';

            if(key !== '')
            {
                whereClauseValue += ' AND ( T.Code LIKE ##%' + key + '%## OR T.Text LIKE ##%' + key + '%## OR T.Value LIKE ##%' + key + '%## ) ';
            }

            this.paging.query.scence = 'Query';
            this.paging.query.where.ApplicationId = this.applicationId;
            this.paging.query.where.SearchText = this.searchText.val().trim();
            this.paging.query.orders = ' OrderId, Value ';

            this.getPaging(1);
        },
        /*#endregion*/

        /*#region 函数:getObjectsView(me, list, maxCount)*/
        /*
        * 创建对象列表的视图
        */
        getObjectsView: function(me, list, maxCount)
        {
            var outString = '';

            var counter = 0;

            var classNameValue = '';

            outString += '<div class="table-freeze-head">';
            outString += '<table class="table" >';
            outString += '<thead>';
            outString += '<tr>';
            outString += '<th style="width:80px">参数代码</th>';
            outString += '<th >参数的文本</td>';
            outString += '<th style="width:80px">参数的值</th>';
            outString += '<th style="width:40px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
            outString += '<th style="width:100px">修改日期</th>';
            outString += '<th style="width:30px" title="编辑" ><i class="fa fa-pencil-square-o" ></i></th>';
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
                outString += '<td><a href="javascript:' + me.name + '.openDialog(\'' + node.id + '\');">' + node.text + '</a></td>';
                outString += '<td>' + node.value + '</td>';
                outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
                outString += '<td>' + node.modifiedDateView + '</td>';
                outString += '<td><a href="javascript:' + me.name + '.openDialog(\'' + node.id + '\');" title="编辑" ><i class="fa fa-pencil-square-o" ></i></a></td>';
                // outString += '<td><a href="javascript:' + me.name + '.confirmDelete(\'' + node.id + '\',\'' + node.applicationName + '\');">删除</a></td>';
                outString += '</tr>';

                counter++;
            });

            // 补全

            while(counter < maxCount)
            {
                outString += '<tr >';
                outString += '<td colspan="6" >&nbsp;</td>';
                outString += '</tr>';

                counter++;
            }

            outString += '</tbody>';
            outString += '</table>';
            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:getObjectView(me, param)*/
        /**
         * 创建单个对象的视图
         */
        getObjectView: function(me, param)
        {
            var outString = '';

            outString += '<input id="id" name="id" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.id, '') + '" />';
            outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.modifiedDateTimestampView, '') + '" />';

            outString += '<div class="form-horizontal" style="margin:20px;" >';

            outString += '<div class="form-group">';
            outString += '<label for="applicationName" class="col-sm-2 control-label"><span class="required-text">所属应用名称</span></label>';
            outString += '<div class="col-sm-10">';
            outString += '<input id="applicationId" name="applicationId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.applicationId, '') + '" /> ';
            outString += '<input id="applicationDisplayName" name="applicationDisplayName" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" style="width:420px;" x-dom-data-required-warning="必须填写【所属应用名称】。" value="' + x.isUndefined(param.applicationDisplayName, '') + '" readonly="readonly" /> ';
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label for="applicationName" class="col-sm-2 control-label"><span class="required-text">所属参数分组</span></label>';
            outString += '<div class="col-sm-10">';
            outString += '<div class="form-inline">';
            outString += '<div class="input-group">';
            outString += '<input id="applicationSettingGroupName" name="applicationSettingGroupName" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" x-dom-data-required-warning="必须填写【所属参数分组】。" value="' + (typeof (param.applicationSettingGroupName) === 'undefined' ? '' : (param.applicationSettingGroupId === '00000000-0000-0000-0000-000000000000' ? param.applicationDisplayName : param.applicationSettingGroupName)) + '"  class="form-control" style="width:381px;" /> ';
            outString += '<input id="applicationSettingGroupId" name="applicationSettingGroupId" type="hidden" x-dom-data-type="value" value="' + (typeof (param.applicationSettingGroupId) === 'undefined' ? '' : param.applicationSettingGroupId) + '" /> ';
            outString += '<a href="javascript:x.ui.wizards.getApplicationSettingGroupWizard({applicationId:$(\'#applicationId\').val(),applicationDisplayName:$(\'#applicationDisplayName\').val(),\'targetValueName\':\'applicationSettingGroupId\',\'targetViewName\':\'applicationSettingGroupName\'});" class="input-group-addon" title="编辑" ><i class="glyphicon glyphicon-modal-window"></i></a>';
            outString += '</div>';
            outString += '</div>';
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label class="col-sm-2 control-label"><span class="required-text">参数代码</span></label>';
            outString += '<div class="col-sm-10">';
            if(typeof (param.code) === 'undefined' || param.code === '')
            {
                outString += '<span class="gray-text" >自动生成</span>';
                outString += '<input id="code" name="code" type="hidden" x-dom-data-type="value" value="" />';
            }
            else
            {
                outString += '<input id="code" name="code" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" style="width:420px;" x-dom-data-required-warning="必须填写【分组代码】。" value="' + (typeof (param.code) === 'undefined' ? '' : param.code) + '" readonly="readonly" />';
            }
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label class="col-sm-2 control-label"><span class="required-text">参数文本</span></label>';
            outString += '<div class="col-sm-10">';
            outString += '<input id="text" name="text" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" x-dom-data-required-warning="必须填写【参数文本】。" value="' + (typeof (param.text) === 'undefined' ? '' : param.text) + '" style="width:420px;" />';
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label class="col-sm-2 control-label"><span class="required-text">参数值</span></label>';
            outString += '<div class="col-sm-10">';
            outString += '<input id="value" name="value" type="text" x-dom-data-type="value" x-dom-data-required="1" class="form-control" x-dom-data-required-warning="必须填写【参数值】。" style="width:420px;" value="' + (typeof (param.value) === 'undefined' ? '' : param.value) + '" />';
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label class="col-sm-2 control-label">排序</label>';
            outString += '<div class="col-sm-10">';
            outString += '<input id="orderId" name="orderId" type="text" x-dom-data-type="value" class="form-control" style="width:420px;" value="' + (typeof (param.orderId) === 'undefined' ? '' : param.orderId) + '" />';
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label class="col-sm-2 control-label">备注</label>';
            outString += '<div class="col-sm-10">';
            outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="textarea-normal" style="width:420px;height:50px;" >' + (typeof (param.remark) === 'undefined' ? '' : param.remark) + '</textarea>';
            outString += '</div>';
            outString += '</div>';

            outString += '<div class="form-group">';
            outString += '<label class="col-sm-2 control-label">启用</label>';
            outString += '<div class="col-sm-10">';
            outString += '<input id="status" name="status" type="checkbox" x-dom-feature="checkbox" x-dom-data-type="value" value="' + x.isUndefined(param.status, '') + '" />';
            outString += '</div>';
            outString += '</div>';

            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:getPaging(currentPage)*/
        getPaging: function(currentPage)
        {
            var paging = this.paging;

            paging.currentPage = currentPage;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += paging.toXml();
            outString += '</request>';

            var me = this;

            x.net.xhr('/api/application.setting.query.aspx', outString, {
                waitingType: 'mini',
                waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var paging = me.paging;

                    var list = result.data;

                    paging.load(result.paging);

                    var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                        + '<a href="javascript:' + me.name + '.openDialog();" class="btn btn-default" class="table-toolbar-button" ><i class="glyphicon glyphicon-plus" title="新增"></i> 新增</a>'
                        + '</div>'
                        + '<span>参数设置</span>'
                        + '<div class="clearfix" ></div>';

                    me.tableHeader.html(headerHtml);

                    var containerHtml = me.getObjectsView(me, list, paging.pageSize);

                    me.tableContainer.html(containerHtml);

                    var footerHtml = paging.tryParseMenu('javascript:' + me.name + '.getPaging({0})');

                    me.tableFooter.html(footerHtml);

                    // 调整页面结构尺寸
                    me.resize();
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
                url = '/api/application.setting.create.aspx';

                outString += '<applicationId><![CDATA[' + $('#searchApplicationId').val() + ']]></applicationId>';
            }
            else
            {
                url = '/api/application.setting.findOne.aspx';

                outString += '<id><![CDATA[' + id + ']]></id>';
            }
            outString += '</request>';

            var me = this;

            x.net.xhr(url, outString, {
                callback: function(response)
                {
                    var param = x.toJSON(response).data;

                    var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                        + '<a href="javascript:' + me.name + '.save();" class="btn btn-default" ><i class="fa fa-floppy-o" title="保存"></i> 保存</a> '
                        + '<a href="javascript:' + me.name + '.getPaging(' + me.paging.currentPage + ');" class="btn btn-default" ><i class="fa fa-ban" title="关闭"></i> 关闭</a>'
                        + '</div>'
                        + '<span>应用参数设置</span>'
                        + '<div class="clear"></div>';

                    $('#window-main-table-header')[0].innerHTML = headerHtml;

                    var containerHtml = me.getObjectView(me, param);

                    $('#window-main-table-container')[0].innerHTML = containerHtml;

                    $('#window-main-table-footer')[0].innerHTML = '<img src="/resources/images/transparent.gif" style="height:18px" />';

                    // me.setObjectView(param);

                    x.dom.features.bind();

                    x.ui.pkg.tabs.newTabs();

                    // 调整页面结构尺寸
                    me.resize();
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
            if(!this.checkObject()) { return; }

            // 2.发送数据
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
            outString += '</request>';

            var me = this;

            x.net.xhr('/api/application.setting.save.aspx', outString, {
                popCorrectValue: 1,
                callback: function(response)
                {
                    me.getPaging(me.paging.currentPage);
                }
            });
        },
        /*#endregion*/

        /*#region 函数:confirmDelete(ids)*/
        confirmDelete: function(id)
        {
            if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
            {
                x.net.xhr('/api/application.setting.delete.aspx?id=' + id, {
                    callback: function(response)
                    {
                        this.getPaging(this.paging.currentPage);
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
            var treeViewId = value;
            var treeViewName = this.applicationName;
            var treeViewRootTreeNodeId = value; // 默认值:'00000000-0000-0000-0000-000000000001'
            var treeViewUrl = 'javascript:' + this.name + '.setTreeViewNode(\'{treeNodeId}\')';

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
            outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
            outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
            outString += '<tree><![CDATA[{tree}]]></tree>';
            outString += '<parentId><![CDATA[{parentId}]]></parentId>';
            outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
            outString += '</request>';

            var tree = x.ui.pkg.tree.newTreeView({ name: this.name + '.tree', ajaxMode: true, useCookies: false });

            tree.add({
                id: "0",
                parentId: "-1",
                name: treeViewName,
                url: treeViewUrl.replace('{treeNodeId}', '[ApplicationId]' + treeViewRootTreeNodeId).replace('{treeNodeName}', treeViewName),
                title: treeViewName,
                target: '',
                icon: '/resources/images/tree/tree_icon.gif'
            });

            tree.load('/api/application.settingGroup.getDynamicTreeView.aspx', false, outString);

            this.tree = tree;

            $('#treeViewContainer')[0].innerHTML = tree;
        },
        /*#endregion*/

        /*#region 函数:setTreeViewNode(value)*/
        setTreeViewNode: function(value)
        {
            this.paging.query.scence = 'QueryByApplicationSettingGroupId';

            if(value.indexOf('[ApplicationId]') === 0)
            {
                this.paging.query.where.ApplicationId = value.replace('[ApplicationId]', '');
                this.paging.query.where.ApplicationSettingGroupId = '00000000-0000-0000-0000-000000000000';
            }
            else
            {
                this.paging.query.where.ApplicationSettingGroupId = value;
            }

            this.paging.query.orders = ' OrderId, Value ';

            this.getPaging(1);
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
                'width': '196px',
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

        /*#region 函数:create()*/
        /**
         * 页面加载事件
         */
        create: function()
        {
            var treeViewRootTreeNodeId = this.applicationId;

            this.getTreeView(treeViewRootTreeNodeId);

            this.setTreeViewNode('[ApplicationId]' + treeViewRootTreeNodeId);

            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            var me = this;

            this.searchText.on('keyup', function()
            {
                me.filter();
            });

            this.btnFilter.on('click', function()
            {
                me.filter();
            });

            // 调整页面结构尺寸
            me.resize();
            // 重新调整页面大小
            $(window).resize(me.resize);
            // 重新调整页面大小
            $(document.body).resize(me.resize);
        }
        /*#endregion*/
    });
};

x.ui.windows.getApplicationSettingListWindow = function(name, options)
{
    var name = x.getFriendlyName(location.pathname + '-window-' + name);

    var internalWindow = x.ui.windows.newApplicationSettingListWindow(name, options);

    // 加载界面、数据、事件
    internalWindow.load(options);

    // 绑定到Window对象
    window[name] = internalWindow;

    return internalWindow;
};
