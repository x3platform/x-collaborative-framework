namespace X3Platform
{
    using System;

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
