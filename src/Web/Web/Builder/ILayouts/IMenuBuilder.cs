namespace X3Platform.Web.Builder.ILayouts
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Velocity;

    /// <summary>�Զ���˵��������ӿ�</summary>
    public interface IMenuBuilder
    {
        #region ����:GetMenu(IAccountInfo account, string applicationName)
        /// <summary>��ȡ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        string GetMenu(IAccountInfo account, string applicationName);
        #endregion

        #region ����:GetMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        /// <summary>��ȡ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <param name="parentMenuFullPath">�����˵�������·��</param>
        /// <returns></returns>
        string GetMenu(IAccountInfo account, string applicationName, string parentMenuFullPath);
        #endregion

        #region ����:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>��ȡ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <param name="options">�˵�ѡ��</param>
        /// <returns></returns>
        string GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options);
        #endregion
          
        #region ����:ParseMenu(IAccountInfo account, string menuName)
        /// <summary>�����˵�</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="menuName">�˵�����</param>
        /// <returns></returns>
        [Obsolete]
        string ParseMenu(IAccountInfo account, string menuName);
        #endregion

        #region ����:ParseMenu(IAccountInfo account, string menuName, bool isAdminToken)
        /// <summary>�����˵�</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="menuName">�˵�����</param>
        /// <param name="isAdminToken">�Ƿ��ǹ���Ա</param>
        /// <returns></returns>
        [Obsolete]
        string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken);
        #endregion

        #region ����:ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        /// <summary>�����˵�</summary>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <param name="menuName">�˵�����</param>
        /// <param name="isAdminToken">�Ƿ��ǹ���Ա</param>
        /// <returns></returns>
        string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context);
        #endregion
    }
}