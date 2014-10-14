﻿#region Copyright & Author
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
#endregion

#region Using Libraries
using System;
using System.Xml;
#endregion

namespace X3Platform
{
    /// <summary>序列化对象接口</summary>
    public interface ISerializedObject
    {
        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
        /// <returns></returns>
        string Serializable();
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>序列化对象</summary>
        /// <param name="displayComment">显示注释信息</param>
        /// <param name="displayFriendlyName">显示友好名称信息</param>
        /// <returns></returns>
        string Serializable(bool displayComment, bool displayFriendlyName);
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml元素</param>
        void Deserialize(XmlElement element);
        #endregion
    }
}
