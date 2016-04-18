x.register('main.connect.home');

main.connect.home = {

    paging: x.page.newPagingHelper(50),

    /*#region 函数:filter()*/
    /*
    * 查询
    */
    filter: function()
    {
        var whereClauseValue = ' 1 = 1 ';

        if($('#searchText').val() !== '')
        {
            whereClauseValue += ' AND ( T.Code LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.Title LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.Content LIKE ##%' + x.toSafeLike($('#searchText').val()) + '%## OR T.AccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE ##%' + $('#searchText').val() + '%## OR AccountLoginName LIKE ##%' + $('#searchText').val() + '%## ) OR T.AssignToAccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE ##%' + $('#searchText').val() + '%## OR AccountLoginName LIKE ##%' + $('#searchText').val() + '%## ) ) ';
        }

        // 移除 1 = 1
        if(whereClauseValue.indexOf(' 1 = 1  AND ') > -1)
        {
            whereClauseValue = whereClauseValue.replace(' 1 = 1  AND ', '');
        }

        main.connect.home.paging.whereClause = whereClauseValue;

        main.connect.home.paging.orders = ' T.UpdateDate DESC ';

        main.connect.home.getPaging(1);
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
        outString += '<th style="width:160px">名称</th>';
        outString += '<th >描述</th>';
        outString += '<th style="width:60px" title="状态" >状态</th>';
        outString += '<th style="width:60px" title="编辑" ><i class="fa fa-edit" ></i></th>';
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
        outString += '<col style="width:60px" />';
        outString += '<col style="width:60px" />';
        outString += '</colgroup>';
        outString += '<tbody>';

        x.each(list, function(index, node)
        {
            outString += '<tr>';
            outString += '<td><strong>' + node.name + '</strong></td>';
            outString += '<td>' + node.description + '</td>';
            outString += '<td>' + node.status + '</td>';
            outString += '<td><a href="/connect/overview/' + node.id + '" >编辑</a></td>';
            // outString += '<td><a href="javascript:main.connect.home.confirmDelete(\'' + node.id + '\');">删除</a></td>';
            outString += '</tr>';

            counter++;
        });

        // 补全

        while(counter < maxCount)
        {
            outString += '<tr><td colspan="8" >&nbsp;</td></tr>';

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
        var paging = main.connect.home.paging;

        paging.currentPage = currentPage;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += paging.toXml();
        outString += '</request>';

        x.net.xhr('/api/connect.queryMyList.aspx', outString, {
            waitingType: 'mini',
            waitingMessage: '正在查询数据，请稍后......',
            callback: function(response)
            {
                var result = x.toJSON(response);

                var paging = main.connect.home.paging;

                var list = result.data;

                paging.load(result.paging);

                var containerHtml = main.connect.home.getObjectsView(list, paging.pageSize);

                $('#window-main-table-container').html(containerHtml);

                var footerHtml = paging.tryParseMenu('javascript:main.connect.home.getPaging({0});');

                $('#window-main-table-footer').html(footerHtml);

                masterpage.resize();
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
        if(confirm('确定删除?'))
        {
            x.net.xhr('/api/connect.delete.aspx?id=' + id, {
                callback: function(response)
                {
                    main.connect.home.getPaging(main.connect.home.paging.currentPage);
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:getCategoryWizard()*/
    getCategoryWizard: function()
    {
        x.wizards.getWizard('category', {
            create: function()
            {
                main.connect.home.categoryWizardName = this.name;

                var outString = '';

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:300px; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>类别选择</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div id="' + this.name + '$treeViewContainer" class="winodw-wizard-tree-view" style="width:296px; height:300px;"></div>';

                outString += '<div class="winodw-wizard-result-container" >';
                outString += '<div class="winodw-wizard-result-item" ><span class="winodw-wizard-result-item-text"　>所属类别</span></div>';
                outString += '<div class="winodw-wizard-result-item" ><input id="' + this.name + '$wizardResultText" name="wizardResultText" type="text" value="" class="winodw-wizard-result-item-input" /><input id="' + this.name + '$wizardResultValue" name="wizardResultValue" type="hidden" value="" /></div>';
                outString += '<div class="winodw-wizard-result-item" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div class="clear"></div>';
                outString += '</div>';

                return outString;
            },

            save_callback: function()
            {
                main.connect.home.setCategoryIndex(x.form.query(this.name + '$wizardResultValue').val());

                return 0;
            }
        });

        main.connect.home.getTreeView();
    },
    /*#endregion*/

    /*#region 函数:setCategoryIndex(value)*/
    setCategoryIndex: function(value)
    {
        x.form.query('categoryName').val(x.string.trim(value.replace(/\\/g, '-'), '-'));
        x.form.query('categoryIndex').val(value);
    },
    /*#endregion*/

    /*#region 函数:getTreeView()*/
    getTreeView: function()
    {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<treeViewId><![CDATA[]]></treeViewId>';
        outString += '<treeViewName><![CDATA[问题类别]]></treeViewName>';
        outString += '<treeViewRootTreeNodeId><![CDATA[1]]></treeViewRootTreeNodeId>';
        outString += '<tree><![CDATA[{tree}]]></tree>';
        outString += '<parentId><![CDATA[{parentId}]]></parentId>';
        outString += '<enabledLeafClick><![CDATA[false]]></enabledLeafClick>';
        outString += '<elevatedPrivileges><![CDATA[true]]></elevatedPrivileges>';
        outString += '<url><![CDATA[javascript:main.connect.home.setTreeViewNode(\'{treeNodeId}\')]]></url>';
        outString += '<treeName>tree</treeName>';

        outString += '</request>';

        var tree = x.tree.newTreeView('main.connect.home.tree');

        tree.setAjaxMode(true);

        tree.add("0", "-1", '问题类别', 'javascript:main.connect.home.setTreeViewNode(\'\')', '问题类别', '', '/resources/images/tree/tree_icon.gif');

        tree.load('/api/connect.category.getDynamicTreeView.aspx', false, outString);

        main.connect.home.tree = tree;

        var wizardName = main.connect.home.categoryWizardName;

        x.form.query(wizardName + '$treeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value)*/
    setTreeViewNode: function(value)
    {
        var wizardName = main.connect.home.categoryWizardName;

        x.form.query(wizardName + '$wizardResultText').val(x.string.trim(value.replace(/\\/g, '-'), '-'));
        x.form.query(wizardName + '$wizardResultValue').val(x.string.trim(value, '\\'));
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /*
    * 页面加载事件
    */
    load: function()
    {
        main.connect.home.filter();

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnCategoryWizard').bind('click', function()
        {
            main.connect.home.getCategoryWizard()
        });

        $('#btnFilter').bind('click', function()
        {
            main.connect.home.filter();
        });
    }
    /*#endregion*/
};

$(document).ready(main.connect.home.load);

/*
* [默认]私有回调函数, 供子窗口回调
*/
function window$refresh$callback()
{
    main.connect.home.filter();
}
