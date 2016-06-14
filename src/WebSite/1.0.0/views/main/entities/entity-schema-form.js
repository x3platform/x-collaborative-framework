(function(x, window)
{
    var main = {

        /*#region 函数:save()*/
        /**
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

                x.net.xhr('/api/kernel.entities.schema.save.aspx', outString, {
                    waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
                    popCorrectValue: 1,
                    callback: function(response)
                    {
                        x.page.refreshParentWindow();
                        x.page.close();
                    }
                });
            }
        },
        /*#endregion*/

        load: function()
        {
            // 初始化页签控件
            x.ui.pkg.tabs.newTabs();

            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            $('#btnSave').on('click', function()
            {
                main.save();
            });

            $('#btnCancel').on('click', function()
            {
                x.page.close();
            });
        }
    };

    // 页面初始化加载
    x.dom.ready(main.load);

    // 注册到 Window 对象
    window.main = main;

})(x, window);