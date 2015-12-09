namespace X3Platform.TemplateContent.BLL
{
    using System;
    using System.Collections.Generic;
    using X3Platform.Data;
    using X3Platform.Security;
    using X3Platform.Spring;
    using X3Platform.TemplateContent.IBLL;
    using X3Platform.TemplateContent.IDAL;
    using X3Platform.TemplateContent.Model;
    using X3Platform.TemplateContent.Configuration;

    /// <summary>ҳ��</summary>
    [SecurityClass]
    public class TemplateContentService : SecurityObject, ITemplateContentService
    {
        private ITemplateContentProvider provider = null;

        #region ���캯��:TemplateContentService()
        /// <summary>���캯��</summary>
        public TemplateContentService()
        {
            // �������󹹽���(Spring.NET)
            string springObjectFile = TemplateContentConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TemplateContentConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<ITemplateContentProvider>(typeof(ITemplateContentProvider));
        }
        #endregion

        #region ����:this[string name]
        /// <summary>����</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TemplateContentInfo this[string name]
        {
            get { return this.FindOneByName(name); }
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ�� TemplateContentInfo ʵ������ϸ��Ϣ</returns>
        public TemplateContentInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region ����:IsExistName(string name)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="name">ҳ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region ����:GetHtml(string name)
        /// <summary>��ȡHtml�ı�</summary>
        /// <param name="name">���򻮷�ģ������</param>
        /// <returns>Html�ı�</returns>
        public string GetHtml(string name)
        {
            return this.provider.GetHtml(name);
        }
        #endregion
    }
}
