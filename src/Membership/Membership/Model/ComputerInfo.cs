namespace X3Platform.Membership.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class ComputerInfo: IComputerInfo
    {
        #region 默认构造函数:ComputerInfo()
        /// <summary>默认构造函数</summary>
        public ComputerInfo()
        {
        }
        #endregion

        #region 属性:Id
        /// <summary>计算机唯一标识</summary>
        public  string Id { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        public string Name { get; set; }
        #endregion

        #region 属性:Type
        /// <summary>类型</summary>
        public string Type { get; set; }
        #endregion

        #region 属性:IP
        /// <summary>IP 地址</summary>
        public string IP { get; set; }
        #endregion

        #region 属性:MAC
        /// <summary>MAC 地址</summary>
        public string MAC { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>LDAP 唯一识别名称</summary>
        public string DistinguishedName { get; set; }
        #endregion

        #region 属性:ModifiedDate
        /// <summary>最近一次修改时间</summary>
        public DateTime ModifiedDate { get; set; }
        #endregion

        #region 属性:CreatedDate
        /// <summary>创建时间</summary>
        public DateTime CreatedDate { get; set; }
        #endregion
    }
}
