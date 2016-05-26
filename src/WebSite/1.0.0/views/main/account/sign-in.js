$(document).ready(function ()
{
    // 设置登录主界面位置
    // 主界面上边距 = 窗口高度 - 主界面高度
    var windowHeight = x.page.getViewHeight();
    var height = 640;

    if ((windowHeight - height) > 0)
    {
        $('.window-sign-in-main-wrapper').css({ 'margin-top': (windowHeight - height) + 'px' });
    }

    // 移除访问令牌信息
    localStorage.removeItem('session-access-refresh-token');
    localStorage.removeItem('session-access-token');

    // 设置登录帐号
    var account = localStorage['cache-account'] || '{}';

    if (account != '{}' && account.length > 2)
    {
        $('#remember')[0].checked = true;

        account = x.toJSON(account);

        $('#loginName').val(account.loginName);
    }

    function loginCheck()
    {
        if ($('#loginName').val() == '' || $('#password').val() == '')
        {
            alert('必须填写帐号和密码。');
            return;
        }

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<loginName><![CDATA[' + $('#loginName').val() + ']]></loginName>';
        outString += '<password><![CDATA[' + CryptoJS.SHA1($('#password').val()).toString() + ']]></password>';
        outString += '</request>';

        $('.window-sign-in-loading').show();

        x.net.xhr('/api/connect.auth.authorize.aspx', outString, function (response)
        {
            var result = x.toJSON(response).message;

            switch (Number(result.returnCode))
            {
                case 0:

                    var data = x.toJSON(response).data;

                    if ($('#remember')[0].checked)
                    {
                        // 记住当前用户登录信息
                        var time = new Date(new Date().setDate(new Date().getDate() + 14));
                        localStorage['cache-account'] = '{"loginName":"' + $('#loginName').val() + '"}';
                    }
                    else
                    {
                        localStorage.removeItem('cache-account');
                    }

                    localStorage['session-access-token'] = data.id;
                    localStorage['session-access-refresh-token'] = data.refreshToken;
                    localStorage['session-expireDate'] = data.expireDateTimestampView;

                    var returnUrl = decodeURIComponent(x.net.request.find("returnUrl"));

                    window.location.href = (returnUrl == '') ? '/' : returnUrl;
                    break;

                case 1:
                    x.msg(result.value);
                    break;
            }

            $('.window-sign-in-loading').hide();
        });
    }

    $('#password').keyup(function (event)
    {
        event = (typeof (event) == 'undefined') ? window.event : event;

        if (event.keyCode == 13) { loginCheck(); }
    });

    // 登录按钮事件事件
    $('#btnSubmit').click(loginCheck);

    try
    {
        document.execCommand("ClearAuthenticationCache", "false");
    }
    catch (ex)
    {
    }
});
