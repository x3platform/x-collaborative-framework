namespace X3Platform.Web.Component.Combobox
{
    using System;
    using System.Collections.Generic;

    /// <summary>��Ͽ򹹽���</summary>
    public class ComboboxBuilder
    {
        #region ����:Parse(string dataSource)
        /// <summary></summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static IList<ComboboxItem> Parse(string dataSource)
        {
            // 0:һ��;1:��Ҫ;2:����;
            IList<ComboboxItem> list = new List<ComboboxItem>();

            string[] items = dataSource.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i++)
            {
                list.Add(new ComboboxItem(items[i]));
            }

            return list;
        }
        #endregion

        #region ����:ParseText(string value, string dataSource)
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

        #region ����:ParseValue(string text, string dataSource)
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