DROP FUNCTION IF EXISTS `func_GetDepartmentIdByOrganizationUnitId`;

CREATE FUNCTION `func_GetDepartmentIdByOrganizationUnitId`(OrganizationUnitId  VARCHAR(50),`Level` INT)
 RETURNS varchar(50) CHARSET utf8
BEGIN
 
	DECLARE CorporationId varchar(36);

	DECLARE DepartmentLevel INT;

	DECLARE OrganizationUnitType varchar(20);

	DECLARE BackwardCount INT;

	SET CorporationId = func_GetCorporationIdByOrganizationUnitId(OrganizationUnitId);

	SET DepartmentLevel = func_GetDepartmentLevelByOrganizationUnitId(OrganizationUnitId);
	-- print ('@OrganizationUnitId:' + @DepartmentLevel)

	SET BackwardCount = 1;

	SET OrganizationUnitType = '1';

    -- SELECT @OrganizationUnitId=Id, @OrganizationUnitType=[Type] FROM tb_OrganizationUnit WHERE Id = @OrganizationUnitId

	-- print '@OrganizationUnitId:' + @OrganizationUnitId

	-- 如果要寻找的级次 @Level 大于部门最大级次 @DepartmentLevel, 则返回NULL.
	IF ( DepartmentLevel = (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId;
	ELSEIF( DepartmentLevel < (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SET OrganizationUnitId = NULL;
	END IF;

	-- 组织类型等于零, 则为公司类型, 循环结束.
	WHILE (OrganizationUnitType != '0' && DepartmentLevel > (BackwardCount + `Level`)) DO 
		SET BackwardCount = BackwardCount + 1;

		SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType
		  FROM tb_OrganizationUnit
		 WHERE Id = ( SELECT ParentId FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId );

		-- 如果部门深度 = 反向计数器 + 层级, 则退出循环.
		IF ( DepartmentLevel = ( BackwardCount + `Level`) ) THEN
			-- BREAK;
			SET OrganizationUnitType = 0;
		END IF;

		-- 如果父级组织标识等于公司标识, 则退出循环.
		-- 如果父级组织标识为零, 则退出循环.
		-- 如果递归循环次数大于十次, 则退出循环(可能陷入死循环).
		IF ( CorporationId = OrganizationUnitId || OrganizationUnitId = NULL || BackwardCount = 10 ) THEN
			SET OrganizationUnitId = NULL;
		END IF;
	END WHILE;

	RETURN OrganizationUnitId;
END;

