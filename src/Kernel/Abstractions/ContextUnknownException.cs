namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Text;

    using X3Platform.Util;
    #endregion

    /// <summary>运行环境的未知异常</summary>
    [Serializable]
    public class ContextUnknownException : Exception
    {
        /// <summary>上下文未知的异常</summary>
        private string key = "contextUnknownException";

        /// <summary>返回的代码</summary>
        private int returnCode = 1;

        #region 构造函数:ContextUnknownException(string message)
        public ContextUnknownException(string message)
            : base(message)
        {
        }
        #endregion

        #region 构造函数:ContextUnknownException(string key, int returnCode, Exception innerException)
        public ContextUnknownException(string key, int returnCode, Exception innerException)
            : base(innerException.Message, innerException)
        {
            this.key = key;
            this.returnCode = returnCode;
        }
        #endregion

        #region 函数:ToString()
        /// <summary>转换为字符串</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{\"" + key + "\":{");
            outString.AppendFormat("\"returnCode\":\"{0}\",", this.returnCode);
            outString.AppendFormat("\"category\":\"{0}\",", "exception");
            outString.AppendFormat("\"value\":\"{0}\",", StringHelper.ToSafeJson(this.InnerException.Message));
            outString.AppendFormat("\"description\":\"{0}\"", StringHelper.ToSafeJson(this.InnerException.ToString()));
            outString.Append("}}");

            return outString.ToString();
        }
        #endregion
    }
}