// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :x.workflow.js
//
// Description  :
//
// Author       :Max
//
// Date         :2010-01-01
//
// =============================================================================

/**
* 工作流类
*
*/
x.workflow = {

  // 设置
  settings: {
    // 工作流节点标识
    // workflowNodeActorMethods: '[{text:\'会审(需要所有人审批)\',value:\'会审\'},{text:\'并审(只需其中一个人审)\',value:\'并审\'},{text:\'抄送\',value:\'抄送\'}]',
    workflowNodeActorMethods: '[{text:\'会签\',value:\'会签\'},{text:\'校稿\',value:\'校稿\'},{text:\'审核(需要所有人同意)\',value:\'审核\'},{text:\'并审(需要其中一人同意)\',value:\'并审\'},{text:\'审批(审核+批准)\',value:\'审批\'},{text:\'批准\',value:\'批准\'},{text:\'抄送\',value:\'抄送\'}]',
    // 工作流节点标识
    workflowNodeSelector: '.workflow-node',
    // 工作流节点新增标识
    workflowNodeNewSelector: '.workflow-node-new',
    // 工作流分支条件标识
    workflowSwitcherExitSelector: '.workflow-switcher-exit',
    // 工作流分支条件新增标识
    workflowSwitcherExitNewSelector: '.workflow-switcher-exit-new',
    // 工作流分支条件标识
    workflowSwitcherExitConditionSelector: '.workflow-switcher-exit-condition',
    // 工作流分支条件新增标识
    workflowSwitcherExitConditionNewSelector: '.workflow-switcher-exit-condition-new'
  },

  /*#region 函数:reminder(options)*/
  /**
   * 发送催办信息
   */
  reminder: function(options)
  {
    if(confirm('确定发送催办消息给审批人?'))
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';
      outString += '<request>';
      if(typeof (options.clientTargetObject) !== 'undefined')
      {
        outString += '<clientTargetObject><![CDATA[' + options.clientTargetObject + ']]></clientTargetObject>';
      }
      outString += '<customTableName><![CDATA[' + options.customTableName + ']]></customTableName>';
      outString += '<entityId><![CDATA[' + options.entityId + ']]></entityId>';
      outString += '<entityClassName><![CDATA[' + options.entityClassName + ']]></entityClassName>';
      outString += '<workflowInstanceId><![CDATA[' + options.workflowInstanceId + ']]></workflowInstanceId>';
      outString += '<sendUrlFormat><![CDATA[' + options.sendUrlFormat + ']]></sendUrlFormat>';
      outString += '<tags><![CDATA[' + options.tags + ']]></tags>';
      outString += '</request>';

      x.net.xhr('/api/kernel.entities.operationLog.reminder.aspx', outString, { popResultValue: 1 });
    }
  },
  /*#endregion*/

  /**
  * 处理工作流请求
  */
  workflowRequest: {

    /*#region 函数:start(templateId, entityId, entityClassName, options)*/
    /*
    * 启动流程
    *
    * templateId:模板标识
    */
    start: function(templateId, entityId, entityClassName, options)
    {
      x.workflow.workflowRequest.transact({
        url: '/api/workflow.client.start.aspx',
        templateId: templateId,
        entityId: entityId,
        entityClassName: entityClassName,
        title: (typeof (options) === 'undefined' ? '' : options.title),
        idea: (typeof (options) === 'undefined' ? '' : options.idea)
      });
    },
    /*#endregion*/

    /*#region 函数:next(instanceId, nodeId, historyNodeId, options)*/
    /*
    * 执行流程下一步
    *
    * instanceId:工作流实例标识
    */
    next: function(instanceId, nodeId, historyNodeId, options)
    {
      x.workflow.workflowRequest.transact({
        url: '/api/workflow.client.next.aspx',
        instanceId: instanceId,
        nodeId: nodeId,
        historyNodeId: historyNodeId,
        title: (typeof (options) === 'undefined' ? '' : options.title),
        idea: (typeof (options) === 'undefined' ? '' : options.idea)
      });
    },
    /*#endregion*/

    /*#region 函数:gotoStartNode(instanceId, nodeId, historyNodeId, idea)*/
    gotoStartNode: function(instanceId, nodeId, historyNodeId, idea)
    {
      if(idea === '')
      {
        alert("驳回操作必须填写意见。");
        return;
      }

      x.workflow.workflowRequest.transact({
        url: '/api/workflow.client.gotoStartNode.aspx',
        instanceId: instanceId,
        nodeId: nodeId,
        historyNodeId: historyNodeId,
        idea: idea
      });
    },
    /*#endregion*/

    /*#region 函数:gotoRejectNode(instanceId, nodeId, historyNodeId, options)*/
    gotoRejectNode: function(instanceId, nodeId, historyNodeId, options)
    {
      x.workflow.workflowRequest.transact({
        url: '/api/workflow.client.gotoRejectNode.aspx',
        instanceId: instanceId,
        nodeId: nodeId,
        historyNodeId: historyNodeId,
        title: (typeof (options) === 'undefined' ? '' : options.title),
        idea: (typeof (options) === 'undefined' ? '' : options.idea)
      });
    },
    /*#endregion*/

    /*#region 函数:transact(options)*/
    /* 
    *处理工作流
    */
    transact: function(options)
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<action><![CDATA[start]]></action>';
      outString += '<templateId><![CDATA[' + options.templateId + ']]></templateId>';
      outString += '<entityId><![CDATA[' + options.entityId + ']]></entityId>';
      outString += '<title><![CDATA[' + options.title + ']]></title>';
      outString += '<idea><![CDATA[' + options.idea + ']]></idea>';
      outString += '</request>';

      $.post(options.url, { resultType: 'json', xml: outString }, function(response)
      {
        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
          case 0:
            if(typeof (options.callback) !== 'undefined')
            {
              options.callback();
            }
            break;

          case -1:
          case 1:
            alert(result.value);
            break;

          default:
            break;
        }
      });
    }
    /*#endregion*/
  },

  /**
  * 设计时 工具函数.
  */
  designtime:
  {
    /*#region 函数:createDesignXml()*/
    /**
    * 创建设计模板的Xml
    */
    createDesignXml: function()
    {
      x.debug.warn('函数【x.workflow.designtime.createDesignXml】已过时了，可以使用新的函数【x.workflow.template.serialize】。');

      return x.workflow.template.serialize();
    },
    /*#endregion*/

    /*#region 函数:getTemplateByWorkflowTemplateId(templateId, container, toolbar, fectchExpectedActors, options)*/
    /**
    * 获取模板信息
    *
    * templateId : 模板标识
    * container : 容器
    * toolbar : 工具栏信息
    * fectchExpectedActors : 获取预计执行人信息
    * options : 工作流选项
    */
    getTemplateByWorkflowTemplateId: function(templateId, container, toolbar, fectchExpectedActors, options)
    {
      x.debug.warn('函数【x.designtime.getTemplateByWorkflowTemplateId】已过时了，可以使用新的函数【x.workflow.template.download】。');

      x.workflow.template.download({
        toolbar: toolbar,
        container: container,
        url: '/api/workflow.template.findOne.aspx',
        id: templateId,
        fectchExpectedActors: fectchExpectedActors,
        corporationId: (typeof (options) === 'undefined' ? '' : options.corporationId),
        projectId: (typeof (options) === 'undefined' ? '' : options.projectId),
        startActorId: (typeof (options) === 'undefined' ? '' : options.startActorId)
      });
    },
    /*#endregion*/

    /*#region 函数:getTemplateByWorkflowInstanceId(instanceId, container, toolbar, fectchExpectedActors, fectchFinishedActors, options)*/
    /**
    * 获取模板信息
    *
    * instanceId : 模板标识
    * container : 容器
    * toolbar : 工具栏信息
    * fectchExpectedActors : 获取预计执行人信息
    * fectchFinishedActors : 获取已执行人信息
    * options : 工作流选项
    */
    getTemplateByWorkflowInstanceId: function(instanceId, container, toolbar, fectchExpectedActors, fectchFinishedActors, options)
    {
      x.debug.warn('函数【x.designtime.getTemplateByWorkflowInstanceId】已过时了，可以使用新的函数【x.workflow.template.download】。');

      x.workflow.template.download({
        toolbar: toolbar,
        container: container,
        url: '/api/workflow.instance.getTemplateByWorkflowInstanceId.aspx',
        id: instanceId,
        fectchExpectedActors: fectchExpectedActors,
        fectchFinishedActors: fectchFinishedActors,
        corporationId: (typeof (options) === 'undefined' ? '' : options.corporationId),
        projectId: (typeof (options) === 'undefined' ? '' : options.projectId)
      });
    },
    /*#endregion*/

    /*#region 函数:getTemplateByWorkflowInstanceId(instanceId, container, toolbar, fectchExpectedActors, fectchFinishedActors, options)*/
    /**
    * 获取模板信息
    *
    * nodeId : 模板标识
    * container : 容器
    * toolbar : 工具栏信息
    * fectchExpectedActors : 获取预计执行人信息
    * fectchFinishedActors : 获取已执行人信息
    * options : 工作流选项
    */
    getTemplateByWorkflowNodeId: function(nodeId, container, toolbar, fectchExpectedActors, fectchFinishedActors, options)
    {
      x.debug.warn('函数【x.designtime.getTemplateByWorkflowNodeId】已过时了，可以使用新的函数【x.workflow.template.download】。');

      x.workflow.template.download({
        toolbar: toolbar,
        container: container,
        url: '/api/workflow.instance.getTemplateByWorkflowNodeId.aspx',
        nodeId: nodeId,
        fectchExpectedActors: fectchExpectedActors,
        fectchFinishedActors: fectchFinishedActors,
        corporationId: (typeof (options) === 'undefined' ? '' : options.corporationId),
        projectId: (typeof (options) === 'undefined' ? '' : options.projectId)
      });
    }
    /*#endregion*/
  },

  /**
  * 运行时 工具函数.
  */
  runtime:
  {
    /**
    * 图形化显示流程的设计模板.
    *
    * flowInstanceId : 工作流实例Id
    */
    viewWorkflowByWorkflowInstanceId: function(workflowInstanceId, isPopup)
    {
      var url = '/apps/pages/workflowplus/designer/v2/canvas.aspx?workflowInstanceId=' + encodeURIComponent(workflowInstanceId) + '&status=readonly';

      if(isPopup)
      {
        x.mask.getUrl(url);
      }
      else
      {
        location.href = url;
      }
    },

    /**
    * 图形化显示流程的设计模板.
    *
    * nodeId : 节点Id
    */
    viewWorkflowByWorkflowNodeId: function(nodeId, isPopup)
    {
      var url = "/workflowplus/pages/workflow_view.aspx?nid=" + encodeURIComponent(nodeId);

      if(isPopup)
      {
        x.util.showModalDialog(url, Workflowplus.runtime.modalDialogWidth, Workflowplus.runtime.modalDialogHeight);
      }
      else
      {
        location.href = url;
      }
    },

    /*
    *显示流程业务处理内容，传递flowInstanceId
    */
    viewContentByFlowInstanceId: function(workflowInstanceId)
    {

    },

    /*
    *显示流程业务处理内容，传递node id
    */
    viewContentByNodeId: function(nodeId, isPopup)
    {
      var url = "/workflowplus/pages/workflow_view_dispatch.aspx?nid=" + nodeId;

      if(isPopup)
      {
        x.util.showModalDialog(url, x.workflow.runtime.modalDialogWidth, x.workflow.runtime.modalDialogHeight);
      }
      else
      {
        location.href = url;
      }
    },

    // 用列表方式显示指定流程本人经办的节点信息
    viewMyHistoryByFlowInstanceId: function(flowInstanceId, isPopup)
    {
      var url = "/workflowplus/pages/workflow_query_done_all_node_list.aspx?fid=" + flowInstanceId;

      if(isPopup)
      {
        x.util.showModalDialog(url, x.workflow.runtime.modalDialogWidth, x.workflow.runtime.modalDialogHeight);
      }
      else
      {
        location.href = url;
      }
    },

    /**
    * 用列表方式显示指定流程的所有已处理的节点信息
    *
    * @workflowInstanceId 工作流实例标识
    * @container 接收返回结果的Html元素
    */
    viewHistoryNodesByWorkflowInstanceId: function(workflowInstanceId, container)
    {
      if(workflowInstanceId.indexOf('$') > -1
          || workflowInstanceId.indexOf('{') > -1
          || workflowInstanceId.indexOf('}') > -1
          || workflowInstanceId.indexOf('$') > -1)
      {
        return;
      }

      if(typeof (workflowInstanceId) === 'undefined' || workflowInstanceId === '')
      {
        alert('必须填写【流程实例标识】。');
        return;
      }

      if(typeof (container) === 'undefined' || typeof (container.id) === 'undefined')
      {
        x.cookies.add('workflow-runtime$viewHistoryNodesByWorkflowInstanceId$container', 'workflow-runtime$viewHistoryNodesByWorkflowInstanceId$container');
      }
      else
      {
        x.cookies.add('workflow-runtime$viewHistoryNodesByWorkflowInstanceId$container', container.id);
      }

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<workflowInstanceId><![CDATA[' + workflowInstanceId + ']]></workflowInstanceId>';
      outString += '</request>';

      x.net.xhr('/api/workflow.historyNode.findFinishedHistoryNodes.aspx?workflowInstanceId' + workflowInstanceId, {
        callback: function(response)
        {
          var outString = '';

          var result = x.toJSON(response);

          var list = result.ajaxStorage;

          outString += '<table class="table-style table-full-border-style-table" style="width: 100%; margin-bottom: 4px;" >';

          outString += '<tbody>';
          outString += '<tr class="table-row-title" >';
          outString += '<th style="width: 80px;">审批人</th>';
          outString += '<th >审批意见</th>';
          outString += '<th style="width: 120px;">审批时间</th>';
          outString += '</tr>';

          list.each(function(node, index)
          {
            outString += '<tr class="table-row-normal" >';
            outString += '<td>' + node.actorName + '</td>';
            outString += '<td>' + node.idea + '</td>';
            outString += '<td>' + x.date.create(node.finishedTime).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
            outString += '</tr>';
          });

          outString += '</tbody>';
          outString += '</table>';

          if(x.cookies.find('workflow-runtime$viewHistoryNodesByWorkflowInstanceId$container') === '')
          {
            $("#windowWorkflowHistoryNodesContainer").html(outString);
          } else
          {
            $(document.getElementById(x.cookies.find('workflow-runtime$viewHistoryNodesByWorkflowInstanceId$container'))).html(outString);
          }
        }
      });
    },

    // 显示流程维护界面，传递flow id
    manageFlowByFlowInstanceId: function(flowInstanceId, isPopup)
    {
      //var url = "/workflowplus/pages/workflow_manage_flow_view.aspx?fid=" + flowInstanceId;
      var url = "/workflowplus/pages/workflow_management_instance.aspx?fid=" + flowInstanceId;

      if(isPopup)
      {
        x.util.showModalDialog(url, x.workflow.runtime.modalDialogWidth, x.workflow.runtime.modalDialogHeight);
      }
      else
      {
        location.href = url;
      }
    },

    //显示流程维护界面，传递node id
    manageFlowByNodeId: function(nodeId, isPopup)
    {
      //var url = "/workflowplus/pages/workflow_manage_flow_view.aspx?nid=" + nodeId;
      var url = "/workflowplus/pages/workflow_management_instance.aspx?nid=" + nodeId;

      if(isPopup)
      {
        x.util.showModalDialog(url, x.workflow.runtime.modalDialogWidth, x.workflow.runtime.modalDialogHeight);
      }
      else
      {
        location.href = url;
      }
    }
  }
};/**
* workflow      : template(工作流模板)
*
* require       : x.js, x.net.js, x.workflow.js
*/
x.workflow.template = {

    /*#region 函数:create(options)*/
    /**
    * 新建模板
    */
    create: function(options)
    {
        x.net.xhr('/api/workflow.template.create.aspx?type=' + options.type, { callback: options.callback });
    },
    /*#endregion*/

    /*#region 函数:confirmCopy(options)*/
    /**
    * 复制模板
    */
    confirmCopy: function(options)
    {
        x.net.xhr('/api/workflow.template.copy.aspx?id=' + options.id, { callback: options.callback });
    },
    /*#endregion*/

    /*#region 函数:confirmDelete(options)*/
    /**
    * 删除模板
    */
    confirmDelete: function(options)
    {
        if(options.ids !== null && confirm("确定删除？"))
        {
            x.net.xhr('/api/workflow.template.delete.aspx?ids=' + options.ids, { callback: options.callback });
        }
    },
    /*#endregion*/

    /*#region 函数:download(options)*/
    /**
    * 获取模板信息
    */
    download: function(options)
    {
        var toolbar = (typeof (options.toolbar) === 'undefined') ? '' : options.toolbar;

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        // outString += '<action><![CDATA[' + options.action + ']]></action>';
        if(typeof (options.id) !== 'undefined')
        {
            outString += '<id><![CDATA[' + options.id + ']]></id>';
        }
        if(typeof (options.nodeId) !== 'undefined')
        {
            outString += '<nodeId><![CDATA[' + options.nodeId + ']]></nodeId>';
        }
        if(typeof (options.fectchExpectedActors) !== 'undefined')
        {
            outString += '<fectchExpectedActors><![CDATA[' + options.fectchExpectedActors + ']]></fectchExpectedActors>';
        }
        if(typeof (options.fectchFinishedActors) !== 'undefined')
        {
            outString += '<fectchFinishedActors><![CDATA[' + options.fectchFinishedActors + ']]></fectchFinishedActors>';
        }
        if(typeof (options.corporationId) !== 'undefined')
        {
            outString += '<corporationId><![CDATA[' + options.corporationId + ']]></corporationId>';
        }
        if(typeof (options.projectId) !== 'undefined')
        {
            outString += '<projectId><![CDATA[' + options.projectId + ']]></projectId>';
        }
        outString += '</request>';

        $('#workflow-template-toolbar').prepend(toolbar);

        x.net.xhr(options.url, outString, {
            waitingType: 'mini',
            waitingMessage: i18n.net.waiting.queryTipText,
            callback: function(response)
            {
                var outString = '';

                var template = x.toJSON(response).data;

                // -------------------------------------------------------
                // 设置流程模板属性
                // -------------------------------------------------------

                // 字段 corporationId, projectId, startActorId, startActorName, startRoleId, startRoleName 用于计算预计执行人
                outString += '<input id="workflow-template-id" name="workflow-template-id" type="hidden" value="' + template.id + '" />';
                outString += '<input id="workflow-template-corporationIds" name="workflow-template-corporationIds" type="hidden" value="" />';
                outString += '<input id="workflow-template-projectIds" name="workflow-template-projectIds" type="hidden" value="" />';
                outString += '<input id="workflow-template-startActorId" name="workflow-template-startActorId" type="hidden" value="' + template.startActorId + '" />';
                outString += '<input id="workflow-template-startActorName" name="workflow-template-startActorName" type="hidden" value="' + template.startActorName + '" />';
                outString += '<input id="workflow-template-startRoleId" name="workflow-template-startRoleId" type="hidden" value="' + template.startRoleId + '" />';
                outString += '<input id="workflow-template-startRoleName" name="workflow-template-startRoleName" type="hidden" value="' + template.startRoleName + '" />';
                outString += '<input id="workflow-template-name" name="workflow-template-name" type="hidden" value="' + template.name + '" />';
                outString += '<input id="workflow-template-type" name="workflow-template-type" type="hidden" value="' + template.type + '" />';
                outString += '<input id="workflow-template-tags" name="workflow-template-tags" type="hidden" value="' + template.tags + '" />';
                outString += '<input id="workflow-template-entitySchemaName" name="workflow-template-entitySchemaName" type="hidden" value="' + template.entitySchemaName + '" />';
                outString += '<input id="workflow-template-entityMetaData" name="workflow-template-entityMetaData" type="hidden" value="' + template.entityMetaData + '" />';
                outString += '<input id="workflow-template-entityClass" name="workflow-template-entityClass" type="hidden" value="' + template.entityClass + '" />';
                outString += '<input id="workflow-template-viewer" name="workflow-template-viewer" type="hidden" value="' + template.viewer + '" />';
                outString += '<input id="workflow-template-policy" name="workflow-template-policy" type="hidden" value="' + x.encoding.html.encode(template.policy) + '" />';
                outString += '<input id="workflow-template-config" name="workflow-template-config" type="hidden" value="' + x.encoding.html.encode(template.config) + '" />';
                outString += '<input id="workflow-template-remark" name="workflow-template-remark" type="hidden" value="' + x.encoding.html.encode(template.remark) + '" />';
                outString += '<input id="workflow-template-disabled" name="workflow-template-disabled" type="hidden" value="' + template.disabled + '" />';
                outString += '<input id="workflow-template-orderId" name="workflow-template-orderId" type="hidden" value="' + template.orderId + '" />';
                outString += '<input id="workflow-template-updateDate" name="workflow-template-updateDate" type="hidden" value="' + template.updateDateView + '" />';

                if(x.dom('#workflow-displayCompactTemplate').val() === '1')
                {
                    outString += '<table class="table-style" style="width:100%;" >';
                }
                else
                {
                    outString += '<table class="table-style border-4" style="width:100%" >';
                    outString += '<tr id="workflowTemplateHeaderRow" >';
                    outString += '<td class="table-header" >';
                    outString += '<div class="float-right" >';
                    outString += x.workflow.designtime.templateToolbar;
                    outString += '</div>';
                    outString += '流程[<span id="templateName">' + template.name + '</span>]<div class="clear"></div>';
                    outString += '</td>';
                    outString += '</tr>';
                }

                outString += '<tr>';
                outString += '<td class="table-body" >';

                // 节点 节点名 执行人 执行方式 跳转  编辑 上移 下移
                outString += '<table id="workflow_container" class="table-style" style="width:100%" >';

                // 起始节点
                outString += '<tr id="workflowTemplateTitleRow" class="table-row-title" >';
                outString += '<td style="width:40px;" title="节点" >&nbsp;</td>';
                outString += '<td style="width:100px;" >节点名</td>';
                outString += '<td >执行人</td>';
                outString += '<td style="width:80px;" title="下一步骤" ><i class="fa fa-random"></i></td>';
                outString += '<td style="width:60px;" title="执行方式" ><i class="fa fa-users"></i></td>';
                outString += '<td style="width:40px;" title="时限" ><i class="fa fa-clock-o"></i></td>';
                outString += '<td style="width:30px;" title="编辑" ><i class="fa fa-edit"></i></td>';
                outString += '<td style="width:30px;" title="上移" ><i class="fa fa-arrow-up"></i></td>';
                outString += '<td style="width:30px;" title="下移" ><i class="fa fa-arrow-down"></i></td>';
                outString += '<td style="width:30px;" title="删除" ><i class="fa fa-times"></i></td>';
                outString += '</tr>';

                x.each(template.activities, function(index, node)
                {
                    if(x.dom('#workflow-displayCompactTemplate').val() === '1'
                        && (options.url.indexOf('/api/workflow.template.findOne.aspx') === -1 && x.dom('#workflow-hiddenTemplateTableFooter').val() === '1')
                        && template.activities.length === (index + 1))
                    {
                        if($('#workflow-higthlightTemplateCurrentNode').val() === '1')
                        {
                            outString += '<tr id="' + node.id + '" class="table-row-normal-transparent workflow-node ' + ((node.status === 1) ? 'workflow-node-current' : '') + '" style="background-color:#fcffca;" >';
                        }
                        else
                        {
                            outString += '<tr id="' + node.id + '" class="table-row-normal-transparent workflow-node ' + ((node.status === 1) ? 'workflow-node-current' : '') + '" >';
                        }
                    }
                    else
                    {
                        if($('#workflow-higthlightTemplateCurrentNode').val() === '1')
                        {
                            outString += '<tr id="' + node.id + '" class="table-row-normal workflow-node ' + ((node.status === 1) ? 'workflow-node-current' : '') + '" style="background-color:#fcffca;" >';
                        }
                        else
                        {
                            outString += '<tr id="' + node.id + '" class="table-row-normal workflow-node ' + ((node.status === 1) ? 'workflow-node-current' : '') + '" >';
                        }
                    }

                    outString += "<td></td>";
                    outString += '<td>';
                    outString += '<span id="' + node.id + '_nodeName" >' + node.name + '</span>';
                    outString += '<input id="' + node.id + '_id" type="hidden" value="' + x.isUndefined(node.id, '') + '" />';
                    outString += '<input id="' + node.id + '_name" type="hidden" value="' + x.isUndefined(node.name, '') + '" />';
                    outString += '<input id="' + node.id + '_type" type="hidden" value="' + x.isUndefined(node.type, '') + '" />';
                    outString += '<input id="' + node.id + '_actorScope" type="hidden" value="' + x.isUndefined(node.actorScope, '') + '" />';
                    outString += '<input id="' + node.id + '_actorDescription" type="hidden" value="' + x.isUndefined(node.actorDescription, '') + '" />';
                    outString += '<input id="' + node.id + '_actorCounter" type="hidden" value="' + x.isUndefined(node.actorCounter, '') + '" />';
                    outString += '<input id="' + node.id + '_actorMethod" type="hidden" value="' + x.isUndefined(node.actorMethod, '') + '" />';
                    outString += '<input id="' + node.id + '_editor" type="hidden" value="' + x.isUndefined(node.editor, '') + '" />';
                    outString += '<input id="' + node.id + '_handler" type="hidden" value="' + x.isUndefined(node.handler, '') + '" />';
                    outString += '<input id="' + node.id + '_backNodes" type="hidden" value="' + x.isUndefined(node.backNodes, '') + '" />';
                    outString += '<input id="' + node.id + '_forwardNodes" type="hidden" value="' + x.isUndefined(node.forwardNodes, '') + '" />';
                    outString += '<input id="' + node.id + '_commissionActors" type="hidden" value="' + x.isUndefined(node.commissionActors, '') + '" />';
                    outString += '<input id="' + node.id + '_timelimit" type="hidden" value="' + x.isUndefined(node.timelimit, '') + '" />';
                    outString += '<input id="' + node.id + '_filterActors" type="hidden" value="' + x.isUndefined(node.filterActors, '') + '" />';
                    outString += '<input id="' + node.id + '_sendAlertTask" type="hidden" value="' + x.isUndefined(node.sendAlertTask, '') + '" />';
                    outString += '<input id="' + node.id + '_policy" type="hidden" value="' + (x.isUndefined(node.policy) ? '' : x.encoding.html.encode(node.policy)) + '" />';
                    outString += '<input id="' + node.id + '_remark" type="hidden" value="' + (x.isUndefined(node.remark) ? '' : x.encoding.html.encode(node.remark)) + '" />';
                    outString += '<input id="' + node.id + '_status" type="hidden" value="' + x.isUndefined(node.status, '') + '" />';
                    outString += '<input id="' + node.id + '_positionX" type="hidden" value="' + x.isUndefined(node.positionX, '') + '" />';
                    outString += '<input id="' + node.id + '_positionY" type="hidden" value="' + x.isUndefined(node.positionY, '') + '" />';
                    outString += '<input id="' + node.id + '_repeatDirection" type="hidden" value="' + x.isUndefined(node.repeatDirection, '') + '" />';
                    outString += '<input id="' + node.id + '_zIndex" type="hidden" value="' + x.isUndefined(node.zIndex, '0') + '" />';
                    outString += '<input id="' + node.id + '_canEdit" type="hidden" value="' + x.isUndefined(node.canEdit, '0') + '" />';
                    outString += '<input id="' + node.id + '_canMove" type="hidden" value="' + x.isUndefined(node.canMove, '0') + '" />';
                    outString += '<input id="' + node.id + '_canDelete" type="hidden" value="' + x.isUndefined(node.canDelete, '0') + '" />';
                    outString += '<input id="' + node.id + '_canUploadFile" type="hidden" value="' + x.isUndefined(node.canUploadFile, '0') + '" />';
                    outString += '<input id="' + node.id + '_radiation" type="hidden" value="' + (typeof (node.switcher) === 'undefined' || typeof (node.switcher.radiation) === 'undefined' ? '0' : node.switcher.radiation) + '" />';
                    outString += '<input id="' + node.id + '_exits" type="hidden" value="' + (typeof (node.switcher) === 'undefined' || typeof (node.switcher.exits) === 'undefined' ? '[]' : x.encoding.html.encode(node.switcher.exits)) + '" />';
                    // outString += '<input id="' + node.id + '_exits" type="hidden" value="' + (typeof (node.switcher) === 'undefined' || typeof (node.switcher.exits) === 'undefined' ? '[]' : x.workflow.switcherExit.toSwitcherExitConditionText(node.switcher.exits)) + '" />';

                    outString += '</td>';
                    outString += '<td>';
                    outString += "<span id=\"" + node.id + "_actorDescription\" >" + (typeof (node.actorDescription) === 'undefined' ? '' : node.actorDescription) + "</span>";

                    if(node.expectedActors === '')
                    {
                        outString += '<span id="' + node.id + '_expectedActors" class="ajax-workflow-node-expected-actors-notfound" >(<label>未找到预计执行人.</label>)</span>';
                    }
                    else
                    {
                        outString += '<span id="' + node.id + '_expectedActors" class="ajax-workflow-node-expected-actors" >(<label><strong>预计执行人:</strong>' + (typeof (node.expectedActors) === 'undefined' ? '' : node.expectedActors) + '</label>)</span>';
                    }

                    outString += "</td>";
                    // 下一步骤
                    outString += '<td></td>';
                    // 执行方式
                    outString += "<td>" + x.isUndefined(node.actorMethod, '并审') + "</td>";
                    // 时限
                    outString += "<td>" + x.isUndefined(node.timelimit, '0') + "</td>";
                    // 编辑
                    outString += "<td><a href=\"#\" >编辑</a></td>";
                    // 移动
                    outString += "<td><a href=\"#\" >上移</a></td>";
                    outString += "<td><a href=\"#\" >下移</a></td>";
                    // 删除
                    outString += "<td><a href=\"#\" >删除</a></td>";
                    outString += "</tr>";
                });

                if(options.url.indexOf('/api/workflow.template.findOne.aspx') > -1 || x.dom('#workflow-hiddenTemplateTableFooter').val() !== '1')
                {
                    outString += '<tr class="table-row-normal-transparent workflow-node-new" >';
                    outString += '<td colspan="6" style="text-align:left;" >';
                    // 只有流程模板允许修改发起角色，流程实例不允许修改角色
                    if(options.url.indexOf('/api/workflow.template.findOne.aspx') > -1)
                    {
                        outString += '<span style="padding:0 4px 0 0; font-weight:bold;" >发起角色:</span>';
                        outString += '<span id="workflow-template-startRoleText" style="padding:0 8px 0 0;" >' + template.startRoleName + '</span>';
                        outString += '<a href="javascript:x.workflow.instance.actor.getStartRoleWizard(\'' + template.startActorId + '\');" >编辑</a>';
                    }
                    outString += '</td>';
                    outString += '<td colspan="4" style="text-align:right;" >';
                    if(x.dom('#workflow-hiddenTemplateTableFooter').val() !== '1')
                    {
                        outString += '<a href=\"javascript:x.workflow.node.create();\" >新增节点</a>';
                    }
                    outString += '</td>';
                    outString += '</tr>';
                }

                outString += '</table>';
                outString += '</td>';
                outString += '</tr>';

                // 精简模式
                if(x.dom('#workflow-displayCompactTemplate').val() === '1')
                {
                    outString += '</table>';
                }
                else
                {
                    outString += '<tr id="workflowTemplateFooterRow" ><td class="table-footer" ><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></td></tr></table>';
                }

                $(options.container).html(outString);

                x.dom.features.bind();

                x.workflow.template.setTemplateObject(template);

                x.workflow.template.setTemplateEntityMetaData(template.entityClass);

                x.workflow.node.sync();

                if(typeof (options.callback) !== 'undefined')
                {
                    options.callback();
                }
            }
        });
    },
    /*#endregion*/

    /*#region 函数:getTemplateObject()*/
    /**
    * 保存模板基本信息
    */
    getTemplateObject: function()
    {
        var outString = '';

        outString += '{';
        outString += '"id":"' + x.dom('#workflow-template-id').val() + '", ';
        outString += '"corporationIds":"' + x.dom('#workflow-template-corporationIds').val() + '", ';
        outString += '"projectIds":"' + x.dom('#workflow-template-projectIds').val() + '", ';
        outString += '"startActorId":"' + x.dom('#workflow-template-startActorId').val() + '", ';
        outString += '"startActorName":"' + x.dom('#workflow-template-startActorName').val() + '", ';
        outString += '"startRoleId":"' + x.dom('#workflow-template-startRoleId').val() + '", ';
        outString += '"startRoleName":"' + x.dom('#workflow-template-startRoleName').val() + '", ';
        outString += '"name":"' + x.dom('#workflow-template-name').val() + '", ';
        outString += '"type":"' + x.dom('#workflow-template-type').val() + '", ';
        outString += '"entitySchemaName":"' + x.dom('#workflow-template-entitySchemaName').val() + '", ';
        outString += '"entityMetaData":"' + x.dom('#workflow-template-entityMetaData').val() + '", ';
        outString += '"entityClass":"' + x.dom('#workflow-template-entityClass').val() + '", ';
        outString += '"viewer":"' + x.dom('#workflow-template-viewer').val() + '", ';
        outString += '"policy":"' + x.encoding.html.encode(x.dom('#workflow-template-policy').val()) + '", ';
        outString += '"config":"' + x.encoding.html.encode(x.dom('#workflow-template-config').val()) + '", ';
        outString += '"remark":"' + x.encoding.html.encode(x.dom('#workflow-template-remark').val()) + '", ';
        outString += '"disabled":"' + x.dom('#workflow-template-disabled').val() + '" ';
        outString += '}';

        return x.toJSON(outString);
    },
    /*#endregion*/

    /*#region 函数:setTemplateObject(template)*/
    /**
    * 加载模板基本信息
    */
    setTemplateObject: function(template)
    {
        x.dom('#workflow-template-id').val(template.id);
        x.dom('#workflow-template-name').val(template.name);
        x.dom('#workflow-template-type').val(template.type);
        x.dom('#workflow-template-entityClass').val(template.entityClass);
        x.dom('#workflow-template-viewer').val(template.viewer);
        x.dom('#workflow-template-policy').val(x.encoding.html.decode(template.policy));
        x.dom('#workflow-template-config').val(x.encoding.html.decode(template.config));
        x.dom('#workflow-template-remark').val(x.encoding.html.decode(template.remark));
        x.dom('#workflow-template-disabled').val(template.disabled);
    },
    /*#endregion*/

    /*#region 函数:setTemplateCorporationIds(corporationIds)*/
    /**
    * 设置工作流模板所属的公司标识信息
    */
    setTemplateCorporationIds: function(corporationIds)
    {
        x.dom('#workflow-template-corporationIds').val(corporationIds);
    },
    /*#endregion*/

    /*#region 函数:setTemplateProjectIds(projectIds)*/
    /**
    * 设置工作流模板所属的项目标识信息
    */
    setTemplateProjectIds: function(projectIds)
    {
        x.dom('#workflow-template-projectIds').val(projectIds);
    },
    /*#endregion*/

    /*#region 函数:setTemplateEntityMetaData(entityClassName, entitySchemaName)*/
    /**
    * 设置业务实体元数据信息
    */
    setTemplateEntityMetaData: function(entityClassName, entitySchemaName)
    {
        x.dom('#workflow-template-entityClass').val(entityClassName);

        if(typeof (entitySchemaName) === 'undefined')
        {
            x.dom('#workflow-template-entitySchemaName').val(entitySchemaName);
        }

        x.net.xhr('/api/kernel.entities.metaData.findAllByEntityClassName.aspx?entityClassName=' + entityClassName, '', {
            callback: function(response)
            {
                var outString = '';

                var list = x.toJSON(response).data;

                x.each(list, function(index, node)
                {
                    outString += node.fieldName + ',';
                    // x.debug.log(index + node.fieldName);
                });

                if(outString.substr(outString.length - 1, 1) === ',')
                {
                    outString = outString.substr(0, outString.length - 1);
                }

                document.getElementById('workflow-template-entityMetaData').value = outString;
            }
        });
    },
    /*#endregion*/

    /*#region 函数:getTemplategetInternalMetaData(entityClassName, entitySchemaName)*/
    /**
    * 获取配置信息中内置的实体元数据信息
    */
    getTemplategetInternalMetaData: function()
    {
        var text = x.dom('#workflow-template-config').val();

        if(text == '') { return []; }

        var config = x.toJSON(text);

        return typeof (config.metaData) == 'undefined' ? [] : config.metaData;
    },
    /*#endregion*/

    /*#region 函数:verify()*/
    /*
    * 验证
    */
    verify: function()
    {
        var settings = x.workflow.settings;

        var nodes = $(settings.workflowNodeSelector);

        var hasNodeObject = false;

        for(var i = 0;i < nodes.length;i++)
        {
            var text = nodes[i].childNodes[1].childNodes[nodes[i].childNodes[1].childNodes.length - 1].value;

            if(text !== '')
            {
                var list = x.toJSON(text);

                for(var j = 0;j < list.length;j++)
                {
                    hasNodeObject = false;

                    for(var k = 0;k < nodes.length;k++)
                    {
                        if(list[j].toNodeId === nodes[i].childNodes[1].childNodes[1].value)
                        {
                            hasNodeObject = true;
                        }
                    }
                }
            }
        }
    },
    /*#endregion*/

    /*#region 函数:serialize()*/
    serialize: function()
    {
        var outString = '<?xml version="1.0" encoding="utf-8"?>\r\n';

        var settings = x.workflow.settings;

        var nodes = $(settings.workflowNodeSelector);

        outString += '<workflow ' + x.workflow.template.serializeTemplateAttributes(x.workflow.template.getTemplateObject()) + ' >';

        // -------------------------------------------------------
        // verify
        // -------------------------------------------------------

        x.workflow.template.verify();

        // -------------------------------------------------------
        // designtime
        // -------------------------------------------------------

        outString += x.workflow.template.serializeDesigntimeXml(nodes);

        // -------------------------------------------------------
        // runtime
        // -------------------------------------------------------

        outString += x.workflow.template.serializeRuntimeXml(nodes);

        outString += '</workflow>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeTemplateAttributes(template, options)*/
    /**
    * 序列化设计时Xml模板属性文本
    */
    serializeTemplateAttributes: function(template, options)
    {
        var outString = 'id="' + template.id + '" '
                + 'startActorId="' + template.startActorId + '" '
                + 'startActorName="' + template.startActorName + '" '
                + 'startRoleId="' + template.startRoleId + '" '
                + 'startRoleName="' + template.startRoleName + '" '
                + 'name="' + template.name + '" '
                + 'type="' + template.type + '" '
                + 'entitySchemaName="' + (typeof (template.entitySchemaName) === 'undefined' ? '' : template.entitySchemaName) + '" '
                + 'entityMetaData="' + (typeof (template.entityMetaData) === 'undefined' ? '' : template.entityMetaData) + '" '
                + 'entityClass="' + (typeof (template.entityClass) === 'undefined' ? '' : template.entityClass) + '" '
                + 'viewer="' + (typeof (template.viewer) === 'undefined' ? '' : x.encoding.html.encode(template.viewer)) + '" '
                + 'policy="' + (typeof (template.policy) === 'undefined' ? '' : template.policy) + '" '
                + 'config="' + (typeof (template.config) === 'undefined' ? '' : template.config) + '" '
                + 'remark="' + (typeof (template.remark) === 'undefined' ? '' : template.remark) + '" '
                + 'disabled="' + (typeof (template.disabled) === 'undefined' ? '' : template.disabled) + '" ';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeDesigntimeXml(nodes, options)*/
    /**
    * 序列化设计时Xml模板文本
    */
    serializeDesigntimeXml: function(nodes, options)
    {
        var pointX = 30;
        var pointY = 200;
        // 节点索引
        var nodeIndex = 1;
        // 节点Z轴索引
        var nodeZIndex = 1;
        // 节点间隔
        var nodeSpace = 150;
        // 开始节点偏移量
        var startNodeOffset = 75;
        // 结束节点偏移量
        var terminatNodeOffset = -15;

        var outString = '<designtime>';

        nodes.each(function(index, node)
        {
            if(nodeIndex < nodes.length)
            {
                // nodeIndex === 1 为开始节点
                outString += x.workflow.template.serializeDesigntimeActivityXml(node, {
                    index: index,
                    type: (nodeIndex === 1) ? 'starter' : node.childNodes[1].childNodes[3].value,
                    idPrefix: (nodeIndex === 1) ? 'sn_' : 'nn_',
                    positionX: (nodeIndex === 1) ? (pointX - 42 + startNodeOffset) : (pointX + nodeSpace * (nodeIndex - 1) + 10),
                    positionY: (pointY - 15),
                    zIndex: nodeZIndex++
                });

                if(node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value === "[]")
                {
                    outString += x.workflow.template.serializeDesigntimeRuleXml(node, {
                        index: nodeIndex,
                        type: 'straightline',
                        beginActivityId: node.childNodes[1].childNodes[1].value,
                        beginActivityName: node.childNodes[1].childNodes[2].value,
                        endActivityId: nodes[index + 1].childNodes[1].childNodes[1].value,
                        endActivityName: nodes[index + 1].childNodes[1].childNodes[2].value,
                        beginPointX: (pointX + startNodeOffset),
                        beginPointY: pointY,
                        endPointX: (pointX + nodeSpace),
                        endPointY: pointY,
                        zIndex: nodeZIndex++
                    });
                }
                else
                {
                    exits = x.toJSON(node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value);

                    x.each(exits, function(exitIndex, exit)
                    {
                        outString += x.workflow.template.serializeDesigntimeRuleXml(node, {
                            index: nodeIndex + '_' + (exitIndex + 1),
                            type: 'foldline',
                            condition: exit.friendlyCondition,
                            beginActivityId: node.childNodes[1].childNodes[1].value,
                            beginActivityName: node.childNodes[1].childNodes[2].value,
                            endActivityId: exit.toNodeId,
                            endActivityName: x.workflow.node.attribute(exit.toNodeId, 'name'),
                            beginPointX: (pointX + startNodeOffset),
                            beginPointY: pointY,
                            endPointX: (pointX + nodeSpace),
                            endPointY: pointY,
                            zIndex: nodeZIndex++
                        });
                    });
                }
                nodeIndex++;
            }
            else
            {
                outString += x.workflow.template.serializeDesigntimeActivityXml(node, {
                    index: index,
                    type: 'terminator',
                    idPrefix: 'en_',
                    positionX: (pointX + nodeSpace * (nodeIndex - 1) + terminatNodeOffset),
                    positionY: (pointY - 15),
                    zIndex: nodeZIndex
                });
            }
        });

        outString += '</designtime>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeDesigntimeActivityXml(node, options)*/
    /**
    * 序列化设计时工作流节点
    */
    serializeDesigntimeActivityXml: function(node, options)
    {
        var outString = '<activity id="' + (options.type === 'starter' ? 'sn_0' : (options.idPrefix + options.index)) + '" '
                + 'name="' + node.childNodes[1].childNodes[2].value + '" '
                + 'type="' + options.type + '" '
                + 'actorScope="' + (options.type === 'starter' ? 'initiator#00000000-0000-0000-0000-000000000000' : node.childNodes[1].childNodes[4].value) + '" '
                + 'actorDescription="' + (options.type === 'starter' ? '流程发起人' : node.childNodes[1].childNodes[5].value) + '" '
                + 'actorCounter="' + (options.type === 'starter' ? '1' : node.childNodes[1].childNodes[6].value) + '" '
                + 'actorMethod="' + (options.type === 'starter' ? '提交' : node.childNodes[1].childNodes[7].value) + '" '
                + 'editor="' + node.childNodes[1].childNodes[8].value + '" '
                + 'handler="' + node.childNodes[1].childNodes[9].value + '" '
                + 'backNodes="' + node.childNodes[1].childNodes[10].value + '" '
                + 'forwardNodes="' + node.childNodes[1].childNodes[11].value + '" '
                + 'commissionActors="' + node.childNodes[1].childNodes[12].value + '" '
                + 'timelimit="' + node.childNodes[1].childNodes[13].value + '" '
                + 'filterActors="' + node.childNodes[1].childNodes[14].value + '" '
                + 'sendAlertTask="' + node.childNodes[1].childNodes[15].value + '" '
                + 'policy="' + x.string.trim(JSON.stringify(node.childNodes[1].childNodes[16].value).replace(/"/g, '##'), '##') + '" '
                + 'remark="' + node.childNodes[1].childNodes[17].value + '" '
                + 'status="' + node.childNodes[1].childNodes[18].value + '" '
                + 'positionX="' + options.positionX + '" '
                + 'positionY="' + options.positionY + '" '
                + 'repeatDirection="' + node.childNodes[1].childNodes[21].value + '" '
                + 'zIndex="' + options.zIndex + '" '
                + 'canEdit="' + node.childNodes[1].childNodes[23].value + '" '
                + 'canMove="' + node.childNodes[1].childNodes[24].value + '" '
                + 'canDelete="' + node.childNodes[1].childNodes[25].value + '" '
                + 'canUploadFile="' + node.childNodes[1].childNodes[26].value + '" '
                + 'radiation="' + node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 2].value + '" />';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeDesigntimeRuleXml(node, options)*/
    /**
    * 序列化设计时工作流规则
    */
    serializeDesigntimeRuleXml: function(node, options)
    {
        return '<rule id="rule_' + options.index + '" '
                + 'name="规则' + options.index + '" '
                + 'type="' + options.type + '" '
                + 'condition="' + options.condition + '" '
                + 'beginActivityId="' + options.beginActivityId + '" '
                + 'beginActivityName="' + options.beginActivityName + '" '
                + 'endActivityId="' + options.endActivityId + '" '
                + 'endActivityName="' + options.endActivityName + '" '
                + 'beginPointX="' + options.beginPointX + '" '
                + 'beginPointY="' + options.beginPointY + '" '
                + 'endPointX="' + options.endPointX + '" '
                + 'endPointY="' + options.endPointY + '" '
                + 'turnPoint1X="' + (options.type === 'straightline' ? 0 : options.turnPoint1X) + '" '
                + 'turnPoint1Y="' + (options.type === 'straightline' ? 0 : options.turnPoint1Y) + '" '
                + 'turnPoint2X="' + (options.type === 'straightline' ? 0 : options.turnPoint2X) + '" '
                + 'turnPoint2Y="' + (options.type === 'straightline' ? 0 : options.turnPoint2Y) + '" '
                + 'zIndex="' + options.zIndex + '" />';
    },
    /*#endregion*/

    /*#region 函数:serializeRuntimeXml(nodes, options)*/
    /**
    * 序列化运行时模板
    */
    serializeRuntimeXml: function(nodes, options)
    {
        var outString = '<runtime>';

        // 节点索引
        var nodeIndex = 1;

        var exitToNodeIds = [];

        // 每个出口
        var nodeSwitchExitToNodeIds = [];

        nodes.each(function(index, node)
        {
            nodeSwitchExitToNodeIds[nodeIndex] = [];

            if(nodeIndex === 1)
            {
                // 普通节点
                outString += x.workflow.template.serializeRuntimeNodeXml(node, { type: 'starter', idPrefix: 'sn_', index: index });

                if(node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value === "[]")
                {
                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: nodeIndex,
                        from: (index === 0 ? 'sn_' : 'nn_') + index,
                        fromType: 'node',
                        to: (nodeIndex + 1 === nodes.length ? 'en_' : 'nn_') + nodeIndex,
                        toType: 'node'
                    });
                }
                else
                {
                    // 设置分支器
                    exits = x.toJSON(node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value);

                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: index + '_switcher',
                        from: (index === 0 ? 'sn_' : 'nn_') + index,
                        fromType: 'node',
                        to: 'switcher_' + nodeIndex,
                        toType: 'switcher'
                    });

                    outString += x.workflow.template.serializeRuntimeSwitcherXml(exits, {
                        index: nodeIndex,
                        radiation: 0,
                        field: '',
                        canEdit: 0,
                        canMove: 0,
                        remark: ''
                    });

                    // 设置默认路径
                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: nodeIndex + '_1',
                        from: 'switcher_' + nodeIndex + '_exit_1',
                        fromType: 'switcher_exit',
                        to: 'nn_' + (index + 1),
                        toType: 'node'
                    });

                    x.each(exits, function(exitIndex, exit)
                    {
                        if(exitIndex > 0)
                        {
                            exitToNodeIds[exit.toNodeId] = exit.toNodeId;

                            nodeSwitchExitToNodeIds[nodeIndex][exit.toNodeId] = exit.toNodeId;

                            outString += x.workflow.template.serializeRuntimeRouteXml({
                                index: nodeIndex + '_' + (exitIndex + 1),
                                from: 'switcher_' + nodeIndex + '_exit_' + (exitIndex + 1),
                                fromType: 'switcher_exit',
                                to: 'collector_' + x.workflow.node.nodeIndex(exit.toNodeId),
                                toType: 'collector'
                            });
                        }
                    });
                }

                nodeIndex++;
            }
            else if(nodeIndex < nodes.length)
            {
                if(typeof (exitToNodeIds['nn_' + index]) !== 'undefined')
                {
                    // 设置聚合器
                    outString += x.workflow.template.serializeRuntimeCollectorXml({
                        index: index,
                        condition: true,
                        canEdit: 0,
                        remark: ''
                    });

                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: index + '_collector',
                        from: 'collector_' + index,
                        fromType: 'collector',
                        to: 'nn_' + index,
                        toType: 'node'
                    });
                }

                // 普通节点
                outString += x.workflow.template.serializeRuntimeNodeXml(node, { type: 'node', idPrefix: 'nn_', index: index });

                if(node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value === "[]")
                {
                    // 设置默认路径
                    if(typeof (exitToNodeIds['nn_' + nodeIndex]) === 'undefined' && typeof (exitToNodeIds['en_' + nodeIndex]) === 'undefined')
                    {
                        outString += x.workflow.template.serializeRuntimeRouteXml({
                            index: nodeIndex,
                            from: (index === 0 ? 'sn_' : 'nn_') + index,
                            fromType: 'node',
                            to: (nodeIndex + 1 === nodes.length ? 'en_' : 'nn_') + nodeIndex,
                            toType: 'node'
                        });
                    }
                    else
                    {
                        outString += x.workflow.template.serializeRuntimeRouteXml({
                            index: nodeIndex,
                            from: (index === 0 ? 'sn_' : 'nn_') + index,
                            fromType: 'node',
                            to: 'collector_' + nodeIndex,
                            toType: 'collector'
                        });
                    }
                }
                else
                {
                    // 设置分支器
                    exits = x.toJSON(node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value);

                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: index + '_switcher',
                        from: (index === 0 ? 'sn_' : 'nn_') + index,
                        fromType: 'node',
                        to: 'switcher_' + nodeIndex,
                        toType: 'switcher'
                    });

                    outString += x.workflow.template.serializeRuntimeSwitcherXml(exits, {
                        index: nodeIndex,
                        radiation: 0,
                        field: '',
                        canEdit: 0,
                        canMove: 0,
                        remark: ''
                    });

                    // nodeSwitchExitToNodeIds[nodeIndex][exit.toNodeId] = exit.toNodeId;

                    /*
                    if(typeof (exitToNodeIds['nn_' + nodeIndex]) === 'undefined')
                    {
                        outString += x.workflow.template.serializeRuntimeRouteXml({
                            index: nodeIndex + '_1',
                            from: 'switcher_' + nodeIndex + '_exit_1',
                            fromType: 'switcher_exit',
                            to: 'nn_' + index,
                            toType: 'node'
                        });
                    }
                    else
                    {
                        outString += x.workflow.template.serializeRuntimeRouteXml({
                            index: nodeIndex + '_1',
                            from: 'switcher_' + nodeIndex + '_exit_1',
                            fromType: 'switcher_exit',
                            to: 'collector_' + index,
                            toType: 'collector'
                        });
                    }*/

                    x.each(exits, function(exitIndex, exit)
                    {
                        if(exitIndex > 0)
                        {
                            exitToNodeIds[exit.toNodeId] = exit.toNodeId;

                            outString += x.workflow.template.serializeRuntimeRouteXml({
                                index: nodeIndex + '_' + (exitIndex + 1),
                                from: 'switcher_' + nodeIndex + '_exit_' + (exitIndex + 1),
                                fromType: 'switcher_exit',
                                to: 'collector_' + x.workflow.node.nodeIndex(exit.toNodeId),
                                toType: 'collector'
                            });
                        }
                    });

                    // 设置默认路径
                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: nodeIndex + '_1',
                        from: 'switcher_' + nodeIndex + '_exit_1',
                        fromType: 'switcher_exit',
                        to: 'nn_' + (index + 1),
                        toType: 'node'
                    });
                }

                nodeIndex++;
            }
            else
            {
                if(typeof (exitToNodeIds['en_' + index]) !== 'undefined')
                {
                    // 设置聚合器
                    outString += x.workflow.template.serializeRuntimeCollectorXml({
                        index: index,
                        condition: true,
                        canEdit: 0,
                        remark: ''
                    });

                    outString += x.workflow.template.serializeRuntimeRouteXml({
                        index: index + '_collector',
                        from: 'collector_' + index,
                        fromType: 'collector',
                        to: 'en_' + index,
                        toType: 'node'
                    });
                }

                // 结束节点
                outString += x.workflow.template.serializeRuntimeNodeXml(node, { type: 'terminator', idPrefix: 'en_', index: index });
            }
        });

        outString += '</runtime>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeRuntimeNodeXml(node, options)*/
    /**
    * 序列化运行时工作流节点
    */
    serializeRuntimeNodeXml: function(node, options)
    {
        var outString = '';

        outString += '<' + options.type + ' id="' + options.idPrefix + options.index + '" '
                + 'name="' + node.childNodes[1].childNodes[2].value + '" '
                + 'remark="' + node.childNodes[1].childNodes[15].value + '" '
                + 'editor="' + node.childNodes[1].childNodes[8].value + '" '
                + 'handler="' + node.childNodes[1].childNodes[9].value + '" '
                + 'timelimit="' + node.childNodes[1].childNodes[13].value + '" '
                + 'filterActors="' + node.childNodes[1].childNodes[14].value + '" '
                + 'sendAlertTask="' + node.childNodes[1].childNodes[15].value + '" '
                + 'policy="' + x.string.trim(JSON.stringify(node.childNodes[1].childNodes[16].value).replace(/"/g, '##'), '##') + '" >';

        if(options.type == 'starter')
        {
            outString += '<actor scope="initiator" description="流程发起人" counter="1" method="提交" />';
        }
        else
        {
            outString += '<actor scope="' + node.childNodes[1].childNodes[4].value + '" description="' + node.childNodes[1].childNodes[5].value + '" counter="' + node.childNodes[1].childNodes[6].value + '" method="' + node.childNodes[1].childNodes[7].value + '" />';
        }
        outString += '<operation>';
        outString += '<back cando="' + (node.childNodes[1].childNodes[10].value === '' ? '0' : '1') + '" nodes="' + node.childNodes[1].childNodes[10].value + '" />';
        outString += '<forward cando="' + (node.childNodes[1].childNodes[11].value === '' ? '0' : '1') + '" nodes="' + node.childNodes[1].childNodes[11].value + '" />';
        outString += '<commission cando="' + (node.childNodes[1].childNodes[12].value === '' ? '0' : '1') + '" actors="' + node.childNodes[1].childNodes[12].value + '" />';
        outString += '</operation>';
        outString += '</' + options.type + '>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeRuntimeRouteXml(options)*/
    /**
    * 序列化运行时工作流路由
    */
    serializeRuntimeRouteXml: function(options)
    {
        return '<route id="route_' + options.index + '" '
                + 'from="' + options.from + '" '
                + 'fromType="' + options.fromType + '" '
                + 'to="' + options.to + '" '
                + 'toType="' + options.toType + '" />';
    },
    /*#endregion*/

    /*#region 函数:serializeRuntimeSwitcherXml(exits, options)*/
    /**
    * 序列化运行时工作流路由
    */
    serializeRuntimeSwitcherXml: function(exits, options)
    {
        var outString = '<switcher id="switcher_' + options.index + '" '
                    + 'name="分支器' + options.index + '" '
                    + 'radiation="' + options.radiation + '" '
                    + 'field="' + options.field + '" '
                    + 'canEdit="' + options.canEdit + '" '
                    + 'canMove="' + options.canMove + '" '
                    + 'remark="' + options.remark + '" >';

        x.each(exits, function(exitIndex, exit)
        {
            outString += '<exit id="switcher_' + options.index + '_exit_' + (exitIndex + 1) + '" condition="' + x.workflow.switcherExit.toSwitcherExitConditionText(exit).replace(/"/g, '##') + '" />';
        });

        outString += '</switcher>';

        return outString;
    },
    /*#endregion*/

    /*#region 函数:serializeRuntimeCollectorXml(options)*/
    /**
    * 序列化运行时工作流聚合器
    */
    serializeRuntimeCollectorXml: function(options)
    {
        return '<collector id="collector_' + options.index + '" '
                + 'name="聚合' + options.index + '" '
                + 'condition="' + options.condition + '" '
                + 'canEdit="' + options.index + '" '
                + 'remark="' + options.remark + '" />';
    }
    /*#endregion*/
};/**
* workflow      : instance(流程实例)
*
* require       : x.js, x.net.js, x.workflow.js
*/
x.workflow.instance = {

  /*#region 函数:pause(options)*/
  /**
  * 暂停流程实例
  */
  pause: function(options)
  {
    if(options.id !== '' && confirm('确定要暂停此流程吗？'))
    {
      x.net.xhr('/api/workflow.instance.pause.aspx?id=' + options.id, {
        callback: options.callback
      });
    }
  },
  /*#endregion*/

  /*#region 函数:resume(options)*/
  /**
  * 恢复流程实例
  */
  resume: function(options)
  {
    if(options.id !== '' && confirm('确定要恢复此流程吗？'))
    {
      x.net.xhr('/api/workflow.instance.resume.aspx?id=' + options.id, {
        callback: options.callback
      });
    }
  },
  /*#endregion*/

  /*#region 函数:abort(options)*/
  /**
  * 恢复流程实例
  */
  abort: function(options)
  {
    if(options.id !== '' && confirm("确定要中止此流程吗？ 流程一旦被中止，将无法恢复!"))
    {
      x.net.xhr('/api/workflow.instance.abort.aspx?id=' + options.id, {
        callback: options.callback
      });
    }
  },
  /*#endregion*/

  /*#region 函数:confirmDelete(options)*/
  /**
  * 删除模板
  */
  confirmDelete: function(options)
  {
    if(options.id !== null && confirm("确定删除？"))
    {
      x.net.xhr('/api/workflow.instance.delete.aspx?id=' + options.id, {
        callback: options.callback
      });
    }
  },
  /*#endregion*/

  /*
  * 执行者信息
  */
  actor: {
    /*#region*/
    /*
    * 发起角色选择向导
    */
    getStartRoleWizard: function(accountId)
    {
      // findAll
      x.ui.wizards.getWizard('workflow-startRole', {

        bindOptions: function() { },

        save_callback: function()
        {
          $('#workflow-template-startRoleId').val($('#workflow-startRole-wizardValue').val());
          $('#workflow-template-startRoleName').val($('#workflow-startRole-wizardText').val());
          $('#workflow-template-startRoleText').html($('#workflow-startRole-wizardText').val());

          // 重新获取预计执行人信息
          x.workflow.node.getExpectedActors();

          return 0;
        },

        create: function()
        {
          var outString = '';

          outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:260px; height:auto;" >';

          outString += '<div class="winodw-wizard-toolbar" >';
          outString += '<div class="winodw-wizard-toolbar-close">';
          outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
          outString += '</div>';
          outString += '<div class="float-left">';
          outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>发起角色设置向导</span></div>';
          outString += '<div class="clear"></div>';
          outString += '</div>';
          outString += '<div class="clear"></div>';
          outString += '</div>';

          outString += '<div id="workflow-startRole-wizardTableContainer" style="height:260px;" ></div>';

          outString += '<div class="winodw-wizard-result-container form-inline text-right" >';
          outString += '<label class="winodw-wizard-result-item-text" style="width:40px;" >角色</label> ';
          outString += '<input id="workflow-startRole-wizardText" name="wizardText" type="text" value="' + $('#workflow-template-startRoleName').val() + '" class="winodw-wizard-result-item-input form-control input-sm" style="width:100px;" /> ';
          outString += '<input id="workflow-startRole-wizardValue" name="wizardValue" type="hidden" value="' + $('#workflow-template-startRoleId').val() + '" />';
          outString += '<a href="javascript:' + this.name + '.save();" class="btn btn-default btn-sm" >确定</a>';
          outString += '</div>';

          return outString;
        }
      });

      x.net.xhr('/api/membership.role.findAllByAccountId.aspx?accountId=' + accountId, {
        waitingType: 'mini',
        waitingMessage: i18n.net.waiting.queryTipText,
        callback: function(response)
        {
          var outString = '';

          var counter = 0;

          var result = x.toJSON(response);

          var list = result.data;

          outString += '<table class="table">';
          outString += '<thead>';
          outString += '<tr class="table-row-title">';
          outString += '<th >名称</th>';
          outString += '</tr>';
          outString += '</thead>';
          outString += '<tbody>';
          x.each(list, function(index, node)
          {
            if(node.value !== '')
            {
              classNameValue = (counter % 2 === 0) ? 'table-row-normal' : 'table-row-alternating';

              classNameValue = classNameValue + ((counter + 1) === list.length && (list.length > 8) ? '-transparent' : '');

              outString += '<tr class="' + classNameValue + '">';
              outString += '<td>';
              outString += '<a href="javascript:$(#\'workflow-startRole-wizardValue\').val(\'' + node.id + '\');x.form.query(\'workflow-startRole-wizardText\').val(\'' + node.name + '\'); void(0);" >' + node.name + '</a></li>';

              outString += '</td>';
              outString += '</tr>';

              counter++;
            }
          });

          outString += '</tr>';
          outString += '</tbody>';
          outString += '</table>';

          $('#workflow-startRole-wizardTableContainer')[0].innerHTML = outString;
        }
      });
    }
    /*#endregion*/
  }
}/**
* workflow      : node(分支出口)
*
* require       : x.js, x.net.js, x.workflow.js
*/
x.workflow.node = {

  defaultPolicySetting: '{"setting":{"sendAlertTaskFormat":"","canRating":"0"},"entities":[]}',

  maskWrapper: x.ui.mask.newMaskWrapper('x-workflow-node-maskWrapper', { draggableHeight: 504, draggableWidth: 738 }),

  // 当前节点的索引(编辑状态)
  currentIndex: 0,

  /*#region 函数:attribute(nodeId, name)*/
  /**
  * 获取节点属性
  */
  attribute: function(nodeId, name)
  {
    var target = document.getElementById(nodeId + '_' + name);

    return (target === null) ? '' : target.value;
  },
  /*#endregion*/

  /*#region 函数:nodeIndex(nodeId)*/
  /**
  * 根据节点标识获取节点索引
  */
  nodeIndex: function(nodeId)
  {
    var index = nodeId.indexOf('_');
    return nodeId.substr(index + 1, nodeId.length - index - 1);
  },
  /*#endregion*/

  /*#region 函数:resetNodeId(index, nodeId)*/
  /**
  * 重新设置节点的标识
  */
  resetNodeId: function(index, nodeId)
  {
    var node = $(x.workflow.settings.workflowNodeSelector)[index - 1];

    for(var i = 0;i < node.childNodes[1].childNodes.length;i++)
    {
      var id = node.childNodes[1].childNodes[i].id;

      var tempIndex = id.indexOf('_', 4);

      node.childNodes[1].childNodes[i].id = nodeId + '_' + id.substr(tempIndex + 1, id.length - tempIndex - 1);
    }
  },
  /*#endregion*/

  /*#region 函数:create()*/
  /**
  * 创建节点
  */
  create: function()
  {
    var settings = x.workflow.settings;

    var nodes = $(settings.workflowNodeSelector);

    var nodeName;

    // 节点索引位置
    var index = nodes.length;

    // 节点名称必须不重复
    for(var i = 1;i < 100;i++)
    {
      var isExist = false;

      nodeName = '节点' + i;

      nodes.each(function(nodeIndex, node)
      {
        if(node.childNodes[1].childNodes[2].value === nodeName)
        {
          isExist = true;
        }
      });

      if(!isExist)
      {
        break;
      }
    }

    var outString = '';

    outString += '<tr id="nn_' + index + '" class="table-row-normal workflow-node" >';
    outString += '<td>' + (index > 9 ? '' : '0') + index + '</td>';
    outString += '<td>';
    outString += '<span>' + nodeName + '</span>';
    outString += '<input id="nn_' + index + '_id" type="hidden" value="nn_' + index + '" />';
    outString += '<input id="nn_' + index + '_name" type="hidden" value="' + nodeName + '" />';
    outString += '<input id="nn_' + index + '_type" type="hidden" value="node" />';
    outString += '<input id="nn_' + index + '_actorScope" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_actorDescription" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_actorCounter" type="hidden" value="1" />';
    outString += '<input id="nn_' + index + '_actorMethod" type="hidden" value="并审" />';
    outString += '<input id="nn_' + index + '_handler" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_editor" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_backNodes" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_forwardNodes" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_commissionActors" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_timelimit" type="hidden" value="24" />';
    outString += '<input id="nn_' + index + '_filterActors" type="hidden" value="1" />';
    outString += '<input id="nn_' + index + '_sendAlertTask" type="hidden" value="1" />';
    outString += '<input id="nn_' + index + '_policy" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_remark" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_status" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_positionX" type="hidden" value="0" />';
    outString += '<input id="nn_' + index + '_positionY" type="hidden" value="0" />';
    outString += '<input id="nn_' + index + '_repeatDirection" type="hidden" value="None" />';
    outString += '<input id="nn_' + index + '_zIndex" type="hidden" value="" />';
    outString += '<input id="nn_' + index + '_canEdit" type="hidden" value="1" />';
    outString += '<input id="nn_' + index + '_canMove" type="hidden" value="1" />';
    outString += '<input id="nn_' + index + '_canDelete" type="hidden" value="1" />';
    outString += '<input id="nn_' + index + '_canUploadFile" type="hidden" value="0" />';
    outString += '<input id="nn_' + index + '_radiation" type="hidden" value="0" />';
    outString += '<input id="nn_' + index + '_exits" type="hidden" value="[]" />';
    outString += '</td>';

    outString += '<td>';
    outString += '<span id="nn_' + index + '_actorDescription" ></span>';
    outString += '</td>';

    outString += '<td></td>';
    outString += '<td></td>';
    outString += '<td></td>';
    outString += '<td><a href="#">编辑</a></td>';
    outString += '<td><a href="#">上移</td>';
    outString += '<td><a href="#">下移</td>';
    outString += '<td><a href="#">删除</td>';
    outString += '</tr>';

    if(typeof ($(settings.workflowNodeSelector + ':last')[0]) === 'undefined')
    {
      x.workflow.node.currentIndex = 1;

      $(x.workflow.settings.workflowNodeNewSelector).before(outString);
    }
    else
    {
      index = index + 1;

      x.workflow.node.currentIndex = index;

      $(x.workflow.settings.workflowNodeNewSelector).before(outString);
    }

    x.workflow.node.sync();

    x.workflow.node.edit(index);

    // 如果节点新建后，没按保存按钮，则删除当前节点
    x.workflow.node.maskWrapper.closeEvent = function()
    {
      var nodes = $(settings.workflowNodeSelector);

      nodes.each(function(index, node)
      {
        if(node.childNodes[1].childNodes[4].value === '')
        {
          x.workflow.node.remove(index + 1);
        }
      });
    };
  },
  /*#endregion*/

  /*#region 函数:sync()*/
  /**
  * 同步工作流节点信息，重新分析设置节点数据。
  */
  sync: function()
  {
    var settings = x.workflow.settings;

    var nodes = $(settings.workflowNodeSelector);

    for(var i = 0;i < nodes.length;i++)
    {
      var node = nodes[i];

      var index = i + 1;

      // 设置节点的标识
      if(i === 0)
      {
        node.id = 'sn_0';
      }
      else if(i < nodes.length - 1)
      {
        node.id = 'nn_' + i;
      }
      else
      {
        node.id = 'en_' + i;
      }

      node.childNodes[1].childNodes[1].value = node.id;

      x.workflow.node.resetNodeId(index, node.id);

      // 编号
      node.childNodes[0].innerHTML = (index > 9 ? '' : '0') + index;

      // 设置权限
      if(i === 0)
      {
        // 开始节点
        node.childNodes[1].childNodes[23].value = '0';

        if($(document.getElementById('workflow-administrator')).val() === '1')
        {
          node.childNodes[6].innerHTML = '<a href="javascript:x.workflow.node.edit(' + index + ')" title="编辑"><i class="fa fa-edit"></i></a>';
        }
        else
        {
          node.childNodes[6].innerHTML = '<span class="gray-text" title="编辑" ><i class="fa fa-edit"></i></span>';
        }
        node.childNodes[7].innerHTML = '<span class="gray-text" title="上移" ><i class="fa fa-arrow-up"></i></span>';
        node.childNodes[8].innerHTML = '<span class="gray-text" title="下移" ><i class="fa fa-arrow-down"></i></span>';
        node.childNodes[9].innerHTML = '<span class="gray-text" title="删除" ><i class="fa fa-times"></i></span>';
      }
      else if(($('#workflow-administrator').val() !== '1') && $('#workflow-editableTemplate').val() === '0')
      {
        // 禁止编辑模板
        node.childNodes[6].innerHTML = '<span class="gray-text" title="编辑" ><i class="fa fa-edit"></i></span>';
        node.childNodes[7].innerHTML = '<span class="gray-text" title="上移" ><i class="fa fa-arrow-up"></i></span>';
        node.childNodes[8].innerHTML = '<span class="gray-text" title="下移" ><i class="fa fa-arrow-down"></i></span>';
        node.childNodes[9].innerHTML = '<span class="gray-text" title="删除" ><i class="fa fa-times"></i></span>';
      }
      else if(($(document.getElementById('workflow-administrator')).val() !== '1') && Number(node.childNodes[1].childNodes[18].value) > 0)
      {
        // [不允许编辑]已执行的节点
        node.childNodes[6].innerHTML = '<span class="gray-text" title="编辑" ><i class="fa fa-edit"></i></span>';
        node.childNodes[7].innerHTML = '<span class="gray-text" title="上移" ><i class="fa fa-arrow-up"></i></span>';
        node.childNodes[8].innerHTML = '<span class="gray-text" title="下移" ><i class="fa fa-arrow-down"></i></span>';
        node.childNodes[9].innerHTML = '<span class="gray-text" title="删除" ><i class="fa fa-times"></i></span>';
      }
      else
      {
        // 未执行的节点
        if(($(document.getElementById('workflow-administrator')).val() === '1') || node.childNodes[1].childNodes[23].value === '1')
        {
          node.childNodes[6].innerHTML = '<a href="javascript:x.workflow.node.edit(' + index + ')" title="编辑" ><i class="fa fa-edit"></i></a>';
        }
        else
        {
          node.childNodes[6].innerHTML = '<span class="gray-text" title="编辑" ><i class="fa fa-edit"></i></span>';
        }

        if(($(document.getElementById('workflow-administrator')).val() === '1') || node.childNodes[1].childNodes[24].value === '1')
        {
          node.childNodes[7].innerHTML = '<a href="javascript:x.workflow.node.move(' + index + ',\'up\')" title="上移" ><i class="fa fa-arrow-up"></i></a>';
          node.childNodes[8].innerHTML = '<a href="javascript:x.workflow.node.move(' + index + ',\'down\');" title="下移" ><i class="fa fa-arrow-down"></i></a>';
        }
        else
        {
          node.childNodes[7].innerHTML = '<span class="gray-text" title="上移" ><i class="fa fa-arrow-up"></i></span>';
          node.childNodes[8].innerHTML = '<span class="gray-text" title="下移" ><i class="fa fa-arrow-down"></i></span>';
        }

        if(($(document.getElementById('workflow-administrator')).val() === '1') || node.childNodes[1].childNodes[25].value === '1')
        {
          node.childNodes[9].innerHTML = '<a href="javascript:x.workflow.node.remove(' + index + ');" title="删除" ><i class="fa fa-times"></i></a>';
        }
        else
        {
          node.childNodes[9].innerHTML = '<span class="gray-text" title="删除" ><i class="fa fa-times"></i></span>';
        }
      }
    }

    for(var i = 0;i < nodes.length;i++)
    {
      var node = nodes[i];

      var index = i + 1;

      // 下一步骤
      if(i < nodes.length - 1)
      {
        node.childNodes[3].className = 'vertical-middle';
        node.childNodes[3].innerHTML = '→' + ((index + 1) > 9 ? '' : '0') + (index + 1);

        var exitObjects = node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1];

        if(!(exitObjects.value === '' || exitObjects.value === '[]'))
        {
          var exits = x.toJSON(exitObjects.value);
          var toNodeNames = '';

          x.each(exits, function(exitIndex, exit)
          {
            nodes.each(function(toNodeIndex, toNode)
            {
              if(exit.toNodeId == toNode.childNodes[1].childNodes[1].value)
              {
                toNodeNames += '→' + ((toNodeIndex + 1) > 9 ? '' : '0') + (toNodeIndex + 1) + ' ';
              }
            });
          });

          node.childNodes[3].innerHTML = toNodeNames;
        }
      }

      // 设置最后个节点
      if(i == nodes.length - 1)
      {
        node.childNodes[1].childNodes[node.childNodes[1].childNodes.length - 1].value = '[]';
        node.childNodes[3].innerHTML = '';
      }
    }
  },
  /*#endregion*/

  /*#region 函数:edit(index)*/
  /**
  * 编辑当前节点
  *
  * index : 节点序号
  */
  edit: function(index)
  {
    x.workflow.node.currentIndex = index;

    var settings = x.workflow.settings;

    var node = x.workflow.node.getNodeObject(index);

    var outString = '';

    outString += '<div id="workflow-node-name-' + node.id + '" class="winodw-wizard-wrapper" style="width:720px; height:auto;" >';

    outString += '<div class="winodw-wizard-toolbar" >';
    outString += '<div class="winodw-wizard-toolbar-close">';
    outString += '<a href="javascript:x.workflow.node.maskWrapper.close();" title="关闭" ><i class="fa fa-close"></i></a>';
    outString += '</div>';
    outString += '<div class="float-left">';
    outString += '<div class="winodw-wizard-toolbar-item"><span>编辑节点</span></div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-wrapper" style="height: 360px;">';
    outString += '<div id="workflow-node-tabs-menu-wrapper" class="x-ui-pkg-tabs-menu-wrapper" >';
    outString += '<ul class="x-ui-pkg-tabs-menu nav nav-tabs" >';
    outString += '<li><a href="#workflow-node-tab-1">基本属性</a></li>';
    outString += '<li><a href="#workflow-node-tab-2">高级属性</a></li>';
    outString += '<li><a href="#workflow-node-tab-3">分支设置</a></li>';
    outString += '<li><a href="#workflow-node-tab-4">跳转设置</a></li>';
    outString += '<li><a href="#workflow-node-tab-5">转交设置</a></li>';
    outString += '<li><a href="#workflow-node-tab-6">策略设置</a></li>';
    outString += '</ul>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="workflow-node-tab-1" name="workflow-node-tab-1" >基本属性</a></h2>';
    outString += '<table style="width:100%;">';
    outString += '<tr>';
    outString += '<td class="table-body-text" style="width:120px;">节点名称</td>';
    outString += '<td colspan="3" class="table-body-input">';

    outString += '<input id="workflow-node-id" name="workflow-node-id" type="hidden" value="' + ((typeof (node.id) === 'undefined') ? '' : node.id) + '" />';
    outString += '<input id="workflow-node-policy" name="workflow-node-policy" type="hidden" value="' + ((typeof (node.id) === 'undefined') ? '' : node.policy) + '" />';
    outString += '<input id="workflow-node-backNodes" name="workflow-node-backNodes" type="hidden" value="' + ((typeof (node.backNodes) === 'undefined') ? '' : node.backNodes) + '" />';
    outString += '<input id="workflow-node-forwardNodes" name="workflow-node-forwardNodes" type="hidden" value="' + ((typeof (node.forwardNodes) === 'undefined') ? '' : node.forwardNodes) + '" />';
    outString += '<input id="workflow-node-commissionActors" name="workflow-node-commissionActors" type="hidden" value="' + ((typeof (node.commissionActors) === 'undefined') ? '' : node.commissionActors) + '" />';
    outString += '<input id="workflow-node-name" name="workflow-node-name" type="text" class="form-control" style="width:200px;" value="' + ((typeof (node.name) === 'undefined') ? '' : node.name) + '" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr>';
    outString += '<td class="table-body-text" >节点权限</td>';
    outString += '<td colspan="3" class="table-body-input">';
    outString += '<input id="workflow-node-canEdit" name="workflow-node-canEdit" type="checkbox" ' + ((typeof (node.canEdit) !== 'undefined' && node.canEdit === '1') ? 'checked="checked"' : '') + ' /> 编辑 ';
    outString += '<input id="workflow-node-canMove" name="workflow-node-canMove" type="checkbox" ' + ((typeof (node.canMove) !== 'undefined' && node.canMove === '1') ? 'checked="checked"' : '') + ' /> 移动 ';
    outString += '<input id="workflow-node-canDelete" name="workflow-node-canDelete" type="checkbox" ' + ((typeof (node.canDelete) !== 'undefined' && node.canDelete === '1') ? 'checked="checked"' : '') + ' /> 删除 ';
    outString += '<input id="workflow-node-canUploadFile" name="workflow-node-canUploadFile" type="checkbox" ' + ((typeof (node.canUploadFile) !== 'undefined' && node.canUploadFile === '1') ? 'checked="checked"' : '') + ' /> 上传附件 ';
    outString += '<input id="workflow-node-filterActors" name="workflow-node-filterActors" type="checkbox" ' + ((typeof (node.filterActors) !== 'undefined' && node.filterActors === '1') ? 'checked="checked"' : '') + ' /> 过滤已执行用户 ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr>';
    outString += '<td class="table-body-text">节点执行人</td>';
    outString += '<td colspan="3" class="table-body-input">';
    outString += '<textarea id="workflow-node-actorDescription" name="workflow-node-actorDescription" class="textarea-normal" style="width:520px;cursor:default;" >' + x.isUndefined(node.actorDescription, '') + '</textarea><br />';
    outString += '<input id="workflow-node-actorScope" name="workflow-node-actorScope" type="hidden" value="' + x.isUndefined(node.actorScope, '') + '" />';
    outString += '<input id="node-actorCounter" name="node-actorCounter" type="hidden" value="' + x.isUndefined(node.actorCounter, '') + '" />';
    outString += '<div class="text-right" style="width:520px;" > ';
    outString += '<a href="javascript:x.workflow.node.showActorWizard();" >编辑</a>';
    outString += '</div> ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr>';
    outString += '<td class="table-body-text">备注</td>';
    outString += '<td colspan="3" class="table-body-input">';
    outString += '<textarea id="workflow-node-remark" name="workflow-node-remark" type="text" class="textarea-normal" style="width:520px; height:40px;" >' + (x.isUndefined(node.remark) ? '' : x.encoding.html.encode(node.remark)) + '</textarea>';
    outString += '</tr>';

    outString += '<tr>';
    outString += '<td class="table-body-text">执行方式</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="workflow-node-actorMethod" name="workflow-node-actorMethod" x-dom-feature="combobox" x-dom-combobox-data="' + ((document.getElementById('workflow-nodeActorMethods') === null) ? settings.workflowNodeActorMethods : $('#workflow-nodeActorMethods').val()) + '" x-dom-topOffset="-1" selectedText="' + ((typeof (node.actorMethod) === 'undefined') ? '审核' : node.actorMethod) + '" value="' + ((typeof (node.actorMethod) === 'undefined') ? '并审' : node.actorMethod) + '" class="form-control" style="width:180px" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px;" >处理时限(小时)</td>';
    outString += '<td class="table-body-input"><input id="workflow-node-timelimit" name="workflow-node-timelimit" x-dom-feature="number" type="text" class="form-control" style="width:120px;" value="' + ((typeof (node.timelimit) === 'undefined') ? '24' : node.timelimit) + '" /></td>';
    outString += '</tr>';

    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px;" >提醒方式</td>';
    outString += '<td colspan="3" class="table-body-input">';
    outString += '<div class="vertical-middle" >';
    outString += '<input id="workflow-node-sendAlertTask1" name="workflow-node-sendAlertTask1" type="checkbox" checked="checked" disabled="disabled" /> 待办事宜 ';
    // outString += '<input id="workflow-node-sendAlertTask1" name="workflow-node-sendAlertTask" type="checkbox" checked="checked" disabled="disabled" /> 待办事宜';
    if(typeof (node.sendAlertTask) !== 'undefined' && (node.sendAlertTask === '2' || node.sendAlertTask === '3' || node.sendAlertTask === '7'))
    {
      outString += '<input id="workflow-node-sendAlertTask2" name="workflow-node-sendAlertTask" type="checkbox" checked="checked" /> 邮件 ';
    }
    else
    {
      outString += '<input id="workflow-node-sendAlertTask2" name="workflow-node-sendAlertTask" type="checkbox" /> 邮件 ';
    }
    if(typeof (node.sendAlertTask) !== 'undefined' && (node.sendAlertTask === '4' || node.sendAlertTask === '5' || node.sendAlertTask === '7'))
    {
      outString += '<input id="workflow-node-sendAlertTask4" name="workflow-node-sendAlertTask" type="checkbox" checked="checked" /> 短信 ';
    }
    else
    {
      outString += '<input id="workflow-node-sendAlertTask4" name="workflow-node-sendAlertTask" type="checkbox" /> 短信 ';
    }
    outString += '</div>';
    outString += '</td>';
    outString += '</tr>';

    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="workflow-node-tab-2" name="workflow-node-tab-2">高级属性</a></h2>';

    outString += '<table style="width:100%;">';
    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px" >编辑界面</td>';
    outString += '<td class="table-body-input"><input id="workflow-node-editor" name="workflow-node-editor" type="text" class="form-control" style="width:95%;" value="' + ((typeof (node.editor) === 'undefined') ? '' : node.editor) + '" /></td>';
    outString += '</tr>';

    outString += '<tr >';
    outString += '<td class="table-body-text">处理器</td>';
    outString += '<td class="table-body-input"><input id="workflow-node-handler" name="workflow-node-handler" type="text" class="form-control" style="width:95%;" value="' + ((typeof (node.handler) === 'undefined') ? '' : node.handler) + '" /></td>';
    outString += '</tr>';

    outString += '<tr >';
    outString += '<td class="table-body-text" >自定义待办提醒</td>';
    outString += '<td class="table-body-input vertical-middle">';
    outString += '<input id="workflow-node-policy-settings-sendAlertTaskFormat" name="workflow-node-settings$sendAlertTaskFormat" type="text" value="' + ((typeof (node.policy.setting.sendAlertTaskFormat) === 'undefined') ? '' : node.policy.setting.sendAlertTaskFormat) + '" class="form-control" style="width:360px" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px" >评分功能</td>';
    outString += '<td class="table-body-input vertical-middle">';
    outString += '<input id="workflow-node-policy-settings-canRating" name="workflow-node-settings$canRating" type="checkbox" value="' + ((typeof (node.policy.setting.canRating) === 'undefined') ? '' : node.policy.setting.canRating) + '" ' + (node.policy.setting.canRating === '1' ? 'checked="checked" ' : '') + ' /> 支持评分';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a id="workflow-node-tab-3" name="workflow-node-tab-3">分支设置</a></h2>';

    outString += '<table style="width:100%;">';
    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px" >默认下一步骤</td>';
    outString += '<td class="table-body-input"><span id="workflow-node-defaultSwitcherExit" name="workflow-node-workflow-node-defaultSwitcherExit" ></span></td>';
    outString += '</tr>';
    outString += '<tr >';
    outString += '<td class="table-body-text">发散分支</td>';
    outString += '<td class="table-body-input vertical-middle">';
    //  outString += '<input id="workflow-node-switcher-radiation" name="workflow-node-switcher-radiation" type="checkbox" value="' + ((typeof (node.switcher.radiation) === 'undefined') ? '' : node.switcher.radiation) + '" ' + (node.switcher.radiation === '1' ? 'checked="checked" ' : '') + ' /> ';
    outString += '<input id="workflow-node-switcher-radiation" name="workflow-node-switcher-radiation" type="checkbox" value="' + ((typeof (node.switcher.radiation) === 'undefined') ? '' : node.switcher.radiation) + '" ' + (node.switcher.radiation === '1' ? 'checked="checked" ' : '') + ' disabled="disabled" /> ';
    outString += '<span class="green-text" >允许工作流同时发散到所有分支</span>';
    outString += '</td>';
    outString += '</tr>';
    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px" >分支出口条件</td>';
    outString += '<td class="table-body-input">';
    outString += '<div id="workflow-node-switcher-container" name="workflow-node-switcher-container" style="border:1px solid #ccc; width:460px; height:200px;" >';
    outString += '</div> ';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="workflow-node-tab-4" id="workflow-node-tab-4">跳转设置</a></h2>';

    outString += '<table style="width:100%;">';
    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px" >允许后退的节点</td>';
    outString += '<td class="table-body-input vertical-middle"><div id="workflow-node-backNodes-container" name="workflow-node-backNodes-container" style="border:1px solid #ccc; width:460px; height:80px; padding:4px;" ></div></td>';
    outString += '</tr>';
    outString += '<tr >';
    outString += '</tr>';
    outString += '<tr >';
    outString += '<td class="table-body-text">允许前进的节点</td>';
    outString += '<td class="table-body-input vertical-middle"><div id="workflow-node-forwardNodes-container" name="workflow-node-forwardNodes-container" style="border:1px solid #ccc; width:460px; height:80px; padding:4px;" ></div></td>';
    outString += '</tr>';

    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="workflow-node-tab-5" id="workflow-node-tab-5">转交设置</a></h2>';

    outString += '<table style="width:100%;">';
    outString += '<tr >';
    outString += '<td class="table-body-text" style="width:120px" >允许转交的人员</td>';
    outString += '<td class="table-body-input">';
    outString += '<textarea id="workflow-node-commissionActorDescription" name="workflow-node-commissionActorDescription" class="textarea-normal" style="width:460px;height:80px;cursor:default;" ></textarea><br />';
    outString += '<input id="workflow-node-commissionActors" name="workflow-node-commissionActors" type="hidden" value="' + ((typeof (node.commissionActors) === 'undefined') ? '' : node.commissionActors) + '" />';
    outString += '<div class="text-right" style="width:460px;" > ';
    outString += '<a href="javascript:x.ui.wizards.getContactsWizard({targetViewName:\'workflow-node-commissionActorDescription\',targetValueName:\'workflow-node-commissionActors\', contactTypeText:\'account\'});" >编辑</a>';
    outString += '</div> ';
    outString += '</td>';
    outString += '</tr>';
    outString += '<tr >';
    outString += '<td class="table-body-text">允许任意转交</td>';
    outString += '<td class="table-body-input vertical-middle">';
    outString += '<input id="workflow-node-node-canCommissionEveryone" name="workflow-node-node-canCommissionEveryone" type="checkbox" ' + ((node.commissionActors === 'role#00000000-0000-0000-0000-000000000000#任意用户') ? 'checked="checked"' : '') + '/> ';
    outString += '<span class="green-text" >允许转交给任意用户审批</span>';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="x-ui-pkg-tabs-container" >';
    outString += '<h2 class="x-ui-pkg-tabs-container-head-hidden"><a name="workflow-node-tab-6" id="workflow-node-tab-6">业务策略</a></h2>';

    var template = x.workflow.template.getTemplateObject();

    if(template.entityMetaData === '')
    {
      outString += '<div style="color:#a15300; background-color:#fcffca; padding: 0 0 0 10px;  border-bottom:1px solid #c75400; line-height:200%; font-size:12px;" >';
      outString += '(*)未设置流程的业务对象属性。';
      outString += '</div>';
    }
    else
    {
      var metaData = template.entityMetaData.split(',');

      outString += '<div id="workflow-node-policy-container" name="workflow-node-policy-container" style="height:200px;" >';
      outString += '<table class="table-style" style="width:100%;" >';
      outString += '<tr class="table-row-title">';
      outString += '<td>字段</td>';
      outString += '<td style="width:60px;">编辑</td>';
      outString += '<td style="width:60px;">痕迹</td>';
      outString += '<td style="width:60px;">隐藏</td>';
      outString += '</tr>';

      for(var i = 0;i < metaData.length;i++)
      {
        outString += '<tr class="table-row-normal workflow-node-policy-entity-metaData" >';
        outString += '<td>' + metaData[i] + '</td>';
        outString += '<td><input id="workflow-node-entity-' + metaData[i] + '$canEdit" type="checkbox" /></td>';
        outString += '<td><input id="workflow-node-entity-' + metaData[i] + '$canTrace"  type="checkbox" /></td>';
        outString += '<td><input id="workflow-node-entity-' + metaData[i] + '$canHide"  type="checkbox" /></td>';
        outString += '</tr>';
      }

      outString += '</table>';
      outString += '</div>';
    }

    outString += '</div>';

    outString += '</div>';

    outString += '<div style="border-top:1px solid #ccc;padding:10px 20px 10px 0;" >';
    outString += '<div class="float-right button-2font-wrapper" ><a id="btnCancel" href="javascript:void(0);" class="btn btn-default" >取消</a></div> ';
    outString += '<div class="float-right button-2font-wrapper" style="margin-right:10px;" ><a id="btnOkey" href="javascript:void(0);" class="btn btn-default" >保存</a></div> ';
    outString += '<div class="clear"></div>';
    outString += '</div>';

    outString += '</div>';

    // 加载遮罩和页面内容
    x.ui.mask.getWindow({ content: outString }, x.workflow.node.maskWrapper);

    $('#workflow-node-actorDescription').on('focus', function()
    {
      this.blur();
    });

    x.workflow.node.html = outString;

    // 设置分支条件
    x.workflow.node.setSwitcherExit(index, node.switcher.exits);

    // 设置跳转信息
    x.workflow.node.setGotoNodeView(index, 'back', node.backNodes);

    x.workflow.node.setGotoNodeView(index, 'forward', node.forwardNodes);

    // 设置转交人描述信息
    x.ui.wizards.setContactsView('workflow-node-commissionActorDescription', node.commissionActors);

    // 设置业务策略
    $(node.policy.entities).each(function(index, entity)
    {
      if($('#workflow-node-entity-' + entity.metaData + '-canEdit').size() > 0)
      {
        $('#workflow-node-entity-' + entity.metaData + '-canEdit')[0].checked = (entity.canEdit === '1' ? true : false);
        $('#workflow-node-entity-' + entity.metaData + '-canTrace')[0].checked = (entity.canTrace === '1' ? true : false);
        $('#workflow-node-entity-' + entity.metaData + '-canHide')[0].checked = (entity.canHide === '1' ? true : false);
      }
    });

    $(document.getElementById('workflow-node-node-canCommissionEveryone')).bind('click', function()
    {
      if(document.getElementById('workflow-node-node-canCommissionEveryone').checked)
      {
        document.getElementById('workflow-node-commissionActors').value = 'role#00000000-0000-0000-0000-000000000000#任意用户';
        document.getElementById('workflow-node-commissionActorDescription').value = '任意用户';
      }
      else
      {
        document.getElementById('workflow-node-commissionActors').value = '';
        document.getElementById('workflow-node-commissionActorDescription').value = '';
      }
    });

    $('#btnOkey').bind('click', function()
    {
      var node = x.workflow.node.getEditNodeObject();

      if(typeof (node) !== 'undefined')
      {
        x.workflow.node.setNodeObject(node);

        $(document.getElementById('workflow-node-actorCounter')).val(node.actorCounter);

        x.workflow.node.maskWrapper.close();

        x.workflow.node.getExpectedActors(node.id);
      }
    });

    $('#btnCancel').bind('click', function()
    {
      x.workflow.node.maskWrapper.close();
    });

    x.page.goTop();

    x.dom.features.bind();

    x.ui.pkg.tabs.newTabs();
  },
  /*#endregion*/

  /*#region 函数:move(index, direction)*/
  /**
  * 移动节点
  * index : 节点序号
  * direction : 'up' | 'down'
  */
  move: function(index, direction)
  {
    index = index - 1;

    var fromRow = $('#nn_' + index)[0];

    var toRow;

    // 最后节点的处理
    if(typeof (fromRow) === 'undefined')
    {
      fromRow = $('#en_' + index)[0];

      if(typeof (fromRow) === 'undefined')
      {
        return;
      }
      else if(direction === 'down')
      {
        alert('节点【' + fromRow.childNodes[1].childNodes[2].value + '】为结束节点，禁止向下移动。');
        return;
      }
    }

    var html = '<tr class="' + fromRow.className + '" style="' + fromRow.style.cssText + '">' + fromRow.innerHTML + '</tr>';

    if(direction === 'up')
    {
      toRow = $('#sn_' + (index - 1))[0];

      if(typeof (toRow) !== 'undefined')
      {
        alert('节点【' + toRow.childNodes[1].childNodes[2].value + '】为开始节点，禁止移动。');
        return;
      }

      toRow = $('#nn_' + (index - 1))[0];

      if(typeof (toRow) !== 'undefined')
      {
        //目标节点 禁止移动
        /*if(($(document.getElementById('workflow-administrator')).val() !== '1') && Number(toRow.childNodes[1].childNodes[24].value) === 0)
        {
        return;
        }*/

        if(Number(toRow.childNodes[1].childNodes[18].value) === 0)
        {
          html = '<tr class="' + toRow.className + '" style="' + toRow.style.cssText + '">' + fromRow.innerHTML + '</tr>';

          $(toRow).before(html);

          $(toRow)[0].className = $(fromRow)[0].className;

          $(fromRow).remove();
        }
        else
        {
          alert('节点【' + toRow.childNodes[1].childNodes[2].value + '】已执行完成，禁止移动。');
        }
      }
    }
    else if(direction === 'down')
    {
      toRow = $('#nn_' + (index + 1))[0];

      if(typeof (toRow) === 'undefined')
      {
        toRow = $('#en_' + (index + 1))[0];
      }

      if(typeof (fromRow) === 'undefined' || typeof (toRow) === 'undefined')
      {
        return;
      }

      // 目标行
      if(toRow)
      {
        /*if(($(document.getElementById('workflow-administrator')).val() !== '1') && Number(toRow.childNodes[1].childNodes[24].value) === 0)
        {
        return;
        }*/

        if(Number(toRow.childNodes[1].childNodes[18].value) === 0)
        {
          html = '<tr class="' + toRow.className + '" style="' + toRow.style.cssText + '">' + fromRow.innerHTML + '</tr>';

          $(toRow).after(html);

          $(toRow)[0].className = $(fromRow)[0].className;

          $(fromRow).remove();
        }
        else
        {
          alert('节点【' + toRow.childNodes[1].childNodes[2].value + '】已执行完成，禁止移动。');
        }
      }
    }

    x.workflow.node.sync();
  },
  /*#endregion*/

  /*#region 函数:remove(index)*/
  /**
  * 移除当前节点
  * index : 节点序号
  */
  remove: function(index)
  {
    // -------------------------------------------------------
    // 节点前缀说明
    // sn_ : 开始节点
    // nn_ : 普通节点
    // en_ : 结束节点
    // -------------------------------------------------------

    var settings = x.workflow.settings;

    var target = $('#nn_' + (index - 1))[0];

    target = (target) ? target : $('#en_' + (index - 1))[0];

    if(target)
    {
      var targetNodeId = target.childNodes[1].childNodes[1].value;

      var nodeIndex = x.workflow.node.nodeIndex(targetNodeId);

      x.workflow.node.currentIndex = 0;

      // 移除元素
      $(target).remove();

      // 移除分支条件中相关的节点信息
      var nodes = $(settings.workflowNodeSelector);

      for(var i = 0;i < nodes.length;i++)
      {
        var text = nodes[i].childNodes[1].childNodes[nodes[i].childNodes[1].childNodes.length - 1].value;

        if(text !== '')
        {
          var list = x.toJSON(text);

          text = '[';

          for(var j = 0;j < list.length;j++)
          {
            if(list[j].toNodeId !== targetNodeId)
            {
              var toNodeIndex = x.workflow.node.nodeIndex(list[j].toNodeId);

              text += '{';

              // 被删除节点之前的节点Id不变
              if(nodeIndex > toNodeIndex)
              {
                text += '"toNodeId": "' + list[j].toNodeId + '",';
              }
              else
              {
                text += '"toNodeId": "' + list[j].toNodeId.replace(('_' + toNodeIndex), '_' + (toNodeIndex - 1)) + '",';
              }
              text += '"friendlyCondition": "' + list[j].friendlyCondition + '",';
              text += '"condition":' + x.workflow.switcherExit.toSwitcherExitConditionText(list[j]);
              text += '},';
            }
          }

          text = x.string.rtrim(text, ',');

          text += ']';

          nodes[i].childNodes[1].childNodes[nodes[i].childNodes[1].childNodes.length - 1].value = text;
        }
      }
    }

    x.workflow.node.sync();
  },
  /*#endregion*/

  /*#region 函数:setSwitcherExit(index)*/
  /**
  * index
  */
  setSwitcherExit: function(index, exits)
  {
    var outString = '';

    var nodes = $(x.workflow.settings.workflowNodeSelector);

    // 设置默认出口
    if(typeof (nodes[index]) !== 'undefined')
    {
      var text = '<span class="blod-text" >' + nodes[index].childNodes[1].childNodes[2].value + '</span> ';

      $(document.getElementById('workflow-node-defaultSwitcherExit')).html(text);
    }

    // 设置出口条件
    outString += '<table class="table-style" style="width:100%;" >';
    outString += '<tr class="table-row-title" >';
    outString += '<td style="width:80px;">出口名称</td>';
    outString += '<td >出口条件</td>';
    outString += '<td style="width:60px;">编辑</td>';
    outString += '<td style="width:60px;">启用</td>';
    outString += '</tr>';

    for(var i = 0;i < nodes.length;i++)
    {
      if(i > (index - 1))
      {
        var target = null;

        x.each(exits, function(exitIndex, exit)
        {
          if(nodes[i].childNodes[1].childNodes[1].value == exit.toNodeId)
          {
            target = exit;
          }
        });
        // nodes[i].childNodes[1].childNodes[1].value
        outString += '<tr class="table-row-normal" >';

        if(i == index)
        {
          outString += '<td>';
          outString += '<span>' + nodes[i].childNodes[1].childNodes[2].value + '</span>';
          outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-toNodeId" type="hidden" value="' + nodes[i].childNodes[1].childNodes[1].value + '" />';
          outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-condition" type="hidden" value="" />';
          outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-friendlyCondition" type="hidden" value="" />';
          outString += '</td>';
          outString += '<td></td>';
          outString += '<td><span class="gray-text">编辑</span></td>';
          outString += '<td>';
          outString += '<span class="green-text">默认</span>';
          outString += '<div class="hidden" ><input id="workflow-node-switcher-exit' + (i + 1) + '" name="workflow-node-switcher-exit" type="checkbox" value="' + (i + 1) + '" checked="checked" /></div>';
          outString += '</td>';
        }
        else
        {
          outString += '<td>';
          outString += '<span>' + nodes[i].childNodes[1].childNodes[2].value + '</span>';
          outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-toNodeId" type="hidden" value="' + nodes[i].childNodes[1].childNodes[1].value + '" />';
          outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-condition" type="hidden" value="' + (target === null ? '[]' : x.encoding.html.encode(x.workflow.switcherExit.toSwitcherExitConditionText(target))) + '" />';
          // outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-condition" type="hidden" value="' + x.encoding.html.encode('"}]') + '" />';
          // x.debug.log(target.friendlyCondition);

          outString += '<input id="workflow-node-switcher-exit' + (i + 1) + '-friendlyCondition" type="hidden" value="' + (target === null ? '' : target.friendlyCondition) + '" />';
          outString += '</td>';
          outString += '<td><span id="workflow-node-switcher-exit' + (i + 1) + '-friendlyConditionText" >' + (target === null ? '' : target.friendlyCondition) + '</span></td>';
          outString += '<td><a href="javascript:x.workflow.switcherExit.edit(' + (i + 1) + ');" >编辑</a></td>';
          outString += '<td><input id="workflow-node-switcher-exit' + (i + 1) + '" name="workflow-node-switcher-exit" disabled="disabled" type="checkbox" value="' + (i + 1) + '" ' + (target === null ? '' : 'checked="checked" ') + '/></td>';
        }
        outString += '</tr>';
      }
    }
    outString += '</table>';

    $('#workflow-node-switcher-container').html(outString);
  },
  /*#endregion*/

  /*#region 函数:setGotoNodeView(direction, selectedNodes)*/
  /**
  * direction 移除当前节点
  * selectedNodes : 选择的节点信息
  */
  setGotoNodeView: function(index, direction, selectedNodes)
  {
    var outString = '';

    var nodes = $(x.workflow.settings.workflowNodeSelector);

    for(var i = 0;i < nodes.length;i++)
    {
      if((direction === 'back' && i < (index - 1)) || (direction === 'forward' && i > (index - 1)))
      {
        outString += '<input id="workflow-node-' + direction + 'Nodes-' + index + '" '
                + 'name="workflow-node-' + direction + 'Nodes" '
                + 'type="checkbox" '
                + 'value="' + nodes[i].childNodes[1].childNodes[2].value + '" '
                + (selectedNodes.indexOf(nodes[i].childNodes[1].childNodes[2].value) == -1 ? '' : 'checked="checked" ') + ' > '
                + '<span>' + nodes[i].childNodes[1].childNodes[2].value + '</span> ';
      }
    }

    $('#workflow-node-' + direction + 'Nodes-container').html(outString);
  },
  /*#endregion*/

  /*#region 函数:getEditNodeObject()*/
  /**
  * 获取当前编辑节点
  */
  getEditNodeObject: function()
  {
    var nodeId = $(document.getElementById('workflow-node-id')).val();

    var nodeName = $(document.getElementById('workflow-node-name')).val();

    if(nodeName === '')
    {
      alert('【节点名称】不能为空。');
      return;
    }

    var nodes = $(x.workflow.settings.workflowNodeSelector);

    for(i = 0;i < nodes.length;i++)
    {
      if(nodes[i].childNodes[1].childNodes[1].value !== nodeId && nodes[i].childNodes[1].childNodes[2].value === nodeName)
      {
        alert('节点名称【' + nodeName + '】已存在。');
        return;
      }
    }

    // 设置工作流执行方式
    var actorMethod = $(document.getElementById('workflow-node-actorMethod')).val();

    var actorCounter = 0;

    if(actorMethod === "会审" || actorMethod === "会签" || actorMethod === "审核" || actorMethod === "审批" || actorMethod === "批准")
    {
      actorCounter = 0;
    }
    else if(actorMethod === "并审" || actorMethod === "校稿")
    {
      actorCounter = 1;
    }
    else if(actorMethod === "抄送" || actorMethod === "传阅")
    {
      actorCounter = -1;
    }
    else
    {
      //传阅 自动跳过
      actorCounter = -1;
    }

    // 设置工作流通知方式
    var sendAlertTask = 0;

    // 通知方式: 1 待办事宜 2 邮件 4 短信
    var sendAlertTaskTypes = [1, 2, 4];

    for(var i = 0;i < sendAlertTaskTypes.length;i++)
    {
      var sendAlertTaskObejct = $(document.getElementById('workflow-node-sendAlertTask' + sendAlertTaskTypes[i]));

      if(typeof (sendAlertTaskObejct[0]) !== 'undefined' && sendAlertTaskObejct[0].checked)
      {
        sendAlertTask = sendAlertTask + sendAlertTaskTypes[i];
      }
    }

    // 设置节点策略
    var policy = '{';
    policy += '"setting":{';
    policy += '"sendAlertTaskFormat":"' + $('#workflow-node-policy-settings-sendAlertTaskFormat').val() + '",';
    policy += '"canRating":"' + ($('#workflow-node-policy-settings-canRating')[0].checked ? 1 : 0) + '"},';
    policy += '"entities":[';

    var metaData = $('.workflow-node-policy-entity-metaData');

    for(var i = 0;i < metaData.size() ;i++)
    {
      var metaDataName = metaData[i].childNodes[0].innerHTML;

      policy += '{';
      policy += '"metaData":"' + metaDataName + '",';
      policy += '"canEdit":"' + ($('#workflow-node-entity-' + metaDataName + '-canEdit')[0].checked ? 1 : 0) + '",';
      policy += '"canTrace":"' + ($('#workflow-node-entity-' + metaDataName + '-canTrace')[0].checked ? 1 : 0) + '",';
      policy += '"canHide":"' + ($('#workflow-node-entity-' + metaDataName + '-canHide')[0].checked ? 1 : 0) + '"';
      policy += '}' + ((i + 1) == metaData.size() ? '' : ',');
    }

    policy += ']';
    policy += '}';

    // 
    // workflow-node-policy-entity-item
    // workflow-node-policy-settings-sendAlertTaskFormat
    // workflow-node-policy-settings-canRating
    // workflow-node-policy-entity
    // 设置节点分支出口
    var exits = '[';

    var exitObjects = document.getElementsByName('workflow-node-switcher-exit');

    for(var i = 0;i < exitObjects.length;i++)
    {
      if(document.getElementById('workflow-node-switcher-exit' + exitObjects[i].value + '-condition').value === '')
      {
        document.getElementById('workflow-node-switcher-exit' + exitObjects[i].value + '-condition').value = '[]';
      }

      if(exitObjects[i].checked)
      {
        exits += '{';
        exits += '"toNodeId":"' + document.getElementById('workflow-node-switcher-exit' + exitObjects[i].value + '-toNodeId').value + '", ';
        exits += '"condition":' + document.getElementById('workflow-node-switcher-exit' + exitObjects[i].value + '-condition').value + ', ';
        exits += '"friendlyCondition":"' + document.getElementById('workflow-node-switcher-exit' + exitObjects[i].value + '-friendlyCondition').value + '" ';
        exits += '},';
      }
    }

    if(exits.substr(exits.length - 1, 1) === ',')
    {
      exits = exits.substr(0, exits.length - 1);
    }

    exits += ']';

    // -------------------------------------------------------
    // 输出结果
    // -------------------------------------------------------

    var outString = '';

    outString += '{';
    outString += '"id":"' + $('#workflow-node-id').val() + '", ';
    outString += '"name":"' + $(document.getElementById('workflow-node-name')).val() + '", ';
    outString += '"actorScope":"' + $(document.getElementById('workflow-node-actorScope')).val() + '", ';
    outString += '"actorDescription":"' + $(document.getElementById('workflow-node-actorDescription')).val() + '", ';
    outString += '"actorCounter":"' + actorCounter + '", ';
    outString += '"actorMethod":"' + $(document.getElementById('workflow-node-actorMethod')).val() + '", ';
    outString += '"handler":"' + $(document.getElementById('workflow-node-handler')).val() + '", ';
    outString += '"editor":"' + $(document.getElementById('workflow-node-editor')).val() + '", ';
    outString += '"backNodes":"' + x.dom.util.checkbox.getValue('workflow-node-backNodes') + '", ';
    outString += '"forwardNodes":"' + x.dom.util.checkbox.getValue('workflow-node-forwardNodes') + '", ';
    outString += '"commissionActors":"' + $(document.getElementById('workflow-node-commissionActors')).val() + '", ';
    outString += '"timelimit":"' + $(document.getElementById('workflow-node-timelimit')).val() + '", ';
    outString += '"filterActors":"' + ($(document.getElementById('workflow-node-filterActors'))[0].checked ? '1' : '0') + '", ';
    outString += '"sendAlertTask":"' + sendAlertTask + '", ';
    outString += '"policy":' + policy + ', ';
    outString += '"remark":"' + x.encoding.html.encode($('#workflow-node-remark').val()) + '", ';
    outString += '"canEdit":"' + ($(document.getElementById('workflow-node-canEdit'))[0].checked ? '1' : '0') + '", ';
    outString += '"canMove":"' + ($(document.getElementById('workflow-node-canMove'))[0].checked ? '1' : '0') + '", ';
    outString += '"canDelete":"' + ($(document.getElementById('workflow-node-canDelete'))[0].checked ? '1' : '0') + '", ';
    outString += '"canUploadFile":"' + ($(document.getElementById('workflow-node-canUploadFile'))[0].checked ? '1' : '0') + '", ';
    outString += '"switcher":{';
    outString += '"radiation":"' + ($(document.getElementById('workflow-node-switcher-radiation'))[0].checked ? '1' : '0') + '",';
    outString += '"exits":' + exits + '}';
    outString += '}';

    // x.debug.log(outString);

    return x.toJSON(outString);
  },
  /*#endregion*/

  /*#region 函数:getNodeObject(index)*/
  /**
  * 获取当前节点信息.
  *
  * index : 节点标识
  */
  getNodeObject: function(index)
  {
    var nodeIndex = (typeof (index) === 'undefined') ? x.workflow.node.currentIndex : index;

    var nodes = $(x.workflow.settings.workflowNodeSelector);

    var row = nodes[(Number(nodeIndex) - 1)];

    var actorScope = row.childNodes[1].childNodes[4].value === '' ? 'initiator' : row.childNodes[1].childNodes[4].value;
    var actorDescription = row.childNodes[1].childNodes[5].value === '' ? '流程发起人' : row.childNodes[1].childNodes[5].value;
    var actorCounter = row.childNodes[1].childNodes[6].value === '' ? '1' : row.childNodes[1].childNodes[6].value;
    var actorMethod = row.childNodes[1].childNodes[7].value === '' ? '会审' : row.childNodes[1].childNodes[7].value;

    var editor = row.childNodes[1].childNodes[8].value === '' ? '/workflowplus/console/workflow_running.aspx' : row.childNodes[1].childNodes[8].value;
    var handler = row.childNodes[1].childNodes[9].value === '' ? 'X3Platform.WorkflowPlus.Business.General.Transact' : row.childNodes[1].childNodes[9].value;

    // 设置节点默认策略
    if(row.childNodes[1].childNodes[16].value === ''
        || row.childNodes[1].childNodes[16].value === '####'
        || row.childNodes[1].childNodes[16].value === '1')
    {
      row.childNodes[1].childNodes[16].value = x.workflow.node.defaultPolicySetting;
    }

    // -------------------------------------------------------
    // 输出结果
    // -------------------------------------------------------

    var outString = '';

    outString += '{';
    outString += '"id":"' + row.childNodes[1].childNodes[1].value + '", ';
    outString += '"name":"' + row.childNodes[1].childNodes[2].value + '", ';
    outString += '"editor":"' + editor + '", ';
    outString += '"handler":"' + handler + '", ';
    outString += '"actorScope":"' + actorScope + '", ';
    outString += '"actorDescription":"' + actorDescription + '", ';
    outString += '"actorCounter":"' + actorCounter + '", ';
    outString += '"actorMethod":"' + actorMethod + '", ';
    outString += '"canBack":"' + (row.childNodes[1].childNodes[10].value === '' ? 0 : 1) + '", ';
    outString += '"backNodes":"' + row.childNodes[1].childNodes[10].value + '", ';
    outString += '"canForward":"' + (row.childNodes[1].childNodes[11].value === '' ? 0 : 1) + '", ';
    outString += '"forwardNodes":"' + row.childNodes[1].childNodes[11].value + '", ';
    outString += '"canCommission":"' + (row.childNodes[1].childNodes[12].value === '' ? 0 : 1) + '", ';
    outString += '"commissionActors":"' + row.childNodes[1].childNodes[12].value + '", ';
    outString += '"timelimit":"' + row.childNodes[1].childNodes[13].value + '", ';
    outString += '"filterActors":"' + row.childNodes[1].childNodes[14].value + '", ';
    outString += '"sendAlertTask":"' + row.childNodes[1].childNodes[15].value + '", ';
    outString += '"policy":' + row.childNodes[1].childNodes[16].value + ', ';
    outString += '"remark":"' + x.encoding.html.encode(row.childNodes[1].childNodes[17].value) + '", ';
    outString += '"canEdit":"' + row.childNodes[1].childNodes[23].value + '", ';
    outString += '"canMove":"' + row.childNodes[1].childNodes[24].value + '", ';
    outString += '"canDelete":"' + row.childNodes[1].childNodes[25].value + '", ';
    outString += '"canUploadFile":"' + row.childNodes[1].childNodes[26].value + '", ';
    outString += '"switcher":{';
    outString += '"radiation":"' + row.childNodes[1].childNodes[row.childNodes[1].childNodes.length - 2].value + '",';
    outString += '"exits":' + row.childNodes[1].childNodes[row.childNodes[1].childNodes.length - 1].value + '}';
    outString += '}';

    return x.toJSON(outString);
  },
  /*#endregion*/

  /*#region 函数:setNodeObject(node, index)*/
  /**
  * 设置当前节点信息.
  *
  * index : 节点标识
  */
  setNodeObject: function(node, index)
  {
    var settings = x.workflow.settings;

    var nodeIndex = (typeof (index) === 'undefined') ? x.workflow.node.currentIndex : index;

    var nodes = $(settings.workflowNodeSelector);

    var row = nodes[(Number(nodeIndex) - 1)];

    row.childNodes[1].childNodes[0].innerHTML = node.name;
    row.childNodes[1].childNodes[1].value = node.id;
    row.childNodes[1].childNodes[2].value = node.name;
    // row.childNodes[1].childNodes[3].value = node.type;
    row.childNodes[1].childNodes[4].value = node.actorScope;
    row.childNodes[1].childNodes[5].value = node.actorDescription;
    row.childNodes[1].childNodes[6].value = node.actorCounter;
    row.childNodes[1].childNodes[7].value = node.actorMethod;
    row.childNodes[1].childNodes[8].value = node.editor;
    row.childNodes[1].childNodes[9].value = node.handler;
    row.childNodes[1].childNodes[10].value = node.backNodes;
    row.childNodes[1].childNodes[11].value = node.forwardNodes;
    row.childNodes[1].childNodes[12].value = node.commissionActors;
    row.childNodes[1].childNodes[13].value = node.timelimit;
    row.childNodes[1].childNodes[14].value = node.filterActors;
    row.childNodes[1].childNodes[15].value = node.sendAlertTask;
    row.childNodes[1].childNodes[16].value = JSON.stringify(node.policy);
    row.childNodes[1].childNodes[17].value = node.remark;
    row.childNodes[1].childNodes[23].value = node.canEdit;
    row.childNodes[1].childNodes[24].value = node.canMove;
    row.childNodes[1].childNodes[25].value = node.canDelete;
    row.childNodes[1].childNodes[26].value = node.canUploadFile;

    var exits = '[';

    // 设置分支条件
    if(typeof (node.switcher.exits) !== 'undefined' && node.switcher.exits.length > 1)
    {
      x.each(node.switcher.exits, function(index, exit)
      {
        exits += '{';
        exits += '"toNodeId":"' + exit.toNodeId + '", ';
        exits += '"friendlyCondition":"' + exit.friendlyCondition + '", ';
        exits += '"condition":' + x.workflow.switcherExit.toSwitcherExitConditionText(exit) + ' ';
        exits += '},';
      });

      if(exits.substr(exits.length - 1, 1) === ',')
      {
        exits = exits.substr(0, exits.length - 1);
      }
    }

    exits += ']';

    row.childNodes[1].childNodes[row.childNodes[1].childNodes.length - 2].value = node.switcher.radiation;
    row.childNodes[1].childNodes[row.childNodes[1].childNodes.length - 1].value = exits;

    // 设置执行人描述信息
    row.childNodes[2].childNodes[0].innerHTML = node.actorDescription;

    if(typeof (row.childNodes[2].childNodes[1]) !== 'undefined')
    {
      row.childNodes[2].childNodes[1].innerHTML = '';
    }

    row.childNodes[4].innerHTML = node.actorMethod;

    row.childNodes[5].innerHTML = node.timelimit;

    x.workflow.node.sync();
  },
  /*#endregion*/

  /*#region 函数:setActorScopeByNodeId(workflowInstanceId, workflowNodeId, actorScope)*/
  /**
  * 运行时修改预定义执行人范围
  *
  * flowInstanceId: 运行时的工作流的id.
  * nodeFlag: 运行时的节点的flag.
  */
  setActorScopeByNodeId: function(options)
  {
    if(confirm('是否重新设置这个节点的处理者?'))
    {
      var url = "/api/workflow.node.setActorScope.aspx";

      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += '<workflowInstanceId><![CDATA[' + options.workflowInstanceId + ']]></workflowInstanceId>';
      outString += '<workflowNodeId><![CDATA[' + options.workflowNodeId + ']]></workflowNodeId>';
      outString += '<actorScope><![CDATA[' + options.actorScope + ']]></actorScope>';
      outString += '</request>';

      $.post(url, { resultType: 'json', xml: outString }, function(response)
      {
        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
          case 0:
            if(typeof (options.callback) !== 'undefined')
            {
              options.callback();
            }
            break;

          case -1:
          case 1:
            alert(result.value);
            break;

          default:
            break;
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:setActiveNode(options)*/
  /**
  * 运行时强制设置一个节点为当前节点
  */
  setActiveNode: function(options)
  {
    if(confirm('是否强制将当前的节点设置为活动节点?'))
    {
      var url = "/api/workflow.node.setActiveNode.aspx";

      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += '<nodeId>' + options.nodeId + '</nodeId>';
      outString += '</request>';

      $.post(url, { resultType: 'json', xml: outString }, function(response)
      {
        var result = x.toJSON(response).message;

        switch(Number(result.returnCode))
        {
          case 0:
            if(typeof (options.callback) !== 'undefined')
            {
              options.callback();
            }
            break;

          case -1:
          case 1:
            alert(result.value);
            break;

          default:
            break;
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:showActorWizard()*/
  /**
  * 执行人选择向导
  */
  showActorWizard: function()
  {
    x.workflow.node.actionMethod = 'cancel';

    // 由于多个节点共用一个人员选择向导
    // 所以需要每次清空人员数据重新初始化向导
    var name = x.getFriendlyName(location.pathname + '-workflow-node-actorDescription-workflow-node-actorScope-contacts-wizard');

    window[name] = undefined;

    // 人员选择向导的人员选择范围
    var contactTypeText = document.getElementById('workflow-node-contactTypeText') === null ? 'all' : $('#workflow-node-contactTypeText').val();

    x.ui.wizards.getContactsWizard({
      targetViewName: 'workflow-node-actorDescription',
      targetValueName: 'workflow-node-actorScope',
      contactTypeText: contactTypeText,
      multiSelection: 1,
      includeProhibited: 0,
      save_callback: function(response)
      {
        x.workflow.node.actionMethod = 'save';

        // x.workflow.node.edit(x.workflow.node.currentIndex);

        var resultView = '';
        var resultValue = '';

        var list = x.toJSON(response).list;

        x.each(list, function(index, node)
        {
          resultView += node.text + ';';
          resultValue += node.value + ';';
        });

        if(resultValue.substr(resultValue.length - 1, 1) === ';')
        {
          resultView = resultView.substr(0, resultView.length - 1);
          resultValue = resultValue.substr(0, resultValue.length - 1);
        }

        $('#workflow-node-actorDescription').val(resultView);
        $('#workflow-node-actorScope').val(resultValue);
      },
      cancel_callback: function(response)
      {
        if(x.workflow.node.actionMethod === 'cancel')
        {
          // x.workflow.node.edit(x.workflow.node.currentIndex);
        }
      }
    });
  },
  /*#endregion*/

  /*#region 函数:getExpectedActors()*/
  /**
  * 获取预计执行人信息
  */
  getExpectedActors: function()
  {
    var outString = '';

    var settings = x.workflow.settings;

    var nodes = $(settings.workflowNodeSelector);

    outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    // 所属公司信息
    outString += '<corporationId><![CDATA[' + $('#workflow-template-corporationId').val() + ']]></corporationId>';
    // 所属项目信息
    outString += '<projectId><![CDATA[' + $('#workflow-template-projectId').val() + ']]></projectId>';
    // 发起人信息
    outString += '<startActorId><![CDATA[' + $('#workflow-template-startActorId').val() + ']]></startActorId>';
    // 发起角色信息
    outString += '<startRoleId><![CDATA[' + $('#workflow-template-startRoleId').val() + ']]></startRoleId>';
    // 节点执行人范围
    for(var i = 0;i < nodes.length;i++)
    {
      // 如果传入单个参数，则只获取某一个节点的预计执行人
      if(arguments.length > 0 && arguments[0] != nodes[i].childNodes[1].childNodes[1].value) { continue; }

      outString += '<node>';
      outString += '<id><![CDATA[' + nodes[i].childNodes[1].childNodes[1].value + ']]></id>';
      outString += '<actorScope><![CDATA[' + nodes[i].childNodes[1].childNodes[4].value + ']]></actorScope>';
      outString += '</node>';
    }

    outString += '</request>';

    x.net.xhr('/api/workflow.client.getExpectedActors.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var list = x.toJSON(response).data;

        x.each(list, function(index, node)
        {
          if(node.expectedActors === '')
          {
            $('#' + node.id + '_expectedActors')
                .attr('class', 'x-workflow-node-expected-actors-notfound')
                .html('(<label>未找到预计执行人.</label>)');
          }
          else
          {
            $('#' + node.id + '_expectedActors')
                .attr('class', 'x-workflow-node-expected-actors')
                .html('(<label><strong>预计执行人:</strong>' + node.expectedActors + '</label>)');
          }
        });
      }
    });
  }
  /*#endregion*/
};

/**
* checkbox 元素相关的操作函数
*/
x.dom.util.checkbox = {

  /**
  * 获取checkbox元素的值.
  */
  getValue: function(checkboxName)
  {
    var result = '';

    var list = document.getElementsByName(checkboxName);

    for(var i = 0;i < list.length;i++)
    {
      if(list[i].checked)
      {
        result += (result == '' ? '' : ',') + list[i].value;
      }
    }

    return result.trim(',');
  },

  /**
  * 设置checkbox元素的值.
  */
  setValue: function(checkboxName, value)
  {
    var list = document.getElementsByName(checkboxName);

    for(var i = 0;i < list.length;i++)
    {
      if(list[i].value == value)
      {
        list[i].checked = true;
        x.dom.checkbox.setCheckboxViewValue(list[i].id, true);
      }
    }
  },

  selectAll: function(checkboxName, checked)
  {
    var list = document.getElementsByName(checkboxName);

    checked = typeof (checked) == 'undefined' ? true : checked;

    for(var i = 0;i < list.length;i++)
    {
      list[i].checked = checked;
    }
  },

  /**
  * 反选
  */
  selectInverse: function(checkboxName)
  {
    var list = document.getElementsByName(checkboxName);

    for(var i = 0;i < list.length;i++)
    {
      list[i].checked = !list[i].checked;
    }
  }
};
/**
* workflow      : historyNode(历史节点)
*
* require       : x.js, x.net.js, x.workflow.js
*/
x.workflow.historyNode = {

    showDiscussDialog: function()
    {
        var outString = '';

        outString += '<table style="width: 100%;" class="table-style border-4" >';
        outString += '<tr>';
        outString += '<td class="table-header border-4">协商</td>';
        outString += '</tr>';
        outString += '<tr>';
        outString += '<td class="table-body">';

        outString += '<table class="table-style" style="width:100%" >';
        outString += '<tr class="table-row-normal" >';
        outString += '<td class="table-body-text" >标题</td>';
        outString += '<td class="table-body-input" colspan="3"></td>';
        outString += '<span>' + title + '</span>';
        outString += '<input id="discussHistotyNodeId" name="discussHistotyNodeId" type="hidden" value="' + historyNodeId + '" />';
        outString += '</td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal" >';
        outString += '<td class="table-body-text" style="width: 120px;" >发起人</td>';
        outString += '<td class="table-body-input" style="width: 200px;" ></td>';
        outString += '<td class="table-body-text" style="width: 120px;" >发起时间</td>';
        outString += '<td class="table-body-input" ></td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal" >';
        outString += '<td class="table-body-text" >发送人</td>';
        outString += '<td class="table-body-input" >dd</td>';
        outString += '<td class="table-body-text" >接收人</td>';
        outString += '<td class="table-body-input" ></td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal" >';
        outString += '<td class="table-body-text" >意见</td>';
        outString += '<td class="table-body-input" colspan="3" ><textarea></textarea></td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal" >';
        outString += '<td class="table-body-text" >通知方式</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += '<div class="checkbox-wrapper" >';
        outString += '<input type="checkbox" /><label>待办</label> ';
        outString += '<input type="checkbox" /><label>邮件</label>';
        outString += '<input type="checkbox" /><label>短信</label>';
        outString += '</div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '<tr class="table-row-normal" >';
        outString += '<td class="table-body-text" >&nbsp;</td>';
        outString += '<td class="table-body-input" colspan="3" >';
        outString += '<div class="button-2font-wrapper" ><a class="button-text" href="#">发送</a></div>';
        outString += '</td>';
        outString += '</tr>';
        outString += '</table>';

        outString += '</td>';
        outString += '</tr>';
        outString += '<tr>';
        outString += '<td class="table-footer" ><img style="height: 18px;" src="/resources/images/transparent.gif"></td>';
        outString += '</tr>';
        outString += '</table>';

        var element = x.workflow.node.maskWrapper.open();

        $(element).html(outString);
    },

    /**
    * 讨论 协商
    */
    discuss: function(options)
    {
        var url = '/api/workflow.historyNode.discuss.save.aspx';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<workflowHistoryNodeId><![CDATA[' + options.workflowHistoryNodeId + ']]></workflowHistoryNodeId>';
        outString += '<fromActorId><![CDATA[' + options.fromActorId + ']]></fromActorId>';
        outString += '<toActorId><![CDATA[' + options.toActorId + ']]></toActorId>';
        outString += '<idea><![CDATA[' + options.idea + ']]></idea>';
        outString += '</ajaxStorage>';

        x.net.xhr(url, outString, options);
    }
};/**
* workflow      : switcherExit(分支出口)
*
* require       : x.js, x.net.js, x.workflow.js
*/
x.workflow.switcherExit = {

  maskWrapper: x.ui.mask.newMaskWrapper('x-workflow-switcherExit-maskWrapper', { draggableHeight: 504, draggableWidth: 738 }),

  // 当前分支条件的索引(编辑状态)
  currentIndex: 0,

  /*#region 函数:edit(index)*/
  edit: function(index)
  {
    x.workflow.switcherExit.currentIndex = index;

    var exit = x.workflow.switcherExit.getSwitcherExitObject(index);

    var outString = '';

    outString += '<div id="workflow-node-switcher-exit-name-' + index + '" class="winodw-wizard-wrapper" style="width:720px; height:auto;" >';

    outString += '<div class="winodw-wizard-toolbar" >';
    outString += '<div class="winodw-wizard-toolbar-close">';
    outString += '<a href="javascript:x.workflow.switcherExit.maskWrapper.close();" title="关闭" ><i class="fa fa-close"></i></a>';
    outString += '</div>';
    outString += '<div class="float-left">';
    outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>编辑分支条件</span></div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';
    outString += '<div class="clear"></div>';
    outString += '</div>';

    outString += '<input id="workflow-node-switcher-exit-index" type="hidden" value="' + index + '" />';

    outString += '<table class="table-style" style="width:100%;" >';
    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td ><input id="workflow-node-switcher-exit-condition-join" type="text" x-dom-feature="combobox" topOffset="-1" x-dom-xhr-url="/api/application.setting.getCombobox.aspx" x-dom-xhr-params="{applicationSettingGroupName:\'应用管理_协同平台_工作流管理_分支出口条件管理_链接方式\'}" x-dom-combobox-icon-hidden="1" class="form-control" style="width:53px" /></td>';
    outString += '<td ><input id="workflow-node-switcher-exit-condition-leftBracket" type="text" x-dom-feature="combobox" topOffset="-1" x-dom-xhr-url="/api/application.setting.getCombobox.aspx" x-dom-xhr-params="{applicationSettingGroupName:\'应用管理_协同平台_工作流管理_分支出口条件管理_左侧括号\'}" x-dom-combobox-icon-hidden="1" class="form-control" style="width:50px" /></td>';
    outString += '<td >';
    outString += '<div class="form-inline">';
    outString += '<div class="input-group">';
    outString += '<input id="workflow-node-switcher-exit-condition-expression1-view" type="text" class="form-control" readonly="readonly" style="width:120px" /> ';
    outString += '<input id="workflow-node-switcher-exit-condition-expression1" type="hidden"/> ';
    outString += '<a href="javascript:x.workflow.switcherExit.expression.edit(\'workflow-node-switcher-exit-condition-expression1\',\'left\');" title="编辑" class="input-group-addon"><span class="glyphicon glyphicon-modal-window"></span></a>';
    outString += '</div>';
    outString += '</div>';

    outString += '</td>';
    outString += '<td ><input id="workflow-node-switcher-exit-condition-compare" type="text" x-dom-feature="combobox" topOffset="-1" x-dom-xhr-url="/api/application.setting.getCombobox.aspx" x-dom-xhr-params="{applicationSettingGroupName:\'应用管理_协同平台_工作流管理_分支出口条件管理_判断方式\'}"  x-dom-combobox-icon-hidden="1" class="form-control" style="width:75px" /></td>';
    outString += '<td >';
    outString += '<input id="workflow-node-switcher-exit-condition-expression2" type="hidden"/> ';
    outString += '<input id="workflow-node-switcher-exit-condition-handmade" type="hidden" /> ';
    outString += '<input id="workflow-node-switcher-exit-condition-expression2-view" type="text" class="form-control" style="width:120px" /> ';
    // outString += '<a href="javascript:x.workflow.switcherExit.expression.edit(\'workflow-node-switcher-exit-condition-expression2\',\'right\');" >编辑</a>';
    outString += '</td>';
    outString += '<td ><input id="workflow-node-switcher-exit-condition-rightBracket" type="text" x-dom-feature="combobox" topOffset="-1" x-dom-xhr-url="/api/application.setting.getCombobox.aspx" x-dom-combobox-icon-hidden="1" x-dom-xhr-params="{applicationSettingGroupName:\'应用管理_协同平台_工作流管理_分支出口条件管理_右侧括号\'}" x-dom-combobox-icon-hidden="1" class="form-control" style="width:50px" /></td>';
    outString += '<td >';
    outString += '<button onclick="javascript:x.workflow.switcherExit.condition.setEditConditionObject();" class="btn btn-default" >确定</button>';
    outString += '</td>';
    outString += '</tr>';
    outString += '</table>';

    outString += '<div style="padding:0; height:auto; border-top:1px solid #ccc;" >';
    outString += '<table id="workflow-node-switcher-exit-wizard-condition$table" class="table-style" style="width:100%;" >';
    outString += '<tr class="table-row-title" >';
    outString += '<td style="width:80px;">链接方式</td>';
    outString += '<td >表达式</td>';
    outString += '<td style="width:30px;" title="编辑" ><i class="fa fa-edit" ></i></td>';
    outString += '<td style="width:30px;" title="删除" ><i class="fa fa-trash" ></i></td>';
    outString += '</tr>';
    outString += '<tr class="table-row-normal-transparent workflow-switcher-exit-condition-new" >';
    outString += '<td colspan="4" style="text-align:right;" ><a href="javascript:x.workflow.switcherExit.condition.reset();" >新增分支条件</a></td>';
    outString += '</tr>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div style="border-top:1px solid #ccc;padding:10px 20px 10px 0;" >';
    outString += '<div class="float-right button-2font-wrapper" ><a id="btnCancel" href="javascript:x.workflow.switcherExit.maskWrapper.close();" class="btn btn-default" >取消</a></div> ';
    outString += '<div class="float-right button-2font-wrapper" style="margin-right:10px;" ><a id="btnOkey" href="javascript:x.workflow.switcherExit.save();" class="btn btn-default" >保存</a></div> ';
    outString += '<div class="clear"></div>';
    outString += '</div>';

    outString += '</div>';

    // 加载遮罩和页面内容
    x.ui.mask.getWindow({ content: outString }, x.workflow.switcherExit.maskWrapper);
    // 设置条件表格
    x.workflow.switcherExit.setConditionTable(exit);

    $('#btnCancel').bind('click', function()
    {
      x.workflow.node.maskWrapper.close();
    });

    x.page.goTop();

    x.dom.features.bind();

    // 重置编辑界面
    x.workflow.switcherExit.condition.reset();
  },
  /*#endregion*/

  /*#region 函数:save()*/
  save: function()
  {
    var list = $(x.workflow.settings.workflowSwitcherExitConditionSelector);

    var index = $(x.workflow.settings.workflowSwitcherExitConditionSelector);

    var outString = '';

    // 设置表达式
    outString = '[';

    for(var i = 0;i < list.length;i++)
    {
      outString += '{';
      outString += '"join":"' + list[i].childNodes[1].childNodes[1].value + '", ';
      outString += '"joinText":"' + list[i].childNodes[1].childNodes[2].value + '", ';
      outString += '"leftBracket":"' + list[i].childNodes[1].childNodes[3].value + '", ';
      outString += '"expression1":"' + list[i].childNodes[1].childNodes[4].value + '", ';
      outString += '"expression1Text":"' + list[i].childNodes[1].childNodes[5].value + '", ';
      outString += '"compare":"' + list[i].childNodes[1].childNodes[6].value + '", ';
      outString += '"compareText":"' + list[i].childNodes[1].childNodes[7].value + '", ';
      outString += '"expression2":"' + list[i].childNodes[1].childNodes[8].value + '", ';
      outString += '"expression2Text":"' + list[i].childNodes[1].childNodes[9].value + '", ';
      outString += '"handmade":"' + list[i].childNodes[1].childNodes[10].value + '", ';
      outString += '"rightBracket":"' + list[i].childNodes[1].childNodes[11].value + '", ';
      outString += '"friendlyText":"' + list[i].childNodes[1].childNodes[0].innerHTML.replace(/&nbsp;/g, ' ') + '" ';
      outString += '},';
    }

    if(outString.substr(outString.length - 1, 1) === ',')
    {
      outString = outString.substr(0, outString.length - 1);
    }

    outString += ']';

    // 设置启用
    document.getElementById('workflow-node-switcher-exit' + x.workflow.switcherExit.currentIndex).checked = (outString.length > 2) ? true : false;

    document.getElementById('workflow-node-switcher-exit' + x.workflow.switcherExit.currentIndex + '-condition').value = outString;

    // 设置表达式
    outString = '';

    for(var i = 0;i < list.length;i++)
    {
      // joinText + friendlyText
      outString += list[i].childNodes[1].childNodes[2].value + ' ' + list[i].childNodes[1].childNodes[0].innerHTML + ' ';
      outString = outString.replace(/&nbsp;/g, ' ');
    }

    document.getElementById('workflow-node-switcher-exit' + x.workflow.switcherExit.currentIndex + '-friendlyCondition').value = outString;
    document.getElementById('workflow-node-switcher-exit' + x.workflow.switcherExit.currentIndex + '-friendlyConditionText').innerHTML = outString;
    // x.debug.log('workflow-node-switcher-exit' + x.workflow.switcherExit.currentIndex + '-condition');

    x.workflow.switcherExit.maskWrapper.close();
  },
  /*#endregion*/

  /*#region 函数:setConditionTable(exit)*/
  setConditionTable: function(exit)
  {
    var list = exit.condition;

    var outString = '';

    for(var i = 0;i < list.length;i++)
    {
      var condition = list[i];

      var idPrefix = 'workflow-node-switcher-exit-condition-' + (i + 1);

      outString += '<tr class="table-row-normal workflow-switcher-exit-condition" >';
      outString += '<td>' + condition.joinText + '</td>';
      outString += '<td>';
      outString += '<span>' + condition.friendlyText + '</span>';
      outString += '<input id="' + idPrefix + '-join" type="hidden" value="' + condition.join + '" />';
      outString += '<input id="' + idPrefix + '-joinText" type="hidden" value="' + condition.joinText + '" />';
      outString += '<input id="' + idPrefix + '-leftBracket" type="hidden" value="' + condition.leftBracket + '" />';
      outString += '<input id="' + idPrefix + '-expression1" type="hidden" value="' + condition.expression1 + '" />';
      outString += '<input id="' + idPrefix + '-expression1Text" type="hidden" value="' + condition.expression1Text + '" />';
      outString += '<input id="' + idPrefix + '-compare" type="hidden" value="' + condition.compare + '" />';
      outString += '<input id="' + idPrefix + '-compareText" type="hidden" value="' + condition.compareText + '" />';
      outString += '<input id="' + idPrefix + '-expression2" type="hidden" value="' + condition.expression2 + '" />';
      outString += '<input id="' + idPrefix + '-expression2Text" type="hidden" value="' + condition.expression2Text + '" />';
      outString += '<input id="' + idPrefix + '-handmade" type="hidden" value="' + condition.handmade + '" />';
      outString += '<input id="' + idPrefix + '-rightBracket" type="hidden" value="' + condition.rightBracket + '" />';
      outString += '</td>';
      outString += '<td><a href="javascript:x.workflow.switcherExit.condition.edit(' + (i + 1) + ')" title="编辑" ><i class="fa fa-edit" ></i></a></td>';
      outString += '<td><a href="javascript:x.workflow.switcherExit.condition.remove(' + (i + 1) + ')" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      outString += '</tr>';
    }

    // 默认情况插入倒数第一个
    $(x.workflow.settings.workflowSwitcherExitConditionNewSelector).before(outString);
  },
  /*#endregion*/

  /*#region 函数:getSwitcherExitObject(index)*/
  getSwitcherExitObject: function(index)
  {
    var outString = '{';

    if(document.getElementById('workflow-node-switcher-exit' + index + '-condition').value === '')
    {
      document.getElementById('workflow-node-switcher-exit' + index + '-condition').value = '[]';
    }

    outString += '"toNodeId":"' + document.getElementById('workflow-node-switcher-exit' + index + '-toNodeId').value + '",';
    outString += '"condition":' + document.getElementById('workflow-node-switcher-exit' + index + '-condition').value + ',';
    outString += '"friendlyCondition":"' + document.getElementById('workflow-node-switcher-exit' + index + '-friendlyCondition').value + '"';
    outString += '}';

    return x.toJSON(outString);
  },
  /*#endregion*/

  /*#region 函数:toSwitcherExitConditionText(exit)*/
  toSwitcherExitConditionText: function(exit)
  {
    var outString = '[';

    for(var i = 0;i < exit.condition.length;i++)
    {
      outString += '{';
      outString += '"join":"' + exit.condition[i].join + '", ';
      outString += '"joinText":"' + exit.condition[i].joinText + '", ';
      outString += '"leftBracket":"' + exit.condition[i].leftBracket + '", ';
      outString += '"expression1":"' + exit.condition[i].expression1 + '", ';
      outString += '"expression1Text":"' + exit.condition[i].expression1Text + '", ';
      outString += '"compare":"' + x.encoding.html.encode(exit.condition[i].compare) + '", ';
      outString += '"compareText":"' + exit.condition[i].compareText + '", ';
      outString += '"expression2":"' + exit.condition[i].expression2 + '", ';
      outString += '"expression2Text":"' + exit.condition[i].expression2Text + '", ';
      outString += '"handmade":"' + exit.condition[i].handmade + '", ';
      outString += '"rightBracket":"' + exit.condition[i].rightBracket + '", ';
      outString += '"friendlyText":"' + exit.condition[i].friendlyText + '" ';
      outString += '},';
    }

    if(outString.substr(outString.length - 1, 1) === ',')
    {
      outString = outString.substr(0, outString.length - 1);
    }

    outString += ']';

    return outString;
  },
  /*#endregion*/

  condition: {

    // 当前分支条件的索引(编辑状态)
    currentIndex: 0,

    /*#region 函数:reset()*/
    reset: function()
    {
      x.workflow.switcherExit.condition.currentIndex = 0;

      $('#workflow-node-switcher-exit-condition-join').val('AND');
      $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-join-view').val('并且');
      $('#workflow-node-switcher-exit-condition-leftBracket').val('(');
      $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-leftBracket-view').val('(');
      $('#workflow-node-switcher-exit-condition-expression1').val('');
      $('#workflow-node-switcher-exit-condition-expression1-view').val('');
      $('#workflow-node-switcher-exit-condition-compare').val('=');
      $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-compare-view').val('等于');
      $('#workflow-node-switcher-exit-condition-expression2').val('');
      $('#workflow-node-switcher-exit-condition-expression2-view').val('');
      $('#workflow-node-switcher-exit-condition-handmade').val(0);
      $('#workflow-node-switcher-exit-condition-rightBracket').val(')');
      $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-rightBracket-view').val(')');
    },
    /*#endregion*/

    /*#region 函数:sync()*/
    /**
    * 同步工作流节点信息，重新分析设置节点数据。
    */
    sync: function()
    {
      var list = $(x.workflow.settings.workflowSwitcherExitConditionSelector);

      for(var i = 0;i < list.length;i++)
      {
        var row = list[i];

        var index = i + 1;

        var idPrefix = 'workflow-node-switcher-exit-condition-' + index;

        row.childNodes[1].childNodes[1].id = idPrefix + '-join';
        row.childNodes[1].childNodes[2].id = idPrefix + '-joinText';
        row.childNodes[1].childNodes[3].id = idPrefix + '-leftBracket';
        row.childNodes[1].childNodes[4].id = idPrefix + '-expression1';
        row.childNodes[1].childNodes[5].id = idPrefix + '-expression1Text';
        row.childNodes[1].childNodes[6].id = idPrefix + '-compare';
        row.childNodes[1].childNodes[7].id = idPrefix + '-compareText';
        row.childNodes[1].childNodes[8].id = idPrefix + '-expression2';
        row.childNodes[1].childNodes[9].id = idPrefix + '-expression2Text';
        row.childNodes[1].childNodes[10].id = idPrefix + '-handmade';
        row.childNodes[1].childNodes[11].id = idPrefix + '-rightBracket';
        row.childNodes[2].innerHTML = '<a href="javascript:x.workflow.switcherExit.condition.edit(' + index + ')" title="编辑" ><i class="fa fa-edit" ></i></a>';
        row.childNodes[3].innerHTML = '<a href="javascript:x.workflow.switcherExit.condition.remove(' + index + ')" title="删除" ><i class="fa fa-trash" ></i></a>';
      }
    },
    /*#endregion*/

    /*#region 函数:edit(index)*/
    edit: function(index)
    {
      x.workflow.switcherExit.condition.currentIndex = index;

      var editPrefix = 'workflow-node-switcher-exit-condition';

      var idPrefix = 'workflow-node-switcher-exit-condition-' + index;

      // 加载编辑数据
      document.getElementById(editPrefix + '-join').value = document.getElementById(idPrefix + '-join').value;
      document.getElementById(editPrefix + '-join-view').value = document.getElementById(idPrefix + '-joinText').value;
      document.getElementById(editPrefix + '-leftBracket').value = document.getElementById(idPrefix + '-leftBracket').value;
      document.getElementById(editPrefix + '-leftBracket-view').value = document.getElementById(idPrefix + '-leftBracket').value;
      document.getElementById(editPrefix + '-expression1').value = document.getElementById(idPrefix + '-expression1').value;
      document.getElementById(editPrefix + '-expression1-view').value = document.getElementById(idPrefix + '-expression1Text').value;
      document.getElementById(editPrefix + '-compare').value = document.getElementById(idPrefix + '-compare').value;
      document.getElementById(editPrefix + '-compare-view').value = document.getElementById(idPrefix + '-compareText').value;
      document.getElementById(editPrefix + '-expression2').value = document.getElementById(idPrefix + '-expression2').value;
      document.getElementById(editPrefix + '-expression2-view').value = document.getElementById(idPrefix + '-expression2Text').value;
      document.getElementById(editPrefix + '-handmade').value = document.getElementById(idPrefix + '-handmade').value;
      document.getElementById(editPrefix + '-rightBracket').value = document.getElementById(idPrefix + '-rightBracket').value;
      document.getElementById(editPrefix + '-rightBracket-view').value = document.getElementById(idPrefix + '-rightBracket').value;

      x.form.query(editPrefix + 'expression1-view').css({ 'text-decoration': 'underline', 'font-weight': 'bold' });

      if(document.getElementById(idPrefix + '-handmade').value == '0')
      {
        x.form.query(editPrefix + 'expression2-view').css({ 'text-decoration': 'underline', 'font-weight': 'bold' });
      }
      else
      {
        x.form.query(editPrefix + 'expression2-view').css({ 'text-decoration': 'none', 'font-weight': 'normal' });
      }
    },
    /*#endregion*/

    /*#region 函数:remove(index)*/
    remove: function(index)
    {
      var target = $(x.workflow.settings.workflowSwitcherExitConditionSelector)[index - 1];

      if(target)
      {
        $(target).remove();
      }

      x.workflow.switcherExit.condition.sync();
    },
    /*#endregion*/

    /*#region 函数:setEditConditionObject()*/
    setEditConditionObject: function()
    {
      // 验证

      // 保存
      var list = $(x.workflow.settings.workflowSwitcherExitConditionSelector);

      var condition = x.workflow.switcherExit.condition.getEditConditionObject();

      var outString = '';

      var index = x.workflow.switcherExit.condition.currentIndex;

      if(index === 0)
      {
        index = list.length + 1;

        var idPrefix = 'workflow-node-switcher-exit-condition-' + index;

        outString += '<tr class="table-row-normal workflow-switcher-exit-condition" >';
        outString += '<td>' + (index === 1 ? '' : condition.joinText) + '</td>';
        outString += '<td>';
        outString += '<span>' + condition.friendlyText + '</span>';
        outString += '<input id="' + idPrefix + '-join" type="hidden" value="' + (index === 1 ? '' : condition.join) + '" />';
        outString += '<input id="' + idPrefix + '-joinText" type="hidden" value="' + (index === 1 ? '' : condition.joinText) + '" />';
        outString += '<input id="' + idPrefix + '-leftBracket" type="hidden" value="' + condition.leftBracket + '" />';
        outString += '<input id="' + idPrefix + '-expression1" type="hidden" value="' + condition.expression1 + '" />';
        outString += '<input id="' + idPrefix + '-expression1Text" type="hidden" value="' + condition.expression1Text + '" />';
        outString += '<input id="' + idPrefix + '-compare" type="hidden" value="' + condition.compare + '" />';
        outString += '<input id="' + idPrefix + '-compareText" type="hidden" value="' + condition.compareText + '" />';
        outString += '<input id="' + idPrefix + '-expression2" type="hidden" value="' + condition.expression2 + '" />';
        outString += '<input id="' + idPrefix + '-expression2Text" type="hidden" value="' + condition.expression2Text + '" />';
        outString += '<input id="' + idPrefix + '-handmade" type="hidden" value="' + condition.handmade + '" />';
        outString += '<input id="' + idPrefix + '-rightBracket" type="hidden" value="' + condition.rightBracket + '" />';
        outString += '</td>';
        outString += '<td><a href="javascript:x.workflow.switcherExit.condition.edit(' + index + ')" title="编辑" ><i class="fa fa-edit" ></i></a></td>';
        outString += '<td><a href="javascript:x.workflow.switcherExit.condition.remove(' + index + ')" title="删除" ><i class="fa fa-trash" ></i></a></td>';
        outString += '</tr>';

        // 默认情况插入倒数第一个
        $(x.workflow.settings.workflowSwitcherExitConditionNewSelector).before(outString);
      }
      else
      {
        var row = list[index - 1];

        row.childNodes[0].innerHTML = (index === 1 ? '' : condition.joinText);
        row.childNodes[1].childNodes[0].innerHTML = condition.friendlyText;
        row.childNodes[1].childNodes[1].value = (index === 1 ? '' : condition.join);
        row.childNodes[1].childNodes[2].value = (index === 1 ? '' : condition.joinText);
        row.childNodes[1].childNodes[3].value = condition.leftBracket;
        row.childNodes[1].childNodes[4].value = condition.expression1;
        row.childNodes[1].childNodes[5].value = condition.expression1Text;
        row.childNodes[1].childNodes[6].value = condition.compare;
        row.childNodes[1].childNodes[7].value = condition.compareText;
        row.childNodes[1].childNodes[8].value = condition.expression2;
        row.childNodes[1].childNodes[9].value = condition.expression2Text;
        row.childNodes[1].childNodes[10].value = condition.handmade;
        row.childNodes[1].childNodes[11].value = condition.rightBracket;
      }

      x.debug.log('x.workflow.switcherExit.save();');
      x.debug.log(condition);

      // 重置编辑界面
      x.workflow.switcherExit.condition.reset();
    },
    /*#endregion*/

    /*#region 函数:getEditConditionObject()*/
    /**
    * 获取当前编辑节点
    */
    getEditConditionObject: function()
    {
      var join = $('#workflow-node-switcher-exit-condition-join').val();
      var leftBracket = $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-leftBracket-view').val();
      var expression1 = $('#workflow-node-switcher-exit-condition-expression1-view').val();
      var compare = $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-compare-view').val();
      var expression2 = $('#workflow-node-switcher-exit-condition-expression2-view').val();
      var handmade = $('#workflow-node-switcher-exit-condition-handmade');
      var rightBracket = $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-rightBracket-view').val();

      var friendlyText = leftBracket + '' + expression1 + ' ' + compare + ' ' + expression2 + '' + rightBracket;

      if(handmade === '1')
      {
        $('#workflow-node-switcher-exit-condition-expression2').val($('#workflow-node-switcher-exit-condition-expression2-view').val());
      }

      // -------------------------------------------------------
      // 输出结果
      // -------------------------------------------------------

      var outString = '';

      outString += '{';
      outString += '"join":"' + $('#workflow-node-switcher-exit-condition-join').val() + '", ';
      outString += '"joinText":"' + $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-join-view').val() + '", ';
      outString += '"leftBracket":"' + $('#workflow-node-switcher-exit-condition-leftBracket').val() + '", ';
      outString += '"expression1":"' + $('#workflow-node-switcher-exit-condition-expression1').val() + '", ';
      outString += '"expression1Text":"' + $('#workflow-node-switcher-exit-condition-expression1-view').val() + '", ';
      outString += '"compare":"' + $('#workflow-node-switcher-exit-condition-compare').val() + '", ';
      outString += '"compareText":"' + $('#' + x.ui.classNamePrefix + '-workflow-node-switcher-exit-condition-compare-view').val() + '", ';
      outString += '"expression2":"' + $('#workflow-node-switcher-exit-condition-expression2').val() + '", ';
      outString += '"expression2Text":"' + $('#workflow-node-switcher-exit-condition-expression2-view').val() + '", ';
      outString += '"handmade":"' + ($('#workflow-node-switcher-exit-condition-handmade')[0].checked ? 1 : 0) + '", ';
      outString += '"rightBracket":"' + $('#workflow-node-switcher-exit-condition-rightBracket').val() + '", ';
      outString += '"friendlyText":"' + friendlyText + '" ';
      outString += '}';

      return x.toJSON(outString);
    }
    /*#endregion*/
  },

  expression: {

    maskWrapper: x.ui.mask.newMaskWrapper('x-workflow-switcherExit-expression-maskWrapper', { draggableHeight: 504, draggableWidth: 738 }),

    /*#region 函数:getTreeView()*/
    /*
    * 获取树形菜单
    */
    getTreeView: function(entitySchemaId, expressionType)
    {
      var url = '';
      var treeViewId = '10000000-0000-0000-0000-000000000000';
      var treeViewName = '选择表达式';
      var treeViewRootTreeNodeId = '00000000-0000-0000-0000-000000000001';
      var treeViewUrl = 'javascript:x.workflow.switcherExit.expression.setTreeViewNode(\'{treeNodeId}\',\'{treeNodeName}\')';

      var entitySchemaIds = '04-E01,04-E02' + ((entitySchemaId === 'undefined') ? '' : (',' + entitySchemaId));

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<action><![CDATA[getDynamicTreeView]]></action>';
      outString += '<treeViewId><![CDATA[' + treeViewId + ']]></treeViewId>';
      outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
      outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
      outString += '<entitySchemaIds><![CDATA[' + entitySchemaIds + ']]></entitySchemaIds>';
      outString += '<expressionType><![CDATA[' + expressionType + ']]></expressionType>';
      outString += '<tree><![CDATA[{tree}]]></tree>';
      outString += '<parentId><![CDATA[{parentId}]]></parentId>';
      outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
      outString += '</request>';

      var tree = x.ui.pkg.tree.newTreeView({ name: 'x.workflow.switcherExit.expression.tree', ajaxMode: true });

      tree.add({
        id: "0",
        parentId: "-1",
        name: treeViewName,
        url: treeViewUrl.replace('{treeNodeId}', treeViewRootTreeNodeId),
        title: treeViewName,
        target: '',
        icon: '/resources/images/tree/tree_icon.gif'
      });

      tree.load('/api/workflow.expression.getDynamicTreeView.aspx', false, outString);

      // 增加内置业务实体信息

      var list = x.workflow.template.getTemplategetInternalMetaData();

      if(list.length > 0)
      {
        tree.add({
          id: '00000',
          parentId: '0',
          name: '表单内置信息',
          url: 'javascript:void(0);'
        });

        x.each(list, function(index, node)
        {
          tree.add({
            id: '00000-' + index,
            parentId: '00000',
            name: node.fieldName,
            url: 'javascript:x.workflow.switcherExit.expression.setTreeViewNode(\'meta:' + node.fieldType + '#' + node.dataColumnName + '\',\'' + node.fieldName + '\')'
          });

          // tree.add('00000-' + index, '00000', node.fieldName, 'javascript:x.workflow.switcherExit.expression.setTreeViewNode(\'meta:' + node.fieldType + '#' + node.dataColumnName + '\',\'' + node.fieldName + '\')');
        });
      }

      x.workflow.switcherExit.expression.tree = tree;

      $('#workflow-switcherExit-expression-wizardTreeViewContainer')[0].innerHTML = tree;
    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode (value, text)*/
    setTreeViewNode: function(value, text)
    {
      $('#workflow-switcherExit-expression-wizardExpressionText').val(text);
      $('#workflow-switcherExit-expression-wizardExpressionValue').val(value);
    },
    /*#endregion*/

    /*#region 函数:edit(expressionName)*/
    edit: function(expressionName, expressionType)
    {
      var outString = '';

      outString += '<div id="' + expressionName + '-edit" class="winodw-wizard-wrapper" style="width:300px; height:auto;" >';

      outString += '<div class="winodw-wizard-toolbar" >';
      outString += '<div class="winodw-wizard-toolbar-close">';
      outString += '<a href="javascript:x.workflow.switcherExit.expression.maskWrapper.close();" title="关闭" ><i class="fa fa-close"></i></a>';
      outString += '</div>';
      outString += '<div class="float-left">';
      outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>设置表达式</span></div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';
      outString += '<div class="clear"></div>';
      outString += '</div>';

      outString += '<div id="workflow-switcherExit-expression-wizardTreeViewContainer" class="winodw-wizard-tree-view" style="height:300px;" ></div>';

      outString += '<div class="winodw-wizard-result-container form-inline text-right" >';
      outString += '<label class="winodw-wizard-result-item-text" >表达式</label> ';
      outString += '<input id="workflow-switcherExit-expression-wizardExpressionText" name="wizardExpressionText" type="text" value="" class="winodw-wizard-result-item-input form-control input-sm" style="width:160px" /> ';
      outString += '<input id="workflow-switcherExit-expression-wizardExpressionValue" name="wizardExpressionValue" type="hidden" value="" />';
      outString += '<a href="javascript:x.workflow.switcherExit.expression.save(\'' + expressionName + '\');" class="btn btn-default btn-sm" >确定</a>';
      outString += '</div>';

      // 加载遮罩和页面内容
      x.ui.mask.getWindow({ content: outString }, x.workflow.switcherExit.expression.maskWrapper);

      // 初始化数据
      $('#workflow-switcherExit-expression-wizardExpressionText').val($('#' + expressionName + '-view').val());
      $('#workflow-switcherExit-expression-wizardExpressionValue').val($('#' + expressionName).val());

      x.net.xhr('/api/kernel.entities.schema.findOneByEntityClassName.aspx?entityClassName=' + encodeURIComponent($('#workflow-template-entityClass').val()), {
        callback: function(response)
        {
          var param = x.toJSON(response).data;

          x.workflow.switcherExit.expression.getTreeView(param.id, expressionType);
        }
      });
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function(expressionName)
    {
      x.workflow.switcherExit.expression.maskWrapper.close();

      $('#' + expressionName + '-view').val($('#workflow-switcherExit-expression-wizardExpressionText').val());
      $('#' + expressionName).val($('#workflow-switcherExit-expression-wizardExpressionValue').val());
      $('#' + expressionName + '-view').css({ 'text-decoration': 'underline', 'font-weight': 'bold' });
      $('#workflow-node-switcher-exit-condition-handmade').val(0);
    }
    /*#endregion*/
  }
};