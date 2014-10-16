namespace X3Platform.CategoryIndexes
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>对象缓存接口</summary>
    public interface ICategoryIndex
    {
        string Text { get; set; }

        string Value { get; set; }

        ICategoryIndex Parent { get; }
        
        IList<ICategoryIndex> ChildNodes { get; }
        
        bool HasChildren { get; }

        void LoadChildNode(ICategoryIndex node);

        void LoadChildNodes(IList<ICategoryIndex> nodes);
    }
}