namespace X3Platform.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>本地化信息</summary>
    public class Localization
    {
        /// <summary>默认本土化缓存信息</summary>
        Localizer defaultLocalizer = null;

        /// <summary>本土化缓存信息</summary>
        private Dictionary<string, Localizer> dictionary = null;

        private string fileName;
        private string nodeName;

        /// <summary></summary>
        /// <param name="fileName"></param>
        /// <param name="nodeName"></param>
        public Localization(string fileName, string nodeName)
        {
            // 设置文件名称和节点名称
            this.fileName = fileName;
            this.nodeName = nodeName;

            // 初始化默认翻译
            this.defaultLocalizer = new Localizer(KernelConfigurationView.Instance.ApplicationPathRoot + "locales/" + KernelConfigurationView.Instance.CultureName + "/" + fileName, nodeName);

            // 初始化默认翻译
            dictionary = new Dictionary<string, Localizer>();
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                return this[name, StringCase.Default];
            }
        }
        
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="stringCase"></param>
        /// <returns></returns>
        public string this[string name, StringCase stringCase]
        {
            get
            {
                string text = this.GetLocalizer().GetText(name);

                text = text == null ? this.defaultLocalizer.GetText(name) : text;

                return ToStringCase(text, stringCase);
            }
        }

        /// <summary></summary>
        /// <param name="applicationName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string applicationName, string name]
        {
            get
            {
                return this[applicationName, name, StringCase.Default];
            }
        }

        /// <summary></summary>
        /// <param name="applicationName"></param>
        /// <param name="name"></param>
        /// <param name="stringCase"></param>
        /// <returns></returns>
        public string this[string applicationName, string name, StringCase stringCase]
        {
            get
            {
                string text = this.GetLocalizer().GetText(applicationName, name);

                text = text == null ? this.defaultLocalizer.GetText(applicationName, name) : text;

                return ToStringCase(text, stringCase);
            }
        }

        /// <summary>获取本土化信息</summary>
        /// <returns></returns>
        private Localizer GetLocalizer()
        {
            Localizer localizer = null;

            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            string languageFolder = KernelConfigurationView.Instance.ApplicationPathRoot + "locales/" + culture.Name + "/" + fileName;

            if (dictionary.ContainsKey(culture.Name))
            {
                localizer = dictionary[culture.Name];
            }
            else
            {
                if (File.Exists(languageFolder))
                {
                    localizer = new Localizer(languageFolder, nodeName);

                    if (!dictionary.ContainsKey(culture.Name))
                    {
                        dictionary.Add(culture.Name, localizer);
                    }
                }
            }

            return localizer == null ? this.defaultLocalizer : localizer;
        }

        private string ToStringCase(string text, StringCase stringCase)
        {
            switch (stringCase)
            {
                case StringCase.Lower:
                    return text.ToLower();
                case StringCase.FirstLower:
                    return StringHelper.ToFirstLower(text);
                case StringCase.Upper:
                    return text.ToUpper();
                case StringCase.FirstUpper:
                    return StringHelper.ToFirstUpper(text);
                case StringCase.Title:
                    return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
                default:
                    return text;
            }
        }
    }
}
