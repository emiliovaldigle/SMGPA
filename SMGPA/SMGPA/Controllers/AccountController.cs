using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using SMGPA.Models;

namespace SMGPA.Controllers
{
    public class AccountController : Controller
    {
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
            return View();
        }
        [HttpPost]
        public ActionResult Register(Functionary account)
        {
            if (ModelState.IsValid)
            {
                using (SMGPAContext db = new SMGPAContext())
                {
                    db.Functionary.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Nombre + " " + account.Apellido + " se ha registrado exitosamente";
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult AdminCreate(Administrator account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (SMGPAContext db = new SMGPAContext())
        //        {
        //            db.Administrator.Add(account);
        //            db.SaveChanges();
        //        }
        //        ModelState.Clear();
        //        ViewBag.Message = account.Nombre + " " + account.Apellido + " se ha registrado exitosamente";
        //    }
        //    return View();
        //}
       
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