// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :ByteHelper.cs
//
// Description  :�ֽڴ�������
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2010-01-01
//
// =============================================================================

using System;
using System.IO;

namespace X3Platform.Util
{
    /// <summary>�ֽڴ�������</summary>
    public sealed class ByteHelper
    {
        #region 属性:ToFile(byte[] buffer, string path)
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

        #region 属性:ToStream(byte[] buffer)
        /// <summary></summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Stream ToStream(byte[] buffer)
        {
            Stream stream = new MemoryStream(buffer);

            return stream;
        }
        #endregion

        #region 属性:ToString(byte[] buffer)
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
