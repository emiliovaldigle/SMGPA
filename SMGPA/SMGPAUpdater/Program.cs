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
                    updateActivities();
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
                            Activity actividad = db.Activity.Where(a => a.idActivity.Equals(t.idActivity)).FirstOrDefault();
                            actividad.state = States.Activa;
                            string link = "http://localhost/SMGPA/Tasks/Details/" + t.idTask;
                            Notification notificator = new Notification();
                            string DestinatarioA = db.Functionary.Find(t.idFunctionary).MailInstitucional;
                            notificator.NotificateAll(DestinatarioA, link, 1);
                            string CuerpoA = "Haz sido asignado como Responsable a la Tarea "+ t.Operacion.Nombre;
                            addNotification(CuerpoA, t.idFunctionary, t.fechaInicio, link);
                            foreach (Functionary f in t.Participantes.Involucrados)
                            {
                                string DestinatarioB = db.Functionary.Find(f.idUser).MailInstitucional;
                                if (f.idUser != t.idFunctionary)
                                {
                                    string CuerpoB = "Haz sido asignado como Validador a la Tarea "+ t.Operacion.Nombre;
                                    notificator.NotificateAll(DestinatarioB, link, 2);
                                    addNotification(CuerpoB, f.idUser, t.fechaInicio, link);
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
                            string Cuerpo = "Tarea " + t.Operacion.Nombre + " ha sido reprogramada debido a falta de gestión";
                            addNotification(Cuerpo,f.idUser, t.fechaInicio, link);
                            notificator.NotificateAll(Destinatario, link, 3);
                        }
                        Console.WriteLine("Task: " + t.idTask + " updated ");
                    }
                }
                db.SaveChanges();
            }
        }
        static void addNotification(string Cuerpo, Guid? idFunctionary, DateTime? Fecha, string URL)
        {
     
            using(SMGPAContext db = new SMGPAContext())
            {
                Functionary User = db.Functionary.Find(idFunctionary);
                Notificacion n = new Notificacion();
                n.idNotification = Guid.NewGuid();
                n.Funcionario = User;
                n.Cuerpo = Cuerpo;
                n.Fecha = Fecha;
                n.UrlAction = URL;
                User.Notificaciones.Add(n);
                db.SaveChanges();
            }
        }
        static void updateActivities()
        {
            using (SMGPAContext db = new SMGPAContext())
            {
                List<Activity> Activities = db.Activity.ToList();
                List<Activity> Actividades =Activities.Where(a => a.state.Equals(States.Inactiva)).ToList();
                if(Actividades != null)
                {
                    foreach(Activity a in Actividades)
                    {
                        foreach(Tasks t in a.Tareas)
                        {
                            if(t.Estado.Equals(StatusEnum.EN_PROGRESO)|| t.Estado.Equals(StatusEnum.ACTIVA))
                            {
                                Console.WriteLine("Updating Activities status from Inactiva to Active");
                                a.state = States.Activa;
                               
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
                
        }
    }
}
