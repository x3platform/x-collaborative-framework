namespace X3Platform.Membership.Passwords
{
    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;

    using X3Platform.Spring;
    using X3Platform.Membership;
    using X3Platform.Security;

    /// <summary>密码SHAI不可逆加密类型</summary>
    public class SHA1PasswordEncryptionManagement : IPasswordEncryptionManagement
    {
        #region 属性:Name
        private string m_Name = "SHA1";

        /// <summary>名称</summary>
        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }
        #endregion

        #region 函数:Decrypt(string encryptedPassword)
        /// <summary>解密密码</summary>
        /// <param name="encryptedPassword">加密的密码</param>
        /// <returns>密码</returns>
        public string Decrypt(string encryptedPassword)
        {
            throw new NotSupportedException("SHA1 加密方式不支持解密。");
        }
        #endregion

        #region 函数:Encrypt(string unencryptedPassword)
        /// <summary>加密密码</summary>
        /// <param name="unencryptedPassword">未加密的密码</param>
        /// <returns>加密的密码</returns>
        public string Encrypt(string unencryptedPassword)
        {
            return Encrypter.EncryptSHA1(unencryptedPassword);
        }
        #endregion
    }
}