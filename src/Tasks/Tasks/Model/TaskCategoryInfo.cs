namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;

    using X3Platform.Membership;
    #endregion

    /// <summary>任务分类信息</summary>
    [Serializable]
    public class TaskCategoryInfo
    {
        #region 构造函数:TaskCategoryInfo()
        /// <summary>默认构造函数</summary>
        public TaskCategoryInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>
        /// 标识
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(m_AccountName) && !string.IsNullOrEmpty(this.AccountId))
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(this.AccountId);

                    if (account != null)
                    {
                        m_AccountName = account.Name;
                    }
                }

                return m_AccountName;
            }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex = string.Empty;

        /// <summary>类别索引</summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex; }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>
        /// 办理说明
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags = string.Empty;

        /// <summary>标签</summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

        /// <summary>排序编号</summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status = 1;

        /// <summary>
        /// 状态 1:启用 | 0:禁用
        /// </summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
