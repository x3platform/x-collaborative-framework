x.register('main.membership.organization.validator');

main.membership.organization.validator = {

    /*#region 函数:setGlobalName()*/
    /*
    * 设置全局名称
    */
    setGlobalName: function()
    {
        if($('#globalName').val() == $('#originalGlobalName').val()) { return; }

        if(confirm('确定设置此用户的全局名称为【' + $('#globalName').val() + '】。'))
        {
            var url = '/services/X3Platform/Membership/Ajax.OrganizationWrapper.aspx';

            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<ajaxStorage>';
            outString += '<action><![CDATA[setGlobalName]]></action>';
            outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
            outString += '<globalName><![CDATA[' + $("#globalName").val() + ']]></globalName>';
            outString += '</ajaxStorage>';

            var options = {
                resultType: 'json',
                xml: outString
            };

            $.post(url, options, function(response)
            {
                x.net.fetchException(response);

                var result = x.toJSON(response).message;

                switch(Number(result.returnCode))
                {
                    case 0:
                        location.reload();
                        break;

                    case 1:
                    case -1:
                    default:
                        alert(result.value);
                        break;
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:setExchangeEmail: function ()*/
    /*
    * 设置企业邮箱 (启用/禁用)
    */
    setExchangeEmail: function()
    {
        $('#enableExchangeEmail')[0].checked = !$('#enableExchangeEmail')[0].checked;

        var message = '';

        if($('#enableExchangeEmail')[0].checked)
        {
            message = '确定启用企业邮箱，将在邮件服务器上创建相关邮箱数据，是否继续?';
        }
        else
        {
            message = '确定禁用企业邮箱，此操作具有一定的危险性，会删除所有相关邮箱数据，是否继续?';
        }

        if(confirm(message))
        {
            var url = '/services/X3Platform/Exchange/Client/Ajax.DistributionGroupWrapper.aspx';

            var outString = '<?xml version="1.0" encoding="utf-8"?>';

            outString += '<ajaxStorage>';
            outString += '<action><![CDATA[setExchangeEmail]]></action>';
            outString += '<type><![CDATA[organization]]></type>';
            outString += '<id><![CDATA[' + $("#id").val() + ']]></id>';
            outString += '<enableExchangeEmail><![CDATA[' + ($('#enableExchangeEmail')[0].checked ? '1' : '0') + ']]></enableExchangeEmail>';
            outString += '</ajaxStorage>';

            var options = {
                resultType: 'json',
                xml: outString
            };

            $.post(url, options, function(response)
            {
                x.net.fetchException(response);

                var result = x.toJSON(response).message;

                switch(Number(result.returnCode))
                {
                    case 0:
                        location.reload();
                        break;

                    case 1:
                    case -1:
                    default:
                        alert(result.value);
                        break;
                }
            });
        }
        else
        {
            // 用户按取消后，将复选框恢复原状
            $('#enableExchangeEmail')[0].checked = !$('#enableExchangeEmail')[0].checked;
        }
    },
    /*#endregion*/

    /*#region 函数:load: function ()*/
    /*
    * 页面加载事件
    */
    load: function()
    {
    }
    /*#endregion*/
}

$(document).ready(main.membership.organization.validator.load);
