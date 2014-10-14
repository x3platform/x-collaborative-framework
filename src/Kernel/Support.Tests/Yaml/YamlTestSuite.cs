using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;
using System.Resources;
using System.Reflection;
using System.IO;
using X3Platform.Yaml.RepresentationModel;
using System.Diagnostics;

namespace X3Platform.Tests.Yaml
{
    /// <summary>字符资源加载工具类</summary>
    [TestClass]
    public class YamlTests
    {
        [TestMethod]
        public void TestLoadStream()
        {
            //test_data_key_value.default.yaml
            // using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("X3Platform.Support.Tests.defaults.test_data.default.yaml"))
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("X3Platform.Support.Tests.defaults.test_data_key_value.default.yaml"))
            {
                var reader = new StreamReader(stream);
                // var input = reader.ReadToEnd();

                var result = "";

                Assert.IsNotNull(result);

                // Load the stream
                var yaml = new YamlStream();
                yaml.Load(reader);

                // Examine the stream
                var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                foreach (var entry in mapping.Children)
                {
                    Trace.WriteLine(((YamlScalarNode)entry.Key).Value);
                }

                // List all the keys
                var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("keys")];
                foreach (YamlMappingNode item in items)
                {
                    Trace.WriteLine(string.Format("{0}\t{1}",
                        item.Children[new YamlScalarNode("name")],
                        item.Children[new YamlScalarNode("descrip")]
                    ));
                }
            }
        }

        [TestMethod]
        public void TestLoadString()
        {
            var result = "";

            Assert.IsNotNull(result);

            // Setup the input
            var input = new StringReader(Document);

            // Load the stream
            var yaml = new YamlStream();
            yaml.Load(input);

            // Examine the stream
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            foreach (var entry in mapping.Children)
            {
                Trace.WriteLine(((YamlScalarNode)entry.Key).Value);
            }

            // List all the items
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("items")];
            foreach (YamlMappingNode item in items)
            {
                Trace.WriteLine(string.Format("{0}\t{1}",
                    item.Children[new YamlScalarNode("part_no")],
                    item.Children[new YamlScalarNode("descrip")]
                ));
            }
        }

        private const string Document = @"---
            receipt:    Oz-Ware Purchase Invoice
            date:        2007-08-06
            customer:
                given:   Dorothy
                family:  Gale

            items:
                - part_no:   A4786
                  descrip:   Water Bucket (Filled)
                  price:     1.47
                  quantity:  4

                - part_no:   E1628
                  descrip:   High Heeled ""Ruby"" Slippers
                  price:     100.27
                  quantity:  1

            bill-to:  &id001
                street: |
                        123 Tornado Alley
                        Suite 16
                city:   East Westville
                state:  KS

            ship-to:  *id001

            specialDelivery:  >
                Follow the Yellow Brick
                Road to the Emerald City.
                Pay no attention to the
                man behind the curtain.
...";
    }
}