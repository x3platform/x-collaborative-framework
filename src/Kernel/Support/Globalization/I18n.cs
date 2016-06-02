namespace X3Platform.Globalization
{
    using Common.Logging;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using X3Platform.Configuration;

    /// <summary>本地化设置对象</summary>
    public class I18n
    {
        static Dictionary<string, object> lockObjects = new Dictionary<string, object>() {
            { "translates", new object() },
            { "strings", new object() },
            { "menu", new object() },
            { "exceptions", new object() }
        };

        #region 属性:Translates
        private static volatile Localization values = null;

        /// <summary>本地化的翻译信息</summary>
        public static Localization Translates
        {
            get
            {
                if (values == null)
                {
                    lock (lockObjects["translates"])
                    {
                        if (values == null)
                        {
                            values = new Localization("translates.xml", "translate");
                        }
                    }
                }

                return values;
            }
        }
        #endregion

        #region 属性:Strings
        private static volatile Localization strings = null;

        /// <summary>本地化的文本信息 系统提示信息 警告信息 错误信息</summary>
        public static Localization Strings
        {
            get
            {
                if (strings == null)
                {
                    lock (lockObjects["strings"])
                    {
                        if (strings == null)
                        {
                            strings = new Localization("strings.xml", "string");
                        }
                    }
                }

                return strings;
            }
        }
        #endregion

        #region 属性:Menu
        private static volatile Localization menu = null;

        /// <summary>本地化的菜单信息</summary>
        public static Localization Menu
        {
            get
            {
                if (menu == null)
                {
                    lock (lockObjects["menu"])
                    {
                        if (menu == null)
                        {
                            menu = new Localization("menu.xml", "menu", false);
                        }
                    }
                }

                return menu;
            }
        }
        #endregion

        #region 属性:Exceptions
        private static volatile Localization m_Exceptions = null;

        /// <summary>本地化的异常信息</summary>
        public static Localization Exceptions
        {
            get
            {
                if (m_Exceptions == null)
                {
                    lock (lockObjects["exceptions"])
                    {
                        if (m_Exceptions == null)
                        {
                            m_Exceptions = new Localization("exceptions.xml", "exception");
                        }
                    }
                }

                return m_Exceptions;
            }
        }
        #endregion

        private I18n() { }
    }
}
