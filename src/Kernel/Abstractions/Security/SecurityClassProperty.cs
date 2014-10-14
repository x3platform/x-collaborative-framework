namespace X3Platform.Security
{
    using System;

    using System.Runtime.Remoting.Contexts;
    using System.Runtime.Remoting.Messaging;

    public class SecurityClassProperty : IContextProperty, IContributeObjectSink
    {
        /// <summary></summary>
        public SecurityClassProperty()
        {

        }

        //
        // IContextProperty 成员
        //
        public string Name
        {
            get { return "ContextAuthorityInfoValid"; }
        }

        public void Freeze(Context newContext)
        {
        }

        public bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        //
        // IContributeObjectSink 成员
        //
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new SecurityClassMessageSink(nextSink);
        }
    }
}
