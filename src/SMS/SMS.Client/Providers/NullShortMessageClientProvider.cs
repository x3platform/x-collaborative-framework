namespace X3Platform.SMS.Client.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using Common.Logging;
    using X3Platform.Configuration;
    using X3Platform.SMS.Client.Configuration;

    /// <summary>�յķ�����</summary>
    public class NullShortMessageClientProvider : IShortMessageClientProvider
    {
        /// <summary>���Ͷ���</summary>
        /// <param name="message">����</param>
        public string Send(ShortMessage message)
        {
            return string.Empty;
        }
    }
}
