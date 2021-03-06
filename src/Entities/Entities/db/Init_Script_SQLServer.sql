USE [Sewells_Ford]
GO
/****** Object:  Table [dbo].[tb_Entity_Click]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_Click](
	[Id] [nvarchar](36) NOT NULL,
	[EntityId] [nvarchar](36) NULL,
	[EntityClassName] [nvarchar](400) NULL,
	[AccountId] [nvarchar](36) NULL,
	[Click] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_tb_Entity_VisitorLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_Entity_Draft]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_Draft](
	[Id] [nvarchar](36) NOT NULL,
	[EntityId] [nvarchar](36) NULL,
	[EntityClassName] [nvarchar](400) NULL,
	[OriginalEntityId] [nvarchar](36) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Entity_Draft] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_Entity_LifeHistory]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_LifeHistory](
	[Id] [nvarchar](36) NOT NULL,
	[EntityId] [nvarchar](36) NULL,
	[EntityClassName] [nvarchar](400) NULL,
	[AccountId] [nvarchar](36) NULL,
	[MethodName] [nvarchar](36) NULL,
	[ContextDiffLog] [ntext] NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_tb_Entity_LifeHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_Entity_MetaData]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_MetaData](
	[Id] [nvarchar](36) NOT NULL,
	[EntitySchemaId] [nvarchar](36) NULL,
	[FieldName] [nvarchar](100) NULL,
	[FieldType] [nvarchar](20) NULL,
	[Description] [ntext] NULL,
	[DataColumnName] [nvarchar](100) NULL,
	[EffectScope] [int] NULL,
	[Locking] [int] NULL,
	[OrderId] [nvarchar](20) NULL,
	[Status] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_tb_Entity_MetaData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_Entity_OperationLog]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_OperationLog](
	[Id] [nvarchar](36) NOT NULL,
	[EntityId] [nvarchar](36) NULL,
	[EntityClassName] [nvarchar](400) NULL,
	[AccountId] [nvarchar](36) NULL,
	[OperationType] [int] NULL,
	[ToAuthorizationObjectType] [nvarchar](20) NULL,
	[ToAuthorizationObjectId] [nvarchar](36) NULL,
	[Reason] [nvarchar](800) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_tb_Entity_OperationLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_Entity_Schema]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_Schema](
	[Id] [nvarchar](36) NOT NULL,
	[Code] [nvarchar](30) NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](200) NULL,
	[EntityClassName] [nvarchar](400) NULL,
	[DataTableName] [nvarchar](100) NULL,
	[DataTablePrimaryKey] [nvarchar](100) NULL,
	[Tags] [nvarchar](200) NULL,
	[Locking] [int] NULL,
	[OrderId] [nvarchar](20) NULL,
	[Status] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_tb_Entity_Schema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_Entity_Snapshot]    Script Date: 2015/4/11 22:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Entity_Snapshot](
	[Id] [nvarchar](36) NOT NULL,
	[EntityId] [nvarchar](36) NULL,
	[EntityClassName] [nvarchar](400) NULL,
	[SnapshotObject] [ntext] NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_tb_Entity_Snapshot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
