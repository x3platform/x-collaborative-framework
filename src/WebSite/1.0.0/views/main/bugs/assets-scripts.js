x.register('main.bugs.util');

main.bugs.util = {

  /*#region 函数:setColorStatusView(status)*/
  /*
  * 获取落实情况列表
  */
  setColorStatusView: function(status)
  {
    switch(status)
    {
      case '0':
      case '新问题':
        return '<span style="color:tomato;" >新问题</span>';
      case '1':
      case '处理中':
        return '<span class="blue-text" >处理中</span>';
      case '2':
      case '已解决':
        return '<span class="green-text" >已解决</span>';
      case '3':
      case '延后处理':
        return '<span style="color:orange;" >延后处理</span>';
      case '4':
      case '无法修复':
        return '<span >无法修复</span>';
      case '5':
      case '已关闭':
        return '<span class="gray-text" >已关闭</span>';
      case '6':
      case '被打回':
        return '<span class="red-text" >被打回</span>';
      default:
        return statusView;
    }
  },
  /*#endregion*/

  /*#region 函数:setColorPriorityView(priority)*/
  /*
  * 获取落实情况列表
  */
  setColorPriorityView: function(priority)
  {
    switch(priority)
    {
      case '1':
      case '重要':
        return '<span class="blue-text" >重要</span>';
      case '2':
      case '紧急':
        return '<span class="red-text" >紧急</span>';
      default:
        return '';
    }
  },
  /*#endregion*/

  /*
  * 反馈
  */
  comment: {

    pages: x.page.newPagingHelper(),

    /*
    * 创建对象列表的视图
    */
    getObjectsView: function(list)
    {
      var outString = '';

      var counter = 0;

      var classNameValue = '';

      outString += '<table class="table">';
      outString += '<tr class="table-row-title">';
      outString += '<td style="width:120px">姓名</td>';
      outString += '<td >内容</td>';
      outString += '<td style="width:160px">时间</td>';
      outString += '</tr>';

      x.each(list, function(index, node)
      {
        outString += '<tr ' + ((index + 1 == list.length) ? ' style="border-bottom:2px solid #333;" ' : '') + '>';
        outString += '<td>';
        outString += node.accountName;
        // if(node.accountId == memberToken.id)
        // {
        //  outString += ' - <a href="javascript:main.bugs.util.comment.confirmDelete(\'' + node.id + '\');">删除</a>';
        // }
        outString += '</td>';
        outString += '<td>' + node.content + '</td>';
        outString += '<td>' + x.date.create(node.createDate).toString('yyyy-MM-dd HH:mm:ss') + '</td>';
        outString += '</tr>';

        counter++;
      });

      classNameValue = (counter % 2 == 0) ? 'table-row-normal' : 'table-row-alternating';

      outString += '<tr class="' + classNameValue + '">';
      outString += '<td colspan="3">';
      outString += '<div class="window-bugzilla-comment-wrapper" style="width:95%">';
      outString += '<div class="window-bugzilla-comment-title" >填写反馈信息</div>';
      outString += '<div class="window-bugzilla-comment-content" >';
      outString += '<input id="comment-title" name="comment-title" type="hidden" value="' + $('#title').val() + '" />';
      outString += '<textarea id="comment-content" name="comment-content" class="textarea-normal" style="text-align:left;height:120px;width:100%"></textarea>';
      outString += '</div>';

      outString += '<div class="window-bugzilla-comment-button" style="margin-top:4px;" >';
      outString += '<a class="btn btn-default" href="javascript:main.bugs.util.comment.save();">提交</a>';
      outString += '</div>';
      outString += '</td>';
      outString += '</tr>';

      outString += '</table>';

      return outString;
    },

    /*
    * 查询全部
    */
    findAll: function()
    {
      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<ajaxStorage>';

      outString += '<request><![CDATA[BugId=##' + $('#id').val() + '##]]></whereClause>';
      outString += '<length><![CDATA[0]]></length>';
      outString += '</request>';

      x.net.xhr('/api/bug.comment.findAll.aspx', outString, {
        callback: function(response)
        {
          var outString = '';

          var result = x.toJSON(response);

          var list = result.data;

          var containerHtml = main.bugs.util.comment.getObjectsView(list);

          $('#window-bug-comment-container').html(containerHtml);
        }
      });
    },

    /*
    * 检测对象的必填数据
    */
    checkObject: function()
    {
      if($('#comment-title').val() == '')
      {
        $('#comment-title')[0].focus();
        alert('必须填写【标题】信息');
        return false;
      }

      if($('#comment-title').val() == '')
      {
        $('#comment-title')[0].focus();
        alert('必须填写【内容】信息');
        return false;
      }

      return true;
    },

    save: function()
    {
      // 1.数据检测

      if(!main.bugs.util.comment.checkObject()) { return; }

      // 2.发送数据

      var outString = '<?xml version="1.0" encoding="utf-8" ?>';

      outString += '<request>';
      outString += '<action><![CDATA[save]]></action>';
      outString += '<id><![CDATA[' + x.guid.create() + ']]></id>';
      outString += '<bugId><![CDATA[' + $('#id').val() + ']]></bugId>';
      outString += '<title><![CDATA[' + $('#comment-title').val() + ']]></title>';
      outString += '<content><![CDATA[' + $('#comment-content').val() + ']]></content>';
      outString += '</request>';

      x.net.xhr('/api/bug.comment.save.aspx', outString, {
        callback: function(response)
        {
          main.bugs.util.comment.findAll();
        }
      });
    },

    /*
    * 删除对象
    */
    confirmDelete: function(ids)
    {
      if(confirm('确定删除?'))
      {
        var outString = '<?xml version="1.0" encoding="utf-8"?>';

        outString += '<request>';
        outString += '<action><![CDATA[delete]]></action>';
        outString += '<ids><![CDATA[' + ids + ']]></ids>';
        outString += '</request>';

        x.net.xhr('/api/bug.comment.delete.aspx?ids=' + ids, {
          callback: function(response)
          {
            main.bugs.util.comment.findAll();
          }
        });
      }
    }
  }
};