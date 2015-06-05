CREATE TABLE tb_Customize_Page
(
	Id varchar(36) NOT NULL,
	AuthorizationObjectType varchar(20) NULL,
	AuthorizationObjectId varchar(36) NULL,
	AuthorizationObjectName varchar(50) NULL,
	Name varchar(50) NULL,
	Title varchar(100) NULL,
	Html text NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Customize_Page ADD CONSTRAINT PK_tb_Customize_Page PRIMARY KEY CLUSTERED (Id);

CREATE TABLE tb_Customize_Widget
(
	Id varchar(36) NOT NULL,
	Name varchar(50) NULL,
	Title varchar(50) NULL,
	Height int NULL,
	Width int NULL,
	Url varchar(400) NULL,
	Description varchar(400) NULL,
	Options text NULL,
	OptionHtml text NULL,
	Tags varchar(50) NULL,
	ClassName varchar(400) NULL,
	RedirctUrl varchar(400) NULL,
	OrderId varchar(20) NULL,
	Status int NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Customize_Widget ADD CONSTRAINT PK_tb_Customize_Widget PRIMARY KEY CLUSTERED (Id);

CREATE TABLE tb_Customize_WidgetInstance
(
	Id varchar(36) NOT NULL,
	PageId varchar(36) NULL,
	PageName varchar(50) NULL,
	WidgetId varchar(36) NULL,
	WidgetName varchar(50) NULL,
	Title varchar(50) NULL,
	Height int NULL,
	Width int NULL,
	Options text NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
)

ALTER TABLE tb_Customize_WidgetInstance ADD CONSTRAINT PK_tb_Customize_WidgetInstance PRIMARY KEY CLUSTERED (Id);

CREATE TABLE tb_Customize_Layout
(
	Id varchar(36) NOT NULL,
	Name varchar(50) NULL,
	Description varchar(400) NULL,
	Html text NULL,
	UpdateDate datetime NULL,
	CreateDate datetime NULL
);

-- 设置主键: Id
ALTER TABLE tb_Customize_Layout ADD CONSTRAINT PK_Customize_Layout PRIMARY KEY CLUSTERED (Id);
