namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.IO;
    #endregion

    /// <summary>目录处理辅助类</summary>
    public class DirectoryHelper
    {
        #region 函数:Create(string path)
        /// <summary>创建目录</summary>
        /// <param name="path"></param>
        public static void Create(string path)
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                path += Path.DirectorySeparatorChar;

            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion

        #region 函数:Copy(string fromPath, string toPath)
        public static void Copy(string fromPath, string toPath)
        {
            //
            // 实现一个静态方法将指定文件夹下面的所有内容copy到目标文件夹下面
            // 如果目标文件夹为只读属性就会报错。
            //

            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (toPath[toPath.Length - 1] != Path.DirectorySeparatorChar)
                toPath += Path.DirectorySeparatorChar;

            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(toPath)) Directory.CreateDirectory(toPath);

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = System.IO.Directory.GetFiles(fromPath);

            string[] fileList = Directory.GetFileSystemEntries(fromPath);

            // 遍历所有的文件和目录

            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (Directory.Exists(file))
                {
                    Copy(file, toPath + Path.GetFileName(file));
                }
                else
                {
                    // 否则直接Copy文件

                    File.Copy(file, toPath + Path.GetFileName(file), true);
                }
            }
        }
        #endregion

        #region 函数:Delete(string path)
        public static void Delete(string path)
        {
            /* 
             * 实现一个静态方法将指定文件夹下面的所有内容删除.
             * 
             * 测试的时候要小心操作，删除之后无法恢复。
             * 
             */

            // 检查目标目录是否以目录分割字符结束如果不是则添加之

            if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                path += Path.DirectorySeparatorChar;

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = System.IO.Directory.GetFiles(toPath);

            string[] fileList = Directory.GetFileSystemEntries(path);

            // 遍历所有的文件和目录

            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件

                if (Directory.Exists(file))
                {
                    Delete(path + Path.GetFileName(file));
                }
                else
                {
                    // 否则直接Delete文件

                    File.Delete(path + Path.GetFileName(file));
                }
            }

            //删除文件夹

            Directory.Delete(path, true);
        }
        #endregion

        #region 函数:FormatLocalPath(string path)
        /// <summary>格式化本地路径</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatLocalPath(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                return path.Replace("\\", "/");
            }
            else
            {
                return path.Replace("/", "\\");
            }
        }
        #endregion

        #region 函数:FormatTimePath()
        /// <summary></summary>
        /// <returns></returns>
        public static string FormatTimePath()
        {
            return FormatTimePath(DateTime.Now);
        }
        #endregion

        #region 函数:FormatTimePath(DateTime datetime)
        /// <summary></summary>
        /// <returns></returns>
        public static string FormatTimePath(DateTime datetime)
        {
            return datetime.Year + "/" + (((datetime.Month - 1) / 3) + 1) + "Q/" + datetime.Month + "/";
        }
        #endregion
    }
}
