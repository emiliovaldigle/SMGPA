using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;
using SMGPA.Filters;

namespace SMGPA.Controllers
{
    [Authorizate(Disabled = true)]
    public class TasksController : Controller { 
    

        private SMGPAContext db = new SMGPAContext();

        // GET: Tasks
        
       public ActionResult Tasks()
        {
            Guid idUser = (Guid) Session["UserID"];
            if(idUser == null)
            {
                RedirectToAction("LogOut", "Account");
            }
            Functionary usuario = db.Functionary.Find(idUser);
            List<Tasks> Tareas = new List<Tasks>();
            List<Tasks> tasks = db.Task.ToList();
            List<Tasks> tareasresponsable = tasks.Where(t => t.idFunctionary == usuario.idUser).ToList();
            List<Entities> entidades = usuario.Entidades.ToList();
            Tareas = tareasresponsable;
            foreach(Entities e in entidades)
            {
                List<Tasks> tareasvalidador = db.Task.Where(t => t.idEntities == e.idEntities).ToList();
                Tareas.AddRange(tareasvalidador);
            }
            return View(Tareas);
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
