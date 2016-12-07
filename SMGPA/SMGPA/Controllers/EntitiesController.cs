﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;
using SMGPA.Filters;

namespace SMGPA.Controllers
{
    public class EntitiesController : AsyncController
    {
        private SMGPAContext db = new SMGPAContext();

        // GET: Entities
        public ActionResult Index(string sortOrder)
        {
            return View(db.Entity.ToList());
        }

        // GET: Entities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entities = db.Entity.Find(id);
            if (entities == null)
            {
                return HttpNotFound();
            }
            return View(entities);
        }

        // GET: Entities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Entities/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Entities entities)
        {
            if (ModelState.IsValid)
            {
                entities.idEntities = Guid.NewGuid();
                db.Entity.Add(entities);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entities);
        }
        // GET: Entities/CreateFaculty
        public ActionResult CreateFaculty()
        {
            Faculty facultad = new Faculty();
            facultad.Carreras = new List<Career>();
            TempData["Facultad"] = facultad;
            List<Career> Carreras = db.Career.ToList();
            ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
            return View(facultad);
        }

        // POST: Entities/CreateFaculty
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFaculty([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Faculty faculty)
        {
                List<Career> _Carreras = new List<Career>();
                List<Career> Carreras = db.Career.ToList();
                if (ModelState.IsValid)
                {
                    Guid idEntities = Guid.NewGuid();
                    faculty.idEntities = idEntities;
                    List<Career> Careers = (List<Career>)TempData["Carreras"];
                    //Si viene sin carreras retorno mensaje indicando que no es posible
                    if (Careers == null)
                    {
                        ViewBag.Errores = "No puede crear una Facultad sin Carreras asociadas";
                        ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
                        return View(faculty);
                    }
                    foreach (Career c in Careers)
                    {
                        Career car = db.Career.Find(c.idCareer);
                        car.idFaculty = idEntities;
                        List<Functionary> funcionarios = db.Functionary.Where(f => f.idCareer == c.idCareer).ToList();
                        if (funcionarios != null)
                        {
                            foreach (Functionary f in funcionarios)
                            {
                                faculty.Involucrados.Add(f);
                            }
                        }
                    }

                    db.Faculty.Add(faculty);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
                return View(faculty);
        }
        [HttpPost]
        public  async Task<JsonResult> AddCareer(Guid id)
        {
            if (id == null)
            {
                return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
            }
            Faculty facultad = (Faculty)TempData["Facultad"];
            Career carrera = await db.Career.FindAsync(id);
            facultad.Carreras.Add(carrera); 
            TempData["Carreras"] = facultad.Carreras;
            TempData["Faculta0.d"] = facultad;
            return Json(new { sucess = true , nombre = carrera.Nombre, idcareer = carrera.idCareer}, JsonRequestBehavior.AllowGet);
        }
        // GET: Entities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entities = db.Entity.Find(id);
            if (entities == null)
            {
                return HttpNotFound();
            }
            return View(entities);
        }

        // POST: Entities/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Entities entities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entities);
        }

        // GET: Entities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entities = db.Entity.Find(id);
            if (entities == null)
            {
                return HttpNotFound();
            }
            return View(entities);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Faculty facultad = db.Faculty.Find(id);
            if(facultad != null)
            {
                if(facultad.Carreras != null)
                {
                    foreach(Career c in facultad.Carreras)
                    {
                        c.idFaculty = null;
                    }
                }
                db.Faculty.Remove(facultad);
                db.SaveChanges();
            }
            if(facultad == null)
            {
                Entities entities = db.Entity.Find(id);
                db.Entity.Remove(entities);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Functionaries(Guid? id)
        {
            List<Functionary> involucrados = new List<Functionary>();
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entity = await db.Entity.FindAsync(id);
            TempData["Entity"] = entity;
            ViewBag.Entidad = entity.Nombre;
            if (entity.Involucrados == null)
            {
                return PartialView();
            }
            return PartialView("_Functionaries", entity.Involucrados.ToList());
        }
        [Authorizate(Disabled = true, Public = false)]
        public JsonResult RutAutoComplete(string term)
        {
            List<String> Rut = new List<String>();
            Rut = db.Functionary.Where(x => x.Rut.StartsWith(term)).Select(y => y.Rut).ToList();
            return Json(Rut, JsonRequestBehavior.AllowGet);

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorizate(Disabled =true, Public = false)]
        [HttpGet]
        public JsonResult CheckUser(string rut)
        {
            if (rut == null)
            {
                return Json(new { sucess = false });
            }
            Functionary functionary =  db.Functionary.Where(f => f.Rut.Equals(rut)).FirstOrDefault();
            if(functionary == null)
            {
                return Json(new { sucess = false });
            }
            string carrera = functionary.idCareer == null ? "No figura": functionary.Carrera.Nombre;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera , sucess = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> AddFunctionary(string rut)
        {
            Entities entidad = (Entities)TempData["Entity"];
            Entities entity = await db.Entity.FindAsync(entidad.idEntities);
            if(entity == null)
            {
                return Json(new { sucess = false, reload = true }, JsonRequestBehavior.AllowGet);
            }
            Functionary functionary = db.Functionary.Where(f => f.Rut.Equals(rut)).FirstOrDefault();
            Functionary funcionario = entity.Involucrados.Where(fu => fu.Rut.Equals(rut)).FirstOrDefault();
            if(functionary == null || funcionario != null)
            {
                TempData["Entity"] = entity;
                return Json(new { sucess = false}, JsonRequestBehavior.AllowGet );
            }
            string carrera = functionary.idCareer == null ? " " : functionary.Carrera.Nombre;
            entity.Involucrados.Add(functionary);
            await db.SaveChangesAsync();
            TempData["Entity"] = entity;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera, sucess = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteFunctionary(Guid? id)
        {
            if (id == null)
            {
                return Json(new { sucess = false, reload = true }, JsonRequestBehavior.AllowGet);
            }
            Entities entidad = (Entities)TempData["Entity"];
            Entities entity = await db.Entity.FindAsync(entidad.idEntities);
            if(entity == null)
            {
                return Json(new { sucess = false, JsonRequestBehavior.AllowGet });
            }
            Functionary functionary = await db.Functionary.FindAsync(id);
            bool result = (entity.Involucrados.Remove(functionary)) ? true : false;
            string carrera = functionary.idCareer == null ? "No figura" : functionary.Carrera.Nombre;
            await db.SaveChangesAsync();
            TempData["Entity"] = entity;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera, sucess = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
   
