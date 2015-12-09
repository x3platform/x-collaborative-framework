namespace X3Platform.TemplateContent.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.TemplateContent.Configuration;
    using X3Platform.TemplateContent.Model;
    using X3Platform.TemplateContent.IDAL;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class TemplateContentProvider : ITemplateContentProvider
    {
        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ�����</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Template_Content";

        #region ���캯��:TemplateContentProvider()
        /// <summary>���캯��</summary>
        public TemplateContentProvider()
        {
            this.ibatisMapping = TemplateContentConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", name);

            return this.ibatisMapper.QueryForObject<TemplateContentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);
        }
        #endregion

        #region ����:IsExistName(string name)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="name">ҳ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("���Ʋ���Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{1}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region ����:GetHtml(string name)
        /// <summary>��ȡHtml�ı�</summary>
        /// <param name="name">���򻮷�ģ������</param>
        /// <returns>Html�ı�</returns>
        public string GetHtml(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", StringHelper.ToSafeSQL(name));

            return this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetHtml", tableName)), args).ToString();
        }
        #endregion
    }
}
