#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IContext
//
// Description  :���л����ӿ�
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

using System;

namespace X3Platform
{
    /// <summary>���л����ӿ�</summary>
    public interface IContext
	{
        #region 属性:����
        /// <summary>����</summary>
        string Name { get; }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        void Reload();
        #endregion
	}
}
