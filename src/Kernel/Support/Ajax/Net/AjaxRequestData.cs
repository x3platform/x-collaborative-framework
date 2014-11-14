// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxRequestData.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Net;
using System.Collections.Generic;

namespace X3Platform.Ajax.Net
{
    /// <summary>Ajax请求的数据</summary>
    public class AjaxRequestData
    {
        #region 属性:LoginName
        private string m_LoginName;

        /// <summary>登录名</summary>
        public string LoginName
        {
            get { return m_LoginName; }
            set { m_LoginName = value; }
        }
        #endregion

        #region 属性:Password
        private string m_Password;

        /// <summary>密码</summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }
        #endregion

        #region 属性:Args
        private Dictionary<string, string> m_Args = new Dictionary<string, string>();

        /// <summary>参数信息</summary>
        public Dictionary<string, string> Args
        {
            get { return m_Args; }
            set { m_Args = value; }
        }
        #endregion

        #region 属性:ActionUri
        private Uri m_ActionUri;

        /// <summary>访问资源的地址</summary>
        public Uri ActionUri
        {
            get { return m_ActionUri; }
            set { m_ActionUri = value; }
        }
        #endregion
    }
}
