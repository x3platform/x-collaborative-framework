/****** Object:  Table [dbo].[Data_DigitalNumber]    Script Date: 08/15/2010 20:36:21 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Data_DigitalNumber]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[Data_DigitalNumber]
GO
/****** Object:  Table [dbo].[Data_DigitalNumber]    Script Date: 08/15/2010 20:36:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Data_DigitalNumber]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Data_DigitalNumber](
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Expression] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Seed] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_Data_DateRunningNumber] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)
)
END
GO

INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Key_Guid', N'{guid}', 1, CAST(0x00009DBD00013DBE AS DateTime), CAST(0x00009D4C015514B0 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Key_RunningNumber', N'{date:yyyyMMdd}{dailyIncrement:seed:10}', 0, CAST(0x00009D4C016FD61B AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Key_Timestamp', N'{date:yyyyMMddHHmmssfff}', 0, CAST(0x00009DCA01118738 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_Bugzilla_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:8}', 1, CAST(0x00009DAE0174B0E2 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_Event_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:8}', 1, CAST(0x00009D8E00ACD884 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_Meeting_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:8}', 12, CAST(0x00009D8F011A4359 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_Meeting_Room_Lock_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:8}', 1, CAST(0x00009D8F010A0476 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_News_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:8}', 1, CAST(0x00009DA600A95600 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_Wiki_Book_Key_Id', N'{tag:X_ISBN_}{int:seed}', 14, CAST(0x00009DBD00D81C09 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_WorkflowInstance_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:10}', 1, CAST(0x00009D8F00EF0CF6 AS DateTime), CAST(0x00009D4C015514B0 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'Table_WorkflowTemplate_Key_Id', N'{date:yyyyMMdd}{dailyIncrement:seed:10}', 1, CAST(0x00009D6E00D7F0CD AS DateTime), CAST(0x00009D4C015514B0 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'test1', N'{int:seed:8}', 5, CAST(0x00009D4C016FC974 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'test2', N'{date:yyyyMMddHHmmssfff}', 2, CAST(0x00009D4C016FD61B AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'test3', N'{date:yyyyMMddHH}{dailyIncrement:seed:8}', 1, CAST(0x00009D4C016FE8F9 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'test4', N'{date:yyyyMMdd}{tag:-}{int:seed}', 1, CAST(0x00009D4C016FF297 AS DateTime), CAST(0x00009D4C01580FC2 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'test5', N'{guid}', 1, CAST(0x00009D4C015514B8 AS DateTime), CAST(0x00009D4C015514B0 AS DateTime))
INSERT [dbo].[Data_DigitalNumber] ([Name], [Expression], [Seed], [UpdateDate], [CreateDate]) VALUES (N'test6', N'{random:abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ:10}', 1, CAST(0x00009D4C015514B8 AS DateTime), CAST(0x00009D4C015514B0 AS DateTime))
