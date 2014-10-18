namespace X3Platform.Membership
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>扩展属性</summary>
    public interface IExtensionInformation
    {
        /// <summary>属性索引信息</summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        object this[string name] { get; set; }

        /// <summary>从Xml文档中加载扩展信息</summary>
        void Load(XmlDocument doc);

        /// <summary>根据参数从数据库中加载扩展信息</summary>
        void Load(Dictionary<string, object> args);

        /// <summary>保存</summary>
        void Save();

        /// <summary>删除</summary>
        void Delete();
    }
}
