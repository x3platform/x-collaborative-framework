
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_FindOneByName]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_FindOneByName]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_Authority_FindOneByName]
(
	@Name nvarchar(400) 
)

AS

--===========================================================
-- Author: RuanYu
-- Summary: 
-- Date: 2007-11-14
--===========================================================

SET NOCOUNT ON
DECLARE @Err int

BEGIN

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
		[tb_Authority] T

	WHERE
		T.[Name] = @Name

END

SET @Err = @@Error
RETURN @Err