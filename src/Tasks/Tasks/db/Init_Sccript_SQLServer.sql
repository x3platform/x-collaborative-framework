SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tb_Task](
	[Id] [nvarchar](36) NOT NULL,
	[ApplicationId] [nvarchar](36) NULL,
	[TaskCode] [nvarchar](120) NULL,
	[Type] [nvarchar](20) NULL,
	[Title] [nvarchar](500) NULL,
	[Content] [nvarchar](800) NULL,
	[Tags] [nvarchar](100) NULL,
	[SenderId] [nvarchar](36) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_tb_Task] PRIMARY KEY CLUSTERED 
 (
	[Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- ===========================================================
-- 任务工作项视图
-- ===========================================================

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_Task_WorkItem]'))
	DROP VIEW [dbo].[view_Task_WorkItem]
GO

CREATE VIEW [dbo].[view_Task_WorkItem]
AS
SELECT 
	T2.[Id], 
	T2.[ApplicationId], 
	T2.[TaskCode], 
	T2.[Type], 
	T2.[Title], 
	T2.[Content], 
	T2.[Tags], 
	T2.[SenderId], 
	T1.[ReceiverId], 
	T1.[IsRead], 
	T1.[Status], 
	T1.[FinishTime], 
	T2.[CreateDate]
FROM
	dbo.tb_Task_Receiver AS T1 LEFT JOIN dbo.tb_Task AS T2 ON T2.Id = T1.TaskId

GO

-- proc_SendTaskWaiting

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_SendTaskWaiting]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[proc_SendTaskWaiting]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[proc_SendTaskWaiting]
AS
BEGIN
  DECLARE @todayNo int

  SET @todayNo = datepart(dw,CURRENT_TIMESTAMP)   


 
  Create table #taskId
  (Id varchar(50))

  --今天是周五
  if (@todayNo=6)
  begin
    insert into #taskId(Id)
    select Id from Base_HrAlert_Task,Base_HrAlert_TaskReceiver
    where Base_HrAlert_Task.Id = Base_HrAlert_TaskReceiver.taskId
    and Dateadd(dd,-3,alertTime) = dbo.ToShortDate(CURRENT_TIMESTAMP)
    and isSend = '0'

    insert into #taskId(Id)
    select Id from Base_HrAlert_Task,Base_HrAlert_TaskReceiver
    where Base_HrAlert_Task.Id = Base_HrAlert_TaskReceiver.taskId
    and Dateadd(dd,-2,alertTime) = dbo.ToShortDate(CURRENT_TIMESTAMP)
    and isSend = '0'

    insert into #taskId(Id)
    select Id from Base_HrAlert_Task,Base_HrAlert_TaskReceiver
    where Base_HrAlert_Task.Id = Base_HrAlert_TaskReceiver.taskId
    and Dateadd(dd,-1,alertTime) = dbo.ToShortDate(CURRENT_TIMESTAMP)
    and isSend = '0'
  END
  ELSE
  BEGIN
    insert into #taskId(Id)
    select Id from Base_HrAlert_Task,Base_HrAlert_TaskReceiver
    where Base_HrAlert_Task.Id = Base_HrAlert_TaskReceiver.taskId
    and Dateadd(dd,-1,alertTime) = dbo.ToShortDate(CURRENT_TIMESTAMP)
    and isSend = '0'
  END

  insert into dbo.Task(ID,TaskCode,ThirdPartyAccountId,[Type],Title,Url,Category,SenderId)
  select ID,TaskCode,ThirdPartyAccountId,[Type],Title,Url,Category,SenderId from Base_HrAlert_Task
  where Id in (select distinct Id from #taskId)

  insert into dbo.TaskReceiver(TaskID,ReceiverId,status,isRead,finishTime)
  select TaskId,ReceiverId,'0','0','2001-1-1' from Base_HrAlert_TaskReceiver
  where TaskId in (select distinct Id from #taskId)

  UPDATE Base_HrAlert_TaskReceiver Set isSend = '1'
  WHERE TaskId in (SELECT DISTINCT Id from #taskId)

  DROP TABLE #taskId
END

-- job_SendTaskWaiting

USE [msdb]
GO

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0

-- 设置分类信息
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[X3Platform.longfor]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[X3Platform.longfor]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)

-- 常规
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'job_SendTaskWaiting', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'发送定时任务提醒。', 
		@category_name=N'[X3Platform.longfor]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

/****** Object:  Step [job_AlertToTask]    Script Date: 09/14/2010 10:16:58 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'发送定时任务提醒', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC [LH_TASK].[dbo].[Up_AlertToTask]', 
		@database_name=N'X3Platform.longfor', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

-- 设置执行的周期计划
-- @@freq_type = 4 表示每天 | 8 表示每周 | 16 表示每月
-- @active_start_time = 40000 表示4:00 
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'每天4:00执行', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20100101, 
		@active_end_date=99991231, 
		@active_start_time=40000, 
		@active_end_time=235959, 
		@schedule_uid=N'472faeba-0e32-4d68-9fea-85d59d1e6d55'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

-- 设置目标服务器
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION

GOTO EndSave


QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION

EndSave:

GO