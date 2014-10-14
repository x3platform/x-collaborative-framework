#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IAccountStorageStrategy.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Sessions
{
    using X3Platform.Membership;

    /// <summary>�ʺŴ洢���Խӿ�</summary>
    public interface IAccountStorageStrategy
    {
        #region ����:Deserialize(AccountCacheInfo accountCache)
        /// <summary>�����л�������Ϣ��Ϣ</summary>
        /// <returns></returns>
        IAccountInfo Deserialize(AccountCacheInfo accountCache);
        #endregion

        #region ����:Serialize(string accountIdentity, IAccountInfo account)
        /// <summary>���л�������Ϣ��Ϣ</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns></returns>
        AccountCacheInfo Serialize(string accountIdentity, IAccountInfo account);
        #endregion
    }
}