/**
* form feature  : contacts(人员选择框)
*
* require       : x.js, x.dom.js
*/
x.dom.features.contacts = {

    bind: function(inputName)
    {
        if (typeof (x.wizards) === 'undefined' || typeof (x.wizards.newContactsWizard) === 'undefined')
        {
            x.debug.error('需加载资源文件【/views/shared/wizards/contacts-wizard.js】。');
            return;
        }

        // 绑定数据(data标签)
        x.customForm.bindInputData({
            featureName: 'contacts',
            inputName: inputName,
            multiSelection: x.dom.query(inputName).attr('contactMultiSelection') == '1' ? 1 : 0
        });

        // <input id="dateView" name="dateView" type="text" />
        // <div id="dateView_calendar" style="display:none;" ></div>

        var input = x.dom.query(inputName);

        var maskName = inputName + '$$mask';

        var viewName = inputName + '$$view';

        var width = input.width();
        var height = input.height();

        // 隐藏原始对象
        input.wrap('<div id="' + maskName + '" style="display:none;" ></div>');

        // 设置新的显示元素
        var outString = '';

        // x.debug.log('input height:' + height);

        var contactTypeText = input.attr('contactTypeText');
        var contactMultiSelection = input.attr('contactMultiSelection');

        var selectedText = x.isUndefined(input.attr('selectedText'), '');

        var callback = x.isUndefined(input.attr('callback'), '');

        if (typeof (contactTypeText) == 'undefined')
        {
            // 单选默认只能选择人员
            // 多选默认选择人员、角色、组织
            contactTypeText = (typeof (contactMultiSelection) == 'undefined') ? 'account' : 'default';
        }

        var url = 'javascript:x.wizards.' + ((contactMultiSelection === '1') ? 'getContactsWizard' : 'getContactWizard') + '({';

        url += 'targetViewName:\'' + viewName + '\',targetValueName:\'' + inputName + '\',contactTypeText:\'' + contactTypeText + '\',callback:\'' + x.toSafeJSON(callback) + '\'});';

        if (height > 20)
        {
            // 多行文本框
            outString += '<textarea id="' + viewName + '" name="' + viewName + '" readonly="readonly" class="textarea-normal ' + input.attr('class') + '" style="' + input.attr('style') + '" ></textarea>'
            outString += '<div style="text-align:right; width:' + width + 'px;" >'
            outString += '<a id="' + inputName + '$$btnEdit" href="' + url + '" >编辑</a>';
            outString += '</div>';
        }
        else
        {
            // 单行文本框
            outString += '<input id="' + viewName + '" name="' + viewName + '" type="text" readonly="readonly" class="' + input.attr('class') + '" style="' + input.attr('style') + '" /> '
            outString += '<a id="' + inputName + '$$btnEdit" href="' + url + '" >编辑</a>';
        }

        $(document.getElementById(maskName)).after(outString);

        // 交换相关验证组件需要的标签信息
        x.dom.swap({ from: inputName, to: viewName, attributes: ['dataVerifyWarning', 'dataRegExpWarning', 'dataIgnoreCase', 'dataRegExpName', 'dataRegExp'] });
        
        if (selectedText === '')
        {
            // 如果不填写所选的文本, 根据值自动设置显示文本信息
            x.wizards.setContactsView(viewName, input.val());
        }
        else
        {
            x.dom.query(viewName).val(selectedText);
        }
    }
};