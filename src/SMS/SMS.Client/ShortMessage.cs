using System;
namespace X3Platform.SMS.Client
{
    /// <summary>短信信息</summary>
    public class ShortMessage
    {
        /// <summary>短信流水号</summary>
        public string SerialNumber { get; set; }

        /// <summary>电话号码</summary>
        public string PhoneNumber { get; set; }

        /// <summary>短信内容</summary>
        public string MessageContent { get; set; }

        /// <summary>短信模板代码</summary>
        public string MessageTemplateCode { get; set; }

        /// <summary>短信模板参数</summary>
        public string MessageTemplateParam { get; set; }
    }
}
