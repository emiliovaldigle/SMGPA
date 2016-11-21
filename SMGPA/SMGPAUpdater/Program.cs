using System;
using System.Collections.Generic;
using System.Linq;
using SMGPA.Models;
using System.Threading.Tasks;
using Notificator;
namespace SMGPAUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Background Tasks of SMPGA");
                while (true)
                {
                UpdateActiva();
                UpdatePendiente();
                ReSchedule();
                }
        }
        static void UpdateActiva()
        {
            using (SMGPAContext db = new SMGPAContext())
            {
                DateTime Today = DateTime.Now;
                string DateToday = Today.ToString("yyyy-MM-dd HH:mm");
                DateTime myDate = DateTime.ParseExact(DateToday, "yyyy-MM-dd HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                List<Tasks> Tareas = db.Task.Where(t => t.fechaInicio == myDate).ToList();
                if (Tareas.Count > 0)
                {
                    List<Tasks> _Tasks = Tareas.Where(t => t.Estado == StatusEnum.INACTIVA).ToList();
                    if (_Tasks.Count > 0)
                    {
                        Console.WriteLine("Updating Inactive to Active Status on Tasks");
                        foreach (Tasks t in _Tasks)
                        {
                            t.Estado = StatusEnum.ACTIVA;
                            string link = "http://localhost/SMGPA/Tasks/Details/" + t.idTask;
                            Notification notificator = new Notification();
                            string DestinatarioA = db.Functionary.Find(t.idFunctionary).MailInstitucional;
                            notificator.NotificateAll(DestinatarioA, link, 1);
                            foreach (Functionary f in t.Participantes.Involucrados)
                            {
                                string DestinatarioB = db.Functionary.Find(f.idUser).MailInstitucional;
                                if (f.idUser != t.idFunctionary)
                                {
                                    notificator.NotificateAll(DestinatarioB, link, 2);
                                }
                            }
                            Console.WriteLine("Task: " + t.idTask + " updated ");
                        }
                    }
                }
                db.SaveChanges();
            }
        }
        static void UpdatePendiente()
        {
            using (SMGPAContext db = new SMGPAContext())
            {
                DateTime Today = DateTime.Now;
                string DateToday = Today.ToString("yyyy-MM-dd HH:mm");
                DateTime myDate = DateTime.ParseExact(DateToday, "yyyy-MM-dd HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                List<Tasks> Tareas = db.Task.Where(t => t.Estado == StatusEnum.ACTIVA && myDate >= t.fechaFin).ToList();
                Tareas.AddRange(db.Task.Where(t => t.Estado == StatusEnum.EN_PROGRESO && myDate >= t.fechaFin).ToList());
                if (Tareas.Count > 0)
                {
                    foreach (Tasks t in Tareas)
                    {
                        Console.WriteLine("Task: " + t.idTask + " updated to Pendiente");
                        t.Estado = StatusEnum.PENDIENTE;
                    }
                }
                db.SaveChanges();
            }
            
        }
        static void ReSchedule()
        {
            using (SMGPAContext db = new SMGPAContext())
            {
                List<Tasks> Pendientes = db.Task.Where(t => t.Estado == StatusEnum.PENDIENTE).ToList();
                if (Pendientes.Count > 0)
                {
                    foreach (Tasks t in Pendientes)
                    {
                        DateTime initialDate = (DateTime)t.fechaInicio;
                        DateTime endDate = (DateTime)t.fechaFin;
                        TimeSpan difference = endDate - initialDate;
                        t.Estado = StatusEnum.ACTIVA;
                        var days = difference.TotalDays;
                        t.fechaInicio = DateTime.Now;
                        t.fechaFin = DateTime.Now.AddDays(days);
                        t.Reprogramaciones = t.Reprogramaciones == null ? 1 : t.Reprogramaciones + 1;
                        Console.WriteLine("Task: " + t.idTask + " Scheduled");
                        string link = "http://localhost/SMGPA/Tasks/Details/" + t.idTask;
                        Notification notificator = new Notification();
                        foreach (Functionary f in t.Participantes.Involucrados)
                        {
                            string Destinatario = db.Functionary.Find(f.idUser).MailInstitucional;
                            if (f.idUser != t.idFunctionary)
                            {
                                notificator.NotificateAll(Destinatario, link, 3);
                            }
                        }
                        Console.WriteLine("Task: " + t.idTask + " updated ");
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
