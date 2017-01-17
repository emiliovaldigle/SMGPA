using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;
using SMGPA.Filters;
using PagedList;
namespace SMGPA.Controllers
{
    public class ActivitiesController : Controller
    {
        private SMGPAContext db = new SMGPAContext();

        /*GET: Activities
        Return the View Index with Collection of
        Activities, it also can be filtered by  4 differents
        filters, on initial date, end date, status and type of 
        the process of the Activity*/
        public ActionResult Index(string proc_search, string dateini, string datend, string state, int? page, string cfilter1, string cfilter2, string cfilter3, string cfilter4)
        {
            int pageSize;
            int pageNumber;
            var activity = db.Activity.Include(a => a.Proceso);
            if (proc_search != null || dateini != null || datend != null || state != null)
            {
                page = 1;
            }
            else
            {
                if (dateini == null)
                {
                    dateini = cfilter1;
                }
                if (datend == null)
                {
                    datend = cfilter2;
                }
                if (proc_search == null)
                {
                    proc_search = cfilter3;
                }
                if (state == null)
                {
                    state = cfilter4;
                }
            }
            ViewBag.cfilter1 = dateini;
            ViewBag.cfilter2 = datend;
            ViewBag.cfilter3 = proc_search;
            ViewBag.cfilter4 = state;
            if (!string.IsNullOrEmpty(proc_search))
            {
                activity = activity.Where(a => a.idProcess.ToString().Equals(proc_search));
            }
            if (!string.IsNullOrEmpty(dateini) && !string.IsNullOrEmpty(datend))
            {
                DateTime myDateStart;
                DateTime myDateEnd;
                if (DateTime.TryParse(dateini, out myDateStart) && DateTime.TryParse(datend, out myDateEnd))
                {
                    myDateStart = DateTime.ParseExact(dateini, "dd/MM/yyyy",
                                   System.Globalization.CultureInfo.InvariantCulture);
                    myDateEnd = DateTime.ParseExact(datend, "dd/MM/yyyy",
                                               System.Globalization.CultureInfo.InvariantCulture);
                    activity = activity.Where(a => a.start_date >= myDateStart && a.end_date <= myDateEnd);
                }
                else
                {
                    pageSize = 6;
                    pageNumber = (page ?? 1);
                    ViewBag.proc_search = new SelectList(db.Process, "idProcess", "Criterio");
                    return View(activity.ToList().ToPagedList(pageNumber, pageSize));
                }
            }

            if (!string.IsNullOrEmpty(state))
            {
                States estatus;
                Enum.TryParse(state, out estatus);
                activity = activity.Where(a => a.state == estatus);
            }
            pageSize = 6;
            pageNumber = (page ?? 1);
            ViewBag.proc_search = new SelectList(db.Process, "idProcess", "Criterio");
            return View(activity.ToList().ToPagedList(pageNumber, pageSize));
        }

        /* GET: Activities/Details/id
         Return View with full object Activity
         but his id
        */
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

        /* GET: Activities/Create
           Return the View Create who let the user        
           create an Activity */
        public ActionResult Create()
        {
            ViewBag.idProcess = new SelectList(db.Process, "idProcess", "Criterio");
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre");
            return View();
        }

        /* POST: Activities/Create
        Post the acivity into the bd if
        model is Valid */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idActivity,state,start_date,end_date,Nombre,idProcess, idCareer")] Activity activity)
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
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre");
            return View(activity);
        }
        /*Function that add the Tasks from the Operations existing
        in the process related to the Activity
        */
        public static void GenerateTasks(Activity activity)
        {
            using (SMGPAContext db = new SMGPAContext())
            {
                Process proceso = db.Process.Find(activity.idProcess);
                List<Operation> operaciones = proceso.Operations.ToList();
                Activity actividad = db.Activity.Find(activity.idActivity);
                foreach (Operation o in operaciones)
                {
                    Tasks tarea = new Tasks();
                    tarea.idTask = Guid.NewGuid();
                    tarea.Operacion = o;
                    tarea.Actividad = actividad;
                    if (o.idPredecesora != null)
                    {
                        Operation predecesora = db.Operation.Find(o.idPredecesora);
                        List<Tasks> tareas = actividad.Tareas.ToList();
                        foreach (Tasks ta in tareas)
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

        /* GET: Activities/Edit/id
        Return the View with the Activity to 
        Update*/
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
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre");
            return View(activity);
        }

        /* POST: Activities/Edit/id
        Update the Activity with the new params
        from the View*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idActivity,state,start_date,end_date,Nombre, idCareer")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                Activity actividad = db.Activity.Single(a => a.idActivity == activity.idActivity);
                actividad.Nombre = activity.Nombre;
                actividad.Carrera = activity.Carrera;
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.idProcess = new SelectList(db.Process, "idProcess", "Criterio", activity.idProcess);
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre");
            return View(activity);
        }

        /* GET: Activities/Delete/id
        Return the View who deletes physically 
        the record with the given id    */
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

        /* POST: Activities/DeleteConfirmed/id
        Deletes physcally the Activity with
        the given id    */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (Tasks t in activity.Tareas.ToList())
            {
                List<Observation> obs = db.Observation.Where(o => o.Tarea.idTask == t.idTask).ToList();
                foreach (Observation ob in obs)
                {
                    db.Observation.Remove(ob);
                }
                List<Document> docs = db.Document.Where(d => d.idTask == t.idTask).ToList();
                foreach (Document doc in docs)
                {
                    db.Document.Remove(doc);
                }
                db.Task.Remove(t);
            }
            db.Activity.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /* Function that return the View Tasks with
        the Tasks of the given id of an Activity
        */
        [HttpGet]
        public ActionResult Tasks(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity actividad = db.Activity.Find(id);
            if (actividad == null)
            {
                return PartialView("Tasks");
            }
            ViewBag.Actividad = actividad.Nombre;
            TempData["Activity"] = actividad;
            return View("Tasks", actividad.Tareas.ToList());
        }
        /* Function that return the Modal _DetailsTask
        with the full object (Task) loaded in here the
        user would see observations and documents*/
        [HttpGet]
        public async Task<ActionResult> DetailsTask(Guid? id)
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
            if (tarea.Documentos.Count > 0)
            {
                ViewBag.Documento = "Documentos";
            }
            ViewBag.Tarea = tarea.Operacion.Nombre;
            return PartialView("_DetailsTask", tarea);

        }
        /*Return the View that let the user set temporallity and
        assign responsables to the Task
            */
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
            Activity activity = (Activity)TempData["Activity"];
            Activity actividad = db.Activity.Find(activity.idActivity);
            TempData["Activity"] = actividad;
            ViewBag.Tarea = tarea.Operacion.Nombre;
            ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
            ViewBag.idResponsable = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
            ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
            ViewBag.Tarea = tarea.Operacion.Nombre;
            TempData["Task"] = tarea;
            return PartialView("_ConfigureTask", tarea);

        }
        /* Function that Post the params
        of temporallity and responsables and/or validators
        it has validation for dates */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfigureTask([Bind(Include = "idTask,fechaInicio,fechaFin,TiempoInactividad,DesplazamientoHoras,DesplazamientoDias, Estado, idFunctionary, idResponsable, idEntities, idOperation")] Tasks task)
        {
            Activity activity = (Activity)TempData["Activity"];
            DateTime? higherdate = null;
            DateTime? lowerdate = null;
            Activity actividad = db.Activity.Find(activity.idActivity);
            Tasks Tarea = (Tasks)TempData["Task"];
            if (ModelState.IsValid)
            {
                Tasks tarea = await db.Task.SingleAsync(t => t.idTask.Equals(Tarea.idTask));
                if (tarea == null)
                {
                    return HttpNotFound();
                }
                Operation operacion = await db.Operation.FindAsync(tarea.idOperation);
                switch (operacion.Type)
                {
                    case OperationType.ENTIDAD:
                        if (task.idResponsable != null)
                        {
                            tarea.ResponsableEntity = await db.Entity.FindAsync(task.idResponsable);
                        }
                        else
                        {
                            TempData["Task"] = tarea;
                            ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
                            ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                            ViewBag.idResponsable = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                            ViewBag.Creada = null;
                            ViewBag.Errores = "Tarea con Errores, debe especificar a la Entidad responsable";
                            TempData["Activity"] = db.Activity.Find(activity.idActivity);
                            return PartialView("_ConfigureTask", await db.Task.FindAsync(Tarea.idTask));
                        }
                        break;
                    case OperationType.FUNCIONARIO:
                        if (task.idFunctionary != null)
                        {
                            tarea.Responsable = await db.Functionary.FindAsync(task.idFunctionary);
                        }
                        else
                        {
                            TempData["Task"] = tarea;
                            ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
                            ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                            ViewBag.Creada = null;
                            ViewBag.Errores = "Tarea con Errores, debe especificar al funcionario Responsable";
                            TempData["Activity"] = db.Activity.Find(activity.idActivity);
                            return PartialView("_ConfigureTask", await db.Task.FindAsync(Tarea.idTask));
                        }
                        break;
                }
                if (operacion.Validable)
                {
                    if (task.idEntities != null)
                    {
                        tarea.Participantes = await db.Entity.FindAsync(task.idEntities);
                    }
                    else
                    {
                        TempData["Task"] = tarea;
                        ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
                        ViewBag.idResponsable = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                        ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                        ViewBag.Creada = null;
                        ViewBag.Errores = "Tarea con Errores, debe especificar a la Entidad validadora";
                        TempData["Activity"] = db.Activity.Find(activity.idActivity);
                        return PartialView("_ConfigureTask", await db.Task.FindAsync(Tarea.idTask));
                    }
                }
                tarea.fechaInicio = task.fechaInicio;
                tarea.fechaFin = task.fechaFin;
                tarea.Estado = StatusEnum.INACTIVA;
                if (tarea.fechaInicio != null && tarea.fechaFin != null && tarea.fechaInicio > DateTime.Now
                    && tarea.fechaFin > tarea.fechaInicio)
                {
                    foreach (Tasks t in actividad.Tareas)
                    {
                        if (t.fechaInicio < lowerdate || lowerdate == null)
                        {
                            lowerdate = t.fechaInicio;
                        }
                        if (t.fechaFin > higherdate || higherdate == null)
                        {
                            higherdate = t.fechaFin;
                        }

                    }
                    actividad.start_date = ((DateTime)lowerdate).Date;
                    actividad.end_date = ((DateTime)higherdate).Date;
                    await db.SaveChangesAsync();
                    TempData["Task"] = tarea;
                    ViewBag.Creada = "Tarea ha sido configurada correctamente";
                    ViewBag.Errores = null;
                    ViewBag.idResponsable = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                    ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
                    ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
                    return PartialView("_ConfigureTask", tarea);
                }
            }
            TempData["Task"] = await db.Task.FindAsync(Tarea.idTask);
            ViewBag.idResponsable = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
            ViewBag.idFunctionary = new SelectList(db.Functionary.ToList(), "idUser", "Nombre");
            ViewBag.idEntities = new SelectList(db.Entity.ToList(), "idEntities", "Nombre");
            ViewBag.Creada = null;
            ViewBag.Errores = "Tarea con Errores, considerar que Fecha de Inicio debe ser mayor al tiempo actual";
            TempData["Activity"] = db.Activity.Find(activity.idActivity);
            return PartialView("_ConfigureTask", await db.Task.FindAsync(Tarea.idTask));

        }
        //Function that allows to Download the required File
        [Authorizate(Disabled = true, Public = false)]
        public FileResult Download(string file)
        {
            return File("~/uploads/" + file, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorizate(Disabled = true, Public = false)]
        public JsonResult RutAutoComplete(string term)
        {
            List<string> Rut = new List<string>();
            Rut = db.Functionary.Where(x => x.Rut.StartsWith(term)).Select(y => y.Rut).ToList();
            return Json(Rut, JsonRequestBehavior.AllowGet);

        }
        [Authorizate(Disabled = true, Public = false)]
        [HttpGet]
        public JsonResult CheckUser(string rut)
        {
            if (rut == null)
            {
                return Json(new { sucess = false });
            }
            Functionary functionary = db.Functionary.Where(f => f.Rut.Equals(rut)).FirstOrDefault();
            if (functionary == null)
            {
                return Json(new { sucess = false });
            }
            string carrera = functionary.idCareer == null ? "No figura" : functionary.Carrera.Nombre;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera, sucess = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorizate(Disabled = true, Public = false)]
        public JsonResult EntityAutoComplete(string term)
        {
            List<string> Nombre = new List<string>();
            Nombre = db.Entity.Where(x => x.Nombre.StartsWith(term)).Select(y => y.Nombre).ToList();
            return Json(Nombre, JsonRequestBehavior.AllowGet);

        }
        [Authorizate(Disabled = true, Public = false)]
        [HttpGet]
        public JsonResult CheckEntity(string nombre)
        {
            if (nombre == null)
            {
                return Json(new { sucess = false });
            }
            Entities entidad = db.Entity.Where(e => e.Nombre.Equals(nombre)).FirstOrDefault();
            if (entidad == null)
            {
                return Json(new { sucess = false });
            }
            return Json(new { identity = entidad.idEntities, sucess = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
