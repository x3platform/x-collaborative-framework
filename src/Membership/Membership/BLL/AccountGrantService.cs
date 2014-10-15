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

namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;

    /// <summary>�ʺŴ�������</summary>
    public class AccountGrantService : IAccountGrantService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IAccountGrantProvider provider = null;

        #region ���캯��:AccountGrantService()
        /// <summary>���캯��</summary>
        public AccountGrantService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IAccountGrantProvider>(typeof(IAccountGrantProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAccountGrantInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IAccountGrantInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</returns>
        public IAccountGrantInfo Save(IAccountGrantInfo param)
        {
            // ��ʽί�п�ʼʱ��Ϊ 00:00:00
            param.GrantedTimeFrom = Convert.ToDateTime(param.GrantedTimeFrom.ToString("yyyy-MM-dd 00:00:00"));
            // ��ʽί�н���ʱ��Ϊ 23:59:59������ί�н�������û���յ�������Ϣ
            param.GrantedTimeTo = Convert.ToDateTime(param.GrantedTimeTo.ToString("yyyy-MM-dd 23:59:59"));

            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        public IAccountGrantInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        public IList<IAccountGrantInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        public IList<IAccountGrantInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        public IList<IAccountGrantInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="IAccountGrantInfo"/></returns>
        public IList<IAccountGrantInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
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

        #region 属性:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>��ѯ�Ƿ���������ί���˵ļ�¼.</summary>
        /// <param name="grantorId">ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.provider.IsExistGrantor(grantorId, grantedTimeFrom, grantedTimeTo);
        }
        #endregion

        #region 属性:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>��ѯ�Ƿ���������ί���˵ļ�¼.</summary>
        /// <param name="grantorId">ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <param name="ignoreIds">����ί�б�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        {
            return this.provider.IsExistGrantor(grantorId, grantedTimeFrom, grantedTimeTo, ignoreIds);
        }
        #endregion

        #region 属性:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>��ѯ�Ƿ��������ر�ί���˵ļ�¼.</summary>
        /// <param name="granteeId">��ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.provider.IsExistGrantee(granteeId, grantedTimeFrom, grantedTimeTo);
        }
        #endregion

        #region 属性:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>��ѯ�Ƿ��������ر�ί���˵ļ�¼.</summary>
        /// <param name="granteeId">��ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <param name="ignoreIds">����ί�б�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        {
            return this.provider.IsExistGrantee(granteeId, grantedTimeFrom, grantedTimeTo, ignoreIds);
        }
        #endregion

        #region 属性:Abort(string id)
        /// <summary>��ֹ��ǰί��</summary>
        /// <param name="id">��ʶ</param>
        /// <returns></returns>
        public int Abort(string id)
        {
            return this.provider.Abort(id);
        }
        #endregion

        // -------------------------------------------------------
        // ͬ������
        // -------------------------------------------------------

        #region 属性:SyncFromPackPage(IAccountGrantInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        public int SyncFromPackPage(IAccountGrantInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion
    }
}