using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;

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
        public ActionResult CreateOperation()
        {
            return PartialView("_CreateOperation");
        }
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<ActionResult> AddOperation([Bind(Include = "idOperation,Name,Descripcion,Type")] Operation operation) 
        {
            if (ModelState.IsValid)
            {
                Process proc =(Process) TempData["Process"];
                Process process = db.Process.Find(proc.idProcess);
                process.Operations.Add(operation);
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("_CreateOperation",operation);
        }
    }
}
