DROP FUNCTION IF EXISTS `func_GetDepartmentLevelByOrganizationId`;

CREATE FUNCTION `func_GetDepartmentLevelByOrganizationId`(OrganizationId VARCHAR(50))
 RETURNS varchar(50)
BEGIN
	DECLARE OrganizationType VARCHAR(10);
 
	DECLARE BackwardCount INT;

	SET BackwardCount = 1;

	SELECT Id, Type INTO OrganizationId, OrganizationType FROM tb_Organization WHERE Id = OrganizationId;

	-- 组织类型等于零, 则为公司类型, 循环结束.
	WHILE (OrganizationType != '0') DO  
		SET BackwardCount  = BackwardCount + 1;

		SELECT Id, Type INTO OrganizationId, OrganizationType
		  FROM tb_Organization
		 WHERE Id = ( SELECT ParentId FROM tb_Organization WHERE Id = OrganizationId );

		-- 如果父级组织标识为零, 则退出循环.
		-- 如果递归循环次数大于十次, 则退出循环(可能陷入死循环).
		IF (OrganizationId = NULL || BackwardCount = 10) THEN
			SET OrganizationType = 0;
		END IF;
	END WHILE;

	RETURN BackwardCount;
End;

