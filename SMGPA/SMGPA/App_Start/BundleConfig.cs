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
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js").Include( 
            "~/Scripts/jquery-ui-timepicker-addon.js").Include(
            "~/Scripts/jquery.canvasjs.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/bootstrap-hover-dropdown.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/modalform").Include("~/Scripts/modalform.js"));
            bundles.Add(new ScriptBundle("~/bundles/tasks").Include("~/Scripts/tasks.js"));
            bundles.Add(new ScriptBundle("~/bundles/roles").Include("~/Scripts/rolespermission.js"));
            bundles.Add(new ScriptBundle("~/bundles/entities").Include("~/Scripts/entitiesfunctionary.js"));
            bundles.Add(new ScriptBundle("~/bundles/operations").Include("~/Scripts/operations.js"));
            bundles.Add(new ScriptBundle("~/bundles/chart").Include("~/Scripts/chart.js"));
            bundles.Add(new ScriptBundle("~/bundles/notification").Include("~/Scripts/notification.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css").Include("~/Content/bootstrap.css").Include("~/Content/jquery-ui-timepicker-addon.css"));
            bundles.Add(new StyleBundle("~/Content/login").Include("~/Content/login.css", "~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/register").Include("~/Content/register.css", "~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/public/bundles/scss").Include("~/Content/navbar-fixed-side.css", "~/Content/navbar-fixed-side.scss"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            "~/Content/themes/base/jquery.ui.core.css",
            "~/Content/themes/base/jquery.ui.resizable.css",
            "~/Content/themes/base/jquery.ui.selectable.css",
            "~/Content/themes/base/jquery.ui.accordion.css",
            "~/Content/themes/base/jquery.ui.autocomplete.css",
            "~/Content/themes/base/jquery.ui.button.css",
            "~/Content/themes/base/jquery.ui.dialog.css",
            "~/Content/themes/base/jquery.ui.slider.css",
            "~/Content/themes/base/jquery.ui.tabs.css",
            "~/Content/themes/base/jquery.ui.datepicker.css",
            "~/Content/themes/base/jquery.ui.progressbar.css",
            "~/Content/themes/base/jquery.ui.theme.css",
             "~/Content/themes/base/jquery.ui.theme.css"));
            BundleTable.EnableOptimizations = false;
        }
    }
}
    