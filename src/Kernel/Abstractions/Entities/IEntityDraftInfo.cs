namespace X3Platform.Entities
{
    /// <summary>实体对象草稿信息接口</summary>
    public interface IEntityDraftInfo : ISerializedObject
    {
        #region 属性:EnetiyId
        /// <summary></summary>
        string EnetiyId { get; set; }
        #endregion

        #region 属性:EntityClassName
        /// <summary></summary>
        string EntityClassName { get; set; }
        #endregion
    }
}
