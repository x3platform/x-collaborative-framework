x.register('main.account.admin');

main.account.admin = {

  /*#region 函数:getStrengthMessage()*/
  /*
  * 获取密码强度警告消息
  */
  getStrengthMessage: function()
  {
    var strengthCode = x.net.request.find('strengthCode');

    var message = $('#passwordStrengthMessage');

    switch(strengthCode)
    {
      case '2':
        message.html('密码复杂度提示:密码中必须包含一个【0～9】数字。');
        message.css({ display: '' });
        break;
      case '3':
        message.html('密码复杂度提示:密码中必须包含一个【A～Z或a～z】字符。');
        message.css({ display: '' });
        break;
      case '4':
        message.html('密码复杂度提示:密码中必须包含一个【# $ @ !】特殊字符。');
        message.css({ display: '' });
        break;
      case '5':
        message.html('密码复杂度提示:密码长度必须大于【' + $('#passwordPolicyMinimumLength').val() + '】。');
        message.css({ display: '' });
        break;
      case '6':
        message.html('密码复杂度提示:密码中相邻字符重复次数不能超过【' + $('#passwordPolicyCharacterRepeatedTimes').val() + '】次。');
        message.css({ display: '' });
        break;
      case '9':
        message.html('密码复杂度提示:密码为默认密码，强烈建议修改为其他密码。');
        message.css({ display: '' });
        break;
      default:
        message.css({ display: 'none' });
        break;
    }
  },
  /*#endregion*/

  /*#region 函数:checkObject()*/
  /*
  * 检测对象的必填数据
  */
  checkObject: function()
  {
    if(x.dom.data.check()) { return false; }

    var originalPassword = $('#originalPassword').val();
    var newPassword = $('#newPassword').val();
    var confirmPassword = $('#confirmPassword').val();
    var passwordPolicyRules = $('#passwordPolicyRules').val();
    var passwordPolicyMinimumLength = Number($('#passwordPolicyMinimumLength').val());
    var passwordPolicyCharacterRepeatedTimes = Number($('#passwordPolicyCharacterRepeatedTimes').val());

    // 规则判断标识
    var flag = false;
    var charCode = -1;

    if(originalPassword == '')
    {
      x.msg('必须填写【旧密码】信息');
      $('#originalPassword')[0].focus();
      return false;
    }

    if(newPassword == '')
    {
      x.msg('必须填写【新密码】信息');
      $('#newPassword')[0].focus();
      return false;
    }

    if(confirmPassword == '')
    {
      x.msg('必须填写【确认密码】信息');
      $('#confirmPassword')[0].focus();
      return false;
    }

    if(passwordPolicyRules.indexOf('[Number]') > -1)
    {
      flag = false;
      charCode = -1;

      // charCode 48 - 57
      for(var i = 0;i < newPassword.length;i++)
      {
        charCode = newPassword.charCodeAt(i);

        if(charCode >= 48 && charCode <= 57)
        {
          flag = true;
          break;
        }
      }

      if(!flag)
      {
        x.msg('必须包含一个【0～9】数字');
        return false;
      }
    }

    if(passwordPolicyRules.indexOf('[Character]') > -1)
    {
      flag = false;
      charCode = -1;

      for(var i = 0;i < newPassword.length;i++)
      {
        charCode = newPassword.charCodeAt(i);

        if((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122))
        {
          flag = true;
          break;
        }
      }

      if(!flag)
      {
        x.msg('必须包含一个【A～Z或a～z】字符');
        return false;
      }
    }

    if(passwordPolicyRules.indexOf('[SpecialCharacter]') > -1)
    {
      flag = false;
      charCode = -1;

      // ! 33 # 35 $ 36 @ 64
      for(var i = 0;i < newPassword.length;i++)
      {
        charCode = newPassword.charCodeAt(i);

        if(charCode == 33 || charCode == 35 || charCode == 36 || charCode == 64)
        {
          flag = true;
          break;
        }
      }

      if(!flag)
      {
        x.msg('必须包含一个【# $ @ !】特殊字符');
        return false;
      }
    }

    if(passwordPolicyMinimumLength > 0 && newPassword.length < passwordPolicyMinimumLength)
    {
      x.msg('密码长度必须大于【' + passwordPolicyMinimumLength + '】');
      $('#confirmPassword')[0].focus();
      return false;
    }

    if(passwordPolicyCharacterRepeatedTimes > 1 && newPassword.length > passwordPolicyCharacterRepeatedTimes)
    {
      // 判断字符连续出现的次数
      var repeatedTimes = 1;

      for(var i = 0;i < newPassword.length - passwordPolicyCharacterRepeatedTimes;i++)
      {
        charCode = newPassword.charCodeAt(i);

        repeatedTimes = 1;

        for(var j = 1;j < passwordPolicyCharacterRepeatedTimes;j++)
        {
          if(charCode == newPassword.charCodeAt(i + j))
          {
            repeatedTimes++;
          }
          else
          {
            break;
          }
        }

        if(repeatedTimes >= passwordPolicyCharacterRepeatedTimes)
        {
          x.msg('【' + newPassword.charAt(i) + '】在密码中重复多次出现，相邻字符重复次数不能超过【' + passwordPolicyCharacterRepeatedTimes + '】次。');
          return false;
        }
      }
    }

    if(newPassword != confirmPassword)
    {
      x.msg('新密码与确认密码不匹配，请重新输入。');

      $('#newPassword').val('');
      $('#confirmPassword').val('');
      $('#newPassword')[0].focus();
      return false;
    }

    return true;
  },
  /*#endregion*/

  /*#region 函数:changePassword()*/
  /**
   * 修改密码
   */
  changePassword: function()
  {
    if(main.account.admin.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += '<password><![CDATA[' + CryptoJS.SHA1($("#newPassword").val()) + ']]></password>';
      outString += '<originalPassword><![CDATA[' + CryptoJS.SHA1($("#originalPassword").val()) + ']]></originalPassword>';
      outString += '</request>';

      x.net.xhr('/api/hr.general.changePassword.aspx', outString, {
        waitingType: 'mini',
        waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
        popCorrectValue: 1,
        callback: function(response)
        {
          $('#originalPassword').val('');
          $('#newPassword').val('');
          $('#confirmPassword').val('');
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:changeLoginName()*/
  /**
   * 修改登录名
   */
  changeLoginName: function()
  {
    if(main.account.admin.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += '<originalPassword><![CDATA[' + CryptoJS.SHA1($("#originalPassword").val()) + ']]></originalPassword>';
      outString += '</request>';

      x.net.xhr('/api/hr.general.changeLoginName.aspx', outString, {
        waitingType: 'mini',
        waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
        popCorrectValue: 1,
        callback: function(response)
        {
          $('#originalPassword').val('');
          $('#newPassword').val('');
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:changeLoginName()*/
  /**
   * 修改登录名
   */
  disableAccount: function()
  {
    if(main.account.admin.checkObject())
    {
      x.net.xhr('/api/hr.general.disableAccount.aspx', outString, {
        waitingType: 'mini',
        waitingMessage: i18n.strings.msg_net_waiting_save_tip_text,
        popCorrectValue: 1,
        callback: function(response)
        {
          masterpage.logout();
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /**
   * 页面加载事件
   */
  load: function()
  {
    // x.ui.pkg.tabs.newTabs();

    // 设置高度
    var height = x.page.getViewHeight();

    var freezeHeight = 0;

    $('.x-freeze-height').each(function(index, node)
    {
      freezeHeight += $(node).outerHeight();
    });

    var freezeTableHeadHeight = $('#window-main-table-body .table-freeze-head').outerHeight();
    var freezeTableSidebarSearchHeight = $('#window-main-table-body .table-sidebar-search').outerHeight();

    $('#window-main-table').css(
    {
      'height': (height - freezeHeight - freezeTableHeadHeight) + 'px',
      'overflow-y': 'scroll'
    });
  }
  /*#endregion*/
}

x.dom.ready(main.account.admin.load);
