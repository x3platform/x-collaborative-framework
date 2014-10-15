// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :FileHelper.cs
//
// Description  :file helper
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2010-01-01
//
// =============================================================================

using System;
using System.IO;
using System.Text;

namespace X3Platform.Util
{
    /// <summary>�ļ�������</summary>
    public class FileHelper
    {
        /// <summary>���ļ���ȡ Stream</summary>
        public static Stream ToStream(string path)
        {
            byte[] buffer = null;

            // �����ļ�
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // ��ȡ�ļ��� byte[]
                buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                stream.Close();
            }

            return new MemoryStream(buffer);
        }

        #region 属性:Create(string path)
        public static void Create(string path)
        {
            // �ж�Ŀ��Ŀ¼�Ƿ������������������½�֮
            if (!File.Exists(path))
            {
                FileStream stream = File.Create(path);

                stream.Close();
            }
        }
        #endregion

        #region 属性:Copy(string fromPath, string toPath)
        public static void Copy(string fromPath, string toPath)
        {
            //
            // �����ļ�, ��Ŀ���ļ��Ѵ�������д.
            //

            File.Copy(fromPath, toPath, true);
        }
        #endregion

        #region 属性:Delete(string path)
        public static void Delete(string path)
        {
            //ɾ���ļ���

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion

        #region 属性:Encoding GetEncoding(string path)
        /// <summary>��ȡ�ļ��ַ�����</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(string path)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Default;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                encoding = GetEncoding(fileStream);

                fileStream.Close();
            }

            return encoding;
        }
        #endregion

        #region 属性:Encoding GetEncoding(FileStream fileStream)
        /// <summary>��ȡ�ļ����ַ�����</summary>
        public static Encoding GetEncoding(FileStream fileStream)
        {
            /*
             * byte[] Unicode=new byte[]{0xFF,0xFE};
             * byte[] UnicodeBIG=new byte[]{0xFE,0xFF};
             * byte[] UTF8=new byte[]{0xEF,0xBB,0xBF};
             */
            byte[] buffer = null;

            using (BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.Default))
            {
                buffer = binaryReader.ReadBytes(3);

                binaryReader.Close();
            }

            // �������� Coding=��������.ASCII;
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
        }
        #endregion

        #region 属性:GetTempFileName()
        /// <summary>��ȡ��ʱ�ļ�����</summary>
        public static string GetTempFileName()
        {
            return Path.GetRandomFileName();
        }
        #endregion
    }
}
