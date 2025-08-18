using System.Web.Optimization;
using Europa.Web;

namespace Europa.Treinamento.Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Static/scripts/jquery-3.1.1.min.js",
                       "~/Static/scripts/jquery.mask.min.js",
                       "~/Static/scripts/jquery.form.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Static/scripts/jquery.validate.min.js",
                        "~/Static/scripts/jquery.validate.unobstrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                       "~/Static/scripts/moment-with-locates.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Static/scripts/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryNumeric").Include(
                        "~/Static/scripts/jquery.numeric.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Static/scripts/bootstrap.js",
                        "~/Static/scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-daterangepicker").Include(
                       "~/Static/bootstrap-daterangepicker/js/daterangepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-validator").Include(
                      "~/Static/bootstrap-validator/validator.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-treeview")
                .Include("~/Static/bootstrap-treeview/js/bootstrap-treeview.min.js"));

            //repare que aqui temos um Bundle e não um ScriptBundle. Foi decidigo não minificar por contas de erro do angular que a minificação gerava
            var angularBundle = new Bundle("~/bundles/angular").Include(
                        "~/Static/angular/angular.min.js",
                        "~/Static/europa/js/europa-angular-app.js");
            bundles.Add(angularBundle);

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                      "~/Static/select2/js/select2.full.min.js",
                      "~/Static/select2/js/i18n/pt-BR.js"));

            bundles.Add(new StyleBundle("~/css/fullcalendar")
                .Include("~/Static/fullcalendar/css/fullcalendar.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar")
                .Include("~/Static/fullcalendar/js/fullcalendar.min.js")
                .Include("~/Static/fullcalendar/js/locale-all.js")
            );

            bundles.Add(new StyleBundle("~/css/notify")
                .Include("~/Static/bootstrap-notify/css/animate.css"));

            bundles.Add(new ScriptBundle("~/bundles/notify")
                .Include("~/Static/bootstrap-notify/js/bootstrap-notify.min.js"));

            //repare que aqui temos um Bundle e não um ScriptBundle. Foi decidido não minificar por contas de erro do angular que a minificação gerava
            var angularDatatableBunde = new Bundle("~/bundles/angular-datatable")
                    .Include("~/Static/angular-datatable/js/jquery.dataTables.js",
                    "~/Static/angular-datatable/js/angular-datatables.js",
                    "~/Static/angular-datatable/js/dataTables.select.min.js",
                    "~/Static/angular-datatable/js/angular-datatables.select.js",
                    "~/Static/angular-datatable/js/dataTables.columnFilter.js",
                    "~/Static/angular-datatable/js/angular-datatables.columnfilter.js",
                    "~/Static/angular-datatable/js/angular-datatables.bootstrap.min.js",
                    "~/Static/angular-datatable/js/dataTables.rowReorder.min.js",
                    "~/Static/europa/js/europa-angular-datatable.js");
            bundles.Add(angularDatatableBunde);

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Static/jquery-ui/js/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/css/angular-datatable").Include(
                        "~/Static/angular-datatable/css/jquery.dataTables.min.css",
                        "~/Static/angular-datatable/css/angular-datatables.css",
                        "~/Static/angular-datatable/css/select.dataTables.min.css",
                        "~/Static/angular-datatable/css/select.dataTables.min.css",
                        "~/Static/angular-datatable/css/rowReorder.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/css/jquery-ui").Include(
                "~/Static/jquery-ui/css/jquery-ui.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/europa").Include(
                         "~/Static/europa/js/europa-app.js",
                         "~/Static/europa/js/europa-commons.js",
                         "~/Static/europa/js/europa-date.js",
                         "~/Static/europa/js/europa-dialogo.js",
                         "~/Static/europa/js/europa-exception-handler.js",
                         "~/Static/europa/js/europa-form.js",
                         "~/Static/europa/js/europa-chart.js",
                         "~/Static/europa/js/europa-scheduler.js",
                         "~/Static/europa/js/europa-order-list.js",
                         "~/Static/europa/js/europa-mask.js",
                         "~/Static/europa/js/europa-tree-view.js",
                         "~/Static/europa/js/europa-validator.js"));

            //Cascade Style Sheets
            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                      "~/Static/bootstrap/css/bootstrap.min.css",
                      "~/Static/bootstrap/css/bootstrap-switch.min.css",
                      "~/Static/bootstrap/css/bootstrap-theme.min.css"));

            bundles.Add(new StyleBundle("~/css/bootstrap-daterangepicker").Include(
                      "~/Static/bootstrap-daterangepicker/css/daterangepicker.css"));

            bundles.Add(new Bundle("~/css/font-awesome")
                .Include("~/Static/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Static/font-awesome/css/font-awesome-animation.min.css"));

            bundles.Add(new StyleBundle("~/css/europa").Include(
                      "~/Static/europa/css/europa-angular-datatable.css",
                      "~/Static/europa/css/europa-bootstrap.css",
                      "~/Static/europa/css/europa-main.css",
                      "~/Static/europa/css/europa-select2.css"));

            bundles.Add(new StyleBundle("~/css/select2").Include("~/Static/select2/css/select2.min.css",
                      "~/Static/select2/css/select2-bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce")
                .Include("~/Static/tinymce/tinymce.min.js",
                    "~/Static/tinymce/jquery.tinymce.min.js",
                    "~/Static/tinymce/themes/modern/theme.min.js",
                    "~/Static/tinymce/plugins/lists/plugin.min.js",
                    "~/Static/tinymce/plugins/advlist/plugin.min.js",
                    "~/Static/tinymce/plugins/link/plugin.min.js",
                    "~/Static/tinymce/plugins/image/plugin.min.js",
                    "~/Static/tinymce/langs/pt_BR.js"
                    ));

            bundles.Add(new StyleBundle("~/css/tinymce")
                .Include("~/Static/tinymce/skins/lightgray/skin.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/chart")
                .Include("~/Static/chart/Chart.min.js",
                "~/Static/chart/chart.funnel.min.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap-treeview")
                .Include("~/Static/bootstrap-treeview/css/bootstrap-treeview.min.css"));
        }
    }
}