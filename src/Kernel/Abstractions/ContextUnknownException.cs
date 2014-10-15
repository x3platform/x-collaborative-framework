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

using System;
using System.Text;

using X3Platform.Util;

namespace X3Platform
{
    /// <summary>���л�����δ֪�쳣</summary>
    [Serializable]
    public class ContextUnknownException : Exception
    {
        /// <summary>������δ֪���쳣</summary>
        private string key = "contextUnknownException";

        /// <summary>���صĴ���</summary>
        private int returnCode = 1;

        #region ���캯��:ContextUnknownException(string message)
        public ContextUnknownException(string message)
            : base(message)
        {
        }
        #endregion

        #region ���캯��:ContextUnknownException(string key, int returnCode, Exception innerException)
        public ContextUnknownException(string key, int returnCode, Exception innerException)
            : base(innerException.Message, innerException)
        {
            this.key = key;
            this.returnCode = returnCode;
        }
        #endregion

        #region 属性:ToString()
        /// <summary>ת��Ϊ�ַ���</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{\"" + key + "\":{");
            outString.AppendFormat("\"returnCode\":\"{0}\",", this.returnCode);
            outString.AppendFormat("\"category\":\"{0}\",", "exception");
            outString.AppendFormat("\"value\":\"{0}\",", StringHelper.ToSafeJson(this.InnerException.Message));
            outString.AppendFormat("\"description\":\"{0}\"", StringHelper.ToSafeJson(this.InnerException.ToString()));
            outString.Append("}}");

            return outString.ToString();
        }
        #endregion
    }
}