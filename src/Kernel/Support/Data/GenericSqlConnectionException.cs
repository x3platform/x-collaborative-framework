namespace X3Platform.Data
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>数据库连接异常</summary>
    public class GenericSqlConnectionException : Exception
    {
        #region 构造函数:GenericSqlConnectionException(string message)
        public GenericSqlConnectionException(string message)
            : base(message)
        {
        }
        #endregion

    }
}