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

namespace X3Platform.CategoryIndexes
{
    using System;
    using System.Collections.Generic;

    /// <summary></summary>
    public class CategoryIndexWriter
    {
        ICategoryIndex root = null;

        public CategoryIndexWriter(string name)
        {
            root = new TextCategoryIndex(name);
        }

        public void Read(string index)
        {
            root.LoadChildNode(new TextCategoryIndex(root, index));
        }

        public ICategoryIndex Write()
        {
            return root;
        }
    }
}