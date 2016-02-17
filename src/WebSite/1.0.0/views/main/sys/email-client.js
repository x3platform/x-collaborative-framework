x.register('main.sys.email.client');

main.sys.email.client = {

  /*#region 函数:getSmtpServer()*/
  /*
  * 获得邮件服务器
  */
  getSmtpServer: function()
  {
    x.net.xhr('/api/email.client.getSmtpServer.aspx?token=' + x.randomText.create(8), {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.queryTipText,
      callback: function(response)
      {
        var param = x.toJSON(response).data;

        $('#from').val(param.defaultSenderEmailAddress);
        $('#host').val(param.host);
        $('#port').val((param.port == 0) ? 25 : param.port);
        $('#enableSsl')[0].checked = (param.enableSsl == 'True') ? true : false;

        $('#username').val(param.username);
        $('#password').val(param.password);
      }
    });
  },
  /*#endregion*/

  /*#region 函数:setSmtpServer()*/
  /*
  * 设置邮件服务器
  */
  setSmtpServer: function(defaultSenderEmailAddress, host, port, enableSsl, username, password)
  {
    $('#from').val(defaultSenderEmailAddress);
    $('#host').val(host);
    $('#port').val((port == 0) ? 25 : port);
    $('#enableSsl')[0].checked = (enableSsl.toUpperCase() == 'ON') ? true : false;

    $('#username').val(username);
    $('#password').val(password);
  },
  /*#endregion*/

  /*#region 函数:testSmtpServer()*/
  /*
  * 测试邮件服务器
  */
  sendMail: function()
  {
    var outString = '<?xml version="1.0" encoding="utf-8"?>';

    outString += '<request>';
    outString += '<from><![CDATA[' + $('#from').val() + ']]></from>';
    outString += '<to><![CDATA[' + $('#to').val() + ']]></to>';
    outString += '<subject><![CDATA[' + $('#subject').val() + ']]></subject>';
    outString += '<body><![CDATA[' + $('#content').val() + ']]></body>';
    outString += '<isBodyHtml><![CDATA[true]]></isBodyHtml>';
    outString += '</request>';

    x.net.xhr('/api/email.client.sendMail.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.commitTipText,
      popCorrectValue: 1
    });
  },
  /*#endregion*/

  /*#region 函数:testSmtpServer()*/
  /*
  * 测试邮件服务器
  */
  testSmtpServer: function()
  {
    if($('#host').val() == '')
    {
      $('#host')[0].focus();

      x.msg('邮件服务器地址未填写');
      return false;
    }

    var outString = '<?xml version="1.0" encoding="utf-8"?>';

    outString += '<request>';
    outString += '<host><![CDATA[' + $('#host').val() + ']]></host>';
    outString += '<port><![CDATA[' + $('#port').val() + ']]></port>';
    outString += '<enableSsl><![CDATA[' + ($('#enableSsl')[0].checked ? 'On' : 'Off') + ']]></enableSsl>';
    outString += '<username><![CDATA[' + $('#username').val() + ']]></username>';
    outString += '<password><![CDATA[' + $('#password').val() + ']]></password>';
    outString += '<fromEmailAddress><![CDATA[' + $('#from').val() + ']]></fromEmailAddress>';
    outString += '<toEmailAddress><![CDATA[' + $('#to').val() + ']]></toEmailAddress>';
    outString += '<body><![CDATA[' + $('#content').val() + ']]></body>';
    outString += '</request>';

    x.net.xhr('/api/email.client.testSmtpServer.aspx', outString, {
      waitingType: 'mini',
      waitingMessage: i18n.net.waiting.commitTipText,
      popCorrectValue: 1
    });
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    main.sys.email.client.getSmtpServer();

    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------
    //[]
  }
  /*#endregion*/
};

$(document).ready(main.sys.email.client.load);
