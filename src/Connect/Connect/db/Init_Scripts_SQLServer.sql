-- ================================================
-- 应用连接管理初始化脚本(2010-01-01)
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
-- 应用连接基本信息表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Connect]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Connect]
    CREATE TABLE [dbo].[tb_Connect]
    (
        [Id] [nvarchar] (36) NOT NULL,
        [AccountId] [nvarchar] (36),
        [AccountName] [nvarchar] (50),
        [AppKey] [nvarchar] (36),
        [AppSecret] [nvarchar] (50),
        [AppType] [nvarchar] (20),
        [Code] [nvarchar] (30),
        [Name] [nvarchar] (50),
        [Description] [nvarchar] (800),
        [Domain] [nvarchar] (200),
        [RedirectUri] [nvarchar] (800),
        [Options] [ntext],
        [IsInternal] [bit],
        [AuthorizationScope] [ntext],
        [CertifiedCode] [nvarchar] (36),
        [Status] [int] NULL,
        [UpdateDate] [datetime],
        [CreateDate] [datetime]
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Connect] ADD CONSTRAINT [PK_tb_Connect] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 应用连接授权码表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Connect_AuthorizationCode]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Connect_AuthorizationCode]
    CREATE TABLE [dbo].[tb_Connect_AuthorizationCode]
    (
        [Id] [nvarchar] (36) NOT NULL,
        [AppKey] [nvarchar] (36),
        [AccountId] [nvarchar] (36),
        [AuthorizationScope] [ntext],
        [UpdateDate] [datetime],
        [CreateDate] [datetime]
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Connect_AuthorizationCode] ADD CONSTRAINT [PK_tb_Connect_AuthorizationCode] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 应用连接访问令牌表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Connect_AccessToken]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Connect_AccessToken]
    CREATE TABLE [dbo].[tb_Connect_AccessToken]
    (
        [Id] [nvarchar] (36) NOT NULL,
        [AppKey] [nvarchar] (36),
        [AccountId] [nvarchar] (36),
	    [ExpireDate] [datetime],
        [RefreshToken] [nvarchar](36),
        [UpdateDate] [datetime],
        [CreateDate] [datetime]
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Connect_AccessToken] ADD CONSTRAINT [PK_tb_Connect_AccessToken] PRIMARY KEY CLUSTERED ([Id])
END
GO

-- ================================================
-- 应用连接调用记录
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Connect_Call]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Connect_Call]
    CREATE TABLE [dbo].[tb_Connect_Call]
    (
        [Id] [nvarchar] (36) NOT NULL,
        [AppKey] [nvarchar] (36),
        [RequestUri] [nvarchar] (800),
        [RequestData] [ntext],
        [StartTime] [datetime],
        [FinishTime] [datetime],
        [TimeSpan] [float],
        [ReturnCode] [int],
        [IP] [nvarchar] (20),
        [Date] [datetime]
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Connect_Call] ADD CONSTRAINT [PK_tb_Connect_Call] PRIMARY KEY CLUSTERED ([Id])
END
GO