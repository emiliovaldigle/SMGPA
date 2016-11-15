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
            ViewBag.Actividad = actividad.Nombre;
            if(actividad == null)
            {
                return PartialView("Tasks");
            }
            return PartialView("Tasks", actividad.Tareas.ToList());
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
            ViewBag.Tarea = tarea.Operacion.Nombre;
            return PartialView("_ConfigureTask", tarea);

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
