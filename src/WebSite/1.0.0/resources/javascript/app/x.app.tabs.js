x.register('x.app.tabs');

/*
* 常用页签管理
*/
x.app.tabs = {

  /*#region 函数:getTabView(targetContainerName, options)*/
  /*
  * 获取页签视图
  */
  getTabView: function(targetContainerName, options, overwrite)
  {
    // 设置容器的名称
    if(typeof (targetContainerName) === 'undefined')
    {
      alert('参数【targetContainerName】必须填写。');
      return;
    }

    // 设置页签类型
    if(typeof (options.type) === 'undefined')
    {
      alert('参数【type】必须填写。');
      return;
    }

    var name = x.getFriendlyName(location.pathname + '$' + targetContainerName + '$' + options.type + '$tab');

    // 初始化常用页签对象
    var tab = null;

    if(typeof (window[name]) === 'undefined' || overwrite === 1)
    {
      switch(options.type)
      {
        case 'workflow-template':
          // 审批模板页签
          tab = x.app.tabs.newWorkflowTemplateTab(name, targetContainerName, options);
          break;

        case 'approval-record':
          // 审批记录页签
          tab = x.app.tabs.newApprovalRecordTab(name, targetContainerName, options);
          break;

        case 'operation-log':
          // 相关记录页签
          tab = x.app.tabs.newOperationLogTab(name, targetContainerName, options);
          break;

        case 'click':
          // 阅读记录页签
          tab = x.app.tabs.newClickTab(name, targetContainerName, options);
          break;

        case 'implementation':
          // 落实情况页签
          tab = x.app.tabs.newImplementationTab(name, targetContainerName, options);
          break;

        case 'document-history':
          // 历史版本页签
          tab = x.app.tabs.newDocumentHistoryTab(name, targetContainerName, options);
          break;

        default:
          alert('未知的页签对象类型【' + options.type + '】。');
          return;
      }

      // 加载界面、数据、事件
      tab.load();
      // 绑定到Window对象
      window[name] = tab;
    }
    else
    {
      tab = window[name];
    }

    tab.refresh();
  },
  /*#endregion*/

  /*#region 函数:newWorkflowTemplateTab(name, targetContainerName, options)*/
  /*
  * 审批模板页签
  */
  newWorkflowTemplateTab: function(name, targetContainerName, options)
  {
    var tab = {

      // 实例名称
      name: name,

      // 选项信息
      options: options,

      // 数据表名称
      customTableName: null,

      // 实体标识
      entityId: null,

      // 实体类型名称
      entityClassName: null,

      // 目标容器名称
      targetContainerName: targetContainerName,

      /*#region 函数:refresh()*/
      refresh: function()
      {
        x.workflow.designtime.getTemplateByWorkflowInstanceId(
            this.options.workflowInstanceId,
            $(document.getElementById(this.targetContainerName)),
            this.options.toolbar,
            this.options.fectchExpectedActors,
            this.options.fectchFinishedActors,
            { corporationId: this.options.corporationId, projectId: this.options.projectId }
         );
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置数据表名称
        if(typeof (options.customTableName) === 'undefined')
        {
          alert('参数【customTableName】必须填写。');
          return;
        }
        else
        {
          this.customTableName = options.customTableName;
        }

        // 设置实体标识
        if(typeof (options.entityId) === 'undefined')
        {
          alert('参数【entityId】必须填写。');
          return;
        }
        else
        {
          this.entityId = options.entityId;
        }

        // 设置实体类型名称
        if(typeof (options.entityClassName) === 'undefined')
        {
          alert('参数【entityClassName】必须填写。');
          return;
        }
        else
        {
          this.entityClassName = options.entityClassName;
        }

        // 设置实体类型名称
        if(typeof (options.workflowInstanceId) === 'undefined')
        {
          alert('参数【workflowInstanceId】必须填写。');
          return;
        }
      }
      /*#endregion*/
    };

    return tab;
  },
  /*#endregion*/

  /*#region 函数:newApprovalRecordTab(name, targetContainerName, options)*/
  /*
  * 审批记录页签
  */
  newApprovalRecordTab: function(name, targetContainerName, options)
  {
    var tab = {

      // 实例名称
      name: name,

      // 选项信息
      options: options,

      // 数据表名称
      customTableName: null,

      // 实体标识
      entityId: null,

      // 实体类型名称
      entityClassName: null,

      // 目标容器名称
      targetContainerName: targetContainerName,

      /*#region 函数:reminder(options)*/
      reminder: function(options)
      {
        x.customForm.workflow.reminder({
          clientTargetObject: this.name,
          customTableName: options.customTableName,
          entityId: options.entityId,
          entityClassName: options.entityClassName,
          workflowInstanceId: options.workflowInstanceId,
          sendUrlFormat: options.sendUrlFormat,
          tags: options.tags
        });
      },
      /*#endregion*/

      /*#region 函数:refresh()*/
      refresh: function()
      {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<id><![CDATA[' + this.options.workflowInstanceId + ']]></id>';
        outString += '</ajaxStorage>';

        x.net.xhr('/api/workflow.instance.getWorkflowOverview.aspx', outString, {
          callback: function(response)
          {
            var result = x.toJSON(response);

            var clientTargetObject = window[result.clientTargetObject];

            var param = result.data;

            // 当前活动节点
            clientTargetObject.workflowActiveNodeName = typeof (result.activeNode) === 'undefined' ? '' : result.activeNode.name;

            clientTargetObject.workflowStatus = param.status;

            // 已审批执行人
            $(document.getElementById(clientTargetObject.name + '-docStatus')).html((clientTargetObject.workflowStatus === '已完成' ? '已发布' : clientTargetObject.workflowStatus));

            // 已审批执行人
            $(document.getElementById(clientTargetObject.name + '-approvedActors')).html(result.approvedActors);

            // 正在审批的执行人
            if(result.approveingActors !== '')
            {
              $(document.getElementById(clientTargetObject.name + '-approveingActors')).html(result.approveingActors);

              var toolbar = '<a style="margin-right:5px" href="javascript:' + clientTargetObject.name + '.reminder({'
                      + 'customTableName:\'' + clientTargetObject.options.customOperationLogTableName + '\','
                      + 'entityId:\'' + clientTargetObject.entityId + '\','
                      + 'entityClassName:\'' + clientTargetObject.entityClassName + '\','
                      + 'workflowInstanceId:\'' + clientTargetObject.options.workflowInstanceId + '\','
                      + 'sendUrlFormat:\'' + clientTargetObject.options.reminderSendUrlFormat + '\','
                      + 'tags:\'' + x.isUndefined(clientTargetObject.options.reminderTags, '') + '\' '
                      + '});" class="btn btn-default btn-xs" ><i class="fa fa-bell"></i> 催办</a>';

              $(document.getElementById(clientTargetObject.name + '-approveingActors-toolbar')).get(0).className = 'table-toolbar';
              $(document.getElementById(clientTargetObject.name + '-approveingActors-toolbar')).css('margin-bottom', '5px');
              $(document.getElementById(clientTargetObject.name + '-approveingActors-toolbar')).html(toolbar);

              outString += '<div id="' + this.name + '-approveingActors-toolbar"></div>';
            }

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<ajaxStorage>';
            outString += '<action><![CDATA[getWorkflowTemplate]]></action>';
            outString += '<clientTargetObject><![CDATA[' + clientTargetObject.name + ']]></clientTargetObject>';
            outString += '<id><![CDATA[' + clientTargetObject.options.workflowInstanceId + ']]></id>';
            outString += '<fectchExpectedActors><![CDATA[1]]></fectchExpectedActors>';
            outString += '<fectchFinishedActors><![CDATA[1]]></fectchFinishedActors>';
            outString += '<corporationId><![CDATA[' + clientTargetObject.options.corporationId + ']]></corporationId>';
            outString += '<projectId><![CDATA[' + clientTargetObject.options.projectId + ']]></projectId>';
            outString += '</ajaxStorage>';

            x.net.xhr('/api/workflow.instance.getTemplateByWorkflowInstanceId.aspx', outString, {
              callback: function(response)
              {
                var result = x.toJSON(response);

                var clientTargetObject = window[result.clientTargetObject];

                var list = result.data.activities;

                var outString = '';

                if(list.length === 0)
                {
                  outString += '<div style="height:30px;"></div>';
                }
                else
                {
                  outString += '<table class="table-style table-full-border-style" style="width:100%;" >';
                  outString += '<tr class="table-row-title" >';
                  outString += '<td style="width:45px">编号</td>';
                  outString += '<td style="width:300px" >审批人</td>';
                  outString += '<td >预计执行人</td>';
                  outString += '<td style="width:80px" >审批方式</td>';
                  outString += '</tr>';

                  x.each(list, function(index, node)
                  {
                    if(node.name === clientTargetObject.workflowActiveNodeName)
                    {
                      outString += '<tr class="table-row-normal" >';
                      outString += '<td ><img alt="当前节点" style="margin:-4px;" src="/resources/images/icon/icon-hand-point.gif"/></td>';
                      outString += '<td style="font-weight:bold; background-color:#f1fde9;" >' + node.actorDescription; +'</td>';
                      outString += '<td style="font-weight:bold; background-color:#f1fde9;" >' + node.expectedActors + '</td>';
                      outString += '<td style="font-weight:bold; background-color:#f1fde9;" >' + node.actorMethod + '</td>';
                      outString += '</tr>';
                    }
                    else
                    {
                      outString += '<tr class="table-row-normal">';
                      outString += '<td>' + ((index + 1) < 9 ? '0' : '') + (index + 1) + '</td>';
                      outString += '<td>' + node.actorDescription; +'</td>';
                      outString += '<td>' + node.expectedActors + '</td>';
                      outString += '<td>' + node.actorMethod + '</td>';
                      outString += '</tr>';
                    }
                  });

                  outString += '</tbody>';
                  outString += '</table>';
                }

                $(document.getElementById(clientTargetObject.name + '-table-workflow-template')).html(outString);
              }
            });
          }
        });
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<table class="table-style table-full-border-style" style="width:100%; margin-bottom:10px;" >';
        outString += '<tr>';
        outString += '<td class="table-body-text" style="width:120px" >文档状态</td>';
        outString += '<td class="table-body-input" ><div id="' + this.name + '-docStatus">已发布</div></td>';
        outString += '</tr>';

        outString += '<tr><td class="table-body-text">已会签/审批</td>';
        outString += '<td class="table-body-input"><div id="' + this.name + '-approvedActors"></div></td>';
        outString += '</tr>';

        outString += '<tr>';
        outString += '<td class="table-body-text"><div style="margin-bottom:5px" >正在会签/审批</div>';
        outString += '<div id="' + this.name + '-approveingActors-toolbar"></div>';
        outString += '</td>';
        outString += '<td class="table-body-input"><span id="' + this.name + '-approveingActors"></span>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div id = "' + this.name + '-table-workflow-template" >';
        outString += '<span class="tooltip-loading-text"><img src="/resources/images/loading.gif" alt="正在加载..." /></span></td>';
        outString += '</div>';

        return outString;
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置数据表名称
        if(typeof (options.customTableName) === 'undefined')
        {
          alert('参数【customTableName】必须填写。');
          return;
        }
        else
        {
          this.customTableName = options.customTableName;
        }

        // 设置实体标识
        if(typeof (options.entityId) === 'undefined')
        {
          alert('参数【entityId】必须填写。');
          return;
        }
        else
        {
          this.entityId = options.entityId;
        }

        // 设置实体类型名称
        if(typeof (options.entityClassName) === 'undefined')
        {
          alert('参数【entityClassName】必须填写。');
          return;
        }
        else
        {
          this.entityClassName = options.entityClassName;
        }

        // 设置实体类型名称
        if(typeof (options.workflowInstanceId) === 'undefined')
        {
          alert('参数【workflowInstanceId】必须填写。');
          return;
        }

        $(document.getElementById(this.targetContainerName)).css('margin', '10px');
        $(document.getElementById(this.targetContainerName)).html(this.create());
      }
      /*#endregion*/
    };

    return tab;
  },
  /*#endregion*/

  /*#region 函数:newOperationLogTab(name, targetContainerName, options)*/
  /*
  * 相关记录页签
  */
  newOperationLogTab: function(name, targetContainerName, options)
  {
    var tab = {

      // 实例名称
      name: name,

      // 选项信息
      options: options,

      // 遮罩
      maskWrapper: null,

      // 数据表名称
      customTableName: null,

      // 实体标识
      entityId: null,

      // 实体类型名称
      entityClassName: null,

      // 目标容器名称
      targetContainerName: targetContainerName,

      // 需要显示的元素
      displayElements: '[审批记录][紧急审批反馈][催办记录][推荐记录]',

      // 协商guid
      discussNewId: '',

      /*#region 函数:getWorkflowResult(node)*/
      getWorkflowResult: function(node)
      {
        if(node.operation.indexOf('【重新提交】') === 0
        || node.operation.indexOf('驳回后重新提交') === 0
        || node.remark.indexOf('【重新提交】') === 0
        || node.remark.indexOf('驳回后重新提交') === 0)
        {
          return '【重新提交】';
        }

        if(node.operation.indexOf('【提交】') === 0
        || node.operation.indexOf('[提交]') === 0
        || node.remark.indexOf('【提交】') === 0
        || node.remark.indexOf('[提交]') === 0)
        {
          return '【提交】';
        }

        if(node.operation.indexOf('【审批】') === 0
        || node.operation.indexOf('[审批]') === 0
        || node.remark.indexOf('【审批】') === 0
        || node.remark.indexOf('【审批】') === 0
        || node.remark.indexOf('[审批]') === 0)
        {
          return '【通过】';
        }

        if(node.operation.indexOf('【驳回】') === 0
        || node.operation.indexOf('[驳回]') === 0
        || node.operation.indexOf('驳回到发起人节点') === 0
        || node.remark.indexOf('【驳回】') === 0
        || node.remark.indexOf('[驳回]') === 0
        || node.remark.indexOf('驳回到发起人节点') === 0)
        {
          return '【驳回】';
        }

        return '';
      },
      /*#endregion*/

      /*#region 函数:openWorkflowHistoryNodeDiscussWindow(workflowHistoryNodeId, toActorId)*/
      /*
      * 打开协商窗口
      */
      openWorkflowHistoryNodeDiscussWindow: function(workflowHistoryNodeId, toActorId)
      {
        // 协商信息的标识
        var id = x.guid.create();

        this.discussNewId = id;

        this.maskWrapper = x.mask.newMaskWrapper(this.name + '$maskWrapper');

        var outString = '';

        outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:400px; height:auto;" >';

        outString += '<div class="winodw-wizard-toolbar" >';
        outString += '<div class="winodw-wizard-toolbar-close">';
        outString += '<a href="javascript:' + this.name + '.closeWorkflowHistoryNodeDiscussWindow();" class="button-text" >关闭</a>';
        outString += '</div>';
        outString += '<div class="float-left">';
        outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>协商</span></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';

        outString += '<table class="table-style" style="width:100%;" >';
        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" style="width:80px;" ><span class="required-text">协商人</span></td>';
        outString += '<td class="table-body-input" >';
        outString += this.options.fromAccountName;
        outString += '<input id="' + this.name + '$discuss$id"  name="' + this.name + '$discuss$id" type="hidden" value="' + id + '" />';
        outString += '<input id="' + this.name + '$discuss$workflowHistoryNodeId"  name="' + this.name + '$discuss$workflowHistoryNodeId" type="hidden" value="' + workflowHistoryNodeId + '" />';
        outString += '<input id="' + this.name + '$discuss$toActorId"  name="' + this.name + '$discuss$toActorId" type="hidden" value="' + toActorId + '" />';
        outString += '</td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" ><span class="required-text">协商意见</span></td>';
        outString += '<td class="table-body-input" ><textarea id="' + this.name + '$discuss$idea" class="textarea-normal" style="width:260px; height:80px;" ></textarea></td>';
        outString += '</tr>';

        if(this.options.uploadFileWizard != '')
        {
          outString += '<tr class="table-row-normal-transparent" >';
          outString += '<td class="table-body-text" >附件</td>';
          outString += '<td class="table-body-input" >';
          outString += '<input id="' + this.name + '$uploadFileWizard" name="' + this.name + '$uploadFileWizard" feature="uploads" uploadUrl="' + this.options.uploadFileWizard + '" uploadEntityId="' + id + '" uploadEntityClassName="Elane.X.WorkflowPlus.Model.WorkflowHistoryNodeDiscussInfo, Elane.X.WorkflowPlus" uploadAttachmentFolder="workflowplus" uploadFileSizeLimit="20" uploadFileTypes="*.*" uploadFileUploadLimit="0" class="input-normal" style="height:18px; width:100%" />';
          outString += '</td>';
          outString += '</tr>';
        }

        outString += '<tr class="table-row-normal-transparent" >';
        outString += '<td class="table-body-text" ></td>';
        outString += '<td class="table-body-input" >';
        outString += '<div style="margin:0 0 4px 2px;" >';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.sendWorkflowHistoryNodeDiscuss();" class="button-text" >确定</a></div>';
        outString += '<div class="float-left button-2font-wrapper" style="margin-right:10px;" ><a href="javascript:' + this.name + '.closeWorkflowHistoryNodeDiscussWindow();" class="button-text" >取消</a></div>';
        outString += '<div class="clear"></div>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '<div class="clear"></div>';
        outString += '</div>';

        // 加载遮罩和页面内容
        x.mask.getWindow(outString, this.maskWrapper);

        x.form.features.bind();
      },
      /*#endregion*/

      /*#region 函数:closeWorkflowHistoryNodeDiscussWindow()*/
      /*
      * 打开协商窗口
      */
      closeWorkflowHistoryNodeDiscussWindow: function()
      {
        this.maskWrapper.close();
      },
      /*#endregion*/

      /*#region 函数:sendWorkflowHistoryNodeDiscuss()*/
      /*
      * 协商
      */
      sendWorkflowHistoryNodeDiscuss: function()
      {
        var idea = $(document.getElementById(this.name + '$discuss$idea')).val();

        if(idea == '')
        {
          alert("【意见】必填。");
          return false;
        }

        this.closeWorkflowHistoryNodeDiscussWindow();

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[send]]></action>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<id><![CDATA[' + $(document.getElementById(this.name + '$discuss$id')).val() + ']]></id>';
        outString += '<entityId><![CDATA[' + this.options.entityId + ']]></entityId>';
        outString += '<entitySubject><![CDATA[' + this.options.entitySubject + ']]></entitySubject>';
        outString += '<entityAccountId><![CDATA[' + this.options.entityAccountId + ']]></entityAccountId>';
        outString += '<idea><![CDATA[' + $(document.getElementById(this.name + '$discuss$idea')).val() + ']]></idea>';
        outString += '<workflowHistoryNodeId><![CDATA[' + $(document.getElementById(this.name + '$discuss$workflowHistoryNodeId')).val() + ']]></workflowHistoryNodeId>';
        outString += '<toActorId><![CDATA[' + $(document.getElementById(this.name + '$discuss$toActorId')).val() + ']]></toActorId>';
        outString += '<applicationName><![CDATA[' + this.options.applicationName + ']]></applicationName>';
        outString += '<reminderTags><![CDATA[' + this.options.reminderTags + ']]></reminderTags>';
        outString += '<entityViewUrlFormat><![CDATA[' + this.options.entityViewUrlFormat + ']]></entityViewUrlFormat>';
        outString += '</ajaxStorage>';

        x.net.xhr('/api/workflow.historyNodeDiscuss.send.aspx', outString, {
          waitingMessage: '正在提交协商数据，请稍候……',
          callback: function(response)
          {
            window[x.toJSON(response).clientTargetObject].refresh();
          }
        });
      },
      /*#endregion*/

      /*#region 函数:createWorkflowHistoryNodeHtml()*/
      /*
      * 协商
      */
      createWorkflowHistoryNodeHtml: function(node, isDiscuss)
      {
        var outString = '';

        outString += '<tr class="table-row-normal" >';

        if(isDiscuss && this.options.entityAccountId == this.options.fromAccountId && this.options.fromAccountId != node.actorId)
        {
          outString += '<td style="vertical-align: top;" >';
          outString += '<a href="javascript:' + this.name + '.openWorkflowHistoryNodeDiscussWindow(\'' + node.id + '\',\'' + node.actorId + '\');" ';
          outString += 'title="与【' + node.actorName + '】协商" >' + node.actorName + '</a>';
          outString += '</td>';
        }
        else
        {
          if(node.isGranted === 'True')
          {
            outString += '<td style="vertical-align: top;" >' + node.granteeName + '</td>';
          }
          else
          {
            outString += '<td style="vertical-align: top;" >' + node.actorName + '</td>';
          }
        }

        var discuss = x.toJSON(node.discuss);

        var discussAttachmentFiles = x.toJSON(node.discussAttachmentFiles);

        if(typeof (discuss) === 'undefined' || discuss.length === 0)
        {
          outString += '<td>' + this.getWorkflowResult(node) + node.idea;
          if(node.isGranted === 'True') { outString += ' -【委托人:' + node.grantorName + '】'; }
          if(node.rating > 0) { outString += ' - <span class="rating-text" >评分:' + node.rating + '</span> '; }

          if(node.attachmentFiles != undefined)
          {
            var nodeAttachmentFiles = x.toJSON(node.attachmentFiles);

            outString += '<div style="margin-top:4px;" >';

            for(var k1 = 0;k1 < nodeAttachmentFiles.length;k1++)
            {
              outString += '<img style="margin-right:2px;vertical-align: middle;" src="/resources/images/icon/' + x.attachment.getfileExtImg(nodeAttachmentFiles[k1].fileType) + '"/>';
              outString += '<a style="vertical-align: bottom;text-decoration:none" href="/attachment/archive/' + nodeAttachmentFiles[k1].id + '.aspx" >' + nodeAttachmentFiles[k1].attachmentName + '</a>';
              outString += '<span style="vertical-align: middle;margin-left:1px;text-decoration:none">(' + x.expression.formatNumberRound2(nodeAttachmentFiles[k1].fileSize / 1024) + 'KB)</span> ';
            }

            outString += '</div>'
          }

          outString += '</td>';

        }
        else
        {
          outString += '<td style="padding:0px;" >';
          outString += '<table style="width:100%; border:0px #fff solid;" >';
          outString += '<tr><td style="border:none;" >' + this.getWorkflowResult(node) + node.idea;
          if(node.isGranted === 'True') { outString += ' -【委托人:' + node.grantorName + '】'; }
          if(node.rating > 0) { outString += ' - <span class="rating-text" >评分:' + node.rating + '</span> '; }

          if(node.attachmentFiles != undefined)
          {
            var nodeAttachmentFiles = x.toJSON(node.attachmentFiles);

            outString += '<div style="margin-top:4px;" >';

            for(var k1 = 0;k1 < nodeAttachmentFiles.length;k1++)
            {
              outString += '<img style="margin-right:2px;vertical-align: middle;" src="/resources/images/icon/' + x.attachment.getfileExtImg(nodeAttachmentFiles[k1].fileType) + '"/>';
              outString += '<a style="vertical-align: middle;" href="/attachment/archive/' + nodeAttachmentFiles[k1].id + '.aspx" >' + nodeAttachmentFiles[k1].attachmentName + '</a>';
              outString += '<span style="vertical-align: middle;margin-left:1px;text-decoration:none">(' + x.expression.formatNumberRound2(nodeAttachmentFiles[k1].fileSize / 1024) + 'KB)</span> ';
            }

            outString += '</div>'
          }
          outString += '</td></tr>';

          outString += '<tr><td style="border:none; border-top:1px #ccc solid; font-weight:bold;">协商记录</td></tr>';
          for(var i = 0;i < discuss.length;i++)
          {
            outString += '<tr>'
            outString += '<td style="border:none;" >';
            outString += '<div>';
            if(isDiscuss && this.options.entityAccountId == this.options.fromAccountId && this.options.fromAccountId != discuss[i].fromActorId)
            {
              outString += '<a href="javascript:' + clientTargetObject.name + '.openWorkflowHistoryNodeDiscussWindow(\'' + node.id + '\',\'' + discuss[i].fromActorId + '\');" ';
              outString += 'title="与【' + discuss[i].fromActorName + '】协商" >' + discuss[i].fromActorName + '</a> ';
            }
            else
            {
              outString += discuss[i].fromActorName + ' ';
            }
            outString += x.date.newTime(discuss[i].createDate).toString('yyyy-MM-dd HH:mm:ss');
            outString += '</div>';

            outString += '<div style="margin-top:4px;" >' + discuss[i].idea + '</div>';

            var isExistAttachmentFile;

            for(var k = 0;k < discussAttachmentFiles.length;k++)
            {
              if(discussAttachmentFiles[k].entityId == discuss[i].id)
              {
                isExistAttachmentFile = true;
                break;
              }
            }

            if(isExistAttachmentFile)
            {
              outString += '<div style="margin-top:4px;" >';

              for(var k = 0;k < discussAttachmentFiles.length;k++)
              {
                if(discussAttachmentFiles[k].entityId == discuss[i].id)
                {
                  outString += '<img style="margin-right:2px;vertical-align: middle;" src="/resources/images/icon/' + x.attachment.getfileExtImg(discussAttachmentFiles[k].fileType) + '"/>';
                  outString += '<a style="vertical-align: middle;" href="/attachment/archive/' + discussAttachmentFiles[k].id + '.aspx" >' + discussAttachmentFiles[k].attachmentName + '</a>';
                  outString += '<span style="vertical-align: middle;margin-left:1px;text-decoration:none">(' + x.expression.formatNumberRound2(discussAttachmentFiles[k].fileSize / 1024) + 'KB)</span> ';
                }
              }

              outString += '</div>';
            }

            outString += '</td>';
            outString += '</tr>';
          }
          outString += '</table>';
          outString += '</td>';
        }

        outString += '<td style="vertical-align: top;" >' + x.date.newTime(node.finishedTime).toString('yyyy-MM-dd HH:mm:ss') + '</td>';

        outString += '</tr>';

        return outString;
      },
      /*#endregion*/

      /*#region 函数:refresh()*/
      refresh: function()
      {
        var url = '';

        var outString = '';

        // 获取会签记录
        if(this.displayElements.indexOf('[会签和审批记录]') > -1)
        {
          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<accountId><![CDATA[' + this.options.entityAccountId + ']]></accountId>';
          outString += '<workflowInstanceId><![CDATA[' + this.options.workflowInstanceId + ']]></workflowInstanceId>';
          outString += '<isFinishedDiscuss><![CDATA[' + this.options.isFinishedDiscuss + ']]></isFinishedDiscuss>';
          outString += '</ajaxStorage>';

          x.net.xhr('/api/workflow.historyNode.findFinishedHistoryNodesWithDiscuss.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              var list = result.ajaxStorage;

              var isDiscuss = clientTargetObject.options.isFinishedDiscuss ? true : result.workflowInstance.isFinished == "1" ? false : true;

              if(list.length > 0)
              {
                var outString = '';

                outString += '<table class="table-style table-full-border-style-table" style="width: 100%;" >'
                outString += '<tbody>';
                outString += '<tr class="table-row-title" >';
                outString += '<th style="width: 80px;">会签人</th>';
                outString += '<th >会签意见</th>';
                outString += '<th style="width: 160px;">会签时间</th>';
                outString += '</tr>';

                x.each(list, function(index, node)
                {
                  if(node.actorMethod === '会签')
                  {
                    outString += clientTargetObject.createWorkflowHistoryNodeHtml(node, isDiscuss);
                  }
                });

                outString += '</tbody>';
                outString += '</table>';

                $(document.getElementById(clientTargetObject.name + '-table-discuss')).css('margin-bottom', '4px');
                $(document.getElementById(clientTargetObject.name + '-table-discuss'))[0].innerHTML = outString;

                // 显示审批意见
                outString = '';

                outString += '<table class="table-style table-full-border-style-table" style="width: 100%;" >'
                outString += '<tbody>';
                outString += '<tr class="table-row-title" >';
                outString += '<th style="width: 80px;">审批人</th>';
                outString += '<th >审批意见</th>';
                outString += '<th style="width: 160px;">审批时间</th>';
                outString += '</tr>';

                x.each(list, function(index, node)
                {
                  // if (node.actorMethod !== '提交' && node.actorMethod !== '会签')
                  if(node.actorMethod !== '会签')
                  {
                    outString += clientTargetObject.createWorkflowHistoryNodeHtml(node, isDiscuss);
                  }
                });

                outString += '</tbody>';
                outString += '</table>';

                $(document.getElementById(clientTargetObject.name + '-table-workflow')).css('margin-bottom', '4px');
                $(document.getElementById(clientTargetObject.name + '-table-workflow'))[0].innerHTML = outString;;
              }
            }
          });
        }

        // 获取审批记录(必须有)
        if(this.displayElements.indexOf('[审批记录]') > -1)
        {
          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<workflowInstanceId><![CDATA[' + this.options.workflowInstanceId + ']]></workflowInstanceId>';
          outString += '</ajaxStorage>';

          x.net.xhr('/api/workflow.historyNode.findFinishedHistoryNodes.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              var list = result.data;

              var outString = '';

              if(list.length > 0)
              {
                outString += '<table class="table-style table-full-border-style-table" style="width: 100%;" >'

                outString += '<tbody>';
                outString += '<tr class="table-row-title" >';
                outString += '<th style="width: 80px;">审批人</th>';
                outString += '<th >审批意见</th>';
                outString += '<th style="width: 160px;">审批时间</th>';
                outString += '</tr>';

                x.each(list, function(index, node)
                {
                  outString += clientTargetObject.createWorkflowHistoryNodeHtml(node, false);
                });

                outString += '</tbody>';
                outString += '</table>';
              }

              $('#' + clientTargetObject.name + '-table-workflow').css('margin-bottom', '4px');
              $('#' + clientTargetObject.name + '-table-workflow').html(outString);
            }
          });
        }

        // 获取紧急审批反馈
        if(this.displayElements.indexOf('[紧急审批反馈]') > -1)
        {
          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<workflowInstanceId><![CDATA[' + this.options.workflowInstanceId + ']]></workflowInstanceId>';
          outString += '</ajaxStorage>';

          x.net.xhr('/api/workflow.historyNode.findAllForcedRequest.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              var list = result.ajaxStorage;

              var outString = '';

              if(list.length > 0)
              {
                outString += '<table class="table-style table-full-border-style-table" style="width: 100%;" >'

                outString += '<tbody>';
                outString += '<tr class="table-row-title" >';
                outString += '<th style="width: 80px;">节点名称</th>';
                outString += '<th style="width: 80px;">反馈人</th>';
                outString += '<th >反馈内容</th>';
                outString += '<th style="width: 80px;">确认情况</th>';
                outString += '<th style="width: 120px;">反馈时间</th>';
                outString += '</tr>';

                x.each(list, function(index, node)
                {
                  outString += '<tr class="table-row-normal" >';
                  outString += '<td>' + node.workflowNodeName + '</td>';
                  outString += '<td>' + node.actorName + '</td>';
                  outString += '<td>' + node.idea + '</td>';
                  outString += '<td>' + (node.actionResult == 1 ? '等待确认' : (node.actionResult == 2 ? '<span class="green-text">确认</span>' : '<span class="red-text">不确认</span>')) + '</td>';
                  outString += '<td>' + x.date.create(node.updateDate).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
                  outString += '</tr>';
                });

                outString += '</tbody>';
                outString += '</table>';
              }

              $('#' + clientTargetObject.name + '-table-forcedRequest').css('margin-bottom', '4px');
              $('#' + clientTargetObject.name + '-table-forcedRequest').html(outString);
            }
          });
        }

        // 获取催办记录
        if(this.displayElements.indexOf('[催办记录]') > -1)
        {
          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<customTableName><![CDATA[' + this.customTableName + ']]></customTableName>';
          outString += '<entityId><![CDATA[' + this.entityId + ']]></entityId>';
          outString += '<entityClassName><![CDATA[' + this.entityClassName + ']]></entityClassName>';
          outString += '<operationType><![CDATA[1]]></operationType>';
          outString += '</ajaxStorage>';

          x.net.xhr('/api/kernel.entities.operationLog.findAllByEntityId.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              var list = result.data;

              var outString = '';

              if(list.length > 0)
              {
                outString += '<table style="width: 100%;" class="table-style table-full-border-style-table">';

                outString += '<tbody>';
                outString += '<tr class="table-row-title">';
                outString += '<th style="width: 80px;">催办人</th>';
                outString += '<th>催办情况</th>';
                outString += '<th style="width: 120px;">催办时间</th>';
                outString += '</tr>';

                x.each(list, function(index, node)
                {
                  outString += '<tr class="table-row-normal" >';
                  outString += '<td>' + node.accountName + '</td>';
                  outString += '<td>向【' + node.toAuthorizationObjectName + '】发送了一条催办信息。</td>';
                  outString += '<td>' + x.date.newTime(node.createDate).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
                  outString += '</tr>';
                });

                outString += '</tbody>';
                outString += '</table>';

                $('#' + clientTargetObject.name + '-table-reminder').css('margin-bottom', '4px');
                $('#' + clientTargetObject.name + '-table-reminder').html(outString);
              }
            }
          });
        }

        // 获取推荐记录
        if(this.displayElements.indexOf('[推荐记录]') > -1)
        {
          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          // outString += '<action><![CDATA[findAllByEntityId]]></action>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<customTableName><![CDATA[' + this.customTableName + ']]></customTableName>';
          outString += '<entityId><![CDATA[' + this.entityId + ']]></entityId>';
          outString += '<entityClassName><![CDATA[' + this.entityClassName + ']]></entityClassName>';
          outString += '<operationType><![CDATA[2]]></operationType>';
          outString += '</ajaxStorage>';

          x.net.xhr('/api/kernel.entities.operationLog.findAllByEntityId.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              var list = result.data;

              var outString = '';

              if(list.length > 0)
              {
                outString += '<table style="width: 100%;" class="table-style table-full-border-style-table">';

                outString += '<tbody>';
                outString += '<tr class="table-row-title">';
                outString += '<th style="width: 80px;">推荐人</th>';
                outString += '<th>推荐情况</th>';
                outString += '<th style="width: 120px;">推荐时间</th>';
                outString += '</tr>';

                x.each(list, function(index, node)
                {
                  outString += '<tr class="table-row-normal" >';
                  outString += '<td>' + node.accountName + '</td>';
                  if(node.reason == '')
                  {
                    outString += '<td>推荐给【' + node.toAuthorizationObjectName + '】。</td>';
                  }
                  else
                  {
                    outString += '<td>推荐给【' + node.toAuthorizationObjectName + '】，推荐原因：' + node.reason + '。</td>';
                  }
                  outString += '<td>' + x.date.newTime(node.createDate).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
                  outString += '</tr>';
                });

                outString += '</tbody>';
                outString += '</table>';
              }

              $('#' + clientTargetObject.name + '-table-recommend').css('margin-bottom', '4px');
              $('#' + clientTargetObject.name + '-table-recommend').html(outString);
            }
          });
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<table id="' + this.name + '$table" style="width: 100%;" class="table-style table-full-border-style">';

        outString += '<tbody>';

        // 会签和审批记录
        if(this.displayElements.indexOf('[会签和审批记录]') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text" style="width:120px;" >会签记录</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-discuss"></div></td>';
          outString += '</tr>';
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text" style="width:120px;" >审批记录</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-workflow"></div></td>';
          outString += '</tr>';
        }

        // 审批记录
        if(this.displayElements.indexOf('[审批记录]') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text" style="width:120px;" >审批记录</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-workflow"></div></td>';
          outString += '</tr>';
        }

        // 紧急审批反馈
        if(this.displayElements.indexOf('[紧急审批反馈]') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text">紧急审批反馈</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-forcedRequest"></div></td>';
          outString += '</tr>';
        }

        // 催办记录
        if(this.displayElements.indexOf('[催办记录]') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text">催办记录</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-reminder"></div></td>';
          outString += '</tr>';
        }

        // 未阅读人员
        if(this.displayElements.indexOf('[推荐记录]') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text" style="width:120px;" >推荐记录</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-recommend"></div></td>';
          outString += '</tr>';
        }

        outString += '</tbody>';
        outString += '</table>';

        return outString;
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置数据表名称
        if(typeof (options.customTableName) === 'undefined')
        {
          alert('参数【customTableName】必须填写。');
          return;
        }
        else
        {
          this.customTableName = options.customTableName;
        }

        // 设置实体标识
        if(typeof (options.entityId) === 'undefined')
        {
          alert('参数【entityId】必须填写。');
          return;
        }
        else
        {
          this.entityId = options.entityId;
        }

        // 设置实体类型名称
        if(typeof (options.entityClassName) === 'undefined')
        {
          alert('参数【entityClassName】必须填写。');
          return;
        }
        else
        {
          this.entityClassName = options.entityClassName;
        }

        // 设置需要显示的元素
        if(typeof (options.displayElements) !== 'undefined')
        {
          this.displayElements = options.displayElements;
        }

        $(document.getElementById(this.targetContainerName)).css('margin', '10px');
        $(document.getElementById(this.targetContainerName)).html(this.create());
      }
      /*#endregion*/
    }

    return tab;
  },
  /*#endregion*/

  /*#region 函数:newClickTab(name, targetContainerName, options)*/
  /*
  * 阅读记录页签
  */
  newClickTab: function(name, targetContainerName, options)
  {
    var tab = {

      // 实例名称
      name: name,

      // 选项信息
      options: options,

      // 数据表名称
      customTableName: null,

      // 实体标识
      entityId: null,

      // 实体类型名称
      entityClassName: null,

      // 目标容器名称
      targetContainerName: targetContainerName,

      // 需要显示的元素
      displayElements: '[可阅读人员][已阅读人员][未阅读人员]',

      /*#region 函数:refresh()*/
      refresh: function()
      {
        var url = '';

        var outString = '';

        if(this.displayElements.indexOf('可阅读人员') > -1)
        {
          var scopeTableName = '';

          if(typeof (options.entityScopeTableName) !== 'undefined')
          {
            scopeTableName = this.options.entityScopeTableName;
          }
          else if(typeof (options.entityTableName) !== 'undefined')
          {
            if(this.options.entityTableName.substr(this.options.entityTableName.length - 1, 1) === ']')
            {
              scopeTableName = this.options.entityTableName.substr(0, this.options.entityTableName.length - 1) + '_Scope]';
            }
            else
            {
              scopeTableName = this.options.entityTableName + '_Scope';
            }
          }

          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<scopeTableName><![CDATA[' + scopeTableName + ']]></scopeTableName>';
          outString += '<entityId><![CDATA[' + this.entityId + ']]></entityId>';
          outString += '<entityClassName><![CDATA[' + this.entityClassName + ']]></entityClassName>';
          outString += '<authorityName><![CDATA[应用_通用_查看权限]]></authorityName>';
          outString += '</ajaxStorage>';

          x.net.xhr('/api/kernel.entities.scope.getAuthorizationScopeObjectView.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              $(document.getElementById(clientTargetObject.name + '-table-canread')).html(result.ajaxStorage);
            }
          });
        }

        // 已阅读人员
        if(this.displayElements.indexOf('已阅读人员') > -1)
        {
          outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<ajaxStorage>';
          outString += '<action><![CDATA[findAllByEntityId]]></action>';
          outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
          outString += '<customTableName><![CDATA[' + this.customTableName + ']]></customTableName>';
          outString += '<entityId><![CDATA[' + this.entityId + ']]></entityId>';
          outString += '<entityClassName><![CDATA[' + this.entityClassName + ']]></entityClassName>';
          if(this.options.resultMap)
          {
            var resultMap = this.options.resultMap;
            outString += '<resultMap>';
            for(resultProperty in resultMap)
            {
              outString += '<result property="' + resultProperty + '" column="' + resultMap[resultProperty] + '" />';
            }
            outString += '</resultMap>';
          }
          outString += '</ajaxStorage>';

          x.net.xhr('/api/kernel.entities.click.findAllByEntityId.aspx', outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var clientTargetObject = window[result.clientTargetObject];

              var list = result.data;

              var outString = '';

              x.each(list, function(index, node)
              {
                outString += '<span title="' + node.accountName + '已阅读' + node.click + '次，最后阅读时间' + x.date.newTime(node.updateDate).toString('yyyy-MM-dd HH:mm:ss') + '。" >' + node.accountName + '</span>;';
              });

              $(document.getElementById(clientTargetObject.name + '-table-haveread')).html(outString);
            }
          });
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      create: function()
      {
        var outString = '';

        outString += '<table id="' + this.name + '$table" style="width: 100%;" class="table-style table-full-border-style">';

        outString += '<tbody>';

        // 可阅读人员
        if(this.displayElements.indexOf('可阅读人员') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text" style="width:120px;" >可阅读人员</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-canread"></div></td>';
        }

        // 已阅读人员
        if(this.displayElements.indexOf('已阅读人员') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text">已阅读人员</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-haveread"></div></td>';
          outString += '</tr>';
        }

        // 未阅读人员
        if(this.displayElements.indexOf('未阅读人员') > -1)
        {
          outString += '<tr class="table-row-normal">';
          outString += '<td class="table-body-text" style="width:120px;" >未阅读人员</td>';
          outString += '<td class="table-body-input" ><div id="' + this.name + '-table-unread"></div></td>';
        }

        outString += '</tbody>';
        outString += '</table>';

        return outString;
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置数据表名称
        if(typeof (options.customTableName) === 'undefined')
        {
          alert('参数【customTableName】必须填写。');
          return;
        }
        else
        {
          this.customTableName = options.customTableName;
        }

        // 设置实体标识
        if(typeof (options.entityId) === 'undefined')
        {
          alert('参数【entityId】必须填写。');
          return;
        }
        else
        {
          this.entityId = options.entityId;
        }

        // 设置实体类型名称
        if(typeof (options.entityClassName) === 'undefined')
        {
          alert('参数【entityClassName】必须填写。');
          return;
        }
        else
        {
          this.entityClassName = options.entityClassName;
        }

        // 设置需要显示的元素
        if(typeof (options.displayElements) !== 'undefined')
        {
          this.displayElements = options.displayElements;
        }

        $(document.getElementById(this.targetContainerName)).css('margin', '10px');
        $(document.getElementById(this.targetContainerName)).html(this.create());
      }
      /*#endregion*/
    }

    return tab;
  },
  /*#endregion*/

  /*#region 函数:newImplementationTab(name, targetContainerName, options)*/
  /*
  * 落实情况页签
  */
  newImplementationTab: function(name, targetContainerName, options)
  {
    var tab = {

      // 实例名称
      name: name,

      // 选项信息
      options: options,

      // 数据表名称
      customTableName: null,

      // 实体标识
      entityId: null,

      // 实体类型名称
      entityClassName: null,

      // 目标容器名称
      targetContainerName: targetContainerName,

      /*#region 函数:refresh()*/
      refresh: function()
      {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<customTableName><![CDATA[' + this.customTableName + ']]></customTableName>';
        outString += '<entityId><![CDATA[' + this.entityId + ']]></entityId>';
        outString += '<entityClassName><![CDATA[' + this.entityClassName + ']]></entityClassName>';
        outString += '</ajaxStorage>';

        x.net.xhr('/api/kernel.entities.implementation.findAllByEntityId.aspx', outString, {
          callback: function(response)
          {
            var result = x.toJSON(response);

            var clientTargetObject = window[result.clientTargetObject];

            var list = result.ajaxStorage;

            var outString = '';

            if(list.length === 0)
            {
              outString += '<div style="height:30px;"></div>';
            }
            else
            {
              outString += '<table style="width: 100%;" class="table-style table-full-border-style-table">';

              outString += '<tbody>';
              outString += '<tr class="table-row-title" >';
              outString += '<th style="width:80px;" >提交人</th>';
              outString += '<th style="width:120px;" >完成情况</th>';
              outString += '<th>详细情况描述</th>';
              outString += '<th style="width:120px;" >提交时间</th>';
              outString += '</tr>';

              x.each(list, function(index, node)
              {
                var classNameValue = "table-row-normal" + ((index + 1) === list.length ? '-transparent' : '');

                outString += '<tr class="' + classNameValue + '">';
                outString += '<td>' + node.accountName + '</td>';
                outString += '<td>' + node.completionStatus + '</td>';
                outString += '<td>' + node.workContent + '</td>';
                outString += '<td>' + x.date.newTime(node.createDate).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
                outString += '</tr>';
              });

              outString += '</tbody>';
              outString += '</table>';
            }

            $('#' + clientTargetObject.name).html(outString);
          }
        });
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置数据表名称
        if(typeof (options.customTableName) === 'undefined')
        {
          alert('参数【customTableName】必须填写。');
          return;
        }
        else
        {
          this.customTableName = options.customTableName;
        }

        // 设置实体标识
        if(typeof (options.entityId) === 'undefined')
        {
          alert('参数【entityId】必须填写。');
          return;
        }
        else
        {
          this.entityId = options.entityId;
        }

        // 设置实体类型名称
        if(typeof (options.entityClassName) === 'undefined')
        {
          alert('参数【entityClassName】必须填写。');
          return;
        }
        else
        {
          this.entityClassName = options.entityClassName;
        }
      }
      /*#endregion*/
    }

    return tab;
  },
  /*#endregion*/

  /*#region 函数:newDocumentHistoryTab(name, targetContainerName, options)*/
  /*
  * 历史版本页签
  */
  newDocumentHistoryTab: function(name, targetContainerName, options)
  {
    var tab = {

      // 实例名称
      name: name,

      // 选项信息
      options: options,

      // 数据表名称
      customTableName: null,

      // 实体标识
      entityId: null,

      // 实体类型名称
      entityClassName: null,

      // 目标容器名称
      targetContainerName: targetContainerName,

      // 文档标记
      docToken: null,

      // 文档查看页面地址规则
      docUrlFormat: null,

      /*#region 函数:refresh()*/
      refresh: function()
      {
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<clientTargetObject><![CDATA[' + this.name + ']]></clientTargetObject>';
        outString += '<customTableName><![CDATA[' + this.customTableName + ']]></customTableName>';
        outString += '<docToken><![CDATA[' + this.docToken + ']]></docToken>';
        if(this.options.resultMap)
        {
          var resultMap = this.options.resultMap;
          outString += '<resultMap>';
          for(resultProperty in resultMap)
          {
            outString += '<result property="' + resultProperty + '" column="' + resultMap[resultProperty] + '" />';
          }
          outString += '</resultMap>';
        }
        outString += '</ajaxStorage>';

        x.net.xhr('/api/kernel.entities.docObject.findAllByDocToken.aspx', outString, {
          callback: function(response)
          {
            var result = x.toJSON(response);

            var clientTargetObject = window[result.clientTargetObject];

            var list = result.ajaxStorage;

            var outString = '';

            if(list.length === 0)
            {
              outString += '<div style="height:30px;">暂时还没有历史记录。</div>';
            }
            else
            {
              outString += '<table style="width: 100%;" class="table-style table-full-border-style-table" >';

              outString += '<tbody>';
              outString += '<tr class="table-row-title" >';
              outString += '<th>版本</th>';
              outString += '</tr>';

              x.each(list, function(index, node)
              {
                var classNameValue = "table-row-normal" + ((index + 1) === list.length ? '-transparent' : '');

                outString += '<tr class="' + classNameValue + '" >';
                outString += '<td><a href="' + clientTargetObject.docUrlFormat.replace('{0}', node.id) + '" target="_blank" >' + node.docTitle + '</a> <strong>' + node.docVersion + '版</strong>【' + node.accountName + '】【' + x.date.newTime(node.createDate).toString('yyyy-MM-dd HH:mm:ss') + '】</td>';
                outString += '</tr>';
              });

              outString += '</tbody>';
              outString += '</table>';
            }

            $(document.getElementById(clientTargetObject.targetContainerName)).html(outString);
          }
        });
      },
      /*#endregion*/

      /*#region 函数:load()*/
      load: function()
      {
        // 设置数据表名称
        if(typeof (options.customTableName) === 'undefined')
        {
          alert('参数【customTableName】必须填写。');
          return;
        }
        else
        {
          this.customTableName = options.customTableName;
        }

        // 设置实体标识
        if(typeof (options.docToken) === 'undefined')
        {
          alert('参数【docToken】必须填写。');
          return;
        }
        else
        {
          this.docToken = options.docToken;
        }

        // 设置实体查看页面
        if(typeof (options.docUrlFormat) === 'undefined')
        {
          alert('参数【docUrlFormat】必须填写。');
          return;
        }
        else
        {
          this.docUrlFormat = options.docUrlFormat;
        }
      }
      /*#endregion*/
    }

    return tab;
  }
  /*#endregion*/
};