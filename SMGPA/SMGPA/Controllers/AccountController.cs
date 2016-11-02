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
    [Authorizate(Disabled = true)]
    public class AccountController : AsyncController
    {
        MD5Encoder mdencoder = new MD5Encoder();
        private SMGPAContext db = new SMGPAContext();
        // GET: Account
        public ActionResult NotAuthorized()
        {
            return View();
        }
        public ActionResult Index()
        {
            using(SMGPAContext db = new SMGPAContext())
            {
                return View(db.User.ToList());
            }
        }

        [Authorizate(Disabled = true)]
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
                        if(a.MailInstitucional.Equals(user.MailInstitucional) && a.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
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
        
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(User user)
        {
            User usuario = db.User.Where(f => f.MailInstitucional.Equals(user.MailInstitucional)).FirstOrDefault();
            if(usuario != null)
            {
                string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("ResetPass", "Account", new { id = usuario.idUser});
                var body = "<p>Por favor ingresar a la siguiente URL para restablecer contraseña</p>"+
                    "<p>"+link+"</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(usuario.MailInstitucional));  // replace with valid value 
                message.From = new MailAddress("soportesmgpa@gmail.com");  // replace with valid value
                message.Subject = "Restablecer contraseña SMGPA";
                message.Body = string.Format(body);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new System.Net.NetworkCredential
                    {
                        UserName = "soportesmgpa@gmail.com",  // replace with valid value
                        Password = "123.pass"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
                ViewBag.Mensaje = "Hemos enviado un correo para que restablezcas tu contraseña";
            }
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
            using( SMGPAContext db = new SMGPAContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                User u =  await db.User.FindAsync(user.idUser);
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