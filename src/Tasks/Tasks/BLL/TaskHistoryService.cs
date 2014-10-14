#region Copyright & Author
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
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary></summary>
    public class TaskHistoryService : ITaskHistoryService
    {
        private TasksConfiguration configuration = null;

        private ITaskHistoryProvider provider = null;

        /// <summary></summary>
        public TaskHistoryService()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<ITaskHistoryProvider>(typeof(ITaskHistoryProvider));
        }

        #region ����:this[string id, string receiverId]
        /// <summary>����</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns></returns>
        public TaskHistoryItemInfo this[string id, string receiverId]
        {
            get { return this.FindOne(id, receiverId); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(TaskHistoryItemInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="TaskHistoryItemInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="TaskHistoryItemInfo"/>��ϸ��Ϣ</returns>
        public TaskHistoryItemInfo Save(TaskHistoryItemInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new Exception("ʵ����ʶ����Ϊ�ա�");
            }

            // ����XSS�����ַ� 
            param = StringHelper.ToSafeXSS<TaskHistoryItemInfo>(param);

            return this.provider.Save(param);
        }
        #endregion

        #region ����:Delete(string id, string receiverId)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        public void Delete(string id, string receiverId)
        {
            this.provider.Delete(id, receiverId);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id, string receiverId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ʵ��<see cref="TaskHistoryItemInfo"/></returns>
        public TaskHistoryItemInfo FindOne(string id, string receiverId)
        {
            return this.provider.FindOne(id, receiverId);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns> 
        public IList<TaskHistoryItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(receiverId, startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region ����:IsExist(string id, string receiverId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id, string receiverId)
        {
            return this.provider.IsExist(id, receiverId);
        }
        #endregion
    }
}
