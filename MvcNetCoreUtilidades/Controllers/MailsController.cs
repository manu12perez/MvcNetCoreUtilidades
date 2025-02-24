using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace MvcNetCoreUtilidades.Controllers
{
    public class MailsController : Controller
    {
        //NECESITAMOS EL FICHERO DE CONFIGURACION
        private IConfiguration configuration;

        public MailsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMailAsync(string to, string asunto, string mensaje)
        {
            MailMessage mail = new MailMessage();
            //DEBEMOS INDICAR EL FROM, ES DECIR, DE QUE CUENTA VIENE 
            //EL CORREO (LA NUESTRA DE appsettings.json)
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            mail.From = new MailAddress(user);

            //LOS DESTINATARIOS SON UNA COLECCION
            mail.To.Add(to);
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string host = this.configuration.GetValue<string>("MailSettings:Server:Host");
            int port = this.configuration.GetValue<int>("MailSettings:Server:Port");
            bool ssl = this.configuration.GetValue<bool>("MailSettings:Server:Ssl");
            bool defaultCredentials = this.configuration.GetValue<bool>("MailSettings:Server:DefaultCredentials");

            //CREAMOS LA CLASE SERVIDOR SMPT
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = defaultCredentials;

            //CREAMOS LAS CREDENCIALES PARA EL MAIL
            NetworkCredential credentials = new NetworkCredential(user, password);
            smtpClient.Credentials = credentials;

            await smtpClient.SendMailAsync(mail);

            ViewData["MENSAJE"] = "Mail enviado correctamente??";

            return View();
        }
    }
}
