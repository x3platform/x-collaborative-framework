日程运维
===============

更改数据库密码
------------------------------------------------------------

备份配置数据库
------------------------------------------------------------

**MySQL版**

执行脚本 ``<Drive>\cloudwalk\backup\scripts\database\database-backup-mysql.cmd``

根据提示输入 MariaDB 的超级管理员密码.

执行成功后, 可以在 ``<Drive>\cloudwalk\backup\files\`` 目录下, 找到相关的备份文件.

备份 cn.cloudwalk.bigdb.80 站点
------------------------------------------------------------

执行脚本 ``<Drive>\cloudwalk\web\cn.cloudwalk.bigdb.80\backup\backup.cmd``

执行成功后, 可以在 ``<Drive>\cloudwalk\web\cn.cloudwalk.bigdb.80\backup\`` 目录下, 找到 cn.cloudwalk.bigdb.80_vx.x.x.zip 类似的备份文件.
