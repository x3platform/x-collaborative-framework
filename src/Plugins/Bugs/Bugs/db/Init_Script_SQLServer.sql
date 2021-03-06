﻿-- ================================================
-- 问题跟踪初始化脚本(2012-01-01)
-- ================================================

-- ================================================
-- sys.objects 表的 xtype 字段说明
-- TYPE IN (N'P', N'PC') 
-- P = 存储过程 
-- PC = 程序集 (CLR) 存储过程
-- TYPE IN (N'U')
-- U = 用户表 
-- MSDN: http://msdn.microsoft.com/zh-cn/library/ms177596(v=sql.105).aspx
-- ================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 问题数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Bug]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Bug]
    CREATE TABLE [dbo].[tb_Bug]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [Code] [nvarchar] (30) COLLATE Chinese_PRC_CI_AS NULL,
        [AccountId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [AccountName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [CategoryId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [CategoryIndex] [nvarchar] (200) COLLATE Chinese_PRC_CI_AS NULL,
        [ProjectId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [Title] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [Content] [nvarchar] (2000) COLLATE Chinese_PRC_CI_AS NULL,
        [Tags] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [IsInternal] [bit] NULL,
        [AssignToAccountId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [AssignToAccountName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [SimilarBugIds] [nvarchar] (400) COLLATE Chinese_PRC_CI_AS NULL,
        [Priority] [int] NULL,
        [Status] [int] NULL,
        [OrderId] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_Station_Bug_OrderId] DEFAULT ((0)),
        [ModifiedDate] [datetime] NULL,
        [CreatedDate] [datetime] NULL CONSTRAINT [DF_Station_Bug_CreatedDate] DEFAULT (GETDATE())
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Bug] ADD CONSTRAINT [PK_Station_Bug] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 问题类别数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Bug_Category]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Bug_Category]
    CREATE TABLE [dbo].[tb_Bug_Category]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [AccountId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [AccountName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [CategoryIndex] [nvarchar] (200) COLLATE Chinese_PRC_CI_AS NULL,
        [Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
        [OrderId] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,
        [Status] [int] NULL,
        [ModifiedDate] [datetime] NULL,
        [CreatedDate] [datetime] NULL
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Bug_Category] ADD CONSTRAINT [PK_tb_Bug_Category] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 问题类别授权范围数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Bug_Category_Scope]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Bug_Category_Scope]
    CREATE TABLE [dbo].[tb_Bug_Category_Scope]
    (
        [EntityId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [EntityClassName] [nvarchar] (400) COLLATE Chinese_PRC_CI_AS NULL,
        [AuthorityId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [AuthorizationObjectId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [AuthorizationObjectType] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL
    )
    -- 设置主键: [EntityId], [AuthorityId], [AuthorizationObjectId], [AuthorizationObjectType]
    ALTER TABLE [dbo].[tb_Bug_Category_Scope] ADD CONSTRAINT [PK_tb_Bug_Category_Scope] PRIMARY KEY CLUSTERED ([EntityId], [AuthorityId], [AuthorizationObjectId], [AuthorizationObjectType])
END
GO

-- ================================================
-- 问题反馈数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Bug_Comment]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Bug_Comment]
    CREATE TABLE [dbo].[tb_Bug_Comment]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [BugId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [AccountId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [Title] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [Content] [nvarchar] (800) COLLATE Chinese_PRC_CI_AS NULL,
        [IsPrivate] [bit] NULL,
        [ModifiedDate] [datetime] NULL,
        [CreatedDate] [datetime] NULL
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Bug_Comment] ADD CONSTRAINT [PK_tb_Bug_Comment] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 问题状态历史记录数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Bug_History]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Bug_History]
    CREATE TABLE [dbo].[tb_Bug_History]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [BugId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [AccountId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [FromStatus] [int] NULL,
        [ToStatus] [int] NULL,
        [CreatedDate] [datetime] NULL
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Bug_History] ADD CONSTRAINT [PK_tb_Bug_History] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 初始化应用默认配置
-- ================================================

-- 应用信息
INSERT INTO [tb_Application] ([Id],[AccountId],[ParentId],[Code],[ApplicationName],[ApplicationDisplayName],[ApplicationKey],[ApplicationSecretSignal],[PinYin],[Description],[AdministratorEmail],[Authority],[IsSync],[ReciveUrl],[ReciveFilter],[IconPath],[BigIconPath],[HelpUrl],[LicenseStatus],[Hidden],[Lock],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000001','00000000-0000-0000-0000-000000000010','0006','Bug','问题跟踪','3231ad1fa59847ddb7be46a14d0558ab','hello','','问题跟踪','',1,False,'','','','','','',,,'',1,'','2000-01-01','2000-01-01')
GO

INSERT INTO [tb_Application_Scope] ([ApplicationId],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId],[ModifiedDate],[CreatedDate]) VALUES ('00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000001001','Account','00000000-0000-0000-0000-000000100000','2000-01-01','2000-01-01')
GO

-- 应用选项信息

-- 应用方法信息
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E01-M01','00000000-0000-0000-0000-000000000006','04-03-E01-M01','bugzilla.query','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugWrapper,X3Platform.Plugins.Bugs",methodName:"GetPaging"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E01-M03','00000000-0000-0000-0000-000000000006','04-03-E01-M03','bugzilla.findOne','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugWrapper,X3Platform.Plugins.Bugs",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E01-M05','00000000-0000-0000-0000-000000000006','04-03-E01-M05','bugzilla.save','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugWrapper,X3Platform.Plugins.Bugs",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E01-M06','00000000-0000-0000-0000-000000000006','04-03-E01-M06','bugzilla.delete','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugWrapper,X3Platform.Plugins.Bugs",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E01-M10','00000000-0000-0000-0000-000000000006','04-03-E01-M10','bugzilla.queryMyBug','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugWrapper,X3Platform.Plugins.Bugs",methodName:"GetMyBugPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E02-M01','00000000-0000-0000-0000-000000000006','04-03-E02-M01','bugzilla.category.query','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCategoryWrapper,X3Platform.Plugins.Bugs",methodName:"GetPaging"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E02-M05','00000000-0000-0000-0000-000000000006','04-03-E02-M05','bugzilla.category.save','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCategoryWrapper,X3Platform.Plugins.Bugs",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E02-M06','00000000-0000-0000-0000-000000000006','04-03-E02-M06','bugzilla.category.delete','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCategoryWrapper,X3Platform.Plugins.Bugs",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E02-M07','00000000-0000-0000-0000-000000000006','04-03-E02-M07','bugzilla.category.getDynamicTreeView','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCategoryWrapper,X3Platform.Plugins.Bugs",methodName:"GetDynamicTreeView"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E03-M02','00000000-0000-0000-0000-000000000006','04-03-E03-M02','bugzilla.comment.findAll','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCommentWrapper,X3Platform.Plugins.Bugs",methodName:"FindAll"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E03-M05','00000000-0000-0000-0000-000000000006','04-03-E03-M05','bugzilla.comment.save','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCommentWrapper,X3Platform.Plugins.Bugs",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Method] ([Id],[ApplicationId],[Code],[Name],[Description],[Type],[Options],[EffectScope],[Version],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('04-03-E03-M06','00000000-0000-0000-0000-000000000006','04-03-E03-M06','bugzilla.comment.delete','','generic','{className:"X3Platform.Plugins.Bugs.Ajax.BugCommentWrapper,X3Platform.Plugins.Bugs",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01')
GO

-- 应用参数分组信息
INSERT INTO [tb_Application_SettingGroup] ([Id],[ApplicationId],[ParentId],[Code],[Name],[DisplayName],[ContentType],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('a4c35065-0eff-4bb1-b222-d7d697c05760','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','100033','应用管理_协同平台_问题跟踪_问题状态','问题状态',,'1',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_SettingGroup] ([Id],[ApplicationId],[ParentId],[Code],[Name],[DisplayName],[ContentType],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('aaae4e87-d84f-4116-9816-6cc0d01ed0e3','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','100034','应用管理_协同平台_问题跟踪_问题优先级','问题优先级',,'2',1,'','2000-01-01','2000-01-01')
GO

-- 应用参数信息
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('eed1cceb-73c5-4bcb-b14a-69a090f69756','00000000-0000-0000-0000-000000000006','a4c35065-0eff-4bb1-b222-d7d697c05760','100041','新问题','0','0001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('0a402464-c200-405b-b131-29409700c4e5','00000000-0000-0000-0000-000000000006','a4c35065-0eff-4bb1-b222-d7d697c05760','100042','处理中','1','0002',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('4409806f-777c-4c3f-b81f-2397c179663a','00000000-0000-0000-0000-000000000006','a4c35065-0eff-4bb1-b222-d7d697c05760','100043','已解决','2','0003',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('851685a7-b5bc-475d-9107-83d5ca1459e2','00000000-0000-0000-0000-000000000006','a4c35065-0eff-4bb1-b222-d7d697c05760','100044','延后处理','3','0004',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('b0ab6eef-59c1-4775-b6b8-7fc01c95aa3e','00000000-0000-0000-0000-000000000006','a4c35065-0eff-4bb1-b222-d7d697c05760','100045','无法修复','4','0005',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('7a0288db-6a19-4ed0-8f1d-36691b55551e','00000000-0000-0000-0000-000000000006','a4c35065-0eff-4bb1-b222-d7d697c05760','100046','已关闭','5','0006',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('5ed0e16b-0721-4c19-9ea3-1e23b0b93e50','00000000-0000-0000-0000-000000000006','aaae4e87-d84f-4116-9816-6cc0d01ed0e3','100038','一般','0','0001',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('44d67c41-3688-4fa5-84b5-c28a872aa10f','00000000-0000-0000-0000-000000000006','aaae4e87-d84f-4116-9816-6cc0d01ed0e3','100039','重要','1','0002',1,'','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Setting] ([Id],[ApplicationId],[ApplicationSettingGroupId],[Code],[Text],[Value],[OrderId],[Status],[Remark],[ModifiedDate],[CreatedDate]) VALUES ('10d123b1-61a0-4ab3-8684-89d42f7f5257','00000000-0000-0000-0000-000000000006','aaae4e87-d84f-4116-9816-6cc0d01ed0e3','100040','紧急','2','0003',1,'','2000-01-01','2000-01-01')
GO

-- 应用菜单信息
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('4176b203-ca6b-4698-9240-9e4bd0ae19e4','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111001','问题列表','问题列表','/apps/pages/bugzilla/','_self','ApplicationMenu','','','MenuItem',0,'','10001',1,'','问题跟踪\问题查询','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('c6f42b69-79d2-44a1-a53e-b8c509ad522e','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111002','提交新的问题','提交新的问题','/apps/pages/bugzilla/bugzilla-form.aspx','_blank','ApplicationMenu','','','MenuItem',0,'','10002',1,'','问题跟踪\提交新的问题','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('bb259313-8578-48fc-b3de-9b0fb8b10344','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111003','我的问题列表','我的问题列表','/apps/pages/bugzilla/my-bugzilla-list.aspx','_self','ApplicationMenu','','','MenuItem',0,'','10003',1,'','问题跟踪\我的问题列表','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('3b1599c3-20c9-4ce0-a7c2-4562fdda4d87','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111004','基础设置','基础设置','#','_self','ApplicationMenu','','','MenuGroup',0,'','10004',1,'','问题跟踪\基础设置','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('7e68b9b2-3fa1-4ebd-83b2-4c6dad9ac716','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111005','参数设置','参数设置','/apps/pages/bugzilla/bugzilla-setting-list.aspx','_self','ApplicationMenu','','','MenuItem',0,'','10005',1,'','问题跟踪\参数设置','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('daf8baf5-abaf-4ade-8267-895edc257fc3','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111006','类别设置','类别设置','/apps/pages/bugzilla/bugzilla-category-list.aspx','_self','ApplicationMenu','','','MenuItem',0,'','10006',1,'','问题跟踪\类别设置','2000-01-01','2000-01-01')
GO

INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('3b1599c3-20c9-4ce0-a7c2-4562fdda4d87','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('4176b203-ca6b-4698-9240-9e4bd0ae19e4','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('7e68b9b2-3fa1-4ebd-83b2-4c6dad9ac716','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('bb259313-8578-48fc-b3de-9b0fb8b10344','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('c6f42b69-79d2-44a1-a53e-b8c509ad522e','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('daf8baf5-abaf-4ade-8267-895edc257fc3','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
GO

-- 应用错误信息
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('4176b203-ca6b-4698-9240-9e4bd0ae19e4','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111001','问题列表','问题列表','/apps/pages/bugzilla/','_self','ApplicationMenu','','','MenuItem',0,'','10001',1,'','问题跟踪\问题查询','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('c6f42b69-79d2-44a1-a53e-b8c509ad522e','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111002','提交新的问题','提交新的问题','/apps/pages/bugzilla/bugzilla-form.aspx','_blank','ApplicationMenu','','','MenuItem',0,'','10002',1,'','问题跟踪\提交新的问题','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('bb259313-8578-48fc-b3de-9b0fb8b10344','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111003','我的问题列表','我的问题列表','/apps/pages/bugzilla/my-bugzilla-list.aspx','_self','ApplicationMenu','','','MenuItem',0,'','10003',1,'','问题跟踪\我的问题列表','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('3b1599c3-20c9-4ce0-a7c2-4562fdda4d87','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111004','基础设置','基础设置','#','_self','ApplicationMenu','','','MenuGroup',0,'','10004',1,'','问题跟踪\基础设置','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('7e68b9b2-3fa1-4ebd-83b2-4c6dad9ac716','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111005','参数设置','参数设置','/apps/pages/bugzilla/bugzilla-setting-list.aspx','_self','ApplicationMenu','','','MenuItem',0,'','10005',1,'','问题跟踪\参数设置','2000-01-01','2000-01-01')
INSERT INTO [tb_Application_Menu] ([Id],[ApplicationId],[ParentId],[Code],[Name],[Description],[Url],[Target],[MenuType],[IconPath],[BigIconPath],[DisplayType],[HasChild],[ContextObject],[OrderId],[Status],[Remark],[FullPath],[ModifiedDate],[CreatedDate]) VALUES ('daf8baf5-abaf-4ade-8267-895edc257fc3','00000000-0000-0000-0000-000000000006','00000000-0000-0000-0000-000000000000','111006','类别设置','类别设置','/apps/pages/bugzilla/bugzilla-category-list.aspx','_self','ApplicationMenu','','','MenuItem',0,'','10006',1,'','问题跟踪\类别设置','2000-01-01','2000-01-01')
GO

INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('3b1599c3-20c9-4ce0-a7c2-4562fdda4d87','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('4176b203-ca6b-4698-9240-9e4bd0ae19e4','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('7e68b9b2-3fa1-4ebd-83b2-4c6dad9ac716','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('bb259313-8578-48fc-b3de-9b0fb8b10344','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('c6f42b69-79d2-44a1-a53e-b8c509ad522e','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
INSERT INTO [tb_Application_Menu_Scope] ([EntityId],[EntityClassName],[AuthorityId],[AuthorizationObjectType],[AuthorizationObjectId]) VALUES ('daf8baf5-abaf-4ade-8267-895edc257fc3','X3Platform.Apps.Model.ApplicationMenuInfo, X3Platform.Apps','00000000-0000-0000-0000-000000000001','Role','00000000-0000-0000-0000-000000000000')
GO
