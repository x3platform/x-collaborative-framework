// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :SecurityClassMessageSink.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System.Runtime.Remoting.Messaging;
using System.ComponentModel;

namespace X3Platform.Security
{
    /// <summary>安全类消息装置</summary>
    public class SecurityClassMessageSink : IMessageSink
    {
        private IMessageSink _NextSink;  //保存下一个接收器

        /// <summary></summary>
        /// <param name="nextSink"></param>
        public SecurityClassMessageSink(IMessageSink nextSink)
        {
            this._NextSink = nextSink;
        }

        #region IMessageSink 成员

        /// <summary>
        /// 
        /// </summary>
        public IMessageSink NextSink { get { return this._NextSink; } }

        //IMessageSink的接口方法，当消息传递的时被调用
        //
        /// <summary></summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            //拦截消息，做前处理
            Preprocess(msg);
            //传递消息给下一个接收器
            IMessage retMsg = this._NextSink.SyncProcessMessage(msg);

            return retMsg;
        }

        //IMessageSink接口方法，用于异步处理，权限验证中不需要异步所以返回null
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// 调用前处理方法
        /// </summary>
        /// <param name="msg"></param>
        protected void Preprocess(IMessage msg)
        {
            ValidAuthorityInfos(msg);
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="msg"></param>
        protected virtual void ValidAuthorityInfos(IMessage message)
        {
            IMethodCallMessage methodCallMessage = message as IMethodCallMessage;

            SecurityMethodAttribute[] mustAuthorities;

            //取得所有验证属性
            mustAuthorities = methodCallMessage.MethodBase.GetCustomAttributes(typeof(SecurityMethodAttribute), false) as SecurityMethodAttribute[];

            if (mustAuthorities == null || mustAuthorities.Length == 0)
                return;

            foreach (SecurityMethodAttribute authority in mustAuthorities)
            {
                //验证权限
                if (!authority.Check(methodCallMessage))
                {
                    //验证未通过则抛出异常
                    throw new SecurityException(authority.LoginName, methodCallMessage.MethodName, "Oops, you don't have authority.");
                }
            }
        }
    }
}
