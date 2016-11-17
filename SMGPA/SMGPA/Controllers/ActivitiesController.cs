using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;

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
                    Guid idTarea = Guid.NewGuid();
                    Tasks tarea = new Tasks();
                    tarea.idTask = idTarea;
                    tarea.Operacion = o;
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
                tarea.DesplazamientoDias = task.DesplazamientoDias;
                tarea.DesplazamientoHoras = task.DesplazamientoHoras;
                tarea.Estado = StatusEnum.ACTIVA;    
                if(tarea.fechaInicio != null && tarea.fechaFin != null && tarea.Responsable != null && tarea.TiempoInactividad != null
                    && tarea.Participantes != null && tarea.DesplazamientoDias != null && tarea.DesplazamientoHoras != null)
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
            ViewBag.Errores = "Tarea con Errores"; 
            TempData["Activity"] = db.Activity.Find(activity.idActivity);
            return PartialView("_ConfigureTask", task);

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
