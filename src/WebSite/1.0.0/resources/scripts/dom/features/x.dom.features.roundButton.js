/**
* form feature  : checkbox(复选框)
*
* require       : x.js, x.dom.js
*/
x.dom.features.checkbox = {

    /**
    * 绑定
    */
    bind: function(inputName)
    {
        // 参数初始化
        var input = x.dom.query(inputName);

        var maskName = inputName + '$$mask';

        var viewName = inputName + '$$view';

        var originalValue = input.val();

        var callback = (typeof (input.attr('callback')) == 'undefined') ? undefined : input.attr('callback');

        // 隐藏原始对象
        input.wrap('<div id="' + maskName + '" style="display:none;" ></div>');

        // 设置新的显示元素
        var outString = '';

        if (x.net.browser.brand('ie') && x.net.browser.getVersion() < 7)
        {
            // IE 6
            outString += '<input id="' + viewName + '" name="' + viewName + '" type="checkbox" value="' + originalValue + '" ' + (originalValue === '1' ? 'checked=checked' : '') + '/>';

            x.dom.query(maskName).after(outString);

            // 绑定相关事件
            x.dom.query(viewName).bind('click', function(event)
            {
                x.dom.query(this.id.replace('$$view', '')).val(this.checked ? '1' : '0');
                x.dom.query(this.id.replace('$$view', ''))[0].checked = this.checked;

                // 保存后执行的回调方法
                x.call(callback);
            });
        }
        else
        {
            outString += '<span class="input-checkbox-wrapper" style="' + input.attr('style') + '" >';
            outString += '<span class="input-checkbox-container" style="background: url(/resources/images/x-ajax-controls.png) -3px -41px no-repeat;" >';
            outString += '<label id="' + viewName + '" name="' + viewName + '" checkboxValue="' + originalValue + '" class="input-checkbox-ticked" ' + (originalValue === '1' ? 'style="background: url(/resources/images/x-ajax-controls.png) -3px -117px no-repeat;"' : '') + '/>';
            outString += '</span>';
            outString += '</span>';

            x.dom.query(maskName).after(outString);

            // 绑定相关事件
            x.dom.query(viewName).bind('click', function(event)
            {
                var value = ($(this).attr('checkboxValue') == '1') ? 0 : 1;

                x.debug.log(value);
                // x.dom.query(this.id.replace('$$view$$cotainer', '')).val(value);

                x.dom.query(this.id.replace('$$view', '')).val(value);
                x.dom.query(this.id.replace('$$view', ''))[0].checked = (value === 1) ? true : false;

                if (value === 1)
                {
                    $(this).css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -117px no-repeat' });
                    $(this).parent().css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -41px no-repeat' });
                }
                else
                {
                    $(this).css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -41px no-repeat' });
                }

                $(this).attr('checkboxValue', value);

                // 保存后执行的回调方法
                x.call(callback);
            });

            x.dom.query(viewName).bind('mousedown', function(event)
            {
                var value = ($(this).attr('checkboxValue') == '1') ? 1 : 0;

                if (value === 1)
                {
                    $(this).css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -155px no-repeat' });
                    $(this).parent().css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -61px no-repeat' });
                }
                else
                {
                    $(this).css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -61px no-repeat' });
                    $(this).parent().css({ 'background': 'url(/resources/images/x-ajax-controls.png) -3px -41px no-repeat' });
                }
            });

            // 禁止双击图片时选中文本信息
            x.dom.query(viewName).parent().parent().parent().bind('selectstart', function(event)
            {
                return false;
            });
        }
    }
};