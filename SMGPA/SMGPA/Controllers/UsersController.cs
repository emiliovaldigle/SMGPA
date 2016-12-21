using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;
using System.Security.Cryptography;
using System.Text;
using PagedList;

namespace SMGPA.Controllers
{
    public class UsersController : Controller
    {
        private SMGPAContext db = new SMGPAContext();
        MD5Encoder encoder = new MD5Encoder();
        // GET: Users
        public ActionResult Index(string sortOrder, string searchString, string rutString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RutSortParam = sortOrder == "Rut" ? "rut_desc" : "Rut";
            if (searchString != null || rutString != null)
            {
                page = 1;
            }
            else
            {
                if (searchString == null)
                {
                    searchString = currentFilter;
                }
                if (rutString == null)
                {
                    rutString = currentFilter;
                }
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentFilter2 = rutString;
            var Users = from u in db.User
                           select u;
            if (!string.IsNullOrEmpty(rutString))
            {
                Users = Users.Where(e => e.Rut.Contains(rutString));
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(e => e.Apellido.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Users = Users.OrderByDescending(e => e.Apellido);
                    break;
                case "Rut":
                    Users = Users.OrderBy(e => e.Rut);
                    break;
                case "rut_desc":
                    Users = Users.OrderByDescending(e => e.Rut);
                    break;
                default:
                    Users = Users.OrderBy(e => e.Apellido);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(Users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.idRole = new SelectList(db.Role, "idRole", "Nombre");
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idUser,Rut,Nombre,Apellido,MailInstitucional,Contrasena,Activo,idRole")] Administrator administrator)
        {
            foreach (User u in db.User.ToList())
            {
                if (u.MailInstitucional.Equals(administrator.MailInstitucional))
                { 
                    ViewBag.CIDisponible =  "Correo institucional se encuentra en uso";
                    ViewBag.idRole = new SelectList(db.Role, "idRole", "Nombre", administrator.idRole);
                    return View(administrator);
                }
            }
            foreach (Functionary f in db.Functionary.ToList())
            {
                if (f.CorreoPersonal.Equals(administrator.MailInstitucional))
                {
                    ViewBag.CIDisponible = "Correo institucional se encuentra en uso";
                    ViewBag.idRole = new SelectList(db.Role, "idRole", "Nombre", administrator.idRole);
                    return View(administrator);
                }
            }
            if (ModelState.IsValid)
            {
                administrator.Contrasena = encoder.EncodePasswordMd5(administrator.Contrasena);
                administrator.idUser = Guid.NewGuid();
                db.Administrator.Add(administrator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idRole = new SelectList(db.Role, "idRole", "Nombre", administrator.idRole);
            return View(administrator);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idRole = new SelectList(db.Role, "idRole", "Nombre");
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idUser,Rut,Nombre,Apellido,MailInstitucional,Contrasena,Activo,idRole")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idRole = new SelectList(db.Role, "idRole", "Nombre", administrator.idRole);
            return View(administrator);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator user = db.Administrator.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Administrator user = db.Administrator.Find(id);
            db.Administrator.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Off(Guid? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Functionary funcionario = db.Functionary.Find(id);
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", funcionario.idCareer);
            return View(funcionario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Off([Bind(Include = "idUser,Rut,Nombre,Apellido,MailInstitucional,Contrasena,Activo,NumeroTelefono,CorreoPersonal,idCareer")] Functionary user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", user.idCareer);
            return View(user);
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
