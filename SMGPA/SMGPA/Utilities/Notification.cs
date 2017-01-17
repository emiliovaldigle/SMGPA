using SMGPA.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Configuration;
namespace SMGPA
{
    public class Notification
    {
        public Notification()
        {

        }
        public static string clientMail = ConfigurationManager.AppSettings["clientMail"].ToString();
        public static string passwordMail = ConfigurationManager.AppSettings["passwordMail"].ToString();
        public static string hostMail = ConfigurationManager.AppSettings["hostMail"].ToString();
        public static string portMail = ConfigurationManager.AppSettings["portMail"].ToString();
        public async Task<bool> RecoverPassword(User usuario, string link)
        {
            if (usuario != null)
            {
                var body = "<p>Por favor ingresar a la siguiente URL para restablecer contraseña</p>" +
                    "<p>" + link + "</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(usuario.MailInstitucional)); 
                message.From = new MailAddress(clientMail);  // replace with valid value
                message.Subject = "Restablecer contraseña SMGPA";
                message.Body = string.Format(body);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new System.Net.NetworkCredential
                    {
                        UserName = clientMail,  // replace with valid value
                        Password = passwordMail // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = hostMail;
                    smtp.Port = int.Parse(portMail);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

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
            message.To.Add(new MailAddress(Destinatario.MailInstitucional));
            message.To.Add(new MailAddress(Destinatario.CorreoPersonal)); 
            message.From = new MailAddress(clientMail); 
            message.Subject = "Notificación SMGPA" + subj;
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new System.Net.NetworkCredential
                {
                    UserName = clientMail,  // replace with valid value
                    Password = passwordMail // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = hostMail;
                smtp.Port = int.Parse(portMail);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

            }
            return true;
        }
        public async Task<bool> NotificateParticipants(Functionary Responsable, Functionary Destinatario, Tasks Tarea, string link)
        {
            var body = "<p>El Funcionario" + Responsable.Nombre + " " + Responsable.Apellido + " ha subido un Documento en la Tarea </p>"
               + "<p>" + link + "</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(Destinatario.MailInstitucional));
            message.To.Add(new MailAddress(Destinatario.CorreoPersonal));// replace with valid value 
            message.From = new MailAddress(clientMail);  // replace with valid value
            message.Subject = "Notificación SMGPA, Funcionario ha subido documento";
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new System.Net.NetworkCredential
                {
                    UserName = clientMail,  // replace with valid value
                    Password = passwordMail // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = hostMail;
                smtp.Port = int.Parse(portMail);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

            }
            return true;
        }
        public void NotificateAll(string Destinatario, string Destinatario_2, string link, int Mensaje)
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
            if (Mensaje == 4)
            {
                body = "<p>Por favor realice acciones sobre siguiente Tarea"
                 + "<p>" + link;
                subj = "- Ausencia de gestión";
            }

            var message = new MailMessage();
            message.To.Add(new MailAddress(Destinatario));  // replace with valid value 
            message.To.Add(new MailAddress(Destinatario_2));
            message.From = new MailAddress(clientMail);  // replace with valid value
            message.Subject = "Notificación SMGPA" + subj;
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new System.Net.NetworkCredential
                {
                    UserName = clientMail,  // replace with valid value
                    Password = passwordMail // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = hostMail;
                smtp.Port = int.Parse(portMail);
                smtp.EnableSsl = true;
                smtp.Send(message);

            }

        }
        public bool sendSMS(Functionary f, string Message)
        {
            string url = "http://ida.itdchile.cl/publicSms/sendSms.html?username=evaldivia&password=evaldivia.unab&phone=" + 569 + f.NumeroTelefono + "+&message=" + Message + "&" +
            "originator =  & custom_id = vvv";
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(string.Format(url));
            webReq.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();

            //I don't use the response for anything right now. But I might log the response answer later on.   
            Stream answer = webResponse.GetResponseStream();
            StreamReader _recivedAnswer = new StreamReader(answer);
            return true;
        }
    }
}