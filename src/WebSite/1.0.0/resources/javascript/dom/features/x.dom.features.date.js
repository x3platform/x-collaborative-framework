/**
* form feature  : date(日期选择框)
*
* require	    : x.js, x.dom.js
*/
x.dom.features.date = {

  /*
  * 绑定日期
  */
  bind: function(inputName)
  {
    x.require({
      files: [
          { fileType: 'script', id: 'x-ui-pkg-calendar-script', path: x.ui.pkg.dir() + 'x.ui.pkg.calendar.js' }
      ],
      data: { inputName: inputName },
      callback: function(context)
      {
        var data = context.data;

        // 参数初始化
        var calendarName = x.getFriendlyName(data.inputName + '-calendar');

        var input = $('#' + data.inputName);

        // eval(x.dom.query(inputName).attr('bind'));

        // 设置新的显示元素

        input.wrap('<div class="input-group dropdown"></div>');
        input.after('<div class="input-group-addon"><span class="glyphicon glyphicon-calendar" ></span></div>');
        input.after('<div id="' + calendarName + '" style="display:none;" ></div>');
        // input.after('<div ><div id="xQueryBeginDateCalendar-innerContainer" class="x-ui-pkg-calendar"><div class="x-ui-pkg-calendar-header"><div class="x-ui-pkg-calendar-header-button"><a href="javascript:xQueryBeginDateCalendar.jump(\'2014-5-01\')"><img src="/resources/images/calendar/calendar_up_year.png" class="x-ui-pkg-calendar-dummy"></a>&nbsp;&nbsp;<a href="javascript:xQueryBeginDateCalendar.jump(\'2015-4-01\')"><img src="/resources/images/calendar/calendar_up.png" class="x-ui-pkg-calendar-dummy"></a></div> <div class="x-ui-pkg-calendar-header-day">2015 - 05</div><div class="x-ui-pkg-calendar-header-button"><a href="javascript:xQueryBeginDateCalendar.jump(\'2015-6-01\')"><img src="/resources/images/calendar/calendar_down.png" class="x-ui-pkg-calendar-dummy"></a>&nbsp;&nbsp;<a href="javascript:xQueryBeginDateCalendar.jump(\'2016-5-01\')"><img src="/resources/images/calendar/calendar_down_year.png" class="x-ui-pkg-calendar-dummy"></a></div><div class="clear"></div></div><div class="x-ui-pkg-calendar-days"><div class="x-ui-pkg-calendar-day-week">日</div><div class="x-ui-pkg-calendar-day-week">一</div><div class="x-ui-pkg-calendar-day-week">二</div><div class="x-ui-pkg-calendar-day-week">三</div><div class="x-ui-pkg-calendar-day-week">四</div><div class="x-ui-pkg-calendar-day-week">五</div><div class="x-ui-pkg-calendar-day-week">六</div><div class="x-ui-pkg-calendar-day"> &nbsp; </div><div class="x-ui-pkg-calendar-day"> &nbsp; </div><div class="x-ui-pkg-calendar-day"> &nbsp; </div><div class="x-ui-pkg-calendar-day"> &nbsp; </div><div class="x-ui-pkg-calendar-day"> &nbsp; </div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-1\');">1</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-2\');">2</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-3\');">3</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-4\');">4</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-5\');">5</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-6\');">6</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-7\');">7</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-8\');">8</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-9\');">9</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-10\');">10</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-11\');">11</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-12\');">12</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-13\');">13</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-14\');">14</a></div><div class="x-ui-pkg-calendar-today"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-15\');">15</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-16\');">16</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-17\');">17</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-18\');">18</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-19\');">19</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-20\');">20</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-21\');">21</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-22\');">22</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-23\');">23</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-24\');">24</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-25\');">25</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-26\');">26</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-27\');">27</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-28\');">28</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-29\');">29</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-30\');">30</a></div><div class="x-ui-pkg-calendar-day"><a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-31\');">31</a></div><div class="clear"></div></div><div class="x-ui-pkg-calendar-footer"><div class="x-ui-pkg-calendar-button-close"><a href="javascript:xQueryBeginDateCalendar.close();"><img src="/resources/images/calendar/calendar_close.png"></a></div><strong>今天: <a href="javascript:xQueryBeginDateCalendar.getDay(\'2015-5-15\');">2015-5-15</a></strong></div></div></div>');
        // 

        var calendar = window[calendarName] = x.ui.pkg.calendar.newCalendar(calendarName, calendarName, inputName, '', {
          getDayEvent: function()
          {
            if(typeof (this.input.attr('bind')) !== 'undefined')
            {
              eval(this.input.attr('bind'));
            }
          }
        });

        $('#' + calendarName).attr('class', calendar.options.classNamePrefix + '-wrapper dropdown-menu');

        // x.debug.log(window[calendarName]);
        /*
        if(input.width() < 85)
        {
            input.css('width', '85px');
        }

        if(input.width() > 20)
        {
            input.css({
                'background-image': 'url("/resources/images/form/calendar_icon.gif")',
                'background-repeat': 'no-repeat',
                'background-position': (input.width() - 20) + 'px 2px'
            });
        }
        */

        $(document).on('click', function(event)
        {
          var target = window.event ? window.event.srcElement : event ? event.target : null;

          var list = $('.' + calendar.options.classNamePrefix + '-wrapper');

          targetName = x.getFriendlyName(target.id + '-calendar');

          for(var i = 0;i < list.length;i++)
          {
            if(target.id == list[i].id)
            {
              continue;
            }

            if(targetName == list[i].id || target.className.indexOf(calendar.options.classNamePrefix + '-dummy') > -1)
            {
              continue;
            }

            window[list[i].id].close();
          }
        });

        input.on('click', function()
        {
          // window[(this.id + '_calendar')].open();
          window[calendarName].open();
        });

        input.on('blur', function()
        {
          if(this.value != '' && !x.expressions.exists({ text: this.value, regexpName: 'date' }))
          {
            x.msg('请填写正确的日期，例如【2000-01-01】。');
            this.focus();
          }
        });
      }
    });
  }
};