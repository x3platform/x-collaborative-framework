#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :RegionsConfiguration.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

#region Using Libraries
using System;
using System.Configuration;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
using X3Platform.Yaml.RepresentationModel;
#endregion

namespace X3Platform.Location.Regions.Configuration
{
    /// <summary>������Ϣ</summary>
    public class RegionsConfiguration : XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Regions";

        /// <summary>�������������</summary>
        public const string SectionName = "regions";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region ���캯��:RegionsConfiguration()
        public RegionsConfiguration()
        {
            // �������� YAML ��Դ�����ļ���ʼ��������Ϣ

            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Location.Regions.defaults.config.yaml");

            // ���� Keys ��ֵ������Ϣ
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
