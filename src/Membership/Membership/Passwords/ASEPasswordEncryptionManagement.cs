namespace X3Platform.Membership.Passwords
{
    using System;
    using System.Security;
    using System.Security.Principal;

    using X3Platform.Security;
    using X3Platform.Spring;

    using X3Platform.Membership;

    /// <summary>密码ASE可逆加密类型</summary>
    public class ASEPasswordEncryptionManagement : IPasswordEncryptionManagement
    {
        #region 属性:Name
        private string m_Name = "ASE";

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
            return Encrypter.DecryptAES(encryptedPassword);
        }
        #endregion

        #region 函数:Encrypt(string password)
        /// <summary>加密密码</summary>
        /// <param name="unencryptedPassword">未加密的密码</param>
        /// <returns></returns>
        public string Encrypt(string unencryptedPassword)
        {
            return Encrypter.EncryptAES(unencryptedPassword);
        }
        #endregion
    }
}