-- ================================================
-- ��֤������ʼ���ű� MySQL �� (2010-01-01)
-- ================================================

-- �������ݿ�: sys_config
-- CREATE DATABASE sys_config

-- ������: tb_VerificationCode
CREATE TABLE tb_VerificationCode (
	Id varchar(36) NOT NULL ,
	ObjectType  varchar(20) NULL ,
	ObjectValue  varchar(36) NULL ,
	Code  varchar(6) NULL ,
	ValidationType  varchar(20) NULL ,
	CreateDate  datetime NULL
);

-- ��������: Id
ALTER TABLE tb_VerificationCode ADD CONSTRAINT PK_tb_VerificationCode PRIMARY KEY CLUSTERED (Id);