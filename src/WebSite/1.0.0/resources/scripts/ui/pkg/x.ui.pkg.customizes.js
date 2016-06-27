/**
* 页面自定义拖拉拽
*
* require	: jQuery($), jQuery UI & sortable/draggable UI modules
* require	: x.js
*/
x.ui.pkg.customizes = {

  // 页面实例
  pageInstance: {},
  // 部件实例
  widgetInstance: {},

  settings: {
    wrapperSelector: '.x-ui-pkg-customize-wrapper',
    menuLayoutSelector: '.x-ui-pkg-customize-menu-layout-wrapper',
    menuWidgetSelector: '.x-ui-pkg-customize-menu-widget-wrapper',
    columnSelector: '.x-ui-pkg-customize-column',
    dialogSelector: '.x-ui-pkg-customize-dialog',
    widgetSelector: '.x-ui-pkg-customize-widget',
    handleSelector: '.x-ui-pkg-customize-widget-head',
    settingSelector: '.x-ui-pkg-customize-widget-setting',
    contentSelector: '.x-ui-pkg-customize-widget-content',

    // 默认框架菜单
    layoutMenu: '<a href="this.layout([{width:50%},{width:50%,}]);" title="第一列宽:33.3%,第二列宽:33.3%,第三列宽:33.3%" >三列</a> | '
            + '<a href="this.layout([{width:50%},{width:50%,}]);" title="第一列宽:50%,第二列宽:50%" >两列</a>',

    // 默认部件菜单
    widgetMenu: '',

    // 默认设置
    widgetDefault: {
      movable: true,
      removable: true,
      collapsible: true,
      editable: true,
      colorClasses: [
          'color-yellow',
          'color-red',
          'color-blue',
          'color-white',
          'color-orange',
          'color-green']
    },

    // 实例设置
    widgetInstance: {
      intro: {
        movable: false,
        removable: false,
        collapsible: false,
        editable: false
      }
    }
  },

  /*#region 函数:getSettings(id)*/
  /**
  * 获取配置信息
  */
  getSettings: function(id)
  {
    var settings = x.ui.pkg.customizes.settings;

    if(id && settings.widgetInstance[id])
    {
      return $.extend({}, settings.widgetDefault, settings.widgetInstance[id])
    }
    else
    {
      return settings.widgetDefault;
    }
  },
  /*#endregion*/

  /**
  * 页面部件
  */
  widget: {

    /**
    * 创建菜单
    */
    createMenu: function(options)
    {
      // 保存数据到服务器端
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<whereClause><![CDATA[ Status = 1 ORDER BY OrderId ]]></whereClause>';
      outString += '<length><![CDATA[0]]></length>';
      outString += '</request>';

      x.net.xhr('/api/web.customizes.customizeWidget.findAll.aspx', outString, {
        callback: function(response)
        {
          var list = x.toJSON(response).data;

          var outString = '<div class="btn-group" role="group" >';

          x.each(list, function(index, node)
          {
            outString += '<button onclick="x.ui.pkg.customizes.widget.create(\'' + node.name + '\',{';
            outString += 'pageId:\'' + options.id + '\',';
            outString += 'url:\'' + node.url + '\',';
            outString += 'name:\'' + node.name + '\',';
            outString += 'title:\'' + node.title + '\',';
            outString += 'options:\'' + x.toSafeJSON(node.options).replace(/'/g, '\'') + '\',';
            outString += 'tags:\'' + node.tags + '\',';
            outString += 'className:\'' + node.className + '\'';
            outString += '});" class="btn btn-default" >' + node.description + '</button>';
          });

          outString += '</div>';

          x.ui.pkg.customizes.settings.widgetMenu = outString;

          var settings = x.ui.pkg.customizes.settings;

          var selector = x.ui.pkg.customizes.settings.menuWidgetSelector;

          $(selector).html(settings.widgetMenu);
        }
      });
    },

    /**
    * 打开对话框
    */
    openDialog: function()
    {
      var settings = x.ui.pkg.customizes.settings;

      var dialogSelector = x.ui.pkg.customizes.settings.menuWidgetSelector;

      $(dialogSelector).html(settings.widgetMenu);
      $(dialogSelector).parent().show();
    },

    /**
    * 关闭对话框
    */
    closeDialog: function()
    {
      var dialogSelector = x.ui.pkg.customizes.settings.dialogSelector;

      $(dialogSelector).html('');
      $(dialogSelector).parent().hide();
    },

    /**
    * 创建
    */
    create: function(name, options)
    {
      x.ui.pkg.customizes.widget.newWidget(name, options);
    },

    newWidget: function(name, options)
    {
      var widget = {

        name: name,

        options: options,

        load: function(options)
        {
          // 保存数据到服务器端
          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<id><![CDATA[' + x.guid.create() + ']]></id>';
          outString += '<pageId><![CDATA[' + options.pageId + ']]></pageId>';
          outString += '<pageName><![CDATA[' + options.pageName + ']]></pageName>';
          outString += '<widgetName><![CDATA[' + name + ']]></widgetName>';
          outString += '<title><![CDATA[' + options.title + ']]></title>';
          outString += '</request>';

          if(typeof (options.callback) === 'undefined')
          {
            options.callback = function(response)
            {
              var param = x.toJSON(response).data;

              var outString = '<li id="' + param.id + '" class="x-ui-pkg-customize-widget panel panel-default" widget="' + param.widgetName + '" >'
                    + '<div class="x-ui-pkg-customize-widget-head panel-heading">'
                    + '<h3>' + param.title + '</h3>'
                    + '</div>'
                    // + '<div class="x-ui-pkg-customize-widget-content panel-body" >' + param.html + '</div>'
                    + '<div class="x-ui-pkg-customize-widget-content panel-body" ></div>'
                    + '</li>';

              $(outString).prependTo($(x.ui.pkg.customizes.settings.columnSelector)[0]);

              x.ui.pkg.customizes.pageInstance[param.pageId].reload();
            }
          }

          x.net.xhr('/api/web.customizes.customizeWidgetInstance.create.aspx', outString, { callback: options.callback });
        }
      }

      widget.load(options);

      return widget;
    },

    /*
    * 重定向地址
    */
    redirct: function(url)
    {
      if(!this.designtime)
      {
        location.href = url;
      }
    }
  },

  /*
  * 页面框架
  */
  layout: {

    /**
    * 创建菜单
    */
    createMenu: function(options)
    {
      // 保存数据到服务器端
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<whereClause><![CDATA[ Status = 1 ORDER BY OrderId ]]></whereClause>';
      outString += '<length><![CDATA[0]]></length>';
      outString += '</request>';

      x.net.xhr('/api/web.customizes.customizeLayout.findAll.aspx', outString, {
        callback: function(response)
        {
          var list = x.toJSON(response).data;

          var outString = '<div class="btn-group" role="group" >';

          x.each(list, function(index, node)
          {
            outString += '<button onclick="x.ui.pkg.customizes.layout.create(\'' + node.name + '\',{';
            outString += 'id:\'' + node.id + '\',';
            outString += 'name:\'' + node.name + '\',';
            outString += 'html:\'' + x.encoding.html.encode(node.html) + '\',';
            outString += '});" class="btn btn-default" >' + node.description + '</button>';
          });

          outString += '</div>';

          x.ui.pkg.customizes.settings.layoutMenu = outString;

          var settings = x.ui.pkg.customizes.settings;

          var selector = x.ui.pkg.customizes.settings.menuLayoutSelector;

          $(selector).html(settings.layoutMenu);
        }
      });
    },

    openDialog: function()
    {
      var settings = x.ui.pkg.customizes.settings;

      var dialogSelector = x.ui.pkg.customizes.settings.dialogSelector;

      $(dialogSelector).html(settings.layoutMenu);
      $(dialogSelector).parent().show();
    },

    closeDialog: function()
    {
      var dialogSelector = x.ui.pkg.customizes.settings.dialogSelector;

      $(dialogSelector).html('');
      $(dialogSelector).parent().hide();
    },

    create: function(name, options)
    {
      if(confirm('重新设置页面布局将删除当前页面的所有部件数据, 是否需要重新设置页面布局?'))
      {
        var settings = x.ui.pkg.customizes.settings;

        $(settings.wrapperSelector).html(options.html);
      }
    },

    load: function(html)
    {
      $(elementName).hide();
    },

    /*
    * 保存
    */
    html: function()
    {
      var outString = '';

      var settings = x.ui.pkg.customizes.settings;

      $(settings.columnSelector).each(function(index, node)
      {
        $(settings.handleSelector, $(node)).removeAttr('style');

        $('.collapse', $(node)).remove();
        $('.edit', $(node)).remove();
        $('.remove', $(node)).remove();
        $('.widget-option-html', $(node)).remove();
      });

      return $(settings.wrapperSelector).html();
    },

    /**
    * 布局
    */
    layout: function()
    {
      var outString = '';

      var settings = x.ui.pkg.customizes.settings;

      //            $(settings.columnSelector).each(function(index, node)
      //            {
      //                outString += '<ul id="column' + (index + 1) + '" class="x-ui-pkg-customize-column" >';

      //                $(settings.widgetSelector, $(node)).each(function(index, node)
      //                {
      //                    outString += '<li id="' + $(node).attr('id') + '" class="x-ui-pkg-customize-widget" widget="' + $(node).attr('widget') + '" >';
      //                    outString += '<div class="x-ui-pkg-customize-widget-head"><h3>' + $('h3', $(node)).html() + '</h3></div>';
      //                    outString += '<div class="x-ui-pkg-customize-widget-content" ></div>';
      //                    outString += '</li>';

      //                });

      //                outString += '</ul>';
      //            });

      var wrapper = $(settings.wrapperSelector).clone();

      $(wrapper).find(settings.columnSelector).each(function(index, node)
      {
        $(node).find(settings.widgetSelector).each(function(index, node)
        {
          $(node).find(settings.handleSelector).html('<h3>' + $(node).find('h3').html() + '</h3>');
          $(node).find(settings.contentSelector).html('');
        });
      });

      return x.ui.pkg.customizes.layout._fixIEHtml(wrapper.html());
    },

    // 此方法 格式化IE的 innerHTML 元素大写 和 双引号取消
    _fixIEHtml: function(html)
    {
      if(x.browser.ie)
      {
        var fixedHtml = html;

        // remove jQuery flag
        fixedHtml = fixedHtml.replace(/aria-disabled=false/g, '').replace(/unselectable="on"/g, '');

        var elements = fixedHtml.match(/<\/?\w+((\s+\w+(\s*=\s*(?:".*?"|'.*?'|[^'">\s]+))?)+\s*|\s*)\/?>/g);

        if(elements)
        {
          for(var i = 0;i < elements.length;i++)
          {
            var originalElement = elements[i];

            // 将元素字符名称小写
            elements[i] = elements[i].replace(/(<?\w+)|(<\/?\w+)\s/, function(a) { return a.toLowerCase(); });

            var value = elements[i].match(/\=[a-zA-Z0-9-_\/]+[?\s+|?>]/g);

            if(value)
            {
              for(var j = 0;j < value.length;j++)
              {
                // 将元素的属性加上双引号
                elements[i] = elements[i].replace(value[j], value[j].replace(/\=([a-zA-Z0-9-_\/]+)([?\s+|?>])/g, '="$1"$2'));
              }
            }

            fixedHtml = fixedHtml.replace(originalElement, elements[i]);
          }
        }

        return fixedHtml;
      }
      else
      {
        return html;
      }
    }
  },

  /**
  * 页面
  */
  newPage: function(name, options)
  {
    var page = {
      // 对象名称
      name: name,
      // 选项
      options: options,
      // 设计时模式
      designtime: false,

      /*
      * 页面部件
      */
      widget: {

        list: '',

        newWidgetInstance: function(id, name, title, href, callback)
        {
          // 保存数据到服务器端
          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[create]]></action>';
          outString += '<id><![CDATA[' + id + ']]></id>';
          outString += '<pageName><![CDATA[' + this.pageName + ']]></pageName>';
          outString += '<widgetName><![CDATA[' + name + ']]></widgetName>';
          outString += '<title><![CDATA[' + title + ']]></title>';
          outString += '</request>';

          x.net.xhr((typeof (href) == 'undefined') ? '/api/web.customize.widget.instance.aspx' : href, outString, {
            callback: (typeof (callback) == 'undefined') ? this.widget.newWidget_callback : callback
          });
        },

        bindControls: function(parent)
        {
          var settings = x.ui.pkg.customizes.settings;

          var that = parent;

          $(settings.widgetSelector, $(settings.columnSelector)).each(function()
          {
            var thisWidgetSettings = x.ui.pkg.customizes.getSettings(this.id);

            // 取消点击事件，防止拖拽后跳转到别的页面
            $(this).unbind('click');

            if(thisWidgetSettings.removable)
            {
              $('<a href="#" class="remove" title="删除" ><i class="fa fa-remove" ></i></a>')
                    .mousedown(function(event) { event.stopPropagation(); })
                    .click(function()
                    {
                      if(confirm('确定删除此部件?'))
                      {
                        // 保存数据到服务器端
                        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

                        outString += '<request>';
                        outString += '<ids><![CDATA[' + $(this).parent().parent()[0].id + ']]></ids>';
                        outString += '</request>';

                        x.net.xhr('/api/web.customizes.customizeWidgetInstance.delete.aspx', outString, {
                          callback: function(response)
                          {
                            var result = x.toJSON(response).message;

                            switch(Number(result.returnCode))
                            {
                              case 0:
                                break;

                              case -1:
                              case 1:
                                alert(result.value);
                                break;

                              default:
                                break;
                            }
                          }
                        });

                        $(this).parents(settings.widgetSelector).animate({ opacity: 0 }, function()
                        {
                          $(this).wrap('<div/>').parent().slideUp(function()
                          {
                            $(this).remove();
                          });
                        });
                      }
                      return false;
                    })
                    .appendTo($(settings.handleSelector, this));
            }

            if(thisWidgetSettings.editable)
            {
              $('<a href="#" class="edit" title="编辑"><i class="fa fa-edit" ></i></a>')
                  //.mousedown(function(event) { event.stopPropagation(); })
                  .on('click', function()
                  {
                    var widgetOptionHtml = $(this).parents(settings.widgetSelector).find('.widget-option-html').html();

                    if(widgetOptionHtml == '')
                    {
                      // 加载编辑框内容
                      var url = that.options.widgetInstanceUrl;

                      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

                      outString += '<request>';
                      outString += '<id><![CDATA[' + $(this).parent().parent()[0].id + ']]></id>';
                      outString += '</request>';

                      var parent = $(this).parent().parent();

                      x.net.xhr('/api/web.customizes.customizeWidgetInstance.getOptionHtml.aspx', outString, {
                        callback: function(response)
                        {
                          var html = x.toJSON(response).data;

                          $('.widget-option-html', parent).html(html).show();

                          x.dom.features.bind();
                        }
                      });
                    }
                    else
                    {
                      if($('.widget-option-html', parent).css('display') == 'none')
                      {
                        $('.widget-option-html', parent).css('display', '');
                      }
                      else
                      {
                        // 保存编辑框内容
                        var url = that.options.widgetInstanceUrl;

                        var parent = $(this).parent().parent();

                        var widgetOptions = '{';

                        $('.widget-option-item', parent).each(function(index, node)
                        {
                          widgetOptions += node.id.replace(parent[0].id + '$', '') + ':\'' + node.value.replace('\\','\\\\') + '\',';
                        });

                        if(widgetOptions.substr(widgetOptions.length - 1, 1) == ',')
                        {
                          widgetOptions = widgetOptions.substr(0, widgetOptions.length - 1);
                        }

                        widgetOptions += '}';

                        x.debug.log(widgetOptions);

                        // 设置标题
                        var widgetTitileInput = $(document.getElementById(parent[0].id + '$title'));

                        $(widgetTitileInput).parents(settings.widgetSelector).find('h3').text($(widgetTitileInput).val().length > 20 ? $(widgetTitileInput).val().substr(0, 20) + '...' : $(widgetTitileInput).val());

                        // 设置高度和宽度
                        var widgetHeightInput = $(document.getElementById(parent[0].id + '$height'));
                        var widgetWidthInput = $(document.getElementById(parent[0].id + '$width'));

                        $(settings.contentSelector, parent).children().css({
                          'height': (Number(widgetHeightInput.val()) > 0 ? (widgetHeightInput.val() + 'px') : 'auto'),
                          'width': (Number(widgetWidthInput.val()) > 0 ? (widgetWidthInput.val() + 'px') : 'auto')
                        });

                        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

                        outString += '<request>';
                        outString += '<id><![CDATA[' + parent[0].id + ']]></id>';
                        outString += '<options><![CDATA[' + widgetOptions + ']]></options>';
                        outString += '</request>';

                        x.net.xhr('/api/web.customizes.customizeWidgetInstance.setOptions.aspx', outString, {
                          callback: function(response)
                          {
                            $('.widget-option-html', parent).hide();
                          }
                        });
                      }
                    }

                    return false;
                  }
                  //,

                  //function()
                  //{
                  //  var widgetOptionHtml = $(this).parents(settings.widgetSelector).find('.widget-option-html').html();

                  //  if(widgetOptionHtml !== '')
                  //  {
                  //    // 保存编辑框内容
                  //    var url = that.options.widgetInstanceUrl;

                  //    var parent = $(this).parent().parent();

                  //    var widgetOptions = '{';

                  //    $('.widget-option-item', parent).each(function(index, node)
                  //    {
                  //      widgetOptions += node.id.replace(parent[0].id + '$', '') + ':\'' + node.value + '\',';
                  //    });

                  //    if(widgetOptions.substr(widgetOptions.length - 1, 1) == ',')
                  //    {
                  //      widgetOptions = widgetOptions.substr(0, widgetOptions.length - 1);
                  //    }

                  //    widgetOptions += '}';

                  //    x.debug.log(widgetOptions);

                  //    // 设置标题
                  //    var widgetTitileInput = $(document.getElementById(parent[0].id + '$title'));

                  //    $(widgetTitileInput).parents(settings.widgetSelector).find('h3').text($(widgetTitileInput).val().length > 20 ? $(widgetTitileInput).val().substr(0, 20) + '...' : $(widgetTitileInput).val());

                  //    // 设置高度和宽度
                  //    var widgetHeightInput = $(document.getElementById(parent[0].id + '$height'));
                  //    var widgetWidthInput = $(document.getElementById(parent[0].id + '$width'));

                  //    $(settings.contentSelector, parent).children().css({
                  //      'height': (Number(widgetHeightInput.val()) > 0 ? (widgetHeightInput.val() + 'px') : 'auto'),
                  //      'width': (Number(widgetWidthInput.val()) > 0 ? (widgetWidthInput.val() + 'px') : 'auto')
                  //    });

                  //    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

                  //    outString += '<request>';
                  //    outString += '<id><![CDATA[' + parent[0].id + ']]></id>';
                  //    outString += '<options><![CDATA[' + widgetOptions + ']]></options>';
                  //    outString += '</request>';

                  //    x.net.xhr('/api/web.customizes.customizeWidgetInstance.setOptions.aspx', outString, {
                  //      callback: function(response)
                  //      {
                  //        $('.widget-option-html', parent).hide();
                  //      }
                  //    });
          //          }
          //          else
          //          {
          //            $(this).parents(settings.widgetSelector).find('.widget-option-html').hide();
          //          }
          //          return false;
          //}
                  )
                  .appendTo($(settings.handleSelector, this));

              $('<div class="x-ui-pkg-customize-widget-setting-container widget-option-html" style="display:none;" />')
                  .insertAfter($(settings.handleSelector, this));

              /*
              // 设置部件配置参数
              var widgetSettings = $(settings.widgetSelector + ' ' + settings.settingSelector);

              widgetSetings.each(function (node, index)
              {
              x.debug.log('node:' + node.id);
              });
              */
            }

            if(thisWidgetSettings.collapsible)
            {
              $('<a href="#" class="collapse">+</a>')
                  .mousedown(function(e) { e.stopPropagation(); })
                  .toggle(function()
                  {
                    $(this).parents(settings.widgetSelector)
                        .find(settings.contentSelector).hide();
                    //                    $(this).css({backgroundPosition: '-38px 0'})
                    //                        .parents(settings.widgetSelector)
                    //                            .find(settings.contentSelector).hide();
                    //
                    return false;
                  },
                  function()
                  {
                    $(this).css({ backgroundPosition: '' })
                        .parents(settings.widgetSelector)
                        .find(settings.contentSelector).show();
                    return false;
                  }).prependTo($(settings.handleSelector, this));
            }
          });

          $('.widget-option-html').each(function()
          {
            $('input', this).keyup(function()
            {
              $(this).parents(settings.widgetSelector).find('h3').text($(this).val().length > 20 ? $(this).val().substr(0, 20) + '...' : $(this).val());
            });

            $('ul.colors li', this).click(function()
            {
              var colorStylePattern = /\bcolor-[\w]{1,}\b/,
              thisWidgetColorClass = $(this).parents(settings.widgetSelector).attr('class').match(colorStylePattern)
              if(thisWidgetColorClass)
              {
                $(this).parents(settings.widgetSelector)
            .removeClass(thisWidgetColorClass[0])
            .addClass($(this).attr('class').match(colorStylePattern)[0]);
              }

              return false;
            });
          });
        }
      },

      /*#region 函数:save()*/
      /**
      * 保存页面信息
      */
      save: function(data)
      {
        var settings = x.ui.pkg.customizes.settings;

        $(settings.wrapperSelector).html(x.ui.pkg.customizes.layout.html());

        $(settings.handleSelector).css({ cursor: 'default' });

        this.designtime = false;

        this.options = x.ext(this.options, data);

        x.ui.pkg.customizes.settings.widgetDefault.movable = false;

        // 保存数据到服务器端
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<id><![CDATA[' + this.options.id + ']]></id>';
        outString += '<authorizationObjectType><![CDATA[' + this.options.authorizationObjectType + ']]></authorizationObjectType>';
        outString += '<authorizationObjectId><![CDATA[' + this.options.authorizationObjectId + ']]></authorizationObjectId>';
        outString += '<name><![CDATA[' + this.options.name + ']]></name>';
        outString += '<title><![CDATA[' + this.options.title + ']]></title>';
        outString += '<html><![CDATA[' + x.ui.pkg.customizes.layout.layout() + ']]></html>';
        outString += '</request>';

        x.net.xhr('/api/web.customizes.customizePage.save.aspx', outString);
      },
      /*#endregion*/

      /*#region 函数:makeSortable()*/
      /**
      * 允许移动和排序
      */
      makeSortable: function()
      {
        settings = x.ui.pkg.customizes.settings,

        $sortableItems = (function()
        {
          var notSortable = '';
          $(settings.widgetSelector, $(settings.columnSelector)).each(function(i)
          {
            if(!x.ui.pkg.customizes.getSettings(this.id).movable)
            {
              if(!this.id)
              {
                this.id = 'widget-no-id-' + i;
              }
              notSortable += '#' + this.id + ',';
            }
          });

          if(notSortable != '')
            return $('> li:not(' + notSortable + ')', settings.columnSelector);
          else
            return $('li', settings.columnSelector);
        })();

        $sortableItems.find(settings.handleSelector).css({ cursor: 'move' })
            .mousedown(function(e)
            {
              $sortableItems.css({ width: '' });
              $(this).parent().css({
                width: ($(this).parent().width() + 2) + 'px'
              });
            })
            .mouseup(function()
            {
              if(!$(this).parent().hasClass('dragging'))
              {
                $(this).parent().css({ width: '' });
              }
              else
              {
                $(settings.columnSelector).sortable('disable');
              }

            })
            .mousemove(function()
            {
              $('.x-ui-pkg-customize-widget-placeholder')
                  .addClass('x-ui-pkg-customize-widget')
                  .html($(this).parent().html());
            });

        $(settings.columnSelector).sortable({
          items: $sortableItems,
          connectWith: $(settings.columnSelector),
          handle: settings.handleSelector,
          placeholder: 'x-ui-pkg-customize-widget-placeholder',
          forcePlaceholderSize: true,
          revert: 300,
          delay: 100,
          opacity: 0.6,
          containment: 'document',
          start: function(e, ui)
          {
            $(ui.helper).addClass('dragging');
          },
          stop: function(e, ui)
          {
            $(ui.item).css({ width: '' }).removeClass('dragging');
            $(settings.columnSelector).sortable('enable');
          }
        });
      },
      /*#endregion*/

      /*#region 函数:refresh()*/
      /**
      * 刷新
      */
      refresh: function()
      {
        $('widgetSelector')
        {
          // 保存数据到服务器端
          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<id><![CDATA[' + id + ']]></id>';
          outString += '<pageName><![CDATA[' + this.name + ']]></pageName>';
          outString += '<widgetName><![CDATA[' + name + ']]></widgetName>';
          outString += '<title><![CDATA[' + title + ']]></title>';
          outString += '</request>';

          $.post('/api/web.customize.widget.create.aspx', outString, {
            callback: (typeof (callback) == 'undefined') ? this.widget.newWidget_callback : callback
          });
        }
      },
      /*#endregion*/

      /*#region 函数:reload()*/
      /**
      * 重新加载
      */
      reload: function()
      {
        $(x.ui.pkg.customizes.settings.wrapperSelector).html(x.ui.pkg.customizes.layout.html());

        this.load(this.options);
      },
      /*#endregion*/

      /*#region 函数:load()*/
      /**
      * 加载
      */
      load: function(options)
      {
        // 设置页面名称
        if(typeof (options.name) !== 'undefined' && options.name !== '')
        {
          this.name = options.name;
        }

        this.options = x.ext({}, options);

        x.ui.pkg.customizes.layout.createMenu(options);

        x.ui.pkg.customizes.widget.createMenu(options);

        // 设置页面是否可以移动
        x.ui.pkg.customizes.settings.widgetDefault.movable = true;
        // 设置页面是设计时
        this.designtime = true;
        // 绑定控件
        this.widget.bindControls(this);
        // 添加排序特性
        this.makeSortable();
      }
      /*#endregion*/
    }

    page.load(options);

    x.ui.pkg.customizes.pageInstance[options.id] = page;

    return page;
  }
};
