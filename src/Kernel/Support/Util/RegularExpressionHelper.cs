// =============================================================================
//
// Copyright (c) 2010 RuanYu
//
// Filename     :RegularExpressionHelper.cs
//
// Summary      :regular expression helper
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2007-06-03
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace X3Platform.Util
{
    /// <summary>��������ʽ������</summary>
    public class RegularExpressionHelper
    {
        /// <summary>ץȡ���ӵ�ַ</summary>
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

        #region 属性:MDYToDMY(string text)
        /// <summary>dd-mm-yy ��������ʽ���� mm/dd/yy ��������ʽ</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MDYToDMY(string text)
        {
            return Regex.Replace(text, "\\b(?\\d{1,2})/(?\\d{1,2})/(?\\d{2,4})\\b", "${day}-${month}-${year}");
        }
        #endregion

        #region 属性:IsUnsafeSQL(string text)
        /// <summary>�����Ƿ��ǲ���ȫ��SQL����</summary>
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

        #region 属性:IsEmail(string text)
        /// <summary>�����ַ��Ƿ�ΪEmail��ַ</summary>
        /// <param name="text">Email��ַ��Ϣ.</param>
        /// <returns>true | false</returns>
        public static bool IsEmail(string text)
        {
            // Return true if text is in valid e-mail format. 
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            return Regex.IsMatch(text, pattern);
        }
        #endregion

        #region 属性:IsDecimal(string text)
        /// <summary>�����ַ��Ƿ�Ϊʮ������.</summary>
        /// <param name="text">���������ַ���</param>
        /// <returns></returns>
        public static bool IsDecimal(string text)
        {
            string pattern = @"[0].\d{1,2}|[1]";

            return Regex.IsMatch(text, pattern);
        }
        #endregion

        #region 属性:IsNumeric(string text)
        /// <summary>�����ַ��Ƿ�Ϊ����</summary>
        /// <param name="text">���������ַ���</param>
        /// <returns>����ֵ</returns>
        public static bool IsNumeric(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            for (int i = 0; i < text.Length; i++)
            {
                // ����ÿ���ַ�
                if (!Char.IsNumber(text, i))
                    return false;
            }

            return true;
        }
        #endregion

        #region 属性:IsTelephone(string text)
        /// <summary>�����Ƿ�Ϊ�绰����</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsTelephone(string text)
        {
            return Regex.IsMatch(text, @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?");
        }
        #endregion

        #region 属性:IsDate(string text)
        /// <summary>����������</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsDate(string text)
        {
            //return Regex.IsMatch(strIn, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])(?:0?[1-9]|1\d|2[0-3]):(?:0?[1-9]|[1-5]\d):(?:0?[1-9]|[1-5]\d)$");
            return Regex.IsMatch(text, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])$");
        }
        #endregion

        #region 属性:IsSafeFilePostfix(string text, string postfix)
        /// <summary>���ⰲȫ���ļ�����׺</summary>
        public static bool IsSafeFilePostfix(string text, string postfix)
        {
            string filePostfix = string.IsNullOrEmpty(postfix) ? "gif|jpg|png" : postfix;

            return Regex.IsMatch(text, @"\.(?i:" + filePostfix + ")$");
        }
        #endregion

        #region 属性:IsByte(string text)
        /// <summary>�����ַ��Ƿ���4��12֮��</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsByte(string text)
        {
            return Regex.IsMatch(text, @"^[a-z]{4,12}$");
        }
        #endregion

        #region 属性:IsIP(string text)
        /// <summary>����IP</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsIP(string text)
        {
            return Regex.IsMatch(text, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }
        #endregion

        #region 属性:IsGuid(string text)
        /// <summary>����Guid</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsGuid(string text)
        {
            return Regex.IsMatch(text, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
