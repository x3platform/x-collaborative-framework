namespace X3Platform.Storages
{
    using System;

    /// <summary>方法接口</summary>
    public interface IStorageAdapter
    {
        /// <summary>执行</summary>
        /// <returns></returns>
        object Execute(string commandText, string[] args);
    }
}