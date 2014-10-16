
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Authority_Update]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_Authority_Update]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_Authority_Update]
(
	@Id nvarchar(36),
	@Name nvarchar(200),
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

	UPDATE [tb_Authority] SET
		[Name] = @Name,
		[Description] = @Description,
		[Lock] = @Lock,
		[Tags] = @Tags,
		[OrderId] = @OrderId,
		[UpdateDate] = CURRENT_TIMESTAMP
		
	WHERE
		[Id] = @Id

END

SET @Err = @@Error
RETURN @Err