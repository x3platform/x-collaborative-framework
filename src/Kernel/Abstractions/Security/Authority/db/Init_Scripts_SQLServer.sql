SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tb_Authority](
	[Id] [nvarchar](36) NOT NULL,
	[Name] [nvarchar](400) NULL,
	[Description] [nvarchar](800) NULL,
	[Lock] [int] NULL,
	[Tags] [nvarchar](50) NULL,
	[OrderId] [nvarchar](20) NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_tb_Authority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tb_Authority] ADD  CONSTRAINT [DF_tb_Authority_OrderId]  DEFAULT ((0)) FOR [OrderId]
GO

