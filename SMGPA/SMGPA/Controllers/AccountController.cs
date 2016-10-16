using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGMPA.Models;
using System.Data.SqlClient;

namespace SGMPA.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using(SGMPAContext db = new SGMPAContext())
            {
                return View(db.Users.ToList());
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Functionary account)
        {
            if (ModelState.IsValid)
            {
                using (SGMPAContext db = new SGMPAContext())
                {
                    db.Functionary.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Nombre + " " + account.Apellido + " se ha registrado exitosamente";
            }
            return View();
        }
        [HttpPost]
        public ActionResult AdminCreate(Administrator account)
        {
            if (ModelState.IsValid)
            {
                using (SGMPAContext db = new SGMPAContext())
                {
                    db.Administrator.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Nombre + " " + account.Apellido + " se ha registrado exitosamente";
            }
            return View();
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
            using (SGMPAContext db = new SGMPAContext())
            {
                try
                {
                    ICollection<User> Users = db.Users.ToList();
                    foreach(User us in Users)
                    {
                        if(us.MailInstitucional.Equals(user.MailInstitucional) && us.Contrasena.Equals(user.Contrasena))
                        {
                            Session["UserID"] = us.UserId;
                            Session["Username"] = us.Nombre + " " + us.Apellido;
                            break;  
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
            if(Session["UserID"]!=null)
            return View("LoggedIn");

            return View();

        }
       
       public ActionResult LoggedIn()
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
        public ActionResult LogOut()
        {
            if (Session["UserID"] != null)
            {
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