// =============================================================================
//
// Copyright (c) 2007 RuanYu
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2007-09-28
//
// =============================================================================

namespace X3Platform.Security.Authority
{
    using System;
    using X3Platform.CacheBuffer;

    /// <summary>Ȩ����Ϣ</summary>
    public class AuthorityInfo : ICacheable
    {
        /// <summary>���캯��</summary>
        public AuthorityInfo() { }

        /// <summary>���캯��</summary>
        /// <param name="id">��ʶ</param>
        public AuthorityInfo(string id)
        {
            this.m_Id = id;
        }

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Name
        private string m_NickName;

        /// <summary></summary>
        public string Name
        {
            get { return m_NickName; }
            set { m_NickName = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description;

        /// <summary>������Ϣ</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Lock
        private int m_Lock;

        /// <summary>������Ϣ</summary>
        public int Lock
        {
            get { return m_Lock; }
            set { m_Lock = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary>��ǩ</summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>�޸�ʱ��</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>����ʱ��</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        //
        // ��ʽʵ�� ICacheable
        // 

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(6);

        /// <summary>����ʱ��</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}
