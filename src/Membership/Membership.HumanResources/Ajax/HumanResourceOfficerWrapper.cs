namespace X3Platform.Membership.HumanResources.Ajax
{
    using System;
    using System.Xml;
    using X3Platform.Ajax;
    using X3Platform.Location.IPQuery;

    using X3Platform.Membership.Model;
    using X3Platform.Membership.HumanResources.IBLL;
    using X3Platform.Membership.HumanResources.Model;
    using X3Platform.Util;

    public class HumanResourceOfficerWrapper : ContextWrapper
    {
        private IHumanResourceOfficerService service = HumanResourceManagement.Instance.HumanResourceOfficerService;

        #region 函数:RegisterMember(XmlDocument doc)
        /// <summary>注册人员信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string RegisterMember(XmlDocument doc)
        {
            IAccountInfo account = KernelContext.Current.User;

            if (!HumanResourceManagement.IsHumanResourceOfficer(account))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"只有人力资源管理应用的管理员才能设置此信息。\"}}";
            }

            MemberInfo member = new MemberInfo();

            MemberExtensionInformation memberProperties = new MemberExtensionInformation();

            member = (MemberInfo)AjaxUtil.Deserialize(member, doc);

            member.ExtensionInformation.Load(doc);

            if (string.IsNullOrEmpty(member.AccountId))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写相关帐号标识。\"}}";
            }
            else
            {
                // 创建新的空白帐号信息并保存到数据库
                MembershipManagement.Instance.AccountService.Save(MembershipManagement.Instance.AccountService.CreateEmptyAccount(member.AccountId));

                member.Account.Code = XmlHelper.Fetch("code", doc);
                member.Account.Name = XmlHelper.Fetch("name", doc);
                member.Account.IdentityCard = XmlHelper.Fetch("identityCard", doc);
            }

            HumanResourceManagement.Instance.GeneralAccountService.SetMemberCard(member);

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(member.AccountId, "创建用户信息", "【" + account.Name + "】创建了【" + member.Account.Name + "】的个人信息，【IP:" + IPQueryContext.GetClientIP() + "】。", account.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"创建用户信息成功。\"}}";
        }
        #endregion

        #region 函数:SetMemberCard(XmlDocument doc)
        /// <summary>设置人员卡片信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SetMemberCard(XmlDocument doc)
        {
            IAccountInfo account = KernelContext.Current.User;

            if (!HumanResourceManagement.IsHumanResourceOfficer(account))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"只有人力资源管理应用的管理员才能设置此信息。\"}}";
            }

            MemberInfo member = new MemberInfo();

            MemberExtensionInformation memberProperties = new MemberExtensionInformation();

            member = (MemberInfo)AjaxUtil.Deserialize(member, doc);

            member.ExtensionInformation.Load(doc);

            if (string.IsNullOrEmpty(member.AccountId))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写相关帐号标识。\"}}";
            }
            else
            {
                member.Account.Code = XmlHelper.Fetch("code", doc);
                member.Account.IdentityCard = XmlHelper.Fetch("identityCard", doc);
            }

            string partTimeJobsText = XmlHelper.Fetch("partTimeJobsText", doc);
          
            this.ResetPartTimeJobss(member, partTimeJobsText);

            HumanResourceManagement.Instance.GeneralAccountService.SetMemberCard(member);

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(member.AccountId, "设置用户信息", "【" + account.Name + "】更新了【" + member.Account.Name + "】的个人信息，【IP:" + IPQueryContext.GetClientIP() + "】。", account.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置用户信息成功。\"}}";
        }
        #endregion

        /// <summary></summary>
        /// <param name="relationType"></param>
        /// <param name="relationText"></param>
        private void ResetPartTimeJobss(IMemberInfo member, string partTimeJobsText)
        {
            string[] list = partTimeJobsText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            member.PartTimeJobs.Clear();

            // 设置组织关系

            foreach (string item in list)
            {
                string[] keys = item.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                if (keys.Length > 2 && keys[0] == "assignedJob")
                {
                    AssignedJobInfo assignedJob = new AssignedJobInfo();

                    assignedJob.Id = keys[1];
                    assignedJob.Name = keys[1];

                    member.PartTimeJobs.Add(assignedJob);
                }
            }
        }

        #region 函数:GrantApply(XmlDocument doc)
        /// <summary>委托申请</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("grantApply")]
        public string GrantApply(XmlDocument doc)
        {
            string grantorId = XmlHelper.Fetch("grantorId", doc);

            string granteeId = XmlHelper.Fetch("granteeId", doc);

            DateTime grantedTimeFrom = Convert.ToDateTime(XmlHelper.Fetch("grantedTimeFrom", doc));

            DateTime grantedTimeTo = Convert.ToDateTime(XmlHelper.Fetch("grantedTimeTo", doc));

            string remark = XmlHelper.Fetch("remark", doc);

            this.service.GrantApply(grantorId, granteeId, grantedTimeFrom, grantedTimeTo, remark);

            return "{message:{\"returnCode\":0,\"value\":\"申请成功。\"}}";
        }
        #endregion
    }
}