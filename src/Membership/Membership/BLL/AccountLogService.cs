#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountLogService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class AccountLogService : IAccountLogService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IAccountLogProvider provider = null;

        #region ���캯��:AccountLogService()
        /// <summary>���캯��</summary>
        public AccountLogService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IAccountLogProvider>(typeof(IAccountLogProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountLogInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(AccountLogInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="AccountLogInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="AccountLogInfo"/>��ϸ��Ϣ</returns>
        public AccountLogInfo Save(AccountLogInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="AccountLogInfo"/>����ϸ��Ϣ</returns>
        public AccountLogInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="AccountLogInfo"/>����ϸ��Ϣ</returns>
        public IList<AccountLogInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="AccountLogInfo"/>����ϸ��Ϣ</returns>
        public IList<AccountLogInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="AccountLogInfo"/>����ϸ��Ϣ</returns>
        public IList<AccountLogInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="AccountLogInfo"/></returns>
        public IList<AccountLogInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:Log(string accountId, string optionName, string description)
        /// <summary>������־��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="optionName">����属性:��¼ �༭ ɾ��</param>
        /// <param name="description">������Ϣ</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        public int Log(string accountId, string optionName, string description)
        {
            return this.Log(accountId, optionName, description, string.Empty);
        }
        #endregion

        #region 属性:Log(string accountId, string optionName, string description, string optionAccountId)
        /// <summary>�����ʺŲ�����־��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="optionName">����属性: �鿴</param>
        /// <param name="description">������Ϣ</param>
        /// <param name="optionAccountId">�����˵��ʺű�ʶ</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        public int Log(string accountId, string optionName, string description, string optionAccountId)
        {
            return this.Log(accountId, optionName, null, description, optionAccountId);
        }
        #endregion

        #region 属性:Log(string accountId, string optionName, IAccountInfo originalObject, string description, string optionAccountId)
        /// <summary>������־��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="optionName">����属性:��¼ �༭ ɾ��</param>
        /// <param name="originalObject">ԭʼ�Ķ�����Ϣ</param>
        /// <param name="description">������Ϣ</param>
        /// <param name="optionAccountId">�����˵��ʺű�ʶ</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        public int Log(string accountId, string optionName, IAccountInfo originalObject, string description, string optionAccountId)
        {
            IAccountInfo account = KernelContext.Current.User;

            // ����ʵ�����ݲ�����¼
            AccountLogInfo param = new AccountLogInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");
            param.AccountId = accountId;
            param.OptionAccountId = optionAccountId;
            param.OptionName = optionName;
            param.OriginalObjectValue = originalObject == null ? string.Empty : originalObject.Serializable();
            param.Description = description;
            param.Date = DateTime.Now;

            param = this.Save(param);

            return param == null ? 1 : 0;
        }
        #endregion
    }
}
