-- ================================================
-- 会话管理初始化脚本 SQL Server 版 (2010-01-01)
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
-- 帐号缓存数据表
-- ================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_SMS]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_SMS]
    CREATE TABLE [dbo].[tb_SMS]
    (
        [AccountIdentity] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [SMSValue] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [AccountObject] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
        [AccountObjectType] [nvarchar] (400) COLLATE Chinese_PRC_CI_AS NULL,
        [IP] [nvarchar] (30) COLLATE Chinese_PRC_CI_AS NULL,
        [ValidFrom] [datetime] NULL,
        [ValidTo] [datetime] NULL,
        [Date] [datetime] NULL
    )
    
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_SMS] ADD CONSTRAINT [PK_tb_SMS] PRIMARY KEY CLUSTERED ([AccountIdentity])
    
    -- 设置主键: [Id]
    CREATE NONCLUSTERED INDEX [IX_tb_Account_ValidTo] ON [dbo].[tb_SMS] ([ValidTo])
    
    -- 设置主键: [Id]
    CREATE NONCLUSTERED INDEX [IX_tb_Account_Date] ON [dbo].[tb_SMS] ([Date])
        
END
GO
