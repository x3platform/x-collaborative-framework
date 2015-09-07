-- ������: tb_VerificationCode
CREATE TABLE dbo.tb_VerificationCode
(
	Id nvarchar(36) NOT NULL,
	ObjectType nvarchar(20) NULL,
	ObjectValue nvarchar(50) NULL,
	Code nvarchar(8) NULL,
	ValidationType nvarchar(20) NULL,
	CreatedDate datetime NULL
)
GO

-- ��������: Id
ALTER TABLE tb_VerificationCode ADD CONSTRAINT PK_tb_VerificationCode PRIMARY KEY CLUSTERED (Id)
GO

