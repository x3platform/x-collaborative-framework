using System;
namespace X3Platform.SMS.Client
{
    /// <summary>������Ϣ</summary>
    public class ShortMessage
    {
        /// <summary>������ˮ��</summary>
        public string SerialNumber { get; set; }

        /// <summary>�绰����</summary>
        public string PhoneNumber { get; set; }

        /// <summary>��������</summary>
        public string MessageContent { get; set; }

        /// <summary>����ģ�����</summary>
        public string MessageTemplateCode { get; set; }

        /// <summary>����ģ�����</summary>
        public string MessageTemplateParam { get; set; }
    }
}
