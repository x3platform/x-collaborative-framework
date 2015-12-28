x.register('main.bugs.bug.form');

main.bugs.bug.form = {

    maskWrapper: x.ui.mask.newMaskWrapper('main-bugs-bug-form-maskWrapper'),

    /*#region 函数:checkObject()*/
    /*
    * 检测对象的必填数据
    */
    checkObject: function()
    {
        if(x.dom.data.check())
        {
            return false;
        }

        if($('#title').val() == '')
        {
            $('#title')[0].focus();
            x.msg('必须填写【标题】信息');
            return false;
        }

        if($('#content').val() == '')
        {
            $('#content')[0].focus();
            x.msg('必须填写【内容】信息');
            return false;
        }

        return true;
    },
    /*#endregion*/

    /*#region 函数:save()*/
    save: function()
    {
        if(main.bugs.bug.form.checkObject())
        {
            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += x.dom.data.serialize({ storageType: 'xml', includeRequestNode: false });
            outString += '</request>';

            x.net.xhr('/api/bug.save.aspx', outString, {
                waitingMessage: i18n.net.waiting.saveTipText,
                callback: function(response)
                {
                    x.page.refreshParentWindow();
                    x.page.close();
                }
            });
        }
    },
    /*#endregion*/

    /*#region 函数:getCategoryWindow()*/
    getCategoryWindow: function()
    {
        if(document.getElementById("TreeViewContainer") == null)
        {
            var outString = '';

            outString += '<div id="windowProjectWizardWrapper" class="winodw-wizard-wrapper" style="width:300px; height:360px;" >';

            outString += '<div class="winodw-wizard-toolbar" >';
            outString += '<div class="winodw-wizard-toolbar-close" >';
            outString += '<a href="javascript:main.bugs.bug.form.maskWrapper.close();" class="button-text" >关闭</a>';
            outString += '</div>';

            outString += '<div class="float-left"><div class="winodw-wizard-toolbar-item" style="width: 200px;"><span>类别选择</span></div></div>';
            outString += '<div class="clear"></div>';
            outString += '</div>';

            outString += '<div id="treeViewContainer" style="padding-top:5px;padding-left:5px;height:318px;width:295px;overflow:auto"></div>';

            outString += '</div>';

            // 加载遮罩和页面内容
            x.ui.mask.getWindow({ content: outString }, main.bugs.bug.form.maskWrapper);

            main.bugs.bug.form.getTreeView();
        }
        else
        {
            main.bugs.bug.form.maskWrapper.open();
        }
    },
    /*#endregion*/

    /*#region 函数:getTreeView()*/
    getTreeView: function()
    {
        var treeViewName = '问题类别';
        var treeViewRootTreeNodeId = '1';
        var treeViewUrl = 'javascript:main.bugs.bug.form.setTreeViewNode(\'{treeNodeToken}\',\'{treeNodeId}\')';

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<treeViewId><![CDATA[]]></treeViewId>';
        outString += '<treeViewName><![CDATA[' + treeViewName + ']]></treeViewName>';
        outString += '<treeViewRootTreeNodeId><![CDATA[' + treeViewRootTreeNodeId + ']]></treeViewRootTreeNodeId>';
        outString += '<tree><![CDATA[{tree}]]></tree>';
        outString += '<parentId><![CDATA[{parentId}]]></parentId>';
        outString += '<enabledLeafClick><![CDATA[true]]></enabledLeafClick>';
        outString += '<elevatedPrivileges><![CDATA[false]]></elevatedPrivileges>';
        outString += '<url><![CDATA[' + treeViewUrl + ']]></url>';
        outString += '<treeName>tree</treeName>';
        outString += '</request>';

        var tree = x.ui.pkg.tree.newTreeView({ name: 'main.bugs.bug.form.tree', ajaxMode: true });

        tree.add({
            id: "0",
            parentId: "-1",
            name: treeViewName,
            url: 'javascript:void(0);',
            title: treeViewName,
            target: '',
            icon: '/resources/images/tree/tree_icon.gif'
        });

        tree.load('/api/bug.category.getDynamicTreeView.aspx', false, outString);

        main.bugs.bug.form.tree = tree;

        $('#treeViewContainer')[0].innerHTML = tree;

    },
    /*#endregion*/

    /*#region 函数:setTreeViewNode(value, name)*/
    setTreeViewNode: function(value, name)
    {
        $("#categoryIndex").val(x.string.trim(name, '\\'));

        $("#categoryId").val(value);

        main.bugs.bug.form.maskWrapper.close();
    },
    /*#endregion*/

    /*
    * 页面加载事件
    */
    load: function()
    {
        var entityId = $("#id").val();
        var entityClassName = $("#entityClassName").val();

        // 初始化页签控件
        x.ui.pkg.tabs.newTabs();

        // 加载附件信息
        // x.attachment.findAll($('#id').val(), 'Elane.X.Bugzilla.Model.BugzillaInfo, Elane.X.Bugzilla', "attachment", "windowAttachmentContainer");

        if($('#windowBugzillaCommentContainer').size() > 0)
        {
            // 加载评论信息
            main.bugs.util.comment.findAll();
        }

        x.app.files.findAll({
            entityId: entityId,
            entityClassName: entityClassName,
            targetViewName: "window-attachment-wrapper"
        });

        // -------------------------------------------------------
        // 设置上传文件事件
        // -------------------------------------------------------

        $('#fileupload').fileupload({
            url: '/api/attachmentStorage.util.file.upload.aspx?clientId=' + x.dom('#session-client-id').val() + '&clientSignature=' + x.dom('#session-client-signature').val() + '&timestamp=' + x.dom('#session-timestamp').val() + '&nonce=' + x.dom('#session-nonce').val(),
            formData: {
                entityId: entityId,
                entityClassName: entityClassName,
                attachmentEntityClassName: '',
                attachmentFolder: 'bugs'
            },
            // 返回结果类型
            dataType: 'text',
            done: function(e, data)
            {
                // x.debug.log(data.result);
                x.debug.log(data.result);

                x.app.files.findAll({
                    entityId: entityId,
                    entityClassName: entityClassName,
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

        // -------------------------------------------------------
        // 绑定事件
        // -------------------------------------------------------

        // [空]

    }
}

$(document).ready(main.bugs.bug.form.load);

//// 附件上传设置

//// 附件上传状态监测
//setInterval(uploadSuccess_callback, 200)

//// 附件上传成功后的回调信息
//function uploadSuccess_callback()
//{
//  if(x.cookies.find('uploadSuccessValue') == '1')
//  {
//    x.cookies.add('uploadSuccessValue', '0', '', '/', ('.' + document.getElementById('system$domain').value));

//    // 回调后执行的方法
//    x.attachment.findAll($("#id").val(), 'Elane.X.Bugzilla.Model.BugzillaInfo, Elane.X.Bugzilla', "attachment", "windowAttachmentContainer");
//  }
//}