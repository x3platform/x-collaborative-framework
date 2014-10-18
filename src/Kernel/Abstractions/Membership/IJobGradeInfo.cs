namespace X3Platform.Membership
{
    /// <summary>职级信息</summary>
    public interface IJobGradeInfo : IAuthorizationObject
    {
        #region 属性:DisplayName
        /// <summary>全局名称</summary>
        string DisplayName { get; }
        #endregion

        #region 属性:Value
        /// <summary>职级的值</summary>
        int Value { get; set; }
        #endregion
    }
}
