-- ================================================
-- Ӧ�ù����ʼ���ű� MySQL �� (2010-01-01)
-- ================================================

-- �������ݿ�: `sys_config`
-- CREATE DATABASE `sys_config`

-- ������: `tb_DigitalNumber`
CREATE TABLE `tb_DigitalNumber`(
	`Name` varchar(100) NOT NULL,
	`Expression` varchar(200) NULL,
	`Seed` int NULL,
	`UpdateDate` datetime NULL,
	`CreateDate` datetime NULL
);

-- ��������: `Name`
ALTER TABLE `tb_DigitalNumber` ADD CONSTRAINT `PK_tb_DigitalNumber` PRIMARY KEY CLUSTERED (`Name`);
