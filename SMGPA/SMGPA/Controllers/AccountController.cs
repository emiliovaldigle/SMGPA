using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using SMGPA.Models;
using System;
using SMGPA.Filters;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SMGPA.Controllers
{
    [Authorizate(Disabled = true, Public = true)]
    public class AccountController : AsyncController
    {
        MD5Encoder mdencoder = new MD5Encoder();
        private SMGPAContext db = new SMGPAContext();
        // GET: Account
        public ActionResult NotAuthorized()
        {
            return View();
        }


        public ActionResult Login()
        {
            if (Session["UserID"] == null)
            {
                return View();
            }
            if (Session["Admin"] != null)
                return RedirectToAction("LoggedInAdmin", "Account");

            return RedirectToAction("LoggedInFunctionary", "Account");
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

                    foreach (Administrator a in Administrator)
                    {
                        if (a.MailInstitucional.Equals(user.MailInstitucional) && a.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
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
                        if (f.MailInstitucional.Equals(user.MailInstitucional) && f.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
                        {
                            Session["UserID"] = f.idUser;
                            Session["Username"] = f.Nombre + " " + f.Apellido;
                            ViewBag.Notificaciones = db.Notificacion.Where(n => n.idUser == f.idUser && !n.Vista).ToList();
                            ViewBag.Total = db.Notificacion.Where(n => n.idUser == f.idUser && !n.Vista).Count();
                            return View("LoggedInFunctionary");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Usuario o contraseña erróneos");
                            ViewBag.validation = "Usuario o Contraseña incorrecta";
                        }
                    }

                }
                catch (SqlException ex) when (ex.Errors.Count > 0)
                {

                }

            }
            return View();

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
                functionary.Contrasena = mdencoder.EncodePasswordMd5(functionary.Contrasena);
                functionary.Activo = true; ;
                functionary.idUser = Guid.NewGuid();
                db.Functionary.Add(functionary);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", functionary.idCareer);
            return View(functionary);
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
            if (Session["UserID"] != null)
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

        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(User user)
        {
            User functionary = db.User.Where(f => f.MailInstitucional.Equals(user.MailInstitucional)).FirstOrDefault();
            string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("ResetPass", "Account", new { id = functionary.idUser });
            Notification n = new Notification();
            await n.RecoverPassword(functionary, link);
            ViewBag.Mensaje = "Hemos enviado un correo para que restablezcas tu contraseña";
            return View();
        }
        public ActionResult ResetPass(Guid? id)
        {
            User user = db.User.Find(id);
            TempData["account"] = user;
            if (user != null)
            {
                return View(user);

            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> ResetPass(User usuario)
        {
            User user = (User)TempData["account"];
            using (SMGPAContext db = new SMGPAContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                User u = await db.User.FindAsync(user.idUser);
                u.Contrasena = mdencoder.EncodePasswordMd5(usuario.Contrasena);
                db.User.Attach(u);
                db.Entry(u).Property(x => x.Contrasena).IsModified = true;
                await db.SaveChangesAsync();
                ViewBag.MensajeRestablecido = "Contraseña restablecida";
                return View();
            }

        } 
    }
}