
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Autobuses.Helpers
{
    public class Mail
    {
        public static int Send(string mailUsuario, string asunto, string msg)
        {
            int resp = 0;

            try
            {
                string correo = ConfigurationManager.AppSettings["correo"];
                string key = ConfigurationManager.AppSettings["key"];
                string server = ConfigurationManager.AppSettings["server"];
                int port = int.Parse(ConfigurationManager.AppSettings["port"]);

                MailMessage message = new MailMessage();
                message.Subject = asunto;
                message.IsBodyHtml = true;
                message.Body = $"<h3>{asunto}</h3><p>{msg}</p>";
                message.From = new MailAddress(correo);
                message.To.Add(new MailAddress(mailUsuario));

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = server;
                smtpClient.EnableSsl = true;
                smtpClient.Port = port;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(correo, key);

                smtpClient.Send(message);
                resp = 1;
            }
            catch (Exception ex)
            {
                resp = 0;
            }

            return resp;
        }
    }
}