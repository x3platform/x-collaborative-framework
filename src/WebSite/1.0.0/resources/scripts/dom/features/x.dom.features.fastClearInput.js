/**
* form feature  : checkbox(复选框)
*
* require       : x.js, x.dom.js
*/
x.dom.features.fastClearInput = {

    /**
    * 绑定
    */
    bind: function(inputName)
    {
        var input = x.dom.query(inputName);

        // 绑定相关事件
        input.bind('click', function(event)
        {
            this.select();
        });

        // 绑定相关事件
        input.bind('keydown', function(event)
        {
            var event = x.getEventObject(event);

            var target = x.getEventTarget(event);

            // 只允许按Delete键和Backspace键
            if (event.keyCode == 8 || event.keyCode == 46)
            {
                $(target).val('');

                // 清除上下文的属性
                var input = x.dom.query(target.id);

                var ids = input.attr('fastClearInputHiddenIds');

                if (ids != null)
                {
                    var list = ids.split(',');

                    for (var i = 0; i < list.length; i++)
                    {
                        x.dom.query(list[i]).val('');
                    }
                }
            }
            else
            {
                x.stopEventPropagation();
            }
        });
    }
}; 