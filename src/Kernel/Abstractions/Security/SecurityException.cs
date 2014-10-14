using System;


namespace X3Platform.Security
{
    /// <summary>安全异常</summary>
    public class SecurityException : Exception
    {
        /// <summary></summary>
        /// <param name="loginName">登录名</param>
        /// <param name="method">方法</param>
        /// <param name="message">消息</param>
        public SecurityException(string loginName, string method, string message)
            : base(string.Format("用户[{0}]访问系统的[{1}]方法被拒绝可能原因为[{2}]", loginName, method, message))
        {
        }
    }
}
