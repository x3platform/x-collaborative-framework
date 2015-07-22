x.register('x.wizards');

/*
* 项目选择向导
*/
x.wizards.newProjectWizard = function(name, targetViewName, targetValueName, options)
{
  var wizard = {

    // 实例名称
    name: name,

    // 配置信息
    options: options,

    // 遮罩
    maskWrapper: null,

    // 树型对象
    tree: null,

    // 多项选择
    multiSelection: 1,

    // 本地数据源(JSON格式)
    localStorage: null,

    // 返回值
    returnValue: null,

    // 目标视图对象名称
    targetViewName: targetViewName,

    // 目标值对象名称
    targetValueName: targetValueName,

    // SQL查询条件
    whereClause: '',

    /*
    * 查询
    */
    filter: function()
    {
      var whereClauseValue = '';

      var key = $(document.getElementById(this.name + '$wizardSearchText')).val();

      if(key.trim() !== '')
      {
        whereClauseValue = ' T.Name LIKE ##%' + key + '%## ';

        this.whereClause = whereClauseValue;

        this.findAll();
      }
    },

    /*
    * 根据关键字查询
    */
    findAll: function()
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<whereClause><![CDATA[' + this.whereClause + ']]></whereClause>';
      outString += '<length><![CDATA[0]]></length>';
      outString += '</ajaxStorage>';

      x.net.xhr('/api/project.findAll.aspx', outString, { callback: this.findAll_callback });
    },

    findAllByCorporationId: function(corporationId)
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<corporationId><![CDATA[' + corporationId + ']]></corporationId>';
      outString += '</ajaxStorage>';

      x.net.xhr('/api/project.findAllByCorporationId.aspx', outString, { callback: this.findAll_callback });
    },

    findAll_callback: function(response)
    {
      var outString = '';

      var result = x.toJSON(response);

      var clientTargetObject = result.clientTargetObject;

      var list = result.data;

      outString += '{"list":[';

      x.each(list, function(index, node)
      {
        outString += '{"text":"' + node.name + '", "value":"project#' + node.id + '#' + node.name + '"},';
      });

      if(outString === '{[')
      {
        return;
      }
      else if(outString.substr(outString.length - 1, 1) === ',')
      {
        outString = outString.substr(0, outString.length - 1);
      }

      outString += ']}';

      x.dom.util.select.convert(clientTargetObject + '$wizardOriginalContainer', x.toJSON(outString).list);
    },

    /*#region 函数:open()*/
    open: function()
    {
      this.maskWrapper.open();
    },
    /*#endregion*/

    /*#region 函数:add(isSelectAll)*/
    /*
    * 添加
    */
    add: function(isSelectAll)
    {
      // 单选处理
      if(this.multiSelection === 0) { this.remove(true); }

      var outString = '{"list":[';

      var targetContainer = document.getElementById(this.name + '$wizardTargetContainer');

      for(var i = 0;i < targetContainer.options.length;i++)
      {
        if(targetContainer.options[i].value !== '')
        {
          outString += '{"text":"' + targetContainer.options[i].text + '","value":"' + targetContainer.options[i].value + '"},';

          // 单选处理
          if(this.multiSelection === 0) { break; }
        }
      }

      var originalContainer = document.getElementById(this.name + '$wizardOriginalContainer');

      for(var index = 0;index < originalContainer.options.length;index++)
      {
        if(isSelectAll || originalContainer.options[index].selected)
        {
          var isInsert = true;

          for(var i = 0;i < targetContainer.options.length;i++)
          {
            if(originalContainer.options[index].value === targetContainer.options[i].value)
            {
              isInsert = false;
              break;
            }
          }

          if(isInsert)
          {
            outString += '{"text":"' + originalContainer.options[index].text + '","value":"' + originalContainer.options[index].value + '"},';

            // 单选处理
            if(this.multiSelection === 0) { break; }
          }
        }
      }

      if(outString === '{"list":[')
      {
        return;
      }
      else if(outString.substr(outString.length - 1, 1) === ',')
      {
        outString = outString.substr(0, outString.length - 1);
      }

      outString += ']}';

      x.dom.util.select.convert(this.name + '$wizardTargetContainer', x.toJSON(outString).list);

      this.returnValue = outString;
    },
    /*#endregion*/

    /*#region 函数:remove(isSelectAll)*/
    /*
    * 移除节点
    */
    remove: function(isSelectAll)
    {
      var targetContainer = document.getElementById(this.name + '$wizardTargetContainer');

      if(isSelectAll)
      {
        x.dom.util.select.clear(this.name + '$wizardTargetContainer');

        this.returnValue = '{"list":[]}';
      }
      else
      {
        var outString = '{"list":[';

        for(var i = 0;i < targetContainer.options.length;i++)
        {
          if(!targetContainer.options[i].selected)
          {
            outString += '{"text":"' + targetContainer.options[i].text + '","value":"' + targetContainer.options[i].value + '"},';
          }
        }

        //if(outString === '{"list":[') return;

        if(outString.substr(outString.length - 1, 1) === ',')
        {
          outString = outString.substr(0, outString.length - 1);
        }

        outString += ']}';

        x.dom.util.select.convert(this.name + '$wizardTargetContainer', x.toJSON(outString).list);

        // parent window callback

        this.returnValue = outString;
      }
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
      var outString = '{"list":[';

      var targetContainer = document.getElementById(this.name + '$wizardTargetContainer');

      for(var i = 0;i < targetContainer.options.length;i++)
      {
        outString += '{"text":"' + targetContainer.options[i].text + '","value":"' + targetContainer.options[i].value + '"},';
      }

      if(outString.substr(outString.length - 1, 1) === ',')
      {
        outString = outString.substr(0, outString.length - 1);
      }

      outString += ']}';

      this.returnValue = outString;

      if(typeof (options.save_callback) !== 'undefined')
      {
        this.save_callback = options.save_callback;
      }

      this.save_callback(this.returnValue);

      this.cancel();

      // 保存后执行的回调方法
      if(typeof (this.options.callback) !== 'undefined')
      {
        if(typeof (this.options.callback) === 'function')
        {
          this.options.callback();
        }
        else
        {
          if(this.options.callback !== '') { eval(this.options.callback); }
        }
      }
    },
    /*#endregion*/

    /*#region 函数:save_callback(response)*/
    save_callback: function(response)
    {
      var resultView = '';
      var resultValue = '';

      var list = x.toJSON(response).list;

      x.each(list, function(index, node)
      {
        resultView += node.text + ';';
        resultValue += node.value + ',';
      });

      if(resultValue.substr(resultValue.length - 1, 1) == ',')
      {
        resultView = resultView.substr(0, resultView.length - 1)
        resultValue = resultValue.substr(0, resultValue.length - 1);
      }

      if(document.getElementById(this.targetViewName).tagName.toUpperCase() == 'INPUT'
          || document.getElementById(this.targetViewName).tagName.toUpperCase() == 'TEXTAREA')
      {
        $(document.getElementById(this.targetViewName)).val(resultView);
      }
      else
      {
        $(document.getElementById(this.targetViewName)).html(resultView);
      }

      if(document.getElementById(this.targetValueName).tagName.toUpperCase() == 'INPUT'
          || document.getElementById(this.targetValueName).tagName.toUpperCase() == 'TEXTAREA')
      {
        $(document.getElementById(this.targetValueName)).val(resultValue);
      }
      else
      {
        $(document.getElementById(this.targetValueName)).html(resultValue);
      }
    },
    /*#endregion*/

    /*#region 函数:cancel()*/
    cancel: function()
    {
      this.maskWrapper.close();

      if(typeof (options.cancel_callback) !== 'undefined')
      {
        this.cancel_callback = options.cancel_callback;

        this.cancel_callback(this.returnValue);
      }
    },
    /*#endregion*/

    /*#region 函数:getTreeView()*/
    /*
    * 获取树形菜单
    */
    getTreeView: function()
    {
      var treeViewId = '10000000-0000-0000-0000-000000000000';
      var treeViewName = '组织架构';
      var treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';
      var treeViewUrl = 'javascript:' + this.name + '.setTreeViewNode(\'{treeNodeId}\',\'{treeNodeName}\')';

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[getCorporationTreeView]]></action>';
      outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
      outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
      outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
      outString += '<tree><![CDATA[{tree}]]></tree>';
      outString += '<parentId><![CDATA[{parentId}]]></parentId>';
      outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
      outString += '</ajaxStorage>';

      var tree = x.ui.pkg.tree.newTreeView(this.name + '.tree');

      tree.setAjaxMode(true);

      // tree.add("0", "-1", treeViewName, treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId), treeViewName, '', '/resources/images/tree/tree_icon.gif');

      tree.add({
        id: "0",
        parentId: "-1",
        name: treeViewName,
        url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
        title: treeViewName,
        target: '',
        icon: '/resources/images/tree/tree_icon.gif'
      });

      tree.load('/api/membership.organization.getCorporationTreeView.aspx', false, outString);

      this.tree = tree;

      document.getElementById(this.name + '$wizardTreeViewContainer').innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value, text)*/
    setTreeViewNode: function(value, text)
    {
      this.findAllByCorporationId(value.replace('[CorporationTreeNode]', ''));
    },
    /*#endregion*/

    /*#region 函数:create()*/
    /*
    * 创建容器的结构
    */
    create: function()
    {
      var outString = '';

      outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:720px; height:auto" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item"><span style="width:200px;" >项目选择向导</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      // 侧栏
      outString += '<div class="winodw-wizard-sidebar" >';
      outString += '<div class="winodw-wizard-search" ><input id="' + this.name + '$wizardSearchText" name="contactsSearchText" type="text" value="" class="winodw-wizard-search-text" /> <span id="' + this.name + '$wizardBtnFilter" class="winodw-wizard-search-button">查询</span></div>';
      outString += '<div id="' + this.name + '$wizardTreeViewContainer" class="winodw-wizard-tree-view" ></div>';
      outString += '</div>';

      // 选择框
      outString += '<div class="winodw-wizard-container" >';
      outString += '<select id="' + this.name + '$wizardOriginalContainer" multiple="multiple" ondblclick="' + this.name + '.add(false)" >';
      outString += '</select>';
      outString += '</div>';

      outString += '<div class="winodw-wizard-help-toolbar" >';
      // outString += '<div class="winodw-wizard-help-container" ><div class="winodw-wizard-help-text"></div></div>';
      outString += '<div class="float-left button-2font-wrapper" ><a href="javascript:' + this.name + '.add(false);" class="button-text" >添加</a></div> ';
      if(this.multiSelection === 1)
      {
        // 多选处理
        outString += '<div class="float-left button-4font-wrapper" style="margin-left:10px;" ><a href="javascript:' + this.name + '.add(true);" class="button-text" >全部添加</a></div> ';
      }
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<div class="winodw-wizard-container" >';
      outString += '<select id="' + this.name + '$wizardTargetContainer" multiple="multiple" ondblclick="' + this.name + '.remove(false)" >';
      outString += '</select>';
      outString += '</div>';

      outString += '<div class="winodw-wizard-help-toolbar" >';
      outString += '<div class="float-right button-2font-wrapper" ><a href="javascript:' + this.name + '.cancel();" class="button-text" >取消</a></div> ';
      outString += '<div class="float-right button-2font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >保存</a></div> ';
      outString += '<div class="float-left button-2font-wrapper" ><a href="javascript:' + this.name + '.remove(false);" class="button-text" >删除</a></div> ';
      if(this.multiSelection === 1)
      {
        // 多选处理
        outString += '<div class="float-left button-4font-wrapper" style="margin-left:10px;" ><a href="javascript:' + this.name + '.remove(true);" class="button-text" >全部删除</a></div> ';
      }
      outString += '</div>';
      outString += '<div class="clear"></div>';
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
          width: '720px',
          height: '430px',
          top: 40
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

      // 设置保存回调函数
      if(typeof (options.save_callback) !== 'undefined')
      {
        this.save_callback = options.save_callback;
      }

      // 设置取消回调函数
      if(typeof (options.cancel_callback) !== 'undefined')
      {
        this.cancel_callback = options.cancel_callback;
      }

      // 设置多项选择
      if(typeof (options.multiSelection) !== 'undefined')
      {
        this.multiSelection = options.multiSelection;
      }

      // 加载遮罩和页面内容
      x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);

      // 正常加载
      this.getTreeView();

      this.filter();

      $(document.getElementById(this.name + '$wizardSearchText')).bind("keyup", function()
      {
        window[this.id.replace('$wizardSearchText', '')].filter();
      });

      $(document.getElementById(this.name + '$wizardBtnFilter')).bind("click", function()
      {
        window[this.id.replace('$wizardBtnFilter', '')].filter();
      });

      // -------------------------------------------------------
      // 设置目标容器数据
      // -------------------------------------------------------

      if(this.localStorage !== null)
      {
        x.dom.util.select.convert(this.name + '$wizardTargetContainer', x.toJSON(this.localStorage).list);

        this.returnValue = this.localStorage;
      }
    }
    /*#endregion*/
  };

  return wizard;
};

/*#region 函数:x.wizards.getProjectsWizard(options)*/
/*
* 快速创建单例
*/
x.wizards.getProjectsWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '$' + options.targetViewName + '$' + options.targetValueName + '$projects$wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var viewArray = $(document.getElementById(options.targetViewName)).val().split(';');
    var valueArray = $(document.getElementById(options.targetValueName)).val().split(',');

    var storage = '{"list":[';

    for(var i = 0;i < valueArray.length;i++)
    {
      if(valueArray[i] !== '')
      {
        storage += '{"text":"' + viewArray[i] + '","value":"' + valueArray[i] + '"},';
      }
    }

    if(storage.substr(storage.length - 1, 1) === ',')
    {
      storage = storage.substr(0, storage.length - 1);
    }

    storage += ']}';

    // 配置参数
    options.multiSelection = 1;

    options.localStorage = storage;

    // 初始化向导
    var wizard = x.wizards.newProjectWizard(name, options.targetViewName, options.targetValueName, options);

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

/*#region 函数:x.wizards.getProjectWizard(options)*/
/*
* 快速创建单例
*/
x.wizards.getProjectWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '$' + options.targetViewName + '$' + options.targetValueName + '$project$wizard');

  if(typeof (window[name]) === 'undefined')
  {
    // 配置参数
    options.multiSelection = 0;

    // 设置数据源
    var projectName = x.dom('#' + options.targetViewName).val();
    var projectId = x.dom('#' + options.targetValueName).val();

    options.localStorage = '{"list":[{"text":"' + projectName + '","value":"project#' + projectId + '#' + projectName + '"}]}';

    options.save_callback = function(response)
    {
      var projectName = '';
      var projectId = '';

      var list = x.toJSON(response).list;

      x.each(list, function(index, node)
      {
        projectName += node.text + ';';
        projectId += node.value.substring(node.value.indexOf('#') + 1, node.value.lastIndexOf('#')) + ',';
      });

      if(projectId.substr(projectId.length - 1, 1) == ',')
      {
        projectName = projectName.substr(0, projectName.length - 1);
        projectId = projectId.substr(0, projectId.length - 1);
      }

      if(document.getElementById(this.targetViewName).tagName.toUpperCase() == 'INPUT'
          || document.getElementById(this.targetViewName).tagName.toUpperCase() == 'TEXTAREA')
      {
        $(document.getElementById(this.targetViewName)).val(projectName);
      }
      else
      {
        $(document.getElementById(this.targetViewName)).html(projectName);
      }

      if(document.getElementById(this.targetValueName).tagName.toUpperCase() == 'INPUT'
          || document.getElementById(this.targetValueName).tagName.toUpperCase() == 'TEXTAREA')
      {
        x.dom('#' + this.targetValueName).val(projectId);
      }
      else
      {
        x.dom('#' + this.targetValueName).html(projectId);
      }

      // 绑定 selectedText 和 data 属性
      x.dom('#' + this.targetValueName).attr('selectedText', projectName);
      x.dom('#' + this.targetValueName).attr('data', 'project#' + projectId + '#' + projectName);
    };

    // 初始化向导
    var wizard = x.wizards.newProjectWizard(name, options.targetViewName, options.targetValueName, options);

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

///*#region 函数:x.wizards.getProjectWizardSingleton(targetViewName, targetValueName, multiSelection, save_callback, cancel_callback)*/
///*
//* 快速创建项目选择向导(已过时)
//*/
//x.wizards.getProjectWizardSingleton = function(targetViewName, targetValueName, multiSelection, save_callback, cancel_callback)
//{
//    x.debug.warn('函数【x.wizards.getProjectWizardSingleton】已过时了，可以使用新的函数【x.wizards.getProjectWizard】。');

//    // 配置参数
//    var options = {
//        targetViewName: targetViewName,
//        targetValueName: targetValueName,
//        multiSelection: multiSelection,
//        save_callback: save_callback,
//        cancel_callback: cancel_callback
//    };

//    x.wizards.getProjectWizard(options);
//};
///*#endregion*/

/**
* select 元素相关的操作函数
*/
x.dom.util.select = {

  /**
  * 清空select元素中的所有option元素.
  */
  clear: function(selectName)
  {
    var select = x.dom.query(selectName)[0];

    // clear options
    try
    {
      if(x.net.browser.getName() == 'Internet Explorer')
      {
        var selectObjectLength = select.options.length;

        // IE
        while(selectObjectLength !== 0)
        {
          selectObjectLength = select.options.length;

          for(var i = 0;i < selectObjectLength;i++)
          {
            select.options.remove(i);
          }

          selectObjectLength = select.options.length;
        }
      }
      else
      {
        select.innerHTML = '';
      }
    }
    catch(ex)
    {
      select.innerHTML = ''; // Firefox
    }
  },

  /**
  * 添加 select 元素的 options 元素
  * @method add
  * @memberof x.dom.util.select
  */
  add: function(selectName, text, value)
  {
    var select = x.dom.query(selectName)[0];

    var option = document.createElement('option');

    option.value = value;
    option.innerHTML = text;

    select.appendChild(option);
  },

  /**
  * 把一个数组信息转换为 select 元素的 options 元素.
  */
  convert: function(selectName, options)
  {
    // clear options
    x.dom.util.select.clear(selectName);

    x.dom.util.select.append(selectName, options);
  },

  /**
  * 把一个数组信息追加为select元素的options元素.
  */
  append: function(selectName, options)
  {
    var select = x.dom.query(selectName)[0];

    // append options
    for(var i = 0;i < options.length;i++)
    {
      x.dom.util.select.add(selectName, options[i].text, options[i].value);
    }

    // 如果长度为1, 直接禁用.
    if(select.options.lenght == 1) { select.disabled = false; }
  },

  /**
  * 获取 select 元素的文本信息.
  * @method getText
  * @memberof x.dom.util.select
  */
  getText: function(selectName)
  {
    var select = x.dom.query(selectName)[0];

    var value = x.dom.util.select.getValue(selectName);

    for(var i = 0;i < select.options.length;i++)
    {
      if(select.options[i].value == value)
      {
        return select.options[i].text;
      }
    }

    return '';
  },

  /**
  * 获取 select 元素的值
  * @method getValue
  * @memberof x.dom.util.select
  */
  getValue: function(selectName)
  {
    document.getElementById(selectName).value;
  },

  /**
  * 设置 select 元素的值
  * @method setValue
  * @memberof x.dom.util.select
  */
  setValue: function(selectName, value)
  {
    var select = $(selectName);

    for(var i = 0;i < select.options.length;i++)
    {
      if(select.options[i].value == value)
      {
        try
        {
          select.selectedIndex = i;
          select.options[i].selected = true;
          return true;
        }
        catch(ex)
        {
        }
      }
    }

    select.selectedIndex = 0;
  }
};