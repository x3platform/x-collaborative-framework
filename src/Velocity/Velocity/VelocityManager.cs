
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

    /// <summary>Velocityģ�������</summary>
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

        /// <summary>�ϲ�</summary>
        /// <param name="context">�����Ļ���</param>
        /// <param name="templatePath">ģ�����·��</param>
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
                // �����ڲ�����, ��������.
                instance = null;

                engine = null;

                return string.Empty;
            }
        }

        /// <summary>�ϲ�</summary>
        /// <param name="context">�����Ļ���</param>
        /// <param name="templatePath">ģ�����·��</param>
        /// <returns></returns>
        public string Merge(VelocityContext context, Template template)
        {
            StringWriter writer = new StringWriter();

            template.Merge(context, writer);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>��ֵ</summary>
        /// <param name="context">�����Ļ���</param>
        /// <param name="templateValue">ģ����Ϣ</param>
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
                // �����ڲ�����, ��������.
                instance = null;

                engine = null;

                throw;
            }
        }

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        private object lockDictionaryObject = new object();

        /// <summary>����ģ������·��</summary>
        /// <param name="context">�����Ļ���</param>
        /// <param name="templateValue">ģ����Ϣ</param>
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

        /// <summary>��ȡģ����Ϣ</summary>
        /// <param name="virtualPath">ģ����Ϣ</param>
        /// <returns></returns>
        public Template GetTemplateByVirtualPath(string virtualPath)
        {
            Runtime.Directive.ParseTemplateVirtualPath.VirtualPathTemplate template = new Runtime.Directive.ParseTemplateVirtualPath.VirtualPathTemplate();

            template.Load(engine, virtualPath);

            return template;
        }
    }
}