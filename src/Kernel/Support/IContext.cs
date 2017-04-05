using System;

namespace X3Platform
{
    /// <summary>上下文接口</summary>
    public interface IContext
	{
        #region 属性:Name
        /// <summary>名称</summary>
        string Name { get; }
        #endregion

        #region 属性:Reload()
        /// <summary>重新加载</summary>
        void Reload();
        #endregion
	}
}
