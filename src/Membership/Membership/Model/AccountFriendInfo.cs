namespace X3Platform.Membership.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using X3Platform.Membership;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary></summary>
    public class AccountFriendInfo
    {
        #region 默认构造函数:AccountFriendInfo()
        /// <summary>默认构造函数</summary>
        public AccountFriendInfo()
        {
        }
        #endregion

        #region 属性:AccountId
        /// <summary></summary>
        public string AccountId { get; set; }
        #endregion

        #region 属性:FriendAccountId
        /// <summary></summary>
        public string FriendAccountId { get; set; }
        #endregion

        #region 属性:FriendDisplayName
        private string m_FriendDisplayName;

        /// <summary></summary>
        public string FriendDisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_FriendDisplayName))
                {
                    this.m_FriendDisplayName = MembershipManagement.Instance.AccountService[this.FriendAccountId].Name;
                }

                return this.m_FriendDisplayName;
            }
            set { this.m_FriendDisplayName = value; }
        }
        #endregion

        #region 属性:FriendCertifiedAvatar
        private string m_FriendCertifiedAvatar;

        /// <summary>好友的头像</summary>
        public string FriendCertifiedAvatar
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_FriendCertifiedAvatar))
                {
                    this.m_FriendCertifiedAvatar = MembershipManagement.Instance.AccountService[this.FriendAccountId].CertifiedAvatar;
                }

                return this.m_FriendCertifiedAvatar;
            }
            set { this.m_FriendCertifiedAvatar = value; }
        }
        #endregion

        #region 属性:FriendCertifiedAvatarView
        /// <summary>已验证的头像虚拟路径</summary>
        public string FriendCertifiedAvatarView
        {
            get { return this.FriendCertifiedAvatar.Replace("{avatar}", MembershipConfigurationView.Instance.AvatarVirtualFolder); }
        }
        #endregion

        #region 属性:Status
        /// <summary></summary>
        public int Status { get; set; }
        #endregion

        #region 属性:CreatedDate
        /// <summary></summary>
        public DateTime CreatedDate { get; set; }
        #endregion
    }
}