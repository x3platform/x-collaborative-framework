
#region Using Libraries
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.DigitalNumber.Configuration
{
    /// <summary>流水号配置信息</summary>
    public class DigitalNumberConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "DigitalNumber";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "digitalNumber";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
