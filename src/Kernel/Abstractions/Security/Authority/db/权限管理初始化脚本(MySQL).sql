
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_Delete]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_Delete]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_Authority_Delete]
(
	@Ids nvarchar(800)
)

AS

--===========================================================
-- Author	: RuanYu
-- Summary	: 
-- Date		: 2009-06-20
--===========================================================

SET NOCOUNT ON
DECLARE @Err int

BEGIN
	
	DECLARE @SQL AS nvarchar(4000)

	SET @SQL = 'DELETE FROM [tb_Authority] WHERE [Id] IN (' + @Ids + ') '

	EXEC sp_executesql @SQL
	
END