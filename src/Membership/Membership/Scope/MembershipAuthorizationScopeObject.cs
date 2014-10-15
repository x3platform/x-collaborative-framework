// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :MembershipAuthorizationScopeObject.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.Scope
{
    using System;

    using X3Platform.Util;

    /// <summary>授权范围对象</summary>
    public class MembershipAuthorizationScopeObject
    {
        /// <summary></summary>
        public MembershipAuthorizationScopeObject(string scopeText)
        {
            string[] args = scopeText.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            this.AuthorizationObjectType = args[0] == null ? string.Empty : args[0];
            this.AuthorizationObjectId = args[1] == null ? string.Empty : args[1];

            if (args.Length == 3)
            {
                this.AuthorizationObjectDescription = args[2] == null ? string.Empty : args[2];
            }
        }

        /// <summary></summary>
        /// <param name="authorizationObjectType"></param>
        /// <param name="authorizationObjectId"></param>
        public MembershipAuthorizationScopeObject(string authorizationObjectType, string authorizationObjectId)
            : this(authorizationObjectType, authorizationObjectId, string.Empty)
        {

        }

        /// <summary></summary>
        /// <param name="authorizationObjectType"></param>
        /// <param name="authorizationObjectId"></param>
        /// <param name="authorizationObjectDescription"></param>
        public MembershipAuthorizationScopeObject(string authorizationObjectType, string authorizationObjectId, string authorizationObjectDescription)
        {
            this.AuthorizationObjectType = authorizationObjectType;
            this.AuthorizationObjectId = authorizationObjectId;
            this.AuthorizationObjectDescription = authorizationObjectDescription;
        }

        #region 属性:AuthorizationObjectType
        private string m_AuthorizationObjectType = string.Empty;

        /// <summary>名称</summary>
        public string AuthorizationObjectType
        {
            get { return m_AuthorizationObjectType; }
            set { m_AuthorizationObjectType = value; }
        }
        #endregion

        #region 属性:AuthorizationObjectId
        private string m_AuthorizationObjectId = string.Empty;

        /// <summary>标识</summary>
        public string AuthorizationObjectId
        {
            get { return m_AuthorizationObjectId; }
            set { m_AuthorizationObjectId = value; }
        }
        #endregion

        #region 属性:AuthorizationObjectDescription
        private string m_AuthorizationObjectDescription = string.Empty;

        /// <summary>描述</summary>
        public string AuthorizationObjectDescription
        {
            get { return m_AuthorizationObjectDescription; }
            set { m_AuthorizationObjectDescription = value; }
        }
        #endregion

        #region 函数:GetAuthorizationObject()
        /// <summary></summary>
        /// <returns></returns>
        public IAuthorizationObject GetAuthorizationObject()
        {
            return MembershipManagement.Instance.AuthorizationObjectService.FindOne(this.AuthorizationObjectType, this.AuthorizationObjectId);
        }
        #endregion

        #region 函数:ToString()
        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            return StringHelper.ToFirstLower(this.AuthorizationObjectType) + "#"
                 + this.AuthorizationObjectId + "#"
                 + (string.IsNullOrEmpty(this.AuthorizationObjectDescription) ? this.AuthorizationObjectId : this.AuthorizationObjectDescription) + ",";
        }
        #endregion
    }
}
