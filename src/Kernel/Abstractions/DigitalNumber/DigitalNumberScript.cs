// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :DigitalNumberScript.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.DigitalNumber
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Util;

    /// <summary>��ˮ�Žű�</summary>
    public class DigitalNumberScript
    {
        /// <summary>������ˮ��ǰ׺�ű�</summary>
        /// <param name="expression"></param>
        /// <param name="prefixCode"></param>
        /// <returns></returns>
        public static string RunPrefixScript(string expression, string prefixCode)
        {
            return RunPrefixScript(expression, prefixCode, DateTime.Now);
        }

        /// <summary>������ˮ��ǰ׺�ű�</summary>
        /// <param name="expression"></param>
        /// <param name="prefixCode"></param>
        /// <param name="updateDate"></param>
        /// <returns></returns>
        public static string RunPrefixScript(string expression, string prefixCode, DateTime updateDate)
        {
            string result = null;

            // ����ǰ׺��ǩ
            expression = string.IsNullOrEmpty(prefixCode) ?
                expression.Replace("{prefix}", string.Empty) : expression.Replace("{prefix}", "{tag:" + prefixCode + "}");

            // ����Ϊ�ӱ���ʽ�б�
            IList<string[]> subexpressions = SplitExpression(expression);

            foreach (string[] subexpression in subexpressions)
            {
                // ������Ҫ�������ӵı���ʽ����: code | int | dailyIncrement
                if (subexpression[0] == "code" || subexpression[0] == "int" || subexpression[0] == "dailyIncrement")
                {
                    continue;
                }

                result += AnalyzeExpression(subexpression, updateDate, 0);
            }

            return result;
        }

        /// <summary></summary>
        /// <param name="expression"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string RunScript(string expression, ref int seed)
        {
            return RunScript(expression, DateTime.Now, ref seed);
        }

        /// <summary></summary>
        /// <param name="expression"></param>
        /// <param name="updateDate"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string RunScript(string expression, DateTime updateDate, ref int seed)
        {
            return RunScript(expression, string.Empty, DateTime.Now, ref seed);
        }

        /// <summary></summary>
        /// <param name="expression"></param>
        /// <param name="updateDate"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string RunScript(string expression, string prefixCode, DateTime updateDate, ref int seed)
        {
            string result = null;

            // ����ǰ׺��ǩ
            expression = string.IsNullOrEmpty(prefixCode) ?
                expression.Replace("{prefix}", string.Empty) : expression.Replace("{prefix}", "{tag:" + prefixCode + "}");

            // ��������
            seed++;

            // ����Ϊ�ӱ���ʽ�б�
            IList<string[]> subexpressions = SplitExpression(expression);

            foreach (string[] subexpression in subexpressions)
            {
                result += AnalyzeExpression(subexpression, updateDate, seed);
            }

            return result;
        }

        /// <summary></summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static IList<string[]> SplitExpression(string expression)
        {
            // ����ʽ ʾ��
            // {dailyIncrement:seed:6}
            // {date:yyyyMMdd}{tag:-}{int:seed}
            IList<string[]> subexpressions = new List<string[]>();

            string[] list = expression.Split('}');

            foreach (string item in list)
            {
                if (item.Length < 2)
                {
                    continue;
                }

                subexpressions.Add(item.Substring(1, item.Length - 1).Split(':'));
            }

            return subexpressions;
        }

        /// <summary>��������ʽ</summary>
        /// <param name="subexpression"></param>
        /// <param name="updateDate"></param>
        /// <param name="seed"></param>
        /// <returns>����</returns>
        private static string AnalyzeExpression(string[] subexpression, DateTime updateDate, int seed)
        {
            switch (subexpression[0])
            {
                // ��ǩ����
                case "tag":
                    return subexpression[1];

                // ��������
                case "date":
                    if (subexpression.Length == 2)
                    {
                        return DateTime.Now.ToString(subexpression[1]);
                    }
                    else
                    {
                        return DateTime.Now.ToString("yyyyMMdd");
                    }

                // Guid����
                case "guid":
                    if (subexpression.Length == 1)
                    {
                        return Guid.NewGuid().ToString("D");
                    }
                    else
                    {
                        if (subexpression[1].ToUpper() == "32digits")
                        {
                            return Guid.NewGuid().ToString("N");
                        }
                        else
                        {
                            return Guid.NewGuid().ToString(subexpression[1].ToUpper());
                        }
                    }

                // �����ַ���
                case "random":
                    if (subexpression.Length == 3)
                    {
                        return StringHelper.ToRandom(subexpression[1], Convert.ToInt32(subexpression[2]));
                    }
                    else
                    {
                        return StringHelper.ToRandom(Convert.ToInt32(subexpression[1]));
                    }

                case "int":
                    if (subexpression.Length == 3)
                    {
                        return PaddingZero(seed, Convert.ToInt32(subexpression[2]));
                    }
                    else
                    {
                        return seed.ToString();
                    }

                // ÿ������������
                case "dailyIncrement":
                    if (updateDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        if (subexpression.Length == 3)
                        {
                            return PaddingZero(seed, Convert.ToInt32(subexpression[2]));
                        }
                        else
                        {
                            return seed.ToString();
                        }
                    }
                    else
                    {
                        if (subexpression.Length == 3)
                        {
                            return PaddingZero(1, Convert.ToInt32(subexpression[2]));
                        }
                        else
                        {
                            return "1";
                        }
                    }

                // ������������������
                case "code":
                    if (subexpression.Length == 2)
                    {
                        return PaddingZero(seed, Convert.ToInt32(subexpression[1]));
                    }
                    else
                    {
                        return PaddingZero(seed, 3);
                    }

                default:
                    return "UnkownExpression";
            }
        }

        /// <summary>���ֲ���</summary>
        /// <param name="seed"></param>
        /// <param name="lengthText"></param>
        /// <returns></returns>
        private static string PaddingZero(int seed, int length)
        {
            string zero = null;

            for (int i = 0; i < length; i++)
            {
                zero += "0";
            }

            return string.IsNullOrEmpty(zero) ? seed.ToString() : seed.ToString(zero);
        }
    }
}
