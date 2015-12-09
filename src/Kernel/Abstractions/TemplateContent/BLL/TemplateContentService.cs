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

    /// <summary>页面</summary>
    [SecurityClass]
    public class TemplateContentService : SecurityObject, ITemplateContentService
    {
        private ITemplateContentProvider provider = null;

        #region 构造函数:TemplateContentService()
        /// <summary>构造函数</summary>
        public TemplateContentService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = TemplateContentConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TemplateContentConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ITemplateContentProvider>(typeof(ITemplateContentProvider));
        }
        #endregion

        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TemplateContentInfo this[string name]
        {
            get { return this.FindOneByName(name); }
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
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:GetHtml(string name)
        /// <summary>获取Html文本</summary>
        /// <param name="name">区域划分模板名称</param>
        /// <returns>Html文本</returns>
        public string GetHtml(string name)
        {
            return this.provider.GetHtml(name);
        }
        #endregion
    }
}
