x.register('main.applications.application.feature.setting');

main.applications.application.feature.setting = {

  pages: x.page.newPagingHelper(),

  getTreeTableView: function()
  {
    if($('#authorizationObjectName').val() == '' && $('#applicationName').val() == '')
    {
      $('#window-application-feature-setting-message').html('请选择标准角色名称和应用名称。');
      $('#window-application-feature-setting-message').css('display', '');
      return;
    }

    if($('#authorizationObjectName').val() == '')
    {
      $('#window-application-feature-setting-message').html('请选择标准角色名。');
      $('#window-application-feature-setting-message').css('display', '');
      return;
    }

    if($('#applicationName').val() == '')
    {
      $('#window-application-feature-setting-message').html('请选择标应用名称。');
      $('#window-application-feature-setting-message').css('display', '');
      return;
    }

    $('#window-application-feature-setting-message').html('');
    $('#window-application-feature-setting-message').css('display', 'none');

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[getTreeTableView]]></action>';
    outString += '<authorizationObjectType><![CDATA[' + $('#authorizationObjectType').val() + ']]></authorizationObjectType>';
    outString += '<authorizationObjectId><![CDATA[' + $('#authorizationObjectId').val() + ']]></authorizationObjectId>';
    outString += '<applicationId><![CDATA[' + $('#applicationId').val() + ']]></applicationId>';
    outString += '</request>';

    x.net.xhr('/api/application.feature.getTreeTableView.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var outString = '';

        var result = x.toJSON(response);

        var list = result.data;

        var counter = 0;

        var classNameValue = '';

        outString += '<table class="table-style table-full-border-style ajax-tree-table" style="width:100%;" >';
        outString += '<tr class="table-row-title" >';
        outString += '<th >功能标识</th>';
        outString += '<th >上级功能标识</th>';
        outString += '<th style="width:200px" >功能名称</th>';
        outString += '<th >功能全称</th>';
        outString += '<th class="end" style="width:80px" >授权</th>';
        outString += '</tr>';

        // [关键] class="x-ui-tree-table-tbody" 
        outString += '<tbody id="x-ui-tree-table-container" >';

        x.each(list, function(index, node)
        {
          classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

          outString += '<tr class="' + classNameValue + '">';
          outString += '<td>' + node.id + '</td>';
          outString += '<td>' + (node.parentId == '00000000-0000-0000-0000-000000000000' ? '' : node.parentId) + '</td>';
          outString += '<td>' + node.name + '</td>';
          outString += '<td>' + (node.type == 'action' ? '动作' : '功能') + '</td>';
          outString += '<td><input id="' + node.id + '" name="' + node.id + '" type="checkbox" ' + (node.authorized == '1' ? 'checked="checked"' : '') + ' class="bind-feature-setting" /></td>';
          outString += '</tr>';

          counter++;
        });

        outString += '</tbody>';
        outString += '</table>';

        $('#window-application-feature-setting-container').html(outString);

        x.ui.pkg.treeTable.newTreeTableView({ targetName: 'x-ui-tree-table-container' });
        // 只能应用于tbody
        // $("#x-ui-tree-table-container").treeTable();
      }
    });
  },

  /**
  * 保存
  */
  setTreeTableView: function()
  {
    var applicationFeatureIds = '';

    $('.bind-feature-setting').each(function(index, node)
    {
      if(node.checked)
      {
        applicationFeatureIds += node.id + ',';
      }
    });

    if(applicationFeatureIds.length > 0 && applicationFeatureIds.substr(applicationFeatureIds.length - 1, 1) == ',')
    {
      applicationFeatureIds = applicationFeatureIds.substr(0, applicationFeatureIds.length - 1);
    }

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += '<action><![CDATA[setTreeTableView]]></action>';
    outString += '<applicationId><![CDATA[' + $('#applicationId').val() + ']]></applicationId>';
    outString += '<authorizationObjectType><![CDATA[' + $('#authorizationObjectType').val() + ']]></authorizationObjectType>';
    outString += '<authorizationObjectId><![CDATA[' + $('#authorizationObjectId').val() + ']]></authorizationObjectId>';
    outString += '<applicationFeatureIds><![CDATA[' + applicationFeatureIds + ']]></applicationFeatureIds>';
    outString += '</request>';

    x.net.xhr('/api/application.feature.setTreeTableView.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response).message;

        alert(result.value);
      }
    });
  },

  /*#region 函数:load()*/
  /**
   * 页面加载事件
   */
  load: function()
  {
    $('#btnContactsWizard').on('click', function()
    {
      x.ui.wizards.getContactWizard({ targetTypeName: 'authorizationObjectType', targetViewName: 'authorizationObjectName', targetValueName: 'authorizationObjectId', contactTypeText: 'all' });
    });

    $('#btnApplicationWizard').on('click', function()
    {
      x.ui.wizards.getApplicationWizard({ targetViewName: 'applicationDisplayName', targetValueName: 'applicationId' });
    });
  }
  /*#endregion*/
}

$(document).ready(main.applications.application.feature.setting.load);