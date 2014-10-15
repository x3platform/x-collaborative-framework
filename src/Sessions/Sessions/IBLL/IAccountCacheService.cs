// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IAccountCacheService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Sessions.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Sessions.IBLL.IAccountCacheService")]
    public interface IAccountCacheService
    {
        #region 属性:this[string accountIdentity]
        /// <summary>����������</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns></returns>
        AccountCacheInfo this[string accountIdentity] { get; }
        #endregion

        #region 属性:GetAuthAccount(IAccountStorageStrategy strategy, string accountIdentity)
        /// <summary>��ȡ��ǰ�ʺŻ�����Ϣ</summary>
        /// <param name="strategy">�ʺŴ洢����</param>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        IAccountInfo GetAuthAccount(IAccountStorageStrategy strategy, string accountIdentity);
        #endregion

        #region 属性:Authorize(string accountIdentity)
        /// <summary>������Ȩ��Ϣ</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns></returns>
        bool Authorize(string accountIdentity);
        #endregion

        #region 属性:Authorize(string accountIdentity, string appKey)
        /// <summary>������Ȩ��Ϣ</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="appKey">App Key</param>
        /// <returns></returns>
        bool Authorize(string accountIdentity, string appKey);
        #endregion

        #region 属性:Read(string accountIdentity)
        /// <summary>���һ�����¼</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        AccountCacheInfo Read(string accountIdentity);
        #endregion

        #region 属性:Read(string accountIdentity)
        /// <summary>���һ�����¼</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="appKey">App Key</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        // AccountCacheInfo Read(string accountIdentity, string appKey);
        #endregion

        #region 属性:ReadWithAccountCacheValue(string accountCacheValue)
        /// <summary>���һ�����¼</summary>
        /// <param name="accountCacheValue">������ֵ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        AccountCacheInfo ReadWithAccountCacheValue(string accountCacheValue);
        #endregion

        //#region 属性:ReadWithAccountCacheValue(string accountCacheValue, string ip)
        ///// <summary>���һ�����¼</summary>
        ///// <param name="accountCacheValue">������ֵ</param>
        ///// <param name="ip">������IP</param>
        ///// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        //AccountCacheInfo ReadWithAccountCacheValue(string accountCacheValue, string ip);
        //#endregion

        #region 属性:Write(IAccountStorageStrategy strategy)
        /// <summary>д����Ϣ</summary>
        /// <param name="strategy">����</param>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        void Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account);
        #endregion

        #region 属性:FindByAccountIdentity(string accountIdentity)
        /// <summary>���ݲ���ĳ����¼</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        AccountCacheInfo FindByAccountIdentity(string accountIdentity);
        #endregion

        #region 属性:FindByAccountCacheValue(string accountCacheValue)
        /// <summary>����ĳ����¼</summary>
        /// <param name="value">������ֵ</param>
        /// <returns>����һ�� ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
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
        /// <summary>���¼�¼</summary>
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

        #region 属性:Clear()
        /// <summary>���ջ�����¼</summary>
        int Clear();
        #endregion
    }
}
