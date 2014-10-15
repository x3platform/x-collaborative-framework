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
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>Ӧ��������Ϣ</summary>
    public class ConnectInfo : EntityClass, ICacheable
    {
        public ConnectInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>�ύ�˱�ʶ</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>�ύ������</summary>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>Ӧ�ñ�ʶ</summary>
        public string AppKey
        {
            get { return this.m_AppKey; }
            set { this.m_AppKey = value; }
        }
        #endregion

        #region 属性:AppSecret
        private string m_AppSecret = string.Empty;

        /// <summary>Ӧ��˽Կ</summary>
        public string AppSecret
        {
            get { return this.m_AppSecret; }
            set { this.m_AppSecret = value; }
        }
        #endregion

        #region 属性:AppType
        private string m_AppType = string.Empty;

        /// <summary>Ӧ������</summary>
        public string AppType
        {
            get { return this.m_AppType; }
            set { this.m_AppType = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary>����</summary>
        public string Code
        {
            get { return this.m_Code; }
            set { this.m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary>����</summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>����</summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region 属性:Domain
        private string m_Domain = string.Empty;

        /// <summary>��</summary>
        public string Domain
        {
            get { return this.m_Domain; }
            set { this.m_Domain = value; }
        }
        #endregion

        #region 属性:RedirectUri
        private string m_RedirectUri = string.Empty;

        /// <summary>��¼�ɹ����ض����ĵ�ַ</summary>
        public string RedirectUri
        {
            get { return this.m_RedirectUri; }
            set { this.m_RedirectUri = value; }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath = string.Empty;

        /// <summary>ͼ���ļ�</summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:Options
        private string m_Options = string.Empty;

        /// <summary>�Զ�������</summary>
        public string Options
        {
            get { return this.m_Options; }
            set { this.m_Options = value; }
        }
        #endregion

        #region 属性:IsInternal
        private bool m_IsInternal = false;

        /// <summary>���ö���</summary>
        public bool IsInternal
        {
            get { return this.m_IsInternal; }
            set { this.m_IsInternal = value; }
        }
        #endregion

        #region 属性:AuthorizationScope
        private string m_AuthorizationScope = string.Empty;

        /// <summary>���ӵ���Ȩ��Χ(������Ӧ��Ĭ�ϵ�Ȩ��)</summary>
        public string AuthorizationScope
        {
            get { return this.m_AuthorizationScope; }
            set { this.m_AuthorizationScope = value; }
        }
        #endregion

        #region 属性:CertifiedCode
        private string m_CertifiedCode;

        /// <summary>��֤����</summary>
        public string CertifiedCode
        {
            get { return m_CertifiedCode; }
            set { m_CertifiedCode = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary>״̬</summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>�޸�����</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>��������</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ���� EntityClass ��ʶ
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>ʵ��������ʶ</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

        // -------------------------------------------------------
        // ��ʽʵ�� ICacheable
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.MaxValue;

        /// <summary>����ʱ��</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}
