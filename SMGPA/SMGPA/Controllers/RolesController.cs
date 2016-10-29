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
using System.IO;
using System.Web.WebPages;

namespace SMGPA.Controllers
{
    public class RolesController : AsyncController
    {
        private SMGPAContext db = new SMGPAContext();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Role.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRole,Nombre,Descripcion")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.idRole = Guid.NewGuid();
                db.Role.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRole,Nombre,Descripcion")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Role role = db.Role.Find(id);
            db.Role.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Permissions(Guid? id)
        {
            bool isIn = false;
            List<Permission> permisos = new List<Permission>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await db.Role.FindAsync(id);
            TempData["Role"] = role;
            foreach (Permission p in db.Permission.ToList())
            {
                foreach(Permission pe in role.Permisos)
                {
                    if (p == pe)
                    {
                        isIn = true;
                    }
                }
                if (!isIn)
                {
                    permisos.Add(p);
      
                }
                isIn = false;
            }
            ViewBag.Role = role.Nombre;
            ViewBag.Permission = permisos;
            if (role.Permisos == null)
            {
                return PartialView();
            }
            return PartialView("_Permissions", role.Permisos.ToList());
        }
        //[HttpPost]
        //public async Task<ActionResult> AddPermission(Guid? selectedidPermission)
        //{
        //    Role role = (Role)TempData["Role"];
        //    Role rol = await db.Role.FindAsync(role.idRole);
        //    Permission permission = await db.Permission.FindAsync(selectedidPermission);
        //    rol.Permisos.Add(permission);
        //    await db.SaveChangesAsync();
        //    TempData["Role"] = role;
        //    return Json(new { sucess = true });
        //}
        //[Authorize]
        [HttpPost]
        public ActionResult AddPermission(string selectedidPermission)
        {
            Role role = (Role)TempData["Role"];
            Role rol =  db.Role.Find(role.idRole);
            Permission permission =  db.Permission.Find(selectedidPermission);
            rol.Permisos.Add(permission);
            db.SaveChanges();
            TempData["Role"] = role;
            return Json(new { sucess = true });
        }
        [HttpPost]
        public async Task<ActionResult> DeletePermission(Guid? id)
        {
            if(id == null)
            {
                return Json(new { sucess = false });
            }
            Role role = (Role)TempData["Role"];
            Role rol =   await db.Role.FindAsync(role.idRole);
            Permission permission =  await db.Permission.FindAsync(id);
            bool result = (rol.Permisos.Remove(permission)) ? true : false;
            await db.SaveChangesAsync();
            TempData["Role"] = role;
            return Json(new { sucess = result },JsonRequestBehavior.AllowGet);
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

