/**
* form feature  : corporation(公司选择向导)
*
* require       : x.js, x.dom.js
*/
x.dom.features.corporation = {

  /**
  * 绑定
  */
  bind: function(inputName)
  {
    // 检测外部资源文件加载
    if(typeof (x.wizards) === 'undefined' || typeof (x.wizards.newCorporationWizard) === 'undefined')
    {
      x.debug.error('需加载资源文件【/apps/pages/wizards/corporation-wizard.js】。');
      return;
    }

    // 绑定数据(data标签)
    x.customForm.bindInputData({
      featureName: 'corporation',
      inputName: inputName,
      multiSelection: x.dom.query(inputName).attr('corporationMultiSelection') == '1' ? 1 : 0
    });

    // 参数初始化
    var input = x.dom.query(inputName);

    var maskName = inputName + '$$mask';

    var viewName = inputName + '$$view';

    var width = input.width();
    var height = input.height();

    // 隐藏原始对象
    input.wrap('<div id="' + maskName + '" style="display:none;" ></div>');

    // 设置新的显示元素
    var outString = '';

    // x.debug.log('input height:' + input.height());
    // var corporationMultiSelection = input.attr('corporationMultiSelection');

    var selectedText = x.isUndefined(input.attr('selectedText'), '');

    var callback = x.isUndefined(input.attr('callback'), '');

    var url = 'javascript:x.wizards.getCorporationWizard({targetViewName:\'' + viewName + '\',targetValueName:\'' + inputName + '\',callback:\'' + x.toSafeJSON(callback) + '\'});';

    if(height > 20)
    {
      // 多行文本框
      outString += '<textarea id="' + viewName + '" name="' + viewName + '" readonly="readonly" ';
      outString += 'class="textarea-normal ' + input.attr('class') + '" style="' + input.attr('style') + '" >' + selectedText + '</textarea>';
      outString += '<div style="text-align:right; width:' + width + 'px;" >';
      outString += '<a id="' + inputName + '$$btnEdit" href="' + url + '" >编辑</a>';
      outString += '</div>';
    }
    else
    {
      // 单行文本框
      outString += '<input id="' + viewName + '" name="' + viewName + '" type="text" readonly="readonly" ';
      outString += 'class="' + input.attr('class') + '" style="' + input.attr('style') + '" value="' + selectedText + '" /> ';
      outString += '<a id="' + inputName + '$$btnEdit" href="' + url + '" >编辑</a>';
    }

    $(document.getElementById(maskName)).after(outString);

    // 交换相关验证组件需要的标签信息
    x.dom.swap({ from: inputName, to: viewName, attributes: ['dataVerifyWarning', 'dataRegExpWarning', 'dataIgnoreCase', 'dataRegExpName', 'dataRegExp'] });
  }
};