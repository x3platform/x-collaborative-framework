x.register('main.applications.application.menu.form');

main.applications.application.menu.form = {

    /*#region 函数:checkObject()*/
    /*
    * 检测对象的必填数据
    */
    checkObject: function()
    {
        if(x.dom.data.check())
        {
            return false;
        }

        return true;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
        if(main.applications.application.menu.form.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' });
            outString += '</request>';

            x.net.xhr('/api/application.menu.save.aspx', outString, {
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

        $('#btnEditAuthorizationReadScopeObjectAsEveryone').on('click', function()
        {
            $('#authorizationReadScopeObjectView').val('所有人;');
            $('#authorizationReadScopeObjectText').val('role#00000000-0000-0000-0000-000000000000#所有人;');
        });

        $('#btnEditAuthorizationReadScopeObject').on('click', function()
        {
            x.ui.wizards.getContactsWizard({ targetViewName: 'authorizationReadScopeObjectView', targetValueName: 'authorizationReadScopeObjectText', contactType: 'all' });
        });
    }
    /*#endregion*/
};

$(document).ready(main.applications.application.menu.form.load);
