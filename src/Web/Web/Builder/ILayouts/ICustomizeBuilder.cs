namespace X3Platform.Web.Builder.ILayouts
{
    #region Using Libraries
    using X3Platform.Membership;
    #endregion

    /// <summary>�Զ���ҳ�湹����</summary>
    public interface ICustomizeBuilder
    {
        #region ����:ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        /// <summary>����ҳ��</summary>
        /// <param name="authorizationObjectType">��Ȩ�������</param>
        /// <param name="authorizationObjectId">��Ȩ�����ʶ</param>
        /// <param name="pageName">ҳ������</param>
        /// <returns></returns>
        string ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName);
        #endregion

        #region ����:ParsePage(IAccountInfo account, string pageName)
        /// <summary>����ҳ��</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="pageName">ҳ������</param>
        /// <returns></returns>
        string ParsePage(IAccountInfo account, string pageName);
        #endregion

        #region ����:ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        /// <summary>������ҳ</summary>
        /// <param name="authorizationObjectType">��Ȩ�������</param>
        /// <param name="authorizationObjectId">��Ȩ�����ʶ</param>
        /// <returns></returns>
        string ParseHomePage(string authorizationObjectType, string authorizationObjectId);
        #endregion

        #region ����:ParseHomePage(IAccountInfo account)
        /// <summary>������ҳ</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns></returns>
        string ParseHomePage(IAccountInfo account);
        #endregion
    }
}