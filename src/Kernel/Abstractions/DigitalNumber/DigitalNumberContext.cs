// =============================================================================
//
// Copyright (c) ruanyu@live.com
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

namespace X3Platform.DigitalNumber
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.IBLL;
    using X3Platform.Data;

    /// <summary>��ˮ�������Ļ���</summary>
    public class DigitalNumberContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "��Ŀ�б�"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile DigitalNumberContext instance = null;

        private static object lockObject = new object();

        public static DigitalNumberContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new DigitalNumberContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:DigitalNumberService
        private IDigitalNumberService m_DigitalNumberService = null;

        /// <summary>���±��ṩ��</summary>
        public IDigitalNumberService DigitalNumberService
        {
            get { return m_DigitalNumberService; }
        }
        #endregion

        DigitalNumberConfiguration configuration = null;

        private DigitalNumberContext()
        {
            Restart();
        }

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                Reload();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }
        #endregion

        private void Reload()
        {
            configuration = DigitalNumberConfigurationView.Instance.Configuration;

            this.m_DigitalNumberService = SpringContext.Instance.GetObject<IDigitalNumberService>(typeof(IDigitalNumberService));
        }

        /// <summary>����ͨ�õ���ˮ����</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Generate(string name)
        {
            return Instance.DigitalNumberService.Generate(name);
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateCodeByPrefixCode(string entityTableName, string prefixCode)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{code}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{code:" + incrementCodeLength + "}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{code}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{code:" + incrementCodeLength + "}");
        }

        /// <summary>�����Զ����ı���ǰ׺����������ˮ����</summary>
        public static string GenerateDateCodeByPrefixCode(string entityTableName, string prefixCode)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{date}{code}");
        }

        /// <summary>�����Զ����ı���ǰ׺����������ˮ����</summary>
        public static string GenerateDateCodeByPrefixCode(string entityTableName, string prefixCode, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }

        /// <summary>�����Զ����ı���ǰ׺����������ˮ����</summary>
        public static string GenerateDateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, "{prefix}{date}{code}");
        }

        /// <summary>�����Զ����ı���ǰ׺����������ˮ����</summary>
        public static string GenerateDateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateDateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateDateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateDateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code}");
        }

        /// <summary>�����Զ����ı���ǰ׺���ɵ���ˮ����</summary>
        public static string GenerateDateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }
    }
}
