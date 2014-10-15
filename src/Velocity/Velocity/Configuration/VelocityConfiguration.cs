
#region Using Libraries
using System;
using System.Collections.Generic;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.Velocity.Configuration
{
    /// <summary>模板区域的配置信息</summary>
    public class VelocityConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Velocity";

        /// <summary>配置区域的名称</summary>
        public const string SectionName = "velocity";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        private NameValueConfigurationCollection m_Keys = new NameValueConfigurationCollection();

        /// <summary>键值配置信息</summary>
        public NameValueConfigurationCollection Keys
        {
            get { return this.m_Keys; }
            set { this.m_Keys = value; }
        }

        #region 函数:Configure(XmlElement element)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="element">配置节点的Xml元素</param>
        public override void Configure(XmlElement element)
        {
            try
            {
                // 加载集合:Keys
                XmlNodeList nodes = element.SelectNodes(@"keys/add");

                foreach (XmlNode node in nodes)
                {
                    this.Keys.Add(new NameValueConfigurationElement(node.Attributes["name"].Value, node.Attributes["value"].Value));
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion
    }
}
