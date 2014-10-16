namespace X3Platform.CategoryIndexes
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>文本和值对应的分类索引</summary>
    public class TextValueCategoryIndex : TextCategoryIndex
    {
        /// <summary></summary>
        /// <param name="text"></param>
        public TextValueCategoryIndex(string text)
            : this(text, text)
        {
        }

        public TextValueCategoryIndex(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }
    }
}