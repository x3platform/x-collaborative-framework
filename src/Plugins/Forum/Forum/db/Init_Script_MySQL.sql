-- ================================================
-- 任务管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: db_forum
-- CREATE DATABASE db_forum

-- 创建表: tb_Forum_Category
CREATE TABLE IF NOT EXISTS tb_Forum_Category (
	Id varchar(36) NOT NULL,
	AccountId varchar(36) NULL,
	AccountName varchar(30) NULL,
	CategoryIndex varchar(200) NULL,
	Keywords varchar(50) NULL,
	Description varchar(200) NULL,
	Anonymous int NULL,
	PublishThreadPoint int NULL,
	PublishCommentPoint int NULL,
	TodayCount int NULL,
	WeekCount int NULL,
	MonthCount int NULL,
	TotalCount int NULL,
	Hidden int NULL,
	OrderId varchar(20) NULL,
	Status int NULL,
	Remark varchar(200) NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Forum_Category ADD CONSTRAINT PK_tb_Forum_Category PRIMARY KEY CLUSTERED (Id);

-- 创建表: tb_Forum_Category_Scope
CREATE TABLE IF NOT EXISTS tb_Forum_Category_Scope(
	EntityId varchar(36) NOT NULL,
	EntityClassName varchar(400) NULL,
	AuthorityId varchar(36) NOT NULL,
	AuthorizationObjectType varchar(20) NOT NULL,
	AuthorizationObjectId varchar(36) NOT NULL
);

-- 设置主键: EntityId, AuthorityId, AuthorizationObjectId, AuthorizationObjectType
ALTER TABLE tb_Forum_Category_Scope ADD CONSTRAINT PK_tb_Forum_Category_Scope PRIMARY KEY CLUSTERED (EntityId, AuthorityId, AuthorizationObjectId, AuthorizationObjectType);

-- 创建表: tb_Forum_Thread
CREATE TABLE IF NOT EXISTS tb_Forum_Thread(
	Id varchar(36) NOT NULL,
	AccountId varchar(36) NULL,
	AccountName varchar(50) NULL,
	CategoryId varchar(36) NULL,
	CategoryIndex varchar(200) NULL,
	Code varchar(100) NULL,
	Title varchar(50) NULL,
	Content text NULL,
	Click int NULL,
	LatestCommentAccountId varchar(36) NULL,
	LatestCommentAccountName varchar(50) NULL,
	CommentCount int NULL,
	AttachmentFileCount int NULL,
	Anonymous int NULL,
	IsTop int NULL,
	TopExpiryDate datetime NULL,
	IsHot int NULL,
	HotExpiryDate datetime NULL,
	IsEssential int NULL,
	Locking int NULL,
	IP varchar(20) NULL,
	Status int NULL,
	UpdateHistoryLog varchar(400) NULL,
	StorageNodeIndex int NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Forum_Thread ADD CONSTRAINT PK_tb_Forum_Thread PRIMARY KEY CLUSTERED (Id);

-- 创建表: tb_Forum_Comment
CREATE TABLE IF NOT EXISTS tb_Forum_Comment(
	Id varchar(36) NOT NULL,
	AccountId varchar(36) NULL,
	AccountName varchar(50) NULL,
	ThreadId varchar(36) NULL,
	ReplyCommentId varchar(36) NULL,
	Content text NULL,
	AttachmentFileCount int NULL,
	Anonymous int NULL,
	IP varchar(20) NULL,
	UpdateHistoryLog varchar(400) NULL,
	StorageNodeIndex int NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Forum_Comment ADD CONSTRAINT PK_tb_Forum_Comment PRIMARY KEY CLUSTERED (Id);

-- 创建表: tb_Forum_Member
CREATE TABLE IF NOT EXISTS tb_Forum_Member (
	Id varchar(36) NOT NULL,
	AccountId varchar(36) NULL,
	AccountName varchar(20) NULL,
	OrganizationPath varchar(200) NULL,
	Headship varchar(200) NULL,
	Signature text NULL,
	IconPath varchar(400) NULL,
	Point int NULL,
	PublishThreadCount int NULL,
	PublishCommentCount int NULL,
	FollowCount int NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Forum_Member ADD CONSTRAINT PK_tb_Forum_Member PRIMARY KEY CLUSTERED (Id);

-- 创建表: tb_Forum_Follow_Account
CREATE TABLE IF NOT EXISTS tb_Forum_Follow_Account (
	Id varchar(36) NOT NULL,
	AccountId varchar(36) NOT NULL,
	FollowAccountId varchar(36) NOT NULL,
	CreateDate datetime NOT NULL
);

-- 设置主键: Id
ALTER TABLE tb_Forum_Follow_Account ADD CONSTRAINT PK_tb_Forum_Follow_Account PRIMARY KEY CLUSTERED (Id);

INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('125d5281-b7ee-4e03-968f-59deb6605bd0', '0', NULL, '工作园地\沟通与交流', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('146ee670-abe7-4fcb-a96e-0c37e9750c15', '0', NULL, '生活频道\生活贴士', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('293e16fa-5e55-443b-90f8-f2c6d21dd9d7', '0', NULL, '工作园地\投诉与建议', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('30386ce3-da2c-4ed9-9028-d888064a4fb6', '0', NULL, '生活频道\闲聊灌水', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('3e716582-9cad-49b7-b932-75d605196a40', '0', NULL, '工作园地\杂谈', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('58c3a4c4-35cc-4715-b9ed-491d91ab62da', '0', NULL, '生活频道\浓情祝语 ', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('5a5d6952-892a-4f56-853f-27047aa934ed', '0', NULL, '工作园地\新员工天地', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('6b10a072-8f77-4813-b66d-bc9ad887e2f6', '0', NULL, '生活频道\幽默人生', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('7a85502f-2469-40c5-99fc-9955bd5544a7', '0', NULL, '生活频道\学习分享', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('98881ab4-ce89-4439-b727-dba4533268c3', '0', NULL, '生活频道', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('9bb69e4c-a8eb-4261-8550-7cba979b5baf', '0', NULL, '工作园地', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
INSERT tb_Forum_Category (Id, AccountId, AccountName, CategoryIndex, Keywords, Description, Anonymous, PublishThreadPoint, PublishCommentPoint, TodayCount, WeekCount, MonthCount, TotalCount, Hidden, OrderId, Status, Remark, UpdateDate, CreateDate) VALUES ('cc649e5f-4c75-4ade-ac36-3d72c6ca9def', '0', NULL, '工作园地\领导力及职业发展', '', '0', 2, NULL, NULL, NULL, 1, NULL, NULL, 1, '0', 1, NULL, NULL, NULL);
