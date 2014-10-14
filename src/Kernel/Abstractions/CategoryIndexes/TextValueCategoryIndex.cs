// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;

namespace X3Platform.CategoryIndexes
{
    /// <summary>文本和值对应的分类索引</summary>
    public class TextValueCategoryIndex : TextCategoryIndex
    {
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