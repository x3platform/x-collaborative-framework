namespace X3Platform.DigitalNumber
{
    #region Using Libraries
    using System;

    using X3Platform.Data;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.IBLL;
    #endregion
    
    /// <summary>流水号上下文环境</summary>
    public class DigitalNumberContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "流水号"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile DigitalNumberContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
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

        /// <summary>流水号服务提供者</summary>
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

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
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

        /// <summary>生成通用的流水编号</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Generate(string name)
        {
            return Instance.DigitalNumberService.Generate(name);
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateCodeByPrefixCode(string entityTableName, string prefixCode)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{code}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{code:" + incrementCodeLength + "}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{code}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{code:" + incrementCodeLength + "}");
        }

        /// <summary>根据自定义的编号前缀生成日期流水编号</summary>
        public static string GenerateDateCodeByPrefixCode(string entityTableName, string prefixCode)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{date}{code}");
        }

        /// <summary>根据自定义的编号前缀生成日期流水编号</summary>
        public static string GenerateDateCodeByPrefixCode(string entityTableName, string prefixCode, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(entityTableName, prefixCode, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }

        /// <summary>根据自定义的编号前缀生成日期流水编号</summary>
        public static string GenerateDateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, "{prefix}{date}{code}");
        }

        /// <summary>根据自定义的编号前缀生成日期流水编号</summary>
        public static string GenerateDateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateDateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateDateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateDateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code}");
        }

        /// <summary>根据自定义的编号前缀生成的流水编号</summary>
        public static string GenerateDateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, int incrementCodeLength)
        {
            return Instance.DigitalNumberService.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, "{prefix}{date}{code:" + incrementCodeLength + "}");
        }
    }
}
