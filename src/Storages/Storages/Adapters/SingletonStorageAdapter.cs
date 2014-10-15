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
    public sealed class SingletonStorageAdapter : AbstractStorageAdapter
    {
        /// <summary>���캯��</summary>
        public SingletonStorageAdapter(IStorageSchema storageSchema)
        {
            // ��ȡ�洢�ܹ���Ϣ
            this.storageSchema = storageSchema;

            // ��ʼ���洢���Ժʹ洢�ڵ���Ϣ
            this.Initialize();
        }
        
        /// <summary>ִ��</summary>
        /// <returns></returns>
        public override object Execute(string commandText, string[] args)
        {
            return null;
        }
    }
}