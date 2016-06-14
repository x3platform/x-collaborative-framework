x.register('main.membership.account.list');

main.membership.account.list = {

    paging: x.page.newPagingHelper(50),

    maskWrapper: x.ui.mask.newMaskWrapper('main.membership.account.list.maskWrapper'),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
        var searchText = $('#searchText').val().trim();

        if (searchText !== '')
        {
            // whereClauseValue = ' ( T.Name LIKE ##%' + x.toSafeLike(key) + '%## OR T.GlobalName LIKE ##%' + x.toSafeLike(key) + '%## OR T.LoginName LIKE ##%' + x.toSafeLike(key) + '%## ) ';
            main.membership.account.list.paging.query.scence = 'Query';
            main.membership.account.list.paging.query.where.SearchText = x.toSafeLike(searchText);
        }

        main.membership.account.list.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:getObjectsView(list, maxCount)*/
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
        outString += '<th >姓名(登录名)</th>';
        outString += '<th style="width:100px" >全局名称</th>';
        outString += '<th style="width:50px" >状态</th>';
        outString += '<th style="width:100px" >更新日期</th>';
        outString += '<th style="width:100px" >数据验证</th>';
        outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
        outString += '<th class="table-freeze-head-padding" ></th>';
        outString += '</tr>';
        outString += '</thead>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="table-freeze-body">';
        outString += '<table class="table table-striped">';
        outString += '<colgroup>';
        outString += '<col />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:50px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:100px" />';
        outString += '<col style="width:30px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td><a href="javascript:main.membership.account.list.openDialog(\'' + node.id + '\');" >' + node.name + (node.loginName == '' ? '</a> <span class="red-text">待初始化帐号</span>' : '(' + node.loginName + ')</a>') + '</td>';
            outString += '<td>' + node.globalName + '</td>';
            outString += '<td>' + x.app.setColorStatusView(node.status) + '</td>';
            // outString += '<td>' + x.date.newTime(node.loginDate).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
            outString += '<td>' + node.modifiedDateView + '</td>';
            outString += '<td><a href="/membership/account/validator?accountId=' + node.id + '" target="_blank" >数据验证</a></td>';
            if (node.locking === '1')
            {
                outString += '<td><span class="gray-text" title="删除" ><i class="fa fa-trash" ></i></span></td>';
            }
            else
            {
                outString += '<td><a href="javascript:main.membership.account.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
            }
            outString += '</tr>';

            counter++;
        });

        // 补全

        while (counter < maxCount)
        {
            outString += '<tr><td colspan="6" >&nbsp;</td></tr>';

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
        outString += '<li><a href="#tab-2">岗位信息</a></li>';
        outString += '<li><a href="#tab-3">角色信息</a></li>';
        outString += '<li><a href="#tab-4">群组信息</a></li>';
        outString += '<li><a href="#tab-5">操作日志</a></li>';
        outString += '<li><a href="#tab-6">备注</a></li>';
        outString += '</ul>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden" ><a id="tab-1" name="tab-1" >基本信息</a></h2>';
        outString += '<table class="table-style" style="width:100%" >';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" style="width:120px;" ><span class="required-text">编号</span></td>';
        outString += '<td class="table-body-input" style="width:160px" >';
        outString += '<input id="id" name="id" type="hidden" value="' + x.isUndefined(param.id, '') + '" />';
        // outString += '<input id="password" name="password" type="hidden" value="' + (typeof (param.password) === 'undefined' ? '' : param.password) + '" />';
        outString += '<input id="ip" name="ip" type="hidden" value="' + x.isUndefined(param.ip, '') + '" />';
        outString += '<input id="distinguishedName" name="distinguishedName" type="hidden" value="' + x.isUndefined(param.distinguishedName, '') + '" />';
        outString += '<input id="loginDate" name="loginDate" type="hidden" value="' + x.isUndefined(param.loginDateTimestampView, '') + '" />';
        if (typeof (param.code) === 'undefined' || param.code === '')
        {
            outString += '<span class="gray-text">自动编号</span>';
            outString += '<input id="code" name="code" type="hidden" value="" />';
        }
        else
        {
            outString += '<input id="code" name="code" type="text" value="' + x.isUndefined(param.code, '') + '" dataVerifyWarning="【编号】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" />';
        }
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px;" ><span class="required-text">姓名</span></td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="originalName" name="originalName" type="hidden" value="' + x.isUndefined(param.name, '') + '" />';
        outString += '<input id="name" name="name" type="text" value="' + x.isUndefined(param.name, '') + '" dataVerifyWarning="【姓名】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" ><span class="required-text">登录名</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="loginName" name="loginName" type="text" value="' + x.isUndefined(param.loginName, '') + '" dataVerifyWarning="【登录名】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" /> ';

        if (typeof (param.loginName) === 'undefined' || param.loginName === '')
        {
            outString += '<a href="javascript:main.membership.account.list.setLoginName();">编辑</a>';
        }

        outString += '</td>';
        outString += '<td class="table-body-text" >显示名称</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="displayName" name="displayName" type="text" value="' + x.isUndefined(param.displayName, '') + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" ><span class="required-text">全局名称</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="originalGlobalName" name="originalGlobalName" type="hidden" value="' + x.isUndefined(param.globalName, '') + '" />';
        outString += '<input id="globalName" name="globalName" type="text" value="' + x.isUndefined(param.globalName, '') + '" dataVerifyWarning="【全局名称】必须填写。" class="form-control custom-forms-data-required" style="width:120px;" />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px;" >拼音</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="pinyin" name="pinyin" type="text" value="' + x.isUndefined(param.pinyin, '') + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >身份证</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="identityCard" name="identityCard" type="text" value="' + x.isUndefined(param.identityCard, '') + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '<td class="table-body-text" >帐号类型</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="type" name="type" type="text" value="' + x.isUndefined(param.type, '') + '" x-dom-feature="combobox" selectedText="' + x.isUndefined(param.typeView, '') + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_帐号管理_帐号类别## ) " class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >绑定的电话</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="certifiedTelephone" name="certifiedTelephone" type="text" value="' + x.isUndefined(param.certifiedTelephone, '') + '" class="form-control" style="width:120px;" /> ';
        outString += '</td>';
        outString += '<td class="table-body-text" >绑定的邮箱</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="certifiedEmail" name="certifiedEmail" type="text" value="' + x.isUndefined(param.certifiedEmail, '') + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >排序</td>';
        outString += '<td class="table-body-input">';
        outString += '<input id="orderId" name="orderId" type="text" value="' + x.isUndefined(param.orderId, '') + '" class="form-control" style="width:120px;" />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px" >启用企业邮箱</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="enableExchangeEmail" name="enableExchangeEmail" type="checkbox" value="' + x.isUndefined(param.enableExchangeEmail, '') + '" class="checkbox-normal" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >启用</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="status" name="status" type="checkbox" value="' + x.isUndefined(param.status, '') + '" class="checkbox-normal" />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px" >防止意外删除</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="lock" name="lock" type="checkbox" value="' + x.isUndefined(param.lock, '') + '" class="checkbox-normal" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >更新时间</td>';
        outString += '<td class="table-body-input" >';
        outString += x.date.newTime(param.modifiedDate).toString('yyyy-MM-dd HH:mm:ss');
        outString += '<input id="modifiedDate" name="modifiedDate" type="hidden" dataType="value" value="' + x.isUndefined(param.modifiedDateTimestampView, '') + '" />';
        outString += '</td>';
        outString += '<td class="table-body-text" >创建时间</td>';
        outString += '<td class="table-body-input" >';
        outString += x.date.newTime(param.createDate).toString('yyyy-MM-dd HH:mm:ss');
        outString += '<input id="createDate" name="createDate" type="hidden" dataType="value" value="' + x.isUndefined(param.createDateTimestampView, '') + '" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '</table>';

        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-2" id="tab-2">岗位信息</a></h2>';

        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px;" >默认所属部门</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += '<textarea id="organizationPathView" name="organizationPathView" type="text" readonly="readonly" class="textarea-normalarea" style="width:432px; height:40px;" ></textarea>';
        outString += '<input id="organizationId" id="organizationId" type="hidden" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px;" >职务</td>';
        outString += '<td class="table-body-input" style="width:160px;" >';
        outString += '<input id="headship" name="headship" type="text" readonly="readonly" class="form-control" style="width:120px; " />';
        outString += '</td>';
        outString += '<td class="table-body-text" style="width:120px;" >职级</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="jobGradeDisplayName" name="jobGradeDisplayName" type="text" readonly="readonly" class="form-control" style="width:120px; " />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >主职岗位</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="assignedJobName" name="assignedJobName" type="text" readonly="readonly" class="form-control" style="width:120px; " />';
        outString += '</td>';
        outString += '<td class="table-body-text" >职位</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="jobName" name="jobName" type="text" readonly="readonly" class="form-control" style="width:120px; " />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >兼职岗位</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += '<textarea id="partTimeJobsView" name="partTimeJobsView" type="text" readonly="readonly" class="textarea-normal" style="width:432px; height:80px;" ></textarea>';
        outString += '<input id="partTimeJobsText" name="partTimeJobsText" type="hidden"/>';
        outString += '</td>';
        outString += '</tr>';

        outString += '</table>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-3" id="tab-3">角色信息</a></h2>';

        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" >所属组织</td>';
        outString += '<td class="table-body-input" >';
        outString += '<textarea id="organizationView" name="organizationView" type="text" class="textarea-normalarea" style="width:460px; height:80px;" >' + x.isUndefined(param.organizationView, '') + '</textarea>';
        outString += '<input id="organizationText" name="organizationText" type="hidden" value="' + x.isUndefined(param.organizationText, '') + '" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px;">所属角色</td>';
        outString += '<td class="table-body-input" >';
        outString += '<textarea id="roleView" name="roleView" type="text" class="textarea-normalarea" style="width:460px; height:80px;" >' + x.isUndefined(param.roleView, '') + '</textarea>';
        outString += '<input id="roleText" name="roleText" type="hidden" value="' + x.isUndefined(param.roleText, '') + '" />';
        outString += '<div class="text-right" style="width:460px; "><a href="javascript:x.ui.wizards.getContactsWizard({targetViewName:\'roleView\', targetValueName:\'roleText\', contactTypeText:\'role\'});" >编辑</a></div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-4" id="tab-4">群组信息</a></h2>';

        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px;">所属群组</td>';
        outString += '<td class="table-body-input" >';
        outString += '<textarea id="groupView" name="groupView" type="text" class="textarea-normalarea" style="width:460px; height:80px;" >' + x.isUndefined(param.groupView, '') + '</textarea>';
        outString += '<input id="groupText" name="groupText" type="hidden" value="' + x.isUndefined(param.groupText, '') + '" />';
        outString += '<div class="text-right" style="width:460px; "><a href="javascript:x.ui.wizards.getContactsWizard({targetViewName:\'groupView\', targetValueName:\'groupText\', contactTypeText:\'group\'});" >编辑</a></div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';
        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-5" id="tab-5">操作日志</a></h2>';

        outString += '<table id="windowAccountLogTable" class="table-style" style="width:100%;" >';
        outString += '<tr>';
        outString += '<td id="windowAccountLogTableContainer" ></td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '</div>';

        outString += '<div class="x-ui-pkg-tabs-container" >';
        outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="tab-6" id="tab-6">备注</a></h2>';

        outString += '<table class="table-style" style="width:100%">';

        outString += '<tr class="table-row-normal-transparent">';
        outString += '<td class="table-body-text" style="width:120px;">备注</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += '<textarea id="remark" name="remark" type="text" class="textarea-normalarea" style="width:460px; height:80px;" >' + x.isUndefined(param.remark, '') + '</textarea>';
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
        var paging = main.membership.account.list.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/membership.account.query.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.membership.account.list.paging;

                var list = result.data;

                paging.load(result.paging);

                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.membership.account.list.openDialog();" class="btn btn-default" >新增</a>'
                       + '</div>'
                       + '<span>帐号管理</span>'
                       + '<div class="clearfix" ></div>';

                $('#window-main-table-header').html(headerHtml);

                var containerHtml = main.membership.account.list.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.membership.account.list.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

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

        var url = '';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';

        var isNewObject = false;

        if (id === '')
        {
            url = '/api/membership.account.create.aspx';

            isNewObject = true;
        }
        else
        {
            url = '/api/membership.account.findOne.aspx';

            outString += '<id><![CDATA[' + id + ']]></id>';
        }

        outString += '</request>';

        x.net.xhr(url, outString, {
            waitingType: 'mini',
            waitingMessage: i18n.strings.msg_net_waiting_query_tip_text,
            callback: function(response)
            {
                var param = x.toJSON(response).data;

                //        var headerHtml = '<div>'
                //                       + '<div class="float-right">'
                //                       + '<a href="javascript:main.membership.account.list.setPassword(x.randomText.create(8));">重置密码</a> '
                //                       + '<a href="javascript:main.membership.account.list.save();" >保存</a> '
                //                       + '<a href="javascript:main.membership.account.list.getPaging(' + main.membership.account.list.paging.currentPage + ');" >取消</a>'
                //                       + '</div>'
                //                       + '<div>帐号</div>'
                //                       + '<div class="clear"></div>'
                //                       + '</div>';

                // 龙湖默认密码为六个八
                var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.membership.account.list.setPassword(\'888888\');" class="btn btn-default" >重置密码</a> '
                       + '<a href="javascript:main.membership.account.list.save();" class="btn btn-default" >保存</a> '
                       + '<a href="javascript:main.membership.account.list.getPaging(' + main.membership.account.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                       + '</div>'
                       + '<span>帐号信息</span>'
                       + '<div class="clear"></div>';

                $('#window-main-table-header')[0].innerHTML = headerHtml;

                var containerHtml = main.membership.account.list.getObjectView(param);

                $('#window-main-table-container')[0].innerHTML = containerHtml;

                // var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

                // $('#window-main-table-footer')[0].innerHTML = footerHtml;

                main.membership.account.list.setObjectView(param);

                // -------------------------------------------------------
                // 读取所属人员信息
                // -------------------------------------------------------

                if (!isNewObject)
                {
                    main.membership.account.list.getMemberByAccountId(param.id);
                }

                // -------------------------------------------------------
                // 读取所属帐号日志信息
                // -------------------------------------------------------

                main.membership.account.list.accountLog.getPaging(param.id, 1);

                x.dom.features.bind();

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
        // x.util.readonly('certifiedTelephone');
        // x.util.readonly('certifiedEmail');

        // .util.readonly('loginDate');
        // x.util.readonly('modifiedDate');

        if (!(typeof (param.id) === 'undefined' || param.id === '0' || param.loginName === ''))
        {
            ///x.util.readonly('loginName');
        }

        if (param.enableExchangeEmail === '1')
        {
            $('#enableExchangeEmail')[0].checked = true;
        }

        if (param.lock === '1')
        {
            $('#lock')[0].checked = true;
        }

        if (param.status === '1')
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
        if (x.dom.data.check()) { return false; }

        return true;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    /*
    * 保存对象
    */
    save: function()
    {
        if (main.membership.account.list.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<request>';
            outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
            outString += '<code><![CDATA[' + $("#code").val() + ']]></code>';
            outString += '<loginName><![CDATA[' + $("#loginName").val() + ']]></loginName>';
            outString += '<name><![CDATA[' + $("#name").val() + ']]></name>';
            outString += '<displayName><![CDATA[' + $("#displayName").val() + ']]></displayName>';
            outString += '<globalName><![CDATA[' + $("#globalName").val() + ']]></globalName>';
            outString += '<pinyin><![CDATA[' + $("#pinyin").val() + ']]></pinyin>';
            outString += '<identityCard><![CDATA[' + $("#identityCard").val() + ']]></identityCard>';
            outString += '<enableExchangeEmail><![CDATA[' + ($("#enableExchangeEmail")[0].checked ? '1' : '0') + ']]></enableExchangeEmail>';
            outString += '<lock><![CDATA[' + ($("#lock")[0].checked ? '1' : '0') + ']]></lock>';
            outString += '<orderId><![CDATA[' + $("#orderId").val() + ']]></orderId>';
            outString += '<status><![CDATA[' + ($("#status")[0].checked ? '1' : '0') + ']]></status>';
            outString += '<remark><![CDATA[' + $("#remark").val() + ']]></remark>';
            outString += '<ip><![CDATA[' + $("#ip").val() + ']]></ip>';
            outString += '<loginDate><![CDATA[' + $("#loginDate").val() + ']]></loginDate>';
            outString += '<modifiedDate><![CDATA[' + $("#modifiedDate").val() + ']]></modifiedDate>';
            outString += '<organizationText><![CDATA[' + $("#organizationText").val() + ']]></organizationText>';
            outString += '<roleText><![CDATA[' + $("#roleText").val() + ']]></roleText>';
            outString += '<groupText><![CDATA[' + $("#groupText").val() + ']]></groupText>';
            outString += '<originalName><![CDATA[' + $("#originalName").val() + ']]></originalName>';
            outString += '<originalGlobalName><![CDATA[' + $("#originalGlobalName").val() + ']]></originalGlobalName>';
            outString += '</request>';

            x.net.xhr('/api/membership.account.save.aspx', outString, {
                callback: function(response)
                {
                    main.membership.account.list.getPaging(main.membership.account.list.paging.currentPage);
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
            x.net.xhr('/api/membership.account.delete.aspx?id=' + id, {
                callback: function(response)
                {
                    main.membership.account.list.getPaging(main.membership.account.list.paging.currentPage);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:setPassword(password)*/
    /*
    * 重置帐号密码
    */
    setPassword: function(password)
    {
        if (confirm('确定将此帐号密码重置为[' + password + ']?'))
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<request>';
            outString += '<action><![CDATA[setPassword]]></action>';
            outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
            outString += '<password><![CDATA[' + CryptoJS.SHA1(password).toString() + ']]></password>';
            outString += '</request>';

            x.net.xhr('/api/membership.account.setPassword.aspx', outString, {
                callback: function(response)
                {
                    var result = x.toJSON(response).message;

                    switch (Number(result.returnCode))
                    {
                        case 0:
                            alert(result.value);
                            break;

                        case -1:
                        case 1:
                            alert(result.value);
                            break;

                        default:
                            break;
                    }
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:setLoginName()*/
    /*
    * 重置帐号登录名
    */
    setLoginName: function()
    {
        if (confirm('确定设置此用户的登录名为[' + $('#loginName').val() + ']?'))
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<request>';
            outString += '<action><![CDATA[setLoginName]]></action>';
            outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
            outString += '<loginName><![CDATA[' + $("#loginName").val() + ']]></loginName>';
            outString += '</request>';

            var options = {
                resultType: 'json',
                xml: outString
            };

            $.post(main.membership.account.list.url,
                   options,
                   main.membership.account.list.setLoginName_callback);
        }
    },

    setLoginName_callback: function(response)
    {
        x.net.fetchException(response);

        var result = x.toJSON(response).message;

        switch (Number(result.returnCode))
        {
            case 0:
                main.membership.account.list.openDialog($('#id').val());
                break;

            case 1:
            case -1:
                alert(result.value);
                break;
            default:
                alert(result.value);
                break;
        }
    },
    /*#endregion*/

    /*#region 函数:getMemberByAccountId(value)*/
    /*
    * 获取帐号的用户信息
    */
    getMemberByAccountId: function(value)
    {
        var accountId = (typeof (value) === 'undefined') ? '' : value;

        x.net.xhr('/api/membership.member.findOneByAccountId.aspx?accountId=' + accountId, {
            callback: function(response)
            {
                var param = x.toJSON(response).data;

                $('#organizationPathView').val(param.corporationName
                    + (param.departmentName === '' ? param.departmentName : '\\' + param.departmentName)
                    + (param.department2Name === '' ? param.department2Name : '\\' + param.department2Name)
                    + (param.department3Name === '' ? param.department3Name : '\\' + param.department3Name));

                $('#headship').val(param.headship);
                $('#jobGradeDisplayName').val(param.jobGradeDisplayName);
                $('#jobName').val(param.jobName);
                $('#assignedJobName').val(param.assignedJobName);
                $('#partTimeJobsView').val(param.partTimeJobsView);
            }
        });
    },
    /*#endregion*/

    /*#region 函数:getTreeView(organizationId)*/
    /*
    * 获取树形菜单
    */
    getTreeView: function(organizationId)
    {
        var treeViewType = 'organization';
        var treeViewId = '10000000-0000-0000-0000-000000000000';
        var treeViewName = '组织架构';
        var treeViewRootTreeNodeId = organizationId; // 默认值:'00000000-0000-0000-0000-000000000001'
        var treeViewUrl = 'javascript:main.membership.account.list.setTreeViewNode(\'{treeNodeId}\')';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<action><![CDATA[getDynamicTreeView]]></action>';
        outString += '<treeViewType><![CDATA[' + treeViewType + ']]></treeViewType>';
        outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
        outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
        outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
        outString += '<tree><![CDATA[{tree}]]></tree>';
        outString += '<parentId><![CDATA[{parentId}]]></parentId>';
        outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
        outString += '</request>';

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.membership.account.list.tree', ajaxMode: true });

        tree.add({
            id: "0",
            parentId: "-1",
            name: treeViewName,
            url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
            title: treeViewName,
            target: '',
            icon: '/resources/images/tree/tree_icon.gif'
        });

        tree.load('/api/membership.contacts.getDynamicTreeView.aspx', false, outString);

        main.membership.account.list.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        // var whereClauseValue = ' T.Id IN ( SELECT AccountId FROM tb_Account_Role WHERE RoleId IN ( SELECT Id FROM tb_Role WHERE OrganizationUnitId =  ##' + value + '## ) ) ';

        // main.membership.account.list.paging.whereClause = whereClauseValue;

        main.membership.account.list.paging.query.scence = 'QueryByOrganizationUnitId';
        main.membership.account.list.paging.query.where.OrganizationUnitId = value;
        main.membership.account.list.getPaging(1);
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /*
    * 页面加载事件
    */
    load: function()
    {
        // 正常加载
        var organizationId = '00000000-0000-0000-0000-000000000001';

        main.membership.account.list.getTreeView(organizationId);

        main.membership.account.list.setTreeViewNode(organizationId);

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#searchText').bind('keyup', function()
        {
            main.membership.account.list.filter();
        });

        $('#btnFilter').bind('click', function()
        {
            main.membership.account.list.filter();
        });
    },
    /*#endregion*/

    /**
     * 帐号操作日志
     */
    accountLog: {

        paging: x.page.newPagingHelper(),

        /*#region 函数:getPaging(accountId, currentPage)*/
        /*
        * 分页
        */
        getPaging: function(accountId, currentPage)
        {
            var paging = main.membership.account.list.accountLog.paging;

            paging.whereClause = ' AccountId = ##' + accountId + '## ';
            paging.currentPage = currentPage;
            paging.pageSize = 8;

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += paging.toXml();
            outString += '</request>';

            x.net.xhr('/api/membership.accountLog.query.aspx', outString, {
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var paging = main.membership.account.list.accountLog.paging;

                    var list = result.data;

                    paging.load(result.paging);

                    var outString = '';

                    var maxCount = paging.pageSize;

                    var counter = 0;

                    var classNameValue = '';

                    outString += '<table class="table-style" style="width:100%">';
                    outString += '<tbody>';
                    outString += '<tr class="table-row-title">';
                    outString += '<td >日志内容</td>';
                    outString += '<td style="width:120px" >时间</td>';
                    outString += '<td class="end" style="width:40px" >删除</td>';
                    outString += '</tr>';

                    x.each(list, function(index, node)
                    {
                        classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

                        outString += '<tr class="' + classNameValue + '">';
                        outString += '<td>' + node.description + '</td>';
                        outString += '<td>' + x.date.newTime(node.date).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
                        outString += '<td><a href="javascript:main.membership.account.list.accountLog.confirmDelete(\'' + node.id + '\');">删除</a></td>';
                        outString += '</tr>';

                        counter++;
                    });

                    // 补全

                    while (counter < maxCount)
                    {
                        classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

                        outString += '<tr class="' + classNameValue + '">';
                        outString += '<td colspan="6" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
                        outString += '</tr>';

                        counter++;
                    }

                    outString += '<tr class="table-row-alternating-transparent">';
                    outString += '<td colspan="6" >' + paging.tryParseMenu('javascript:main.membership.account.list.accountLog.getPaging(\'' + accountId + '\',{0});') + '</td>';
                    outString += '</tr>';

                    outString += '</tbody>';
                    outString += '</table>';

                    $('#windowAccountLogTableContainer').html(outString);
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
            if (confirm('确定删除?'))
            {
                x.net.xhr('/api/membership.accountLog.delete.aspx?id=' + id, {
                    callback: function(response)
                    {
                        main.membership.account.list.accountLog.getPaging($('#id').val(), main.membership.account.list.accountLog.paging.currentPage);
                    }
                });
            }
        }
        /*#endregion*/
    }
};

$(document).ready(main.membership.account.list.load);