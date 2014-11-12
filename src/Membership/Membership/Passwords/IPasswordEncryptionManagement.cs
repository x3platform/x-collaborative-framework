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

        #region 函数:Decrypt(string encryptedPassword)
        /// <summary>解密密码</summary>
        /// <param name="encryptedPassword">加密的密码</param>
        /// <returns>密码</returns>
        string Decrypt(string encryptedPassword);
        #endregion

        #region 函数:Encrypt(string unencryptedPassword)
        /// <summary>加密密码</summary>
        /// <param name="unencryptedPassword">未加密的密码</param>
        /// <returns>加密的密码</returns>
        string Encrypt(string unencryptedPassword);
        #endregion
    }
}