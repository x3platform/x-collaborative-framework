﻿x.register('x.ui.wizards');

/**
 * 应用参数分组向导
 */
x.ui.wizards.newApplicationFeatureWizard = function(name, targetViewName, targetValueName, options)
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

    // 本地数据源(JSON格式)
    localStorage: null,

    // 返回值
    returnValue: null,

    // 目标视图对象名称
    targetViewName: targetViewName,

    // 目标值对象名称
    targetValueName: targetValueName,

    /*#region 函数:open()*/
    open: function()
    {
      this.maskWrapper.open();
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
      this.returnValue = '{"text":"' + $(document.getElementById(this.name + '-wizardApplicationFeatureText')).val() + '","value":"' + $(document.getElementById(this.name + '-wizardApplicationFeatureValue')).val() + '"}';

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
    /*
    * 默认回调函数，可根据需要自行修改此函数。
    */
    save_callback: function(response)
    {
      var result = x.toJSON(this.returnValue);

      if(document.getElementById(this.targetViewName).tagName.toUpperCase() == 'INPUT'
          || document.getElementById(this.targetViewName).tagName.toUpperCase() == 'TEXTAREA')
      {
        x.dom('#' + this.targetViewName).val(result.text);
      }
      else
      {
        x.dom('#' + this.targetViewName).html(result.text);
      }

      if(document.getElementById(this.targetValueName).tagName.toUpperCase() == 'INPUT'
          || document.getElementById(this.targetValueName).tagName.toUpperCase() == 'TEXTAREA')
      {
        x.dom('#' + this.targetValueName).val(result.value);
      }
      else
      {
        x.dom('#' + this.targetValueName).html(result.value);
      }
    },
    /*#endregion*/

    cancel: function()
    {
      this.maskWrapper.close();

      if(typeof (this.cancel_callback) !== 'undefined')
      {
        this.cancel_callback(this.returnValue);
      }
    },

    /*#region 函数:getTreeView(treeViewId, treeViewName)*/
    /**
     * 获取树形菜单
     */
    getTreeView: function(treeViewId, treeViewName)
    {
      var treeViewId = treeViewId;
      var treeViewName = treeViewName;
      var treeViewRootTreeNodeId = treeViewId;
      var treeViewUrl = 'javascript:' + this.name + '.setTreeViewNode(\'{treeNodeId}\',\'{treeNodeName}\')';

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
      outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
      outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
      outString += '<tree><![CDATA[{tree}]]></tree>';
      outString += '<parentId><![CDATA[{parentId}]]></parentId>';
      outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
      outString += '</request>';

      var tree = x.ui.pkg.tree.newTreeView({ name: this.name + '.tree', ajaxMode: true });

      tree.add({
        id: "0",
        parentId: "-1",
        name: treeViewName,
        url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
        title: treeViewName,
        target: '',
        icon: '/resources/images/tree/tree_icon.gif'
      });

      tree.load('/api/application.feature.getDynamicTreeView.aspx', false, outString);

      this.tree = tree;

      document.getElementById(this.name + '-wizardTreeViewContainer').innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode (value, text)*/
    /**
     *
     */
    setTreeViewNode: function(value, text)
    {
      $('#' + this.name + '-wizardApplicationFeatureText').val(text);
      $('#' + this.name + '-wizardApplicationFeatureValue').val(value);

      this.returnValue = '{"text":"' + text + '","value":"' + value + '"}';
    },
    /*#endregion*/

    /*#region 函数:create()*/
    /**
     * 创建容器的结构
     */
    create: function()
    {
      var outString = '';

      outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:300px; height:auto;" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item" style="width:200px" ><span>应用功能选择向导</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<div id="' + this.name + '-wizardTreeViewContainer" class="winodw-wizard-tree-view" style="width:296px; height:300px;" ></div>';

      outString += '<div class="winodw-wizard-result-container form-inline text-right" >';
      outString += '<label class="winodw-wizard-result-item-text" >功能名称</label> ';
      outString += '<input id="' + this.name + '-wizardApplicationFeatureText" name="' + this.name + '-wizardApplicationFeatureText" type="text" value="" class="winodw-wizard-result-item-input form-control input-sm" style="width:160px" /> ';
      outString += '<input id="' + this.name + '-wizardApplicationFeatureValue" name="' + this.name + '-wizardApplicationFeatureValue" type="hidden" value="" />';
      outString += '<a href="javascript:' + this.name + '.save();" class="btn btn-default btn-sm" >确定</a>';
      outString += '</div>';

      return outString;
    },
    /*#endregion*/

    /*#region 函数:load()*/
    /**
     * 页面加载事件
     */
    load: function()
    {
      // 设置遮罩对象
      if(typeof (options.maskWrapper) === 'undefined')
      {
        this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', {
          width: '318px',
          // height: '430px',
          top: 40
          // draggableWidth: 318
        });
      }
      else
      {
        this.maskWrapper = options.maskWrapper;
      }

      // 设置本地数据源
      if(typeof (options.localStorage) !== 'undefined')
      {
        this.localStorage = options.localStorage;
      }

      // 设置创建后回调函数
      this.create_callback = options.create_callback || x.noop;

      // 设置保存后回调函数
      this.save_callback = options.save_callback || this.save_callback;

      // 设置取消回调函数
      this.cancel_callback = options.cancel_callback || x.noop;

      // 加载遮罩和页面内容
      x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);

      // 正常加载
      this.getTreeView(options.applicationId, options.applicationDisplayName);

      $('#' + this.name + '-wizardTreeViewContainer').width(296);

      // -------------------------------------------------------
      // 设置目标容器数据
      // -------------------------------------------------------

      if(this.localStorage !== null)
      {
        var node = x.toJSON(this.localStorage);

        $('#' + this.name + '-wizardApplicationFeatureText').val(node.text);
        $('#' + this.name + '-wizardApplicationFeatureValue').val(node.value);
      }
    }
  };

  return wizard;
};

/*#region 函数:x.ui.wizards.getApplicationFeatureWizard(options)*/
/*
* 快速创建单例
*/
x.ui.wizards.getApplicationFeatureWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-' + options.applicationId + '-' + options.targetViewName + '-' + options.targetValueName + '-application-feature-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    // 配置参数
    options.localStorage = '{"text":"' + x.dom('#' + options.targetViewName).val() + '","value":"' + x.dom('#' + options.targetValueName).val() + '"}';

    // 初始化向导
    var wizard = x.ui.wizards.newApplicationFeatureWizard(name, options.targetViewName, options.targetValueName, options);

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