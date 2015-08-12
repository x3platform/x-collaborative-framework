x.register('main.forum.thread.form');

main.forum.thread.form = {

  /*
  * 检测对象的必填数据
  */
  checkObject: function()
  {
    if(x.dom.data.check())
    {
      return false;
    }

    if($('#content').val() == '')
    {
      $('#content')[0].focus();
      alert('必须填写【内容】。');

      return false;
    }

    return true;
  },

  /*#region 函数:save(status)*/
  save: function(status)
  {
    // 设置编辑器
    var editor = CKEDITOR.instances['content']
    var content = editor.getData();

    $('#content').val(content);


    // 1.数据检测
    if(main.forum.thread.form.checkObject())
    {
      //设置帖子状态
      $('#status').val(status);

      // 2.发送数据
      var outString = '<?xml version="1.0" encoding="utf-8"?>';

      outString += '<request>';
      outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
      outString += '</request>';

      x.net.xhr('/api/forum.thread.save.aspx', outString, {
        waitingMessage: i18n.net.waiting.saveTipText,
        // popResultValue: 1,
        callback: function(response)
        {
          x.page.refreshParentWindow();

          x.page.close();
        }
      });
    }
  },
  /*#endregion*/

  /*#region 函数:getForumCategoryWizardWindow()*/
  /*
  * 打开论坛版块选择向导窗口
  */
  getForumCategoryWizardWindow: function()
  {
    var storage = '';
    var viewArray = '';
    var valueArray = '';

    //
    // 关键代码 开始
    //

    // main.forum.forumcategorywizard.localStorage = '{"text":"' + $('#categoryIndex').val() + '","value":"' + $('#categoryIndex').val() + '"}';

    // 保存回调函数
    main.forum.forumcategorywizard.save_callback = function(response)
    {
      var resultView = '';
      var resultValue = '';

      var node = x.toJSON(response);

      resultView += node.text + ';';
      resultValue += node.value + ';';

      if(resultValue.substr(resultValue.length - 1, 1) == ';')
      {
        resultView = resultView.substr(0, resultView.length - 1)
        resultValue = resultValue.substr(0, resultValue.length - 1);
      }

      $('#categoryIndex').val(resultView);
      $('#categoryId').val(resultValue);

      main.forum.forumcategorywizard.localStorage = response;
    }

    // 取消回调函数
    // 注:执行完保存回调函数后, 默认执行取消回调函数.
    main.forum.forumcategorywizard.cancel_callback = function(response)
    {
      if(main.forum.forumcategorywizard.maskWrapper != '')
      {
        main.forum.forumcategorywizard.maskWrapper.close();
      }
    }

    //
    // 关键代码 结束
    //

    // 非模态窗口, 需要设置.
    main.forum.forumcategorywizard.maskWrapper = main.forum.thread.form.maskWrapper;

    // 加载地址簿信息
    main.forum.forumcategorywizard.load();
  },
  /*#endregion*/

  /*#region 函数:setAuthorView()*/
  /*
  * 设置作者视图
  */
  setAuthorView: function()
  {
    if($('#anonymous')[0].checked)
    {
      $('#authorView').html('匿名');
    }
    else
    {
      $('#authorView').html($('#accountName').val());
    }
  },
  /*#endregion*/

  /*#region 函数:load()*/
  /*
  * 页面加载事件
  */
  load: function()
  {
    CKEDITOR.replace('content');

    // -------------------------------------------------------
    // 设置上传文件事件
    // -------------------------------------------------------

    $('#fileupload').fileupload({
      url: '/api/attachmentStorage.util.file.upload.aspx?clientId=' + x.dom('#session-client-id').val() + '&clientSignature=' + x.dom('#session-client-signature').val() + '&timestamp=' + x.dom('#session-timestamp').val() + '&nonce=' + x.dom('#session-nonce').val(),
      formData: {
        entityId: $('#id').val(),
        entityClassName: $('#entityClassName').val(),
        attachmentEntityClassName: '',
        attachmentFolder: 'forum'
      },
      // 返回结果类型
      dataType: 'text',
      done: function(e, data)
      {
        // x.debug.log(data.result);
        x.debug.log(data.result);

        x.app.files.findAll({
          entityId: $("#id").val(),
          entityClassName: $('#entityClassName').val(),
          targetViewName: "window-attachment-wrapper"
        });
      },
      progressall: function(e, data)
      {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progress .progress-bar').css(
            'width',
            progress + '%'
        );
      }
    })
  }
  /*#endregion*/
};

$(document).ready(main.forum.thread.form.load);