namespace X3Platform.Web.Builder.ILayouts
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;

    /// <summary>����������</summary>
    public interface INavigationBuilder
    {
        #region ����:GetStartMenu(IAccountInfo account)
        /// <summary>��ȡ��ʼ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetStartMenu(IAccountInfo account);
        #endregion

        #region ����:GetTopMenu(IAccountInfo account)
        /// <summary>��ȡ�����˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetTopMenu(IAccountInfo account);
        #endregion

        #region ����:GetBottomMenu(IAccountInfo account)
        /// <summary>��ȡ�ײ��˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetBottomMenu(IAccountInfo account);
        #endregion

        #region ����:GetShortcutMenu(IAccountInfo account)
        /// <summary>��ȡ��ݷ�ʽ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetShortcutMenu(IAccountInfo account);
        #endregion

        #region ����:GetApplicationMenu(IAccountInfo account, string applicationName)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="applicationName">Ӧ������</param>
        /// <returns></returns>
        string GetApplicationMenu(IAccountInfo account, string applicationName);
        #endregion

        #region ����:GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="applicationName">Ӧ������</param>
        /// <param name="parentMenuFullPath">�����˵�������·��</param>
        /// <returns></returns>
        string GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath);
        #endregion

        #region ����:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="applicationName">Ӧ������</param>
        /// <param name="options">�˵�ѡ��</param>
        /// <returns></returns>
        string GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options);
        #endregion

        #region ����:GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        /// <summary>��ȡӦ�ù��ܵ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName);
        #endregion
    }
}