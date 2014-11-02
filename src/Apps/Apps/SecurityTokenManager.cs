namespace X3Platform.Apps
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;

    using X3Platform.Security;
    using X3Platform.Util;

    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    #endregion

    /// <summary>安全标记管理</summary>
    public static class SecurityTokenManager
    {
        /// <summary></summary>
        public static string Authenticate(string securityToken)
        {
            Hashtable table = JsonHelper.ToHashtable(securityToken);

            string applicationId = table["applicationId"].ToString();

            applicationId = StringHelper.ToGuid(new Guid(applicationId));

            string applicationSecretSignal = table["applicationSecretSignal"].ToString();

            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentNullException("securityToken.applicationId");
            }
            else
            {
                ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOne(applicationId);

                if (param == null)
                {
                    throw new NullReferenceException("securityToken.applicationId");
                }
                else
                {
                    if (param.ApplicationSecret == Encrypter.DecryptAES(applicationSecretSignal, param.ApplicationKey, param.ApplicationKey))
                    {
                        return applicationId;
                    }
                }
            }

            return "Unkwon SecurityToken";
        }

        /// <summary></summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static string CreateSecurityToken(string applicationId)
        {
            ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOne(applicationId);

            // 时间戳
            var timestamp = DateHelper.GetTimestamp();
            // 随机数
            var nonce = DigitalNumberContext.Generate("Key_Nonce");
            // 签名
            var clientSignature = Encrypter.EncryptSHA1(Encrypter.SortAndConcat(
                param.ApplicationSecret,
                timestamp.ToString(),
                nonce.ToString()));

            return "{\"clientId\":\"" + param.Id + "\","
                + "\"clientSignature\":\"" + clientSignature + "\","
                + "\"timestamp\":\"" + timestamp + "\","
                + "\"nonce\":\"" + nonce + "\"}";
        }
    }
}
