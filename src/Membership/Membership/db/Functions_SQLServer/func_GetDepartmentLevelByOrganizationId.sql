-- 创建函数: func_GetDepartmentLevelByOrganizationUnitId
CREATE FUNCTION [dbo].[func_GetDepartmentLevelByOrganizationUnitId]
(
	@OrganizationUnitId  nvarchar(50)
)

RETURNS varchar(50)

WITH EXECUTE AS CALLER
AS

BEGIN

	DECLARE @OrganizationUnitType nvarchar(10)

	DECLARE @BackwardCount int

	SET @BackwardCount = 1

	SELECT @OrganizationUnitId=Id, @OrganizationUnitType=[Type] FROM tb_OrganizationUnit WHERE Id = @OrganizationUnitId

	-- 组织类型等于零, 则为公司类型, 循环结束.
	WHILE (@OrganizationUnitType != '0')
	BEGIN   
		SET @BackwardCount  = @BackwardCount + 1

		SELECT @OrganizationUnitId=Id, @OrganizationUnitType=[Type] 
		FROM tb_OrganizationUnit
		WHERE Id = (
			SELECT ParentId FROM tb_OrganizationUnit WHERE Id = @OrganizationUnitId )

		-- 如果父级组织标识为零, 则退出循环.
		-- 如果递归循环次数大于十次, 则退出循环(可能陷入死循环).
		IF (@OrganizationUnitId=null OR @BackwardCount=10)
		BEGIN
			BREAK;
		END
	END

	RETURN @BackwardCount;
End