namespace X3Platform.CacheBuffer
{
    using System;

    /// <summary>缓存操作异常</summary>
    [Serializable]
    public class CachingException : Exception
    {
        public CachingException(string message)
            : base(message)
        {
        }

        public CachingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}