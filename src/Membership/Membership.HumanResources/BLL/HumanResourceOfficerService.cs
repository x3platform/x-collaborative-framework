namespace X3Platform.Membership.HumanResources.BLL
{
    using System;
    using X3Platform.Spring;
    using X3Platform.Membership.Model;
    using X3Platform.Util;

    using X3Platform.Membership.HumanResources.IDAL;
    using X3Platform.Membership.HumanResources.Configuration;
    using X3Platform.Membership.HumanResources.IBLL;
    using System.Collections.Generic;

    public class HumanResourceOfficerService : IHumanResourceOfficerService
    {
        private HumanResourcesConfiguration configuration = null;

        private IMemberExtensionInformationProvider m_MemberExtensionInformationProvider = null;

        public IMemberExtensionInformationProvider MemberExtensionInformationProvider
        {
            get { return m_MemberExtensionInformationProvider; }
        }

        public HumanResourceOfficerService()
        {
            configuration = HumanResourcesConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(HumanResourcesConfiguration.ApplicationName, springObjectFile);

            this.m_MemberExtensionInformationProvider = objectBuilder.GetObject<IMemberExtensionInformationProvider>(typeof(IMemberExtensionInformationProvider));
        }

        public bool IsHumanResourceOfficer(IAccountInfo account)
        {
            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAllByAccountId(account.Id);

            foreach (IRoleInfo item in list) 
            {
                if (item.Name.IndexOf("人力资源") > -1) { return true; }
            }

            return false;
        }

        #region 函数:GrantApply(string grantorId, string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string remark)
        /// <summary>委托申请</summary>
        /// <param name="grantorId">委托人</param>
        /// <param name="granteeId">被委托人</param>
        /// <param name="grantedTimeFrom">委托结束时间</param>
        /// <param name="grantedTimeTo">委托开始时间</param>
        /// <param name="remark">备注</param>
        public void GrantApply(string grantorId, string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string remark)
        {
            AccountGrantInfo param = new AccountGrantInfo();

            param.Id = StringHelper.ToGuid();

            param.GrantorId = grantorId;
            param.GranteeId = granteeId;
            param.GrantedTimeFrom = grantedTimeFrom;
            param.GrantedTimeTo = grantedTimeTo;
            param.Remark = remark;

            MembershipManagement.Instance.AccountGrantService.Save(param);
        }
        #endregion
    }
}