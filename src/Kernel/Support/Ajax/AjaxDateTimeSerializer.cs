namespace X3Platform.Ajax
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using X3Platform.Util;

    /// <summary>日期格式序列化接口</summary>
    public sealed class AjaxDateTimeSerializer : IDateTimeSerializer
    {
        #region 函数:Serialize(StringBuilder outString, string namingRule, string key, DateTime date)
        /// <summary>解析</summary>
        /// <param name="target">目标对象</param>
        /// <param name="doc">Xml文档</param>
        /// <returns></returns>
        public void Serialize(StringBuilder outString, string namingRule, string key, DateTime date)
        {
            outString.Append(string.Format(" \"{0}\" : \"{1}\",", key, date.ToString("yyyy,MM,dd,HH,mm,ss,fff")));

            if (namingRule == "underline")
            {
                outString.Append(string.Format(" \"{0}_view\" : \"{1}\",", key, date.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            else
            {
                outString.Append(string.Format(" \"{0}View\" : \"{1}\",", key, date.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion
    }
}
