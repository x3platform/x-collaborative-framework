x.register('x.ui.wizards');

/*
* 人员选择向导(地址簿)
*/
x.ui.wizards.newContactsWizard = function(name, targetViewName, targetValueName, options)
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

    // 包括被禁用的对象
    includeProhibited: 0,

    // 联系人类型
    contactType: 127,

    // 本地数据源(JSON格式)
    localStorage: null,

    // 返回值
    returnValue: null,

    // 目标视图对象名称
    targetViewName: targetViewName,

    // 目标值对象名称
    targetValueName: targetValueName,

    /*#region 函数:findAll()*/
    /*
    * 根据关键字查询
    */
    findAll: function()
    {
      var key = $(document.getElementById(this.name + '-wizardSearchText')).val().trim();

      // 少于两个字符由于匹配结果太多，所以不查询数据
      if(key === '' || key.length < 2) { return; }

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[findAll]]></action>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<contactType><![CDATA[' + this.contactType + ']]></contactType>';
      outString += '<includeProhibited><![CDATA[' + this.includeProhibited + ']]></includeProhibited>';
      outString += '<key><![CDATA[' + key + ']]></key>';
      outString += '</ajaxStorage>';

      x.net.xhr('/api/membership.contacts.findAll.aspx', outString, {
        callback: this.findAll_callback
      });
    },
    /*#endregion*/

    /*#region 函数:findAllByOrganizationId(organizationId)*/
    findAllByOrganizationId: function(organizationId)
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[findAllByOrganizationId]]></action>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<contactType><![CDATA[' + this.contactType + ']]></contactType>';
      outString += '<includeProhibited><![CDATA[' + this.includeProhibited + ']]></includeProhibited>';
      outString += '<organizationId><![CDATA[' + organizationId + ']]></organizationId>';
      outString += '</ajaxStorage>';

      x.net.xhr('/api/membership.contacts.findAllByOrganizationId.aspx', outString, {
        callback: this.findAll_callback
      });
    },
    /*#endregion*/

    /*#region 函数:findAllByStandardOrganizationId(standardOrganizationId)*/
    findAllByStandardOrganizationId: function(standardOrganizationId)
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[findAllByStandardOrganizationId]]></action>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<contactType><![CDATA[' + this.contactType + ']]></contactType>';
      outString += '<includeProhibited><![CDATA[' + this.includeProhibited + ']]></includeProhibited>';
      outString += '<standardOrganizationId><![CDATA[' + standardOrganizationId + ']]></standardOrganizationId>';
      outString += '</ajaxStorage>';

      x.net.xhr('/api/membership.contacts.findAllByStandardOrganizationId.aspx', outString, {
        callback: this.findAll_callback
      });
    },
    /*#endregion*/

    /*#region 函数:findAllByGroupTreeNodeId(groupType, groupTreeNodeId)*/
    findAllByGroupTreeNodeId: function(groupType, groupTreeNodeId)
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<action><![CDATA[findAllByGroupNodeId]]></action>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<groupType><![CDATA[' + groupType + ']]></groupType>';
      outString += '<includeProhibited><![CDATA[' + this.includeProhibited + ']]></includeProhibited>';
      outString += '<groupTreeNodeId><![CDATA[' + groupTreeNodeId + ']]></groupTreeNodeId>';
      outString += '</ajaxStorage>';

      x.net.xhr('/api/membership.contacts.findAllByGroupNodeId.aspx', outString, {
        callback: this.findAll_callback
      });
    },
    /*#endregion*/

    /*#region 函数:findAll_callback(response)*/
    findAll_callback: function(response)
    {
      var outString = '';

      var result = x.toJSON(response);

      var clientTargetObject = result.clientTargetObject;

      var list = result.data;

      outString += '{"list":[';

      x.each(list, function(index, node)
      {
        outString += '{"text":"' + node.name + '", "value":"' + node.type + '#' + node.id + '#' + node.name + '"},';
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

      x.dom.util.select.convert(clientTargetObject + '-wizardOriginalContainer', x.toJSON(outString).list);
    },
    /*#endregion*/

    /*#region 函数:view()*/
    view: function()
    {
      if($(document.getElementById(this.name + '-wizardOriginalContainer')).val() !== null)
      {
        var outString = '<?xml version="1.0" encoding="utf-8"?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[view]]></action>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<key>' + $(document.getElementById(this.name + '-wizardOriginalContainer')).val() + '</key>';
        outString += '</ajaxStorage>';

        x.net.xhr('/api/membership.contacts.view.aspx', outString, {
          callback: function(response)
          {
            var clientTargetObject = x.toJSON(response).clientTargetObject;

            $(document.getElementById(clientTargetObject + '-wizardHelpText')).html(x.toJSON(response).message.value);
          }
        });
      }
    },
    /*#endregion*/

    /*#region 函数:open()*/
    open: function()
    {
      if($(document.getElementById(this.maskWrapper.name)).children('.ajax-mask-popup-wrapper').size() === 0)
      {
        this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper');

        // 加载遮罩
        var element = this.maskWrapper.open();

        // 加载页面结构
        element.innerHTML = this.create();

        this.maskWrapper.resize();

        // 加载数据
        this.parseTreeView();

        $(document.getElementById(this.name + '-wizardSearchText')).bind("keyup", function()
        {
          window[this.id.replace('-wizardSearchText', '')].findAll();
        });

        $(document.getElementById(this.name + '-wizardBtnFilter')).bind("click", function()
        {
          window[this.id.replace('-wizardBtnFilter', '')].findAll();
        });

        //
        // 设置目标容器数据
        //

        if(this.localStorage !== null)
        {
          x.dom.util.select.convert(this.name + '-wizardTargetContainer', x.toJSON(this.localStorage).list);

          this.returnValue = this.localStorage;
        }
      }

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

      var targetContainer = document.getElementById(this.name + '-wizardTargetContainer');

      for(var i = 0;i < targetContainer.options.length;i++)
      {
        if(targetContainer.options[i].value !== '')
        {
          outString += '{"text":"' + targetContainer.options[i].text + '","value":"' + targetContainer.options[i].value + '"},';

          // 单选处理
          if(this.multiSelection === 0) { break; }
        }
      }

      var originalContainer = document.getElementById(this.name + '-wizardOriginalContainer');

      for(var index = 0;index < originalContainer.options.length;index++)
      {
        if(isSelectAll || originalContainer.options[index].selected)
        {
          var isInsert = true;

          for(var i = 0;i < targetContainer.options.length;i++)
          {
            // 判断已存在的值，则不插入数据
            if(originalContainer.options[index].value.toLowerCase() === targetContainer.options[i].value.toLowerCase()
            || (targetContainer.options[i].value.toLowerCase().substr(0, targetContainer.options[i].value.lastIndexOf('#')) !== '' && originalContainer.options[index].value.toLowerCase().indexOf(targetContainer.options[i].value.toLowerCase().substr(0, targetContainer.options[i].value.lastIndexOf('#'))) === 0))
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

      x.dom.util.select.convert(this.name + '-wizardTargetContainer', x.toJSON(outString).list);

      this.returnValue = outString;
    },
    /*#endregion*/

    /*#region 函数:remove(isSelectAll)*/
    /*
    * 移除节点
    */
    remove: function(isSelectAll)
    {
      var targetContainer = document.getElementById(this.name + '-wizardTargetContainer');

      if(isSelectAll)
      {
        x.dom.util.select.clear(this.name + '-wizardTargetContainer');

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

        x.dom.util.select.convert(this.name + '-wizardTargetContainer', x.toJSON(outString).list);

        // parent window callback

        this.returnValue = outString;
      }
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
      var outString = '{"list":[';

      var targetContainer = document.getElementById(this.name + '-wizardTargetContainer');

      for(var i = 0;i < targetContainer.options.length;i++)
      {
        outString += '{"text":"' + targetContainer.options[i].text + '","value":"' + targetContainer.options[i].value + '"},';
      }

      if(outString.substr(outString.length - 1, 1) === ',')
      {
        outString = outString.substr(0, outString.length - 1);
      }

      outString += ']}';

      this.returnValue = this.localStorage = outString;

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
        resultView = resultView.substr(0, resultView.length - 1);
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

      // 绑定 selectedText 和 data 属性
      $('#' + this.targetValueName).attr('selectedText', resultView);
      $('#' + this.targetValueName).attr('data', resultValue);
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

    /*#region 函数:getTreeView()*/
    /*
    * 获取树形菜单
    */
    getTreeView: function(treeViewType)
    {
      var treeViewId = '';
      var treeViewName = '';
      var treeViewRootTreeNodeId = '';
      var treeViewUrl = '';

      switch(treeViewType)
      {
        case 'organization':
          treeViewId = '10000000-0000-0000-0000-000000000000';
          treeViewName = '组织架构';
          treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';
          treeViewUrl = 'javascript:' + this.name + '.findAllByOrganizationId(\'{treeNodeId}\')';
          break;
        case 'my-corporation':
          treeViewId = '20000000-0000-0000-0000-000000000000';
          treeViewName = '我的公司';
          treeViewRootTreeNodeId = '20000000-0000-0000-0000-000000000000';
          treeViewUrl = 'javascript:' + this.name + '.findAllByOrganizationId(\'{treeNodeId}\')';
          break;
        case 'group':
          treeViewId = '40000000-0000-0000-0000-000000000000';
          treeViewName = '常用群组';
          treeViewRootTreeNodeId = '40000000-0000-0000-0000-000000000000';
          treeViewUrl = 'javascript:' + this.name + '.findAllByGroupTreeNodeId(\'group\',\'{treeNodeId}\')';
          break;
        case 'general-role':
          treeViewId = '50000000-0000-0000-0000-000000000000';
          treeViewName = '通用角色';
          treeViewRootTreeNodeId = '50000000-0000-0000-0000-000000000000';
          treeViewUrl = 'javascript:' + this.name + '.findAllByGroupTreeNodeId(\'general-role\',\'{treeNodeId}\')';
          break;
        case 'workflow-role':
          treeViewId = '60000000-0000-0000-0000-000000000000';
          treeViewName = '流程角色';
          treeViewRootTreeNodeId = '60000000-0000-0000-0000-000000000000';
          treeViewUrl = 'javascript:' + this.name + '.findAllByGroupTreeNodeId(\'workflow-role\',\'{treeNodeId}\')';
          break;
        case 'standard-organization':
          treeViewId = '60000000-0000-0000-0000-000000000000';
          treeViewName = '标准组织';
          treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';
          treeViewUrl = 'javascript:' + this.name + '.findAllByStandardOrganizationId(\'{treeNodeId}\')';
          break;
        case 'standard-role':
          treeViewId = '70000000-0000-0000-0000-000000000000';
          treeViewName = '标准角色';
          treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';
          treeViewUrl = 'javascript:' + this.name + '.findAllByStandardOrganizationId(\'{treeNodeId}\')';
          break;
        case 'standard-general-role':
          treeViewId = '80000000-0000-0000-0000-000000000000';
          treeViewName = '标准通用角色';
          treeViewRootTreeNodeId = '80000000-0000-0000-0000-000000000000';
          treeViewUrl = 'javascript:' + this.name + '.findAllByGroupTreeNodeId(\'standard-general-role\',\'{treeNodeId}\')';
          break;
        default:
          treeViewId = '10000000-0000-0000-0000-000000000000';
          treeViewName = '组织架构';
          treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';
          treeViewUrl = 'javascript:' + this.name + '.findAllByOrganizationId(\'{treeNodeId}\')';
          break;
      }

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += '<treeViewType><![CDATA[' + treeViewType + ']]></treeViewType>';
      outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
      outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
      outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
      outString += '<tree><![CDATA[{tree}]]></tree>';
      outString += '<parentId><![CDATA[{parentId}]]></parentId>';
      outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
      outString += '</ajaxStorage>';

      var tree = x.ui.pkg.tree.newTreeView({ name: this.name + '.tree' });

      tree.setAjaxMode(true);

      tree.add({
        id: "0",
        parentId: "-1",
        name: treeViewName,
        url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
        title: treeViewName,
        target: '',
        icon: '/resources/images/tree/tree_icon.gif'
      });

      tree.load('/api/membership.contacts.getDynamicTreeView.aspx', false, outString);

      this.tree = tree;
      this.treeViewType = treeViewType;

      document.getElementById(this.name + '-wizardTreeViewContainer').innerHTML = tree.toString();
    },
    /*#endregion*/

    /*#region 函数:create()*/
    /*
    * 创建容器的结构
    */
    create: function()
    {
      var outString = '';

      outString += '<div id="' + this.name + '-wrapper" class="winodw-wizard-wrapper" style="width:720px; height:auto;" >';

      outString += '<div id="' + this.name + '-draggable" class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      // 解析菜单
      outString += this.parseToolbarItmes();
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      // 侧栏
      outString += '<div class="winodw-wizard-sidebar" >';
      outString += '<div class="winodw-wizard-search form-inline" ><input id="' + this.name + '-wizardSearchText" name="contactsSearchText" type="text" value="" class="table-sidebar-search-text form-control input-sm" /> <button id="' + this.name + '-wizardBtnFilter" class="btn btn-default btn-sm" title="查询"><i class="glyphicon glyphicon-search"></i></button></div>';
      outString += '<div id="' + this.name + '-wizardTreeViewContainer" class="winodw-wizard-tree-view" ></div>';
      outString += '</div>';

      // 选择框
      outString += '<div class="winodw-wizard-container" >';
      outString += '<select id="' + this.name + '-wizardOriginalContainer" multiple="multiple" ondblclick="' + this.name + '.add(false)" onclick="' + this.name + '.view()" >';
      outString += '</select>';
      outString += '</div>';

      outString += '<div class="winodw-wizard-help-toolbar" >';
      outString += '<div class="winodw-wizard-help-container" ><div id="' + this.name + '-wizardHelpText" class="winodw-wizard-help-text"></div></div>';
      outString += '<div class="float-left button-2font-wrapper" ><a href="javascript:' + this.name + '.add(false);" class="btn btn-default" >添加</a></div> ';
      if(this.multiSelection === 1)
      {
        // 多选处理
        outString += '<div class="float-left button-4font-wrapper" style="margin-left:10px;" ><a href="javascript:' + this.name + '.add(true);" class="btn btn-default" >全部添加</a></div> ';
      }
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<div class="winodw-wizard-container" >';
      outString += '<select id="' + this.name + '-wizardTargetContainer" multiple="multiple" ondblclick="' + this.name + '.remove(false)" >';
      outString += '</select>';
      outString += '</div>';

      outString += '<div class="winodw-wizard-help-toolbar" >';
      outString += '<div class="float-right button-2font-wrapper" ><a href="javascript:' + this.name + '.cancel();" class="btn btn-default" >取消</a></div> ';
      outString += '<div class="float-right button-2font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.save();" class="btn btn-default" >保存</a></div> ';
      outString += '<div class="float-left button-2font-wrapper" ><a href="javascript:' + this.name + '.remove(false);" class="btn btn-default" >删除</a></div> ';
      if(this.multiSelection === 1)
      {
        // 多选处理
        outString += '<div class="float-left button-4font-wrapper" style="margin-left:10px;" ><a href="javascript:' + this.name + '.remove(true);" class="btn btn-default" >全部删除</a></div> ';
      }
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<div class="clear"></div>';
      outString += '</div>';

      return outString;
    },
    /*#endregion*/

    /*#region 函数:parseTreeView()*/
    parseTreeView: function()
    {
      if(this.contactType === 8)
      {
        this.getTreeView('group');
      }
      else if(this.contactType === 16)
      {
        this.getTreeView('standard-organization');
      }
      else if(this.contactType === 32)
      {
        this.getTreeView('standard-role');
      }
      else if(this.contactType === 48)
      {
        this.getTreeView('standard-organization');
      }
      else if(this.contactType === 64)
      {
        this.getTreeView('standard-general-role');
      }
      else if(this.contactType === 128)
      {
        this.getTreeView('general-role');
      }
      else
      {
        this.getTreeView('my-corporation');
      }
    },
    /*#endregion*/

    /*#region 函数:parseToolbarItmes()*/
    parseToolbarItmes: function()
    {
      var outString = '';

      if(this.contactType === 1 || this.contactType === 2 || this.contactType === 4)
      {
        // 人员、组织、角色
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'organization\')" >组织架构</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'my-corporation\')" >我的公司</a></div>';
      }
      else if(this.contactType === 8)
      {
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'group\')" >常用群组</a></div>';
      }
      else if(this.contactType === 16)
      {
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'standard-organization\')" >标准组织</a></div>';
      }
      else if(this.contactType === 32)
      {
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'standard-role\')" >标准角色</a></div>';
      }
      else if(this.contactType === 48)
      {
        // 标准角色
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'standard-organization\')" >标准组织</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'standard-role\')" >标准角色</a></div>';
      }
      else if(this.contactType === 64)
      {
        outString += '<div class="winodw-wizard-toolbar-item" style="width:100px;" ><a href="javascript:' + this.name + '.getTreeView(\'standard-general-role\')" >标准通用角色</a></div>';
      }
      else if(this.contactType === 128)
      {
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'general-role\')" >通用角色</a></div>';
      }
      else if(this.contactType === 255)
      {
        // 全部
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'organization\')" >组织架构</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'my-corporation\')" >我的公司</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'group\')" >常用群组</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'general-role\')" >通用角色</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'standard-organization\')" >标准组织</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'standard-role\')" >标准角色</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:100px;" ><a href="javascript:' + this.name + '.getTreeView(\'standard-general-role\')" >标准通用角色</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'workflow-role\')" >流程角色</a></div>';
      }
      else if(this.contactType === 10240)
      {
        // 职位
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'organization\')" >职位</a></div>';
      }
      else if(this.contactType === 20480)
      {
        // 岗位
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'organization\')" >组织架构</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'my-corporation\')" >我的公司</a></div>';
      }
      else
      {
        // 默认
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'organization\')" >组织架构</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'my-corporation\')" >我的公司</a></div>';
        outString += '<div class="winodw-wizard-toolbar-item"><a href="javascript:' + this.name + '.getTreeView(\'group\')" >常用群组</a></div>';
      }

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
          top: 40,
          draggableWindowName: this.name + '-draggable',
          draggableWidth: 720,
          // draggableHeight: 430
        });
      }
      else
      {
        this.maskWrapper = options.maskWrapper;
      }

      // 设置选择的类别
      if(typeof (options.contactType) !== 'undefined')
      {
        this.contactType = options.contactType;
      }

      // 设置多项选择
      if(typeof (options.multiSelection) !== 'undefined')
      {
        this.multiSelection = options.multiSelection;
      }

      // 设置多项选择
      if(typeof (options.includeProhibited) !== 'undefined')
      {
        this.includeProhibited = options.includeProhibited;
      }

      // 设置本地数据源
      if(typeof (options.localStorage) !== 'undefined')
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

      // 加载遮罩和页面内容
      x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);

      // 加载数据
      this.parseTreeView();

      $(document.getElementById(this.name + '-wizardSearchText')).bind("keyup", function()
      {
        window[this.id.replace('-wizardSearchText', '')].findAll();
      });

      $(document.getElementById(this.name + '-wizardBtnFilter')).bind("click", function()
      {
        window[this.id.replace('-wizardBtnFilter', '')].findAll();
      });

      // -------------------------------------------------------
      // 设置目标容器数据
      // -------------------------------------------------------

      if(this.localStorage !== null)
      {
        x.dom.util.select.convert(this.name + '-wizardTargetContainer', x.toJSON(this.localStorage).list);

        this.returnValue = this.localStorage;
      }
    }
    /*#endregion*/
  };

  return wizard;
};

/*#region 函数:x.ui.wizards.setContactsView(targetViewName, targetValue)*/
/*
* 此函数适用于将只有数据的选项在客户端转换为可查看信息
*/
x.ui.wizards.setContactsView = function(targetViewName, targetValue)
{
  var resultView = '';

  if(typeof (targetValue) !== 'undefined' && targetValue.indexOf('#') > -1)
  {
    var list = targetValue.split(',');

    for(var i = 0;i < list.length;i++)
    {
      resultView += list[i].split('#')[2] + ';';
    }
  }

  resultView = x.string.rtrim(resultView, ';')

  if(document.getElementById(targetViewName).tagName.toUpperCase() == 'INPUT'
      || document.getElementById(targetViewName).tagName.toUpperCase() == 'TEXTAREA')
  {
    $(document.getElementById(targetViewName)).val(resultView);
  }
  else
  {
    $(document.getElementById(targetViewName)).html(resultView);
  }
};
/*#endregion*/

/*#region 函数:x.ui.wizards.setContactType(contactTypeText)*/
/*
* 快速创建多项选择向导单例
*/
x.ui.wizards.setContactType = function(contactTypeText)
{
  switch(contactTypeText)
  {
    case 'account': return 1;
    case 'organization': return 2;
    case 'role': return 4;
    case 'group': return 8;
    case 'standard-organization': return 16;
    case 'standard-role': return 32;
    case 'standard-general-role': return 64;
    case 'general-role': return 128;
    case 'job': return 10240;
    case 'assignedJob': return 20480;
    case 'contact': return 65536;
    case 'all': return 255;
    case 'default': return 7;
    default: return contactTypeText;
  }
}
/*#endregion*/

/*#region 函数:x.ui.wizards.getContactsWizard(options)*/
/*
* 快速创建多项选择向导单例
*/
x.ui.wizards.getContactsWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '$' + options.targetViewName + '$' + options.targetValueName + '$contacts-wizard');

  // 设置类型
  options.contactType = 7;

  options.contactType = x.ui.wizards.setContactType(options.contactTypeText);

  // 设置数据源
  var viewArray = $(document.getElementById(options.targetViewName)).val().split(';');
  var valueArray = $(document.getElementById(options.targetValueName)).val().replace(/;/g, ',').split(',');

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

  options.localStorage = storage;

  // 初始化向导
  var wizard = x.ui.wizards.newContactsWizard(name, options.targetViewName, options.targetValueName, options);

  // 加载界面、数据、事件
  wizard.load();

  // 绑定到Window对象
  window[name] = wizard;
};
/*#endregion*/

/*#region 函数:x.ui.wizards.getContactWizard(options)*/
/*
* 快速创建单项选择向导单例
*/
x.ui.wizards.getContactWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-' + options.targetViewName + '-' + options.targetValueName + '-contacts-wizard');

  // 设置类型
  options.contactType = 1;

  options.contactType = x.ui.wizards.setContactType(options.contactTypeText);

  // 设置数据源
  var authorizationObjectType = $('#' + options.targetTypeName).val();
  var authorizationObjectName = $('#' + options.targetViewName).val();
  var authorizationObjectId = $('#' + options.targetValueName).val();

  options.multiSelection = 0;

  options.localStorage = '{"list":[{"text":"' + authorizationObjectName + '","value":"' + (authorizationObjectType == '' ? authorizationObjectId : authorizationObjectType) + '#' + authorizationObjectId + '#' + authorizationObjectName + '"}]}';

  options.save_callback = function(response)
  {
    var authorizationObjectType = '';
    var authorizationObjectName = '';
    var authorizationObjectId = '';

    var list = x.toJSON(response).list;

    // 取最后一条数据
    x.each(list, function(index, node)
    {
      authorizationObjectType += node.value.substring(0, node.value.indexOf('#'));
      authorizationObjectName += node.text;
      authorizationObjectId += node.value.substring(node.value.indexOf('#') + 1, node.value.lastIndexOf('#'));
    });
    
    // [可选] targetTypeName 
    if($('#' + options.targetTypeName).size() == 1)
    {
      if(document.getElementById(options.targetTypeName).tagName.toUpperCase() == 'INPUT'
           || document.getElementById(options.targetTypeName).tagName.toUpperCase() == 'TEXTAREA')
      {
        $('#' + options.targetTypeName).val(authorizationObjectType);
      }
      else
      {
        $('#' + options.targetTypeName).html(authorizationObjectType);
      }
    }

    if(document.getElementById(this.targetViewName).tagName.toUpperCase() == 'INPUT'
            || document.getElementById(this.targetViewName).tagName.toUpperCase() == 'TEXTAREA')
    {
      $('#' + this.targetViewName).val(authorizationObjectName);
    }
    else
    {
      $('#' + this.targetViewName).html(authorizationObjectName);
    }

    if(document.getElementById(this.targetValueName).tagName.toUpperCase() == 'INPUT'
            || document.getElementById(this.targetValueName).tagName.toUpperCase() == 'TEXTAREA')
    {
      $('#' + this.targetValueName).val(authorizationObjectId);
    }
    else
    {
      $('#' + this.targetValueName).html(authorizationObjectId);
    }

    // 绑定 selectedText 和 data 属性
    $('#' + this.targetValueName).attr('selectedText', authorizationObjectName);
    $('#' + this.targetValueName).attr('data', this.options.contactTypeText + '#' + authorizationObjectId + '#' + authorizationObjectName);
  };

  // 初始化向导
  var wizard = x.ui.wizards.newContactsWizard(name, options.targetViewName, options.targetValueName, options);

  // 加载界面、数据、事件
  wizard.load();

  // 绑定到Window对象
  window[name] = wizard;
};
/*#endregion*/

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