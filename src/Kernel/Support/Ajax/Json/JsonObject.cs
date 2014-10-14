// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxRequest.cs
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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary></summary>
    public class JsonObject
    {
        private IDictionary<string, object> dictionary = null;

        public ICollection<string> Keys
        {
            get { return dictionary.Keys; }
        }

        public JsonObject(IDictionary<string, object> dictionary)
        {
            this.dictionary = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> item in dictionary)
            {
                if (item.Value is IDictionary<string, object>)
                {
                    this.dictionary.Add(item.Key, new JsonObject((IDictionary<string, object>)(item.Value)));
                }
                else if (item.Value is ArrayList)
                {
                    this.dictionary.Add(item.Key, new JsonArray(item.Key, (item.Value as ArrayList).ToArray()));
                }
                else
                {
                    this.dictionary.Add(item.Key, new JsonPrimary(item.Key, item.Value));
                }
            }
        }

        public object this[string key]
        {
            get
            {
                if (this.dictionary.ContainsKey(key))
                {
                    return this.dictionary[key];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary></summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T TryGet<T>(string key)
        {
            object result = this[key];

            if (result == null) { return default(T); }

            return (T)result;
        }

        /// <summary></summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object TryGetValue(string key)
        {
            object result = this[key];

            if (result == null)
            {
                return null;
            }

            if (result is JsonPrimary)
            {
                return ((JsonPrimary)result).Value;
            }

            if (result is JsonArray)
            {
                return ((JsonArray)result).Value;
            }

            return null;
        }
    }
}
