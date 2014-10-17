namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Xml;
    #endregion

    /// <summary>抽象实体类</summary>
    public abstract class EntityClass : ISerializedObject
    {
        #region 属性:EntityId
        /// <summary>实体类名称</summary>
        public abstract string EntityId
        {
            get;
        }
        #endregion

        #region 属性:EntityClassName
        /// <summary>实体类名称</summary>
        public string EntityClassName
        {
            get { return KernelContext.ParseObjectType(this.GetType()); }
        }
        #endregion

        #region 属性:Properties
        /// <summary>属性</summary>
        private Hashtable propertieCache = new Hashtable(13);

        /// <summary>属性</summary>
        public Hashtable Properties
        {
            get { return propertieCache; }
        }
        #endregion

        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
        public virtual string Serializable()
        {
            // 实现类需要重新实现此方法.
            throw new NotImplementedException("此对象未实现方法：string Serializable()。");
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>序列化对象</summary>
        /// <param name="displayComment">显示注释信息</param>
        /// <param name="displayFriendlyName">显示友好名称信息</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            // 实现类需要重新实现此方法.
            throw new NotImplementedException("此对象未实现方法：string Serializable(bool displayComment, bool displayFriendlyName)。");
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml元素</param>
        public virtual void Deserialize(XmlElement element)
        {
            // 实现类需要重新实现此方法.
            throw new NotImplementedException("此对象未实现方法：void Deserialize(XmlElement element)。");
        }
        #endregion

        #region 函数:Find(string id)
        /// <summary>查找实体对象</summary>
        /// <param name="id">标识</param>
        public virtual void Find(string id)
        {
            // 实现类需要重新实现此方法.
            throw new NotImplementedException("此对象未实现方法：void Find(string id) 。");
        }
        #endregion
    }
}
