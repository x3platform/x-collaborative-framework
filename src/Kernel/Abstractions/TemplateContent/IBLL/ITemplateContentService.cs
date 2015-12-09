namespace X3Platform.TemplateContent.IBLL
{
    #region Using Libraries
    using System.Collections.Generic;
    using X3Platform.Data;
    using X3Platform.Spring;
    using X3Platform.TemplateContent.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.TemplateContent.IBLL.ITemplateContentService")]
    public interface ITemplateContentService
    {
        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TemplateContentInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个 TemplateContentInfo 实例的详细信息</returns>
        TemplateContentInfo FindOneByName(string name);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:GetHtml(string name)
        /// <summary>获取Html文本</summary>
        /// <param name="name">区域划分模板名称</param>
        /// <returns>Html文本</returns>
        string GetHtml(string name);
        #endregion
    }
}