-- ================================================
-- 应用管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: sys_config
-- CREATE DATABASE sys_config

-- 创建表: tb_DigitalNumber
CREATE TABLE tb_DigitalNumber(
	Name varchar(100) NOT NULL,
	Expression varchar(200) NULL,
	Seed int NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Name
ALTER TABLE tb_DigitalNumber ADD CONSTRAINT PK_tb_DigitalNumber PRIMARY KEY CLUSTERED (Name);

-- 默认数据
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Key_32DigitGuid', '{guid:N}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Key_Guid', '{guid}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Key_Random_10', '{random:abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ:10}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Key_RunningNumber', '{date:yyyyMMdd}{dailyIncrement:seed:10}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Key_Sessio', '{guid}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Key_Timestamp', '{date:yyyyMMddHHmmssfff}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Example_01', '{int:seed:8}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Example_02', '{date:yyyyMMddHHmmssfff}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Example_03', '{date:yyyyMMddHH}{dailyIncrement:seed:8}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Example_04', '{date:yyyyMMdd}{tag:-}{int:seed}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Example_05', '{guid}', 0, '2000-01-01', '2000-01-01');
INSERT tb_DigitalNumber (Name, Expression, Seed, UpdateDate, CreateDate) VALUES ('Example_06', '{random:abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ:10}', 0, '2000-01-01', '2000-01-01');
