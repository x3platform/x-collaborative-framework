SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.tb_VerificationCode(
	Id nvarchar(36) NOT NULL,
	Name nvarchar(400) NULL,
	Description nvarchar(800) NULL,
	Locking int NULL,
	Tags nvarchar(50) NULL,
	OrderId nvarchar(20) NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL,
 CONSTRAINT PK_tb_VerificationCode PRIMARY KEY CLUSTERED 
(
	Id ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON PRIMARY
) ON PRIMARY

GO

ALTER TABLE dbo.tb_VerificationCode ADD  CONSTRAINT DF_tb_VerificationCode_OrderId  DEFAULT ((0)) FOR OrderId
GO

