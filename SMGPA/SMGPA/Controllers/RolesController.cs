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

        /* GET: Roles
        Return the View Index with Collection of
        Roles*/
        public ActionResult Index()
        {
            return View(db.Role.ToList());
        }

        /* GET: Roles/Details/id    
            Return View with full object Role
            but his id
           */
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

        /* GET: Roles/Create
          Return the View Create who let the user        
          create an Role */
        public ActionResult Create()
        {
            return View();
        }

        /* POST: Roles/Create
         Post the role into the bd if
         model is Valid */
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

        /* GET: Roles/Edit/id
           Return the View with the Role to 
           Update*/
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

        /* POST: Roles/Edit/id
           Update the Role with the new params
          from the View*/
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

        /* GET: Roles/Delete/id
           Return the View who deletes physically 
           the record with the given id    */
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

        /* POST: Roles/DeleteConfirmed/id
        Deletes physcally the Career with
        the given id    */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Role role = db.Role.Find(id);
            db.Role.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /* GET: Roles/Permissions/id
       Return the Partial View who opens a Modal 
       with the Permissions from the given id of Role    */
        [HttpGet]
        public async Task<ActionResult> Permissions(Guid? id)
        {
            List<Permission> permisos = new List<Permission>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await db.Role.FindAsync(id);
            TempData["Role"] = role;
            ViewBag.Role = role.Nombre;
            if (role.Permisos == null)
            {
                return PartialView();
            }
            await LoadPermission(role.idRole);
            return PartialView("_Permissions", role.Permisos.ToList());
        }
        /* GET: Roles/LoadPermissions/id
       Fullfill the Dropdown with the Permissions avaliables  */
        public async Task<ActionResult> LoadPermission(Guid? idRole)
        {
            if(idRole  != null)
            {
                bool isIn = false;
                List<SelectListItem> Permisos = new List<SelectListItem>();
                Role rol = await db.Role.FindAsync(idRole);
                if(rol == null)
                {
                    return Json(new { sucess = false });
                }
                foreach (Permission p in db.Permission.ToList())
                {
                    foreach (Permission pe in rol.Permisos)
                    {
                        if (p == pe)
                        {
                            isIn = true;
                        }
                    }
                    if (!isIn)
                    {
                        SelectListItem sl = new SelectListItem { Value = p.idPermission.ToString(), Text = p.TextLink };
                        Permisos.Add(sl);
                    }
                    isIn = false;
                }
                ViewData["Permisos"] = Permisos;
                return Json(new { sucess = true });
            }
            return Json(new { sucess = false });
        }
        /* POST: Roles/AddPermission/id
        Add the Permission to the Role stored
        in param TempData["Role]"*/
        [HttpPost]
        public async Task<ActionResult> AddPermission(Guid? id)
        {
            Role rol = (Role)TempData["Role"];
            Role role = await db.Role.FindAsync(rol.idRole);
            if (role == null)
            {
                return Json(new { sucess = false , JsonRequestBehavior.AllowGet});
            }
            Permission permission = await db.Permission.FindAsync(id);
            if (!role.Permisos.Contains(permission))
            {
             role.Permisos.Add(permission);
                await db.SaveChangesAsync();
                TempData["Role"] = role;
                return Json(new { idpermission = permission.idPermission, textlink = permission.TextLink, controller = permission.Controller, actionresult = permission.ActionResult, sucess = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
          
        }
        /* POST: Roles/DeletePermission/id
        Delete the Permission from the Role stored
        in param TempData["Role]"*/
        [HttpPost]
        public async Task<ActionResult> DeletePermission(Guid? id)
        {
            if(id == null)
            {
                return Json(new { sucess = false, JsonRequestBehavior.AllowGet });
            }
            Role role = (Role)TempData["Role"];
            Role rol =   await db.Role.FindAsync(role.idRole);
            if(rol == null)
            {
                return Json(new { sucess = false, JsonRequestBehavior.AllowGet });
            }
            Permission permission =  await db.Permission.FindAsync(id);
            bool result = (rol.Permisos.Remove(permission)) ? true : false;
            await db.SaveChangesAsync();
            TempData["Role"] = role;
            return Json(new { idpermission = permission.idPermission,textlink=permission.TextLink,sucess = result },JsonRequestBehavior.AllowGet);
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

