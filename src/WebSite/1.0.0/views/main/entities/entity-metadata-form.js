x.register('main.entities.entity.metadata.form');

main.entities.entity.metadata.form = {

    /*#region 函数:save()*/
    /*
    * 保存
    */
    save: function()
    {
        if(!x.dom.data.check())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
            outString += '</request>';

            x.net.xhr('/api/kernel.entities.metadata.save.aspx', outString, {
                waitingMessage: i18n.net.waiting.saveTipText,
                popResultValue: 1,
                callback: function(response)
                {
                    x.page.refreshParentWindow();
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
        // 初始化页签控件
        x.ui.pkg.tabs.newTabs();
    }
    /*#endregion*/
};

$(document).ready(main.entities.entity.metadata.form.load);