-- ��������: func_GetDepartmentLevelByOrganizationId
CREATE FUNCTION [dbo].[func_GetDepartmentLevelByOrganizationId]
(
	@OrganizationId  nvarchar(50)
)

RETURNS varchar(50)

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
		WHERE Id = (
			SELECT ParentId FROM tb_Organization WHERE Id = @OrganizationId )

		-- ���������֯��ʶΪ��, ���˳�ѭ��.
		-- ����ݹ�ѭ����������ʮ��, ���˳�ѭ��(����������ѭ��).
		IF (@OrganizationId=null OR @BackwardCount=10)
		BEGIN
			BREAK;
		END
	END

	RETURN @BackwardCount;
End