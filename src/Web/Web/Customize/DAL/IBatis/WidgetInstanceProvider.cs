#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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
#endregion

namespace X3Platform.Web.Customize.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customize.Model;
    using X3Platform.Web.Customize.IDAL;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class WidgetInstanceProvider : IWidgetInstanceProvider
    {
        /// <summary>����</summary>
        private WebConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Customize_WidgetInstance";

        #region ���캯��:WidgetInstanceProvider()
        /// <summary>���캯��</summary>
        public WidgetInstanceProvider()
        {
            this.configuration = WebConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInstanceInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <returns>WidgetInstanceInfo ʵ����ϸ��Ϣ</returns>
        public WidgetInstanceInfo Save(WidgetInstanceInfo param)
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

        #region 属性:Insert(WidgetInstanceInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ������ϸ��Ϣ</param>
        public void Insert(WidgetInstanceInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(WidgetInstanceInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ������ϸ��Ϣ</param>
        public void Update(WidgetInstanceInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="param">WidgetInstanceInfo Id��</param>
        /// <returns>����һ�� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        public WidgetInstanceInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            WidgetInstanceInfo param = this.ibatisMapper.QueryForObject<WidgetInstanceInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInstanceInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<WidgetInstanceInfo> list = this.ibatisMapper.QueryForList<WidgetInstanceInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // ��ҳ
        // -------------------------------------------------------


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
        /// <returns>����һ���б�ʵ��</returns> 
        public IList<WidgetInstanceInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("pageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<WidgetInstanceInfo> list = this.ibatisMapper.QueryForList<WidgetInstanceInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("ʵ����ʶ����Ϊ�ա�");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 属性:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        /// <summary>ɾ��δ�󶨵Ĳ���ʵ��</summary>
        /// <param name="pageId">ҳ������</param>
        /// <param name="bindingWidgetInstanceIds">�󶨵Ĳ�����ʶ</param>
        /// <returns>����ֵ</returns>
        public int RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" PageId = '{0}' AND Id NOT IN ('{1}') ", StringHelper.ToSafeSQL(pageId), StringHelper.ToSafeSQL(bindingWidgetInstanceIds).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            return 0;
        }
        #endregion
    }
}
