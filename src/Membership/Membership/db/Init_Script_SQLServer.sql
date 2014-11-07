
-- 创建函数: `func_GetCorporationIdByOrganizationId`
CREATE FUNCTION [dbo].[func_GetCorporationIdByOrganizationId]
(
	@OrganizationId  NVARCHAR(50)
)

RETURNS NVARCHAR(50)

WITH EXECUTE AS CALLER
AS

BEGIN

	DECLARE @OrganizationType nvarchar(10)

	DECLARE @BackwardCount int

	SET @BackwardCount = 1

	SELECT @OrganizationId=Id, @OrganizationType=[Type] FROM tb_Organization WHERE Id = @OrganizationId

	-- 组织类型等于零, 则为公司类型, 循环结束.
	WHILE (@OrganizationType != '0')
	BEGIN   
		SET @BackwardCount  = @BackwardCount + 1

		SELECT @OrganizationId=Id, @OrganizationType=[Type] 
		FROM tb_Organization
		WHERE Id = ( SELECT ParentId FROM tb_Organization WHERE Id = @OrganizationId )

		-- 如果父级组织标识为零, 则退出循环.
		-- 如果递归循环次数大于十次, 则退出循环(可能陷入死循环).
		IF ( @OrganizationId = NULL OR @BackwardCount=10)
		BEGIN
			BREAK;

		END

	END

	RETURN @OrganizationId;
End
