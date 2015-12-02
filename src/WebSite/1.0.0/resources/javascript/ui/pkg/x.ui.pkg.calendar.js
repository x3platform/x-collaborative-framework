// -*- ecoding=utf-8 -*-

/**
* 日历
*
* require	: x.js
*/
x.ui.pkg.calendar = {

    // 默认配置
    defaults: {
        /** 样式名称默认前缀 */
        classNamePrefix: 'x-ui-pkg-calendar',
        // 图片资源路径
        imageResourcePath: '/resources/images/calendar/',
        // 今天的文本信息
        todayText: '今天',
        // 星期日至星期六的文本信息
        dayOfWeekNameText: ['日', '一', '二', '三', '四', '五', '六']
    },

    newCalendar: function(name, containerName, inputName, defaultDateValue, options)
    {
        options = x.ext({}, x.ui.pkg.calendar.defaults, options || {});

        var calendar = {

            name: name,

            options: options,

            container: x.dom.query(containerName),

            input: x.dom.query(inputName),

            // 内部容器对象
            innerContainer: null,

            daysOfMonth: 0,

            currentDate: new Date(),

            today: new Date(),

            lockObject: false,

            debug: '',

            getToday: function()
            {
                return this.currentDate.getFullYear() + "-" + (this.currentDate.getMonth() + 1) + "-" + this.currentDate.getDate();
            },

            getDayEvent: null,

            getDay: function(date)
            {
                this.input.val(date);

                this.close();

                // 重新设置验证组件
                if(this.input.hasClass('custom-forms-data-required') || this.input.hasClass('custom-forms-data-regexp'))
                {
                    x.customForm.checkDataInput(this.input[0], 1);
                }

                if(this.getDayEvent !== null && this.getDayEvent !== '')
                {
                    this.getDayEvent();
                }
            },

            open: function()
            {
                if(!this.lockObject)
                {
                    this.lockObject = true;

                    var dateValue = null;

                    if(this.input.val() == '')
                    {
                        dateValue = this.currentDate.getFullYear() + "-" + (this.currentDate.getMonth() + 1) + "-01";
                    }
                    else
                    {
                        dateValue = x.date.create(this.input.val()).toString('yyyy-MM-01');
                    }

                    this.create(dateValue);
                    /*
                    if (x.browser.ie)
                    {
                        // IE
                        this.container.css({
                            'vertical-align': 'baseline',
                            'margin-top': '-2px'
                        });
                    }
                    else if (x.browser.webkit)
                    {
                        // Chrome
                        this.container.css({
                            'position': 'absolute',
                            'top': ((x.page.getElementAbsoluteTop(this.input[0]) + x.page.getElementRange(this.input[0]).height) - 1) + 'px'
                        });
                    }
                    else
                    {
                        // Firefox
                        // this.container.css({ 'height': '1px',  'margin-top': '-1px' });
                        this.container.css({ 'height': '0px' });
                    }
                    */
                    this.container.css({ 'display': 'block' });

                    // this.innerContainer.slideDown('slow');
                    this.innerContainer.css({ 'display': 'block' });

                    // 取消验证组件
                    this.input.unbind('blur');
                }
            },

            close: function()
            {
                // 重新设置验证组件
                if(this.lockObject && (this.input.hasClass('custom-forms-data-required') || this.input.hasClass('custom-forms-data-regexp')))
                {
                    // x.customForm.checkDataInput(this.input[0], 1);
                }

                this.lockObject = false;

                // var that = this;

                // this.innerContainer.slideUp('slow', function()
                // {
                //    that.innerContainer.hide();
                // });

                this.innerContainer.css({ 'display': 'none' });

                this.container.css({ 'display': 'none' });
            },

            jump: function(dateValue)
            {
                this.create(dateValue);

                this.innerContainer.show();
            },

            toggle: function()
            {
                if(this.lockObject)
                {
                    this.close();
                }
                else
                {
                    this.open();
                }
            },

            /*
            * 创建日历
            *
            * dateValue
            */
            create: function(dateValue)
            {
                if(x.isFunction(this.options.getDayEvent))
                {
                    this.getDayEvent = this.options.getDayEvent;
                }

                var temp = dateValue.split('-');

                var date = new Date(temp[0], temp[1] - 1, temp[2]);

                var outString = '';

                var count = date.getDay();

                var dayOfWeekName = options.dayOfWeekNameText;

                var classNamePrefix = options.classNamePrefix;
                var imageResourcePath = options.imageResourcePath;

                outString += '<div id="' + this.name + '-innerContainer" class="' + classNamePrefix + '" style="display: none;" >';

                outString += '<div class="' + classNamePrefix + '-header">'
                          + '<div class="' + classNamePrefix + '-header-button" ><a href="javascript:' + this.name + '.jump(\'' + (date.getFullYear() - 1) + "-" + (date.getMonth() + 1) + '-01' + '\')" ><span class="glyphicon glyphicon-backward ' + classNamePrefix + '-dummy"><span></a>&nbsp;&nbsp;<a href="javascript:' + this.name + '.jump(\'' + date.getFullYear() + '-' + date.getMonth() + '-01' + '\')"><span class="glyphicon glyphicon-triangle-left ' + classNamePrefix + '-dummy"><span></a></div> '
                          + '<div class="' + classNamePrefix + '-header-day" >' + date.getFullYear() + ' - ' + ((date.getMonth() < 9) ? 0 : '') + (date.getMonth() + 1) + '</div>'
                          + '<div class="' + classNamePrefix + '-header-button" ><a href="javascript:' + this.name + '.jump(\'' + date.getFullYear() + '-' + (date.getMonth() + 2) + "-01" + '\')" ><span class="glyphicon glyphicon-triangle-right ' + classNamePrefix + '-dummy"><span></a>&nbsp;&nbsp;<a href="javascript:' + this.name + '.jump(\'' + (date.getFullYear() + 1) + '-' + (date.getMonth() + 1) + '-01' + '\')"><span class="glyphicon glyphicon-forward ' + classNamePrefix + '-dummy"><span></a></div>'
                          + '<div class="clearfix"></div></div>';

                outString += '<div class="' + classNamePrefix + '-days">';

                for(var i = 0;i < 7;i++)
                {
                    outString += '<div class="' + classNamePrefix + '-day-week">' + dayOfWeekName[i] + '</div>';
                }

                for(var i = 0;i < count;i++)
                {
                    outString += '<div class="' + classNamePrefix + '-day" > &nbsp; </div>';
                }

                // 查找月份最后的天数
                var day = new Date(date.getFullYear(), date.getMonth() + 1, 0);

                for(var i = 0;i < day.getDate() ;i++)
                {
                    if(date.getFullYear() === this.currentDate.getFullYear()
                        && date.getMonth() === this.currentDate.getMonth()
                        && i + 1 === this.currentDate.getDate())
                    {
                        outString += '<div class="' + classNamePrefix + '-today" >';
                    }
                    else
                    {
                        outString += '<div class="' + classNamePrefix + '-day" >';
                    }

                    outString += '<a href="javascript:' + this.name + '.getDay(\''
            + date.getFullYear() + '-'
            + (date.getMonth() + 1) + '-'
            + (i + 1) + '\');" >'
            + (i + 1) + '</a></div>';

                    count++;
                }

                outString += '<div class="clearfix"></div>';
                outString += '</div>';
                outString += '<div class="' + classNamePrefix + '-footer"><div class="' + classNamePrefix + '-button-close"><a href="javascript:' + this.name + '.close();"><span class="glyphicon glyphicon-remove"></span></a></div>';

                outString += '<strong>' + options.todayText + ': <a href="javascript:' + this.name + '.getDay(\'' + this.getToday() + '\');">' + this.getToday() + '</a></strong></div>';

                outString += '</div>';

                outString += '</div>';

                this.container[0].innerHTML = outString;

                this.innerContainer = $('#' + this.name + '-innerContainer');
            },

            toString: function()
            {
            }
        };

        if(x.isUndefined(defaultDateValue))
        {
            calendar.create(x.date.format(new Date(), 'yyyy-MM-dd'));
        }
        else
        {
            calendar.create(defaultDateValue);
        }

        return calendar;
    }
};