x.register('x.ui.windows');

x.ui.windows.newWorkflowTemplateListWindow = function(name, options)
{
    return x.ext(x.ui.windows.newWindow(name, options), {

        paging: x.page.newPagingHelper(20),

        /*#region 函数:bindOptions(options)*/
        bindOptions: function(options)
        {
            this.btnCreateBlankTemplate = options.btnCreateBlankTemplate;

            this.type = options.type;

            this.searchText = options.searchText;
            this.btnFilter = options.btnFilter;

            this.tableHeader = options.tableHeader;
            this.tableContainer = options.tableContainer;
            this.tableFooter = options.tableFooter;
        },
        /*#endregion*/

        /*#region 函数:filter()*/
        /**
         * 查询
         */
        filter: function()
        {
            // var whereClauseValue = ' Type IN (##' + this.type.replace(/[,]/g, '##,##') + '##) ';

            // var key = this.searchText ? this.searchText.val() : '';

            // if(key.trim() !== '')
            //{
            //    whereClauseValue += ' AND ( T.Name LIKE ##%' + key + '%## ) ';
            //}

            //this.paging.whereClause = whereClauseValue;

            this.paging.query.scence = 'QueryByWizard';
            this.paging.query.where.Name = this.searchText.val().trim();
            this.paging.query.where.Type = this.type;
            this.paging.query.orders = ' UpdateDate DESC ';

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
            outString += '<th >名称</th>';
            outString += '<th style="width:40px" title="状态" ><i class="fa fa-dot-circle-o"></i></th>';
            outString += '<th style="width:100px" >更新日期</th>';
            outString += '<th style="width:30px" title="复制" ><i class="fa fa-files-o" ></i></th>';
            outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
            outString += '<th class="table-freeze-head-padding" ></th>';
            outString += '</tr>';
            outString += '</thead>';
            outString += '</table>';
            outString += '</div>';

            outString += '<div class="table-freeze-body">';
            outString += '<table class="table table-striped">';
            outString += '<colgroup>';
            outString += '<col /><col style="width:40px" /><col style="width:100px" /><col style="width:30px" /><col style="width:30px" />';
            outString += '</colgroup>';
            outString += '<tbody>';

            x.each(list, function(index, node)
            {
                outString += '<tr>';
                outString += '<td><a href="/workflowplus/workflow-designer?id=' + encodeURIComponent(node.id) + '" target="_blank" >' + node.name + '</a></td>';
                outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
                outString += '<td>' + node.modifiedDateView + '</td>';
                outString += '<td><a href="javascript:' + me.name + '.copy(\'' + node.id + '\');" title="复制" ><i class="fa fa-files-o" ></i></a></td>';
                outString += '<td><a href="javascript:' + me.name + '.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
                outString += '</tr>';

                counter++;
            });

            // 补全

            while(counter < maxCount)
            {
                outString += '<tr>';
                outString += '<td colspan="10" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
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
        getPaging: function(currentPage)
        {
            var paging = this.paging;

            paging.currentPage = currentPage;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += paging.toXml();
            outString += '</request>';

            var me = this;

            x.net.xhr('/api/workflow.template.query.aspx', outString, {
                waitingType: 'mini',
                waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var paging = me.paging;

                    var list = result.data;

                    paging.load(result.paging);

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

        /*#region 函数:confirmDelete(id)*/
        /*
        * 删除对象
        */
        confirmDelete: function(id)
        {
            if(confirm(i18n.msg.are_you_sure_you_want_to_delete))
            {
                var me = this;

                x.net.xhr('/api/workflow.template.delete.aspx?id=' + id, {
                    callback: function(response)
                    {
                        me.getPaging(me.paging.currentPage);
                    }
                });
            }
        },
        /*#endregion*/

        /*#region 函数:copy(id)*/
        /*
        * 复制对象
        */
        copy: function(id)
        {
            if(confirm('确定复制此模板?'))
            {
                var me = this;

                x.net.xhr('/api/workflow.template.copy.aspx?id=' + id, {
                    callback: function(response)
                    {
                        me.getPaging(me.paging.currentPage);
                    }
                });
            }
        },
        /*#endregion*/

        /*#region 函数:createBlankTemplate(type)*/
        /*
        * 新建空白模板
        */
        createBlankTemplate: function(type)
        {
            var me = this;

            x.net.xhr('/api/workflow.template.createBlankTemplate.aspx?type=' + encodeURIComponent(type), {
                callback: function(response)
                {
                    me.getPaging(me.paging.currentPage);
                }
            });
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

            $('#treeViewContainer').css({
                'height': (height - 115) + 'px',
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
        /*
        * 页面加载事件
        */
        create: function()
        {
            this.filter();

            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            var me = this;

            this.btnFilter.on('click', function()
            {
                me.filter();
            });

            this.btnCreateBlankTemplate.on('click', function()
            {
                me.createBlankTemplate(me.type);
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

x.ui.windows.getWorkflowTemplateListWindow = function(name, options)
{
    var name = x.getFriendlyName(location.pathname + '-window-' + name);

    var internalWindow = x.ui.windows.newWorkflowTemplateListWindow(name, options);

    // 加载界面、数据、事件
    internalWindow.load(options);

    // 绑定到Window对象
    window[name] = internalWindow;

    // [默认]私有回调函数, 供子窗口回调
    window.window$refresh$callback = function()
    {
        window[name].filter();
    };

    return internalWindow;
};