namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public class AccountBindingWrapper
    {
        /// <summary>数据服务</summary>
        private IAccountBindingService service = MembershipManagement.Instance.AccountBindingService;

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = XmlHelper.Fetch("accountId", doc);
            string bindingType = XmlHelper.Fetch("bindingType", doc);

            AccountBindingInfo param = this.service.FindOne(accountId, bindingType);

            outString.Append("{\"data\":" + AjaxUtil.Parse<AccountBindingInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByAccountId(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAllByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = XmlHelper.Fetch("accountId", doc);

            IList<AccountBindingInfo> list = this.service.FindAllByAccountId(accountId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<AccountBindingInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:Bind(XmlDocument doc)
        /// <summary>绑定记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Bind(XmlDocument doc)
        {
            string accountId = KernelContext.Current.User.Id;

            string bindingType = XmlHelper.Fetch("bindingType", doc);
            string bindingObjectId = XmlHelper.Fetch("bindingObjectId", doc);
            string bindingOptions = XmlHelper.Fetch("bindingOptions", doc);

            this.service.Bind(accountId, bindingType, bindingObjectId, bindingOptions);

            return "{\"message\":{\"returnCode\":0,\"value\":\"绑定成功。\"}}";
        }
        #endregion

        #region 函数:Unbind(XmlDocument doc)
        /// <summary>绑定记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Unbind(XmlDocument doc)
        {
            string accountId = KernelContext.Current.User.Id;

            string bindingType = XmlHelper.Fetch("bindingType", doc);
            string bindingObjectId = XmlHelper.Fetch("bindingObjectId", doc);

            this.service.Unbind(accountId, bindingType, bindingObjectId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"已解除绑定。\"}}";
        }
        #endregion
    }
}
