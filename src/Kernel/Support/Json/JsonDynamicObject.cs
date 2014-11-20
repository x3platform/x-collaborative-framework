namespace X3Platform.Json
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using System;

    public class JsonDynamicObject : DynamicObject
    {
        private IDictionary<string, object> dictionary { get; set; }

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

            result = WrapResultObject(result);
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

                result = WrapResultObject(result);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        private static object WrapResultObject(object result)
        {
            var dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
                return new JsonDynamicObject(dictionary);

            var arrayList = result as ArrayList;
        
            if (arrayList != null && arrayList.Count > 0)
            {
                return arrayList[0] is IDictionary<string, object>
                    ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new JsonDynamicObject(x)))
                    : new List<object>(arrayList.Cast<object>());
            }

            return result;
        }

        public override string ToString()
        {
            var outString = new StringBuilder("{");
            
            ToString(outString);

            return outString.ToString();
        }

        private void ToString(StringBuilder outString)
        {
            var firstInDictionary = true;
            foreach (var pair in this.dictionary)
            {
                if (!firstInDictionary)
                    outString.Append(",");
                firstInDictionary = false;
                var value = pair.Value;
                var name = pair.Key;
                if (value is string)
                {
                    outString.AppendFormat("{0}:\"{1}\"", name, value);
                }
                else if (value is IDictionary<string, object>)
                {
                    new JsonDynamicObject((IDictionary<string, object>)value).ToString(outString);
                }
                else if (value is ArrayList)
                {
                    outString.Append(name + ":[");
                    var firstInArray = true;
                    foreach (var arrayValue in (ArrayList)value)
                    {
                        if (!firstInArray)
                            outString.Append(",");
                        firstInArray = false;
                        if (arrayValue is IDictionary<string, object>)
                            new JsonDynamicObject((IDictionary<string, object>)arrayValue).ToString(outString);
                        else if (arrayValue is string)
                            outString.AppendFormat("\"{0}\"", arrayValue);
                        else
                            outString.AppendFormat("{0}", arrayValue);

                    }
                    outString.Append("]");
                }
                else
                {
                    outString.AppendFormat("{0}:{1}", name, value);
                }
            }
            outString.Append("}");
        }
    }
}
