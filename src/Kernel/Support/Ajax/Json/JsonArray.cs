namespace X3Platform.Ajax.Json
{
    using System.Collections;

    /// <summary>Json Array</summary>
    public class JsonArray : ArrayList
    {
        private string m_Key;

        /// <summary>键</summary>
        public string Key
        {
            get { return m_Key; }
        }

        /// <summary>值</summary>
        public ArrayList Value
        {
            get { return this; }
        }

        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="array"></param>
        public JsonArray(string key, object[] array)
        {
            this.m_Key = key;

            base.AddRange(array);
        }
    }
}
