using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMGPA.Models;
using SMGPA.Controllers;

namespace SMGPA.Filters
{
    public class Notificator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session["Admin"] == null)
            {
             
                using (SMGPAContext db = new SMGPAContext())
                {
                    Guid userId = (Guid)HttpContext.Current.Session["UserID"];
                    List<Notificacion> NotificacionesActivas = db.Notificacion.Where(n => n.idUser == userId  && !n.Vista).ToList();
                    var controller = filterContext.Controller as Controller;
                    filterContext.Controller.ViewBag.Total = NotificacionesActivas.Count();
                    filterContext.Controller.ViewBag.Notificaciones = NotificacionesActivas;
                    return;
                }
            }
            return;
            
        }
    }
}