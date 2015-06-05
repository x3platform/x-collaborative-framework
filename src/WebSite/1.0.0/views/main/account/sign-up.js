x.dom.ready(function()
{
    // 帐号输入框事件
    x.dom('#loginName').on('blur', function()
    {
        this.className = 'window-sign-up-input-style';
    });

    x.dom('#loginName').on('focus', function()
    {
        this.className = 'window-sign-up-input-style-over';
    });

    // 密码输入框事件
    x.dom('#originalPassword').on('blur', function()
    {
        this.className = 'window-sign-up-input-style';
    });

    x.dom('#originalPassword').on('focus', function()
    {
        this.className = 'window-sign-up-input-style-over';
    });

    function checkAndSignUp()
    {
        var email = x.dom('#email').val();
        var originalPassword = x.dom('#originalPassword').val();

        x.dom('#password').val(CryptoJS.SHA1(originalPassword).toString());

        if (x.dom('#email').val() == '' || originalPassword == '')
        {
            x.msg('必须填写电子邮箱和密码。');
            return;
        }

        if (originalPassword.length < 6)
        {
            x.msg('密码长度必须大于八位。');
            return;
        }

        if (originalPassword != x.dom('#confirmPassword').val())
        {
            x.dom('#originalPassword').val('');
            x.dom('#confirmPassword').val('');

            x.msg('密码和确认密码不匹配，请重新输入。');
            return;
        }

        /*
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<loginName><![CDATA[' + $('#loginName').val() + ']]></loginName>';
        outString += '<password><![CDATA[' + $('#password').val() + ']]></password>';
        outString += '</ajaxStorage>';
        */

        x.dom('.loading').css({ 'display': '' });

        x.net.xhr('/api/membership.member.register.aspx', x.dom.data.serialize(), function(response)
        {
            var result = x.toJSON(response).message;

            switch (Number(result.returnCode))
            {
                case 0:
                    var returnUrl = decodeURIComponent(x.net.request.find("returnUrl"));
                    window.location.href = (returnUrl == '') ? '/' : returnUrl;
                    break;

                case 1:
                    x.msg(result.value);
                    break;
            }

            x.dom('.loading').css({ 'display': 'none' });
        });
    }

    x.dom('#password').on('keyup', function(event)
    {
        event = (typeof (event) == 'undefined') ? window.event : event;

        if (event.keyCode == 13) { checkAndSignUp(); }
    });

    // 登录按钮事件事件
    x.dom('#btnSubmit').on('click', checkAndSignUp);

    // 加载表单特性
    x.dom.features.bind();
});
