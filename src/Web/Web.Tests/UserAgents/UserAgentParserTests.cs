namespace X3Platform.Web.Tests.UserAgents
{
    using System;
    using System.Xml;
    using System.Resources;
    using System.Reflection;
    using System.Linq;

    using Common.Logging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Web.APIs.Configuration;
    using X3Platform.Web.UserAgents;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System.Diagnostics;
    using X3Platform.Yaml.RepresentationModel;

    /// <summary></summary>
    [TestClass]
    public class UserAgentParserTests
    {
        /// <summary>测试解析用户代理信息</summary>
        [TestMethod]
        public void TestParse()
        {
            var parser = Parser.GetDefault();

            ClientInfo client = null;

            client = parser.Parse("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0");

            Assert.AreEqual(client.OS.Family, "Windows 7");
            Assert.AreEqual(client.UserAgent.Family, "Firefox");

            // iPhone 4
            client = parser.Parse("Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_2_1 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8C148 Safari/6533.18.5");

            Assert.AreEqual(client.OS.Family, "iOS");
            Assert.AreEqual(client.UserAgent.Family, "Mobile Safari");

            // iPad 2
            client = parser.Parse("Mozilla/5.0 (iPad; CPU OS 4_3_5 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8L1 Safari/6533.18.5");

            Assert.AreEqual(client.OS.Family, "iOS");
            Assert.AreEqual(client.UserAgent.Family, "Mobile Safari");

            // Google Nexus 7
            client = parser.Parse("Mozilla/5.0 (Linux; Android 4.3; Nexus 7 Build/JSS15Q) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.72 Safari/537.36");

            Assert.AreEqual(client.OS.Family, "Android");
            Assert.AreEqual(client.UserAgent.Family, "Chrome");

            // Samsung Galaxy S4
            client = parser.Parse("Mozilla/5.0 (Linux; Android 4.2.2; GT-I9505 Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.59 Mobile Safari/537.36");

            Assert.AreEqual(client.OS.Family, "Android");
            Assert.AreEqual(client.UserAgent.Family, "Chrome Mobile");

            // Amazon Kindle Fire HDX 8.9"
            client = parser.Parse("Mozilla/5.0 (Linux; U; en-us; KFAPWI Build/JDQ39) AppleWebKit/535.19 (KHTML, like Gecko) Silk/3.13 Safari/535.19 Silk-Accelerated=true");

            Assert.AreEqual(client.Device.Family, "Kindle Fire HDX 8.9\" WiFi");
            Assert.AreEqual(client.OS.Family, "Android");
            Assert.AreEqual(client.UserAgent.Family, "Amazon Silk");
        }

        /// <summary>测试解析用户代理信息</summary>
        [TestMethod]
        public void TestParse1()
        {
            var parser = Parser.GetDefault();

            ClientInfo client = null;

            client = parser.Parse("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0");

            Assert.AreEqual(client.OS.Family, "Windows 7");
            Assert.AreEqual(client.UserAgent.Family, "Firefox");
        }

        [TestMethod]
        public void Test2()
        {
            // var uaString = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
            // var uaString = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_2_1 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8C148 Safari/6533.18.5";
            var uaString = "Mozilla/5.0 (Linux; U; en-us; KFAPWI Build/JDQ39) AppleWebKit/535.19 (KHTML, like Gecko) Silk/3.13 Safari/535.19 Silk-Accelerated=true";

            var parser = Parser.GetDefault();

            var config = parser.GetRegexConfig();

            foreach (YamlMappingNode node in config.OSParsers)
            {
                var regex = new Regex(((YamlScalarNode)node.Children[new YamlScalarNode("regex")]).Value);

                var match = regex.Match(uaString);

                if (match.Success)
                {
                    var os = ((YamlScalarNode)node.Children[new YamlScalarNode("os_replacement")]).Value;
                    var v1 = node.Children.ContainsKey(new YamlScalarNode("os_v1_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("os_v1_replacement")]).Value
                        : null;
                    var v2 = node.Children.ContainsKey(new YamlScalarNode("os_v2_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("os_v2_replacement")]).Value
                        : null;

                    // return m.Success ? binder(m, num) : default(T)
                    Console.WriteLine(((YamlScalarNode)node.Children[new YamlScalarNode("os_replacement")]).Value);

                    Console.WriteLine(new OS(os, v1, v2, null, null));
                    break;
                }
            }

            foreach (YamlMappingNode node in config.DeviceParsers)
            {
                var regex = new Regex(((YamlScalarNode)node.Children[new YamlScalarNode("regex")]).Value);

                var match = regex.Match(uaString);

                if (match.Success)
                {
                    var v1 = node.Children.ContainsKey(new YamlScalarNode("device_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("device_replacement")]).Value
                        : match.Groups[1].Value;
                   
                    Console.WriteLine(match.Groups[1].Value);

                    break;
                }
            }

            foreach (YamlMappingNode node in config.UserAgentParsers)
            {
                var regex = new Regex(((YamlScalarNode)node.Children[new YamlScalarNode("regex")]).Value);

                var match = regex.Match(uaString);

                if (match.Success)
                {
                    var family = ((YamlScalarNode)node.Children[new YamlScalarNode("family_replacement")]).Value;
                    var v1 = node.Children.ContainsKey(new YamlScalarNode("v1_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("v1_replacement")]).Value
                        : null;
                    var v2 = node.Children.ContainsKey(new YamlScalarNode("v2_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("v2_replacement")]).Value
                        : null;

                    // return m.Success ? binder(m, num) : default(T)
                    Console.WriteLine(((YamlScalarNode)node.Children[new YamlScalarNode("family_replacement")]).Value);

                    Console.WriteLine(new UserAgent(family, v1, v2, null));
                    break;
                }
            }
        }

        string MyTrace(string text)
        {
            Trace.WriteLine("text:" + text);

            return text;
        }

        public sealed class OS1
        {
            public OS1(string family, string major, string minor, string patch, string patchMinor)
            {
                Family = family;
                Major = major;
                Minor = minor;
                Patch = patch;
                PatchMinor = patchMinor;

                Trace.WriteLine("OS1:" + family);
            }

            public string Family { get; private set; }
            public string Major { get; private set; }
            public string Minor { get; private set; }
            public string Patch { get; private set; }
            public string PatchMinor { get; private set; }
        }

        Func<string, T> Create<T>(Regex regex, Func<Match, IEnumerator<int>, T> binder)
        {
            return input =>
            {
                var m = regex.Match(input);
                var num = Generate(1, n => n + 1);
                return m.Success ? binder(m, num) : default(T);
            };
        }

        IEnumerator<T> Generate<T>(T initial, Func<T, T> next)
        {
            for (var state = initial; ; state = next(state))
                yield return state;
            // ReSharper disable once FunctionNeverReturns
        }
    }
}