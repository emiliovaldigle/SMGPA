using SMGPA.Filters;
using System.Web.Mvc;

namespace SMGPA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Authorizate());
            filters.Add(new Notificator());
        }
    }
}
