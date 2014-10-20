namespace X3Platform.Web.Component.Combobox
{
    using System;
    using System.Collections.Generic;

    /// <summary>组合框构建器</summary>
    public class ComboboxBuilder
    {
        #region 函数:Parse(string dataSource)
        /// <summary></summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static IList<ComboboxItem> Parse(string dataSource)
        {
            // 0:一般;1:重要;2:紧急;
            IList<ComboboxItem> list = new List<ComboboxItem>();

            string[] items = dataSource.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i++)
            {
                list.Add(new ComboboxItem(items[i]));
            }

            return list;
        }
        #endregion

        #region 函数:ParseText(string value, string dataSource)
        /// <summary></summary>
        /// <param name="value"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static string ParseText(string value, string dataSource)
        {
            IList<ComboboxItem> list = Parse(dataSource);

            foreach (ComboboxItem item in list)
            {
                if (item.Value == value) 
                    return item.Text;
            }

            return string.Empty;
        }
        #endregion

        #region 函数:ParseValue(string text, string dataSource)
        /// <summary></summary>
        /// <param name="text"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static string ParseValue(string text, string dataSource)
        {
            IList<ComboboxItem> list = Parse(dataSource);

            foreach (ComboboxItem item in list)
            {
                if (item.Text == text)
                    return item.Value;
            }

            return string.Empty;
        }
        #endregion
    }
}