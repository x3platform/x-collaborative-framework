namespace X3Platform.Pooling
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>这个接口用来产生 Resource Pool 管理的资源</summary>
    /// <typeparam name="T"></typeparam>
    public interface IResourceObject<T> where T : class, IDisposable
    {
        /// <summary>获得资源</summary>
        /// <returns>成功则返回资源对象，否则返回null</returns>
        T Request();
    }
}
