CREATE TABLE [dbo].[tb_Logging] (

    [Id] [int] IDENTITY (1, 1) NOT NULL ,
    [Date] [datetime] NOT NULL ,
    [Thread] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
    [Level] [varchar] (200) COLLATE Chinese_PRC_CI_AS NOT NULL ,
    [Logger] [varchar] (200) COLLATE Chinese_PRC_CI_AS NOT NULL ,
    [Message] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NOT NULL ,
    [Exception] [varchar] (4000) COLLATE Chinese_PRC_CI_AS NULL

) ON [PRIMARY]

GO

 

ALTER TABLE [dbo].[Data_Logging] WITH NOCHECK ADD

    CONSTRAINT [PK_Data_Logging] PRIMARY KEY  CLUSTERED
    (
        [Id]

    )  ON [PRIMARY]

GO