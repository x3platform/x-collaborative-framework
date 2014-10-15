using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Location.IPQuery
{
    /// <summary>
    /// IP��ַ������
    /// </summary>
    public abstract class IPAddressParser
    {
        #region ����:Parse(byte[] IPUnit, out int depth)
        /// <summary>���� IP��ַ��</summary>
        /// <param name="IPUnit">IP��ַ��Ԫ��</param>
        /// <param name="depth">��� [1-4]</param>
        /// <returns>IP��ַ��</returns>
        public abstract IList<IPAddressQueryInfo> Parse(byte[] IPUnit, out int depth);
        #endregion
    }
}
