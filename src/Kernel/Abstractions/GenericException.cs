namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Text;

    using X3Platform.Util;
    #endregion

    /// <summary>通常的异常信息</summary>
    [Serializable]
    public class GenericException : Exception
    {
        /// <summary>是否成功执行 0 失败 1 成功</summary>
        private int success = 0;

        /// <summary>返回的代码</summary>
        private int returnCode = -1;

        #region 构造函数:GenericException(string message)
        /// <summary>构造函数</summary>
        /// <param name="message">消息</param>
        public GenericException(string message)
            : base(message)
        {
        }
        #endregion

        #region 构造函数:GenericException(int returnCode, string message)
        /// <summary>构造函数</summary>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="message">消息</param>
        public GenericException(int returnCode, string message)
            : base(message)
        {
            this.success = 1;
            this.returnCode = returnCode;
        }
        #endregion

        #region 构造函数:GenericException(int success, int returnCode, string message)
        /// <summary>构造函数</summary>
        /// <param name="success">是否成功执行 0 失败 1 成功</param>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="message">消息</param>
        public GenericException(int success, int returnCode, string message)
            : base(message)
        {
            this.success = success;
            this.returnCode = returnCode;
        }
        #endregion

        #region 构造函数:GenericException(int returnCode, Exception innerException)
        /// <summary>构造函数</summary>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="innerException">内部异常</param>
        public GenericException(int returnCode, Exception innerException)
            : base(innerException.Message, innerException)
        {
            this.success = 0;
            this.returnCode = returnCode;
        }
        #endregion

        #region 构造函数:GenericException(int success, int returnCode, Exception innerException)
        /// <summary>构造函数</summary>
        /// <param name="success">是否成功执行 0 失败 1 成功</param>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="innerException">内部异常</param>
        public GenericException(int success, int returnCode, Exception innerException)
            : base(innerException.Message, innerException)
        {
            this.success = success;
            this.returnCode = returnCode;
        }
        #endregion

        #region 函数:ToString()
        /// <summary>转换为字符串</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }
        #endregion

        #region 函数:ToString(bool nobrace)
        /// <summary>转换为字符串</summary>
        /// <param name="nobrace">对象不包含最外面的大括号</param>
        /// <returns></returns>
        public string ToString(bool nobrace)
        {
            StringBuilder outString = new StringBuilder();

            if (!nobrace)
            {
                outString.Append("{");
            }

            outString.Append("\"message\":{");
            outString.AppendFormat("\"returnCode\":\"{0}\",", this.returnCode);
            outString.AppendFormat("\"value\":\"{0}\"", StringHelper.ToSafeJson(this.InnerException == null ? this.Message : this.InnerException.Message));
            outString.Append("},");

            // 是否成功执行
            outString.Append("\"success\":" + this.success + ",");
            if (this.success == 0)
            {
                // 如果是执行失败输出异常信息
                outString.AppendFormat("\"msg\":\"{0}\"", StringHelper.ToSafeJson(this.InnerException == null ? this.Message : this.InnerException.ToString()));
            }
            else
            {
                // 如果是执行成功输出 success
                outString.AppendFormat("\"msg\":\"success\"");
            }

            if (!nobrace)
            {
                outString.Append("}");
            }

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 静态方法
        // -------------------------------------------------------

        /// <summary>序列化为 JOSN 格式消息</summary>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static string Serialize(int returnCode, string message)
        {
            return new GenericException(returnCode, message).ToString();
        }

        /// <summary>序列化为 JOSN 格式消息</summary>
        /// <param name="success">是否成功执行 0 失败 1 成功</param>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static string Serialize(int success, int returnCode, string message)
        {
            return new GenericException(success, returnCode, message).ToString();
        }

        /// <summary>序列化为 JOSN 格式消息</summary>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="message">消息</param>
        /// <param name="nobrace">对象不包含最外面的大括号</param>
        /// <returns></returns>
        public static string Serialize(int returnCode, string message, bool nobrace)
        {
            return new GenericException(returnCode, message).ToString(nobrace);
        }

        /// <summary>序列化为 JOSN 格式消息</summary>
        /// <param name="success">是否成功执行 0 失败 1 成功</param>
        /// <param name="returnCode">返回的异常代码</param>
        /// <param name="message">消息</param>
        /// <param name="nobrace">对象不包含最外面的大括号</param>
        /// <returns></returns>
        public static string Serialize(int success, int returnCode, string message, bool nobrace)
        {
            return new GenericException(success, returnCode, message).ToString(nobrace);
        }
    }
}