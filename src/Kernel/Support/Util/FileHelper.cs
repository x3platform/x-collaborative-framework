namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.IO;
    using System.Text;
    #endregion

    /// <summary>文件处理辅助类</summary>
    public class FileHelper
    {
        /// <summary>从文件读取 Stream</summary>
        public static Stream ToStream(string path)
        {
            byte[] buffer = null;

            // 打开文件
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // 读取文件的 byte[]
                buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                stream.Close();
            }

            return new MemoryStream(buffer);
        }

        #region 函数:Create(string path)
        public static void Create(string path)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!File.Exists(path))
            {
                FileStream stream = File.Create(path);

                stream.Close();
            }
        }
        #endregion

        #region 函数:Copy(string fromPath, string toPath)
        public static void Copy(string fromPath, string toPath)
        {
            //
            // 复制文件, 若目标文件已存在则改写.
            //

            File.Copy(fromPath, toPath, true);
        }
        #endregion

        #region 函数:Delete(string path)
        public static void Delete(string path)
        {
            //删除文件夹

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion

        #region 函数:Encoding GetEncoding(string path)
        /// <summary>获取文件字符编码</summary>
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

        #region 函数:Encoding GetEncoding(FileStream fileStream)
        /// <summary>获取文件流字符编码</summary>
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

            // 编码类型 Coding=编码类型.ASCII;
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

        #region 函数:GetTempFileName()
        /// <summary>获取临时文件名称</summary>
        public static string GetTempFileName()
        {
            return Path.GetRandomFileName();
        }
        #endregion
    }
}
