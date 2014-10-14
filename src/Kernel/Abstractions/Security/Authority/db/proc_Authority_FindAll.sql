
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_FindAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_FindAll]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_Authority_FindAll]
(
	@WhereClause nvarchar(800),
	@Length int
)

AS

--===========================================================
-- Author: RuanYu
-- Summary: 在 [Configuration_Authority] 中查询相关记录.
-- Date: 2007-11-14
--===========================================================

SET NOCOUNT ON
DECLARE @Err int

BEGIN

	DECLARE @SQL as nvarchar(4000)

	SET @SQL = 'SELECT '

	IF @Length > 0 
	BEGIN
		SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar(15),@Length) + ' '
	END

	SET @SQL = @SQL + '
		T.[Id],
		T.[Name],
		T.[Description],
		T.[Lock],
		T.[Tags],
		T.[OrderId],
		T.[UpdateDate],
		T.[CreateDate]

	FROM tb_Authority T '

	IF Len(@WhereClause) > 0 
	BEGIN
		SET @SQL = @SQL + ' WHERE ' + @WhereClause 
	END
	EXEC sp_executesql @SQL
END

SET @Err = @@Error
RETURN @Err