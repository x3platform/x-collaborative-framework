﻿x.register('main.applications.application.form');

main.applications.application.form = {

    /*#region 函数:save()*/
    save: function()
    {
        if(!x.dom.data.check())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' });
            outString += '</request>';

            x.net.xhr('/api/application.save.aspx', outString, {
                waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
                callback: function(response)
                {
                    var result = x.toJSON(response).message;

                    // 如果有父级窗口，则调用父级窗口默认刷新函数。
                    x.page.refreshParentWindow();

                    x.msg(result.value);

                    x.page.close();
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
        x.ui.pkg.tabs.newTabs();

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#applicationName').on('keydown', function()
        {
            // 应用名称只能是英文字符
            this.value = x.expression.formatEnglish(this.value);
        });

        $('#applicationName').on('keyup', function()
        {
            // 应用名称只能是英文字符
            this.value = x.expression.formatEnglish(this.value);
        });
    }
    /*#endregion*/
};

$(document).ready(main.applications.application.form.load);
