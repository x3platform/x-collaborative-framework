namespace X3Platform.Web.Builder.Layouts.CollaborationPlatform
{
    using System;

    using X3Platform.Membership;
    using X3Platform.Web.Customize;
    using X3Platform.Web.Builder.ILayouts;

    public class CustomizeBuilder : CommonBuilder, ICustomizeBuilder
    {
        #region ����:ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        /// <summary>����ҳ��</summary>
        /// <param name="authorizationObjectType">��Ȩ�������</param>
        /// <param name="authorizationObjectId">��Ȩ�����ʶ</param>
        /// <param name="pageName">ҳ������</param>
        /// <returns></returns>
        public string ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        {
            try
            {
                return CustomizeContext.Instance.PageService.TryParseHtml(authorizationObjectType, authorizationObjectId, pageName);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region ����:ParsePage(IAccountInfo account, string pageName)
        /// <summary>����ҳ��</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="pageName">ҳ������</param>
        /// <returns></returns>
        public string ParsePage(IAccountInfo account, string pageName)
        {
            if (account == null)
            {
                return string.Empty;
            }

            return this.ParsePage("Account", account.Id, pageName);
        }
        #endregion

        #region ����:ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        /// <summary>������ҳ</summary>
        /// <param name="authorizationObjectType">��Ȩ�������</param>
        /// <param name="authorizationObjectId">��Ȩ�����ʶ</param>
        /// <returns></returns>
        public string ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        {
            return this.ParsePage(authorizationObjectType, authorizationObjectId, "HomePage");
        }
        #endregion

        #region ����:ParseHomePage(IAccountInfo account)
        /// <summary>������ҳ</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns></returns>
        public string ParseHomePage(IAccountInfo account)
        {
            return this.ParsePage(account, "HomePage");
        }
        #endregion
    }
}
