-- ================================================
-- 验证码管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: sys_config
-- CREATE DATABASE sys_config

-- 创建表: tb_VerificationCode
CREATE TABLE tb_VerificationCode (
	Id varchar(36) NOT NULL ,
	ObjectType  varchar(20) NULL ,
	ObjectValue  varchar(36) NULL ,
	Code  varchar(6) NULL ,
	ValidationType  varchar(20) NULL ,
	CreateDate  datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_VerificationCode ADD CONSTRAINT PK_tb_VerificationCode PRIMARY KEY CLUSTERED (Id);