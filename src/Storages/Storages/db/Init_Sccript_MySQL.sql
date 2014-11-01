-- ================================================
-- 存储管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: sys_storages
-- CREATE DATABASE sys_storages

-- 创建表: tb_Storage_Schema
CREATE TABLE IF NOT EXISTS tb_Storage_Schema (
	Id varchar(36) NOT NULL,
	ApplicationId varchar(36) NULL,
	AdapterClassName varchar(400) NULL,
	StrategyClassName varchar(400) NULL,
	Options text NULL,
	OrderId varchar(20) NULL,
	Remark varchar(200) NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Storage_Schema ADD CONSTRAINT PK_tb_Storage_Schema PRIMARY KEY CLUSTERED (Id);

-- 创建表: tb_Storage_Node
CREATE TABLE IF NOT EXISTS tb_Storage_Node(
	Id varchar(36) NOT NULL,
	StorageSchemaId varchar(36) NULL,
	Name varchar(50) NULL,
	Type varchar(36) NULL,
	ProviderName varchar(100) NULL,
	ConnectionString varchar(800) NULL,
	ConnectionState int NULL,
	OrderId varchar(20) NULL,
	Status int NULL,
	Remark varchar(200) NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Storage_Node ADD CONSTRAINT PK_tb_Storage_Node PRIMARY KEY CLUSTERED (Id);

-- 创建表: tb_Storage_Adapter
CREATE TABLE IF NOT EXISTS tb_Storage_Adapter(
	Id varchar(36) NOT NULL,
	Name varchar(50) NULL,
	EntityClassName varchar(400) NULL
);

-- 设置主键: Id
ALTER TABLE tb_Storage_Adapter ADD CONSTRAINT PK_tb_Storage_Adapter PRIMARY KEY CLUSTERED (Id);
