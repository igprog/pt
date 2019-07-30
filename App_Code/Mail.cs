using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text;

/// <summary>
/// SendMail
/// </summary>
[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Mail : System.Web.Services.WebService {
    string myEmail = ConfigurationManager.AppSettings["myEmail"];
    string myEmailName = ConfigurationManager.AppSettings["myEmailName"];
    string myPassword = ConfigurationManager.AppSettings["myPassword"];
    int myServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["myServerPort"]);
    string myServerHost = ConfigurationManager.AppSettings["myServerHost"];

    public Mail() {
    }

    [WebMethod]
    public string SendMail(string sendTo, string subject, string body, string[] cc, string file) {
        try {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(myEmail, myEmailName);
            mail.To.Add(sendTo);
            if (cc.Length > 0) {
                foreach (string c in cc) {
                    mail.CC.Add(c);
                }
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(file)) {
                Attachment attachment = new Attachment(Server.MapPath(file.Replace("../", "~/")));
                mail.Attachments.Add(attachment);
            }
            SmtpClient smtp = new SmtpClient(myServerHost, myServerPort);
            NetworkCredential Credentials = new NetworkCredential(myEmail, myPassword);
            smtp.Credentials = Credentials;
            smtp.Send(mail);
            return "message sent successfully";
        } catch (Exception e) {
            return e.Message;
        }
    }
}
