CREATE TABLE tb_Entity_Click(
	Id varchar(36) NOT NULL,
	EntityId varchar(36) NULL,
	EntityClassName varchar(400) NULL,
	AccountId varchar(36) NULL,
	Click int NULL,
	ModifiedDate datetime NULL,
	CreatedDate datetime NULL,
    CONSTRAINT PK_tb_Entity_Click PRIMARY KEY CLUSTERED (Id ASC)
);

ALTER TABLE tb_Entity_Click ADD PRIMARY KEY (EntityClassName, AccountId, EntityId);

CREATE TABLE tb_Entity_Draft(
	Id varchar(36) NOT NULL,
	EntityId varchar(36) NULL,
	EntityClassName varchar(400) NULL,
	OriginalEntityId varchar(36) NULL,
	Date datetime NULL,
 CONSTRAINT PK_Entity_Draft PRIMARY KEY CLUSTERED 
(
	Id ASC
));

CREATE TABLE tb_Entity_LifeHistory(
	Id varchar(36) NOT NULL,
	EntityId varchar(36) NULL,
	EntityClassName varchar(400) NULL,
	AccountId varchar(36) NULL,
	MethodName varchar(36) NULL,
	CotextDiffLog text NULL,
	Date datetime NULL,
 CONSTRAINT PK_tb_Entity_LifeHistory PRIMARY KEY CLUSTERED 
(
	Id ASC
));

CREATE TABLE tb_Entity_MetaData(
	Id varchar(36) NOT NULL,
	EntitySchemaId varchar(36) NULL,
	FieldName varchar(100) NULL,
	FieldType varchar(20) NULL,
	Description text NULL,
	DataColumnName varchar(100) NULL,
	EffectScope int NULL,
	Lockinging int NULL,
	OrderId varchar(20) NULL,
	Status int NULL,
	Remark varchar(200) NULL,
	ModifiedDate datetime NULL,
	CreatedDate datetime NULL,
 CONSTRAINT PK_tb_Entity_MetaData PRIMARY KEY CLUSTERED 
(
	Id ASC
));

CREATE TABLE tb_Entity_OperationLog(
	Id varchar(36) NOT NULL,
	EntityId varchar(36) NULL,
	EntityClassName varchar(400) NULL,
	AccountId varchar(36) NULL,
	OperationType int NULL,
	ToAuthorizationObjectType varchar(20) NULL,
	ToAuthorizationObjectId varchar(36) NULL,
	Reason varchar(800) NULL,
	CreatedDate datetime NULL,
    CONSTRAINT PK_tb_Entity_OperationLog PRIMARY KEY CLUSTERED ( Id ASC)
);

CREATE TABLE tb_Entity_Schema(
	Id varchar(36) NOT NULL,
	Code varchar(30) NULL,
	Name varchar(50) NULL,
	Description varchar(200) NULL,
	EntityClassName varchar(400) NULL,
	DataTableName varchar(100) NULL,
	DataTablePrimaryKey varchar(100) NULL,
	Tags varchar(200) NULL,
	Lockinging int NULL,
	OrderId varchar(20) NULL,
	Status int NULL,
	Remark varchar(200) NULL,
	ModifiedDate datetime NULL,
	CreatedDate datetime NULL,
	CONSTRAINT PK_tb_Entity_Schema PRIMARY KEY CLUSTERED (Id ASC)
);
CREATE TABLE tb_Entity_Snapshot(
	Id varchar(36) NOT NULL,
	EntityId varchar(36) NULL,
	EntityClassName varchar(400) NULL,
	SnapshotObject text NULL,
	Date datetime NULL,
	CONSTRAINT PK_tb_Entity_Snapshot PRIMARY KEY CLUSTERED (Id ASC)
);
