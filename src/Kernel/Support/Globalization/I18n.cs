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

    /// <summary>本地化设置对象</summary>
    public class I18n
    {
        #region 属性:Translates
        private static volatile Localization values = null;

        private static object lockTranslatesObject = new object();

        /// <summary>本地化的翻译信息</summary>
        public static Localization Translates
        {
            get
            {
                if (values == null)
                {
                    lock (lockTranslatesObject)
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

        private static object lockStringsObject = new object();

        /// <summary>本地化的文本信息 系统提示信息 警告信息 错误信息</summary>
        public static Localization Strings
        {
            get
            {
                if (strings == null)
                {
                    lock (lockStringsObject)
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

        private static object lockMenuObject = new object();

        /// <summary>本地化的菜单信息</summary>
        public static Localization Menu
        {
            get
            {
                if (menu == null)
                {
                    lock (lockMenuObject)
                    {
                        if (menu == null)
                        {
                            menu = new Localization("menu.xml", "menu");
                        }
                    }
                }

                return menu;
            }
        }
        #endregion

        private I18n() { }
    }
}
