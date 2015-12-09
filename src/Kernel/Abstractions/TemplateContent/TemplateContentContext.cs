namespace X3Platform.TemplateContent
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;
    using X3Platform.TemplateContent.IBLL;
    using X3Platform.TemplateContent.Configuration;

    /// <summary>自定义模板上下文</summary>
    public sealed class TemplateContentContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "页面自定义"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile TemplateContentContext instance = null;

        private static object lockObject = new object();

        public static TemplateContentContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TemplateContentContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:TemplateContentService
        private ITemplateContentService m_TemplateContentService = null;

        public ITemplateContentService TemplateContentService
        {
            get { return m_TemplateContentService; }
        }
        #endregion

        #region 构造函数:TemplateContentContext()
        /// <summary>构造函数</summary>
        private TemplateContentContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                Reload();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = TemplateContentConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TemplateContentConfiguration.ApplicationName, springObjectFile);

            this.m_TemplateContentService = objectBuilder.GetObject<ITemplateContentService>(typeof(ITemplateContentService));
        }
        #endregion
    }
}