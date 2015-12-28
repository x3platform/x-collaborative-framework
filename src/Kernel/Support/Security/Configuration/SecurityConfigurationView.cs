namespace X3Platform.Security.Configuration
{
    #region Using Libraries
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    using X3Platform.Util;
    using System;
    #endregion

    /// <summary>安全配置视图</summary>
    public class SecurityConfigurationView : XmlConfigurationView<SecurityConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Security.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = SecurityConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile SecurityConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static SecurityConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SecurityConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:SecurityConfigurationView()
        /// <summary>构造函数</summary>
        private SecurityConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 基类初始化后会默认执行 Reload() 函数
        }
        #endregion

        #region 属性:Reload()
        /// <summary>重新加载配置信息</summary>
        public override void Reload()
        {
            base.Reload();

            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义属性
        // -------------------------------------------------------

        #region 属性:MD5CryptoCiphertextFormat
        private string m_MD5CryptoCiphertextFormat = string.Empty;

        /// <summary>MD5 加密方式的默认密文格式</summary>
        public string MD5CryptoCiphertextFormat
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MD5CryptoCiphertextFormat))
                {
                    // 读取配置信息
                    this.m_MD5CryptoCiphertextFormat = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "MD5CryptoCiphertextFormat", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MD5CryptoCiphertextFormat = StringHelper.NullOrEmptyTo(this.m_MD5CryptoCiphertextFormat, "HexStringWhitoutHyphen");
                }

                return this.m_MD5CryptoCiphertextFormat;
            }
        }
        #endregion

        #region 属性:SHA1CryptoCiphertextFormat
        private string m_SHA1CryptoCiphertextFormat = string.Empty;

        /// <summary>SHA1 加密方式的默认密文格式</summary>
        public string SHA1CryptoCiphertextFormat
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SHA1CryptoCiphertextFormat))
                {
                    // 读取配置信息
                    this.m_SHA1CryptoCiphertextFormat = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SHA1CryptoCiphertextFormat", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_SHA1CryptoCiphertextFormat = StringHelper.NullOrEmptyTo(this.m_SHA1CryptoCiphertextFormat, "HexStringWhitoutHyphen");
                }

                return this.m_SHA1CryptoCiphertextFormat;
            }
        }
        #endregion

        #region 属性:DESCryptoKey
        private string m_DESCryptoKey = string.Empty;

        /// <summary>DES 加密方式的默认密钥 长度为八位内容</summary>
        public string DESCryptoKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DESCryptoKey))
                {
                    // 读取配置信息
                    this.m_DESCryptoKey = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DESCryptoKey", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_DESCryptoKey = StringHelper.NullOrEmptyTo(this.m_DESCryptoKey, "00000000");
                }

                return this.m_DESCryptoKey;
            }
        }
        #endregion

        #region 属性:DESCryptoIV
        private string m_DESCryptoIV = string.Empty;

        /// <summary>DES 加密方式的默认初始化向量</summary>
        public string DESCryptoIV
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DESCryptoIV))
                {
                    // 读取配置信息
                    this.m_DESCryptoIV = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DESCryptoIV", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_DESCryptoIV = StringHelper.NullOrEmptyTo(this.m_DESCryptoIV, "00000000");
                }

                return this.m_DESCryptoIV;
            }
        }
        #endregion

        #region 属性:DESCryptoCiphertextFormat
        private string m_DESCryptoCiphertextFormat = string.Empty;

        /// <summary>DES 加密方式的默认密文格式</summary>
        public string DESCryptoCiphertextFormat
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DESCryptoCiphertextFormat))
                {
                    // 读取配置信息
                    this.m_DESCryptoCiphertextFormat = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DESCryptoCiphertextFormat", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_DESCryptoCiphertextFormat = StringHelper.NullOrEmptyTo(this.m_DESCryptoCiphertextFormat, "HexStringWhitoutHyphen");
                }

                return this.m_DESCryptoCiphertextFormat;
            }
        }
        #endregion

        #region 属性:AESCryptoKey
        private string m_AESCryptoKey = string.Empty;

        /// <summary>AES 加密方式的默认密钥 长度为八位内容</summary>
        public string AESCryptoKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AESCryptoKey))
                {
                    // 读取配置信息
                    this.m_AESCryptoKey = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "AESCryptoKey", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_AESCryptoKey = StringHelper.NullOrEmptyTo(this.m_AESCryptoKey, "00000000");
                }

                return this.m_AESCryptoKey;
            }
        }
        #endregion

        #region 属性:AESCryptoIV
        private string m_AESCryptoIV = string.Empty;

        /// <summary>AES 加密方式的默认初始化向量</summary>
        public string AESCryptoIV
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AESCryptoIV))
                {
                    // 读取配置信息
                    this.m_AESCryptoIV = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "AESCryptoIV", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_AESCryptoIV = StringHelper.NullOrEmptyTo(this.m_AESCryptoIV, "0000000000000000");
                }

                return this.m_AESCryptoIV;
            }
        }
        #endregion

        #region 属性:AESCryptoMode
        private string m_AESCryptoMode = string.Empty;

        /// <summary>AES 加密方式的默认运算模式</summary>
        public string AESCryptoMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AESCryptoMode))
                {
                    // 读取配置信息
                    this.m_AESCryptoMode = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "AESCryptoMode", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_AESCryptoMode = StringHelper.NullOrEmptyTo(this.m_AESCryptoMode, "CBC");
                }

                return this.m_AESCryptoMode;
            }
        }
        #endregion

        #region 属性:AESCryptoPadding
        private string m_AESCryptoPadding = string.Empty;

        /// <summary>AES 加密方式的默认填充模式</summary>
        public string AESCryptoPadding
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AESCryptoPadding))
                {
                    // 读取配置信息
                    this.m_AESCryptoPadding = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "AESCryptoPadding", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_AESCryptoPadding = StringHelper.NullOrEmptyTo(this.m_AESCryptoPadding, "PKCS7");
                }

                return this.m_AESCryptoPadding;
            }
        }
        #endregion

        #region 属性:AESCryptoCiphertextFormat
        private string m_AESCryptoCiphertextFormat = string.Empty;

        /// <summary>AES 加密方式的默认密文格式</summary>
        public string AESCryptoCiphertextFormat
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AESCryptoCiphertextFormat))
                {
                    // 读取配置信息
                    this.m_AESCryptoCiphertextFormat = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "AESCryptoCiphertextFormat", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_AESCryptoCiphertextFormat = StringHelper.NullOrEmptyTo(this.m_AESCryptoCiphertextFormat, "HexStringWhitoutHyphen");
                }

                return this.m_AESCryptoCiphertextFormat;
            }
        }
        #endregion

        #region 属性:CaptchaMode
        private string m_CaptchaMode = string.Empty;

        /// <summary>图形验证码状态</summary>
        public string CaptchaMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_CaptchaMode))
                {
                    // 读取配置信息
                    this.m_CaptchaMode = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CaptchaMode", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_CaptchaMode = StringHelper.NullOrEmptyTo(this.m_CaptchaMode, "Off");

                    this.m_CaptchaMode = this.m_CaptchaMode.ToUpper();
                }

                return this.m_CaptchaMode;
            }
        }
        #endregion

        #region 属性:DefaultRequestLimitMinutes
        private int m_DefaultRequestLimitMinutes = -1;

        /// <summary>默认请求限制的时间段 单位:分钟</summary>
        public int DefaultRequestLimitMinutes
        {
            get
            {
                if (this.m_DefaultRequestLimitMinutes < 0)
                {
                    // 读取配置信息
                    this.m_DefaultRequestLimitMinutes = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DefaultRequestLimitMinutes", this.Configuration.Keys));

                    // 如果配置文件里未设置则设置一个默认值
                    if (this.m_DefaultRequestLimitMinutes < 0)
                    {
                        this.m_DefaultRequestLimitMinutes = 0;
                    }
                }

                return this.m_DefaultRequestLimitMinutes;
            }
        }
        #endregion

        #region 属性:DefaultRequestLimitCount
        private int m_DefaultRequestLimitCount = -1;

        /// <summary>默认请求限制的分钟数内请求的次数</summary>
        public int DefaultRequestLimitCount
        {
            get
            {
                if (this.m_DefaultRequestLimitCount < 0)
                {
                    // 读取配置信息
                    this.m_DefaultRequestLimitCount = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DefaultRequestLimitCount", this.Configuration.Keys));

                    // 如果配置文件里未设置则设置一个默认值
                    if (this.m_DefaultRequestLimitCount < 0)
                    {
                        this.m_DefaultRequestLimitCount = 0;
                    }
                }

                return this.m_DefaultRequestLimitCount;
            }
        }
        #endregion
    }
}
