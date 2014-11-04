namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Threading;
    #endregion

    /// <summary>字符串处理辅助类</summary>
    public sealed class StringHelper
    {
        #region 函数:UnicodeEncode(string text)
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

        #region 函数:UnicodeDecode(string text)
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
        // 字符串常用处理
        //-------------------------------------------------------

        #region 函数:ToBytes(string text)
        /// <summary>将文本信息转成字节信息</summary>
        /// <param name="text">文本信息</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }
        #endregion

        #region 函数:ToStream(string text)
        /// <summary></summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static Stream ToStream(string text)
        {
            return new MemoryStream(Encoding.Default.GetBytes(text));
        }
        #endregion

        /// <summary>串联字符串数组的所有非空内容，其中在每个元素之间使用逗号分隔符。</summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Join(params string[] values)
        {
            return Join(",", values);
        }

        /// <summary>串联字符串数组的所有非空元素，其中在每个元素之间使用指定的分隔符。</summary>
        /// <param name="separator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Join(string separator, params string[] values)
        {
            List<string> list = new List<string>();

            foreach (var text in values)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    list.Add(text);
                }
            }

            return string.Join(separator, list.ToArray());
        }

        //-------------------------------------------------------
        // 字符串格式化处理
        //-------------------------------------------------------

        #region 函数:ToLeftString(string text, int length)
        /// <summary></summary>
        /// <param name="text"></param>
        /// <param name="length">length</param>
        /// <returns>string</returns>
        public static string ToLeftString(string text, int length)
        {
            return ToLeftString(text, length, true);
        }

        /// <summary></summary>
        /// <param name="text"></param>
        /// <param name="length">length</param>
        /// <returns>string</returns>
        public static string ToLeftString(string text, int length, bool hasEllipsis)
        {
            if (string.IsNullOrEmpty(text) || text.Length - 1 < length) { return text; }

            return string.Format("{0}{1}", text.Remove(length), (hasEllipsis ? "..." : string.Empty));
        }
        #endregion

        #region 函数:ToRightString(string text, int length, bool hasEllipsis)
        /// <summary>
        /// 取右边的几个
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="length"></param>
        /// <param name="hasEllipsis"></param>
        /// <returns></returns>
        public static string ToRightString(string text, int length, bool hasEllipsis)
        {
            if (string.IsNullOrEmpty(text) || text.Length < length) { return text; }

            return string.Format("{0}{1}", (hasEllipsis ? "..." : string.Empty), text.Substring(length));
        }
        #endregion

        #region 函数:ToSubString(string text, string tagStart, bool tagStartBool, string tagEnd, bool tagEndBool)
        /// <summary>截取部分字符</summary>
        /// <param name="text">待处理的字符串</param>
        /// <param name="tagStart">开始标签</param>
        /// <param name="tagStartBool">是否包含开始标签</param>
        /// <param name="tagEnd">结束标签</param>
        /// <param name="tagEndBool">是否结束结束标签</param>
        /// <returns>处理后的字符</returns>
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

        #region 函数:ToCutString(string text, int length)
        /// <summary>字符长度控制 中文 英文识别,一个汉字作为2个字符长度处理</summary>
        /// <param name="text">要进行切割的字符串</param>
        /// <param name="length">返回的长度（自动识别中英文）</param>
        /// <returns></returns>
        public static string ToCutString(string text, int length)
        {
            return ToCutString(text, length, false);
        }

        /// <summary>字符长度控制 中文 英文识别,一个汉字作为2个字符长度处理</summary>
        /// <param name="text">要进行切割的字符串</param>
        /// <param name="length">返回的长度（自动识别中英文）</param>
        /// <param name="hasEllipsis">是否输出省略号</param>
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

        #region 函数:ToDate(string date)
        /// <summary>格式化日期</summary>
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
                //当日期格式无法识别,转换失败,返回原始数据.
                return date;
            }
        }

        /// <summary>
        /// 统一日期格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate(DateTime date)
        {
            // return date.ToString("yyyy-MM-dd HH:mm:ss");
            return date.ToString("yyyy-MM-dd");
        }
        #endregion

        #region 函数:ToTime(string date)
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region 函数:ToSmartTime(string date)
        /// <summary>智能日期格式</summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToSmartTime(DateTime date)
        {
            //
            // 1.今年以前的时间显示 "yyyy年"
            //
            // 2.今年的时间显示为 "MM月dd日"
            //
            // 3.当天的时间显示为 "HH:mm:ss"
            //
            // 3.其他时间 显示为 "yyyy-MM-dd HH:mm:ss"
            //

            if (date.Year < DateTime.Now.Year)
            {
                return date.ToString("yyyy年");
            }
            else
            {
                if (DateTime.Now.Date == date.Date)
                {
                    return date.ToString("HH:mm:ss");
                }
                else if (DateTime.Now.Year == date.Year)
                {
                    return date.ToString("MM月dd日");
                }
                else
                {
                    return date.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
        #endregion

        #region 函数:ToGuid(Guid g)
        /// <summary>生成一个Guid格式字符串</summary>
        /// <returns></returns>
        public static string ToGuid()
        {
            return ToGuid(Guid.NewGuid());
        }

        /// <summary>生成一个Guid格式字符串</summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string ToGuid(Guid g)
        {
            //
            // 说明符      返回值的格式
            //
            //N             32 位：
            //              xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            //
            //D             由连字符分隔的 32 位数字：
            //              xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
            //
            //B             括在大括号中、由连字符分隔的 32 位数字：
            //              {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
            //
            //P             括在圆括号中、由连字符分隔的 32 位数字：
            //              (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
            //

            return g.ToString("D");
        }
        #endregion

        #region 函数:To16DigitGuid(Guid g)
        /// <summary>以一个Guid为因子生成一个16位格式的字符串</summary>
        /// <returns></returns>
        public static string To16DigitGuid()
        {
            return To16DigitGuid(Guid.NewGuid());
        }

        /// <summary>以一个Guid为因子生成一个16位格式的字符串</summary>
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

            // {0:x} 标识16进制格式的数字
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

        #region 函数:ToMD5(string text)
        /// <summary>取得MD5 Hash值</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToMD5(string text)
        {
            byte[] buffer = Encoding.Default.GetBytes(text);

            return StringHelper.ToMD5(buffer);
        }

        /// <summary>取得MD5 Hash值</summary>
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

        #region 函数:ToChineseTag(string text)
        /// <summary>将英文标点符号转换为中文标点符号 , Punctuation</summary>
        /// <param name="text">需处理的字符串</param>
        /// <returns>字符串</returns>
        public static string ToChineseTag(string text)
        {
            /*
             * 半角字符对应的编号
             *
             * 32           空格
             * 33-47        标点
             * 48-57        0~9
             * 58-64        标点
             * 65-90        A~Z
             * 91-96        标点
             * 97-122       a~z
             * 123-126      标点
             *
             *
             * 半角字符 + 65248 = 全角字符
             *
             * 全角空格例外
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

        #region 函数:ToFirstUpper(string text)
        /// <summary>对输入的字符串格式化，得到首字母为大写的字符床. e.g. user -> User.</summary>
        /// <param name="text">需要处理的字符串</param>
        /// <returns></returns>
        public static string ToFirstUpper(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }
        #endregion

        #region 函数:ToFirstLower(string text)
        /// <summary>对输入的字符格式化，得到首字母为小写的字符串. 例如 User -> user.</summary>
        /// <param name="text">需要处理的字符串</param>
        /// <returns></returns>
        public static string ToFirstLower(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }
        #endregion

        #region 函数:ToProcedurePrefix(string text)
        /// <summary>对输入的字符格式化，得到首字母为小写的字符串. 例如 tb_User -> proc_User.</summary>
        /// <param name="text">数据表名称</param>
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

        #region 函数:ToProcedurePrefix(string text, string tablePrefix)
        /// <summary>对输入的字符格式化，得到首字母为小写的字符串. 例如 tb_User -> proc_User.</summary>
        /// <param name="text">数据表名称</param>
        /// <param name="tablePrefix">数据表的前缀</param>
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

        #region 函数:ToSafeXml(string text)
        /// <summary>处理Xml节点中的非法字符</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeXml(string text)
        {
            return string.Format("<![CDATA[{0}]]>", text);
        }
        #endregion

        #region 函数:ToSafeJson(string text)
        /// <summary>Json格式字符串中的特殊字符.</summary>
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

        #region 函数:ToSafeSQL(string text)
        /// <summary>处理SQL格式中的非法字符</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeSQL(string text)
        {
            string[] removeTags = { };
            return ToSafeSQL(text, removeTags);
        }
        #endregion

        #region 函数:ToSafeSQL(string text, bool removeQuotes)
        /// <summary>处理SQL格式中的非法字符</summary>
        /// <param name="text"></param>
        /// <param name="removeQuotes"></param>
        /// <returns></returns>
        public static string ToSafeSQL(string text, bool removeQuotes)
        {
            string[] removeTags = removeQuotes ? new string[] { "'" } : new string[] { };

            return ToSafeSQL(text, removeTags);
        }
        #endregion

        #region 函数:ToSafeSQL(string text, string[] removeTags)
        /// <summary>处理SQL格式中的非法字符</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeSQL(string text, string[] removeTags)
        {
            //-------------------------------------------------------
            // 1.替换一个单引号为两个单引号.
            //
            // 2.替换两个井号为一个单引号.
            //-------------------------------------------------------

            if (string.IsNullOrEmpty(text)) { return string.Empty; }

            text = text.Replace(";", string.Empty).Replace("'", "''").Replace("--", "''--''");

            // 字符串两边必须留白
            Regex regex = new Regex(@"##(.*?)##");

            MatchCollection matches = regex.Matches(text);

            string matchValue = string.Empty;

            for (int i = 0; i < matches.Count; i++)
            {
                matchValue = matches[i].Value.Substring(2, matches[i].Value.Length - 4).Trim();

                // 内容中禁止以 OR 或者 -- 开始和结束
                if (!Regex.IsMatch(matchValue.ToUpper(), "^(OR|--)\\s+|\\s+(OR|--])$"))
                {
                    text = text.Replace(matches[i].Value, string.Format("'{0}'", matchValue));
                }
            }

            // 移除自定义标签
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

        #region 函数:ToSafeSQL(string text)
        /// <summary>处理多个以逗号隔开标识中的非法字符</summary>
        /// <param name="text">文本信息</param>
        /// <returns></returns>
        public static string ToSafeIds(string text)
        {
            if (string.IsNullOrEmpty(text)) { return string.Empty; }

            string[] ids = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (ids.Length == 0) { return string.Empty; }

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = Regex.Replace(ids[i], ";|'|--", string.Empty);
            }

            return string.Join(",", ids);
        }
        #endregion

        #region 函数:ToSafeLike(string text)
        /// <summary>处理SQL Like 条件中的通配字符</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeLike(string text)
        {
            return text.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }
        #endregion

        #region 函数:ToSafeHtml(string text)
        /// <summary>处理安全的Html字符</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeHtml(string text)
        {
            text = HttpUtility.HtmlEncode(text);

            return text.Replace("&lt;br /&gt;", "<br />").Replace("&lt;p&gt;", "<p>").Replace("&lt;/p&gt;", "</p>");
        }
        #endregion

        #region 函数:ToSafeXSS<T>(T targetObject)
        /// <summary>处理安全的跨站脚本攻击字符</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T ToSafeXSS<T>(T targetObject)
        {
            if (targetObject == null) { return default(T); }

            object result = null;

            Type type = targetObject.GetType();

            //
            // 对象的属性 反射为set_MethodName 或者 get_MethodName
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

        #region 函数:ToSafeXSS(string text)
        /// <summary>处理安全的跨站脚本攻击字符</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSafeXSS(string text)
        {
            // 原文参考 http://www.dbuggr.com/leothenerd/php-function-remove-xss/

            // remove all non-printable characters. CR(0a) and LF(0b) and TAB(9) are allowed  
            // 移除所有非打印字符，只允许回车符 CR(0a) 换行符 LF(0b) 和 TAB(9)

            // this prevents some character re-spacing such as <java\0script>
            // 这可以防止一些字符间的特殊字符，例如 <java\0script>
            // note that you have to handle splits with \n, \r, and \t later since they *are* allowed in some inputs  
            // 

            text = Regex.Replace(text, "([\x00-\x08|\x0b-\x0c|\x0e-\x19])", string.Empty);

            // straight replacements, the user should never need these since they're normal characters  
            // 连续更换，
            // this prevents like <IMG SRC=@avascript:alert('XSS')>  
            // 这可以防止下面这些情况，例如 <IMG SRC="@avascript:alert('XSS')" >

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
                // ord() 函数返回字符串第一个字符的ASCII 值
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
               // 由于富文本编辑器呈现的很多样式都是通过 style 标签展现的，所以取消过滤 style 标签。
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

        #region 函数:ToSafeUrl(string text)
        /// <summary>过滤Url地址</summary>
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

        #region 函数:ToRandom(int length)
        /// <summary>取得一个随机的字符串</summary>
        /// <param Name="length">字符串的长度</param>
        /// <returns>长为length的随机的字符串</returns>
        public static string ToRandom(int length)
        {
            return ToRandom("abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ", length);
        }
        #endregion

        #region 函数:ToRandom(string chars, int length)
        /// <summary>取得一个随机的字符串</summary>
        /// <param Name="chars">指定字符</param>
        /// <param Name="length">字符串的长度</param>
        /// <returns>长为length的随机的字符串</returns>
        public static string ToRandom(string chars, int length)
        {
            if (length <= 0) { return string.Empty; }

            // 等待定时器的推进, 避免在时间极短的情况下生成相同的随机数
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
        // 空字符串处理
        //-------------------------------------------------------

        #region 函数:NullTo(string text)
        /// <summary>默认的空值转换, 把 null 转换为 "" .</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string NullTo(string text)
        {
            return NullTo(text, string.Empty);
        }
        #endregion

        #region 函数:NullTo(string text, string replaceText)
        /// <summary>空值转换，把 null 转换为 replaceText. e.g. NullTo(null,"ok") 返回 "ok", NullTo("1","ok")返回"1"</summary>
        /// <param name="text"></param>
        /// <param name="replaceText"></param>
        /// <returns></returns>
        public static string NullTo(string text, string replaceText)
        {
            return (text == null) ? replaceText : text;
        }
        #endregion

        #region 函数:NullOrEmptyTo(string text, string replaceText)
        /// <summary>空字符串转换，把 null 或 "" 转换为 replaceText. e.g. NullOrEmptyTo(null,"ok")返回"ok", NullOrEmptyTo("","ok")返回"ok".</summary>
        /// <param name="text"></param>
        /// <param name="replaceText"></param>
        /// <returns></returns>
        public static string NullOrEmptyTo(string text, string replaceText)
        {
            return (string.IsNullOrEmpty(text)) ? replaceText : text;
        }
        #endregion

        //-------------------------------------------------------
        // 清理标记
        //-------------------------------------------------------

        #region 函数:TrimEnd(string text, string trimText)
        /// <summary>清除文本最后的标记</summary>
        /// <param name="text">需处理的字符</param>
        /// <param name="trimText">标签</param>
        /// <returns>字符串</returns>
        public static StringBuilder TrimEnd(StringBuilder text, string trimText)
        {
            return (text.ToString().Substring(text.Length - trimText.Length, trimText.Length) == trimText) ? text.Remove(text.Length - trimText.Length, trimText.Length) : text;
        }
        #endregion

        #region 函数:TrimEnd(string text, string trimText)
        /// <summary>清除文本最后的标记</summary>
        /// <param name="text">需处理的字符</param>
        /// <param name="trimText">标签</param>
        /// <returns>字符串</returns>
        public static string TrimEnd(string text, string trimText)
        {
            return (text.Substring(text.Length - trimText.Length, trimText.Length) == trimText) ? text.Substring(0, text.Length - trimText.Length) : text;
        }
        #endregion

        #region 函数:RemoveEnterTag(string text)
        /// <summary>移除换行符</summary>
        /// <param name="text">需要处理的字符串</param>
        public static string RemoveEnterTag(string text)
        {
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", string.Empty);

            return text;
        }
        #endregion

        #region 函数:RemoveHtmlTag(string text)
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

        #region 函数:FixSQL(string text, string type)
        /// <summary>将SQL语句特殊字符</summary>
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
