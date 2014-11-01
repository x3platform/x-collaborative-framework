namespace X3Platform.Security
{
    /// <summary>密文格式</summary>
    public enum CiphertextFormat
    {
        /// <summary>Base64 文本信息</summary>
        Base64String,
        /// <summary>十六进制文本信息</summary>
        HexString,
        /// <summary>没有连字符的十六进制文本信息</summary>
        HexStringWhitoutHyphen
    }
}
