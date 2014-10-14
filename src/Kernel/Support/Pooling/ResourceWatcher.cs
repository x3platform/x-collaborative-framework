using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Pooling
{
    /// <summary>资源标签</summary>
    /// <typeparam name="T"></typeparam>
    internal class ResourceWatcher<T>
    {
        private T m_Resource;

        /// <summary>资源</summary>
        internal T Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; }
        }

        private bool inUse;

        /// <summary>是否正在使用</summary>
        internal bool InUse
        {
            get { return inUse; }
            set { inUse = value; }
        }

        public ResourceWatcher(T message, bool inUse)
        {
            Resource = message;
            InUse = inUse;
        }
    }
}
