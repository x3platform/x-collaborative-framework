x.register('main.tasks.category.list');

main.tasks.category.list = {

    paging: x.page.newPagingHelper(),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
        var whereClauseValue = ' Status IN (0, 1)';

        var searchText = $('#searchText').val();

        if(searchText != '')
        {
            whereClauseValue += ' AND CategoryIndex LIKE ##%' + x.toSafeLike(searchText) + '%##  ';
        }

        main.tasks.category.list.paging.whereClause = whereClauseValue;

        main.tasks.category.list.paging.orderBy = ' T.OrderId, T.UpdateDate DESC';

        main.tasks.category.list.getPaging(1);
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

        outString += '<table class="table-style" style="width:100%">';
        outString += '<tbody>';
        outString += '<tr class="table-row-title" >';
        outString += '<td >类别名称</td>';
        outString += '<td style="width:80px" >创建人</td>';
        outString += '<td style="width:30px" >状态</td>';
        outString += '<td style="width:80px" >更新时间</td>';
        outString += '<td class="end" style="width:40px" >删除</td>';
        outString += '</tr>';

        x.each(list, function(index, node)
        {
            classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

            outString += '<tr class="' + classNameValue + '" >';
            outString += '<td><a href="javascript:main.tasks.category.list.openDialog(\'' + node.id + '\');" target="_blank" >' + node.categoryIndex + '</a></td>';
            outString += '<td>' + node.accountName + '</td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            outString += '<td>' + node.updateDateView + '</td>';
            outString += '<td><a href="javascript:main.tasks.category.list.confirmDelete(\'' + node.id + '\');" >删除</a></td>';
            outString += '</tr>';

            counter++;
        });

        // 补全
        while(counter < maxCount)
        {
            var classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

            outString += '<tr class="' + classNameValue + '">';
            outString += '<td colspan="5" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
            outString += '</tr>';

            counter++;
        }

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
        var outString = '';

        outString += '<div class="ajax-tabs-wrapper">';
        outString += '<div class="ajax-tabs-menu-wrapper" >';
        outString += '<ul class="ajax-tabs-menu" >';
        outString += '<li><a href="#tab-1">基本信息</a></li>';
        outString += '</ul>';
        outString += '</div>';

        outString += '<div class="ajax-tabs-container" >';
        outString += '<h2 class="ajax-tabs-container-head-hidden" ><a id="tab-1" name="tab-1" >基本信息</a></h2>';
        outString += '<table class="table-style" style="width:100%" >';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" style="width:120px;" ><span class="required-text">类别索引</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="id" name="id" type="hidden" dataType="value" value="' + x.isUndefined(param.id, '') + '" />';
        outString += '<input id="accountId" name="accountId" type="hidden" dataType="value" value="' + x.isUndefined(param.accountId, '') + '" />';
        outString += '<input id="accountName" name="accountName" type="hidden" dataType="value" value="' + x.isUndefined(param.accountName, '') + '" />';
        outString += '<input id="categoryIndex" name="categoryIndex" type="text" dataType="value" value="' + x.isUndefined(param.categoryIndex, '') + '" dataVerifyWarning="【类别索引】必须填写。" class="input-normal custom-forms-data-required" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >描述信息</td>';
        outString += '<td class="table-body-input" >';
        outString += '<textarea id="description" name="description" type="text" dataType="value" style="width:460px; height:60px;" >' + x.isUndefined(param.description, '') + '</textarea> ';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >标签</td>';
        outString += '<td class="table-body-input" >';
        outString += '<textarea id="tags" name="tags" type="text" dataType="value" style="width:460px; height:60px;" >' + x.isUndefined(param.tags, '') + '</textarea> ';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >排序</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="orderId" name="orderId" type="text" dataType="value" value="' + x.isUndefined(param.orderId, '') + '" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="status" name="status" type="checkbox" dataType="value" value="' + x.isUndefined(param.status, '1') + '" feature="checkbox" class="checkbox-normal" />';
        outString += '</td>';
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
        var paging = main.tasks.category.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/task.category.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.msg.are_you_sure_you_want_to_delete,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.tasks.category.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.tasks.category.list.openDialog();" class="table-toolbar-button" >新增</a>'
                       + '</div>'
                       + '<span>类别设置</span>'
                       + '<div class="clearfix" ></div>';

                $('#window-main-table-header').html(headerHtml);

                var containerHtml = main.tasks.category.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.tasks.category.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                $('.table-row-filter').css({ display: '' });
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

        var url = '';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';

        var isNewObject = false;

        if(id === '')
        {
            url = '/api/task.category.create.aspx';

            isNewObject = true;
        }
        else
        {
            url = '/api/task.category.findOne.aspx';

            outString += '<id><![CDATA[' + id + ']]></id>';
        }

        outString += '</request>';

        x.net.xhr(url, outString, {
            waitingType: 'mini',
            waitingMessage: i18n.msg.are_you_sure_you_want_to_delete,
            callback: function(response)
            {
                var param = x.toJSON(response).data;

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.tasks.category.list.save();" class="table-toolbar-button-single-2font" >保存</a> '
                       + '<a href="javascript:main.tasks.category.list.getPaging(' + main.tasks.category.list.paging.currentPage + ');" class="table-toolbar-button-single-2font" >取消</a>'
                       + '</div>'
                       + '<span>类别设置</span>'
                       + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.tasks.category.list.getObjectView(param);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

                $('#window-main-table-footer')[0].innerHTML = footerHtml;

                $('.table-row-filter').css({ display: 'none' });

                x.form.features.bind();

                x.tabs.create();
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
        if(x.customForm.checkDataStorage()) { return false; }

        return true;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    /*
    * 保存对象
    */
    save: function()
    {
        if(main.tasks.category.list.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<ajaxStorage>';
            outString += x.customForm.getDataStorage({ storageType: 'xml', includeajaxStorageNode: false });
            outString += '</ajaxStorage>';

            x.net.xhr('/api/task.category.save.aspx', outString, {
                waitingMessage: i18n.net.commitTipText,
                popResultValue: 1,
                callback: function(response)
                {
                    main.tasks.category.list.getPaging(main.tasks.category.list.paging.currentPage);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:confirmDelete(id)*/
    confirmDelete: function(id)
    {
        if(confirm('您确定删除?'))
        {
            x.net.xhr('/api/task.category.delete.aspx?id=' + id, {
                waitingType: 'mini',
                waitingMessage: i18n.net.waiting.deleteTipText,
                callback: function(response)
                {
                    main.tasks.category.list.getPaging(main.tasks.category.list.paging.currentPage);
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
        main.tasks.category.list.filter();

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnFilter').bind('click', function()
        {
            main.tasks.category.list.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.tasks.category.list.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.tasks.category.list.filter();
}
