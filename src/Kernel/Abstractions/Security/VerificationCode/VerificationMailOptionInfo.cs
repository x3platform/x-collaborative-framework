namespace X3Platform.Security.VerificationCode
{
    #region Using Libraries
    using System;
    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>验证邮件选项信息</summary>
    public class VerificationMailOptionInfo
    {
        /// <summary>构造函数</summary>
        public VerificationMailOptionInfo() { }

        #region 属性:ValidationType
        private string m_ValidationType;

        /// <summary></summary>
        public string ValidationType
        {
            get { return this.m_ValidationType; }
            set { this.m_ValidationType = value; }
        }
        #endregion

        #region 属性:Subject
        private string m_Subject;

        /// <summary>邮件主题</summary>
        public string Subject
        {
            get { return this.m_Subject; }
            set { this.m_Subject = value; }
        }
        #endregion

        #region 属性:TemplateContentName
        private string m_TemplateContentName = null;

        /// <summary>模板内容名称</summary>
        public string TemplateContentName
        {
            get { return this.m_TemplateContentName; }
            set { this.m_TemplateContentName = value; }
        }
        #endregion
    }
}
