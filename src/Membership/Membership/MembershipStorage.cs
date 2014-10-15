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

namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CacheBuffer;
    using X3Platform.Membership.Model;
    
    /// <summary></summary> 
    public sealed class MembershipStorage
    {
        /// <summary>����ʱ��</summary>
        private DateTime expiryDate = DateTime.MaxValue;
        /// <summary>����</summary>
        private DateTime createDate = DateTime.Now;

        private static MembershipStorage instance = null;

        /// <summary></summary>
        public static MembershipStorage Instance
        {
            get
            {
                //
                // ˢ�¹�������
                //

                if (instance == null)
                {
                    instance = new MembershipStorage();
                }

                return instance;
            }
        }

        private MembershipStorage()
        {
            //  expiryDate.AddHours(6);
        }

        /// <summary>���¼���</summary>
        public void Reload()
        {
            instance = null;
        }

        #region 属性:Accounts
        private Dictionary<string, IAccountInfo> accountCache = null;

        /// <summary>�ʺ�</summary>
        public Dictionary<string, IAccountInfo> Accounts
        {
            get
            {
                if (accountCache == null)
                {
                    accountCache = new Dictionary<string, IAccountInfo>();

                    IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAll();

                    foreach (IAccountInfo item in list)
                    {
                        this.accountCache.Add(item.Id, item);
                    }
                }

                return accountCache;
            }
        }
        #endregion

        #region 属性:AuthenticatedAccounts

        private Dictionary<string, IAccountInfo> authenticatedAccountCache = null;

        /// <summary>��֤���ʺ�</summary>
        public Dictionary<string, IAccountInfo> AuthenticatedAccounts
        {
            get
            {
                if (authenticatedAccountCache == null)
                {
                    authenticatedAccountCache = new Dictionary<string, IAccountInfo>();

                    foreach (KeyValuePair<string, IAccountInfo> entry in this.Accounts)
                    {
                        if (entry.Value.Status == 1)
                            authenticatedAccountCache.Add(entry.Key, entry.Value);
                    }
                }

                return authenticatedAccountCache;
            }
        }
        #endregion

        #region 属性:Members
        private Dictionary<string, IMemberInfo> memberCache = null;

        /// <summary>�û�</summary>
        public Dictionary<string, IMemberInfo> Members
        {
            get
            {
                if (memberCache == null)
                {
                    memberCache = new Dictionary<string, IMemberInfo>();

                    IList<IMemberInfo> list = MembershipManagement.Instance.MemberService.FindAll();

                    foreach (IMemberInfo item in list)
                    {
                        this.memberCache.Add(item.Id, item);
                    }
                }

                return memberCache;
            }
        }
        #endregion

        #region 属性:Organizations
        private Dictionary<string, IOrganizationInfo> organizationCache = null;

        /// <summary>��֯��λ</summary>
        public Dictionary<string, IOrganizationInfo> Organizations
        {
            get
            {
                if (organizationCache == null)
                {
                    organizationCache = new Dictionary<string, IOrganizationInfo>();

                    IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAll();

                    foreach (IOrganizationInfo item in list)
                    {
                        this.organizationCache.Add(item.Id, item);
                    }
                }

                return organizationCache;
            }
        }
        #endregion

        #region 属性:Zones
        private Dictionary<string, IOrganizationInfo> zoneCache = null;

        /// <summary>����</summary>
        public Dictionary<string, IOrganizationInfo> Zones
        {
            get
            {
                if (zoneCache == null)
                {
                    zoneCache = new Dictionary<string, IOrganizationInfo>();

                    foreach (KeyValuePair<string, IOrganizationInfo> entry in this.Organizations)
                    {
                        if (entry.Value.Type == -1 && entry.Value.Status == 1)
                            zoneCache.Add(entry.Key, entry.Value);
                    }
                }

                return zoneCache;
            }
        }
        #endregion

        #region 属性:Corporations
        private Dictionary<string, IOrganizationInfo> corporationCache = null;

        /// <summary>��˾</summary>
        public Dictionary<string, IOrganizationInfo> Corporations
        {
            get
            {
                if (corporationCache == null)
                {
                    corporationCache = new Dictionary<string, IOrganizationInfo>();

                    foreach (KeyValuePair<string, IOrganizationInfo> entry in this.Organizations)
                    {
                        if (entry.Value.Type == 0 && entry.Value.Status == 1)
                            corporationCache.Add(entry.Key, entry.Value);
                    }
                }

                return corporationCache;
            }
        }
        #endregion

        #region 属性:Departments
        private Dictionary<string, IOrganizationInfo> departmentCache = null;

        /// <summary>����()</summary>
        public Dictionary<string, IOrganizationInfo> Departments
        {
            get
            {
                if (departmentCache == null)
                {
                    departmentCache = new Dictionary<string, IOrganizationInfo>();

                    foreach (KeyValuePair<string, IOrganizationInfo> entry in this.Organizations)
                    {
                        if (entry.Value.Type > -1 && entry.Value.Status == 1)
                            departmentCache.Add(entry.Key, entry.Value);
                    }
                }

                return departmentCache;
            }
        }
        #endregion

        #region 属性:Groups
        private Dictionary<string, GroupInfo> groupCache = null;

        /// <summary>Ⱥ��</summary>
        public Dictionary<string, GroupInfo> Groups
        {
            get
            {
                if (groupCache == null)
                {
                    groupCache = new Dictionary<string, GroupInfo>();

                    //foreach (KeyValuePair<string, IOrganizationInfo> entry in this.Organizations)
                    //{
                    //    if (entry.Value.Type == 65536 && entry.Value.Status == 1)
                    //        groupCache.Add(entry.Key, entry.Value);
                    //}

                    IList<IGroupInfo> list = MembershipManagement.Instance.GroupService.FindAll();

                    foreach (GroupInfo item in list)
                    {
                        this.groupCache.Add(item.Id, item);
                    }
                }

                return groupCache;
            }
        }
        #endregion
    }
}
