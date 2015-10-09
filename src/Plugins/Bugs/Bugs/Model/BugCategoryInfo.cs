#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary>���������Ϣ</summary>
    [Serializable]
    public class BugCategoryInfo : EntityClass
    {
        #region ���캯��:BugCategoryInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public BugCategoryInfo()
        {
        }
        #endregion

        #region ����:Id
        private string m_Id = string.Empty;

        /// <summary>
        /// ��ʶ
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region ����:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>
        /// ������Id
        /// </summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region ����:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>
        /// ����������
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

        #region ����:CategoryIndex
        private string m_CategoryIndex = string.Empty;

        /// <summary>�������</summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex; }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region ����:Description
        private string m_Description = string.Empty;

        /// <summary>
        /// ����˵��
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region ����:OrderId
        private string m_OrderId = "0";

        /// <summary>
        /// ����ţ�Ĭ��0��
        /// </summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region ����:Status
        private int m_Status = 1;

        /// <summary>
        /// ״̬��1��Ч��0��Ч��
        /// </summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region ����:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>
        /// ����޸�ʱ��
        /// </summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region ����:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // �������Ա��Ϣ
        // -------------------------------------------------------

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_���Ȩ��</summary>
        private AuthorityInfo authorizationAdd = AuthorityContext.Instance.AuthorityService["Ӧ��_ͨ��_���Ȩ��"];

        #region ����:BindAuthorizationAddScope(string scopeText)
        /// <summary>�����Ȩ��</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationAddScope(string scopeText)
        {
            // ��ջ�������
            this.m_AuthorizationAddScopeObjectText = null;
            this.m_AuthorizationAddScopeObjectView = null;

            if (this.m_AuthorizationAddScopeObjects == null)
            {
                this.m_AuthorizationAddScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationAddScopeObjects, scopeText);
        }
        #endregion

        #region ����:AuthorizationAddScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationAddScopeObjects = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationAddScopeObjects
        {
            get
            {
                if (m_AuthorizationAddScopeObjects == null)
                {
                    m_AuthorizationAddScopeObjects = BugContext.Instance.BugCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationAdd.Name);

                    // ����Ĭ��Ȩ���ǿ�
                    if (m_AuthorizationAddScopeObjects.Count == 0)
                    {
                        IAuthorizationObject authorizationObject = MembershipManagement.Instance.RoleService.GetEveryoneObject();

                        m_AuthorizationAddScopeObjects.Add(new MembershipAuthorizationScopeObject(authorizationObject.Type, authorizationObject.Id, authorizationObject.Name));
                    }
                }

                return m_AuthorizationAddScopeObjects;
            }
        }
        #endregion

        #region ����:AuthorizationAddScopeObjectText
        private string m_AuthorizationAddScopeObjectText = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ�ı�</summary>
        public string AuthorizationAddScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AuthorizationAddScopeObjectText))
                {
                    this.m_AuthorizationAddScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationAddScopeObjects);
                }

                return this.m_AuthorizationAddScopeObjectText;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BindAuthorizationReadScope(value);
                }
            }
        }
        #endregion

        #region ����:AuthorizationReadScopeObjectView
        private string m_AuthorizationAddScopeObjectView = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ��ͼ</summary>
        public string AuthorizationAddScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationAddScopeObjectView))
                {
                    m_AuthorizationAddScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationAddScopeObjects);
                }

                return m_AuthorizationAddScopeObjectView;
            }
        }
        #endregion

        // -------------------------------------------------------
        // Ĭ�Ͽ��Ķ���Ա��Ϣ
        // -------------------------------------------------------

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ��</summary>
        private AuthorityInfo authorizationRead = AuthorityContext.Instance.AuthorityService["Ӧ��_ͨ��_�鿴Ȩ��"];

        #region ����:BindAuthorizationReadScope(string scopeText)
        /// <summary>�󶨲鿴Ȩ��</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationReadScope(string scopeText)
        {
            // ��ջ�������
            this.m_AuthorizationReadScopeObjectText = null;
            this.m_AuthorizationReadScopeObjectView = null;

            if (this.m_AuthorizationReadScopeObjects == null)
            {
                this.m_AuthorizationReadScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationReadScopeObjects, scopeText);
        }
        #endregion

        #region ����:AuthorizationReadScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationReadScopeObjects = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationReadScopeObjects
        {
            get
            {
                if (m_AuthorizationReadScopeObjects == null)
                {
                    m_AuthorizationReadScopeObjects = BugContext.Instance.BugCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationRead.Name);

                    // ����Ĭ��Ȩ����������
                    if (m_AuthorizationReadScopeObjects.Count == 0)
                    {
                        IAuthorizationObject authorizationObject = MembershipManagement.Instance.RoleService.GetEveryoneObject();

                        m_AuthorizationReadScopeObjects.Add(new MembershipAuthorizationScopeObject(authorizationObject.Type, authorizationObject.Id, authorizationObject.Name));
                    }
                }

                return m_AuthorizationReadScopeObjects;
            }
        }
        #endregion

        #region ����:AuthorizationReadScopeObjectText
        private string m_AuthorizationReadScopeObjectText = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ�ı�</summary>
        public string AuthorizationReadScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectText))
                {
                    m_AuthorizationReadScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationReadScopeObjects);
                }

                return m_AuthorizationReadScopeObjectText;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BindAuthorizationReadScope(value);
                }
            }
        }
        #endregion

        #region ����:AuthorizationReadScopeObjectView
        private string m_AuthorizationReadScopeObjectView = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ��ͼ</summary>
        public string AuthorizationReadScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectView))
                {
                    m_AuthorizationReadScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationReadScopeObjects);
                }

                return m_AuthorizationReadScopeObjectView;
            }
        }
        #endregion

        // -------------------------------------------------------
        // ��ά����Ա��Ϣ
        // -------------------------------------------------------

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�޸�Ȩ��</summary>
        private AuthorityInfo authorizationEdit = AuthorityContext.Instance.AuthorityService["Ӧ��_ͨ��_�޸�Ȩ��"];

        #region ����:BindAuthorizationEditScope(string scopeText)
        /// <summary>���޸�Ȩ��</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationEditScope(string scopeText)
        {
            // ��ջ�������
            this.m_AuthorizationEditScopeObjectText = null;
            this.m_AuthorizationEditScopeObjectView = null;

            if (this.m_AuthorizationEditScopeObjects == null)
            {
                this.m_AuthorizationEditScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationEditScopeObjects, scopeText);
        }
        #endregion

        #region ����:AuthorizationEditScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationEditScopeObjects = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationEditScopeObjects
        {
            get
            {
                if (this.m_AuthorizationEditScopeObjects == null)
                {
                    this.m_AuthorizationEditScopeObjects = BugContext.Instance.BugCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationEdit.Name);

                    // ����Ĭ��Ȩ���ǿ�
                    if (this.m_AuthorizationEditScopeObjects.Count == 0)
                    {

                    }
                }

                return this.m_AuthorizationEditScopeObjects;
            }
        }
        #endregion

        #region ����:AuthorizationEditScopeObjectText
        private string m_AuthorizationEditScopeObjectText = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ�ı�</summary>
        public string AuthorizationEditScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationEditScopeObjectText))
                {
                    m_AuthorizationEditScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationEditScopeObjects);
                }

                return m_AuthorizationEditScopeObjectText;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BindAuthorizationReadScope(value);
                }
            }
        }
        #endregion

        #region ����:AuthorizationEditScopeObjectView
        private string m_AuthorizationEditScopeObjectView = null;

        /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ��ͼ</summary>
        public string AuthorizationEditScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationEditScopeObjectView))
                {
                    m_AuthorizationEditScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationEditScopeObjects);
                }

                return m_AuthorizationEditScopeObjectView;
            }
        }
        #endregion

        //
        // ���� EntityClass ��ʶ
        // 

        #region ����:EntityId
        /// <summary>ʵ������ʶ</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion
    }
}
