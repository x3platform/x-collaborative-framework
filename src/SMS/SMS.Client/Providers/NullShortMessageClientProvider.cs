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

    /// <summary>空的发送器</summary>
    public class NullShortMessageClientProvider : IShortMessageClientProvider
    {
        /// <summary>发送短信</summary>
        /// <param name="message">短信</param>
        public string Send(ShortMessage message)
        {
            return string.Empty;
        }
    }
}
