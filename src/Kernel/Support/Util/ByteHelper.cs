namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.IO;
    #endregion

    /// <summary>字节处理辅助类</summary>
    public sealed class ByteHelper
    {
        #region 函数:ToFile(byte[] buffer, string path)
        /// <summary></summary>
        /// <param name="buffer"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long ToFile(byte[] buffer, string path)
        {
            Stream stream = new MemoryStream(buffer);

            long length = stream.Length;

            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                using (FileStream fileStream = File.Create(path))
                {
                    fileStream.Write(binaryReader.ReadBytes((int)length), 0, (int)length);

                    fileStream.Close();
                }

                binaryReader.Close();
            }

            return length;
        }
        #endregion

        #region 函数:ToStream(byte[] buffer)
        /// <summary></summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Stream ToStream(byte[] buffer)
        {
            return new MemoryStream(buffer);
        }
        #endregion

        #region 函数:ToString(byte[] buffer)
        /// <summary></summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToString(byte[] buffer)
        {
            string text = null;

            using (Stream stream = new MemoryStream(buffer))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    text = streamReader.ReadToEnd();
                }
            }

            return text;
        }
        #endregion

        #region 函数:ToBase64(byte[] buffer)
        /// <summary>将 Base64 编码文本转换成文本</summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToBase64(byte[] buffer)
        {
            int base64ArraySize = (int)Math.Ceiling(buffer.Length / 3d) * 4;

            char[] charBuffer = new char[base64ArraySize];

            Convert.ToBase64CharArray(buffer, 0, buffer.Length, charBuffer, 0);

            return new string(charBuffer);
        }
        #endregion

        #region 函数:FromBase64(string base64Text)
        /// <summary>将 Base64 编码文本转换成 byte[]</summary>
        /// <param name="base64Text">Base64 编码文本</param>
        /// <returns></returns>
        public static Byte[] FromBase64(string base64Text)
        {
            return Convert.FromBase64String(base64Text);
        }
        #endregion
    }
}
