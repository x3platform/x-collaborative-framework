
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
    /// <summary>��ˮ��������Ϣ</summary>
    public class DigitalNumberConfiguration : XmlConfiguraton
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "DigitalNumber";

        /// <summary>������������</summary>
        public const string SectionName = "digitalNumber";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
