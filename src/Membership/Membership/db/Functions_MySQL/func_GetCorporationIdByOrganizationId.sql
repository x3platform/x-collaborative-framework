DROP FUNCTION IF EXISTS `func_GetCorporationIdByOrganizationUnitId`;

CREATE FUNCTION `func_GetCorporationIdByOrganizationUnitId`(OrganizationUnitId  VARCHAR(50))
 RETURNS varchar(50) CHARSET utf8
BEGIN
 
    DECLARE OrganizationUnitType VARCHAR(10);

    DECLARE BackwardCount INT;

    SET BackwardCount = 1;

    SET BackwardCount = 1;

    SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId;

    -- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
    WHILE (OrganizationUnitType != '0') DO
        SET BackwardCount = BackwardCount + 1;

        SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType
        FROM tb_OrganizationUnit
        WHERE Id = ( SELECT ParentId FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId );

        -- ���������֯��ʶΪ��, ���˳�ѭ��.
        -- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF ( OrganizationUnitId = NULL || BackwardCount = 10) THEN
        -- BREAK;
       SET OrganizationUnitType = 0;
    END IF;

    END WHILE;

    RETURN OrganizationUnitId;
End;