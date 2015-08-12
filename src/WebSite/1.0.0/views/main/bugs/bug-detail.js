$(document).ready(function()
{
  var entityId = $("#id").val();
  var entityClassName = $("#entityClassName").val();

  // 初始化页签控件
  x.ui.pkg.tabs.newTabs();

  $('#contentWrapper')[0].style.overflow = 'auto';
  $('#contentWrapper')[0].innerHTML = $('#contentWrapper')[0].innerHTML.replace(/\n/g, '<br />');

  x.app.files.findAll({
    entityId: entityId,
    entityClassName: entityClassName,
    readonly: 1,
    targetViewName: "window-attachment-wrapper"
  });

  // var entityId = $("#id").val();
  // var entityClassName = 'Elane.X.Bugzilla.Model.BugzillaInfo, Elane.X.Bugzilla';

  // 加载附件信息
  /*
  x.customForm.attachmentStorage.findAll(entityId, entityClassName, function(response)
  {
    x.net.fetchException(response);

    var outString = '';

    var result = x.toJSON(response);

    var list = result.ajaxStorage;

    list.each(function(node, index)
    {
      outString += '<div>';
      outString += '<span class="list-item" style="width:300px">';

      if(node.fileType == '.jpg' || node.fileType == '.png' || node.fileType == '.gif')
      {
        outString += '<a href="javascript:site.v1.apps.pages.bugzilla.util.attachment.preview(\'/attachment/archive/' + node.id + '.aspx\');" >' + node.attachmentName + '(' + x.expression.formatNumberRound2(node.fileSize / 1024) + 'KB)</a>';
      } else
      {
        outString += '<a href="/attachment/archive/' + node.id + '.aspx" target="_blank">' + node.attachmentName + '(' + x.expression.formatNumberRound2(node.fileSize / 1024) + 'KB)</a>';
      }
      outString += '</span>';
      outString += '</div>';
    });

    $('.window-attachment-wrapper').html(outString);

    if(list.length > 0)
    {
      $('.window-attachment-wrapper').css('display', '');
      // $('.window-attachment-wrapper').css('margin-bottom', '8px');
    }
  });
  */

  // 加载评论信息
  main.bugs.util.comment.findAll();
});
