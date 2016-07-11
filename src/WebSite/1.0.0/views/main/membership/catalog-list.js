(function(x, window)
{
    var main = {

        paging: x.page.newPagingHelper(50),

        /*#region 函数:filter()*/
        /*
        * 过滤
        */
        filter: function()
        {
            var whereClauseValue = '';

            var key = $('#searchText').val();

            if (key.trim() !== '')
            {
                // whereClauseValue = ' T.Name LIKE ##%' + key + '%## ';
                main.paging.query.where.name = key;
            }

            main.paging.query.orders = ' OrderId ';

            main.getPaging(1);
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

            outString += '<div class="table-freeze-head">';
            outString += '<table class="table" >';
            outString += '<thead>';
            outString += '<tr>';
            outString += '<th >名称</th>';
            outString += '<th style="width:80px">状态</th>';
            outString += '<th style="width:100px">更新日期</th>';
            outString += '<th style="width:30px" title="编辑" ><i class="fa fa-edit" ></i></th>';
            // outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
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
                classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

                classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

                outString += '<tr class="' + classNameValue + '">';
                if (node.displayType === 'Internal')
                {
                    outString += '<td>' + node.name + '</td>';
                }
                else
                {
                    outString += '<td><a href="/membership/catalog-item/list?treeViewId=' + node.id + '" target="_blank" >' + node.name + '</a></td>';
                }
                outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
                outString += '<td>' + x.date.newTime(node.modifiedDateView).toString('yyyy-MM-dd') + '</td>';
                outString += '<td><a href="javascript:main.openDialog(\'' + node.id + '\');" title="编辑" ><i class="fa fa-edit" ></i></a></td>';
                // outString += '<td><a href="javascript:main.confirmDelete(\'' + node.id + '\');" >删除</a></td>';
                // outString += '<td><span class="gray-text">删除</span></td>';
                outString += '</tr>';

                counter++;
            });

            // 补全

            while (counter < maxCount)
            {
                var classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

                classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

                outString += '<tr class="' + classNameValue + '">';
                outString += '<td colspan="5" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
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
            var outString = '<div class="x-ui-pkg-tabs-wrapper">';

            outString += '<div class="x-ui-pkg-tabs-menu-wrapper" >';
            outString += '<ul class="x-ui-pkg-tabs-menu nav nav-tabs" >';
            outString += '<li><a href="#tab-1">基本信息</a></li>';
            outString += '</ul>';
            outString += '</div>';

            outString += '<div class="x-ui-pkg-tabs-container" >';
            outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="tab-1" name="tab-1" >基本信息</a></h2>';
            outString += '<table class="table-style" style="width:100%">';

            outString += '<tr class="table-row-normal-transparent">';
            outString += '<td class="table-body-text bold-text" style="width:120px" >名称</td>';
            outString += '<td class="table-body-input">';
            outString += '<input id="name" name="name" type="text" value="' + x.isUndefined(param.name, '') + '" x-dom-data-type="value" x-dom-data-required="1" x-dom-data-required-warning="【名称】必须填写。" class="form-control" style="width:160px;" />';
            outString += '</td>';
            outString += '</tr>';

            outString += '<tr class="table-row-normal-transparent">';
            outString += '<td class="table-body-text bold-text" style="width:120px" >排序</td>';
            outString += '<td class="table-body-input">';
            outString += '<input id="orderId" name="orderId" type="text" value="' + x.isUndefined(param.orderId, '') + '" x-dom-data-type="value" class="form-control" style="width:160px;" />';
            outString += '</td>';
            outString += '</tr>';

            outString += '<tr class="table-row-normal-transparent">';
            outString += '<td class="table-body-text bold-text" style="width:120px" >启用</td>';
            outString += '<td class="table-body-input">';
            outString += '<input id="status" name="status" type="checkbox" value="' + x.isUndefined(param.status, '') + '" x-dom-data-type="value" />';
            outString += '</td>';
            outString += '</tr>';

            outString += '<tr class="table-row-normal-transparent">';
            outString += '<td class="table-body-text bold-text" style="width:120px" >备注</td>';
            outString += '<td class="table-body-input">';
            outString += '<textarea id="remark" name="remark" type="text" x-dom-data-type="value" class="text-normal" style="width:460px;height:60px;" >' + x.isUndefined(param.remark, '') + '</textarea>';
            outString += '</td>';
            outString += '</tr>';

            outString += '</table>';
            outString += '</div>';

            outString += '</div>';

            //
            // 隐藏值设置
            //
            outString += '<input id="id" name="id" type="hidden" value="' + x.isUndefined(param.id, '') + '"  x-dom-data-type="value" />';
            outString += '<input id="displayType" name="displayType" type="hidden" value="' + x.isUndefined(param.displayType, '') + '"  x-dom-data-type="value" />';
            outString += '<input id="rootTreeNodeId" name="rootTreeNodeId" type="hidden" value="' + x.isUndefined(param.rootTreeNodeId, '') + '"  x-dom-data-type="value" />';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:getPaging(currentPage)*/
        /**
         * 分页
         */
        getPaging: function(currentPage)
        {
            var paging = main.paging;

            paging.currentPage = currentPage;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += paging.toXml();
            outString += '</request>';

            x.net.xhr('/api/membership.catalog.query.aspx', outString, {
                waitingType: 'mini',
                waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var paging = main.paging;

                    var list = result.data;

                    paging.load(result.paging);

                    var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                                   + '<a href="javascript:main.openDialog();" class="btn btn-default" >新增</a>'
                                   + '</div>'
                                   + '<span>目录设置</span>'
                                   + '<div class="clearfix" ></div>';

                    $('#window-main-table-header')[0].innerHTML = headerHtml;

                    var containerHtml = main.getObjectsView(list, paging.pagingize);

                    $('#window-main-table-container')[0].innerHTML = containerHtml;

                    var footerHtml = paging.tryParseMenu('javascript:main.getPaging({0});');

                    $('#window-main-table-footer')[0].innerHTML = footerHtml;

                    // 显示查询框
                    $('.table-row-filter').show();

                    masterpage.resize();
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
            if (id === '')
            {
                url = '/api/membership.catalog.create.aspx';

                outString += '<action><![CDATA[createNewObject]]></action>';
            }
            else
            {
                url = '/api/membership.catalog.findOne.aspx';
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
                                   + '<span>目录设置</span>'
                                   + '<div class="clear"></div>';

                    $('#window-main-table-header')[0].innerHTML = headerHtml;

                    var containerHtml = main.getObjectView(param);

                    $('#window-main-table-container')[0].innerHTML = containerHtml;

                    var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

                    $('#window-main-table-footer')[0].innerHTML = footerHtml;

                    main.setObjectView(param);

                    // 隐藏查询框
                    $('.table-row-filter').hide();
                    $('#window-main-table-footer').hide();

                    x.ui.pkg.tabs.newTabs();

                    masterpage.resize();
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
            if (param.status === '1')
            {
                $('#status')[0].checked = 'checked';
            }
        },
        /*#endregion*/

        /*#region 函数:checkObject()*/
        /*
        * 检测对象的必填数据
        */
        checkObject: function()
        {
            if (x.dom.data.check()) { return false; }

            //if($('#name').val() === '')
            //{
            //    $('#name')[0].focus();
            //    alert('必须填写[名称]信息');
            //    return false;
            //}

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
                outString += x.dom.data.serialize({ storageType: 'xml', includeAjaxStorageNode: false })
                // outString += '<action><![CDATA[save]]></action>';
                // outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
                // outString += '<name><![CDATA[' + $("#name").val() + ']]></name>';
                // outString += '<remark><![CDATA[' + $("#remark").val() + ']]></remark>';
                // outString += '<orderId><![CDATA[' + $("#orderId").val() + ']]></orderId>';
                // outString += '<status><![CDATA[' + ($("#status")[0].checked ? '1' : '0') + ']]></status>';
                outString += '</request>';

                x.net.xhr('/api/membership.catalog.save.aspx', outString, {
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
            if (confirm('确定删除?'))
            {
                x.net.xhr('/api/membership.catalog.delete.aspx?id=' + id, {
                    waitingMessage: i18n.strings.msg_net_waiting_delete_tip_text,
                    callback: function(response)
                    {
                        main.membership.organization.list.getPaging(main.membership.organization.list.paging.currentPage);
                        main.getPaging(main.paging.currentPage);
                    }
                });
            }
        },
        /*#endregion*/

        /*#region 函数:load()*/
        /*
        * 页面加载事件
        */
        load: function()
        {
            main.filter();

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

    // 私有回调函数, 供子窗口回调
    window.window$refresh$callback = function()
    {
        main.filter();
    }

    // 注册到 Window 对象
    window.main = main;

    x.dom.ready(main.load);

}(x, window));
