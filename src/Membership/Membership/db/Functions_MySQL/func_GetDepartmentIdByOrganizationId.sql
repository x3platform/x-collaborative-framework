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

	-- ���ҪѰ�ҵļ��� @Level ���ڲ�����󼶴� @DepartmentLevel, �򷵻�NULL.
	IF ( DepartmentLevel = (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId;
	ELSEIF( DepartmentLevel < (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SET OrganizationUnitId = NULL;
	END IF;

	-- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
	WHILE (OrganizationUnitType != '0' && DepartmentLevel > (BackwardCount + `Level`)) DO 
		SET BackwardCount = BackwardCount + 1;

		SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType
		  FROM tb_OrganizationUnit
		 WHERE Id = ( SELECT ParentId FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId );

		-- ���������� = ��������� + �㼶, ���˳�ѭ��.
		IF ( DepartmentLevel = ( BackwardCount + `Level`) ) THEN
			-- BREAK;
			SET OrganizationUnitType = 0;
		END IF;

		-- ���������֯��ʶ���ڹ�˾��ʶ, ���˳�ѭ��.
		-- ���������֯��ʶΪ��, ���˳�ѭ��.
		-- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF ( CorporationId = OrganizationUnitId || OrganizationUnitId = NULL || BackwardCount = 10 ) THEN
			SET OrganizationUnitId = NULL;
		END IF;
	END WHILE;

	RETURN OrganizationUnitId;
END;

