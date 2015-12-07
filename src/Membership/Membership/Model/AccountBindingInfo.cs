namespace X3Platform.Membership.Model
{ 
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary></summary>
    public class AccountBindingInfo
    {
        #region 默认构造函数:AccountBindingInfo()
        /// <summary>默认构造函数</summary>
        public AccountBindingInfo()
        {
        }
        #endregion

        #region 属性:Id
        /// <summary></summary>
        public string Id { get; set; }
        #endregion

        #region 属性:AccountId
        /// <summary></summary>
        public string AccountId { get; set; }
        #endregion

        #region 属性:BindingType
        /// <summary></summary>
        public string BindingType { get; set; }
        #endregion

        #region 属性:BindingObjectId
        /// <summary></summary>
        public string BindingObjectId { get; set; }
        #endregion

        #region 属性:BindingOptions
        /// <summary></summary>
        public string BindingOptions { get; set; }
        #endregion

        #region 属性:CreatedDate
        /// <summary></summary>
        public DateTime CreatedDate { get; set; }
        #endregion
    }
}