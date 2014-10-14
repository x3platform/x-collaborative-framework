// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :ISerializedObject.cs
//
// Description  :序列化对象接口
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Xml;

namespace X3Platform
{
    /// <summary>序列化JSON对象接口</summary>
    public interface ISerializedJavaScriptObject
    {
        #region 函数:ToJSON
        /// <summary>序列化JSON对象</summary>
        /// <returns></returns>
        string ToJSON();
        #endregion

        #region 函数:FromJSON(string json)
        /// <summary>反序列化JSON对象</summary>
        /// <param name="json">json元素对象</param>
        void FromJSON(string json);
        #endregion
    }
}
