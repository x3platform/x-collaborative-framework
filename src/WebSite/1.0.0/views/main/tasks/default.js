x.register('main.tasks.home');

main.tasks.home = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
        main.tasks.home.paging.query.scence = 'Query';

        main.tasks.home.paging.query.where.Type = $('#type').val();
        main.tasks.home.paging.query.where.Status = $('#status').val();
        main.tasks.home.paging.query.where.SearchText = $('#searchText').val();
        main.tasks.home.paging.query.where.DateBegin = $('#dateBegin').val();
        main.tasks.home.paging.query.where.DateEnd = $('#dateEnd').val();

        if($('#type').val() === '2,4')
        {
            main.tasks.home.paging.query.orders = ' Status ASC, CreateDate DESC';
        }
        else
        {
            main.tasks.home.paging.query.orders = ' CreateDate DESC ';
        }

        main.tasks.home.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:getObjectsView(containerId, list, maxCount)*/
    /*
    * 创建对象列表的视图
    */
    getObjectsView: function(containerId, list, maxCount)
    {
        var outString = '';

        var counter = 0;

        var classNameValue = '';

        outString += '<div class="table-freeze-head">';
        outString += '<table class="table" >';
        outString += '<thead>';
        outString += '<tr>';
        outString += '<th style="width:30px" ><input id="' + containerId + 'SelectTask" name="' + containerId + 'SelectTask" type="checkbox" onclick="main.tasks.home.selectAll(\'' + containerId + '\',false);"/></th>';
        outString += '<th >标题</th>';
        outString += '<th style="width:60px" >状态</th>';
        outString += '<th style="width:100px" >发送时间</th>';
        outString += '<th class="table-freeze-head-padding" ></th>';
        outString += '</tr>';
        outString += '</thead>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="table-freeze-body">';
        outString += '<table class="table table-striped">';
        outString += '<colgroup>';
        outString += '<col style="width:30px" />';
        outString += '<col />';
        outString += '<col style="width:60px" />';
        outString += '<col style="width:100px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

            classNameValue = classNameValue + ((counter + 1) === maxCount ? '-transparent' : '');

            outString += '<tr class="' + classNameValue + '">';
            outString += '<td><input name="' + containerId + 'SelectTaskId" id="id_' + node.id + '" type="checkbox" value="' + node.id + '" /></td>';
            outString += '<td>';
            switch(node.type)
            {
                case '1':
                    outString += '<a href="/api/task.redirect.aspx?taskId=' + node.id + '&receiverId=' + node.receiverId + '&' + main.tasks.home.signature() + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[审批]</span>';
                    break;
                case '2':
                    outString += '<a href="/tasks/task-workitem/view?id=' + node.id + '&receiverId=' + node.receiverId + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[消息]</span>';
                    break;
                case '4':
                    outString += '<a href="/api/task.redirect.aspx?taskId=' + node.id + '&receiverId=' + node.receiverId + '&' + main.tasks.home.signature() + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[通知]</span>';
                    break;
                case '8':
                    outString += '<a href="/api/task.redirect.aspx?taskId=' + node.id + '&receiverId=' + node.receiverId + '&' + main.tasks.home.signature() + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[催办]</span>';
                    break;
                case '258':
                    outString += '<a href="/api/task.redirect.aspx?taskId=' + node.id + '&receiverId=' + node.receiverId + '&' + main.tasks.home.signature() + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[通知]</span>';
                    break;
                case '260':
                    outString += '<a href="/api/task.redirect.aspx?taskId=' + node.id + '&receiverId=' + node.receiverId + '&' + main.tasks.home.signature() + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[通知]</span>';
                    break;
                case '266':
                    outString += '<a href="/api/task.redirect.aspx?taskId=' + node.id + '&receiverId=' + node.receiverId + '&' + main.tasks.home.signature() + '" target="_blank" >' + node.title + '</a> ';
                    outString += '<span class="green-text">' + node.tags + '</span> ';
                    outString += '<span class="gray-text">[自动催办]</span>';
                    break;
            }
            outString += '</td>';
            // getStatusWizard
            // outString += '<td style="cursor:pointer;" onclick="main.tasks.home.getStatusWizard(\'' + node.id + '\');" >';
            outString += '<td style="cursor:pointer;" onclick="main.tasks.home.setStatus(\'' + node.id + '\', ' + (node.status === '0' ? 1 : 0) + ');" >';

            if(node.status === '0')
            {
                outString += '<span class="red-text" >未完成</span>';
            }
            else if(node.status === '1')
            {
                outString += '<span class="green-text" >已完成</span>';
            }
            else
            {
                outString += main.tasks.home.setStatusText(node.status);
            }

            outString += '</td>';

            outString += '<td>' + node.createDate + '</td>';
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

    /*#region 函数:getPaging(currentPage)*/
    /*
    * 分页
    */
    getPaging: function(currentPage)
    {
        var paging = main.tasks.home.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/task.workItem.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.tasks.home.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerId = x.dom('#containerId').val();

                var containerHtml = main.tasks.home.getObjectsView(containerId, list, paging.pageSize);

                x.dom('#' + containerId).html(containerHtml);

                var footerHtml = paging.tryParseMenu('main.tasks.home.getPaging');

                $('#window-main-table-footer').html(footerHtml);

                masterpage.resize();

                // 设置 确定按钮 样式
                $('#filterSubmitText').css('color', '');
                $('#filterSubmitText').css('cursor', 'pointer');
            }
        });
    },
    /*#endregion*/

    /*#region 函数:getStatusWizard(taskId)*/
    getStatusWizard: function(taskId)
    {
        x.wizards.getWizard('status', {

            taskId: taskId,

            save_callback: function()
            {
                main.tasks.home.setStatus(x.dom('#' + this.name + '-wizardTaskId').val(), x.dom('#' + this.name + '-wizardTaskStatusValue').val());

                return 0;
            },

            create: function()
            {
                var outString = '';

                var counter = 0;

                var list = x.toJSON($('#statusList').val());

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:260px; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>待办状态设置向导</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<table class="table-style" style="width:100%;">';
                outString += '<tr>';
                outString += '<td id="' + this.name + '$wizardTableContainer" class="table-body" >';

                outString += '<table class="table-style" style="width:100%">';
                outString += '<tbody>';
                outString += '<tr class="table-row-title">';
                outString += '<td >状态</td>';
                outString += '</tr>';

                var that = this;

                x.each(list, function(index, node)
                {
                    if(node.value !== '')
                    {
                        classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

                        classNameValue = classNameValue + ((counter + 2) === list.length ? '-transparent' : '');

                        outString += '<tr class="' + classNameValue + '">';
                        outString += '<td>';
                        outString += '<a href="javascript:x.dom(\'#' + that.name + '-wizardTaskStatusText\').val(\'' + node.text + '\');x.dom(\'#' + that.name + '-wizardTaskStatusValue\').val(\'' + node.value + '\'); void(0);" >' + node.text + '</a></li>';

                        outString += '</td>';
                        outString += '</tr>';

                        counter++;
                    }
                });

                outString += '</td>';
                outString += '</tr>';
                outString += '</table>';

                outString += ' <input id="' + this.name + '$wizardTaskId" name="wizardTaskId" type="hidden" value="' + taskId + '" />';

                outString += '<div class="winodw-wizard-result-container" >';
                outString += '<div class="winodw-wizard-result-item" ><span class="winodw-wizard-result-item-text" style="width:40px;" >状态</span></div>';
                outString += '<div class="winodw-wizard-result-item" ><input id="' + this.name + '$wizardTaskStatusText" name="wizardTaskStatusText" type="text" value="" class="winodw-wizard-result-item-input" style="width:100px;" /><input id="' + this.name + '$wizardTaskStatusValue" name="wizardTaskStatusValue" type="hidden" value="" /></div>';
                outString += '<div class="winodw-wizard-result-item" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div class="clear"></div>';
                outString += '</div>';

                return outString;
            }
        });
    },
    /*#endregion*/

    /*#region 函数:setStatus(taskId, status)*/
    setStatus: function(taskId, status)
    {
        if(confirm('是否强制更改当前的待办信息状态为【' + main.tasks.home.setStatusText(status) + '】?'))
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<ajaxStorage>';
            outString += '<taskId>' + taskId + '</taskId>';
            outString += '<status>' + status + '</status>';
            outString += '</ajaxStorage>';

            x.net.xhr('/api/task.workItem.setStatus.aspx', outString, {
                popCorrectValue: 0,
                callback: function(response)
                {
                    main.tasks.home.getPaging(main.tasks.home.paging.currentPage);

                    main.tasks.home.getUnfinishedQuantities(x.dom('#session-accountId').val(), 0);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:setStatusText(status)*/
    setStatusText: function(status)
    {
        var statusText = 'Unkown';

        var list = x.toJSON($('#statusList').val());

        x.each(list, function(index, node)
        {
            if(node.value === String(status))
            {
                statusText = node.text;
                return;
            }
        });

        return statusText;
    },
    /*#endregion*/

    /*#region 函数:getCategoryIndexWizard(categoryIndex)*/
    getCategoryIndexWizard: function(options)
    {
        x.wizards.getWizard('categoryIndex', {

            save_callback: function()
            {
                main.tasks.home.setStatus(x.dom('#' + this.name + '-wizardTaskId').val(), x.dom('#' + this.name + '-wizardTaskStatusValue').val());

                return 0;
            },

            create: function()
            {
                var outString = '';

                var counter = 0;

                // var list = x.toJSON($('#types').val());

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:260px; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>待办类别设置向导</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<table class="table-style" style="width:100%;">';
                outString += '<tr>';
                outString += '<td id="' + this.name + '$wizardTableContainer" class="table-body" >';

                outString += '<table class="table-style" style="width:100%">';
                outString += '<tbody>';
                outString += '<tr class="table-row-title">';
                outString += '<td >名称</td>';
                outString += '</tr>';
                /*
                var that = this;
        
                x.each(list, function(index, node)
                {
                if (node.value !== '')
                {
                classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';
        
                classNameValue = classNameValue + ((counter + 2) === list.length ? '-transparent' : '');
        
                outString += '<tr class="' + classNameValue + '">';
                outString += '<td>';
                outString += '<a href="javascript:x.dom(\'' + that.name + '$wizardTaskStatusText\').val(\'' + node.text + '\');x.dom(\'' + that.name + '$wizardTaskStatusValue\').val(\'' + node.value + '\'); void(0);" >' + node.text + '</a></li>';
        
                outString += '</td>';
                outString += '</tr>';
        
                counter++;
                }
                });
                */
                outString += '</table>';

                outString += '</td>';
                outString += '</tr>';
                outString += '</table>';

                // outString += ' <input id="' + this.name + '$wizardTaskId" name="wizardTaskId" type="hidden" value="' + taskId + '" />';

                outString += '<div class="winodw-wizard-result-container" >';
                outString += '<div class="winodw-wizard-result-item" ><span class="winodw-wizard-result-item-text" style="width:40px;" >名称</span></div>';
                outString += '<div class="winodw-wizard-result-item" ><input id="' + this.name + '$wizardTaskStatusText" name="wizardTaskStatusText" type="text" value="" class="winodw-wizard-result-item-input" style="width:100px;" /><input id="' + this.name + '$wizardTaskStatusValue" name="wizardTaskStatusValue" type="hidden" value="" /></div>';
                outString += '<div class="winodw-wizard-result-item" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div class="clear"></div>';
                outString += '</div>';

                return outString;
            }
        });
    },
    /*#endregion*/

    /*#region 函数:setFinished(containerId)*/
    setFinished: function(containerId)
    {
        var taskIds = '';

        var list = document.getElementsByName(containerId + 'SelectTaskId');

        for(var i = 0;i < list.length;i++)
        {
            if(list[i].checked)
            {
                taskIds += list[i].value + ',';
            }
        }

        taskIds = x.string.trim(taskIds, ',');

        if(confirm('是否强制更改当前所选勾选的待办信息状态为【已完成】?'))
        {
            x.net.xhr('/api/task.workItem.setFinished.aspx?taskIds=' + taskIds, {
                waitingType: 'mini',
                waitingMessage: i18n.net.waiting.commitTipText,
                popCorrectValue: 0,
                callback: function(response)
                {
                    main.tasks.home.getPaging(main.tasks.home.paging.currentPage);

                    main.tasks.home.getUnfinishedQuantities(x.dom('#session-accountId').val(), 0);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:setAllFinished()*/
    setAllFinished: function()
    {
        if(confirm('是否强制更改当前所有的待办信息状态为【已完成】?'))
        {
            x.net.xhr('/api/task.workItem.setFinished.aspx?random=' + x.randomText.create(8), {
                waitingType: 'mini',
                waitingMessage: i18n.net.waiting.commitTipText,
                popCorrectValue: 0,
                callback: function(response)
                {
                    main.tasks.home.getPaging(main.tasks.home.paging.currentPage);

                    main.tasks.home.getUnfinishedQuantities(x.dom('#session-accountId').val(), 0);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:selectAll(containerId, toggleHeader)*/
    /*
    * 全选
    */
    selectAll: function(containerId, toggleHeader)
    {
        if(toggleHeader)
        {
            x.dom('#' + containerId + 'SelectTask')[0].checked = !x.dom('#' + containerId + 'SelectTask')[0].checked;
        }

        x.form.checkbox.selectAll('#' + containerId + 'SelectTaskId', x.dom('#' + containerId + 'SelectTask')[0].checked);
    },
    /*#endregion*/

    /*#region 函数:selectInverse()*/
    /*
    * 反选
    */
    selectInverse: function(containerId)
    {
        x.dom('#' + containerId + 'SelectTask')[0].checked = !x.dom('#' + containerId + 'SelectTask')[0].checked;

        x.form.checkbox.selectInverse('#' + containerId + 'SelectTaskId');
    },
    /*#endregion*/

    /*#region 函数:setDateRange()*/
    /*
    * 反选
    */
    setDateRange: function(dateBegin, dateEnd, autoFilter)
    {
        x.dom('#dateBegin').val(dateBegin);
        x.dom('#dateEnd').val(dateEnd);

        if(autoFilter == 1)
        {
            main.tasks.home.filter();
        }
    },
    /*#endregion*/

    /*#region 函数:setTimeSpan()*/
    /*
    * 反选
    */
    setTimeSpan: function()
    {
        var timeSpan = x.dom('#timeSpan').val();

        if(timeSpan === '')
        {
            main.tasks.home.setDateRange('', '', 0);
        }
        else
        {
            var time = x.date.newTime(new Date());

            main.tasks.home.setDateRange(time.add('day', -(Number(timeSpan) + 1)).toString('yyyy-MM-dd 00:00:00'), '', 0);
        }
    },
    /*#endregion*/

    /*#region 函数:getTab(tabName)*/
    /*
    * 获取页签信息
    */
    getTab: function(tabName)
    {
        var tabIndex = 0;

        var containerId = '';

        var toolbarHtml = '';

        var filterbarHtml = '';

        var time = x.date.newTime(new Date());

        switch(tabName)
        {
            case '1':
                //【催办】页签
                tabIndex = 0;

                containerId = 'tab-' + (tabIndex + 1) + '-container';

                toolbarHtml += '<a href="javascript:main.tasks.home.selectAll(\'' + containerId + '\',true);" class="btn btn-default" >全选</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.selectInverse(\'' + containerId + '\');" class="btn btn-default" >反选</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.setFinished(\'' + containerId + '\');" class="btn btn-default" >打钩催办设置为已完成</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.setAllFinished();" class="btn btn-default" >所有待办设置为已完成</a> ';
                toolbarHtml += '<input id="searchText" name="searchText" type="hidden" value="" />';
                toolbarHtml += '<input id="status" name="status" type="hidden" value="0,2,3,4" />';
                toolbarHtml += '<input id="type" name="type" type="hidden" value="8,266" />';
                toolbarHtml += '<input id="dateBegin" name="dateBegin" type="hidden" value="" />';
                toolbarHtml += '<input id="dateEnd" name="dateEnd" type="hidden" value="" />';

                filterbarHtml += '<div>';
                filterbarHtml += '<div class="float-left" style="margin:5px 0 3px 0;" >';
                filterbarHtml += '<div style="float:left; margin:0 12px 0 0;" >';
                filterbarHtml += '<a href="javascript:main.tasks.home.setDateRange(\'\',\'\',1);" >全部</a> ';
                filterbarHtml += '</div>';
                filterbarHtml += '<div style="float:left; margin:0 12px 0 0;" >';
                filterbarHtml += '<a href="javascript:main.tasks.home.setDateRange(\'' + time.add('day', -4).toString('yyyy-MM-dd 00:00:00') + '\',\'\',1);" >最近3天</a> ';
                filterbarHtml += '</div>';
                filterbarHtml += '<div style="float:left; margin:0 12px 0 0;" >';
                filterbarHtml += '<a href="javascript:main.tasks.home.setDateRange(\'' + time.add('day', -8).toString('yyyy-MM-dd 00:00:00') + '\',\'\',1);" >最近7天</a> ';
                filterbarHtml += '</div>';
                filterbarHtml += '<div class="clearfix" ></div>';
                filterbarHtml += '</div>';
                /*
                filterbarHtml += '<div class="float-right" >';
                filterbarHtml += '<table class="table-row-filter-container" >';
                filterbarHtml += '<tr>';
                filterbarHtml += '<td >所属类别</td>';
                filterbarHtml += '<td ><input id="type" name="type" type="text" feature="combobox" topOffset="-1" class="input-normal" value="" selectedText="全部" data="${type}" style="width:120px" /> <a href="javascript:main.tasks.home.getCategoryIndexWizard(\'\');">编辑</a></td>';
                filterbarHtml += '<td class="table-row-filter-button" ><span id="btnFilter" onclick="main.tasks.home.filter();" >查询</span></td>';
                filterbarHtml += '</tr>';
                filterbarHtml += '</table>';
                filterbarHtml += '</div>';
                */
                filterbarHtml += '<div class="clearfix" ></div>';
                filterbarHtml += '</div>';

                break;

            case '2':
                //【审批】页签
                tabIndex = 1;

                containerId = 'tab-' + (tabIndex + 1) + '-container';

                toolbarHtml += '<a href="javascript:main.tasks.home.selectAll(\'' + containerId + '\',true);" class="btn btn-default" >全选</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.selectInverse(\'' + containerId + '\');" class="btn btn-default" >反选</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.setFinished(\'' + containerId + '\');" class="btn btn-default" >打钩审批设置为已完成</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.setAllFinished();" class="btn btn-default" >所有待办设置为已完成</a> ';
                toolbarHtml += '<input id="searchText" name="searchText" type="hidden" value="" />';
                toolbarHtml += '<input id="status" name="status" type="hidden" value="0,2,3,4" />';
                toolbarHtml += '<input id="type" name="type" type="hidden" value="1" />';
                toolbarHtml += '<input id="dateBegin" name="dateBegin" type="hidden" value="" />';
                toolbarHtml += '<input id="dateEnd" name="dateEnd" type="hidden" value="" />';

                filterbarHtml += '<div>';
                filterbarHtml += '<div class="float-left" style="margin:5px 0 3px 0;" >';
                filterbarHtml += '<div style="float:left; margin:0 12px 0 0;" >';
                filterbarHtml += '<a href="javascript:main.tasks.home.setDateRange(\'\',\'\',1);" >全部</a> ';
                filterbarHtml += '</div>';
                filterbarHtml += '<div style="float:left; margin:0 12px 0 0;" >';
                filterbarHtml += '<a href="javascript:main.tasks.home.setDateRange(\'' + time.add('day', -4).toString('yyyy-MM-dd 00:00:00') + '\',\'\',1);" >最近3天</a> ';
                filterbarHtml += '</div>';
                filterbarHtml += '<div style="float:left; margin:0 12px 0 0;" >';
                filterbarHtml += '<a href="javascript:main.tasks.home.setDateRange(\'' + time.add('day', -8).toString('yyyy-MM-dd 00:00:00') + '\',\'\',1);" >最近7天</a> ';
                filterbarHtml += '</div>';
                filterbarHtml += '<div class="clearfix" ></div>';
                filterbarHtml += '</div>';
                /*
                filterbarHtml += '<div class="float-right" >';
                filterbarHtml += '<table class="table-row-filter-container" >';
                filterbarHtml += '<tr>';
                filterbarHtml += '<td >所属类别</td>';
                filterbarHtml += '<td ><input id="type" name="type" type="text" feature="combobox" topOffset="-1" class="input-normal" value="" selectedText="全部" data="${type}" style="width:120px" /> <a href="">编辑</a></td>';
                filterbarHtml += '<td class="table-row-filter-button" ><span id="btnFilter" onclick="main.tasks.home.filter();" >查询</span></td>';
                filterbarHtml += '</tr>';
                filterbarHtml += '</table>';
                filterbarHtml += '</div>';
                */
                filterbarHtml += '<div class="clearfix" ></div>';
                filterbarHtml += '</div>';

                break;

            case '3':
                //【通知】页签
                tabIndex = 2;

                containerId = 'tab-' + (tabIndex + 1) + '-container';

                toolbarHtml += '<a href="javascript:main.tasks.home.selectAll(\'' + containerId + '\',true);" class="btn btn-default" >全选</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.selectInverse(\'' + containerId + '\');" class="btn btn-default" >反选</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.setFinished(\'' + containerId + '\');" class="btn btn-default" >打钩通知设置为已完成</a> ';
                toolbarHtml += '<a href="javascript:main.tasks.home.setAllFinished();" class="btn btn-default" >所有待办设置为已完成</a> ';
                toolbarHtml += '<input id="status" name="status" type="hidden" value="" />';
                toolbarHtml += '<input id="type" name="type" type="hidden" value="2,4" />';
                toolbarHtml += '<input id="dateBegin" name="dateBegin" type="hidden" value="" />';
                toolbarHtml += '<input id="dateEnd" name="dateEnd" type="hidden" value="" />';

                filterbarHtml += '<table class="table-row-filter-container" >';
                filterbarHtml += '<tr>';
                filterbarHtml += '<td >关键字</td>';
                filterbarHtml += '<td ><input id="searchText" name="searchText" type="text" class="input-normal" value="" /></td>';
                // filterbarHtml += '<td >所属类别</td>';
                // filterbarHtml += '<td ><input id="category" name="type" type="text" feature="combobox" topOffset="-1" class="input-normal" value="" selectedText="全部" data="${type}" style="width:120px" /> <a href="">编辑</a></td>';
                filterbarHtml += '<td >发送时间</td>';
                filterbarHtml += '<td ><input id="timeSpan" name="timeSpan" type="text" value="" feature="combobox" selectedtext="" topOffset="-1" url="/api/application.setting.getCombobox.aspx" comboboxEmptyItemText="全部" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_任务管理_工作项查询天数范围## ) " callback="main.tasks.home.setTimeSpan()" class="input-normal" style="width:100px"  /></td>';
                filterbarHtml += '<td class="table-row-filter-button" ><span id="btnFilter" onclick="main.tasks.home.filter();" >查询</span></td>';
                filterbarHtml += '</tr>';
                filterbarHtml += '</table>';

                break;

            case '4':
                //【已完成审批】页签
                tabIndex = 3;

                containerId = 'tab-' + (tabIndex + 1) + '-container';

                toolbarHtml += '<input id="status" name="status" type="hidden" value="1" />';
                toolbarHtml += '<input id="type" name="type" type="hidden" value="1" />';
                toolbarHtml += '<input id="dateBegin" name="dateBegin" type="hidden" value="" />';
                toolbarHtml += '<input id="dateEnd" name="dateEnd" type="hidden" value="" />';

                filterbarHtml += '<table class="table-row-filter-container" >';
                filterbarHtml += '<tr>';
                filterbarHtml += '<td >关键字</td>';
                filterbarHtml += '<td ><input id="searchText" name="searchText" type="text" class="input-normal" value="" /></td>';
                // filterbarHtml += '<td >所属类别</td>';
                // filterbarHtml += '<td ><input id="type" name="type" type="text" feature="combobox" topOffset="-1" class="input-normal" value="" selectedText="全部" data="${type}" style="width:120px" /> <a href="">编辑</a></td>';
                filterbarHtml += '<td >发送时间</td>';
                filterbarHtml += '<td ><input id="timeSpan" name="timeSpan" type="text" value="" feature="combobox" selectedtext="" topOffset="-1" url="/api/application.setting.getCombobox.aspx" comboboxEmptyItemText="全部" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_任务管理_工作项查询天数范围## ) " callback="main.tasks.home.setTimeSpan()" class="input-normal" style="width:100px"  /></td>';
                filterbarHtml += '<td class="table-row-filter-button" ><span id="btnFilter" onclick="main.tasks.home.filter();" >查询</span></td>';
                filterbarHtml += '</tr>';
                filterbarHtml += '</table>';

                break;
        }

        $('.x-ui-pkg-tabs-menu li').removeClass('active');
        $('.x-ui-pkg-tabs-container').css('display', 'none');

        // 设置当前页签信息
        $($('.x-ui-pkg-tabs-menu li')[tabIndex]).addClass('active');
        $($('.x-ui-pkg-tabs-container')[tabIndex]).css('display', '');

        $('#containerId').val(containerId);

        $('#toolbar').html(toolbarHtml);
        $('#filterbarHtml').html(filterbarHtml);

        main.tasks.home.filter();

        x.dom.features.bind();
    },
    /*#endregion*/

    /*#region 函数:getUnfinishedQuantities(receiverId, autoNavTab)*/
    getUnfinishedQuantities: function(receiverId, autoNavTab)
    {
        x.net.xhr('/api/task.workItem.getUnfinishedQuantities.aspx?receiverId=' + receiverId, {
            waitingType: 'mini',
            waitingMessage: i18n.msg.are_you_sure_you_want_to_delete,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var list = result.data;

                // 设置未完成数量
                var tabUnfinishedQuantities = [0, 0, 0];

                for(var i = 0;i < list.length;i++)
                {
                    if(list[i].key == "8" || list[i].key == "266")
                    {
                        tabUnfinishedQuantities[0] += Number(list[i].value);
                    }

                    if(list[i].key == "1")
                    {
                        tabUnfinishedQuantities[1] += Number(list[i].value);
                    }

                    if(list[i].key == "2" || list[i].key == "4")
                    {
                        tabUnfinishedQuantities[2] += Number(list[i].value);
                    }
                }

                $($('.x-ui-pkg-tabs-menu li')[0].childNodes[0]).html('催办' + (tabUnfinishedQuantities[0] == 0 ? '' : (tabUnfinishedQuantities[0] > 99 ? '(99+)' : '(' + tabUnfinishedQuantities[0] + ')'))).css({ 'color': '#b92f2f' });

                $($('.x-ui-pkg-tabs-menu li')[1].childNodes[0]).html('审批' + (tabUnfinishedQuantities[1] == 0 ? '' : (tabUnfinishedQuantities[1] > 99 ? '(99+)' : '(' + tabUnfinishedQuantities[1] + ')'))).css({ 'color': '#b92f2f' });

                $($('.x-ui-pkg-tabs-menu li')[2].childNodes[0]).html('通知' + (tabUnfinishedQuantities[2] == 0 ? '' : (tabUnfinishedQuantities[2] > 99 ? '(99+)' : '(' + tabUnfinishedQuantities[2] + ')')));

                if(autoNavTab == 1)
                {
                    // 跳转至正确的页签
                    if(tabUnfinishedQuantities[0] > 0)
                    {
                        // 查看催办页签
                        main.tasks.home.getTab('1');
                    }
                    else if(tabUnfinishedQuantities[1] > 0)
                    {
                        // 查看审批页签
                        main.tasks.home.getTab('2');
                    }
                    else
                    {
                        // 查看通知页签
                        main.tasks.home.getTab('3');
                    }
                }
            }
        });
    },
    /*#endregion*/

    signature: function()
    {
        return 'clientId=' + x.dom('#session-client-id').val() + '&clientSignature=' + x.dom('#session-client-signature').val() + '&timestamp=' + x.dom('#session-timestamp').val() + '&nonce=' + x.dom('#session-nonce').val()
    },

    /*#region 函数:load()*/
    /*
    * 页面加载事件
    */
    load: function()
    {
        main.tasks.home.getUnfinishedQuantities(x.dom('#session-accountId').val(), 1);

        // 两分钟刷新一次待办的未读数
        var timer = x.newTimer(120, function(timer)
        {
            x.debug.log(x.date.newTime(new Date()).toString() + ' 正在读取待办未完成统计信息.');

            main.tasks.home.getUnfinishedQuantities(x.dom('#session-accountId').val(), 0);
        });

        timer.start();

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnFilter').bind('click', function()
        {
            main.tasks.home.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.tasks.home.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.tasks.home.filter();
}
