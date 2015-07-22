x.register('x.wizards');

/*
* 收藏设置向导
*/
x.wizards.newChangeCommitAccountWizard = function(name, options)
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
            outString += '<action><![CDATA[setAccount]]></action>';
            outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
            outString += '<entityId><![CDATA[' + x.form.query(this.name + '$change$commitAccount$wizard$entityId').val() + ']]></entityId>';
            outString += '<accountId><![CDATA[' + x.form.query(this.name + '$change$commitAccount$wizard$accountId').val() + ']]></accountId>';
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
            if (typeof (this.cancel_callback) !== 'undefined')
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

            outString += '<div id="' + this.name + '$change$commitAccount$wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close">';
            outString += '<a id="' + this.name + '$change$commitAccount$wizard$close" href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
            outString += '</div>';
            outString += '<div class="float-left">';
            outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>修改提交人</span></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

            outString += '<tr>';
            outString += '<td class="table-body-text" style="width:80px" >当前提交人</td>';
            outString += '<td class="table-body-input" >';
            outString += '<input id="' + this.name + '$change$commitAccount$wizard$entityId" name="' + this.name + '$change$commitAccount$wizard$entityId" type="hidden" value="' + this.options.entityId + '" />';
            outString += '<input id="' + this.name + '$change$commitAccount$wizard$originalAccountId" name="' + this.name + '$change$commitAccount$wizard$originalAccountId" type="hidden" value="' + this.options.originalAccountId + '" />';
            outString += '<input id="' + this.name + '$change$commitAccount$wizard$originalAccountName" name="' + this.name + '$change$commitAccount$wizard$originalAccountName" type="text" value="' + this.options.originalAccountName + '" class="input-normal" style="width:260px;" />';
            outString += '</td>';
            outString += '</tr>';
            outString += '<tr>';
            outString += '<td class="table-body-text" >新的提交人</td>';
            outString += '<td class="table-body-input" >';
            outString += '<input id="' + this.name + '$change$commitAccount$wizard$accountId" name="' + this.name + '$change$commitAccount$wizard$accountId" type="hidden" />';
            outString += '<input id="' + this.name + '$change$commitAccount$wizard$accountName" name="' + this.name + '$change$commitAccount$wizard$accountName" readonly="readonly" class="input-normal" style="width:260px;" />';
            outString += '<div class="text-right" style="width:260px;"><a href="javascript:x.wizards.getContactWizard({targetViewName: \'' + this.name + '$change$commitAccount$wizard$accountName\', targetValueName: \'' + this.name + '$change$commitAccount$wizard$accountId\', contactTypeText: \'account\'});">编辑</a></div>';
            outString += '</td>';
            outString += '</tr>';
            outString += '<tr>';
            outString += '<td ></td>';
            outString += '<td class="table-body-input" >';
            outString += '<div style="margin:0 0 4px 2px;" >';
            outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '$change$commitAccount$wizard$save" href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div>';
            outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '$change$commitAccount$wizard$cancel" href="javascript:' + this.name + '.cancel();" class="button-text" >取消</a></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
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
            if (typeof (options.maskWrapper) === 'undefined')
            {
                this.maskWrapper = x.mask.newMaskWrapper(this.name + '$maskWrapper', { draggableWidth: 418 });
            }
            else
            {
                this.maskWrapper = options.maskWrapper;
            }

            // 设置实体标识
            if (typeof (options.entityId) === 'undefined')
            {
                alert('参数【entityId】必须填写。');
                return;
            }

            // 设置实体查看页面
            if (typeof (options.originalAccountId) === 'undefined')
            {
                alert('参数【originalAccountId】必须填写。');
                return;
            }

            if (typeof (options.originalAccountName) === 'undefined')
            {
                alert('参数【originalAccountName】必须填写。');
                return;
            }

            // 设置保存回调函数
            if (typeof (options.save_callback) !== 'undefined')
            {
                this.save_callback = options.save_callback;
            }

            // 设置取消回调函数
            if (typeof (options.cancel_callback) !== 'undefined')
            {
                this.cancel_callback = options.cancel_callback;
            }

            x.mask.getWindow(this.create(), this.maskWrapper);
        }
        /*#endregion*/
    }

    return wizard;
};

/*#region 函数:x.wizards.getChangeCommitAccountWizard(options)*/
/*
* 快速创建单例
*/
x.wizards.getChangeCommitAccountWizard = function(options)
{
    var name = x.getFriendlyName(location.pathname + '$change$commitAccount$wizard');

    if (typeof (window[name]) === 'undefined')
    {
        // 初始化向导
        var wizard = x.wizards.newChangeCommitAccountWizard(name, options);

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
