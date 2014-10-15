using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Location.IPQuery
{
    /// <summary>
    /// IP地址分析器
    /// </summary>
    public abstract class IPAddressParser
    {
        #region 函数:Parse(byte[] IPUnit, out int depth)
        /// <summary>分析 IP地址段</summary>
        /// <param name="IPUnit">IP地址的元素</param>
        /// <param name="depth">深度 [1-4]</param>
        /// <returns>IP地址段</returns>
        public abstract IList<IPAddressQueryInfo> Parse(byte[] IPUnit, out int depth);
        #endregion
    }
}
