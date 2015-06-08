// -*- ecoding=utf-8 -*-

/**
* @namespace tree
* @memberof x.ui.pkg
* @description 树
*/
x.ui.pkg.tree = {

  // 修改自dTree, 具体参考以下信息. -*-
  // dTree - http://www.destroydrop.com/javascripts/tree/

  /**
  * 默认配置信息
  */
  defaults: {
    /** 样式名称默认前缀 */
    classNamePrefix: 'x-ui-pkg-tree',
    /** 图片路径 */
    imagePathRoot: '/resources/images/tree/',
    target: null,
    folderLinks: true,
    useSelection: true,
    useCookies: true,
    // 显示线
    useLines: true,
    uselementIcons: true,
    useStatusText: false,
    // 关闭同级的其他节点
    closeSameLevel: true,
    inOrder: false,
    ajaxMode: false

  },

  // 缓存
  cache: {},

  /**
  * 树视图
  * @description 树视图
  * @class TreeView
  * @constructor newTreeView
  * @memberof x.ui.pkg.tree
  * @param {object} options 选项
  */
  newTreeView: function(options)
  {
    var options = x.ext({}, x.ui.pkg.tree.defaults, options || {});

    // 图片路径
    var imagePath = options.imagePathRoot;

    var treeView =
    {
      // 配置
      options: null,

      icon: {
        root: imagePath + 'tree_icon.gif',
        folder: imagePath + 'folder.gif',
        folderOpen: imagePath + 'folderOpen.gif',
        node: imagePath + 'leaf.gif',
        empty: imagePath + 'empty.gif',
        line: imagePath + 'line.gif',
        join: imagePath + 'join.gif',
        joinBottom: imagePath + 'joinbottom.gif',
        plus: imagePath + 'plus.gif',
        plusBottom: imagePath + 'plusbottom.gif',
        minus: imagePath + 'minus.gif',
        minusBottom: imagePath + 'minusbottom.gif',
        nlPlus: imagePath + 'nolines_plus.gif',
        nlMinus: imagePath + 'nolines_minus.gif'
      },

      ajaxLoadUrl: '',

      /**
      * 设置Ajax模式开关
      */
      setAjaxMode: function(mode)
      {
        this.options.ajaxMode = mode;
        this.options.useCookies = false;
      },

      load: function(url, async, xml)
      {
        this.ajaxLoadUrl = url;

        this.ajaxLoadXml = xml;

        var outString = this.ajaxLoadXml;

        outString = outString.replace('{tree}', this.name);
        outString = outString.replace('{parentId}', 0);

        x.net.xhr(this.ajaxLoadUrl, outString,
            {
              async: async,
              callback: this.load_callback
            });
      },

      post: function(url, async, xml)
      {
        this.ajaxLoadUrl = url;

        x.net.xhr(this.ajaxLoadUrl, xml,
        {
          type: 'POST',
          async: async,
          data: { tree: this.name, parentId: 0 },
          callback: this.load_callback
        });
      },

      load_callback: function(response)
      {
        var result = x.toJSON(response);

        var tree = x.ui.pkg.tree.cache[result.data.tree];

        var parentId = result.data.parentId;

        var childNodes = result.data.childNodes;

        tree.bindChildNodes(childNodes);
      },

      bindChildNodes: function(childNodes)
      {
        for(var i = 0;i < childNodes.length;i++)
        {
          this.add({
            id: childNodes[i].id,
            parentId: childNodes[i].parentId,
            name: childNodes[i].name,
            url: childNodes[i].url,
            title: childNodes[i].title,
            target: childNodes[i].target,
            icon: (typeof (childNodes[i].icon) == 'undefined' ? '' : childNodes[i].icon),
            iconOpen: (typeof (childNodes[i].iconOpen) == 'undefined' ? '' : childNodes[i].iconOpen),
            open: (typeof (childNodes[i].open) == 'undefined' ? false : (childNodes[i].open == 'true' ? true : false)),
            hasChildren: (typeof (childNodes[i].hasChildren) == 'undefined' ? true : (childNodes[i].hasChildren == 'true' ? true : false)),
            ajaxLoading: (typeof (childNodes[i].ajaxLoading) == 'undefined' ? true : (childNodes[i].ajaxLoading == 'true' ? true : false))
          });

          if(!x.isUndefined(childNodes[i].childNodes) && childNodes[i].childNodes.length > 0)
          {
            this.bindChildNodes(childNodes[i].childNodes);
          }
        }
      },

      /*#region 函数:add(options)*/
      /**
      * 添加新的节点到节点数组
      * @method add
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} name 名称
      */
      add: function(options)
      {
        this.nodes[this.nodes.length] = x.ui.pkg.tree.newTreeNode(options);

        // ajaxMode
        if(this.options.ajaxMode && options.hasChildren)
        {
          this.nodes[this.nodes.length - 1].hasChildren = options.hasChildren;
          this.nodes[this.nodes.length - 1].ajaxLoading = options.ajaxLoading;
        }
      },
      /*#endregion*/

      /*#region 函数:openAll()*/
      /**
      * 打开所有的节点
      * @method openAll
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      openAll: function()
      {
        this.toggleAll(true);
      },
      /*#endregion*/

      /*#region 函数:closeAll()*/
      /**
      * 关闭所有的节点
      * @method closeAll
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      closeAll: function()
      {
        this.toggleAll(false);
      },
      /*#endregion*/

      /*#region 函数:toString()*/
      /**
      * 输出树的 html 格式文本
      * @method toString
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      toString: function()
      {
        var outString = '<div id="' + this.name + '" class="' + options.classNamePrefix + '">\n';

        if(document.getElementById)
        {
          if(this.options.useCookies)
          {
            this.selectedNode = this.getSelected();
          }

          outString += this.addNode(this.root);
        }
        else
        {
          outString += 'Browser not supported.';
        }

        outString += '</div>';

        if(!this.selectedFound)
        {
          this.selectedNode = null;
        }

        this.completed = true;

        return outString;
      },
      /*#endregion*/

      // Creates the tree structure

      addNode: function(parentNode)
      {
        var outString = '';

        var i = 0;

        if(this.options.inOrder)
          i = parentNode._atIndex;

        for(i;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == parentNode.id)
          {
            var childNode = this.nodes[i];

            childNode._parent = parentNode;

            childNode._atIndex = i;

            this.setProperty(childNode);

            if(!childNode.target && this.options.target)
              childNode.target = this.options.target;

            if(childNode.hasChildren && !childNode._isSelectedOpen && this.options.useCookies)
              childNode._isSelectedOpen = this.isOpen(childNode.id);

            if(!this.options.folderLinks && childNode.hasChildren)
              childNode.url = null;

            if(this.options.useSelection && childNode.id == this.selectedNode && !this.selectedFound)
            {
              childNode._isSelected = true;

              this.selectedNode = i;

              this.selectedFound = true;
            }

            outString += this.node(childNode, i);

            if(childNode._isLastNode)
              break;
          }

        }
        return outString;
      },

      // Creates the node icon, url and text

      node: function(node, nodeId)
      {
        var outString = '<div class="x-ui-pkg-tree-node" >' + this.indent(node, nodeId);

        if(this.options.uselementIcons)
        {
          if(!node.icon)
          {
            node.icon = (this.root.id == node.parentId) ? this.icon.root : ((node.hasChildren) ? this.icon.folder : this.icon.node);
          }

          if(!node.iconOpen)
          {
            node.iconOpen = (node.hasChildren) ? this.icon.folderOpen : this.icon.node;
          }

          if(this.root.id == node.parentId)
          {

            node.icon = this.icon.root;

            node.iconOpen = this.icon.root;

          }

          outString += '<img id="i' + this.name + nodeId + '" src="' + ((node._isSelectedOpen) ? node.iconOpen : node.icon) + '" alt="" />';

        }

        if(node.url)
        {
          outString += '<a id="s' + this.name + nodeId + '" class="' + ((this.options.useSelection) ? ((node._isSelected ? 'nodeSel' : 'node')) : 'node') + '" href="' + node.url + '"';

          if(node.title) outString += ' title="' + node.title + '"';

          if(node.target) outString += ' target="' + node.target + '"';

          if(this.options.useStatusText) outString += ' onmouseover="window.status=\'' + node.name + '\';return true;" onmouseout="window.status=\'\';return true;" ';

          if(this.options.useSelection && ((node.hasChildren && this.options.folderLinks) || !node.hasChildren))
          {
            outString += ' onclick="javascript:x.ui.pkg.tree.cache[\'' + this.name + '\'].select(' + nodeId + ');"';
          }

          outString += '>';

        }
        else if((!this.options.folderLinks || !node.url) && node.hasChildren && node.parentId != this.root.id)
        {
          outString += '<a href="javascript:x.ui.pkg.tree.cache[\'' + this.name + '\'].toggle(' + nodeId + ');" class="node" >';
        }

        outString += node.name;

        if(node.url || ((!this.options.folderLinks || !node.url) && node.hasChildren)) outString += '</a>';

        outString += '</div>';

        if(node.hasChildren)
        {

          outString += '<div id="d' + this.name + nodeId + '" class="clip" style="display:' + ((this.root.id == node.parentId || node._isSelectedOpen) ? 'block' : 'none') + ';">';

          outString += this.addNode(node);

          outString += '</div>';

        }

        this.arrayIndent.pop();

        return outString;

      },

      // Adds the empty and line icons

      /**
      * 缩进
      */
      indent: function(node, nodeId)
      {
        var outString = '';

        if(this.root.id != node.parentId)
        {

          for(var n = 0;n < this.arrayIndent.length;n++)
          {
            outString += '<img src="' + ((this.arrayIndent[n] == 1 && this.options.useLines) ? this.icon.line : this.icon.empty) + '" alt="" />';
          }

          (node._isLastNode) ? this.arrayIndent.push(0) : this.arrayIndent.push(1);

          if(node.hasChildren)
          {

            outString += '<a href="javascript:x.ui.pkg.tree.cache[\'' + this.name + '\'].toggle(' + nodeId + ');"><img id="j' + this.name + nodeId + '" src="';

            if(!this.options.useLines)
            {
              outString += (node._isSelectedOpen) ? this.icon.nlMinus : this.icon.nlPlus;
            }
            else
            {
              outString += ((node._isSelectedOpen) ? ((node._isLastNode && this.options.useLines) ? this.icon.minusBottom : this.icon.minus) : ((node._isLastNode && this.options.useLines) ? this.icon.plusBottom : this.icon.plus));
            }

            outString += '" alt="" /></a>';

          } else outString += '<img src="' + ((this.options.useLines) ? ((node._isLastNode) ? this.icon.joinBottom : this.icon.join) : this.icon.empty) + '" alt="" />';

        }

        return outString;

      },

      // Checks if a node has any children and if it is the last sibling

      /** 
      * 设置节点属性
      * @method setProperty
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {x.ui.tree.newTreeNode} node 树节点
      */
      setProperty: function(node)
      {
        var lastId;

        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == node.id)
          {
            // 判断节点是否有子节点
            node.hasChildren = true;
          }

          if(this.nodes[i].parentId == node.parentId)
          {
            lastId = this.nodes[i].id;
          }
        }

        node._isLastNode = (lastId == node.id) ? true : false;

        // x.debug.log(node);
      },

      /*#region 函数:getSelectedNodeId()*/
      /** 
      * 获取当前选择的节点标识
      * @method getSelectedNodeId
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      getSelectedNodeId: function()
      {
        return this.selectedNodeId;
      },
      /*#endregion*/

      /*#region 函数:getSelectedNode()*/
      /** 
      * 获取当前选择的节点对象
      * @method getSelectedNode
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      getSelectedNode: function()
      {
        return (this.selectedNodeId == '') ? null : this.nodes[this.selectedNodeId];
      },
      /*#endregion*/

      /*#region 函数:getNodeByNodeId(nodeId)*/
      /** 
      * 根据节点标识, 获取节点对象
      * @method getSelectedNode
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} nodeId 节点标识
      */
      getNodeByNodeId: function(nodeId)
      {
        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].id == nodeId)
          {
            return this.nodes[i];
          }
        }

        return null;
      },
      /*#endregion*/

      /*#region 函数:removeNode(nodeId)*/
      /** 
      * 移除一个节点
      * @method removeNode
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} nodeId 节点标识
      */
      removeNode: function(nodeId)
      {
        var index = -1;

        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].id == nodeId)
          {
            index = i;
          }
        }

        if(index != -1)
        {
          for(var i = index + 1;i < this.nodes.length;i++)
          {
            var nodeTemp = this.nodes[i];

            this.nodes[i] = null;

            this.nodes[i - 1] = nodeTemp;
          }

          this.nodes.pop();
        }
      },
      /*#endregion*/

      /*#region 函数:getSelected()*/
      /** 
      * 获取当前选着的节点
      * @method getSelected
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      getSelected: function()
      {
        var selectedNode = x.cookies.query('cs' + this.name);

        return (selectedNode) ? selectedNode : null;
      },
      /*#endregion*/

      // Highlights the selected node
      // s -> select

      select: function(id)
      {
        if(!this.options.useSelection) return;

        var childNode = this.nodes[id];

        if(childNode.hasChildren && !this.options.folderLinks) return;

        if(this.selectedNode != id)
        {

          if(this.selectedNode || this.selectedNode == 0)
          {
            eOld = document.getElementById("s" + this.name + this.selectedNode);

            if(eOld != null)
            {
              eOld.className = "node";
            }
          }

          eNew = document.getElementById("s" + this.name + id);

          eNew.className = "nodeSel";

          this.selectedNode = id;

          // jiqiliang nodearr 下标
          this.selectedNodeId = id;

          if(this.options.useCookies) x.cookies.add('cs' + this.name, childNode.id);

        }
      },

      /**
      * 打开或关闭
      */
      toggle: function(index)
      {
        if(this.nodes[index].ajaxLoading)
        {
          this.nodes[index].ajaxLoading = false;

          var outString = this.ajaxLoadXml;

          outString = outString.replace('{tree}', this.name);
          outString = outString.replace('{parentId}', this.nodes[index].id);

          x.net.xhr(this.ajaxLoadUrl, outString,
              {
                async: false,
                callback: this.toggle_callback
              });
        }
        else
        {
          var childNode = this.nodes[index];

          this.setStatus(!childNode._isSelectedOpen, index, childNode._isLastNode);

          childNode._isSelectedOpen = !childNode._isSelectedOpen;

          if(this.options.closeSameLevel)
          {
            this.closeLevel(childNode);
          }

          if(this.options.useCookies)
          {
            this.updateCookie();
          }
        }
      },

      toggle_callback: function(response)
      {
        var result = x.toJSON(response);

        var tree = x.ui.pkg.tree.cache[result.data.tree];

        var parentId = result.data.parentId;

        var childNodes = result.data.childNodes;

        var index = -1;

        var hasChildren = false;

        for(var i = 0;i < tree.nodes.length;i++)
        {
          if(tree.nodes[i].id == parentId)
          {
            index = i;

            hasChildren = (childNodes.length > 0) ? true : false;

            tree.nodes[i].hasChildren = hasChildren;
            tree.nodes[i].icon = (tree.nodes[i].hasChildren) ? tree.icon.folder : tree.icon.node;

            break;
          }
        }

        tree.bindChildNodes(childNodes);

        $(document.getElementById(tree.name)).parent().html(tree.toString());

        if(index > -1 && hasChildren)
        {
          tree.toggle(index);
        }
      },

      /*#region 函数:toggleAll(status)*/
      /**
      * 展开或关闭全部节点
      * @method toggleAll
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} status 状态 true 张开
      */
      toggleAll: function(status)
      {

        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].hasChildren && this.nodes[i].parentId != this.root.id)
          {
            this.setStatus(status, i, this.nodes[i]._isLastNode)

            this.nodes[i]._isSelectedOpen = status;
          }
        }

        if(this.options.useCookies)
        {
          this.updateCookie();
        }
      },
      /*#endregion*/

      // Opens the tree to a specific node

      openTo: function(nId, bSelect, bFirst)
      {
        if(!bFirst)
        {
          for(var n = 0;n < this.nodes.length;n++)
          {
            if(this.nodes[n].id == nId)
            {
              nId = n;
              break;
            }
          }
        }

        var childNode = this.nodes[nId];

        if(childNode.parentId == this.root.id || !childNode._parent) { return; }

        childNode._isSelectedOpen = true;

        childNode._isSelected = bSelect;

        if(this.completed && childNode.hasChildren) this.setStatus(true, childNode._atIndex, childNode._isLastNode);

        if(this.completed && bSelect) this.select(childNode._atIndex);

        else if(bSelect) this._sn = childNode._atIndex;

        this.openTo(childNode._parent._atIndex, false, true);

      },

      // Closes all nodes on the same level as certain node
      //
      closeLevel: function(node)
      {
        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == node.parentId && this.nodes[i].id != node.id && this.nodes[i].hasChildren)
          {
            this.setStatus(false, i, this.nodes[i]._isLastNode);

            this.nodes[i]._isSelectedOpen = false;

            this.closeAllChildren(this.nodes[i]);
          }
        }
      },

      /*#region 函数:closeAllChildren(node)*/
      /**
      * 关闭所有的节点信息
      * @method closeAllChildren
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} name 名称
      */
      closeAllChildren: function(node)
      {
        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == node.id && this.nodes[i].hasChildren)
          {
            if(this.nodes[i]._isSelectedOpen) this.setStatus(false, i, this.nodes[i]._isLastNode);

            this.nodes[i]._isSelectedOpen = false;

            this.closeAllChildren(this.nodes[i]);
          }
        }
      },
      /*#endregion*/

      // Change the status of a node(open or closed)

      /*#region 函数:setStatus(status, index, bottom)*/
      /**
      * 设置状态
      * @method setStatus
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} status
      * @param {string} index
      * @param {string} bottom
      */
      setStatus: function(status, index, bottom)
      {
        var elementDiv = document.getElementById('d' + this.name + index);

        var elementJoin = document.getElementById('j' + this.name + index);

        if(this.options.uselementIcons)
        {
          elementIcon = document.getElementById('i' + this.name + index);

          elementIcon.src = (status) ? this.nodes[index].iconOpen : this.nodes[index].icon;
        }

        elementJoin.src = (this.options.useLines) ?

    ((status) ? ((bottom) ? this.icon.minusBottom : this.icon.minus) : ((bottom) ? this.icon.plusBottom : this.icon.plus)) :

    ((status) ? this.icon.nlMinus : this.icon.nlPlus);

        elementDiv.style.display = (status) ? 'block' : 'none';

      },
      /*#endregion*/

      /*
      // [Cookie] Clears a cookie
    
      clearCookie: function()
      {
    
      var now = new Date();
    
      var yesterday = new Date(now.getTime() - 1000 * 60 * 60 * 24);
    
      this.setCookie('co' + this.name, 'cookieValue', yesterday);
    
      this.setCookie('cs' + this.name, 'cookieValue', yesterday);
    
      },
          
      // [Cookie] Sets value in a cookie
    
      setCookie: function(cookieName, cookieValue, expires, path, domain, secure)
      {
    
      document.cookie =
    
      escape(cookieName) + '=' + escape(cookieValue)
    
      + (expires ? '; expires=' + expires.toGMTString() : '')
    
      + (path ? '; path=' + path : '')
    
      + (domain ? '; domain=' + domain : '')
    
      + (secure ? '; secure' : '');
    
      },
    
      // [Cookie] Gets a value from a cookie
    
      getCookie: function(cookieName)
      {
    
      var cookieValue = '';
    
      var posName = document.cookie.indexOf(escape(cookieName) + '=');
    
      if (posName != -1)
      {
    
      var posValue = posName + (escape(cookieName) + '=').length;
    
      var endPos = document.cookie.indexOf(';', posValue);
    
      if (endPos != -1) cookieValue = unescape(document.cookie.substring(posValue, endPos));
    
      else cookieValue = unescape(document.cookie.substring(posValue));
    
      }
    
      return (cookieValue);
    
      },
      */

      // [Cookie] Returns ids of open nodes as a string

      updateCookie: function()
      {
        var outString = '';

        for(var n = 0;n < this.nodes.length;n++)
        {
          if(this.nodes[n]._isSelectedOpen && this.nodes[n].parentId != this.root.id)
          {
            if(outString) outString += '.';

            outString += this.nodes[n].id;
          }

        }

        x.cookies.add('co' + this.name, outString);
      },

      // [Cookie] Checks if a node id is in a cookie

      /*#region 函数:isOpen(id)*/
      /**
      * 判断节点是否开启, 需要设置选项 useCookies = true
      * @method isOpen
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} id 节点标识
      */
      isOpen: function(id)
      {
        var list = x.cookies.query('co' + this.name).split('.');

        for(var i = 0;i < list.length;i++)
        {
          if(list[i] == id) return true;
        }

        return false;
      },
      /*#endregion*/

      /*#region 函数:create(options)*/
      /**
      * 初始化树对象
      * @method create
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {object} options 选项
      */
      create: function(options)
      {
        this.options = options;

        this.name = options.name;

        this.nodes = [];

        this.arrayIndent = [];

        this.root = x.ui.pkg.tree.newTreeNode({ id: -1 });

        this.selectedNode = null;

        this.selectedFound = false;

        this.selectedNodeId = 0;

        this.completed = false;

        this.icon = {
          root: options.imagePathRoot + 'tree_icon.gif',
          folder: options.imagePathRoot + 'folder.gif',
          folderOpen: options.imagePathRoot + 'folderOpen.gif',
          node: options.imagePathRoot + 'leaf.gif',
          empty: options.imagePathRoot + 'empty.gif',
          line: options.imagePathRoot + 'line.gif',
          join: options.imagePathRoot + 'join.gif',
          joinBottom: options.imagePathRoot + 'joinbottom.gif',
          plus: options.imagePathRoot + 'plus.gif',
          plusBottom: options.imagePathRoot + 'plusbottom.gif',
          minus: options.imagePathRoot + 'minus.gif',
          minusBottom: options.imagePathRoot + 'minusbottom.gif',
          nlPlus: options.imagePathRoot + 'nolines_plus.gif',
          nlMinus: options.imagePathRoot + 'nolines_minus.gif'
        };
      }
      /*#endregion*/
    };

    treeView.create(options);

    // 存为全局变量
    x.ui.pkg.tree.cache[options.name] = treeView;

    return treeView;
  },

  /*#region 类:newTreeNode(options)*/
  /**
  * 树节点
  * @description 树节点
  * @class TreeNode
  * @constructor newTreeNode
  * @memberof x.ui.tree
  * @param {object} options 选项<br />
  * 可选键值范围: 
  * <table class="param-options" >
  * <thead>
  * <tr><th>名称</th><th>类型</th><th class="last" >描述</th></tr>
  * </thead>
  * <tbody>
  * <tr><td class="name" >id</td><td>string</td><td>节点标识</td></tr> 
  * <tr><td class="name" >parentId</td><td>string</td><td>父级节点标识</td></tr>
  * <tr><td class="name" >name</td><td>string</td><td>名称</td></tr>
  * <tr><td class="name" >url</td><td>string</td><td>链接地址</td></tr>
  * </tbody>
  * </table>
  */
  newTreeNode: function(options)
  {
    return x.ext({
      id: '',
      parentId: '-1',
      name: '',
      url: '',
      title: '',
      target: '_self',
      icon: '',
      iconOpen: '',
      hasChildren: false,
      ajaxLoading: false,
      _isSelectedOpen: false,
      _isSelected: false,
      _isLastNode: false,
      _atIndex: 0,
      _parent: undefined
    }, options || {});
  }
  /*#endregion*/
}
