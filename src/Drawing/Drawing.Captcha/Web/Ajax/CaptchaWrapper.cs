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

namespace X3Platform.Membership.Ajax
{
    #region Using Libraries


    #endregion

    /// <summary></summary>
    public sealed class CaptchaWrapper 
    {
        #region ����:Generate(XmlDocument doc)
        /// <summary>������ˮ��</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns> 
        public string Generate(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string name = XmlHelper.Fetch("name", doc);

            string result = this.service.Generate(name);

            outString.Append("{\"ajaxStorage\":\"" + result + "\",\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
