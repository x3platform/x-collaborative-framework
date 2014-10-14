
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_GetPages]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_GetPages]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_Authority_GetPages]
(
	@StartIndex int,
	@PageSize int,
	@WhereClause nvarchar(200),
	@OrderBy nvarchar(200),
	@RowCount int output
)

AS

--===========================================================
-- Author	: RuanYu
-- Summary	: 
-- Date		: 2007-11-14
--===========================================================

SET NOCOUNT ON
DECLARE @Err int

BEGIN

	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	
	-- First set the rowcount

	-- Set the page bounds

	SET @PageLowerBound = @StartIndex
	SET @PageUpperBound = @StartIndex + @PageSize

	-- Create a temp table to store the select results
	CREATE TABLE #PageIndex
	(
		[IndexId] int IDENTITY (1, 1) NOT NULL,
		[ID] nvarchar(36)
	)

	-- Insert into the temp table
	DECLARE @SQL AS nvarchar(4000)
	SET @SQL = 'INSERT INTO #PageIndex ([Id])'
	SET @SQL = @SQL + ' SELECT [Id]'
	SET @SQL = @SQL + ' FROM [tb_Authority] T '
	IF LEN(@WhereClause) > 0
	BEGIN
		SET @SQL = @SQL + ' WHERE ' + @WhereClause
	END

	IF LEN(@OrderBy) > 0
	BEGIN
		SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
	END

	-- Order by the first column.
	ELSE
	BEGIN
		SET @SQL = @SQL + ' ORDER BY [OrderId] DESC'
	END

	-- Populate the temp table
	EXEC sp_executesql @SQL

	-- Return total count
	SET @ROWCOUNT = @@ROWCOUNT

	-- Set RowCount After Total Rows is determined
	-- Return paged results

	SELECT 
		T.[Id],
		T.[Name],
		T.[Description],
		T.[Lock],
		T.[Tags],
		T.[OrderId],
		T.[UpdateDate],
		T.[CreateDate]

	FROM 
		tb_Authority T,
		#PageIndex PageIndex 

	WHERE 
		T.Id = PageIndex.Id 
		AND PageIndex.IndexId > @PageLowerBound 
		AND PageIndex.IndexID <= @PageUpperBound 

	ORDER BY 
		PageIndex.IndexId 

END

SET @Err = @@Error
RETURN @Err