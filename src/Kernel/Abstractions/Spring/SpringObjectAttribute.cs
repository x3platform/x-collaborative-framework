namespace X3Platform.Spring
{
    using System;

    /// <summary>Srping�������Ա�ǩ</summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SpringObjectAttribute : Attribute
    {
        private string objectName;

        /// <summary>���캯��</summary>
        /// <param name="name">�ӿ�����</param>
        public SpringObjectAttribute(string name)
        {
            this.objectName = name;
        }

        /// <summary>�ӿ�����</summary>
        public string Name
        {
            get { return this.objectName; }
        }
    }
}
