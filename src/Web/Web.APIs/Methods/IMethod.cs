namespace X3Platform.Web.APIs.Methods
{
    using System;

    /// <summary>方法接口</summary>
    public interface IMethod
    {
        /// <summary>验证必填参数</summary>
        void Validate();

        /// <summary>执行</summary>
        /// <returns></returns>
        object Execute();
    }
}