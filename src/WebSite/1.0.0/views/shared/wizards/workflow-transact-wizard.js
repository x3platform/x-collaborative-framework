x.register('x.ui.wizards');

/*#region 函数:x.ui.wizards.newTransactWorkflowWizard(name, options)*/
/*
* 流程审批向导
*/
x.ui.wizards.newTransactWorkflowWizard = function(name, options)
{
  var wizard = {

    // 实例名称
    name: name,

    // 选项信息
    options: options,

    // 遮罩
    maskWrapper: null,

    /*#region 函数:open()*/
    open: function()
    {
      this.maskWrapper.open();
    },
    /*#endregion*/

    /*#region 函数:close()*/
    close: function()
    {
      this.maskWrapper.close();
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function(e)
    {
      var result = this.save_callback(e);

      if(result === 0)
      {
        this.close();
      }
    },
    /*#endregion*/

    /*#region 函数:save_callback()*/
    /*
    * 默认回调函数，可根据需要自行修改此函数。
    */
    save_callback: function()
    {
      var idea = $('#' + this.name + '-transactWorkflowRequest-wizard-idea').val().trim();

      if(this.options.isReject === 1 && idea === '')
      {
        alert('【驳回】操作必须填写意见。');
        return 1;
      }
      else if(idea === '')
      {
        // 默认意见为同意
        idea = '同意';
      }

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      if(this.options.isReject === 1)
      {
        // 驳回
        outString += '<action><![CDATA[gotoStartNodeWorkflowRequest]]></action>';
      }
      else if(this.options.isReview === 1)
      {
        // 会签
        outString += '<action><![CDATA[reviewWorkflowRequest]]></action>';
      }
      else
      {
        // 审批
        outString += '<action><![CDATA[transactWorkflowRequest]]></action>';
      }
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<id><![CDATA[' + $('#' + this.name + '-transactWorkflowRequest-wizard-id').val() + ']]></id>';
      outString += '<idea><![CDATA[' + idea + ']]></idea>';
      outString += '<historyNodeId><![CDATA[' + $('#' + this.name + '-transactWorkflowRequest-wizard-historyNodeId').val() + ']]></historyNodeId>';

      if(this.options.workflowRuntimeType === 'default')
      {
        // 默认工作流模板
        outString += '<customTemplateId><![CDATA[' + $('#' + this.name + '-transactWorkflowRequest-wizard-customTemplateId').val() + ']]></customTemplateId>';
      }
      else if(this.options.workflowRuntimeType === 'custom')
      {
        // 自定义工作流模板
        var customTemplateText = '';

        customTemplateText = $('#' + this.options.workflowTemplateContainerName).html().toUpperCase().trim();

        if(!(customTemplateText === '<DIV></DIV>' || customTemplateText === ''))
        {
          customTemplateText = x.workflow.designtime.createDesignXml();
        }

        if(customTemplateText === '')
        {
          alert('正在加载审批流程模板，请稍候...');
          return 1;
        }

        outString += '<customTemplateText><![CDATA[' + customTemplateText + ']]></customTemplateText>';
      }

      outString += '</request>';

      x.net.xhr(this.options.url, outString, {
        waitingMessage: i18n.net.waiting.commitTipText,
        popCorrectValue: 1,
        callback: function(response)
        {
          x.page.refreshParentWindow();
          x.page.close();
        }
      });

      return 0;
    },
    /*#endregion*/

    /*#region 函数:cancel()*/
    cancel: function()
    {
      if(typeof (this.cancel_callback) !== 'undefined')
      {
        this.cancel_callback();
      }

      this.close();
    },
    /*#endregion*/

    /*#region 函数:create()*/
    create: function()
    {
      var outString = '';

      var titileText = '同意';

      var actionText = '审批';

      if(this.options.isReject)
      {
        titileText = '驳回';
        actionText = '驳回';
      }
      else if(this.options.isReview)
      {
        titileText = '会签';
        actionText = '会签';
      }

      outString += '<div id="' + this.name + '-transactWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a id="' + this.name + '-transactWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>' + titileText + '</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

      outString += '<tr>';
      outString += '<td class="table-body-text" style="width:90px" ><span class="required-text">' + actionText + '人</span></td>';
      outString += '<td class="table-body-input" >';
      outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-id" name="' + this.name + '-forcedWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
      outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-historyNodeId" name="' + this.name + '-forcedWorkflowRequest-wizard-historyNodeId" type="hidden" value="' + this.options.historyNodeId + '" />';
      outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-actorId" name="' + this.name + '-forcedWorkflowRequest-wizard-actorId" type="hidden" value="' + this.options.actorId + '" />';
      outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-actorName" name="' + this.name + '-forcedWorkflowRequest-wizard-actorName" type="text" value="' + this.options.actorName + '" readonly="readonly" class="form-control" style="width:200px;" />';
      outString += '</td>';
      outString += '<tr>';
      outString += '<td class="table-body-text" ><span class="required-text">' + actionText + '意见</span></td>';
      outString += '<td class="table-body-input" ><textarea id="' + this.name + '-transactWorkflowRequest-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" >' + (titileText === '同意' ? 'OK' : '') + '</textarea></td>';
      outString += '</tr>';
      /*
      outString += '<tr class="table-row-normal-transparent" style="display:none;" >';
      outString += '<td class="table-body-text" >附件</td>';
      outString += '<td class="table-body-input" >';
      outString += '<iframe id="' + this.name + '-transactWorkflowRequest-wizard-workflowHistoryNodeUploadFileWizard" name="uploadFileWizard" style="height:20px;width:100%" frameborder="0" src="' + this.options.uploadFileWizard + '?entityId=' + this.options.historyNodeId + '&amp;entityClassName=Elane.X.WorkflowPlus.Model.WorkflowHistoryNodeInfo, Elane.X.WorkflowPlus&amp;attachmentFolder=workflow&amp;fileSizeLimit=10&amp;fileTypes=*.*"></iframe>';
      outString += '<div id="' + this.name + '-transactWorkflowRequest-wizard-workflowHistoryNodeAttachmentFiles"></div>';
      outString += '</td>';
      outString += '</tr>';
      */
      outString += '<tr>';
      outString += '<td ></td>';
      outString += '<td class="table-body-input" >';
      outString += '<div style="margin:0 0 4px 2px;" >';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-transactWorkflowRequest-wizard-save" href="javascript:void(0);" onclick="' + this.name + '.save(event);" class="btn btn-default" >确定</a></div>';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-transactWorkflowRequest-wizard-cancel" href="javascript:void(0);" onclick="' + this.name + '.cancel(event);" class="btn btn-default" >取消</a></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '</td>';
      outString += '</tr>';
      outString += '</table>';

      outString += '<div class="clear"></div>';
      outString += '</div>';

      return outString;
    },
    /*#endregion*/

    /*#region 函数:load()*/
    load: function()
    {
      // 设置遮罩对象
      if(typeof (options.maskWrapper) === 'undefined')
      {
        this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
      }
      else
      {
        this.maskWrapper = options.maskWrapper;
      }

      // 工作流运行时类型
      if(typeof (options.workflowRuntimeType) === 'undefined')
      {
        alert('参数【workflowRuntimeType】必须填写。');
        return;
      }
      else
      {
        if(options.workflowRuntimeType === 'default' && typeof (options.workflowTemplateId) === 'undefined')
        {
          alert('默认工作流模板，参数【workflowTemplateId】必须填写。');
          return;
        }

        if(options.workflowRuntimeType === 'custom' && typeof (options.workflowTemplateContainerName) === 'undefined')
        {
          alert('自定义工作流模板，参数【workflowTemplateContainerName】必须填写。');
          return;
        }
      }

      // 设置实体标识
      if(typeof (options.entityId) === 'undefined')
      {
        alert('参数【entityId】必须填写。');
        return;
      }

      // 设置历史节点标识
      if(typeof (options.historyNodeId) === 'undefined')
      {
        alert('参数【historyNodeId】必须填写。');
        return;
      }

      // 设置驳回标识
      if(typeof (options.isReject) === 'undefined')
      {
        options.isReject = 0;
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
    }
    /*#endregion*/
  };

  return wizard;
};
/*#endregion*/

/*#region 函数:x.ui.wizards.getTransactWorkflowWizard(options)*/
/*
* 快速创建单例
*/
x.ui.wizards.getTransactWorkflowWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-transactWorkflowRequest-wizard');

  // 初始化向导
  var wizard = x.ui.wizards.newTransactWorkflowWizard(name, options);

  // 加载界面、数据、事件
  wizard.load();

  // 绑定到Window对象
  window[name] = wizard;
};
/*#endregion*/

/*#region 函数:x.ui.wizards.newExecuteWorkflowRequestWizard(name, options)*/
/*
* 流程审批向导(基类)
*/
x.ui.wizards.newExecuteWorkflowRequestWizard = function(name, options)
{
  return x.ext(x.ui.wizards.newWizard(name, options), {

    /*#region 函数:appendWorkflowTemplate()*/
    /*
    * 附加工作流模板
    */
    appendWorkflowTemplate: function()
    {
      var outString = '';

      if(this.options.workflowRuntimeType === 'default')
      {
        var workflowTemplateId = $('#' + this.options.workflowTemplateContainerName).val();

        if(workflowTemplateId === '')
        {
          throw new Error('未定义相关的流程模板标识');
        }

        // 默认工作流模板
        outString += '<workflowTemplateId><![CDATA[' + workflowTemplateId + ']]></workflowTemplateId>';
      }
      else if(this.options.workflowRuntimeType === 'custom')
      {
        // 自定义工作流模板
        var workflowTemplateText = $('#' + this.options.workflowTemplateContainerName).html().toUpperCase().trim();

        if(!(workflowTemplateText === '<DIV></DIV>' || workflowTemplateText === ''))
        {
          workflowTemplateText = x.workflow.designtime.createDesignXml();
        }

        if(workflowTemplateText === '')
        {
          throw new Error('未定义相关的流程模板内容');
        }

        outString += '<workflowTemplateText><![CDATA[' + workflowTemplateText + ']]></workflowTemplateText>';
      }

      return outString;
    },
    /*#endregion*/

    /*#region 函数:execute(url, outString)*/
    /*
    * 执行工作流请求
    */
    execute: function(url, outString)
    {
      x.net.xhr(url, outString, {
        waitingMessage: i18n.net.waiting.commitTipText,
        popCorrectValue: 1,
        callback: function(response)
        {
          x.page.refreshParentWindow();
          x.page.close();
        }
      });

      return 0;
    },
    /*#endregion*/

    /*#region 函数:bindGeneralOptions(options)*/
    bindGeneralOptions: function(options)
    {
      // 工作流运行时类型
      if(typeof (options.workflowRuntimeType) === 'undefined')
      {
        alert('参数【workflowRuntimeType】必须填写。');
        return;
      }
      else
      {
        if(options.workflowRuntimeType === 'default' && typeof (options.workflowTemplateId) === 'undefined')
        {
          alert('默认工作流模板，参数【workflowTemplateId】必须填写。');
          return;
        }

        if(options.workflowRuntimeType === 'custom' && typeof (options.workflowTemplateContainerName) === 'undefined')
        {
          alert('自定义工作流模板，参数【workflowTemplateContainerName】必须填写。');
          return;
        }
      }

      // 设置实体标识
      if(typeof (options.entityId) === 'undefined')
      {
        alert('参数【entityId】必须填写。');
        return;
      }

      // 设置历史节点标识
      if(typeof (options.historyNodeId) === 'undefined')
      {
        alert('参数【historyNodeId】必须填写。');
        return;
      }
    },
    /*#endregion*/

    /*#region 函数:bindOptions(options)*/
    bindOptions: function(options)
    {
      this.bindGeneralOptions(options);
    },
    /*#endregion*/

    /*#region 函数:load()*/
    load: function()
    {
      // 设置遮罩对象
      if(typeof (options.maskWrapper) === 'undefined')
      {
        this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
      }
      else
      {
        this.maskWrapper = options.maskWrapper;
      }

      // 验证并绑定选项信息
      this.bindOptions(options);

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

      x.ui.mask.getWindow({ content: this.create() }, this.maskWrapper);

      x.dom.features.bind();
    }
    /*#endregion*/
  });
};
/*#endregion*/

/*#region 函数:x.ui.wizards.getTransactWorkflowRequestWizard(options)*/
/*
* 同意
*/
x.ui.wizards.getTransactWorkflowRequestWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-transactWorkflowRequest-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var wizard = x.ext(x.ui.wizards.newExecuteWorkflowRequestWizard(name, options), {

      /*#region 函数:save_callback()*/
      /*
      * 默认回调函数，可根据需要自行修改此函数。
      */
      save_callback: function(e)
      {
        try
        {
          var idea = $('#' + this.name + '-transactWorkflowRequest-wizard-idea').val().trim();

          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[transactWorkflowRequest]]></action>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<id><![CDATA[' + $('#' + this.name + '-transactWorkflowRequest-wizard-id').val() + ']]></id>';
          outString += '<idea><![CDATA[' + idea + ']]></idea>';
          outString += '<historyNodeId><![CDATA[' + $('#' + this.name + '-transactWorkflowRequest-wizard-historyNodeId').val() + ']]></historyNodeId>';
          outString += this.appendWorkflowTemplate();
          outString += '</request>';

          this.execute(this.options.url, outString);

          return 0;
        }
        catch(ex)
        {
          if(ex.message == '未定义相关的流程模板内容') { alert('正在加载审批流程模板，请稍候...'); return 1; }
          throw ex;
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<div id="' + this.name + '-transactWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a id="' + this.name + '-transactWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>同意</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:90px" ><span class="required-text">审批人</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-id" name="' + this.name + '-transactWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
        outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-historyNodeId" name="' + this.name + '-transactWorkflowRequest-wizard-historyNodeId" type="hidden" value="' + this.options.historyNodeId + '" />';
        // outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-actorId" name="' + this.name + '-transactWorkflowRequest-wizard-actorId" type="hidden" value="' + this.options.actorId + '" />';
        outString += '<input id="' + this.name + '-transactWorkflowRequest-wizard-actorName" name="' + this.name + '-transactWorkflowRequest-wizard-actorName" type="text" value="' + this.options.actorName + '" readonly="readonly" class="form-control" style="width:200px;" />';
        outString += '</td>';
        outString += '<tr>';
        outString += '<td class="table-body-text" ><span class="required-text">审批意见</span></td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '-transactWorkflowRequest-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" >OK</textarea></td>';
        outString += '</tr>';
        /*
        outString += '<tr class="table-row-normal-transparent" style="display:none;" >';
        outString += '<td class="table-body-text" >附件</td>';
        outString += '<td class="table-body-input" >';
        outString += '<iframe id="' + this.name + '-transactWorkflowRequest-wizard-workflowHistoryNodeUploadFileWizard" name="uploadFileWizard" style="height:20px;width:100%" frameborder="0" src="' + this.options.uploadFileWizard + '?entityId=' + this.options.historyNodeId + '&amp;entityClassName=Elane.X.WorkflowPlus.Model.WorkflowHistoryNodeInfo, Elane.X.WorkflowPlus&amp;attachmentFolder=workflow&amp;fileSizeLimit=10&amp;fileTypes=*.*"></iframe>';
        outString += '<div id="' + this.name + '-transactWorkflowRequest-wizard-workflowHistoryNodeAttachmentFiles"></div>';
        outString += '</td>';
        outString += '</tr>';
        */
        outString += '<tr>';
        outString += '<td ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-transactWorkflowRequest-wizard-save" href="javascript:void(0);" onclick="' + this.name + '.save(event);" class="btn btn-default" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-transactWorkflowRequest-wizard-cancel" href="javascript:void(0);" onclick="' + this.name + '.cancel(event);" class="btn btn-default" >取消</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      }
      /*#endregion*/
    });

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

/*#region 函数:x.ui.wizards.getReviewWorkflowRequestWizard(options)*/
/*
* 驳回
*/
x.ui.wizards.getReviewWorkflowRequestWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-reviewWorkflowRequest-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var wizard = x.ext(x.ui.wizards.newExecuteWorkflowRequestWizard(name, options), {

      /*#region 函数:save_callback()*/
      /*
      * 默认回调函数，可根据需要自行修改此函数。
      */
      save_callback: function(e)
      {
        try
        {
          var idea = $('#' + this.name + '-reviewWorkflowRequest-wizard-idea').val().trim();

          var idea = $('#' + this.name + '-reviewWorkflowRequest-wizard-idea').val().trim();

          if(idea === '')
          {
            alert('【驳回】操作必须填写意见。');
            return 1;
          }

          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[reviewWorkflowRequest]]></action>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<id><![CDATA[' + $('#' + this.name + '-reviewWorkflowRequest-wizard-id').val() + ']]></id>';
          outString += '<idea><![CDATA[' + idea + ']]></idea>';
          outString += '<historyNodeId><![CDATA[' + $('#' + this.name + '-reviewWorkflowRequest-wizard-historyNodeId').val() + ']]></historyNodeId>';
          outString += this.appendWorkflowTemplate();
          outString += '</request>';

          this.execute(this.options.url, outString);

          return 0;
        }
        catch(ex)
        {
          if(ex.message == '未定义相关的流程模板内容') { alert('正在加载审批流程模板，请稍候...'); return 1; }
          throw ex;
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<div id="' + this.name + '-reviewWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a id="' + this.name + '-reviewWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>会签</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:90px" ><span class="required-text">会签人</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="' + this.name + '-reviewWorkflowRequest-wizard-id" name="' + this.name + '-forcedWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
        outString += '<input id="' + this.name + '-reviewWorkflowRequest-wizard-historyNodeId" name="' + this.name + '-forcedWorkflowRequest-wizard-historyNodeId" type="hidden" value="' + this.options.historyNodeId + '" />';
        // outString += '<input id="' + this.name + '-reviewWorkflowRequest-wizard-actorId" name="' + this.name + '-forcedWorkflowRequest-wizard-actorId" type="hidden" value="' + this.options.actorId + '" />';
        outString += '<input id="' + this.name + '-reviewWorkflowRequest-wizard-actorName" name="' + this.name + '-forcedWorkflowRequest-wizard-actorName" type="text" value="' + this.options.actorName + '" readonly="readonly" class="form-control" style="width:200px;" />';
        outString += '</td>';
        outString += '<tr>';
        outString += '<td class="table-body-text" ><span class="required-text">会签意见</span></td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '-reviewWorkflowRequest-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" >OK</textarea></td>';
        outString += '</tr>';
        /*
        outString += '<tr class="table-row-normal-transparent" style="display:none;" >';
        outString += '<td class="table-body-text" >附件</td>';
        outString += '<td class="table-body-input" >';
        outString += '<iframe id="' + this.name + '-reviewWorkflowRequest-wizard-workflowHistoryNodeUploadFileWizard" name="uploadFileWizard" style="height:20px;width:100%" frameborder="0" src="' + this.options.uploadFileWizard + '?entityId=' + this.options.historyNodeId + '&amp;entityClassName=Elane.X.WorkflowPlus.Model.WorkflowHistoryNodeInfo, Elane.X.WorkflowPlus&amp;attachmentFolder=workflow&amp;fileSizeLimit=10&amp;fileTypes=*.*"></iframe>';
        outString += '<div id="' + this.name + '-reviewWorkflowRequest-wizard-workflowHistoryNodeAttachmentFiles"></div>';
        outString += '</td>';
        outString += '</tr>';
        */
        outString += '<tr>';
        outString += '<td ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-reviewWorkflowRequest-wizard-save" href="javascript:void(0);" onclick="' + this.name + '.save(event);" class="btn btn-default" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-reviewWorkflowRequest-wizard-cancel" href="javascript:void(0);" onclick="' + this.name + '.cancel(event);" class="btn btn-default" >取消</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      }
      /*#endregion*/
    });

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

/*#region 函数:x.ui.wizards.getGotoStartNodeWorkflowRequestWizard(options)*/
/*
* 驳回
*/
x.ui.wizards.getGotoStartNodeWorkflowRequestWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-gotoStartNodeWorkflowRequest-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var wizard = x.ext(x.ui.wizards.newExecuteWorkflowRequestWizard(name, options), {

      /*#region 函数:save_callback()*/
      /*
      * 默认回调函数，可根据需要自行修改此函数。
      */
      save_callback: function(e)
      {
        try
        {
          var idea = $('#' + this.name + '-gotoStartNodeWorkflowRequest-wizard-idea').val().trim();

          var idea = $('#' + this.name + '-gotoStartNodeWorkflowRequest-wizard-idea').val().trim();

          if(idea === '')
          {
            alert('【驳回】操作必须填写意见。');
            return 1;
          }

          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[gotoStartNodeWorkflowRequest]]></action>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<id><![CDATA[' + $('#' + this.name + '-gotoStartNodeWorkflowRequest-wizard-id').val() + ']]></id>';
          outString += '<idea><![CDATA[' + idea + ']]></idea>';
          outString += '<historyNodeId><![CDATA[' + $('#' + this.name + '-gotoStartNodeWorkflowRequest-wizard-historyNodeId').val() + ']]></historyNodeId>';
          outString += this.appendWorkflowTemplate();
          outString += '</request>';

          this.execute(this.options.url, outString);

          return 0;
        }
        catch(ex)
        {
          if(ex.message == '未定义相关的流程模板内容') { alert('正在加载审批流程模板，请稍候...'); return 1; }
          throw ex;
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<div id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>驳回</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:90px" ><span class="required-text">驳回人</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-id" name="' + this.name + '-forcedWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
        outString += '<input id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-historyNodeId" name="' + this.name + '-forcedWorkflowRequest-wizard-historyNodeId" type="hidden" value="' + this.options.historyNodeId + '" />';
        // outString += '<input id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-actorId" name="' + this.name + '-forcedWorkflowRequest-wizard-actorId" type="hidden" value="' + this.options.actorId + '" />';
        outString += '<input id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-actorName" name="' + this.name + '-forcedWorkflowRequest-wizard-actorName" type="text" value="' + this.options.actorName + '" readonly="readonly" class="form-control" style="width:200px;" />';
        outString += '</td>';
        outString += '<tr>';
        outString += '<td class="table-body-text" ><span class="required-text">驳回意见</span></td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" ></textarea></td>';
        outString += '</tr>';
        /*
        outString += '<tr class="table-row-normal-transparent" style="display:none;" >';
        outString += '<td class="table-body-text" >附件</td>';
        outString += '<td class="table-body-input" >';
        outString += '<iframe id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-workflowHistoryNodeUploadFileWizard" name="uploadFileWizard" style="height:20px;width:100%" frameborder="0" src="' + this.options.uploadFileWizard + '?entityId=' + this.options.historyNodeId + '&amp;entityClassName=Elane.X.WorkflowPlus.Model.WorkflowHistoryNodeInfo, Elane.X.WorkflowPlus&amp;attachmentFolder=workflow&amp;fileSizeLimit=10&amp;fileTypes=*.*"></iframe>';
        outString += '<div id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-workflowHistoryNodeAttachmentFiles"></div>';
        outString += '</td>';
        outString += '</tr>';
        */
        outString += '<tr>';
        outString += '<td ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-save" href="javascript:void(0);" onclick="' + this.name + '.save(event);" class="btn btn-default" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-gotoStartNodeWorkflowRequest-wizard-cancel" href="javascript:void(0);" onclick="' + this.name + '.cancel(event);" class="btn btn-default" >取消</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      }
      /*#endregion*/
    });

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

/*#region 函数:x.ui.wizards.getCommissionWorkflowRequestWizard(options)*/
/*
* 转交
*/
x.ui.wizards.getCommissionWorkflowRequestWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-commissionWorkflowRequest-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    // 初始化向导
    var wizard = x.ext(x.ui.wizards.newExecuteWorkflowRequestWizard(name, options), {

      // 转交人信息
      commissionActors: options.commissionActors,

      /*#region 函数:getCommissionActorWizard(options)*/
      getCommissionActorWizard: function(options)
      {
        x.ui.wizards.getWizard('commissionActorWizard', {

          save_callback: function()
          {
            var commissionActorText = x.form.checkbox.getValue(this.name + '-commissionActors');
            var commissionActorView = '';

            var list = commissionActorText.split(',');

            for(var i = 0;i < list.length;i++)
            {
              var keys = list[i].split('#');

              if(keys.length == 3) { commissionActorView += keys[2] + ';'; }
            }

            $('#' + options.targetViewName).val(commissionActorView);
            $('#' + options.targetValueName).val(commissionActorText);

            return 0;
          },

          create: function()
          {
            var outString = '';

            var classNameValue = '';

            var counter = 0;

            var list = options.storages.replace(/\[人员\]/g, '').split(',');

            outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:260px; height:auto;" >';

            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close">';
            outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
            outString += '</div>';
            outString += '<div class="float-left">';
            outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>转交人设置向导</span></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<table class="table-style" style="width:100%;">';
            outString += '<tr>';
            outString += '<td id="' + this.name + '-wizardTableContainer" class="table-body" >';

            outString += '<table class="table-style" style="width:100%">';
            outString += '<tbody>';
            outString += '<tr class="table-row-title">';
            outString += '<td >姓名</td>';
            outString += '</tr>';

            var that = this;

            list.each(function(node, index)
            {
              if(node !== '')
              {
                classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

                classNameValue = classNameValue + ((counter + 1) === list.length ? '-transparent' : '');

                var keys = node.split('#');

                outString += '<tr class="' + classNameValue + '" >';
                outString += '<td class="vertical-middle" >';
                outString += '<input id="' + that.name + '-commissionActor-' + (index + 1) + '" name="' + that.name + '-commissionActors" type="checkbox" value="' + node + '" /> ' + keys[2].replace('[人员]', '');
                outString += '</td>';
                outString += '</tr>';

                counter++;
              }
            });

            outString += '</td>';
            outString += '</tr>';
            outString += '</table>';

            outString += '<div class="winodw-wizard-result-container" >';
            outString += '<div class="winodw-wizard-result-item" style="float:right" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="btn btn-default" >确定</a></div></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<div class="clear"></div>';
            outString += '</div>';

            return outString;
          }
        });
      },
      /*#endregion*/

      /*#region 函数:save_callback()*/
      /*
      * 默认回调函数，可根据需要自行修改此函数。
      */
      save_callback: function()
      {
        try
        {
          var idea = $('#' + this.name + '-commissionWorkflowRequest-wizard-idea').val().trim();

          if(document.getElementById(this.name + '-commissionWorkflowRequest-wizard-commissionActorText').value === '')
          {
            alert('必须填写【转交人】。');
            return 1;
          }

          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[commissionWorkflowRequest]]></action>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<id><![CDATA[' + $('#' + this.name + '-commissionWorkflowRequest-wizard-id').val() + ']]></id>';
          outString += '<idea><![CDATA[' + idea + ']]></idea>';
          outString += '<workflowInstanceId><![CDATA[' + $('#' + this.name + '-commissionWorkflowRequest-wizard-workflowInstanceId').val() + ']]></workflowInstanceId>';
          outString += '<commissionActorText><![CDATA[' + $('#' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText').val() + ']]></commissionActorText>';
          outString += '<commissionIsRepeat><![CDATA[' + $('#' + this.name + '-commissionWorkflowRequest-wizard-commissionIsRepeat').val() + ']]></commissionIsRepeat>';
          outString += this.appendWorkflowTemplate();
          outString += '</request>';

          this.execute(this.options.url, outString);

          return 0;
        }
        catch(ex)
        {
          if(ex.message == '未定义相关的流程模板内容') { alert('正在加载审批流程模板，请稍候...'); return 1; }
          throw ex;
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<div id="' + this.name + '-transactWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a id="' + this.name + '-transactWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>转交</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:90px" ><span class="required-text">转交人</span></td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="' + this.name + '-commissionWorkflowRequest-wizard-id" name="' + this.name + '-commissionWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
        outString += '<input id="' + this.name + '-commissionWorkflowRequest-wizard-workflowInstanceId" name="' + this.name + '-commissionWorkflowRequest-wizard-workflowInstanceId" type="hidden" value="' + this.options.workflowInstanceId + '" />';
        outString += '<input id="' + this.name + '-commissionWorkflowRequest-wizard-actorName" name="' + this.name + '-commissionWorkflowRequest-wizard-actorName" type="text" value="' + this.options.actorName + '"  readonly="readonly" class="form-control" style="width:200px;" />';
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr>';
        outString += '<td class="table-body-text" ><span class="required-text">接收人</span></td>';
        outString += '<td class="table-body-input" >';
        if(this.commissionActors === 'role#00000000-0000-0000-0000-000000000000#Everyone')
        {
          outString += '<input id="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText" name="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText" feature="contacts" contactTypeText="account" contactMultiSelection="1" class="form-control" style="width:260px; height:40px;" />';
        }
        else
        {
          outString += '<input id="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText" name="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText" type="hidden" />';
          outString += '<textarea id="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText$$view" name="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText$$view" class="form-control" style="width:260px; height:40px;" ></textarea>';
          outString += '<div style="text-align:right; width:260px;"><a href="javascript:' + this.name + '.getCommissionActorWizard({targetViewName:\'' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText$$view\',targetValueName:\'' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText\',storages:\'' + this.commissionActors + '\'});" id="' + this.name + '-commissionWorkflowRequest-wizard-commissionActorText$$btnEdit">编辑</a></div>';
        }
        outString += '</td>';
        outString += '</tr>';

        outString += '<tr>';
        outString += '<td class="table-body-text" >转交意见</td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '-commissionWorkflowRequest-wizard-idea" name="' + this.name + '-commissionWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" ></textarea></td>';
        outString += '</tr>';

        outString += '<tr>';
        outString += '<td class="table-body-text" >重复转交</td>';
        outString += '<td class="table-body-input" ><input id="' + this.name + '-commissionWorkflowRequest-wizard-commissionIsRepeat" name="' + this.name + '-commissionWorkflowRequest-wizard-commissionIsRepeat" type="checkbox" feature="checkbox" value="1" /></td>';
        outString += '</tr>';

        outString += '<tr>';
        outString += '<td class="table-body-text" ></td>';
        outString += '<td class="table-body-input" ><div class="tip" style="width:240px;" >本流程实例因业务流转(驳回 前进 后退)重新进入此节点时, 是否允许再次执行转交操作.</div></td>';
        outString += '</tr>';

        outString += '<tr>';
        outString += '<td ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-transactWorkflowRequest-wizard-save" href="javascript:void(0);" onclick="' + this.name + '.save(event);" class="btn btn-default" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-transactWorkflowRequest-wizard-cancel" href="javascript:void(0);" onclick="' + this.name + '.cancel(event);" class="btn btn-default" >取消</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      }
      /*#endregion*/
    });

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

/*#region 函数:x.ui.wizards.getGotoAnyNodeWorkflowRequestWizard(options)*/
/*
* 快速创建单例
*/
x.ui.wizards.getGotoAnyNodeWorkflowRequestWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-gotoAnyNodeWorkflowRequest-wizard');

  // 初始化向导
  var wizard = x.ext(x.ui.wizards.newExecuteWorkflowRequestWizard(name, options), {

    /*#region 函数:save_callback()*/
    /*
    * 默认回调函数，可根据需要自行修改此函数。
    */
    save_callback: function()
    {
      try
      {
        var idea = $('#' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-idea').val().trim();

        if(document.getElementById(this.name + '-gotoAnyNodeWorkflowRequest-wizard-targetNodeId').value === '')
        {
          alert('必须填写【选择节点】。');
          return 1;
        }

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<action><![CDATA[gotoAnyNodeWorkflowRequest]]></action>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<id><![CDATA[' + $('#' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-id').val() + ']]></id>';
        outString += '<idea><![CDATA[' + idea + ']]></idea>';
        outString += '<historyNodeId><![CDATA[' + $('#' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-historyNodeId').val() + ']]></historyNodeId>';
        outString += '<targetNodeId><![CDATA[' + $('#' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-targetNodeId').val() + ']]></targetNodeId>';
        outString += this.appendWorkflowTemplate();
        outString += '</request>';

        this.execute(this.options.url, outString);

        return 0;
      }
      catch(ex)
      {
        if(ex.message == '未定义相关的流程模板内容') { alert('正在加载审批流程模板，请稍候...'); return 1; }
        throw ex;
      }
    },
    /*#endregion*/

    /*#region 函数:create()*/
    create: function()
    {
      var outString = '';

      var titileText = '节点跳转';

      var actionText = '处理';

      if(this.options.direction == 'forward')
      {
        titileText = '将节点向前跳转';
      }
      else if(this.options.direction == 'back')
      {
        titileText = '将节点向后跳转';
      }

      outString += '<div id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>' + titileText + '</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

      outString += '<tr>';
      outString += '<td class="table-body-text" style="width:90px" ><span class="required-text">' + actionText + '人</span></td>';
      outString += '<td class="table-body-input" >';
      outString += '<input id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-id" name="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
      outString += '<input id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-historyNodeId" name="' + this.name + '-forcedWorkflowRequest-wizard-historyNodeId" type="hidden" value="' + this.options.historyNodeId + '" />';
      // outString += '<input id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-actorId" name="' + this.name + '-forcedWorkflowRequest-wizard-actorId" type="hidden" value="' + this.options.actorId + '" />';
      outString += '<input id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-actorName" name="' + this.name + '-forcedWorkflowRequest-wizard-actorName" type="text" value="' + this.options.actorName + '" readonly="readonly" class="form-control" style="width:200px;" />';
      outString += '</td>';
      outString += '<tr>';
      outString += '<td class="table-body-text" ><span class="required-text">选择节点</span></td>';
      outString += '<td class="table-body-input" ><input id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-targetNodeId" name="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-targetNodeId" value="" feature="combobox" selectedText="请选择节点" data="' + x.ui.wizards.toWorkflowNodesText(options.nodes) + '" class="form-control" style="width:200px;" ></td>';
      outString += '</tr>';
      outString += '<tr>';
      outString += '<td class="table-body-text" ><span class="required-text">' + actionText + '意见</span></td>';
      outString += '<td class="table-body-input" ><textarea id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-idea" name="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" ></textarea></td>';
      outString += '</tr>';
      outString += '<tr>';
      outString += '<td ></td>';
      outString += '<td class="table-body-input" >';
      outString += '<div style="margin:0 0 4px 2px;" >';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-save" href="javascript:void(0);" onclick="' + this.name + '.save(event);" class="btn btn-default" >确定</a></div>';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-gotoAnyNodeWorkflowRequest-wizard-cancel" href="javascript:void(0);" onclick="' + this.name + '.cancel(event);" class="btn btn-default" >取消</a></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '</td>';
      outString += '</tr>';
      outString += '</table>';

      outString += '<div class="clear"></div>';
      outString += '</div>';

      return outString;
    }
    /*#endregion*/
  });

  // 加载界面、数据、事件
  wizard.load();

  // 绑定到Window对象
  window[name] = wizard;
};
/*#endregion*/

/*#region 函数:x.ui.wizards.newForcedWorkflowRequestWizard(name, options)*/
/*
* 紧急审批对话框
*/
x.ui.wizards.newForcedWorkflowRequestWizard = function(name, options)
{
  var wizard = {

    // 实例名称
    name: name,

    // 选项信息
    options: options,

    // 遮罩
    maskWrapper: null,

    /*#region 函数:open()*/
    open: function()
    {
      this.maskWrapper.open();
    },
    /*#endregion*/

    /*#region 函数:close()*/
    close: function()
    {
      this.maskWrapper.close();
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
      this.save_callback();

      this.close();
    },
    /*#endregion*/

    /*#region 函数:save_callback()*/
    /*
    * 默认回调函数，可根据需要自行修改此函数。
    */
    save_callback: function()
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<action><![CDATA[forcedWorkflowRequest]]></action>';
      outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
      outString += '<id><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-id').val() + ']]></id>';
      outString += '<idea><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-idea').val() + ']]></idea>';
      outString += '<workflowInstanceId><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-workflowInstanceId').val() + ']]></workflowInstanceId>';
      outString += '<forcedActorId><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-forcedActorId').val() + ']]></forcedActorId>';
      outString += '</request>';

      x.net.xhr(this.options.url, outString, {
        waitingMessage: i18n.net.waiting.commitTipText,
        popCorrectValue: 1,
        callback: function(response)
        {
          x.page.refreshParentWindow();
          x.page.close();
        }
      });

      return 0;
    },
    /*#endregion*/

    /*#region 函数:cancel()*/
    cancel: function()
    {
      if(typeof (this.cancel_callback) !== 'undefined')
      {
        this.cancel_callback();
      }

      this.close();
    },
    /*#endregion*/

    /*#region 函数:create()*/
    create: function()
    {
      var outString = '';

      outString += '<div id="' + this.name + '-forcedWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a id="' + this.name + '-forcedWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>紧急审批</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

      outString += '<tr>';
      outString += '<td class="table-body-text" style="width:90px" >审批人</td>';
      outString += '<td class="table-body-input" >';
      outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-id" name="' + this.name + '-forcedWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
      outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-workflowInstanceId" name="' + this.name + '-forcedWorkflowRequest-wizard-workflowInstanceId" type="hidden" value="' + this.options.workflowInstanceId + '" />';
      outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorId" name="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorId" type="hidden" />';
      outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorName" name="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorName" type="text" class="form-control" style="width:200px;" />';
      outString += '<div><div id="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorContainer" name="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorContainer" ></div></div>';
      outString += '</td>';
      outString += '<tr>';
      outString += '<td class="table-body-text" >审批意见</td>';
      outString += '<td class="table-body-input" ><textarea id="' + this.name + '-forcedWorkflowRequest-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" ></textarea></td>';
      outString += '</tr>';
      outString += '<tr>';
      outString += '<td ></td>';
      outString += '<td class="table-body-input" >';
      outString += '<div style="margin:0 0 4px 2px;" >';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-forcedWorkflowRequest-wizard-save" href="javascript:' + this.name + '.save();" class="btn btn-default" >确定</a></div>';
      outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-forcedWorkflowRequest-wizard-cancel" href="javascript:' + this.name + '.cancel();" class="btn btn-default" >取消</a></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '</td>';
      outString += '</tr>';
      outString += '</table>';

      outString += '<div class="clear"></div>';
      outString += '</div>';

      return outString;
    },
    /*#endregion*/

    /*#region 函数:load()*/
    load: function()
    {
      // 设置遮罩对象
      if(typeof (options.maskWrapper) === 'undefined')
      {
        this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
      }
      else
      {
        this.maskWrapper = options.maskWrapper;
      }

      // 设置实体标识
      if(typeof (options.entityId) === 'undefined')
      {
        alert('参数【entityId】必须填写。');
        return;
      }

      // 设置实体标识
      if(typeof (options.workflowInstanceId) === 'undefined')
      {
        alert('参数【workflowInstanceId】必须填写。');
        return;
      }

      // 设置实体标识
      if(typeof (options.comboboxUrl) === 'undefined')
      {
        alert('参数【comboboxUrl】必须填写。');
        return;
      }

      // 设置实体标识
      if(typeof (options.comboboxAjaxMethod) === 'undefined')
      {
        alert('参数【comboboxAjaxMethod】必须填写。');
        return;
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

      x.ui.mask.getWindow(this.create(), this.maskWrapper);

      // 设置人员下拉列表
      var comboboxName = this.name + '-forcedWorkflowRequest-wizard-forcedActorId';

      var comboboxContainerName = this.name + '-forcedWorkflowRequest-wizard-forcedActorContainer';
      var comboboxTextInputName = this.name + '-forcedWorkflowRequest-wizard-forcedActorName';
      var comboboxValueInputName = this.name + '-forcedWorkflowRequest-wizard-forcedActorId';

      window[comboboxName] = x.combobox.newCombobox(comboboxName, comboboxContainerName, comboboxTextInputName, comboboxValueInputName, {
        show: 'text',
        topOffset: '-2',
        widthOffset: '1',
        selectedValue: 'selectedValue',
        comboboxType: 'dynamic',
        url: this.options.comboboxUrl,
        whereClause: ' Id IN ( SELECT ActorId FROM tb_Workflow_HistoryNode WHERE WorkflowInstanceId = ##' + this.options.workflowInstanceId + '## AND Status = 1 ) ',
        ajaxMethod: this.options.comboboxAjaxMethod,
        ajaxPreload: '1'
      });

      var inputView = $('#' + comboboxTextInputName);

      if(inputView.width() > 20)
      {
        inputView.css('background-image', 'url("/resources/images/form/combobox_icon.gif")');
        inputView.css('background-repeat', 'no-repeat');
        inputView.css('background-position', (inputView.width() - 20) + 'px 2px');
      }

      $('#' + comboboxTextInputName).bind('focus', function()
      {
        window[comboboxName].open();
      });

      $('#' + comboboxTextInputName).bind('keyup', function()
      {
        window[comboboxName].open();
      });

      $(document).bind('click', function(event)
      {
        var target = window.event ? window.event.srcElement : event ? event.target : null;

        if(target.id != comboboxTextInputName && target.id != comboboxContainerName)
        {
          window[comboboxName].close();
        }
      });
    }
    /*#endregion*/
  };

  return wizard;
};
/*#endregion*/

/*#region 函数:x.ui.wizards.getForcedWorkflowRequestWizard(options)*/
/*
* 快速创建单例
*/
x.ui.wizards.getForcedWorkflowRequestWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-forcedWorkflowRequest-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var wizard = x.ext(x.ui.wizards.newWizard(name, options), {

      /*#region 函数:save_callback()*/
      /*
      * 默认回调函数，可根据需要自行修改此函数。
      */
      save_callback: function()
      {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<action><![CDATA[forcedWorkflowRequest]]></action>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<id><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-id').val() + ']]></id>';
        outString += '<idea><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-idea').val() + ']]></idea>';
        outString += '<workflowInstanceId><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-workflowInstanceId').val() + ']]></workflowInstanceId>';
        outString += '<forcedActorId><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequest-wizard-forcedActorId').val() + ']]></forcedActorId>';
        outString += '</request>';

        x.net.xhr(this.options.url, outString, {
          waitingMessage: i18n.net.waiting.commitTipText,
          // popCorrectValue: 1,
          callback: function(response)
          {
            x.page.refreshParentWindow();
            x.page.close();
          }
        });

        return 0;
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<div id="' + this.name + '-forcedWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a id="' + this.name + '-forcedWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>紧急审批</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:90px" >审批人</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-id" name="' + this.name + '-forcedWorkflowRequest-wizard-id" type="hidden" value="' + this.options.entityId + '" />';
        outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-workflowInstanceId" name="' + this.name + '-forcedWorkflowRequest-wizard-workflowInstanceId" type="hidden" value="' + this.options.workflowInstanceId + '" />';
        outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorId" name="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorId" type="hidden" />';
        outString += '<input id="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorName" name="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorName" type="text" readonly="readonly" class="form-control" style="width:200px;" />';
        outString += '<div><div id="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorContainer" name="' + this.name + '-forcedWorkflowRequest-wizard-forcedActorContainer" ></div></div>';
        outString += '</td>';
        outString += '<tr>';
        outString += '<td class="table-body-text" >审批意见</td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '-forcedWorkflowRequest-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" ></textarea></td>';
        outString += '</tr>';
        outString += '<tr>';
        outString += '<td ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-forcedWorkflowRequest-wizard-save" href="javascript:' + this.name + '.save();" class="btn btn-default" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-forcedWorkflowRequest-wizard-cancel" href="javascript:' + this.name + '.cancel();" class="btn btn-default" >取消</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      },
      /*#endregion*/

      /*#region 函数:bindOptions(options)*/
      bindOptions: function(options)
      {
        x.debug.error('必须重写向导对象的 bindOptions() 方法。');
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置遮罩对象
        if(typeof (options.maskWrapper) === 'undefined')
        {
          this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
        }
        else
        {
          this.maskWrapper = options.maskWrapper;
        }

        // 设置实体标识
        if(typeof (options.entityId) === 'undefined')
        {
          alert('参数【entityId】必须填写。');
          return;
        }

        // 设置实体标识
        if(typeof (options.workflowInstanceId) === 'undefined')
        {
          alert('参数【workflowInstanceId】必须填写。');
          return;
        }

        // 设置实体标识
        if(typeof (options.comboboxUrl) === 'undefined')
        {
          alert('参数【comboboxUrl】必须填写。');
          return;
        }

        // 设置实体标识
        if(typeof (options.comboboxAjaxMethod) === 'undefined')
        {
          alert('参数【comboboxAjaxMethod】必须填写。');
          return;
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

        x.ui.mask.getWindow(this.create(), this.maskWrapper);

        // 设置人员下拉列表
        var comboboxName = this.name + '-forcedWorkflowRequest-wizard-forcedActorId';

        var comboboxContainerName = this.name + '-forcedWorkflowRequest-wizard-forcedActorContainer';
        var comboboxTextInputName = this.name + '-forcedWorkflowRequest-wizard-forcedActorName';
        var comboboxValueInputName = this.name + '-forcedWorkflowRequest-wizard-forcedActorId';

        window[comboboxName] = x.combobox.newCombobox(comboboxName, comboboxContainerName, comboboxTextInputName, comboboxValueInputName, {
          show: 'text',
          topOffset: '-2',
          widthOffset: '1',
          selectedValue: 'selectedValue',
          comboboxType: 'dynamic',
          url: this.options.comboboxUrl,
          whereClause: ' Id IN ( SELECT ActorId FROM tb_Workflow_HistoryNode WHERE WorkflowInstanceId = ##' + this.options.workflowInstanceId + '## AND Status = 1 ) ',
          ajaxMethod: this.options.comboboxAjaxMethod,
          ajaxPreload: '1'
        });

        var inputView = $('#' + comboboxTextInputName);

        if(inputView.width() > 20)
        {
          inputView.css('background-image', 'url("/resources/images/form/combobox_icon.gif")');
          inputView.css('background-repeat', 'no-repeat');
          inputView.css('background-position', (inputView.width() - 20) + 'px 2px');
        }

        $('#' + comboboxTextInputName).bind('focus', function()
        {
          window[comboboxName].open();
        });

        $('#' + comboboxTextInputName).bind('keyup', function()
        {
          window[comboboxName].open();
        });

        $(document).bind('click', function(event)
        {
          var target = window.event ? window.event.srcElement : event ? event.target : null;

          if(target.id != comboboxTextInputName && target.id != comboboxContainerName)
          {
            window[comboboxName].close();
          }
        });
      }
      /*#endregion*/

    });

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

/*#region 函数:x.ui.wizards.getForcedWorkflowRequestFeedbackWizard(options)*/
/*
* 快速创建单例
*/
x.ui.wizards.getForcedWorkflowRequestFeedbackWizard = function(options)
{
  var name = x.getFriendlyName(location.pathname + '-workflow-forcedWorkflowRequestFeedback-wizard');

  if(typeof (window[name]) === 'undefined')
  {
    var wizard = x.ext(x.ui.wizards.newWizard(name, options), {

      /*#region 函数:setActionResult(actionResult)*/
      /*
      * 设置操作结果。
      */
      setActionResult: function(actionResult)
      {
        $('#' + this.name + '-forcedWorkflowRequestFeedback-wizard-actionResult').val(actionResult);

        this.save();
      },
      /*#endregion*/

      /*#region 函数:save_callback()*/
      /*
      * 默认回调函数，可根据需要自行修改此函数。
      */
      save_callback: function()
      {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<action><![CDATA[forcedWorkflowRequest]]></action>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<id><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequestFeedback-wizard-id').val() + ']]></id>';
        outString += '<idea><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequestFeedback-wizard-idea').val() + ']]></idea>';
        outString += '<actionResult><![CDATA[' + $('#' + this.name + '-forcedWorkflowRequestFeedback-wizard-actionResult').val() + ']]></actionResult>';
        outString += '</request>';

        x.net.xhr(this.options.url, outString, {
          waitingMessage: i18n.net.waiting.commitTipText,
          popCorrectValue: 1,
          callback: function(response)
          {
            x.page.refreshParentWindow();
            x.page.close();
          }
        });

        return 0;
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<div id="' + this.name + '-forcedWorkflowRequest-wizard" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a id="' + this.name + '-forcedWorkflowRequest-wizard-close" href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>紧急审批反馈</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%; margin-top:10px;" >';

        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:90px" >反馈人</td>';
        outString += '<td class="table-body-input" >';
        outString += '<input id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-id" name="' + this.name + '-forcedWorkflowRequestFeedback-wizard-id" type="hidden" value="' + this.options.forcedRequestId + '" />';
        outString += '<input id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-forcedActorId" name="' + this.name + '-forcedWorkflowRequestFeedback-wizard-forcedActorId" type="hidden" value="' + this.options.forcedActorId + '" />';
        outString += '<input id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-forcedActorName" name="' + this.name + '-forcedWorkflowRequestFeedback-wizard-forcedActorName" type="text" readonly="readonly" value="' + this.options.forcedActorName + '" class="form-control" style="width:200px;" />';
        outString += '<input id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-actionResult" name="' + this.name + '-forcedWorkflowRequestFeedback-wizard-actionResult" type="hidden" value="1" />';
        outString += '</td>';
        outString += '<tr>';
        outString += '<td class="table-body-text" >反馈意见</td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-idea" name="' + this.name + '-forcedWorkflowRequest-wizard-idea" class="form-control" style="width:260px; height:80px;" ></textarea></td>';
        outString += '</tr>';
        outString += '<tr>';
        outString += '<td ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-yes" href="javascript:' + this.name + '.setActionResult(2);" class="btn btn-default" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a id="' + this.name + '-forcedWorkflowRequestFeedback-wizard-no" href="javascript:' + this.name + '.setActionResult(4);" class="btn btn-default" >不确定</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        return outString;
      },
      /*#endregion*/

      /*#region 函数:bindOptions(options)*/
      bindOptions: function(options)
      {
        x.debug.error('必须重写向导对象的 bindOptions() 方法。');
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置遮罩对象
        if(typeof (options.maskWrapper) === 'undefined')
        {
          this.maskWrapper = x.ui.mask.newMaskWrapper(this.name + '-maskWrapper', { draggableWidth: 418 });
        }
        else
        {
          this.maskWrapper = options.maskWrapper;
        }

        // 设置实体标识
        if(typeof (options.forcedRequestId) === 'undefined')
        {
          alert('参数【forcedRequestId】必须填写。');
          return;
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

        x.ui.mask.getWindow(this.create(), this.maskWrapper);
      }
      /*#endregion*/
    });

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

/*#region 函数:x.ui.wizards.createWorkflowTransactButtons(name, options)*/
/*
* 紧急审批对话框
*/
x.ui.wizards.createWorkflowNodeButtonObjects = function(name, options)
{
  var placeholder = $('#' + name);

  x.net.xhr('/api/workflow.node.createButtonObjects.aspx?workflowNodeId=' + options.workflowNodeId, {
    waitingType: 'mini',
    waitingMessage: '正在查询数据，请稍后......',
    callback: function(response)
    {
      var outString = '';

      var result = x.toJSON(response).data;

      if(result.actorMethod === '会签')
      {
        // 会签
        outString += '<a href="javascript:x.ui.wizards.getReviewWorkflowRequestWizard({'
            + 'url:\'' + options.urls.reviewWorkflowRequestUrl + '\','
            + 'uploadFileWizard:\'' + options.uploadFileWizard + '\','
            + 'workflowRuntimeType:\'' + options.workflowRuntimeType + '\','
            + 'workflowTemplateContainerName:\'' + options.workflowTemplateContainerName + '\','
            + 'entityId:\'' + options.entityId + '\','
            + 'historyNodeId:\'' + options.workflowHistoryNodeId + '\','
            + 'actorName:\'' + result.actorName + '\'});" class="btn btn-default">会签</a> ';
      }
      else
      {
        outString += '<a href="javascript:x.ui.wizards.getTransactWorkflowRequestWizard({'
            + 'url:\'' + options.urls.transactWorkflowRequestUrl + '\','
            + 'uploadFileWizard:\'' + options.uploadFileWizard + '\','
            + 'workflowRuntimeType:\'' + options.workflowRuntimeType + '\','
            + 'workflowTemplateContainerName:\'' + options.workflowTemplateContainerName + '\','
            + 'entityId:\'' + options.entityId + '\','
            + 'historyNodeId:\'' + options.workflowHistoryNodeId + '\','
            + 'actorName:\'' + result.actorName + '\'});" class="btn btn-default">同意</a> ';

        outString += '<a href="javascript:x.ui.wizards.getGotoStartNodeWorkflowRequestWizard({'
            + 'url:\'' + options.urls.gotoStartNodeWorkflowRequestUrl + '\','
            + 'uploadFileWizard:\'' + options.uploadFileWizard + '\','
            + 'workflowRuntimeType:\'' + options.workflowRuntimeType + '\','
            + 'workflowTemplateContainerName:\'' + options.workflowTemplateContainerName + '\','
            + 'entityId:\'' + options.entityId + '\','
            + 'historyNodeId:\'' + options.workflowHistoryNodeId + '\','
            + 'actorName:\'' + result.actorName + '\'});" class="btn btn-default">驳回</a> ';

        // 转交
        if(result.canCommission == '1')
        {
          outString += '<a href="javascript:x.ui.wizards.getCommissionWorkflowRequestWizard({'
              + 'url:\'' + options.urls.commissionWorkflowRequestUrl + '\','
              + 'uploadFileWizard:\'' + options.uploadFileWizard + '\','
              + 'workflowRuntimeType:\'' + options.workflowRuntimeType + '\','
              + 'workflowTemplateContainerName:\'' + options.workflowTemplateContainerName + '\','
              + 'workflowInstanceId:\'' + options.workflowInstanceId + '\','
              + 'entityId:\'' + options.entityId + '\','
              + 'historyNodeId:\'' + options.workflowHistoryNodeId + '\','
              + 'commissionActors:\'' + result.commissionActors + '\','
              + 'actorName:\'' + result.actorName + '\'});" class="btn btn-default">转交</a> ';
        }

        // 后退
        if(result.canBack == '1')
        {
          outString += '<a href="javascript:x.ui.wizards.getGotoAnyNodeWorkflowRequestWizard({'
              + 'url:\'' + options.urls.gotoAnyNodeWorkflowRequestUrl + '\','
              + 'uploadFileWizard:\'' + options.uploadFileWizard + '\','
              + 'workflowRuntimeType:\'' + options.workflowRuntimeType + '\','
              + 'workflowTemplateContainerName:\'' + options.workflowTemplateContainerName + '\','
              + 'entityId:\'' + options.entityId + '\','
              + 'historyNodeId:\'' + options.workflowHistoryNodeId + '\','
              + 'actorName:\'' + result.actorName + '\','
              + 'direction:\'back\','
              + 'nodes:' + x.ui.wizards.toWorkflowNodesText(result.backNodes) + '});" class="btn btn-default">后退</a> ';
        }

        // 前进
        if(result.canForward == '1')
        {
          outString += '<a href="javascript:x.ui.wizards.getGotoAnyNodeWorkflowRequestWizard({'
              + 'url:\'' + options.urls.gotoAnyNodeWorkflowRequestUrl + '\','
              + 'uploadFileWizard:\'' + options.uploadFileWizard + '\','
              + 'workflowRuntimeType:\'' + options.workflowRuntimeType + '\','
              + 'workflowTemplateContainerName:\'' + options.workflowTemplateContainerName + '\','
              + 'entityId:\'' + options.entityId + '\','
              + 'historyNodeId:\'' + options.workflowHistoryNodeId + '\','
              + 'actorName:\'' + result.actorName + '\','
              + 'direction:\'forward\','
              + 'nodes:' + x.ui.wizards.toWorkflowNodesText(result.forwardNodes) + '});" class="btn btn-default">前进</a> ';
        }
      }

      placeholder.before(outString);
      placeholder.remove();
    }
  });
};
/*#endregion*/

/*#region 函数:x.ui.wizards.toWorkflowNodesText(list)*/
x.ui.wizards.toWorkflowNodesText = function(list)
{
  var outString = '[';

  for(var i = 0;i < list.length;i++)
  {
    outString += '{text:\'' + list[i].text + '\',value:\'' + list[i].value + '\'},';
  }

  if(outString.substr(outString.length - 1, 1) === ',')
  {
    outString = outString.substr(0, outString.length - 1);
  }

  outString += ']';

  return outString;
};
/*#endregion*/