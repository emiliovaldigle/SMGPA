using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using SMGPA.Models;
using System;

namespace SMGPA.Controllers
{
    public class AccountController : Controller
    {
        private SMGPAContext db = new SMGPAContext();
        // GET: Account
        public ActionResult Index()
        {
            using(SMGPAContext db = new SMGPAContext())
            {
                return View(db.User.ToList());
            }
        }
        public ActionResult Register()
        {
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "idUser,Rut,Nombre,Apellido,Nombre_Apellido,MailInstitucional,Contrasena,Activo,NumeroTelefono,CorreoPersonal,idCareer")] Functionary functionary)
        {
            if (ModelState.IsValid)
            {
                functionary.Activo = true; ;
                functionary.idUser = Guid.NewGuid();
                db.Functionary.Add(functionary);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", functionary.idCareer);
            return View(functionary);
        }

        public ActionResult Login()
        {
            if (Session["UserID"] == null)
            {
                return View();
            }
           return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(User user)
        {  
            using (SMGPAContext db = new SMGPAContext())
            {
                try
                {
                    ICollection<Administrator> Administrator = db.Administrator.ToList();
                    ICollection<Functionary> Functionary = db.Functionary.ToList();
                   
                    foreach(Administrator a in Administrator)
                    {
                        if(a.MailInstitucional.Equals(user.MailInstitucional) && a.Contrasena.Equals(user.Contrasena))
                        {
                            Session["Admin"] = a;
                            Session["UserID"] = a.idUser;
                            Session["Username"] = a.Nombre + " " + a.Apellido;
                            return View("LoggedInAdmin");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Usuario o contraseña erróneos");
                            ViewBag.validation = "Usuario o Contraseña incorrecta";
                        }
                    }
                    foreach (Functionary f in Functionary)
                    {
                        if (f.MailInstitucional.Equals(user.MailInstitucional) && f.Contrasena.Equals(user.Contrasena))
                        {
                            Session["UserID"] = f.idUser;
                            Session["Username"] = f.Nombre + " " + f.Apellido;
                            return View("LoggedInFunctionary");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Usuario o contraseña erróneos");
                            ViewBag.validation = "Usuario o Contraseña incorrecta";
                        }
                    }

                }
                catch(SqlException ex) when (ex.Errors.Count > 0) { 

                }
             
          }
            return View();

        }
        public PartialViewResult BuildMenu()
        {
            if (Session["Admin"] != null)
            {
                using (SMGPAContext db = new SMGPAContext())
                {
                    Administrator administrator = (Administrator)Session["Admin"];
                    List<Permission> permission = new List<Permission>();
                    Role rol = db.Role.Find(administrator.idRole);
                    foreach (Permission p in rol.Permisos)
                    {
                        if (p.ActiveMenu)
                            permission.Add(p);
                    }
                    return PartialView(permission);
                }
            }
            return PartialView();
        }
       
       public ActionResult LoggedInFunctionary()
        {
            if(Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult LoggedInAdmin()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult LogOut()
        {
            if (Session["UserID"] != null)
            {
                Session["Admin"] = null;
                Session["UserID"] = null;
                Session["Username"] = null;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

     
    }
}