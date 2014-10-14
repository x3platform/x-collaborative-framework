using System;
using System.Runtime.Remoting.Messaging;

using X3Platform.Membership;

namespace X3Platform.Security
{
    /// <summary>用于验证方法所需要的权限</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class SecurityMethodAttribute : Attribute
    {
        /// <summary>验证权限方法，在 ContextAuthorityInfoValidSink 中调用此方法进行验证.</summary>
        /// <param name="methodCallMessage">方法调用消息</param>
        /// <returns></returns>
        public abstract bool Check(IMethodCallMessage methodCallMessage);

        /// <summary>用户信息，可以自行编写取得当前用户信息的方法</summary>
        protected abstract IAccountInfo Account { get; }

        /// <summary>登录名</summary>
        public string LoginName
        {
            get { return Account.LoginName; }
        }
    }
}
