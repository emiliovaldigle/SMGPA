using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;

namespace SMGPA.Controllers
{
    public class EntitiesController : Controller
    {
        private SMGPAContext db = new SMGPAContext();

        // GET: Entities
        public ActionResult Index()
        {
            return View(db.Entity.ToList());
        }

        // GET: Entities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entities = db.Entity.Find(id);
            if (entities == null)
            {
                return HttpNotFound();
            }
            return View(entities);
        }

        // GET: Entities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Entities/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Entities entities)
        {
            if (ModelState.IsValid)
            {
                entities.idEntities = Guid.NewGuid();
                db.Entity.Add(entities);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entities);
        }

        // GET: Entities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entities = db.Entity.Find(id);
            if (entities == null)
            {
                return HttpNotFound();
            }
            return View(entities);
        }

        // POST: Entities/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Entities entities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entities);
        }

        // GET: Entities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entities = db.Entity.Find(id);
            if (entities == null)
            {
                return HttpNotFound();
            }
            return View(entities);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Entities entities = db.Entity.Find(id);
            db.Entity.Remove(entities);
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
    }
}
