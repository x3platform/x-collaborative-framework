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
            Stream stream = new MemoryStream(buffer);

            return stream;
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
    }
}
