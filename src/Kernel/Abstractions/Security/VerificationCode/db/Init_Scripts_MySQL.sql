-- ================================================
-- 验证码管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建表: tb_VerificationCode
CREATE TABLE tb_VerificationCode (
	Id varchar(36) NOT NULL ,
	ObjectType  varchar(20) NULL ,
	ObjectValue  varchar(50) NULL ,
	Code  varchar(8) NULL ,
	ValidationType  varchar(20) NULL ,
	CreatedDate  datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_VerificationCode ADD CONSTRAINT PK_tb_VerificationCode PRIMARY KEY CLUSTERED (Id);