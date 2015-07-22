x.register('x.ui.wizards');

/*
* 收藏设置向导
*/
x.ui.wizards.newRecommendWizard = function(name, options)
{
    var wizard = {

        // 实例名称
        name: name,

        // 选项信息
        options: options,

        // 遮罩
        maskWrapper: null,

        /*#region 函数:open()*/
        open: function()
        {
            this.maskWrapper.open();
        },
        /*#endregion*/

        /*#region 函数:close()*/
        close: function()
        {
            this.maskWrapper.close();
        },
        /*#endregion*/

        /*#region 函数:save()*/
        save: function()
        {
            this.save_callback(this.returnValue);

            this.close();
        },
        /*#endregion*/

        /*#region 函数:save_callback(response)*/
        /*
        * 默认回调函数，可根据需要自行修改此函数。
        */
        save_callback: function(response)
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<ajaxStorage>';
            outString += '<action><![CDATA[recommend]]></action>';
            outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
            outString += '<customTableName><![CDATA[' + $(document.getElementById(this.name + '-recommend-wizard-customTableName')).val() + ']]></customTableName>';
            outString += '<entityId><![CDATA[' + $(document.getElementById(this.name + '-recommend-wizard-entityId')).val() + ']]></entityId>';
            outString += '<entityTitle><![CDATA[' + $(document.getElementById(this.name + '-recommend-wizard-entityTitle')).val() + ']]></entityTitle>';
            outString += '<entityViewUrlFormat><![CDATA[' + $(document.getElementById(this.name + '-recommend-wizard-entityViewUrlFormat')).val() + ']]></entityViewUrlFormat>';
            outString += '<authorizationObjectText><![CDATA[' + $(document.getElementById(this.name + '-recommend-wizard-authorizationObjectText')).val() + ']]></authorizationObjectText>';
            outString += '<reason><![CDATA[' + $(document.getElementById(this.name + '-recommend-wizard-reason')).val() + ']]></reason>';
            outString += '</ajaxStorage>';

            x.net.xhr(this.options.url, outString, {
                popResultValue: 1,
                callback: function(response) { }
            });
        },
        /*#endregion*/

        /*#region 函数:cancel()*/
        cancel: function()
        {
            if(typeof (this.cancel_callback) !== 'undefined')
            {
                this.cancel_callback(this.returnValue);
            }

            this.close();
        },
        /*#endregion*/

        /*#region 函数:create()*/
        create: function()
        {
            var outString = '';

            outString += '<div id="' + this.name + '-recommend-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close">';
            outString += '<a id="' + this.name + '-recommend-wizard-close" href="javascript:' + this.name + '.cancel();"  title="关闭" ><i class="fa fa-close"></i></a>';
            outString += '</div>';
            outString += '<div class="float-left">';
            outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>推荐</span></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

            outString += '<tr>';
            outString += '<td class="table-body-text" style="width:80px" >推荐人员</td>';
            outString += '<td class="table-body-input" >';
            outString += '<input id="' + this.name + '-recommend-wizard-customTableName" name="' + this.name + '-recommend-wizard-customTableName" type="hidden" value="' + this.options.customTableName + '" />';
            outString += '<input id="' + this.name + '-recommend-wizard-entityId" name="' + this.name + '-recommend-wizard-entityId" type="hidden" value="' + this.options.entityId + '" />';
            outString += '<input id="' + this.name + '-recommend-wizard-entityTitle" name="' + this.name + '-recommend-wizard-entityTitle" type="hidden" value="' + this.options.entityTitle + '" />';
            outString += '<input id="' + this.name + '-recommend-wizard-entityViewUrlFormat" name="' + this.name + '-recommend-wizard-entityViewUrlFormat" type="hidden" value="' + this.options.entityViewUrlFormat + '" />';
            outString += '<input id="' + this.name + '-recommend-wizard-authorizationObjectText" name="' + this.name + '-recommend-wizard-authorizationObjectText" type="hidden" />';
            outString += '<div class="panel panel-default" style="width:290px;">';
            outString += '<div class="panel-body">';
            outString += '<div id="' + this.name + '-recommend-wizard-authorizationObjectView" name="' + this.name + '-recommend-wizard-authorizationObjectView" style="height:50px;" ></div>';
            outString += '</div>';
            outString += '<div class="panel-footer text-right" ><a href="javascript:x.ui.wizards.getContactsWizard({targetViewName:\'' + this.name + '-recommend-wizard-authorizationObjectView\',targetValueName:\'' + this.name + '-recommend-wizard-authorizationObjectText\',contactTypeText:\'account\'});" ><i class="fa fa-pencil-square-o"></i> 编辑</a></div>';
            outString += '</div>';
            outString += '</td>';
            outString += '</tr>';
            outString += '<tr>';
            outString += '<td class="table-body-text" >推荐原因</td>';
            outString += '<td class="table-body-input" ><textarea id="' + this.name + '-recommend-wizard-reason" name="' + this.name + '-recommend-wizard-reason" class="form-control" style="width:290px; height:80px;" ></textarea></td>';
            outString += '</tr>';
            outString += '<tr>';
            outString += '<td ></td>';
            outString += '<td class="table-body-input" >';
            outString += '<button id="' + this.name + '-recommend-wizard-save" onclick="' + this.name + '.save();" class="btn btn-default" style="margin:0 15px 15px 0;" ><i class="fa fa-check"></i> 确定</button>';
            outString += '<button id="' + this.name + '-recommend-wizard-cancel" onclick="' + this.name + '.cancel();" class="btn btn-default" style="margin:0 15px 15px 0;" ><i class="fa fa-times"></i> 取消</button>';
            outString += '</td>';
            outString += '</tr>';
            outString += '</table>';

            outString += '<div class="clear"></div>';
            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:load()*/
        load: function()
        {
            // 设置遮罩对象
            if(typeof (options.maskWrapper) === 'undefined')
            {
                this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
            }
            else
            {
                this.maskWrapper = options.maskWrapper;
            }

            // 设置数据表名称
            if(typeof (options.customTableName) === 'undefined')
            {
                alert('参数【customTableName】必须填写。');
                return;
            }
            else
            {
                this.customTableName = options.customTableName;
            }

            // 设置实体标识
            if(typeof (options.entityId) === 'undefined')
            {
                alert('参数【entityId】必须填写。');
                return;
            }

            // 设置实体查看页面
            if(typeof (options.entityTitle) === 'undefined')
            {
                alert('参数【entityTitle】必须填写。');
                return;
            }

            // 设置实体标题
            if(typeof (options.entityTitle) === 'undefined')
            {
                alert('参数【entityTitle】必须填写。');
                return;
            }

            // 设置实体查看页面
            if(typeof (options.entityViewUrlFormat) === 'undefined')
            {
                alert('参数【entityViewUrlFormat】必须填写。');
                return;
            }

            // 设置保存回调函数
            if(typeof (options.save_callback) !== 'undefined')
            {
                this.save_callback = options.save_callback;
            }

            // 设置取消回调函数
            if(typeof (options.cancel_callback) !== 'undefined')
            {
                this.cancel_callback = options.cancel_callback;
            }

            x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);
        }
        /*#endregion*/
    }

    return wizard;
};

/*#region 函数:x.ui.wizards.getRecommendWizard(options)*/
/*
* 快速创建单例
*/
x.ui.wizards.getRecommendWizard = function(options)
{
    var name = x.getFriendlyName(location.pathname + '-recommend-wizard');

    if(typeof (window[name]) === 'undefined')
    {
        // 初始化向导
        var wizard = x.ui.wizards.newRecommendWizard(name, options);

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
