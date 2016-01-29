namespace X3Platform.Location.Regions.DAL.IBatis
{
    using System;
    using System.Collections.Generic;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Location.Regions.Configuration;
    using X3Platform.Location.Regions.Model;
    using X3Platform.Location.Regions.IDAL;
    using X3Platform.Data;

    public class RegionProvider : IRegionProvider
    {
        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ�����</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Region";

        public RegionProvider()
        {
            this.ibatisMapping = RegionsConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // ���� ��� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(RegionInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</returns>
        public RegionInfo Save(RegionInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region ����:Insert(RegionInfo param)
        /// <summary>��Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</param>
        public void Insert(RegionInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region ����:Update(RegionInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</param>
        public void Update(RegionInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            // ɾ���ʺ���Ϣ
            args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="param">RegionInfo Id��</param>
        /// <returns>����һ�� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
        public RegionInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<RegionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region ����:FindAll(DataQuery query)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
        public IList<RegionInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } });

            args.Add("WhereClause", whereClause);
            args.Add("OrderBy", "Id");
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<RegionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        public IList<RegionInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" Name "));

            args.Add("RowCount", 0);

            IList<RegionInfo> list = this.ibatisMapper.QueryForList<RegionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion
    }
}
