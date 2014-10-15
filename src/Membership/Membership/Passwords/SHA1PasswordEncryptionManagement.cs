namespace X3Platform.Membership.Passwords
{
    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;

    using X3Platform.Spring;
    using X3Platform.Membership;

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

        #region 函数:Decrypt(string loginName)
        /// <summary>解密密码</summary>
        /// <returns></returns>
        public string Decrypt(string encryptedPassword)
        {
            throw new NotSupportedException("SHA1 加密方式不支持解密。");
        }
        #endregion

        #region 函数:Encrypt(string password)
        /// <summary>加密密码</summary>
        /// <param name="unencryptedPassword">未加密的密码</param>
        /// <returns></returns>
        public string Encrypt(string unencryptedPassword)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(unencryptedPassword);

                byte[] encryptedPassword = sha1.ComputeHash(buffer);

                sha1.Clear();

                return Convert.ToBase64String(encryptedPassword);
            }
        }
        #endregion
    }
}