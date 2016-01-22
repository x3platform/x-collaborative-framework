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
            outString += '<validationType>忘记密码</validationType>';
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
        resetPassword: function()
        {
            var registration = $('#registration').val();

            if(registration == 'mobile')
            {
                if(x.dom('#mobile').val() == '')
                {
                    x.msg('必须填写手机号码。');
                    return;
                }

                if(x.dom('#code').val() == '')
                {
                    x.msg('必须填写短信验证码。');
                    return;
                }
            }
            else
            {
                if(x.dom('#email').val() == '')
                {
                    x.msg('必须填写电子邮箱。');
                    return;
                }

                if(x.dom('#code').val() == '')
                {
                    x.msg('必须填写邮件验证码。');
                    return;
                }
            }

            var code = x.dom('#code').val();

            if(registration == 'mobile')
            {
                location.href = '/account/set-password?registration=' + registration + '&mobile=' + x.dom('#mobile').val() + '&code=' + x.dom('#code').val();
            }
            else
            {
                location.href = '/account/set-password?registration=' + registration + '&email=' + x.dom('#code').val() + '&code=' + x.dom('#code').val();
            }
        }
    };

    x.dom.ready(function()
    {
        if(x.dom('#captchaMode').val() == 'ON')
        {
            x.dom('#captchaImage').on('click', main.loadCaptcha);

            // 初次加载
            main.loadCaptcha();
        }

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        x.dom('#btnVerificationCode').on('click', main.sendVerificationCode);

        x.dom('#btnSubmit').on('click', main.resetPassword);

        // 加载表单特性
        x.dom.features.bind();
    });
})(x, document);
