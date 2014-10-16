namespace X3Platform.CategoryIndexes
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

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