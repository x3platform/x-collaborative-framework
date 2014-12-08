namespace X3Platform.Json
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;

    using X3Platform.Ajax.Configuration;
    using X3Platform.Util;

    /// <summary>JSON 动态对象</summary>
    public class JsonDynamicObject : DynamicObject
    {
        private IDictionary<string, object> dictionary { get; set; }

        public JsonDynamicObject(JsonData data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }

            this.dictionary = new Dictionary<string, object>();

            foreach (string key in data.Keys)
            {
                dictionary.Add(key, data[key]);
            }
        }

        public JsonDynamicObject(IDictionary<string, object> dictionary)
        {
            if (dictionary == null) { throw new ArgumentNullException("dictionary"); }

            this.dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!this.dictionary.TryGetValue(binder.Name, out result))
            {
                // return null to avoid exception.  caller can check for null this way...
                result = null;
                return true;
            }

            result = GetResultObject(result);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] != null)
            {
                if (!this.dictionary.TryGetValue(indexes[0].ToString(), out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = GetResultObject(result);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool camelStyle)
        {
            var outString = new StringBuilder("{");

            ToString(outString, camelStyle);

            return outString.ToString();
        }

        private void ToString(StringBuilder outString, bool camelStyle)
        {
            var firstInDictionary = true;
            foreach (var pair in this.dictionary)
            {
                if (!firstInDictionary)
                    outString.Append(",");
                firstInDictionary = false;

                var value = pair.Value;
                var name = pair.Key;

                if (camelStyle)
                {
                    if (AjaxConfigurationView.Instance.Configuration.SpecialWords[name] == null)
                    {
                        name = StringHelper.ToFirstLower(name);
                    }
                    else
                    {
                        name = AjaxConfigurationView.Instance.Configuration.SpecialWords[name].Value;
                    }
                }

                var data = value as JsonData;

                if (value == null)
                {
                    outString.AppendFormat("\"{0}\":null", name);
                }
                else if (value is string || (data != null && data.IsString))
                {
                    outString.AppendFormat("\"{0}\":\"{1}\"", name, value);
                }
                else if (value is DateTime)
                {
                    outString.AppendFormat("\"{0}\":\"{1}\"", name, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else if (value is IDictionary<string, object>)
                {
                    new JsonDynamicObject((IDictionary<string, object>)value).ToString(outString, camelStyle);
                }
                else if (value is ArrayList)
                {
                    outString.AppendFormat("\"{0}\":[", name);

                    var firstInArray = true;

                    foreach (var arrayValue in (ArrayList)value)
                    {
                        if (!firstInArray)
                            outString.Append(",");
                        firstInArray = false;
                        if (arrayValue is IDictionary<string, object>)
                            new JsonDynamicObject((IDictionary<string, object>)arrayValue).ToString(outString, camelStyle);
                        else if (arrayValue is string)
                            outString.AppendFormat("\"{0}\"", arrayValue);
                        else
                            outString.AppendFormat("{0}", arrayValue);

                    }

                    outString.Append("]");
                }
                else if (data != null && data.IsBoolean)
                {
                    outString.AppendFormat("\"{0}\":{1}", name, value.ToString().ToLower());
                }
                else if (data != null && data.IsArray)
                {
                    outString.AppendFormat("\"{0}\":[", name);

                    var firstInArray = true;

                    for (int i = 0; i < data.Count; i++)
                    {
                        if (!firstInArray)
                            outString.Append(",");
                        firstInArray = false;

                        if (data[i] is JsonData)
                            outString.Append(GetResultObject(data[i]).ToString());
                        else if (data[i] is IDictionary<string, object>)
                           new JsonDynamicObject((IDictionary<string, object>)data[i]).ToString(outString, camelStyle);
                        else
                            outString.Append(data[i]);
                    }
                    outString.Append("]");
                }
                else if (data != null && data.IsObject)
                {
                    outString.AppendFormat("\"{0}\":{1}", name, data.ToJson());
                }
                else
                {
                    outString.AppendFormat("\"{0}\":{1}", name, value);
                }
            }

            outString.Append("}");
        }

        private static object GetResultObject(object result)
        {
            var data = result as JsonData;

            if (data != null)
            {
                if (data.IsString)
                {
                    return data.ToString();
                }
                else if (data.IsBoolean)
                {
                    return Convert.ToBoolean(data.ToString());
                }
                else if (data.IsDouble)
                {
                    return Convert.ToDouble(data.ToString());
                }
                else if (data.IsLong)
                {
                    return Convert.ToInt64(data.ToString());
                }
                else if (data.IsInt)
                {
                    return Convert.ToInt64(data.ToString());
                }
                else if (data.IsArray)
                {
                    var array = new ArrayList();

                    for (int i = 0; i < data.Count; i++)
                    {
                        array.Add(GetResultObject(data[i]));
                    }

                    return new List<object>(array.Cast<object>()); ;
                }
                else if (data.IsObject)
                {
                    IDictionary<string, object> buffer = new Dictionary<string, object>();

                    foreach (string key in data.Keys)
                    {
                        buffer.Add(key, data[key]);
                    }

                    return new JsonDynamicObject(buffer);
                }
            }

            // 字典类型
            var dictionary = result as IDictionary<string, object>;

            if (dictionary != null)
            {
                return new JsonDynamicObject(dictionary);
            }

            // 数组类型
            var arrayList = result as ArrayList;

            if (arrayList != null && arrayList.Count > 0)
            {
                return arrayList[0] is IDictionary<string, object>
                    ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new JsonDynamicObject(x)))
                    : new List<object>(arrayList.Cast<object>());
            }

            return result;
        }
    }
}
