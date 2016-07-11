namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    public class CatalogService : ICatalogService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private ICatalogProvider provider = null;

        #region ���캯��:CatalogService()
        /// <summary>���캯��</summary>
        public CatalogService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<ICatalogProvider>(typeof(ICatalogProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatalogInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(CatalogInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="CatalogInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="CatalogInfo"/>��ϸ��Ϣ</returns>
        public CatalogInfo Save(CatalogInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new ApplicationException("�����������ı�ʶ����Ϊ�ա�");
            }

            CatalogItemInfo node = new CatalogItemInfo();

            node.Id = param.Id;

            node.Name = param.Name;

            node.ParentId = Guid.Empty.ToString();

            node.IsKey = true;

            MembershipManagement.Instance.CatalogItemService.Save(node);

            param.RootCatalogItemId = param.Id;

            provider.Save(param);

            return param;
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="CatalogInfo"/>����ϸ��Ϣ</returns>
        public CatalogInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="CatalogInfo"/>����ϸ��Ϣ</returns>
        public IList<CatalogInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="CatalogInfo"/>����ϸ��Ϣ</returns>
        public IList<CatalogInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="CatalogInfo"/>����ϸ��Ϣ</returns>
        public IList<CatalogInfo> FindAll(string whereClause, int length)
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
        /// <returns>返回一个列表实例<see cref="CatalogInfo"/></returns>
        public IList<CatalogInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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
    }
}
