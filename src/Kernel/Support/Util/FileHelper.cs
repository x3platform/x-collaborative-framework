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
        #region 函数:ToStream(string path)
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
        #endregion

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
            Encoding encoding = Encoding.Default;

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
            // http://technet.microsoft.com/zh-cn/dd374101%28v=vs.100%29.aspx

            // Byte-order mark      Description
            // EF BB BF	            UTF-8
            // FF FE	            UTF-16, little endian
            // FE FF	            UTF-16, big endian
            // FF FE 00 00	        UTF-32, little endian
            // 00 00 FE FF	        UTF-32, big-endian

            /*
            * byte[] Unicode=new byte[]{0xFF,0xFE};
            * byte[] UnicodeBIG=new byte[]{0xFE,0xFF};
            * byte[] UTF8=new byte[]{0xEF,0xBB,0xBF};
            */
            byte[] buffer = null;

            using (BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.Default))
            {
                buffer = binaryReader.ReadBytes(Convert.ToInt32(fileStream.Length));
                binaryReader.Close();
            }

            if ((buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF))
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
            {
                return System.Text.Encoding.Unicode;
            }
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
            {
                return System.Text.Encoding.BigEndianUnicode;
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE && buffer[2] == 0x00 && buffer[3] == 0x00)
            {
                return System.Text.Encoding.UTF32;
            }
            else if (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[3] == 0xFE && buffer[2] == 0xFF)
            {
                return System.Text.Encoding.GetEncoding("UTF-32BE");
            }
            else
            {
                if (IsUTF8(buffer))
                    return Encoding.GetEncoding(65001); //utf8  
                else
                    // Encoding.ASCII
                    return System.Text.Encoding.Default;
            }
        }
        #endregion

        #region 私有函数:IsUTF8(byte[] bytes)
        /// <summary>判断无签名的UTF8文件</summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static bool IsUTF8(byte[] bytes)
        {
            // 参考资料
            // http://www.codeguru.com/cpp/misc/misc/multi-lingualsupport/article.php/c10451/The-Basics-of-UTF8.htm
            // http://www.cnblogs.com/mingxing/archive/2009/03/30/1424984.html

            // 汉字编码区别于其他编码的标志就是汉字编码的最高位是1. 
            // 0x80在计算机内部表示为 1000 0000
            // temp & 0x80 == 0x80 常用于判断当前字符是否是汉字.&是按位与, 对应都是1时才为1, 其它情况均未0. 如: 
            // 1010 1011 & 1000 0000 = 1000 0000 即 temp&0x80

            // 计算当前正分析的字符应还有的字节数
            int charByteCounter = 1;

            // 当前分析的字节
            byte curByte;

            for (int i = 0; i < bytes.Length; i++)
            {
                curByte = bytes[i];

                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        // 判断当前字节信息
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }

                        // 标记位首位若为非0 则至少以2个1开始 如:110X XXXX ........... 1111 110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // 若是 UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }

                    charByteCounter--;
                }
            }

            if (charByteCounter > 1) { throw new Exception("非预期的字节格式"); }

            return true;
        }
        #endregion

        #region 函数:GetTempFileName()
        /// <summary>获取临时文件名称</summary>
        public static string GetTempFileName()
        {
            return Path.GetRandomFileName();
        }
        #endregion

        #region 函数:GetFileType(byte[] buffer)
        /// <summary>获取文件类型</summary>
        public static string GetFileType(byte[] buffer)
        {
            // JPEG (jpg)，文件头：FFD8FF
// PNG (png)，文件头：89504E47
// GIF (gif)，文件头：47494638
// TIFF (tif)，文件头：49492A00
// Windows Bitmap (bmp)，文件头：424D
// CAD (dwg)，文件头：41433130

// Adobe Photoshop (psd)，文件头：38425053

// Rich Text Format (rtf)，文件头：7B5C727466

// XML (xml)，文件头：3C3F786D6C

// HTML (html)，文件头：68746D6C3E

// Email [thorough only] (eml)，文件头：44656C69766572792D646174653A

// Outlook Express (dbx)，文件头：CFAD12FEC5FD746F

// Outlook (pst)，文件头：2142444E

// MS Word/Excel (xls.or.doc)，文件头：D0CF11E0

// MS Access (mdb)，文件头：5374616E64617264204A

// WordPerfect (wpd)，文件头：FF575043

// Postscript (eps.or.ps)，文件头：252150532D41646F6265

// Adobe Acrobat (pdf)，文件头：255044462D312E

// Quicken (qdf)，文件头：AC9EBD8F

// Windows Password (pwl)，文件头：E3828596

// ZIP Archive (zip)，文件头：504B0304

// RAR Archive (rar)，文件头：52617221

// Wave (wav)，文件头：57415645

// AVI (avi)，文件头：41564920

//Real Audio (ram)，文件头：2E7261FD

//Real Media (rm)，文件头：2E524D46

//MPEG (mpg)，文件头：000001BA

//MPEG (mpg)，文件头：000001B3

//Quicktime (mov)，文件头：6D6F6F76

//Windows Media (asf)，文件头：3026B2758E66CF11

//MIDI (mid)，文件头：4D546864
            
            string result = null;
            // buffer[0].ToString(16);
            return "j";
        }
        #endregion
    }
}
