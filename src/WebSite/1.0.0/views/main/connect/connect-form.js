x.register('main.connect.form');

main.connect.form = {

    /*#region 函数:save()*/
    save: function()
    {
        if (!x.dom.data.check())
        {
            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
            outString += '</request>';

            x.net.xhr('/api/connect.save.aspx', outString, {
                waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
                callback: function(response)
                {
                    if ($('#applicationName').val() == 'PersonalSettings')
                    {
                        location.href = '/account/settings/applications';
                    }
                    else
                    {
                        history.back();
                    }
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
        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        // [空]
    }
    /*#endregion*/
};

$(document).ready(main.connect.form.load);