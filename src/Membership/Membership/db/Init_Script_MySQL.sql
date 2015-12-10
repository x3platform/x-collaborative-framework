-- ================================================
-- ��Ա��Ȩ�޹����ʼ���ű� MySQL �� (2010-01-01)
-- ================================================

-- �������ݿ�: `sys_hris`
-- CREATE DATABASE `sys_hris`

-- ������: `tb_Account`
CREATE TABLE IF NOT EXISTS `tb_Account`(
	`Id` varchar(36) NOT NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`GlobalName` varchar(100) NULL,
	`DisplayName` varchar(50) NULL,
	`PinYin` varchar(100) NULL,
	`LoginName` varchar(50) NULL,
	`Password` varchar(50) NULL,
	`IdentityCard` varchar(30) NULL,
	`Type` int NULL,
	`CertifiedMobile` varchar(50) NULL,
	`CertifiedEmail` varchar(50) NULL,
	`CertifiedAvatar` varchar(100) NULL,
	`EnableExchangeEmail` int NULL,
	`IsDraft` bit NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`IP` varchar(20) NULL,
	`LoginDate` datetime NULL,
	`DistinguishedName` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Account` ADD CONSTRAINT `PK_tb_Account` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Account_AssignedJob`
CREATE TABLE IF NOT EXISTS `tb_Account_AssignedJob`(
	`AccountId` varchar(36) NOT NULL,
	`AssignedJobId` varchar(36) NOT NULL,
	`IsDefault` bit NULL,
	`BeginDate` datetime NULL,
	`EndDate` datetime NULL
);

-- ��������: `AccountId`, `AssignedJobId`
ALTER TABLE `tb_Account_AssignedJob` ADD CONSTRAINT `PK_tb_Account_AssignedJob` PRIMARY KEY CLUSTERED (`AccountId`, `AssignedJobId`);

-- ������: `tb_Account_Grant`
CREATE TABLE IF NOT EXISTS `tb_Account_Grant`(
	`Id` varchar(36) NOT NULL,
	`Code` varchar(30) NULL,
	`GrantorId` varchar(36) NULL,
	`GranteeId` varchar(36) NULL,
	`GrantedTimeFrom` datetime NULL,
	`GrantedTimeTo` datetime NULL,
	`WorkflowGrantMode` int NULL,
	`DataQueryGrantMode` int NULL,
	`IsAborted` bit NULL,
	`ApprovedUrl` varchar(800) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Account_Grant` ADD CONSTRAINT `PK_tb_Account_Grant` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Account_Group`
CREATE TABLE IF NOT EXISTS `tb_Account_Group`(
	`AccountId` varchar(36) NOT NULL,
	`GroupId` varchar(36) NOT NULL,
	`IsDefault` bit NULL,
	`BeginDate` datetime NULL,
	`EndDate` datetime NULL
);

-- ��������: `AccountId`, `GroupId`
ALTER TABLE `tb_Account_Group` ADD CONSTRAINT `PK_tb_Account_Group` PRIMARY KEY CLUSTERED (`AccountId`, `GroupId`);

-- ������: `tb_Account_Log`
CREATE TABLE IF NOT EXISTS `tb_Account_Log`(
	`Id` varchar(36) NOT NULL,
	`AccountId` varchar(36) NULL,
	`OriginalObjectValue` text NULL,
	`OperatedBy` varchar(36) NULL,
	`OperationName` varchar(50) NULL,
	`Description` text NULL,
	`Date` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Account_Log` ADD CONSTRAINT `PK_tb_Account_Log` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Account_OrganizationUnit`
CREATE TABLE IF NOT EXISTS `tb_Account_OrganizationUnit`(
	`AccountId` varchar(36) NOT NULL,
	`OrganizationUnitId` varchar(36) NOT NULL,
	`IsDefault` bit NULL,
	`BeginDate` datetime NULL,
	`EndDate` datetime NULL
);

-- ��������: `AccountId`, `OrganizationUnitId`
ALTER TABLE `tb_Account_OrganizationUnit` ADD CONSTRAINT `PK_tb_Account_OrganizationUnit` PRIMARY KEY CLUSTERED (`AccountId`, `OrganizationUnitId`);

-- ������: `tb_Account_Role`
CREATE TABLE IF NOT EXISTS `tb_Account_Profile`(
    `Id` varchar(36) NOT NULL,
	`AccountId` varchar(36) NULL,
	`Options` text NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Account_Profile` ADD CONSTRAINT `PK_tb_Account_Profile` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Account_Role`
CREATE TABLE IF NOT EXISTS `tb_Account_Role`(
	`AccountId` varchar(36) NOT NULL,
	`RoleId` varchar(36) NOT NULL,
	`IsDefault` bit NULL,
	`BeginDate` datetime NULL,
	`EndDate` datetime NULL
);

-- ��������: `AccountId`, `RoleId`
ALTER TABLE `tb_Account_Role` ADD CONSTRAINT `PK_tb_Account_Role` PRIMARY KEY CLUSTERED (`AccountId`, `RoleId`);

CREATE TABLE IF NOT EXISTS `tb_AssignedJob`(
	`Id` varchar(36) NOT NULL,
	`JobId` varchar(36) NULL,
	`OrganizationUnitId` varchar(36) NULL,
	`ParentId` varchar(36) NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`Description` text NULL,
	`RoleId` varchar(36) NULL,
	`Locking` int NULL,
	`Status` int NULL,
	`OrderId` varchar(20) NULL,
	`Remark` varchar(200) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_AssignedJob` ADD CONSTRAINT `PK_tb_AssignedJob` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_GeneralRole`
CREATE TABLE IF NOT EXISTS `tb_GeneralRole`(
	`Id` varchar(36) NOT NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`GlobalName` varchar(100) NULL,
	`PinYin` varchar(50) NULL,
	`GroupTreeNodeId` varchar(36) NULL,
	`EnableExchangeEmail` int NULL,
	`Locking` int NULL,
	`OrderId` varchar(50) NULL,
	`Status` int NULL,
	`Remark` varchar(100) NULL,
	`FullPath` varchar(400) NULL,
	`DistinguishedName` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_GeneralRole` ADD CONSTRAINT `PK_tb_GeneralRole` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Group`
CREATE TABLE IF NOT EXISTS `tb_Group`(
	`Id` varchar(36) NOT NULL,
	`ParentId` varchar(36) NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`GlobalName` varchar(100) NULL,
	`PinYin` varchar(50) NULL,
	`Type` varchar(50) NULL,
	`GroupTreeNodeId` varchar(36) NULL,
	`EnableExchangeEmail` int NULL,
	`EffectScope` int NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`FullPath` varchar(400) NULL,
	`DistinguishedName` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Group` ADD CONSTRAINT `PK_tb_Group` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_GroupTree`
CREATE TABLE IF NOT EXISTS `tb_GroupTree`(
	`Id` varchar(36) NOT NULL,
	`Name` varchar(50) NULL,
	`DisplayType` varchar(30) NULL,
	`RootTreeNodeId` varchar(36) NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_GroupTree` ADD CONSTRAINT `PK_tb_GroupTree` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_GroupTreeNode`
CREATE TABLE IF NOT EXISTS `tb_GroupTreeNode`(
	`Id` varchar(36) NOT NULL,
	`ParentId` varchar(36) NULL,
	`Name` varchar(50) NULL,
	`IsKey` bit NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`DistinguishedName` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_GroupTreeNode` ADD CONSTRAINT `PK_tb_GroupTreeNode` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Job`
CREATE TABLE IF NOT EXISTS `tb_Job`(
	`Id` varchar(36) NOT NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`Status` int NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Job` ADD CONSTRAINT `PK_tb_Job` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Job_StandardRole`
CREATE TABLE IF NOT EXISTS `tb_Job_StandardRole`(
	`JobId` varchar(36) NOT NULL,
	`StandardRoleId` varchar(36) NOT NULL,
	`IsDefault` bit NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Job_StandardRole` ADD CONSTRAINT `PK_tb_Job_StandardRole` PRIMARY KEY CLUSTERED (`JobId`, `StandardRoleId`);

-- ������: `tb_JobFamily`
CREATE TABLE IF NOT EXISTS `tb_JobFamily`(
	`Id` varchar(36) NOT NULL,
	`Name` varchar(50) NULL,
	`Status` int NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_JobFamily` ADD CONSTRAINT `PK_tb_JobFamily` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_JobGrade`
CREATE TABLE IF NOT EXISTS `tb_JobGrade`(
	`Id` varchar(36) NOT NULL,
	`JobFamilyId` varchar(36) NULL,
	`Name` varchar(50) NULL,
	`Value` int NULL,
	`Status` int NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_JobGrade` ADD CONSTRAINT `PK_tb_JobGrade` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Member`
CREATE TABLE IF NOT EXISTS `tb_Member`(
	`Id` varchar(36) NOT NULL,
	`AccountId` varchar(36) NULL,
	`CorporationId` varchar(36) NULL,
	`DepartmentId` varchar(36) NULL,
	`Department2Id` varchar(36) NULL,
	`Department3Id` varchar(36) NULL,
	`OrganizationUnitId` varchar(36) NULL,
	`RoleId` varchar(36) NULL,
	`JobId` varchar(36) NULL,
	`AssignedJobId` varchar(36) NULL,
	`JobGradeId` varchar(36) NULL,
	`JobGradeDisplayName` varchar(20) NULL,
	`Headship` varchar(50) NULL,
	`Sex` varchar(4) NULL,
	`Birthday` datetime NULL,
	`GraduationDate` datetime NULL,
	`EntryDate` datetime NULL,
	`PromotionDate` datetime NULL,
	`Hometown` varchar(20) NULL,
	`City` varchar(20) NULL,
	`Mobile` varchar(50) NULL,
	`Telephone` varchar(50) NULL,
	`QQ` varchar(30) NULL,
	`MSN` varchar(50) NULL,
	`Email` varchar(50) NULL,
	`Rtx` varchar(30) NULL,
	`PostCode` varchar(50) NULL,
	`Address` varchar(100) NULL,
	`Url` varchar(500) NULL,
	`FullPath` varchar(400) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Member` ADD CONSTRAINT `PK_tb_Member` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Member_ExtensionInformation`
CREATE TABLE IF NOT EXISTS `tb_Member_ExtensionInformation`(
	`Id` varchar(36) NULL,
	`AccountId` varchar(36) NOT NULL,
	`Country` varchar(30) NULL,
	`National` varchar(30) NULL,
	`Passport` varchar(100) NULL,
	`MaritalStatus` varchar(30) NULL,
	`Professional` varchar(200) NULL,
	`Hobby` varchar(800) NULL,
	`Profile` varchar(800) NULL,
	`HighestEducation` varchar(50) NULL,
	`HighestDegree` varchar(50) NULL,
	`ForeignLanguage` varchar(50) NULL,
	`ForeignLanguageLevel` varchar(50) NULL,
	`GraduationSchool` varchar(50) NULL,
	`GraduationCertificateId` varchar(50) NULL,
	`Major` varchar(100) NULL,
	`EmployeeId` varchar(50) NULL,
	`AttendanceCardId` varchar(50) NULL,
	`JobBegindate` datetime NULL,
	`JobOfficialDate` datetime NULL,
	`JobEndDate` datetime NULL,
	`JobStatus` int NULL,
	`ContractType` varchar(50) NULL,
	`ContractExpireDate` datetime NULL,
	`PaidHoliday` int NULL,
	`Remark` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `AccountId`
ALTER TABLE `tb_Member_ExtensionInformation` ADD CONSTRAINT `PK_tb_Member_ExtensionInformation` PRIMARY KEY CLUSTERED (`AccountId`);

-- ������: `tb_OrganizationUnit`
CREATE TABLE IF NOT EXISTS `tb_OrganizationUnit`(
	`Id` varchar(36) NOT NULL,
	`ParentId` varchar(36) NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`GlobalName` varchar(100) NULL,
	`FullName` varchar(50) NULL,
	`PinYin` varchar(100) NULL,
	`Type` int NULL,
	`Level` int NULL,
	`StandardOrganizationUnitId` varchar(36) NULL,
	`EnableExchangeEmail` int NULL,
	`EffectScope` int NULL,
	`TreeViewScope` int NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`FullPath` varchar(400) NULL,
	`DistinguishedName` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_OrganizationUnit` ADD CONSTRAINT `PK_tb_OrganizationUnit` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_Role`
CREATE TABLE IF NOT EXISTS `tb_Role`(
	`Id` varchar(36) NOT NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(100) NULL,
	`GlobalName` varchar(100) NULL,
	`PinYin` varchar(100) NULL,
	`Type` int NULL,
	`ParentId` varchar(36) NULL,
	`StandardRoleId` varchar(36) NULL,
	`OrganizationUnitId` varchar(36) NULL,
	`GeneralRoleId` varchar(36) NULL,
	`EnableExchangeEmail` int NULL,
	`EffectScope` int NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`FullPath` varchar(400) NULL,
	`DistinguishedName` varchar(800) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_Role` ADD CONSTRAINT `PK_tb_Role` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_StandardGeneralRole`
CREATE TABLE IF NOT EXISTS `tb_StandardGeneralRole`(
	`Id` varchar(36) NOT NULL,
	`Code` varchar(10) NULL,
	`Name` varchar(50) NULL,
	`GroupTreeNodeId` varchar(36) NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_StandardGeneralRole` ADD CONSTRAINT `PK_tb_StandardGeneralRole` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_StandardGeneralRole_Mapping`
CREATE TABLE IF NOT EXISTS `tb_StandardGeneralRole_Mapping`(
	`StandardGeneralRoleId` varchar(36) NOT NULL,
	`OrganizationUnitId` varchar(36) NOT NULL,
	`RoleId` varchar(36) NOT NULL,
	`StandardRoleId` varchar(36) NOT NULL
);

-- ��������: `StandardGeneralRoleId`, `OrganizationUnitId`, `RoleId`
ALTER TABLE `tb_StandardGeneralRole_Mapping` ADD CONSTRAINT `PK_tb_StandardGeneralRole_Mapping` PRIMARY KEY CLUSTERED (`StandardGeneralRoleId`, `OrganizationUnitId`, `RoleId`);

-- ������: `tb_StandardOrganizationUnit`
CREATE TABLE IF NOT EXISTS `tb_StandardOrganizationUnit`(
	`Id` varchar(36) NOT NULL,
	`ParentId` varchar(36) NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(50) NULL,
	`GlobalName` varchar(100) NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_StandardOrganizationUnit` ADD CONSTRAINT `PK_tb_StandardOrganizationUnit` PRIMARY KEY CLUSTERED (`Id`);

-- ������: `tb_StandardRole`
CREATE TABLE IF NOT EXISTS `tb_StandardRole`(
	`Id` varchar(36) NOT NULL,
	`ParentId` varchar(36) NULL,
	`Code` varchar(30) NULL,
	`Name` varchar(200) NULL,
	`Type` int NULL,
	`Priority` int NULL,
	`StandardOrganizationUnitId` varchar(36) NULL,
	`GroupTreeNodeId` varchar(36) NULL,
	`IsKey` bit NULL,
	`IsDraft` bit NULL,
	`Locking` int NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`Remark` varchar(200) NULL,
	`ModifiedDate` datetime NULL,
	`CreatedDate` datetime NULL
);

-- ��������: `Id`
ALTER TABLE `tb_StandardRole` ADD CONSTRAINT `PK_tb_StandardRole` PRIMARY KEY CLUSTERED (`Id`);

-- ������ͼ: `view_AuthorizationObject_Account_Temp` 
CREATE VIEW `view_AuthorizationObject_Account_Temp` AS
	
	-- �û� => ��Ȩ�ʺ� �Ĺ�ϵ
	SELECT 
		Id AS AuthorizationObjectId, 
		Name AS AuthorizationObjectName,
		Id AS AccountId,
		N'Account' AS AuthorizationObjectType
	FROM         
		tb_Account

	-- ��֯(Ĭ��) => ��Ȩ�ʺ� �Ĺ�ϵ
	UNION 
	SELECT 
		tb_OrganizationUnit.Id AS AuthorizationObjectId, 
		tb_OrganizationUnit.Name AS AuthorizationObjectName,
		tb_Account_OrganizationUnit.AccountId AS AccountId,
		N'OrganizationUnit' AS AuthorizationObjectType
	FROM 
		tb_Account_OrganizationUnit 
			INNER JOIN tb_OrganizationUnit ON tb_Account_OrganizationUnit.OrganizationUnitId = tb_OrganizationUnit.Id 
	WHERE
		BeginDate < CURRENT_TIMESTAMP AND EndDate > CURRENT_TIMESTAMP 

	-- ��ɫ => ��Ȩ�ʺ� �Ĺ�ϵ
	UNION 
	SELECT 
		RoleId AS AuthorizationObjectId, 
		Name AS AuthorizationObjectName,
		AccountId,
		N'Role' AS AuthorizationObjectType
	FROM 
		tb_Account_Role
			INNER JOIN tb_Role ON tb_Account_Role.RoleId = tb_Role.Id 
	WHERE
		BeginDate < CURRENT_TIMESTAMP AND EndDate > CURRENT_TIMESTAMP

	-- Ⱥ�� => ��Ȩ�ʺ� �Ĺ�ϵ
	UNION
	SELECT     
		tb_Group.Id AS AuthorizationObjectId, 
		tb_Group.Name AS AuthorizationObjectName, 
		tb_Account_Group.AccountId AS AccountId, 
		N'Group' AS AuthorizationObjectType
	FROM         
		tb_Account_Group 
			INNER JOIN tb_Group ON tb_Group.Id = tb_Account_Group.GroupId
	WHERE
		BeginDate < CURRENT_TIMESTAMP AND EndDate > CURRENT_TIMESTAMP 

	-- ͨ�ý�ɫ => ��Ȩ�ʺ� �Ĺ�ϵ
	UNION
    SELECT     
		tb_GeneralRole.Id AS AuthorizationObjectId, 
		tb_GeneralRole.Name AS AuthorizationObjectName, 
		tb_Account_Role.AccountId AS AccountId, 
		N'GeneralRole' AS AuthorizationObjectType
	FROM         
		tb_Account_Role 
			INNER JOIN tb_Role ON tb_Role.Id = tb_Account_Role.RoleId 
			INNER JOIN tb_GeneralRole ON tb_GeneralRole.Id = tb_Role.GeneralRoleId
	WHERE
		BeginDate < CURRENT_TIMESTAMP AND EndDate > CURRENT_TIMESTAMP 

	-- ��׼��ɫ => ��Ȩ�ʺ� �Ĺ�ϵ
	UNION
    SELECT     
		tb_StandardRole.Id AS AuthorizationObjectId, 
		tb_StandardRole.Name AS AuthorizationObjectName, 
		tb_Account_Role.AccountId AS AccountId, 
		N'StandardRole' AS AuthorizationObjectType
	FROM         
		tb_Account_Role 
			INNER JOIN tb_Role ON tb_Role.Id = tb_Account_Role.RoleId
			INNER JOIN tb_StandardRole ON tb_StandardRole.Id = tb_Role.StandardRoleId
	WHERE
		BeginDate < CURRENT_TIMESTAMP AND EndDate > CURRENT_TIMESTAMP 

	-- ������(��ɫ) => ��Ȩ�ʺ� �Ĺ�ϵ
	UNION
	SELECT     
		'00000000-0000-0000-0000-000000000000' AS AuthorizationObjectId, 
		'������' AS AuthorizationObjectName, 
		Id AS AccountId, 
        N'Role' AS AuthorizationObjectType
	FROM         
		tb_Account AS tb_Everyone
;

-- ������ͼ: `view_AuthorizationObject_Account` 
CREATE VIEW `view_AuthorizationObject_Account` AS
	
	SELECT 
		T.AuthorizationObjectId, 
		T.AuthorizationObjectType,
		T.AuthorizationObjectName, 
		Account.Id AS AccountId, 
		Account.GlobalName AS AccountGlobalName,
		Account.LoginName AS AccountLoginName ,
		CASE IFNULL(`Grant`.GranteeId,'') WHEN '' then Account.Id ELSE `Grant`.GranteeId END AS GranteeId,
		CASE IFNULL(`Grant`.GranteeId,'') WHEN '' then Account.GlobalName ELSE ( SELECT GlobalName FROM tb_Account WHERE Id = `Grant`.GranteeId ) END AS GranteeGlobalName

	FROM view_AuthorizationObject_Account_Temp T
		INNER JOIN tb_Account AS Account ON Account.Id = T.AccountId 
		LEFT JOIN tb_Account_Grant AS `Grant` ON (
			`Grant`.GrantorId = Account.Id AND DataQueryGrantMode = 1
			AND ( GrantedTimeFrom < CURRENT_TIMESTAMP AND GrantedTimeTo > CURRENT_TIMESTAMP ) )
;

-- drop function func_GetCorporationIdByOrganizationUnitId
DROP FUNCTION IF EXISTS `func_GetCorporationIdByOrganizationUnitId`;

-- create function func_GetCorporationIdByOrganizationUnitId
DELIMITER $$
-- ��������: `func_GetCorporationIdByOrganizationUnitId`
CREATE FUNCTION `func_GetCorporationIdByOrganizationUnitId`
(
	OrganizationUnitId  VARCHAR(50)
)

RETURNS VARCHAR(50)

-- WITH EXECUTE AS CALLER
-- AS

BEGIN

    DECLARE OrganizationUnitType VARCHAR(10);

    DECLARE BackwardCount INT;

    SET BackwardCount = 1;

    SET BackwardCount = 1;

    SELECT Id, Type INTO OrganizationUnitId, OrganizationUnitType FROM tb_OrganizationUnit WHERE Id = OrganizationUnitId;

    -- ��֯���͵�����, ��Ϊ��˾����, ѭ������.
    WHILE (OrganizationUnitType != '0') DO
        SET BackwardCount  = BackwardCount + 1;

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
End $$

-- tb_Account
INSERT `tb_Account` (`Id`, `Code`, `Name`, `GlobalName`, `DisplayName`, `PinYin`, `LoginName`, `Password`, `IdentityCard`, `Type`, `CertifiedMobile`, `CertifiedEmail`, `CertifiedAvatar`, `EnableExchangeEmail`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `IP`, `LoginDate`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000000', N'0000', N'�ο�', N'�ο�', N'�ο�', N'', N'guest', N'p@ssw0rd', N'', 0, N'', N'', NULL, 0, NULL, NULL, N'', 1, N'', N'127.0.0.1', '2000-01-01', N'CN=guest,OU=��֯�û�,DC=sharepoint,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_Account` (`Id`, `Code`, `Name`, `GlobalName`, `DisplayName`, `PinYin`, `LoginName`, `Password`, `IdentityCard`, `Type`, `CertifiedMobile`, `CertifiedEmail`, `CertifiedAvatar`, `EnableExchangeEmail`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `IP`, `LoginDate`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', N'0001', N'��������Ա', N'��������Ա', N'��������Ա', N'', N'admin', N'p@ssw0rd', N'', 0, N'', N'', NULL, 1, NULL, 1, N'', 1, N'', N'127.0.0.1', '2000-01-01', N'CN=admin,OU=��֯�û�,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_Account` (`Id`, `Code`, `Name`, `GlobalName`, `DisplayName`, `PinYin`, `LoginName`, `Password`, `IdentityCard`, `Type`, `CertifiedMobile`, `CertifiedEmail`, `CertifiedAvatar`, `EnableExchangeEmail`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `IP`, `LoginDate`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000002', N'0002', N'�����ʺ�', N'�����ʺ�', N'���������ʺ�', N'', N'service', N'p@ssw0rd', N'', 0, N'', N'', NULL, 1, NULL, NULL, N'', 1, N'', N'127.0.0.1', '2000-01-01', N'CN=service,OU=��֯�û�,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');

-- tb_Member
INSERT `tb_Member` (`Id`, `AccountId`, `CorporationId`, `DepartmentId`, `Department2Id`, `Department3Id`, `OrganizationUnitId`, `RoleId`, `JobId`, `AssignedJobId`, `JobGradeId`, `JobGradeDisplayName`, `Headship`, `Sex`, `Birthday`, `GraduationDate`, `EntryDate`, `PromotionDate`, `Hometown`, `City`, `Mobile`, `Telephone`, `QQ`, `MSN`, `Email`, `Rtx`, `PostCode`, `Address`, `Url`, `FullPath`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'a3291748-4856-495e-ba6f-193896ca3345', N'7b5bb868-b31e-4126-a4fa-b6c9e22fb09a', N'', N'', N'7b5bb868-b31e-4126-a4fa-b6c9e22fb09a', N'AF36AF26-68B6-4BC0-9375-358A4492677D', N'', N'', N'', N'', N'', N'', NULL, NULL, NULL, NULL, N'', N'', NULL, N'', N'', N'', N'', N'', N'', N'', N'', NULL, '2000-01-01', '2000-01-01');
INSERT `tb_Member` (`Id`, `AccountId`, `CorporationId`, `DepartmentId`, `Department2Id`, `Department3Id`, `OrganizationUnitId`, `RoleId`, `JobId`, `AssignedJobId`, `JobGradeId`, `JobGradeDisplayName`, `Headship`, `Sex`, `Birthday`, `GraduationDate`, `EntryDate`, `PromotionDate`, `Hometown`, `City`, `Mobile`, `Telephone`, `QQ`, `MSN`, `Email`, `Rtx`, `PostCode`, `Address`, `Url`, `FullPath`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', N'00000000-0000-0000-0000-000000000001', N'00000000-0000-0000-0000-000000000004', N'00000000-0000-0000-0000-000000000007', N'00000000-0000-0000-0000-000000000008', N'', N'de00fd53-0d33-419d-9cac-9e4de0ff38ca', N'00000000-0000-0000-0000-000000000005', N'', N'', N'', N'', N'δ��', N'��', '1900-01-01', '1900-01-01', NULL, NULL, N'', N'', NULL, N'', N'', N'', N'δ��', N'', N'', N'', N'', NULL, '2000-01-01', '2000-01-01');
INSERT `tb_Member` (`Id`, `AccountId`, `CorporationId`, `DepartmentId`, `Department2Id`, `Department3Id`, `OrganizationUnitId`, `RoleId`, `JobId`, `AssignedJobId`, `JobGradeId`, `JobGradeDisplayName`, `Headship`, `Sex`, `Birthday`, `GraduationDate`, `EntryDate`, `PromotionDate`, `Hometown`, `City`, `Mobile`, `Telephone`, `QQ`, `MSN`, `Email`, `Rtx`, `PostCode`, `Address`, `Url`, `FullPath`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000002', N'00000000-0000-0000-0000-000000000002', N'a3291748-4856-495e-ba6f-193896ca3345', N'7b5bb868-b31e-4126-a4fa-b6c9e22fb09a', N'', N'', N'7b5bb868-b31e-4126-a4fa-b6c9e22fb09a', N'AF36AF26-68B6-4BC0-9375-358A4492677D', N'', N'', N'', N'', N'', N'', NULL, NULL, NULL, NULL, N'', N'', NULL, N'4', N'', N'', N'', N'', N'', N'', N'', NULL, '2000-01-01', '2000-01-01');

-- tb_OrganizationUnit
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', N'00000000-0000-0000-0000-000000000000', N'0000', N'��֯�ṹ', N'��֯�ṹ', N'��֯�ܹ�', N'root', 1, 0, N'', 0, 0, 0, 1, N'0001', 1, N'', N'', N'CN=��֯�ṹ,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000002', N'00000000-0000-0000-0000-000000000001', N'9000', N'����', N'����', N'����', N'uncategorized', 0, 1, N'', 1, 0, 0, 1, N'9999', 1, N'', N'', N'CN=�����ܲ�,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000003', N'00000000-0000-0000-0000-000000000002', N'9001', N'�ⲿ�û���', N'�ⲿ�û���', N'�ⲿ�û���', N'externalusers ', 1, 1, N'', 1, 0, 0, 1, N'0001', 1, N'', N'', N'CN=�����ܲ�,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000004', N'00000000-0000-0000-0000-000000000001', N'0001', N'�����ܲ�', N'�����ܲ�', N'�����ܲ�', N'jtzb', 0, 1, N'', 1, 0, 0, 1, N'0001', 1, N'', N'', N'CN=�����ܲ�,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000005', N'00000000-0000-0000-0000-000000000004', N'0002', N'������Դ��', N'����������Դ��', N'������Դ��', N'jtrlzyb', 1, 2, N'', 1, 0, 0, 1, N'0002', 1, N'', N'��֯�ṹ\�����ܲ�\������Դ��', N'CN=������Դ��,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000006', N'00000000-0000-0000-0000-000000000004', N'0003', N'����', N'���Ų���', N'', N'jtcwb', 1, 2, N'', 1, 0, 0, 1, N'0003', 1, N'', N'', N'CN=���Ų���,OU=����,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000007', N'00000000-0000-0000-0000-000000000004', N'0004', N'��������������', N'���Ź�������������', N'', N'jtxzb', 1, 2, N'', 1, 0, 0, 1, N'0004', 1, N'', N'', N'CN=���Ų���,OU=����,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_OrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `FullName`, `PinYin`, `Type`, `Level`, `StandardOrganizationUnitId`, `EnableExchangeEmail`, `EffectScope`, `TreeViewScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000008', N'00000000-0000-0000-0000-000000000007', N'0005', N'��Ϣ����', N'������Ϣ����', N'��Ϣ����', N'jtxxzx', 1, 3, N'', 1, 0, 0, 1, N'0001', 1, N'', N'��֯�ṹ\�����ܲ�\��������������\��Ϣ����', N'CN=��Ϣ����,OU=��������������,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');

-- tb_Role
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000000', N'0000', N'������', N'������', N'', NULL, N'', N'', N'', N'', NULL, NULL, NULL, N'', 1, N'', N'', N'', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', N'0001', N'Ĭ�Ͻ�ɫ', N'Ĭ�Ͻ�ɫ', N'', NULL, N'00000000-0000-0000-0000-000000000000', N'', N'00000000-0000-0000-0000-000000000002', N'', NULL, NULL, NULL, N'', NULL, N'', N'', N'', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000002', N'0002', N'�ⲿ�û�', N'�ⲿ�û�', N'', NULL, N'00000000-0000-0000-0000-000000000000', N'', N'00000000-0000-0000-0000-000000000002', N'', NULL, NULL, NULL, N'', NULL, N'', N'', N'', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000003', N'0003', N'���Ŷ��³�', N'���Ŷ��³�', N'', 3, N'00000000-0000-0000-0000-000000000000', N'bd13143c-d17a-452c-9d32-933341b2f358', N'00000000-0000-0000-0000-000000000004', N'', 0, 0, 0, N'2', 0, N'', N'��֯�ṹ\�����ܲ�\���Ŷ��³�', N'CN=���Ŷ��³�,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000004', N'0004', N'���Ų��񲿸�����', N'���Ų��񲿸�����', N'', 3, N'00000000-0000-0000-0000-000000000003', N'bd13143c-d17a-452c-9d32-933341b2f358', N'00000000-0000-0000-0000-000000000006', N'', 0, 0, 0, N'2', 1, N'', N'��֯�ṹ\�����ܲ�\����\���Ų��񲿸�����', N'CN=���Ų��񲿸�����,OU=����,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000005', N'0005', N'������Ϣ����ϵͳ����Ա', N'������Ϣ����ϵͳ����Ա', N'', 3, N'00000000-0000-0000-0000-000000000003', N'bd13143c-d17a-452c-9d32-933341b2f358', N'00000000-0000-0000-0000-000000000008', N'', 0, 0, 0, N'2', 1, N'', N'��֯�ṹ\�����ܲ�\��������������\��Ϣ����\������Ϣ����ϵͳ����Ա', N'CN=������Ϣ����ϵͳ����Ա,OU=��Ϣ����,OU=��������������,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000006', N'0006', N'���Ź�������������������', N'���Ź�������������������', N'', 3, N'00000000-0000-0000-0000-000000000003', N'', N'00000000-0000-0000-0000-000000000007', N'', NULL, NULL, NULL, N'2', 1, N'', N'', N'', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000007', N'0007', N'����������Դ��������', N'����������Դ��������', N'', 3, N'00000000-0000-0000-0000-000000000006', N'bd13143c-d17a-452c-9d32-933341b2f358', N'00000000-0000-0000-0000-000000000005', N'', 0, 0, 0, N'2', 1, N'', N'��֯�ṹ\�����ܲ�\������Դ��\����������Դ��������', N'CN=����������Դ��������,OU=������Դ��,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');
INSERT `tb_Role` (`Id`, `Code`, `Name`, `GlobalName`, `PinYin`, `Type`, `ParentId`, `StandardRoleId`, `OrganizationUnitId`, `GeneralRoleId`, `EnableExchangeEmail`, `EffectScope`, `Locking`, `OrderId`, `Status`, `Remark`, `FullPath`, `DistinguishedName`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000008', N'0008', N'Ա��', N'Ա��', N'', 0, N'00000000-0000-0000-0000-000000000000', N'bd13143c-d17a-452c-9d32-933341b2f358', N'00000000-0000-0000-0000-000000000004', NULL, 0, 0, 1, N'', 1, N'', N'��֯�ṹ\�����ܲ�\Ա��', N'CN=Ա��,OU=�����ܲ�,OU=��֯�ṹ,DC=lhwork,DC=net', '2000-01-01', '2000-01-01');

-- tb_StandardOrganizationUnit
INSERT `tb_StandardOrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', N'00000000-0000-0000-0000-000000000000', N'0', N'��׼��֯�ܹ�', N'��׼��֯�ܹ�', 1, N'10001', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardOrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000002', N'00000000-0000-0000-0000-000000000001', N'99999', N'����', N'����', 1, N'10004', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardOrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000003', N'00000000-0000-0000-0000-000000000001', N'10002', N'��Ŀ�Ŷ�', N'��Ŀ�Ŷ�', 1, N'10003', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardOrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000004', N'00000000-0000-0000-0000-000000000001', N'10001', N'�����ֹ�˾', N'�����ֹ�˾', 1, N'10002', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardOrganizationUnit` (`Id`, `ParentId`, `Code`, `Name`, `GlobalName`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000005', N'00000000-0000-0000-0000-000000000001', N'10000', N'�����ܲ�', N'�����ܲ�', 1, N'10001', 1, N'', '2000-01-01', '2000-01-01');

-- tb_StandardRole
INSERT `tb_StandardRole` (`Id`, `ParentId`, `Code`, `Name`, `Type`, `Priority`, `StandardOrganizationUnitId`, `GroupTreeNodeId`, `IsKey`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', N'00000000-0000-0000-0000-000000000000', N'99999', N'Ĭ�ϱ�׼��ɫ', 0, 0, '00000000-0000-0000-0000-000000000002', N'', 0, 0, 1, N'', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardRole` (`Id`, `ParentId`, `Code`, `Name`, `Type`, `Priority`, `StandardOrganizationUnitId`, `GroupTreeNodeId`, `IsKey`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000002', N'00000000-0000-0000-0000-000000000000', N'10003', N'��Ŀ������', 2, 0, '00000000-0000-0000-0000-000000000003', N'', 1, 0, 1, N'', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardRole` (`Id`, `ParentId`, `Code`, `Name`, `Type`, `Priority`, `StandardOrganizationUnitId`, `GroupTreeNodeId`, `IsKey`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000003', N'00000000-0000-0000-0000-000000000000', N'10002', N'�����ֹ�˾����', 1, 80, '00000000-0000-0000-0000-000000000004', N'', 1, 0, 1, N'', 1, N'', '2000-01-01', '2000-01-01');
INSERT `tb_StandardRole` (`Id`, `ParentId`, `Code`, `Name`, `Type`, `Priority`, `StandardOrganizationUnitId`, `GroupTreeNodeId`, `IsKey`, `IsDraft`, `Locking`, `OrderId`, `Status`, `Remark`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000004', N'00000000-0000-0000-0000-000000000000', N'10001', N'���Ŷ��³�', 0, 90, '00000000-0000-0000-0000-000000000005', N'', 1, 0, 1, N'', 1, N'', '2000-01-01', '2000-01-01');

-- tb_JobFamily
INSERT INTO `tb_JobFamily` (`Id`, `Name`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES ('00000000-0000-0000-0000-000000000001','����', 1, '2000-01-01', '2000-01-01');
INSERT INTO `tb_JobFamily` (`Id`, `Name`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES ('00000000-0000-0000-0000-000000000002','ְ��', 1, '2000-01-01', '2000-01-01');
INSERT INTO `tb_JobFamily` (`Id`, `Name`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES ('00000000-0000-0000-0000-000000000003','Ӫ��', 1, '2000-01-01', '2000-01-01');
INSERT INTO `tb_JobFamily` (`Id`, `Name`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES ('00000000-0000-0000-0000-000000000004','����', 1, '2000-01-01', '2000-01-01');
INSERT INTO `tb_JobFamily` (`Id`, `Name`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES ('00000000-0000-0000-0000-000000000005','����', 1, '2000-01-01', '2000-01-01');

-- tb_JobGrade
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000000', N'Ĭ��ְ��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000001', '00000000-0000-0000-0000-000000000001', N'һ��Ա��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000002', '00000000-0000-0000-0000-000000000001', N'����Ա��', 20, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000003', '00000000-0000-0000-0000-000000000001', N'����Ա��', 30, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000004', '00000000-0000-0000-0000-000000000001', N'�ļ�Ա��', 40, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000005', '00000000-0000-0000-0000-000000000001', N'�弶Ա��', 50, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000006', '00000000-0000-0000-0000-000000000001', N'����Ա��', 60, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000007', '00000000-0000-0000-0000-000000000001', N'�߼�Ա��', 70, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000008', '00000000-0000-0000-0000-000000000001', N'�˼�Ա��', 80, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-100000000009', '00000000-0000-0000-0000-000000000001', N'�ż�Ա��', 90, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-200000000001', '00000000-0000-0000-0000-000000000003', N'A1��Ա��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-200000000002', '00000000-0000-0000-0000-000000000003', N'A2��Ա��', 20, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-200000000003', '00000000-0000-0000-0000-000000000003', N'A3��Ա��', 30, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-200000000004', '00000000-0000-0000-0000-000000000003', N'A4��Ա��', 40, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-200000000005', '00000000-0000-0000-0000-000000000003', N'A5��Ա��', 50, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-200000000006', '00000000-0000-0000-0000-000000000003', N'A6��Ա��', 60, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-300000000001', '00000000-0000-0000-0000-000000000003', N'M1��Ա��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-300000000002', '00000000-0000-0000-0000-000000000003', N'M2��Ա��', 20, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-300000000003', '00000000-0000-0000-0000-000000000003', N'M3��Ա��', 30, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-300000000004', '00000000-0000-0000-0000-000000000003', N'M4��Ա��', 40, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-300000000005', '00000000-0000-0000-0000-000000000003', N'M5��Ա��', 50, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-300000000006', '00000000-0000-0000-0000-000000000003', N'M6��Ա��', 60, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-400000000001', '00000000-0000-0000-0000-000000000004', N'T1��Ա��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-400000000002', '00000000-0000-0000-0000-000000000004', N'T2��Ա��', 20, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-400000000003', '00000000-0000-0000-0000-000000000004', N'T3��Ա��', 30, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-400000000004', '00000000-0000-0000-0000-000000000004', N'T4��Ա��', 40, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-400000000005', '00000000-0000-0000-0000-000000000004', N'T5��Ա��', 50, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-400000000006', '00000000-0000-0000-0000-000000000004', N'T6��Ա��', 60, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-500000000001', '00000000-0000-0000-0000-000000000005', N'O1��Ա��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-500000000002', '00000000-0000-0000-0000-000000000005', N'O2��Ա��', 10, 1, '2000-01-01', '2000-01-01');
INSERT `tb_JobGrade` (`Id`, `JobFamilyId`, `Name`, `Value`, `Status`, `ModifiedDate`, `CreatedDate`) VALUES (N'00000000-0000-0000-0000-500000000003', '00000000-0000-0000-0000-000000000005', N'O3��Ա��', 10, 1, '2000-01-01', '2000-01-01');

-- �������ݿ� 
use sys_config;

-- Ӧ�÷�����Ϣ
DELETE FROM `tb_Application_Method` WHERE ApplicationId = '00000000-0000-0000-0000-000000000100';

INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M01','00000000-0000-0000-0000-000000000100','01-05-E01-M01','membership.contacts.getDynamicTreeView','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"GetDynamicTreeView"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M02','00000000-0000-0000-0000-000000000100','01-05-E01-M02','membership.contacts.findAll','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"FindAll"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M03','00000000-0000-0000-0000-000000000100','01-05-E01-M03','membership.contacts.findAllByOrganizationUnitId','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"FindAllByOrganizationUnitId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M04','00000000-0000-0000-0000-000000000100','01-05-E01-M04','membership.contacts.findAllByGroupNodeId','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"FindAllByGroupNodeId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M05','00000000-0000-0000-0000-000000000100','01-05-E01-M05','membership.contacts.findAllByStandardOrganizationUnitId','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"FindAllByStandardOrganizationUnitId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M10','00000000-0000-0000-0000-000000000100','01-05-E01-M10','membership.contacts.view','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"View"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E01-M11','00000000-0000-0000-0000-000000000100','01-05-E01-M11','membership.contacts.getTreeView','','generic','{className:"X3Platform.Membership.Ajax.ContactsWrapper,X3Platform.Membership",methodName:"GetTreeView"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M01','00000000-0000-0000-0000-000000000100','01-05-E02-M01','membership.member.query','','generic','{className:"X3Platform.Membership.Ajax.MemberWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M01','00000000-0000-0000-0000-000000000100','01-05-E02-M01','membership.account.query','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M03','00000000-0000-0000-0000-000000000100','01-05-E02-M03','membership.account.findOne','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M03','00000000-0000-0000-0000-000000000100','01-05-E02-M03','membership.member.findOne','','generic','{className:"X3Platform.Membership.Ajax.MemberWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M04','00000000-0000-0000-0000-000000000100','01-05-E02-M04','membership.account.create','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M05','00000000-0000-0000-0000-000000000100','01-05-E02-M05','membership.account.save','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M06','00000000-0000-0000-0000-000000000100','01-05-E02-M06','membership.account.delete','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M07','00000000-0000-0000-0000-000000000100','01-05-E02-M07','membership.member.login','','generic','{className:"X3Platform.Membership.Ajax.MemberWrapper,X3Platform.Membership",methodName:"LoginCheck"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M08','00000000-0000-0000-0000-000000000100','01-05-E02-M08','membership.member.read','','generic','{className:"X3Platform.Membership.Ajax.MemberWrapper,X3Platform.Membership",methodName:"Read"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M10','00000000-0000-0000-0000-000000000100','01-05-E02-M10','membership.member.findOneByAccountId','','generic','{className:"X3Platform.Membership.Ajax.MemberWrapper,X3Platform.Membership",methodName:"FindOneByAccountId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M10','00000000-0000-0000-0000-000000000100','01-05-E02-M10','membership.account.setLoginName','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"SetLoginName"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E03-M11','00000000-0000-0000-0000-000000000100','01-05-E02-M11','membership.account.setPassword','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"SetPassword"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M15','00000000-0000-0000-0000-000000000100','01-05-E02-M15','membership.member.setContactCard','','generic','{className:"X3Platform.Membership.Ajax.MemberWrapper,X3Platform.Membership",methodName:"SetContactCard"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M01','00000000-0000-0000-0000-000000000100','01-05-E03-M01','membership.organization.query','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M03','00000000-0000-0000-0000-000000000100','01-05-E03-M03','membership.organization.findOne','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M04','00000000-0000-0000-0000-000000000100','01-05-E03-M04','membership.organization.create','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M05','00000000-0000-0000-0000-000000000100','01-05-E03-M05','membership.organization.save','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M06','00000000-0000-0000-0000-000000000100','01-05-E03-M06','membership.organization.delete','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M01','00000000-0000-0000-0000-000000000100','01-05-E04-M01','membership.role.query','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M03','00000000-0000-0000-0000-000000000100','01-05-E04-M03','membership.role.findOne','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M04','00000000-0000-0000-0000-000000000100','01-05-E04-M04','membership.role.create','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M05','00000000-0000-0000-0000-000000000100','01-05-E04-M05','membership.role.save','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M06','00000000-0000-0000-0000-000000000100','01-05-E04-M06','membership.role.delete','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M10','00000000-0000-0000-0000-000000000100','01-05-E04-M10','membership.organization.getCorporationTreeView','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"GetCorporationTreeView"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M11','00000000-0000-0000-0000-000000000100','01-05-E04-M11','membership.role.findAllByAccountId','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"FindAllByAccountId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M12','00000000-0000-0000-0000-000000000100','01-05-E04-M12','membership.role.findAllByProjectId','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"FindAllByProjectId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E04-M20','00000000-0000-0000-0000-000000000100','01-05-E04-M20','membership.organization.setGlobalName','','generic','{className:"X3Platform.Membership.Ajax.OrganizationUnitWrapper,X3Platform.Membership",methodName:"SetGlobalName"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E06-M01','00000000-0000-0000-0000-000000000100','01-05-E05-M01','membership.group.query','','generic','{className:"X3Platform.Membership.Ajax.GroupWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E06-M02','00000000-0000-0000-0000-000000000100','01-05-E05-M02','membership.group.findAll','','generic','{className:"X3Platform.Membership.Ajax.GroupWrapper,X3Platform.Membership",methodName:"FindAll"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E06-M03','00000000-0000-0000-0000-000000000100','01-05-E05-M03','membership.group.findOne','','generic','{className:"X3Platform.Membership.Ajax.GroupWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E06-M04','00000000-0000-0000-0000-000000000100','01-05-E05-M04','membership.group.create','','generic','{className:"X3Platform.Membership.Ajax.GroupWrapper,X3Platform.Membership",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E06-M06','00000000-0000-0000-0000-000000000100','01-05-E05-M06','membership.group.delete','','generic','{className:"X3Platform.Membership.Ajax.GroupWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E02-M20','00000000-0000-0000-0000-000000000100','01-05-E05-M20','membership.account.setGlobalName','','generic','{className:"X3Platform.Membership.Ajax.AccountWrapper,X3Platform.Membership",methodName:"setGlobalName"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E05-M20','00000000-0000-0000-0000-000000000100','01-05-E05-M20','membership.role.setGlobalName','','generic','{className:"X3Platform.Membership.Ajax.RoleWrapper,X3Platform.Membership",methodName:"setGlobalName"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E07-M01','00000000-0000-0000-0000-000000000100','01-05-E07-M01','membership.standardOrganizationUnit.query','','generic','{className:"X3Platform.Membership.Ajax.StandardOrganizationUnitWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E07-M05','00000000-0000-0000-0000-000000000100','01-05-E07-M05','membership.standardOrganizationUnit.save','','generic','{className:"X3Platform.Membership.Ajax.StandardOrganizationUnitWrapper,X3Platform.Membership",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E07-M06','00000000-0000-0000-0000-000000000100','01-05-E07-M06','membership.standardOrganizationUnit.delete','','generic','{className:"X3Platform.Membership.Ajax.StandardOrganizationUnitWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E07-M07','00000000-0000-0000-0000-000000000100','01-05-E07-M07','membership.standardOrganizationUnit.getDynamicTreeView','','generic','{className:"X3Platform.Membership.Ajax.StandardOrganizationUnitWrapper,X3Platform.Membership",methodName:"GetDynamicTreeView"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E08-M01','00000000-0000-0000-0000-000000000100','01-05-E08-M01','membership.standardRole.query','','generic','{className:"X3Platform.Membership.Ajax.StandardRoleWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E08-M03','00000000-0000-0000-0000-000000000100','01-05-E08-M03','membership.standardRole.findOne','','generic','{className:"X3Platform.Membership.Ajax.StandardRoleWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E08-M04','00000000-0000-0000-0000-000000000100','01-05-E08-M04','membership.standardRole.create','','generic','{className:"X3Platform.Membership.Ajax.StandardRoleWrapper,X3Platform.Membership",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E08-M05','00000000-0000-0000-0000-000000000100','01-05-E08-M05','membership.standardRole.save','','generic','{className:"X3Platform.Membership.Ajax.StandardRoleWrapper,X3Platform.Membership",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E08-M06','00000000-0000-0000-0000-000000000100','01-05-E08-M06','membership.standardRole.delete','','generic','{className:"X3Platform.Membership.Ajax.StandardRoleWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M01','00000000-0000-0000-0000-000000000100','01-05-E09-M01','membership.standardGeneralRole.query','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M03','00000000-0000-0000-0000-000000000100','01-05-E09-M03','membership.standardGeneralRole.findOne','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M04','00000000-0000-0000-0000-000000000100','01-05-E09-M04','membership.standardGeneralRole.create','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M05','00000000-0000-0000-0000-000000000100','01-05-E09-M05','membership.standardGeneralRole.save','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M06','00000000-0000-0000-0000-000000000100','01-05-E09-M06','membership.standardGeneralRole.delete','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M10','00000000-0000-0000-0000-000000000100','01-05-E09-M10','membership.standardGeneralRole.getMappingTable','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"GetMappingTable"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M11','00000000-0000-0000-0000-000000000100','01-05-E09-M11','membership.standardGeneralRole.queryMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"GetMappingRelationPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M12','00000000-0000-0000-0000-000000000100','01-05-E09-M12','membership.standardGeneralRole.findAllMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"FindAllMappingRelation"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M13','00000000-0000-0000-0000-000000000100','01-05-E09-M13','membership.standardGeneralRole.findOneMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"FindOneMappingRelation"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M14','00000000-0000-0000-0000-000000000100','01-05-E09-M14','membership.standardGeneralRole.createMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"CreateMappingRelation"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M15','00000000-0000-0000-0000-000000000100','01-05-E09-M15','membership.standardGeneralRole.addMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"AddMappingRelation"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M16','00000000-0000-0000-0000-000000000100','01-05-E09-M16','membership.standardGeneralRole.removeMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"RemoveMappingRelation"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E09-M17','00000000-0000-0000-0000-000000000100','01-05-E09-M17','membership.standardGeneralRole.hasMappingRelation','','generic','{className:"X3Platform.Membership.Ajax.StandardGeneralRoleWrapper,X3Platform.Membership",methodName:"HasMappingRelation"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E10-M11','00000000-0000-0000-0000-000000000100','01-05-E10-M11','membership.groupTree.getDynamicTreeView','','generic','{className:"X3Platform.Membership.Ajax.GroupTreeWrapper,X3Platform.Membership",methodName:"GetDynamicTreeView"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E11-M01','00000000-0000-0000-0000-000000000100','01-05-E11-M01','membership.accountGrant.query','','generic','{className:"X3Platform.Membership.Ajax.AccountGrantWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E12-M01','00000000-0000-0000-0000-000000000100','01-05-E12-M01','membership.accountLog.query','','generic','{className:"X3Platform.Membership.Ajax.AccountLogWrapper,X3Platform.Membership",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`ModifiedDate`,`CreatedDate`) VALUES ('01-05-E12-M06','00000000-0000-0000-0000-000000000100','01-05-E12-M016','membership.accountLog.delete','','generic','{className:"X3Platform.Membership.Ajax.AccountLogWrapper,X3Platform.Membership",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
