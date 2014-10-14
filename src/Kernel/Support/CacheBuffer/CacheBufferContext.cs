// =============================================================================
//
// Copyright (c) ruany@live.com
//
// FileName     :CacheBufferContext.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;

using X3Platform.Logging;
using X3Platform.Spring;

using X3Platform.CacheBuffer.Configuration;

namespace X3Platform.CacheBuffer
{
    /// <summary></summary>
    public sealed class CacheBufferContext : IContext
    {
        #region ��̬����:Instance
        private static volatile CacheBufferContext instance = null;

        private static object lockObject = new object();

        /// <summary>
        /// ʵ��
        /// </summary>
        public static CacheBufferContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new CacheBufferContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ����:Name
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return "����"; }
        }
        #endregion

        #region ����:Provider
        private ICacheBufferProvider cacheBufferProvider;

        ///// <summary>�����ṩ��</summary>
        //public ICacheBufferProvider Provider
        //{
        //    get { return provider; }
        //}
        #endregion

        #region ����:Configuration
        private CacheBufferConfiguration configuration = null;

        /// <summary>����</summary>
        public CacheBufferConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ���캯��:CacheBufferContext()
        private CacheBufferContext()
        {
            Reload();
        }
        #endregion

        #region ����:Reload()
        /// <summary>
        /// ���¼���
        /// </summary>
        public void Reload()
        {
            this.configuration = CacheBufferConfigurationView.Instance.Configuration;

            this.cacheBufferProvider = SpringContext.Instance.GetObject<ICacheBufferProvider>(typeof(ICacheBufferProvider));
        }
        #endregion

        #region ����:Contains(string key)
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return this.cacheBufferProvider.Contains(key);
        }
        #endregion

        #region ����:Write(string key, object value, int minutes)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Read(string key)
        {
            return this.cacheBufferProvider.Read(key);
        }
        #endregion

        #region ����:Write(string key, object value)
        /// <summary>д�뻺����</summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        public void Write(string key, object value)
        {
            this.cacheBufferProvider.Write(key, value);
        }
        #endregion

        #region ����:Write(string key, object value, int minutes)
        /// <summary>д�뻺����</summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="minutes">��Ч����</param>
        public void Write(string key, object value, int minutes)
        {
            this.cacheBufferProvider.Write(key, value, minutes);
        }
        #endregion

        #region ����:Delete(string key)
        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="key">��</param>
        public void Delete(string key)
        {
            this.cacheBufferProvider.Delete(key);
        }
        #endregion
    }
}