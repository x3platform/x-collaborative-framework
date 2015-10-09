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

        /// <summary></summary>
        /// <param name="name"></param>
        public CategoryIndexWriter(string name)
        {
            root = new TextCategoryIndex(name);
        }

        /// <summary></summary>
        /// <param name="index"></param>
        public void Read(string index)
        {
            root.LoadChildNode(new TextCategoryIndex(root, index));
        }

        /// <summary></summary>
        /// <returns></returns>
        public ICategoryIndex Write()
        {
            return root;
        }
    }
}