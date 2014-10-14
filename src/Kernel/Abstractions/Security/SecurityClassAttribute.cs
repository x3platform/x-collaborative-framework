using System;

using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;

namespace X3Platform.Security
{
    /// <summary>安全类</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SecurityClassAttribute : ContextAttribute
    {
        /// <summary>构造函数</summary>
        public SecurityClassAttribute()
            : base("SecurityClass")
        {
        }

        /// <summary>将当前上下文属性添加到给定的消息.</summary>
        /// <param name="ctorMsg"></param>
        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            //实例化一个 ContextAuthorityInfoValidProperty 添加到上下文属性列表中
            ctorMsg.ContextProperties.Add(new SecurityClassProperty());
        }
    }
}