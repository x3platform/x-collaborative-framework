#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    
    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    using X3Platform.Configuration;
    using X3Platform.Membership;
    using X3Platform.Security.Authentication;
    #endregion

    /// <summary>���Ļ���</summary>
    public sealed class KernelContext : IContext
    {
        #region 属性:Name
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return "���Ļ���"; }
        }
        #endregion

        #region 属性:Current
        private static volatile KernelContext instance = null;

        private static object lockObject = new object();

        /// <summary>
        /// ��ǰ��Ϣ
        /// </summary>
        public static KernelContext Current
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new KernelContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:User
        private IDictionary<string, IAccountInfo> cacheStorage = new SyncDictionary<string, IAccountInfo>();

        /// <summary>�û���Ϣ</summary>
        public IAccountInfo User
        {
            get
            {
                string identity = AuthenticationManagement.GetIdentityValue();

                if (string.IsNullOrEmpty(identity))
                {
                    return null;
                }
                else
                {
                    if (!cacheStorage.ContainsKey(identity))
                    {
                        IAccountInfo account = AuthenticationManagement.GetAuthUser();

                        if (account == null)
                        {
                            cacheStorage.Remove(identity);
                            return null;
                        }
                        else
                        {
                            cacheStorage.Add(identity, account);
                        }
                    }

                    return cacheStorage[identity];
                }
            }

            set
            {
                string identity = authenticationManagement.GetIdentityValue();

                if (!string.IsNullOrEmpty(identity) && value != null)
                {
                    cacheStorage[identity] = value;
                }
            }
        }
        #endregion

        #region 属性:Configuration
        private KernelConfiguration configuration = null;

        /// <summary>������Ϣ</summary>
        public KernelConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:AuthenticationManagement
        private IAuthenticationManagement authenticationManagement = null;

        /// <summary>��֤����</summary>
        public IAuthenticationManagement AuthenticationManagement
        {
            get { return authenticationManagement; }
        }
        #endregion

        #region ���캯��:KernelContext()
        private KernelContext()
        {
            this.Reload();
        }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        public void Reload()
        {
            this.configuration = KernelConfigurationView.Instance.Configuration;

            string authenticationManagementType = KernelConfigurationView.Instance.AuthenticationManagementType;

            this.authenticationManagement = (IAuthenticationManagement)CreateObject(authenticationManagementType);
        }
        #endregion

        // -------------------------------------------------------
        // ���ߺ���
        // -------------------------------------------------------

        #region 属性:CreateObject(string type)
        /// <summary>��������</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateObject(string type)
        {
            return CreateObject(type, null);
        }
        #endregion

        #region 属性:CreateObject(string type, object[] args)
        /// <summary>��������</summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateObject(string type, params object[] args)
        {
            if (string.IsNullOrEmpty(type)) { return null; }

            Type objectType = Type.GetType(type);

            if (objectType == null) { return null; }

            return Activator.CreateInstance(objectType, args);
        }
        #endregion

        #region 属性:ParseObjectType(Type type)
        /// <summary>������������</summary>
        /// <param name="type"></param>
        /// <returns>��ʽ:X3Platform.KernelContext,X3Platform</returns>
        public static string ParseObjectType(Type type)
        {
            if (type == null)
                return null;

            string assemblyQualifiedName = type.AssemblyQualifiedName;

            int length = assemblyQualifiedName.IndexOf(',', assemblyQualifiedName.IndexOf(',') + 1);

            return assemblyQualifiedName.Substring(0, length);
        }
        #endregion
    }
}
