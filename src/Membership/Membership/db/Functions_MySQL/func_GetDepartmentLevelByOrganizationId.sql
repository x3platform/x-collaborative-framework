DROP FUNCTION IF EXISTS `func_GetDepartmentLevelByOrganizationId`;

CREATE FUNCTION `func_GetDepartmentLevelByOrganizationId`(OrganizationId VARCHAR(50))
 RETURNS varchar(50)
BEGIN
	DECLARE OrganizationType VARCHAR(10);
 
	DECLARE BackwardCount INT;

	SET BackwardCount = 1;

	SELECT Id, Type INTO OrganizationId, OrganizationType FROM tb_Organization WHERE Id = OrganizationId;

	-- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
	WHILE (OrganizationType != '0') DO  
		SET BackwardCount  = BackwardCount + 1;

		SELECT Id, Type INTO OrganizationId, OrganizationType
		  FROM tb_Organization
		 WHERE Id = ( SELECT ParentId FROM tb_Organization WHERE Id = OrganizationId );

		-- ���������֯��ʶΪ��, ���˳�ѭ��.
		-- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF (OrganizationId = NULL || BackwardCount = 10) THEN
			SET OrganizationType = 0;
		END IF;
	END WHILE;

	RETURN BackwardCount;
End;

