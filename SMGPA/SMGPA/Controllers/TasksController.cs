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

        // GET: Tasks

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
                        if (f.idUser.Equals(idUser))
                        {
                            Tareas.Add(t);
                        }
                    }
                }
            }
            return View(Tareas);
        }
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
            TempData["Tarea"] = tarea;
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
                Tasks tarea = (Tasks)TempData["Tarea"];
                Tasks task = db.Task.Single(t => t.idTask == tarea.idTask);    
               
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        var pathToSave = "~/uploads";
                        var fileName = Path.GetFileName(file.FileName);
                        string Ruta = Path.Combine(
                            Server.MapPath(pathToSave), fileName);
                        file.SaveAs(Ruta);
                        Document documento = new Document { idDocument = Guid.NewGuid(), Path = fileName, idTask = task.idTask, Fecha = DateTime.Today };
                        task.Documentos.Add(documento);
                        task.Estado = StatusEnum.EN_PROGRESO;
                    }               
                    string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Details", "Tasks", new { id = task.idTask });
                    Notification notificator = new Notification();
                    Tasks _Tarea = await db.Task.FindAsync(tarea.idTask);
                    Functionary user = await db.Functionary.FindAsync((Guid)Session["UserID"]);
                    foreach (Functionary f in _Tarea.Participantes.Involucrados)
                    {
                        Notificacion n = new Notificacion();
                        n.idNotification = Guid.NewGuid();
                        n.Fecha = DateTime.Now;
                        n.Funcionario = f;
                        n.Cuerpo = "El Funcionario " + user.Nombre + " " + user.Apellido + " ha subido un Documento en la Tarea: " + _Tarea.Operacion.Nombre;
                        n.UrlAction = link;
                        n.Vista = false;
                        f.Notificaciones.Add(n);
                        await notificator.NotificateParticipants(_Tarea.Responsable,db.Functionary.Find(f.idUser), _Tarea, link);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Tasks",task);
                }
            }
            return HttpNotFound();
        }
        public FileResult Download(string file)
        {
            return File("~/uploads/" + file, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
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
        [HttpPost]
        public async Task<ActionResult> AddObservation([Bind(Include ="idObservation,FechaComentario,Comentario,ValidacionEstatus")] Observation observation)
        {
            Tasks Task = (Tasks)TempData["Task"];
            Tasks Tarea = await db.Task.FindAsync(Task.idTask);
            Functionary user = await db.Functionary.FindAsync((Guid)Session["UserID"]);
            if (ModelState.IsValid)
            {
                observation.idObservation = Guid.NewGuid();
                observation.FechaComentario = DateTime.Now;
                observation.Funcionario = user;
                Tarea.Observaciones.Add(observation);
                user.Observaciones.Add(observation);
                Tarea.Estado = StatusEnum.EN_PROGRESO;
                List<Guid> idsUsuario = new List<Guid>();
                bool Completada = false;
                Entities Entidad = await db.Entity.FindAsync(Tarea.idEntities);
                List<Observation> Obs = Tarea.Observaciones.ToList();
                foreach (Observation o in Obs)
                {
                    if (!idsUsuario.Contains(o.Funcionario.idUser))
                    {
                        var userObs = Obs.Where(ob => ob.Funcionario.idUser == o.Funcionario.idUser);
                        var lastObs = userObs.Last();
                        Completada = (lastObs.ValidacionEstatus.Equals(Validate.APROBADO)) ? true : false;
                        idsUsuario.Add(o.Funcionario.idUser);
                    }
                }
                //se debe validar que todos los Funcionarios de la Entidad tengan observaciones sobre la Tarea 
                bool Comented = false;
                //esto para poder considerar la tarea como Completada
                foreach (Functionary f in Entidad.Involucrados)
                {
                    if (f.idUser != Tarea.idFunctionary)
                    {
                        if (idsUsuario.Contains(f.idUser))
                        {
                            Comented = true;
                        }
                        else
                        {
                            Comented = false;
                        }
                    }
                }
                if (Comented)
                {
                    Tarea.Estado = Completada ? StatusEnum.COMPLETADA : StatusEnum.EN_PROGRESO;
                    if (Tarea.Estado == StatusEnum.COMPLETADA)
                    {
                        List<Tasks> dependencies = db.Task.Where(t=> t.idPredecesora == Tarea.idTask).ToList();
                        foreach(Tasks ta in dependencies)
                        {
                            ta.Documentos.Add(Tarea.Documentos.Last());
                        }
                        
                    }
                }
                string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Details", "Tasks", new { id = Tarea.idTask });
                Notification notificator = new Notification();
                List<Notificacion> Notificaciones = new List<Notificacion>();
                Tasks _Tarea = await db.Task.FindAsync(Tarea.idTask);
                foreach (Functionary f in _Tarea.Participantes.Involucrados)
                {
                    await notificator.NotificateAll(user, db.Functionary.Find(f.idUser), _Tarea, link,3);
                    Notificacion n = new Notificacion();
                    n.idNotification = Guid.NewGuid();
                    n.Fecha = DateTime.Now;
                    n.Funcionario = f;
                    n.Cuerpo = "El Funcionario "+ user.Nombre +" "+user.Apellido+ " ha comentado la Tarea: " +_Tarea.Operacion.Nombre;
                    n.UrlAction = link;
                    n.Vista = false;
                    f.Notificaciones.Add(n);

                }
                ViewBag.Agregada = "Observación agregada";
                await db.SaveChangesAsync();
                return PartialView();
            }
            return PartialView();
        }
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
