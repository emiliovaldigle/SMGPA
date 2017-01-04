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
    public class CareersController : Controller
    {
        private SMGPAContext db = new SMGPAContext();

     /* GET: Careers
     Return the View Index with Collection of
     Careers*/
        public ActionResult Index()
        {
            return View(db.Career.ToList());
        }

        /* GET: Careers/Details/id
       Return View with full object Career
       but his id
      */
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Career.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        /* GET: Careers/Create
         Return the View Create who let the user        
         create an Career */
        public ActionResult Create()
        {
            return View();
        }

        /* POST: Careers/Create
      Post the career into the bd if
      model is Valid */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCareer,Nombre,Descripcion")] Career career)
        {
            if (ModelState.IsValid)
            {
                career.idCareer = Guid.NewGuid();
                db.Career.Add(career);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(career);
        }

        /* GET: Careers/Edit/id
           Return the View with the Career to 
           Update*/
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Career.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        /* POST: Careers/Edit/id
         Update the Career with the new params
        from the View*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCareer,Nombre,Descripcion")] Career career)
        {
            if (ModelState.IsValid)
            {
                db.Entry(career).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(career);
        }

        /* GET: Careers/Delete/id
           Return the View who deletes physically 
           the record with the given id    */
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Career.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }


        /* POST: Careers/DeleteConfirmed/id
        Deletes physcally the Career with
        the given id    */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Career career = db.Career.Find(id);
            db.Career.Remove(career);
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
