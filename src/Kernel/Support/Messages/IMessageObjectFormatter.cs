#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IMessageObject.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Messages
{
    /// <summary>消息对象输出为符合规定的 JSON 字符串</summary>
    public interface IMessageObjectFormatter
    {
        /// <summary>消息格式化</summary>
        /// <param name="json">JSON 格式字符串</param>
        /// <param name="nobrace">对象不包含最外面的大括号</param>
        /// <returns>格式化后的字符串</returns>
        string Format(string json, bool nobrace);
    }
}
