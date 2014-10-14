// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :StreamHelper.cs
//
// Description  :stream helper
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
    /// <summary>��������</summary>
    public sealed class StreamHelper
    {
        #region ����:ToFile(Stream stream, string path)
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

        #region ����:ToBytes(Stream stream)
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

        #region ����:ToString(Stream stream)
        /// <summary></summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToString(Stream stream)
        {
            string text = null;

            //
            // ��������һ�������ƶ�����ĩβ�������ٴζ���Ҫ�ƶ�ָ�롣
            // �����ͻ�����û�ж������ݵ������� 
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

        #region ����:ToUTF8String(Stream stream)
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
            // ��������һ�������ƶ�����ĩβ�������ٴζ���Ҫ�ƶ�ָ�롣
            // �����ͻ�����û�ж������ݵ������� 
            //
            // Set the position to the beginning of the stream.
            stream.Seek(0, SeekOrigin.Begin);

            using (StreamReader streamReader = new StreamReader(stream))
            {
                text = streamReader.ReadToEnd();
            }
            //}

            // ȥ��utf-8���׸��ַ�.
            if (!string.IsNullOrEmpty(text) && text[0] == 65279)
                text = text.Substring(1);

            return text.Trim();
        }
        #endregion
    }
}
