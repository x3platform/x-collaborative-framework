/**
* form feature  : combobox(组合框)
*
* require       : x.js, x.dom.js, x.combobox.js
*/
x.dom.features.combobox = {

  /**
  * 绑定
  */
  bind: function(inputName)
  {
    // x.config.net.require['x-ui-combobox'].path;

    x.require({
      files: [
          { fileType: 'script', id: 'x-ui-pkg-combobox-script', async: false, path: x.ui.pkg.dir() + 'x.ui.pkg.combobox.js' }
      ],
      callback: function(context)
      {
        // <div id="combobox1_mask" style="display:none;" ><input id="combobox1" /></div>
        // <input id="combobox1_view" style="width:160px;" />
        // <div><div id="combobox1_combobox" style="display:none;" ></div></div>
        var classNamePrefix = x.ui.classNamePrefix;

        var input = x.dom.query(inputName);

        var options = x.dom.options(inputName);

        var maskName = classNamePrefix + '-' + inputName + '-mask';

        var viewName = classNamePrefix + '-' + inputName + '-view';

        var comboboxName = classNamePrefix + '-' + inputName + '-combobox';

        var selectedText = (typeof (input.attr('selectedText')) == 'undefined') ? '' : input.attr('selectedText');

        if(typeof (input.attr('url')) != 'undefined')
        {
          input.attr('comboboxType', 'dynamic');
        }

        input.wrap('<div id="' + maskName + '" style="display:none;" ></div>');

        $('#' + maskName).after('<input id="' + viewName + '" readonly="readonly" name="' + viewName + '" type="text" value="' + selectedText + '" data-toggle="dropdown" class="' + input.attr('class') + '" style="' + input.attr('style') + '" />');

        $('#' + viewName).wrap('<div class="form-inline"></div>');
        $('#' + viewName).wrap('<div class="form-group"></div>');
        $('#' + viewName).wrap('<div id="' + classNamePrefix + '-' + inputName + '-wrapper" class="input-group dropdown"></div>');
        $('#' + viewName).after('<div class="input-group-addon"><span class="glyphicon glyphicon-list" ></span></div>');
        $('#' + viewName).after('<div id="' + comboboxName + '" class="dropdown-menu" style="display:none;" ></div>');

        // 交换相关验证组件需要的标签信息
        x.dom.swap({ from: inputName, to: viewName, attributes: ['dataVerifyWarning', 'dataRegExpWarning', 'dataIgnoreCase', 'dataRegExpName', 'dataRegExp'] });

        var iconHidden = (typeof (input.attr('comboboxIconHidden')) == 'undefined') ? '0' : input.attr('comboboxIconHidden');

        var inputView = $('#' + viewName);

        //if(inputView.width() > 20 && input.attr('hiddeIcon') != '1' && iconHidden == '0')
        //{
        //    inputView.css({
        //        'background-image': 'url("/resources/images/form/combobox_icon.gif")',
        //        'background-repeat': 'no-repeat',
        //        'background-position': (inputView.width() - 20) + 'px 2px'
        //    });
        //}

        // x-dom-options='{show:text}'

        var options = {
          show: (typeof (input.attr('show')) == 'undefined') ? 'text' : input.attr('show'),
          topOffset: (typeof (input.attr('topOffset')) == 'undefined' || input.attr('topOffset') == '') ? '0' : input.attr('topOffset'),
          widthOffset: (typeof (input.attr('widthOffset')) == 'undefined' || input.attr('widthOffset') == '') ? '0' : input.attr('widthOffset'),
          selectedValue: input.val(),
          comboboxEmptyItemText: (typeof (input.attr('comboboxEmptyItemText')) == 'undefined') ? undefined : input.attr('comboboxEmptyItemText'),
          comboboxType: (typeof (input.attr('comboboxType')) == 'undefined') ? 'static' : input.attr('comboboxType'),
          comboboxIconHidden: (typeof (input.attr('comboboxIconHidden')) == 'undefined') ? '0' : input.attr('comboboxIconHidden'),
          ajaxMethod: (typeof (input.attr('ajaxMethod')) == 'undefined') ? undefined : input.attr('ajaxMethod'),
          callback: (typeof (input.attr('callback')) == 'undefined') ? undefined : input.attr('callback')
        };

        if(options.comboboxType == 'static')
        {
          options.list = x.toJSON(input.attr('data'));
        }
        else if(options.comboboxType == 'dynamic')
        {
          options.url = input.attr('url');

          if(typeof (input.attr('comboboxWhereClause')) != 'undefined')
          {
            options.whereClause = input.attr('comboboxWhereClause');
          }
          else
          {
            options.whereClause = ' Name LIKE ##%{0}%## ';
          }
        }

        window[comboboxName] = x.ui.pkg.combobox.newCombobox(comboboxName, comboboxName, viewName, inputName, options);

        inputView.bind('focus', function()
        {
          window[(this.id.replace('-view', '') + '-combobox')].open();
        });

        inputView.on('keyup', function()
        {
          window[(this.id.replace('-view', '') + '-combobox')].open();
        });

        $(document).on('click', function(event)
        {
          var target = x.event.getTarget(event);

          var list = $('.' + classNamePrefix + '-combobox-wrapper');

          for(var i = 0;i < list.length;i++)
          {
            if(target.id != list[i].id.replace('-wrapper', '') && target.id != list[i].id.replace('-wrapper', '-view'))
            {
              var targetObject = window[list[i].id.replace('-wrapper', '') + '-combobox'];

              if(targetObject != null)
              {
                targetObject.close();
              }
            }
          }
        });
      }
    });
  }
};