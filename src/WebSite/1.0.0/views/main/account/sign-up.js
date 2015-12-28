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
            var registration = $('#registration').val();

            var url = null;

            if(registration == 'mobile')
            {
                url = '/api/hr.general.sendVerificationSMS.aspx';
            }
            else
            {
                url = '/api/hr.general.sendVerificationMail.aspx';
            }

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';

            if(x.dom('#captchaMode').val() == 'ON')
            {
                outString += '<captcha><![CDATA[' + x.dom('#captcha').val() + ']]></captcha>';
            }

            if(registration == 'mobile')
            {
                if(x.dom('#mobile').val() == '')
                {
                    x.msg('必须填写手机号码。');
                    return;
                }
                outString += '<phoneNumber><![CDATA[' + x.dom('#mobile').val() + ']]></phoneNumber>';
            }
            else
            {
                if(x.dom('#email').val() == '')
                {
                    x.msg('必须填写邮箱地址。');
                    return;
                }
                outString += '<email><![CDATA[' + x.dom('#email').val() + ']]></email>';
            }
            outString += '<validationType>用户注册</validationType>';
            outString += '</request>';

            x.net.xhr(url, outString, {
                callback: function(response)
                {
                    var result = x.toJSON(response).message;

                    switch(Number(result.returnCode))
                    {
                        case 0:
                            x.dom('#btnVerificationCode')[0].disabled = true;

                            x.msg(result.value);

                            // 初始化一个60秒的倒计时计时器
                            var counter = 60;

                            var timer = x.newTimer(1, function(timer)
                            {
                                x.dom('#btnVerificationCode').html((counter--) + '秒后重新获取');

                                if(counter < 0)
                                {
                                    x.dom('#btnVerificationCode').html('重新发送验证码');
                                    x.dom('#btnVerificationCode')[0].disabled = false;

                                    // 停止计时器
                                    timer.stop();
                                }
                            });

                            // 启动计时器
                            timer.start();

                            break;

                        case 1:
                            x.dom('#btnVerificationCode')[0].disabled = false;
                            x.msg(result.value);
                            break;
                    }
                }
            });
        },

        checkAndSignUp: function()
        {
            var registration = $('#registration').val();
            var originalPassword = x.dom('#originalPassword').val();

            x.dom('#password').val(CryptoJS.SHA1(originalPassword).toString());

            if(registration == 'mobile')
            {
                if(x.dom('#mobile').val() == '' || originalPassword == '')
                {
                    x.msg('必须填写手机号码和密码。');
                    return;
                }
            }
            else
            {
                if(x.dom('#email').val() == '' || originalPassword == '')
                {
                    x.msg('必须填写电子邮箱和密码。');
                    return;
                }
            }

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

        if(x.dom('#captchaMode').val() == 'ON')
        {
            x.dom('#captchaImage').on('click', main.loadCaptcha);

            // 初次加载
            main.loadCaptcha();
        }

        x.dom('#btnVerificationCode').on('click', main.sendVerificationCode);

        // 登录按钮事件事件
        x.dom('#btnSubmit').on('click', main.checkAndSignUp);

        // 加载表单特性
        x.dom.features.bind();
    });
})(x, document);