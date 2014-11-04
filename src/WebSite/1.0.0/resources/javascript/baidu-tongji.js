setTimeout(function()
{
    try
    {
        // 百度统计代码
        var tongjiToken = '50ec37835e145af07441db70959c8575'; // => x3platform.com
        var tongji = document.createElement('script');
        tongji.type = 'text/javascript';
        tongji.async = true;
        tongji.src = (('https:' == document.location.protocol) ? 'https://' : 'http://') + 'hm.baidu.com/h.js?' + tongjiToken;

        var script = document.getElementsByTagName('script')[0];

        script.parentNode.insertBefore(tongji, script);
    }
    catch (ex)
    {
        if (typeof (console) !== 'undefined')
        {
            console.error(ex);
        }
    }
}, 1000);
