// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :JobService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IBLL;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;
using X3Platform.Data;

namespace X3Platform.Membership.BLL
{
    /// <summary></summary>
    public class JobService : IJobService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IJobProvider provider = null;

        #region ���캯��:JobService()
        /// <summary>���캯��</summary>
        public JobService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IJobProvider>(typeof(IJobProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IJobInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IJobInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</returns>
        public IJobInfo Save(IJobInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        public IJobInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        public IList<IJobInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        public IList<IJobInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        public IList<IJobInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
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
        /// <returns>返回一个列表实例<see cref="IJobInfo"/></returns>
        public IList<IJobInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">ְλ����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">ְλ��ʶ</param>
        /// <param name="name">ְλ����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            return provider.Rename(id, name);
        }
        #endregion

        #region 属性:SyncFromPackPage(IJobInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">ְλ��Ϣ</param>
        public int SyncFromPackPage(IJobInfo param)
        {
            provider.SyncFromPackPage(param);
        
            return 0;
        }
        #endregion

        #region 属性:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>�������ݰ�</summary>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<IJobInfo> list = MembershipManagement.Instance.JobService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"job\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<jobs>");

            foreach (IJobInfo item in list)
            {
                outString.Append(item.Serializable());
            }

            outString.Append("</jobs>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion
    }
}
