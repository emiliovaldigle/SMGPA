using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMGPA.Controllers
{
    public class ProcessesController : AsyncController
    {
        private SMGPAContext db = new SMGPAContext();

        // GET: Processes
        public ActionResult Index()
        {
            return View(db.Process.ToList());
        }

        // GET: Processes/Details/5
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

        // GET: Processes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Processes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Processes/Edit/5
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

        // POST: Processes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Processes/Delete/5
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

        // POST: Processes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Process process = db.Process.Find(id);
            db.Process.Remove(process);
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
        public async Task<ActionResult> Operations(Guid? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = await db.Process.FindAsync(id);
            TempData["Process"] = process;
            ViewBag.Process = process.Criterio;
            if (process.Operations == null)
            {
                return PartialView();
            }
            return PartialView("_Operations",process.Operations.ToList());
        }
       public async Task<ActionResult> AddOperation(Guid? id)
        {
            Process process = await db.Process.FindAsync(id);
            ViewBag.idPredecesora = new SelectList(process.Operations.ToList(), "idOperation", "Nombre");
            ViewBag.Proceso = process.Criterio;
            TempData["Proceso"] = process;
            return PartialView("_AddOperation");
        }
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<ActionResult> AddOperation([Bind(Include = "idOperation,Nombre,Descripcion,Type,idPredecesora")] Operation operation) 
        {
            Process proc = (Process)TempData["Proceso"];
            if (ModelState.IsValid)
            {
                Process process = db.Process.Find(proc.idProcess);
                process.Operations.Add(operation);
                ViewBag.Proceso = process.Criterio;
                await db.SaveChangesAsync();
                ViewBag.idPredecesora = new SelectList(process.Operations.ToList(), "idOperation", "Nombre");
                return PartialView("_AddOperation");
            }
            TempData["Proceso"] = proc;
            ViewBag.Proceso = proc.Criterio;
            ViewBag.idPredecesora = new SelectList(proc.Operations.ToList(), "idOperation", "Nombre");
            return PartialView("_AddOperation");
        }
        [HttpPost]
        public async Task<ActionResult> DeleteOperation(Guid? id)
        {
            if (id == null)
            {
                return Json(new { sucess = false });
            }
            Process proc = (Process)TempData["Process"];
            Process process = await db.Process.FindAsync(proc.idProcess);
            Operation operation = process.Operations.Where(o => o.idProcess == process.idProcess && o.idOperation == id).FirstOrDefault();
            db.Operation.Remove(operation);
            await db.SaveChangesAsync();
            TempData["Process"] = proc;
            return Json(new { sucess = true });
        }
        [HttpGet]
        public ActionResult EditOperation(Guid? id)
        {
            if(id == null || TempData["Process"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process proceso = (Process)TempData["Process"];
            Process process =  db.Process.Find(proceso.idProcess);
            Operation operation = process.Operations.Where(o => o.idOperation == id).First();
            List<Operation> operaciones = process.Operations.Where(o => o.idOperation != id).ToList();
            ViewBag.idPredecesora = new SelectList(operaciones, "idOperation", "Nombre");
            ViewBag.Proceso = process.Criterio;
            TempData["ProcesoAux"] = process;
            return View("EditOperation", operation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOperation([Bind(Include = "idOperation,Nombre,Descripcion,Type,idPredecesora")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                Operation operacion = db.Operation.Single(o=> o.idOperation == operation.idOperation);
                operacion.Nombre = operation.Nombre;
                operacion.Descripcion = operation.Descripcion;
                operacion.Type = operation.Type;
                operacion.Predecesora = operation.Predecesora;
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.idPredecesora = new SelectList(db.Operation.ToList(), "idOperation", "Nombre");
            return View("EditOperation", operation);
        }
    }
}
