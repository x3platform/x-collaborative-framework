x.register('x.app');

x.app = {

  /*#region 函数:setColorStatusView(statusView)*/
  /**
   * 设置通用状态信息
   */
  setColorStatusView: function(statusView)
  {
    switch(statusView)
    {
      case '-1':
      case '草稿':
        return '<span style="color:tomato;" title="草稿" ><i class="fa fa-pencil-square"></i></span>';
      case '0':
      case '禁用':
        return '<span class="red-text" title="禁用" ><i class="fa fa-times-circle"></i></span>';
      case '1':
      case '启用':
        return '<span class="green-text" title="启用" ><i class="fa fa-check-circle"></i></span>';
      case '2':
      case '回收':
        return '<span class="gray-text" title="回收" ><i class="fa fa-recycle"></i></span>';
      default:
        return statusView;
    }
  },
  /*#endregion*/

  /*#region 函数:setColorDocStatusView(docStatusView)*/
  /**
   * 设置文档状态信息
   */
  setColorDocStatusView: function(docStatusView)
  {
    switch(docStatusView)
    {
      case 'Draft':
      case '草稿':
        return '<span style="color:tomato;" >草稿</span>';
      case 'Published':
      case '已发布':
        return '<span class="green-text" >已发布</span>';
      case 'Overdue':
      case '已过期':
        return '<span class="gray-text" >已过期</span>';
      case 'Abandon':
      case '废弃':
        return '<span class="gray-text" >废弃</span>';
      case 'Review':
      case '会签':
        return '<span style="color:orange;" >会签</span>';
      case 'ProofreadDraft':
      case '校稿前草稿':
        return '<span class="blue-text" >校稿前草稿</span>';
      case 'Proofreading':
      case '校稿中':
        return '<span class="blue-text" >校稿中</span>';
      case 'Approving':
      case '审批中':
        return '<span class="blue-text" >审批中</span>';
      case 'Rejected':
      case '被驳回':
        return '<span class="red-text" >被驳回</span>';
      case 'Archived':
      case '归档':
        return '<span >归档</span>';
      default:
        return docStatusView;
    }
  }
  /*#endregion*/
};