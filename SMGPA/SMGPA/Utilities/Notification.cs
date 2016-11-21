using SMGPA.Models;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;

namespace SMGPA
{
    public class Notification
    {
        public async Task<bool> RecoverPassword(User usuario, string link)
        {
            if (usuario != null)
            {
                var body = "<p>Por favor ingresar a la siguiente URL para restablecer contraseña</p>" +
                    "<p>" + link + "</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(usuario.MailInstitucional));  // replace with valid value 
                message.From = new MailAddress("soportesmgpa@gmail.com");  // replace with valid value
                message.Subject = "Restablecer contraseña SMGPA";
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
                    await smtp.SendMailAsync(message);
                    return true;
                }
           
            }
            return false;
        }
        public async Task<bool> NotificateAll(Functionary Responsable, Functionary Destinatario, Tasks Tarea, string link, int Mensaje)
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
                body = "<p>El Funcionario" + Responsable.Nombre + " " + Responsable.Apellido + " ha comentado la Tarea</p>"
               + "<p>" + link + "</p>";
                subj = "- Asignación a Tarea";
            }
            var message = new MailMessage();
            message.To.Add(new MailAddress(Destinatario.MailInstitucional));  // replace with valid value 
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
                await smtp.SendMailAsync(message);

            }
            return true;
        }
        public async Task<bool> NotificateParticipants(Functionary Responsable,Functionary Destinatario, Tasks Tarea, string link)
        {
                var body = "<p>El Funcionario"+Responsable.Nombre +" "+Responsable.Apellido +" ha subido un Documento en la Tarea </p>"
                   + "<p>"+link+"</p>" ;
                var message = new MailMessage();
                message.To.Add(new MailAddress(Destinatario.MailInstitucional));  // replace with valid value 
                message.From = new MailAddress("soportesmgpa@gmail.com");  // replace with valid value
                message.Subject = "Notificación SMGPA, Funcionario ha subido documento";
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
                    await smtp.SendMailAsync(message);
          
                }
            return true;
        }

        public async Task<bool> AddNotification()
        {
            return false;
        }

    }
}