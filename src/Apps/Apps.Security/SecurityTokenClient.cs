namespace X3Platform.Apps.Security
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Management;
    using System.Security.Cryptography;

    /// <summary>安全令牌客户端</summary>
    public class SecurityTokenClient
    {
        /// <summary>创建安全令牌</summary>
        /// <param name="appKey">应用许可号</param>
        /// <param name="appSecret">应用密钥</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns>加密签名</returns>
        public static string CreateSecurityToken(string appSecret, string timestamp, string nonce)
        {
            // 将字符串数组排序后拼接成一个文本信息
            ArrayList list = new ArrayList() ;

            list.Add(appSecret);
            list.Add(timestamp);
            list.Add(nonce);
            
            list.Sort();

            string text = string.Concat(list.ToArray());
      
            // SHA1 加密
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] result = sha1.ComputeHash(Encoding.Default.GetBytes(text));

            // 输出 Signature
            return Encoding.Default.GetString(result);
        }
    }
}
