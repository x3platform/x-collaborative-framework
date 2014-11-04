#region Apache License, Version 2.0
// 
// Copyright 2014 Atif Aziz
// Portions Copyright 2012 Søren Enemærke
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace X3Platform.Web.UserAgents
{
    #region Imports
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using X3Platform.Yaml.RepresentationModel;
    #endregion

    public sealed class Parser
    {
        const string OTHER = "Other";

        private RegexConfig regexConfig = null;

        public RegexConfig GetRegexConfig() { return regexConfig; }

        readonly Func<string, OS> _osParser;
        readonly Func<string, Device> _deviceParser;
        readonly Func<string, UserAgent> _userAgentParser;

        Parser(string yaml)
        {
            var ys = new YamlStream();
            using (var reader = new StringReader(yaml))
                ys.Load(reader);

            var entries =
                from doc in ys.Documents
                select doc.RootNode as YamlMappingNode into rn
                where rn != null
                from e in rn.Children
                select new
                {
                    Key = e.Key as YamlScalarNode,
                    Value = e.Value as YamlSequenceNode
                } into e
                where e.Key != null && e.Value != null
                select e;

            var config = entries.ToDictionary(e => e.Key.Value,
                                              e => e.Value,
                                              StringComparer.OrdinalIgnoreCase);

            this.regexConfig = new RegexConfig(config);

            var defaultDevice = new Device(OTHER, isSpider: false);
        }

        public static Parser FromYaml(string yaml) { return new Parser(yaml); }

        public static Parser FromYamlFile(string path) { return new Parser(File.ReadAllText(path)); }

        public static Parser GetDefault()
        {
            using (var stream = typeof(Parser).Assembly.GetManifestResourceStream("X3Platform.Web.UserAgents.defaults.regexes.yaml"))

            using (var reader = new StreamReader(stream))
            {
                return new Parser(reader.ReadToEnd());
            }
        }

        public ClientInfo Parse(string uaString)
        {
            var os = ParseOS(uaString);
            var device = ParseDevice(uaString);
            var ua = ParseUserAgent(uaString);

            return new ClientInfo(os, device, ua);
        }

        public OS ParseOS(string uaString)
        {
            foreach (YamlMappingNode node in this.regexConfig.OSParsers)
            {
                var regex = new Regex(((YamlScalarNode)node.Children[new YamlScalarNode("regex")]).Value);

                var match = regex.Match(uaString);

                if (match.Success)
                {
                    var os = node.Children.ContainsKey(new YamlScalarNode("os_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("os_replacement")]).Value
                        : match.Groups[1].Value;
                    var v1 = node.Children.ContainsKey(new YamlScalarNode("os_v1_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("os_v1_replacement")]).Value
                        : null;
                    var v2 = node.Children.ContainsKey(new YamlScalarNode("os_v2_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("os_v2_replacement")]).Value
                        : null;

                    return new OS(os, v1, v2, null, null);
                }
            }

            return new OS(OTHER, null, null, null, null);
        }

        public Device ParseDevice(string uaString)
        {
            foreach (YamlMappingNode node in this.regexConfig.DeviceParsers)
            {
                var regex = new Regex(((YamlScalarNode)node.Children[new YamlScalarNode("regex")]).Value);

                var match = regex.Match(uaString);

                if (match.Success)
                {
                    var family = node.Children.ContainsKey(new YamlScalarNode("device_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("device_replacement")]).Value
                        : match.Groups[1].Value;

                    return new Device(family, false);
                }
            }

            return new Device(OTHER, isSpider: false);
        }

        public UserAgent ParseUserAgent(string uaString)
        {
            foreach (YamlMappingNode node in this.regexConfig.UserAgentParsers)
            {
                var regex = new Regex(((YamlScalarNode)node.Children[new YamlScalarNode("regex")]).Value);

                var match = regex.Match(uaString);

                if (match.Success)
                {
                    var family = node.Children.ContainsKey(new YamlScalarNode("family_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("family_replacement")]).Value
                        : match.Groups[1].Value;
                    var v1 = node.Children.ContainsKey(new YamlScalarNode("v1_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("v1_replacement")]).Value
                        : null;
                    var v2 = node.Children.ContainsKey(new YamlScalarNode("v2_replacement"))
                        ? ((YamlScalarNode)node.Children[new YamlScalarNode("v2_replacement")]).Value
                        : null;

                    return new UserAgent(family, v1, v2, null);
                }
            }

            return new UserAgent(OTHER, null, null, null);
        }
    }
}
