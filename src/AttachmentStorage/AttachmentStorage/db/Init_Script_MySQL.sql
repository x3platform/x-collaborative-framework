-- ================================================
-- 附件存储管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: sys_files
-- CREATE DATABASE sys_files

-- 创建表: tb_Attachment_File
CREATE TABLE tb_Attachment_File (
    Id varchar(36) NOT NULL ,
    EntityId varchar(50) NULL ,
    EntityClassName varchar(400) NULL ,
    AttachmentName varchar(100) NULL ,
    VirtualPath varchar(1000) NULL ,
    FolderRule varchar(100) NULL ,
    FileType varchar(20) NULL ,
    FileSize int NULL ,
    FileStatus int NULL ,
    OrderId varchar(40) NULL ,
    CreatedDate datetime NULL ,
    StorageNodeIndex int NULL 
);

-- 设置查询索引: EntityId, EntityClassName
CREATE INDEX IX_tb_Attachment_File_EntityId_EntityClassName ON tb_Attachment_File (EntityId ASC, EntityClassName ASC);

-- 设置主键: Id
ALTER TABLE tb_Attachment_File ADD PRIMARY KEY (Id);

-- 创建表: tb_Attachment_DistributedFile
CREATE TABLE tb_Attachment_DistributedFile (
    Id varchar(36) NOT NULL ,
    VirtualPath varchar(1000) NULL ,
    FileData blob NULL ,
    CreatedDate datetime NULL 
);

ALTER TABLE tb_Attachment_DistributedFile ADD PRIMARY KEY (Id);

-- 创建表: tb_Attachment_Warn
CREATE TABLE tb_Attachment_Warn (
    Id varchar(36) NOT NULL ,
    WarnType varchar(20) NULL ,
    Message varchar(200) NULL ,
    AttachmentStorageId nvarchar(36) NULL ,
    VirtualPath varchar(1000) NULL ,
    AttachmentName varchar(100) NULL ,
    FileType varchar(20) NULL ,
    FileSize int NULL ,
    CreatedDate datetime NULL 
);

-- 设置主键: Id
ALTER TABLE tb_Attachment_Warn ADD CONSTRAINT PK_tb_Attachment_Warn PRIMARY KEY CLUSTERED (Id);
