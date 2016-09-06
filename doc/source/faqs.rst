常见问题
===============

如何查看日志
--------------------------------

日志存储位置 ``<Drive>\cloudwalk\web\cn.cloudwalk.bigdb.80\log\``

Web 方式快速查看最新日志

URL 路径格式 ``http://bigdb.cloudwalk.cn/log/YYYY/MM/DD.txt``

例如: http://bigdb.cloudwalk.cn/log/2015/01/01.txt

failed to create group, no station is specified
------------------------------------------------
创建分组失败, 可能是 Workstaion 未连上成功

failed to use io, because of invalid group
------------------------------------------------
错误的分组信息

MySQL 端口被占用
------------------------------------------------

修改 ``<Drive>\\opt\\MariaDb\\my.ini`` 配置文件

修改 ``<Drive>\\cloudwalk\\web\\cn.cloudwalk.bigdb.80\\web.config`` 配置文件

修改 ``<Drive>\\cloudwalk\\services\\cn.cloudwalk.bigdb.daemon.host\\X3Platform.Services.Host.exe.config`` 配置文件
