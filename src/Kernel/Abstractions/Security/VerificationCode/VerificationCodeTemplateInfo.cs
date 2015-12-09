namespace X3Platform.Security.VerificationCode
{
    #region Using Libraries
    using System;
    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>验证码模板信息</summary>
    public class VerificationCodeTemplateInfo
    {
        /// <summary>构造函数</summary>
        public VerificationCodeTemplateInfo() { }

        #region 属性:ObjectType
        private string m_ObjectType;

        /// <summary></summary>
        public string ObjectType
        {
            get { return this.m_ObjectType; }
            set { this.m_ObjectType = value; }
        }
        #endregion

        #region 属性:ValidationType
        private string m_ValidationType;

        /// <summary></summary>
        public string ValidationType
        {
            get { return this.m_ValidationType; }
            set { this.m_ValidationType = value; }
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

        #region 属性:Options
        private string m_Options;

        /// <summary>邮件主题</summary>
        public string Options
        {
            get { return this.m_Options; }
            set { this.m_Options = value; }
        }
        #endregion
    }
}
