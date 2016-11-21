using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Helper
{
    public class Mailer
    {
        public static string FROM_EMAIL { get { return ConfigurationManager.AppSettings["FROM_EMAIL"]; } set { ConfigurationManager.AppSettings["FROM_EMAIL"] = value; } }
        public static string FROM_NAME { get { return ConfigurationManager.AppSettings["FROM_NAME"]; } set { ConfigurationManager.AppSettings["FROM_NAME"] = value; } }
        public static string FROM_PASSWORD { get { return ConfigurationManager.AppSettings["FROM_PASSWORD"]; } set { ConfigurationManager.AppSettings["FROM_PASSWORD"] = value; } }
        public static string MAIL_HOST { get { return ConfigurationManager.AppSettings["MAIL_HOST"]; } set { ConfigurationManager.AppSettings["MAIL_HOST"] = value; } }
        public static int MAIL_PORT { get { return int.Parse(ConfigurationManager.AppSettings["MAIL_PORT"]); } set { ConfigurationManager.AppSettings["MAIL_PORT"] = value.ToString(); } }
        public static bool ENABLE_SSL { get { return bool.Parse(ConfigurationManager.AppSettings["MAIL_ENABLE_SSL"]); } set { ConfigurationManager.AppSettings["MAIL_ENABLE_SSL"] = value.ToString(); } }
        public const int TIMEOUT = 20000;

        public static bool sendText(string ToName, string ToEmail, string subject, string content) {
            return sendText(ToName, ToEmail, FROM_NAME, FROM_EMAIL, subject, content);
        }

        public static bool sendText(string ToName, string ToEmail, string FromName, string FromEmail, string Subject, string Content)
        {
            MailAddress from = new MailAddress(FromEmail, FromName);
            MailAddress to = new MailAddress(ToEmail, ToName);

            using (
                var smtp = new SmtpClient
                {
                    Host = MAIL_HOST,
                    Port = MAIL_PORT,
                    EnableSsl = ENABLE_SSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(FROM_EMAIL, FROM_PASSWORD),
                    Timeout = TIMEOUT
                }
            )
            {


                MailMessage message = new MailMessage()
                {
                    From = from,
                    Subject = Subject,
                    IsBodyHtml = true,
                    Body = Content
                };

                message.To.Add(to);
                try
                {
                    smtp.Send(message);
                    var appBAse = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    var path = appBAse + @"\log\email.txt";

                    System.IO.File.AppendAllText(path, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                    System.IO.File.AppendAllText(path, "Email Pengiraim " + FROM_EMAIL + Environment.NewLine);
                    System.IO.File.AppendAllText(path, "Email Penerima " + MAIL_HOST + Environment.NewLine);
                    System.IO.File.AppendAllText(path, "Sukses Terkirim" + Environment.NewLine);

                    return true;
                }
                catch (Exception exc)
                {
                    //lo tanggal ,
                    var appBAse = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    var path = appBAse + @"\log\email.txt";
                    System.IO.File.AppendAllText(path, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                    System.IO.File.AppendAllText(path, "Email Pengiraim " + FROM_EMAIL + Environment.NewLine);
                    System.IO.File.AppendAllText(path, "Email Penerima " + MAIL_HOST + Environment.NewLine);
                    System.IO.File.AppendAllText(path, exc.Message + Environment.NewLine);


                    return false;
                }
            }
        }
    }
}
