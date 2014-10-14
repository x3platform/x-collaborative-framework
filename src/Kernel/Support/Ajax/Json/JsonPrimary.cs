// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :JsonPrimary.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Ajax.Json
{
    /// <summary>Json Primary</summary>
    public class JsonPrimary
    {
        private string m_Key;

        /// <summary>键</summary>
        public string Key
        {
            get { return this.m_Key; }
        }

        private object m_Value;

        /// <summary>值</summary>
        public object Value
        {
            get { return this.m_Value; }
        }

        /// <summary></summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public JsonPrimary(string key, object value)
        {
            this.m_Key = key;
            this.m_Value = value;
        }
    }
}
