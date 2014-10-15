#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IJobGradeInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership
{
    /// <summary>ְ����Ϣ</summary>
    public interface IJobGradeInfo : IAuthorizationObject
    {
        #region 属性:DisplayName
        /// <summary>ȫ������</summary>
        string DisplayName { get; }
        #endregion

        #region 属性:Value
        /// <summary>ְ����ֵ</summary>
        int Value { get; set; }
        #endregion
    }
}
