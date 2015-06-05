x.register('main.customizes.customize.page.form');

main.customizes.customize.page.form = {

  page: undefined,

  dialogWrapperSelector: '.x-ui-pkg-customize-dialog-wrapper',

  currentDialogType: '',

  setMenu: function(html)
  {
    $('.x-ui-pkg-customize-menu')[0].style.padding = '0 0 0 10px';
    $('.customize-extension-menu').remove();

    if(html == '<a href="javascript:main.customizes.customize.page.form.edit();">编辑页面</a>')
    {
      html = '<div class="header-account-menu-line setting-menu-show customize-extension-menu"></div>'
           + '<div class="header-account-menu-item setting-menu-show customize-extension-menu">'
           + '<a class="setting-menu-show" href="javascript:main.customizes.customize.page.form.edit();" >'
           + '<span class="menu-text setting-menu-show">编辑页面</span>'
           + '<span class="menu-discption setting-menu-show">设置相关的自定义参数.</span>'
           + '</a>'
           + '</div>';
    }
    else
    {
      html = '<div class="header-account-menu-line setting-menu-show customize-extension-menu"></div>'
           + '<div class="header-account-menu-item setting-menu-show customize-extension-menu">'
           + '<a class="setting-menu-show" href="javascript:main.customizes.customize.page.form.save();" >'
           + '<span class="menu-text setting-menu-show">保存页面</span>'
           + '<span class="menu-discption setting-menu-show">保存当前页面.</span>' + '</a>'
           + '</div>';
    }

    $('#navigationCustomizeMenu').after(html);
  },

  edit: function()
  {
    $('#btnEdit').unbind('click');

    $('#btnEdit').html('<i class="fa fa-floppy-o" title="保存"></i> 保存');

    $('#btnEdit').on('click', function()
    {
      main.customizes.customize.page.form.save();
    });

    var page = x.ui.pkg.customizes.newPage($('#customize-page-name').val(), {
      authorizationObjectType: $('#customize-page-authorizationObjectType').val(),
      authorizationObjectId: $('#customize-page-authorizationObjectId').val(),
      name: $('#customize-page-name').val()
      // pageUrl: '/services/Elane/X/Web/Customize/Ajax.PageWrapper.aspx',
      // widgetZoneUrl: '/services/Elane/X/Web/Customize/Ajax.WidgetZoneWrapper.aspx',
      // widgetUrl: '/services/Elane/X/Web/Customize/Ajax.WidgetWrapper.aspx',
      // widgetInstanceUrl: '/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx'
    });

    main.customizes.customize.page.form.page = page;

    var menu = '<a href="javascript:main.customizes.customize.page.form.toggle(\'widget\');">页面部件</a> | '
        + '<a href="javascript:main.customizes.customize.page.form.toggle(\'widgetZone\');">页面框架</a> | '
        + '<a href="javascript:main.customizes.customize.page.form.save();">保存页面</a>';

    main.customizes.customize.page.form.setMenu(menu);

    main.customizes.customize.page.form.toggle('widget');
  },

  toggle: function(type)
  {
    var currentDialogType = main.customizes.customize.page.form.currentDialogType;

    var dialogWrapperSelector = main.customizes.customize.page.form.dialogWrapperSelector;

    var display = $(dialogWrapperSelector).css('display');

    if(type == 'widget')
    {
      if(currentDialogType != type || display == 'none')
      {
        x.ui.pkg.customizes.widget.openDialog();
      }
      else
      {
        x.ui.pkg.customizes.widget.closeDialog();
      }
    }
    else
    {
      if(currentDialogType != type || display == 'none')
      {
        x.ui.pkg.customizes.widgetZone.openDialog();
      }
      else
      {
        x.ui.pkg.customizes.widgetZone.closeDialog();
      }
    }

    main.customizes.customize.page.form.currentDialogType = type;
  },

  /**
  * 保存页面
  */
  save: function()
  {
    var page = main.customizes.customize.page.form.page;

    if(typeof (page) !== 'undefined')
    {
      page.save();
    }

    $('#btnEdit').unbind('click');

    $('#btnEdit').html('<i class="fa fa-edit" title="编辑"></i> 编辑');

    $('#btnEdit').on('click', function()
    {
      main.customizes.customize.page.form.edit();
    });

    // main.customizes.customize.page.form.setMenu('<a href="javascript:main.customizes.customize.page.form.edit();">编辑页面</a>');
  },

  load: function()
  {
    x.ui.pkg.tabs.newTabs();

    main.customizes.customize.page.form.edit();
  }
};

$(document).ready(main.customizes.customize.page.form.load);