-- 删除成员信息
DELETE FROM tb_Member where AccountId NOT IN (SELECT Id FROM tb_Account);
-- 删除应用事件
DELETE FROM tb_Application_Event;
-- 删除用户日志
DELETE FROM tb_Account_Log;
-- 删除会话日志
DELETE FROM tb_AccountCache;
-- 删除接口访问令牌
DELETE FROM tb_Connect_AccessToken;
-- 删除接口调用日志
DELETE FROM tb_Connect_Call;
-- 删除短信日志
DELETE FROM tb_SMS;
