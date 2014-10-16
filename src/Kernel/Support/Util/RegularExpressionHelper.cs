namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    #endregion

    /// <summary>正则表达式处理辅助类</summary>
    public class RegularExpressionHelper
    {
        /// <summary>抓取链接地址</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DumpHrefs(string text)
        {
            StringBuilder outString = new StringBuilder();

            Regex r = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Match m;

            for (m = r.Match(text); m.Success; m = m.NextMatch())
            {
                outString.AppendLine(m.Groups[1].Value);
            }

            return outString.ToString();
        }

        #region 函数:MDYToDMY(string text)
        /// <summary>dd-mm-yy 的日期形式代替 mm/dd/yy 的日期形式</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MDYToDMY(string text)
        {
            return Regex.Replace(text, "\\b(?\\d{1,2})/(?\\d{1,2})/(?\\d{2,4})\\b", "${day}-${month}-${year}");
        }
        #endregion

        #region 函数:IsUnsafeSQL(string text)
        /// <summary>检测是否是不安全的SQL语句</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsUnsafeSQL(string text)
        {
            bool isUnsafe = false;

            string badText = "=,%,?,\",\',&,:,#,@,$,<,>,!,|,+,-,*,(,),%20,;";

            string[] badSQL = badText.Split(',');

            for (int i = 0; i < badSQL.Length; i++)
            {
                if (text.IndexOf(badSQL[i]) != -1)
                {
                    isUnsafe = true;
                    break;
                }
            }

            return isUnsafe;
        }
        #endregion

        #region 函数:IsEmail(string text)
        /// <summary>检测字符是否为Email地址</summary>
        /// <param name="text">Email地址信息.</param>
        /// <returns>true | false</returns>
        public static bool IsEmail(string text)
        {
            // Return true if text is in valid e-mail format. 
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            return Regex.IsMatch(text, pattern);
        }
        #endregion

        #region 函数:IsDecimal(string text)
        /// <summary>检测字符是否为十进制数.</summary>
        /// <param name="text">需检测的字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string text)
        {
            string pattern = @"[0].\d{1,2}|[1]";

            return Regex.IsMatch(text, pattern);
        }
        #endregion

        #region 函数:IsNumeric(string text)
        /// <summary>检测字符是否为数字</summary>
        /// <param name="text">需检测的字符串</param>
        /// <returns>布尔值</returns>
        public static bool IsNumeric(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            for (int i = 0; i < text.Length; i++)
            {
                // 检测每个字符
                if (!Char.IsNumber(text, i))
                    return false;
            }

            return true;
        }
        #endregion

        #region 函数:IsTelephone(string text)
        /// <summary>检测是否为电话号码</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsTelephone(string text)
        {
            return Regex.IsMatch(text, @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?");
        }
        #endregion

        #region 函数:IsDate(string text)
        /// <summary>检测年月日</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsDate(string text)
        {
            //return Regex.IsMatch(strIn, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])(?:0?[1-9]|1\d|2[0-3]):(?:0?[1-9]|[1-5]\d):(?:0?[1-9]|[1-5]\d)$");
            return Regex.IsMatch(text, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])$");
        }
        #endregion

        #region 函数:IsSafeFilePostfix(string text, string postfix)
        /// <summary>检测安全的文件名后缀</summary>
        public static bool IsSafeFilePostfix(string text, string postfix)
        {
            string filePostfix = string.IsNullOrEmpty(postfix) ? "gif|jpg|png" : postfix;

            return Regex.IsMatch(text, @"\.(?i:" + filePostfix + ")$");
        }
        #endregion

        #region 函数:IsByte(string text)
        /// <summary>检测字符是否在4至12之间</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsByte(string text)
        {
            return Regex.IsMatch(text, @"^[a-z]{4,12}$");
        }
        #endregion

        #region 函数:IsIP(string text)
        /// <summary>检测IP</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsIP(string text)
        {
            return Regex.IsMatch(text, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }
        #endregion

        #region 函数:IsGuid(string text)
        /// <summary>检测Guid</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsGuid(string text)
        {
            return Regex.IsMatch(text, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
