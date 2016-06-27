// -*- ecoding=utf-8 -*-

/**
* Combobox
*
* require	: x.js
*/
x.ui.pkg.combobox = {

  // 默认配置
  defaults: {
    /** 样式名称默认前缀 */
    classNamePrefix: 'x-ui-pkg-combobox'
  },

  // 缓存
  caches: {},

  newCombobox: function(name, containerName, targetViewName, targetValueName, options)
  {
    options = x.ext({}, x.ui.pkg.combobox.defaults, options || {});

    //
    // options : {selectedValue:'selectedValue',comboboxType:'static',list[{text:'text1',value:'value1'},{text:'text2',value:'value2'}]}
    // options : {selectedValue:'selectedValue',comboboxType:'dynamic',url:'/fff/ffe.aspx'}
    //

    var combobox = {
      // 对象名称
      name: name,
      // 选项
      options: options,
      // 容器对象
      container: x.dom.query(containerName),
      // 目标视图对象
      targetViewObject: x.dom.query(targetViewName),
      // 目标值对象
      targetValueObject: x.dom.query(targetValueName),
      // 内部容器对象
      innerContainer: null,
      // 选择的文本
      selectedText: '',
      // 选择的值
      selectedValue: '',
      // 默认空数据源显示的文本信息
      defaultEmptyText: '##请配置数据源##',

      lockObject: false,

      /*#region 函数:getValue(text, value)*/
      /*
      * 获取值
      */
      getValue: function(text, value, disableCallback)
      {
        if(text === this.defaultEmptyText)
        {
          this.targetViewObject.val('');
          this.targetValueObject.val('');
        }

        this.targetViewObject.val(text);
        this.targetValueObject.val(value);
        this.targetValueObject.attr('selectedText', text);

        this.selectedText = text;
        this.selectedValue = value;

        // 设置验证组件
        if(this.targetViewObject.hasClass('custom-forms-data-required') || this.targetViewObject.hasClass('custom-forms-data-regexp'))
        {
          // x.customForm.checkDataInput(this.targetViewObject[0], 1);
        }

        this.close();

        if(!disableCallback)
        {
          x.call(options.callback);
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      /*
      * 创建
      */
      create: function()
      {
        var width = this.targetViewObject.width();

        if(typeof (this.options.widthOffset) !== 'undefined')
        {
          width = width + Number(this.options.widthOffset);
        }

        this.container.hide();

        this.container.html('<div id="' + this.name + '-innerContainer" style="display: none;" ></div>');

        this.innerContainer = x.dom.query(this.name + '-innerContainer');

        this.innerContainer.attr('class', options.classNamePrefix);

        if(this.options.comboboxType === 'static')
        {
          var list = this.options.list;

          if(typeof (list) === 'undefined')
          {
            list = this.options.list = [{ text: this.defaultEmptyText, value: '' }];
          }

          for(var i = 0;i < list.length;i++)
          {
            if(list[i].value === this.targetValueObject.val())
            {
              // 创建时, 不运行回调函数.
              this.getValue(list[i].text, list[i].value, true);
              break;
            }
          }
        }
        else if(this.options.comboboxType === 'dynamic' && this.options.ajaxPreload > 0)
        {
          // 强制转换为数字型
          this.options.ajaxPreload = Number(this.options.ajaxPreload);

          this.preload();
        }
      },
      /*#endregion*/

      /*#region 函数:open()*/
      /*
      * 打开
      */
      open: function()
      {
        if(!this.lockObject)
        {
          this.lockObject = true;

          this.selectedValue = this.targetValueObject.val();

          // 当输入框在隐藏模式下初始化是, 代码获取不到元素宽度, 因此在此处重新设置宽度
          var width = this.targetViewObject.width();
          /*
          if(x.browser.ie)
          {
              // IE
              this.container.css({
                  'vertical-align': 'baseline',
                  'width': width + 'px',
                  'margin-top': (this.options.topOffset - 1) + 'px'
              });
          }
          else if(x.browser.webkit)
          {
              // Chrome
              this.container.css({
                  'position': 'absolute',
                  'width': width + 'px',
                  'top': ((x.page.getElementAbsoluteTop(this.targetViewObject[0]) + x.page.getElementRange(this.targetViewObject[0]).height) - 1) + 'px' 
              });
          }
          else
          {
              // Firefox
              this.container.css({
                  'vertical-align': 'baseline',
                  'width': width + 'px',
                  'height': (Number(this.options.topOffset) < 0 ? -Number(this.options.topOffset) : 0) + 'px',
                  'margin-top': this.options.topOffset + 'px'
              });
          }
          */
          // combobox.container.show();
          this.container.css({ 'display': '' });

          // this.innerContainer.css({ 'width': width + 'px' });

          this.bind();

          // 取消验证组件
          this.targetValueObject.unbind('blur');
          this.targetViewObject.unbind('blur');
        }
      },
      /*#endregion*/

      /*#region 函数:close()*/
      /*
      * 关闭
      */
      close: function()
      {
        // 设置验证组件
        if(this.lockObject && (this.targetViewObject.hasClass('custom-forms-data-required') || this.targetViewObject.hasClass('custom-forms-data-regexp')))
        {
          x.customForm.checkDataInput(this.targetViewObject[0], 1);
        }

        // this.container.slideToggle("slow"); 
        this.lockObject = false;

        // var that = this;

        // this.innerContainer.slideUp("slow", function()
        // {
        //    that.innerContainer.hide();
        // });

        this.innerContainer.css({ 'display': 'none' });
      },
      /*#endregion*/

      /*#region 函数:preload()*/
      /*
      * Ajax方式预加载，可以提高显示速度。
      */
      preload: function()
      {
        // 预加载人员选择列表，加快显示速度。
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<action><![CDATA[' + ((typeof (this.options.ajaxMethod) === 'undefined') ? 'getCombobox' : this.options.ajaxMethod) + ']]></action>';
        outString += '<combobox><![CDATA[' + this.name + ']]></combobox>';
        outString += '<selectedValue><![CDATA[' + x.util.htmlEncode(this.targetValueObject.val()) + ']]></selectedValue>';
        if(typeof (this.options.comboboxEmptyItemText) !== 'undefined' && this.options.comboboxEmptyItemText.trim() !== '')
        {
          // 空白选项 全部 
          outString += '<emptyItemText><![CDATA[' + this.options.comboboxEmptyItemText.trim() + ']]></emptyItemText>';
        }
        if(!x.isUndefined(this.options.xhrParams))
        {
          x.each(this.options.xhrParams, function(node)
          {
            outString += '<' + node + '><![CDATA[' + node + ']]></' + node + '>';
          });
        }
        if(x.isUndefined(this.options.xhrWhere))
        {
          outString += '<whereClause><![CDATA[' + this.options.xhrWhere.replace('{0}', this.targetViewObject.val()).replace(/\\/g, '') + ']]></whereClause>';
        }
        outString += '<length>0</length>';
        outString += '</request>';

        x.net.xhr(combobox.options.url, outString, {
          callback: function(response)
          {
            var result = x.toJSON(response);

            var list = result.ajaxStorage;

            var combobox = window[result.combobox];

            if(list.length >= combobox.options.ajaxPreload)
            {
              combobox.targetViewObject.val(list[combobox.options.ajaxPreload - 1].text);
              combobox.targetValueObject.val(list[combobox.options.ajaxPreload - 1].value);

              combobox.selectedText = list[combobox.options.ajaxPreload - 1].text;
              combobox.selectedValue = list[combobox.options.ajaxPreload - 1].value;
            }
          }
        });
      },
      /*#endregion*/

      /*#region 函数:bind()*/
      /*
      * 绑定数据
      */
      bind: function()
      {
        if(this.options.comboboxType === 'static')
        {
          this.show(this.options.show, this.options.list);

          var html = this.innerContainer.html();

          if(html !== '' && html !== '<ul class="' + options.classNamePrefix + '-ul" ></ul>')
          {
            // this.innerContainer.slideDown('slow');
            this.innerContainer.css({ 'display': '' });
          }
        }
        else if(this.options.comboboxType === 'dynamic')
        {
          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[' + ((typeof (this.options.ajaxMethod) === 'undefined') ? 'getCombobox' : this.options.ajaxMethod) + ']]></action>';
          outString += '<combobox><![CDATA[' + this.name + ']]></combobox>';
          outString += '<selectedValue><![CDATA[' + this.targetValueObject.val() + ']]></selectedValue>';
          if(typeof (this.options.comboboxEmptyItemText) !== 'undefined' && this.options.comboboxEmptyItemText.trim() !== '')
          {
            // 空白选项 全部 
            outString += '<emptyItemText><![CDATA[' + this.options.comboboxEmptyItemText.trim() + ']]></emptyItemText>';
          }
          if(!x.isUndefined(this.options.xhrParams))
          {
            x.each(this.options.xhrParams, function(key, value)
            {
              x.debug.log(key);
              outString += '<' + key + '><![CDATA[' + value + ']]></' + key + '>';
            });
          }
          if(!x.isUndefined(this.options.xhrWhere))
          {
            outString += '<whereClause><![CDATA[' + this.options.xhrWhere.replace('{0}', this.targetViewObject.val()).replace(/\\/g, '') + ']]></whereClause>';
          }
          outString += '<length>0</length>';
          outString += '</request>';

          x.net.xhr(this.options.url, outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var list = result.data;

              if(list.length > 0)
              {
                var combobox = window[result.combobox];

                combobox.show(combobox.options.show, list);

                var html = combobox.innerContainer.html();

                if(html !== '' && html !== '<ul></ul>')
                {
                  // combobox.innerContainer.slideDown('slow');
                  combobox.innerContainer.css({ 'display': '' });
                }
              }
            }
          });
        }
      },
      /*#endregion*/

      /*#region 函数:show(type, list)*/
      show: function(type, list)
      {
        switch(type)
        {
          case 'textAndDescription':
            this.showText(list);
            break;

          case 'text':
            this.showText(list);
            break;

          default:
            this.showText(list);
            break;
        }
      },
      /*#endregion*/

      /*#region 函数:showText(list)*/
      /*
      * 显示
      */
      showText: function(list)
      {
        var outString = '';

        var classNamePrefix = options.classNamePrefix;

        outString += '<ul>';

        for(var i = 0;i < list.length;i++)
        {
          if(list[i] != undefined)
          {
            outString += '<li>';
            // outString += '<div class="' + classNamePrefix + '-list-text"><a href="javascript:window[\'' + this.name + '\'].getValue(\'' + list[i].text + '\',\'' + list[i].value + '\');" >' + this.setTextBlod(list[i].text, this.selectedText) + '</a></div>';
            outString += '<a href="javascript:window[\'' + this.name + '\'].getValue(\'' + list[i].text + '\',\'' + list[i].value + '\');" >' + this.setTextBlod(list[i].text, this.selectedText) + '</a>';
            outString += '</li>';
          }
        }

        outString += '</ul>';

        this.innerContainer.html(outString);

        if(list.length > 6)
        {
          $('.' + classNamePrefix + '-ul').css({
            'height': '132px',
            'overflow': 'auto'
          });
        }
      },
      /*#endregion*/

      /*#region 函数:showTextAndDescription(list)*/
      showTextAndDescription: function(list)
      {
        var outString = '';

        var classNamePrefix = options.classNamePrefix;

        outString += '<ul class="' + classNamePrefix + '-ul" >';

        for(var i = 0;i < list.length;i++)
        {
          if(list[i] != undefined)
          {
            outString += '<li><a href="javascript:window[\'' + this.name + '\'].getValue(\'' + list[i].text + '\',\'' + list[i].value + '\');" >';
            outString += '<div class="' + classNamePrefix + '-description">' + list[i].description + '</div>';
            outString += '<div class="' + classNamePrefix + '-text">' + this.setTextBlod(list[i].text, this.selectedText) + '</div>';
            //outString += '<div class="clear"></div>';
            outString += '</a></li>';
          }
        }

        outString += '</ul>';

        this.innerContainer.html(outString);

        if(list.length > 6)
        {
          $('.' + classNamePrefix + '-ul').css({ 'height': '132px', 'overflow': 'auto' });
        }
      },
      /*#endregion*/

      /*#region 函数:setTextBlod(text, selectedText)*/
      setTextBlod: function(text, selectedText)
      {
        return text.replace(selectedText, '<strong>' + selectedText + '</strong>');
      }
      /*#endregion*/
    };

    combobox.create();

    return combobox;
  }
};