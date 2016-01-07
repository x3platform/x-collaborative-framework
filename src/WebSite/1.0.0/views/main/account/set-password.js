(function(x, document)
{
    var main = {
        setPassword: function()
        {
            var email = x.dom('#email').val();
            var code = x.dom('#code').val();

            var originalPassword = x.dom('#originalPassword').val();

            x.dom('#password').val(CryptoJS.SHA1(originalPassword).toString());

            if(email == '' || code == '')
            {
                x.msg('必须填写电子邮箱地址和验证码。');
                return;
            }

            var code = x.dom('#code').val();

            if(originalPassword.length < 7)
            {
                x.msg('密码长度必须大于八位。');
                return;
            }

            if(originalPassword != x.dom('#confirmPassword').val())
            {
                x.dom('#originalPassword').val('');
                x.dom('#confirmPassword').val('');

                x.msg('密码和确认密码不匹配，请重新输入。');
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
        }
    };

    x.dom.ready(function()
    {
        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        x.dom('#btnSubmit').on('click', main.setPassword);
    });
})(x, document);
