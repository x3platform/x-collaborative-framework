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

using System.Collections.Generic;
using System.Text;
using System.Xml;

using X3Platform.Ajax;
using X3Platform.Data;
using X3Platform.Util;

using X3Platform.Security.Authority.IBLL;

namespace X3Platform.Security.Authority.Ajax
{
    /// <summary>Ȩ��</summary>
    public class AuthorityWrapper : ContextWrapper
    {
        IAuthorityService service = AuthorityContext.Instance.AuthorityService;

        //-------------------------------------------------------
        // ���� ɾ��
        //-------------------------------------------------------

        #region ����:Save(XmlDocument doc)
        /// <summary>������Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            AuthorityInfo param = new AuthorityInfo();

            param = AjaxStorageConvertor.Deserialize<AuthorityInfo>(param, doc);

            service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region ����:Delete(XmlDocument doc)
        /// <summary>ɾ������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

            service.Delete(ids);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        //-------------------------------------------------------
        // ��ѯ
        //-------------------------------------------------------

        #region ����:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            AuthorityInfo param = service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<AuthorityInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindAll(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            IList<AuthorityInfo> list = service.FindAll(new DataQuery());

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<AuthorityInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        //-------------------------------------------------------
        // �Զ��幦��
        //-------------------------------------------------------

        #region ����:Query(XmlDocument doc)
        /// <summary>��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Query(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(AjaxStorageConvertor.Fetch("paging", doc, "xml"), AjaxStorageConvertor.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<AuthorityInfo> list = this.service.Query(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<AuthorityInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
