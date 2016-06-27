/**
* form feature  : time(时间选择向导)
*
* require	    : x.js, x.dom.js
*/
x.dom.features.time = {

  /*
  * 绑定日期
  */
  bind: function(inputName)
  {
    x.require({
      files: [
          { fileType: 'script', id: 'x-ui-pkg-calendar-script', path: x.ui.pkg.dir() + 'x.ui.pkg.calendar.js' },
          { fileType: 'script', id: 'x-ui-pkg-combobox-script', path: x.ui.pkg.dir() + 'x.ui.pkg.combobox.js' }
      ],
      data: { inputName: inputName },
      callback: function(context)
      {
        var data = context.data;

        // <input id="dateView" name="dateView" type="text" />
        // <div id="dateView-calendar" style="display:none;" ></div>

        // 参数初始化
        var input = x.dom.query(data.inputName);

        var maskName = x.getFriendlyName(data.inputName + '-mask');

        var viewName = x.getFriendlyName(data.inputName + '-view');
        // 设置日历控件
        var calendarInputName = data.inputName + '-calendar-value';
        var calendarName = x.getFriendlyName(calendarInputName);
        // 设置时间控件
        var timeInputName = data.inputName + '-time-value';
        var timeName = x.getFriendlyName(timeInputName);

        var time = input.val() == '' ? x.date.newTime() : x.date.newTime(input.val());

        var defaultCalendarValue = typeof (input.attr('defaultCalendarValue')) == 'undefined' ? (time == null ? '' : time.toString('yyyy-MM-dd')) : input.attr('defaultCalendarValue');
        var defaultTimeValue = typeof (input.attr('defaultTimeValue')) == 'undefined' ? (time == null ? '' : time.toString('HH:mm')) : input.attr('defaultTimeValue');

        var timeBeginHourValue = Number(typeof (input.attr('timeBeginHourValue')) == 'undefined' ? '0' : input.attr('timeBeginHourValue'));
        var timeEndHourValue = Number(typeof (input.attr('timeEndHourValue')) == 'undefined' ? '24' : input.attr('timeEndHourValue'));

        // 隐藏原始对象
        input.wrap('<div id="' + maskName + '" style="display:none;" ></div>');

        // 设置新的显示元素
        x.dom.query(maskName).after('<div class="form-inline" >'
                              + '<div class="input-group" style="width:180px" >'
                              + '<div class="input-group dropdown" >'
                              + '<input id="' + calendarInputName + '" name="' + calendarInputName + '" type="text" value="' + defaultCalendarValue + '" class="form-control x-ui-pkg-calendar-dummy" style="width:120px;" />'
                              + '<div id="' + calendarName + '" class="x-ui-pkg-calendar-wrapper dropdown-menu" ></div>'
                              + '<div class="input-group-addon"><i class="glyphicon glyphicon-calendar" ></i></div>'
                              + '</div>'
                              + '</div>'
                              + '<div class="input-group dropdown">'
                              + '<input id="' + timeInputName + '" name="' + timeInputName + '" value="' + defaultTimeValue + '" type="text" data-toggle="dropdown" class="form-control " style="width:70px;" /> '
                              + '<div id="' + timeInputName + '-wrapper" class="x-ui-pkg-combobox-wrapper dropdown-menu" style="height:200px;" ><div id="' + timeName + '" style="display:none;" ></div></div>'
                              + '<div class="input-group-addon"><span class="glyphicon glyphicon-time" ></span></div>'
                              + '</div>'
                              + '</div>');

        // 日历
        window[calendarName] = x.ui.pkg.calendar.newCalendar(calendarName, calendarName, calendarInputName, defaultCalendarValue, {
          getDayEvent: function()
          {
            x.dom.features.time.setValue(inputName);
          }
        });

        $(document).on('click', function(event)
        {
          var target = window.event ? window.event.srcElement : event ? event.target : null;

          var list = $('.x-ui-pkg-calendar-wrapper');

          for(var i = 0;i < list.length;i++)
          {
            if(target.id == list[i].id)
            {
              continue;
            }

            if(x.getFriendlyName(target.id) == list[i].id || target.className.indexOf('x-ui-pkg-calendar-dummy') > -1)
            {
              continue;
            }

            window[list[i].id].close();
          }
        });

        $('#' + calendarInputName).on('click', function()
        {
          window[x.getFriendlyName(this.id)].open();
        });

        $('#' + calendarInputName).on('blur', function()
        {
          if(this.value != '' && !x.expressions.exists({ text: this.value, regexpName: 'date' }))
          {
            x.msg('请填写正确的日期，例如【2000-01-01】。');
            this.focus();
          }
        });

        // 时间

        var options = {
          show: 'text',
          topOffset: '-1',
          widthOffset: '0',
          selectedValue: '00:00',
          comboboxType: 'static',
          list: [],
          callback: function() { x.dom.features.time.setValue(inputName); }
        };

        var timeText = '[';

        for(var i = timeBeginHourValue;i < timeEndHourValue;i++)
        {
          timeText += '{text:\'' + ((i < 10) ? '0' : '') + i + ':00\',value:\'' + ((i < 10) ? '0' : '') + i + ':00\'},';
          timeText += '{text:\'' + ((i < 10) ? '0' : '') + i + ':30\',value:\'' + ((i < 10) ? '0' : '') + i + ':30\'}';

          if((i + 1) < timeEndHourValue)
          {
            timeText += ',';
          }

          if((i + 1) == timeEndHourValue && i < 23)
          {
            timeText += ',{text:\'' + ((i + 1 < 10) ? '0' : '') + (i + 1) + ':00\',value:\'' + ((i + 1 < 10) ? '0' : '') + (i + 1) + ':00\'}';
          }
        }

        timeText += ']';

        options.list = x.toJSON(timeText);

        window[timeName] = x.ui.pkg.combobox.newCombobox(timeName, timeName, timeInputName, timeInputName, options);

        $('#' + timeInputName).on('focus', function()
        {
          window[x.getFriendlyName(this.id)].open();
        });

        $('#' + timeInputName).on('keyup', function()
        {
          window[x.getFriendlyName(this.id)].open();
        });

        $(document).on('click', function(event)
        {
          var target = window.event ? window.event.srcElement : event ? event.target : null;

          var list = $('.x-ui-pkg-combobox-wrapper');

          for(var i = 0;i < list.length;i++)
          {
            if(target.id != list[i].id.replace('-wrapper', '') && target.id != list[i].id.replace('-wrapper', '-time'))
            {
              var targetObject = window[list[i].id.replace('-wrapper', '') + '-time'];

              if(targetObject != null)
              {
                targetObject.close();
              }
            }
          }
        });
      }
    });
  },

  setValue: function(inputName)
  {
    var calendarValue = $('#' + inputName + '-calendar-value').val();
    var timeValue = $('#' + inputName + '-time-value').val();

    var value = calendarValue + ' ' + timeValue;

    x.debug.log(value);

    x.dom.query(inputName).val(value);
  }
};