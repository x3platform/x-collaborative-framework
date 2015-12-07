(function(x, document)
{
    var main = {
        // 加载验证码
        loadCaptcha: function()
        {
            x.net.xhr('/api/kernel.security.verificationCode.captcha.create.aspx?width=100&height=34', {
                callback: function(responseText)
                {
                    var result = x.toJSON(responseText);

                    $('#captchaImage')[0].src = 'data:image/png;base64,' + result.data.base64;
                    $('#captchaImage').css({
                        'width': result.data.width,
                        'height': result.data.height,
                        'border': '1px solid #ccc',
                        'border-radius': '5px',
                        'margin-left': '10px'
                    });

                    // console.log(responseText);
                }
            });
        },

        sendVerificationCode: function()
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += '<captcha><![CDATA[' + x.dom('#captcha').val() + ']]></captcha>';
            outString += '<email><![CDATA[' + x.dom('#email').val() + ']]></email>';
            outString += '<validationType>注册帐号</validationType>';
            outString += '</request>';

            x.net.xhr('/api/hr.general.sendVerificationMail.aspx', outString, {
                callback: function(response)
                {
                    var result = x.toJSON(response).message;

                    switch(Number(result.returnCode))
                    {
                        case 0:
                            break;

                        case 1:
                            x.msg(result.value);
                            break;
                    }
                }
            });
        },

        checkAndSignUp: function()
        {
            var email = x.dom('#email').val();
            var originalPassword = x.dom('#originalPassword').val();

            x.dom('#password').val(CryptoJS.SHA1(originalPassword).toString());

            if(x.dom('#email').val() == '' || originalPassword == '')
            {
                x.msg('必须填写电子邮箱和密码。');
                return;
            }

            if(originalPassword.length < 6)
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

            x.net.xhr('/api/membership.member.register.aspx', x.dom.data.serialize(), function(response)
            {
                var result = x.toJSON(response).message;

                switch(Number(result.returnCode))
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

    };

    x.dom.ready(function()
    {
        x.dom('#password').on('keyup', function(event)
        {
            event = (typeof (event) == 'undefined') ? window.event : event;

            if(event.keyCode == 13) { main.checkAndSignUp(); }
        });

        x.dom('#captchaImage').on('click', main.loadCaptcha);

        x.dom('#btnVerificationCode').on('click', main.sendVerificationCode);

        // 登录按钮事件事件
        x.dom('#btnSubmit').on('click', main.checkAndSignUp);

        // 加载表单特性
        x.dom.features.bind();

        // 初次加载
        main.loadCaptcha();
    });
})(x, document);