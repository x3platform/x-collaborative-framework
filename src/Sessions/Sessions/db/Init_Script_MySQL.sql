-- ================================================
-- 会话管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: `sys_sessions`
-- CREATE DATABASE `sys_sessions`

-- 删除表: `tb_AccountCache`
-- DROP TABLE IF EXISTS `tb_AccountCache`;

-- 创建表: `tb_AccountCache`
CREATE TABLE IF NOT EXISTS `tb_AccountCache`
(
    `AccountIdentity` varchar(100) NOT NULL,
    `AccountCacheValue` varchar(50) NOT NULL,
    `AccountObject` text,
    `AccountObjectType` varchar(400) default NULL,
    `IP` varchar(30),
    `BeginDate` datetime NULL,
    `EndDate` datetime NULL,
    `UpdateDate` datetime NULL
);
    
-- 设置主键: `Id`
ALTER TABLE `tb_AccountCache` ADD CONSTRAINT `PK_tb_AccountCache` PRIMARY KEY CLUSTERED (`AccountIdentity`);

-- 设置索引: `EndDate`
CREATE INDEX `IX_tb_Account_EndDate` ON `tb_AccountCache` (`EndDate`);
    
-- 设置索引: `UpdateDate`
CREATE INDEX `IX_tb_Account_UpdateDate` ON `tb_AccountCache` (`UpdateDate`);