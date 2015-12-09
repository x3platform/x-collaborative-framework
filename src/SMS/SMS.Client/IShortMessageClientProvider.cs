namespace X3Platform.SMS.Client
{
    using System.Net.Mail;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.SMS.Client.IShortMessageClientProvider")]
    public interface IShortMessageClientProvider
    {
        #region 函数:Send(ShortMessage message)
        /// <summary>发送短信</summary>
        /// <param name="message">短信</param>
        string Send(ShortMessage message);
        #endregion
    }
}
