// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IAccountCacheProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Sessions.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Sessions.IDAL.IAccountCacheProvider")]
    public interface IAccountCacheProvider
    {
        #region 属性:FindByAccountIdentity(string accountIdentity)
        /// <summary>���ݲ���ĳ����¼</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        AccountCacheInfo FindByAccountIdentity(string accountIdentity);
        #endregion

        #region 属性:FindByAccountCacheValue(string accountCacheValue)
        /// <summary>����ĳ����¼</summary>
        /// <param name="accountCacheValue">�ʺŻ�����ֵ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        AccountCacheInfo FindByAccountCacheValue(string accountCacheValue);
        #endregion

        #region 属性:Dump()
        /// <summary>ת�����м�¼��Ϣ</summary>
        /// <returns>����һ��<see cref="AccountCacheInfo"/>�б�</returns>
        IList<AccountCacheInfo> Dump();
        #endregion

        #region 属性:Insert(AccountCacheInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</param>
        void Insert(AccountCacheInfo param);
        #endregion

        #region 属性:Update(AccountCacheInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</param>
        void Update(AccountCacheInfo param);
        #endregion

        #region 属性:Delete(string accountIdentity)
        /// <summary>ɾ����¼</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        int Delete(string accountIdentity);
        #endregion

        #region 属性:IsExist(string accountIdentity)
        /// <summary>������¼�Ƿ�����</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        bool IsExist(string accountIdentity);
        #endregion

        #region 属性:IsExistAccountCacheValue(string accountCacheValue)
        /// <summary>������¼�Ƿ�����</summary>
        /// <param name="accountCacheValue">�ʺŻ���ֵ</param>
        bool IsExistAccountCacheValue(string accountCacheValue);
        #endregion

        #region 属性:Clear(DateTime expiryTime)
        /// <summary>��������ʱ��֮ǰ�Ļ�����¼</summary>
        /// <param name="expiryTime">����ʱ��</param>
        int Clear(DateTime expiryTime);
        #endregion
    }
}
