using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using SMGPA.Models;
using System;
using SMGPA.Filters;
using System.Threading.Tasks;
using System.Data.Entity;
namespace SMGPA.Controllers
{
    /*Controller in charge of the managment of public Accounts of the System*/
    [Authorizate(Disabled = true, Public = true)]
    public class AccountController : AsyncController
    {
        MD5Encoder mdencoder = new MD5Encoder();
        private SMGPAContext db = new SMGPAContext();
        // GET: Account
        // This View is loaded when a User doesn't has the privileges to access a View or Function
        public ActionResult NotAuthorized()
        {
            return View();
        }

        // Function that returns the View for Login in the APP
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
        // Function that Post credentials from param user and validate if he exists/ is avaliable and password matchs
        [HttpPost]
        public ActionResult Login(User user)
        {
            using (SMGPAContext db = new SMGPAContext())
            {
                if (user.MailInstitucional != null && user.Contrasena != null)
                {
                    try
                    {
                        ICollection<Administrator> Administrator = db.Administrator.ToList();
                        ICollection<Functionary> Functionary = db.Functionary.ToList();

                        foreach (Administrator a in Administrator)
                        {
                            if (a.MailInstitucional.Equals(user.MailInstitucional) && a.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
                            {
                                if (a.Activo)
                                {
                                    Session["Admin"] = a;
                                    Session["UserID"] = a.idUser;
                                    Session["Username"] = a.Nombre + " " + a.Apellido;
                                    return View("LoggedInAdmin");
                                }
                                if (!a.Activo)
                                {
                                    ModelState.AddModelError("", "Usuario inactivo");
                                    ViewBag.validation = "Usuario inactivo";
                                    return View();
                                }
                            }
                            if (!a.MailInstitucional.Equals(user.MailInstitucional) || !a.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
                            {
                                ModelState.AddModelError("", "Usuario o contraseña erróneos");
                                ViewBag.validation = "Usuario o Contraseña incorrecta";
                            }
                        }
                        foreach (Functionary f in Functionary)
                        {
                            if (f.MailInstitucional.Equals(user.MailInstitucional) && f.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
                            {
                                if (f.Activo)
                                {
                                    Session["UserID"] = f.idUser;
                                    Session["Username"] = f.Nombre + " " + f.Apellido;
                                    ViewBag.Notificaciones = db.Notificacion.Where(n => n.idUser == f.idUser && !n.Vista).ToList();
                                    ViewBag.Total = db.Notificacion.Where(n => n.idUser == f.idUser && !n.Vista).Count();
                                    return View("LoggedInFunctionary");
                                }
                                if (!f.Activo)
                                {
                                    ModelState.AddModelError("", "Usuario inactivo");
                                    ViewBag.validation = "Usuario inactivo";
                                    return View();
                                }
                            }
                            if (!f.MailInstitucional.Equals(user.MailInstitucional) || !f.Contrasena.Equals(mdencoder.EncodePasswordMd5(user.Contrasena)))
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


            }
            return View();

        }
        // Function that returns the Register View for creation of Functionary Accounts
        public ActionResult Register()
        {
            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre");
            return View();

        }
        // Function that Post params from user type Functionary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "idUser,Rut,Nombre,Apellido,Nombre_Apellido,MailInstitucional,Contrasena,Activo,NumeroTelefono,CorreoPersonal,idCareer")] Functionary functionary)
        {
            foreach (User u in db.User.ToList())
            {
                if (u.MailInstitucional.Equals(functionary.MailInstitucional) || functionary.CorreoPersonal.Equals(u.MailInstitucional))
                {
                    ViewBag.CIDisponible = !functionary.CorreoPersonal.Equals(u.MailInstitucional) ? "Correo institucional se encuentra en uso" : null;
                    ViewBag.CPDisponible = functionary.CorreoPersonal.Equals(u.MailInstitucional) ? "Correo Personal ingresado se encuentra en uso" : null;
                    ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", functionary.idCareer);
                    return View();
                }
            }
            foreach (Functionary f in db.Functionary.ToList())
            {
                if (f.CorreoPersonal.Equals(functionary.CorreoPersonal))
                {
                    ViewBag.CPDisponible = "Correo Personal ingresado se encuentra en uso";
                    ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", functionary.idCareer);
                    return View();
                }
            }
            if (ModelState.IsValid)
            {
                functionary.Contrasena = mdencoder.EncodePasswordMd5(functionary.Contrasena);
                functionary.Activo = true; ;
                functionary.idUser = Guid.NewGuid();
                db.Functionary.Add(functionary);
                if (functionary.idCareer != null)
                {
                    Career c = db.Career.Find(functionary.idCareer);
                    if (c.idFaculty != null)
                    {
                        Faculty faculty = db.Faculty.Find(c.idFaculty);
                        faculty.Involucrados.Add(functionary);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.idCareer = new SelectList(db.Career, "idCareer", "Nombre", functionary.idCareer);
            return View(functionary);
        }
        // Function that build the Menu for the logged Administrator
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
        // Function that redirect the Functionaries to Main View for them
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
        [Authorizate(Disabled = true, Public = false)]
        public ActionResult Dashboard()
        {
            if (Session["UserID"] != null)
            {
                //Barra de Actividades por Carrera

                //Calcula cantidad de Total de Actividades por Carrera
                List<Activity> DBActividades = db.Activity.ToList();
                var Actividades = DBActividades.GroupBy(a => a.Carrera.Nombre)
                                           .Select(g => new { Nombre = g.Key, Count = g.Count() });
                List<Activity> Completadas = DBActividades.Where(a => a.state.Equals(States.Completada)).ToList();
                var ActividadesCompletadas = Completadas
                                     .GroupBy(a => a.Carrera.Nombre)
                                     .Select(g => new { Nombre = g.Key, Count = g.Count() });
                List<string> NombreCarreras = new List<string>();
                List<double> TotalCarreras = new List<double>();
                List<double> CompletadasCarreras = new List<double>();
                var Carreras = db.Career.ToList();
                if (Actividades.Count() > 0)
                {
                    foreach (var a in Actividades.ToList())
                    {
                        NombreCarreras.Add(a.Nombre);
                        TotalCarreras.Add(a.Count);
                    }
                }
                if (ActividadesCompletadas.Count() > 0)
                {
                    foreach (var a in ActividadesCompletadas.ToList())
                    {
                        CompletadasCarreras.Add(a.Count);

                    }
                    ViewBag.CompletadasCarreras = CompletadasCarreras;
                }
                else
                {
                    List<double> TotalCarreras_B = new List<double>();
                    foreach (var a in Actividades.ToList())
                    {
                        TotalCarreras_B.Add(0);
                    }
                    ViewBag.CompletadasCarreras = TotalCarreras_B;
                }


                ViewBag.Carreras = NombreCarreras;
                ViewBag.TotalCarreras = TotalCarreras;
                List<double> ValoresCompletadas = new List<double>();
                List<double> ValoresTotales = new List<double>();
                //Evolutivo de Actividades
                var CompletadasPorMes = Completadas
                                   .GroupBy(x => new
                                   {
                                       month = x.start_date.GetValueOrDefault().Month,
                                       year = x.start_date.GetValueOrDefault().Year,
                    
                                  })
                                   .Select(g => new { Nombre = g.Key, Count = g.Count() });
                foreach(var a in CompletadasPorMes)
                {
                    ValoresCompletadas.Add(a.Count);
                }
                var TotalesporMes = DBActividades
                                 .GroupBy(x => new
                                 {
                                     month = x.start_date.GetValueOrDefault().Month,
                                     year = x.start_date.GetValueOrDefault().Year,

                                 })
                                 .Select(g => new { Nombre = g.Key, Count = g.Count() });
                foreach (var a in TotalesporMes)
                {
                    ValoresTotales.Add(a.Count);
                }
                ViewBag.ActividadPorMes = ValoresTotales;
                ViewBag.ActividadCompletadaPorMes = ValoresCompletadas;
                //Evolutivo de Tareas
                //Gráfico de Torta
                List<Tasks> Tareas = db.Task.ToList();
                ViewBag.Completed = Tareas.Where(t => t.Estado.Equals(StatusEnum.COMPLETADA)).Count();
                ViewBag.InProgress = Tareas.Where(t => t.Estado.Equals(StatusEnum.EN_PROGRESO)).Count();
                ViewBag.Cerrada = Tareas.Where(t => t.Estado.Equals(StatusEnum.CERRADA_SIN_CONCLUIR)).Count();
                ViewBag.Activa = Tareas.Where(t => t.Estado.Equals(StatusEnum.ACTIVA)).Count();
                //Gráfico de Torta
                ViewBag.Funcionarios = db.Functionary.Count();
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        // Function that redirect to Administrator to Main View for them, in this case the Admin Dashboard
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
        // Function that closes current user's session
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
        // Function that return the View for resetting the Password of an Account
        public ActionResult ResetPassword()
        {
            return View();
        }
        /* Function that post the e-mail placed in the input and
         send a e-mail with an URL in order to reset
         the password*/
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
        /*Function triggered when user access to URL given
         this return the View who let the user reset
         his  password*/
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
        /*Function that post the password from input
        then encrypt it and update the record in db
        */
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
        public ActionResult MyChart()
        {
            return null;
        }

    }
}