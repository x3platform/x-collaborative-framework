// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :EntityClass.cs
//
// Description  :����ʵ����
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform
{
    using System;
    using System.Collections;
    using System.Xml;

    /// <summary>����ʵ����</summary>
    public abstract class EntityClass : ISerializedObject
    {
        #region 属性:EntityId
        /// <summary>ʵ��������</summary>
        public abstract string EntityId
        {
            get;
        }
        #endregion

        #region 属性:EntityClassName
        /// <summary>ʵ��������</summary>
        public string EntityClassName
        {
            get { return KernelContext.ParseObjectType(this.GetType()); }
        }
        #endregion

        #region 属性:Properties
        /// <summary>����</summary>
        private Hashtable propertieCache = new Hashtable(13);

        /// <summary>����</summary>
        public Hashtable Properties
        {
            get { return propertieCache; }
        }
        #endregion

        #region 属性:Serializable()
        /// <summary>���л�����</summary>
        public virtual string Serializable()
        {
            // ʵ������Ҫ����ʵ�ִ˷���.
            throw new NotImplementedException("�˶���δʵ�ַ�����string Serializable()��");
        }
        #endregion

        #region 属性:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>���л�����</summary>
        /// <param name="displayComment">��ʾע����Ϣ</param>
        /// <param name="displayFriendlyName">��ʾ�Ѻ�������Ϣ</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            // ʵ������Ҫ����ʵ�ִ˷���.
            throw new NotImplementedException("�˶���δʵ�ַ�����string Serializable(bool displayComment, bool displayFriendlyName)��");
        }
        #endregion

        #region 属性:Deserialize(XmlElement element)
        /// <summary>�����л�����</summary>
        /// <param name="element">XmlԪ��</param>
        public virtual void Deserialize(XmlElement element)
        {
            // ʵ������Ҫ����ʵ�ִ˷���.
            throw new NotImplementedException("�˶���δʵ�ַ�����void Deserialize(XmlElement element)��");
        }
        #endregion

        #region 属性:Find(string id)
        /// <summary>����ʵ������</summary>
        /// <param name="id">��ʶ</param>
        public virtual void Find(string id)
        {
            // ʵ������Ҫ����ʵ�ִ˷���.
            throw new NotImplementedException("�˶���δʵ�ַ�����void Find(string id) ��");
        }
        #endregion
    }
}
