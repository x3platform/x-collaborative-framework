x.register('main.forum.util');

main.forum.util = {

    followUrl: '/services/X3Platform/Forum/Ajax.ForumFollowWrapper.aspx',

    refresh: false,

    getTextAreaValue: function(textareaName)
    {
        var re = /\n/g;

        var value = $(textareaName).val().replace(re, '</p><p>');

        value = '<p>' + value + '</p>';

        return value;
    },

    /*
    *
    */
    setTextAreaValue: function(textareaName, value)
    {
        var re = /<\/p><p>/g;

        value = value.replace(re, "\n");

        re = /<p>/g;
        value = value.replace(re, '');

        re = /<\/p>/g;
        value = value.replace(re, '');

        if ($(textareaName) != null)
            $(textareaName).val(value);

        return value;
    },

    /*#region 函数:isHot(click, comment)*/
    /*
    * 判断是否热门
    */
    isHot: function(click, comment)
    {
        var outString = '';

        if (click >= 1000 && comment >= 10)
        {
            outString = '<img src="/resources/images/icon/icon-status-hot.gif" alt="热门" title="热门" style="margin:0 0 -2px 6px;" />';
        }

        return outString;
    },
    /*#endregion*/

    /*#region 函数:isEssential(status)*/
    /*
    * 判断是否精华
    */
    isEssential: function(status)
    {
        var outString = '';

        if (status == 1)
        {
            outString = '<img src="/resources/images/icon/icon-status-essential.gif" alt="精华" title="精华" style="margin:0 0 -2px 6px;" />';
        }
        return outString;
    },
    /*#endregion*/

    /*#region 函数:isNew(time, nowDate)*/
    /*
    * 判断是否新帖，此处三天之内都是新帖
    */
    isNew: function(time, nowDate)
    {
        var outString = '';

        var strDate = time.split(',');
        var date = new Date(strDate[0], strDate[1] - 1, strDate[2], strDate[3], strDate[4], strDate[5]);
        var time = new Date(nowDate);
        time.setDate(time.getDate() - 3);

        if (date >= time)
        {
            outString = '<img src="/resources/images/icon/icon-status-new.gif" alt="新的" title="新的" style="margin:0 0 -2px 6px;" />';
        }

        return outString;
    },
    /*#endregion*/

    /*#region 函数:getTimeHelper(time)*/
    /*
    * 格式化时间，得到年月日时分秒
    */
    getTimeHelper: function(time)
    {
        var times = time.split(',');
        var timestr = times[0] + '-' + times[1] + '-' + times[2] + ' ' + times[3] + ':' + times[4];
        return timestr;
    },
    /*#endregion*/

    /*#region 函数:getPageSizeByScreen(screenHeight, rowHeight)*/
    /*
    * 根据屏幕取得自适应显示记录条数
    */
    getPageSizeByScreen: function(screenHeight, rowHeight)
    {
        var num = parseInt(screenHeight / rowHeight);
        if (num < 5)
        {
            num = 5;
        }
        return num;
    },
    /*#endregion*/

    /*#region 函数:getContactsWindow(targetViewName, targetValueName, contactTypeText)*/
    /*
    * 打开人员选择向导(地址簿)窗口
    */
    getContactsWindow: function(targetViewName, targetValueName, contactTypeText)
    {
        x.wizards.getContactsWizardSingleton(targetViewName, targetValueName, contactTypeText);
    },
    /*#endregion*/

    /*#region 函数:addFollow(tag, followAccountId)*/
    /*
    * 添加关注
    */
    addFollow: function(tag, followAccountId)
    {
        main.forum.util.refresh = tag;
        var outString = '<?xml version="1.0" encoding="utf-8"?>';
        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[insert]]></action>';
        outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
        outString += '<followAccountId><![CDATA[' + followAccountId + ']]></followAccountId>';
        outString += '</ajaxStorage>';

        var options = {
            resultType: 'json',
            xml: outString
        };

        $.post(
                main.forum.util.followUrl,
                options,
                main.forum.util.addFollow_callback);
    },

    addFollow_callback: function(response)
    {
        var result = x.toJSON(response).message;

        switch (Number(result.returnCode))
        {
            case 0:
                // 关注用户成功 
                alert(result.value);
                if (main.forum.util.refresh)
                {
                    main.forum.my.follow.list.getMyFollowPages(main.forum.my.follow.list.pages.currentPage);
                }
                break;
            default:
                alert(result.value);
                break;
        }
    },
    /*#endregion*/

    /*#region 函数:deleteFollow(followAccountId)*/
    /*
    * 取消关注
    */
    deleteFollow: function(followAccountId)
    {
        var outString = '<?xml version="1.0" encoding="utf-8"?>';
        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[delete]]></action>';
        outString += '<applicationTag><![CDATA[' + $("#applicationTag").val() + ']]></applicationTag>';
        outString += '<followAccountId><![CDATA[' + followAccountId + ']]></followAccountId>';
        outString += '</ajaxStorage>';

        var options = {
            resultType: 'json',
            xml: outString
        };

        $.post(
                main.forum.util.followUrl,
                options,
                main.forum.util.deleteFollow_callback);
    },

    deleteFollow_callback: function(response)
    {
        var result = x.toJSON(response).message;

        switch (Number(result.returnCode))
        {
            case 0:
                // 取消关注用户成功 
                alert("取消关注成功！");
                main.forum.my.follow.list.getMyFollowPages(main.forum.my.follow.list.pages.currentPage);
                break;
            default:
                alert(result.value);
                break;
        }
    }
    /*#endregion*/
}

////禁用右键菜单、复制、选择文本
//document.oncontextmenu = function () { return false; }
//document.onselectstart = function () { return false; }
//document.onkeydown = function CtrlKeyDown() { if (event.keyCode == 67 || event.keyCode == 83) { return false } }
