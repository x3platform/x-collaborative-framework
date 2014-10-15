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

namespace X3Platform.Storages
{
    using System;

    /// <summary>�����ӿ�</summary>
    public interface IStorageAdapter
    {
        /// <summary>ִ��</summary>
        /// <returns></returns>
        object Execute(string commandText, string[] args);
    }
}