namespace X3Platform.Membership
{
    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CacheBuffer;

    /// <summary>计算机信息</summary>
    public interface IComputerInfo
    {
        /// <summary>标识</summary>
        string Id { get; set; }

        /// <summary>名称</summary>
        string Name { get; set; }

        /// <summary>IP 地址</summary>
        string IP { get; set; }

        /// <summary>MAC 地址</summary>
        string MAC { get; set; }
    }
}
