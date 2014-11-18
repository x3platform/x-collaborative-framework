namespace X3Platform.DigitalNumber
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Util;
    #endregion

    /// <summary>流水号脚本</summary>
    public class DigitalNumberScript
    {
        /// <summary>运行流水号前缀脚本</summary>
        /// <param name="expression"></param>
        /// <param name="prefixCode"></param>
        /// <returns></returns>
        public static string RunPrefixScript(string expression, string prefixCode)
        {
            return RunPrefixScript(expression, prefixCode, DateTime.Now);
        }

        /// <summary>运行流水号前缀脚本</summary>
        /// <param name="expression"></param>
        /// <param name="prefixCode"></param>
        /// <param name="updateDate"></param>
        /// <returns></returns>
        public static string RunPrefixScript(string expression, string prefixCode, DateTime updateDate)
        {
            string result = null;

            // 处理前缀标签
            expression = string.IsNullOrEmpty(prefixCode) ?
                expression.Replace("{prefix}", string.Empty) : expression.Replace("{prefix}", "{tag:" + prefixCode + "}");

            // 拆分为子表达式列表
            IList<string[]> subexpressions = SplitExpression(expression);

            foreach (string[] subexpression in subexpressions)
            {
                // 忽略需要自增种子的表达式类型: code | int | dailyIncrement
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
            return RunScript(expression, string.Empty, updateDate, ref seed);
        }

        /// <summary></summary>
        /// <param name="expression"></param>
        /// <param name="updateDate"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string RunScript(string expression, string prefixCode, DateTime updateDate, ref int seed)
        {
            string result = null;

            // 处理前缀标签
            expression = string.IsNullOrEmpty(prefixCode) ?
                expression.Replace("{prefix}", string.Empty) : expression.Replace("{prefix}", "{tag:" + prefixCode + "}");

            // 种子自增
            seed++;

            // 拆分为子表达式列表
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
            // 表达式 示例
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

        /// <summary>分析表达式</summary>
        /// <param name="subexpression"></param>
        /// <param name="updateDate"></param>
        /// <param name="seed"></param>
        /// <returns>结果</returns>
        private static string AnalyzeExpression(string[] subexpression, DateTime updateDate, int seed)
        {
            switch (subexpression[0])
            {
                // 标签类型
                case "tag":
                    return subexpression[1];

                // 日期类型
                case "date":
                    if (subexpression.Length == 2)
                    {
                        return DateTime.Now.ToString(subexpression[1]);
                    }
                    else
                    {
                        return DateTime.Now.ToString("yyyyMMdd");
                    }

                // Guid类型
                case "guid":
                    if (subexpression.Length == 1)
                    {
                        return Guid.NewGuid().ToString("D");
                    } 
                    else if (subexpression.Length == 3)
                    {
                        if (subexpression[2].ToUpper() == "U")
                        {
                            // U = Upper
                            return Guid.NewGuid().ToString(subexpression[1].ToUpper()).ToUpper();
                        }
                        else if (subexpression[2].ToUpper() == "L")
                        {
                            // L = Lower
                            return Guid.NewGuid().ToString(subexpression[1].ToUpper()).ToLower();
                        }
                        else 
                        {
                            return Guid.NewGuid().ToString(subexpression[1].ToUpper());
                        }
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

                // 随机字符串
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

                // 每日自增型数字
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

                // 整数类型自增型数字
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

        /// <summary>数字补零</summary>
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
