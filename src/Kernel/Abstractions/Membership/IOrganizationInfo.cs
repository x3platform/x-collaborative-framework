namespace X3Platform.Membership
{
    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CacheBuffer;

    /// <summary>组织信息</summary>
    public interface IOrganizationInfo
    {
        /// <summary>标识</summary>
        string Id { get; set; }

        /// <summary>名称</summary>
        string Name { get; set; }

        /// <summary>域</summary>
        string Domain { get; set; }

        /// <summary>模式</summary>
        string Pattern { get; set; }

    }
}
