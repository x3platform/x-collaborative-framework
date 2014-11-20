namespace X3Platform.Json
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    public class JsonDynamicObject : DynamicObject
    {
        private IDictionary<string, object> Dictionary { get; set; }

        public JsonDynamicObject(IDictionary<string, object> dictionary)
        {
            this.Dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.Dictionary[binder.Name];

            if (result is IDictionary<string, object>)
            {
                result = new JsonDynamicObject(result as IDictionary<string, object>);
            }
            else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
            {
                result = new List<JsonDynamicObject>((result as ArrayList).ToArray().Select(x => new JsonDynamicObject(x as IDictionary<string, object>)));
            }
            else if (result is ArrayList)
            {
                result = new List<object>((result as ArrayList).ToArray());
            }

            return this.Dictionary.ContainsKey(binder.Name);
        }
    }
}
