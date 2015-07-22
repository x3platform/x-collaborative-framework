x.register('x.wizards');

/*
* 收藏设置向导
*/
x.wizards.newFavoriteWizardWizard = function(name, categoryIndex, prefixCategoryIndex, options)
{
    var wizard = {

        // 实例名称
        name: name,

        // 配置信息
        options: options,

        // 遮罩
        maskWrapper: null,

        localStorage: '',

        returnValue: '',

        categoryIndex: categoryIndex,

        prefixCategoryIndex: prefixCategoryIndex,

        /*#region 函数:open()*/
        open: function()
        {
            this.maskWrapper.open();
        },
        /*#endregion*/

        /*#region 函数:save()*/
        save: function()
        {
            var categoryIndex = $(document.getElementById(this.name + '$wizardCategoryIndex')).val();

            var prefixCategoryIndex = $(document.getElementById(this.name + '$wizardPrefixCategoryIndex')).val();

            if(prefixCategoryIndex != '')
            {
                categoryIndex = (prefixCategoryIndex + '\\' + categoryIndex).replace('\\\\', '\\');
            }

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<ajaxStorage>';
            outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
            outString += '<id><![CDATA[' + x.guid.create() + ']]></id>';
            outString += '<accountId><![CDATA[' + this.memberToken.id + ']]></accountId>';
            outString += '<categoryIndex><![CDATA[' + categoryIndex + ']]></categoryIndex>';
            outString += '<title><![CDATA[' + x.form.query(this.name + '$wizardTitle').val() + ']]></title>';
            outString += '<description><![CDATA[' + x.form.query(this.name + '$wizardDescription').val() + ']]></description>';
            outString += '<url><![CDATA[' + x.form.query(this.name + '$wizardUrl').val() + ']]></url>';
            outString += '<tags><![CDATA[]]></tags>';
            outString += '<iconPath><![CDATA[]]></iconPath>';
            outString += '<isTop><![CDATA[0]]></isTop>';
            outString += '</ajaxStorage>';

            x.net.xhr('/api/favorite.save.aspx', outString, { callback: this.save_callback });
        },
        /*#endregion*/

        /*#region 函数:save_callback(response)*/
        /*
        * 默认回调函数，可根据需要自行修改此函数。
        */
        save_callback: function(response)
        {
            var result = x.toJSON(response).message;

            switch(Number(result.returnCode))
            {
                case 0:
                    window[x.toJSON(response).clientTargetObject].cancel();
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

        /*#region 函数:create()*/
        /*
        * 创建容器的结构
        */
        create: function()
        {
            var outString = '';

            outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';
            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close">';
            outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
            outString += '</div>';
            outString += '<div class="float-left">';
            outString += '<div class="winodw-wizard-toolbar-item"><span>收藏设置向导</span></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<table class="table-style" style="width:100%;" >';
            outString += '<tr class="table-row-normal-transparent" >';
            outString += '<td class="table-body-text" style="width:65px;" ><span class="required-text">标题</span></td>';
            outString += '<td class="table-body-input" ><input id="' + this.name + '$wizardTitle" type="text" class="input-normal" style="width:280px;" value="" /></td>';
            outString += '</tr>';

            outString += '<tr class="table-row-normal-transparent" >';
            outString += '<td class="table-body-text" >描述</td>';
            outString += '<td class="table-body-input" ><textarea  id="' + this.name + '$wizardDescription" class="input-normal" style="width:280px; height:40px;" ></textarea></td>';
            outString += '</tr >';

            outString += '<tr class="table-row-normal-transparent" >';
            outString += '<td class="table-body-text" ><span class="required-text">地址</span></td>';
            outString += '<td class="table-body-input" ><textarea id="' + this.name + '$wizardUrl" class="textarea-normal" style="width:280px; height:60px;" ></textarea></td>';
            outString += '</tr>';

            outString += '<tr class="table-row-normal-transparent" >';
            outString += '<td class="table-body-text" >类别</td>';
            outString += '<td class="table-body-input" >';
            outString += '<input id="' + this.name + '$wizardPrefixCategoryIndex" type="hidden" value="' + this.prefixCategoryIndex + '" />';
            outString += '<input id="' + this.name + '$wizardCategoryIndex" type="text" class="input-normal" style="width:280px;" value="' + this.categoryIndex + '" />';
            outString += '</td>';
            outString += '</tr>';

            outString += '<tr class="table-row-normal-transparent" >';
            outString += '<td class="table-body-text" ></td>';
            outString += '<td class="table-body-input" >';
            outString += '<div class="button-2font-wrapper" style="margin:0 0 8px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >保存</a></div>';
            outString += '</td>';
            outString += '</tr>';
            outString += '</table>';

            outString += '<div class="clear"></div>';
            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:getMemberToken()*/
        getMemberToken: function()
        {
            var memberIdentity = x.cookies.find($('#session-identity-name').val());

            var memberToken = x.cookies.find($('#session-identity-name').val());

            if(memberIdentity === '')
            {
                return;
            }
            else if(memberToken === '')
            {
                var me = this;

                x.net.xhr('/api/membership.member.read.aspx?key=' + memberIdentity, {
                    callback: function(response)
                    {
                        x.cookies.add('memberToken', response);

                        me.memberToken = x.toJSON(response).data;
                    }
                });
            }
            else
            {
                this.memberToken = x.toJSON(memberToken).data;
            }
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
                this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
            }
            else
            {
                this.maskWrapper = options.maskWrapper;
            }

            // 设置本地数据源
            if(typeof (options.localStorage) !== 'undefined')
            {
                this.localStorage = options.localStorage;
            }

            this.getMemberToken();

            // 加载遮罩和页面内容
            x.mask.getWindow(this.create(), this.maskWrapper);

            // -------------------------------------------------------
            // 设置目标容器数据
            // -------------------------------------------------------

            if(this.localStorage !== '')
            {
                var node = x.toJSON(this.localStorage);

                $(document.getElementById(this.name + '$wizardTitle')).val(node.text);
                $(document.getElementById(this.name + '$wizardUrl')).val(node.value);
            }
        }
        /*#endregion*/
    }

    return wizard;
};

/*#region 函数:x.wizards.getFavoriteWizard(options)*/
/*
* 快速创建单例
*/
x.wizards.getFavoriteWizard = function(options)
{
    var name = x.getFriendlyName(location.pathname + '$favorite$wizard');

    if(typeof (window[name]) === 'undefined')
    {
        // 配置参数
        options.localStorage = '{"text":"' + document.title + '","value":"' + location.href + '"}';

        // 初始化向导
        var wizard = x.wizards.newFavoriteWizardWizard(name, options.categoryIndex, options.prefixCategoryIndex, options);

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