﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMGPA.Models;
using System.Threading.Tasks;
using SMGPA.Filters;
using PagedList;
namespace SMGPA.Controllers
{
    public class EntitiesController : AsyncController
    {
        private SMGPAContext db = new SMGPAContext();

        /* GET: Entities
       Return the View Index with Collection of
       Entities, also the user can filter and sort 
       the results*/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var Entities = from e in db.Entity
                           select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entities = Entities.Where(e => e.Nombre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Entities = Entities.OrderByDescending(e => e.Nombre);
                    break;
                default:
                    Entities = Entities.OrderBy(e => e.Nombre);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(Entities.ToPagedList(pageNumber, pageSize));
        }

        /* GET: Entities/Details/id    
          Return View with full object Entity
          but his id
         */
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

        /* GET: Entities/Create
       Return the View Create who let the user        
       create an Entity */
        public ActionResult Create()
        {
            return View();
        }

        /* POST: Entities/Create
         Post the entity into the bd if
         model is Valid */
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
        /* GET: Entities/CreateFaculty
      Return the View CreateFaculty who let the user        
      create a Faculty who inherits properties from
      Model Entities*/
        public ActionResult CreateFaculty()
        {
            Faculty facultad = new Faculty();
            facultad.Carreras = new List<Career>();
            TempData["Facultad"] = facultad;
            List<Career> Carreras = db.Career.ToList();
            ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
            return View(facultad);
        }

        /* POST: Entities/CreateFaculty
       Post the Faculty given and
       create the record in the db*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFaculty([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Faculty faculty)
        {
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
                TempData["Facultad"] = faculty;
                db.Faculty.Add(faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
            return View(faculty);
        }

        /* POST: Entities/AddCareer
       Post the Career given and
       associate it with the Faculty*/
        [HttpPost]
        public async Task<JsonResult> AddCareer(Guid id)
        {
            if (id == null)
            {
                return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
            }
            Faculty facultad = (Faculty)TempData["Facultad"];
            Career carrera = await db.Career.FindAsync(id);
            facultad.Carreras.Add(carrera);
            TempData["Carreras"] = facultad.Carreras;
            TempData["Facultad"] = facultad;
            return Json(new { sucess = true, nombre = carrera.Nombre, idcareer = carrera.idCareer }, JsonRequestBehavior.AllowGet);
        }

        /* GET: Entities/Edit/id
           Return the View with the Entity to 
           Update*/
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
        /* POST: Entities/Edit/id
           Update the Entity with the new params
          from the View*/
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
        /* GET: Entities/EditFaculty/id
        Return the View with the Faculty to 
        Update*/
        public ActionResult EditFaculty(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty Faculty = db.Faculty.Find(id);
            TempData["Facultad"] = Faculty;
            List<Career> Carreras = db.Career.ToList();
            ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
            if (Faculty == null)
            {
                return HttpNotFound();
            }
            return View(Faculty);
        }

        /* POST: Entities/EditFaculty/id
        Update the Faculty with the new params
        from the View*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFaculty([Bind(Include = "idEntities,Nombre,Descripcion,Activo")] Faculty faculty)
        {
            List<Career> Carreras = db.Career.ToList();
            Faculty TempFaculty = (Faculty)TempData["Facultad"];
            if (ModelState.IsValid)
            {
                faculty.idEntities = TempFaculty.idEntities;
                var carreras = TempData["Carreras"];
                ICollection<Career> Careers = (ICollection<Career>)carreras;
                //Si viene sin carreras retorno mensaje indicando que no es posible
                if (Careers == null && faculty.Carreras == null)
                {
                    ViewBag.Errores = "No puede crear una Facultad sin Carreras asociadas";
                    ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
                    return View(faculty);
                }
                if (Careers != null)
                {
                    foreach (Career c in Careers)
                    {
                        if (c.idFaculty == null)
                        {
                            Career car = db.Career.Find(c.idCareer);
                            car.idFaculty = TempFaculty.idEntities;
                            List<Functionary> funcionarios = db.Functionary.Where(f => f.idCareer == c.idCareer).ToList();
                            if (funcionarios != null)
                            {
                                foreach (Functionary f in funcionarios)
                                {
                                    faculty.Involucrados.Add(f);
                                }
                            }

                        }
                    }
                }
                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCareer = new SelectList(Carreras.Where(c => c.idFaculty == null), "idCareer", "Nombre");
            return View(faculty);
        }

        /* GET: Entities/Delete/id
           Return the View who deletes physically 
           the record with the given id    */
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
        /* POST: Entities/DeleteConfirmed/id
        Deletes physcally the Entity with
        the given id    */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            List<Tasks> Tareas = db.Task.ToList();
            if(Tareas.Where(t=> t.idEntities.Equals(id)|| t.idResponsable.Equals(id)).Count() > 0) {
                return RedirectToAction("Index");
            }
            Entities entidad = db.Entity.Find(id);
            var type = entidad.GetType().UnderlyingSystemType.Name.ToString();
            if (type.Contains("Faculty"))
            {
                Faculty facultad = db.Faculty.Find(id);
                if (facultad.Carreras != null)
                {
                    foreach (Career c in facultad.Carreras)
                    {
                        c.idFaculty = null;
                    }
                }
                int index = db.Task.ToList().FindIndex(t => t.idEntities.Equals(facultad.idEntities));
                if (index > 0)
                {
                    return RedirectToAction("Index");
                }
                db.Faculty.Remove(facultad);
                db.SaveChanges();
            }
            if (type.Contains("Entities"))
            {
                Entities entities = db.Entity.Find(id);
                db.Entity.Remove(entities);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        /* GET: Entities/Functionaries/id
      open a Modal with the Functionaries of
      the Entity or Faculty*/
        [HttpGet]
        public async Task<ActionResult> Functionaries(Guid? id)
        {
            List<FunctionaryEntity> involucrados = new List<FunctionaryEntity>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities entity = await db.Entity.FindAsync(id);
            TempData["Entity"] = entity;
            ViewBag.Entidad = entity.Nombre;
            ICollection<FunctionaryEntity> entidadfuncionario = db.FuncionarioEntidad.Where(e => e.idEntities.Equals(entity.idEntities)).ToList();
            foreach (FunctionaryEntity fe in entidadfuncionario)
            {
                Functionary f = db.Functionary.Find(fe.idUser);
                FunctionaryEntity fn = new FunctionaryEntity();
                fn.Funcionario = f;
                fn.Entidad = fe.Entidad;
                fn.Cargo = fe.Cargo;
                involucrados.Add(fn);
            }
            return PartialView("_Functionaries", involucrados);
        }
        /* Function that get the records to Autocomplete
        the RUT of the input in Functionaries Modal*/
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
        /*Function that check if RUT match any
         existing user in db*/
        [Authorizate(Disabled = true, Public = false)]
        [HttpGet]
        public JsonResult CheckUser(string rut)
        {
            if (rut == null)
            {
                return Json(new { sucess = false });
            }
            Functionary functionary = db.Functionary.Where(f => f.Rut.Equals(rut)).FirstOrDefault();
            if (functionary == null)
            {
                return Json(new { sucess = false });
            }
            string carrera = functionary.idCareer == null ? "No figura" : functionary.Carrera.Nombre;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera, sucess = true }, JsonRequestBehavior.AllowGet);
        }
        /*POST: Entities/AddFunctionary/rut
        Assign the Functionary to the Entity*/
        [HttpPost]
        public async Task<JsonResult> AddFunctionary(string rut, string cargo)
        {
            Entities entidad = (Entities)TempData["Entity"];
            Entities entity = await db.Entity.FindAsync(entidad.idEntities);
            if (entity == null)
            {
                return Json(new { sucess = false, reload = true }, JsonRequestBehavior.AllowGet);
            }
            Functionary functionary = db.Functionary.Where(f => f.Rut.Equals(rut)).FirstOrDefault();
            Functionary funcionario = entity.Involucrados.Where(fu => fu.Rut.Equals(rut)).FirstOrDefault();
            if (functionary == null || funcionario != null)
            {
                TempData["Entity"] = entity;
                return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
            }
            string carrera = functionary.idCareer == null ? " " : functionary.Carrera.Nombre;
            FunctionaryEntity fe = new FunctionaryEntity { idEntities = entity.idEntities, idUser = functionary.idUser, Cargo = cargo };
            entity.FuncionarioEntidad.Add(fe);
            entity.Involucrados.Add(functionary);
            functionary.Entidades.Add(entity);
            functionary.FuncionarioEntidad.Add(fe);
            await db.SaveChangesAsync();
            TempData["Entity"] = entity;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera, sucess = true }, JsonRequestBehavior.AllowGet);
        }
        /*POST: Entities/DeleteFunctionary/
        Delete the Functionary from the Entity*/
        [HttpPost]
        public async Task<JsonResult> DeleteFunctionary(Guid? id)
        {
            if (id == null)
            {
                return Json(new { sucess = false, reload = true }, JsonRequestBehavior.AllowGet);
            }
            Entities entidad = (Entities)TempData["Entity"];
            Entities entity = await db.Entity.FindAsync(entidad.idEntities);
            if (entity == null)
            {
                return Json(new { sucess = false, JsonRequestBehavior.AllowGet });
            }
            Functionary functionary = await db.Functionary.FindAsync(id);
            FunctionaryEntity funcionarioentidad = await db.FuncionarioEntidad.FindAsync(functionary.idUser, entity.idEntities);
            bool result = (entity.Involucrados.Remove(functionary)) ? true : false;
            db.FuncionarioEntidad.Remove(funcionarioentidad);
            string carrera = functionary.idCareer == null ? "No figura" : functionary.Carrera.Nombre;
            await db.SaveChangesAsync();
            TempData["Entity"] = entity;
            return Json(new { iduser = functionary.idUser, nombre = functionary.Nombre, apellido = functionary.Apellido, carrera = carrera, sucess = true }, JsonRequestBehavior.AllowGet);
        }
    }
}

