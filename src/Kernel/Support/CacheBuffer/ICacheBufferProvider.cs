// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :ICacheBufferProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;

using X3Platform.Spring;

namespace X3Platform.CacheBuffer
{
    /// <summary>���������ṩ��</summary>
    [SpringObject("X3Platform.CacheBuffer.ICacheBufferProvider")]
    public interface ICacheBufferProvider
    {
        #region ����:����
        /// <summary>����</summary>
        object this[string key] { get; set; }
        #endregion

        #region ����:Contains(string key)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);
        #endregion

        #region ����:Read(string key)
        ///<summary>��ȡ������¼</summary>
        ///<param name="key">��ʶ</param>
        ///<returns>���ػ�����������ϸ��Ϣ</returns>
        object Read(string key);
        #endregion

        #region ����:Write(string key, object value)
        ///<summary>д�뻺����¼</summary>
        ///<param name="key">��־</param>
        ///<param name="value">RuanYu.CacheBuffer.Model.TaskInfo Id��</param>
        void Write(string key, object value);
        #endregion

        #region ����:Write(string key, object value, int minutes)
        void Write(string key, object value, int minutes);
        #endregion

        #region ����:Delete(string key)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="Id">RuanYu.CacheBuffer.Model.TaskInfo Id��</param>
        ///<returns>����һ�� RuanYu.CacheBuffer.Model.TaskInfo ʵ������ϸ��Ϣ</returns>
        void Delete(string key);
        #endregion

        #region ����:Clear()
        ///<summary>��������</summary>
        void Clear();
        #endregion
    }
}