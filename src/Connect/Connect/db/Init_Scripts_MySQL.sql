-- ================================================
-- 应用连接管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: `sys_connect`
-- CREATE DATABASE `sys_connect`

-- 创建表: `tb_Connect`
CREATE TABLE IF NOT EXISTS `tb_Connect` (
    `Id` varchar(36),
    `AccountId` varchar(36),
    `AccountName` varchar(50),
    `AppKey` varchar(36),
    `AppSecret` varchar(50),
    `AppType` varchar(20),
    `Code` varchar(30),
    `Name` varchar(50),
    `Description` varchar(800),
    `Domain` varchar(200),
    `RedirectUri` varchar(800),
    `Options` text,
    `IsInternal` bit,
    `AuthorizationScope` text,
    `CertifiedCode` varchar(36),
    `Status` int,
    `ModifiedDate` datetime,
    `CreatedDate` datetime
);

-- 设置主键: `Id`
ALTER TABLE `tb_Connect` ADD CONSTRAINT `PK_tb_Connect` PRIMARY KEY CLUSTERED (`Id`);

-- 创建表: `tb_Connect_AuthorizationCode`
CREATE TABLE IF NOT EXISTS `tb_Connect_AuthorizationCode`
(
    `Id` varchar(36),
    `AppKey` varchar(36),
    `AccountId` varchar(36),
    `AuthorizationScope` text,
    `ModifiedDate` datetime,
    `CreatedDate` datetime
);

-- 设置主键: `Id`
ALTER TABLE `tb_Connect_AuthorizationCode` ADD CONSTRAINT `PK_tb_Connect_AuthorizationCode` PRIMARY KEY CLUSTERED (`Id`);

-- 创建表: `tb_Connect_AccessToken`
CREATE TABLE IF NOT EXISTS `tb_Connect_AccessToken`
(
	`Id` varchar(36),
	`AppKey` varchar(36),
	`AccountId` varchar(36),
	`ExpireDate` datetime,
	`RefreshToken` varchar(36),
	`ModifiedDate` datetime,
	`CreatedDate` datetime
);

-- 设置主键: `Id`
ALTER TABLE `tb_Connect_AccessToken` ADD CONSTRAINT `PK_tb_Connect_AccessToken` PRIMARY KEY CLUSTERED (`Id`);

-- 创建表: `tb_Connect_Call`
CREATE TABLE IF NOT EXISTS `tb_Connect_Call`
(
    `Id` varchar(36),
    `AppKey` varchar(36),
    `RequestUri` varchar(800),
    `RequestData` text,
    `StartTime` datetime,
    `FinishTime` datetime,
    `TimeSpan` float,
    `ReturnCode` int,
    `IP` varchar(20),
    `Date` datetime
);

-- 设置主键: `Id`
ALTER TABLE `tb_Connect_Call` ADD CONSTRAINT `PK_tb_Connect_Call` PRIMARY KEY CLUSTERED (`Id`);
