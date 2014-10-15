
-- ��������: `func_GetCorporationIdByOrganizationId`
CREATE FUNCTION [dbo].[func_GetCorporationIdByOrganizationId]
(
	@OrganizationId  NVARCHAR(50)
)

RETURNS NVARCHAR(50)

WITH EXECUTE AS CALLER
AS

BEGIN

	DECLARE @OrganizationType nvarchar(10)

	DECLARE @BackwardCount int

	SET @BackwardCount = 1

	SELECT @OrganizationId=Id, @OrganizationType=[Type] FROM tb_Organization WHERE Id = @OrganizationId

	-- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
	WHILE (@OrganizationType != '0')
	BEGIN   
		SET @BackwardCount  = @BackwardCount + 1

		SELECT @OrganizationId=Id, @OrganizationType=[Type] 
		FROM tb_Organization
		WHERE Id = ( SELECT ParentId FROM tb_Organization WHERE Id = @OrganizationId )

		-- ���������֯��ʶΪ��, ���˳�ѭ��.
		-- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF ( @OrganizationId = NULL OR @BackwardCount=10)
		BEGIN
			BREAK;

		END

	END

	RETURN @OrganizationId;
End
