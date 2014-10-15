// =============================================================================
//
// Copyright (c) ruanyu@live.com
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

using System;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;

using X3Platform.Logging;
using X3Platform.CacheBuffer;
using X3Platform.Security.Authority;

namespace X3Platform.Membership.Scope
{
    /// <summary></summary>
    public class MembershipAuthorizationScope : IAuthorizationScope
    {
        /// <summary></summary>
        public MembershipAuthorizationScope(EntityClass entity, AuthorityInfo authority, IAuthorizationObject authorizationObject)
        {
            this.EntityClass = entity;
            this.Authority = authority;
            this.AuthorizationObject = authorizationObject;
        }

        #region 属性:EntityClass
        private EntityClass m_EntityClass = null;

        /// <summary>实体类</summary>
        public EntityClass EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
        }
        #endregion

        #region 属性:Authority
        private AuthorityInfo m_Authority = null;

        /// <summary>权限信息</summary>
        public AuthorityInfo Authority
        {
            get { return m_Authority; }
            set { m_Authority = value; }
        }
        #endregion

        #region 属性:AuthorizationObject
        private IAuthorizationObject m_AuthorizationObject = null;

        /// <summary>权限对象信息</summary>
        public IAuthorizationObject AuthorizationObject
        {
            get { return m_AuthorizationObject; }
            set { m_AuthorizationObject = value; }
        }
        #endregion
    }
}
