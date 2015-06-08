x.register('x.app.files');

x.app.files = {

    /*
    * 查询全部  delSource = onlyTr attachment
    * @entityId 
    * @entityClassName 
    * @delSourceType 
    * @targetViewName
    */
    findAll: function(options)
    {
        options = x.ext({
            showIcon: 1,
            displayInline: 0,
            downType: 1,
            readonly: false
        }, options);

        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<request>';
        outString += '<entityId><![CDATA[' + options.entityId + ']]></entityId>';
        outString += '<entityClassName><![CDATA[' + options.entityClassName + ']]></entityClassName>';
        outString += '<length><![CDATA[100]]></length>';
        outString += '</request>';

        x.net.xhr('/api/attachmentStorage.findAll.aspx', outString, {
            callback: function(response)
            {
                var outString = '';

                var list = x.toJSON(response).data;

                if(options.readonly)
                {
                    outString = x.app.files.getReadOnlyTable(list, options);

                    $('#' + options.targetViewName).html(outString);
                }
                else
                {
                    // setEditableTable
                    x.app.files.fillTable(list, options.delSourceType, options.targetViewName);
                }

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
                    outString += '<a style="vertical-align: bottom;" href="/attachment/archive/' + list[i].id + '.aspx" >' + list[i].attachmentName + '</a>';
                    outString += '<span style="vertical-align: middle; margin: 0 10px 0 5px; text-decoration:none" >(' + x.expressions.formatNumberRound2(list[i].fileSize / 1024) + 'KB)</span>';
                    outString += (options.displayInline === 1) ? '</span>' : '</div>';
                }
            }
        }

        return outString;
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
            window.location.href = "/attachment/archive/" + nodeId + ".aspx";
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
                    window.location.href = "/attachment/archive/" + nodeId + ".aspx";
            }
            else
                window.location.href = "/attachment/archive/" + nodeId + ".aspx";
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
                if(x.app.files.docObject.getAttributes(attachmentTable.rows[i], "delSource") == "attachment")
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

    fillTable: function(list, sourceTag, tableContainerId)
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
                x.app.files.docObject.setAttributes(newTr, "delSource", sourceTag);
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
        }
    },

    confirmDelete: function(obj)
    {
        if(confirm('确定删除?'))
        {
            var currentTr = x.app.files.docObject.getParentObj(obj, "TR");

            var delSource = x.app.files.docObject.getAttributes(currentTr, "delSource");

            var attachmentId = x.app.files.docObject.getAttributes(currentTr, "attachmentId");

            switch(delSource)
            {
                case "onlyTr":
                    currentTr.parentNode.removeChild(currentTr);
                    break;
                case "attachment":
                    x.app.files.deleteAttachment(attachmentId);
                    currentTr.parentNode.removeChild(currentTr);
                    break;
            }
        }
    },

    deleteAttachment: function(ids)
    {
        $.ajax({
            type: 'POST',
            url: '/api/attachmentStorage.delete.aspx?ids=' + ids,
            data: 'clientSignature=' + x.net.getClientSignature(),
            async: false
        });
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
                    if(x.app.files.docObject.getAttributes(currentTr, "delSource") == "onlyTr")
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

    docObject:
    {
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
