x.dom.ready(function()
{
  // 发送验证码的事件
  x.dom('#btnVerificationCode').on('click', function()
  {
    var email = x.dom('#email').val();

    if(x.dom('#email').val() == '')
    {
      x.msg('必须填写电子邮箱地址。');
      return;
    }

    x.dom('.loading').css({ 'display': '' });

    x.net.xhr('/api/hr.general.forgotPassword.aspx', x.dom.data.serialize(), function(response)
    {
      var result = x.toJSON(response).message;

      switch(Number(result.returnCode))
      {
        case 0:
          x.msg(result.value);
          break;

        case 1:
          x.msg(result.value);
          break;
      }

      x.dom('.loading').css({ 'display': 'none' });
    });
  });

  // 重置密码按钮事件
  x.dom('#btnSubmit').on('click', function()
  {
    var email = x.dom('#email').val();
    var code = x.dom('#code').val();

    if(email == '' || code == '')
    {
      x.msg('必须填写电子邮箱地址和验证码。');
      return;
    }

    x.dom('.loading').css({ 'display': '' });

    x.net.xhr('/api/hr.general.setPassword.aspx', x.dom.data.serialize(), function(response)
    {
      var result = x.toJSON(response).message;

      switch(Number(result.returnCode))
      {
        case 0:
          x.msg(result.value);
          break;

        case 1:
          x.msg(result.value);
          break;
      }

      x.dom('.loading').css({ 'display': 'none' });
    });
  });

  // 加载表单特性
  x.dom.features.bind();
});
