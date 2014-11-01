-- ================================================
-- 存储管理初始化脚本 SQL Server 版 (2010-01-01)
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
-- 存储适配器数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Storage_Adapter]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Storage_Adapter]
    CREATE TABLE [dbo].[tb_Storage_Adapter]
    (
        [Id] [nvarchar] (36) NOT NULL,
        [Name] [nvarchar] (50) NULL,
        [EntityClassName] [nvarchar] (400) NULL
    )

    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Storage_Adapter] ADD CONSTRAINT [PK_tb_Storage_Adapter] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 存储节点数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Storage_Node]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Storage_Node]
    CREATE TABLE [dbo].[tb_Storage_Node]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [StorageSchemaId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
        [Type] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [ProviderName] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NULL,
        [ConnectionString] [nvarchar] (800) COLLATE Chinese_PRC_CI_AS NULL,
        [ConnectionState] [int] NULL,
        [OrderId] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,
        [Status] [int] NULL,
        [Remark] [nvarchar] (200) COLLATE Chinese_PRC_CI_AS NULL,
        [UpdateDate] [datetime] NULL,
        [CreateDate] [datetime] NULL
    )

    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Storage_Node] ADD CONSTRAINT [PK_tb_Storage_Node] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 存储架构数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Storage_Schema]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Storage_Schema]
    CREATE TABLE [dbo].[tb_Storage_Schema]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [ApplicationId] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NULL,
        [AdapterClassName] [nvarchar] (400) COLLATE Chinese_PRC_CI_AS NULL,
        [StrategyClassName] [nvarchar] (400) COLLATE Chinese_PRC_CI_AS NULL,
        [Options] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
        [OrderId] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,
        [Remark] [nvarchar] (200) COLLATE Chinese_PRC_CI_AS NULL,
        [UpdateDate] [datetime] NULL,
        [CreateDate] [datetime] NULL
    )

    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Storage_Schema] ADD CONSTRAINT [PK_tb_Storage_Schema] PRIMARY KEY CLUSTERED ([Id])
END
GO
