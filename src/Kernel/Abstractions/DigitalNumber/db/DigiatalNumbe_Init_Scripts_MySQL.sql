-- ================================================
-- 应用管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: `sys_config`
-- CREATE DATABASE `sys_config`

-- 创建表: `tb_DigitalNumber`
CREATE TABLE `tb_DigitalNumber`(
	`Name` varchar(100) NOT NULL,
	`Expression` varchar(200) NULL,
	`Seed` int NULL,
	`UpdateDate` datetime NULL,
	`CreateDate` datetime NULL
);

-- 设置主键: `Name`
ALTER TABLE `tb_DigitalNumber` ADD CONSTRAINT `PK_tb_DigitalNumber` PRIMARY KEY CLUSTERED (`Name`);
