(function(x, window)
{
    var main = {

        /*#region 函数:invite(friendAccountId)*/
        /**
        * 邀请好友
        */
        invite: function(friendAccountId)
        {
            x.net.xhr('/api/membership.accountFriend.invite.aspx?friendAccountId=' + friendAccountId, {
                waitingType: 'mini',
                callback: function(response)
                {
                    var result = x.toJSON(response).message;

                    x.msg(result.value);
                }
            });
        },
        /*#endregion*/

        /*#region 函数:load()*/
        /**
         * 页面加载事件
         */
        load: function()
        {
            // -------------------------------------------------------
            // 绑定事件
            // -------------------------------------------------------

            x.dom('#btnInvite').on('click', function()
            {
                main.invite(x.dom('#id').val());
            });
        }
        /*#endregion*/
    }

    // 注册到 Window 对象
    window.main = main;

    x.dom.ready(main.load);

}(x, window));
