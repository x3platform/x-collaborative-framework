
-- ��������: `func_GetCorporationIdByOrganizationUnitId`
CREATE FUNCTION [dbo].[func_GetCorporationIdByOrganizationUnitId]
(
	@OrganizationUnitId  NVARCHAR(50)
)

RETURNS NVARCHAR(50)

WITH EXECUTE AS CALLER
AS

BEGIN

	DECLARE @OrganizationUnitType nvarchar(10)

	DECLARE @BackwardCount int

	SET @BackwardCount = 1

	SELECT @OrganizationUnitId=Id, @OrganizationUnitType=[Type] FROM tb_OrganizationUnit WHERE Id = @OrganizationUnitId

	-- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
	WHILE (@OrganizationUnitType != '0')
	BEGIN   
		SET @BackwardCount  = @BackwardCount + 1

		SELECT @OrganizationUnitId=Id, @OrganizationUnitType=[Type] 
		FROM tb_OrganizationUnit
		WHERE Id = ( SELECT ParentId FROM tb_OrganizationUnit WHERE Id = @OrganizationUnitId )

		-- ���������֯��ʶΪ��, ���˳�ѭ��.
		-- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF ( @OrganizationUnitId = NULL OR @BackwardCount=10)
		BEGIN
			BREAK;

		END

	END

	RETURN @OrganizationUnitId;
End
