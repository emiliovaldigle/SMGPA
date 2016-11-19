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
    [Authorizate(Disabled = true)]
    public class TasksController : Controller { 
    

        private SMGPAContext db = new SMGPAContext();

        // GET: Tasks
        
       public ActionResult Tasks()
        {
            Guid idUser = (Guid) Session["UserID"];
            if(idUser == null)
            {
                RedirectToAction("LogOut", "Account");
            }
            Functionary usuario = db.Functionary.Find(idUser);
            List<Tasks> Tareas = new List<Tasks>();
            List<Tasks> tasks = db.Task.ToList();
            List<Tasks> tareasresponsable = tasks.Where(t => t.idFunctionary == usuario.idUser).ToList();
            List<Entities> entidades = usuario.Entidades.ToList();
            Tareas = tareasresponsable;
            foreach(Entities e in entidades)
            {
                List<Tasks> tareasvalidador = db.Task.Where(t => t.idEntities == e.idEntities).ToList();
                Tareas.AddRange(tareasvalidador);
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
            if(tarea == null)
            {
                return HttpNotFound();
            }
            TempData["Tarea"] = tarea;
            ViewBag.DocumentO = tarea.Document.Path;
            List<SelectListItem> ValidacionEstatus = new List<SelectListItem>();
            ViewBag.EstatusEnum = new SelectList(db.Role, "idRole", "Nombre");
            if (tarea.idFunctionary == (Guid)Session["UserID"])
            {
                return View("_DetailsUpload", tarea);
            }
            return View("_DetailsValidate", tarea);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(Tasks _task)
        {
            if (_task == null)
            { 
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                Tasks tarea = (Tasks)TempData["Tarea"];
                Tasks task = db.Task.Single(t => t.idTask == tarea.idTask);    
                if(task.idDocument != null)
                {
                    Document doc = db.Document.Find(task.idDocument);
                    db.Document.Remove(doc);
                }
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        var pathToSave = "~/uploads";
                        var fileName = Path.GetFileName(file.FileName);
                        _task.Document.Path = Path.Combine(
                            Server.MapPath(pathToSave), fileName);
                        file.SaveAs(_task.Document.Path);
                        Document documento = new Document { idDocument = Guid.NewGuid(), Path = fileName };
                        task.Document = documento;
                        task.Estado = StatusEnum.EN_PROGRESO;
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
                await db.SaveChangesAsync();
                ViewBag.Agregada = "Observación agregada";
                return PartialView();
            }
            return PartialView();
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
