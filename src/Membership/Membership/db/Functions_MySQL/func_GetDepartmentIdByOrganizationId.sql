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

	-- ���ҪѰ�ҵļ��� @Level ���ڲ�����󼶴� @DepartmentLevel, �򷵻�NULL.
	IF ( DepartmentLevel = (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SELECT Id, Type INTO OrganizationId, OrganizationType FROM tb_Organization WHERE Id = OrganizationId;
	ELSEIF( DepartmentLevel < (`Level` + 1)) THEN
		SET DepartmentLevel = BackwardCount;
		SET OrganizationId = NULL;
	END IF;

	-- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
	WHILE (OrganizationType != '0' && DepartmentLevel > (BackwardCount + `Level`)) DO 
		SET BackwardCount = BackwardCount + 1;

		SELECT Id, Type INTO OrganizationId, OrganizationType
		  FROM tb_Organization
		 WHERE Id = ( SELECT ParentId FROM tb_Organization WHERE Id = OrganizationId );

		-- ���������� = ��������� + �㼶, ���˳�ѭ��.
		IF ( DepartmentLevel = ( BackwardCount + `Level`) ) THEN
			-- BREAK;
			SET OrganizationType = 0;
		END IF;

		-- ���������֯��ʶ���ڹ�˾��ʶ, ���˳�ѭ��.
		-- ���������֯��ʶΪ��, ���˳�ѭ��.
		-- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF ( CorporationId = OrganizationId || OrganizationId = NULL || BackwardCount = 10 ) THEN
			SET OrganizationId = NULL;
		END IF;
	END WHILE;

	RETURN OrganizationId;
END;

