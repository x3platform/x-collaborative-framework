// -*- ecoding=utf-8 -*-

/**
* @namespace tabs
* @memberof x.ui.pkg
* @description 页签
*/
x.ui.pkg.tabs = {

  /**
  * -*- 修改自DOMtab, 具体参考以下信息. -*-
  *
  * DOMtab Version 3.1415927
  * Updated March the First 2006
  * written by Christian Heilmann
  * check blog for updates: http://www.wait-till-i.com
  */
  defaults: {
    /** 样式名称前缀 */
    classNamePrefix: 'x-ui-pkg-tabs',
    /** 页签特性是否已加载标识属性名称 */
    tabLoadedAttributeName: 'x-ui-pkg-tabs-loaded',
    tabWrapperClass: 'x-ui-pkg-tabs-wrapper', // class to trigger tabbing
    tabMenuClass: 'x-ui-pkg-tabs-menu', // class of the menus
    tabContainerClass: 'x-ui-pkg-tabs-container', //  class to containers
    tabContainerHeadHiddenClass: 'x-ui-pkg-tabs-container-head-hidden', //  class to containers
    activeClass: 'active', // 当前链接样式
    contentElements: 'div', // elements to loop through
    backToLinks: /#top/, // pattern to check "back to top" links
    printId: 'windowTabsPrintWiew', // id of the print all link
    showAllLinkText: '显示全部内容', // text for the print all link
    prevNextIndicator: 'tabsprevnext', // class to trigger prev and next links
    prevNextClass: 'x-ui-pkg-tabs-prevnext', // class of the prev and next list
    // 上一页链接文本
    prevLabel: '上一页',
    // 下一页链接文本
    nextLabel: '下一页',
    // 上一页默认样式
    prevClass: 'prev',
    // 下一页默认样式                    
    nextClass: 'next'
  },

  /**
  * 页签
  * @description 页签
  * @class Tabs
  * @constructor newTabs
  * @memberof x.ui.pkg.tabs
  * @param {object} options 选项
  */
  newTabs: function(options)
  {
    var options = x.ext({}, x.ui.pkg.tabs.defaults, options || {});

    var tabs = {
      /*
      * 检测地址
      */
      //    checkUrl: function()
      //    {
      //        var id, cur;
      //        var loc = window.location.toString();

      //        loc = /#/.test(loc) ? loc.match(/#(\w.+)/)[1] : '';

      //        if (loc === '') { return; }

      //        var elm = document.getElementById(loc);

      //        if (!elm) { return; }

      //        var parentMenu = elm.parentNode.parentNode.parentNode;

      //        parentMenu.currentSection = loc;

      //        // 链接地址中带有锚点信息时, 不隐藏菜单.
      //        // parentMenu.getElementsByTagName(x.ui.pkg.tabs.contentElements)[0].style.display = 'none';

      //        x.css.remove(parentMenu.getElementsByTagName('a')[0].parentNode, options.activeClass);

      //        var links = parentMenu.getElementsByTagName('a');
      //        for (var i = 0; i < links.length; i++)
      //        {
      //            if (!links[i].getAttribute('href')) { continue; }
      //            if (!/#/.test(links[i].getAttribute('href').toString())) { continue; }

      //            id = links[i].href.match(/#(\w.+)/)[1];

      //            if (id === loc)
      //            {
      //                cur = links[i].parentNode.parentNode;
      //                x.css.add(links[i].parentNode, options.activeClass);
      //                break;
      //            }
      //        }

      //        this.changeTab(elm, 1);
      //        // elm.focus();
      //        cur.currentLink = links[i];
      //        cur.currentSection = loc;
      //    },

      /**
      * 显示全部
      */
      showAll: function(event)
      {
        x.query('#' + options.printId).parentNode.removeChild(x.query('#' + options.printId));

        var tempElement = document.getElementsByTagName('div');

        for(var i = 0;i < tempElement.length;i++)
        {
          if(!x.css.check(tempElement[i], options.tabWrapperClass))
          {
            continue;
          }

          var sec = tempElement[i].getElementsByTagName(x.ui.pkg.tabs.contentElements);

          for(var j = 0;j < sec.length;j++)
          {
            sec[j].style.display = 'block';
          }
        }

        // menu ul    
        tempElement = document.getElementsByTagName('ul');

        for(i = 0;i < tempElement.length;i++)
        {
          if(!x.css.check(tempElement[i], options.prevNextClass)) { continue; }
          tempElement[i].parentNode.removeChild(tempElement[i]);
          i--;
        }

        // title h2
        tempElement = document.getElementsByTagName('h2');

        for(i = 0;i < tempElement.length;i++)
        {
          if(x.css.check(tempElement[i], options.tabContainerHeadHiddenClass))
          {
            x.css.remove(tempElement[i], options.tabContainerHeadHiddenClass);
          }
        }

        x.event.stopPropagation(e);
      },

      /*
      *
      */
      addPrevNext: function(menu)
      {
        var temp;

        // 源代码: var sections = menu.getElementsByTagName(x.ui.pkg.tabs.contentElements);
        var sections = $('.' + options.tabContainerClass);

        for(var i = 0;i < sections.length;i++)
        {
          temp = x.ui.pkg.tabs.createPrevNext();

          if(i === 0)
          {
            temp.removeChild(temp.getElementsByTagName('li')[0]);
          }

          if(i === sections.length - 1)
          {
            temp.removeChild(temp.getElementsByTagName('li')[1]);
          }

          temp.i = i; // h4xx0r!
          temp.menu = menu;
          sections[i].appendChild(temp);
        }
      },

      /*
      *
      */
      removeBackLinks: function(menu)
      {
        var links = menu.getElementsByTagName('a');
        for(var i = 0;i < links.length;i++)
        {
          if(!options.backToLinks.test(links[i].href)) { continue; }
          links[i].parentNode.removeChild(links[i]);
          i--;
        }
      },

      /*
      * 创建Tabs菜单
      */
      createTabMenu: function(elements)
      {
        var me = this;

        var id;

        var menu = elements.getElementsByTagName('ul');

        var thismenu;

        for(var i = 0;i < menu.length;i++)
        {
          if(x.css.check(menu[i], options.tabMenuClass))
          {
            var thismenu = menu[i];
            break;
          }
        }

        if(!thismenu) { return; }

        thismenu.currentSection = '';
        thismenu.currentLink = '';

        var links = thismenu.getElementsByTagName('a');

        for(i = 0;i < links.length;i++)
        {
          if(!/#/.test(links[i].getAttribute('href').toString())) { continue; }

          id = links[i].href.match(/#(\w.+)/)[1];

          if(document.getElementById(id))
          {
            x.event.add(links[i], 'click', function(event) { me.showTab(event) }, false);

            // 源码: links[i].onclick = function () { alert($(this)[0].href ); return false; } // safari hack
            links[i].onclick = function()
            {
              x.call(options.bind);
              return false;
            };

            this.changeTab(document.getElementById(id), 0);
          }
        }

        if(links[0].href.match(/#(\w.+)/) != null)
        {
          id = links[0].href.match(/#(\w.+)/)[1];

          if(document.getElementById(id))
          {
            this.changeTab(document.getElementById(id), 1);
            thismenu.currentSection = id;
            thismenu.currentLink = links[0];
            x.css.add(links[0].parentNode, options.activeClass);
          }
        }
      },

      createPrevNext: function()
      {
        var me = this;

        // this would be so much easier with innerHTML, darn you standards fetish!
        var temp = document.createElement('ul');
        temp.className = options.prevNextClass;
        temp.appendChild(document.createElement('li'));
        temp.getElementsByTagName('li')[0].appendChild(document.createElement('a'));
        temp.getElementsByTagName('a')[0].setAttribute('href', '#');
        temp.getElementsByTagName('a')[0].innerHTML = options.prevLabel;
        temp.getElementsByTagName('li')[0].className = options.prevClass;
        temp.appendChild(document.createElement('li'));
        temp.getElementsByTagName('li')[1].appendChild(document.createElement('a'));
        temp.getElementsByTagName('a')[1].setAttribute('href', '#');
        temp.getElementsByTagName('a')[1].innerHTML = options.nextLabel;
        temp.getElementsByTagName('li')[1].className = options.nextClass;

        x.event.add(temp.getElementsByTagName('a')[0], 'click', function(event) { me.navTabs(event) }, false);
        x.event.add(temp.getElementsByTagName('a')[1], 'click', function(event) { me.navTabs(event) }, false);

        // safari fix
        temp.getElementsByTagName('a')[0].onclick = function() { return false; };
        temp.getElementsByTagName('a')[1].onclick = function() { return false; };

        return temp;
      },

      /*
      *
      */
      navTabs: function(event)
      {
        // x.debug.log('x.ui.pkg.tabs.navTabs(e)');

        var li = x.ui.pkg.tabs.getTarget(event);
        var menu = li.parentNode.parentNode.menu;
        var count = li.parentNode.parentNode.i;

        // 源代码: var sections = menu.getElementsByTagName(x.ui.pkg.tabs.contentElements);
        var sections = $('.' + options.tabContainerClass);

        // x.debug.log(x.ui.pkg.tabs.tabContainerClass);

        var links = menu.getElementsByTagName('a');
        var otherCount = (li.parentNode.className === options.prevClass) ? count - 1 : count + 1;

        sections[count].style.display = 'none';
        x.css.remove(links[count].parentNode, options.activeClass);

        sections[otherCount].style.display = 'block';
        x.css.add(links[otherCount].parentNode, options.activeClass);

        var parent = links[count].parentNode.parentNode;
        parent.currentLink = links[otherCount];
        parent.currentSection = links[otherCount].href.match(/#(\w.+)/)[1];

        x.event.stopPropagation(e);
      },

      /*
      * 更改Tab状态
      */
      changeTab: function(element, status)
      {
        do
        {
          element = element.parentNode;
        }
        while(element.nodeName.toLowerCase() != options.contentElements);

        element.style.display = (status === 0) ? 'none' : 'block';
      },

      /*
      *
      */
      showTab: function(event)
      {
        // x.debug.log('x.ui.pkg.tabs.showTab(e)');

        var tabWrapperTarget = this.getTabWrapperTarget(event);

        // title h2 hidden
        var titleTarget = document.getElementsByTagName('h2');

        for(i = 0;i < titleTarget.length;i++)
        {
          if(!x.css.check(titleTarget[i], options.tabContainerHeadHiddenClass))
          {
            x.css.add(titleTarget[i], options.tabContainerHeadHiddenClass);
          }
        }

        // container hidden
        var sections = x.queryAll('.' + options.tabContainerClass, tabWrapperTarget);

        for(var i = 0;i < sections.length;i++)
        {
          sections[i].style.display = 'none';
        }

        // set target

        var target = this.getTarget(event);

        // x.debug.log('e.target.id:' + target.id);

        if(target.parentNode.parentNode.currentSection !== '')
        {
          this.changeTab(document.getElementById(target.parentNode.parentNode.currentSection), 0);
          x.css.remove(target.parentNode.parentNode.currentLink.parentNode, options.activeClass);
        }

        var id = target.href.match(/#(\w.+)/)[1];

        target.parentNode.parentNode.currentSection = id;
        target.parentNode.parentNode.currentLink = target;
        x.css.add(target.parentNode, options.activeClass);
        this.changeTab(document.getElementById(id), 1);

        document.getElementById(id).focus();
        x.event.stopPropagation(event);
      },

      /*
      * helper methods
      */
      getTabWrapperTarget: function(event, tabWrapperClass)
      {
        var target = x.event.getTarget(event);

        if(!target) { return false; }

        var count = 0;

        while(target.nodeName.toLowerCase() != 'div' && target.className == tabWrapperClass)
        {
          target = target.parentNode;

          if(count++ > 5)
          {
            return false;
          }
        }
        return target;
      },

      getTarget: function(event)
      {
        var target = x.event.getTarget(event);

        if(!target) { return false; }

        if(target.nodeName.toLowerCase() != 'a') { target = target.parentNode; }

        return target;
      },

      create: function(options)
      {
        var tempElements = document.getElementsByTagName('div');

        for(var i = 0;i < tempElements.length;i++)
        {
          if(!x.css.check(tempElements[i], options.tabWrapperClass))
          {
            continue;
          }

          if(x.dom.attr(tempElements[i], options.tabLoadedAttributeName) != '1')
          {
            if(tempElements[i].id == '')
            {
              tempElements[i].id = x.randomText.create(8);
            }

            // x.debug.log('name:' + tempElements[i].id);

            this.createTabMenu(tempElements[i]);

            this.removeBackLinks(tempElements[i]);

            if(x.css.check(tempElements[i], options.prevNextIndicator))
            {
              this.addPrevNext(tempElements[i]);
            }

            // x.ui.pkg.tabs.checkUrl();

            // 加载完毕后,加个tabsLoaded标识,避免重复加载效果.
            x.dom.attr(tempElements[i], options.tabLoadedAttributeName, '1');
          }
        }

        //
        if(x.query('#' + options.printId) && !x.query('#' + options.printId).getElementsByTagName('a')[0])
        {
          var newlink = document.createElement('a');
          newlink.setAttribute('href', '#');
          x.event.add(newlink, 'click', this.showAll, false);
          newlink.onclick = function() { return false; }; // safari hack
          newlink.appendChild(document.createTextNode(x.ui.pkg.tabs.showAllLinkText));
          document.getElementById(options.printId).appendChild(newlink);
        }
      }
    };

    tabs.create(options);

    return tabs;
  }
};
