
#region Using Libraries
using Common.Logging;

using X3Platform.Configuration;
using System.IO;
using X3Platform.Yaml.RepresentationModel;
#endregion

namespace X3Platform.ActiveDirectory.Configuration
{
    /// <summary>ActiveDirectory ������Ϣ</summary>
    public class ActiveDirectoryConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>������������</summary>
        public const string SectionName = "activeDirectory";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        public ActiveDirectoryConfiguration()
        {
            using (var stream = typeof(ActiveDirectoryConfiguration).Assembly.GetManifestResourceStream("X3Platform.ActiveDirectory.defaults.ActiveDirectory.yaml"))
            {
                using (var reader = new StreamReader(stream))
                {
                    // ��������
                    var yaml = new YamlStream();

                    yaml.Load(reader);

                    // ���ø��ڵ�
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;

                    // ���ؼ���:Keys
                    YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);
                }
            }

            this.Initialized = true;
        }
    }
}
