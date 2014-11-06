-- 创建数据库: `db_plugins_contacts`
-- CREATE DATABASE `db_plugins_contacts`

-- 创建表: `tb_Contact`
CREATE TABLE IF NOT EXISTS `tb_Contact` (
  `Id` varchar(36) NOT NULL,
  `AccountId` varchar(36),
  `Name` varchar(50),
  `Mobile` varchar(50),  
  `Telephone` varchar(50),  
  `Email` varchar(50),
  `QQ` varchar(50),
  `UpdateDate` datetime,
  `CreateDate` datetime
);

-- 设置主键: `Id`
ALTER TABLE `tb_Contact` ADD CONSTRAINT `PK_tb_Contact` PRIMARY KEY CLUSTERED (`Id`);
