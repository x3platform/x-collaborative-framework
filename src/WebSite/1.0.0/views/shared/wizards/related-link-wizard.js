x.register('x.ui.wizards');

/*
* 相关链接设置向导
*/
x.ui.wizards.newRelatedLinkWizard = function(name, targetViewName, targetValueName, options)
{
  var wizard = {

    // 实例名称
    name: name,

    // 配置信息
    options: options,

    // 遮罩
    maskWrapper: null,

    // 本地数据源(JSON格式)
    localStorage: null,

    // 返回值
    returnValue: null,

    // 目标视图对象名称
    targetViewName: targetViewName,

    // 目标值对象名称
    targetValueName: targetValueName,

    // 相关链接地址
    linkValue: null,

    // 相关链接地址
    linkText: null,

    // 相关链接表格
    linkTable: null,

    /*#region 函数:open()*/
    open: function()
    {
      this.maskWrapper.open();
    },
    /*#endregion*/

    /*#region 函数:add()*/
    /*
    * 添加相关链接
    */
    add: function()
    {
      if(this.linkValue.value === '' || this.linkText.value === '')
      {
        return;
      }

      var row = this.linkTable.insertRow(this.linkTable.rows.length);

      row.id = x.guid.create();
      row.className = 'table-row-normal';

      var column1 = row.insertCell(0);
      var column2 = row.insertCell(1);

      column1.innerHTML = "<a href='" + this.linkValue.value + "' target='_blank'>" + this.linkText.value + "</a>";
      column2.innerHTML = '<a href="javascript:' + this.name + '.remove(\'' + row.id + '\');" >删除</a>';

      this.linkText.value = '';
      this.linkValue.value = 'http://';

      this.linkText.focus();

      if(this.linkTable.rows.length > 1)
      {
        $(this.linkTable).show();
      }
    },
    /*#endregion*/

    /*#region 函数:remove(rowId)*/
    /*
    * 移除相关链接
    */
    remove: function(rowId)
    {
      $(document.getElementById(rowId)).remove();

      if(this.linkTable.rows.length === 1)
      {
        $(this.linkTable).hide();
      }
    },
    /*#endregion*/

    /*#region 函数:getResult()*/
    /*
    * 获取结果
    */
    calculateResult: function()
    {
      var returnValue = '{"list":[';

      for(var i = 1;i < this.linkTable.rows.length;i++)
      {
        returnValue += '{"text":"' + x.toSafeJSON(this.linkTable.rows[i].cells[0].childNodes[0].innerHTML) + '","value":"' + this.linkTable.rows[i].cells[0].childNodes[0].href + '"},';
      }

      if(returnValue.substr(returnValue.length - 1, 1) === ',')
      {
        returnValue = returnValue.substr(0, returnValue.length - 1);
      }

      returnValue += ']}';

      return returnValue;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
      if(this.linkText.value !== '' && this.linkValue.value !== 'http://')
      {
        if(window.confirm('是否需增加相关链接【' + this.linkText.value + '】'))
        {
          this.add();
        }
      }

      // 计算结果
      this.returnValue = this.calculateResult();

      this.save_callback(this.returnValue);

      this.cancel();
    },
    /*#endregion*/

    /*#region 函数:save_callback(response)*/
    /*
    * 默认回调函数，可根据需要自行修改此函数。
    */
    save_callback: function(response)
    {
      x.ui.wizards.setRelatedLinkView(this.targetViewName, response);

      var targetValueObject = document.getElementById(this.targetValueName);
      var targetViewObject = document.getElementById(this.targetViewName);

      if(targetValueObject.tagName.toUpperCase() == 'INPUT' || targetValueObject.tagName.toUpperCase() == 'TEXTAREA')
      {
        $(targetValueObject).val(response);
      }
      else
      {
        $(targetValueObject).html(response);
      }

      $(targetViewObject).height((x.toJSON(response).list.length * 18));

      if($(targetViewObject).height() < 40)
      {
        $(targetViewObject).height(40);
      }
    },
    /*#endregion*/

    /*#region 函数:cancel()*/
    cancel: function()
    {
      this.maskWrapper.close();

      if(typeof (this.cancel_callback) !== 'undefined')
      {
        this.cancel_callback(this.returnValue);
      }
    },
    /*#endregion*/

    /*#region 函数:create()*/
    /*
    * 创建容器的结构
    */
    create: function()
    {
      var outString = '';

      outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>相关链接设置向导</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<table class="table-style" >';
      outString += '<tr class="table-row-normal-transparent" >';
      outString += '<td>';
      outString += '<span style="font-weight:bold; padding:0 10px 0 0px;" >链接名称</span><input id="' + this.name + '$wizardLinkText" type="text" class="input-normal" style="width:300px" />';
      outString += '</td>';
      outString += '</tr>';

      outString += '<tr class="table-row-normal-transparent" >';
      outString += '<td>';
      outString += '<span style="font-weight:bold; padding:0 10px 0 0px;">链接地址</span><input id="' + this.name + '$wizardLinkValue" type="text" class="input-normal" value="http://" style="width:300px" />';
      outString += '</td>';
      outString += '</tr>';

      outString += '<tr class="table-row-normal-transparent" >';
      outString += '<td>';
      outString += '<div style="margin:0 0 10px 62px;" >';
      outString += '<div class="float-left button-4font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.add();" class="btn btn-default" >添加链接</a></div>';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.save();" class="btn btn-default" >保存</a></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '</td>';
      outString += '</tr>';
      outString += '</table>';

      outString += '<div class="winodw-wizard-result-container" style="padding:0;" >';
      outString += '<table id="' + this.name + '$wizardLinkTable" class="table-style" style="width:100%;" >';
      outString += '<tr class="table-row-title" ><td >链接</td><td style="width:40px;">操作</td></tr>';
      outString += '</table>';
      outString += '</div>';

      outString += '<div class="clear"></div>';
      outString += '</div>';

      return outString;
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /*
    * 加载界面、数据、事件等信息
    */
    load: function()
    {
      // 设置遮罩对象
      if(typeof (options.maskWrapper) === 'undefined')
      {
        this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', {
          width: '418px',
          // height: '430px',
          top: 40
          // draggableWidth: 418
        });
      }
      else
      {
        this.maskWrapper = options.maskWrapper;
      }

      // 设置本地数据源
      if(typeof (options.localStorage) !== 'undefined' && options.localStorage !== '')
      {
        this.localStorage = options.localStorage;
      }

      // 加载遮罩和页面内容
      x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);

      // 加载事件
      $('#wizardLinkText').bind('keypress', function(e)
      {
        var event = window.event ? window.event : e;

        if(event.keyCode === '13')
        {
          if(event.srcElement.value !== '')
          {
            var textbox = document.getElementById("wizardLinkValue");
            var r = textbox.createTextRange();
            r.collapse(true);
            r.moveStart('character', textbox.value.length);
            r.select();
          }
          else
          {
            alert("【链接名称】必填。");
          }
        }
      });

      $('#wizardLinkValue').bind('keypress', function(e)
      {
        var event = window.event ? window.event : e;

        if(event.keyCode === "13")
        {
          if(event.srcElement.value !== '')
          {
            this.add();
          }
          else
          {
            alert("【链接地址】必填。");
          }
        }
      });

      this.linkText = document.getElementById(this.name + '$wizardLinkText');
      this.linkValue = document.getElementById(this.name + '$wizardLinkValue');
      this.linkTable = document.getElementById(this.name + '$wizardLinkTable');

      this.linkText.focus();

      // -------------------------------------------------------
      // 设置目标容器数据
      // -------------------------------------------------------

      if(this.localStorage !== null && this.localStorage.trim() !== '')
      {
        var list = x.toJSON(this.localStorage.trim()).list;

        for(var i = 0;i < list.length;i++)
        {
          var row = this.linkTable.insertRow(this.linkTable.rows.length);

          row.id = x.guid.create();
          row.className = 'table-row-normal';

          var column1 = row.insertCell(0);
          var column2 = row.insertCell(1);

          column1.innerHTML = '<a href="' + list[i].value + '" target="_blank" >' + list[i].text + '</a>';
          column2.innerHTML = '<a href="javascript:' + this.name + '.remove(\'' + row.id + '\');" >删除</a>';
        }
      }

      if(this.linkTable.rows.length === 1)
      {
        $(this.linkTable).hide();
      }
    }
    /*#endregion*/
  };

  return wizard;
};

/*#region 函数:x.ui.wizards.getRelatedLinkWizard(options)*/
/**
 * 快速创建选择向导
 */
x.ui.wizards.getRelatedLinkWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-' + options.targetViewName + '-' + options.targetValueName + '-related-link-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var targetValueObject = document.getElementById(options.targetValueName);

    // 配置参数
    options.localStorage = ((targetValueObject.tagName.toUpperCase() === 'INPUT' || targetValueObject.tagName.toUpperCase() === 'TEXTAREA') ? $(targetValueObject).val() : $(targetValueObject).html());

    // 初始化向导
    var wizard = x.ui.wizards.newRelatedLinkWizard(name, options.targetViewName, options.targetValueName, options);

    // 加载界面、数据、事件
    wizard.load();

    // 绑定到Window对象
    window[name] = wizard;
  }
  else
  {
    window[name].open();
  }
};
/*#endregion*/

/*#region 函数:x.ui.wizards.setRelatedLinkView(targetViewName, localStorage, options)*/
/**
 * 将JSON格式的数据转换成可阅读方式
 */
x.ui.wizards.setRelatedLinkView = function(targetViewName, localStorage, options)
{
  if(typeof (options) === 'undefined')
  {
    options = {
      displayInline: 0
    };
  }

  if(typeof (localStorage) !== 'undefined' && localStorage.trim() !== '')
  {
    var outString = '';

    var list = x.toJSON(localStorage.trim()).list;

    if(list.length > 0)
    {
      if(typeof (options.prefixText) !== 'undefined')
      {
        outString += options.prefixText + ' ';
      }

      for(var i = 0;i < list.length;i++)
      {
        if(options.displayInline === 1)
        {
          outString += '<span style="margin:0 10px 0 0;"><a href="' + list[i].value + '" target="_blank" >' + list[i].text + '</a></span>';
        }
        else
        {
          outString += '<div class="related-link-item" ><a href="' + list[i].value + '" target="_blank" >' + list[i].text + '</a></div>';
        }
      }

      var targetViewObject = document.getElementById(targetViewName);

      $(targetViewObject).html(outString);

      if(options.displayInline === 0)
      {
        $(targetViewObject).height(list.length * 18);

        if($(targetViewObject).height() < 18)
        {
          $(targetViewObject).height(18);
        }
      }
    }
  }
};
/*#endregion*/