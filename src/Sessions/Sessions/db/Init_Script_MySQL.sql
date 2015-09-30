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
    `ValidFrom` datetime NULL,
    `ValidTo` datetime NULL,
    `Date` datetime NULL
);
    
-- 设置主键: `Id`
ALTER TABLE `tb_AccountCache` ADD CONSTRAINT `PK_tb_AccountCache` PRIMARY KEY CLUSTERED (`AccountIdentity`);

-- 设置索引: `ValidTo`
CREATE INDEX `IX_tb_Account_ValidTo` ON `tb_AccountCache` (`ValidTo`);
    
-- 设置索引: `Date`
CREATE INDEX `IX_tb_Account_Date` ON `tb_AccountCache` (`Date`);
