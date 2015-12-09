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
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Template_Content";

        #region 构造函数:TemplateContentProvider()
        /// <summary>构造函数</summary>
        public TemplateContentProvider()
        {
            this.ibatisMapping = TemplateContentConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个 TemplateContentInfo 实例的详细信息</returns>
        public TemplateContentInfo FindOneByName(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", name);

            return this.ibatisMapper.QueryForObject<TemplateContentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{1}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:GetHtml(string name)
        /// <summary>获取Html文本</summary>
        /// <param name="name">区域划分模板名称</param>
        /// <returns>Html文本</returns>
        public string GetHtml(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", StringHelper.ToSafeSQL(name));

            return this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetHtml", tableName)), args).ToString();
        }
        #endregion
    }
}
