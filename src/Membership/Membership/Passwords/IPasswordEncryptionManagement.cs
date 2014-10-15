namespace X3Platform.Membership.Passwords
{
    using System;
    using System.Security;
    using System.Security.Principal;

    using X3Platform.Spring;
    using X3Platform.Membership;

    /// <summary>密码加密类型</summary>
    [SpringObject("X3Platform.Membership.Passwords.IPasswordEncryptionManagement")]
    public interface IPasswordEncryptionManagement
    {
        #region 属性:Name
        /// <summary>名称</summary>
        string Name { get; }
        #endregion

        #region 函数:Decrypt(string loginName)
        /// <summary>解密密码</summary>
        /// <returns></returns>
        string Decrypt(string encryptedPassword);
        #endregion

        #region 函数:Encrypt(string password)
        /// <summary>加密密码</summary>
        /// <param name="unencryptedPassword">未加密的密码</param>
        /// <returns></returns>
        string Encrypt(string unencryptedPassword);
        #endregion
    }
}