using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;
using SMGPA.Filters;
using System.Threading.Tasks;
using System.IO;

namespace SMGPA.Controllers
{
    [Authorizate(Disabled = true, Public = false)]
    public class TasksController : Controller { 
    

        private SMGPAContext db = new SMGPAContext();

        /* GET: Tasks
        Get The ACTIVES and IN_PROGRESS Tasks from db
        that match de stored user's id in the Session var
        */
        public ActionResult Tasks()
        {
            Guid idUser = (Guid)Session["UserID"];
            if (idUser == null)
            {
                RedirectToAction("LogOut", "Account");
            }
            Functionary usuario = db.Functionary.Find(idUser);
            List<Tasks> Tareas = new List<Tasks>();
            List<Tasks> Tasks = db.Task.ToList();
            List<Tasks> TasksFiltered = Tasks.Where(t => t.Estado.Equals(StatusEnum.ACTIVA) || t.Estado.Equals(StatusEnum.EN_PROGRESO)).ToList();
            foreach (Tasks t in TasksFiltered)
            {
                if (t.idFunctionary != null)
                {
                    if (t.idFunctionary.Equals(idUser))
                    {
                        Tareas.Add(t);
                    }
                }
                if (t.idResponsable != null)
                {
                    foreach (Functionary f in db.Entity.Find(t.idResponsable).Involucrados)
                    {
                        if (f.idUser.Equals(idUser))
                        {
                            Tareas.Add(t);
                        }
                    }
                }
                if (t.idEntities != null)
                {
                    foreach (Functionary f in db.Entity.Find(t.idEntities).Involucrados)
                    {
                        if (f.idUser.Equals(idUser)&& !Tareas.Contains(t))
                        {
                            Tareas.Add(t);
                        }
                    }
                }
            }
            return View(Tareas);
        }
        /*GET: Tasks/Details/id
        Return the View that has the full Task object
        and depending in the Responsabillity of the user
        is what View will return->
        ---->_Details (CAN UPLOAD FILE && POST OBSERVATION)
        ---->_DetailsValidate(ONLY CAN POST OBSERVATION)
        ---->_DetailsUpload(ONLY CAN POST FILE)
        */
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Tasks tarea = await db.Task.FindAsync(id);
            ViewBag.Tarea = tarea.Operacion.Nombre;
            if (tarea == null)
            {
                return HttpNotFound();
            }
            TempData["Tarea"] = id;
            if (tarea.Documentos.Count > 0)
            {
                ViewBag.Documento = "Tareas";
            }
            List<SelectListItem> ValidacionEstatus = new List<SelectListItem>();
            ViewBag.EstatusEnum = new SelectList(db.Role, "idRole", "Nombre");
            if (tarea.idFunctionary == (Guid)Session["UserID"])
            {
                return View("_DetailsUpload", tarea);
                
            }
            if(tarea.idResponsable != null)
            {
                foreach(Functionary f in db.Entity.Find(tarea.idResponsable).Involucrados)
                {
                    if (f.idUser.Equals((Guid)Session["UserID"]))
                    {
                        return View("_Details", tarea);
                    }
                }
            }
            return View("_DetailsValidate", tarea);
        }
        /*POST: Tasks/UploadFile/filedoc
        Store requested file in the server
        if document exists this function will rename it
        following {X} number of doc, also notificate the
        participants of the Task*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase fileDoc)
        {
            if (fileDoc == null)
            {
                return Content("Error, no hay archivo seleccionado");
            }
            if (ModelState.IsValid)
            {
                Guid id = (Guid)TempData["Tarea"];
                Tasks task = db.Task.Single(t => t.idTask == id);         
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        var pathToSave = "~/uploads";
                        var fileName = Path.GetFileName(file.FileName);
                        int count = 1;
                        string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
                        string extension = Path.GetExtension(fileName);
                        string path = Path.GetDirectoryName(fileName);
                        string newFullPath = fileName;
                        string absolutePath = HttpContext.Server.MapPath(pathToSave+'/'+newFullPath);
                        bool exists = System.IO.File.Exists(absolutePath);
                        while (System.IO.File.Exists(absolutePath))
                        {
                            string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                            newFullPath = Path.Combine(path, tempFileName + extension);
                            absolutePath = HttpContext.Server.MapPath(pathToSave + '/' + newFullPath);
                        }
                        string Ruta = Path.Combine(
                         Server.MapPath(pathToSave), newFullPath);
                        file.SaveAs(Ruta);
                        Document documento = new Document { idDocument = Guid.NewGuid(), Path = newFullPath, idTask = task.idTask, Fecha = DateTime.Today };
                        task.Documentos.Add(documento);
                        task.Estado = StatusEnum.EN_PROGRESO;
                    }               
                    string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Details", "Tasks", new { id = task.idTask });
                    Notification notificator = new Notification();
                    Functionary user = await db.Functionary.FindAsync((Guid)Session["UserID"]);
                    if (!task.idResponsable.Equals(task.idEntities))
                    {
                        Operation Operacion = db.Operation.Find(task.idOperation);
                        if (Operacion.Validable)
                        {
                            foreach (Functionary f in task.Participantes.Involucrados)
                            {
                                Notificacion n = new Notificacion();
                                n.idNotification = Guid.NewGuid();
                                n.Fecha = DateTime.Now;
                                n.Funcionario = f;
                                n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha subido un Documento en la Tarea: " + task.Operacion.Nombre;
                                n.UrlAction = link;
                                n.Vista = false;
                                f.Notificaciones.Add(n);
                                await notificator.NotificateParticipants(user, db.Functionary.Find(f.idUser), task, link);
                            }
                        }
                        if (Operacion.Type.Equals(OperationType.ENTIDAD))
                        {
                            foreach (Functionary f in task.ResponsableEntity.Involucrados)
                            {
                                Notificacion n = new Notificacion();
                                n.idNotification = Guid.NewGuid();
                                n.Fecha = DateTime.Now;
                                n.Funcionario = f;
                                n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha subido un Documento en la Tarea: " + task.Operacion.Nombre;
                                n.UrlAction = link;
                                n.Vista = false;
                                f.Notificaciones.Add(n);
                                await notificator.NotificateParticipants(user, db.Functionary.Find(f.idUser), task, link);
                            }
                        }
                    }
                    
                    db.SaveChanges();
                    Tasks updatedTask = db.Task.Find(task.idTask);
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            return HttpNotFound();
        }
        /*GET: Tasks/Download/file
        Download the requested file*/

        public FileResult Download(string file)
        {
            return File("~/uploads/" + file, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
        /*GET: Tasks/AddObservation/id
        Pops up a Modal that allows the user to post
        an Observation of the task that containas a validation
        status from whatever
        */

        public async Task<ActionResult> AddObservation(Guid? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            Tasks task = await db.Task.FindAsync(id);
            if(task == null)
            {
                return HttpNotFound();
            }
            TempData["Task"] = task;
            return PartialView("_AddObservation");
        }
        /*POST: Tasks/AddObservation/observation
        Attach a Observation on the Task 
        found
        */
        [HttpPost]
        public async Task<ActionResult> AddObservation([Bind(Include ="idObservation,FechaComentario,Comentario,ValidacionEstatus")] Observation observation)
        {
            Tasks Task = (Tasks)TempData["Task"];
            Tasks Tarea = await db.Task.FindAsync(Task.idTask);
            Functionary user = await db.Functionary.FindAsync((Guid)Session["UserID"]);
            string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Details", "Tasks", new { id = Tarea.idTask });
            Notification notificator = new Notification();
            if (ModelState.IsValid)
            {
                observation.idObservation = Guid.NewGuid();
                observation.FechaComentario = DateTime.Now;
                observation.Funcionario = user;
                Tarea.Observaciones.Add(observation);
                user.Observaciones.Add(observation);
                Tarea.Estado = StatusEnum.EN_PROGRESO;
                List<Guid> idsUsuario = new List<Guid>();
                List<Observation> Obs = Tarea.Observaciones.ToList();
                //obtengo todas las observaciones aceptadas de tarea
                foreach (Observation o in Obs)
                {
                    if (!idsUsuario.Contains(o.Funcionario.idUser) && o.ValidacionEstatus == Validate.APROBADO)
                    {
                        idsUsuario.Add(o.Funcionario.idUser);
                    }
                }
                //se debe validar que todos los Funcionarios de la Entidad tengan observaciones sobre la Tarea 
                switch (Tarea.Operacion.Type)
                {
                    case OperationType.ENTIDAD:
                        bool EntidadParticipo = false;
                        bool EntidadResponsable = false;
                        if (Tarea.Operacion.Validable)
                        {

                            foreach (Functionary f in Tarea.Participantes.Involucrados)
                            {
                                await notificator.NotificateAll(user, db.Functionary.Find(f.idUser), Tarea, link, 3);
                                Notificacion n = new Notificacion();
                                n.idNotification = Guid.NewGuid();
                                n.Fecha = DateTime.Now;
                                n.Funcionario = f;
                                n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha comentado la Tarea: " + Tarea.Operacion.Nombre;
                                n.UrlAction = link;
                                n.Vista = false;
                                f.Notificaciones.Add(n);
                            }
                            int divisor1 = idsUsuario.Count;
                            int dividendo1 = Tarea.Participantes.Involucrados.Count + Tarea.ResponsableEntity.Involucrados.Count;
                            double division1 = (double)divisor1 / dividendo1;
                            double resultado1 = division1 * 100.0;
                            //verificamos que al menos el 50% de los involucrados haya comentado
                            if (resultado1 >= 50)
                            {
                                EntidadParticipo = true;
                            }
                            foreach (Functionary f in Tarea.ResponsableEntity.Involucrados)
                            {
                                await notificator.NotificateAll(user, db.Functionary.Find(f.idUser), Tarea, link, 3);
                                Notificacion n = new Notificacion();
                                n.idNotification = Guid.NewGuid();
                                n.Fecha = DateTime.Now;
                                n.Funcionario = f;
                                n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha comentado la Tarea: " + Tarea.Operacion.Nombre;
                                n.UrlAction = link;
                                n.Vista = false;
                                f.Notificaciones.Add(n);
                            }
                            int divisor2 = idsUsuario.Count;
                            int dividendo2 = Tarea.ResponsableEntity.Involucrados.Count + Tarea.Participantes.Involucrados.Count;
                            double division2 = (double)divisor2 / dividendo2;
                            double resultado2 = division2 * 100.0;
                            if (resultado2 >= 50)
                            {
                                EntidadResponsable = true;
                            }
                            if (EntidadParticipo && EntidadResponsable)
                            {
                               Tarea.Estado = StatusEnum.COMPLETADA;
                            }
                        }
                        else
                        {
                            foreach (Functionary f in Tarea.ResponsableEntity.Involucrados)
                            {
                                await notificator.NotificateAll(user, db.Functionary.Find(f.idUser), Tarea, link, 3);
                                Notificacion n = new Notificacion();
                                n.idNotification = Guid.NewGuid();
                                n.Fecha = DateTime.Now;
                                n.Funcionario = f;
                                n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha comentado la Tarea: " + Tarea.Operacion.Nombre;
                                n.UrlAction = link;
                                n.Vista = false;
                                f.Notificaciones.Add(n);
                            }
                            int divisor2 = idsUsuario.Count;
                            int dividendo2 = Tarea.ResponsableEntity.Involucrados.Count;
                            double division2 = (double)divisor2 / dividendo2;
                            double resultado2 = division2 * 100.0;
                            if (resultado2 >= 50.0)
                            {
                                EntidadResponsable = true;
                            }  
                        }
                                  
                        break;
                    case OperationType.FUNCIONARIO:
                        bool EntidadValidadora = false;
                        if (Tarea.Operacion.Validable)
                        {
                            foreach (Functionary f in Tarea.Participantes.Involucrados)
                            {
                                    await notificator.NotificateAll(user, db.Functionary.Find(f.idUser), Tarea, link, 3);
                                    Notificacion n = new Notificacion();
                                    n.idNotification = Guid.NewGuid();
                                    n.Fecha = DateTime.Now;
                                    n.Funcionario = f;
                                    n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha comentado la Tarea: " + Tarea.Operacion.Nombre;
                                    n.UrlAction = link;
                                    n.Vista = false;
                                    f.Notificaciones.Add(n);
                            }
                            int divisor1 = idsUsuario.Count;
                            int dividendo1 = Tarea.Participantes.Involucrados.Count;
                            double division1 = (double)divisor1 / dividendo1;
                            double resultado1 = division1 * 100.0;
                            //verificamos que al menos el 50% de los involucrados haya comentado
                            if (resultado1 >= 50)
                            {
                                EntidadParticipo = true;
                            }
                            //verificamos que al menos el 50% de los involucrados haya comentado
                            if (EntidadValidadora)
                            {
                                Tarea.Estado = StatusEnum.COMPLETADA;
                            }
                        }
                        else
                        {
                            if(Tarea.Documentos.Count > 0)
                            {
                                Tarea.Estado = StatusEnum.COMPLETADA;
                            }
                        }
                        break;
                }
                await db.SaveChangesAsync();
                return PartialView();
            }
            return PartialView();
        }
        /*Function who set the Notification status to Watched from a task
        of the user */
        [HttpPost]
        public async Task<JsonResult> SetNotificationView(Guid? id)
        {
            Notificacion notification = await db.Notificacion.FindAsync(id);
            if (notification != null)
            {
                notification.Vista = true;
                await db.SaveChangesAsync();
                return Json(new { sucess = true });
            }
            return Json(new { sucess = false });

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
