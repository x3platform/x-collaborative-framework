namespace X3Platform.Apps.Security
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Management;
    using System.Security.Cryptography;

    /// <summary>��ȫ���ƿͻ���</summary>
    public class SecurityTokenClient
    {
        /// <summary>������ȫ����</summary>
        /// <param name="appKey">Ӧ����ɺ�</param>
        /// <param name="appSecret">Ӧ����Կ</param>
        /// <param name="timestamp">ʱ���</param>
        /// <param name="nonce">�����</param>
        /// <returns>����ǩ��</returns>
        public static string CreateSecurityToken(string appSecret, string timestamp, string nonce)
        {
            // ���ַ������������ƴ�ӳ�һ���ı���Ϣ
            ArrayList list = new ArrayList() ;

            list.Add(appSecret);
            list.Add(timestamp);
            list.Add(nonce);
            
            list.Sort();

            string text = string.Concat(list.ToArray());
      
            // SHA1 ����
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] result = sha1.ComputeHash(Encoding.Default.GetBytes(text));

            // ��� Signature
            return Encoding.Default.GetString(result);
        }
    }
}
