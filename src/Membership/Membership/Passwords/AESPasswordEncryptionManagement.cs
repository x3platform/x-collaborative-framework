namespace X3Platform.Membership.Passwords
{
    using System;
    using System.Security;
    using System.Security.Principal;

    using X3Platform.Security;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using System.Text;

    /// <summary>密码AES可逆加密类型</summary>
    public class AESPasswordEncryptionManagement : IPasswordEncryptionManagement
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

        private byte[] key = null;

        private byte[] iv = null;

        private void LoadKey()
        {
            if (key == null)
            {
                if (MembershipConfigurationView.Instance.PasswordEncryptionKey.Length == 32)
                {
                    this.key = UTF8Encoding.UTF8.GetBytes(MembershipConfigurationView.Instance.PasswordEncryptionKey);
                }
                else
                {
                    throw new Exception("ASE密码加密方式必须填写长度为32的密钥");
                }

                if (MembershipConfigurationView.Instance.PasswordEncryptionIV.Length > 0)
                {
                    this.iv = UTF8Encoding.UTF8.GetBytes(MembershipConfigurationView.Instance.PasswordEncryptionIV);
                }
            }
        }

        #region 函数:Decrypt(string encryptedPassword)
        /// <summary>解密密码</summary>
        /// <param name="encryptedPassword">加密的密码</param>
        /// <returns></returns>
        public string Decrypt(string encryptedPassword)
        {
            this.LoadKey();

            return Encrypter.DecryptAES(encryptedPassword, key, iv, CiphertextFormat.HexString);
        }
        #endregion

        #region 函数:Encrypt(string password)
        /// <summary>加密密码</summary>
        /// <param name="unencryptedPassword">未加密的密码</param>
        /// <returns></returns>
        public string Encrypt(string unencryptedPassword)
        {
            this.LoadKey();

            return Encrypter.EncryptAES(unencryptedPassword, key, iv, CiphertextFormat.HexString);
        }
        #endregion
    }
}