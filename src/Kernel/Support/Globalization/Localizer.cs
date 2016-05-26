using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace X3Platform.Globalization
{
    /// <summary>本地化翻译器</summary>
    public class Localizer
    {
        private XmlDocument doc = new XmlDocument();

        private string nodeName = null;

        /// <summary></summary>
        /// <param name="file"></param>
        /// <param name="nodeName"></param>
        public Localizer(string file, string nodeName)
        {
            if (File.Exists(file))
            {
                doc = new XmlDocument();

                doc.Load(file);

                string directiory = Path.GetDirectoryName(file);
                string fileName = Path.GetFileNameWithoutExtension(file);

                string[] files = Directory.GetFiles(directiory, string.Concat(fileName, "*"));

                foreach (string tempFile in files)
                {
                    if (tempFile != file)
                    {
                        XmlDocument tempDoc = new XmlDocument();

                        tempDoc.Load(tempFile);

                        XmlNodeList nodes = tempDoc.DocumentElement.ChildNodes;

                        foreach (XmlNode node in nodes)
                        {
                            XmlNode importNode = doc.ImportNode(node, true);

                            doc.DocumentElement.AppendChild(importNode);
                        }
                    }
                }
            }

            this.nodeName = nodeName;
        }

        /// <summary>获取文本信息</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetText(string name)
        {
            XmlNode node = doc.SelectSingleNode(string.Concat("resources/", nodeName, "[@name='", name, "']"));

            return node == null ? null : node.InnerText;
        }

        /// <summary>获取文本信息</summary>
        /// <param name="applicationName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetText(string applicationName, string name)
        {
            XmlNode node = doc.SelectSingleNode(string.Concat("resources/application[@name='", applicationName, "']/", nodeName, "[@name='", name, "']"));

            return node == null ? null : node.InnerText;
        }
    }
}
