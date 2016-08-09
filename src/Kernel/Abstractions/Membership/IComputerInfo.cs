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
        /// <summary>计算机唯一标识</summary>
        string Id { get; set; }

        /// <summary>名称</summary>
        string Name { get; set; }

        /// <summary>类型</summary>
        string Type { get; set; }

        /// <summary>IP 地址</summary>
        string IP { get; set; }

        /// <summary>MAC 地址</summary>
        string MAC { get; set; }

        #region 属性:DistinguishedName
        /// <summary>LDAP 唯一识别名称</summary>
        string DistinguishedName { get; set; }
        #endregion
    }
}
