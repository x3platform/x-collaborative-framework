using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace X3Platform.Pooling
{
    //
    // 数据库连接资源提供器(示例代码)
    // 

    /// <summary>资源提供器</summary>
    public class DbConnectionResourceProvider : IResourceObject<IDbConnection>
    {
        #region 函数:Request()
        public IDbConnection Request()
        {
            IDbConnection conn = new SqlConnection();

            return conn;
        }
        #endregion
    }
}
