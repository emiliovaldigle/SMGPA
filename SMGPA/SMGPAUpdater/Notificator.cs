using SMGPA.Models;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Collections.Generic;

namespace Notificator
{
    public class Notification
    {
        public Notification()
        {

        }
        public void NotificateAll(string Destinatario, string link, int Mensaje)
        {
            var body = "";
            var subj = "";
            if (Mensaje == 1)
            {
                body = "<p>Se le ha asignado como Responsable a la Tarea"
                 + "<p>" + link + "</p>";
                subj = "- Asignación de Tarea";
            }
            if (Mensaje == 2)
            {
                body = "<p>Se le ha asignado como Validador a la Tarea"
                 + "<p>" + link + "</p>";
                subj = "- Asignación a Tarea";
            }
            if (Mensaje == 3)
            {
                body = "<p>Se ha reprogramado la Tarea"
                 + "<p>" + link + " Debido a falta de gestión por parte de los participantes</p>";
                subj = "- Re programación de Tarea";
            }

            var message = new MailMessage();
            message.To.Add(new MailAddress(Destinatario));  // replace with valid value 
            message.From = new MailAddress("soportesmgpa@gmail.com");  // replace with valid value
            message.Subject = "Notificación SMGPA" + subj;
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new System.Net.NetworkCredential
                {
                    UserName = "soportesmgpa@gmail.com",  // replace with valid value
                    Password = "123.pass"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);

            }

        }


        public async Task<bool> AddNotification()
        {
            return false;
        }

    }
}