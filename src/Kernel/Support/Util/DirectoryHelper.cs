// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :DirectoryHelper.cs
//
// Description  :Ŀ¼������
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
    /// <summary>Ŀ¼������</summary>
    public class DirectoryHelper
    {
        #region ����:Create(string path)
        public static void Create(string path)
        {
            // ����Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�������������������֮
            if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                path += Path.DirectorySeparatorChar;

            // �ж�Ŀ��Ŀ¼�Ƿ������������������½�֮
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion

        #region ����:Copy(string fromPath, string toPath)
        public static void Copy(string fromPath, string toPath)
        {
            //
            // ʵ��һ����̬������ָ���ļ�����������������copy��Ŀ���ļ�������
            // ����Ŀ���ļ���Ϊֻ�����Ծͻᱨ��
            //

            // ����Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�������������������֮
            if (toPath[toPath.Length - 1] != Path.DirectorySeparatorChar)
                toPath += Path.DirectorySeparatorChar;

            // �ж�Ŀ��Ŀ¼�Ƿ������������������½�֮
            if (!Directory.Exists(toPath)) Directory.CreateDirectory(toPath);

            // �õ�ԴĿ¼���ļ��б��������ǰ����ļ��Լ�Ŀ¼·����һ������
            // ������ָ��copyĿ���ļ��������ļ���������Ŀ¼��ʹ�������ķ���
            // string[] fileList = System.IO.Directory.GetFiles(fromPath);

            string[] fileList = Directory.GetFileSystemEntries(fromPath);

            // �������е��ļ���Ŀ¼

            foreach (string file in fileList)
            {
                // �ȵ���Ŀ¼����������������Ŀ¼�͵ݹ�Copy��Ŀ¼�������ļ�
                if (Directory.Exists(file))
                {
                    Copy(file, toPath + Path.GetFileName(file));
                }
                else
                {
                    // ����ֱ��Copy�ļ�

                    File.Copy(file, toPath + Path.GetFileName(file), true);
                }
            }
        }
        #endregion

        #region ����:Delete(string path)
        public static void Delete(string path)
        {
            /* 
             * ʵ��һ����̬������ָ���ļ�����������������ɾ��.
             * 
             * ���Ե�ʱ��ҪС�Ĳ�����ɾ��֮���޷��ָ���
             * 
             */

            // ����Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�������������������֮

            if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                path += Path.DirectorySeparatorChar;

            // �õ�ԴĿ¼���ļ��б��������ǰ����ļ��Լ�Ŀ¼·����һ������
            // ������ָ��DeleteĿ���ļ��������ļ���������Ŀ¼��ʹ�������ķ���
            // string[] fileList = System.IO.Directory.GetFiles(toPath);

            string[] fileList = Directory.GetFileSystemEntries(path);

            // �������е��ļ���Ŀ¼

            foreach (string file in fileList)
            {
                // �ȵ���Ŀ¼����������������Ŀ¼�͵ݹ�Delete��Ŀ¼�������ļ�

                if (Directory.Exists(file))
                {
                    Delete(path + Path.GetFileName(file));
                }
                else
                {
                    // ����ֱ��Delete�ļ�

                    File.Delete(path + Path.GetFileName(file));
                }
            }

            //ɾ���ļ���

            Directory.Delete(path, true);
        }
        #endregion

        #region ����:FormatLocalPath(string path)
        /// <summary>��ʽ�����ؾ���·��</summary>
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

        #region ����:FormatTimePath()
        /// <summary></summary>
        /// <returns></returns>
        public static string FormatTimePath()
        {
            return FormatTimePath(DateTime.Now);
        }
        #endregion

        #region ����:FormatTimePath(DateTime datetime)
        /// <summary></summary>
        /// <returns></returns>
        public static string FormatTimePath(DateTime datetime)
        {
            return datetime.Year + "/" + (((datetime.Month - 1) / 3) + 1) + "Q/" + datetime.Month + "/";
        }
        #endregion
    }
}
