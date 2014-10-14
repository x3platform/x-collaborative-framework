-- ================================================
-- 任务管理初始化脚本 MySQL 版 (2010-01-01)
-- ================================================

-- 创建数据库: `sys_tasks`
-- CREATE DATABASE `sys_tasks`

-- 创建表: `tb_Task_WorkItem`
CREATE TABLE IF NOT EXISTS `tb_Task_WorkItem` (
    `Id` varchar(36) NOT NULL,
    `TrackingSignal` varchar(20) NULL,
    `ApplicationId` varchar(36) NULL,
    `TaskCode` varchar(120) NULL,
    `CategoryId` varchar(36) NULL,
    `Type` varchar(20) NULL,
    `Title` varchar(600) NULL,
    `Content` varchar(800) NULL,
    `Tags` varchar(50) NULL,
    `SenderId` varchar(36) NOT NULL,
    `ReceiverId` varchar(36) NOT NULL,
    `IsRead` bit(1) NULL,
    `Status` int NULL,
    `FinishTime` datetime NULL,
    `CreateDate` datetime NULL
);

-- 设置主键: `Id`, `ReceiverId`
ALTER TABLE `tb_Task_WorkItem` ADD CONSTRAINT `PK_tb_Task_WorkItem` PRIMARY KEY CLUSTERED (`Id`, `ReceiverId`);

-- 创建表: `tb_Task_HistoryItem`
CREATE TABLE IF NOT EXISTS `tb_Task_HistoryItem` (
	`Id` varchar(36) NOT NULL,
	`TrackingSignal` varchar(20) NULL,
    `ApplicationId` varchar(36) NULL,
	`TaskCode` varchar(120) NULL,
    `CategoryId` varchar(36) NULL,
	`Type` varchar(20) NULL,
	`Title` varchar(600) NULL,
	`Content` varchar(800) NULL,
	`Tags` varchar(50) NULL,
	`SenderId` varchar(36) NOT NULL,
	`ReceiverId` varchar(36) NOT NULL,
	`IsRead` bit(1) NULL,
	`Status` int NULL,
	`FinishTime` datetime NULL,
	`CreateDate` datetime NULL
);

-- 设置主键: `Id`, `ReceiverId`
ALTER TABLE `tb_Task_HistoryItem` ADD CONSTRAINT `PK_tb_Task_HistoryItem` PRIMARY KEY CLUSTERED (`Id`, `ReceiverId`);

-- 创建表: `tb_Task_WaitingItem`
CREATE TABLE IF NOT EXISTS `tb_Task_WaitingItem` (
	`Id` varchar(36) NOT NULL,
	`TrackingSignal` varchar(20) NULL,
    `ApplicationId` varchar(36) NULL,
	`TaskCode` varchar(120) NULL,
	`Type` varchar(20) NULL,
	`Title` varchar(500) NULL,
	`Content` varchar(800) NULL,
	`Tags` varchar(100) NULL,
	`SenderId` varchar(36) NULL,
    `ReceiverId` varchar(36) NOT NULL,
	`TriggerTime` datetime NULL,
	`IsSend` bit(1) NULL,
	`SendTime` datetime NULL,
	`CreateDate` datetime NULL
);

-- 设置主键: `Id`, `ReceiverId`
ALTER TABLE `tb_Task_WaitingItem` ADD CONSTRAINT `PK_tb_Task_WaitingItem` PRIMARY KEY CLUSTERED (`Id`, `ReceiverId`);

-- 创建表: `tb_Task_Category`
CREATE TABLE IF NOT EXISTS `tb_Task_Category` (
	`Id` varchar(36) NOT NULL,
	`AccountId` varchar(36) NULL,
	`AccountName` varchar(50) NULL,
	`CategoryIndex` varchar(200) NULL,
	`Description` text NULL,
	`Tags` varchar(800) NULL,
	`OrderId` varchar(20) NULL,
	`Status` int NULL,
	`UpdateDate` datetime NULL,
	`CreateDate` datetime NULL
);

-- 设置主键: `Id`
ALTER TABLE `tb_Task_Category` ADD CONSTRAINT `PK_tb_Task_Category` PRIMARY KEY CLUSTERED (`Id`);

-- 创建查询视图
CREATE VIEW view_Task_WorkItem 
AS
SELECT * FROM `sys_tasks_node_1`.`tb_Task_WorkItem`
UNION SELECT * FROM `sys_tasks_node_2`.`tb_Task_WorkItem`
UNION SELECT * FROM `sys_tasks_node_3`.`tb_Task_WorkItem`
UNION SELECT * FROM `sys_tasks_node_4`.`tb_Task_WorkItem`
UNION SELECT * FROM `sys_tasks_node_5`.`tb_Task_WorkItem`

CREATE VIEW view_Task_HistoryItem
AS
SELECT * FROM `sys_tasks_node_1`.`tb_Task_HistoryItem`
UNION SELECT * FROM `sys_tasks_node_2`.`tb_Task_HistoryItem`
UNION SELECT * FROM `sys_tasks_node_3`.`tb_Task_HistoryItem`
UNION SELECT * FROM `sys_tasks_node_4`.`tb_Task_HistoryItem`
UNION SELECT * FROM `sys_tasks_node_5`.`tb_Task_HistoryItem`

CREATE VIEW view_Task_WaitingItem 
AS
SELECT * FROM `sys_tasks_node_1`.`tb_Task_WaitingItem`
UNION SELECT * FROM `sys_tasks_node_2`.`tb_Task_WaitingItem`
UNION SELECT * FROM `sys_tasks_node_3`.`tb_Task_WaitingItem`
UNION SELECT * FROM `sys_tasks_node_4`.`tb_Task_WaitingItem`
UNION SELECT * FROM `sys_tasks_node_5`.`tb_Task_WaitingItem`

--
use sys_config;

-- 应用方法信息
DELETE FROM tb_Application_Method WHERE ApplicationId = '1f3e1dec-c411-44e0-82cf-ae4447a36769';

INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M01','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M01','task.query','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M02','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M02','task.delete','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M03','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M03','task.update','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"Update"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M08','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M08','task.send','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"Send"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M10','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M10','task.redirect','','generic','{className:"X3Platform.Tasks.Ajax.TaskReadingWrapper,X3Platform.Tasks",methodName:"Redirect"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M11','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M11','task.setFinished','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"SetFinished"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M12','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M12','task.setUsersFinished','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"SetUsersFinished"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M13','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M13','task.archive','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"Archive"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E01-M14','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E01-M14','task.removeUnfinishedWorkItems','','generic','{className:"X3Platform.Tasks.Ajax.TaskWrapper,X3Platform.Tasks",methodName:"RemoveUnfinishedWorkItems"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M01','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M01','task.workItem.query','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M02','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M02','task.workItem.findAllByReceiverId','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"FindAllByReceiverId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M09','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M09','task.workItem.setRead','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"SetRead"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M10','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M10','task.workItem.setStatus','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"SetStatus"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M11','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M11','task.workItem.setFinished','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"SetFinished"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M12','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M12','task.workItem.setUnfinished','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"SetUnfinished"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E02-M13','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E02-M13','task.workItem.getUnfinishedQuantities','','generic','{className:"X3Platform.Tasks.Ajax.TaskReceiverWrapper,X3Platform.Tasks",methodName:"GetUnfinishedQuantities"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E03-M01','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E03-M01','task.historyItem.query','','generic','{className:"X3Platform.Tasks.Ajax.TaskHistoryWrapper,X3Platform.Tasks",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E03-M06','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E03-M06','task.historyItem.delete','','generic','{className:"X3Platform.Tasks.Ajax.TaskHistoryWrapper,X3Platform.Tasks",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E03-M10','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E03-M10','task.historyItem.redirect','','generic','{className:"X3Platform.Tasks.Ajax.TaskHistoryWrapper,X3Platform.Tasks",methodName:"Redirect"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E04-M01','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E04-M01','task.waitingItem.query','','generic','{className:"X3Platform.Tasks.Ajax.TimingTaskReceiverWrapper,X3Platform.Tasks",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E04-M06','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E04-M06','task.waitingItem.delete','','generic','{className:"X3Platform.Tasks.Ajax.TimingTaskReceiverWrapper,X3Platform.Tasks",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E05-M01','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E05-M01','task.category.query','','generic','{className:"X3Platform.Tasks.Ajax.TaskCategoryWrapper,X3Platform.Tasks",methodName:"GetPages"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E05-M02','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E05-M02','task.category.findAllByReceiverId','','generic','{className:"X3Platform.Tasks.Ajax.TaskCategoryWrapper,X3Platform.Tasks",methodName:"FindAllByReceiverId"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E05-M03','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E05-M03','task.category.findOne','','generic','{className:"X3Platform.Tasks.Ajax.TaskCategoryWrapper,X3Platform.Tasks",methodName:"FindOne"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E05-M04','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E05-M04','task.category.create','','generic','{className:"X3Platform.Tasks.Ajax.TaskCategoryWrapper,X3Platform.Tasks",methodName:"CreateNewObject"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E05-M05','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E05-M05','task.category.save','','generic','{className:"X3Platform.Tasks.Ajax.TaskCategoryWrapper,X3Platform.Tasks",methodName:"Save"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
INSERT INTO `tb_Application_Method` (`Id`,`ApplicationId`,`Code`,`Name`,`Description`,`Type`,`Options`,`EffectScope`,`Version`,`OrderId`,`Status`,`Remark`,`UpdateDate`,`CreateDate`) VALUES ('01-09-E05-M06','1f3e1dec-c411-44e0-82cf-ae4447a36769','01-09-E05-M06','task.category.delete','','generic','{className:"X3Platform.Tasks.Ajax.TaskCategoryWrapper,X3Platform.Tasks",methodName:"Delete"}',1,1,'10001',1,'','2000-01-01','2000-01-01');
