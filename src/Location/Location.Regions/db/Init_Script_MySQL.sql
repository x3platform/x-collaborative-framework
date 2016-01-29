-- ================================================
-- 应用管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: `sys_config`
-- CREATE DATABASE `sys_config`

-- 创建表: `tb_Regions`
CREATE TABLE IF NOT EXISTS `tb_Region` (
	`Id` varchar(36) NOT NULL COMMENT '标识',
	`Name` varchar(50) NULL COMMENT '名称',
	`Path` varchar(200) NULL COMMENT '完整路径'
) COMMENT = '区域信息';

-- 设置主键: `Id`
ALTER TABLE `tb_Region` ADD CONSTRAINT `PK_tb_Region` PRIMARY KEY CLUSTERED (`Id`);
