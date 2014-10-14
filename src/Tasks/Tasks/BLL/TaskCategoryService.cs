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
    public class TaskCategoryService : ITaskCategoryService
    {
        private TasksConfiguration configuration = null;

        private ITaskCategoryProvider provider = null;

        /// <summary></summary>
        public TaskCategoryService()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<ITaskCategoryProvider>(typeof(ITaskCategoryProvider));
        }

        /// <summary></summary>
        public TaskCategoryInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(TaskCategoryInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="TaskCategoryInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="TaskCategoryInfo"/>��ϸ��Ϣ</returns>
        public TaskCategoryInfo Save(TaskCategoryInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new Exception("ʵ����ʶ����Ϊ�ա�");
            }

            bool isNewObject = !this.IsExist(param.Id);

            string methodName = isNewObject ? "����" : "�༭";

            IAccountInfo account = KernelContext.Current.User;

            if (methodName == "����")
            {
                param.AccountId = account.Id;
                param.AccountName = account.Name;
            }

            // ����XSS�����ַ� 
            param = StringHelper.ToSafeXSS<TaskCategoryInfo>(param);

            this.provider.Save(param);

            return param;
        }
        #endregion

        #region ����:CanDelete(string id)
        /// <summary>�������������ܷ���ɾ��</summary>
        /// <param name="id">����������ʶ</param>
        /// <returns></returns>
        public bool CanDelete(string id)
        {
            return this.provider.CanDelete(id);
        }
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">����������ʶ</param>
        public void Delete(string id)
        {
            if (this.CanDelete(id))
            {
                this.provider.Delete(id);
            }
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public TaskCategoryInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region ����:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">��������</param>
        /// <returns>����ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex)
        {
            return this.provider.FindOneByCategoryIndex(categoryIndex);
        }
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<TaskCategoryInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<TaskCategoryInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<TaskCategoryInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns> 
        public IList<TaskCategoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��Ա��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region ����:SetStatus(int status)
        /// <summary>��������״̬(ͣ��/����)</summary>
        /// <param name="id">����������ʶ</param>
        /// <param name="status">1 ��ͣ�õ��������ã�0 �����õ�����ͣ��</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            return this.provider.SetStatus(id, status);
        }
        #endregion
    }
}
