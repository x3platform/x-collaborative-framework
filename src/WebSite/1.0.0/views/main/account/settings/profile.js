x.register('main.account.profile');

main.account.profile = {

  /*#region 函数:setMemberCard()*/
  /*
  * 设置个人信息
  */
  setMemberCard: function()
  {
    if(!x.dom.data.check())
    {
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml' });
      // outString += '<jobBegindate><![CDATA[' + $("#entryDate").val() + ']]></jobBegindate>';
      // outString += '<employeeId><![CDATA[' + $("#code").val() + ']]></employeeId>';
      outString += '</request>';

      x.net.xhr('/api/hr.general.setMemberCard.aspx', outString, {
        waitingMessage: i18n.net.waiting.saveTipText,
        popResultValue: 1,
        callback: function(response)
        {
          // x.page.refreshParentWindow();
          // x.page.close();
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    x.ui.pkg.tabs.newTabs();
  }
  /*#endregion*/
}

x.dom.ready(main.account.profile.load);