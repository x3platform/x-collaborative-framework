x.register('main.customizes.customize.layout.form');

main.customizes.customize.layout.form = {

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
        if(main.customizes.customize.layout.form.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml' });
            outString += '</request>';

            x.net.xhr('/api/web.customizes.customizeLayout.save.aspx', outString, {
                waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
                callback: function(response)
                {
                    var result = x.toJSON(response).message;

                    x.msg(result.value);

                    // 如果有父级窗口，则调用父级窗口默认刷新函数。
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
        // -------------------------------------------------------
        // 初始化代码编辑器
        // -------------------------------------------------------

        var textarea = $('textarea[id="html"]');

        var editor = ace.edit("html-editor");
        // 设置主题 
        editor.setTheme("ace/theme/github");
        // 设置程序语言模式
        editor.getSession().setMode("ace/mode/html");
        // 设置制表符的大小 
        editor.getSession().setTabSize(2);
        // 设置打印边距是否可见
        editor.setShowPrintMargin(false);
        // 设置内容
        editor.getSession().setValue(html_beautify(textarea.val(), { indent_size: 2 }));
        // 设置事件
        editor.getSession().on('change', function()
        {
            textarea.val(editor.getSession().getValue().replace(/\n/g, ''));
        });

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        $('#btnSave').on('click', function()
        {
            main.customizes.customize.layout.form.save();
        });

        $('#btnCancel').on('click', function()
        {
            x.page.close();
        });
    }
    /*#endregion*/
};

$(document).ready(main.customizes.customize.layout.form.load);
