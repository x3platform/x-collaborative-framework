namespace X3Platform.Email.Client
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Net;
  using System.Net.Mail;

  using X3Platform.Logging;
  using X3Platform.Messages;

  public class SmtpClientTester
  {
    private static SmtpClient smtp = null;

    public static MessageObject Test(string host, string port, string enableSsl, string username, string password, string fromEmailAddress, string toEmailAddress)
    {
      MessageObject message = new MessageObject();

      try
      {
        smtp = new SmtpClient(host);

        if (!string.IsNullOrEmpty(username))
        {
          smtp.UseDefaultCredentials = false;

          smtp.Credentials = new NetworkCredential(username, password);
        }

        smtp.Port = Convert.ToInt32(port);

        smtp.Port = smtp.Port == 0 ? 25 : smtp.Port;

        if (enableSsl.ToUpper() == "ON")
          smtp.EnableSsl = true;

        // LoggingContext.Instance.Write(string.Format("Host:{0},Port:{1},EnableSsl:{2},UseDefaultCredentials:{3}", smtp.Host, smtp.Port, smtp.EnableSsl, smtp.UseDefaultCredentials));

        smtp.Send(fromEmailAddress, toEmailAddress,
            string.Format("��ϲ, һ������{0}�Ĳ����ʼ�. ʱ��:{1}", fromEmailAddress, DateTime.Now),
            string.Format("��ϲ, ����һ������{0}�Ĳ����ʼ�. ������-{1}. ����ɾ��. ʱ��:{2}", fromEmailAddress, Environment.MachineName, DateTime.Now));

        message.Set("0", "�����ʼ����ͳɹ���");
      }
      catch (Exception ex)
      {
        message.Set("1", ex.ToString());
      }

      return message;
    }
  }
}
