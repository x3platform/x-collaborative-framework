// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System.Web;

namespace X3Platform
{
    /// <summary>���л����Ĵ������ӿ�</summary>
    public interface IContextWrapper
    {
        #region ����:ProcessRequest(HttpContext context)
        /// <summary>��������</summary>
        /// <param name="context">�����Ļ���</param>
        void ProcessRequest(HttpContext context);
        #endregion
    }
}
