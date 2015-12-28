namespace X3Platform.SMS.Client.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    using X3Platform.Membership;
    using X3Platform.Spring;
    using X3Platform.Security;
    using X3Platform.Security.Authority;

    using X3Platform.SMS.Client.Configuration;
    using X3Platform.SMS.Client.IBLL;
    using X3Platform.SMS.Client.IDAL;
    using Common.Logging;
    using X3Platform.Util;
    using X3Platform.Security.VerificationCode;
    using X3Platform.TemplateContent;
    using X3Platform.Configuration;
    using X3Platform.Json;
    using X3Platform.Location.IPQuery;
    #endregion

    /// <summary>短信服务</summary>
    public class SMSService : ISMSService
    {
        IShortMessageClientProvider client = null;

        ISMSProvider provider = null;

        #region 构造函数:SMSService()
        /// <summary>构造函数</summary>
        public SMSService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = SMSConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(SMSConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<ISMSProvider>(typeof(ISMSProvider));

            this.client = (IShortMessageClientProvider)KernelContext.CreateObject(SMSConfigurationView.Instance.ClientProvider);
        }
        #endregion

        /// <summary></summary>
        /// <param name="phoneNumber"></param>
        /// <param name="validationType"></param>
        /// <returns></returns>
        public int Send(string phoneNumber, string validationType)
        {
            return this.Send(string.Empty, phoneNumber, validationType);
        }

        /// <summary></summary>
        /// <param name="accountId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="validationType"></param>
        /// <returns></returns>
        public int Send(string accountId, string phoneNumber, string validationType)
        {
            if (string.IsNullOrEmpty(phoneNumber)) { throw new NullReferenceException(phoneNumber); }

            string objectType = VerificationObjectType.Mobile.ToString();

            // 获取验证码信息
            VerificationCodeInfo verificationCode = VerificationCodeContext.Instance.VerificationCodeService.Create(objectType, phoneNumber, validationType, IPQueryContext.GetClientIP());

            // 获取验证模板信息
            VerificationCodeTemplateInfo template = VerificationCodeContext.Instance.VerificationCodeTemplateService.FindOne(objectType, validationType);

            string messageContent = TemplateContentContext.Instance.TemplateContentService.GetHtml(template.TemplateContentName);

            string serialNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff") + StringHelper.ToRandom("0123456789", 3);

            // 设置短信内容
            ShortMessage message = new ShortMessage();

            message.SerialNumber = serialNumber;
            message.PhoneNumber = phoneNumber;
            message.MessageContent = string.Format(messageContent, verificationCode.Code);

            if (string.IsNullOrEmpty(template.Options)) { template.Options = "{}"; }

            // 在参数中加入验证码对象
            template.Options = template.Options.Insert(1, "\"code\":\"" + verificationCode.Code + "\",");

            // 设置短信模板代码
            JsonData data = JsonMapper.ToObject(template.Options);

            message.MessageTemplateCode = JsonHelper.GetDataValue(data, "templateCode");
            // message.MessageTemplateParam = "{\"code\":\"" + verificationCode.Code + "\",\"product\":\"" + JsonHelper.GetDataValue(data, "product") + "\"}";
            message.MessageTemplateParam = data.ToJson();

            // 发送短信
            string returnCode = client.Send(message);

            // 保存结果至数据库
            this.provider.Send(serialNumber, accountId, phoneNumber, message.MessageContent, returnCode);

            if (string.IsNullOrEmpty(returnCode)) return 0;

            return Convert.ToInt32(returnCode);
        }
    }
}
