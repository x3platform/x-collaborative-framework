
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_Insert]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_Insert]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[proc_Authority_Insert]
(
	@Id nvarchar(36),
	@Name nvarchar(400),
	@Description nvarchar(800),
	@Lock int,
	@Tags nvarchar(50),
	@OrderId nvarchar(50)
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

	INSERT INTO [tb_Authority] 
	(
		[Id],
		[Name],
		[Description],
		[Lock],
		[Tags],
		[OrderId],
		[UpdateDate],
		[CreateDate]
	)
	VALUES
	(
		@Id,
		@Name,
		@Description,
		@Lock,
		@Tags,
		@OrderId,
		GETDATE(),
		GETDATE()	
	)
	SET @Id = @@IDENTITY
END

SET @Err = @@Error
RETURN @Err