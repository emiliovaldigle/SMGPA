using System.Web;
using System.Web.Optimization;

namespace SMGPA
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/bootstrap-hover-dropdown.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include("~/Scripts/modalform.js"));

            bundles.Add(new ScriptBundle("~/bundles/dynamics").Include("~/Scripts/dynamics.js"));

            bundles.Add(new StyleBundle("~/public/bundles/css").Include("~/Content/site.css", "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include("~/Content/login.css", "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/public/bundles/scss").Include("~/Content/navbar-fixed-side.css", "~/Content/navbar-fixed-side.scss"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css"));

        }
    }
}
