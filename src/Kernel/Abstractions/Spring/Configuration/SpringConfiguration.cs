namespace X3Platform.Spring.Configuration
{
    #region Using Libraries
    using System;
    using System.Xml;

    using Common.Logging;

    using X3Platform.Configuration;
    #endregion

    /// <summary>Spring 配置信息</summary>
    public class SpringConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>配置区的名称</summary>
        public const string SectionName = "spring";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        private NameValueConfigurationCollection m_Files = new NameValueConfigurationCollection();

        /// <summary>配置文件列表</summary>
        public NameValueConfigurationCollection Files
        {
            get { return this.m_Files; }
        }

        #region 函数:Configure(XmlElement element)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="element">配置节点的Xml元素</param>
        public override void Configure(XmlElement element)
        {
            try
            {
                base.Configure(element);

                // 加载 Files 键值配置信息
                XmlConfiguratonOperator.SetKeyValues(this.Files, element.SelectNodes(@"files/add"));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion
    }
}
