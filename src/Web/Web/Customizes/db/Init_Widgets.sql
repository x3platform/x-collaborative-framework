-- ================================================
-- ���á�����Ԥ�����༭������
-- ================================================
INSERT [dbo].[tb_Customize_Widget] (
    [Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]
) VALUES (
N'00000000-0000-0000-0009-000000000001', 
N'weather', 
N'����Ԥ��', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'����Ԥ��', N'{title:''����Ԥ��'', height:''0'', width:''0''}', N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" class="input-normal widget-option-item" value="${title}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', 
N'ͨ��', 
N'X3Platform.Web.Customize.Widgets.WeatherWidget,X3Platform.Web.Customize', 
N'', 
N'', 
0, 
'2000-01-01', 
'2000-01-01');

UPDATE tb_Customize_Widget SET 
	Options = N'{title:''����Ԥ��'', height:''0'', width:''0''}', OptionHtml = N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" class="input-normal widget-option-item" value="${title}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>
'

WHERE Id='00000000-0000-0000-0009-000000000001'

-- ================================================
-- ���á��������ˡ��༭������
-- ================================================

UPDATE tb_Customize_Widget SET 
	Options = N'{title:''��������'', height:''0'', width:''0'', maxRowCount:''30''}', OptionHtml = N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" class="input-normal widget-option-item" value="${title}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" feature="number" class="input-normal widget-option-item" value="${maxRowCount}" style="width:80px;" /></td>
</tr>
</table>
'

WHERE Id='00000000-0000-0000-0001-000000000003'

-- ================================================
-- ���á���̬���ݡ��༭������
-- ================================================

INSERT [dbo].[tb_Customize_Widget] (
	[Id], 
	[Name], 
	[Title], 
	[Height], 
	[Width], 
	[Url], 
	[Description], 
	[Options], 
	[OptionHtml], 
	[Tags], 
	[ClassName], 
	[RedirctUrl], 
	[OrderId], 
	[Status], 
	[UpdateDate], 
	[CreateDate]
) VALUES (
	N'00000000-0000-0000-0009-000000000003', 
	N'html', 
	N'��̬����', 
	0, 
	0, 
	N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', 
	N'��̬����', 
	N'{title:''��̬����'', height:''0'', width:''0''}', 
	N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" class="input-normal widget-option-item" value="${title}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><textarea id="${Id}$widgetHtml" name="${Id}$widgetHtml" class="input-normal widget-option-item" style="width:80%;height:120px;" />${widgetHtml}</textarea>
</tr>
</table>
', 
	N'ͨ��', 
	N'X3Platform.Web.Customize.Widgets.StaticHtmlWidget,X3Platform.Web.Customize', 
	N'',
	N'',
	1,
	'2000-01-01', 
	'2000-01-01'
);

UPDATE tb_Customize_Widget SET 
	Options = N'{title:''��̬����'', height:''0'', width:''0'', widgetHtml:''''}', OptionHtml = N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" class="input-normal widget-option-item" value="${title}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><textarea id="${Id}$widgetHtml" name="${Id}$widgetHtml" class="input-normal widget-option-item" style="width:80%;height:120px;" />${widgetHtml}</textarea>
</tr>
</table>
'

WHERE Id='00000000-0000-0000-0009-000000000003'

-- ================================================
-- ���á����Ź����༭������
-- ================================================

UPDATE tb_Customize_Widget SET 
	Options = N'{title:''���Ź���'', height:''0'', width:''0'', maxRowCount:''30'', categoryIndex:'''', maxLength:''12''}', OptionHtml = N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" value="${maxRowCount}" feature="number" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�������</td>
<td class="table-body-input" ><input id="${Id}$categoryIndex" name="${Id}$categoryIndex" value="${categoryIndex}" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >������ʾ����</td>
<td class="table-body-input" ><input id="${Id}$maxLength" name="${Id}$maxLength" value="${maxLength}" feature="number" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
</table>
'

WHERE Id='00000000-0000-0000-0000-000000000004'

-- ================================================
-- ���á��ղؼС��༭������
-- ================================================

UPDATE tb_Customize_Widget SET 
	Options = N'{title:''�ղؼ�'', height:''0'', width:''0'', maxRowCount:''30'', maxLength:''12''}', OptionHtml = N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" value="${maxRowCount}" feature="number" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
</table>
'

WHERE Id='00000000-0000-0000-0000-000000000011'

--

INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0000-000000000001', N'general', N'ͨ�����ݹ���', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'ͨ�����ݹ���', N'{title:''ͨ�����ݹ���'', tableName:''tb_Cost'', tableColumns:''Id,Subject'',orderBy:''UpdateDate'',url:''{0}'',height:''0'', width:''0'', maxRowCount:''8''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���ݱ�����</td>
<td class="table-body-input" ><input id="${Id}$tableName" name="${Id}$tableName" value="${tableName}" class="input-normal widget-option-item" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���ݱ���</td>
<td class="table-body-input" ><input id="${Id}$tableColumns" name="${Id}$tableColumns" value="${tableColumns}" class="input-normal widget-option-item" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�������</td>
<td class="table-body-input" ><input id="${Id}$orderBy" name="${Id}$orderBy" value="${orderBy}" class="input-normal widget-option-item" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���ӵ�ַ��ʽ</td>
<td class="table-body-input" ><input id="${Id}$url" name="${Id}$url" value="${url}" class="input-normal widget-option-item" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:90%;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" value="${maxRowCount}" feature="number" class="input-normal widget-option-item" style="width:90%;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Web.Customize.Widgets.GeneralWidget,X3Platform.Web.Customize', NULL, N'10002', 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000001', N'docs', N'����֪ʶ', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'֪ʶ����', N'{title:''֪ʶ����'', height:''0'', width:''0'', maxRowCount:''8''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Docs.Customize.DocsWidget,X3Platform.Docs', NULL, NULL, 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000003', N'tasks', N'��������', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'��������', N'{title:''��������'', height:''0'', width:''0'', maxRowCount:''30''}', N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" class="input-normal widget-option-item" value="${title}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" feature="number" class="input-normal widget-option-item" value="${maxRowCount}" style="width:80px;" /></td>
</tr>
</table>
', N'ͨ��', N'X3Platform.Tasks.Customize.TaskWidget,X3Platform.Tasks', NULL, N'10002', 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000004', N'news', N'����', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'���Ź���', N'{title:''���Ź���'', height:''0'', width:''0'', maxRowCount:''6'', categoryIndex:'''', maxLength:''12''}', N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" value="${maxRowCount}" feature="number" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�������</td>
<td class="table-body-input" ><input id="${Id}$categoryIndex" name="${Id}$categoryIndex" value="${categoryIndex}" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >������ʾ����</td>
<td class="table-body-input" ><input id="${Id}$maxLength" name="${Id}$maxLength" value="${maxLength}" feature="number" class="input-normal widget-option-item" style="width:160px;" /></td>
</tr>
</table>
', N'ͨ��', N'X3Platform.Plugins.News.Customize.NewsWidget,X3Platform.Plugins.News', N'/news/default.aspx', N'10001', 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000005', N'wiki', N'����֪ʶ', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'֪ʶ����', N'{title:''֪ʶ����'', height:''0'', width:''0'', maxRowCount:''8''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Wiki.Customize.WikiWidget,X3Platform.Wiki', NULL, NULL, 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000006', N'forum', N'��̳', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'��̳����', N'{title:''��̳����'', height:''0'', width:''0'', maxRowCount:''10''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Forum.Customize.BugzillaWidget,X3Platform.Forum', NULL, NULL, 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000007', N'projects', N'��Ŀ', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'��Ŀ����', N'{title:''��Ŀ����'', height:''0'', width:''0'', maxRowCount:''10''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" value="${maxRowCount}" feature="number" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Projects.Customize.ProjectWidget,X3Platform.Projects', NULL, NULL, 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000008', N'bug', N'�������', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'�������', N'{title:''�������'', height:''0'', width:''0'', maxRowCount:''10''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Bugzilla.Customize.BugzillaWidget,X3Platform.Bugzilla', NULL, NULL, 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000011', N'favorite', N'�ҵ��ղؼ�', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'�ҵ��ղؼ�', N'{title:''�ҵ��ղؼ�'', height:''0'', width:''0'', maxRowCount:''10'', maxLength:''12''}', N'
<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >����</td>
<td class="table-body-input" ><input id="${Id}$maxRowCount" name="${Id}$maxRowCount" value="${maxRowCount}" feature="number" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
</table>
', N'ͨ��', N'X3Platform.Plugins.Favorite.Customize.FavoriteWidget,X3Platform.Plugins.Favorite', NULL, NULL, 1, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0001-000000000012', N'meeting', N'�������', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'�������', N'{title:''�������'', height:''0'', width:''0'', maxRowCount:''10'', maxLength:''12''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Meeting.Customize.MeetingWidget,X3Platform.Meeting', NULL, NULL, 0, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0009-000000000002', N'calendar', N'����', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'����', N'{title:''����'', height:''0'', width:''0''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Web.Customize.Widgets.TodayWidget,X3Platform.Web.Customize', NULL, NULL, 0, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0009-000000000009', N'ipSpy', N'IP��ѯ', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'IP��ѯ', N'{title:''IP��ѯ'', height:''0'', width:''0'', maxRowCount:''30''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Web.Customize.Widgets.IPSpyWidget,X3Platform.Web.Customize', NULL, NULL, 0, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
INSERT [dbo].[tb_Customize_Widget] ([Id], [Name], [Title], [Height], [Width], [Url], [Description], [Options], [OptionHtml], [Tags], [ClassName], [RedirctUrl], [OrderId], [Status], [UpdateDate], [CreateDate]) VALUES (N'00000000-0000-0000-0009-000000000010', N'mobileSpy', N'�ֻ������ز�ѯ', 0, 0, N'/services/Elane/X/Web/Customize/Ajax.WidgetInstanceWrapper.aspx', N'�ֻ������ز�ѯ', N'{title:''�ֻ������ز�ѯ'', height:''0'', width:''0'', maxRowCount:''30''}', N'<table class="table-style" style="width:100%; border-bottom:2px solid #333;" >
<tr class="table-row-normal-transparent">
<td class="table-body-text" style="width:80px;" >����</td>
<td class="table-body-input" ><input id="${Id}$title" name="${Id}$title" value="${title}" class="input-normal widget-option-item" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >�߶�</td>
<td class="table-body-input" ><input id="${Id}$height" name="${Id}$height" feature="number" class="input-normal widget-option-item" value="${height}" style="width:80px;" /></td>
</tr>
<tr class="table-row-normal-transparent">
<td class="table-body-text" >���</td>
<td class="table-body-input" ><input id="${Id}$width" name="${Id}$width" feature="number" class="input-normal widget-option-item" value="${width}" style="width:80px;" /></td>
</tr>
</table>', N'ͨ��', N'X3Platform.Web.Customize.Widgets.MobileSpyWidget,X3Platform.Web.Customize', NULL, NULL, 0, CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime))
