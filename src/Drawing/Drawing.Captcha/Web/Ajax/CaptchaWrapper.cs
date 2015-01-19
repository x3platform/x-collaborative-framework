#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :OrganizationWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Drawing.Captcha.Web.Ajax
{
    #region Using Libraries
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using X3Platform.Util;
    #endregion

    /// <summary>��֤��</summary>
    public sealed class CaptchaWrapper
    {
        #region ����:CreateNewObject(XmlDocument doc)
        /// <summary>������ˮ��</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns> 
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();
            
            string base64String = string.Empty;

            string name = XmlHelper.Fetch("name", doc);
            
            // ��ʼ����֤��
            Captcha captcha = new Captcha(new
            {
                waves = true
            });

            using (MemoryStream stream = new MemoryStream())
            {
                captcha.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                byte[] buffer = new byte[stream.Length];

                stream.Position = 0;
                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();

                base64String = Convert.ToBase64String(buffer);
            }

            return "{\"data\":{\"width\":" + captcha.Image.Width + ",\"height\":" + captcha.Image.Height + ",\"base64\":\"" + base64String + "\"},\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion
    }
}
