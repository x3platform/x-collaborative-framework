x.register('main.membership.accountGrant.list');

main.membership.accountGrant.list = {

  paging: x.page.newPagingHelper(50),

  maskWrapper: x.ui.mask.newMaskWrapper('main.membership.accountGrant.list.maskWrapper'),

  /*
  * 过滤
  */
  filter: function()
  {
    var whereClauseValue = '';

    var key = $('#searchText').val();

    if(key.trim() != '')
    {
      whereClauseValue = ' T.GrantorId IN ( SELECT Id FROM tb_Account WHERE LoginName LIKE ##%' + key + '%## OR Name LIKE ##%' + key + '%## ) OR T.GranteeId IN ( SELECT Id FROM tb_Account WHERE LoginName LIKE ##%' + key + '%## OR Name LIKE ##%' + key + '%## ) ';
    }

    main.membership.accountGrant.list.paging.whereClause = whereClauseValue;

    main.membership.accountGrant.list.getPaging(1);
  },

  /*
  * 创建对象列表的视图
  */
  getObjectsView: function(list, maxCount)
  {
    var counter = 0;

    var classNameValue = '';

    var outString = '';

    outString += '<div class="table-freeze-head">';
    outString += '<table class="table" >';
    outString += '<thead>';
    outString += '<tr>';
    outString += '<th >委托人</td>';
    outString += '<th style="width:200px">被委托人</th>';
    outString += '<th style="width:80px">开始时间</th>';
    outString += '<th style="width:80px">结束时间</th>';
    outString += '<th style="width:60px">中止</th>';
    outString += '<th style="width:30px" title="删除" ><i class="fa fa-trash" ></i></th>';
    outString += '<th class="table-freeze-head-padding" ></th>';
    outString += '</tr>';
    outString += '</thead>';
    outString += '</table>';
    outString += '</div>';

    outString += '<div class="table-freeze-body">';
    outString += '<table class="table table-striped">';
    outString += '<colgroup>';
    outString += '<col />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:50px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:100px" />';
    outString += '<col style="width:50px" />';
    outString += '</colgroup>';
    outString += '<tbody>';

    x.each(list, function(index, node)
    {
      classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) == maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '">';
      outString += '<td><a href="javascript:main.membership.accountGrant.list.openDialog(\'' + node.id + '\');">' + node.grantorName + '</a></td>';
      outString += '<td>' + node.granteeName + '</td>';
      outString += '<td>' + node.grantedTimeFromView + '</td>';
      outString += '<td>' + node.grantedTimeToView + '</td>';

      if(node.isAborted == 'True')
      {
        outString += '<td><span class="red-text">已中止</span></td>';
      }
      else
      {
        outString += '<td><a href="javascript:main.membership.accountGrant.list.abort(\'' + node.id + '\');">中止</a></td>';
      }
      outString += '<td><a href="javascript:main.membership.accountGrant.list.confirmDelete(\'' + node.id + '\');" title="删除" ><i class="fa fa-trash" ></i></a></td>';
      outString += '</tr>';

      counter++;
    });

    // 补全

    while(counter < maxCount)
    {
      var classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

      classNameValue = classNameValue + ((counter + 1) == maxCount ? '-transparent' : '');

      outString += '<tr class="' + classNameValue + '">';
      outString += '<td colspan="6" ><img src="/resources/images/transparent.gif" alt="" style="height:18px;" /></td>';
      outString += '</tr>';

      counter++;
    }

    outString += '</tbody>';
    outString += '</table>';
    outString += '</div>';

    return outString;
  },

  /*
  * 创建单个对象的视图
  */
  getObjectView: function(param)
  {
    var outString = '<table class="table-style" style="width:100%">';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px;" ><span class="required-text">编号</span></td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<input id="id" name="id" type="hidden" value="' + ((typeof (param.id) == 'undefined' || param.id == '0') ? '' : param.id) + '" />';
    outString += '<input id="isAborted" name="isAborted" type="hidden" value="' + (typeof (param.isAborted) == 'undefined' ? '' : param.isAborted) + '" />';
    outString += '<input id="actualFinishedTime" name="actualFinishedTime" type="hidden" value="' + (typeof (param.actualFinishedTimeTimestampView) == 'undefined' ? '' : param.actualFinishedTimeTimestampView) + '" />';
    outString += '<input id="status" name="status" type="hidden" value="' + (typeof (param.status) == 'undefined' ? '0' : param.status) + '" />';
    if(typeof (param.code) == 'undefined' || param.code == '')
    {
      outString += '<span class="gray-text">自动编号</span>';
      outString += '<input id="code" name="code" type="hidden" value="" />';
    }
    else
    {
      outString += '<input id="code" name="code" type="text" value="' + (typeof (param.code) == 'undefined' ? '' : param.code) + '" dataVerifyWarning="【编号】必须填写。" class="form-control x-ajax-input custom-forms-data-required" style="width:120px;" />';
    }
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" style="width:120px;" ><span class="required-text">委托人</span></td>';
    outString += '<td class="table-body-input" style="width:160px;" >';
    outString += '<input id="grantorId" name="grantorId" type="hidden" value="' + (typeof (param.grantorId) == 'undefined' ? '' : param.grantorId) + '" />';
    outString += '<input id="grantorName" name="grantorName" type="text" value="' + (typeof (param.grantorName) == 'undefined' ? '' : param.grantorName) + '" dataVerifyWarning="【委托人】必须填写。"  class="form-control x-ajax-input custom-forms-data-required" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getSingleObjectWizardSingleton(\'grantorName\',\'grantorId\',\'account\',1);" >编辑</a> ';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" ><span class="required-text">被委托人</span></td>';
    outString += '<td class="table-body-input" >';
    outString += '<input id="granteeId" name="granteeId" type="hidden" value="' + (typeof (param.granteeId) == 'undefined' ? '' : param.granteeId) + '" />';
    outString += '<input id="granteeName" name="granteeName" type="text" value="' + (typeof (param.granteeName) == 'undefined' ? '' : param.granteeName) + '" dataVerifyWarning="【被委托人】必须填写。"  class="form-control x-ajax-input custom-forms-data-required" style="width:120px;" /> ';
    outString += '<a href="javascript:x.ui.wizards.getSingleObjectWizardSingleton(\'granteeName\',\'granteeId\',\'account\',0);" >编辑</a> ';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" ><span class="required-text">委托开始时间</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="grantedTimeFrom" name="grantedTimeFrom" type="text" x-dom-feature="date" value="' + (typeof (param.grantedTimeFrom) == 'undefined' ? '' : param.grantedTimeFromView) + '" dataVerifyWarning="【委托开始时间】必须填写。" class="form-control x-ajax-input custom-forms-data-required" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" ><span class="required-text">委托结束时间</span></td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="grantedTimeTo" name="grantedTimeTo" type="text" x-dom-feature="date" value="' + (typeof (param.grantedTimeTo) == 'undefined' ? '' : param.grantedTimeToView) + '" dataVerifyWarning="【委托结束时间】必须填写。" class="form-control x-ajax-input custom-forms-data-required" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >流程审批</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="workflowGrantMode" name="workflowGrantMode" type="text" value="' + (typeof (param.workflowGrantMode) == 'undefined' ? '' : param.workflowGrantMode) + '" x-dom-feature="combobox" selectedText="' + (typeof (param.workflowGrantModeView) == 'undefined' ? '' : param.workflowGrantModeView) + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_帐号委托_流程审批委托类别## ) " class="form-control x-ajax-input" style="width:120px;" />';
    outString += '</td>';
    outString += '<td class="table-body-text" style="width:120px" >数据授权</td>';
    outString += '<td class="table-body-input">';
    outString += '<input id="dataQueryGrantMode" name="dataQueryGrantMode" type="text" value="' + (typeof (param.dataQueryGrantMode) == 'undefined' ? '' : param.dataQueryGrantMode) + '" x-dom-feature="combobox" selectedText="' + (typeof (param.dataQueryGrantModeView) == 'undefined' ? '' : param.dataQueryGrantModeView) + '" url="/api/application.setting.getCombobox.aspx" comboboxWhereClause=" ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##应用管理_协同平台_人员及权限管理_帐号委托_数据授权委托类别## ) " class="form-control x-ajax-input" style="width:120px;" />';
    outString += '</td>';
    outString += '</tr>';

    if(param.isAborted == 'True')
    {
      outString += '<tr class="table-row-normal-transparent">';
      outString += '<td class="table-body-text" >中止</td>';
      outString += '<td class="table-body-input">';
      outString += '<input id="isAbortedView" name="isAbortedView" type="checkbox" checked="checked" class="checkbox-normal x-ajax-checkbox" />';
      outString += '</td>';
      outString += '<td class="table-body-text" >实际结束时间</td>';
      outString += '<td class="table-body-input">' + (typeof (param.actualFinishedTimeTimestampView) == 'undefined' ? '' : param.actualFinishedTimeTimestampView) + '</td>';
      outString += '</tr>';
    }

    outString += '<!–[if IE]>';
    outString += '</table>';
    outString += '<table class="table-style" style="width:100%" >';
    outString += '<![endif]–>';

    outString += '<tr class="table-row-normal-transparent">';
    outString += '<td class="table-body-text" style="width:120px" >相关审批地址</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<input id="approvedUrl" name="approvedUrl" type="text" value="' + (typeof (param.approvedUrl) == 'undefined' ? '' : param.approvedUrl) + '" class="form-control x-ajax-input" style="width:460px;" />';
    outString += '</td>';
    outString += '</tr>';

    outString += '<tr class="table-row-normal-transparent" >';
    outString += '<td class="table-body-text" >备注</td>';
    outString += '<td class="table-body-input" colspan="3" >';
    outString += '<textarea id="remark" name="remark" type="text" class="textarea-normal x-ajax-textarea" style="width:460px; height:60px;" >' + (typeof (param.remark) == 'undefined' ? '' : param.remark) + '</textarea>';
    outString += '</td>';
    outString += '</tr>';
    outString += '<input id="isAborted" name="isAborted" type="hidden" value="' + (typeof (param.isAborted) == 'undefined' ? '' : param.isAborted) + '" />';
    outString += '<input id="actualFinishedTime" name="actualFinishedTime" type="hidden" value="' + (typeof (param.actualFinishedTimeView) == 'undefined' ? '' : param.actualFinishedTimeView) + '" />';

    outString += '</table>';

    return outString;
  },

  /*
  * 分页
  */
  getPaging: function(currentPage)
  {
    var paging = main.membership.accountGrant.list.paging;

    paging.currentPage = currentPage;

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';
    outString += paging.toXml();
    outString += '</request>';

    x.net.xhr('/api/membership.accountGrant.query.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        var paging = main.membership.accountGrant.list.paging;

        var list = result.data;

        var counter = 0;

        paging.load(result.paging);

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.membership.accountGrant.list.openDialog();" class="btn btn-default" >新增</a>'
                       + '</div>'
                       + '<span>帐号委托管理</span>'
                       + '<div class="clearfix" ></div>';

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.accountGrant.list.getObjectsView(list, paging.pagingize);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        var footerHtml = paging.tryParseMenu('javascript:main.membership.accountGrant.list.getPaging({0});');

        $('#window-main-table-footer')[0].innerHTML = footerHtml;

        $('.table-row-filter').css('display', '');

        masterpage.resize();
      }
    });
  },

  /*
  * 查看单个记录的信息
  */
  openDialog: function(value)
  {
    var id = x.isUndefined(value, '');

    var url = '';

    var outString = '<?xml version="1.0" encoding="utf-8" ?>';

    outString += '<request>';

    var isNewObject = false;

    if(id === '')
    {
      url = '/api/membership.accountGrant.create.aspx';

      isNewObject = true;
    }
    else
    {
      url = '/api/membership.accountGrant.findOne.aspx';

      outString += '<id><![CDATA[' + id + ']]></id>';
    }

    outString += '</request>';

    x.net.xhr(url, outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        var headerHtml = '<div id="toolbar" class="table-toolbar" >'
                       + '<a href="javascript:main.membership.accountGrant.list.save();" class="btn btn-default" >保存</a> '
                       + '<a href="javascript:main.membership.accountGrant.list.getPaging(' + main.membership.accountGrant.list.paging.currentPage + ');" class="btn btn-default" >取消</a>'
                       + '</div>'
                       + '<span>帐号委托信息</span>'
                       + '<div class="clear"></div>';

        if(param.isAborted == 'True')
        {
          headerHtml = '<div id="toolbar" class="table-toolbar" >'
                     + '<a href="javascript:main.membership.accountGrant.list.getPaging(' + main.membership.accountGrant.list.paging.currentPage + ');" >取消</a>'
                     + '</div>'
                     + '<span>帐号委托信息</span>'
                     + '<div class="clear"></div>';
        }

        $('#window-main-table-header')[0].innerHTML = headerHtml;

        var containerHtml = main.membership.accountGrant.list.getObjectView(param);

        $('#window-main-table-container')[0].innerHTML = containerHtml;

        var footerHtml = '<div><img src="/resources/images/transparent.gif" alt="" style="height:18px" /></div>';

        $('#window-main-table-footer')[0].innerHTML = footerHtml;

        $('.table-row-filter').css('display', 'none');

        x.dom.features.bind();
      }
    });
  },

  /*
  * 检测对象的必填数据
  */
  checkObject: function()
  {
    if(x.dom.data.check()) { return false; }

    return true;
  },

  /*
  * 保存对象
  */
  save: function()
  {
    // 1.数据检测

    if(!main.membership.accountGrant.list.checkObject())
      return;

    // 2.发送数据

    var outString = '<?xml version="1.0" encoding="utf-8"?>';

    outString += '<request>';
    outString += '<action><![CDATA[save]]></action>';
    outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
    outString += '<code><![CDATA[' + $("#code").val() + ']]></code>';
    outString += '<grantorId><![CDATA[' + $("#grantorId").val() + ']]></grantorId>';
    outString += '<granteeId><![CDATA[' + $("#granteeId").val() + ']]></granteeId>';
    outString += '<grantedTimeFrom><![CDATA[' + $("#grantedTimeFrom").val() + ']]></grantedTimeFrom>';
    outString += '<grantedTimeTo><![CDATA[' + $("#grantedTimeTo").val() + ']]></grantedTimeTo>';
    outString += '<workflowGrantMode><![CDATA[' + $("#workflowGrantMode").val() + ']]></workflowGrantMode>';
    outString += '<dataQueryGrantMode><![CDATA[' + $("#dataQueryGrantMode").val() + ']]></dataQueryGrantMode>';
    outString += '<isAborted><![CDATA[' + $("#isAborted").val() + ']]></isAborted>';
    outString += '<actualFinishedTime><![CDATA[' + $("#actualFinishedTime").val() + ']]></actualFinishedTime>';
    outString += '<approvedUrl><![CDATA[' + $("#approvedUrl").val() + ']]></approvedUrl>';
    outString += '<status><![CDATA[' + $("#status").val() + ']]></status>';
    outString += '<remark><![CDATA[' + $("#remark").val() + ']]></remark>';
    outString += '</request>';

    var options = {
      resultType: 'json',
      xml: outString
    };

    $.post(main.membership.accountGrant.list.url,
           options,
           main.membership.accountGrant.list.save_callback);
  },

  save_callback: function(response)
  {
    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
        main.membership.accountGrant.list.getPaging(main.membership.accountGrant.list.paging.currentPage);
        break;

      case -1:
      case 1:
      default:
        alert(result.value);
        break;
    }
  },

  /*
  * 删除对象
  */
  confirmDelete: function(value)
  {
    if(confirm('确定删除?'))
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';
      outString += '<request>';
      outString += '<action><![CDATA[delete]]></action>';
      outString += '<ids><![CDATA[' + value + ']]></ids>';
      outString += '</request>';

      var options = {
        resultType: 'json',
        xml: outString
      };

      $.post(main.membership.accountGrant.list.url,
             options,
             main.membership.accountGrant.list.confirmDelete_callback);
    }
  },

  confirmDelete_callback: function(response)
  {
    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
        main.membership.accountGrant.list.getPaging(main.membership.accountGrant.list.paging.currentPage);
        break;

      case 1:
        alert(result.value);
        break;

      default:
        break;
    }
  },

  /*
  * 中止
  */
  abort: function(value)
  {
    if(confirm('确定中止当前委托信息?'))
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';
      outString += '<request>';
      outString += '<action><![CDATA[abort]]></action>';
      outString += '<id><![CDATA[' + value + ']]></id>';
      outString += '</request>';

      var options = {
        resultType: 'json',
        xml: outString
      };

      $.post(main.membership.accountGrant.list.url,
             options,
             main.membership.accountGrant.list.abort_callback);
    }
  },

  abort_callback: function(response)
  {
    var result = x.toJSON(response).message;

    switch(Number(result.returnCode))
    {
      case 0:
        main.membership.accountGrant.list.getPaging(main.membership.accountGrant.list.paging.currentPage);
        break;

      case 1:
        alert(result.value);
        break;

      default:
        break;
    }
  },

  /*
  * 打开联系人窗口
  */
  getContactsWindow: function(type)
  {
    type = (typeof (type) == 'undefined') ? 'default' : type;

    var storage = '';
    var viewArray = '';
    var valueArray = '';

    if(type == 'grantor')
    {
      main.contacts.home.contactType = 1;

      viewArray = $('#grantorName').val().split(';');
      valueArray = $('#grantorId').val().split(';');

      storage += '{"list":[';

      for(var i = 0;i < valueArray.length;i++)
      {
        if(valueArray[i] != '')
        {
          storage += '{"text":"' + viewArray[i] + '","value":"account#' + valueArray[i] + '#' + viewArray[i] + '"},'
        }
      }

      if(storage.substr(storage.length - 1, 1) == ',')
        storage = storage.substr(0, storage.length - 1);

      storage += ']}';

      main.contacts.home.localStorage = storage;

      main.contacts.home.save_callback = function(response)
      {
        var resultView = '';
        var resultText = '';

        var list = x.toJSON(response).list;

        list.each(function(node, index)
        {
          resultView += node.text + ';';
          resultText += node.value.substring(node.value.indexOf('#') + 1, node.value.lastIndexOf('#')) + ',';
        });

        if(resultText.substr(resultText.length - 1, 1) == ',')
        {
          resultView = resultView.substr(0, resultView.length - 1)
          resultText = resultText.substr(0, resultText.length - 1);
        }

        $('#grantorName').val(resultView);
        $('#grantorId').val(resultText);

        main.contacts.home.localStorage = response;
      }
    }
    else if(type == 'grantee')
    {
      main.contacts.home.contactType = 1;

      viewArray = $('#granteeName').val().split(';');
      valueArray = $('#granteeId').val().split(';');

      storage += '{"list":[';

      for(var i = 0;i < valueArray.length;i++)
      {
        if(valueArray[i] != '')
        {
          storage += '{"text":"' + viewArray[i] + '","value":"account#' + valueArray[i] + '#' + viewArray[i] + '"},'
        }
      }

      if(storage.substr(storage.length - 1, 1) == ',')
        storage = storage.substr(0, storage.length - 1);

      storage += ']}';

      main.contacts.home.localStorage = storage;

      main.contacts.home.save_callback = function(response)
      {
        var resultView = '';
        var resultText = '';

        var list = x.toJSON(response).list;

        list.each(function(node, index)
        {
          resultView += node.text + ';';
          resultText += node.value.substring(node.value.indexOf('#') + 1, node.value.lastIndexOf('#')) + ',';
        });

        if(resultText.substr(resultText.length - 1, 1) == ',')
        {
          resultView = resultView.substr(0, resultView.length - 1)
          resultText = resultText.substr(0, resultText.length - 1);
        }

        $('#granteeName').val(resultView);
        $('#granteeId').val(resultText);

        main.contacts.home.localStorage = response;
      }
    }

    main.contacts.home.cancel_callback = function(response)
    {
      main.membership.accountGrant.list.maskWrapper.close();
    }

    //
    // 关键代码 结束
    //

    // 非模态窗口, 需要设置.
    main.contacts.home.maskWrapper = main.membership.accountGrant.list.maskWrapper;

    // 加载地址簿信息
    main.contacts.home.load();
  },

  /*
  * 页面加载事件
  */
  load: function()
  {
    main.membership.accountGrant.list.filter();

    // -------------------------------------------------------
    // 过滤事件
    // -------------------------------------------------------

    $('#searchText').bind('keyup', function()
    {
      main.membership.accountGrant.list.filter();
    });

    $('.table-sidebar-search-button').bind('click', function()
    {
      main.membership.accountGrant.list.filter();
    });
  }
}

$(document).ready(main.membership.accountGrant.list.load);
