namespace X3Platform.CacheBuffer
{
    using System;

    /// <summary>缓存操作异常</summary>
    [Serializable]
    public class CacheBufferException : Exception
    {
        public CacheBufferException(string message)
            : base(message)
        {
        }

        public CacheBufferException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}