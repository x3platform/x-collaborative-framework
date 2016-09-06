新手入门
===============

系统配置

================================  ================================  
名称                              描述           
================================  ================================   
操作系统要求       				  Windows 2008 R2 SP1  
.NET Framework                    4.6
IIS                               7.0
MySQL                             MySQL 5.5 或者 MariaDB 10.0.20
================================  ================================  
 
下载
----------------

BigDb 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

下载地址 http://192.168.10.222/dist/bigdb-v2.1.0.zip

可选组件
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

================================  ================================  
名称                              下载           
================================  ================================  
MariaDB                           http://192.168.10.222/dist/mariadb-10.0.20-winx64.zip
Oracle Client                     http://192.168.10.222/dist/oracle-instantclient-11g-x64.zip
Git                               http://192.168.10.222/dist/Git-2.8.3-64-bit.exe
================================  ================================

安装
-------------
手工安装步骤适用于大型分布式环境, **不适用于默认安装包安装**.

安装 MySQL 环境, 参考 `安装 MariaDB (可选)`_ , 如果操作系统已安装 MySQL 环境, 可以忽略此步骤.

安装 IIS 环境, 参考 `安装 IIS (可选)`_ , 如果操作系统已安装 MySQL 环境, 可以忽略此步骤.

安装 BigDb
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Zip 方式安装 
::

	http://192.168.10.222/dist/bigdb-v2.1.0.zip

Git 方式安装

如果没有 Git 环境, 参考 `安装 Git (可选)`_
::

	git clone https://yoshow.visualstudio.com/DefaultCollection/_git/cloudwalk

**安装位置**
::

	<Drive>\cloudwalk\

文件夹结构说明

:: 

	├─ backup\ 
	├─ license\ 
	├─ services\ 
	├─ tools\ 
	├─ web\ 
	└─ workstation\ - workstaion.exe 安装目录 

**注:以下步骤需要以管理员权限执行**

以下为手动安装步骤, 或者可以执行安装脚本 ``<Drive>\cloudwalk\bigdb-install.cmd``

**部署 bigdb.cloudwalk.cn 站点**

执行脚本 ``<Drive>\cloudwalk\web\cn.cloudwalk.bigdb.80-deployment.cmd``

**部署守护进程**

执行脚本 ``<Drive>\cloudwalk\services\cn.cloudwalk.bigdb.daemon.host\service.install.cmd``

安装 Git (可选)
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
如果操作系统已安装 Git 环境, 可以忽略此步骤.

下载地址 https://git-scm.com/downloads

内网地址 http://192.168.10.222/dist/Git-2.8.3-64-bit.exe

**安装位置:**
::

	<Drive>\opt\Git\

验证是否安装成功  
::

	Win+R 输入 cmd, 打开控制台窗口 git --version, 看到版本信息表示安装成功.

安装 IIS (可选)
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
如果操作系统已安装 IIS 环境, 可以忽略此步骤.

执行脚本 
::

	<Drive>\cloudwalk\web\web-host-deployment.cmd

验证是否安装成功
::

	打开浏览器, 输入 http://localhost/, 看到欢迎界面表示成功.

安装 MariaDB (可选)
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
如果操作系统已安装 MySQL 环境, 可以忽略此步骤.

目前系统推荐的 MySQL 环境为 MariaDB 10.0.20

内网地址 http://192.168.10.222/dist/mariadb-10.0.20-winx64.zip

**安装位置**
::

	<Drive>\opt\MariaDB\

**自动安装脚本**

执行脚本 ``<Drive>\opt\MariaDB\mariadb-install.cmd``

配置 my.ini 文件 位置 ``<Drive>\opt\MariaDB\my.ini``, 
根据实际情况将其中的相关路径配置为实际的路径.

验证是否安装成功
::

	Win+R 输入 cmd, 打开控制台窗口 mysql --version, 看到版本信息表示安装成功.

**以服务方式运行数据库**
::

	Win+R 输入 cmd, 打开控制台窗口
	执行 mysqld --install MariaDB && net start MariaDB
	
安装 Oracle Client (可选)
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
如果机器上已经有 Oracle 客户端环境, 可以忽略此步骤.

目前系统推荐的 Oracle Client 环境为 11.2.0.1

内网地址 http://192.168.10.222/dist/oracle-instantclient-11g-x64.zip

**安装位置**
::

  <Drive>\\opt\\Oracle\\64-bit\\instantclient_11_2

配置 Path 环境变量
<Drive>\\opt\\Oracle\\64-bit\\instantclient_11_2 至系统环境

验证是否安装成功
Win+R 输入 cmd, 打开控制台窗口 sqlplus /nolog, 看到相关版本信息表示安装成功.

配置
----------------


配置参数
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
cn.cloudwalk.bigdb.80 可选的配置参数信息

================================  ================================  
名称                              描述           
================================  ================================
BigDb.EngineType                  引擎类型, =1 1.0版, =2 2.0版
BigDb.Port                        端口, 默认配置 8899
BigDb.CommandThreadTimeout        引擎命令线程超时时间, 单位:秒
BigDb.DbSegment                   数据库分段抓取的数量
BigDb.ValidAppKey                 有效的 AppKey, 根据配置的 AppKey 信息设置库的初始状态, 达到不同的环境管理不同的库
BigDb.NeedCompressedImageMinSize  需要压缩图片的最小大小, =0 表示所有图片不压缩 目前最大限制 64K, 单位:字节
BigDb.SearchImageExtensions       检索图片格式限制, 默认jpg png
BigDb.SearchImageMaxSize          检索图片最大大小限制, 目前最大限制 768KB, 单位:字节
BigDb.SearchListLimit             检索结果最大长度, 最大长度限制 1000
BigDb.DynamicCaptureId            动态抓拍库唯一标识
BigDb.DynamicCaptureMaxDays       动态抓拍最大天数
BigDb.FaceDetectMode              人脸检测状态 On 启用 Off 禁用(默认)
================================  ================================  

打开 <Drive>\\cloudwalk\\web\\cn.cloudwalk.bigdb.80\\web.config

.. code-block:: html
   :emphasize-lines: 4
   
	<configuration>
	  <kernel>
		<!-- 大库检索引擎 类型 -->
		<key name="BigDb.EngineType" value="2" />
		<!-- 大库检索引擎 临时图片保留时间 -->
		<key name="BigDb.TempImageRetentionTime" value="86400" />
		<!-- 大库检索引擎 端口 -->
		<key name="BigDb.Port" value="10090" />
		<!-- 大库检索引擎 引擎命令线程超时时间, 单位:秒 -->
		<key name="BigDb.CommandThreadTimeout" value="300" />
		<!-- 大库检索引擎 数据库分段抓取的数量 -->
		<key name="BigDb.DbSegment" value="100" />
		<!-- 大库检索引擎 需要压缩图片的最小大小, =0 表示所有图片不压缩 目前最大限制 64K, 单位:字节 -->
		<key name="BigDb.NeedCompressedImageMinSize" value="0" />
		<!-- 大库检索引擎 检索图片格式限制 -->
		<key name="BigDb.SearchImageExtensions" value=".jpg,.png" />
		<!-- 大库检索引擎 检索图片最大大小限制, 目前最大限制 64K, 单位:字节 64K=65535 256K=262144 768KB=786432 -->
		<key name="BigDb.SearchImageMaxSize" value="786432" />
		<!-- 大库检索引擎 检索结果最大长度, 最大长度限制 1000 -->
		<key name="BigDb.SearchListLimit" value="1000" />
		<!-- 大库检索引擎 动态抓拍库唯一标识 -->
		<key name="BigDb.DynamicCaptureId" value="1238" />
		<!-- 大库检索引擎 动态抓拍最大天数 -->
		<key name="BigDb.DynamicCaptureMaxDays" value="365" />
		<!-- 大库检索引擎 人脸检测状态 On 启用 Off 禁用(默认) -->
		<key name="BigDb.FaceDetectMode" value="On" />
	  </kernel>
	</configuration>

配置 WebSocket 日志
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: html
   :emphasize-lines: 5,11,15-28,45,53
   
	<configuration>
	  <logging>
		<logger name="cloudwalk-log" additivity="true" >
		  <level value="ALL"/>
		  <appender-ref ref="WebSocketAppender" />
		  <appender-ref ref="RollingFileAppender" />
		</logger>
		<root>
		  <!-- [Levels: ALL < TRACE < DEBUG < INFO < WARN < ERROR < FATAL < OFF] -->
		  <level value="INFO" />
		  <appender-ref ref="WebSocketAppender" />
		  <appender-ref ref="RollingFileAppender" />
		</root>
		<!-- 定义日志输出到 WebSocket -->
		<appender name="WebSocketAppender" type="Cloudwalk.BigDb.Logging.Appender.WebSocketAppender,Cloudwalk.BigDb.Compatibility">
		  <accountName value="services"/>
		  <socketUri value="ws://localhost:10089/"/>
		  <layout type="X3Platform.Logging.Layout.PatternLayout">
			<conversionPattern value="%message"/>
		  </layout>
		  <filter type="X3Platform.Logging.Filter.LoggerMatchFilter">
			<param name="LoggerToMatch" value="Cloudwalk.BigDb" />
		  </filter>
		  <filter type="X3Platform.Logging.Filter.LoggerMatchFilter">
			<param name="LoggerToMatch" value="RecogEngine.SearchEnginImpl" />
		  </filter>
		  <filter type="X3Platform.Logging.Filter.DenyAllFilter" />
		</appender>
		<!-- 定义日志输出到 本地文件  -->
		<appender name="RollingFileAppender" type="X3Platform.Logging.Appender.RollingFileAppender">
		  <!--定义文件存放位置-->
		  <file value="log" />
		  <appendToFile value="true" />
		  <encoding value="utf-8" />
		  <maximumFileSize value="2MB" />
		  <maxSizeRollBackups value="100" />
		  <staticLogFileName value="false" />
		  <datePattern value="\\yyyy\\MM\\dd&quot;.txt&quot;" />
		  <rollingStyle value="Composite" />
		  <layout type="X3Platform.Logging.Layout.PatternLayout">
			<conversionPattern value="%5level %date{yyyy-MM-dd HH:mm:ss.fff} thread: [%thread] source: [%logger] description: %message%n" />
		  </layout>
		</appender>
	  </logging>
	  <superSocket logFactory="WebSocketLogFactory" disablePerformanceDataCollector="true" >
		<servers>
		  <server name="WebSocketServer"
				  serverTypeName="WebSocketServer"
				  maxConnectionNumber="1000"
				  maxRequestLength="4096"
				  sendTimeOut="300000">
			<listeners>
			  <add ip="Any" port="10089" />
			</listeners>
		  </server>
		</servers>
		<serverTypes>
		  <add name="WebSocketServer"
			   type="SuperSocket.WebSocket.WebSocketServer,SuperSocket.WebSocket" />
		</serverTypes>
		<logFactories>
		  <add name="WebSocketLogFactory"
			   type="Cloudwalk.BigDb.WebSockets.Logging.WebSocketLogFactory, Cloudwalk.BigDb.WebSockets" />
		</logFactories>
	  </superSocket>
	</configuration>

操作
----------------

配置
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
导航 ``首页 - 人像库管理``

建模
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
导航 ``首页 - 人像库管理``

选择已就绪的人像库记录, 点击操作栏的``运行``按钮

加载
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
导航 ``首页 - 人像库管理``

选择已建模的人像库记录, 点击操作栏的``运行``按钮

搜索
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
导航 ``首页 - 人像搜索``

上传人像图片, 设置过滤条件, 点击``搜索``按钮
