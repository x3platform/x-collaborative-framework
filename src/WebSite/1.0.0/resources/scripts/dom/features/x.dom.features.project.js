/**
* form feature  : project(工作流模板选择向导)
*
* require       : x.js, x.form.js
*/
x.dom.features.project = {

  bind: function(inputName)
  {
    x.require({
      files: [
          { fileType: 'script', id: 'shared-wizards-project-wizard-script', async: false, path: '/views/shared/wizards/project-wizard.js' }
      ],
      callback: function(context)
      {
        // 检测外部资源文件加载
        if(typeof (x.ui.wizards) === 'undefined' || typeof (x.ui.wizards.newProjectWizard) === 'undefined')
        {
          x.msg('You need to load the file -> /views/shared/wizards/project-wizard.js');
          return;
        }

        // 绑定数据(data标签)
        x.dom.data.bindInputData({
          featureName: 'project',
          inputName: inputName,
          multiSelection: $('#' + inputName).attr('projectMultiSelection') == '1' ? 1 : 0
        });

        // <input id="dateView" name="dateView" type="text" />
        // <div id="dateView_calendar" style="display:none;" ></div>

        var input = $('#' + inputName);

        var maskName = inputName + '-mask';

        var viewName = inputName + '-view';

        var width = input.width();
        var height = input.height();

        // 隐藏原始对象
        input.wrap('<div id="' + maskName + '" style="display:none;" ></div>');

        // 设置新的显示元素
        var outString = '';

        // x.debug.log('input height:' + input.height());

        var projectMultiSelection = input.attr('projectMultiSelection');

        var selectedText = x.isUndefined(input.attr('selectedText'), '');

        var callback = x.isUndefined(input.attr('callback'), '');

        var url = 'javascript:x.ui.wizards.' + ((projectMultiSelection === '1') ? 'getProjectsWizard' : 'getProjectWizard') + '({';

        url += 'targetViewName:\'' + viewName + '\',targetValueName:\'' + inputName + '\',callback:\'' + x.toSafeJSON(callback) + '\'});';

        if(height > 20)
        {
          // 多行文本框
          outString += '<textarea id="' + viewName + '" name="' + viewName + '" readonly="readonly" ';
          outString += 'class="textarea-normal ' + input.attr('class') + '" style="' + input.attr('style') + '" >' + selectedText + '</textarea>';
          outString += '<div style="text-align:right; width:' + width + 'px;" >';
          outString += '<a id="' + inputName + '-btnEdit" href="' + url + '" >编辑</a>';
          outString += '</div>';
        }
        else
        {
          // 单行文本框
          outString += '<div class="input-group">';
          outString += '<input id="' + viewName + '" name="' + viewName + '" type="text" readonly="readonly" ';
          outString += 'class="' + input.attr('class') + '" style="' + input.attr('style') + '" value="' + selectedText + '"/> ';
          outString += '<a id="' + inputName + '-btnEdit" href="' + url + '" class="input-group-addon" title="编辑"><i class="glyphicon glyphicon-modal-window"></i></a>';
          outString += '</div>';
        }

        $('#' + maskName).after(outString);
        // 
        // 交换相关验证组件需要的标签信息
        x.dom.swap({ from: inputName, to: viewName, attributes: ['x-dom-data-verify-warning', 'x-dom-data-regexp-warning', 'x-dom-data-ignoreCase', 'x-dom-data-regexp-name', 'x-dom-data-regexp'] });

        // x.wizards.setContactsView(viewName, input.val());
      }
    });
  }
};