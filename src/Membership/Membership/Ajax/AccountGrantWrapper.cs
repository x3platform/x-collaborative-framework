#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountGrantWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;
    
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public sealed class AccountGrantWrapper : ContextWrapper
    {
        private IAccountGrantService service = MembershipManagement.Instance.AccountGrantService; // ���ݷ���

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            IAccountGrantInfo param = new AccountGrantInfo();

            param = (IAccountGrantInfo)AjaxStorageConvertor.Deserialize(param, doc);

            if (this.service.IsExistGrantor(param.GrantorId, param.GrantedTimeFrom, param.GrantedTimeTo, param.Id))
            {
                return "{message:{\"returnCode\":1,\"value\":\"ί������ί��ʱ�������Ѿ�����ί����Ϣ�����޸��������á�\"}}";
            }

            if (this.service.IsExistGrantee(param.GrantorId, param.GrantedTimeFrom, param.GrantedTimeTo, param.Id))
            {
                return "{message:{\"returnCode\":2,\"value\":\"ί������ί��ʱ�������Ѿ��汻ί�й�ϵ��ϵͳ����������ί����Ȩ�����޸��������á�\"}}";
            }

            if (param.GrantedTimeFrom > param.GrantedTimeTo)
            {
                return "{message:{\"returnCode\":3,\"value\":\"ί�еĿ�ʼʱ�����ڽ���ʱ�䣬���޸��������á�\"}}";
            }

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            this.service.Delete(id);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindAll()
        /// <summary>��ѯ��������</summary>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            IList<IAccountGrantInfo> list = this.service.FindAll(whereClause);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindOne(XmlDocument doc)
        /// <summary>��ȡ������Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IAccountGrantInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns> 
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IAccountGrantInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:CreateNewObject(XmlDocument doc)
        /// <summary>�����µĶ���</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            IAccountGrantInfo param = new AccountGrantInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.GrantedTimeFrom = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            param.GrantedTimeTo = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd 00:00:00"));

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:Abort(XmlDocument doc)
        /// <summary>��ֹ��ǰί��</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("abort")]
        public string Abort(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            this.service.Abort(id);

            return "{message:{\"returnCode\":0,\"value\":\"���óɹ���\"}}";
        }
        #endregion
    }
}