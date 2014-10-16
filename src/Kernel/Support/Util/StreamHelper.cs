namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.IO;
    #endregion

    /// <summary>流处理辅助类</summary>
    public sealed class StreamHelper
    {
        #region 函数:ToFile(Stream stream, string path)
        public static long ToFile(Stream stream, string path)
        {
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

        #region 函数:ToBytes(Stream stream)
        public static byte[] ToBytes(Stream stream)
        {
            long length = stream.Length;

            byte[] buffer = null;

            // Set the position to the beginning of the stream.
            stream.Seek(0, SeekOrigin.Begin);

            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                buffer = binaryReader.ReadBytes((int)length);

                binaryReader.Close();
            }

            return buffer;
        }
        #endregion

        #region 函数:ToString(Stream stream)
        /// <summary></summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToString(Stream stream)
        {
            string text = null;

            //
            // 在流读过一遍后被移动到了末尾，如果再次读需要移动指针。
            // 否则就会出现没有读到数据的情况。 
            //

            // Set the position to the beginning of the stream.
            stream.Seek(0, SeekOrigin.Begin);

            using (StreamReader streamReader = new StreamReader(stream))
            {
                text = streamReader.ReadToEnd();
            }

            return text.Trim();
        }
        #endregion

        #region 函数:ToUTF8String(Stream stream)
        /// <summary></summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToUTF8String(Stream stream)
        {
            string text = null;

            //if (stream is System.IO.MemoryStream)
            //{
            //    text = System.Text.Encoding.UTF8.GetString(((MemoryStream)stream).ToArray());
            //}
            //else
            //{
            //
            // 在流读过一遍后被移动到了末尾，如果再次读需要移动指针。
            // 否则就会出现没有读到数据的情况。 
            //
            // Set the position to the beginning of the stream.
            stream.Seek(0, SeekOrigin.Begin);

            using (StreamReader streamReader = new StreamReader(stream))
            {
                text = streamReader.ReadToEnd();
            }
            //}

            // 去除utf-8的首个字符.
            if (!string.IsNullOrEmpty(text) && text[0] == 65279)
                text = text.Substring(1);

            return text.Trim();
        }
        #endregion
    }
}
