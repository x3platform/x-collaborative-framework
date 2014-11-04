namespace X3Platform.Web.UserAgents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using X3Platform.Yaml.RepresentationModel;

    public class RegexConfig
    {
        public YamlSequenceNode UserAgentParsers { get; private set; }
        
        public YamlSequenceNode OSParsers { get; private set; }
        
        public YamlSequenceNode DeviceParsers { get; private set; }

        public RegexConfig(IDictionary<string, YamlSequenceNode> dictionary)
        {
            this.UserAgentParsers = dictionary.Find("user_agent_parsers");

            this.OSParsers = dictionary.Find("os_parsers");

            this.DeviceParsers = dictionary.Find("device_parsers");
        }
    }
}
