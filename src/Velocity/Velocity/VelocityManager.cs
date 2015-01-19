
namespace X3Platform.Velocity
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using X3Platform.Plugins;
    using X3Platform.Velocity.App;
    using X3Platform.Velocity.Configuration;
    using X3Platform.Velocity.Runtime;
    using X3Platform.Web;

    /// <summary>Velocity模板管理器</summary>
    public class VelocityManager : CustomPlugin
    {
        private static VelocityManager instance = new VelocityManager();

        private static object lockObject = new object();

        public static VelocityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new VelocityManager();
                        }
                    }
                }

                return instance;
            }
        }

        private VelocityEngine engine = null;

        public VelocityEngine Engine
        {
            get { return this.engine; }
        }

        private VelocityConfiguration configuration = null;

        private VelocityManager()
        {
            Reload();
        }

        public override int Restart()
        {
            try
            {
                Reload();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        public void Reload()
        {
            this.engine = new VelocityEngine();

            engine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            engine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, VelocityConfigurationView.Instance.TemplatePath);

            engine.Init();
        }

        /// <summary>合并</summary>
        /// <param name="context">上下文环境</param>
        /// <param name="templatePath">模板相对路径</param>
        /// <returns></returns>
        public string Merge(VelocityContext context, string templatePath)
        {
            try
            {
                if (engine.TemplateExists(templatePath))
                {
                    Template template = engine.GetTemplate(templatePath);

                    return Merge(context, template);
                }
                else
                {
                    return "Can't find \"" + templatePath + "\".";
                }
            }
            catch
            {
                // 发生内部错误, 重启引擎.
                instance = null;

                engine = null;

                return string.Empty;
            }
        }

        /// <summary>合并</summary>
        /// <param name="context">上下文环境</param>
        /// <param name="templatePath">模板相对路径</param>
        /// <returns></returns>
        public string Merge(VelocityContext context, Template template)
        {
            StringWriter writer = new StringWriter();

            template.Merge(context, writer);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>求值</summary>
        /// <param name="context">上下文环境</param>
        /// <param name="templateValue">模板信息</param>
        /// <returns></returns>
        public string Evaluate(VelocityContext context, string templateValue)
        {
            try
            {
                StringWriter writer = new StringWriter();

                engine.Evaluate(context, writer, string.Empty, templateValue);

                return writer.GetStringBuilder().ToString();
            }
            catch
            {
                // 发生内部错误, 重启引擎.
                instance = null;

                engine = null;

                throw;
            }
        }

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        private object lockDictionaryObject = new object();

        /// <summary>解析模板虚拟路径</summary>
        /// <param name="context">上下文环境</param>
        /// <param name="templateValue">模板信息</param>
        /// <returns></returns>
        public string ParseTemplateVirtualPath(VelocityContext context, string virtualPath)
        {
            string templateValue = null;

            if (VelocityConfigurationView.Instance.TemplateCacheMode == "ON")
            {
                lock (lockDictionaryObject)
                {
                    if (dictionary.ContainsKey(virtualPath))
                    {
                        templateValue = dictionary[virtualPath];
                    }
                    else
                    {
                        templateValue = File.ReadAllText(VirtualPathHelper.GetPhysicalPath(virtualPath));

                        dictionary.Add(virtualPath, templateValue);
                    }
                }
            }
            else
            {
                templateValue = File.ReadAllText(VirtualPathHelper.GetPhysicalPath(virtualPath));
            }

            return Evaluate(context, templateValue);
        }

        /// <summary>获取模板信息</summary>
        /// <param name="virtualPath">模板信息</param>
        /// <returns></returns>
        public Template GetTemplateByVirtualPath(string virtualPath)
        {
            Runtime.Directive.ParseTemplateVirtualPath.VirtualPathTemplate template = new Runtime.Directive.ParseTemplateVirtualPath.VirtualPathTemplate();

            template.Load(engine, virtualPath);

            return template;
        }
    }
}