-- ================================================
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

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Region]') AND TYPE IN (N'U'))
BEGIN
    -- 创建表: [tb_Region]
    CREATE TABLE [dbo].[tb_Region]
    (
        [Id] [nvarchar] (36) COLLATE Chinese_PRC_CI_AS NOT NULL,
        [Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,
	    [Path] [nvarchar] (200) COLLATE Chinese_PRC_CI_AS NULL
    )
    -- 设置主键: [Id]
    ALTER TABLE [dbo].[tb_Region] ADD CONSTRAINT [PK_tb_Region] PRIMARY KEY CLUSTERED ([Id])
END
GO
