namespace X3Platform.Ajax.Json
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
#if !NETCORE10
    using System.Web.Script.Serialization;

    /// <summary></summary>
    public class JsonObjectConverter : JavaScriptConverter
    {
        /// <summary></summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            if (type == typeof(JsonObject))
            {
                return new JsonObject(dictionary);
            }

            return null;
        }

        /// <summary></summary>
        /// <param name="obj"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(object) })); }
        }

        /// <summary></summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JsonObject Deserialize(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.RegisterConverters(new JavaScriptConverter[] { new JsonObjectConverter() });

            return serializer.Deserialize<JsonObject>(json);
        }
    }
#endif
}
