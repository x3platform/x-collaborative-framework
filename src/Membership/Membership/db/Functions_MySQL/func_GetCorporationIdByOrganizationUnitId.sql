DROP FUNCTION IF EXISTS `func_GetCorporationIdByOrganizationUnitId`;

CREATE FUNCTION `func_GetCorporationIdByOrganizationUnitId`(OrganizationUnitId  VARCHAR(50))
 RETURNS varchar(50) CHARSET utf8
BEGIN
 
    DECLARE OrganizationUnitType VARCHAR(10);

    DECLARE BackwardCount INT;

    SET BackwardCount = 1;

    SET BackwardCount = 1;

    SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId;

    -- 组织类型等于零, 则为公司类型, 循环结束.
    WHILE (OrganizationUnitType != '0') DO
        SET BackwardCount = BackwardCount + 1;

        SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType
        FROM tb_OrganizationUnit
        WHERE Id = ( SELECT ParentId FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId );

        -- 如果父级组织标识为零, 则退出循环.
        -- 如果递归循环次数大于十次, 则退出循环(可能陷入死循环).
		IF ( OrganizationUnitId = NULL || BackwardCount = 10) THEN
        -- BREAK;
       SET OrganizationUnitType = 0;
    END IF;

    END WHILE;

    RETURN OrganizationUnitId;
End;