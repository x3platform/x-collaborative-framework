x.register('x.ui.wizards');

/*
* 流程模板选择向导
*/
x.ui.wizards.newWorkflowTemplateWizard = function(name, targetViewName, targetValueName, options)
{
    var wizard = {

        // 实例名称
        name: name,

        // 配置信息
        options: options,

        // 遮罩
        maskWrapper: null,

        // 树型对象
        tree: null,

        // 本地数据源(JSON格式)
        localStorage: null,

        // 返回值
        returnValue: null,

        // 目标视图对象名称
        targetViewName: targetViewName,

        // 目标值对象名称
        targetValueName: targetValueName,

        // 工作流模板类型
        workflowTemplateType: null,

        // SQL查询条件
        whereClause: '',

        /*#region 函数:filter()*/
        /*
        * 查询
        */
        filter: function()
        {
            var whereClauseValue = ' Status = 1 ';

            var key = $('#' + this.name + '-wizardSearchText').val().trim();

            if(key !== '')
            {
                whereClauseValue += ' AND ( T.Name LIKE ##%' + key + '%## ) ';
            }

            if(typeof (this.workflowTemplateType) !== 'undefined' && this.workflowTemplateType !== null && this.workflowTemplateType !== '')
            {
                whereClauseValue += ' AND ( T.Type = ##' + this.workflowTemplateType + '## ) ';
            }

            this.whereClause = whereClauseValue + ' ORDER BY Name ';

            this.findAll();
        },
        /*#endregion*/

        /*#region 函数:findAll()*/
        /**
        * 查询
        */
        findAll: function()
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
            outString += '<whereClause><![CDATA[' + this.whereClause + ']]></whereClause>';
            outString += '<length><![CDATA[0]]></length>';
            outString += '</request>';

            x.net.xhr('/api/workflow.template.findAll.aspx', outString, {
                callback: function(response)
                {
                    var outString = '';

                    var result = x.toJSON(response);

                    var list = result.data;

                    var clientTargetObject = result.clientTargetObject;

                    var maxCount = 10;

                    var counter = 0;

                    var classNameValue = '';

                    outString += '<div class="table-freeze-head">';
                    outString += '<table class="table" >';
                    outString += '<thead>';
                    outString += '<tr>';
                    outString += '<th >名称</td>';
                    outString += '<th style="width:50px" >编辑</th>';
                    outString += '<th class="table-freeze-head-padding" style="width:' + x.page.getScrollBarWidth().vertical + 'px" >&nbsp;</th>';
                    outString += '</thead>';
                    outString += '</table>';
                    outString += '</div>';

                    outString += '<div id="' + clientTargetObject + '-wizardListContainer" style="overflow-y:scroll; overflow-x:hidden;" >';
                    outString += '<table class="table table-striped">';
                    outString += '<colgroup><col /><col style="width:50px" /></colgroup>';
                    outString += '<tbody>';

                    x.each(list, function(index, node)
                    {
                        // 
                        outString += '<tr>';
                        outString += '<td><a href="javascript:' + clientTargetObject + '.setTemplate(\'' + node.id + '\',\'' + node.name + '\');">' + node.name + '</a></td>';
                        outString += '<td><a href="/workflowplus/workflow-designer?id=' + encodeURIComponent(node.id) + '" target="_blank" >编辑</a></td>';
                        outString += '</tr>';

                        counter++;
                    });

                    // 补全

                    while(counter < maxCount)
                    {
                        outString += '<tr><td colspan="2" >&nbsp;</td></tr>';

                        counter++;
                    }

                    outString += '</tbody>';
                    outString += '</table>';
                    outString += '</div>';

                    $(document.getElementById(clientTargetObject + '-wizardTableContainer')).html(outString);

                    $(document.getElementById(clientTargetObject + '-wizardTreeViewContainer')).height(231);
                    $(document.getElementById(clientTargetObject + '-wizardListContainer')).height(239);
                }
            });
        },
        /*#endregion*/

        /*#region 函数:setTemplate(id, name)*/
        setTemplate: function(id, name)
        {
            $(document.getElementById(this.name + '-wizardWorkflowTemplateText')).val(name);
            $(document.getElementById(this.name + '-wizardWorkflowTemplateValue')).val(id);
        },
        /*#endregion*/

        /*#region 函数:open()*/
        open: function()
        {
            this.maskWrapper.open();
        },
        /*#endregion*/

        /*#region 函数:save()*/
        save: function()
        {
            this.returnValue = '{"text":"' + $(document.getElementById(this.name + '-wizardWorkflowTemplateText')).val() + '","value":"' + $(document.getElementById(this.name + '-wizardWorkflowTemplateValue')).val() + '"}';

            this.save_callback(this.returnValue);

            this.cancel();
        },
        /*#endregion*/

        /*#region 函数:save_callback(response)*/
        /*
        * 默认回调函数，可根据需要自行修改此函数。
        */
        save_callback: function(response)
        {
            var result = x.toJSON(this.returnValue);

            if(document.getElementById(this.targetViewName).tagName.toUpperCase() == 'INPUT'
                || document.getElementById(this.targetViewName).tagName.toUpperCase() == 'TEXTAREA')
            {
                $(document.getElementById(this.targetViewName)).val(result.text);
            }
            else
            {
                $(document.getElementById(this.targetViewName)).html(result.text);
            }

            if(document.getElementById(this.targetValueName).tagName.toUpperCase() == 'INPUT'
                || document.getElementById(this.targetValueName).tagName.toUpperCase() == 'TEXTAREA')
            {
                $(document.getElementById(this.targetValueName)).val(result.value);
            }
            else
            {
                $(document.getElementById(this.targetValueName)).html(result.value);
            }
        },
        /*#endregion*/

        /*#region 函数:cancel()*/
        cancel: function()
        {
            this.maskWrapper.close();

            if(typeof (this.cancel_callback) !== 'undefined')
            {
                this.cancel_callback(this.returnValue);
            }
        },
        /*#endregion*/

        /*#region 函数:getTreeView()*/
        /*
        * 获取树形菜单
        */
        getTreeView: function()
        {
            var treeViewId = '10000000-0000-0000-0000-000000000000';
            var treeViewName = '流程模板类别';
            var treeViewRootTreeNodeId = '';
            var treeViewUrl = 'javascript:' + this.name + '.setTreeViewNode(\'{treeNodeToken}\',\'{treeNodeName}\')';

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

            var tree = x.ui.pkg.tree.newTreeView({ name: this.name + '.tree', ajaxMode: true });

            tree.add({
                id: "0",
                parentId: "-1",
                name: treeViewName,
                url: treeViewUrl.replace('{treeNodeToken}', treeViewRootTreeNodeId).replace('{treeNodeName}', ''),
                title: treeViewName,
                target: '',
                icon: '/resources/images/tree/tree_icon.gif'
            });

            tree.load('/api/workflow.category.getDynamicTreeView.aspx', false, outString);

            this.tree = tree;

            document.getElementById(this.name + '-wizardTreeViewContainer').innerHTML = tree;
        },
        /*#endregion*/

        /*#region 函数:setTreeViewNode(value)*/
        setTreeViewNode: function(value)
        {
            var whereClauseValue = '';

            value = value.replace('[CategoryTreeNode]', '');

            if(value === '')
            {
                // whereClauseValue = ' T.Id IN ( SELECT DISTINCT WorkflowTemplateId FROM tb_Workflow_TemplateMapping )';
            }
            else
            {
                whereClauseValue = ' T.Id IN ( SELECT WorkflowTemplateId FROM tb_Workflow_TemplateMapping WHERE WorkflowCategoryId = ##' + value + '## )';
            }

            if(typeof (this.workflowTemplateType) !== 'undefined' && this.workflowTemplateType !== null && this.workflowTemplateType !== '')
            {
                whereClauseValue += (whereClauseValue === '' ? '' : ' AND ') + ' ( T.Type = ##' + this.workflowTemplateType + '## ) ';
            }

            this.whereClause = whereClauseValue;

            this.findAll();
        },
        /*#endregion*/

        /*#region 函数:create()*/
        /**
         * 创建容器的结构
         */
        create: function()
        {
            var outString = '';

            outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:720px; height:auto;" >';

            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close">';
            outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
            outString += '</div>';
            outString += '<div class="float-left">';
            outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>流程模板选择向导</span></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<table class="table-style" style="width:100%;">';
            outString += '<tr>';
            outString += '<td class="table-sidebar" >';
            outString += '<div class="table-sidebar-search form-inline" ><input id="' + this.name + '-wizardSearchText" type="text" value="" class="table-sidebar-search-text form-control input-sm" /> <button id="' + this.name + '-wizardBtnFilter" class="btn btn-default btn-sm" title="查询"><i class="glyphicon glyphicon-search"></i></button></div>';
            outString += '<div id="' + this.name + '-wizardTreeViewContainer" class="table-sidebar-tree-view" ></div>';
            outString += '</td>';
            outString += '<td id="' + this.name + '-wizardTableContainer" class="table-body">';
            outString += '<span class="tooltip-loading-text"><img src="/resources/images/loading.gif" alt="正在加载..." /></span>';
            outString += '</td>';
            outString += '</tr>';
            outString += '</table>';

            outString += '<div class="winodw-wizard-result-container form-inline text-right" >';
            outString += '<label class="winodw-wizard-result-item-text" >模板名称</label> ';
            outString += '<input id="' + this.name + '-wizardWorkflowTemplateText" name="wizardWorkflowTemplateText" type="text" value="" class="winodw-wizard-result-item-input form-control input-sm" style="width:350px;" /> ';
            outString += '<input id="' + this.name + '-wizardWorkflowTemplateValue" name="wizardWorkflowTemplateValue" type="hidden" value="" />';
            outString += '<a href="javascript:' + this.name + '.save();" class="btn btn-default btn-sm" >确定</a>';
            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:load()*/
        /*
        * 加载界面、数据、事件等信息
        */
        load: function()
        {
            // 设置遮罩对象
            if(typeof (options.maskWrapper) === 'undefined')
            {
                this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 738, draggableHeight: 382 });
            }
            else
            {
                this.maskWrapper = options.maskWrapper;
            }

            // 设置工作流模板类型
            if(typeof (options.workflowTemplateType) !== 'undefined')
            {
                this.workflowTemplateType = options.workflowTemplateType;
            }

            // 设置本地数据源
            if(typeof (options.localStorage) !== 'undefined')
            {
                this.localStorage = options.localStorage;
            }

            // 加载遮罩和页面内容
            x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);

            $(document.getElementById(this.name + '-wizardTreeViewContainer')).css('height', '291px');
            $(document.getElementById(this.name + '-wizardTreeViewContainer')).css('width', '200px');
            $(document.getElementById(this.name + '-wizardTreeViewContainer')).css('overflow', 'auto');

            // 加载数据
            this.getTreeView();

            this.filter();

            $(document.getElementById(this.name + '-wizardTreeViewContainer')).height(231);

            // 加载事件
            $(document.getElementById(this.name + '-wizardSearchText')).on("keyup", function()
            {
                window[this.id.replace('-wizardSearchText', '')].filter();
            });

            $(document.getElementById(this.name + '-wizardBtnFilter')).on("click", function()
            {
                window[this.id.replace('-wizardBtnFilter', '')].filter();
            });

            // -------------------------------------------------------
            // 设置目标容器数据
            // -------------------------------------------------------

            if(this.localStorage !== null)
            {
                var node = x.toJSON(this.localStorage);

                $(document.getElementById(this.name + '-wizardWorkflowTemplateText')).val(node.text);
                $(document.getElementById(this.name + '-wizardWorkflowTemplateValue')).val(node.value);
            }
        }
        /*#endregion*/
    };

    return wizard;
};

/*#region 函数:x.ui.wizards.getWorkflowTemplateWizard(options)*/
/**
 * 快速创建选择向导
 */
x.ui.wizards.getWorkflowTemplateWizard = function(options)
{
    var name = x.getFriendlyName(location.pathname + '-' + options.targetViewName + '-' + options.targetValueName + '-workflow-template-wizard');

    if(typeof (window[name]) === 'undefined')
    {
        // 配置参数
        options.localStorage = '{"text":"' + $('#' + options.targetViewName).val() + '","value":"' + $('#' + options.targetValueName).val() + '"}'

        // 初始化向导
        var wizard = x.ui.wizards.newWorkflowTemplateWizard(name, options.targetViewName, options.targetValueName, options);

        // 加载界面、数据、事件
        wizard.load();

        // 绑定到Window对象
        window[name] = wizard;
    }
    else
    {
        window[name].open();
    }
};
/*#endregion*/