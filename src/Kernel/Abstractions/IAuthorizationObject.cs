namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>权限对象接口</summary>
    public interface IAuthorizationObject : ISerializedObject
    {
        #region 属性:Type
        /// <summary>类型</summary>
        string Type { get; }
        #endregion

        #region 属性:Id
        /// <summary>标识</summary>
        string Id { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        string Name { get; set; }
        #endregion

        #region 属性:Locking
        /// <summary>防止意外删除 0 不锁定 | 1 锁定(默认)</summary>
        int Locking { get; set; }
        #endregion

        #region 属性:Status
        /// <summary>状态</summary>
        int Status { get; set; }
        #endregion

        #region 属性:Remark
        /// <summary>备注</summary>
        string Remark { get; set; }
        #endregion

        #region 属性:ModifiedDate
        /// <summary>更新时间</summary>
        DateTime ModifiedDate { get; set; }
        #endregion
    }
}
