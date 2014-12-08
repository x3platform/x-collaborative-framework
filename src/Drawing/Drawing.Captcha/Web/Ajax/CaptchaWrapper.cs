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
        #region 函数:Generate(XmlDocument doc)
        /// <summary>生成流水号</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string Generate(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string name = XmlHelper.Fetch("name", doc);

            string result = this.service.Generate(name);

            outString.Append("{\"ajaxStorage\":\"" + result + "\",\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
