/****** Object:  Table tb_DigitalNumber    Script Date: 08/15/2010 20:36:21 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID('tb_DigitalNumber') AND OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE tb_DigitalNumber
GO
/****** Object:  Table tb_DigitalNumber    Script Date: 08/15/2010 20:36:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID('DigitalNumber') AND OBJECTPROPERTY(id, 'IsUserTable') = 1)
BEGIN
CREATE TABLE tb_DigitalNumber(
	Name nvarchar(100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	Expression nvarchar(200) COLLATE Chinese_PRC_CI_AS NULL,
	Seed int NULL,
	ModifiedDate datetime NULL,
	CreatedDate datetime NULL,
    CONSTRAINT PK_tb_DateRunningNumber PRIMARY KEY CLUSTERED 
    (
	    Name ASC
    )
)
END
GO

INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Key_32DigitGuid', '{guid:N}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Key_Guid', '{guid}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Key_Random_10', '{random:abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ:10}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Key_RunningNumber', '{date:yyyyMMdd}{dailyIncrement:seed:10}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Key_Sessio', '{guid}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Key_Timestamp', '{date:yyyyMMddHHmmssfff}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Example_01', '{int:seed:8}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Example_02', '{date:yyyyMMddHHmmssfff}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Example_03', '{date:yyyyMMddHH}{dailyIncrement:seed:8}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Example_04', '{date:yyyyMMdd}{tag:-}{int:seed}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Example_05', '{guid}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, ModifiedDate, CreatedDate) VALUES ('Example_06', '{random:abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ:10}', 0, '2000-01-01', '2000-01-01');
GO
