x.register('x.app.files');

x.app.files = {

    /*
    * 查询全部  deleteType = virtual | physical
    * @entityId 
    * @entityClassName 
    * @deleteType 
    * @targetViewName
    */
    findAll: function(options)
    {
        options = x.ext({
            showIcon: 1,
            displayInline: 0,
            downType: 1,
            readonly: false,
            deleteType: 'physical'
        }, options);

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<entityId><![CDATA[' + options.entityId + ']]></entityId>';
        outString += '<entityClassName><![CDATA[' + options.entityClassName + ']]></entityClassName>';
        outString += '<length><![CDATA[100]]></length>';
        outString += '</request>';

        x.net.xhr('/api/attachmentStorage.findAll.aspx', outString, {
            callback: options.callback || function(response)
            {
                var outString = '';

                var list = x.toJSON(response).data;

                if(options.readonly)
                {
                    // 获取只读列表

                    outString = x.app.files.getReadOnlyTable(list, options);
                }
                else
                {
                    // 获取可编辑的列表

                    // setEditableTable
                    // x.app.files.fillTable(list, options.deleteType, options.targetViewName);
                    outString = x.app.files.getEditableTable(list, options);

                    // $('#' + options.targetViewName).html(outString);
                }

                $('#' + options.targetViewName).html(outString);
            }
        });
    },

    /**
    * 只读紧缩展示
    */
    getReadOnlyTable: function(list, options)
    {
        var outString = '';

        if(list.length > 0)
        {
            if(typeof (options.prefixText) !== 'undefined')
            {
                outString += options.prefixText + ' ';
            }

            for(var i = 0;i < list.length;i++)
            {
                if(options.downType == 0)
                {
                    if(x.app.files._onlineReadModel != "down")
                    {
                        outString += '<div class="window-attachment-item" fileType="' + list[i].fileType + '" attachmentId="' + list[i].id + '" attachmentName="' + list[i].attachmentName + '">';
                        outString += '<img style="vertical-align: middle;margin-right:2px" src="/resources/images/icon/' + x.app.files.getfileExtImg(list[i].fileType) + '"/>';
                        outString += '<a style="vertical-align: bottom;" href="#" oncontextmenu="x.app.files.popMenuInit(this)" onclick="x.app.files.popMenuInit(this)" >' + list[i].attachmentName + '</a>';
                        outString += '<span style="vertical-align: middle; margin: 0 10px 0 5px; text-decoration:none">(' + x.expressions.formatNumberRound2(list[i].fileSize / 1024) + "KB)</span>";
                        outString += '</div>';
                    }
                }
                else
                {
                    outString += ((options.displayInline === 1) ? '<span' : '<div') + ' class="window-attachment-item" >';

                    if(options.showIcon === 1)
                    {
                        outString += '<img style="vertical-align: middle; margin-right:2px" src="/resources/images/icon/' + x.app.files.getfileExtImg(list[i].fileType) + '"/>';
                    }

                    // 支持图片在线预览
                    if(list[i].fileType == '.jpg' || list[i].fileType == '.png' || list[i].fileType == '.gif')
                    {
                        // outString += '<a href="javascript:x.app.files.preview(\'/attachment/archive/' + node.id + '\');" >' + node.attachmentName + '(' + x.expressions.formatNumberRound2(node.fileSize / 1024) + 'KB)</a>';
                        outString += '<a style="vertical-align: bottom;" href="javascript:x.app.files.preview(\'/attachment/archive/' + list[i].id + '\');" >' + list[i].attachmentName + '</a>';
                    }
                    else
                    {
                        outString += '<a style="vertical-align: bottom;" href="/attachment/archive/' + list[i].id + '" >' + list[i].attachmentName + '</a>';
                    }

                    // outString += '<a style="vertical-align: bottom;" href="/attachment/archive/' + list[i].id + '" >' + list[i].attachmentName + '</a>';
                    outString += '<span style="vertical-align: middle; margin: 0 10px 0 5px; text-decoration:none" >(' + x.expressions.formatNumberRound2(list[i].fileSize / 1024) + 'KB)</span>';
                    outString += (options.displayInline === 1) ? '</span>' : '</div>';
                }
            }
        }

        return outString;
    },

    getEditableTable: function(list, options)
    {
        var outString = '';

        if(list.length > 0)
        {
            if(typeof (options.prefixText) !== 'undefined')
            {
                outString += options.prefixText + ' ';
            }

            for(var i = 0;i < list.length;i++)
            {
                outString += '<div class="window-attachment-item" fileType="' + list[i].fileType + '" attachmentId="' + list[i].id + '" attachmentName="' + list[i].attachmentName + '" deleteType="' + options.deleteType + '">';

                if(options.showIcon === 1)
                {
                    outString += '<img style="vertical-align: middle; margin-right:2px" src="/resources/images/icon/' + x.app.files.getfileExtImg(list[i].fileType) + '"/>';
                }

                // 支持图片在线预览
                if(list[i].fileType == '.jpg' || list[i].fileType == '.png' || list[i].fileType == '.gif')
                {
                    outString += '<a style="vertical-align: bottom;" href="javascript:x.app.files.preview(\'/attachment/archive/' + list[i].id + '\');" >' + list[i].attachmentName + '</a>';
                }
                else
                {
                    outString += '<a style="vertical-align: bottom;" href="/attachment/archive/' + list[i].id + '" >' + list[i].attachmentName + '</a>';
                }

                outString += '<span style="vertical-align: middle; margin: 0 10px 0 5px; text-decoration:none" >(' + x.expressions.formatNumberRound2(list[i].fileSize / 1024) + 'KB)</span>';

                outString += '<span style="vertical-align: middle;margin-left:5px;text-decoration:none;color:#000000;cursor:pointer" onclick="x.app.files.confirmDelete(this)">删除</span>';

                outString += '</div>';
            }
        }

        return outString;
        /*
                var attachmentTable = x.app.files.createTable(tableContainerId);
        
                //x.app.files.cleanBlankRow();
        
                x.app.files.cleanAttachmentRow(attachmentTable);
        
                //呈现
                if(attachmentTable != null)
                {
                    for(var i = 0;i < list.length;i++)
                    {
                        var node = list[i];
        
                        var newTr = attachmentTable.insertRow(attachmentTable.rows.length);
                        x.app.files.docObject.setAttributes(newTr, "deleteType", deleteType);
                        x.app.files.docObject.setAttributes(newTr, "attachmentId", node.id);
                        x.app.files.docObject.setAttributes(newTr, "attachmentName", node.attachmentName);
        
                        var nameTd = newTr.insertCell(0);
                        nameTd.style.border = '0 solid #ffffff';
                        nameTd.style.padding = '0 0 4px 0';
                        var nameTdInnerHtml = "";
                        nameTdInnerHtml += '<img style="margin-right:2px;vertical-align: middle;" src="/resources/images/icon/' + x.app.files.getfileExtImg(node.fileType) + '"/>';
                        nameTdInnerHtml += '<a style="vertical-align: middle;text-decoration:none" href="/attachment/archive/' + node.id + '.aspx" >' + node.attachmentName + '</a>';
                        nameTdInnerHtml += "<span style='vertical-align: middle;margin-left:1px;text-decoration:none'>(" + x.expressions.formatNumberRound2(node.fileSize / 1024) + "KB)</span>";
        
                        nameTdInnerHtml += '<span title="删除此附件" style="vertical-align: middle;margin-left:5px;text-decoration:none;color:#000000;cursor:pointer" onclick="x.app.files.confirmDelete(this)">删除</span>';
        
                        nameTd.innerHTML = nameTdInnerHtml;
                    }
                }*/
    },

    /*#region preview:(virtualPath)*/
    /*
    * 图片类型附件预览
    */
    preview: function(virtualPath)
    {
        // 设置图片的最大宽度
        var maxWidth = 800;

        x.ui.wizards.getWizard('x-file-preview', {
            create: function()
            {
                var outString = '';

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:' + (maxWidth + 2) + 'px; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" title="关闭" ><i class="fa fa-close"></i></a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>图片查看</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                // outString += '<div id="' + this.name + '-treeViewContainer" class="winodw-wizard-tree-view" style="width:296px; height:300px;"></div>';
                outString += '<div class="text-center"><img id="x-file-preview-image-viewer" src="' + virtualPath + '" class="preview-image-width" /></div>';
                outString += '<div class="text-right" style="padding: 6px 10px 4px 0;"><a href="' + virtualPath + '"><i class="fa fa-download"></i> 下载原图</a></div>';

                outString += '</div>';

                return outString;
            }
        });

        // 设置图片超过页面范围情况下, 将其设置为页面宽度.
        $('#x-file-preview-image-viewer').on('load', function()
        {
            // var maxWidth = 720; 

            if($(this).width() > maxWidth)
            {
                $(this).width(maxWidth);
            }
        })

        x.page.goTop();
    },

    // 附件当前行
    _attachmentRowObject: null,
    // 在线打开模式
    _onlineReadModel: "down", //rms office xps down

    inheritContainerTabCollection: null,

    // 注册上一版本附件容器
    registerInheritAttachmentContainer: function(tableContainerId, newEntityId)
    {
        if(x.app.files.inheritContainerTabCollection == null)
        {
            x.app.files.inheritContainerTabCollection = x.types.newHashTable();
        }
        x.app.files.inheritContainerTabCollection.add(tableContainerId, newEntityId);
    },

    setOnlineRead: function(model)
    {
        if(arguments.length == 1) { x.app.files._onlineReadModel = model; }

        if(navigator.userAgent.indexOf("Firefox") == -1)
        {

            if(document.getElementById("doc$attachmentRead") == null)
            {
                x.ui.neverPopmenu.item('阅读', "x.app.files.read");
                x.ui.neverPopmenu.item('下载', "x.app.files.down");
            }
            else
            {
                //管理员
                if(document.getElementById("app$administrator").value == "True" || document.getElementById("app$viewer").value == "True")
                {
                    x.ui.neverPopmenu.item('阅读', "x.app.files.read");
                    x.ui.neverPopmenu.item('下载', "x.app.files.down");
                }
                else
                {
                    if(document.getElementById("doc$attachmentRead").value == "True")
                        x.ui.neverPopmenu.item('阅读', "x.app.files.read");
                    else
                        x.ui.neverPopmenu.item('阅读', "x.app.files.read", "disabled");

                    if(document.getElementById("doc$attachmentDown").value == "True")
                        x.ui.neverPopmenu.item('下载', "x.app.files.down");
                    else
                        x.ui.neverPopmenu.item('下载', "x.app.files.down", "disabled");
                }
            }
            x.ui.neverPopmenu.init();
        }
    },

    popMenuInit: function(element)
    {
        if(navigator.userAgent.indexOf("Firefox") == -1)
        {
            x.app.files._attachmentRowObject = element;

            x.ui.neverPopmenu.view();
        }
        else
        {
            alert("请切换IE浏览器");
        }
    },

    read: function()
    {
        var tr = x.app.files.docObject.getParentObj(x.app.files._attachmentRowObject, "DIV");

        var nodeId = x.app.files.docObject.getAttributes(tr, "attachmentId");

        var attachmentName = x.app.files.docObject.getAttributes(tr, "attachmentName");

        var fileType = x.app.files.docObject.getAttributes(tr, "filetype");

        if(fileType == ".doc" || fileType == ".xls" || fileType == ".ppt" || fileType == ".pptx")
        {
            x.ui.neverPopmenu.hide();

            var url = "/resources/editor/office2003/officeclient.aspx?id=" + nodeId + '&contentType=' + fileType;

            var xpsUrl = "/resources/editor/xpsbrowser/xpsbrowser.aspx?id=" + nodeId + "&folderRoot=" + $("#applicationTag").val();


            if(document.getElementById("doc$xpsAttachmentFile") == null)
            {
                x.app.files.docObject.anchorLink(url)
            }
            else
            {

                if(document.getElementById("doc$xpsAttachmentFile").value.indexOf(nodeId) != -1)
                    x.app.files.docObject.anchorLink(xpsUrl);
                else
                    x.app.files.docObject.anchorLink(url);
            }
            //window.location.href = "/resources/editor/office2003/officeclient.aspx?id=" + nodeId + '&contentType=' + fileType;
        }
        else
        {
            x.ui.neverPopmenu.hide();
            x.app.files.down();
        }
    },

    down: function()
    {
        x.ui.neverPopmenu.hide();

        var tr = x.app.files.docObject.getParentObj(x.app.files._attachmentRowObject, "DIV");

        var nodeId = x.app.files.docObject.getAttributes(tr, "attachmentId");

        var fileType = x.app.files.docObject.getAttributes(tr, "fileType");

        if(document.getElementById("doc$rmsSwitch") == null)
            window.location.href = '/attachment/archive/' + nodeId;
        else
        {
            if(document.getElementById("doc$rmsSwitch").value == "On")
            {

                if(document.getElementById("doc$rmsFilesFilter").value.indexOf(fileType) != -1)
                {

                    var url = "/apps/pages/docs/doc-Rms.aspx?attachmentId=" + nodeId + '&applicationTag=' + $("#applicationTag").val() + "&entityId=" + $("#id").val();

                    window.location.href = url;

                }
                else
                {
                    window.location.href = "/attachment/archive/" + nodeId;
                }
            }
            else
            {
                window.location.href = "/attachment/archive/" + nodeId;
            }
        }
    },

    isExist: function(fileName)
    {
        var tbAttachment = document.getElementById(x.app.files._tableId)

        for(var i = 1;i < tbAttachment.rows.length;i++)
        {
            if(fileName == x.app.files.docObject.getAttributes(attachmentTable.rows[i], "attachmentName"))
                return true;
        }
        return false;
    },
    /*
    * 构建表格
    */
    createTable: function(tableContainerId)
    {
        if(document.getElementById(tableContainerId).tagName != "TABLE")
        {
            var tbAttachment = document.getElementById(tableContainerId + "_table");

            if(tbAttachment == null)
            {
                //构建表格
                var outString = '';
                outString += '<table id="' + tableContainerId + "_table" + '" class="" style="width:100%">';
                outString += '</table>';
                $('#' + tableContainerId).html(outString);
            }
            return $('#' + tableContainerId + '_table').get(0);
        }

        return $('#' + tableContainerId).get(0);
    },

    cleanAttachmentRow: function(attachmentTable)
    {
        //清空附件行
        if(attachmentTable != null)
        {
            for(var i = attachmentTable.rows.length - 1;i >= 0;i--)
            {
                if(x.app.files.docObject.getAttributes(attachmentTable.rows[i], "deleteType") == "attachment")
                {
                    attachmentTable.rows[i].parentNode.removeChild(attachmentTable.rows[i]);
                }
            }
        }
    },

    cleanAllRow: function(attachmentContainerId)
    {
        var attachmentTable = document.getElementById(attachmentContainerId + "_table");
        //清空附件行
        if(attachmentTable != null)
        {
            while(attachmentTable.rows.length > 0)
            {
                attachmentTable.rows[0].parentNode.removeChild(attachmentTable.rows[0]);
            }
        }
    },

    fillTable: function(list, deleteType, tableContainerId)
    {
        var attachmentTable = x.app.files.createTable(tableContainerId);

        //x.app.files.cleanBlankRow();

        x.app.files.cleanAttachmentRow(attachmentTable);

        //呈现
        if(attachmentTable != null)
        {
            for(var i = 0;i < list.length;i++)
            {
                var node = list[i];

                var newTr = attachmentTable.insertRow(attachmentTable.rows.length);
                x.app.files.docObject.setAttributes(newTr, "deleteType", deleteType);
                x.app.files.docObject.setAttributes(newTr, "attachmentId", node.id);
                x.app.files.docObject.setAttributes(newTr, "attachmentName", node.attachmentName);

                var nameTd = newTr.insertCell(0);
                nameTd.style.border = '0 solid #ffffff';
                nameTd.style.padding = '0 0 4px 0';
                var nameTdInnerHtml = "";
                nameTdInnerHtml += '<img style="margin-right:2px;vertical-align: middle;" src="/resources/images/icon/' + x.app.files.getfileExtImg(node.fileType) + '"/>';

                // 支持图片在线预览
                if(list[i].fileType == '.jpg' || list[i].fileType == '.png' || list[i].fileType == '.gif')
                {
                    // outString += '<a href="javascript:x.app.files.preview(\'/attachment/archive/' + node.id + '\');" >' + node.attachmentName + '(' + x.expressions.formatNumberRound2(node.fileSize / 1024) + 'KB)</a>';
                    nameTdInnerHtml += '<a style="vertical-align: middle;text-decoration:none" href="javascript:x.app.files.preview(\'/attachment/archive/' + node.id + '\');" >' + node.attachmentName + '</a>';
                }
                else
                {
                    nameTdInnerHtml += '<a style="vertical-align: middle;text-decoration:none" href="/attachment/archive/' + node.id + '" >' + node.attachmentName + '</a>';
                }

                nameTdInnerHtml += "<span style='vertical-align: middle;margin-left:1px;text-decoration:none'>(" + x.expressions.formatNumberRound2(node.fileSize / 1024) + "KB)</span>";

                nameTdInnerHtml += '<span title="删除此附件" style="vertical-align: middle;margin-left:5px;text-decoration:none;color:#000000;cursor:pointer" onclick="x.app.files.confirmDelete(this)">删除</span>';

                nameTd.innerHTML = nameTdInnerHtml;
            }
        }
    },

    confirmDelete: function(obj)
    {
        if(confirm('确定删除?'))
        {
            var parent = $(obj).parent();

            var attachmentId = parent.attr('attachmentId');
            var deleteType = parent.attr('deleteType');

            switch(deleteType)
            {
                case "virtual":
                    parent.remove();
                    break;
                case "physical":
                    x.app.files.deleteAttachment(attachmentId);
                    parent.remove();
                    break;
            }
        }
    },

    deleteAttachment: function(id)
    {
        x.net.xhr('/api/attachmentStorage.delete.aspx?id=' + id, { async: false });
    },

    getfileExtImg: function(fileType)
    {
        var fileName = '';

        switch(fileType.toLowerCase())
        {
            case ".xls":
            case ".xlsx":
                fileName = 'icon-file-xls.gif';
                break;
            case ".doc":
            case ".docx":
                fileName = 'icon-file-doc.gif';
                break;
            case ".ppt":
            case ".pptx":
                fileName = 'icon-file-ppt.gif';
                break;
            case ".pdf":
                fileName = 'icon-file-pdf.gif';
                break;
            case ".zip":
            case ".rar":
            case ".7z":
                fileName = 'icon-file-rar.gif';
                break;
            case ".jpg":
            case ".jpeg":
            case ".gif":
            case ".png":
                fileName = 'icon-file-png.gif';
                break;
            case ".txt":
                fileName = 'icon-file-txt.gif';
                break;
            default:
                fileName = 'icon-file-ms.gif';
                break;
        }

        return fileName;
    },

    getInheritRowXml: function()
    {
        if(x.app.files.inheritContainerTabCollection == null)
        {
            alert("需调用x.app.files,registerNewAttachmentContainer(tableContainerId,newEntityId)");
            return false;
        }
        else
        {
            if(x.app.files.inheritContainerTabCollection._list.length == 0)
            {
                alert("需调用x.app.files,registerNewAttachmentContainer(tableContainerId,newEntityId)");
                return false;
            }
        }

        var resultXml = "<inheritAttachments>";

        for(var k = 0;k < x.app.files.inheritContainerTabCollection._list.length;k++)
        {
            var tableName = x.app.files.inheritContainerTabCollection._list[k].name;
            var newEntityId = x.app.files.inheritContainerTabCollection._list[k].value;

            var tb = document.getElementById(tableName + "_table");

            if(tb != null)
            {
                for(var i = 0;i < tb.rows.length;i++)
                {
                    var currentTr = tb.rows[i];
                    if(x.app.files.docObject.getAttributes(currentTr, "deleteType") == "onlyTr")
                    {
                        resultXml += '<attachmentId newEntityId="' + newEntityId + '">';
                        resultXml += x.app.files.docObject.getAttributes(currentTr, "attachmentId");
                        resultXml += "</attachmentId>";
                    }
                }
            }
        }

        resultXml += "</inheritAttachments>";

        return resultXml;
    },

    docObject: {

        anchorLink: function(url)
        {
            var vra;
            if(document.getElementById("hiddAnchor") == null)
            {
                vra = document.createElement('a');
                vra.id = "hiddAnchor"
                vra.target = '_blank';

                document.body.appendChild(vra);
            }
            else
                vra = document.getElementById("hiddAnchor");

            vra.href = url;
            vra.click();
        },

        getDocObject: function(objectOrId)
        {
            if(typeof (objectOrId) == "object")
            {
                return objectOrId;
            }
            else
            {
                return document.getElementById(objectOrId);
            }
        },

        getParentObj: function(objOrId, tagName)
        {
            var currentObj = x.app.files.docObject.getDocObject(objOrId);

            while(currentObj.tagName != tagName && currentObj.tagName != 'BODY')
                currentObj = currentObj.parentNode;

            return currentObj;
        },

        setAttributes: function(objOrId, attributesName, value)
        {
            var selectObject = x.app.files.docObject.getDocObject(objOrId);

            var oAttrColl = selectObject.attributes;

            var newAttr = document.createAttribute(attributesName);

            newAttr.value = value;

            oAttrColl.setNamedItem(newAttr);
        },

        getAttributes: function(objOrId, attributesName)
        {
            var selectObject = x.app.files.docObject.getDocObject(objOrId);

            var oAttrColl = selectObject.attributes;

            var oAttr = oAttrColl.getNamedItem(attributesName);

            if(oAttr == null)
                return null
            else
                return oAttr.value;
        }
    }
}
