// =============================================================================
//
// Copyright (c) x3platfrom.com
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

using System;
using System.Collections.Generic;

using X3Platform.Spring;
using X3Platform.AttachmentStorage.Configuration;
using X3Platform.AttachmentStorage.IBLL;
using X3Platform.AttachmentStorage.IDAL;

namespace X3Platform.AttachmentStorage.BLL
{
    public sealed class AttachmentDistributedFileService : IAttachmentDistributedFileService
    {
        /// <summary>����</summary>
        private AttachmentStorageConfiguration configuration = null;

        private IAttachmentDistributedFileProvider provider = null;

        public AttachmentDistributedFileService()
        {
            // ��ȡ������Ϣ
            this.configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // �������ݷ�������
            this.provider = objectBuilder.GetObject<IAttachmentDistributedFileProvider>(typeof(IAttachmentDistributedFileProvider));
        }

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DistributedFileInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">AccountInfo ʵ����ϸ��Ϣ</param>
        ///<param name="message">���ݿ����󷵻ص�������Ϣ</param>
        ///<returns>AccountInfo ʵ����ϸ��Ϣ</returns>
        public DistributedFileInfo Save(DistributedFileInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="keys">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(int id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">AccountInfo Id��</param>
        ///<returns>����һ�� AccountInfo ʵ������ϸ��Ϣ</returns>
        public DistributedFileInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<DistributedFileInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<DistributedFileInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<DistributedFileInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
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
        public IList<DistributedFileInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="id">��Ա��ʶ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion
    }
}
