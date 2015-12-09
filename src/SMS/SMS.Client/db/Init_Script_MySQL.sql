-- ================================================
-- 会话管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: `sys_sessions`
-- CREATE DATABASE `sys_sessions`

-- 删除表: `tb_SMS`
-- DROP TABLE IF EXISTS `tb_SMS`;

-- 创建表: `tb_SMS`
CREATE TABLE IF NOT EXISTS `tb_SMS`
(
    `AccountIdentity` varchar(100) NOT NULL,
    `SMSValue` varchar(50) NOT NULL,
    `AccountObject` text,
    `AccountObjectType` varchar(400) default NULL,
    `IP` varchar(30),
    `ValidFrom` datetime NULL,
    `ValidTo` datetime NULL,
    `Date` datetime NULL
);
    
-- 设置主键: `Id`
ALTER TABLE `tb_SMS` ADD CONSTRAINT `PK_tb_SMS` PRIMARY KEY CLUSTERED (`AccountIdentity`);

-- 设置索引: `ValidTo`
CREATE INDEX `IX_tb_Account_ValidTo` ON `tb_SMS` (`ValidTo`);
    
-- 设置索引: `Date`
CREATE INDEX `IX_tb_Account_Date` ON `tb_SMS` (`Date`);
