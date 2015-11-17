x.register('main.connect.overview');

main.connect.overview = {

  resetAppSecret: function()
  {
    x.net.xhr('/api/connect.resetAppSecret.aspx?appKey=' + $('#appKey').val(), {
      waitingMessage: i18n.net.waiting.commitTipText,
      callback: function(response)
      {
        var result = x.toJSON(response);

        $('#appSecretView').html(result.data.appSecret);
      }
    });

  },

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    // -------------------------------------------------------
    // 绑定事件
    // -------------------------------------------------------

    // [空]
  }
  /*#endregion*/
};

$(document).ready(main.connect.overview.load);