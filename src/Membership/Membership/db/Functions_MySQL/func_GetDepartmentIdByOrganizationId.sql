DROP FUNCTION IF EXISTS `func_GetDepartmentIdByOrganizationId`;

CREATE FUNCTION `func_GetDepartmentIdByOrganizationId`(OrganizationId  VARCHAR(50),`Level` INT)
 RETURNS varchar(50) CHARSET utf8
BEGIN
 
	DECLARE CorporationId varchar(36);

	DECLARE DepartmentLevel INT;

	DECLARE OrganizationType varchar(20);

	DECLARE BackwardCount INT;

	SET CorporationId = func_GetCorporationIdByOrganizationId(OrganizationId);

	SET DepartmentLevel = func_GetDepartmentLevelByOrganizationId(OrganizationId);
	-- print ('@OrganizationId:' + @DepartmentLevel)

	SET BackwardCount = 1;

	SET OrganizationType = '1';

    -- SELECT @OrganizationId=Id, @OrganizationType=[Type] FROM tb_Organization WHERE Id = @OrganizationId

	-- print '@OrganizationId:' + @OrganizationId

	-- 如果要寻找的级次 @Level 大于部门最大级次 @DepartmentLevel, 则返回NULL.
	IF ( DepartmentLevel = (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SELECT Id, Type INTO OrganizationId, OrganizationType FROM tb_Organization WHERE Id = OrganizationId;
	ELSEIF( DepartmentLevel < (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SET OrganizationId = NULL;
	END IF;

	-- 组织类型等于零, 则为公司类型, 循环结束.
	WHILE (OrganizationType != '0' && DepartmentLevel > (BackwardCount + `Level`)) DO 
		SET BackwardCount = BackwardCount + 1;

		SELECT Id, Type INTO OrganizationId, OrganizationType
		  FROM tb_Organization
		 WHERE Id = ( SELECT ParentId FROM tb_Organization WHERE Id = OrganizationId );

		-- 如果部门深度 = 反向计数器 + 层级, 则退出循环.
		IF ( DepartmentLevel = ( BackwardCount + `Level`) ) THEN
			-- BREAK;
			SET OrganizationType = 0;
		END IF;

		-- 如果父级组织标识等于公司标识, 则退出循环.
		-- 如果父级组织标识为零, 则退出循环.
		-- 如果递归循环次数大于十次, 则退出循环(可能陷入死循环).
		IF ( CorporationId = OrganizationId || OrganizationId = NULL || BackwardCount = 10 ) THEN
			SET OrganizationId = NULL;
		END IF;
	END WHILE;

	RETURN OrganizationId;
END;

