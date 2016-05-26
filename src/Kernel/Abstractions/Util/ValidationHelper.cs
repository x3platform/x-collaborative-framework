namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using X3Platform.Globalization;
    #endregion

    /// <summary>验证辅助类</summary>
    public static class ValidationHelper
    {
        /// <summary>文本为空</summary>
        /// <param name="text"></param>
        public static void IsNull(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new GenericException(I18n.Exceptions["code_kernel_text_is_null"],
                    I18n.Exceptions["text_kernel_text_is_null"]);
            }
        }

        /// <summary>对象为空</summary>
        /// <param name="objName">对象名称</param>
        /// <param name="obj">对象信息</param>
        public static void IsNull(string objName, object obj)
        {
            if (obj == null)
            {
                throw new GenericException(I18n.Exceptions["code_kernel_object_is_null"],
                   string.Format(I18n.Exceptions["text_kernel_object_is_null"], objName));
            }
        }

        /// <summary>对象属性为空</summary>
        /// <param name="func"></param>
        public static void IsNull<T>(string propertyName, T obj, Func<T, string> func)
        {
            IsNull<T>(obj, func, I18n.Exceptions["code_kernel_property_is_null"],
                string.Format(I18n.Exceptions["text_kernel_property_is_null"], propertyName));
        }

        /// <summary>对象属性为空</summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        public static void IsNull<T>(T obj, Func<T, string> func, string message)
        {
            IsNull<T>(obj, func, I18n.Exceptions["code_kernel_property_is_null"], message);
        }

        /// <summary>对象属性为空</summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public static void IsNull<T>(T obj, Func<T, string> func, string returnCode, string message)
        {
            string property = func(obj);

            if (string.IsNullOrEmpty(property))
            {
                throw new GenericException(returnCode, message);
            }
        }

        /// <summary>对象必填</summary>
        /// <param name="objName">对象名称</param>
        /// <param name="obj">对象信息</param>
        public static void Require(string objName, object obj)
        {
            if (obj == null)
            {
                throw new GenericException(I18n.Exceptions["code_kernel_object_is_required"],
                   string.Format(I18n.Exceptions["text_kernel_object_is_required"], objName));
            }
        }

        /// <summary>必填</summary>
        /// <param name="func"></param>
        public static void Require<T>(string propertyName, T obj, Func<T, string> func)
        {
            Require(obj, func, I18n.Exceptions["code_kernel_property_is_required"],
                string.Format(I18n.Exceptions["text_kernel_property_is_required"], propertyName));
        }

        /// <summary>必填</summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public static void Require<T>(T obj, Func<T, string> func, string message)
        {
            Require<T>(obj, func, I18n.Exceptions["code_kernel_property_is_required"], message);
        }

        /// <summary>必填</summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public static void Require<T>(T obj, Func<T, string> func, string returnCode, string message)
        {
            string property = func(obj);

            if (string.IsNullOrEmpty(property))
            {
                throw new GenericException(returnCode, message);
            }
        }

        /// <summary>必填的参数</summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public static void RequiredParam(string paramName, object paramValue)
        {
            if (paramValue == null)
            {
                throw new GenericException(I18n.Exceptions["code_kernel_param_is_required"],
                   string.Format(I18n.Exceptions["text_kernel_param_is_required"], paramName));
            }

            if (paramValue is String && string.IsNullOrEmpty(paramValue.ToString()))
            {
                throw new GenericException(I18n.Exceptions["code_kernel_param_is_required"],
                   string.Format(I18n.Exceptions["text_kernel_param_is_required"], paramName));
            }
        }
    }
}
