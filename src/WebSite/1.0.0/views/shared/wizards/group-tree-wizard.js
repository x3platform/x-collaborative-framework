x.register('x.wizards');

/*
* 公司选择向导
*/
x.wizards.newGroupTreeViewWizard = function (name, targetViewName, targetValueName, options)
{
    var wizard = {

        url: '/services/X3Platform/Membership/Ajax.GroupTreeWrapper.aspx',

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

        /*#region 函数:open()*/
        open: function ()
        {
            this.maskWrapper.open();
        },
        /*#endregion*/

        /*#region 函数:save()*/
        save: function ()
        {
            this.returnValue = '{"text":"' + $(document.getElementById(this.name + '$wizardGroupTreeViewText')).val() + '","value":"' + $(document.getElementById(this.name + '$wizardGroupTreeViewValue')).val() + '"}';

            this.save_callback(this.returnValue);

            this.cancel();
        },
        /*#endregion*/

        /*#region 函数:save_callback(response)*/
        /*
        * 默认回调函数，可根据需要自行修改此函数。
        */
        save_callback: function (response)
        {
            var result = x.toJSON(this.returnValue);

            if (document.getElementById(this.targetViewName).tagName.toUpperCase() == 'INPUT'
                || document.getElementById(this.targetViewName).tagName.toUpperCase() == 'TEXTAREA')
            {
                $(document.getElementById(this.targetViewName)).val(result.text);
            }
            else
            {
                $(document.getElementById(this.targetViewName)).html(result.text);
            }

            if (document.getElementById(this.targetValueName).tagName.toUpperCase() == 'INPUT'
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
        cancel: function ()
        {
            this.maskWrapper.close();

            if (typeof (this.cancel_callback) !== 'undefined')
            {
                this.cancel_callback(this.returnValue);
            }
        },
        /*#endregion*/

        /*#region 函数:getTreeView()*/
        /*
        * 获取树形菜单
        */
        getTreeView: function ()
        {
            var treeViewId = this.treeViewId;
            var treeViewName = this.treeViewName;
            var treeViewRootTreeNodeId = this.treeViewId;
            var treeViewUrl = 'javascript:' + this.name + '.setTreeViewNode(\'{treeNodeId}\',\'{treeNodeName}\')';

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

            var tree = x.tree.newTreeView(this.name + '.tree');

            tree.setAjaxMode(true);

            tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId).replace('{treeNodeName}', treeViewName.replace('类别', '')), treeViewName, '', '/resources/images/tree/tree_icon.gif');

            tree.load(this.url, false, outString);

            this.tree = tree;

            document.getElementById(this.name + '$wizardTreeViewContainer').innerHTML = tree;
        },
        /*#endregion*/

        /*#region 函数:setTreeViewNode(value, text)*/
        setTreeViewNode: function (value, text)
        {
            $(document.getElementById(this.name + '$wizardGroupTreeViewText')).val(text);
            $(document.getElementById(this.name + '$wizardGroupTreeViewValue')).val(value);

            this.returnValue = '{"text":"' + text + '","value":"' + value.replace('[GroupTreeViewTreeNode]', '') + '"}';
        },
        /*#endregion*/

        /*#region 函数:create()*/
        /*
        * 创建容器的结构
        */
        create: function ()
        {
            var outString = '';

            outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:300px; height:auto;" >';

            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close">';
            outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
            outString += '</div>';
            outString += '<div class="float-left">';
            outString += '<div class="winodw-wizard-toolbar-item"><span>类别选择向导</span></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<div id="' + this.name + '$wizardTreeViewContainer" class="winodw-wizard-tree-view" style="height:300px;" ></div>';

            outString += '<div class="winodw-wizard-result-container" >';
            outString += '<div class="winodw-wizard-result-item"><span class="winodw-wizard-result-item-text" >类别名称</span></div>';
            outString += '<div class="winodw-wizard-result-item"><input id="' + this.name + '$wizardGroupTreeViewText" name="wizardGroupTreeViewText" type="text" value="" class="winodw-wizard-result-item-input" /><input id="' + this.name + '$wizardGroupTreeViewValue" name="wizardGroupTreeViewValue" type="hidden" value="" /></div>';
            outString += '<div class="winodw-wizard-result-item"><div class="button-2font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<div class="clear"></div>';
            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:load()*/
        /*
        * 加载界面、数据、事件等信息
        */
        load: function ()
        {
            // 设置遮罩对象
            if (typeof (options.maskWrapper) === 'undefined')
            {
                this.maskWrapper = x.mask.newMaskWrapper(this.name + '$maskWrapper', { draggableWidth: 318 });
            }
            else
            {
                this.maskWrapper = options.maskWrapper;
            }

            // 设置本地数据源
            if (typeof (options.localStorage) !== 'undefined')
            {
                this.localStorage = options.localStorage;
            }

            // 设置树视图的标识
            if (typeof (options.treeViewId) !== 'undefined')
            {
                this.treeViewId = options.treeViewId;
            }

            // 设置树视图的名称
            if (typeof (options.treeViewName) !== 'undefined')
            {
                this.treeViewName = options.treeViewName;
            }

            // 加载遮罩和页面内容
            x.mask.getWindow(this.create(), this.maskWrapper);

            // 加载数据
            this.getTreeView();

            $(document.getElementById(this.name + '$wizardTreeViewContainer')).width(296);

            // -------------------------------------------------------
            // 设置目标容器数据
            // -------------------------------------------------------

            if (this.localStorage !== null)
            {
                var node = x.toJSON(this.localStorage);

                $(document.getElementById(this.name + '$wizardGroupTreeViewText')).val(node.text);
                $(document.getElementById(this.name + '$wizardGroupTreeViewValue')).val(node.value);
            }
        }
        /*#endregion*/
    };

    return wizard;
};

/*#region 函数:x.wizards.getGroupTreeWizardSingleton(targetViewName, targetValueName, treeViewId, treeViewName, save_callback, cancel_callback)*/
/*
* 快速创建单例
*/
x.wizards.getGroupTreeWizardSingleton = function (targetViewName, targetValueName, treeViewId, treeViewName, save_callback, cancel_callback)
{
    var name = x.getFriendlyName(location.pathname + '$' + targetViewName + '$' + targetValueName + '$group$tree$wizard');

    if (typeof (window[name]) === 'undefined')
    {
        // 配置参数
        var options = {
            localStorage: '{"text":"' + $(document.getElementById(targetViewName)).val() + '","value":"' + $(document.getElementById(targetValueName)).val() + '"}',
            treeViewId: treeViewId,
            treeViewName: treeViewName,
            save_callback: save_callback,
            cancel_callback: cancel_callback
        };

        // 初始化向导
        var wizard = x.wizards.newGroupTreeViewWizard(name, targetViewName, targetValueName, options);

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