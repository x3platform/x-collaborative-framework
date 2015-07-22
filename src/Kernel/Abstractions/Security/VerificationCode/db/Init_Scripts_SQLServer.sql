-- 创建表: tb_VerificationCode
CREATE TABLE dbo.tb_VerificationCode
(
	Id nvarchar(36) NOT NULL,
	ObjectType nvarchar(20) NULL,
	ObjectValue nvarchar(30) NULL,
	Code nvarchar(8) NULL,
	ValidationType nvarchar(20) NULL,
	CreateDate datetime NULL
)
GO

-- 设置主键: Id
ALTER TABLE tb_VerificationCode ADD CONSTRAINT PK_tb_VerificationCode PRIMARY KEY CLUSTERED (Id)
GO

