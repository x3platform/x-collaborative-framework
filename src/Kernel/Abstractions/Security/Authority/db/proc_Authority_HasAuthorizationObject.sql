
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_HasAuthorizationObject]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_HasAuthorizationObject]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_Authority_HasAuthorizationObject]
(
	@WhereClause nvarchar(800),
	@Count int output
)

AS

--===========================================================
-- Author	: RuanYu
-- Summary	: 
-- Date		: 2010-01-01
--===========================================================

SET NOCOUNT ON
DECLARE @Err int

BEGIN
	
	DECLARE @SQL AS nvarchar(4000)
	
	SET @SQL = ' SELECT @Count = COUNT(*) FROM [view_AuthorizationObject_Account] '
	
	IF LEN(@WhereClause) > 0
	BEGIN
		SET @SQL = @SQL + ' WHERE ' + @WhereClause
	END
	
	ELSE
	BEGIN
		SET @SQL = @SQL + ' WHERE 1<>1 '
	END
	
	EXEC sp_executesql @SQL,N'@Count int output',@Count output

END

SET @Err = @@Error
RETURN @Err