x.register('main.entities.entity.schema.form');

main.entities.entity.schema.form = {

  /*#region 函数:checkObject()*/
  /*
  * 必填项的检测
  */
  checkObject: function()
  {
    if(x.customForm.checkDataStorage())
    {
      return false;
    }

    return true;
  },
  /*#endregion*/

  /*#region 函数:save()*/
  /*
  * 保存
  */
  save: function()
  {
    if(main.entities.entity.schema.form.checkObject())
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';
      outString += x.customForm.getDataStorage({ storageType: 'xml', includeajaxStorageNode: false });
      outString += '</ajaxStorage>';

      x.net.xhr('/api/kernel.entities.schema.save.aspx', outString, {
        waitingMessage: '正在提交数据，请稍后......',
        popResultValue: 1,
        callback: function(response)
        {
          x.page.refreshParentWindow();
          x.page.close();
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
  }
  /*#endregion*/
};

$(document).ready(main.entities.entity.schema.form.load);
