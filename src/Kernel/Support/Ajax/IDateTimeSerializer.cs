namespace X3Platform.Ajax
{
    using System;
    using System.Text;

    /// <summary>日期格式序列化接口</summary>
    public interface IDateTimeSerializer
    {
        void Serialize(StringBuilder outString, string namingRule, string key, DateTime date);
    }
}
