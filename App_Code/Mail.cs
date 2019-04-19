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
    string myEmail = ConfigurationManager.AppSettings["myEmail"]; // "program.prehrane@yahoo.com";
    string myPassword = ConfigurationManager.AppSettings["myPassword"]; // "Tel546360";
    int myServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["myServerPort"]); // 587;
    string myServerHost = ConfigurationManager.AppSettings["myServerHost"]; // "smtp.mail.yahoo.com";

    public Mail() {
    }

    //TODO
    public void SendOrder(Orders.NewOrder x) {

      //  -----------Send mail to me-------- -
        string messageSubject = "Nova narudžba";
        string messageBody = string.Format(
@"
<h3>Nova Narudžba:</h3>
<p>Ime i prezime: {0} {1},</p>
<p>Tvrtka: {2}</p>
<p>Ulica i broj: {3}</p>
<p>Poštanski broj: {4}</p>
<p>Grad: {5}</p>
<p>Država: {6}</p>
<p>OIB: {7}</p>"
, x.orderId
, x.userId
, x.items
, x.netPrice);

        SendMail(myEmail, messageSubject, messageBody);

     //  ------Send mailo to customer------ -
      messageSubject = "Program Prehrane - podaci za uplatu";
        messageBody = string.Format(
@"
<p>Poštovani/a,</p>
<p>Vaša narudžba je uspješno zaprimljena.
<br/>
Broj narudžbe: {0}
<br />
<p>Aplikacija će biti aktivna nakon primitka uplate ili nakon što nam pošaljete potvrdu o uplati.</p> 
<br />
<b>Podaci za uplatu na žiro račun:</b>
<hr/>
....
<p>IBAN:---</p>
<p>Iznos: {0} kn</p>
<hr/>
<br />"
, x.orderId
, x.netPrice);

       // SendMail(email, messageSubject, messageBody);


    }

    public string SendMail(string sendTo, string messageSubject, string messageBody) {

        //web@promo-tekstil.com
        //pass: vMap90#3
        //port 465 ili 25 ili 2525 non secure
        //host: mail.promo-tekstil.com

        //ako nebude onda https://mi3-wss5.a2hosting.com

       /* Za slanje mailova:
        Server: mi3 - wss5.a2hosting.com
Email: web @promo-teksti.com
Username: web @promo-teksti.com
Pass: vMap90#3
Vrsta: SMTP
Security: SSL - TLS
Method: Normal password
Port: 465 */


        try {
            MailMessage mailMessage = new MailMessage();
            
            SmtpClient Smtp_Server = new SmtpClient();
            Smtp_Server.UseDefaultCredentials = false;
            Smtp_Server.Credentials = new NetworkCredential(myEmail, myPassword);
            Smtp_Server.Port = myServerPort;
            Smtp_Server.EnableSsl = true;
            Smtp_Server.Host = myServerHost;
            Smtp_Server.DeliveryMethod = SmtpDeliveryMethod.Network;
           // Smtp_Server.Timeout = 10000;
            mailMessage.To.Add(sendTo);
            mailMessage.From = new MailAddress(myEmail);
            mailMessage.Subject = messageSubject;
            mailMessage.Body = messageBody;
            mailMessage.IsBodyHtml = true;
            Smtp_Server.Send(mailMessage);
            return "OK";
        } catch (Exception e) {
            return e.Message;
        }
    }


    [WebMethod]
    public string Send(string name, string email, string messageSubject, string message) {
       string messageBody = string.Format(
@"
<hr>Novi upit</h3>
<p>Ime: {0}</p>
<p>Email: {1}</p>
<p>Poruka: {2}</p>", name, email, message);
            return SendMail(myEmail, messageSubject, messageBody);
    }

   [WebMethod]
    public string TestMail1() {
        return SendMail("igprog@yahoo.com", "test", "test");
    }

    [WebMethod]
    public string TestMail2() {
        return sendMailA2();
    }



    //TEST
    private string sendMailA2()
    {
        SmtpClient smtpClient = new SmtpClient("mi3-wss5.a2hosting.com", 465);

        smtpClient.Credentials = new NetworkCredential("web@promo-tekstil.com", "vMap90#3");
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        MailMessage mailMessage = new MailMessage("web@promo-tekstil.com", "igprog@yahoo.com");
        mailMessage.Subject = "test";
        mailMessage.Body = "test"; 

        try
        {
            smtpClient.Send(mailMessage);
            return "ok";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


}
