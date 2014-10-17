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

namespace X3Platform.AttachmentStorage.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.IDAL;

    [DataObject]
    public class AttachmentStorageProvider : IAttachmentStorageProvider
    {
        /// <summary>����</summary>
        private AttachmentStorageConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Attachment_Storage";

        /// <summary></summary>
        public AttachmentStorageProvider()
        {
            this.configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }

        public IAttachmentFileInfo this[string index]
        {
            get { return this.FindOne(index); }
        }

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IAttachmentFileInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">IAttachmentFileInfo ʵ����ϸ��Ϣ</param>
        ///<returns>IAttachmentFileInfo ʵ����ϸ��Ϣ</returns>
        public IAttachmentFileInfo Save(IAttachmentFileInfo param)
        {
            if (this.IsExist(param.Id)) { new Exception("The same entity's id already exists. |-_-||"); }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Save", this.tableName)), param);

            return param;
        }
        #endregion

        #region 属性:Delete(string id)
        ///<summary>ɾ����¼</summary>
        ///<param name="id">��ʶ</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            // 01.ɾ�������ļ���Ϣ

            string whereClause = " Id IN (##" + id.Replace(",", "##,##") + "##) ";

            IList<IAttachmentFileInfo> list = FindAll(whereClause, 0);

            foreach (IAttachmentFileInfo item in list)
            {
                string path = item.VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            // 02.ɾ�����ݿ���Ϣ

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", id.Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

            // 03.ɾ���ֲ�ʽ�ļ�����

            DistributedFileStorage.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="param">IAttachmentFileInfo Id��</param>
        ///<returns>����һ�� IAttachmentFileInfo ʵ������ϸ��Ϣ</returns>
        public IAttachmentFileInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<IAttachmentFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� IAttachmentFileInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAttachmentFileInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            if (whereClause.IndexOf(" ORDER BY ") == -1)
            {
                whereClause += " ORDER BY OrderId, AttachmentName ";
            }

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<IAttachmentFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        #region 属性:FindAllByEntityId(string entityClassName, string entityId)
        ///<summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="entityId">ʵ������ʶ</param>
        ///<returns>�������� IAttachmentFileInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId)
        {
            string whereClause = string.Format(" ( EntityClassName = ##{0}## AND EntityId = ##{1}## ) ", entityClassName, entityId);

            return this.FindAll(whereClause, 0);
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
        /// <returns>����һ���б�ʵ��</returns> 
        public IList<IAttachmentFileInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " CreateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IAttachmentFileInfo> list = this.ibatisMapper.QueryForList<IAttachmentFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="id">IAttachmentFileInfo ʵ����ϸ��Ϣ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("ʵ����ʶ����Ϊ��.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 属性:Rename(string id, string name)
        ///<summary>������</summary>
        ///<param name="id">������ʶ</param>
        ///<param name="name">�µĸ�������</param>
        public void Rename(string id, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("AttachmentName", StringHelper.ToSafeSQL(name));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", this.tableName)), args);
        }
        #endregion
    }
}
