using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMGPA.Filters
{
    public class Authorizate : ActionFilterAttribute, IAuthorizationFilter
    {
        public bool Disabled { get; set; }
        public bool Public { get; set; }
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            if (!Disabled)
            {
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    string actionName = filterContext.ActionDescriptor.ActionName;
                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                    using (SMGPAContext db = new SMGPAContext())
                    {
                        Administrator admin = db.Administrator.Find(HttpContext.Current.Session["UserID"]);
                        if (admin != null)
                        {
                            Role rol = db.Role.Find(admin.idRole);
                            Permission Permiso = rol.Permisos.Where(p => p.ActionResult == actionName && p.Controller == controllerName).FirstOrDefault();
                            if (Permiso != null)
                                return;
                        }
                    }
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" }));
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
            if(Disabled && Public)
            {
                if(HttpContext.Current.Session["UserID"] == null)
                {
                    return;
                }
            }
            if (Disabled)
            {
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
        }
    }
}