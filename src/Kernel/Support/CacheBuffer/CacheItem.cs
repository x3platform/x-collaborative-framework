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
// Date         :2009-05-10
//
// =============================================================================

//
// 变更记录
//
// == 2009-05-10 == 
//
// 新建
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace X3Platform.CacheBuffer
{
    /// <summary>缓存项</summary>
    public sealed class CacheItem : ICacheable
    {
        public CacheItem(object value)
            : this(value, DateTime.Now.AddMonths(1))
        {
        }

        public CacheItem(object value, DateTime expires)
        {
            m_Value = value;
            m_Expires = expires;
        }

        #region 属性:Expires
        private object m_Value = null;

        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        #endregion

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now;

        /// <summary></summary>
        public DateTime Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}