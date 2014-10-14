// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// Filename     :StringHelper.cs
//
// Description  :�ַ���������
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2010-01-01
//
// =============================================================================

namespace X3Platform.Util
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Threading;

    /// <summary>�ַ���������</summary>
    public sealed class StringHelper
    {
        #region ����:UnicodeEncode(string text)
        /// <summary></summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnicodeEncode(string text)
        {
            StringBuilder outString = new StringBuilder();

            char[] chars = text.ToCharArray();

            foreach (char ch in chars)
            {
                outString.Append("\\u" + ((int)ch).ToString("x4"));
            }

            return outString.ToString();
        }
        #endregion

        #region ����:UnicodeDecode(string text)
        /// <summary></summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnicodeDecode(string text)
        {
            StringBuilder outString = new StringBuilder();

            MatchCollection matches = Regex.Matches(text, @"([\w]+)|(\\u([\w]{4}))");

            if (matches != null && matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    byte[] codes = new byte[2];

                    string word = match.Value.Substring(2);

                    codes[0] = (byte)Convert.ToInt32(word.Substring(2), 16);
                    codes[1] = (byte)Convert.ToInt32(word.Substring(0, 2), 16);

                    outString.Append(Encoding.Unicode.GetString(codes));
                }

                outString.ToString();
            }

            return outString.ToString();
        }
        #endregion

        //-------------------------------------------------------
        // �ַ������ô���
        //-------------------------------------------------------

        #region ����:ToBytes(string text)
        /// <summary>���ı���Ϣת���ֽ���Ϣ</summary>
        /// <param name="text">�ı���Ϣ</param>
        /// <returns>�ֽ�����</returns>
        public static byte[] ToBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }
        #endregion

        #region ����:ToStream(string text)
        /// <summary></summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static Stream ToStream(string text)
        {
            return new MemoryStream(Encoding.Default.GetBytes(text));
        }
        #endregion

        //-------------------------------------------------------
        // �ַ�����ʽ������
        //-------------------------------------------------------

        #region ����:ToLeftString(string text, int length)
        /// <summary>��ȡ�ı�����������</summary>
        /// <param name="text">�ı���Ϣ</param>
        /// <param name="length">�ַ�����</param>
        /// <returns>�ı���Ϣ</returns>
        public static string ToLeftString(string text, int length)
        {
            return ToLeftString(text, length, true);
        }

        /// <summary>��ȡ�ı�����������</summary>
        /// <param name="text">�ı���Ϣ</param>
        /// <param name="length">�ַ�����</param>
        /// <param name="hasEllipsis">�Ƿ�����ʡ�Ժ�</param>
        /// <returns>�ı���Ϣ</returns>
        public static string ToLeftString(string text, int length, bool hasEllipsis)
        {
            if (string.IsNullOrEmpty(text) || text.Length - 1 < length) { return text; }

            return string.Format("{0}{1}", text.Remove(length), (hasEllipsis ? "..." : string.Empty));
        }
        #endregion

        #region ����:ToRightString(string text, int length, bool hasEllipsis)
        /// <summary>��ȡ�ı��Ҳ�������</summary>
        /// <param name="text">�ı���Ϣ</param>
        /// <param name="length">�ַ�����</param>
        /// <param name="hasEllipsis">�Ƿ�����ʡ�Ժ�</param>
        /// <returns>�ı���Ϣ</returns>
        public static string ToRightString(string text, int length, bool hasEllipsis)
        {
            if (string.IsNullOrEmpty(text) || text.Length < length) { return text; }

            return string.Format("{0}{1}", (hasEllipsis ? "..." : string.Empty), text.Substring(length));
        }
        #endregion

        #region ����:ToSubString(string text, string tagStart, bool tagStartBool, string tagEnd, bool tagEndBool)
        /// <summary>��ȡ�����ַ�</summary>
        /// <param name="text">���������ַ���</param>
        /// <param name="tagStart">��ʼ��ǩ</param>
        /// <param name="tagStartBool">�Ƿ�������ʼ��ǩ</param>
        /// <param name="tagEnd">������ǩ</param>
        /// <param name="tagEndBool">�Ƿ�����������ǩ</param>
        /// <returns>���������ַ�</returns>
        public static string ToSubString(string text, string tagStart, bool tagStartBool, string tagEnd, bool tagEndBool)
        {
            try
            {
                int counterStart, counterEnd, contentLength;

                if (tagStartBool)
                {
                    counterStart = text.IndexOf(tagStart);
                }
                else
                {
                    counterStart = text.IndexOf(tagStart) + tagStart.Length;
                }

                if (tagEndBool)
                {
                    counterEnd = text.IndexOf(tagEnd, counterStart) + tagEnd.Length;
                }
                else
                {
                    counterEnd = text.IndexOf(tagEnd, counterStart);
                }

                contentLength = counterEnd - counterStart;

                return text.Substring(counterStart, contentLength).Trim();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0}{1}{2}", tagStart, tagEnd, ex.ToString()));

                return null;
            }
        }
        #endregion

        #region ����:ToCutString(string text, int length)
        /// <summary>�ַ����ȿ��� ���� Ӣ��ʶ��,һ��������Ϊ2���ַ����ȴ���</summary>
        /// <param name="text">Ҫ�����и����ַ���</param>
        /// <param name="length">���صĳ��ȣ��Զ�ʶ����Ӣ�ģ�</param>
        /// <returns></returns>
        public static string ToCutString(string text, int length)
        {
            return ToCutString(text, length, false);
        }

        /// <summary>�ַ����ȿ��� ���� Ӣ��ʶ��,һ��������Ϊ2���ַ����ȴ���</summary>
        /// <param name="text">Ҫ�����и����ַ���</param>
        /// <param name="length">���صĳ��ȣ��Զ�ʶ����Ӣ�ģ�</param>
        /// <param name="hasEllipsis">�Ƿ�����ʡ�Ժ�</param>
        /// <returns></returns>
        public static string ToCutString(string text, int length, bool hasEllipsis)
        {
            byte[] buffer = System.Text.Encoding.Default.GetBytes(text);

            if (buffer.Length > length)
            {
                return System.Text.Encoding.Default.GetString(buffer, 0, length) + (hasEllipsis ? "..." : "");
            }
            else
            {
                return text;
            }
        }
        #endregion

        #region ����:ToDate(string date)
        /// <summary>��ʽ������</summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate(string date)
        {
            try
            {
                return ToDate(DateTime.Parse(date));
            }
            catch
            {
                // �����ڸ�ʽ�޷�ʶ��,ת��ʧ��,����ԭʼ����.
                return date;
            }
        }

        /// <summary>
        /// ͳһ���ڸ�ʽ
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate(DateTime date)
        {
            // return date.ToString("yyyy-MM-dd HH:mm:ss");
            return date.ToString("yyyy-MM-dd");
        }
        #endregion

        #region ����:ToTime(string date)
        /// <summary>
        /// ��ʽ��ʱ��
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region ����:ToSmartTime(string date)
        /// <summary>�������ڸ�ʽ</summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToSmartTime(DateTime date)
        {
            //
            // 1.������ǰ��ʱ����ʾ "yyyy��"
            //
            // 2.������ʱ����ʾΪ "MM��dd��"
            //
            // 3.������ʱ����ʾΪ "HH:mm:ss"
            //
            // 3.����ʱ�� ��ʾΪ "yyyy-MM-dd HH:mm:ss"
            //

            if (date.Year < DateTime.Now.Year)
            {
                return date.ToString("yyyy��");
            }
            else
            {
                if (DateTime.Now.Date == date.Date)
                {
                    return date.ToString("HH:mm:ss");
                }
                else if (DateTime.Now.Year == date.Year)
                {
                    return date.ToString("MM��dd��");
                }
                else
                {
                    return date.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
        #endregion

        #region ����:ToGuid(Guid g)
        /// <summary>����һ��Guid��ʽ�ַ���</summary>
        /// <returns></returns>
        public static string ToGuid()
        {
            return ToGuid(Guid.NewGuid());
        }

        /// <summary>����һ��Guid��ʽ�ַ���</summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string ToGuid(Guid g)
        {
            //
            // ˵����      ����ֵ�ĸ�ʽ
            //
            //N             32 λ��
            //              xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            //
            //D             �����ַ��ָ��� 32 λ���֣�
            //              xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
            //
            //B             ���ڴ������С������ַ��ָ��� 32 λ���֣�
            //              {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
            //
            //P             ����Բ�����С������ַ��ָ��� 32 λ���֣�
            //              (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
            //

            return g.ToString("D");
        }
        #endregion

        #region ����:To16DigitGuid(Guid g)
        /// <summary>��һ��GuidΪ��������һ��16λ��ʽ���ַ���</summary>
        /// <returns></returns>
        public static string To16DigitGuid()
        {
            return To16DigitGuid(Guid.NewGuid());
        }

        /// <summary>��һ��GuidΪ��������һ��16λ��ʽ���ַ���</summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string To16DigitGuid(Guid g)
        {
            // http://zh.wikipedia.org/zh-cn/Base64
            long i = 1;

            foreach (byte b in g.ToByteArray())
            {
                i *= ((int)b + 1);
            }

            // {0:x} ��ʶ16���Ƹ�ʽ������
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        #endregion

        public static string ToInt64Guid()
        {
            return ToInt64Guid(Guid.NewGuid());
        }

        public static string ToInt64Guid(Guid g)
        {
            byte[] buffer = g.ToByteArray();

            return BitConverter.ToInt64(buffer, 0).ToString();
        }

        #region ����:ToMD5(string text)
        /// <summary>ȡ��MD5 Hashֵ</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToMD5(string text)
        {
            byte[] buffer = Encoding.Default.GetBytes(text);

            return StringHelper.ToMD5(buffer);
        }

        /// <summary>ȡ��MD5 Hashֵ</summary>
        public static string ToMD5(byte[] buffer)
        {
            StringBuilder outString = new StringBuilder();

            byte[] MD5Buffer = new MD5CryptoServiceProvider().ComputeHash(buffer);

            for (int i = 0; i < MD5Buffer.Length; i++)
            {
                outString.Append(MD5Buffer[i].ToString("x"));
            }

            return outString.ToString();
        }
        #endregion

        #region ����:ToChineseTag(string text)
        /// <summary>��Ӣ�ı�������ת��Ϊ���ı������� , Punctuation</summary>
        /// <param name="text">�账�����ַ���</param>
        /// <returns>�ַ���</returns>
        public static string ToChineseTag(string text)
        {
            /*
             * �����ַ���Ӧ�ı���
             *
             * 32           �ո�
             * 33-47        ����
             * 48-57        0~9
             * 58-64        ����
             * 65-90        A~Z
             * 91-96        ����
             * 97-122       a~z
             * 123-126      ����
             *
             *
             * �����ַ� + 65248 = ȫ���ַ�
             *
             * ȫ�ǿո�����
             */

            StringBuilder outString = new StringBuilder();

            foreach (char ch in text)
            {
                int chValue = (int)ch;

                if (chValue < 127 && (char.IsPunctuation(ch) || char.IsSymbol(ch)))
                {
                    outString.Append(((char)(chValue + 65248)));
                }
                else
                {
                    outString.Append(ch);
                }
            }

            return outString.ToString();
        }
        #endregion

        #region ����:ToFirstUpper(string text)
        /// <summary>���������ַ�����ʽ�����õ�����ĸΪ��д���ַ���. e.g. user -> User.</summary>
        /// <param name="text">��Ҫ�������ַ���</param>
        /// <returns></returns>
        public static string ToFirstUpper(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }
        #endregion

        #region ����:ToFirstLower(string text)
        /// <summary>���������ַ���ʽ�����õ�����ĸΪСд���ַ���. ���� User -> user.</summary>
        /// <param name="text">��Ҫ�������ַ���</param>
        /// <returns></returns>
        public static string ToFirstLower(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }
        #endregion

        #region ����:ToProcedurePrefix(string text)
        /// <summary>���������ַ���ʽ�����õ�����ĸΪСд���ַ���. ���� tb_User -> proc_User.</summary>
        /// <param name="text">���ݱ�����</param>
        /// <returns></returns>
        public static string ToProcedurePrefix(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.IndexOf("_") > 0)
            {
                return text.Replace(text.Substring(0, text.IndexOf("_") + 1), "proc_");
            }

            return text;
        }
        #endregion

        #region ����:ToProcedurePrefix(string text, string tablePrefix)
        /// <summary>���������ַ���ʽ�����õ�����ĸΪСд���ַ���. ���� tb_User -> proc_User.</summary>
        /// <param name="text">���ݱ�����</param>
        /// <param name="tablePrefix">���ݱ���ǰ׺</param>
        /// <returns></returns>
        public static string ToProcedurePrefix(string text, string tablePrefix)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.IndexOf(tablePrefix) > 0)
            {
                return text.Replace(tablePrefix, "proc_");
            }

            return text;
        }
        #endregion

        #region ����:ToSafeXml(string text)
        /// <summary>����Xml�ڵ��еķǷ��ַ�</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeXml(string text)
        {
            return string.Format("<![CDATA[{0}]]>", text);
        }
        #endregion

        #region ����:ToSafeJson(string text)
        /// <summary>Json��ʽ�ַ����е������ַ�.</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeJson(string text)
        {
            text = NullTo(text);

            StringBuilder outString = new StringBuilder(text.Length);

            foreach (char ch in text)
            {
                if (Char.IsControl(ch))
                {
                    int ich = (int)ch;
                    outString.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\'' || ch == '\\')
                {
                    outString.Append('\\');
                }

                outString.Append(ch);
            }

            return outString.ToString();
        }
        #endregion

        #region ����:ToSafeSQL(string text)
        /// <summary>����SQL��ʽ�еķǷ��ַ�</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeSQL(string text)
        {
            string[] removeTags = { };
            return ToSafeSQL(text, removeTags);
        }
        #endregion

        #region ����:ToSafeSQL(string text, bool removeQuotes)
        /// <summary>����SQL��ʽ�еķǷ��ַ�</summary>
        /// <param name="text"></param>
        /// <param name="removeQuotes"></param>
        /// <returns></returns>
        public static string ToSafeSQL(string text, bool removeQuotes)
        {
            string[] removeTags = removeQuotes ? new string[] { "'" } : new string[] { };

            return ToSafeSQL(text, removeTags);
        }
        #endregion

        #region ����:ToSafeSQL(string text, string[] removeTags)
        /// <summary>����SQL��ʽ�еķǷ��ַ�</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeSQL(string text, string[] removeTags)
        {
            //
            // 1.�滻һ��������Ϊ����������.
            //
            // 2.�滻���򾮺�Ϊһ��������.
            //

            if (string.IsNullOrEmpty(text)) { return string.Empty; }

            text = text.Replace(";", string.Empty).Replace("'", "''").Replace("--", "''--''");

            // �ַ������߱�������
            Regex regex = new Regex(@"##(.*?)##");

            MatchCollection matches = regex.Matches(text);

            string matchValue = string.Empty;

            for (int i = 0; i < matches.Count; i++)
            {
                matchValue = matches[i].Value.Substring(2, matches[i].Value.Length - 4).Trim();

                // �����н�ֹ�� OR ���� -- ��ʼ�ͽ���
                if (!Regex.IsMatch(matchValue.ToUpper(), "^(OR|--)\\s+|\\s+(OR|--])$"))
                {
                    text = text.Replace(matches[i].Value, string.Format("'{0}'", matchValue));
                }
            }

            // �Ƴ��Զ�����ǩ
            foreach (string removeTag in removeTags)
            {
                if (matchValue.IndexOf(removeTag) != 0)
                {
                    text = text.Replace(removeTag, string.Empty);
                }
            }

            return text;
        }
        #endregion

        #region ����:ToSafeLike(string text)
        /// <summary>����SQL Like �����е�ͨ���ַ�</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeLike(string text)
        {
            return text.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }
        #endregion

        #region ����:ToSafeHtml(string text)
        /// <summary>����ȫ��Html�ַ�</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeHtml(string text)
        {
            text = HttpUtility.HtmlEncode(text);

            return text.Replace("&lt;br /&gt;", "<br />").Replace("&lt;p&gt;", "<p>").Replace("&lt;/p&gt;", "</p>");
        }
        #endregion

        #region ����:ToSafeXSS<T>(T targetObject)
        /// <summary>����ȫ�Ŀ�վ�ű������ַ�</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T ToSafeXSS<T>(T targetObject)
        {
            if (targetObject == null) { return default(T); }

            object result = null;

            Type type = targetObject.GetType();

            //
            // ���������� ����Ϊset_MethodName ���� get_MethodName
            //

            MethodInfo[] methods = type.GetMethods();

            string key;

            foreach (MethodInfo method in methods)
            {
                if (method.Name.Contains("get_"))
                {
                    key = method.Name.Substring(4, method.Name.Length - 4);

                    if (!string.IsNullOrEmpty(key))
                    {
                        result = type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { });

                        if (result == null)
                        {
                            continue;
                        }
                        else if (result.GetType().FullName == "System.String")
                        {
                            for (int i = 0; i < methods.Length; i++)
                            {
                                if (methods[i].Name == ("set_" + key))
                                {
                                    type.InvokeMember("set_" + key, BindingFlags.InvokeMethod, null, targetObject, new object[] { ToSafeXSS(result.ToString()) });
                                }
                            }
                        }
                    }
                }
            }

            return targetObject;
        }
        #endregion

        #region ����:ToSafeXSS(string text)
        /// <summary>����ȫ�Ŀ�վ�ű������ַ�</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeXSS(string text)
        {
            // ԭ�Ĳο� http://www.dbuggr.com/leothenerd/php-function-remove-xss/

            // remove all non-printable characters. CR(0a) and LF(0b) and TAB(9) are allowed  
            // �Ƴ����зǴ�ӡ�ַ���ֻ�����س��� CR(0a) ���з� LF(0b) �� TAB(9)

            // this prevents some character re-spacing such as <java\0script>
            // �����Է�ֹһЩ�ַ����������ַ������� <java\0script>
            // note that you have to handle splits with \n, \r, and \t later since they *are* allowed in some inputs  
            // 

            text = Regex.Replace(text, "([\x00-\x08|\x0b-\x0c|\x0e-\x19])", string.Empty);

            // straight replacements, the user should never need these since they're normal characters  
            // ����������
            // this prevents like <IMG SRC=@avascript:alert('XSS')>  
            // �����Է�ֹ������Щ���������� <IMG SRC="@avascript:alert('XSS')" >

            string search = "abcdefghijklmnopqrstuvwxyz"
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                + "1234567890!@#$%^&*()"
                + "~`\";:?+/={}[]-_|\'\\";

            for (int i = 0; i < search.Length; i++)
            {
                // ;? matches the ;, which is optional 
                // 0{0,7} matches any padded zeros, which are optional and go up to 8 chars 

                // @ @ search for the hex values 
                // $val = preg_replace('/(&#[xX]0{0,8}'.dechex(ord($search[$i])).';?)/i', $search[$i], $val); // with a ; 

                text = Regex.Replace(text, "(&#[xX]0{0,8}" + ((int)search[i]).ToString("X") + ";?)", search[i].ToString(), RegexOptions.IgnoreCase);

                // @ @ 0{0,7} matches '0' zero to seven times  
                // ord() ���������ַ�����һ���ַ���ASCII ֵ
                // $val = preg_replace('/(&#0{0,8}'.ord($search[$i]).';?)/', $search[$i], $val); // with a = 97 ; 

                text = Regex.Replace(text, "(&#0{0,8}" + (int)search[i] + ";?)", search[i].ToString());
            }

            // now the only remaining whitespace attacks are \t, \n, and \r 
            string[] ra = new string[] {
               "javascript", 
               "vbscript", 
               "expression", 
               "applet", 
               "meta", 
               "xml", 
               "blink", 
               "link", 
               // ���ڸ��ı��༭�����ֵĺܶ���ʽ����ͨ�� style ��ǩչ�ֵģ�����ȡ������ style ��ǩ��
               // "style", 
               "script", 
               "embed", 
               "object", 
               "iframe", 
               "frame", 
               "frameset", 
               "ilayer", 
               "layer", 
               "bgsound", 
               "title", 
               "base", 
               "onabort",
               "onactivate", 
               "onafterprint", 
               "onafterupdate", 
               "onbeforeactivate", 
               "onbeforecopy", 
               "onbeforecut", 
               "onbeforedeactivate", 
               "onbeforeeditfocus", 
               "onbeforepaste", 
               "onbeforeprint", 
               "onbeforeunload", 
               "onbeforeupdate", 
               "onblur", 
               "onbounce", 
               "oncellchange", 
               "onchange", 
               "onclick", 
               "oncontextmenu", 
               "oncontrolselect", 
               "oncopy", 
               "oncut", 
               "ondataavailable", 
               "ondatasetchanged", 
               "ondatasetcomplete", 
               "ondblclick", 
               "ondeactivate", 
               "ondrag", 
               "ondragend", 
               "ondragenter", 
               "ondragleave", 
               "ondragover", 
               "ondragstart", 
               "ondrop", 
               "onerror", 
               "onerrorupdate", 
               "onfilterchange", 
               "onfinish", 
               "onfocus", 
               "onfocusin", 
               "onfocusout", 
               "onhelp",
               "onkeydown",
               "onkeypress", 
               "onkeyup",
               "onlayoutcomplete",
               "onload", 
               "onlosecapture",
               "onmousedown", 
               "onmouseenter", 
               "onmouseleave",
               "onmousemove",
               "onmouseout", 
               "onmouseover", 
               "onmouseup",
               "onmousewheel", 
               "onmove",
               "onmoveend", 
               "onmovestart", 
               "onpaste", 
               "onpropertychange", 
               "onreadystatechange", 
               "onreset", 
               "onresize",
               "onresizeend",
               "onresizestart", 
               "onrowenter",
               "onrowexit", 
               "onrowsdelete", 
               "onrowsinserted", 
               "onscroll",
               "onselect",
               "onselectionchange", 
               "onselectstart", 
               "onstart", 
               "onstop",
               "onsubmit",
               "onunload"
            };

            // keep replacing as long as the previous round replaced something 
            bool found = true;

            while (found == true)
            {
                string beforeText = text;

                for (int i = 0; i < ra.Length; i++)
                {
                    string pattern = string.Empty;

                    for (int j = 0; j < ra[i].Length; j++)
                    {
                        if (j > 0)
                        {
                            pattern += "(";
                            pattern += "(&#[xX]0{0,8}([9ab]);)";
                            pattern += "|";
                            pattern += "|(&#0{0,8}([9|10|13]);)";
                            pattern += ")*";
                        }
                        pattern += ra[i][j];
                    }

                    // $replacement = substr($ra[$i], 0, 2).'<x>'.substr($ra[$i], 2); // add in <> to nerf the tag  
                    string replacement = ra[i].Substring(0, 2) + "<safe-xss>" + ra[i].Substring(2);

                    // text = preg_replace($pattern, $replacement, text); // filter out the hex tags  
                    text = Regex.Replace(text, pattern, replacement, RegexOptions.IgnoreCase);
                }

                if (beforeText == text)
                {
                    // no replacements were made, so exit the loop  
                    found = false;
                }
            }

            return text;
        }
        #endregion

        #region ����:ToSafeUrl(string text)
        /// <summary>����Url��ַ</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeUrl(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("%25", string.Empty);     // %25 => %
                text = text.Replace("%0a", string.Empty);     // %0a => :
                text = text.Replace("%22", string.Empty);     // %22 => "
                text = text.Replace("%27", string.Empty);     // %27 => '
                text = text.Replace("%5c", string.Empty);     // %27 => \
                text = text.Replace("%2f", string.Empty);     // %2f => /
                text = text.Replace("%3c", string.Empty);     // %2c => <
                text = text.Replace("%3e", string.Empty);     // %3e => >
                text = text.Replace("%26", string.Empty);     // %26 => &
            }

            return text;
        }
        #endregion

        #region ����:ToRandom(int length)
        /// <summary>ȡ��һ���������ַ���</summary>
        /// <param Name="length">�ַ����ĳ���</param>
        /// <returns>��Ϊlength���������ַ���</returns>
        public static string ToRandom(int length)
        {
            return ToRandom("abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ", length);
        }
        #endregion

        #region ����:ToRandom(string chars, int length)
        /// <summary>ȡ��һ���������ַ���</summary>
        /// <param Name="chars">ָ���ַ�</param>
        /// <param Name="length">�ַ����ĳ���</param>
        /// <returns>��Ϊlength���������ַ���</returns>
        public static string ToRandom(string chars, int length)
        {
            if (length <= 0) { return string.Empty; }

            // �ȴ���ʱ�����ƽ�, ������ʱ�伫�̵�������������ͬ��������
            Thread.Sleep(1);

            char[] buffer = new char[length];

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = (int)(random.NextDouble() * chars.Length);
                buffer[i] = chars[index];
            }

            return new String(buffer);
        }
        #endregion

        //-------------------------------------------------------
        // ���ַ�������
        //-------------------------------------------------------

        #region ����:NullTo(string text)
        /// <summary>Ĭ�ϵĿ�ֵת��, �� null ת��Ϊ "" .</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string NullTo(string text)
        {
            return NullTo(text, string.Empty);
        }
        #endregion

        #region ����:NullTo(string text, string replaceText)
        /// <summary>��ֵת������ null ת��Ϊ replaceText. e.g. NullTo(null,"ok") ���� "ok", NullTo("1","ok")����"1"</summary>
        /// <param name="text"></param>
        /// <param name="replaceText"></param>
        /// <returns></returns>
        public static string NullTo(string text, string replaceText)
        {
            return (text == null) ? replaceText : text;
        }
        #endregion

        #region ����:NullOrEmptyTo(string text, string replaceText)
        /// <summary>���ַ���ת������ null �� "" ת��Ϊ replaceText. e.g. NullOrEmptyTo(null,"ok")����"ok", NullOrEmptyTo("","ok")����"ok".</summary>
        /// <param name="text"></param>
        /// <param name="replaceText"></param>
        /// <returns></returns>
        public static string NullOrEmptyTo(string text, string replaceText)
        {
            return (string.IsNullOrEmpty(text)) ? replaceText : text;
        }
        #endregion

        //-------------------------------------------------------
        // ��������
        //-------------------------------------------------------

        #region ����:TrimEnd(string text, string trimText)
        /// <summary>�����ı������ı���</summary>
        /// <param name="text">�账�����ַ�</param>
        /// <param name="trimText">��ǩ</param>
        /// <returns>�ַ���</returns>
        public static StringBuilder TrimEnd(StringBuilder text, string trimText)
        {
            return (text.ToString().Substring(text.Length - trimText.Length, trimText.Length) == trimText) ? text.Remove(text.Length - trimText.Length, trimText.Length) : text;
        }
        #endregion

        #region ����:TrimEnd(string text, string trimText)
        /// <summary>�����ı������ı���</summary>
        /// <param name="text">�账�����ַ�</param>
        /// <param name="trimText">��ǩ</param>
        /// <returns>�ַ���</returns>
        public static string TrimEnd(string text, string trimText)
        {
            return (text.Substring(text.Length - trimText.Length, trimText.Length) == trimText) ? text.Substring(0, text.Length - trimText.Length) : text;
        }
        #endregion

        #region ����:RemoveEnterTag(string text)
        /// <summary>�Ƴ����з�</summary>
        /// <param name="text">��Ҫ�������ַ���</param>
        public static string RemoveEnterTag(string text)
        {
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", string.Empty);

            return text;
        }
        #endregion

        #region ����:RemoveHtmlTag(string text)
        /// <summary>clear html tag of the text.</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string text)
        {
            if (string.IsNullOrEmpty(text)) { return string.Empty; }

            string[] aryReg ={
                        @"<script[^>]*?>.*?</script>",
                        @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                        @"([\r\n])[\s]+",
                        @"&(quot|#34);",
                        @"&(amp|#38);",
                        @"&(lt|#60);",
                        @"&(gt|#62);",
                        @"&(nbsp|#160);",
                        @"&(iexcl|#161);",
                        @"&(cent|#162);",
                        @"&(pound|#163);",
                        @"&(copy|#169);",
                        @"&#(\d+);",
                        @"-->",
                        @"<!--.*\n"
                        };

            string[] aryRep = {
                        "",
                        "",
                        "",
                        "\"",
                        "&",
                        "<",
                        ">",
                        " ",
                        "\xa1",//chr(161),
                        "\xa2",//chr(162),
                        "\xa3",//chr(163),
                        "\xa9",//chr(169),
                        "",
                        "\r\n",
                        ""
                        };

            string newReg = aryReg[0];

            string outString = text;

            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);

                outString = regex.Replace(outString, aryRep[i]);
            }

            outString.Replace("<", "");
            outString.Replace(">", "");
            outString.Replace("\r\n", "");
            outString.Replace(" ", "");
            outString.Replace(" ", "");
            //strOutput.Replace("&nb","");
            outString.Replace(" ", "");

            return outString;
        }
        #endregion

        #region ����:FixSQL(string text, string type)
        /// <summary>��SQL���������ַ�</summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FixSQL(string text, string type)
        {
            type = type.ToLower();

            if (type == "mysql")
            {
                return text.Replace("\\", "\\\\\\");
            }

            return text;
        }
        #endregion
    }
}
