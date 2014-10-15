namespace X3Platform.Membership.HumanResources.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;
    using X3Platform.Membership.HumanResources.IDAL;

    [SpringObject("X3Platform.Membership.HumanResources.IBLL.IHumanResourceOfficerService")]
    public interface IHumanResourceOfficerService
    {
        IMemberExtensionInformationProvider MemberExtensionInformationProvider { get; }

        bool IsHumanResourceOfficer(IAccountInfo account);
        
        #region 函数:GrantApply(string grantorId, string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string remark)
        /// <summary>委托申请</summary>
        /// <param name="grantorId">委托人</param>
        /// <param name="granteeId">被委托人</param>
        /// <param name="grantedTimeFrom">委托结束时间</param>
        /// <param name="grantedTimeTo">委托开始时间</param>
        /// <param name="remark">备注</param>
        void GrantApply(string grantorId, string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string remark);
        #endregion
    }
}
