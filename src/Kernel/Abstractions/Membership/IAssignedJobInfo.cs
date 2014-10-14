#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IAssignedJobInfo.cs
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
    /// <summary>��λ��Ϣ</summary>
    public interface IAssignedJobInfo : IAuthorizationObject
    {
        #region ����:OrganizationId
        /// <summary>������֯��ʶ</summary>
        string OrganizationId { get; set; }
        #endregion

        #region ����:Organization
        /// <summary>������֯��Ϣ</summary>
        IOrganizationInfo Organization { get; }
        #endregion

        #region ����:JobId
        /// <summary>����ְλ��ʶ</summary>
        string JobId { get; set; }
        #endregion

        #region ����:Job
        /// <summary>����ְλ��Ϣ</summary>
        IJobInfo Job { get; }
        #endregion

        #region ����:JobGradeId
        /// <summary>����ְ����ʶ</summary>
        string JobGradeId { get; set; }
        #endregion

        #region ����:Job
        /// <summary>����ְ����Ϣ</summary>
        IJobGradeInfo JobGrade { get; }
        #endregion
    }
}
