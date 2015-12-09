namespace X3Platform.SMS.Client
{
    using System.Net.Mail;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.SMS.Client.IShortMessageClientProvider")]
    public interface IShortMessageClientProvider
    {
        #region ����:Send(ShortMessage message)
        /// <summary>���Ͷ���</summary>
        /// <param name="message">����</param>
        string Send(ShortMessage message);
        #endregion
    }
}
