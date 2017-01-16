using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using SMGPA.Filters;

namespace SMGPA.Controllers
{
    public class ProcessesController : AsyncController
    {
        private SMGPAContext db = new SMGPAContext();

        /* GET: Processes
       Return the View Index with Collection of
       Processes*/
        public ActionResult Index()
        {
            return View(db.Process.ToList());
        }

        /* GET: Processes/Details/id    
            Return View with full object Process
            but his id
           */
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = db.Process.Find(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            return View(process);
        }

        /* GET: Processes/Create
       Return the View Create who let the user        
       create an Process */
        public ActionResult Create()
        {
            return View();
        }

        /* POST: Processes/Create
         Post the process into the bd if
         model is Valid */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProcess,Criterio,Descripcion")] Process process)
        {
            if (ModelState.IsValid)
            {
                db.Process.Add(process);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(process);
        }

        /* GET: Processes/Edit/id
           Return the View with the Process to 
           Update*/
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = db.Process.Find(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            return View(process);
        }

        /* POST: Processes/Edit/id
           Update the Process with the new params
          from the View*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProcess,Criterio,Descripcion")] Process process)
        {
            if (ModelState.IsValid)
            {
                db.Entry(process).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(process);
        }

        /* GET: Processes/Delete/id
           Return the View who deletes physically 
           the record with the given id    */
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = db.Process.Find(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            return View(process);
        }

        /* POST: Processes/DeleteConfirmed/id
        Deletes physcally the Process with
        the given id    */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Process process = db.Process.Find(id);
            db.Process.Remove(process);
            List<Activity> actividades = db.Activity.Where(a => a.idProcess == id).ToList();
            foreach(Activity a in actividades)
            {
                db.Activity.Remove(a);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /* GET: Processes/Operations/id
        open a Modal with the Operations of
        the Process*/
        public async Task<ActionResult> Operations(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Process process = await db.Process.FindAsync(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            TempData["Process"] = process;
            ViewBag.Process = process.Criterio;
            if (process.Operations == null)
            {
                return PartialView();
            }
            return PartialView("_Operations", process.Operations.ToList());
        }
        /*GET: Process/AddOperation/id
       Return Modal that allows to Create and Add an Operation
       to Process from given id*/
        public async Task<ActionResult> AddOperation(Guid? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            Process process = await db.Process.FindAsync(id);
            if(process == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPredecesora = new SelectList(process.Operations.ToList(), "idOperation", "Nombre");
            ViewBag.Proceso = process.Criterio;
            TempData["Proceso"] = process;
            return PartialView("_AddOperation");
        }
        /*POST: Process/AddOperation/operation
        Assign the Operation to the Process*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOperation([Bind(Include = "idOperation,Nombre,Descripcion,Type ,idPredecesora, Validable, OperationClass, IteracionesPermitidas, PorcentajeAceptacion")] Operation operation)
        {
            Process proc = (Process)TempData["Proceso"];
            if(proc == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                if (operation.IteracionesPermitidas >= 0)
                {
                    Process process = db.Process.Find(proc.idProcess);
                    process.Operations.Add(operation);
                    ViewBag.Proceso = process.Criterio;
                    await db.SaveChangesAsync();
                    ViewBag.idPredecesora = new SelectList(process.Operations.ToList(), "idOperation", "Nombre");
                    return PartialView("_AddOperation");
                }
            }
            TempData["Proceso"] = proc;
            ViewBag.Proceso = proc.Criterio;
            ViewBag.idPredecesora = new SelectList(proc.Operations.ToList(), "idOperation", "Nombre");
            return PartialView("_AddOperation");
        }
        /*GET: Process/EditOperation/id
       Return View that allows to Update a single record
       from Operation obj*/
        [HttpGet]
        public ActionResult EditOperation(Guid? id)
        {
            if(id == null || TempData["Process"] == null)
            {
                return HttpNotFound();
            }
            Process proceso = (Process)TempData["Process"];
            Process process =  db.Process.Find(proceso.idProcess);
            if(process == null)
            {
                return HttpNotFound();
            }
            Operation operation = db.Operation.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            List<Operation> operaciones = process.Operations.Where(o => o.idOperation != id).ToList();
            ViewBag.idPredecesora = new SelectList(operaciones, "idOperation", "Nombre");
            ViewBag.Proceso = process.Criterio;
            TempData["ProcesoAux"] = process;
            return View("EditOperation", operation);
          
        }
        /*POST: Process/EditOperation/operation
        Update the operation with the new params given 
        by reference from View*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOperation([Bind(Include = "idOperation,Nombre,Descripcion,Type,idPredecesora, Validable, OperationClass, IteracionesPermitidas, PorcentajeAceptacion")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                Operation operacion = db.Operation.Single(o=> o.idOperation == operation.idOperation);
                if(operacion == null)
                {
                    return HttpNotFound();
                }
                if (operation.IteracionesPermitidas >= 0)
                {
                    operacion.Nombre = operation.Nombre;
                    operacion.Descripcion = operation.Descripcion;
                    operacion.Type = operation.Type;
                    operacion.Clase = operation.Clase;
                    operacion.Validable = operation.Validable;
                    operacion.Predecesora = operation.Predecesora;
                    operacion.IteracionesPermitidas = operation.IteracionesPermitidas;
                    operacion.PorcentajeAceptacion = operation.PorcentajeAceptacion;
                    db.SaveChanges();
                    db.Dispose();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.idPredecesora = new SelectList(db.Operation.ToList(), "idOperation", "Nombre");
            return View("EditOperation", operation);
        }
        /*POST: Process/DeleteOperation/id
        Delete the Operation from the Process*/
        [HttpPost]
        public async Task<ActionResult> DeleteOperation(Guid? id)
        {
            if (id == null)
            {
                return Json(new { sucess = false });
            }
            Process proc = (Process)TempData["Process"];
            Process process = await db.Process.FindAsync(proc.idProcess);
            Operation operation = db.Operation.Find(id);
            if(process == null||operation == null)
            {
                return Json(new { sucess = false });
            }
            TempData["Operation"] = operation;
            TempData["Process"] = process;
            return Json(new { sucess = true });
           
        }
        /*POST: Process/ConfirmDeleteOperation/id
        Confirm the Delete of the Operation from the Process
        this function deletes physically the record given*/
        [Authorizate(Disabled = true, Public = false)]
        [HttpPost]
        public async Task<ActionResult> ConfirmDeleteOperation()
        {
            Process proc = (Process)TempData["Process"];
            Process proceso = await db.Process.FindAsync(proc.idProcess);
            TempData["Process"] = proceso;
            Operation operation = (Operation)TempData["Operation"];
            Operation operacion = await db.Operation.FindAsync(operation.idOperation);
            if(operacion != null)
            {
                List<Operation> esPredecesora = proceso.Operations.Where(o => o.idPredecesora.Equals(operacion.idOperation)).ToList();
                if (esPredecesora.Count > 0)
                {
                    return Json(new { sucess = false });
                }
                db.Operation.Remove(operacion);
                await db.SaveChangesAsync();
                return Json(new { sucess = true });
            }
            return Json(new { sucess = false });
        }

    }
}
