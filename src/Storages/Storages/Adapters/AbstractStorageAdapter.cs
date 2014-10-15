#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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
#endregion

namespace X3Platform.Storages.StorageAdapters
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary>�����洢������</summary>
    public abstract class AbstractStorageAdapter : IStorageAdapter
    {
        /// <summary>ѡ��</summary>
        protected Dictionary<string, string> options;

        /// <summary>�洢�ڵ�</summary>
        protected IStorageSchema storageSchema = null;

        /// <summary>�洢�ڵ�</summary>
        protected IStorageStrategy storageStrategy = null;

        /// <summary>�洢�ڵ�</summary>
        protected IList<IStorageNode> storageNodes = null;

        /// <summary>���캯��</summary>
        public AbstractStorageAdapter() { }

        /// <summary>��ʼ��</summary>
        protected void Initialize()
        {
            // �󶨴洢������Ϣ 
            this.storageStrategy = (IStorageStrategy)KernelContext.CreateObject(this.storageSchema.StrategyClassName);

            // �󶨴洢�ڵ���Ϣ
            this.storageNodes = StorageContext.Instance.StorageNodeService.FindAllBySchemaId(storageSchema.Id);
        }

        /// <summary>���ݽڵ����ͻ�ȡ�洢�ڵ���Ϣ</summary>
        /// <param name="type">�洢�ڵ����� Master | Search</param>
        public virtual IStorageNode GetStorageNode(string type)
        {
            foreach (IStorageNode storageNode in this.storageNodes)
            {
                if (storageNode.Type == type)
                {
                    return storageNode;
                }
            }

            return null;
        }

        /// <summary>���ݴ洢���Ի�ȡ�洢�ڵ���Ϣ</summary>
        /// <param name="strategy">�洢�ڵ�����</param>
        public virtual IStorageNode GetStorageNode()
        {
            return this.storageStrategy.GetStorageNode();
        }

        /// <summary>ִ��</summary>
        /// <returns></returns>
        public abstract object Execute(string commandText, string[] args);
    }
}