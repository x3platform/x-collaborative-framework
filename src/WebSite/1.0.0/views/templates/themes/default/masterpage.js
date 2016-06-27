var masterpage = {

  /*
  * 展开或关闭设置菜单.
  */
  openSettingMenu: function(index)
  {
    document.onclick = function(event)
    {
      masterpage.closeSettingMenu(event);
    };

    document.onmouseover = function(event)
    {
      masterpage.closeSettingMenu(event);
    };

    $('.header-account-menu').hide();

    $($('.header-account-menu')[index]).show();

    $('.header-account-menu')[0].style.left = (x.page.getElementLeft(x.dom.query('btnSettingMenu')[0]) - 171) + 'px';

    // 设置高度
    var height = document.body.clientHeight - 30;

    // document.title = $('.header-account-menu').height() + ' | ' + height;
    if($($('.header-account-menu')[0]).height() > height)
    {
      $($('.header-account-menu')[0]).height((height - 20));
    }
  },

  closeSettingMenu: function(event)
  {
    var target = window.event ? window.event.srcElement : event ? event.target : null;

    if(!x.css.check(target, 'setting-menu-show'))
    {
      $('.header-account-menu').hide();
    }
  },

  /*
  * 退出事件
  */
  logout: function()
  {
    x.net.xhr('/api/membership.member.quit.aspx?random=' + x.randomText.create(8), {
      callback: function(response)
      {
        try
        {
          document.execCommand("ClearAuthenticationCache", "false");

          var sessionIdentityName = $('#session-identity-name').val();

          x.cookies.remove(sessionIdentityName);

          x.page.close();
        }
        catch(ex)
        {
          x.debug.log(ex);
        }

        try
        {
          // 移除单点登录的Cookie信息
          var domain = $('#session-domain').val();

          var sessionIdentityName = $('#session-identity-name').val();

          x.cookies.remove(sessionIdentityName, '/', domain);
        }
        catch(ex)
        {
          x.debug.log(ex);
        }

        location.reload();
      }
    });
  },

  resize: function()
  {
    if($('.print-container').size() === 1)
    {
      // 打印模式
      $('.web-body')[0].className = '';
      $('.web-container')[0].className = '';
    }
    else if($('.x-ui-pkg-menu-slide-menu-container').size() > 0)
    {
      // 左右框架模式

      var width = x.page.getViewWidth() - 180;

      width = (width < 0) ? 1 : width;

      $('#web-container').css({ 'display': 'table' });
      $('.x-ui-pkg-menu-slide-menu-container').css({ 'display': 'table-cell' });
      $('.x-ui-pkg-menu-slide-menu-handle-bar').css({ 'display': 'table-cell' });
      $('#window-container').css({ 'display': 'table-cell' });

      // 菜单-关闭
      x.dom('#windowApplicationMenuCloseButton').on('click', function()
      {
        var wrapper = x.dom('#windowApplicationMenuContainer')[0];

        if(wrapper.style.display == 'table-cell')
        {
          x.dom('#windowApplicationMenuContainer').css({ 'display': 'none' });
          // x.dom('#windowApplicationMenuToggle').css({ 'width': '20px' });
          // x.dom('#windowApplicationMenuWrapper').css({ 'display': 'none' });
          x.dom('#windowApplicationMenuHandleBar').css({ 'width': '20px' });
          x.dom('#windowApplicationMenuOpenButton').css({ 'display': 'block' });
          x.dom('#windowApplicationMenuHandleBarLabel').css({ 'display': 'block' });
        }
      });

      // 菜单-开启
      x.dom('#windowApplicationMenuOpenButton').on('click', function()
      {
        var wrapper = x.dom('#windowApplicationMenuContainer')[0];

        if(wrapper.style.display == 'none')
        {
          // windowApplicationMenuHandleBarLabel
          // 菜单-开启
          x.dom('#windowApplicationMenuContainer').css({ 'display': 'table-cell' });
          // x.dom('#windowApplicationMenuToggle').css({ 'display': '' });
          x.dom('#windowApplicationMenuWrapper').css({ 'display': '' });
          x.dom('#windowApplicationMenuHandleBar').css({ 'width': '4px' });
          x.dom('#windowApplicationMenuOpenButton').css({ 'display': 'none' });
          x.dom('#windowApplicationMenuHandleBarLabel').css({ 'display': 'none' });
        }
      });
    }
    else
    {
      // 默认模式
      $('#window-container')[0].style.margin = 'auto';
    }

    // 设置高度
    if($('.print-container').size() === 1)
    {

    }
    else if($('.header').css('display') === '' || $('.header').css('display') === 'block')
    {
      var height = x.page.getViewHeight() - 39;

      height = (height < 0) ? 1 : height;

      // $('#window-container')[0].style.height = height + 'px';

      document.body.style.overflow = 'hidden';

      $('.web-body')[0].style.height = height + 'px';
      $('.web-container')[0].style.height = height + 'px';
      $('.web-container')[0].style.overflow = "auto";

      $('#windowApplicationMenuWrapper').css({
        'position': 'absolute',
        'width': '180px',
        'height': (height - 16) + 'px',
        'overflow': 'auto',
        'padding-top': '10px'
      });

      $('#window-container').css({
        'height': height + 'px',
        'overflow': 'auto'
      });

      // 设置高度
      var height = x.page.getViewHeight();

      var freezeHeight = 0;

      $('.x-freeze-height').each(function(index, node)
      {
        freezeHeight += $(node).outerHeight();
      });

      var freezeTableHeadHeight = $('#window-main-table-body .table-freeze-head').outerHeight();
      var freezeTableSidebarSearchHeight = $('#window-main-table-body .table-sidebar-search').outerHeight();

      $('#treeViewContainer').css({
        'height': (height - freezeHeight - freezeTableSidebarSearchHeight) + 'px',
        'width': '200px',
        'overflow': 'auto'
      });

      $('#window-main-table-body .table-freeze-body').css(
      {
        'height': (height - freezeHeight - freezeTableHeadHeight) + 'px',
        'overflow-y': 'scroll'
      });

      $('.table-freeze-head-padding').css({ width: x.page.getScrollBarWidth().vertical, display: (x.page.getScrollBarWidth().vertical == 0 ? 'none' : '') });
    }
    else
    {
      // 默认模式
      $('#window-main-table').css({
        'border': '1px solid #ccc',
        'border-top': '3px solid #ccc',
        'margin': '20px auto',
        'background-color': '#fff'
      });
    }
  },

  /*
  * 页面加载事件
  */
  load: function()
  {
    // 框架页面自动调整
    masterpage.resize();
    // 禁止拷贝
    x.page.forbidCopy.listen();
    // 加载表单元素特性
    x.dom.features.bind();
  }
};

$(document).ready(masterpage.load);
// 重新调整页面大小
$(window).resize(masterpage.load);
// 重新调整页面大小
$(document.body).resize(masterpage.load);