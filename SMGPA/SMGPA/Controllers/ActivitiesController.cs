﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;
using SMGPA.Filters;

namespace SMGPA.Controllers
{
    public class ActivitiesController : Controller
    {
        private SMGPAContext db = new SMGPAContext();

        // GET: Activities
        public ActionResult Index()
        {
            var activity = db.Activity.Include(a => a.Proceso);
            return View(activity.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            ViewBag.idProcess = new SelectList(db.Process, "idProcess", "Criterio");
            return View();
        }

        // POST: Activities/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idActivity,state,start_date,end_date,Nombre,idProcess")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.idActivity = Guid.NewGuid();
                if (db.Activity.Where(a => a.Nombre.Equals(activity.Nombre) && a.idProcess.Equals(activity.idProcess)).FirstOrDefault() == null)
                    ViewBag.Existe = "Actividad ya Existe";
                db.Activity.Add(activity);
                db.SaveChanges();
                GenerateTasks(activity);
                return RedirectToAction("Index");
            }

            ViewBag.idProcess = new SelectList(db.Process, "idProcess", "Criterio", activity.idProcess);
            return View(activity);
        }
        public static void GenerateTasks(Activity activity)
        {
            using(SMGPAContext db = new SMGPAContext())
            {
                Process proceso = db.Process.Find(activity.idProcess);
                List<Operation> operaciones = proceso.Operations.ToList();
                Activity actividad = db.Activity.Find(activity.idActivity);
                foreach (Operation o in operaciones)
                {
                    Tasks tarea = new Tasks();
                    tarea.idTask = Guid.NewGuid();
                    tarea.Operacion = o;
                    if(o.idPredecesora != null)
                    {
                        Operation predecesora = db.Operation.Find(o.idPredecesora);
                        List<Tasks> tareas = actividad.Tareas.ToList();
                        foreach(Tasks ta in tareas)
                        {
                            Tasks aux = db.Task.Find(ta.idTask);
                            if (aux.Operacion.idOperation == predecesora.idOperation)
                            {
                                tarea.Predecesora = ta;
                            }
                        }
                    }
                    actividad.Tareas.Add(tarea);
                    db.SaveChanges();
                }
            }   
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProcess = new SelectList(db.Process, "idProcess", "Criterio", activity.idProcess);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idActivity,state,start_date,end_date,Nombre")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                Activity actividad = db.Activity.Single(a => a.idActivity == activity.idActivity);
                actividad.Nombre = activity.Nombre;
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.idProcess = new SelectList(db.Process, "idProcess", "Criterio", activity.idProcess);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if(activity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach(Tasks t in activity.Tareas.ToList())
            {
                List<Observation> obs = db.Observation.Where(o=> o.Tarea.idTask == t.idTask).ToList();
                foreach(Observation ob in obs)
                {
                    db.Observation.Remove(ob);
                }
                List<Document> docs = db.Document.Where(d => d.idTask == t.idTask).ToList();
                foreach(Document doc in docs)
                {
                    db.Document.Remove(doc);
                }
                db.Task.Remove(t);
            }
            db.Activity.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Tasks(Guid? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity actividad =  db.Activity.Find(id);
            if(actividad == null)
            {
                return PartialView("Tasks");
            }
            ViewBag.Actividad = actividad.Nombre;
            TempData["Activity"] = actividad;
            return View("Tasks", actividad.Tareas.ToList());
        }

        [HttpGet]
        public async Task<ActionResult> DetailsTask(Guid? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tarea = await db.Task.FindAsync(id);
            if(tarea == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(tarea.Documentos.Count > 0)
            {
                ViewBag.Documento = "Documentos";
            }
            ViewBag.Tarea = tarea.Operacion.Nombre;
            return PartialView("_DetailsTask", tarea);
            
        }
        [HttpGet]
        public async Task<ActionResult> ConfigureTask(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tarea = await db.Task.FindAsync(id);
            if (tarea == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = (Activity) TempData["Activity"];
            TempData["Activity"] = db.Activity.Find(activity.idActivity);
            ViewBag.Tarea = tarea.Operacion.Nombre;
            ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
            ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
            ViewBag.Tarea = tarea.Operacion.Nombre;
            TempData["Task"] = tarea;
            return PartialView("_ConfigureTask", tarea);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfigureTask([Bind(Include = "idTask,fechaInicio,fechaFin,TiempoInactividad,DesplazamientoHoras,DesplazamientoDias, Estado, idFunctionary, idEntities, idOperation")] Tasks task)
        {
            Activity activity = (Activity)TempData["Activity"];
            Tasks Tarea = (Tasks)TempData["Task"];
            if (ModelState.IsValid)
            {
                Tasks tarea = await db.Task.SingleAsync(t => t.idTask == Tarea.idTask);
                if (tarea == null)
                {
                    return HttpNotFound();
                }
                tarea.fechaInicio = task.fechaInicio;
                tarea.fechaFin = task.fechaFin;
                tarea.Responsable = await db.Functionary.FindAsync(task.idFunctionary);
                tarea.Participantes = await db.Entity.FindAsync(task.idEntities);
                tarea.TiempoInactividad = task.TiempoInactividad;
                tarea.Estado = StatusEnum.INACTIVA;    
                if(tarea.fechaInicio != null && tarea.fechaFin != null && tarea.Responsable != null && tarea.TiempoInactividad >= 0
                    && tarea.Participantes != null && tarea.fechaInicio >= DateTime.Now
                    && tarea.fechaFin > tarea.fechaInicio)
                {
                    await db.SaveChangesAsync();
                    TempData["Task"] = tarea;
                    ViewBag.Creada = "Tarea ha sido configurada correctamente";
                    ViewBag.Errores = null;
                    ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
                    ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");     
                    return PartialView("_ConfigureTask",tarea);
                }
                TempData["Task"] = tarea;
            }
            ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
            ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
            ViewBag.Creada = null;
            ViewBag.Errores = "Tarea con Errores, considerar que Fecha de Inicio debe ser mayor al tiempo actual"; 
            TempData["Activity"] = db.Activity.Find(activity.idActivity);
            return PartialView("_ConfigureTask", task);

        }
        [Authorizate(Disabled = true, Public = false)]
        public FileResult Download(string file)
        {
            return File("~/uploads/"+file, System.Net.Mime.MediaTypeNames.Application.Octet, file);
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
