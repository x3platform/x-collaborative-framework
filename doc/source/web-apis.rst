应用接口
===============

通用规则
------------------------------------------------------------

API 地址规则
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
规则: ``http://{api-server}/api/{api-name}`` {api-name} 为接口名称, 把中间的**.替换成/**即可.

比如 bigdb.static.search - 检索接口, 对应的地址为 http://{api-server}/api/bigdb/static/search

AccessToken 授权
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
部分接口访问需要相关的授权验证. 相关接口 参考 `connect.oauth.*`_

系统在 OAuth 2.0 接口验证授权后, 获得 AccessToken, 需要加在接口末尾.

比如 http://{api-server}/api/bigdb/static/search?accessToken={accessToken}

HTTP 请求方式
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
默认 HTTP 请求方式: ``POST``

Content-Type 格式
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
请求内容如果是 ``JSON`` 格式的文本需要设置 ``Content-Type: application/json``

错误信息格式
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
.. code-block:: javascript

   {
      "errcode": "1",
      "errmsg": "error msg" 
   }

errcode = 0, 默认情况下表示执行成功.
 
connect.oauth.*
------------------------------------------------------------
OAuth 2.0 接口

connect.oauth2.authorize - 身份验证 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

**输出参数**

connect.oauth2.token - 获取访问令牌 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

获取当前用户信息 connect.oauth2.me
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

刷新访问令牌 connect.oauth2.refresh
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

**输出参数**

bigdb.static.*
------------------------------------------------------------
静态大库接口

bigdb.static.config - 配置
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

.. code-block:: javascript

   {
      "callback":"http://xxx/callback"
   }

**输出参数**

.. code-block:: javascript

   {"errcode":0, "errmsg": "ok"}


bigdb.static.modeling - 建模  
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

.. code-block:: javascript

   {
      // 应用唯一标识
      "app_key": "1",
      "bank_code": "4",
      "bank_name": "abc",
      "sources": [
      {
         "type": "io_oracle",
         "code": "0",
         "connstr": "ip:127.0.0.1;port:10;server:orcl;username:cloudwalk;passwd:1",
         "provider": "Oracle",
         "table_name": "",
         "field_id": "",
         "field_data": "",
         "field_status": "status",
         "field_attrs": "age,gender",
         "field_conds": [
         {
            "name": "gender",
            "type": 0,
            "record ": "gender"
         },
         {
            "name": "birthday",
            "type": 1,
            "record ": "birthday"
         }]
      }],
      "target": {
         "type": "loc_feature",
         "code": "1"
      }
   }

**输出参数**

.. code-block:: javascript

   { "task_id": "123" }

bigdb.static.loading - 加载  
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

.. code-block:: javascript

   {
      "app_key": "1",
      "bank_code": "4"
   }

**输出参数**

.. code-block:: javascript

   { "task_id": "123" }

bigdb.static.search - 检索
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

.. code-block:: javascript

   { 
      // 应用唯一标识
      "app_key": "00000",
      // 人像库代码
      "bank_codes": "0",
      "base64": "base64",
      "top": 10,
      "threshold": "0.85"
      "birthday": "1980|1990",
      "gender": "Male"
      // 更多过滤条件...
   }

**输出参数**

.. code-block:: javascript

   { 
      "timespan": 3,
      "list": [
      {
           "bank_code": "123",
           "face_id": "123",
           "score": 73,
           "quality_score": 67,
           "age": "123",
           "gender": "男"
           "…": "其他属性"
      }
      // 更多项...
      ]
   }

bigdb.static.bank - 人像库信息  
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

.. code-block:: javascript

   {
      // 应用唯一编码
      "app_key": "1",
      // 人像库代码
      "bank_code": "4"
   }

**输出参数**

.. code-block:: javascript

   { 
      // 状态信息
      // 0:初始 1:就绪 2:建模中 3:建模完成 4:加载中 5:加载完成 6:异常
      "status": "0" 
   }

bigdb.static.bank.delete - 删除人像库信息  
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

**输入参数** 

.. code-block:: javascript

   {
      // 应用唯一编码
      "app_key": "1",
      // 人像库代码
      "bank_code": "4"
   }

**输出参数**

.. code-block:: javascript

   { "errcode": 0,	"errmsg": "ok" }

   
bigdb.static.task - 任务信息 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

HTTP请求方式: ``GET``

**输入参数** 

.. table::

   ================  ================  ================
   名称              数据类型           描述 
   ================  ================  ================ 
   app_key           字符串             App Key
   task_id           字符串             任务唯一标识
   ================  ================  ================ 

**输出参数**

.. code-block:: javascript

   {
      // 状态
      // 0 准备中 1 已就绪 2 执行中 3 已完成 4 被暂停 5 被停止 6 异常状态      
      "status": "3"
      // 进度 注:底层引擎处理的数量信息
      "progress": "50",
      // 总量 注:目前只有在建模任务中才有总量信息
      "sum": "10000",
      // 开始时间
      "starttime": "1970-01-01 00:00:00",
      // 结束时间
      "endtime": "1970-01-01 00:00:00",
      // 最后一次状态切换时间
      "statustime": "1970-01-01 00:00:00",
      // 描述信息
      "desc": "描述信息",
      // 异常代码
      "excode": "1",
      // 异常消息
      "exmsg": "异常信息"
   }

bigdb.static.face.add - 新增人像信息
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
根据一段SQL语句, 批量增加人像语句

**输入参数** 

.. code-block:: javascript 

   { 
      // 应用唯一编码
      "app_key": "1",
      // 人像库代码
      "bank_code":"4",
      // 需要增量建模数据来源SQL
      "filter_sql": "SELECT * FROM view_Phtoto WHERE Id between 100 an 200",
      // 异步执行 0 同步执行 1 异步执行 返回 task_id
      "async":"0"
   }

**输出参数**

.. code-block:: javascript 

   { "errcode": 0,	"errmsg": "ok" }

bigdb.static.face.compare - 人像比对
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
1 比 1 人像比对

**输入参数** 

.. code-block:: javascript 

   { 
      // 应用唯一编码
      "app_key": "1",
      // 人像信息 1
      "base64_1": "{Base64 格式字符串}",
      // 人像信息 2
      "base64_2": "{Base64 格式字符串}"
   }

**输出参数**

.. code-block:: javascript 

   {
      // 比对时间
      "timespan": 3,
      // 比对结果, 范围 0 ~ 1
      "score": "0.9415"
   }

bigdb.static.face.detect - 人脸检测 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
输入一张图片, 输出所有人像信息

**输入参数** 

.. table::

   ================  ================  ================
   名称              数据类型           描述 
   ================  ================  ================ 
   image             字符串             一个由字符组成的不可更改的有串行。
   ================  ================  ================ 

**输出参数**

.. code-block:: javascript 

   { 
      "data": [
      {
         // Base64 格式字符串图片
         "image": "base64",
         // 原图人脸框左上角坐标 x
         "x": "123",
         // 原图人脸框左上角坐标 y
         "y": 73,
         // 人脸框宽度
         "width": 67,
         // 人脸框高度
         "height": "123"
      }
      // 更多项...
      ]
   }
